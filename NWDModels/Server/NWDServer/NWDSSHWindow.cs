//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:11
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Text;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using Renci.SshNet;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;
using System.Diagnostics;

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
        StringBuilder TextResult;
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
            double tDeltaAbsolute = (DeltaAbsolute) / 1000.0F;
            
            NWDGUILayout.Section(ScriptTitle + "(executed in "+ tDeltaAbsolute.ToString("F3") + "s )");
            NWDGUILayout.Line();
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (Server != null)
            {
                EditorGUILayout.TextArea(Server.TextCommandResult, TextareaStyle);
            }
            GUILayout.EndScrollView();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ExecuteSSH(NWDServer sServer,string tScriptTitle, List<string> sCommandList, NWDSSHCommandBlock sCommandResultDelegate, int sAltPORT = -1, string sAltUser = null, string sAltPassword = null)
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
            ExecuteAsync();
            //EditorCoroutineUtility.StartCoroutineOwnerless(ExecuteAsync());
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ExecuteAsync()
        {
            NWEBenchmark.Start();
            NWDSSHWindow tWindow = NWDSSHWindow.SharedInstanceFocus();
            Watch.Reset();
            Watch.Start();
            bool tNeedSu = false;
            string tUserEcho = "unknow";
            string tIP = Server.IP.GetValue();
            int tPort = Server.Port;
            string tUser = Server.Root_User;
            string tPassword = Server.Root_Password.GetValue();
            if (Server.AdminInstalled && Server.RootForbidden)
            {
                tUser = Server.Admin_User;
                tPassword = Server.Admin_Password.GetValue();
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
            StringBuilder rTextResult = new StringBuilder(); ;
            try
            {
                tUserEcho = tUser;
                rTextResult.AppendLine("<i>#Local$ Connecting...</i>");
                rTextResult.AppendLine("<i>#Local$ ssh -l " + tUserEcho + " " + tIP + " -p " + tPort + "</i>");
                rTextResult.AppendLine("<i>#Local$ " + tPassword + "</i>");
                PasswordConnectionInfo tConnectionInfo = new PasswordConnectionInfo(tIP, tPort, tUser, tPassword);
                using (SshClient tClientSSH = new SshClient(tConnectionInfo))
                {
                    tClientSSH.Connect();
                    if (tClientSSH.IsConnected == true)
                    {
                        rTextResult.AppendLine("<i>#Local$ host respond.</i>");
                        if (tClientSSH.ConnectionInfo.IsAuthenticated == true)
                        {
                            rTextResult.AppendLine("<i>#Local$ authenfication success</i>");
                            ShellStream tShellStream = tClientSSH.CreateShellStream("xterm", 80, 24, 800, 600, 1024);
                            rTextResult.AppendLine("<i>--------------------</i>");
                            RunCommand("hostnamectl", tShellStream, rTextResult);
                            tUserEcho = RunCommand("whoami", tShellStream, rTextResult);
                            RunCommand("ls", tShellStream, rTextResult);
                            rTextResult.AppendLine("<i>--------------------</i>");
                            if (tNeedSu == true)
                            {
                                //rTextResult.AppendLine("<i>#Local$ cat /dev/null > ~/.bash_history");
                                //tShellStream.WriteLine("cat /dev/null > ~/.bash_history");
                                RunCommand("cat /dev/null > ~/.bash_history", tShellStream, rTextResult);

                                rTextResult.AppendLine("<i>#Local$ try to swith from " + tUserEcho + " to " + Server.Root_User + " with password " + Server.Root_Password.GetValue() + "</i>");

                                rTextResult.AppendLine("<i>#Local$ su -l " + Server.Root_User + "</i>");
                                // Get logged in and get user prompt
                                string prompt = tShellStream.Expect(new Regex(@"[$>]"));

                                // Send command and expect password or user prompt
                                tShellStream.WriteLine("su -l " + Server.Root_User + "");
                                prompt = tShellStream.Expect(new Regex(@"([$#>:])"));

                                // Check to send password
                                if (prompt.Contains("assword:")) // research password: (or Password:) 
                                {
                                    // Send password
                                    rTextResult.AppendLine("<i>#Local$ Good! I have the hand ... put password now!</i>");
                                    tShellStream.WriteLine(Server.Root_Password.GetValue() + "\r");
                                    rTextResult.AppendLine("<i>#Local$ " + Server.Root_Password.GetValue() + "</i>");
                                    prompt = tShellStream.Expect(new Regex(@"([$#>:])"));
                                    if (prompt.Contains("assword:")) // research password: (or Password:) 
                                    {
                                        // not the good password => exit
                                        throw new System.Exception("Password is not the valid");
                                    }
                                    prompt = tShellStream.Expect(new Regex(@"[$#>]"));
                                }
                                tUserEcho = RunCommand("whoami", tShellStream, rTextResult);
                                RunCommand("ls", tShellStream, rTextResult);
                                rTextResult.AppendLine("<i>--------------------</i>");
                                if (tUserEcho == Server.Root_User)
                                {
                                    rTextResult.AppendLine("<i>#Local$ Good! " + tUserEcho + " is connected!</i>");
                                }
                                else
                                {
                                    rTextResult.AppendLine("<i>#Local$ Fail! " + tUserEcho + " is not connected!</i>");
                                }
                                rTextResult.AppendLine("<i>--------------------</i>");
                            }

                            rTextResult.AppendLine("<i>#Local$ Start command list!</i>");
                            rTextResult.AppendLine("<i>--------------------</i>");
                            foreach (string tCommandLine in CommandList)
                            {
                                if (string.IsNullOrEmpty(tCommandLine) == false)
                                {
                                    string tResult = RunCommand(tCommandLine, tShellStream, rTextResult);
                                    if (CommandResultDelegate != null)
                                    {
                                        CommandResultDelegate(tCommandLine, tResult);
                                    }
                                }
                            }
                            rTextResult.AppendLine("<i>--------------------</i>");

                            rTextResult.AppendLine("<i>#Local$ Finish command list!</i>");

                            //rTextResult.AppendLine("<i>#Local$ cat /dev/null > ~/.bash_history");
                            //tShellStream.WriteLine("cat /dev/null > ~/.bash_history");
                            RunCommand("cat /dev/null > ~/.bash_history", tShellStream, rTextResult);

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
                            rTextResult.AppendLine("<i>#Local$ " + tUserEcho + " Disconnecting!</i>");
                            tClientSSH.Disconnect();
                            rTextResult.AppendLine("<i>#Local$  Disconnected!</i>");
                        }
                        else
                        {
                            rTextResult.AppendLine("<i>#Local$ authentification failed</i>");
                        }
                    }
                    else
                    {
                        rTextResult.AppendLine("<i>#Local$ host not respond.</i>");
                    }
                    tClientSSH.Dispose();
                }
            }
            catch (System.Exception e)
            {
                rTextResult.AppendLine(e.ToString());
                rTextResult.AppendLine("<i>#Local$ FAIL!</i>");
                //Debug.Log(rTextResult);
            }
            DeltaAbsolute = Watch.ElapsedMilliseconds;
            Watch.Stop();
            Server.TextCommandResult = rTextResult.ToString();
            NWEBenchmark.Finish();
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
            return output;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif