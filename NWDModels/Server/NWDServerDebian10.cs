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
        public static string CommandInstallServerApache(string sIP, int sPort, string sRoot, string sRootPassword)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("# Debian 10");

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
                    tScriptServer.AppendLine("sudo sed - i 's/^Port .*$/Port '" + sPort + "'/g' /etc/ssh/sshd_config");
                    tScriptServer.AppendLine("sudo service sshd restart");
                }
                tScriptServer.AppendLine("");

                // Install Update
                tScriptServer.AppendLine("sudo apt update");
                tScriptServer.AppendLine("sudo apt -y upgrade");
                tScriptServer.AppendLine("sudo apt -y dist-upgrade");
                tScriptServer.AppendLine("");

                // Install Tools
                tScriptServer.AppendLine("# install tools");
                tScriptServer.AppendLine("sudo apt -y install vim");
                tScriptServer.AppendLine("sudo apt -y install whois");
                tScriptServer.AppendLine("sudo apt install debconf-utils");
                tScriptServer.AppendLine("");

                // Install SFTP
                tScriptServer.AppendLine("# install SFTP");
                tScriptServer.AppendLine("sudo addgroup sftp_chroot");
                tScriptServer.AppendLine("sudo sed -i '$ a \\ ' /etc/ssh/sshd_config");
                tScriptServer.AppendLine("sudo sed -i '$ a \\ ' /etc/ssh/sshd_config");
                tScriptServer.AppendLine("sudo sed -i '$ a # add for sftp-server' /etc/ssh/sshd_config");
                tScriptServer.AppendLine("sudo sed -i '$ a \\ ' /etc/ssh/sshd_config");
                tScriptServer.AppendLine("sudo sed -i '$ a Match Group sftp_chroot' /etc/ssh/sshd_config");
                tScriptServer.AppendLine("sudo sed -i '$ a ChrootDirectory /home/%u' /etc/ssh/sshd_config");
                tScriptServer.AppendLine("sudo sed -i '$ a ForceCommand internal-sftp' /etc/ssh/sshd_config");
                tScriptServer.AppendLine("sudo sed -i '$ a AllowTcpForwarding no' /etc/ssh/sshd_config");
                tScriptServer.AppendLine("sudo sed -i '$ a \\ ' /etc/ssh/sshd_config");
                tScriptServer.AppendLine("sudo service sshd restart");
                tScriptServer.AppendLine("");

                // Install Apache
                tScriptServer.AppendLine("# install apache");
                tScriptServer.AppendLine("");
                tScriptServer.AppendLine("sudo apt -y install apache2");
                tScriptServer.AppendLine("sudo apt -y install apache2-doc");
                tScriptServer.AppendLine("sudo apt -y install apache2-suexec-custom");
                tScriptServer.AppendLine("# active apache mod");
                tScriptServer.AppendLine("sudo a2enmod ssl");
                tScriptServer.AppendLine("sudo a2enmod userdir");
                tScriptServer.AppendLine("sudo a2enmod suexec");
                tScriptServer.AppendLine("# apache configure");
                tScriptServer.AppendLine("sudo sed -i 's/\\/var\\/www/\\/home/g' /etc/apache2/suexec/www-data");
                tScriptServer.AppendLine("sudo sed -i 's/public_html\\/cgi-bin/public_html/g'/etc/apache2/suexec/www-data");
                tScriptServer.AppendLine("sudo sed -i '$ a \\ ' /etc/apache2/apache2.conf");
                tScriptServer.AppendLine("sudo sed -i '$ a # add no signature in error page' /etc/apache2/apache2.conf");
                tScriptServer.AppendLine("sudo sed -i '$ a \\ ' /etc/apache2/apache2.conf");
                tScriptServer.AppendLine("sudo sed -i '$ a ServerSignature Off' /etc/apache2/apache2.conf");
                tScriptServer.AppendLine("sudo sed -i '$ a \\ ' /etc/apache2/apache2.conf");
                tScriptServer.AppendLine("# apache restart");
                tScriptServer.AppendLine("sudo systemctl restart apache2");
                tScriptServer.AppendLine("");

                // Install PHP
                tScriptServer.AppendLine("# php install");
                tScriptServer.AppendLine("sudo apt -y install php7.0");
                tScriptServer.AppendLine("sudo apt -y install php7.0-mysql");
                tScriptServer.AppendLine("sudo apt -y install php7.0-curl");
                tScriptServer.AppendLine("sudo apt -y install php7.0-json");
                tScriptServer.AppendLine("sudo apt -y install php7.0-xml");
                tScriptServer.AppendLine("sudo apt -y install php7.0-mcrypt");
                tScriptServer.AppendLine("sudo apt -y install php7.0-gd");
                tScriptServer.AppendLine("sudo apt -y install php7.0-mbstring");
                tScriptServer.AppendLine("sudo apt -y install php7.0-zip");
                tScriptServer.AppendLine("sudo apt -y install php7.0-mail");
                tScriptServer.AppendLine("sudo apt -y install php7.0-ssh2");
                tScriptServer.AppendLine("sudo apt -y install php-pear");
                tScriptServer.AppendLine("sudo apt -y install libapache2-mod-php");
                tScriptServer.AppendLine("# php configure");
                tScriptServer.AppendLine("sudo sed -i 's/upload_max_filesize = 2M/upload_max_filesize = 20M/g' /etc/php/7.0/apache2/php.ini");
                tScriptServer.AppendLine("sudo sed -i 's/max_file_uploads = 20/max_file_uploads = 200/g'/etc/php/7.0/apache2/ php.ini");
                tScriptServer.AppendLine("sudo sed -i 's/zlib.outpout_compression = Off/zlib.outpout_compression = On/g' /etc/php/7.0/apache2/php.ini");
                tScriptServer.AppendLine("sudo sed -i 's/php_admin_flag engine Off/php_admin_flag engine On/g' /etc/apache2/mods-enabled/php7.0.conf");
                tScriptServer.AppendLine("# php folder default");
                tScriptServer.AppendLine("sudo chgrp -R adm /var/www/html/");
                tScriptServer.AppendLine("sudo chmod 775 /var/www/html/");
                tScriptServer.AppendLine("sudo echo $\"<?php echo phpinfo();?>\" > /var/www/html/phpinfo.php");
                tScriptServer.AppendLine("sudo chmod 775 /var/www/html/index.html");
                tScriptServer.AppendLine("sudo echo $\"Are-you lost ? ok, I will help you, you are here!\" >/var/www/html/index.html");
                tScriptServer.AppendLine("# apache restart");
                tScriptServer.AppendLine("sudo systemctl restart apache2");
                tScriptServer.AppendLine("");

                // Install Certbot
                tScriptServer.AppendLine("# install Let's Encrypt Certbot");
                tScriptServer.AppendLine("sudo echo $\"deb http://ftp.debian.org/debian buster-backports main\" >> /etc/apt/sources.list.d/backports.list");
                tScriptServer.AppendLine("sudo apt update");
                tScriptServer.AppendLine("sudo apt -y install python-certbot-apache -t buster-backports");
                tScriptServer.AppendLine("# apache restart");
                tScriptServer.AppendLine("sudo systemctl restart apache2");
                tScriptServer.AppendLine("");
            }
            else
            {
                tScriptServer.AppendLine("# need SSH Root User!");
            }

            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallServerMySQL(string sIP, int sPort, string sRoot, string sRootPassword, bool sMySQLExternal, bool sMySQLPhpMyAdmin)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("#Debian 10");

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
                    tScriptServer.AppendLine("sudo sed - i 's/^Port .*$/Port '" + sPort + "'/g' /etc/ssh/sshd_config");
                    tScriptServer.AppendLine("sudo service sshd restart");
                }
                tScriptServer.AppendLine("");

                // Install Update
                tScriptServer.AppendLine("sudo apt update");
                tScriptServer.AppendLine("sudo apt -y upgrade");
                tScriptServer.AppendLine("sudo apt -y dist-upgrade");
                tScriptServer.AppendLine("");

                // Install MySQL
                tScriptServer.AppendLine("# mysql install");
                tScriptServer.AppendLine("debconf -set -selections <<< \"mysql -server mysql-server/root_password password ${tmp_mysql_root_password}\"");
                tScriptServer.AppendLine("debconf -set -selections <<< \"mysql -server mysql-server/root_password_again password ${tmp_mysql_root_password}\"");
                tScriptServer.AppendLine("apt -y install mysql-server");
                tScriptServer.AppendLine("apt -y install mysql-client");
                tScriptServer.AppendLine("# mysql start");
                tScriptServer.AppendLine("sudo /etc/init.d/mysql start");
                tScriptServer.AppendLine("");

                if (sMySQLPhpMyAdmin == true)
                {
                    // Install PhpMyAdmin
                    tScriptServer.AppendLine("# phpmyadmin");
                    tScriptServer.AppendLine("debconf -set -selections <<< \"phpmyadmin phpmyadmin/dbconfig-install boolean true\"");
                    tScriptServer.AppendLine("debconf -set -selections <<< \"phpmyadmin phpmyadmin/app-password-confirm password ${tmp_mysql_root_password}\"");
                    tScriptServer.AppendLine("debconf -set -selections <<< \"phpmyadmin phpmyadmin/mysql/admin-pass password ${tmp_mysql_root_password}\"");
                    tScriptServer.AppendLine("debconf -set -selections <<< \"phpmyadmin phpmyadmin/mysql/app-pass password ${tmp_mysql_root_password}\"");
                    tScriptServer.AppendLine("debconf -set -selections <<< \"phpmyadmin phpmyadmin/reconfigure-webserver multiselect none\"");
                    tScriptServer.AppendLine("apt -y -q install phpmyadmin;");
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
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string CommandInstallDatabase(string sIP, int sPort, string sRoot, string sRootPassword, string sMySQLUser, string sMySQLPassword, string sMySQLBase)
        {
            StringBuilder tScriptServer = new StringBuilder();
            tScriptServer.AppendLine("#Debian 10");
            return tScriptServer.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif