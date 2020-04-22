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
using System.Collections.Generic;

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

            GUIContent tButtonTitle = null;
            NWDServer tServer = Server.GetRawData();
            NWDServerDomain tServerDomain = ServerDomain.GetRawData();
            if (tServer != null)
            {
                //-----------------
                tButtonTitle = new GUIContent("Open terminal", " open terminal or console on your desktop");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    // /Applications/Utilities/Terminal.app/Contents/MacOS/Terminal
                    FileInfo tFileInfo = new FileInfo("/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal");
                    System.Diagnostics.Process.Start(tFileInfo.FullName);
                }
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
                tButtonTitle = new GUIContent("Install Apache PHP", "Install Apache and PHP 7");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    List<string> tCommandList = new List<string>();
                    tCommandList.Add("echo \"<color=red> -> server update</color>\"");
                    tCommandList.Add("apt-get update");
                    tCommandList.Add("apt-get -y upgrade");
                    tCommandList.Add("apt-get -y dist-upgrade");

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
                    //"sed -i 's/public_html\\/cgi-bin/public_html/g'/etc/apache2/suexec/www-data",;
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
                    tCommandList.Add("pear install Net_SMTP");

                    tCommandList.Add("echo \"<color=red> -> php folder default</color>\"");
                    tCommandList.Add("chgrp -R www-data /var/www/html/");
                    tCommandList.Add("chmod 750 /var/www/html/");

                    tCommandList.Add("echo \"<color=red> -> files default</color>\"");
                    tCommandList.Add("echo $\"<?php echo phpinfo();?>\" > /var/www/html/phpinfo.php");
                    tCommandList.Add("echo $\"Are you lost? Ok, I'll help you, you're in front of a screen!\" > /var/www/html/index.html");

                    tCommandList.Add("echo \"<color=red> -> install Let's Encrypt Certbot</color>\"");
                    tCommandList.Add("echo $\"deb http://ftp.debian.org/debian stretch-backports main\" >> /etc/apt/sources.list.d/backports.list");
                    tCommandList.Add("apt-get update");
                    tCommandList.Add("apt-get -y install python-certbot-apache -t stretch-backports");

                    tCommandList.Add("echo \"<color=red> -> apache restart</color>\"");
                    tCommandList.Add("systemctl restart apache2");

                    if (tServer != null)
                    {
                        tServer.ExecuteSSH(tButtonTitle.text, tCommandList);
                    }
                }
                tI++;
                //-----------------
                EditorGUI.BeginDisabledGroup(UserInstalled == true);
                tButtonTitle = new GUIContent("Install User", "Install User");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    if (tServerDomain != null)
                    {
                        tServer.ExecuteSSH(tButtonTitle.text, new List<string>()
                {
                "addgroup "+NWDServer.K_SFTP_chroot+"", // Add group exists",
                "useradd --shell /bin/false " + User + "",
                "echo " + User + ":" + Password + " | chpasswd",
                "usermod -a -G "+NWDServer.K_SFTP_chroot+" " + User + "",
                "mkdir /home/" + User + "",
                "chown root /home/" + User + "",
                "chmod go-w  /home/" + User + "",

                "mkdir /home/" + User + "/ssl",
                "openssl req -x509 -sha256 -nodes -days 365 -newkey rsa:2048 -keyout /home/" + User + "/ssl/" + tServerDomain.ServerDNS + ".key -out /home/" + User + "/ssl/" + tServerDomain.ServerDNS + ".crt -subj \"/C=FR/ST=LILLE/L=LILLE/O=Global Security/OU=IT Department/CN=" + tServerDomain.ServerDNS + "\"",
                "chown -R " + User + ":www-data /home/" + User + "/ssl",
                "chmod -R 750 /home/" + User + "/ssl",

                "mkdir /home/" + User + "/" + Folder + "",
                "echo $\"<?php echo phpinfo();?>\" > /home/" + User + "/" + Folder + "/phpinfo.php",
                "echo $\"Hello " + tServerDomain.ServerDNS + "!\" > /home/" + User + "/" + Folder + "/index.html",
                "chown -R " + User + ":www-data /home/" + User + "/" + Folder + "",
                "chmod -R 750 /home/" + User + "/" + Folder + "",

                "mkdir /home/" + User + "/" + Folder + "_NoSSL",
                "echo $\"<?php echo phpinfo();?>\" > /home/" + User + "/" + Folder + "_NoSSL"+ "/phpinfo.php",
                "echo $\"Hello " + tServerDomain.ServerDNS + ", please use SSL connexion!\" > /home/" + User + "/" + Folder + "_NoSSL"+ "/index.html",
                "chown -R " + User + ":www-data /home/" + User + "/" + Folder + "_NoSSL"+ "",
                "chmod -R 750 /home/" + User + "/" + Folder + "_NoSSL"+ "",

                "/etc/init.d/apache2 stop",


                "rm /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",


                // create virtual host without SSL
                "echo \"<VirtualHost *:80>\" > /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",
                "sed -i '$ a ServerAdmin " + tServerDomain.ServerDNS + "' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",
                "sed -i '$ a DocumentRoot /home/" + User + "/" + Folder + "_NoSSL"+ "' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",
                "sed -i '$ a ServerName " + tServerDomain.ServerDNS + "' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",
                "sed -i '$ a DocumentRoot /home/" + User + "/" + Folder + "_NoSSL"+ "' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",
                "sed -i '$ a <Directory />' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",
                "sed -i '$ a AllowOverride All' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",
                "sed -i '$ a </Directory>' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",
                "sed -i '$ a <Directory /home/" + User + "/" + Folder + "_NoSSL" + ">' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",
                "sed -i '$ a Options Indexes FollowSymLinks MultiViews' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",
                "sed -i '$ a AllowOverride all' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",
                "sed -i '$ a Require all granted' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",
                "sed -i '$ a </Directory>' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",
                "sed -i '$ a ErrorLog /var/log/apache2/" + tServerDomain.ServerDNS + "-error.log' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",
                "sed -i '$ a LogLevel error' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",
                "sed -i '$ a </VirtualHost>' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ws.conf",

                "a2ensite " + tServerDomain.ServerDNS + "_ws.conf",

                
                // create virtual host with SSL
                "rm /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",

                "echo \"<VirtualHost *:443>\" > /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a ServerAdmin " + tServerDomain.ServerDNS + "' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a ServerName " + tServerDomain.ServerDNS + "' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a ServerAlias " + tServerDomain.ServerDNS + "' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a DocumentRoot /home/" + User + "/" + Folder + "' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a <Directory />' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a AllowOverride All' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a </Directory>' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a <Directory /home/" + User + "/" + Folder + ">' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a Options Indexes FollowSymLinks MultiViews' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a AllowOverride all' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a Require all granted' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a </Directory>' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a ErrorLog /var/log/apache2/" + tServerDomain.ServerDNS + "-error.log' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a LogLevel error' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a SSLEngine On' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a SSLProtocol all -SSLv2 -SSLv3' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a SSLCipherSuite ALL:!DH:!EXPORT:!RC4:+HIGH:+MEDIUM:!LOW:!aNULL:!eNULL' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a SSLCertificateFile /home/" + User + "/ssl/" + tServerDomain.ServerDNS + ".crt' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a SSLCertificateKeyFile /home/" + User + "/ssl/" + tServerDomain.ServerDNS + ".key' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a CustomLog /var/log/apache2/" + tServerDomain.ServerDNS + "-ssl-access.log combined' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",
                "sed -i '$ a </VirtualHost>' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + "_ssl_ws.conf",

                "a2ensite " + tServerDomain.ServerDNS + "_ssl_ws.conf",

                "/etc/init.d/apache2 start",
                "/etc/init.d/apache2 reload",

                "service sshd restart",
            },
                           delegate (string sCommand, string sResult)
                           {
                               if (sCommand == "service sshd restart")
                               {
                                   UserInstalled = true;
                                   UpdateDataIfModified();
                               };
                           });
                    }
                }
                EditorGUI.EndDisabledGroup();
                tI++;

                //-----------------
                tButtonTitle = new GUIContent("Check apache", "check apache ");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    tServer.ExecuteSSH(tButtonTitle.text, new List<string>()
                {
                    "apache2ctl -S",
                    "httpd -S",
                });
                }
                tI++;
                //-----------------
                tButtonTitle = new GUIContent("Try User connexion", " try connexion with user");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    tServer.ExecuteSSH(tButtonTitle.text, new List<string>()
                {
                    "ls",
                }, null, tServer.Port, User, Password.GetValue());
                }
                tI++;

                //-----------------

                tButtonTitle = new GUIContent("default html", " try connexion to index.html");
                if (GUI.Button(tMatrix[0, tI], tButtonTitle))
                {
                    Application.OpenURL("http://" + tServer.DomainNameServer + "/index.html");
                }
                tButtonTitle = new GUIContent("default phpinfo", " try connexion to index.html with ssl");
                if (GUI.Button(tMatrix[1, tI], tButtonTitle))
                {
                    Application.OpenURL("http://" + tServer.DomainNameServer + "/phpinfo.php");
                }
                tI++;


                tButtonTitle = new GUIContent("WS html", " try connexion to index.html");
                if (GUI.Button(tMatrix[0, tI], tButtonTitle))
                {
                    Application.OpenURL("http://" + tServerDomain.ServerDNS + "/index.html");
                }
                tButtonTitle = new GUIContent("WS ssl html", " try connexion to index.html with ssl");
                if (GUI.Button(tMatrix[1, tI], tButtonTitle))
                {
                    Application.OpenURL("https://" + tServerDomain.ServerDNS + "/index.html");
                }
                tI++;
                tButtonTitle = new GUIContent("WS phpinfo", " try connexion to phpinfo.php");
                if (GUI.Button(tMatrix[0, tI], tButtonTitle))
                {
                    Application.OpenURL("http://" + tServerDomain.ServerDNS + "/phpinfo.php");
                }
                tButtonTitle = new GUIContent("WS ssl phpinfo", " try connexion to phpinfo.php with ssl");
                if (GUI.Button(tMatrix[1, tI], tButtonTitle))
                {
                    Application.OpenURL("https://" + tServerDomain.ServerDNS + "/phpinfo.php");
                }
                tI++;

                //-----------------
                tButtonTitle = new GUIContent("Try certbot SSL", " try connexion to generate certbot ssl (lest's encrypt)");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    tServer.ExecuteSSH(tButtonTitle.text, new List<string>()
                {
                        //"certbot --agree-tos --no-eff-email --apache --redirect --email " + Email + " -d " + tServerDomain.ServerDNS + "",
                        "certbot --agree-tos --no-eff-email --apache --redirect --email " + Email + " -d " + tServerDomain.ServerDNS + "",
                    });
                }
                tI++;

                //GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), tServer.TextCommandResult);
            }





            /*
            //if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Find IP from Server (NWDServerDomain)"))
            //{
            //    string tLocalIP = "0.0.0.0";
            //    NWDServerDomain tDomain = ServerDomain.GetRawData();
            //    if (tDomain != null)
            //    {
            //        foreach (IPAddress tIP in Dns.GetHostAddresses(tDomain.ServerDNS))
            //        {
            //            if (tIP.AddressFamily == AddressFamily.InterNetwork)
            //            {
            //                tLocalIP = tIP.ToString();
            //            }
            //        }
            //        IP.SetValue(tLocalIP);
            //    }
            //    else
            //    {
            //        IP.SetValue("0.0.0.0");
            //    }

            //}
            //tI++;

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
            if (Server != null)
            {
                NWDServer tServer = Server.GetRawData();
                if (tServer != null)
                {
                    if (tServer.IP != null && tServer.Root_Password != null && tServer.Admin_Password != null)
                    {
                        GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Install Server SSH");
                        tI++;
                        GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), NWDServerInstall.CommandInstallServerSSH(tServer));
                        tI += 11;

                        GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Install Server Apache command");
                        tI++;

                        GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), NWDServerInstall.CommandInstallServerApache(tServer));
                        tI += 11;

                    }
                }
                if (GUI.Button(tMatrix[0, tI], "http://" + tServer.IP.GetValue()))
                {
                    Application.OpenURL("http://" + tServer.IP.GetValue());
                }
                if (GUI.Button(tMatrix[1, tI], "https://" + tServer.IP.GetValue()))
                {
                    Application.OpenURL("https://" + tServer.IP.GetValue());
                }
                tI++;

                if (GUI.Button(tMatrix[0, tI], "http://" + "… phpinfo"))
                {
                    Application.OpenURL("http://" + tServer.IP.GetValue() + "/phpinfo.php");
                }
                if (GUI.Button(tMatrix[1, tI], "https://" + "… phpinfo"))
                {
                    Application.OpenURL("https://" + tServer.IP.GetValue() + "/phpinfo.php");
                }
                tI++;
            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;

            NWDServerDomain tServerDNS = ServerDomain.GetRawData();
            if (tServerDNS != null)
            {
                if (string.IsNullOrEmpty(tServerDNS.ServerDNS) == false)
                {

                    GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Zone DNS");
                    tI++;
                    GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), NWDServerInstall.CommandDNS(tServerDNS.ServerDNS, tServer.IP.GetValue()));
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
                    GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), NWDServerInstall.CommandInstallWebService(Server.GetRawData(), tServerDNS.ServerDNS, tServer.Admin_User, tServer.Admin_Password.GetValue(), Folder, Email));
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

            }
            */
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif