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
            float tYadd = NWDGUI.AreaHeight(NWDGUI.kMiniButtonStyle.fixedHeight, 100);
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public string TextCommandResult;
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonEditor(Rect sRect)
        {
            Rect[,] tMatrix = NWDGUI.DiviseArea(sRect, 2, 100);
            int tI = 0;

            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
            tI++;

            EditorGUI.LabelField(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Continent", Continent.ConcatRepresentations());
            tI++;

            if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerate))
            {
                // find ip of server by dns if associated
                EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(DomainNameServer) == true);
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Find IP from Server (NWDServerDomain)"))
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
                tI++;

                NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                tI++;

                GUIContent tButtonTitle = null;

                //-----------------
                EditorGUI.HelpBox(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 1]), "Don't forgot to check your ~/.ssh/known_hosts file permission!", MessageType.Warning);
                tI += 2;
                //tButtonTitle = new GUIContent("Open terminal", " open terminal or console on your desktop");
                //if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                //{
                //    // /Applications/Utilities/Terminal.app/Contents/MacOS/Terminal
                //    //FileInfo tFileInfo = new FileInfo("/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal");
                //    //System.Diagnostics.Process.Start(tFileInfo.FullName);
                //    Application.OpenURL("/Applications/Utilities/");
                //}
                //tI++;
                string tcommandKeyGen = "ssh-keygen -R " + IP.GetValue() + " & ssh-keygen -R " + IP.GetValue() + ":" + Port + " & ssh " + IP.GetValue() + " -l " + Root_User + " -p " + Port;
                if (AdminInstalled)
                {
                    tcommandKeyGen = "ssh-keygen -R " + IP.GetValue() + " & ssh -keygen -R " + IP.GetValue() + ":" + Port + " & ssh " + IP.GetValue() + " -l " + Admin_User + " -p " + Port;
                }
                tButtonTitle = new GUIContent("local ssh-keygen -R", tcommandKeyGen);
                //if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                //{
                //    NWDSSHWindow.ExecuteProcessTerminal(tcommandKeyGen);
                //}
                //tI++;
                GUI.TextField(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 1]), tcommandKeyGen);
                tI += 2;
                NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                tI++;
                //-----------------
                tButtonTitle = new GUIContent("Test security of SSH", "test security for this SSH config");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    Application.OpenURL("https://sshcheck.com/server/" + IP.GetValue() + "/" + Port.ToString() + "");
                }
                tI++;
                tButtonTitle = new GUIContent("Try connexion root", " try connexion with root");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    ExecuteSSH(tButtonTitle.text, new List<string>()
                {
                    //"mkdir /home/" + Root_User + "", // make dir for Root_User
                    //"chown " + Root_User + ":" + Root_User + " /home/" + Root_User + "", // change owner for dir of Root_User
                    //"chmod 770 /home/" + Root_User + "",   // change right for dir of Root_User

                    "mkdir TestFolderForRoot",
                    "chown " + Root_User + ":" + Root_User + " TestFolderForRoot", // change owner for dir of Root_User
                    "chmod 770 TestFolderForRoot",   // change right for dir of Root_User

                    "ls",
                }, null, Port, Root_User, Root_Secure_Password.Decrypt());
                }
                tI++;
                //-----------------
                EditorGUI.BeginDisabledGroup(AdminInstalled == false);
                tButtonTitle = new GUIContent("Try connexion admin", "Try connexion with admin and admin_password");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    ExecuteSSH(this, tButtonTitle.text, new List<string>()
                {
                    "ls",
                }, null, Port, Admin_User, Admin_Secure_Password.Decrypt());
                }
                EditorGUI.EndDisabledGroup();
                tI++;
                //-----------------
                tButtonTitle = new GUIContent("Try connexion ", "Try connexion with admin or root ");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    ExecuteSSH(this, tButtonTitle.text, new List<string>()
                {
                    "ls",
                });
                }
                tI++;

                //-----------------
                tButtonTitle = new GUIContent("change password root", "change password for root");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
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
                tI++;

                //-----------------
                EditorGUI.BeginDisabledGroup(AdminInstalled == false);
                tButtonTitle = new GUIContent("change password admin", "change password for admin");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
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
                tI++;

                //-----------------
                EditorGUI.BeginDisabledGroup(Port == FuturPort);
                tButtonTitle = new GUIContent("Change Port", "change the port from " + Port + " to " + FuturPort + "");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
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
                tI++;

                //-----------------
                tButtonTitle = new GUIContent("Update server", "just run updates from apt-get xxx");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    ExecuteSSH(this, tButtonTitle.text, UpdateCommand(Distribution));
                }
                tI++;

                //-----------------
                tButtonTitle = new GUIContent("Install server", "Install essential on server");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
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
                tI++;


                //-----------------
                EditorGUI.BeginDisabledGroup(AdminInstalled == true);
                tButtonTitle = new GUIContent("Install admin", "Install admin with admin_password");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
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
                tI++;


                //-----------------
                EditorGUI.BeginDisabledGroup(AdminInstalled == false || RootForbidden == true);
                tButtonTitle = new GUIContent("forbidden root", "after that you will never log as root! log as admin and su.");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
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
                tI++;

                //-----------------
                EditorGUI.BeginDisabledGroup(RootForbidden == false);
                tButtonTitle = new GUIContent("authorize root", "after that you will log as root!");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
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
                tI++;

                //-----------------
                tButtonTitle = new GUIContent("Config ssh", "secure ssh config");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
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
                tI++;

                ////-----------------
                //tButtonTitle = new GUIContent("Config sftp", "FTP via ssh config");
                //if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                //{
                //    if (AdminInstalled)
                //    {
                //        ExecuteSSH(this, tButtonTitle.text, new List<string>()
                //{
                //"addgroup "+K_SFTP_chroot+"", // Add group

                //"sed -i 's/^.*Match Group .*$//g' /etc/ssh/sshd_config",
                //"sed -i '$ a Match Group "+K_SFTP_chroot+"' /etc/ssh/sshd_config",

                //"sed -i 's/^.*ChrootDirectory .*$//g' /etc/ssh/sshd_config",
                //"sed -i '$ a    ChrootDirectory /home/%u' /etc/ssh/sshd_config",

                //"sed -i 's/^.*ForceCommand .*$//g' /etc/ssh/sshd_config",
                //"sed -i '$ a    ForceCommand internal-sftp' /etc/ssh/sshd_config",

                //"sed -i 's/^.*AllowTcpForwarding .*$//g' /etc/ssh/sshd_config",
                //"sed -i '$ a    AllowTcpForwarding no' /etc/ssh/sshd_config",
                //"service sshd restart",
                //    },
                //        delegate (string sCommand, string sResult)
                //        {
                //            if (sCommand == "service sshd restart")
                //            {
                //                UpdateDataIfModified();
                //            };
                //        });
                //    }
                //}
                //tI++;
                //-----------------
                tButtonTitle = new GUIContent("show infos", "run hostnamectl");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    ExecuteSSH(this, tButtonTitle.text, new List<string>() { "hostnamectl", });
                }
                tI++;

                //-----------------
                tButtonTitle = new GUIContent("Statut server", "just run df -h on server");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    ExecuteSSH(this, tButtonTitle.text, new List<string>() { "df -h", });
                }
                tI++;

                //-----------------
                tButtonTitle = new GUIContent("Reboot server (5s)", "Reboot server after 5 seconds");
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                {
                    ExecuteSSH(this, tButtonTitle.text, new List<string>() { "shutdown -r -t 5", });
                }
                tI++;



                ////-----------------
                //tButtonTitle = new GUIContent("Install SFTP", "install sftp and lock to user and admin");
                //if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                //{
                //    if (ExecuteSSH(this, new List<string>()
                //    {
                //    "echo \" -> install sftp_chroot group\"",
                //    "addgroup sftp_chroot",
                //    "sed -i '$ a # sftp_chroot start' /etc/ssh/sshd_config",
                //    "sed -i '$ a Match Group sftp_chroot' /etc/ssh/sshd_config",
                //    "sed -i '$ a    ChrootDirectory /home/%u' /etc/ssh/sshd_config",
                //    "sed -i '$ a    ForceCommand internal-sftp' /etc/ssh/sshd_config",
                //    "sed -i '$ a    AllowTcpForwarding no' /etc/ssh/sshd_config",
                //    //"sed -i '$ a X11Forwarding no' /etc/ssh/sshd_config",
                //    "sed -i '$ a # sftp_chroot end' /etc/ssh/sshd_config",
                //    "service sshd restart",


                //}) == true)
                //    {
                //        Debug.Log(tButtonTitle.text + " success");
                //    }
                //    else
                //    { Debug.Log(tButtonTitle.text + " fail"); }
                //}
                //tI++;

                ////-----------------
                //GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), TextCommandResult);
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
        public void ExecuteSSH(string sScriptTitle, List<string> sCommandList, NWDSSHCommandBlock CommandResultDelegate = null, int sAltPORT = -1, string sAltUser = null, string sAltPassword = null)
        {
            ExecuteSSH(this, sScriptTitle, sCommandList, CommandResultDelegate, sAltPORT, sAltUser, sAltPassword);
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void ExecuteSSH(List<string> sCommandList, NWDSSHCommandBlock CommandResultDelegate = null, int sAltPORT = -1, string sAltUser = null, string sAltPassword = null)
        //{
        //    ExecuteSSH(this, "Script unknow", sCommandList, CommandResultDelegate, sAltPORT, sAltUser, sAltPassword);
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static void ExecuteSSH(NWDServer sServer, string sScriptTitle, List<string> sCommandList, NWDSSHCommandBlock CommandResultDelegate = null, int sAltPORT = -1, string sAltUser = null, string sAltPassword = null)
        {
            NWDSSHWindow.ExecuteSSH(sServer, sScriptTitle, sCommandList, CommandResultDelegate, sAltPORT, sAltUser, sAltPassword);
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static bool ExecuteSSH(NWDServer sServer, List<string> sCommandList, int sAltPORT = -1, string sAltUser = null, string sAltPassword = null)
        //{
        //    NWDSSHWindow.SharedInstanceFocus();

        //    bool tNeedSu = false;
        //    string tUserEcho = "unknow";
        //    string tIP = sServer.IP.GetValue();
        //    int tPort = sServer.Port;
        //    string tUser = sServer.Root_User;
        //    string tPassword = sServer.Root_Password.GetValue();
        //    if (sServer.AdminInstalled && sServer.RootForbidden)
        //    {
        //        tUser = sServer.Admin_User;
        //        tPassword = sServer.Admin_Password.GetValue();
        //        tNeedSu = true;
        //    }
        //    if (sAltPORT != -1)
        //    {
        //        tPort = sAltPORT;
        //    }
        //    if (sAltUser != null)
        //    {
        //        tUser = sAltUser;
        //        tNeedSu = false;
        //    }
        //    if (sAltPassword != null)
        //    {
        //        tPassword = sAltPassword;
        //    }
        //    bool rReturn = true;
        //    StringBuilder rTextResult = new StringBuilder(); ;
        //    try
        //    {
        //        tUserEcho = tUser;
        //        rTextResult.AppendLine("#Local$ Connecting...");
        //        rTextResult.AppendLine("#Local$ ssh -l " + tUserEcho + " " + tIP + " -p " + tPort);
        //        rTextResult.AppendLine("#Local$ " + tPassword);
        //        PasswordConnectionInfo tConnectionInfo = new PasswordConnectionInfo(tIP, tPort, tUser, tPassword);
        //        using (SshClient tClientSSH = new SshClient(tConnectionInfo))
        //        {
        //            tClientSSH.Connect();
        //            if (tClientSSH.IsConnected == true)
        //            {
        //                ShellStream tShellStream = tClientSSH.CreateShellStream("xterm", 80, 24, 800, 600, 1024);

        //                rTextResult.AppendLine("--------------------");
        //                tUserEcho = RunCommand("whoami", tShellStream, rTextResult);
        //                RunCommand("ls", tShellStream, rTextResult);
        //                rTextResult.AppendLine("--------------------");
        //                if (tNeedSu == true)
        //                {
        //                    rTextResult.AppendLine("#Local$ try to swith from " + tUserEcho + " to " + sServer.Root_User + "");

        //                    // Get logged in and get user prompt
        //                    string prompt = tShellStream.Expect(new Regex(@"[$>]"));

        //                    // Send command and expect password or user prompt
        //                    tShellStream.WriteLine("su -l " + sServer.Root_User + "");
        //                    prompt = tShellStream.Expect(new Regex(@"([$#>:])"));

        //                    // Check to send password
        //                    if (prompt.Contains("assword:")) // research password: (or Password:) 
        //                    {
        //                        // Send password
        //                        rTextResult.AppendLine("#Local$ Good! I have the hand ... put password now!");
        //                        tShellStream.WriteLine(sServer.Root_Password.GetValue() + "\r");
        //                        prompt = tShellStream.Expect(new Regex(@"[$#>]"));
        //                    }
        //                    tUserEcho = RunCommand("whoami", tShellStream, rTextResult);
        //                    RunCommand("ls", tShellStream, rTextResult);
        //                    rTextResult.AppendLine("--------------------");
        //                    if (tUserEcho == sServer.Root_User)
        //                    {
        //                        rTextResult.AppendLine("#Local$ Good! " + tUserEcho + " is connected!");
        //                    }
        //                    else
        //                    {
        //                        rTextResult.AppendLine("#Local$ Fail! " + tUserEcho + " is not connected!");
        //                    }
        //                    rTextResult.AppendLine("--------------------");
        //                }

        //                rTextResult.AppendLine("===================");
        //                rTextResult.AppendLine("#Local$ Start command list!");
        //                rTextResult.AppendLine("===================");
        //                foreach (string tCommandLine in sCommandList)
        //                {
        //                    if (string.IsNullOrEmpty(tCommandLine) == false)
        //                    {
        //                        RunCommand(tCommandLine, tShellStream, rTextResult);
        //                    }
        //                }
        //                rTextResult.AppendLine("===================");
        //                rTextResult.AppendLine("#Local$ Finish command list!");
        //                rTextResult.AppendLine("===================");
        //                rTextResult.AppendLine("#Local$ " + tUserEcho + " Disconnecting!");
        //                tClientSSH.Disconnect();
        //                rTextResult.AppendLine("#Local$  Disconnected!");
        //            }
        //            else
        //            {
        //                rTextResult.AppendLine("#Local$ Connexion FAIL!");
        //            }
        //            tClientSSH.Dispose();
        //        }
        //    }
        //    catch (System.Exception e)
        //    {
        //        rTextResult.AppendLine(e.ToString());
        //        rTextResult.AppendLine("#Local$ FAIL!");
        //        Debug.Log(rTextResult);
        //        rReturn = false;
        //    }
        //    sServer.TextCommandResult = rTextResult.ToString();
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //const string kWhoamiOfThis = "this-is-the-whoami";
        //const string kStartOfThis = "this-is-the-start";
        //const string kEndOfThis = "this-is-the-end";
        //const string kWhoamiStartOfThis = "echo " + kWhoamiOfThis + ";";
        //const string kEchoEndOfThis = ";echo " + kEndOfThis + ";";
        //const string kEchoStartOfThis = "echo " + kStartOfThis + ";";
        ////-------------------------------------------------------------------------------------------------------------
        //public static string RunCommand(string sCommand, ShellStream sStream, StringBuilder rTextResult)
        //{
        //    //sStream.WriteLine("echo " + kStartOfThis +";" +sCommand + ";echo " + kEndOfThis + "");
        //    string tNewCommand = kWhoamiStartOfThis + "whoami;" + kEchoStartOfThis + sCommand + kEchoEndOfThis;
        //    //Debug.Log(tNewCommand);
        //    sStream.WriteLine(tNewCommand);
        //    while (sStream.Length == 0)
        //    {
        //        Thread.Sleep(500);
        //    }
        //    string rCommand = sCommand;
        //    string rLineReturn = string.Empty;
        //    string tLine;
        //    bool tStart = false;
        //    bool tStartSecond = false;
        //    string tUser = "unknow";
        //    while ((tLine = sStream.ReadLine()) != kEndOfThis)
        //    {
        //        if (tStartSecond == true)
        //        {
        //            tStartSecond = false;
        //            tUser = tLine;
        //        }
        //        if (tStart == true)
        //        {
        //            rLineReturn = rLineReturn + "\n" + tLine;
        //            rLineReturn = rLineReturn.Trim(new char[] { '\n', '\r' });
        //        }
        //        if (tLine == kStartOfThis)
        //        {
        //            tStart = true;
        //        }
        //        if (tLine == kWhoamiOfThis)
        //        {
        //            tStartSecond = true;
        //        }
        //    }
        //    rCommand = rCommand.Replace(kEchoStartOfThis, string.Empty).Replace(kEchoEndOfThis, string.Empty);
        //    rTextResult.AppendLine(">" + tUser + ":~$ " + sCommand);
        //    rTextResult.AppendLine(rLineReturn);
        //    Debug.Log(">" + tUser + ":~$ " + sCommand + '\n' + rLineReturn);
        //    return rLineReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif