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
using UnityEngine;
using System.IO;
using System.Net;
using System.Linq;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
using Renci.SshNet;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServerOther : NWDServer
    {
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight(float sWidth)
        {
            return base.AddonEditorHeight(sWidth);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEditor(Rect sRect)
        {
            //-----------------
            GUIContent tButtonTitle = null;
            //-----------------
            base.AddonEditor(sRect);
            NWDGUILayout.Separator();
            if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerate))
            {
                //-----------------
                tButtonTitle = new GUIContent("Install root on OVh", "Install root on OVH");
                if (GUILayout.Button(tButtonTitle))
                {
                    string tNewPassword = NWDToolbox.RandomStringCypher(24);
                    Debug.Log("tNewPassword : " + tNewPassword);
                    Execute(this, tButtonTitle.text,
                        new List<string>()
                            {
                                "export PATH=\"/usr/local/sbin:/usr/local/bin:/sbin:/bin:/usr/sbin:/usr/bin:/root/bin\"",
                                "sudo -i & echo '" + Root_User + ":" + tNewPassword + "' | chpasswd & exit", // change the password for the Admin
                            },
                         delegate (string sCommand, string sResult)
                           {
                               Debug.Log("tNewPassword : " + tNewPassword + " changed!");
                               Root_Secure_Password.CryptAes(tNewPassword);
                               UpdateDataIfModified();
                           },
                         Port, Admin_User, Admin_Secure_Password.Decrypt());
                }



                //-----------------
                tButtonTitle = new GUIContent("Install Apache PHP", "Install Apache and PHP 7");
                if (GUILayout.Button(tButtonTitle))
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
                    if (Distribution == NWDServerDistribution.debian10)
                    {
                        tCommandList.Add("a2enmod http2");
                    }

                    tCommandList.Add("echo \"<color=red> -> apache configure</color>\"");
                    tCommandList.Add("sed -i 's/\\/var\\/www/\\/home/g' /etc/apache2/suexec/www-data");
                    tCommandList.Add("sed -i 's/^.*ServerSignature .*$//g' /etc/apache2/apache2.conf");
                    tCommandList.Add("sed -i '$ a ServerSignature Off' /etc/apache2/apache2.conf");

                    tCommandList.Add("sed -i 's/^.*SSLProtocol .*$//g' /etc/apache2/apache2.conf");
                    tCommandList.Add("sed -i '$ a SSLProtocol all -SSLv2 -SSLv3 -TLSv1 -TLSv1.1' /etc/apache2/apache2.conf");

                    tCommandList.Add("echo \"<color=red> -> apache restart</color>\"");
                    tCommandList.Add("systemctl restart apache2");

                    if (Distribution == NWDServerDistribution.debian9)
                    {
                        tCommandList.Add("echo \"<color=red> -> install php</color>\"");
                        tCommandList.Add("apt-get -y install php");
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
                        tCommandList.Add("apt-get -y install php-gd");
                        tCommandList.Add("apt-get -y install libapache2-mod-php");
                        tCommandList.Add("pear install Net_SMTP");
                    }
                    if (Distribution == NWDServerDistribution.debian10)
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
                        tCommandList.Add("apt-get -y install php-geoip");
                        tCommandList.Add("apt-get -y install php-gd");
                        tCommandList.Add("pear install Net_SMTP");
                    }
                    tCommandList.Add("systemctl restart apache2");
                    tCommandList.Add("echo \"<color=red> -> php folder default</color>\"");
                    tCommandList.Add("chgrp -R www-data /var/www/html/");
                    tCommandList.Add("chmod 750 /var/www/html/");

                    tCommandList.Add("echo \"<color=red> -> files default</color>\"");
                    tCommandList.Add("echo $\"<?php echo phpinfo();?>\" > /var/www/html/phpinfo.php");
                    tCommandList.Add("echo $\"Are you lost? Ok, I'll help you, you're in front of a screen!\" > /var/www/html/index.html");

                    if (Distribution == NWDServerDistribution.debian9)
                    {
                        tCommandList.Add("echo \"<color=red> -> install Let's Encrypt Certbot</color>\"");
                        tCommandList.Add("echo $\"deb http://ftp.debian.org/debian stretch-backports main\" >> /etc/apt/sources.list.d/backports.list");
                        tCommandList.Add("apt-get update");
                        tCommandList.Add("apt-get -y install python-certbot-apache -t stretch-backports");
                    }
                    if (Distribution == NWDServerDistribution.debian10)
                    {
                        tCommandList.Add("apt-get -y install certbot python-certbot-apache");
                    }

                    tCommandList.Add("echo \"<color=red> -> apache restart</color>\"");
                    tCommandList.Add("systemctl restart apache2");

                    ExecuteSSH(tButtonTitle.text, tCommandList);
                }





                EditorGUI.BeginDisabledGroup(WebDomainNameUserInstalled == true);
                tButtonTitle = new GUIContent("Install User", "Install User");
                if (GUILayout.Button(tButtonTitle))
                {
                    //if (tServerDomain != null)
                    {
                        //string _NoSSL = "_NoSSL";
                        string _NoSSL = "No_SSL";
                        string _SSL = "";
                        ExecuteSSH(tButtonTitle.text, new List<string>()
                {

                "addgroup "+NWDServer.K_SFTP_chroot+"", // Add group exists",
                "useradd --shell /bin/false " + WebDomainNameUser + "",
                "echo " + WebDomainNameUser + ":" + WebDomainNameSecure_Password.Decrypt() + " | chpasswd",
                "usermod -a -G "+NWDServer.K_SFTP_chroot+" " + WebDomainNameUser + "",
                "usermod -a -G www-data " + WebDomainNameUser + "",
                "mkdir /home/" + WebDomainNameUser + "",
                "chown root /home/" + WebDomainNameUser + "",
                "chmod go-w  /home/" + WebDomainNameUser + "",

                "mkdir /home/" + WebDomainNameUser + "/ssl",
                "openssl req -x509 -sha256 -nodes -days 365 -newkey rsa:2048 -keyout /home/" + WebDomainNameUser + "/ssl/" + WebDomainNameServer + ".key -out /home/" + WebDomainNameUser + "/ssl/" + WebDomainNameServer + ".crt -subj \"/C=FR/ST=LILLE/L=LILLE/O=Global Security/OU=IT Department/CN=" + WebDomainNameServer + "\"",
                "chown -R " + WebDomainNameUser + ":www-data /home/" + WebDomainNameUser + "/ssl",
                "chmod -R 750 /home/" + WebDomainNameUser + "/ssl",

                "mkdir /home/" + WebDomainNameUser + "/" + WebDomainNameFolder ,
                "echo $\"<?php echo phpinfo();?>\" > /home/" + WebDomainNameUser + "/" + WebDomainNameFolder + "/phpinfo.php",
                "echo $\"Hello " + WebDomainNameServer + ", please use SSL connexion!\" > /home/" + WebDomainNameUser + "/" + WebDomainNameFolder + "/index.html",
                "chown -R " + WebDomainNameUser + ":www-data /home/" + WebDomainNameUser + "/" + WebDomainNameFolder + "",
                "chmod -R 750 /home/" + WebDomainNameUser + "/" + WebDomainNameFolder + "",

                "/etc/init.d/apache2 stop",


                // create virtual host without SSL
                "rm /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "echo \"<VirtualHost *:80>\" > /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "sed -i '$ a Protocols h2 http/1.1' /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "sed -i '$ a ServerAdmin " + WebDomainNameEmail + "' /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "sed -i '$ a ServerName " + WebDomainNameServer + "' /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "sed -i '$ a ServerAlias www." + WebDomainNameServer + "' /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "sed -i '$ a DocumentRoot /home/" + WebDomainNameUser + "/" + WebDomainNameFolder+ "' /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "sed -i '$ a <Directory />' /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "sed -i '$ a AllowOverride All' /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "sed -i '$ a </Directory>' /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "sed -i '$ a <Directory /home/" + WebDomainNameUser + "/" + WebDomainNameFolder + ">' /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "sed -i '$ a Options Indexes FollowSymLinks MultiViews' /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "sed -i '$ a AllowOverride all' /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "sed -i '$ a Require all granted' /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "sed -i '$ a </Directory>' /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "sed -i '$ a LogLevel error' /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "sed -i '$ a ErrorLog /var/log/apache2/" + WebDomainNameServer + "-error.log' /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "sed -i '$ a CustomLog /var/log/apache2/" + WebDomainNameServer + "-ssl-access.log combined env=NoLog' /etc/apache2/sites-available/" + WebDomainNameServer +  _NoSSL +"_ws.conf",
                "sed -i '$ a </VirtualHost>' /etc/apache2/sites-available/" + WebDomainNameServer + _NoSSL+"_ws.conf",
                "a2ensite " + WebDomainNameServer + _NoSSL+"_ws.conf",

                // create virtual host with SSL
                "rm /etc/apache2/sites-available/" + WebDomainNameServer + _SSL+"_ws.conf",
                "echo \"<IfModule mod_ssl.c>\" > /etc/apache2/sites-available/" + WebDomainNameServer + _SSL +"_ws.conf",
                "sed -i '$ a <VirtualHost *:443>' /etc/apache2/sites-available/" + WebDomainNameServer + _SSL +"_ws.conf",
                "sed -i '$ a Protocols h2 http/1.1' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a ServerAdmin " + WebDomainNameEmail + "' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a ServerName " + WebDomainNameServer + "' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a ServerAlias www." + WebDomainNameServer+ "' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a DocumentRoot /home/" + WebDomainNameUser + "/" + WebDomainNameFolder + "' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a <Directory />' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a AllowOverride All' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a </Directory>' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a <Directory /home/" + WebDomainNameUser + "/" + WebDomainNameFolder+ ">' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a Options Indexes FollowSymLinks MultiViews' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a AllowOverride all' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a Require all granted' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a </Directory>' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a LogLevel error' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a ErrorLog /var/log/apache2/" + WebDomainNameServer + "-error.log' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a CustomLog /var/log/apache2/" + WebDomainNameServer + "-ssl-access.log combined env=NoLog' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a SSLEngine On' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                //"sed -i '$ a SSLProtocol all -SSLv2 -SSLv3 -TLSv1 -TLSv1.1' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a SSLProtocol +TLSv1.2 +TLSv1.3' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a SSLCipherSuite ALL:!DH:!EXPORT:!RC4:+HIGH:+MEDIUM:!LOW:!aNULL:!eNULL' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a SSLCertificateFile /home/" + WebDomainNameUser + "/ssl/" + WebDomainNameServer + ".crt' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a SSLCertificateKeyFile /home/" + WebDomainNameUser + "/ssl/" + WebDomainNameServer + ".key' /etc/apache2/sites-available/" + WebDomainNameServer +  _SSL +"_ws.conf",
                "sed -i '$ a </VirtualHost>' /etc/apache2/sites-available/" + WebDomainNameServer + _SSL +"_ws.conf",
                "sed -i '$ a </IfModule>' /etc/apache2/sites-available/" + WebDomainNameServer + _SSL +"_ws.conf",
                "a2ensite " + WebDomainNameServer + _SSL +"_ws.conf",

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
                                   WebDomainNameUserInstalled = true;
                                   UpdateDataIfModified();
                               };
                           });
                    }
                }
                EditorGUI.EndDisabledGroup();

                //-----------------
                if (WebDomainNameServer != null && WebDomainNameUserInstalled == true /*&& SSLInstalled == false*/)
                {
                    //-----------------
                    string tCerbot = "certbot --agree-tos -n --no-eff-email --apache --redirect --email " + WebDomainNameEmail + " -d " + WebDomainNameServer + "";
                    string tCerbotWWW = "certbot --agree-tos -n --no-eff-email --apache --redirect --email " + WebDomainNameEmail + " -d www." + WebDomainNameServer + "";
                    EditorGUILayout.TextField("Try cerbot", tCerbot);
                    EditorGUILayout.TextField("Try cerbot www", tCerbotWWW);
                    //-----------------
                    tButtonTitle = new GUIContent("Try certbot SSL", " try connexion to generate certbot ssl (lest's encrypt)");
                    if (GUILayout.Button(tButtonTitle))
                    {
                        ExecuteSSH(tButtonTitle.text, new List<string>()
                        {
                        tCerbot,tCerbotWWW
                        });
                    }
                    //-----------------
                }
                //-----------------
                string tURL = "sftp://" + WebDomainNameUser + ":" + WebDomainNameSecure_Password.Decrypt() + "@" + IP.GetValue() + ":" + Port + "/" + WebDomainNameFolder;
                tButtonTitle = new GUIContent("Try sftp directly", tURL);
                if (GUILayout.Button(tButtonTitle))
                {
                    Application.OpenURL(tURL);
                }

                //-----------------
                tButtonTitle = new GUIContent("install MariaDB", " try install MariaDB (fork of MySQL)");
                if (GUILayout.Button(tButtonTitle))
                {
                    List<string> tCommandList = new List<string>();

                    tCommandList.Add("echo \"<color=red> -> server update</color>\"");
                    tCommandList.Add("apt-get update");
                    tCommandList.Add("apt-get -y upgrade");
                    tCommandList.Add("apt-get -y dist-upgrade");

                    tCommandList.Add("/etc/init.d/mysql stop");

                    tCommandList.Add("echo \"<color=red> -> mysql install (MariaDB)</color>\"");
                    tCommandList.Add("debconf-set-selections <<< \"mariadb-server mysql-server/root_password password " + Admin_Secure_Password.Decrypt() + "\"");
                    tCommandList.Add("debconf-set-selections <<< \"mariadb-server mysql-server/root_password_again password " + Admin_Secure_Password.Decrypt() + "\"");
                    tCommandList.Add("apt-get -y install mariadb-server");
                    tCommandList.Add("echo PURGE | debconf-communicate mariadb-server");
                    tCommandList.Add("echo \"<color=red> -> mysql start (MariaDB)</color>\"");

                    if (Distribution == NWDServerDistribution.debian9)
                    {
                        //if (External == true)
                        //{
                        //tCommandList.Add("sed -i 's/^.*bind\\-address.*$/bind\\-address = 0.0.0.0/g' /etc/mysql/mariadb.conf.d/50-server.cnf");
                        //}
                        //else
                        //{
                        tCommandList.Add("sed -i 's/^.*bind-address .*$/bind-address = 127.0.0.1/g' /etc/mysql/mariadb.conf.d/50-server.cnf");
                        //}
                    }
                    if (Distribution == NWDServerDistribution.debian10)
                    {
                        //if (External == true)
                        //{
                        //    tCommandList.Add("sed -i 's/^.*bind\\-address.*$/#bind\\-address = 0.0.0.0/g' /etc/mysql/mariadb.conf.d/50-server.cnf");
                        //}
                        //else
                        //{
                        tCommandList.Add("sed -i 's/^.*bind-address .*$/bind-address = 127.0.0.1/g' /etc/mysql/mariadb.conf.d/50-server.cnf");
                        //}
                    }

                    tCommandList.Add("/etc/init.d/mysql start");
                    ExecuteSSH(tButtonTitle.text, tCommandList);
                }

                //-----------------
                tButtonTitle = new GUIContent("Install User in maria db", " try install User In MariaDB");
                if (GUILayout.Button(tButtonTitle))
                {
                    List<string> tCommandList = new List<string>();
                    tCommandList.Add("echo \"<color=red> -> add user in mysql</color>\"");
                    tCommandList.Add("echo \" -> add user in mysql\"");
                    tCommandList.Add("mysql -u root -p\"" + Admin_Secure_Password.Decrypt() + "\" -e \"CREATE DATABASE IF NOT EXISTS " + WebDomainNameUser + ";\"");
                    tCommandList.Add("mysql -u root -p\"" + Admin_Secure_Password.Decrypt() + "\" -e \"GRANT ALL PRIVILEGES ON " + WebDomainNameUser + ".* TO '" + WebDomainNameUser + "'@'localhost' IDENTIFIED BY '" + WebDomainNameSecure_Password.Decrypt() + "';\"");
                    //if (External == true)
                    //{
                    //    tCommandList.Add("mysql -u root -p\"" + Admin_Secure_Password.Decrypt() + "\" -e \"GRANT ALL PRIVILEGES ON " + WebDomainNameUser + ".* TO '" + WebDomainNameUser + "'@'%' IDENTIFIED BY '" + WebDomainNameSecure_Password.Decrypt() + "';\"");
                    //}
                    //else
                    //{
                    tCommandList.Add("mysql -u root -p\"" + Admin_Secure_Password.Decrypt() + "\" -e \"REVOKE ALL PRIVILEGES ON " + WebDomainNameUser + ".* FROM '" + WebDomainNameUser + "'@'%';\"");
                    //}
                    ExecuteSSH(tButtonTitle.text, tCommandList);
                }
                //-----------------
                tButtonTitle = new GUIContent("Install wordpress", " try install wordpress");
                if (GUILayout.Button(tButtonTitle))
                {
                    List<string> tCommandList = new List<string>();
                    tCommandList.Add("echo \"<color=red> -> add user in mysql</color>\"");
                    tCommandList.Add("echo \" -> add wordpress\"");
                    tCommandList.Add("cd /home/" + WebDomainNameUser + "/" + WebDomainNameFolder + "/");
                    tCommandList.Add("ls");
                    tCommandList.Add("wget https://wordpress.org/latest.tar.gz");
                    tCommandList.Add("tar -xvf latest.tar.gz");
                    tCommandList.Add("rm latest.tar.gz");
                    tCommandList.Add("mv /home/" + WebDomainNameUser + "/" + WebDomainNameFolder + "/wordpress/* /home/" + WebDomainNameUser + "/" + WebDomainNameFolder + "/");
                    tCommandList.Add("rm -R wordpress");
                    tCommandList.Add("rm wordpress");
                    tCommandList.Add("rm index.html");
                    //tCommandList.Add("chown -R " + WebDomainNameUser + ":www-data /home/" + WebDomainNameUser + "/" + WebDomainNameFolder + "/");
                    tCommandList.Add("chown -R www-data:www-data /home/" + WebDomainNameUser + "/" + WebDomainNameFolder + "/");
                    tCommandList.Add("ls");
                    string tConfigPHP = "/home/" + WebDomainNameUser + "/" + WebDomainNameFolder + "/wp-config.php";
                    tCommandList.Add("rm " + tConfigPHP);

                    tCommandList.Add("echo \"<?php\" > " + tConfigPHP);
                    tCommandList.Add("echo \"define('FS_METHOD', 'direct');\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"define('DB_NAME', '" + WebDomainNameUser + "');\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"define('DB_USER', '" + WebDomainNameUser + "');\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"define('DB_PASSWORD', '" + WebDomainNameSecure_Password.Decrypt() + "');\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"define('DB_HOST', 'localhost');\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"define('DB_CHARSET', 'utf8mb4');\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"define('DB_COLLATE', '');\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"define('AUTH_KEY', '" + NWDToolbox.RandomString(32) + "');\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"define('SECURE_AUTH_KEY', '" + NWDToolbox.RandomString(32) + "');\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"define('LOGGED_IN_KEY', '" + NWDToolbox.RandomString(32) + "');\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"define('NONCE_KEY', '" + NWDToolbox.RandomString(32) + "');\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"define('AUTH_SALT', '" + NWDToolbox.RandomString(32) + "');\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"define('SECURE_AUTH_SALT', '" + NWDToolbox.RandomString(32) + "');\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"define('LOGGED_IN_SALT', '" + NWDToolbox.RandomString(32) + "');\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"define('NONCE_SALT', '" + NWDToolbox.RandomString(32) + "');\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"\\$table_prefix = 'wp_';\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"define('WP_DEBUG', false);\" >> " + tConfigPHP);
                    //tCommandList.Add("echo \"if (!defined('ABSPATH')) { define('ABSPATH', __DIR__. '/'); }\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"define('ABSPATH', __DIR__. '/');\" >> " + tConfigPHP);
                    tCommandList.Add("echo \"require_once ABSPATH . 'wp-settings.php';\" >> " + tConfigPHP);

                    tCommandList.Add("chown -R www-data:www-data /home/" + WebDomainNameUser + "/" + WebDomainNameFolder + "/");
                    tCommandList.Add("usermod -a -G www-data " + WebDomainNameUser + "");
                    tCommandList.Add("chmod -R 775 /home/" + WebDomainNameUser + "/" + WebDomainNameFolder + "/");
                    tCommandList.Add("rm /home/" + WebDomainNameUser + "/" + WebDomainNameFolder + "/index.html");

                    ExecuteSSH(tButtonTitle.text, tCommandList);
                }

                //-----------------
                tButtonTitle = new GUIContent("Rights on wordpress", " try install right for wordpress");
                if (GUILayout.Button(tButtonTitle))
                {
                    List<string> tCommandList = new List<string>();
                    tCommandList.Add("chown -R www-data:www-data /home/" + WebDomainNameUser + "/" + WebDomainNameFolder + "/");
                    tCommandList.Add("chmod -R 775 /home/" + WebDomainNameUser + "/" + WebDomainNameFolder + "/");
                    tCommandList.Add("usermod -a -G www-data " + WebDomainNameUser + "");
                    ExecuteSSH(tButtonTitle.text, tCommandList);
                }
                // sudo - i & echo root: pa1452sesd | chpasswd

                /*
                    if (GUI.Button(NWDGUI.AssemblyArea(EditorMatrix[0, EditorMatrixIndex], EditorMatrix[1, EditorMatrixIndex]), "Credentials window"))
                    {
                        NWDProjectCredentialsManager.SharedInstanceFocus();
                    }
                    EditorMatrixIndex++;
                    if (GUI.Button(NWDGUI.AssemblyArea(EditorMatrix[0, EditorMatrixIndex], EditorMatrix[1, EditorMatrixIndex]), "Flush credentials"))
                    {
                        NWDProjectCredentialsManagerContent.FlushCredentials(NWDCredentialsRequired.ForSFTPGenerate);
                    }
                    EditorMatrixIndex++;
                    NWDGUI.Separator(NWDGUI.AssemblyArea(EditorMatrix[0, EditorMatrixIndex], EditorMatrix[1, EditorMatrixIndex]));
                    EditorMatrixIndex++;
                    GUIContent tButtonTitle = null;
                    NWDServer tServer = this;//  Server.GetRawData();
                    if (tServer != null)
                    {
                        //-----------------
                        EditorGUI.HelpBox(NWDGUI.AssemblyArea(EditorMatrix[0, EditorMatrixIndex], EditorMatrix[1, EditorMatrixIndex + 1]), "Don't forgot to check your ~/.ssh/known_hosts file permission!", MessageType.Warning);
                        EditorMatrixIndex += 2;

                        string tcommandKeyGen = "ssh-keygen -R " + tServer.IP.GetValue() + " & ssh-keygen -R " + tServer.IP.GetValue() + ":" + tServer.Port + " & ssh " + tServer.IP.GetValue() + " -l " + tServer.Root_User + " -p " + tServer.Port;
                        if (tServer.AdminInstalled)
                        {
                            tcommandKeyGen = "ssh-keygen -R " + tServer.IP.GetValue() + " & ssh -keygen -R " + tServer.IP.GetValue() + ":" + tServer.Port + " & ssh " + tServer.IP.GetValue() + " -l " + tServer.Admin_User + " -p " + tServer.Port;
                        }


                        tButtonTitle = new GUIContent("local ssh-keygen -R", tcommandKeyGen);
                        if (GUI.Button(NWDGUI.AssemblyArea(EditorMatrix[0, EditorMatrixIndex], EditorMatrix[1, EditorMatrixIndex]), tButtonTitle))
                        {
                            NWDSSHWindow.ExecuteProcessTerminal(tcommandKeyGen);
                        }
                        EditorMatrixIndex++;
                        GUI.TextField(NWDGUI.AssemblyArea(EditorMatrix[0, EditorMatrixIndex], EditorMatrix[1, EditorMatrixIndex + 1]), tcommandKeyGen);
                        EditorMatrixIndex += 2;
                        NWDGUI.Separator(NWDGUI.AssemblyArea(EditorMatrix[0, EditorMatrixIndex], EditorMatrix[1, EditorMatrixIndex]));
                        EditorMatrixIndex++;


                        //-----------------
                        tButtonTitle = new GUIContent("Try connexion", " try connexion with root or admin");
                        if (GUI.Button(NWDGUI.AssemblyArea(EditorMatrix[0, EditorMatrixIndex], EditorMatrix[1, EditorMatrixIndex]), tButtonTitle))
                        {
                            tServer.ExecuteSSH(tButtonTitle.text, new List<string>()
                    {
                        "ls",
                    });
                        }
                        EditorMatrixIndex++;





                        //-----------------
                        if (ServerType == NWDServerOtherType.ActionServer)
                        {
                            tButtonTitle = new GUIContent("Run Action", "run action in server");
                            if (GUI.Button(NWDGUI.AssemblyArea(EditorMatrix[0, EditorMatrixIndex], EditorMatrix[1, EditorMatrixIndex]), tButtonTitle))
                            {
                                tServer.ExecuteSSH(tButtonTitle.text, ActionServer.GetStringList(), null);
                            }
                        }
                        EditorMatrixIndex++;


                        //-----------------
                        if (ServerType == NWDServerOtherType.WebDAV)
                        {
                            string tServerDNS = tServer.DomainNameServer;


                            //-----------------
                            //EditorGUI.BeginDisabledGroup(UserInstalled == false);
                            tButtonTitle = new GUIContent("Install User webdav", "Install webdav");
                            if (GUI.Button(NWDGUI.AssemblyArea(EditorMatrix[0, EditorMatrixIndex], EditorMatrix[1, EditorMatrixIndex]), tButtonTitle))
                            {
                                if (string.IsNullOrEmpty(tServerDNS) == false)
                                {
                                    tServer.ExecuteSSH(tButtonTitle.text, new List<string>()
                    {

                    "useradd --shell /bin/false " + WebDAV_User + "",
                    "echo " + WebDAV_User + ":" + WebDAV_Password.Decrypt() + " | chpasswd",
                    "mkdir /home/" + WebDAV_User + "",
                    "chown root /home/" + WebDAV_User + "",
                    "chmod go-w  /home/" + WebDAV_User + "",

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

                            "mkdir /home/webdav/user/" + WebDAV_User + "",
                            "chown www-data. /home/webdav/user/" + WebDAV_User + "",
                            "chmod 770 /home/webdav/user/" + WebDAV_User + "",
                            "mkdir /home/webdav/psswd/" + WebDAV_User + "",
                            "chown www-data. /home/webdav/psswd/" + WebDAV_User + "",
                            "chmod 770 /home/webdav/psswd/" + WebDAV_User + "",


                            // create virtual host without SSL

                    "a2dissite " + WebDAV_User + "-webdav.conf",
                    "systemctl restart apache2",

                    "rm /etc/apache2/sites-available/" + WebDAV_User + "-webdav.conf",
                    "rm /home/webdav/" + WebDAV_User + "/.htpasswd",
                    "rm /home/webdav/psswd/" + WebDAV_User + "/.htpasswd",


                    "ls /home/webdav/user/" + WebDAV_User + "",

                    "echo \"Alias /" + NWDToolbox.UnixCleaner(WebDAV_Access) + " /home/webdav/user/" + WebDAV_User + "\" > /etc/apache2/sites-available/" + WebDAV_User + "-webdav.conf",
                    "sed -i '$ a <Location /" + NWDToolbox.UnixCleaner(WebDAV_Access) + ">' /etc/apache2/sites-available/" + WebDAV_User + "-webdav.conf",
                    "sed -i '$ a DAV On' /etc/apache2/sites-available/" + WebDAV_User + "-webdav.conf",
                    "sed -i '$ a SSLRequireSSL' /etc/apache2/sites-available/" + WebDAV_User + "-webdav.conf",
                    "sed -i '$ a Options None' /etc/apache2/sites-available/" + WebDAV_User + "-webdav.conf",
                    "sed -i '$ a AuthType Basic' /etc/apache2/sites-available/" + WebDAV_User + "-webdav.conf",
                    "sed -i '$ a AuthName WebDAV' /etc/apache2/sites-available/" + WebDAV_User + "-webdav.conf",
                    "sed -i '$ a AuthUserFile /home/webdav/psswd/" + WebDAV_User + "/.htpasswd' /etc/apache2/sites-available/" + WebDAV_User + "-webdav.conf",
                    "sed -i '$ a <RequireAny>' /etc/apache2/sites-available/" + WebDAV_User + "-webdav.conf",
                    "sed -i '$ a Require method GET POST OPTIONS' /etc/apache2/sites-available/" + WebDAV_User + "-webdav.conf",
                    "sed -i '$ a Require valid-user' /etc/apache2/sites-available/" + WebDAV_User + "-webdav.conf",
                    "sed -i '$ a </RequireAny>' /etc/apache2/sites-available/" + WebDAV_User + "-webdav.conf",
                    "sed -i '$ a </Location>' /etc/apache2/sites-available/" + WebDAV_User + "-webdav.conf",

                    "echo \" \" > /home/webdav/psswd/" + WebDAV_User + "/.htpasswd",
                    "htpasswd -b /home/webdav/psswd/" + WebDAV_User + "/.htpasswd " + WebDAV_User + " " + WebDAV_Password.Decrypt() + "",

                    "a2ensite " + WebDAV_User + "-webdav.conf",

                    "systemctl restart apache2",

                   // "certbot --agree-tos -n --no-eff-email --apache --redirect --email " + "nocontact@no.com" + " -d " + tServer.IP.ToString() + "",


                },
                                       delegate (string sCommand, string sResult)
                                       {
                                       });
                                }
                            }
                            //EditorGUI.EndDisabledGroup();
                            EditorMatrixIndex++;
                            //-----------------




                            //-----------------
                            //EditorGUI.BeginDisabledGroup(UserInstalled == false);
                            tButtonTitle = new GUIContent("ls User webdav", "ls webdav");
                            if (GUI.Button(NWDGUI.AssemblyArea(EditorMatrix[0, EditorMatrixIndex], EditorMatrix[1, EditorMatrixIndex]), tButtonTitle))
                            {
                                if (string.IsNullOrEmpty(tServerDNS) == false)
                                {
                                    tServer.ExecuteSSH(tButtonTitle.text, new List<string>()
                                {
                                    "ls /home/webdav/user/" + WebDAV_User + "",
                                 }, delegate (string sCommand, string sResult)
                                 {
                                 });
                                }
                            }
                            //EditorGUI.EndDisabledGroup();
                            EditorMatrixIndex++;
                            //-----------------

                        }

                        //-----------------

                        if (ServerType == NWDServerOtherType.GitLab)
                        {
                            //-----------------
                            tButtonTitle = new GUIContent("Install GitLab", "Install GitLab");
                            if (GUI.Button(NWDGUI.AssemblyArea(EditorMatrix[0, EditorMatrixIndex], EditorMatrix[1, EditorMatrixIndex]), tButtonTitle))
                            {
                                List<string> tCommandList = new List<string>();
                                tCommandList.Add("echo \"<color=red> -> server update</color>\"");
                                tCommandList.Add("apt-get update");
                                tCommandList.Add("apt-get -y upgrade");
                                tCommandList.Add("apt-get -y dist-upgrade");

                                tCommandList.Add("echo \"<color=red> -> server prepare directories</color>\"");
                                tCommandList.Add("mkdir /home/gitlab");
                                tCommandList.Add("mkdir /home/gitlab/backups");

                                if (tServer != null) { tServer.ExecuteSSH(tButtonTitle.text, tCommandList); }

                                tCommandList.Add("echo \"<color=red> -> server install</color>\"");
                                tCommandList.Add("apt-get -y install curl");
                                tCommandList.Add("apt-get -y install openssh-server");
                                tCommandList.Add("apt-get -y install ca-certificates");
                                tCommandList.Add("apt-get -y install postfix");
                                tCommandList.Add("apt-get -y install mailutils");


                                tCommandList.Add("echo \"<color=red> -> server prepare install gitlab ce </color>\"");


                                tCommandList.Add("# curl -sS https://packages.gitlab.com/install/repositories/gitlab/gitlab-ce/script.deb.sh | bash");
                                tCommandList.Add("apt-get -y install gitlab-ce");

                                tCommandList.Add("echo \"<color=red> -> config gitlab.rb</color>\"");
                                tCommandList.Add("/etc/gitlab/gitlab.rb");

                                tCommandList.Add("sed -i 's/^.*external_url.*$/external_url = \\'" + GitLabDomainNameServer + "\\'/g' /etc/gitlab/gitlab.rb");
                                tCommandList.Add("sed -i 's/^.*letsencrypt\\[\\'enable\\'\\].*$/letsencrypt[\\'enable\\'] = true/g' /etc/gitlab/gitlab.rb");
                                tCommandList.Add("sed -i 's/^.*letsencrypt\\[\\'contact\\_emails\\'\\].*$/letsencrypt[\\'contact_emails\\'] = \\[\\'" + GitLabEmail + "\\'\\]/g' /etc/gitlab/gitlab.rb");

                                tCommandList.Add("echo \"<color=red> -> gitlab configure</color>\"");
                                tCommandList.Add("gitlab-ctl reconfigure");
                                //tCommandList.Add("gitlab-ctl renew-le-certs");
                                tCommandList.Add("gitlab-ctl start");

                                foreach (string tCom in tCommandList) { Debug.Log(tCom); }

                            }
                            EditorMatrixIndex++;
                        }

                        if (ServerType == NWDServerOtherType.WebServer)
                        {
                            //-----------------
                            tButtonTitle = new GUIContent("Install Apache PHP", "Install Apache and PHP 7");
                            if (GUI.Button(NWDGUI.AssemblyArea(EditorMatrix[0, EditorMatrixIndex], EditorMatrix[1, EditorMatrixIndex]), tButtonTitle))
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
                                if (tServer.Distribution == NWDServerDistribution.debian10)
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
                            EditorMatrixIndex++;
                        }
                        //-----------------

                        //GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), tServer.TextCommandResult);
                    }
                */
            }
            else
            {
                if (GUILayout.Button("Need credentials for actions"))
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
