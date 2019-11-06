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
using System.IO;
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDServerDebian10
    {
        //-------------------------------------------------------------------------------------------------------------
        static string SUDO = SUDO + "";
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandSSH(string sIP, int sPort, string sRoot, string sRootPassword)
        {
            StringBuilder tScriptServer = new StringBuilder();
            if (string.IsNullOrEmpty(sRoot) == false)
            {
                // Install Connect
                if (sPort != 22)
                {
                    tScriptServer.AppendLine("ssh -l " + sRoot + " " + sIP + " p" + sPort + "");
                    tScriptServer.AppendLine("# or ssh -l " + sRoot + " " + sIP + " p22");
                }
                else
                {
                    tScriptServer.AppendLine("ssh -l " + sRoot + " " + sIP);
                }
                if (string.IsNullOrEmpty(sRootPassword) == false)
                {
                    tScriptServer.AppendLine("# tape your password (" + sRootPassword + ")");
                }
                if (sPort != 22)
                {
                    // Change Port of SSH
                    tScriptServer.AppendLine(SUDO + "sed - i 's/^Port .*$/Port '" + sPort + "'/g' /etc/ssh/sshd_config");
                    tScriptServer.AppendLine(SUDO + "service sshd restart");
                }
                tScriptServer.AppendLine("");
            }
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallApachePHP()
        {
            StringBuilder tScriptServer = new StringBuilder();

            // Install Apache
            tScriptServer.AppendLine("# install apache");
            tScriptServer.AppendLine("");
            tScriptServer.AppendLine(SUDO + "apt -y install apache2");
            tScriptServer.AppendLine(SUDO + "apt -y install apache2-doc");
            tScriptServer.AppendLine(SUDO + "apt -y install apache2-suexec-custom");
            tScriptServer.AppendLine("# active apache mod");
            tScriptServer.AppendLine(SUDO + "a2enmod ssl");
            tScriptServer.AppendLine(SUDO + "a2enmod userdir");
            tScriptServer.AppendLine(SUDO + "a2enmod suexec");
            tScriptServer.AppendLine("# apache configure");
            tScriptServer.AppendLine(SUDO + "sed -i 's/\\/var\\/www/\\/home/g' /etc/apache2/suexec/www-data");
            tScriptServer.AppendLine(SUDO + "sed -i 's/public_html\\/cgi-bin/public_html/g'/etc/apache2/suexec/www-data");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a \\ ' /etc/apache2/apache2.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a # add no signature in error page' /etc/apache2/apache2.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a \\ ' /etc/apache2/apache2.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a ServerSignature Off' /etc/apache2/apache2.conf");
            tScriptServer.AppendLine(SUDO + "sed -i '$ a \\ ' /etc/apache2/apache2.conf");
            tScriptServer.AppendLine("# apache restart");
            tScriptServer.AppendLine(SUDO + "systemctl restart apache2");
            tScriptServer.AppendLine("");

            // Install PHP
            tScriptServer.AppendLine("# php install");
            tScriptServer.AppendLine(SUDO + "apt -y install php7.0");
            tScriptServer.AppendLine(SUDO + "apt -y install php7.0-mysql");
            tScriptServer.AppendLine(SUDO + "apt -y install php7.0-curl");
            tScriptServer.AppendLine(SUDO + "apt -y install php7.0-json");
            tScriptServer.AppendLine(SUDO + "apt -y install php7.0-xml");
            tScriptServer.AppendLine(SUDO + "apt -y install php7.0-mcrypt");
            tScriptServer.AppendLine(SUDO + "apt -y install php7.0-gd");
            tScriptServer.AppendLine(SUDO + "apt -y install php7.0-mbstring");
            tScriptServer.AppendLine(SUDO + "apt -y install php7.0-gettext");
            tScriptServer.AppendLine(SUDO + "apt -y install php7.0-zip");
            tScriptServer.AppendLine(SUDO + "apt -y install php7.0-mail");
            tScriptServer.AppendLine(SUDO + "apt -y install php7.0-ssh2");
            tScriptServer.AppendLine(SUDO + "apt -y install php-pear");
            tScriptServer.AppendLine(SUDO + "apt -y install libapache2-mod-php");
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
            tScriptServer.AppendLine(SUDO + "echo $\"Are-you lost ? ok, I will help you, you are here!\" >/var/www/html/index.html");
            tScriptServer.AppendLine("# apache restart");
            tScriptServer.AppendLine(SUDO + "systemctl restart apache2");
            tScriptServer.AppendLine("");

            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallServerApache(string sIP, int sPort, string sRoot, string sRootPassword)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("# Debian 10");

            if (string.IsNullOrEmpty(sRoot) == false)
            {
                // Install Connect
                tScriptServer.AppendLine(CommandSSH(sIP, sPort, sRoot, sRootPassword));

                // Install Update
                tScriptServer.AppendLine(SUDO + "apt update");
                tScriptServer.AppendLine(SUDO + "apt -y upgrade");
                tScriptServer.AppendLine(SUDO + "apt -y dist-upgrade");
                tScriptServer.AppendLine("");

                // Install Apache PHP
                tScriptServer.AppendLine(CommandInstallApachePHP());

                // Install Tools
                tScriptServer.AppendLine("# install tools");
                tScriptServer.AppendLine(SUDO + "apt -y install vim");
                tScriptServer.AppendLine(SUDO + "apt -y install whois");
                tScriptServer.AppendLine(SUDO + "apt install debconf-utils");
                tScriptServer.AppendLine("");

                // Install SFTP
                tScriptServer.AppendLine("# install SFTP");
                tScriptServer.AppendLine(SUDO + "addgroup sftp_chroot");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a \\ ' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a \\ ' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a # add for sftp-server' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a \\ ' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a Match Group sftp_chroot' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a ChrootDirectory /home/%u' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a ForceCommand internal-sftp' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a AllowTcpForwarding no' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "sed -i '$ a \\ ' /etc/ssh/sshd_config");
                tScriptServer.AppendLine(SUDO + "service sshd restart");
                tScriptServer.AppendLine("");

                // Install Certbot
                tScriptServer.AppendLine("# install Let's Encrypt Certbot");
                tScriptServer.AppendLine(SUDO + "echo $\"deb http://ftp.debian.org/debian buster-backports main\" >> /etc/apt/sources.list.d/backports.list");
                tScriptServer.AppendLine(SUDO + "apt update");
                tScriptServer.AppendLine(SUDO + "apt -y install python-certbot-apache -t buster-backports");
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
        public static string CommandInstallServerMySQL(string sIP, int sPort, string sRoot, string sRootPassword, string sRootMySQLPassword, bool sMySQLExternal, bool sMySQLPhpMyAdmin)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("#Debian 10");

            if (string.IsNullOrEmpty(sRoot) == false)
            {
                // Install Connect
                tScriptServer.AppendLine(CommandSSH(sIP, sPort, sRoot, sRootPassword));

                // Install Update
                tScriptServer.AppendLine(SUDO + "apt update");
                tScriptServer.AppendLine(SUDO + "apt -y upgrade");
                tScriptServer.AppendLine(SUDO + "apt -y dist-upgrade");
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

                tScriptServer.AppendLine("# mysql install");
                tScriptServer.AppendLine(SUDO + "debconf-set-selections <<< \"mariadb-server mysql-server/root_password password " + sRootMySQLPassword + "\"");
                tScriptServer.AppendLine(SUDO + "debconf-set-selections <<< \"mariadb-server mysql-server/root_password_again password " + sRootMySQLPassword + "\"");
                tScriptServer.AppendLine(SUDO + "apt -y install mariadb-server");
                tScriptServer.AppendLine(SUDO + "apt -y install mariadb-client");
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
                    tScriptServer.AppendLine(SUDO + "apt -y -q install phpmyadmin");
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
        public static string CommandInstallWebService(string sIP, int sPort, string sRoot, string sRootPassword, string sDNS, string sUser, string sPassword, string sFolder, string sEmail)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("#Debian 10");
            // Install Connect
            tScriptServer.AppendLine(CommandSSH(sIP, sPort, sRoot, sRootPassword));


            tScriptServer.AppendLine("# add user");
            tScriptServer.AppendLine(SUDO + "useradd --password " + sPassword + " --gid sftp_chroot --groups sftp_chroot -m --shell /bin/false " + sUser + "");
            tScriptServer.AppendLine("# add user directories");
            tScriptServer.AppendLine(SUDO + "mkdir home/" + sUser + "/" + sFolder + "");
            tScriptServer.AppendLine(SUDO + "chown root home/" + sUser + "");
            tScriptServer.AppendLine(SUDO + "chmod go-w home/" + sUser + "");
            tScriptServer.AppendLine(SUDO + "mkdir home/" + sUser + "/ssl");
            tScriptServer.AppendLine(SUDO + "chown " + sUser + ":sftp_chroot home/" + sUser + "/" + sFolder + "");
            tScriptServer.AppendLine(SUDO + "chmod ug+rwX home/" + sUser + "/" + sFolder + "");

            tScriptServer.AppendLine("# add webservice in apache");
            tScriptServer.AppendLine(SUDO + "echo \"<VirtualHost *:443>\nServerAdmin "+ sEmail + "\n" +
"ServerName "+ sDNS + "\n" +
"ServerAlias "+ sDNS + "\n" +
"DocumentRoot home/" + sUser + "/" + sFolder + "\n" +
"< Directory />\n" +
"AllowOverride All\n" +
"</ Directory >\n" +
"< Directory home/" + sUser + "/" + sFolder + " >\n" +
"Options Indexes FollowSymLinks MultiViews\n" +
"AllowOverride all\n" +
"Require all granted\n" +
"</ Directory >\n" +
"ErrorLog /var/log/apache2/" + sDNS + "-error.log\n" +
"LogLevel error\n" +
"SSLEngine On\n" +
"SSLProtocol all - SSLv2 - SSLv3\n" +
"SSLCipherSuite ALL: !DH:!EXPORT:!RC4:+HIGH:+MEDIUM:!LOW:!aNULL:!eNULL\n" +
"SSLCertificateFile home/" + sUser + "/ssl/" + sDNS + ".crt\n" +
"SSLCertificateKeyFile home/" + sUser + "/ssl/" + sDNS + ".key\n" +
"CustomLog /var/log/apache2/" + sDNS + "-ssl-access.log combined\n" +
"</ VirtualHost >\n" +
"\"> /etc/apache2/sites-available/" + sDNS + "_ssl_ws.conf");

            tScriptServer.AppendLine(SUDO + "systemctl restart apache2");

            tScriptServer.AppendLine("# add certbot");
            tScriptServer.AppendLine("certbot --agree-tos --no-eff-email --apache --redirect --email "+sEmail+" -d "+sDNS+"");

            tScriptServer.AppendLine("");
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallDatabase(string sIP, int sPort, string sRoot, string sRootPassword, string sMySQLUser, string sMySQLPassword, string sMySQLBase)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("#Debian 10");
            // Install Connect
            tScriptServer.AppendLine(CommandSSH(sIP, sPort, sRoot, sRootPassword));
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif