//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Net;
using System.Net.Sockets;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDInstallationContent : NWDEditorWindowContent
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Scroll position in window
        /// </summary>
        private static Vector2 _kScrollPosition;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance for deamon class.
        /// </summary>
        private static NWDInstallationContent _kSharedInstanceContent;
        //-------------------------------------------------------------------------------------------------------------
        // properties
        string Password;
        string VectorString;
        string WWW;
        string Email;
        string SFTP_user;
        string SFTP_password;
        int SFTP_port = 22;
        string MySQL_user;
        string MySQL_database;
        string MySQL_password;
        string AppName;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstanceContent"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDInstallationContent SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstanceContent == null)
            {
                _kSharedInstanceContent = new NWDInstallationContent();
            }
            NWDBenchmark.Finish();
            return _kSharedInstanceContent;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnEnable(NWDEditorWindow sEditorWindow)
        {
            Window = sEditorWindow;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnDisable(NWDEditorWindow sEditorWindow)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI(Rect sRect)
        {
            base.OnPreventGUI(sRect);
            NWDBenchmark.Start();
            NWDGUILayout.Title("Installation");
            _kScrollPosition = GUILayout.BeginScrollView(_kScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (NWDAppConfiguration.SharedInstance().Installed == NWDAppInstallation.Installed)
            {
                if (GUILayout.Button("Remove instalation and close"))
                {
                    NWDAppConfiguration.SharedInstance().Installed = NWDAppInstallation.FormInstallation;
                    Window.Close();
                    NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().DevEnvironment);
                }
            }
            else
            {
                bool tInstalllWillBeOk = true;

                //Form and Analyze
                NWDGUILayout.Section("App installation");
                AppName = EditorGUILayout.TextField("App name", AppName);

                NWDGUILayout.Section("Editor credentials");
                Password = EditorGUILayout.PasswordField("Password", Password);
                VectorString = EditorGUILayout.PasswordField("Vector", VectorString);

                NWDGUILayout.Section("Server installation");
                NWDGUILayout.SubSection("Web domain");
                WWW = EditorGUILayout.TextField("Webservice domain", WWW);
                Email = EditorGUILayout.TextField("Rescue email", Email);
                NWDGUILayout.SubSection("SFTP");
                SFTP_user = EditorGUILayout.TextField("SFTP user", SFTP_user);
                SFTP_port = EditorGUILayout.IntField("SFTP port", SFTP_port);
                if (string.IsNullOrEmpty(Password) == false && string.IsNullOrEmpty(VectorString) == false)
                {
                    SFTP_password = EditorGUILayout.PasswordField("SFTP password", SFTP_password);
                }
                else
                {
                    tInstalllWillBeOk = false;
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField("SFTP password", "create password ans vector");
                    EditorGUI.EndDisabledGroup();
                }
                NWDGUILayout.SubSection("MySQL");
                MySQL_user = EditorGUILayout.TextField("MySQL user", MySQL_user);
                MySQL_database = EditorGUILayout.TextField("MySQL database", MySQL_database);
                if (string.IsNullOrEmpty(Password) == false && string.IsNullOrEmpty(VectorString) == false)
                {
                    MySQL_password = EditorGUILayout.PasswordField("MySQL password", MySQL_password);
                }
                else
                {
                    tInstalllWillBeOk = false;
                    EditorGUI.BeginDisabledGroup(true);
                    EditorGUILayout.TextField("MySQL password", "create password ans vector");
                    EditorGUI.EndDisabledGroup();
                }

                NWDGUILayout.Section("Annexes");

                EditorGUI.BeginDisabledGroup(!tInstalllWillBeOk);
                if (GUILayout.Button("Install and close"))
                {
                    NWDProjectCredentialsManagerContent.Password = Password;
                    NWDProjectCredentialsManagerContent.VectorString = VectorString;

                    NWDAppConfiguration.SharedInstance().DevEnvironment.AppName = AppName;
                    NWDAppConfiguration.SharedInstance().PreprodEnvironment.AppName = AppName;
                    NWDAppConfiguration.SharedInstance().ProdEnvironment.AppName = AppName;

                    NWDServerDomain tServerDomain = new NWDServerDomain();
                    tServerDomain.ServerDNS = WWW;

                    NWDServer tServer = new NWDServer();
                    tServer.DomainNameServer = tServerDomain.ServerDNS;

                    string tLocalIP = "0.0.0.0";
                    foreach (IPAddress tIP in Dns.GetHostAddresses(tServer.DomainNameServer))
                    {
                        if (tIP.AddressFamily == AddressFamily.InterNetwork)
                        {
                            tLocalIP = tIP.ToString();
                        }
                    }
                    tServer.IP.SetValue(tLocalIP);
                    tServer.Port = SFTP_port;
                    tServer.PortChanged = false;

                    NWDServerServices tServerServices = new NWDServerServices();
                    tServerServices.Server.SetData(tServer);
                    tServerServices.User = SFTP_user;
                    tServerServices.Email = Email;
                    tServerServices.Folder = NWDServerServices.K_Public_Webservices;
                    tServerServices.Secure_Password.SetValue(SFTP_password);
                    tServerServices.UserInstalled = true;

                    NWDServerDatas tServerDatas = new NWDServerDatas();
                    tServerDatas.Server.SetData(tServer);
                    tServerDatas.RangeMin = 0;
                    tServerDatas.RangeMax = 9999;
                    tServerDatas.UserMax = 2000000;
                    tServerDatas.MySQLIP.SetValue(tLocalIP);
                    tServerDatas.MySQLPort = 3306;
                    tServerDatas.MySQLUser = MySQL_user;
                    tServerDatas.MySQLBase = MySQL_database;
                    tServerDatas.MySQLSecurePassword.SetValue(MySQL_password);

                    NWDCluster tCluster = new NWDCluster();
                    tCluster.Domains.AddData(tServerDomain);
                    tCluster.Services.AddData(tServerServices);
                    tCluster.DataBases.AddData(tServerDatas);
                    tCluster.AdminKey.CryptAes(NWDToolbox.RandomStringCypher(24));
                    tCluster.SaltServer.CryptAes(NWDToolbox.RandomStringCypher(24));

                    tCluster.MailFrom = Email;
                    tCluster.RescueEmail = Email;

                    tCluster.Dev = true;
                    tCluster.Preprod = true;
                    tCluster.Prod = true;

                    tCluster.UpdateDataIfModified();

                    tServerDomain.UpdateDataIfModified();
                    tServerServices.UpdateDataIfModified();
                    tServerDatas.UpdateDataIfModified();
                    tServer.UpdateDataIfModified();

                    NWDDataManager.SharedInstance().DataQueueExecute();

                    NWDAppConfiguration.SharedInstance().Installed = NWDAppInstallation.Installed;
                    Window.Close();
                    NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().DevEnvironment);
                }
                EditorGUI.EndDisabledGroup();
            }
            GUILayout.EndScrollView();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDInstallation : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string TutorialLink(string sLink = "")
        {
            return NWDConstants.K_NWDURL + "tutorial/";
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        public static NWDInstallation kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDInstallation SharedInstance()
        {
            //NWDBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = CreateInstance(typeof(NWDInstallation)) as NWDInstallation;
            }
            //NWDBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + "Install Engine", false, 0)]
        public static NWDInstallation SharedInstanceFocus()
        {
            //NWDBenchmark.Start();
            SharedInstance().ShowModalUtility();
            //NWDBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Refresh()
        {
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDInstallation));
            foreach (NWDInstallation tWindow in tWindows)
            {
                tWindow.Repaint();
            }
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
        public void OnEnable()
        {
            //NWDBenchmark.Start();
            // set ideal size
            NormalizeWidth = 600;
            NormalizeHeight = 700;
            NormalizeSize();
            // set title
            TitleInit(NWDConstants.K_APP_MODEL_MANAGER_TITLE, typeof(NWDInstallation));
            NWDInstallationContent.SharedInstance().OnEnable(this);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            //NWDBenchmark.Start();
            NWDGUI.LoadStyles();
            NWDInstallationContent.SharedInstance().OnPreventGUI(position);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
