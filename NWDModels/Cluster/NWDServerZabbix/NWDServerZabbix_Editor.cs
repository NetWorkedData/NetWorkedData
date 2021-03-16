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
    /*
     * 
     * https://tecadmin.net/install-zabbix-on-debian/
     * 
     * */
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServerZabbix : NWDServer
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
            if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerateOneOrMore))
            {
                //-----------------
                if (GUILayout.Button(new GUIContent("Install Zabbix as server", "Install NWDServerZabbix")))
                {
                    InstallZabbixServer();
                }
            }
            else
            {
                if (GUILayout.Button("Need credentials for actions"))
                {
                    NWDProjectCredentialsManager.SharedInstanceFocus();
                }
            }
            //-----------------
            NWDGUILayout.Separator();
            base.AddonEditor(sRect);
        }
        //-------------------------------------------------------------------------------------------------------------
        void InstallZabbixServer()
        {
            List<string> tCommandList = new List<string>();
            tCommandList.Add("echo \"<color=red> -> server update</color>\"");
            tCommandList.Add("apt-get clean");
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
            //"sed -i 's/public_html\\/cgi-bin/public_html/g'/etc/apache2/suexec/www-data",;
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

            ExecuteSSH("Save GitLab", tCommandList);
        }
        //-------------------------------------------------------------------------------------------------------------
        /*bool GitLabAWS()
        {
            return string.IsNullOrEmpty(GitLabAWSRegion) == false
                && string.IsNullOrEmpty(GitLabAWSDirectory) == false
                && string.IsNullOrEmpty(GitLabAWSAccessKeyID.Decrypt()) == false
                && string.IsNullOrEmpty(GitLabAWSSecretAccessKey.Decrypt()) == false;
        }*/
        //-------------------------------------------------------------------------------------------------------------
        /*List<string> InstallGitLabCommand()
        {
            List<string> tCommandList = new List<string>();
            tCommandList.Add("echo \"<color=red> -> server update</color>\"");

            tCommandList.Add("apt-get clean");
            tCommandList.Add("apt-get update");
            tCommandList.Add("apt-get -y upgrade");
            tCommandList.Add("apt-get -y dist-upgrade");
            tCommandList.Add("apt-get install -y curl");
            tCommandList.Add("apt-get install -y openssh-server");
            tCommandList.Add("apt-get install -y ca-certificates");
            //tCommandList.Add("apt-get install -y postfix");
            //tCommandList.Add("apt-get install -y mailutils");

            tCommandList.Add("echo \"<color=red> -> server create folder</color>\"");
            tCommandList.Add("mkdir /home/gitlab");
            tCommandList.Add("mkdir /home/gitlab/backups");

            tCommandList.Add("echo \"<color=red> -> gitlab download</color>\"");
            tCommandList.Add("curl -sS https://packages.gitlab.com/install/repositories/gitlab/gitlab-ce/script.deb.sh | sudo bash");
            tCommandList.Add("apt-get install gitlab-ce");

            tCommandList.Add("echo \"<color=red> -> gitlab configure</color>\"");
            string tConfigGitLabRB = "/etc/gitlab/gitlab.rb";
            tCommandList.Add("rm " + tConfigGitLabRB + "");
            tCommandList.Add("echo \"# NWD ServerOther Auto configuration\" >> " + tConfigGitLabRB);
            tCommandList.Add("echo \"external_url 'https://" + GitLabDomainNameServer + "'\" >> " + tConfigGitLabRB);
            tCommandList.Add("echo \"# letencrypt\" >> " + tConfigGitLabRB);
            tCommandList.Add("echo \"letsencrypt['enable'] = true\" >> " + tConfigGitLabRB);
            tCommandList.Add("echo \"letsencrypt['contact_emails'] = ['" + GitLabEmail + "']\" >> " + tConfigGitLabRB);

            if (GitLabAWS())
            {
                tCommandList.Add("echo \"# backup on AWS S3\" >> " + tConfigGitLabRB);
                tCommandList.Add("echo \"gitlab_rails['backup_path'] = \\\"/home/gitlab/backups\\\"\" >> " + tConfigGitLabRB);
                tCommandList.Add("echo \"gitlab_rails['backup_archive_permissions'] = 0644\" >> " + tConfigGitLabRB);
                tCommandList.Add("echo \"gitlab_rails['backup_keep_time'] = 604800\" >> " + tConfigGitLabRB);
                tCommandList.Add("echo \"gitlab_rails['backup_upload_connection'] = {\" >> " + tConfigGitLabRB);
                tCommandList.Add("echo \" 'provider' => 'AWS',\" >> " + tConfigGitLabRB);
                tCommandList.Add("echo \" 'region' => '" + GitLabAWSRegion + "',\" >> " + tConfigGitLabRB);
                tCommandList.Add("echo \" 'aws_access_key_id' => '" + GitLabAWSAccessKeyID.Decrypt() + "',\" >> " + tConfigGitLabRB);
                tCommandList.Add("echo \" 'aws_secret_access_key' => '" + GitLabAWSSecretAccessKey.Decrypt() + "',\" >> " + tConfigGitLabRB);
                tCommandList.Add("echo \"}\" >> " + tConfigGitLabRB);
                tCommandList.Add("echo \"gitlab_rails['backup_upload_remote_directory'] = '" + GitLabAWSDirectory + "'\" >> " + tConfigGitLabRB);
            }

            tCommandList.Add("echo \"<color=red> -> gitlab reconfigure</color>\"");
            tCommandList.Add("gitlab-ctl reconfigure");
            tCommandList.Add("gitlab-ctl renew-le-certs");
            tCommandList.Add("gitlab-ctl start");

            if (GitLabAWS())
            {
                tCommandList.Add("echo \"<color=red> -> install amazon S3 </color>\"");

                // line to add in source
                {
                    string tLineSource = "deb http://http.debian.net/debian buster-backports main";
                    //tCommandList.Add("sed -i '/(" + Regex.Escape(tLineSource).Replace("/", "\\/").Replace("\\ ", " ") + ")/d' /etc/apt/sources.list"); // first remove if exists with full pattern ... not working :-/
                    tCommandList.Add(" sed -i '/buster\\-backports/d' /etc/apt/sources.list");// first remove if exists
                    tCommandList.Add("echo \"" + tLineSource + "\" >> /etc/apt/sources.list"); // j'aime pas mais pas le choix ...
                }

                tCommandList.Add("apt-get update");
                tCommandList.Add("apt-get install -y s3cmd");

                tCommandList.Add("export AWS_ACCESS_KEY_ID=" + GitLabAWSAccessKeyID.Decrypt() + "");
                tCommandList.Add("export AWS_SECRET_ACCESS_KEY=" + GitLabAWSSecretAccessKey.Decrypt() + "");
                tCommandList.Add("export AWS_REGION=" + GitLabAWSRegion + "");
                tCommandList.Add("export AWS_BUCKET=" + GitLabAWSDirectory + "");
                tCommandList.Add("export AWS_PASSWORD=" + GitLabAWSPassword.Decrypt() + "");
                tCommandList.Add("echo \"" +
                    "${AWS_ACCESS_KEY_ID?}\n" +
                    "${AWS_SECRET_ACCESS_KEY?}\n" +
                    "${AWS_REGION}\n" +
                    "\n" +
                    "\n" +
                    "${AWS_PASSWORD}\n" +
                    "\n" +
                    "\n" +
                    "\n" +
                    "y\n" +
                    "y\n\" | s3cmd --configure");

                                // add script file to save some other files
                                string tConfigGitLabBackup = "/etc/gitlab_backup.sh";
                                List<string> tFile = new List<string>();
                                tFile.Add("#!/bin/bash");
                                tFile.Add("");

                                tFile.Add("# purge old backup files");
                                tFile.Add("echo \"<color=red> -> purge old backup files </color>\"");
                                tFile.Add("find /var/opt/gitlab/backups/*.tar -mmin +360 -exec rm {} \\;");
                                tFile.Add("");

                                tFile.Add("# param timestamp");
                                tFile.Add("echo \"<color=red> -> param timestamp </color>\"");
                                tFile.Add("actualtimestamp=$(date '+%s')");
                                tFile.Add("echo \"<color=red> ->actualtimestamp = ${actualtimestamp} </color>\"");
                                tFile.Add("");

                                tFile.Add("# create gitlab backup");
                                tFile.Add("echo \"<color=red> -> create gitlab backup </color>\"");
                                tFile.Add("gitlab-backup create");
                                tFile.Add("");

                                tFile.Add("# create tar for configuration file, keys file and script itself");
                                tFile.Add("echo \"<color=red> -> create tar for configuration file, keys file and script itself </color>\"");
                                tFile.Add("tar -cvf \"/home/gitlab/backups/${actualtimestamp}_gitlab_rb.tar\" /etc/gitlab/gitlab.rb");
                                tFile.Add("tar -cvf \"/home/gitlab/backups/${actualtimestamp}_gitlab_secrets_json.tar\" /etc/gitlab/gitlab-secrets.json");
                                tFile.Add("tar -cvf \"/home/gitlab/backups/${actualtimestamp}_gitlab_backup_sh.tar\" " + tConfigGitLabBackup + "");
                                tFile.Add("");

                                tFile.Add("# amazon upload");
                                tFile.Add("echo \"<color=red> -> amazon upload </color>\"");
                                tFile.Add("s3cmd put \"/home/gitlab/backups/${actualtimestamp}_gitlab_rb.tar\" s3://" + GitLabAWSDirectory + "");
                                tFile.Add("s3cmd put \"/home/gitlab/backups/${actualtimestamp}_gitlab_secrets_json.tar\" s3://" + GitLabAWSDirectory + "");
                                tFile.Add("s3cmd put \"/home/gitlab/backups/${actualtimestamp}_gitlab_backup_sh.tar\" s3://" + GitLabAWSDirectory + "");
                                tFile.Add("");

                                tFile.Add("# update your server apt");
                                tFile.Add("echo \"<color=red> -> update your server apt </color>\"");
                                tFile.Add("sudo apt-get update");
                                tFile.Add("sudo apt-get -y dist-upgrade");
                                tFile.Add("");

                                tFile.Add("# update gitlab cert");
                                tFile.Add("echo \"<color=red> -> update gitlab cert </color>\"");
                                tFile.Add("sudo gitlab-ctl renew-le-certs");
                                tFile.Add("");

                                tFile.Add("# gitlab restart");
                                tFile.Add("echo \"<color=red> -> gitlkab restart </color>\"");
                                tFile.Add("sudo gitlab-ctl restart");
                                tFile.Add("");

                                tCommandList.Add("rm -f " + tConfigGitLabBackup + "");
                                foreach (string tLine in tFile)
                                {
                                    tCommandList.Add("echo '" + tLine.Replace("\'", "\'\\\'\'") + "' >> " + tConfigGitLabBackup); // special escape quote (see https://unix.stackexchange.com/questions/30903/how-to-escape-quotes-in-shell) need use '\'' to escape '
                                }
                                tCommandList.Add("chmod +x " + tConfigGitLabBackup + "");



                                tFile.Add("echo \"<color=red> -> Cron </color>\"");
                                tCommandList.Add("crontab -l > root_cron");
                                tCommandList.Add(" sed -i '/gitlab\\_backup/d' root_cron");// first remove if exists
                                tCommandList.Add("echo \"0 1 * * * " + tConfigGitLabBackup + "\" >> root_cron"); // j'aime pas mais pas le choix ...
                                tCommandList.Add("crontab root_cron");
                                tCommandList.Add("rm root_cron");
                                tCommandList.Add("crontab -l");

                                // run backup first time
                                tCommandList.Add(tConfigGitLabBackup);
                            }
                            return tCommandList;
                        }*/
        //-------------------------------------------------------------------------------------------------------------
        /*List<string> RestoreGitLabCommand()
        {
            List<string> tCommandList = new List<string>();
            if (GitLabAWS())
            {
                tCommandList.Add("echo \"<color=red> -> restaure GitLab </color>\"");
                tCommandList.Add("mkdir /home/gitlab/restaure");

                tCommandList.Add("s3cmd get s3://" + GitLabAWSDirectory + "/" + GitLabBackupTimeStamp + "_gitlab_rb.tar /home/gitlab/restaure/gitlab_rb.tar");
                tCommandList.Add("s3cmd get s3://" + GitLabAWSDirectory + "/" + GitLabBackupTimeStamp + "_gitlab_secrets_json.tar /home/gitlab/restaure/gitlab_secrets_json.tar");
                tCommandList.Add("s3cmd get s3://" + GitLabAWSDirectory + "/" + GitLabBackupTimeStamp + "_gitlab_backup_sh.tar /home/gitlab/restaure/gitlab_backup_sh.tar");

                tCommandList.Add("rm /etc/gitlab/gitlab.rb");
                tCommandList.Add("rm /etc/gitlab/gitlab-secrets.json");
                tCommandList.Add("rm /etc/gitlab_backup.sh");

                tCommandList.Add("cd /home/gitlab/restaure/");

                tCommandList.Add("tar xvf /home/gitlab/restaure/gitlab_rb.tar");
                tCommandList.Add("tar xvf /home/gitlab/restaure/gitlab_secrets_json.tar");
                tCommandList.Add("tar xvf /home/gitlab/restaure/gitlab_backup_sh.tar");

                tCommandList.Add("cp /home/gitlab/restaure/etc/gitlab/gitlab.rb /etc/gitlab/gitlab.rb");
                tCommandList.Add("cp /home/gitlab/restaure/etc/gitlab/gitlab-secrets.json /etc/gitlab/gitlab-secrets.json");
                tCommandList.Add("cp /home/gitlab/restaure/etc/gitlab_backup.sh /etc/gitlab_backup.sh");


                tCommandList.Add("s3cmd get s3://" + GitLabAWSDirectory + "/" + GitLabBackupFile + " /home/gitlab/backups/" + GitLabBackupFile + "");
                tCommandList.Add("chown git.git /home/gitlab/backups/" + GitLabBackupFile + "");

                tCommandList.Add("gitlab-ctl stop unicorn");
                tCommandList.Add("gitlab-ctl stop puma");
                tCommandList.Add("gitlab-ctl stop sidekiq");

                tCommandList.Add("gitlab-backup restore BACKUP=" + GitLabBackupFile.Replace("_gitlab_backup.tar", "") + " force=yes");

                tCommandList.Add("gitlab-ctl stop");
                tCommandList.Add("gitlab-ctl reconfigure");
                tCommandList.Add("gitlab-ctl renew-le-certs");
                tCommandList.Add("gitlab-ctl start");
                tCommandList.Add("gitlab-ctl status");

                tCommandList.Add("rm -R /home/gitlab/restaure");

                tCommandList.Add("rm /var/opt/gitlab/backups/" + GitLabBackupFile + "");
            }
            return tCommandList;
        }*/
        //-------------------------------------------------------------------------------------------------------------
        /*void InstallGitLab()
        {
            ExecuteSSH("Install GitLab", InstallGitLabCommand());
        }*/
        //-------------------------------------------------------------------------------------------------------------
        /* void SaveGitLab()
         {
             List<string> tCommandList = new List<string>();
             tCommandList.Add("echo \"<color=red> -> save GitLab </color>\"");
             string tConfigGitLabBackup = "/etc/gitlab_backup.sh";
             tCommandList.Add(tConfigGitLabBackup);
             ExecuteSSH("Save GitLab", tCommandList);
         }*/
        //-------------------------------------------------------------------------------------------------------------
        /*  void RestoreGitLab()
          {
              ExecuteSSH("Restore GitLab", RestoreGitLabCommand());
          }*/
        //-------------------------------------------------------------------------------------------------------------
        /* void InstallAndRestoreGitLab()
         {
             List<string> tCommandList = new List<string>();
             tCommandList.AddRange(InstallGitLabCommand());
             tCommandList.AddRange(RestoreGitLabCommand());
             ExecuteSSH("Install and Restore GitLab", tCommandList);
         }*/
        //-------------------------------------------------------------------------------------------------------------
        /* void RestartGitLab()
         {
             List<string> tCommandList = new List<string>();
             tCommandList.Add("echo \"<color=red> -> gitlab restart</color>\"");
             tCommandList.Add("gitlab-ctl stop");
             tCommandList.Add("gitlab-ctl renew-le-certs");
             tCommandList.Add("gitlab-ctl start");
             tCommandList.Add("gitlab-ctl status");
             ExecuteSSH("Restart GitLab", tCommandList);
         }
         */
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
