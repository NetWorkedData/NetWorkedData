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

//=====================================================================================================================
namespace NetWorkedData
{
    // doc to read to finish script : https://www.cyberciti.biz/tips/how-do-i-enable-remote-access-to-mysql-database-server.html

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServerSFTP : NWDBasis
    {
        const string K_GITLAB_URL_MASTER = "gitlab.hephaiscode.com/Server/AutoSaveInstallSH/raw/master/";
        const string K_OUTPUT_FOLDER = "/etc/hephaiscode/";
        const string K_SERVER_INSTALL = "server_installer_";
        const string K_WEBSERVICE_ADD = "webservice_add_";
        const string K_WEBSERVICE_REMOVE = "webservice_remove_";
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight(float sWidth)
        {
            float tYadd = NWDGUI.AreaHeight(NWDGUI.kMiniButtonStyle.fixedHeight, 40);
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallServer(NWDServerDistribution sDistribution, string sIP, int sPort, string sServerName, string sNewRootPassword, bool sMySQL, bool sPhpMyAdmin, bool sPostfix)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("ssh -l root " + sIP + " -p22");
            if (sPort != 22)
            {
                tScriptServer.AppendLine("# or ssh -l root " + sIP + " -p" + sPort);
            }
            tScriptServer.AppendLine("");
            tScriptServer.AppendLine("sudo mkdir -p " + K_OUTPUT_FOLDER);
            tScriptServer.AppendLine("");
            tScriptServer.AppendLine("sudo wget --no-cache https" + "://" + K_GITLAB_URL_MASTER + K_SERVER_INSTALL + sDistribution.ToString() + ".sh --output-document=" + K_OUTPUT_FOLDER + K_SERVER_INSTALL + sDistribution.ToString() + ".sh");
            tScriptServer.AppendLine("");
            tScriptServer.AppendLine("sudo chmod +x " + K_OUTPUT_FOLDER + K_SERVER_INSTALL + sDistribution.ToString() + ".sh");
            tScriptServer.AppendLine("");
            string tServerName = sServerName;
            if (string.IsNullOrEmpty(tServerName) == true)
            {
                tServerName = "NoServerName";
            }
            tScriptServer.AppendLine("sudo " + K_OUTPUT_FOLDER + K_SERVER_INSTALL + sDistribution.ToString() + ".sh"
                + " no@contact.me"
                + " " + tServerName
                + " " + sMySQL.ToString().ToLower()
                + " " + sPhpMyAdmin.ToString().ToLower()
                + " " + sPostfix.ToString().ToLower()
                + " " + sNewRootPassword.ToString()
                + " " + sPort.ToString()
                );
            tScriptServer.AppendLine("");
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandDNS(string sServerHTTPS, string sIP)
        {
            StringBuilder tScriptDNS = new StringBuilder();
            tScriptDNS.AppendLine(sServerHTTPS + " 10800 A " + sIP);
            return tScriptDNS.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandWebserviceSFTP(NWDServerDistribution sDistribution, string sIP, int sPort, string sDNS, string sUser, string sPassword, string sFolder, string sEmail)
        {
            StringBuilder tScriptAppache = new StringBuilder();
            tScriptAppache.AppendLine("ssh -l root " + sIP + " -p" + sPort);
            if (sPort != 22)
            {
                tScriptAppache.AppendLine("# or ssh -l root " + sIP + " -p22");
            }
            tScriptAppache.AppendLine("");
            tScriptAppache.AppendLine("sudo mkdir -p /etc/hephaiscode/");
            tScriptAppache.AppendLine("");
            tScriptAppache.AppendLine("sudo wget --no-cache https://" + K_GITLAB_URL_MASTER + K_WEBSERVICE_ADD + sDistribution.ToString() + ".sh --output-document=" + K_OUTPUT_FOLDER + K_WEBSERVICE_ADD + sDistribution.ToString() + ".sh");
            tScriptAppache.AppendLine("sudo wget --no-cache https://" + K_GITLAB_URL_MASTER + K_WEBSERVICE_REMOVE + sDistribution.ToString() + ".sh --output-document=" + K_OUTPUT_FOLDER + K_WEBSERVICE_REMOVE + sDistribution.ToString() + ".sh");
            tScriptAppache.AppendLine("");
            tScriptAppache.AppendLine("sudo chmod +x " + K_OUTPUT_FOLDER + K_WEBSERVICE_ADD + sDistribution.ToString() + ".sh");
            tScriptAppache.AppendLine("sudo chmod +x " + K_OUTPUT_FOLDER + K_WEBSERVICE_REMOVE + sDistribution.ToString() + ".sh");
            tScriptAppache.AppendLine("");

            tScriptAppache.AppendLine("sudo " + K_OUTPUT_FOLDER + K_WEBSERVICE_ADD + sDistribution.ToString() + ".sh"
                + " " + sDNS
                + " " + sUser
                + " " + sPassword
                + " " + sFolder
                + " " + sEmail
                );
            tScriptAppache.AppendLine("# if you need to reactivate certbot do :");
            tScriptAppache.AppendLine("certbot --agree-tos --no-eff-email --apache --redirect --email " + sEmail + " -d " + sDNS + "");

            return tScriptAppache.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEditor(Rect sRect)
        {
            Rect[,] tMatrix = NWDGUI.DiviseArea(sRect, 2, 40);
            int tI = 0;
            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;
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

            if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Open Terminal"))
            {
                // /Applications/Utilities/Terminal.app/Contents/MacOS/Terminal
                FileInfo tFileInfo = new FileInfo("/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal");
                System.Diagnostics.Process.Start(tFileInfo.FullName);
            }
            tI++;

            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;
            GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Install Server command");
            tI++;
            GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), CommandInstallServer(Distribution, IP.GetValue(), Port, ServerName, RootPassword.GetValue(), false, false, false));
            tI += 11;

            NWDServerDNS tServerDNS = Server.GetRawData();
            if (tServerDNS != null)
            {
                if (string.IsNullOrEmpty(tServerDNS.ServerHTTPS) == false)
                {

                    if (GUI.Button(tMatrix[0, tI], "http://" + tServerDNS.ServerHTTPS))
                    {
                        Application.OpenURL("http://" + tServerDNS.ServerHTTPS);
                    }
                    if (GUI.Button(tMatrix[1, tI], "https://" + tServerDNS.ServerHTTPS))
                    {
                        Application.OpenURL("https://" + tServerDNS.ServerHTTPS);
                    }
                    tI++;

                    GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Zone DNS");
                    tI++;
                    GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), CommandDNS(tServerDNS.ServerHTTPS, IP.GetValue()));
                    tI++;
                    GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Install WebService command");
                    tI++;
                    GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), CommandWebserviceSFTP(Distribution, IP.GetValue(), Port, tServerDNS.ServerHTTPS, User, Password.GetValue(), Folder, Email));
                    tI += 11;
                    if (tServerDNS.Dev == true)
                    {
                        if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Upload dev files"))
                        {
                            NWDServerAuthentification tCon = GetServerSFTP(NWDAppConfiguration.SharedInstance().DevEnvironment);
                            if (tCon != null)
                            {
                                NWDAppConfiguration.SharedInstance().DevEnvironment.CreatePHP(NWDDataManager.SharedInstance().mTypeList, true, false, tCon);
                            }
                        }
                        tI++;
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif