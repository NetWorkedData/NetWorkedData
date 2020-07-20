//=====================================================================================================================
//
//  ideMobi 2020©
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
using Renci.SshNet;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;

//=====================================================================================================================
namespace NetWorkedData
{
    // doc to read to finish script : https://www.cyberciti.biz/tips/how-do-i-enable-remote-access-to-mysql-database-server.html

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServerOther : NWDBasisUnsynchronize
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
            if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerate))
            {
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Credentials window"))
                {
                    NWDProjectCredentialsManager.SharedInstanceFocus();
                }
                tI++;
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Flush credentials"))
                {
                    NWDProjectCredentialsManager.FlushCredentials(NWDCredentialsRequired.ForSFTPGenerate);
                }
                tI++;
                NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                tI++;
                GUIContent tButtonTitle = null;
                NWDServer tServer = Server.GetRawData();
                if (tServer != null)
                {
                    //-----------------
                    EditorGUI.HelpBox(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 1]), "Don't forgot to check your ~/.ssh/known_hosts file permission!", MessageType.Warning);
                    tI += 2;
                    //tButtonTitle = new GUIContent("Open terminal", " open terminal or console on your desktop");
                    //if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                    //{
                    //    // /Applications/Utilities/Terminal.app/Contents/MacOS/Terminal
                    //    FileInfo tFileInfo = new FileInfo("/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal");
                    //    System.Diagnostics.Process.Start(tFileInfo.FullName);
                    //}
                    //tI++;
                    //string tcommandKeyGen = "ssh-keygen -R " + tServer.IP.GetValue() + ":" + tServer.Port + " & ssh " + tServer.IP.GetValue() + " -l " + tServer.Root_User + " -p " + tServer.Port;
                    //if (tServer.AdminInstalled)
                    //{
                    //    tcommandKeyGen = "ssh-keygen -R " + tServer.IP.GetValue() + ":" + tServer.Port + " & ssh " + tServer.IP.GetValue() + " -l " + tServer.Admin_User + " -p " + tServer.Port;
                    //}

                    string tcommandKeyGen = "ssh-keygen -R " + tServer.IP.GetValue() + " & ssh-keygen -R " + tServer.IP.GetValue() + ":" + tServer.Port + " & ssh " + tServer.IP.GetValue() + " -l " + tServer.Root_User + " -p " + tServer.Port;
                    if (tServer.AdminInstalled)
                    {
                        tcommandKeyGen = "ssh-keygen -R " + tServer.IP.GetValue() + " & ssh -keygen -R " + tServer.IP.GetValue() + ":" + tServer.Port + " & ssh " + tServer.IP.GetValue() + " -l " + tServer.Admin_User + " -p " + tServer.Port;
                    }


                    tButtonTitle = new GUIContent("local ssh-keygen -R", tcommandKeyGen);
                    if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                    {
                        NWDSSHWindow.ExecuteProcessTerminal(tcommandKeyGen);
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


                    if (ServerType == NWDServerOtherType.WebDAV)
                    {
                        string tServerDNS = tServer.DomainNameServer;


                        //-----------------
                        //EditorGUI.BeginDisabledGroup(UserInstalled == false);
                        tButtonTitle = new GUIContent("Install User webdav", "Install webdav");
                        if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                        {
                            if (string.IsNullOrEmpty(tServerDNS) == false )
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
                        tI++;
                        //-----------------




                        //-----------------
                        //EditorGUI.BeginDisabledGroup(UserInstalled == false);
                        tButtonTitle = new GUIContent("ls User webdav", "ls webdav");
                        if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
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
                        tI++;
                        //-----------------

                    }

                    //-----------------

                    if (ServerType == NWDServerOtherType.GitLab)
                    {
                        //-----------------
                        tButtonTitle = new GUIContent("Install GitLab", "Install GitLab");
                        if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
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
                        tI++;
                    }

                    if (ServerType == NWDServerOtherType.WebServer)
                    {
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
                    }
                    //-----------------

                    //GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), tServer.TextCommandResult);
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