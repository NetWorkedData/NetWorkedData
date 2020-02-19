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

//=====================================================================================================================
namespace NetWorkedData
{
    // doc to read to finish script : https://www.cyberciti.biz/tips/how-do-i-enable-remote-access-to-mysql-database-server.html

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServerServices : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_GITLAB_URL_MASTER = "gitlab.hephaiscode.com/Server/AutoSaveInstallSH/raw/master/";
        const string K_OUTPUT_FOLDER = "/etc/hephaiscode/";
        const string K_SERVER_INSTALL = "server_installer_";
        const string K_WEBSERVICE_ADD = "webservice_add_";
        const string K_WEBSERVICE_REMOVE = "webservice_remove_";
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight(float sWidth)
        {
            float tYadd = NWDGUI.AreaHeight(NWDGUI.kMiniButtonStyle.fixedHeight, 100);
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEditor(Rect sRect)
        {
            Rect[,] tMatrix = NWDGUI.DiviseArea(sRect, 2, 100);
            int tI = 0;
            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;

            if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Find IP from Server (NWDServerDomain)"))
            {
                string tLocalIP = "0.0.0.0";
                NWDServerDomain tDomain = Server.GetRawData();
                if (tDomain != null)
                {
                    foreach (IPAddress tIP in Dns.GetHostAddresses(tDomain.ServerDNS))
                    {
                        if (tIP.AddressFamily == AddressFamily.InterNetwork)
                        {
                            tLocalIP = tIP.ToString();
                        }
                    }
                    IP.SetValue(tLocalIP);
                }
                else
                {
                    IP.SetValue("0.0.0.0");
                }

            }
            tI++;

            if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Test SFTP"))
            {
                NWEBenchmark.Start("test benchmark");
                NWDServerAuthentication tServerAuthentication = GetServerSFTP(NWDAppConfiguration.SharedInstance().DevEnvironment);
                if (tServerAuthentication.ConnectSFTP())
                {
                    NWEBenchmark.Start("test benchmark test");
                    tServerAuthentication.TestSFTPWrite();
                    NWEBenchmark.Finish("test benchmark test");
                    tServerAuthentication.DeconnectSFTP();
                }
                NWEBenchmark.Finish("test benchmark");
            }
            tI++;

            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;

            if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Open Terminal"))
            {
                // /Applications/Utilities/Terminal.app/Contents/MacOS/Terminal
                FileInfo tFileInfo = new FileInfo("/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal");
                System.Diagnostics.Process.Start(tFileInfo.FullName);
            }
            tI++;
            if (IP != null && Root_Password != null && Admin_Password != null)
            {
                GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Install Server SSH");
                tI++;
                GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), NWDServerInstall.CommandInstallServerSSH(Distribution, IP.GetValue(), Port, Root_User, Root_Password.GetValue(), Admin_User, Admin_Password.GetValue()));
                tI += 11;

                GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Install Server Apache command");
                tI++;

                GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), NWDServerInstall.CommandInstallServerApache(Distribution, IP.GetValue(), Port, Admin_User, Admin_Password.GetValue(), Root_Password.GetValue()));
                tI += 11;

            }
            if (GUI.Button(tMatrix[0, tI], "http://" + IP))
            {
                Application.OpenURL("http://" + IP);
            }
            if (GUI.Button(tMatrix[1, tI], "https://" + IP))
            {
                Application.OpenURL("https://" + IP);
            }
            tI++;

            if (GUI.Button(tMatrix[0, tI], "http://" + "… phpinfo"))
            {
                Application.OpenURL("http://" + IP + "/phpinfo.php");
            }
            if (GUI.Button(tMatrix[1, tI], "https://" + "… phpinfo"))
            {
                Application.OpenURL("https://" + IP + "/phpinfo.php");
            }
            tI++;

            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;

            NWDServerDomain tServerDNS = Server.GetRawData();
            if (tServerDNS != null)
            {
                if (string.IsNullOrEmpty(tServerDNS.ServerDNS) == false)
                {

                    GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Zone DNS");
                    tI++;
                    GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), NWDServerInstall.CommandDNS(tServerDNS.ServerDNS, IP.GetValue()));
                    tI++;
                    if (GUI.Button(tMatrix[0, tI], "http://" + tServerDNS.ServerDNS))
                    {
                        Application.OpenURL("http://" + tServerDNS.ServerDNS);
                    }
                    if (GUI.Button(tMatrix[1, tI], "https://" + tServerDNS.ServerDNS))
                    {
                        Application.OpenURL("https://" + tServerDNS.ServerDNS);
                    }
                    tI++;

                    NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                    tI++;

                    GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Install WebService command");
                    tI++;

                    GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), NWDServerInstall.CommandInstallWebService(Distribution, IP.GetValue(), Port, Admin_User, Admin_Password.GetValue(), Root_Password.GetValue(), tServerDNS.ServerDNS, User, Password.GetValue(), Folder, Email));
                    tI += 11;

                    if (tServerDNS.Dev == true)
                    {
                        if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Upload dev files"))
                        {
                            NWDServerAuthentication tCon = GetServerSFTP(NWDAppConfiguration.SharedInstance().DevEnvironment);
                            if (tCon != null)
                            {
                                NWDAppConfiguration.SharedInstance().DevEnvironment.CreatePHP(NWDDataManager.SharedInstance().mTypeList, true, false, tCon);
                            }
                        }
                        tI++;
                    }
                    if (tServerDNS.Preprod == true)
                    {
                        if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Upload preprod files"))
                        {
                            NWDServerAuthentication tCon = GetServerSFTP(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                            if (tCon != null)
                            {
                                NWDAppConfiguration.SharedInstance().PreprodEnvironment.CreatePHP(NWDDataManager.SharedInstance().mTypeList, true, false, tCon);
                            }
                        }
                        tI++;
                    }
                    if (tServerDNS.Prod == true)
                    {
                        if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Upload prod files"))
                        {
                            NWDServerAuthentication tCon = GetServerSFTP(NWDAppConfiguration.SharedInstance().ProdEnvironment);
                            if (tCon != null)
                            {
                                NWDAppConfiguration.SharedInstance().ProdEnvironment.CreatePHP(NWDDataManager.SharedInstance().mTypeList, true, false, tCon);
                            }
                        }
                        tI++;
                    }
                }

                NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                tI++;

                /*
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "copy for dev"))
                {
                    NWDAppEnvironment tDev = NWDAppConfiguration.SharedInstance().DevEnvironment;
                    tDev.ServerHTTPS = "https://" + tServerDNS.ServerDNS;
                    tDev.SFTPHost = tServerDNS.ServerDNS;
                    tDev.SFTPBalanceLoad = tServerDNS.BalanceLoad;
                    tDev.SFTPPort = Port;
                    tDev.SFTPFolder = Folder;
                    tDev.SFTPUser = User;
                    tDev.SFTPPassword = Password.GetValue();
                    NWDAppEnvironmentConfigurationManager.Refresh();
                }
                tI++;

                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "copy for preprod"))
                {
                    NWDAppEnvironment tPreprod = NWDAppConfiguration.SharedInstance().PreprodEnvironment;
                    tPreprod.ServerHTTPS = "https://" + tServerDNS.ServerDNS;
                    tPreprod.SFTPHost = tServerDNS.ServerDNS;
                    tPreprod.SFTPBalanceLoad = tServerDNS.BalanceLoad;
                    tPreprod.SFTPPort = Port;
                    tPreprod.SFTPFolder = Folder;
                    tPreprod.SFTPUser = User;
                    tPreprod.SFTPPassword = Password.GetValue();
                    NWDAppEnvironmentConfigurationManager.Refresh();
                }
                tI++;

                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "copy for prod"))
                {
                    NWDAppEnvironment tProd = NWDAppConfiguration.SharedInstance().ProdEnvironment;
                    tProd.ServerHTTPS = "https://" + tServerDNS.ServerDNS;
                    tProd.SFTPHost = tServerDNS.ServerDNS;
                    tProd.SFTPBalanceLoad = tServerDNS.BalanceLoad;
                    tProd.SFTPPort = Port;
                    tProd.SFTPFolder = Folder;
                    tProd.SFTPUser = User;
                    tProd.SFTPPassword = Password.GetValue();
                    NWDAppEnvironmentConfigurationManager.Refresh();
                }
                tI++;
                */
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif