//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BasicToolBox;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Creates the PHP files .
        /// </summary>
        public void CreatePHP()
        {
            string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();

            DateTime tTime = DateTime.UtcNow;
            string tDateTimeString = tTime.ToString("yyyy-MM-dd");
            string tYearString = tTime.ToString("yyyy");
            // Create folders
            string tOwnerServerFolderPath = NWDToolbox.FindOwnerServerFolder();
            string tServerRootFolder = tOwnerServerFolderPath+ "/" + tWebServiceFolder + "/Environment/" + Environment;
            string tServerDatabaseFolder = tServerRootFolder + "/Engine";
            if (AssetDatabase.IsValidFolder(tOwnerServerFolderPath + "/"  + tWebServiceFolder) == false)
            {
                AssetDatabase.CreateFolder(tOwnerServerFolderPath, tWebServiceFolder);
            }
            if (AssetDatabase.IsValidFolder(tOwnerServerFolderPath + "/"  + tWebServiceFolder + "/Environment") == false)
            {
                AssetDatabase.CreateFolder(tOwnerServerFolderPath + "/" + tWebServiceFolder, "/Environment");
            }
            if (AssetDatabase.IsValidFolder(tOwnerServerFolderPath + "/"  + tWebServiceFolder + "/Environment/" + Environment) == false)
            {
                AssetDatabase.CreateFolder(tOwnerServerFolderPath + "/"  + tWebServiceFolder + "/Environment", Environment);
            }
            if (AssetDatabase.IsValidFolder(tOwnerServerFolderPath + "/"  + tWebServiceFolder + "/Environment/" + Environment + "/Engine") == false)
            {
                AssetDatabase.CreateFolder(tOwnerServerFolderPath + "/"  + tWebServiceFolder + "/Environment/" + Environment, "Engine");
            }
            if (AssetDatabase.IsValidFolder(tOwnerServerFolderPath + "/"  + tWebServiceFolder + "/Environment/" + Environment + "/Engine/Database") == false)
            {
                AssetDatabase.CreateFolder(tOwnerServerFolderPath + "/"  + tWebServiceFolder + "/Environment/" + Environment + "/Engine", "Database");
            }

            //========= ERROR NWDERROR REGENERATE
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeList)
            {
                var tMethodInfo = tType.GetMethod("ErrorRegenerate", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                   tMethodInfo.Invoke(null, null);
                }
            }
            //========= CONSTANTS FILE
            string tConstantsFile = "";
            tConstantsFile += "<?php\n" +
                "\t\t//NWD Autogenerate File at " + tDateTimeString + "\n" +
                "\t\t//Copyright NetWorkedDatas ideMobi " + tYearString + "\n" +
                "\t\t//Created by Jean-François CONTART\n" +
                "\t\t//-------------------- \n" +
                "\t\t// CONSTANTS \n" +
                "\t\t//-------------------- \n" +
                "\t$NWD_TMA = microtime(true);\n" +
                "\t\t//-------------------- \n";
            if (NWDAppConfiguration.SharedInstance().DevEnvironment == this)
            {
                tConstantsFile += "\terror_reporting(E_ALL);\n" +
                    "\tini_set('display_errors', 1);\n" +
                    "\t$NWD_LOG = true;\n";
            }
            else
            {
                tConstantsFile += "\t$NWD_LOG = false;\n";
            }
            tConstantsFile += "" +
                "\t\t//-------------------- \n" +
                "\t\t// CONSTANT FOR WEB\n" +
                "\t$HTTP_URL = '" + ServerHTTPS.TrimEnd('/') +"/"+ NWDAppConfiguration.SharedInstance().WebServiceFolder() + "';\n" +
                "\t$WS_DIR = '" + NWDAppConfiguration.SharedInstance().WebServiceFolder() + "';\n" +
                "\t\t//-------------------- \n" +
                "\t\t// CONSTANT FOR SHA512\n" +
                "\t$NWD_SHA_SEC = '" + DataSHAPassword.Replace("'", "\'") + "';\n" +
                "\t$NWD_SHA_VEC = '" + DataSHAVector.Replace("'", "'") + "';\n" +
                "\t$NWD_SLT_STR = '" + SaltStart.Replace("'", "\'") + "';\n" +
                "\t$NWD_SLT_END = '" + SaltEnd.Replace("'", "\'") + "';\n" +
                "\t$NWD_SLT_SRV = '" + SaltServer.Replace("'", "\'") + "';\n" +
                "\t\t//-------------------- \n" +
                "\t\t// CONSTANT FOR TEMPORAL SALT\n" +
                "\t$NWD_SLT_TMP = " + SaltFrequency.ToString() + ";\n" +
                "\t\t//-------------------- \n" +
                "\t\t// CONSTANT FOR SMTP\n" +
                "\t$SMTP_HOST = '" + MailHost.Trim().Replace("'", "\'") + "';\n" +
                "\t$SMTP_PORT = " + MailPort.ToString() + ";\n" +
                "\t$SMTP_DOMAIN = '" + MailDomain.Trim().Replace("'", "\'") + "';\n" +
                "\t$SMTP_FROM = '" + MailFrom.Trim().Replace("'", "\'") + "';\n" +
                "\t$SMTP_REPLY = '" + MailReplyTo.Trim().Replace("'", "\'") + "';\n" +
                "\t$SMTP_USER = '" + MailUserName.Trim().Replace("'", "\'") + "';\n" +
                "\t$SMTP_PSW = '" + MailPassword.Trim().Replace("'", "\'") + "';\n" +
                "\t$SMTP_AUT = '" + MailAuthentication.Trim().Replace("'", "\'") + "';\n" +
                "\t$SMTP_STARTTLS = '" + MailEnableStarttlsAuto.Trim().Replace("'", "\'") + "';\n" +
                "\t$SMTP_OPENSSL = '" + MailOpenSSLVerifyMode.Trim().Replace("'", "\'") + "';\n" +
                "\t\t//-------------------- \n" +
                "\t\t// CONSTANT TO CONNECT TO SQL DATABASE\n" +
                "\t$SQL_HOT = '" + ServerHost.Replace("'", "\'") + "';\n" +
                "\t$SQL_USR = '" + ServerUser.Replace("'", "\'") + "';\n" +
                "\t$SQL_PSW = '" + ServerPassword.Replace("'", "\'") + "';\n" +
                "\t$SQL_BSE = '" + ServerBase.Replace("'", "\'") + "';\n" +
                "\t\t//connection to mysql socket\n" +
                "\t$SQL_CON = '';\n" +
                "\t$SQL_CONDB = '';\n" +
                "\t$SQL_MNG = false;\n" +
                "\t\t//-------------------- \n" +
                "\t\t// ADMIN SECRET KEY\n" +
                "\t$NWD_ADM_KEY = '" + AdminKey.Replace("'", "\'") + "';\n" +
                "\t\t//-------------------- \n" +
                //"\t\t// RESCUE EMAIL\n" +
                "\t$NWD_RES_MAIL = '" + RescueEmail + "';\n" +
                "\t$NWD_APP_PRO = '" + AppProtocol.Replace("'", "\'") + "';\n" +
                "\t$NWD_APP_NAM = '" + AppName.Replace("'", "\'") + "';\n" +
                "\t\t//-------------------- \n" +
                "\t\t// SOCIALS APP KEY AND SECRET KEY\n" +
                "\t\t// -- facebook\n" +
                "\t$NWD_FCB_AID = '" + FacebookAppID.Replace("'", "\'") + "'; // for " + Environment + "\n" +
                "\t$NWD_FCB_SRT = '" + FacebookAppSecret.Replace("'", "\'") + "'; // for " + Environment + "\n" +
                "\t\t// -- google\n" +
                "\t$NWD_GGO_AID = '" + GoogleAppKey.Replace("'", "\'") + "';\n" +
                "\t\t//-------------------- \n" +
                "\t$ENV = '" + Environment + "';\n" +
                "\t$ENVSYNC = '" + Environment + "Sync';\n" +
                "\t\t//-------------------- \n" +
                "\t$RTH = " + TokenHistoric.ToString() + ";\n" +
                "\t\t//-------------------- \n" +
                //"\t$WSBUILD = "+BTBConfigManager.SharedInstance ().GetInt (NWDConstants.K_NWD_WS_BUILD, 0)+ ";\n" +
                "\t$WSBUILD = " + NWDAppConfiguration.SharedInstance().WebBuild + ";\n" +
                "\t\t//-------------------- \n" +
                "\t?>\n";
            File.WriteAllText(tServerDatabaseFolder + "/constants.php", tConstantsFile);
            AssetDatabase.ImportAsset(tServerDatabaseFolder + "/constants.php");

            //========= MANAGEMENT TABLE FUNCTIONS FILE

            string tManagementFile = "";
            tManagementFile += "<?php\n" +
                "//NWD Autogenerate File at " + tDateTimeString + "\n" +
                "//Copyright NetWorkedDatas ideMobi " + tYearString + "\n" +
                "//Created by Jean-François CONTART\n" +
                "//--------------------\n" +
                "// MANAGEMENT\n" +
                "//--------------------\n" +
                "set_time_limit (600);\n" + // timeout override
                "//--------------------\n" +
                "// Determine the file tree path\n" +
                "$PATH_BASE = dirname(dirname(__DIR__));\n" +
                "// include all necessary files\n" +
                "include_once ($PATH_BASE.'/Environment/" + Environment + "/Engine/constants.php');\n" +
                "$SQL_MNG = true;\n" +
                "// start the generic process\n" +
                "include_once ($PATH_BASE.'/Engine/start.php');\n" +
                "\n" +
                "//-------------------- \n" +
                "// TABLES MANAGEMENT \n" +
                "//-------------------- \n";
            // I need include ALL tables management files to manage ALL tables
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeList)
            {
                var tMethodInfo = tType.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    string tClassName = tMethodInfo.Invoke(null, null) as string;
                    tManagementFile += "include_once ($PATH_BASE.'/Environment/" + Environment + "/Engine/Database/" + tClassName + "/management.php');\n";
                }
            }
            tManagementFile += "//-------------------- \n" +
                "function CreateAllTables ()\n" +
                "\t{\n" +
                "\t\tmyLog('CREATE ALL TABALES ON SERVER', __FILE__, __FUNCTION__, __LINE__);\n";
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeList)
            {
                var tMethodInfo = tType.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    string tClassName = tMethodInfo.Invoke(null, null) as string;
                    tManagementFile += "\t\tCreate" + tClassName + "Table ();\n";
                }
            }
            tManagementFile += "\t}\n" +
                "//-------------------- \n" +
                "function DefragmentAllTables () " +
                "\t{\n" +
                "\t}\n " +
                "//-------------------- \n" +
                "function DropAllTables ()\n" +
                "\t{\n" +
                "\t}\n" +
                "//-------------------- \n" +
                "function FlushAllTables ()\n" +
                "\t{\n" +
                "\t}\n" +
                "//-------------------- \n" +
                "if($admin == true)\n {\nCreateAllTables ();\n};\n" +
                "//--------------------\n" +
                "// script is finished\n" +
                "// finish the generic process\n" +
                "\tinclude_once ($PATH_BASE.'/Engine/finish.php');\n" +
                "//--------------------\n" +
                "?>";
            File.WriteAllText(tServerRootFolder + "/management.php", tManagementFile);
            AssetDatabase.ImportAsset(tServerRootFolder + "/management.php");

            //========= WEBSERVICE FILE
            string tWebServices = "";
            tWebServices += "<?php\n" +
                "//NWD Autogenerate File at " + tDateTimeString + "\n" +
                "//Copyright NetWorkedDatas ideMobi " + tYearString + "\n" +
                "//Created by Jean-François CONTART\n" +
                "//--------------------\n" +
                "// WEBSERVICES FUNCTIONS\n" +
                "//--------------------\n" +
                "// Determine the file tree path\n" +
                "$PATH_BASE = dirname(dirname(__DIR__));\n" +
                "// include all necessary files\n" +
                "include_once ($PATH_BASE.'/Environment/" + Environment + "/Engine/constants.php');\n" +
                "// start the generic process\n" +
                "include_once ($PATH_BASE.'/Engine/start.php');\n" +
                "// start the script\n" +
                //"//--------------------\n" +
                //"global $dico, $uuid;\n" +
                //"//--------------------\n" +
                //"// Ok I create a permanent account if temporary before\n" +
                //"AccountAnonymeGenerate();\n" +
                //"//--------------------\n" +
                //"if ($ban == true)\n" +
                //"\t{\n" +
                //"\t\terror('ACC99');\n" +
                //"\t}\n" +
                "//--------------------\n" +
                "if (!errorDetected())\n" +
                "\t{\n";
            // I need include ALL tables management files to manage ALL tables
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeSynchronizedList)
            {
                var tMethodInfo = tType.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    string tClassName = tMethodInfo.Invoke(null, null) as string;
                    tWebServices += "\tif (isset($dico['" + tClassName + "']))\n\t\t{\n";
                    tWebServices += "\t\t\tinclude_once ( $PATH_BASE.'/Environment/" + Environment + "/Engine/Database/" + tClassName + "/synchronization.php');\n";
                    tWebServices += "\t\t\tSynchronize" + tClassName + " ($dico, $uuid, $admin);\n";
                    tWebServices += "\t\t}\n";
                }
            }
            // I need to prevent Non synchronized class from editor
            if (this == NWDAppConfiguration.SharedInstance().DevEnvironment || this == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {

                tWebServices += "\tif ($admin == true)\n\t\t{\n";
                foreach (Type tType in NWDDataManager.SharedInstance().mTypeUnSynchronizedList)
                {
                    var tMethodInfo = tType.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        string tClassName = tMethodInfo.Invoke(null, null) as string;
                        tWebServices += "\t\tif (isset($dico['" + tClassName + "']))\n\t\t{\n";
                        tWebServices += "\t\t\t\tinclude_once ( $PATH_BASE.'/Environment/" + Environment + "/Engine/Database/" + tClassName + "/synchronization.php');\n";
                        tWebServices += "\t\t\t\tSynchronize" + tClassName + " ($dico, $uuid, $admin);\n";
                        tWebServices += "\t\t\t}\n";
                    }
                }
                tWebServices += "\t\t}\n";
            }
            tWebServices += "// script is finished\n" +
                "\t}\n" +
                "//--------------------\n" +
                "include_once($PATH_BASE.'/Environment/"+ Environment +"/webservices_addon.php');\n" +
                "//--------------------\n" +
                "// finish the generic process\n" +
                "include_once ($PATH_BASE.'/Engine/finish.php');\n" +
                "//-------------------- \n" +
                "?>\n";
            File.WriteAllText(tServerRootFolder + "/webservices.php", tWebServices);
            AssetDatabase.ImportAsset(tServerRootFolder + "/webservices.php");




            //========= WEBSERVICE FILE AS ANNEXE OF ANOTHER FILE
            string tWebServicesAnnexe = "";
            tWebServicesAnnexe += "<?php\n" +
                "//NWD Autogenerate File at " + tDateTimeString + "\n" +
                "//Copyright NetWorkedDatas ideMobi " + tYearString + "\n" +
                "//Created by Jean-François CONTART\n" +
                "//--------------------\n" +
                "// WEBSERVICES FUNCTIONS FOR INSIDE INCLUDING\n" +
                //"//--------------------\n" +
                //"// Ok I create a permanent account if temporary before\n" +
                //"AccountAnonymeGenerate();\n" +
                //"//--------------------\n" +
                //"if ($ban == true)\n" +
                //"\t{\n" +
                //"\t\terror('ACC99');\n" +
                //"\t}\n" +
                "//--------------------\n" +
                "if (!errorDetected())\n" +
                "\t{\n";
            // I need include ALL tables management files to manage ALL tables
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeSynchronizedList)
            {
                var tMethodInfo = tType.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    string tClassName = tMethodInfo.Invoke(null, null) as string;
                    tWebServicesAnnexe += "\tif (isset($dico['" + tClassName + "']))\n\t\t{\n";
                    tWebServicesAnnexe += "\t\t\tinclude_once ( $PATH_BASE.'/Environment/" + Environment + "/Engine/Database/" + tClassName + "/synchronization.php');\n";
                    tWebServicesAnnexe += "\t\t\tSynchronize" + tClassName + " ($dico, $uuid, $admin);\n";
                    tWebServicesAnnexe += "\t\t}\n";
                }
            }
            // I need to prevent Non synchronized class from editor
            if (this == NWDAppConfiguration.SharedInstance().DevEnvironment || this == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                tWebServicesAnnexe += "\tif ($admin == true)\n\t\t{\n";
                foreach (Type tType in NWDDataManager.SharedInstance().mTypeUnSynchronizedList)
                {
                    var tMethodInfo = tType.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        string tClassName = tMethodInfo.Invoke(null, null) as string;
                        tWebServicesAnnexe += "\t\tif (isset($dico['" + tClassName + "']))\n\t\t{\n";
                        tWebServicesAnnexe += "\t\t\t\tinclude_once ( $PATH_BASE.'/Environment/" + Environment + "/Engine/Database/" + tClassName + "/synchronization.php');\n";
                        tWebServicesAnnexe += "\t\t\t\tSynchronize" + tClassName + " ($dico, $uuid, $admin);\n";
                        tWebServicesAnnexe += "\t\t\t}\n";
                    }
                }
                tWebServicesAnnexe += "\t\t}\n";
            }
            tWebServicesAnnexe += "// script is finished\n" +
                "\t}\n" +
                "//-------------------- \n" +
                "?>\n";
            File.WriteAllText(tServerRootFolder + "/webservices_inside.php", tWebServicesAnnexe);
            AssetDatabase.ImportAsset(tServerRootFolder + "/webservices_inside.php");

             //========= WEBSERVICE FILE AS ADDON OF ANOTHER FILE             string tWebServicesAddon = "";             tWebServicesAddon += "<?php\n" +                 "//NWD Autogenerate File at " + tDateTimeString + "\n" +                 "//Copyright NetWorkedDatas ideMobi " + tYearString + "\n" +                 "//Created by Jean-Françis CONTART\n" +                 "//--------------------\n" +                 "// WEBSERVICES FUNCTIONS FOR ADDON INCLUDING\n" +                 //"//--------------------\n" +                 //"// Ok I create a permanent account if temporary before\n" +                 //"AccountAnonymeGenerate();\n" +                 //"//--------------------\n" +                 //"if ($ban == true)\n" +                 //"\t{\n" +                 //"\t\terror('ACC99');\n" +                 //"\t}\n" +                 "//--------------------\n" +                 "if (!errorDetected())\n" +                 "\t{\n";             // I need include ALL tables management files to manage ALL tables             foreach (Type tType in NWDDataManager.SharedInstance().mTypeSynchronizedList)             {
                bool tCanBeAddoned = false;

                foreach (Type tSecondType in NWDDataManager.SharedInstance().mTypeSynchronizedList)
                {
                    foreach (var tProp in tSecondType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        if (tProp.GetCustomAttributes(typeof(NWDNeedReferenceAttribute), true).Length > 0)
                        {
                            foreach (NWDNeedReferenceAttribute tReference in tProp.GetCustomAttributes(typeof(NWDNeedReferenceAttribute), true))
                            {
                                if (tReference.ClassName == tType.Name)
                                {
                                    tCanBeAddoned = true;
                                    break;
                                }
                            }
                        }
                        if (tProp.GetCustomAttributes(typeof(NWDNeedUserAvatarAttribute), true).Length > 0)
                        {
                            if (typeof(NWDUserAvatar).Name == tType.Name)
                            {
                                tCanBeAddoned = true;
                            }
                        }
                        if (tProp.GetCustomAttributes(typeof(NWDNeedAccountNicknameAttribute), true).Length > 0)
                        {
                            if (typeof(NWDAccountNickname).Name == tType.Name)
                            {
                                tCanBeAddoned = true;
                            }
                        }
                        if (tCanBeAddoned == true)
                        {
                           break;
                        }
                    }
                    if (tCanBeAddoned == true)
                    {
                        break;
                    }
                }

                if (tCanBeAddoned == true)
                {
                    var tMethodInfo = tType.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        string tClassName = tMethodInfo.Invoke(null, null) as string;
                        tWebServicesAddon += "\tif (isset($REF_NEEDED['" + tClassName + "']))\n\t\t{\n";
                        tWebServicesAddon += "\t\t\tinclude_once ( $PATH_BASE.'/Environment/" + Environment + "/Engine/Database/" + tClassName + "/synchronization.php');\n";
                       // tWebServicesAddon += "\t\t\t // Need write loop to get Reference\n";
                        tWebServicesAddon += "\t\t\t GetDatas" + tClassName + "ByReferences(array_keys($REF_NEEDED['" + tClassName + "']));\n";
                        tWebServicesAddon += "\t\t}\n";

                        if (NWDDataManager.SharedInstance().mTypeAccountDependantList.Contains(tType))
                        {
                            tWebServicesAddon += "\tif (isset($ACC_NEEDED['" + tClassName + "']))\n\t\t{\n";
                            tWebServicesAddon += "\t\t\tinclude_once ( $PATH_BASE.'/Environment/" + Environment + "/Engine/Database/" + tClassName + "/synchronization.php');\n";
                            //tWebServicesAddon += "\t\t\t // Need write loop to get Datas account link\n";
                            tWebServicesAddon += "\t\t\t GetDatas" + tClassName + "ByAccounts (0, array_keys($ACC_NEEDED['" + tClassName + "']));\n";
                            tWebServicesAddon += "\t\t}\n";
                        }
                    }
                }             }             tWebServicesAddon += "// script is finished\n" +                 "\t}\n" +                 "//-------------------- \n" +                 "?>\n";             File.WriteAllText(tServerRootFolder + "/webservices_addon.php", tWebServicesAddon);             AssetDatabase.ImportAsset(tServerRootFolder + "/webservices_addon.php");


            //========= WEBSERVICE FILE WHEN ACCOUNT IS SIGN-IN SUCCESSED
            string tAccountServices = "";
            tAccountServices += "<?php\n" +
                "//NWD Autogenerate File at " + tDateTimeString + "\n" +
                "//Copyright NetWorkedDatas ideMobi " + tYearString + "\n" +
                "//Created by Jean-François CONTART\n" +
                "//--------------------\n" +
                "// WEBSERVICES FUNCTIONS\n" +
                "//--------------------\n" +
                "// Determine the file tree path\n" +
                "$PATH_BASE = dirname(dirname(__DIR__));\n" +
                "//--------------------\n" +
                "if (!errorDetected())\n" +
                "\t{\n" +
                "\t\tglobal $REP;\n" +
                "\t\tif (isset($REP['signin']))\n" +
                "\t\t\t{\n" +
                "\t\t\t\tif ($REP['signin'] == true)\n" +
                "\t\t\t\t\t{\n";
            // I need include ALL tables management files to manage ALL tables
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeAccountDependantList)
            {
                //				foreach (Type tType in NWDDataManager.SharedInstance().mTypeSynchronizedList) {
                var tMethodInfo = tType.GetMethod("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    string tClassName = tMethodInfo.Invoke(null, null) as string;
                    tAccountServices += "\t\t\t\t\t\t$dico['" + tClassName + "']['sync'] = true;\n";
                    tAccountServices += "\t\t\t\t\t\tinclude_once ( $PATH_BASE.'/Environment/" + Environment + "/Engine/Database/" + tClassName + "/synchronization.php');\n";
                    tAccountServices += "\t\t\t\t\t\tSynchronize" + tClassName + " ($dico, $uuid, false);\n";
                    tAccountServices += "\t\t\t\t\t\t\n";
                }
            }
            tAccountServices += "" +
                "\t\t\t\t\t}\n" +
                "\t\t\t}\n" +
                "\t\t}\n" +
                "//--------------------\n" +
                "?>\n";
            File.WriteAllText(tServerRootFolder + "/accountservices.php", tAccountServices);
            AssetDatabase.ImportAsset(tServerRootFolder + "/accountservices.php");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
