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
using System.Text;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServerServices : NWDBasisUnsynchronize
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
            PropertiesPrevent();
            Rect[,] tMatrix = NWDGUI.DiviseArea(sRect, 2, 100);
            int tI = 0;
            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;

            //EditorGUI.LabelField(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Continent", Continent.ConcatRepresentations());
            //tI++;


            if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerate))
            {
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Credentials window"))
                {
                    NWDProjectCredentialsManager.SharedInstanceFocus();
                }
                tI++;
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Flush credentials"))
                {
                    NWDProjectCredentialsManagerContent.FlushCredentials(NWDCredentialsRequired.ForSFTPGenerate);
                }
                tI++;
                NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                tI++;
                GUIContent tButtonTitle = null;
                NWDServer tServer = Server.GetRawData();
                NWDServerDomain tServerDomain = ServerDomain.GetRawData();
                if (tServer != null)
                {
                    //-----------------
                    EditorGUI.HelpBox(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 1]), "Don't forgot to check your ~/.ssh/known_hosts file permission!", MessageType.Warning);
                    tI += 2;
                    string tcommandKeyGen = "ssh-keygen -R " + tServer.IP.GetValue() + " & ssh-keygen -R " + tServer.IP.GetValue() + ":" + tServer.Port + " & ssh " + tServer.IP.GetValue() + " -l " + tServer.Root_User + " -p " + tServer.Port;
                    if (tServer.AdminInstalled)
                    {
                        tcommandKeyGen = "ssh-keygen -R " + tServer.IP.GetValue() + " & ssh -keygen -R " + tServer.IP.GetValue() + ":" + tServer.Port + " & ssh " + tServer.IP.GetValue() + " -l " + tServer.Admin_User + " -p " + tServer.Port;
                    }
                    tI++;
                    GUI.TextField(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 1]), tcommandKeyGen);
                    tI += 2;
                    NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
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
                        tCommandList.Add("apt-get -y install openssl");

                        tCommandList.Add("echo \"<color=red> -> active apache mod</color>\"");
                        tCommandList.Add("a2enmod ssl");
                        tCommandList.Add("a2enmod userdir");
                        tCommandList.Add("a2enmod suexec");
                        if (tServer.Distribution == NWDServerDistribution.debian10)
                        {
                            tCommandList.Add("a2enmod http2");
                        }

                        tCommandList.Add("echo \"<color=red> -> apache configure</color>\"");
                        tCommandList.Add("sed -i 's/\\/var\\/www/\\/home/g' /etc/apache2/suexec/www-data");
                        //"sed -i 's/public_html\\/cgi-bin/public_html/g'/etc/apache2/suexec/www-data",;
                        tCommandList.Add("sed -i 's/^.*ServerSignature .*$//g' /etc/apache2/apache2.conf");
                        tCommandList.Add("sed -i '$ a ServerSignature Off' /etc/apache2/apache2.conf");


                        tCommandList.Add("sed -i 's/^.*SSLProtocol .*$//g' /etc/apache2/apache2.conf");
                        tCommandList.Add("sed -i '$ a SSLProtocol all -SSLv2 -SSLv3 -TLSv1 -TLSv1.1' /etc/apache2/apache2.conf");


                        tCommandList.Add("echo \"<color=red> -> apache restart</color>\"");
                        tCommandList.Add("systemctl restart apache2");

                        if (tServer.Distribution == NWDServerDistribution.debian9)
                        {
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
                            tCommandList.Add("apt-get -y install php-geoip");
                            tCommandList.Add("apt-get -y install libapache2-mod-php");
                            tCommandList.Add("pear install Net_SMTP");
                        }
                        if (tServer.Distribution == NWDServerDistribution.debian10)
                        {
                            tCommandList.Add("echo \"<color=red> -> install php</color>\"");
                            tCommandList.Add("apt-get -y install php7.3-fpm");
                            tCommandList.Add("sudo a2dismod php7.3");
                            tCommandList.Add("sudo a2enconf php7.3-fpm");
                            tCommandList.Add("sudo a2enmod proxy_fcgi");
                            tCommandList.Add("apt-get -y install php-mysql");
                            tCommandList.Add("apt-get -y install php-curl");
                            tCommandList.Add("apt-get -y install php-json");
                            tCommandList.Add("apt-get -y install php-mcrypt");
                            tCommandList.Add("apt-get -y install php-mbstring");
                            tCommandList.Add("apt-get -y install php-gettext");
                            tCommandList.Add("apt-get -y install php-zip");
                            tCommandList.Add("apt-get -y install php-mail");
                            tCommandList.Add("apt-get -y install php-pear");
                            tCommandList.Add("pear install Net_SMTP");

                            // try install Geoip
                            //tCommandList.Add("apt-get -y install php-geoip");
                            //tCommandList.Add("apt-get -y install geoip-bin");
                            //tCommandList.Add("apt-get -y install libapache2-mod-geoip");
                            //tCommandList.Add("apt-get -y install libgeoip1");
                            //tCommandList.Add("a2enmod geoip");
                            //tCommandList.Add("sed -i 's/^.*GeoIPEnable On.*$/GeoIPEnable Off/g' /etc/apache2/mods-available/geoip.conf");
                            //tCommandList.Add("sed -i 's/^.*GeoIPEnable Off.*$/GeoIPEnable On/g' /etc/apache2/mods-available/geoip.conf");

                        }
                        tCommandList.Add("systemctl restart apache2");
                        tCommandList.Add("echo \"<color=red> -> php folder default</color>\"");
                        tCommandList.Add("chgrp -R www-data /var/www/html/");
                        tCommandList.Add("chmod 750 /var/www/html/");

                        tCommandList.Add("echo \"<color=red> -> files default</color>\"");
                        tCommandList.Add("echo $\"<?php echo phpinfo();?>\" > /var/www/html/phpinfo.php");
                        tCommandList.Add("echo $\"Are you lost? Ok, I'll help you, you're in front of a screen!\" > /var/www/html/index.html");

                        if (tServer.Distribution == NWDServerDistribution.debian9)
                        {
                            tCommandList.Add("echo \"<color=red> -> install Let's Encrypt Certbot</color>\"");
                            tCommandList.Add("echo $\"deb http://ftp.debian.org/debian stretch-backports main\" >> /etc/apt/sources.list.d/backports.list");
                            tCommandList.Add("apt-get update");
                            tCommandList.Add("apt-get -y install python-certbot-apache -t stretch-backports");
                        }
                        if (tServer.Distribution == NWDServerDistribution.debian10)
                        {
                            tCommandList.Add("apt-get -y install certbot python-certbot-apache");
                        }

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
                            string _NoSSL = "_NoSSL";
                            string _SSL = "";
                            tServer.ExecuteSSH(tButtonTitle.text, new List<string>()
                {

                "addgroup "+NWDServer.K_SFTP_chroot+"", // Add group exists",
                "useradd --shell /bin/false " + User + "",
                "echo " + User + ":" + Secure_Password.Decrypt() + " | chpasswd",
                "usermod -a -G "+NWDServer.K_SFTP_chroot+" " + User + "",
                "mkdir /home/" + User + "",
                "chown root /home/" + User + "",
                "chmod go-w  /home/" + User + "",

                "mkdir /home/" + User + "/ssl",
                "openssl req -x509 -sha256 -nodes -days 365 -newkey rsa:2048 -keyout /home/" + User + "/ssl/" + tServerDomain.ServerDNS + ".key -out /home/" + User + "/ssl/" + tServerDomain.ServerDNS + ".crt -subj \"/C=FR/ST=LILLE/L=LILLE/O=Global Security/OU=IT Department/CN=" + tServerDomain.ServerDNS + "\"",
                "chown -R " + User + ":www-data /home/" + User + "/ssl",
                "chmod -R 750 /home/" + User + "/ssl",

                "mkdir /home/" + User + "/" + Folder + _SSL+ "",
                "echo $\"<?php echo phpinfo();?>\" > /home/" + User + "/" + Folder + _SSL+ "/phpinfo.php",
                "echo $\"Hello " + tServerDomain.ServerDNS + ", you are secure!\" > /home/" + User + "/" + Folder + _SSL+ "/index.html",
                "chown -R " + User + ":www-data /home/" + User + "/" + Folder + _SSL+ "",
                "chmod -R 750 /home/" + User + "/" + Folder + _SSL+ "",




                "mkdir /home/" + User + "/" + Folder + _NoSSL,
                "echo $\"<?php echo phpinfo();?>\" > /home/" + User + "/" + Folder + _NoSSL+ "/phpinfo.php",
                "echo $\"Hello " + tServerDomain.ServerDNS + ", please use SSL connexion!\" > /home/" + User + "/" + Folder + _NoSSL+ "/index.html",
                "chown -R " + User + ":www-data /home/" + User + "/" + Folder + _NoSSL+ "",
                "chmod -R 750 /home/" + User + "/" + Folder + _NoSSL+ "",

                "/etc/init.d/apache2 stop",


                // create virtual host without SSL
                "rm /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "echo \"<VirtualHost *:80>\" > /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "sed -i '$ a Protocols h2 http/1.1' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "sed -i '$ a ServerAdmin " + Email + "' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "sed -i '$ a ServerName " + tServerDomain.ServerDNS + "' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "sed -i '$ a ServerAlias " + tServerDomain.ServerDNS + "' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "sed -i '$ a DocumentRoot /home/" + User + "/" + Folder + _NoSSL+ "' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "sed -i '$ a <Directory />' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "sed -i '$ a AllowOverride All' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "sed -i '$ a </Directory>' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "sed -i '$ a <Directory /home/" + User + "/" + Folder + _NoSSL + ">' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "sed -i '$ a Options Indexes FollowSymLinks MultiViews' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "sed -i '$ a AllowOverride all' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "sed -i '$ a Require all granted' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "sed -i '$ a </Directory>' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "sed -i '$ a LogLevel error' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "sed -i '$ a ErrorLog /var/log/apache2/" + tServerDomain.ServerDNS + "-error.log' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "sed -i '$ a CustomLog /var/log/apache2/" + tServerDomain.ServerDNS + "-ssl-access.log combined env=NoLog' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _NoSSL +"_ws.conf",
                "sed -i '$ a </VirtualHost>' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",
                "a2ensite " + tServerDomain.ServerDNS + _NoSSL+"_ws.conf",

                // create virtual host with SSL
                "rm /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _SSL+"_ws.conf",
                "echo \"<IfModule mod_ssl.c>\" > /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _SSL +"_ws.conf",
                "sed -i '$ a <VirtualHost *:443>' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _SSL +"_ws.conf",
                "sed -i '$ a Protocols h2 http/1.1' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a ServerAdmin " + Email + "' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a ServerName " + tServerDomain.ServerDNS + "' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a ServerAlias " + tServerDomain.ServerDNS+ "' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a DocumentRoot /home/" + User + "/" + Folder  + _SSL+ "' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a <Directory />' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a AllowOverride All' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a </Directory>' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a <Directory /home/" + User + "/" + Folder  + _SSL+ ">' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a Options Indexes FollowSymLinks MultiViews' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a AllowOverride all' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a Require all granted' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a </Directory>' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a LogLevel error' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a ErrorLog /var/log/apache2/" + tServerDomain.ServerDNS + "-error.log' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a CustomLog /var/log/apache2/" + tServerDomain.ServerDNS + "-ssl-access.log combined env=NoLog' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a SSLEngine On' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                //"sed -i '$ a SSLProtocol all -SSLv2 -SSLv3 -TLSv1 -TLSv1.1' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a SSLProtocol +TLSv1.2 +TLSv1.3' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a SSLCipherSuite ALL:!DH:!EXPORT:!RC4:+HIGH:+MEDIUM:!LOW:!aNULL:!eNULL' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a SSLCertificateFile /home/" + User + "/ssl/" + tServerDomain.ServerDNS + ".crt' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a SSLCertificateKeyFile /home/" + User + "/ssl/" + tServerDomain.ServerDNS + ".key' /etc/apache2/sites-available/" + tServerDomain.ServerDNS +  _SSL +"_ws.conf",
                "sed -i '$ a </VirtualHost>' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _SSL +"_ws.conf",
                "sed -i '$ a </IfModule>' /etc/apache2/sites-available/" + tServerDomain.ServerDNS + _SSL +"_ws.conf",
                "a2ensite " + tServerDomain.ServerDNS + _SSL +"_ws.conf",

                "rm /etc/apache2/sites-available/000-default-le-ssl.conf",
                "echo \"<IfModule mod_ssl.c>\" > /etc/apache2/sites-available/000-default-le-ssl.conf",
                "sed -i '$ a <VirtualHost *:443>' /etc/apache2/sites-available/000-default-le-ssl.conf",
                "sed -i '$ a ServerAdmin webmaster@localhost' /etc/apache2/sites-available/000-default-le-ssl.conf",
                "sed -i '$ a DocumentRoot /var/www/html' /etc/apache2/sites-available/000-default-le-ssl.conf",
                "sed -i '$ a ErrorLog ${APACHE_LOG_DIR}/error.log' /etc/apache2/sites-available/000-default-le-ssl.conf",
                "sed -i '$ a CustomLog ${APACHE_LOG_DIR}/access.log combined' /etc/apache2/sites-available/000-default-le-ssl.conf",
                "sed -i '$ a </VirtualHost>' /etc/apache2/sites-available/000-default-le-ssl.conf",
                "sed -i '$ a </IfModule>' /etc/apache2/sites-available/000-default-le-ssl.conf",
                //"a2ensite 000-default-le-ssl.conf",
                "a2dissite 000-default-le-ssl.conf", // must disable to activate correctly the ssl 

                "rm /etc/apache2/sites-available/000-default.conf",
                "echo \"<IfModule mod_ssl.c>\" > /etc/apache2/sites-available/000-default.conf",
                "sed -i '$ a <VirtualHost *:80>' /etc/apache2/sites-available/000-default.conf",
                "sed -i '$ a ServerAdmin webmaster@localhost' /etc/apache2/sites-available/000-default.conf",
                "sed -i '$ a DocumentRoot /var/www/html' /etc/apache2/sites-available/000-default.conf",
                "sed -i '$ a ErrorLog ${APACHE_LOG_DIR}/error.log' /etc/apache2/sites-available/000-default.conf",
                "sed -i '$ a CustomLog ${APACHE_LOG_DIR}/access.log combined' /etc/apache2/sites-available/000-default.conf",
                "sed -i '$ a </VirtualHost>' /etc/apache2/sites-available/000-default.conf",
                "sed -i '$ a </IfModule>' /etc/apache2/sites-available/000-default.conf",
                "a2ensite 000-default.conf",

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

                    /*


                    //-----------------
                    //EditorGUI.BeginDisabledGroup(UserInstalled == false);
                    tButtonTitle = new GUIContent("Install User webdav", "Install webdav");
                    if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                    {
                        if (tServerDomain != null)
                        {
                            string _NoSSL = "_NoSSL";
                            string _SSL = "";
                            tServer.ExecuteSSH(tButtonTitle.text, new List<string>()
                {

                "useradd --shell /bin/false " + User + "",
                "echo " + User + ":" + Secure_Password.Decrypt() + " | chpasswd",
                "mkdir /home/" + User + "",
                "chown root /home/" + User + "",
                "chmod go-w  /home/" + User + "",

                        "systemctl stop apache2",
                        "apt-get -y install apache2-utils",
                        "systemctl start apache2",
                        "a2enmod dav* ",
                        "systemctl restart apache2",

                        "mkdir /home/webdav",
                        "chown www-data. /home/webdav",
                        "chmod 770 /home/webdav",

                        "mkdir /home/webdav/user",
                        "chown www-data. /home/webdav/user",
                        "chmod 770 /home/webdav/user",
                        "mkdir /home/webdav/psswd",
                        "chown www-data. /home/webdav/psswd",
                        "chmod 770 /home/webdav/psswd",

                        "mkdir /home/webdav/user/" + User + "",
                        "chown www-data. /home/webdav/user/" + User + "",
                        "chmod 770 /home/webdav/user/" + User + "",
                        "mkdir /home/webdav/psswd/" + User + "",
                        "chown www-data. /home/webdav/psswd/" + User + "",
                        "chmod 770 /home/webdav/psswd/" + User + "",


                        // create virtual host without SSL
                        
                "a2dissite " + User + "-webdav.conf",
                "systemctl restart apache2",

                "rm /etc/apache2/sites-available/" + User + "-webdav.conf",
                "rm /home/webdav/" + User + "/.htpasswd",
                "rm /home/webdav/psswd/" + User + "/.htpasswd",


                "ls /home/webdav/user/" + User + "",

                "echo \"Alias /" + User + " /home/webdav/user/" + User + "\" > /etc/apache2/sites-available/" + User + "-webdav.conf",
                "sed -i '$ a <Location /" + User + ">' /etc/apache2/sites-available/" + User + "-webdav.conf",
                "sed -i '$ a DAV On' /etc/apache2/sites-available/" + User + "-webdav.conf",
                "sed -i '$ a SSLRequireSSL' /etc/apache2/sites-available/" + User + "-webdav.conf",
                "sed -i '$ a Options None' /etc/apache2/sites-available/" + User + "-webdav.conf",
                "sed -i '$ a AuthType Basic' /etc/apache2/sites-available/" + User + "-webdav.conf",
                "sed -i '$ a AuthName WebDAV' /etc/apache2/sites-available/" + User + "-webdav.conf",
                "sed -i '$ a AuthUserFile /home/webdav/psswd/" + User + "/.htpasswd' /etc/apache2/sites-available/" + User + "-webdav.conf",
                "sed -i '$ a <RequireAny>' /etc/apache2/sites-available/" + User + "-webdav.conf",
                "sed -i '$ a Require method GET POST OPTIONS' /etc/apache2/sites-available/" + User + "-webdav.conf",
                "sed -i '$ a Require valid-user' /etc/apache2/sites-available/" + User + "-webdav.conf",
                "sed -i '$ a </RequireAny>' /etc/apache2/sites-available/" + User + "-webdav.conf",
                "sed -i '$ a </Location>' /etc/apache2/sites-available/" + User + "-webdav.conf",

                "echo \" \" > /home/webdav/psswd/" + User + "/.htpasswd",
                "htpasswd -b /home/webdav/psswd/" + User + "/.htpasswd " + User + " " + Secure_Password.Decrypt() + "",

                "a2ensite " + User + "-webdav.conf",

                "systemctl restart apache2",

                "certbot --agree-tos -n --no-eff-email --apache --redirect --email " + Email + " -d " + tServer.IP.ToString() + "",


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
                    //EditorGUI.EndDisabledGroup();
                    tI++;
                    //-----------------




                    //-----------------
                    //EditorGUI.BeginDisabledGroup(UserInstalled == false);
                    tButtonTitle = new GUIContent("ls User webdav", "ls webdav");
                    if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                    {
                        if (tServerDomain != null)
                        {
                            tServer.ExecuteSSH(tButtonTitle.text, new List<string>()
                            {
                                "ls /home/webdav/user/" + User + "",
                             }, delegate (string sCommand, string sResult)
                                {
                                });
                         }
                    }
                    //EditorGUI.EndDisabledGroup();
                    tI++;
                    //-----------------




                    */





                    tButtonTitle = new GUIContent("Check apache", "check apache ");
                    if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                    {
                        tServer.ExecuteSSH(tButtonTitle.text, new List<string>()
                {
                    "netstat -tanpu | grep \"LISTEN\" | grep \"443\"",
                    "apache2ctl -S",
                    //"httpd -S",
                    "a2query -m",
                    //"tail -100 /var/log/apache2/access.log"
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
                }, null, tServer.Port, User, Secure_Password.Decrypt());
                    }
                    tI++;

                    //-----------------
                    tButtonTitle = new GUIContent("default html", " try connexion to index.html");
                    if (GUI.Button(tMatrix[0, tI], tButtonTitle))
                    {
                        Application.OpenURL("http://" + tServer.IP.ToString() + "/index.html");
                    }
                    tButtonTitle = new GUIContent("default phpinfo", " try connexion to index.html with ssl");
                    if (GUI.Button(tMatrix[1, tI], tButtonTitle))
                    {
                        Application.OpenURL("http://" + tServer.IP.ToString() + "/phpinfo.php");
                    }
                    tI++;

                    if (tServerDomain != null)
                    {
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
                    }
                    //-----------------
                    NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                    tI++;

                    if (tServerDomain != null && UserInstalled == true /*&& SSLInstalled == false*/)
                    {
                        //-----------------
                        string tCerbot = "certbot --agree-tos -n --no-eff-email --apache --redirect --email " + Email + " -d " + tServerDomain.ServerDNS + "";
                        EditorGUI.TextField(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Try cerbot", tCerbot);
                        tI++;
                        //-----------------
                        tButtonTitle = new GUIContent("Try certbot SSL", " try connexion to generate certbot ssl (lest's encrypt)");
                        if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                        {
                            tServer.ExecuteSSH(tButtonTitle.text, new List<string>()
                        {
                        tCerbot,
                        });
                        }
                        tI++;
                        //-----------------
                    }
                    //-----------------
                    tButtonTitle = new GUIContent("Test security of SSL", "test security for this SSL config");
                    if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                    {
                        Application.OpenURL("https://www.ssllabs.com/ssltest/analyze.html?d=" + tServerDomain.ServerDNS + "");
                    }
                    tI++;
                    //-----------------
                    NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                    tI++;

                    //-----------------
                    string tURLAdmin = "sftp://" + tServer.Admin_User + ":" + tServer.Admin_Secure_Password.Decrypt() + "@" + tServer.IP.GetValue() + ":" + tServer.Port + "/";
                    tButtonTitle = new GUIContent("Try sftp ADMIN directly", tURLAdmin);
                    if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                    {
                        //NWEClipboard.CopyToClipboard(Password.GetValue());
                        Application.OpenURL(tURLAdmin);
                    }
                    tI++;
                    //-----------------
                    string tURL = "sftp://" + User + ":" + Secure_Password.Decrypt() + "@" + tServer.IP.GetValue() + ":" + tServer.Port + "/" + Folder;
                    tButtonTitle = new GUIContent("Try sftp directly", tURL);
                    if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                    {
                        //NWEClipboard.CopyToClipboard(Password.GetValue());
                        Application.OpenURL(tURL);
                    }
                    tI++;
                    //-----------------
                }
            }
            else
            {
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Need credentials for actions"))
                {
                    NWDProjectCredentialsManager.SharedInstanceFocus();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
