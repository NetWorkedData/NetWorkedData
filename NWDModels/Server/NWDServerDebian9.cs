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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDServerDebian9
    {
        //-------------------------------------------------------------------------------------------------------------
        //static string SUDO = "sudo ";
        static string Distribution = "Debian 9 (strech)";
        static string SUDO = "";
        static string OutputNull = " > /dev/null 2>&1";
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallServerSSH(string sIP, int sPort, string sRoot, string sRootPassword, string sAdmin_User, string sAdmin_Password)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("#!/bin/bash");
            tScriptServer.AppendLine("# " + Distribution);
            if (string.IsNullOrEmpty(sRoot) == false)
            {
                tScriptServer.AppendLine("echo \" -> " + Distribution +"\"");
                // Install Connect
                tScriptServer.AppendLine("ssh -l " + sRoot + " " + sIP);
                if (string.IsNullOrEmpty(sRootPassword) == false)
                {
                    tScriptServer.AppendLine("# enter root password (" + sRootPassword + ")");
                }
                tScriptServer.AppendLine("");
                // Change root user
                tScriptServer.AppendLine("echo \" -> server update\"");
                tScriptServer.AppendLine(SUDO + "apt-get update");// + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get -y upgrade");// + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get -y dist-upgrade");// + OutputNull);
                tScriptServer.AppendLine("");

                tScriptServer.AppendLine("echo \" -> install tools and sftp\"");
                //tScriptServer.AppendLine(SUDO + "apt-get install vim  debconf-utils whois");// + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get install vim");// + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get install whois");// + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get install debconf-utils");// + OutputNull);
                tScriptServer.AppendLine("");

                // Install Locales
                tScriptServer.AppendLine("echo \" -> install Locales\"");
                tScriptServer.AppendLine(SUDO + "apt-get install locales" + OutputNull);
                tScriptServer.AppendLine("export LANGUAGE=en" + OutputNull);
                tScriptServer.AppendLine("export LC_CTYPE=en_US.UTF-8" + OutputNull);
                tScriptServer.AppendLine("export LC_MESSAGES=en_US.UTF-8" + OutputNull);
                tScriptServer.AppendLine("export LC_ALL=en_US.UTF-8" + OutputNull);
                tScriptServer.AppendLine("locale-gen --purge en_US.UTF-8" + OutputNull);
                tScriptServer.AppendLine("");

                if (sPort != 22)
                {
                    // Change Port of SSH
                    tScriptServer.AppendLine("echo \" -> change port\"");
                    //tScriptServer.AppendLine("sed -i 's/^Port .*$/Port " + sPort + "/g' /etc/ssh/sshd_config");
                    //tScriptServer.AppendLine("sed -i 's/^[\\#]+Port .*$/Port " + sPort + "/g' /etc/ssh/sshd_config");
                    tScriptServer.AppendLine("sed -i 's/^.*Port .*$/Port " + sPort + "/g' /etc/ssh/sshd_config");
                    tScriptServer.AppendLine(SUDO + "service sshd restart");
                    tScriptServer.AppendLine("");
                }
                if (string.IsNullOrEmpty(sAdmin_User) == false)
                {
                    // Add admin user
                    tScriptServer.AppendLine("echo \" -> add admin access\"");
                    tScriptServer.AppendLine("tmp_admin_password_crypt=$(mkpasswd " + sAdmin_Password + ")");
                    tScriptServer.AppendLine("useradd --password \"${tmp_admin_password_crypt}\" --shell /bin/bash " + sAdmin_User + "");
                    tScriptServer.AppendLine("mkdir /home/" + sAdmin_User + "");
                    tScriptServer.AppendLine("chown " + sAdmin_User + ":" + sAdmin_User + " /home/" + sAdmin_User + "");
                    tScriptServer.AppendLine("chmod 770 /home/" + sAdmin_User + "");
                    tScriptServer.AppendLine("");

                    tScriptServer.AppendLine("echo \" -> forbidden root access\"");
                    tScriptServer.AppendLine("sed -i 's/^.*PermitRootLogin .*$/PermitRootLogin no/g' /etc/ssh/sshd_config");
                    tScriptServer.AppendLine(SUDO + "service sshd restart");
                    tScriptServer.AppendLine("");
                }

                tScriptServer.AppendLine("echo \" -> install sftp_chroot group\"");
                tScriptServer.AppendLine(SUDO + "addgroup sftp_chroot");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a # sftp_chroot start' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a Match Group sftp_chroot' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a    ChrootDirectory /home/%u' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a    ForceCommand internal-sftp' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a    AllowTcpForwarding no' /etc/ssh/sshd_config");
                //tScriptServer.AppendLine(SUDO + "sed -i '$ a X11Forwarding no' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a # sftp_chroot end' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "service sshd restart");
                tScriptServer.AppendLine("");

                tScriptServer.AppendLine("exit");

                // Install Connect
                tScriptServer.AppendLine("echo \" -> test your admin\"");
                tScriptServer.AppendLine(CommandSSH(sIP, sPort, sAdmin_User, sAdmin_Password, sRootPassword));
                tScriptServer.AppendLine("exit");
                tScriptServer.AppendLine("exit");

            }
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandSSH(string sIP, int sPort, string sAdmin_User, string sAdmin_Password, string sRootPassword)
        {
            StringBuilder tScriptServer = new StringBuilder();
            if (string.IsNullOrEmpty(sAdmin_User) == false)
            {
                // Install Connect
                tScriptServer.AppendLine("ssh -l " + sAdmin_User + " " + sIP + " -p" + sPort + "");
                if (string.IsNullOrEmpty(sAdmin_Password) == false)
                {
                    tScriptServer.AppendLine("# enter admin password (" + sAdmin_Password + ")");
                }
                tScriptServer.AppendLine("su -");
                tScriptServer.AppendLine("# enter root password (" + sRootPassword + ")");
            }
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallApachePHP()
        {
            StringBuilder tScriptServer = new StringBuilder();

            // Install Apache
            tScriptServer.AppendLine("echo \" -> install apache\"");
            //tScriptServer.AppendLine(SUDO + "apt-get -y install apache2 apache2-doc apache2-suexec-custom");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install apache2");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install apache2-doc");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install apache2-suexec-custom");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install logrotate");// + OutputNull);
            tScriptServer.AppendLine("echo \" -> active apache mod\"");
            tScriptServer.AppendLine(SUDO + "a2enmod ssl");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "a2enmod userdir");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "a2enmod suexec");// + OutputNull);
            tScriptServer.AppendLine("echo \" -> apache configure\"");
            tScriptServer.AppendLine(SUDO + "sed -i 's/\\/var\\/www/\\/home/g' /etc/apache2/suexec/www-data");
            //tScriptServer.AppendLine(SUDO + "sed -i 's/public_html\\/cgi-bin/public_html/g'/etc/apache2/suexec/www-data");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a \\ ' /etc/apache2/apache2.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a # add no signature in error page' /etc/apache2/apache2.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a \\ ' /etc/apache2/apache2.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a ServerSignature Off' /etc/apache2/apache2.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a \\ ' /etc/apache2/apache2.conf");
            tScriptServer.AppendLine("echo \" -> apache restart\"");
            tScriptServer.AppendLine(SUDO + "systemctl restart apache2");// + OutputNull);
            tScriptServer.AppendLine("");

            // Install PHP
            tScriptServer.AppendLine("echo \" -> install php\"");
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php php-mysql php-curl php-json php-mcrypt php-mbstring php-gettext php-zip php-mail php-pear");// + OutputNull);
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0");// + OutputNull);
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-mysql");// + OutputNull);
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-curl");// + OutputNull);
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-json");// + OutputNull);
            ////tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-xml");// + OutputNull);
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-mcrypt");// + OutputNull);
            ////tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-gd");// + OutputNull);
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-mbstring");// + OutputNull);
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-gettext");// + OutputNull);
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-zip");// + OutputNull);
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-mail");// + OutputNull);
            ////tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-ssh2");// + OutputNull);
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php-pear");// + OutputNull);

            tScriptServer.AppendLine(SUDO + "apt-get -y install php");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php-mysql");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php-curl");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php-json");// + OutputNull);
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php-xml");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php-mcrypt");// + OutputNull);
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php-gd");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php-mbstring");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php-gettext");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php-zip");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php-mail");// + OutputNull);
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php-ssh2");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php-pear");// + OutputNull);

            //tScriptServer.AppendLine(SUDO + "apt-get -y install libapache2-mod-php7.0");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install libapache2-mod-php");// + OutputNull);
            tScriptServer.AppendLine(SUDO + "pear install Net_SMTP");

            //tScriptServer.AppendLine("echo \" -> php configure\"");
            //tScriptServer.AppendLine(SUDO + "sed -i 's/upload_max_filesize = 2M/upload_max_filesize = 20M/g' /etc/php/7.0/apache2/php.ini");
            //tScriptServer.AppendLine(SUDO + "sed -i 's/max_file_uploads = 20/max_file_uploads = 200/g'/etc/php/7.0/apache2/ php.ini");
            //tScriptServer.AppendLine(SUDO + "sed -i 's/zlib.outpout_compression = Off/zlib.outpout_compression = On/g' /etc/php/7.0/apache2/php.ini");
            //tScriptServer.AppendLine(SUDO + "sed -i 's/php_admin_flag engine Off/php_admin_flag engine On/g' /etc/apache2/mods-enabled/php7.0.conf");
            tScriptServer.AppendLine("echo \" -> php folder default\"");
            tScriptServer.AppendLine(SUDO + "chgrp -R www-data /var/www/html/");
            tScriptServer.AppendLine(SUDO + "chmod 750 /var/www/html/");
            tScriptServer.AppendLine(SUDO + "echo $\"<?php echo phpinfo();?>\" > /var/www/html/phpinfo.php");
            tScriptServer.AppendLine(SUDO + "echo $\"Are you lost? ok, I'll help you, you're in front of a screen!\" > /var/www/html/index.html");
            tScriptServer.AppendLine("echo \" -> apache restart\"");
            tScriptServer.AppendLine(SUDO + "systemctl restart apache2");// + OutputNull);
            tScriptServer.AppendLine("");

            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallServerApache(string sIP, int sPort, string sAdmin_User, string sAdmin_Password, string sRootPassword)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("#!/bin/bash");
            tScriptServer.AppendLine("# " + Distribution);

            if (string.IsNullOrEmpty(sAdmin_User) == false)
            {
                // Install Connect
                tScriptServer.AppendLine(CommandSSH(sIP, sPort, sAdmin_User, sAdmin_Password, sRootPassword));

                // Install Update
                tScriptServer.AppendLine("echo \" -> server update\"");
                tScriptServer.AppendLine(SUDO + "apt-get update" + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get -y upgrade" + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get -y dist-upgrade" + OutputNull);
                tScriptServer.AppendLine("");

                // Install Apache PHP
                tScriptServer.AppendLine(CommandInstallApachePHP());

                // Install Certbot
                tScriptServer.AppendLine("echo \" -> install Let's Encrypt Certbot\"");
                tScriptServer.AppendLine(SUDO + "echo $\"deb http://ftp.debian.org/debian stretch-backports main\" >> /etc/apt/sources.list.d/backports.list");
                tScriptServer.AppendLine(SUDO + "apt-get update" + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get -y install python-certbot-apache -t stretch-backports" + OutputNull);
                tScriptServer.AppendLine("echo \" -> apache restart\"");
                tScriptServer.AppendLine(SUDO + "systemctl restart apache2");
                tScriptServer.AppendLine("");
            }
            else
            {
                tScriptServer.AppendLine("# need SSH Root User!");
                tScriptServer.AppendLine("echo \" -> need SSH Root User!\"");
            }
            tScriptServer.AppendLine(SUDO + "cat /dev/null > ~/.bash_history");
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallServerMySQL(string sIP, int sPort, string sAdmin_User, string sAdmin_Password, string sRootPassword, string sRootMySQLPassword, bool sMySQLExternal, bool sMySQLPhpMyAdmin)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("#!/bin/bash");
            tScriptServer.AppendLine("# " + Distribution);

            if (string.IsNullOrEmpty(sAdmin_User) == false)
            {
                // Install Connect
                tScriptServer.AppendLine(CommandSSH(sIP, sPort, sAdmin_User, sAdmin_Password, sRootPassword));

                // Install Update
                tScriptServer.AppendLine("echo \" -> server update\"");
                tScriptServer.AppendLine(SUDO + "apt-get update" + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get -y upgrade" + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get -y dist-upgrade" + OutputNull);
                tScriptServer.AppendLine("");

                // Install MySQL
                //tScriptServer.AppendLine(SUDO + "debconf-set-selections <<< \"mysql-server mysql-server/root_password password " + sRootMySQLPassword + "\"");
                //tScriptServer.AppendLine(SUDO + "debconf-set-selections <<< \"mysql-server mysql-server/root_password_again password " + sRootMySQLPassword + "\"");
                //tScriptServer.AppendLine(SUDO + "apt -y install mysql-server");
                //tScriptServer.AppendLine(SUDO + "apt -y install mysql-client");
                //tScriptServer.AppendLine("# mysql start");
                //tScriptServer.AppendLine(SUDO + "/etc/init.d/mysql start");
                //tScriptServer.AppendLine("");

                tScriptServer.AppendLine("echo \" -> mysql install (MariaDB)\"");
                tScriptServer.AppendLine(SUDO + "debconf-set-selections <<< \"mariadb-server mysql-server/root_password password " + sRootMySQLPassword + "\"");
                tScriptServer.AppendLine(SUDO + "debconf-set-selections <<< \"mariadb-server mysql-server/root_password_again password " + sRootMySQLPassword + "\"");
                tScriptServer.AppendLine(SUDO + "apt-get -y install mariadb-server");
                tScriptServer.AppendLine(SUDO + "apt-get -y install mariadb-client");
                tScriptServer.AppendLine("echo \" -> mysql start (MariaDB)\"");
                tScriptServer.AppendLine(SUDO + "/etc/init.d/mysql start");
                tScriptServer.AppendLine("");

                if (sMySQLPhpMyAdmin == true)
                {
                    // Install Apache PHP
                    tScriptServer.AppendLine(CommandInstallApachePHP());

                    // Install PhpMyAdmin
                    tScriptServer.AppendLine("echo \" -> phpmyadmin install\"");
                    tScriptServer.AppendLine(SUDO + "debconf-set-selections <<< \"phpmyadmin phpmyadmin/dbconfig-install boolean true\"");
                    tScriptServer.AppendLine(SUDO + "debconf-set-selections <<< \"phpmyadmin phpmyadmin/app-password-confirm password " + sRootMySQLPassword + "\"");
                    tScriptServer.AppendLine(SUDO + "debconf-set-selections <<< \"phpmyadmin phpmyadmin/mysql/admin-pass password " + sRootMySQLPassword + "\"");
                    tScriptServer.AppendLine(SUDO + "debconf-set-selections <<< \"phpmyadmin phpmyadmin/mysql/app-pass password " + sRootMySQLPassword + "\"");
                    tScriptServer.AppendLine(SUDO + "debconf-set-selections <<< \"phpmyadmin phpmyadmin/reconfigure-webserver multiselect none\"");
                    tScriptServer.AppendLine(SUDO + "apt-get -y -q install phpmyadmin");
                    tScriptServer.AppendLine(SUDO + "systemctl restart apache2");
                    tScriptServer.AppendLine("");
                }
            }
            else
            {
                tScriptServer.AppendLine("# need SSH Root User!");
                tScriptServer.AppendLine("echo \" -> need SSH Root User!\"");
            }
            tScriptServer.AppendLine(SUDO + "cat /dev/null > ~/.bash_history");
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallWebService(string sIP, int sPort, string sAdmin_User, string sAdmin_Password, string sRootPassword, string sDNS, string sUser, string sPassword, string sFolder, string sEmail)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("# " + Distribution);

            // https://debian-facile.org/viewtopic.php?id=9607
            // http://www.kitpages.fr/fr/cms/193/installer-un-sftp-avec-chroot-sur-debian
            // Install Connect
            tScriptServer.AppendLine(CommandSSH(sIP, sPort, sAdmin_User, sAdmin_Password, sRootPassword));

            tScriptServer.AppendLine("echo \" -> add user\"");
            tScriptServer.AppendLine("tmp_user_password_crypt=$(mkpasswd " + sPassword + ")");
            //tScriptServer.AppendLine(SUDO + "useradd --password \"${tmp_user_password_crypt}\" --gid sftp_chroot --groups sftp_chroot -m --shell /bin/false " + sUser + "");
            //tScriptServer.AppendLine(SUDO + "useradd --password \"${tmp_user_password_crypt}\" --shell /bin/bash-static " + sUser + "");
            //tScriptServer.AppendLine(SUDO + "useradd --password \"${tmp_user_password_crypt}\" --shell /sbin/nologin " + sUser + "");
            //tScriptServer.AppendLine(SUDO + "useradd --password \"${tmp_user_password_crypt}\" --shell /bin/bash" + sUser + "");
            tScriptServer.AppendLine(SUDO + "useradd --password \"${tmp_user_password_crypt}\" --shell /bin/false " + sUser + "");
            tScriptServer.AppendLine(SUDO + "usermod -a -G sftp_chroot " + sUser + "");
            tScriptServer.AppendLine("echo \" -> add user directories\"");
            tScriptServer.AppendLine(SUDO + "mkdir /home/" + sUser + "");
            //tScriptServer.AppendLine(SUDO + "chown -R " + sUser + ":www-data /home/" + sUser + "");
            tScriptServer.AppendLine(SUDO + "chown root /home/" + sUser + ""); // root for ChrootDirectory
            tScriptServer.AppendLine(SUDO + "chmod go-w  /home/" + sUser + ""); // acces

            tScriptServer.AppendLine(SUDO + "mkdir /home/" + sUser + "/" + sFolder + "");
            tScriptServer.AppendLine(SUDO + "chown -R " + sUser + ":www-data /home/" + sUser + "/" + sFolder + "");
            tScriptServer.AppendLine(SUDO + "chmod -R 750 /home/" + sUser + "/" + sFolder + "");

            tScriptServer.AppendLine(SUDO + "mkdir /home/" + sUser + "/ssl");
            tScriptServer.AppendLine(SUDO + "chown -R " + sUser + ":www-data /home/" + sUser + "/ssl");
            tScriptServer.AppendLine(SUDO + "chmod -R 750 /home/" + sUser + "/ssl");

            tScriptServer.AppendLine(SUDO + "echo $\"<?php echo phpinfo();?>\" > /home/" + sUser + "/" + sFolder + "/phpinfo.php");
            tScriptServer.AppendLine(SUDO + "echo $\"Hello " + sDNS + "!\" > /home/" + sUser + "/" + sFolder + "/index.html");
            tScriptServer.AppendLine("echo \" -> add user webservices in apache\"");

            tScriptServer.AppendLine(SUDO + "/etc/init.d/apache2 stop");

            tScriptServer.AppendLine(SUDO + "openssl req -x509 -sha256 -nodes -days 365 -newkey rsa:2048 -keyout /home/" + sUser + "/ssl/" + sDNS + ".key -out /home/" + sUser + "/ssl/" + sDNS + ".crt -subj \"/C=FR/ST=LILLE/L=LILLE/O=Global Security/OU=IT Department/CN=" + sDNS + "\"");
            tScriptServer.AppendLine(SUDO + "rm /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");

            tScriptServer.AppendLine(SUDO + "echo \"<VirtualHost *:443>\" > /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            if (string.IsNullOrEmpty(sEmail) == false)
            {
                tScriptServer.AppendLine(SUDO + "sed -i '$ a ServerAdmin " + sEmail + "' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            }
            else
            {
                tScriptServer.AppendLine(SUDO + "sed -i '$ a ServerAdmin contact@" + sDNS + "' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            }
            tScriptServer.AppendLine(SUDO + "sed -i '$ a ServerName " + sDNS + "' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a ServerAlias " + sDNS + "' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a DocumentRoot /home/" + sUser + "/" + sFolder + "' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a <Directory />' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a AllowOverride All' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a </Directory>' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a <Directory /home/" + sUser + "/" + sFolder + ">' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a Options Indexes FollowSymLinks MultiViews' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a AllowOverride all' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a Require all granted' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a </Directory>' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a ErrorLog /var/log/apache2/" + sDNS + "-error.log' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a LogLevel error' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a SSLEngine On' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a SSLProtocol all -SSLv2 -SSLv3' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a SSLCipherSuite ALL:!DH:!EXPORT:!RC4:+HIGH:+MEDIUM:!LOW:!aNULL:!eNULL' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a SSLCertificateFile /home/" + sUser + "/ssl/" + sDNS + ".crt' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a SSLCertificateKeyFile /home/" + sUser + "/ssl/" + sDNS + ".key' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a CustomLog /var/log/apache2/" + sDNS + "-ssl-access.log combined' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a </VirtualHost>' /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");

            tScriptServer.AppendLine("echo \" -> add user webservices in apache finish\"");
            tScriptServer.AppendLine(SUDO + "a2ensite " + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "/etc/init.d/apache2 reload");
            tScriptServer.AppendLine(SUDO + "/etc/init.d/apache2 start");
            tScriptServer.AppendLine("echo \" -> add user webservices certbot\"");
            if (string.IsNullOrEmpty(sEmail) == false)
            {
                tScriptServer.AppendLine("certbot --agree-tos --no-eff-email --apache --redirect --email " + sEmail + " -d " + sDNS + "");
            }
            else
            {
                tScriptServer.AppendLine("certbot --agree-tos --no-eff-email --apache --redirect --email contact@" + sDNS + " -d " + sDNS + "");
            }
            tScriptServer.AppendLine(SUDO + "cat /dev/null > ~/.bash_history");
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallDatabase(string sIP, int sPort, string sAdmin_User, string sAdmin_Password, string sRootPassword, string sMySQLRootPassword, string sMySQLUser, string sMySQLPassword, string sMySQLBase)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("#!/bin/bash");
            tScriptServer.AppendLine("# " + Distribution);

            // Install Connect
            tScriptServer.AppendLine(CommandSSH(sIP, sPort, sAdmin_User, sAdmin_Password, sRootPassword));

            // Install mysql database
            tScriptServer.AppendLine("echo \" -> add user in mysql\"");
            tScriptServer.AppendLine("mysql -u root -p\""+ sMySQLRootPassword + "\" -e \"create database "+ sMySQLBase + ";\"");
            tScriptServer.AppendLine("mysql -u root -p\""+ sMySQLRootPassword + "\" -e \"GRANT ALL PRIVILEGES ON " + sMySQLBase + ".* TO " + sMySQLUser + "@localhost IDENTIFIED BY '"+ sMySQLPassword + "';\"");

            tScriptServer.AppendLine(SUDO + "cat /dev/null > ~/.bash_history");
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif