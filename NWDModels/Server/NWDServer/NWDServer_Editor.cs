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
using System.Text.RegularExpressions;
using System.Threading;
using System.Collections;
using UnityEngine;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
using Renci.SshNet;
//=====================================================================================================================
namespace NetWorkedData
{
    // doc to read to finish script : https://www.cyberciti.biz/tips/how-do-i-enable-remote-access-to-mysql-database-server.html
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServer : NWDBasisUnsynchronize
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_GITLAB_URL_MASTER = "gitlab.hephaiscode.com/Server/AutoSaveInstallSH/raw/master/";
        const string K_OUTPUT_FOLDER = "/etc/hephaiscode/";
        const string K_SERVER_INSTALL = "server_installer_";
        const string K_WEBSERVICE_ADD = "webservice_add_";
        const string K_WEBSERVICE_REMOVE = "webservice_remove_";
        //-------------------------------------------------------------------------------------------------------------
        private List<string> UpdateCommand(NWDServerDistribution sDistribution)
        {
            return new List<string>()
                  {
            "apt-get check",
            "apt-get update",
            "apt-get -y autoremove",
            "apt-get autoclean",
            "apt-get -y upgrade",
            "apt-get -y dist-upgrade",
            "apt-get -y autoremove",
            "apt-get autoclean",
            "apt-get clean",
                  };
        }
        //-------------------------------------------------------------------------------------------------------------
        private List<string> InstallCommand(NWDServerDistribution sDistribution)
        {
            return new List<string>()
                  {
            "apt-get update", // do update
            "apt-get -y dist-upgrade", // do update
            "apt-get -y install ntp", // install system network time protocol daemon
            "apt-get -y install fail2ban", // install fail to ban to limit attack on ssh for example
            "apt-get -y install vim", // install vim editor
            "apt-get -y install whois", // install who is package
            "apt-get -y install debconf-utils", // install debian tools

            "apt-get install locales",
            "export LANGUAGE=en",
            "export LC_CTYPE=en_US.UTF-8",
            "export LC_MESSAGES=en_US.UTF-8",
            "export LC_ALL=en_US.UTF-8",
            "locale-gen --purge en_US.UTF-8",
                  };
        }
        //-------------------------------------------------------------------------------------------------------------
        public const string K_SFTP_chroot = "sftp_chroot";
        //-------------------------------------------------------------------------------------------------------------
        public override float AddonEditorHeight(float sWidth)
        {
            return LayoutEditorHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public string TextCommandResult;
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEditor(Rect sRect)
        {
            //-----------------
            GUIContent tButtonTitle = null;
            //-----------------
            base.AddonEditor(sRect);
            if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerate))
            {
                NWDGUILayout.Separator();
                EditorGUILayout.LabelField("Continents", Continent.ConcatRepresentations());
            }
            else
            {
                if (GUILayout.Button("Need credentials for actions"))
                {
                    NWDProjectCredentialsManager.SharedInstanceFocus();
                }
            }
            // find ip of server by dns if associated
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(DomainNameServer) == true);
            if (GUILayout.Button("Find IP from Server (NWDServerDomain)"))
            {
                string tLocalIP = "0.0.0.0";
                foreach (IPAddress tIP in Dns.GetHostAddresses(DomainNameServer))
                {
                    if (tIP.AddressFamily == AddressFamily.InterNetwork)
                    {
                        tLocalIP = tIP.ToString();
                    }
                }
                IP.SetValue(tLocalIP);
            }
            EditorGUI.EndDisabledGroup();
            NWDGUILayout.Separator();
            //-----------------
            EditorGUILayout.HelpBox("Don't forgot to check your ~/.ssh/known_hosts file permission!", MessageType.Warning);
            string tcommandKeyGen = "ssh-keygen -R " + IP.GetValue() + " & ssh-keygen -R " + IP.GetValue() + ":" + Port + " & ssh " + IP.GetValue() + " -l " + Root_User + " -p " + Port;
            if (AdminInstalled)
            {
                tcommandKeyGen = "ssh-keygen -R " + IP.GetValue() + " & ssh -keygen -R " + IP.GetValue() + ":" + Port + " & ssh " + IP.GetValue() + " -l " + Admin_User + " -p " + Port;
            }
            tButtonTitle = new GUIContent("local ssh-keygen -R", tcommandKeyGen);
            GUILayout.TextField(tcommandKeyGen);
            NWDGUILayout.Separator();
            //-----------------
            tButtonTitle = new GUIContent("Test security of SSH", "test security for this SSH config");
            if (GUILayout.Button(tButtonTitle))
            {
                Application.OpenURL("https://sshcheck.com/server/" + IP.GetValue() + "/" + Port.ToString() + "");
            }
            //-----------------
            tButtonTitle = new GUIContent("Try connexion root", " try connexion with root");
            if (GUILayout.Button(tButtonTitle))
            {
                ExecuteSSH(tButtonTitle.text, new List<string>()
                {
                    "mkdir TestFolderForRoot",
                    "chown " + Root_User + ":" + Root_User + " TestFolderForRoot", // change owner for dir of Root_User
                    "chmod 770 TestFolderForRoot",   // change right for dir of Root_User
                    "ls",
                }, null, Port, Root_User, Root_Secure_Password.Decrypt());
            }
            //-----------------
            EditorGUI.BeginDisabledGroup(AdminInstalled == false);
            tButtonTitle = new GUIContent("Try connexion admin", "Try connexion with admin and admin_password");
            if (GUILayout.Button(tButtonTitle))
            {
                ExecuteSSH(this, tButtonTitle.text, new List<string>()
                {
                    "ls",
                }, null, Port, Admin_User, Admin_Secure_Password.Decrypt());
            }
            EditorGUI.EndDisabledGroup();
            //-----------------
            tButtonTitle = new GUIContent("Try connexion ", "Try connexion with admin or root ");
            if (GUILayout.Button(tButtonTitle))
            {
                ExecuteSSH(this, tButtonTitle.text, new List<string>()
                {
                    "ls",
                });
            }
            //-----------------
            tButtonTitle = new GUIContent("change password root", "change password for root");
            if (GUILayout.Button(tButtonTitle))
            {
                string tNewPassword = NWDToolbox.RandomStringCypher(24);
                Debug.Log("tNewPassword : " + tNewPassword);
                ExecuteSSH(this, tButtonTitle.text, new List<string>()
                {
                        "echo " + Root_User + ":" + tNewPassword + " | chpasswd", // change the password for the Admin
                },
                           delegate (string sCommand, string sResult)
                           {
                               Debug.Log("tNewPassword : " + tNewPassword + " changed!");
                               Root_Secure_Password.CryptAes(tNewPassword);
                               UpdateDataIfModified();
                           });
            }
            //-----------------
            EditorGUI.BeginDisabledGroup(AdminInstalled == false);
            tButtonTitle = new GUIContent("change password admin", "change password for admin");
            if (GUILayout.Button(tButtonTitle))
            {
                string tNewPassword = NWDToolbox.RandomStringCypher(24);
                Debug.Log("tNewPassword : " + tNewPassword);
                ExecuteSSH(this, tButtonTitle.text, new List<string>()
                {
                        "echo " + Admin_User + ":" + tNewPassword + " | chpasswd", // change the password for the Admin
                },
                       delegate (string sCommand, string sResult)
                       {
                           Debug.Log("tNewPassword : " + tNewPassword + " changed!");
                           Admin_Secure_Password.CryptAes(tNewPassword);
                           UpdateDataIfModified();
                       });
            }
            EditorGUI.EndDisabledGroup();
            //-----------------
            EditorGUI.BeginDisabledGroup(Port == FuturPort);
            tButtonTitle = new GUIContent("Change Port", "change the port from " + Port + " to " + FuturPort + "");
            if (GUILayout.Button(tButtonTitle))
            {
                ExecuteSSH(this, tButtonTitle.text, new List<string>()
                {
                    "sed -i 's/^.*Port .*$/Port " + FuturPort + "/g' /etc/ssh/sshd_config",
                    "service sshd restart",
                },
                           delegate (string sCommand, string sResult)
                           {
                               if (sCommand == "service sshd restart")
                               {
                                   PortChanged = true;
                                   Port = FuturPort;
                                   UpdateDataIfModified();
                               };
                           });
            }
            EditorGUI.EndDisabledGroup();
            //-----------------
            tButtonTitle = new GUIContent("Update server", "just run updates from apt-get xxx");
            if (GUILayout.Button(tButtonTitle))
            {
                ExecuteSSH(this, tButtonTitle.text, UpdateCommand(Distribution));
            }
            //-----------------
            tButtonTitle = new GUIContent("Install server", "Install essential on server");
            if (GUILayout.Button(tButtonTitle))
            {
                ExecuteSSH(this, tButtonTitle.text, new List<string>()
                {
                    "apt-get update", // do update
                    "apt-get -y dist-upgrade", // do update

                    "apt-get -y install ntp", // install system network time protocol daemon
                    //"systemctl status ntp", // check sync with world's time
                    
                    "apt-get -y install fail2ban", // install fail to ban to limit attack on ssh for example

                    "apt-get -y install vim", // install vim editor
                    "apt-get -y install whois", // install who is package
                    "apt-get -y install debconf-utils", // install debian tools

                    "apt-get install locales",
                    "export LANGUAGE=en",
                    "export LC_CTYPE=en_US.UTF-8",
                    "export LC_MESSAGES=en_US.UTF-8",
                    "export LC_ALL=en_US.UTF-8",
                    "locale-gen --purge en_US.UTF-8",
            });
            }
            //-----------------
            EditorGUI.BeginDisabledGroup(AdminInstalled == true);
            tButtonTitle = new GUIContent("Install admin", "Install admin with admin_password");
            if (GUILayout.Button(tButtonTitle))
            {
                ExecuteSSH(this, tButtonTitle.text, new List<string>()
                {
                "useradd --shell /bin/bash " + Admin_User + "", // create user for Admin
                "echo " + Admin_User + ":" + Admin_Secure_Password.Decrypt() + " | chpasswd", // change the password for the Admin
                //"usermod -aG sudo " + Admin_User + "", // give sudo to the admin ... TODO : is it secure? // anayway the admin will be refuse in ssh connexion :-(
                "mkdir /home/" + Admin_User + "", // make dir for Admin
                "chown " + Admin_User + ":" + Admin_User + " /home/" + Admin_User + "", // change owner for dir of Admin
                "chmod 770 /home/" + Admin_User + "",   // change right for dir of Admin
                "mkdir /home/" + Admin_User + "/TestFolderForAdmin",
                "cut -d: -f1 /etc/passwd", // verif admin user
                "service sshd restart", // restart ssh
            },
                           delegate (string sCommand, string sResult)
                           {
                               if (sCommand == "service sshd restart")
                               {
                                   AdminInstalled = true;
                                   UpdateDataIfModified();
                               };
                           });
            }
            EditorGUI.EndDisabledGroup();
            //-----------------
            EditorGUI.BeginDisabledGroup(AdminInstalled == false || RootForbidden == true);
            tButtonTitle = new GUIContent("forbidden root", "after that you will never log as root! log as admin and su.");
            if (GUILayout.Button(tButtonTitle))
            {
                if (AdminInstalled)
                {
                    ExecuteSSH(this, tButtonTitle.text, new List<string>()
                {
                "echo \" -> forbidden root access\"",
                "sed -i 's/^.*PermitRootLogin .*$//g' /etc/ssh/sshd_config", //
                "sed -i '$ a PermitRootLogin no' /etc/ssh/sshd_config",
                "service sshd restart",
            },
                           delegate (string sCommand, string sResult)
                           {
                               if (sCommand == "service sshd restart")
                               {
                                   RootForbidden = true;
                                   UpdateDataIfModified();
                               };
                           });
                }
            }
            EditorGUI.EndDisabledGroup();
            //-----------------
            EditorGUI.BeginDisabledGroup(RootForbidden == false);
            tButtonTitle = new GUIContent("authorize root", "after that you will log as root!");
            if (GUILayout.Button(tButtonTitle))
            {
                if (AdminInstalled)
                {
                    ExecuteSSH(this, tButtonTitle.text, new List<string>()
                {
                "echo \" -> authorize root access\"",
                "sed -i 's/^.*PermitRootLogin .*$//g' /etc/ssh/sshd_config", //
                "sed -i '$ a PermitRootLogin yes' /etc/ssh/sshd_config",
                "service sshd restart",
            },
                           delegate (string sCommand, string sResult)
                           {
                               if (sCommand == "service sshd restart")
                               {
                                   RootForbidden = false;
                                   UpdateDataIfModified();
                               };
                           });
                }
            }
            EditorGUI.EndDisabledGroup();
            //-----------------
            tButtonTitle = new GUIContent("Config ssh", "secure ssh config");
            if (GUILayout.Button(tButtonTitle))
            {
                // if (AdminInstalled)
                {
                    ExecuteSSH(this, tButtonTitle.text, new List<string>()
                {
                "sed -i 's/^ciphers .*$//g' /etc/ssh/sshd_config", //
                "sed -i '$ a ciphers " +
                "aes128-ctr," +
                "aes192-ctr," +
                "aes256-ctr" +
                //"aes128-cbc" +
                "' /etc/ssh/sshd_config",

                "sed -i 's/^macs .*$//g' /etc/ssh/sshd_config", //
                "sed -i '$ a macs " +
                "hmac-sha2-256," +
                "hmac-sha2-512" +
                "' /etc/ssh/sshd_config",

                "sed -i 's/^kexalgorithms .*$//g' /etc/ssh/sshd_config", //
                "sed -i '$ a kexalgorithms " +
                //"diffie-hellman-group14-sha256," +
                "diffie-hellman-group16-sha512," +
                "diffie-hellman-group18-sha512," +
                "diffie-hellman-group-exchange-sha256," +
                "curve25519-sha256," +
                "curve25519-sha256@libssh.org" +
                "' /etc/ssh/sshd_config",


                "sed -i 's/^HostKeyAlgorithms .*$//g' /etc/ssh/sshd_config", //
                "sed -i '$ a HostKeyAlgorithms " +
                "ssh-ed25519," +
                "rsa-sha2-256," +
                "rsa-sha2-512" +
                "' /etc/ssh/sshd_config",


                "addgroup "+K_SFTP_chroot+"", // Add group

                "sed -i 's/^.*Match Group .*$//g' /etc/ssh/sshd_config",
                "sed -i '$ a Match Group "+K_SFTP_chroot+"' /etc/ssh/sshd_config",

                "sed -i 's/^.*ChrootDirectory .*$//g' /etc/ssh/sshd_config",
                "sed -i '$ a    ChrootDirectory /home/%u' /etc/ssh/sshd_config",

                "sed -i 's/^.*ForceCommand .*$//g' /etc/ssh/sshd_config",
                "sed -i '$ a    ForceCommand internal-sftp' /etc/ssh/sshd_config",

                "sed -i 's/^.*AllowTcpForwarding .*$//g' /etc/ssh/sshd_config",
                "sed -i '$ a    AllowTcpForwarding no' /etc/ssh/sshd_config",


                "service sshd restart",
            },
                        delegate (string sCommand, string sResult)
                        {
                            if (sCommand == "service sshd restart")
                            {
                                UpdateDataIfModified();
                            };
                        }
                            );
                }
            }
            //-----------------
            tButtonTitle = new GUIContent("show infos", "run hostnamectl");
            if (GUILayout.Button(tButtonTitle))
            {
                ExecuteSSH(this, tButtonTitle.text, new List<string>() { "hostnamectl", });
            }
            //-----------------
            tButtonTitle = new GUIContent("Statut server", "just run df -h on server");
            if (GUILayout.Button(tButtonTitle))
            {
                ExecuteSSH(this, tButtonTitle.text, new List<string>() { "df -h", });
            }
            //-----------------
            tButtonTitle = new GUIContent("Reboot server (5s)", "Reboot server after 5 seconds");
            if (GUILayout.Button(tButtonTitle))
            {
                ExecuteSSH(this, tButtonTitle.text, new List<string>() { "shutdown -r -t 5", });
            }
            //-----------------
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ExecuteSSH(string sScriptTitle, List<string> sCommandList, NWDSSHCommandBlock CommandResultDelegate = null, int sAltPORT = -1, string sAltUser = null, string sAltPassword = null)
        {
            ExecuteSSH(this, sScriptTitle, sCommandList, CommandResultDelegate, sAltPORT, sAltUser, sAltPassword);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ExecuteSSH(NWDServer sServer, string sScriptTitle, List<string> sCommandList, NWDSSHCommandBlock CommandResultDelegate = null, int sAltPORT = -1, string sAltUser = null, string sAltPassword = null)
        {
            NWDSSHWindow.ExecuteSSH(sServer, sScriptTitle, sCommandList, CommandResultDelegate, sAltPORT, sAltUser, sAltPassword);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Execute(NWDServer sServer, string tScriptTitle, List<string> sCommandList, NWDSSHCommandBlock sCommandResultDelegate, int sAltPORT = -1, string sAltUser = null, string sAltPassword = null)
        {
            NWDSSHContent.SharedInstance().Execute(sServer, tScriptTitle, sCommandList, sCommandResultDelegate, sAltPORT, sAltUser, sAltPassword);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif