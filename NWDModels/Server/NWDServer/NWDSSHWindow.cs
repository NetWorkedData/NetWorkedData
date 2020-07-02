//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Text;
using UnityEngine;
using UnityEditor;
using Renci.SshNet;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;
using System.Diagnostics;
using Unity.EditorCoroutines.Editor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public delegate void NWDSSHCommandBlock(string sCommand, string sResult);
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDSSHWindow : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDSSHCommandBlock CommandResultDelegate;
        //-------------------------------------------------------------------------------------------------------------
        private static NWDSSHWindow kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        static Vector2 ScrollPosition;
        GUIStyle TextareaStyle;
        //-------------------------------------------------------------------------------------------------------------
        NWDServer Server;
        List<string> CommandList;
        int AltPORT = -1;
        string AltUser = null;
        string AltPassword = null;
        string ScriptTitle = "unknow script";
        PasswordConnectionInfo ConnectionInfo;
        SshClient ClientSSH;
        ShellStream ShellStream;
        Stopwatch Watch = new Stopwatch();
        double DeltaAbsolute;
        StringBuilder TextResult = new StringBuilder();
        int CommandCount = 0;
        int CommandActual = 0;
        string Infos = String.Empty;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the SharedInstance or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDSSHWindow SharedInstance()
        {
            //NWEBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDSSHWindow)) as NWDSSHWindow;
            }
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Repaint all app configuration manager windows.
        /// </summary>
        public static void Refresh()
        {
            //NWEBenchmark.Start();
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDSSHWindow));
            foreach (NWDSSHWindow tWindow in tWindows)
            {
                tWindow.Repaint();
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsSharedInstanced()
        {
            if (kSharedInstance != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the SharedInstance of app configuration manager window and focus on.
        /// </summary>
        /// <returns></returns>
        public static NWDSSHWindow SharedInstanceFocus()
        {
            //NWEBenchmark.Start();
            SharedInstance().Show();
            SharedInstance().Focus();
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// On enable action.
        /// </summary>
        public void OnEnable()
        {
            //NWEBenchmark.Start();
            TitleInit("Console SSH", typeof(NWDSSHWindow));
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI()
        {
            //NWEBenchmark.Start();
            NWDGUI.LoadStyles();

            TextareaStyle = new GUIStyle(EditorStyles.textArea);
            TextareaStyle.richText = true;

            NWDGUILayout.Title("Console SSH");
            NWDGUILayout.Informations("To see the ssh command result!");
            if (Server != null)
            {
                NWDGUILayout.Informations("Distribution is " + Server.Distribution.ToString());
            }
            double tDeltaAbsolute = (DeltaAbsolute) / 1000.0F;

            NWDGUILayout.Section(ScriptTitle + "(executed in " + tDeltaAbsolute.ToString("F3") + "s )");
            NWDGUILayout.Line();
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (Server != null)
            {
                EditorGUILayout.TextArea(TextResult.ToString(), TextareaStyle);
            }
            GUILayout.EndScrollView();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ExecuteSSH(NWDServer sServer, string tScriptTitle, List<string> sCommandList, NWDSSHCommandBlock sCommandResultDelegate, int sAltPORT = -1, string sAltUser = null, string sAltPassword = null)
        {
            NWDSSHWindow.SharedInstanceFocus().Execute(sServer, tScriptTitle, sCommandList, sCommandResultDelegate, sAltPORT, sAltUser, sAltPassword);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Execute(NWDServer sServer, string tScriptTitle, List<string> sCommandList, NWDSSHCommandBlock sCommandResultDelegate, int sAltPORT = -1, string sAltUser = null, string sAltPassword = null)
        {
            Server = sServer;
            CommandList = sCommandList;
            CommandResultDelegate = sCommandResultDelegate;
            AltPORT = sAltPORT;
            AltUser = sAltUser;
            AltPassword = sAltPassword;
            ScriptTitle = tScriptTitle;
            //ExecuteAsync();
            EditorCoroutineUtility.StartCoroutineOwnerless(ExecuteAsync());
        }
        //-------------------------------------------------------------------------------------------------------------
        public IEnumerator ExecuteAsync()
        {
            NWDBenchmark.Start();

            IEnumerator tWaitTime = null;

            NWDSSHWindow tWindow = NWDSSHWindow.SharedInstanceFocus();
            Watch.Reset();
            Watch.Start();
            EditorUtility.DisplayProgressBar(ScriptTitle, "Connexion...",0.0F);
            Refresh();
            yield return tWaitTime;
            TextResult = new StringBuilder();
            Refresh();
            yield return tWaitTime;

            bool tNeedSu = false;
            string tUserEcho = "unknow";
            string tIP = Server.IP.GetValue();
            int tPort = Server.Port;
            string tUser = Server.Root_User;
            string tPassword = Server.Root_Secure_Password.Decrypt();
            if (Server.AdminInstalled && Server.RootForbidden)
            {
                tUser = Server.Admin_User;
                tPassword = Server.Admin_Secure_Password.Decrypt();
                tNeedSu = true;
            }
            if (AltPORT != -1)
            {
                tPort = AltPORT;
            }
            if (AltUser != null)
            {
                tUser = AltUser;
                tNeedSu = false;
            }
            if (AltPassword != null)
            {
                tPassword = AltPassword;
            }
            SshClient tClientSSH = null;
            try
            {
                tUserEcho = tUser;
                TextResult.AppendLine("<i>#Local$ Connecting...</i>");
                TextResult.AppendLine("<i>#Local$ ssh -l " + tUserEcho + " " + tIP + " -p " + tPort + "</i>");
                if (NWDProjectCredentialsManager.ShowPasswordInLog)
                {
                    TextResult.AppendLine("<i>#Local$ " + tPassword + "</i>");
                }
                PasswordConnectionInfo tConnectionInfo = new PasswordConnectionInfo(tIP, tPort, tUser, tPassword);
                tClientSSH = new SshClient(tConnectionInfo);
                tClientSSH.Connect();
            }
            catch (System.Exception e)
            {
                TextResult.AppendLine(e.ToString());
                TextResult.AppendLine("<i>#Local$ FAIL!</i>");
                CommandCount = 0;
                CommandActual = 0;
                EditorUtility.ClearProgressBar();
                if (EditorUtility.DisplayDialog("Error", "error in ssh", "OK") == true)
                {
                }
            }
            if (tClientSSH != null)
            {
                if (tClientSSH.IsConnected == true)
                {
                    TextResult.AppendLine("<i>#Local$ host respond.</i>");
                    if (tClientSSH.ConnectionInfo.IsAuthenticated == true)
                    {
                        TextResult.AppendLine("<i>#Local$ authenfication success</i>");
                        ShellStream tShellStream = tClientSSH.CreateShellStream("xterm", 80, 24, 800, 600, 1024);
                        TextResult.AppendLine("<i>--------------------</i>");
                        RunCommand("hostnamectl", tShellStream, TextResult);
                        tUserEcho = RunCommand("whoami", tShellStream, TextResult);
                        RunCommand("ls", tShellStream, TextResult);
                        TextResult.AppendLine("<i>--------------------</i>");
                        if (tNeedSu == true)
                        {
                            //rTextResult.AppendLine("<i>#Local$ cat /dev/null > ~/.bash_history");
                            //tShellStream.WriteLine("cat /dev/null > ~/.bash_history");
                            RunCommand("cat /dev/null > ~/.bash_history", tShellStream, TextResult);

                            TextResult.AppendLine("<i>#Local$ try to swith from " + tUserEcho + " to " + Server.Root_User + " with password " + Server.Root_Secure_Password.Decrypt() + "</i>");

                            TextResult.AppendLine("<i>#Local$ su -l " + Server.Root_User + "</i>");
                            // Get logged in and get user prompt
                            string prompt = tShellStream.Expect(new Regex(@"[$>]"));

                            // Send command and expect password or user prompt
                            tShellStream.WriteLine("su -l " + Server.Root_User + "");
                            prompt = tShellStream.Expect(new Regex(@"([$#>:])"));

                            // Check to send password
                            if (prompt.Contains("assword:")) // research password: (or Password:) 
                            {
                                // Send password
                                TextResult.AppendLine("<i>#Local$ Good! I have the hand ... put password now!</i>");
                                tShellStream.WriteLine(Server.Root_Secure_Password.Decrypt() + "\r");
                                if (NWDProjectCredentialsManager.ShowPasswordInLog)
                                {
                                    TextResult.AppendLine("<i>#Local$ " + Server.Root_Secure_Password.Decrypt() + "</i>");
                                }
                                prompt = tShellStream.Expect(new Regex(@"([$#>:])"));
                                if (prompt.Contains("assword:")) // research password: (or Password:) 
                                {
                                    // not the good password => exit
                                    throw new System.Exception("Password is not the valid");
                                }
                                prompt = tShellStream.Expect(new Regex(@"[$#>]"));
                            }
                            tUserEcho = RunCommand("whoami", tShellStream, TextResult);
                            RunCommand("ls", tShellStream, TextResult);
                            TextResult.AppendLine("<i>--------------------</i>");
                            if (tUserEcho == Server.Root_User)
                            {
                                TextResult.AppendLine("<i>#Local$ Good! " + tUserEcho + " is connected!</i>");
                            }
                            else
                            {
                                TextResult.AppendLine("<i>#Local$ Fail! " + tUserEcho + " is not connected!</i>");
                            }
                            TextResult.AppendLine("<i>--------------------</i>");
                        }

                        TextResult.AppendLine("<i>#Local$ Start command list!</i>");
                        TextResult.AppendLine("<i>--------------------</i>");
                        CommandCount = CommandList.Count +1;
                        CommandActual = 0;
                        EditorUtility.DisplayProgressBar(ScriptTitle, Infos + " (" + CommandActual.ToString() + "/" + CommandCount.ToString() + ")", (float)CommandActual / (float)CommandCount);
                        foreach (string tCommandLine in CommandList)
                        {
                        EditorUtility.DisplayProgressBar(ScriptTitle, tCommandLine + " (" + CommandActual.ToString() + "/" + CommandCount.ToString() + ")", (float)CommandActual / (float)CommandCount);
                            Refresh();
                            yield return tWaitTime;
                            if (string.IsNullOrEmpty(tCommandLine) == false)
                            {
                                string tResult = RunCommand(tCommandLine, tShellStream, TextResult);
                                if (CommandResultDelegate != null)
                                {
                                    CommandResultDelegate(tCommandLine, tResult);
                                }
                            }
                            CommandActual++;
                        }
                        Refresh();
                        yield return tWaitTime;
                        TextResult.AppendLine("<i>--------------------</i>");

                        TextResult.AppendLine("<i>#Local$ Finish command list!</i>");

                        //rTextResult.AppendLine("<i>#Local$ cat /dev/null > ~/.bash_history");
                        //tShellStream.WriteLine("cat /dev/null > ~/.bash_history");
                        RunCommand("cat /dev/null > ~/.bash_history", tShellStream, TextResult);

                        EditorUtility.DisplayProgressBar(ScriptTitle, "Cleanning...", 1.0F);
                        Refresh();
                        if (tNeedSu == true)
                        {

                            // TODO : Bug in the logout

                            // TODO : bug in su -l Admin ... I am disapointed

                            //rTextResult.AppendLine("<i>#Local$ try to swith from " + tUserEcho + " to " + tUser + " with password " + tPassword + "</i>");
                            //rTextResult.AppendLine("<i>#Local$ su -l " + tUser + "</i>");
                            //// Get logged in and get user prompt
                            //string prompt = tShellStream.Expect(new Regex(@"[$>]"));
                            //// Send command and expect password or user prompt
                            //tShellStream.WriteLine("su -l " + tPassword + "");
                            //prompt = tShellStream.Expect(new Regex(@"([$#>:])"));

                            //// Check to send password
                            //if (prompt.Contains("assword:")) // research password: (or Password:) 
                            //{
                            //    // Send password
                            //    rTextResult.AppendLine("<i>#Local$ Good! I have the hand ... put password now!</i>");
                            //    tShellStream.WriteLine(tPassword + "\r");
                            //    rTextResult.AppendLine("<i>#Local$ " + tPassword + "</i>");
                            //    prompt = tShellStream.Expect(new Regex(@"([$#>:])"));
                            //    if (prompt.Contains("assword:")) // research password: (or Password:) 
                            //    {
                            //        // not the good password => exit
                            //        throw new System.Exception("Password is not the valid");
                            //    }
                            //    prompt = tShellStream.Expect(new Regex(@"[$#>]"));
                            //}
                            ////rTextResult.AppendLine("<i>#Local$ cat /dev/null > ~/.bash_history");
                            ////tShellStream.WriteLine("cat /dev/null > ~/.bash_history");
                            //RunCommand("cat /dev/null > ~/.bash_history", tShellStream, rTextResult);
                        }
                        EditorUtility.DisplayProgressBar(ScriptTitle, "Disconnexion...", 1.0F);
                        Refresh();
                        yield return tWaitTime;
                        TextResult.AppendLine("<i>#Local$ " + tUserEcho + " Disconnecting!</i>");
                        tClientSSH.Disconnect();
                        TextResult.AppendLine("<i>#Local$  Disconnected!</i>");
                    }
                    else
                    {
                        TextResult.AppendLine("<i>#Local$ authentification failed</i>");
                        CommandCount = 0;
                        CommandActual = 0;
                        EditorUtility.ClearProgressBar();
                        Refresh();
                        yield return tWaitTime;
                        if (EditorUtility.DisplayDialog("Error", "authentification failed", "OK") == true)
                        {
                        }
                    }
                }
                else
                {
                    TextResult.AppendLine("<i>#Local$ host not respond.</i>");
                    CommandCount = 0;
                    CommandActual = 0;
                    EditorUtility.ClearProgressBar();
                    Refresh();
                    yield return tWaitTime;
                    if (EditorUtility.DisplayDialog("Error", "host not respond", "OK") == true)
                    {
                    }
                }
                tClientSSH.Dispose();
            }
            //Server.TextCommandResult = rTextResult.ToString();
            EditorUtility.ClearProgressBar();
            Refresh();
            yield return tWaitTime;
            DeltaAbsolute = Watch.ElapsedMilliseconds;
            Watch.Stop();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        const string kWhoamiOfThis = "this-is-the-whoami";
        const string kStartOfThis = "this-is-the-start";
        const string kEndOfThis = "this-is-the-end";
        const string kWhoamiStartOfThis = "echo " + kWhoamiOfThis + ";";
        const string kEchoEndOfThis = ";echo " + kEndOfThis + ";";
        const string kEchoStartOfThis = "echo " + kStartOfThis + ";";
        //-------------------------------------------------------------------------------------------------------------
        public static string RunCommand(string sCommand, ShellStream sStream, StringBuilder rTextResult)
        {
            //sStream.WriteLine("echo " + kStartOfThis +";" +sCommand + ";echo " + kEndOfThis + "");
            string tNewCommand = kWhoamiStartOfThis + "whoami;" + kEchoStartOfThis + sCommand + kEchoEndOfThis;
            //Debug.Log(tNewCommand);
            sStream.WriteLine(tNewCommand);
            while (sStream.Length == 0)
            {
                Thread.Sleep(500);
            }
            string rCommand = sCommand;
            string rLineReturn = string.Empty;
            string tLine;
            bool tStart = false;
            bool tStartSecond = false;
            string tUser = "unknow";
            while ((tLine = sStream.ReadLine()) != kEndOfThis)
            {
                if (tStartSecond == true)
                {
                    tStartSecond = false;
                    tUser = tLine;
                }
                if (tStart == true)
                {
                    rLineReturn = rLineReturn + "\n" + tLine;
                    rLineReturn = rLineReturn.Trim(new char[] { '\n', '\r' });
                }
                if (tLine == kStartOfThis)
                {
                    tStart = true;
                }
                if (tLine == kWhoamiOfThis)
                {
                    tStartSecond = true;
                }
            }
            rCommand = rCommand.Replace(kEchoStartOfThis, string.Empty).Replace(kEchoEndOfThis, string.Empty);
            rTextResult.AppendLine("<color=gray><b>" + tUser + ":~$ </b>" + sCommand + "</color>");
            rTextResult.AppendLine(rLineReturn);
            //Debug.Log(">" + tUser + ":~$ " + sCommand + '\n' + rLineReturn);
            return rLineReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string ExecuteProcessTerminal(string argument)
        {
            string output = null;
            try
            {
                UnityEngine.Debug.Log("============== Start Executing [" + argument + "] ===============");
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = "/bin/bash",
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true,
                    Arguments = " -c \"" + argument + " \""
                };
                Process myProcess = new Process
                {
                    StartInfo = startInfo
                };
                myProcess.Start();
                output = myProcess.StandardOutput.ReadToEnd();
                myProcess.WaitForExit();
                UnityEngine.Debug.Log("============== End ===============");

                return output;
            }
            catch (Exception e)
            {
                UnityEngine.Debug.Log(e);
                UnityEngine.Debug.Log("============== End ===============");
                return output;
            }
            //return output;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif