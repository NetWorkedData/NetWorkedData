﻿//=====================================================================================================================
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
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    // doc to read to finish script : https://www.cyberciti.biz/tips/how-do-i-enable-remote-access-to-mysql-database-server.html

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServerDatas : NWDBasis
    {
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
            GUIStyle tSyleTextArea = new GUIStyle(GUI.skin.textArea);
            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;

            GUIContent tButtonTitle = null;

            NWDServer tServer = Server.GetRawData();
            if (tServer != null)
            {
                //-----------------
                // find ip of server by dns if associated
                EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(tServer.DomainNameServer) == true);
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Find IP from Server (NWDServerDomain)"))
                {
                    string tLocalIP = "0.0.0.0";
                    foreach (IPAddress tIP in Dns.GetHostAddresses(tServer.DomainNameServer))
                    {
                        if (tIP.AddressFamily == AddressFamily.InterNetwork)
                        {
                            tLocalIP = tIP.ToString();
                        }
                    }
                    MySQLIP.SetValue(tLocalIP);
                }
                EditorGUI.EndDisabledGroup();
                tI++;
                //-----------------
                tButtonTitle = new GUIContent("Try connexion", " try connexion with root or admin");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    tServer.ExecuteSSH(tButtonTitle.text, new List<string>()
                {
                    "ls",
                });
                }
                tI++;
                //-----------------
                tButtonTitle = new GUIContent("install MariaDB", " try install MariaDB (fork of MySQL)");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    List<string> tCommandList = new List<string>();

                    tCommandList.Add("echo \"<color=red> -> server update</color>\"");
                    tCommandList.Add("apt-get update");
                    tCommandList.Add("apt-get -y upgrade");
                    tCommandList.Add("apt-get -y dist-upgrade");

                    tCommandList.Add("echo \"<color=red> -> mysql install (MariaDB)</color>\"");
                    tCommandList.Add("debconf-set-selections <<< \"mariadb-server mysql-server/root_password password " + Root_MysqlPassword + "\"");
                    tCommandList.Add("debconf-set-selections <<< \"mariadb-server mysql-server/root_password_again password " + Root_MysqlPassword + "\"");
                    tCommandList.Add("apt-get -y install mariadb-server");
                    tCommandList.Add("echo PURGE | debconf-communicate mariadb-server");
                    tCommandList.Add("echo \"<color=red> -> mysql start (MariaDB)</color>\"");
                    tCommandList.Add("/etc/init.d/mysql start");

                    if (External == true)
                    {

                    }
                    if (PhpMyAdmin == true)
                    {
                        tCommandList.Add("echo \"<color=red> -> install apache</color>\"");
                        tCommandList.Add("apt-get -y install apache2");
                        tCommandList.Add("apt-get -y install apache2-doc");
                        tCommandList.Add("apt-get -y install apache2-suexec-custom");
                        tCommandList.Add("apt-get -y install logrotate");

                        tCommandList.Add("echo \"<color=red> -> active apache mod</color>\"");
                        tCommandList.Add("a2enmod ssl");
                        tCommandList.Add("a2enmod userdir");
                        tCommandList.Add("a2enmod suexec");

                        tCommandList.Add("echo \"<color=red> -> apache configure</color>\"");
                        tCommandList.Add("sed -i 's/\\/var\\/www/\\/home/g' /etc/apache2/suexec/www-data");
                        tCommandList.Add("sed -i 's/^.*ServerSignature .*$//g' /etc/apache2/apache2.conf");
                        tCommandList.Add("sed -i '$ a ServerSignature Off' /etc/apache2/apache2.conf");

                        tCommandList.Add("echo \"<color=red> -> apache restart</color>\"");
                        tCommandList.Add("systemctl restart apache2");

                        tCommandList.Add("echo \"<color=red> -> install php</color>\"");
                        tCommandList.Add("apt-get -y install php");
                        //tCommandList.Add("apt-get -y install php-gd");
                        //tCommandList.Add("apt-get -y install php-bz2");
                        //tCommandList.Add("apt-get -y install php-tcpdf");
                        tCommandList.Add("apt-get -y install php-mysql");
                        tCommandList.Add("apt-get -y install php-curl");
                        tCommandList.Add("apt-get -y install php-json");
                        tCommandList.Add("apt-get -y install php-mcrypt");
                        tCommandList.Add("apt-get -y install php-mbstring");
                        tCommandList.Add("apt-get -y install php-gettext");
                        tCommandList.Add("apt-get -y install php-zip");
                        tCommandList.Add("apt-get -y install php-mail");
                        tCommandList.Add("apt-get -y install php-pear");
                        tCommandList.Add("apt-get -y install libapache2-mod-php");

                        tCommandList.Add("echo \"<color=red> -> phpmyadmin install</color>\"");
                        tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/dbconfig-install boolean true\"");
                        tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/app-password-confirm password " + Root_MysqlPassword + "\"");
                        tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/mysql/admin-pass password " + Root_MysqlPassword + "\"");
                        tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/mysql/app-pass password " + Root_MysqlPassword + "\"");
                        tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/reconfigure-webserver multiselect none\"");
                        tCommandList.Add("apt-get -y -q install phpmyadmin");
                        tCommandList.Add("echo PURGE | debconf-communicate phpmyadmin");
                        tCommandList.Add("systemctl restart apache2");
                    }
                    tServer.ExecuteSSH(tButtonTitle.text, tCommandList);
                }
                tI++;

                //tButtonTitle = new GUIContent("install PhpMyAdmin", " try install PhpMyAdmin");
                //if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                //{
                //    List<string> tCommandList = new List<string>();

                //    tCommandList.Add("echo \"<color=red> -> server update</color>\"");
                //    tCommandList.Add("apt-get update");
                //    tCommandList.Add("apt-get -y upgrade");
                //    tCommandList.Add("apt-get -y dist-upgrade");
                //    if (PhpMyAdmin == true)
                //    {
                //        tCommandList.Add("echo \"<color=red> -> install php</color>\"");

                //        tCommandList.Add("echo \"<color=red> -> phpmyadmin install</color>\"");
                //        tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/dbconfig-install boolean true\"");
                //        tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/app-password-confirm password " + Root_MysqlPassword + "\"");
                //        tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/mysql/admin-pass password " + Root_MysqlPassword + "\"");
                //        tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/mysql/app-pass password " + Root_MysqlPassword + "\"");
                //        tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/reconfigure-webserver multiselect none\"");
                //        tCommandList.Add("apt-get -y -q install phpmyadmin");
                //        tCommandList.Add("echo PURGE | debconf-communicate phpmyadmin");
                //        tCommandList.Add("systemctl restart apache2");
                //    }
                //    tServer.ExecuteSSH(tButtonTitle.text, tCommandList);
                //}
                //tI++;

                tButtonTitle = new GUIContent("Install User", " try install User In MariaDB");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    List<string> tCommandList = new List<string>();

                    tCommandList.Add("echo \"<color=red> -> add user in mysql</color>\"");
                    tCommandList.Add("echo \" -> add user in mysql\"");
                    tCommandList.Add("mysql -u root -p\"" + Root_MysqlPassword + "\" -e \"create database " + MySQLBase + ";\"");
                    tCommandList.Add("mysql -u root -p\"" + Root_MysqlPassword + "\" -e \"GRANT ALL PRIVILEGES ON " + MySQLBase + ".* TO " + MySQLUser + "@localhost IDENTIFIED BY '" + MySQLPassword + "';\"");

                    tServer.ExecuteSSH(tButtonTitle.text, tCommandList);
                }
                tI++;


                if (GUI.Button(tMatrix[0, tI], "http://xxx/phpmyadmin/"))
                {
                    Application.OpenURL("http://" + MySQLIP.GetValue() + "/phpmyadmin/");
                }
                if (GUI.Button(tMatrix[1, tI], "https://xxx/phpmyadmin/"))
                {
                    Application.OpenURL("https://" + MySQLIP.GetValue() + "/phpmyadmin/");
                }
                tI++;

                //}
                //if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Find IP from Server (NWDServerDomain)"))
                //{
                //    string tLocalIP = "0.0.0.0";
                //    //NWDServerDomain tDomain = Server.GetRawData();
                //    //if (tDomain != null)
                //    //{
                //    //    foreach (IPAddress tIP in Dns.GetHostAddresses(tDomain.ServerDNS))
                //    //    {
                //    //        if (tIP.AddressFamily == AddressFamily.InterNetwork)
                //    //        {
                //    //            tLocalIP = tIP.ToString();
                //    //        }
                //    //    }
                //    //    IP.SetValue(tLocalIP);
                //    //}
                //    //else
                //    //{
                //    //    IP.SetValue("0.0.0.0");
                //    //}

                //}
                //tI++;

                //if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Open Terminal"))
                //{
                //    // /Applications/Utilities/Terminal.app/Contents/MacOS/Terminal
                //    FileInfo tFileInfo = new FileInfo("/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal");
                //    System.Diagnostics.Process.Start(tFileInfo.FullName);
                //}
                //tI++;

                //NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                //tI++;

                //GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Install Server MySQL command");
                //tI++;
                //if (Server != null)
                //{
                //    GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), NWDServerInstall.CommandInstallServerMySQL(Server.GetRawData(), Root_MysqlPassword.GetValue(), External, PhpMyAdmin));
                //    tI += 11;

                //    if (PhpMyAdmin == true && Server.GetRawData() != null)
                //    {

                //        if (GUI.Button(tMatrix[0, tI], "http://" + Server.GetRawData().IP + "/phpmyadmin/"))
                //        {
                //            Application.OpenURL("http://" + Server.GetRawData().IP + "/phpmyadmin/");
                //        }
                //        if (GUI.Button(tMatrix[1, tI], "https://" + Server.GetRawData().IP + "/phpmyadmin/"))
                //        {
                //            Application.OpenURL("https://" + Server.GetRawData().IP + "/phpmyadmin/");
                //        }
                //        tI++;
                //    }

                //NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                //tI++;

                //GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Install Database command");
                //tI++;
                //GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), NWDServerInstall.CommandInstallDatabase(Server.GetRawData(), Root_MysqlPassword.GetValue(), MySQLUser, MySQLPassword.GetValue(), MySQLBase));





                tI += 11;
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Push force dev editor data"))
                {
                    //TODO : push data ...
                }
                tI++;
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Push force preprod editor data"))
                {
                    //TODO : push data ...
                }
                tI++;
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Push force prod editor data"))
                {
                    //TODO : push data ...
                }
                tI++;

                NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                tI++;
            }
            /*
            if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "copy for dev"))
            {
                NWDAppEnvironment tDev = NWDAppConfiguration.SharedInstance().DevEnvironment;
                tDev.ServerHost = "localhost";
                tDev.ServerUser = MySQLUser;
                tDev.ServerPassword = MySQLPassword.GetValue();
                tDev.ServerBase = MySQLBase;
                NWDAppEnvironmentConfigurationManager.Refresh();
            }
            tI++;
            if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "copy for preprod"))
            {
                NWDAppEnvironment tPreprod= NWDAppConfiguration.SharedInstance().PreprodEnvironment;
                tPreprod.ServerHost = "localhost";
                tPreprod.ServerUser = MySQLUser;
                tPreprod.ServerPassword = MySQLPassword.GetValue();
                tPreprod.ServerBase = MySQLBase;
                NWDAppEnvironmentConfigurationManager.Refresh();
            }
            tI++;
            if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "copy for prod"))
            {
                NWDAppEnvironment tProd = NWDAppConfiguration.SharedInstance().ProdEnvironment;
                tProd.ServerHost = "localhost";
                tProd.ServerUser = MySQLUser;
                tProd.ServerPassword = MySQLPassword.GetValue();
                tProd.ServerBase = MySQLBase;
                NWDAppEnvironmentConfigurationManager.Refresh();
            }
            tI++;

            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;
            */
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif