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
            if (string.IsNullOrEmpty(sRoot) == false)
            {
                tScriptServer.AppendLine("# "+ Distribution);
                // Install Connect
                tScriptServer.AppendLine("ssh -l " + sRoot + " " + sIP);
                if (string.IsNullOrEmpty(sRootPassword) == false)
                {
                    tScriptServer.AppendLine("# enter root password (" + sRootPassword + ")");
                }
                tScriptServer.AppendLine("");
                if (sPort != 22)
                {
                    // Change Port of SSH
                    tScriptServer.AppendLine("# change port");
                    tScriptServer.AppendLine("sed - i 's/^Port .*$/Port " + sPort + "/g' /etc/ssh/sshd_config");
                    tScriptServer.AppendLine("");
                }
                // Change root user
                tScriptServer.AppendLine("# server update");
                tScriptServer.AppendLine(SUDO + "apt-get update" + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get -y upgrade" + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get -y dist-upgrade" + OutputNull);
                tScriptServer.AppendLine("");

                tScriptServer.AppendLine("# install tools");
                tScriptServer.AppendLine(SUDO + "apt-get install vim" + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get install whois" + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get install debconf-utils" + OutputNull);
                tScriptServer.AppendLine("");

                if (string.IsNullOrEmpty(sAdmin_User) == false)
                {
                    tScriptServer.AppendLine("# change root access");
                    tScriptServer.AppendLine("sed -i 's/^PermitRootLogin .*$/PermitRootLogin no/g' /etc/ssh/sshd_config");
                    tScriptServer.AppendLine(SUDO + "service sshd restart");
                    tScriptServer.AppendLine("");

                    // Add admin user
                    tScriptServer.AppendLine("# add admin access");
                    tScriptServer.AppendLine("tmp_admin_password_crypt=$(mkpasswd " + sAdmin_Password + ")");
                    //tScriptServer.AppendLine("useradd --password \"${tmp_admin_password_crypt}\" --shell /bin/bash " + sAdmin_User + "");
                    tScriptServer.AppendLine("useradd --password \"${tmp_admin_password_crypt}\" --shell /bin/false " + sAdmin_User + "");
                    //tScriptServer.AppendLine("usermod -a -G sftp_chroot " + sAdmin_User + "");
                    //tScriptServer.AppendLine("usermod -a -G sudo " + sAdmin_User + "");
                    tScriptServer.AppendLine("mkdir /home/" + sAdmin_User + "");
                    tScriptServer.AppendLine("chown " + sAdmin_User + ":" + sAdmin_User + " /home/" + sAdmin_User + "");
                    tScriptServer.AppendLine("chmod ug+rwX /home/" + sAdmin_User + "");
                    tScriptServer.AppendLine("");
                }
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
            tScriptServer.AppendLine("# install apache");
            tScriptServer.AppendLine(SUDO + "apt-get -y install apache2" + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install apache2-doc" + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install apache2-suexec-custom" + OutputNull);
            tScriptServer.AppendLine("# active apache mod");
            tScriptServer.AppendLine(SUDO + "a2enmod ssl" + OutputNull);
            tScriptServer.AppendLine(SUDO + "a2enmod userdir" + OutputNull);
            tScriptServer.AppendLine(SUDO + "a2enmod suexec" + OutputNull);
            tScriptServer.AppendLine("# apache configure");
            tScriptServer.AppendLine(SUDO + "sed -i 's/\\/var\\/www/\\/home/g' /etc/apache2/suexec/www-data");
            tScriptServer.AppendLine(SUDO + "sed -i 's/public_html\\/cgi-bin/public_html/g'/etc/apache2/suexec/www-data");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a \\ ' /etc/apache2/apache2.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a # add no signature in error page' /etc/apache2/apache2.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a \\ ' /etc/apache2/apache2.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a ServerSignature Off' /etc/apache2/apache2.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a \\ ' /etc/apache2/apache2.conf");
            tScriptServer.AppendLine("# apache restart");
            tScriptServer.AppendLine(SUDO + "systemctl restart apache2" + OutputNull);
            tScriptServer.AppendLine("");

            // Install PHP
            tScriptServer.AppendLine("# install php");
            tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0" + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-mysql" + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-curl" + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-json" + OutputNull);
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-xml" + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-mcrypt" + OutputNull);
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-gd" + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-mbstring" + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-gettext" + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-zip" + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-mail" + OutputNull);
            //tScriptServer.AppendLine(SUDO + "apt-get -y install php7.0-ssh2" + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install php-pear" + OutputNull);
            tScriptServer.AppendLine(SUDO + "apt-get -y install libapache2-mod-php7.0" + OutputNull);
            tScriptServer.AppendLine("# php configure");
            tScriptServer.AppendLine(SUDO + "sed -i 's/upload_max_filesize = 2M/upload_max_filesize = 20M/g' /etc/php/7.0/apache2/php.ini");
            tScriptServer.AppendLine(SUDO + "sed -i 's/max_file_uploads = 20/max_file_uploads = 200/g'/etc/php/7.0/apache2/ php.ini");
            tScriptServer.AppendLine(SUDO + "sed -i 's/zlib.outpout_compression = Off/zlib.outpout_compression = On/g' /etc/php/7.0/apache2/php.ini");
            tScriptServer.AppendLine(SUDO + "sed -i 's/php_admin_flag engine Off/php_admin_flag engine On/g' /etc/apache2/mods-enabled/php7.0.conf");
            tScriptServer.AppendLine("# php folder default");
            tScriptServer.AppendLine(SUDO + "chgrp -R adm /var/www/html/");
            tScriptServer.AppendLine(SUDO + "chmod 775 /var/www/html/");
            tScriptServer.AppendLine(SUDO + "echo $\"<?php echo phpinfo();?>\" > /var/www/html/phpinfo.php");
            tScriptServer.AppendLine(SUDO + "chmod 775 /var/www/html/index.html");
            tScriptServer.AppendLine(SUDO + "echo $\"Are-you lost ? ok, I will help you, you are here!\" > /var/www/html/index.html");
            tScriptServer.AppendLine("# apache restart");
            tScriptServer.AppendLine(SUDO + "systemctl restart apache2" + OutputNull);
            tScriptServer.AppendLine("");

            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallServerApache(string sIP, int sPort, string sAdmin_User, string sAdmin_Password, string sRootPassword)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("# " + Distribution);

            if (string.IsNullOrEmpty(sAdmin_User) == false)
            {
                // Install Connect
                tScriptServer.AppendLine(CommandSSH(sIP, sPort, sAdmin_User, sAdmin_Password, sRootPassword));

                // Install Update
                tScriptServer.AppendLine("# server update");
                tScriptServer.AppendLine(SUDO + "apt-get update" + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get -y upgrade" + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get -y dist-upgrade" + OutputNull);
                tScriptServer.AppendLine("");

                // Install Locales
                tScriptServer.AppendLine("# install Locales");
                tScriptServer.AppendLine(SUDO + "apt-get install locales" + OutputNull);
                tScriptServer.AppendLine("export LANGUAGE=en" + OutputNull);
                tScriptServer.AppendLine("export LC_CTYPE=en_US.UTF-8" + OutputNull);
                tScriptServer.AppendLine("export LC_MESSAGES=en_US.UTF-8" + OutputNull);
                tScriptServer.AppendLine("export LC_ALL=en_US.UTF-8" + OutputNull);
                tScriptServer.AppendLine("locale-gen --purge en_US.UTF-8" + OutputNull);
                tScriptServer.AppendLine("");

                // Install SFTP
                tScriptServer.AppendLine("# install SFTP");
                tScriptServer.AppendLine(SUDO + "addgroup sftp_chroot");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a # add for sftp-server' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a Match Group sftp_chroot' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a ChrootDirectory /home/%u' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a ForceCommand internal-sftp' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a AllowTcpForwarding no' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "service sshd restart");
                tScriptServer.AppendLine("");

                // Install Apache PHP
                tScriptServer.AppendLine(CommandInstallApachePHP());

                // Install Certbot
                tScriptServer.AppendLine("# install Let's Encrypt Certbot");
                tScriptServer.AppendLine(SUDO + "echo $\"deb http://ftp.debian.org/debian stretch-backports main\" >> /etc/apt/sources.list.d/backports.list");
                tScriptServer.AppendLine(SUDO + "apt-get update" + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get -y install python-certbot-apache -t stretch-backports" + OutputNull);
                tScriptServer.AppendLine("# apache restart");
                tScriptServer.AppendLine(SUDO + "systemctl restart apache2");
                tScriptServer.AppendLine("");
            }
            else
            {
                tScriptServer.AppendLine("# need SSH Root User!");
            }

            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallServerMySQL(string sIP, int sPort, string sAdmin_User, string sAdmin_Password, string sRootPassword, string sRootMySQLPassword, bool sMySQLExternal, bool sMySQLPhpMyAdmin)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("# " + Distribution);

            if (string.IsNullOrEmpty(sAdmin_User) == false)
            {
                // Install Connect
                tScriptServer.AppendLine(CommandSSH(sIP, sPort, sAdmin_User, sAdmin_Password, sRootPassword));

                // Install Update
                tScriptServer.AppendLine(SUDO + "apt-get update" + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get -y upgrade" + OutputNull);
                tScriptServer.AppendLine(SUDO + "apt-get -y dist-upgrade" + OutputNull);
                tScriptServer.AppendLine("");

                // Install MySQL
                //tScriptServer.AppendLine("# mysql install");
                //tScriptServer.AppendLine(SUDO + "debconf-set-selections <<< \"mysql-server mysql-server/root_password password " + sRootMySQLPassword + "\"");
                //tScriptServer.AppendLine(SUDO + "debconf-set-selections <<< \"mysql-server mysql-server/root_password_again password " + sRootMySQLPassword + "\"");
                //tScriptServer.AppendLine(SUDO + "apt -y install mysql-server");
                //tScriptServer.AppendLine(SUDO + "apt -y install mysql-client");
                //tScriptServer.AppendLine("# mysql start");
                //tScriptServer.AppendLine(SUDO + "/etc/init.d/mysql start");
                //tScriptServer.AppendLine("");

                tScriptServer.AppendLine("# mysql install (MariaDB)");
                tScriptServer.AppendLine(SUDO + "debconf-set-selections <<< \"mariadb-server mysql-server/root_password password " + sRootMySQLPassword + "\"");
                tScriptServer.AppendLine(SUDO + "debconf-set-selections <<< \"mariadb-server mysql-server/root_password_again password " + sRootMySQLPassword + "\"");
                tScriptServer.AppendLine(SUDO + "apt-get -y install mariadb-server");
                tScriptServer.AppendLine(SUDO + "apt-get -y install mariadb-client");
                tScriptServer.AppendLine("# mysql start");
                tScriptServer.AppendLine(SUDO + "/etc/init.d/mysql start");
                tScriptServer.AppendLine("");

                if (sMySQLPhpMyAdmin == true)
                {
                    // Install Apache PHP
                    tScriptServer.AppendLine(CommandInstallApachePHP());

                    // Install PhpMyAdmin
                    tScriptServer.AppendLine("# phpmyadmin");
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
            }
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallWebService(string sIP, int sPort, string sAdmin_User, string sAdmin_Password, string sRootPassword, string sDNS, string sUser, string sPassword, string sFolder, string sEmail)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("# " + Distribution);

            // Install Connect
            tScriptServer.AppendLine(CommandSSH(sIP, sPort, sAdmin_User, sAdmin_Password, sRootPassword));

            tScriptServer.AppendLine("# add user");
            tScriptServer.AppendLine("tmp_user_password_crypt=$(mkpasswd " + sPassword + ")");
            //tScriptServer.AppendLine(SUDO + "useradd --password \"${tmp_user_password_crypt}\" --gid sftp_chroot --groups sftp_chroot -m --shell /bin/false " + sUser + "");
            tScriptServer.AppendLine(SUDO + "useradd --password \"${tmp_user_password_crypt}\" --shell /bin/false " + sUser + "");
            //tScriptServer.AppendLine(SUDO + "useradd --password \"${tmp_user_password_crypt}\" --shell /sbin/nologin " + sUser + "");
            tScriptServer.AppendLine(SUDO + "usermod -a -G sftp_chroot " + sUser + "");
            tScriptServer.AppendLine("# add user directories");
            tScriptServer.AppendLine(SUDO + "mkdir /home/" + sUser + "");
            tScriptServer.AppendLine(SUDO + "chown " + sUser + ":" + sUser + " /home/" + sUser + "");
            tScriptServer.AppendLine(SUDO + "chmod ug+rwX /home/" + sUser + "");
            tScriptServer.AppendLine(SUDO + "mkdir /home/" + sUser + "/" + sFolder + "");
            tScriptServer.AppendLine(SUDO + "chown " + sUser + ":www-data /home/" + sUser + "/" + sFolder + "");
            tScriptServer.AppendLine(SUDO + "chmod ug+--X /home/" + sUser + "/" + sFolder + "");
            tScriptServer.AppendLine(SUDO + "echo $\"<?php echo phpinfo();?>\" > /home/" + sUser + "/" + sFolder + "/phpinfo.php");
            tScriptServer.AppendLine(SUDO + "echo $\"Hello " + sUser + "! Are-you lost ? ok, I will help you, you are in front of your screen!\" > /home/" + sUser + "/" + sFolder + "/index.html");
            tScriptServer.AppendLine(SUDO + "mkdir /home/" + sUser + "/ssl");
            tScriptServer.AppendLine("# add webservice in apache");
            tScriptServer.AppendLine(SUDO + "echo \"<VirtualHost *:443>\\nServerAdmin " + sEmail + "\\n" +
"ServerName " + sDNS + "\\n" +
"ServerAlias " + sDNS + "\\n" +
"DocumentRoot home/" + sUser + "/" + sFolder + "\\n" +
"< Directory />\\n" +
"AllowOverride All\\n" +
"</ Directory >\\n" +
"< Directory home/" + sUser + "/" + sFolder + " >\\n" +
"Options Indexes FollowSymLinks MultiViews\\n" +
"AllowOverride all\\n" +
"Require all granted\\n" +
"</ Directory >\\n" +
"ErrorLog /var/log/apache2/" + sDNS + "-error.log\\n" +
"LogLevel error\\n" +
"SSLEngine On\\n" +
"SSLProtocol all - SSLv2 - SSLv3\\n" +
"SSLCipherSuite ALL: !DH:!EXPORT:!RC4:+HIGH:+MEDIUM:!LOW:!aNULL:!eNULL\\n" +
"SSLCertificateFile home/" + sUser + "/ssl/" + sDNS + ".crt\\n" +
"SSLCertificateKeyFile home/" + sUser + "/ssl/" + sDNS + ".key\\n" +
"CustomLog /var/log/apache2/" + sDNS + "-ssl-access.log combined\\n" +
"</ VirtualHost >\\n" +
"\"> /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");
            tScriptServer.AppendLine(SUDO + "systemctl restart apache2");
            tScriptServer.AppendLine("# add certbot");
            tScriptServer.AppendLine("certbot --agree-tos --no-eff-email --apache --redirect --email " + sEmail + " -d " + sDNS + "");
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallDatabase(string sIP, int sPort, string sAdmin_User, string sAdmin_Password, string sRootPassword, string sMySQLUser, string sMySQLPassword, string sMySQLBase)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("#Debian 9");
            // Install Connect
            tScriptServer.AppendLine(CommandSSH(sIP, sPort, sAdmin_User, sAdmin_Password, sRootPassword));
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif