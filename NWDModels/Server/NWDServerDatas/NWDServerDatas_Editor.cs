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
#endif
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
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    // doc to read to finish script : https://www.cyberciti.biz/tips/how-do-i-enable-remote-access-to-mysql-database-server.html

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDServerDatas : NWDBasisUnsynchronize
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
            Rect[,] tMatrix = NWDGUI.DiviseArea(sRect, 3, 100);
            int tI = 0;
            NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[2, tI]));
            tI++;
            //EditorGUI.LabelField(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Continent", Continent.ConcatRepresentations());
            //tI++;

            if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerate))
            {
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[2, tI]), "Credentials window"))
                {
                    NWDProjectCredentialsManager.SharedInstanceFocus();
                }
                tI++;
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[2, tI]), "Flush credentials"))
                {
                    NWDProjectCredentialsManager.FlushCredentials(NWDCredentialsRequired.ForSFTPGenerate);
                }
                tI++;
                NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[2, tI]));
                tI++;
                GUIStyle tSyleTextArea = new GUIStyle(GUI.skin.textArea);

                GUIContent tButtonTitle = null;

                NWDServer tServer = Server.GetRawData();
                if (tServer != null)
                {
                    //-----------------
                    EditorGUI.HelpBox(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[2, tI + 1]), "Don't forgot to check your ~/.ssh/known_hosts file permission!", MessageType.Warning);
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
                    if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[2, tI]), tButtonTitle))
                    {
                        NWDSSHWindow.ExecuteProcessTerminal(tcommandKeyGen);
                    }
                    tI++;
                    GUI.TextField(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[2, tI + 1]), tcommandKeyGen);
                    tI += 2;
                    NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[2, tI]));
                    tI++;
                    //-----------------
                    // find ip of server by dns if associated
                    EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(tServer.DomainNameServer) == true);
                    if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[2, tI]), "Find IP from Server (NWDServerDomain)"))
                    {
                        string tLocalIP = "0.0.0.0";
                        foreach (IPAddress tIP in Dns.GetHostAddresses(tServer.DomainNameServer))
                        {
                            if (tIP.AddressFamily == AddressFamily.InterNetwork)
                            {
                                tLocalIP = tIP.ToString();
                            }
                        }
                        MySQLIP.SetValue(tLocalIP);
                    }
                    EditorGUI.EndDisabledGroup();
                    tI++;
                    //-----------------
                    tButtonTitle = new GUIContent("Try connexion", " try connexion with root or admin");
                    if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[2, tI]), tButtonTitle))
                    {
                        tServer.ExecuteSSH(tButtonTitle.text, new List<string>()
                {
                    "ls",
                });
                    }
                    tI++;
                    //-----------------
                    tButtonTitle = new GUIContent("install MariaDB", " try install MariaDB (fork of MySQL)");
                    if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[2, tI]), tButtonTitle))
                    {
                        List<string> tCommandList = new List<string>();

                        tCommandList.Add("echo \"<color=red> -> server update</color>\"");
                        tCommandList.Add("apt-get update");
                        tCommandList.Add("apt-get -y upgrade");
                        tCommandList.Add("apt-get -y dist-upgrade");

                        tCommandList.Add("/etc/init.d/mysql stop");

                        tCommandList.Add("echo \"<color=red> -> mysql install (MariaDB)</color>\"");
                        tCommandList.Add("debconf-set-selections <<< \"mariadb-server mysql-server/root_password password " + Root_MySQLSecurePassword.Decrypt() + "\"");
                        tCommandList.Add("debconf-set-selections <<< \"mariadb-server mysql-server/root_password_again password " + Root_MySQLSecurePassword.Decrypt() + "\"");
                        tCommandList.Add("apt-get -y install mariadb-server");
                        tCommandList.Add("echo PURGE | debconf-communicate mariadb-server");
                        tCommandList.Add("echo \"<color=red> -> mysql start (MariaDB)</color>\"");

                        if (tServer.Distribution == NWDServerDistribution.debian9)
                        {
                            if (External == true)
                            {
                                tCommandList.Add("sed -i 's/^.*bind\\-address.*$/bind\\-address = 0.0.0.0/g' /etc/mysql/mariadb.conf.d/50-server.cnf");
                            }
                            else
                            {
                                tCommandList.Add("sed -i 's/^.*bind-address .*$/bind-address = 127.0.0.1/g' /etc/mysql/mariadb.conf.d/50-server.cnf");
                            }
                        }
                        if (tServer.Distribution == NWDServerDistribution.debian10)
                        {
                            if (External == true)
                            {
                                tCommandList.Add("sed -i 's/^.*bind\\-address.*$/#bind\\-address = 0.0.0.0/g' /etc/mysql/mariadb.conf.d/50-server.cnf");
                            }
                            else
                            {
                                tCommandList.Add("sed -i 's/^.*bind-address .*$/bind-address = 127.0.0.1/g' /etc/mysql/mariadb.conf.d/50-server.cnf");
                            }
                        }

                        tCommandList.Add("/etc/init.d/mysql start");
                        if (PhpMyAdmin == true)
                        {
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
                            tCommandList.Add("sed -i 's/^.*ServerSignature .*$//g' /etc/apache2/apache2.conf");
                            tCommandList.Add("sed -i '$ a ServerSignature Off' /etc/apache2/apache2.conf");

                            tCommandList.Add("echo \"<color=red> -> apache restart</color>\"");
                            tCommandList.Add("systemctl restart apache2");

                            tCommandList.Add("echo \"<color=red> -> install php</color>\"");
                            tCommandList.Add("apt-get -y install php");
                            //tCommandList.Add("apt-get -y install php-gd");
                            //tCommandList.Add("apt-get -y install php-bz2");
                            //tCommandList.Add("apt-get -y install php-tcpdf");
                            //tCommandList.Add("apt-get -y install php-twig");
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

                            tCommandList.Add("echo \"<color=red> -> phpmyadmin install</color>\"");
                            tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/dbconfig-install boolean true\"");
                            tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/app-password-confirm password " + Root_MySQLSecurePassword.Decrypt() + "\"");
                            tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/mysql/admin-pass password " + Root_MySQLSecurePassword.Decrypt() + "\"");
                            tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/mysql/app-pass password " + Root_MySQLSecurePassword.Decrypt() + "\"");
                            tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/reconfigure-webserver multiselect none\"");
                            tCommandList.Add("apt-get -y -q install phpmyadmin");
                            tCommandList.Add("echo PURGE | debconf-communicate phpmyadmin");
                            tCommandList.Add("systemctl restart apache2");
                        }
                        tServer.ExecuteSSH(tButtonTitle.text, tCommandList);
                    }
                    tI++;

                    //tButtonTitle = new GUIContent("install PhpMyAdmin", " try install PhpMyAdmin");
                    //if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), tButtonTitle))
                    //{
                    //    List<string> tCommandList = new List<string>();

                    //    tCommandList.Add("echo \"<color=red> -> server update</color>\"");
                    //    tCommandList.Add("apt-get update");
                    //    tCommandList.Add("apt-get -y upgrade");
                    //    tCommandList.Add("apt-get -y dist-upgrade");
                    //    if (PhpMyAdmin == true)
                    //    {
                    //        tCommandList.Add("echo \"<color=red> -> install php</color>\"");

                    //        tCommandList.Add("echo \"<color=red> -> phpmyadmin install</color>\"");
                    //        tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/dbconfig-install boolean true\"");
                    //        tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/app-password-confirm password " + Root_MysqlPassword + "\"");
                    //        tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/mysql/admin-pass password " + Root_MysqlPassword + "\"");
                    //        tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/mysql/app-pass password " + Root_MysqlPassword + "\"");
                    //        tCommandList.Add("debconf-set-selections <<< \"phpmyadmin phpmyadmin/reconfigure-webserver multiselect none\"");
                    //        tCommandList.Add("apt-get -y -q install phpmyadmin");
                    //        tCommandList.Add("echo PURGE | debconf-communicate phpmyadmin");
                    //        tCommandList.Add("systemctl restart apache2");
                    //    }
                    //    tServer.ExecuteSSH(tButtonTitle.text, tCommandList);
                    //}
                    //tI++;

                    tButtonTitle = new GUIContent("Install User", " try install User In MariaDB");
                    if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[2, tI]), tButtonTitle))
                    {
                        List<string> tCommandList = new List<string>();
                        tCommandList.Add("echo \"<color=red> -> add user in mysql</color>\"");
                        tCommandList.Add("echo \" -> add user in mysql\"");
                        tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"CREATE DATABASE IF NOT EXISTS " + MySQLBase + ";\"");
                        tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"GRANT ALL PRIVILEGES ON " + MySQLBase + ".* TO '" + MySQLUser + "'@'localhost' IDENTIFIED BY '" + MySQLSecurePassword.Decrypt() + "';\"");
                        if (External == true)
                        {
                            tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"GRANT ALL PRIVILEGES ON " + MySQLBase + ".* TO '" + MySQLUser + "'@'%' IDENTIFIED BY '" + MySQLSecurePassword.Decrypt() + "';\"");
                        }
                        else
                        {
                            tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"REVOKE ALL PRIVILEGES ON " + MySQLBase + ".* FROM '" + MySQLUser + "'@'%';\"");
                        }
                        tServer.ExecuteSSH(tButtonTitle.text, tCommandList);
                    }
                    tI++;


                    if (GUI.Button(tMatrix[0, tI], "http://xxx/phpmyadmin/"))
                    {
                        string tURL = "http://" + MySQLIP.GetValue() + "/phpmyadmin/?pma_username=" + MySQLUser + "&pma_password=" + MySQLSecurePassword.Decrypt() + "";
                        Application.OpenURL(tURL);
                    }
                    if (GUI.Button(tMatrix[1, tI], "https://xxx/phpmyadmin/"))
                    {
                        string tURL = "https://" + MySQLIP.GetValue() + "/phpmyadmin/?pma_username=" + MySQLUser + "&pma_password=" + MySQLSecurePassword.Decrypt() + "";
                        Application.OpenURL(tURL);
                    }
                    tI++;



                    //-----------------
                    string tURLAdmin = "sftp://" + tServer.Admin_User + ":" + tServer.Admin_Secure_Password.Decrypt() + "@" + tServer.IP.GetValue() + ":" + tServer.Port + "/";
                    tButtonTitle = new GUIContent("Try sftp ADMIN directly", tURLAdmin);
                    if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[2, tI]), tButtonTitle))
                    {
                        //NWEClipboard.CopyToClipboard(Password.GetValue());
                        Application.OpenURL(tURLAdmin);
                    }
                    tI++;

                    //-----------------
                    tButtonTitle = new GUIContent("restart phpmyadmin", "try to fix bug in login");
                    if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[2, tI]), tButtonTitle))
                    {
                        List<string> tCommandList = new List<string>();
                        //tCommandList.Add("/etc/init.d/apache2 restart");
                        tCommandList.Add("/etc/init.d/apache2 restart");
                        tCommandList.Add("/etc/init.d/mysql restart");
                        tServer.ExecuteSSH(tButtonTitle.text, tCommandList);
                    }
                    tI++;

                    //-----------------
                    NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[2, tI]));
                    tI++;

                    //-----------------
                    NWDGUI.BeginRedArea();
                    tButtonTitle = new GUIContent("Flush dev account", "warning");
                    if (GUI.Button(tMatrix[0, tI], tButtonTitle))
                    {
                        if (EditorUtility.DisplayDialog("WARNING", "YOU WILL DELETE ALL DATAS OF PLAYERS!", "YES", "CANCEL"))
                        {
                            List<string> tCommandList = new List<string>();
                            List<string> tTableList = new List<string>();
                            foreach (Type tType in NWDDataManager.SharedInstance().ClassAccountDependentList)
                            {
                                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                                if (Dev == true)
                                {
                                    tTableList.Add(tHelper.PHP_TABLENAME(NWDAppConfiguration.SharedInstance().DevEnvironment));
                                }
                            }
                            foreach (string tTable in tTableList)
                            {
                                tCommandList.Add("echo \"<color=orange> -> delete " + tTable + "</color>\"");
                                tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"USE " + MySQLBase + "; DROP TABLE " + MySQLBase + "." + tTable + ";\"");
                                //tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"USE " + MySQLBase + "; DELETE FROM " + MySQLBase + "." + tTable + ";\"");
                            }
                            tServer.ExecuteSSH(tButtonTitle.text, tCommandList);
                        }
                    }

                    tButtonTitle = new GUIContent("Flush preprod account", "warning");
                    if (GUI.Button(tMatrix[1, tI], tButtonTitle))
                    {
                        if (EditorUtility.DisplayDialog("WARNING", "YOU WILL DELETE ALL DATAS OF PLAYERS!", "YES", "CANCEL"))
                        {
                            List<string> tCommandList = new List<string>();
                            List<string> tTableList = new List<string>();
                            foreach (Type tType in NWDDataManager.SharedInstance().ClassAccountDependentList)
                            {
                                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                                if (Preprod == true)
                                {
                                    tTableList.Add(tHelper.PHP_TABLENAME(NWDAppConfiguration.SharedInstance().PreprodEnvironment));
                                }
                            }
                            foreach (string tTable in tTableList)
                            {
                                tCommandList.Add("echo \"<color=orange> -> delete " + tTable + "</color>\"");
                                tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"USE " + MySQLBase + "; DROP TABLE " + MySQLBase + "." + tTable + ";\"");
                                //tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"USE " + MySQLBase + "; DELETE FROM " + MySQLBase + "." + tTable + ";\"");
                            }
                            tServer.ExecuteSSH(tButtonTitle.text, tCommandList);
                        }
                    }

                    tButtonTitle = new GUIContent("Flush prod account", "warning");
                    if (GUI.Button(tMatrix[2, tI], tButtonTitle))
                    {
                        if (EditorUtility.DisplayDialog("WARNING", "YOU WILL DELETE ALL DATAS OF PLAYERS!", "YES", "CANCEL"))
                        {
                            List<string> tCommandList = new List<string>();
                            List<string> tTableList = new List<string>();
                            foreach (Type tType in NWDDataManager.SharedInstance().ClassAccountDependentList)
                            {
                                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                                if (Prod == true)
                                {
                                    tTableList.Add(tHelper.PHP_TABLENAME(NWDAppConfiguration.SharedInstance().ProdEnvironment));
                                }
                            }
                            foreach (string tTable in tTableList)
                            {
                                tCommandList.Add("echo \"<color=orange> -> delete " + tTable + "</color>\"");
                                tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"USE " + MySQLBase + "; DROP TABLE " + MySQLBase + "." + tTable + ";\"");
                                //tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"USE " + MySQLBase + "; DELETE FROM " + MySQLBase + "." + tTable + ";\"");
                            }
                            tServer.ExecuteSSH(tButtonTitle.text, tCommandList);
                        }
                    }
                    NWDGUI.EndRedArea();
                    tI++;
                    //-----------------
                    tButtonTitle = new GUIContent("Flush dev editor", "warning");
                    if (GUI.Button(tMatrix[0, tI], tButtonTitle))
                    {
                        List<string> tCommandList = new List<string>();
                        List<string> tTableList = new List<string>();
                        foreach (Type tType in NWDDataManager.SharedInstance().ClassNotAccountDependentList)
                        {
                            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                            if (Dev == true)
                            {
                                tTableList.Add(tHelper.PHP_TABLENAME(NWDAppConfiguration.SharedInstance().DevEnvironment));
                            }
                        }
                        foreach (string tTable in tTableList)
                        {
                            tCommandList.Add("echo \"<color=orange> -> delete " + tTable + "</color>\"");
                            tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"USE " + MySQLBase + "; DROP TABLE " + MySQLBase + "." + tTable + ";\"");
                            //tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"USE " + MySQLBase + "; DELETE FROM " + MySQLBase + "." + tTable + ";\"");
                        }
                        tServer.ExecuteSSH(tButtonTitle.text, tCommandList);
                    }

                    tButtonTitle = new GUIContent("Flush preprod editor", "warning");
                    if (GUI.Button(tMatrix[1, tI], tButtonTitle))
                    {
                        List<string> tCommandList = new List<string>();
                        List<string> tTableList = new List<string>();
                        foreach (Type tType in NWDDataManager.SharedInstance().ClassNotAccountDependentList)
                        {
                            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                            if (Preprod == true)
                            {
                                tTableList.Add(tHelper.PHP_TABLENAME(NWDAppConfiguration.SharedInstance().PreprodEnvironment));
                            }
                        }
                        foreach (string tTable in tTableList)
                        {
                            tCommandList.Add("echo \"<color=orange> -> delete " + tTable + "</color>\"");
                            tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"USE " + MySQLBase + "; DROP TABLE " + MySQLBase + "." + tTable + ";\"");
                            //tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"USE " + MySQLBase + "; DELETE FROM " + MySQLBase + "." + tTable + ";\"");
                        }
                        tServer.ExecuteSSH(tButtonTitle.text, tCommandList);
                    }

                    tButtonTitle = new GUIContent("Flush prod editor", "warning");
                    if (GUI.Button(tMatrix[2, tI], tButtonTitle))
                    {
                        List<string> tCommandList = new List<string>();
                        List<string> tTableList = new List<string>();
                        foreach (Type tType in NWDDataManager.SharedInstance().ClassNotAccountDependentList)
                        {
                            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                            if (Prod == true)
                            {
                                tTableList.Add(tHelper.PHP_TABLENAME(NWDAppConfiguration.SharedInstance().ProdEnvironment));
                            }
                        }
                        foreach (string tTable in tTableList)
                        {
                            tCommandList.Add("echo \"<color=orange> -> delete " + tTable + "</color>\"");
                            tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"USE " + MySQLBase + "; DROP TABLE " + MySQLBase + "." + tTable + ";\"");
                            //tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"USE " + MySQLBase + "; DELETE FROM " + MySQLBase + "." + tTable + ";\"");
                        }
                        tServer.ExecuteSSH(tButtonTitle.text, tCommandList);
                    }
                    tI++;

                    //-----------------
                    NWDServerDatas tServerDatasOrg = ServerEditorOriginal.GetRawData();
                    EditorGUI.BeginDisabledGroup(tServerDatasOrg == null);
                    {
                        tButtonTitle = new GUIContent("Replace Database from original", " delete and copy editor table ");
                        if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[2, tI]), tButtonTitle))
                        {
                            if (tServerDatasOrg != null)
                            {
                                NWDServer tServerOriginal = tServerDatasOrg.Server.GetRawData();
                                List<string> tCommandList = new List<string>();
                                List<string> tCommandListAnother = new List<string>();

                                List<string> tTableList = new List<string>();
                                foreach (Type tType in NWDDataManager.SharedInstance().ClassNotAccountDependentList)
                                {
                                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                                    if (Dev == true)
                                    {
                                        tTableList.Add(tHelper.PHP_TABLENAME(NWDAppConfiguration.SharedInstance().DevEnvironment));
                                    }
                                    if (Preprod == true)
                                    {
                                        tTableList.Add(tHelper.PHP_TABLENAME(NWDAppConfiguration.SharedInstance().PreprodEnvironment));
                                    }
                                    if (Prod == true)
                                    {
                                        tTableList.Add(tHelper.PHP_TABLENAME(NWDAppConfiguration.SharedInstance().ProdEnvironment));
                                    }
                                }
                                foreach (string tTable in tTableList)
                                {
                                    tCommandList.Add("echo \"<color=orange> -> delete " + tTable + "</color>\"");
                                    tCommandList.Add("mysql -u root -p\"" + Root_MySQLSecurePassword.Decrypt() + "\" -e \"USE " + MySQLBase + "; DROP TABLE " + MySQLBase + "." + tTable + ";\"");

                                    tCommandListAnother.Add("echo \"<color=orange> -> dump and copy " + tTable + "</color>\"");
                                    string tCommandDump = "mysqldump -u " + tServerDatasOrg.MySQLUser + " -p'" + tServerDatasOrg.MySQLSecurePassword.Decrypt() + "' " + tServerDatasOrg.MySQLBase + " " + tTable + " " +
                                " | mysql -h " + MySQLIP.GetValue() + " -u " + MySQLUser + " -p'" + MySQLSecurePassword.Decrypt() + "' " + MySQLBase + "";
                                    tCommandListAnother.Add(tCommandDump);
                                }

                                tServer.ExecuteSSH(tButtonTitle.text, tCommandList);
                                tServerOriginal.ExecuteSSH(tButtonTitle.text, tCommandListAnother);
                            }
                        }
                        tI++;
                        //-----------------
                    }
                    EditorGUI.EndDisabledGroup();
                    //-----------------



                    //}
                    //if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Find IP from Server (NWDServerDomain)"))
                    //{
                    //    string tLocalIP = "0.0.0.0";
                    //    //NWDServerDomain tDomain = Server.GetRawData();
                    //    //if (tDomain != null)
                    //    //{
                    //    //    foreach (IPAddress tIP in Dns.GetHostAddresses(tDomain.ServerDNS))
                    //    //    {
                    //    //        if (tIP.AddressFamily == AddressFamily.InterNetwork)
                    //    //        {
                    //    //            tLocalIP = tIP.ToString();
                    //    //        }
                    //    //    }
                    //    //    IP.SetValue(tLocalIP);
                    //    //}
                    //    //else
                    //    //{
                    //    //    IP.SetValue("0.0.0.0");
                    //    //}

                    //}
                    //tI++;

                    //if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Open Terminal"))
                    //{
                    //    // /Applications/Utilities/Terminal.app/Contents/MacOS/Terminal
                    //    FileInfo tFileInfo = new FileInfo("/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal");
                    //    System.Diagnostics.Process.Start(tFileInfo.FullName);
                    //}
                    //tI++;

                    //NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                    //tI++;

                    //GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Install Server MySQL command");
                    //tI++;
                    //if (Server != null)
                    //{
                    //    GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), NWDServerInstall.CommandInstallServerMySQL(Server.GetRawData(), Root_MysqlPassword.GetValue(), External, PhpMyAdmin));
                    //    tI += 11;

                    //    if (PhpMyAdmin == true && Server.GetRawData() != null)
                    //    {

                    //        if (GUI.Button(tMatrix[0, tI], "http://" + Server.GetRawData().IP + "/phpmyadmin/"))
                    //        {
                    //            Application.OpenURL("http://" + Server.GetRawData().IP + "/phpmyadmin/");
                    //        }
                    //        if (GUI.Button(tMatrix[1, tI], "https://" + Server.GetRawData().IP + "/phpmyadmin/"))
                    //        {
                    //            Application.OpenURL("https://" + Server.GetRawData().IP + "/phpmyadmin/");
                    //        }
                    //        tI++;
                    //    }

                    //NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                    //tI++;

                    //GUI.Label(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Install Database command");
                    //tI++;
                    //GUI.TextArea(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI + 10]), NWDServerInstall.CommandInstallDatabase(Server.GetRawData(), Root_MysqlPassword.GetValue(), MySQLUser, MySQLPassword.GetValue(), MySQLBase));





                    //tI += 11;
                    //if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Push force dev editor data"))
                    //{
                    //    //TODO : push data ...
                    //}
                    //tI++;
                    //if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Push force preprod editor data"))
                    //{
                    //    //TODO : push data ...
                    //}
                    //tI++;
                    //if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "Push force prod editor data"))
                    //{
                    //    //TODO : push data ...
                    //}
                    //tI++;

                    //NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                    //tI++;
                }
                /*
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "copy for dev"))
                {
                    NWDAppEnvironment tDev = NWDAppConfiguration.SharedInstance().DevEnvironment;
                    tDev.ServerHost = "localhost";
                    tDev.ServerUser = MySQLUser;
                    tDev.ServerPassword = MySQLPassword.GetValue();
                    tDev.ServerBase = MySQLBase;
                    NWDAppEnvironmentConfigurationManager.Refresh();
                }
                tI++;
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "copy for preprod"))
                {
                    NWDAppEnvironment tPreprod= NWDAppConfiguration.SharedInstance().PreprodEnvironment;
                    tPreprod.ServerHost = "localhost";
                    tPreprod.ServerUser = MySQLUser;
                    tPreprod.ServerPassword = MySQLPassword.GetValue();
                    tPreprod.ServerBase = MySQLBase;
                    NWDAppEnvironmentConfigurationManager.Refresh();
                }
                tI++;
                if (GUI.Button(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]), "copy for prod"))
                {
                    NWDAppEnvironment tProd = NWDAppConfiguration.SharedInstance().ProdEnvironment;
                    tProd.ServerHost = "localhost";
                    tProd.ServerUser = MySQLUser;
                    tProd.ServerPassword = MySQLPassword.GetValue();
                    tProd.ServerBase = MySQLBase;
                    NWDAppEnvironmentConfigurationManager.Refresh();
                }
                tI++;

                NWDGUI.Separator(NWDGUI.AssemblyArea(tMatrix[0, tI], tMatrix[1, tI]));
                tI++;
                */

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