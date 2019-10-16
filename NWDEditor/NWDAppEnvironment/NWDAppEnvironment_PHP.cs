//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:21
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
//using BasicToolBox;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
        public void CreatePHP(List<Type> sTypeList, bool sCreateAll = true, bool sWriteOnDisk = true)
        {
            NWEBenchmark.Start();
            List<string> tFolders = CreatePHPFolder(sWriteOnDisk);
            Dictionary<string, string> tFilesAndDatas = new Dictionary<string, string>();
            float tCountClass = 4;
            float tOperation = 0;
            tCountClass = tCountClass + sTypeList.Count * 2;
            if (sCreateAll == true)
            {
                tCountClass = tCountClass + 25;
            }
            else
            {
                tCountClass = tCountClass + 5;
            }
            if (sWriteOnDisk == false)
            {
                tCountClass = tCountClass + 1;
            }
            else
            {
                tCountClass = tCountClass + 3;
            }
            string tTitle = Environment + " PHP Creation";
            EditorUtility.DisplayProgressBar(tTitle, "Start", tOperation++ / tCountClass);
            if (sWriteOnDisk == false)
            {
                EditorUtility.DisplayProgressBar(tTitle, "Connect SFTP", tOperation++ / tCountClass);
                ConnectSFTP();
            }
            EditorUtility.DisplayProgressBar(tTitle, "Management file generate", tOperation++ / tCountClass);
            CreatePHPManagementFile(tFilesAndDatas, sWriteOnDisk);
            EditorUtility.DisplayProgressBar(tTitle, "Webservices file generate", tOperation++ / tCountClass);
            CreatePHPWebservicesFile(tFilesAndDatas, sWriteOnDisk);
            EditorUtility.DisplayProgressBar(tTitle, "Webservices inside file generate", tOperation++ / tCountClass);
            //CreatePHPWebservicesInsideFile(tFilesAndDatas, sWriteOnDisk);
            EditorUtility.DisplayProgressBar(tTitle, "Webservices addon file generate", tOperation++ / tCountClass);
            //CreatePHPWebservicesAddonFile(tFilesAndDatas, sWriteOnDisk);
            if (sCreateAll == true)
            {
                EditorUtility.DisplayProgressBar(tTitle, "Error generate", tOperation++ / tCountClass);
                CreatePHPErrorGenerate();
                EditorUtility.DisplayProgressBar(tTitle, "Constant file generate", tOperation++ / tCountClass);
                CreatePHPConstantsFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Account services file generate", tOperation++ / tCountClass);
                //CreatePHPAccountServicesFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Account file generate", tOperation++ / tCountClass);
                CreatePHPAuthentificationFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Blank file generate", tOperation++ / tCountClass);
                CreatePHPBlankFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Index file generate", tOperation++ / tCountClass);
                CreatePHPIndexFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Rescue file generate", tOperation++ / tCountClass);
                //CreatePHPRescueFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "htaccess file generate", tOperation++ / tCountClass);
                CreatePHPDotHTAccessFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Maintenance file generate", tOperation++ / tCountClass);
                CreatePHPMaintenanceFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Obsolete file generate", tOperation++ / tCountClass);
                CreatePHPObsoleteFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Static account file generate", tOperation++ / tCountClass);
                //CreatePHP_StaticAccountFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Static Error file generate", tOperation++ / tCountClass);
                //CreatePHP_StaticErrorFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Static Finish file generate", tOperation++ / tCountClass);
                CreatePHP_StaticFinishFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Static Functions file generate", tOperation++ / tCountClass);
                CreatePHP_StaticFunctionsFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Static Request file generate", tOperation++ / tCountClass);
                CreatePHP_StaticRequestFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Static RequestToken file generate", tOperation++ / tCountClass);
                //CreatePHP_StaticRequestTokenFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Static Rescue file generate", tOperation++ / tCountClass);
                //CreatePHP_StaticRescueFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Static Respond file generate", tOperation++ / tCountClass);
                CreatePHP_StaticRespondFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Static Start file generate", tOperation++ / tCountClass);
                CreatePHP_StaticStartFile(tFilesAndDatas, sWriteOnDisk);
                EditorUtility.DisplayProgressBar(tTitle, "Static Values file generate", tOperation++ / tCountClass);
                CreatePHP_StaticValuesFile(tFilesAndDatas, sWriteOnDisk);
            }
            if (sWriteOnDisk == true)
            {
                EditorUtility.DisplayProgressBar(tTitle, "Writing files on disk", tOperation++ / tCountClass);
                WriteFolderAndFiles(tFolders, tFilesAndDatas);
            }
            else
            {
                EditorUtility.DisplayProgressBar(tTitle, "Send files on server", tOperation++ / tCountClass);
                SendFolderAndFiles(tFolders, tFilesAndDatas, false, false);
            }
            EditorUtility.DisplayProgressBar(tTitle, "Generate class models", tOperation++ / tCountClass);
            // generate models' files
            foreach (Type tType in sTypeList)
            {
                tFolders.Clear();
                tFilesAndDatas.Clear();
                NWDBasisHelper tDatas = NWDBasisHelper.FindTypeInfos(tType);
                tFolders.Add(DBFolder(sWriteOnDisk) + tDatas.ClassNamePHP);
                EditorUtility.DisplayProgressBar(tTitle, "Create " + tType.Name + " files", tOperation++ / tCountClass);

                Dictionary<string, string> tResult = NWDBasisHelper.FindTypeInfos(tType).CreatePHP(this, true);
                foreach (KeyValuePair<string, string> tKeyValue in tResult)
                {
                    tFilesAndDatas.Add(DBFolder(sWriteOnDisk) + tKeyValue.Key, tKeyValue.Value);
                }
                if (sWriteOnDisk == true)
                {
                    EditorUtility.DisplayProgressBar(tTitle, "Writing " + tType.Name + " files on disk", tOperation++ / tCountClass);
                    WriteFolderAndFiles(tFolders, tFilesAndDatas);
                }
                else
                {
                    EditorUtility.DisplayProgressBar(tTitle, "Send " + tType.Name + " files on server", tOperation++ / tCountClass);
                    SendFolderAndFiles(tFolders, tFilesAndDatas, false, false);
                }
            }
            if (sWriteOnDisk == false)
            {
                EditorUtility.DisplayProgressBar(tTitle, "Close SFTP", tOperation++ / tCountClass);
                DeconnectSFTP();
            }
            EditorUtility.ClearProgressBar();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Headlines()
        {
            //NWEBenchmark.Start();
            StringBuilder rReturn = new StringBuilder(string.Empty);

            string tWebService = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            DateTime tTime = DateTime.UtcNow;
            string tDateTimeString = NWDToolbox.DateTimeYYYYMMdd(tTime);
            string tYearString = NWDToolbox.DateTimeYYYY(tTime);

            rReturn.AppendLine(NWD.K_CommentSeparator);
            rReturn.AppendLine("//" + tWebService + " for " + Environment + " Environment");
            rReturn.AppendLine(NWD.K_CommentAutogenerate + tDateTimeString);
            rReturn.AppendLine(NWD.K_CommentCopyright + tYearString);
            rReturn.AppendLine(NWD.K_CommentCreator);
            rReturn.AppendLine(NWD.K_CommentSeparator);
            //NWEBenchmark.Finish();
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        private string EnvFolder(bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            string rReturn = string.Empty;
            string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            if (sWriteOnDisk == true)
            {
                string tWSFolderPath = NWDToolbox.FindOwnerServerFolder() + "/" + tWebServiceFolder + "/";
                rReturn = tWSFolderPath + Environment + "/";
            }
            else
            {
                rReturn = tWebServiceFolder + "/" + Environment + "/";
            }
            //NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private string EngFolder(bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            string rReturn = EnvFolder(sWriteOnDisk) + NWD.K_ENG + "/";
            //NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private string DBFolder(bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            string rReturn = EnvFolder(sWriteOnDisk) + NWD.K_DB + "/";
            //NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private List<string> CreatePHPFolder(bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            List<string> rReturn = new List<string>();
            string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            if (sWriteOnDisk == true)
            {
                string tWSFolderPath = NWDToolbox.FindOwnerServerFolder();
                rReturn.Add(tWSFolderPath + "/" + tWebServiceFolder);
                rReturn.Add(tWSFolderPath + "/" + tWebServiceFolder + "/" + Environment);
                rReturn.Add(tWSFolderPath + "/" + tWebServiceFolder + "/" + Environment + "/" + NWD.K_ENG);
                rReturn.Add(tWSFolderPath + "/" + tWebServiceFolder + "/" + Environment + "/" + NWD.K_DB);
            }
            else
            {
                rReturn.Add(tWebServiceFolder);
                rReturn.Add(tWebServiceFolder + "/" + Environment);
                rReturn.Add(tWebServiceFolder + "/" + Environment + "/" + NWD.K_ENG);
                rReturn.Add(tWebServiceFolder + "/" + Environment + "/" + NWD.K_DB);
            }
            //NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPErrorGenerate(bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            // regenerate basis error
            NWDErrorHelper tErrorHelper = NWDBasisHelper.BasisHelper<NWDError>() as NWDErrorHelper;
            tErrorHelper.GenerateBasisError();
            // regenerate
            //NWDDataManager.SharedInstance().CreateErrorsAndMessagesEngine();
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.ErrorRegenerate();
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPConstantsFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            StringBuilder tConstantsFile = new StringBuilder(string.Empty);
            tConstantsFile.AppendLine("<?php");
            tConstantsFile.AppendLine(Headlines());
            tConstantsFile.AppendLine("// CONSTANTS");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            tConstantsFile.AppendLine("$NWD_TMA = microtime (true);");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            if (LogMode == true)
            {
                tConstantsFile.AppendLine("error_reporting (E_ALL);");
                tConstantsFile.AppendLine("ini_set ('display_errors', 1);");
            }
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            tConstantsFile.AppendLine("// CONSTANT FOR WEB");
            tConstantsFile.AppendLine("$NWD_FLOAT_FORMAT = " + NWDConstants.FloatSQLFormat + ";");
            tConstantsFile.AppendLine("$NWD_DOUBLE_FORMAT = " + NWDConstants.DoubleSQLFormat + ";");
            tConstantsFile.AppendLine("$HTTP_URL = '" + ServerHTTPS.TrimEnd('/') + "/" + NWDAppConfiguration.SharedInstance().WebServiceFolder() + "';");
            tConstantsFile.AppendLine("$WS_DIR = '" + NWDAppConfiguration.SharedInstance().WebServiceFolder() + "';");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            tConstantsFile.AppendLine("// CONSTANT FOR SHA512");
            tConstantsFile.AppendLine("$NWD_SHA_SEC = '" + DataSHAPassword.Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$NWD_SHA_VEC = '" + DataSHAVector.Replace("'", "'") + "';");
            tConstantsFile.AppendLine("$NWD_SLT_STR = '" + SaltStart.Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$NWD_SLT_END = '" + SaltEnd.Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("" + NWD.K_NWD_SLT_SRV + " = '" + SaltServer.Replace("'", "\'") + "';");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            tConstantsFile.AppendLine("// CONSTANT FOR TEMPORAL SALT");
            tConstantsFile.AppendLine("$NWD_SLT_TMP = " + SaltFrequency.ToString() + ";");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            tConstantsFile.AppendLine("// CONSTANT FOR SMTP");
            tConstantsFile.AppendLine("$SMTP_HOST = '" + MailHost.Trim().Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$SMTP_PORT = " + MailPort.ToString() + ";");
            tConstantsFile.AppendLine("$SMTP_DOMAIN = '" + MailDomain.Trim().Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$SMTP_FROM = '" + MailFrom.Trim().Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$SMTP_REPLY = '" + MailReplyTo.Trim().Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$SMTP_USER = '" + MailUserName.Trim().Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$SMTP_PSW = '" + MailPassword.Trim().Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$SMTP_AUT = '" + MailAuthentication.Trim().Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$SMTP_STARTTLS = '" + MailEnableStarttlsAuto.Trim().Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$SMTP_OPENSSL = '" + MailOpenSSLVerifyMode.Trim().Replace("'", "\'") + "';");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            tConstantsFile.AppendLine("// CONSTANT TO CONNECT TO SQL DATABASE");
            tConstantsFile.AppendLine("$SQL_HOT = '" + ServerHost.Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$SQL_USR = '" + ServerUser.Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$SQL_PSW = '" + ServerPassword.Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$SQL_BSE = '" + ServerBase.Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("//connection to mysql socket");
            tConstantsFile.AppendLine("" + NWD.K_SQL_CON + " = '';");
            tConstantsFile.AppendLine("" + NWD.K_SQL_CON + "DB = '';");
            tConstantsFile.AppendLine("$SQL_MNG = false;");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            tConstantsFile.AppendLine("// ADMIN SECRET KEY");
            tConstantsFile.AppendLine("$NWD_ADM_KEY = '" + AdminKey.Replace("'", "\'") + "';");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            tConstantsFile.AppendLine("$NWD_RES_MAIL = '" + RescueEmail + "';");
            tConstantsFile.AppendLine("$NWD_APP_PRO = '" + AppProtocol.Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$NWD_APP_NAM = '" + AppName.Replace("'", "\'") + "';");
            //tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            //tConstantsFile.AppendLine("// SOCIALS APP KEY AND SECRET KEY");
            //tConstantsFile.AppendLine("// -- facebook");
            //tConstantsFile.AppendLine("$NWD_FCB_AID = '" + FacebookAppID.Replace("'", "\'") + "'; // for " + Environment);
            //tConstantsFile.AppendLine("$NWD_FCB_SRT = '" + FacebookAppSecret.Replace("'", "\'") + "'; // for " + Environment);
            //tConstantsFile.AppendLine("// -- google");
            //tConstantsFile.AppendLine("$NWD_GGO_AID = '" + GoogleAppKey.Replace("'", "\'") + "';");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            tConstantsFile.AppendLine("" + NWD.K_ENV + " = '" + Environment + "';");
            tConstantsFile.AppendLine("" + NWD.K_ENV + "SYNC = '" + Environment + "Sync';");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            tConstantsFile.AppendLine("$RTH = " + TokenHistoric.ToString() + ";");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            tConstantsFile.AppendLine("$WSBUILD = " + NWDAppConfiguration.SharedInstance().WebBuild + ";");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            tConstantsFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tConstantsFile.ToString());
            sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_CONSTANTS_FILE, tFileFormatted);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPManagementFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            StringBuilder tManagementFile = new StringBuilder(string.Empty);
            tManagementFile.AppendLine("<?php");
            tManagementFile.AppendLine(Headlines());
            if (LogMode == true)
            {
                tManagementFile.AppendLine("error_reporting (E_ALL);");
                tManagementFile.AppendLine("ini_set ('display_errors', 1);");
            }
            tManagementFile.AppendLine(NWD.K_CommentSeparator);
            tManagementFile.AppendLine("// MANAGEMENT");
            tManagementFile.AppendLine(NWD.K_CommentSeparator);
            tManagementFile.AppendLine("set_time_limit (" + EditorWebTimeOut.ToString() + ");");
            tManagementFile.AppendLine(NWD.K_CommentSeparator);
            tManagementFile.AppendLine("// Determine the file tree path");
            tManagementFile.AppendLine("" + NWD.K_PATH_BASE + " = dirname(__DIR__);");
            tManagementFile.AppendLine("// include all necessary files");
            tManagementFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_CONSTANTS_FILE + "');");
            tManagementFile.AppendLine("$SQL_MNG = true;");
            tManagementFile.AppendLine("// start the generic process");
            tManagementFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_START_PHP + "');");
            tManagementFile.AppendLine(NWD.K_CommentSeparator);
            tManagementFile.AppendLine("// TABLES MANAGEMENT");
            tManagementFile.AppendLine(NWD.K_CommentSeparator);
            tManagementFile.AppendLine("function CreateAllTables ()");
            tManagementFile.AppendLine("{");
            tManagementFile.AppendLine("global $PATH_BASE;");
            // I need include ALL tables management files to manage ALL tables
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeList)
            {
                string tClassName = NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP;
                tManagementFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_DB + "/" + tClassName + "/" + NWD.K_MANAGEMENT_FILE + "');");
                tManagementFile.AppendLine(NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_CREATE_TABLE() + "();");
            }
            tManagementFile.AppendLine("}");
            tManagementFile.AppendLine(NWD.K_CommentSeparator);
            tManagementFile.AppendLine("if($admin == true)");
            tManagementFile.AppendLine("{");
            tManagementFile.AppendLine("CreateAllTables ();");
            tManagementFile.AppendLine("}");
            tManagementFile.AppendLine(NWD.K_CommentSeparator);
            tManagementFile.AppendLine("// finish the generic process");
            tManagementFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FINISH_PHP + "');");
            tManagementFile.AppendLine(NWD.K_CommentSeparator);
            tManagementFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tManagementFile.ToString());
            sFilesAndDatas.Add(EnvFolder(sWriteOnDisk) + NWD.K_MANAGEMENT_FILE, tFileFormatted);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPWebservicesFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            StringBuilder tWebServices = new StringBuilder(string.Empty);
            tWebServices.AppendLine("<?php");
            tWebServices.AppendLine(Headlines());
            if (LogMode == true)
            {
                tWebServices.AppendLine("error_reporting (E_ALL);");
                tWebServices.AppendLine("ini_set ('display_errors', 1);");
            }
            tWebServices.AppendLine("// WEBSERVICES");
            tWebServices.AppendLine(NWD.K_CommentSeparator);
            tWebServices.AppendLine("// Determine the file tree path");
            tWebServices.AppendLine("" + NWD.K_PATH_BASE + " = dirname(__DIR__);");
            tWebServices.AppendLine("// include all necessary files");
            tWebServices.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_CONSTANTS_FILE + "');");
            tWebServices.AppendLine("// start the generic process");
            tWebServices.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_START_PHP + "');");
            tWebServices.AppendLine("// start the script");
            tWebServices.AppendLine(NWD.K_CommentSeparator);
            tWebServices.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
            tWebServices.AppendLine("{");
            tWebServices.AppendLine("if(NWDRequestTokenIsValid($uuid,$token) == true)");
            tWebServices.AppendLine("{");
            tWebServices.AppendLine("if(TestBanAccount($uuid) == false)");
            tWebServices.AppendLine("{");
            // I need include ALL tables management files to manage ALL tables
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeSynchronizedList)
            {
                tWebServices.AppendLine("if (isset($dico['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']))\n{");
                tWebServices.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
                tWebServices.AppendLine("" + NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_SYNCHRONIZE() + " ($dico, $uuid, $admin);");
                tWebServices.AppendLine("}");
            }
            // I need to prevent Non synchronized class from editor
            if (this == NWDAppConfiguration.SharedInstance().DevEnvironment || this == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {

                tWebServices.AppendLine("if ($admin == true)\n{");
                foreach (Type tType in NWDDataManager.SharedInstance().mTypeUnSynchronizedList)
                {
                    tWebServices.AppendLine("if (isset($dico['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']))\n{");
                    tWebServices.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
                    tWebServices.AppendLine("" + NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_SYNCHRONIZE() + " ($dico, $uuid, $admin);");
                    tWebServices.AppendLine("}");
                }
                tWebServices.AppendLine("}");
            }
            tWebServices.AppendLine("}");
            tWebServices.AppendLine("}");
            tWebServices.AppendLine("}");
            tWebServices.AppendLine(NWD.K_CommentSeparator);
            //tWebServices.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_WS_FILE_ADDON + "');");
            //tWebServices.AppendLine(NWD.K_CommentSeparator);
            tWebServices.AppendLine("// finish the generic process");
            tWebServices.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FINISH_PHP + "');");
            tWebServices.AppendLine(NWD.K_CommentSeparator);
            tWebServices.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tWebServices.ToString());
            sFilesAndDatas.Add(EnvFolder(sWriteOnDisk) + NWD.K_WS_FILE, tFileFormatted);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //private void CreatePHPWebservicesInsideFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        //{
        //    //NWEBenchmark.Start();
        //    StringBuilder tWebServicesAnnexe = new StringBuilder(string.Empty);
        //    tWebServicesAnnexe.AppendLine("<?php");
        //    tWebServicesAnnexe.AppendLine(Headlines());
        //    tWebServicesAnnexe.AppendLine("// WEBSERVICES INSIDE");
        //    tWebServicesAnnexe.AppendLine(NWD.K_CommentSeparator);
        //    tWebServicesAnnexe.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
        //    tWebServicesAnnexe.AppendLine("{");
        //    // I need include ALL tables management files to manage ALL tables
        //    foreach (Type tType in NWDDataManager.SharedInstance().mTypeSynchronizedList)
        //    {
        //        //string tClassName = NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP;
        //        tWebServicesAnnexe.AppendLine("if (isset($dico['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']))\n{");
        //        tWebServicesAnnexe.AppendLine("include_once ( " + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
        //        tWebServicesAnnexe.AppendLine("" + NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_SYNCHRONIZE() + " ($dico, $uuid, $admin);");
        //        tWebServicesAnnexe.AppendLine("}");
        //    }
        //    // I need to prevent Non synchronized class from editor
        //    if (this == NWDAppConfiguration.SharedInstance().DevEnvironment || this == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
        //    {
        //        tWebServicesAnnexe.AppendLine("if ($admin == true)\n{");
        //        foreach (Type tType in NWDDataManager.SharedInstance().mTypeUnSynchronizedList)
        //        {
        //            //string tClassName = NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP;
        //            tWebServicesAnnexe.AppendLine("if (isset($dico['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']))\n{");
        //            tWebServicesAnnexe.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
        //            tWebServicesAnnexe.AppendLine("" + NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_SYNCHRONIZE() + " ($dico, $uuid, $admin);");
        //            tWebServicesAnnexe.AppendLine("}");
        //        }
        //        tWebServicesAnnexe.AppendLine("}");
        //    }
        //    tWebServicesAnnexe.AppendLine("}");
        //    tWebServicesAnnexe.AppendLine(NWD.K_CommentSeparator);
        //    tWebServicesAnnexe.AppendLine("?>");
        //    string tFileFormatted = NWDToolbox.CSharpFormat(tWebServicesAnnexe.ToString());
        //    sFilesAndDatas.Add(EnvFolder(sWriteOnDisk) + NWD.K_WS_INSIDE_FILE, tFileFormatted);
        //    //NWEBenchmark.Finish();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //private void CreatePHPWebservicesAddonFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        //{
        //    //NWEBenchmark.Start();
        //    StringBuilder tWebServicesAddon = new StringBuilder(string.Empty);         //    tWebServicesAddon.AppendLine("<?php");
        //    tWebServicesAddon.AppendLine(Headlines());
        //    tWebServicesAddon.AppendLine("// WEBSERVICES ADDON");
        //    tWebServicesAddon.AppendLine(NWD.K_CommentSeparator);
        //    tWebServicesAddon.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
        //    tWebServicesAddon.AppendLine("{");
        //    // I need include ALL tables management files to manage ALL tables
        //    foreach (Type tType in NWDDataManager.SharedInstance().mTypeSynchronizedList)         //    {
        //        bool tCanBeAddoned = false;
        //        foreach (Type tSecondType in NWDDataManager.SharedInstance().mTypeSynchronizedList)
        //        {
        //            foreach (var tProp in tSecondType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        //            {
        //                if (tProp.GetCustomAttributes(typeof(NWDNeedReferenceAttribute), true).Length > 0)
        //                {
        //                    foreach (NWDNeedReferenceAttribute tReference in tProp.GetCustomAttributes(typeof(NWDNeedReferenceAttribute), true))
        //                    {
        //                        if (tReference.ClassName == tType.Name)
        //                        {
        //                            tCanBeAddoned = true;
        //                            break;
        //                        }
        //                    }
        //                }
        //                if (tProp.GetCustomAttributes(typeof(NWDNeedUserAvatarAttribute), true).Length > 0)
        //                {
        //                    if (typeof(NWDUserAvatar).Name == tType.Name)
        //                    {
        //                        tCanBeAddoned = true;
        //                    }
        //                }
        //                if (tProp.GetCustomAttributes(typeof(NWDNeedAccountNicknameAttribute), true).Length > 0)
        //                {
        //                    if (typeof(NWDAccountNickname).Name == tType.Name)
        //                    {
        //                        tCanBeAddoned = true;
        //                    }
        //                }
        //                if (tCanBeAddoned == true)
        //                {
        //                    break;
        //                }
        //            }
        //            if (tCanBeAddoned == true)
        //            {
        //                break;
        //            }
        //        }
        //        if (tCanBeAddoned == true)
        //        {
        //            //string tClassName = NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP;
        //            tWebServicesAddon.AppendLine("if (isset($REF_NEEDED['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']))\n{");
        //            tWebServicesAddon.AppendLine("include_once ( " + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
        //            tWebServicesAddon.AppendLine("" + NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_GET_DATAS_BY_REFERENCES() + "(array_keys($REF_NEEDED['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']));");
        //            tWebServicesAddon.AppendLine("}");

        //            if (NWDDataManager.SharedInstance().mTypeAccountDependantList.Contains(tType))
        //            {
        //                tWebServicesAddon.AppendLine("if (isset($ACC_NEEDED['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']))\t\t{");
        //                tWebServicesAddon.AppendLine("include_once ( " + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
        //                tWebServicesAddon.AppendLine("" + NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_GET_DATAS_BY_ACCOUNT() + "(0, array_keys($ACC_NEEDED['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']));");
        //                tWebServicesAddon.AppendLine("}");
        //            }
        //        }         //    }
        //    tWebServicesAddon.AppendLine("}");
        //    tWebServicesAddon.AppendLine(NWD.K_CommentSeparator);
        //    tWebServicesAddon.AppendLine("?>");
        //    string tFileFormatted = NWDToolbox.CSharpFormat(tWebServicesAddon.ToString());
        //    sFilesAndDatas.Add(EnvFolder(sWriteOnDisk) + NWD.K_WS_FILE_ADDON, tFileFormatted);
        //    //NWEBenchmark.Finish();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //private void CreatePHPAccountServicesFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        //{
        //    //NWEBenchmark.Start();
        //    StringBuilder tAccountServices = new StringBuilder(string.Empty);
        //    tAccountServices.AppendLine("<?php");
        //    tAccountServices.AppendLine(Headlines());
        //    if (LogMode == true)
        //    {
        //        tAccountServices.AppendLine("error_reporting (E_ALL);");
        //        tAccountServices.AppendLine("ini_set ('display_errors', 1);");
        //    }
        //    tAccountServices.AppendLine("// ACCOUNT SERVICES");
        //    tAccountServices.AppendLine(NWD.K_CommentSeparator);
        //    tAccountServices.AppendLine("// Determine the file tree path");
        //    tAccountServices.AppendLine("" + NWD.K_PATH_BASE + " = dirname(__DIR__);");
        //    tAccountServices.AppendLine(NWD.K_CommentSeparator);
        //    tAccountServices.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
        //    tAccountServices.AppendLine("{");
        //    tAccountServices.AppendLine("global $REP;");
        //    tAccountServices.AppendLine("if (isset($REP['signin']))");
        //    tAccountServices.AppendLine("{");
        //    tAccountServices.AppendLine("if ($REP['signin'] == true)");
        //    tAccountServices.AppendLine("{");
        //    // I need include ALL tables management files to manage ALL tables
        //    foreach (Type tType in NWDDataManager.SharedInstance().mTypeAccountDependantList)
        //    {
        //        //foreach (Type tType in NWDDataManager.SharedInstance().mTypeSynchronizedList) {
        //        //string tClassName = NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP;
        //        tAccountServices.AppendLine("$dico['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "'] = true;");
        //        tAccountServices.AppendLine("include_once ( " + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
        //        tAccountServices.AppendLine("" + NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_SYNCHRONIZE() + " ($dico, $uuid, false);");
        //        tAccountServices.AppendLine("");
        //    }
        //    tAccountServices.AppendLine(string.Empty);
        //    tAccountServices.AppendLine("}");
        //    tAccountServices.AppendLine("}");
        //    tAccountServices.AppendLine("}");
        //    tAccountServices.AppendLine(NWD.K_CommentSeparator);
        //    tAccountServices.AppendLine("?>");
        //    string tFileFormatted = NWDToolbox.CSharpFormat(tAccountServices.ToString());
        //    sFilesAndDatas.Add(EnvFolder(sWriteOnDisk) + NWD.K_WS_ACCOUNT_ADDON, tFileFormatted);
        //    //NWEBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPAuthentificationFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            if (LogMode == true)
            {
                tFile.AppendLine("error_reporting (E_ALL);");
                tFile.AppendLine("ini_set ('display_errors', 1);");
            }
            tFile.AppendLine("// ACCOUNT");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// Determine the file tree path");
            tFile.AppendLine("" + NWD.K_PATH_BASE + " = dirname(__DIR__);");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// include all necessary files");
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_CONSTANTS_FILE + "');");
            tFile.AppendLine("// start the generic process");
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_START_PHP + "');");
            tFile.AppendLine("// start the script");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("if (!" + NWDError.FUNCTIONPHP_errorDetected + "())");
            tFile.AppendLine("{");
            tFile.AppendLine("if (paramValue('action', '" + NWD.K_WEB_ACTION_KEY + "', " + NWD.K_WEB_EREG_ACTION + ", '" + NWDError.NWDError_ACC01.Code + "', '" + NWDError.NWDError_ACC02.Code + "')) // test if action is valid");
            tFile.AppendLine("{");

            tFile.AppendLine("//---- SECURE SYNC ----");


            tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
            tFile.AppendLine("{");
            tFile.AppendLine("if(NWDRequestTokenIsValid($uuid,$token) == true)");
            tFile.AppendLine("{");
            tFile.AppendLine("if(TestBanAccount($uuid) == false)");
            tFile.AppendLine("{");
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeSynchronizedList)
            {
                tFile.AppendLine("if (isset($dico['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']))\n{");
                tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
                tFile.AppendLine("" + NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_SYNCHRONIZE() + " ($dico, $uuid, $admin);");
                tFile.AppendLine("}");
            }
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");

            tFile.AppendLine("//---- RESCUE ----");
            tFile.AppendLine("// I ask rescue for my account");
            tFile.AppendLine("if ($action == '" + NWD.K_WEB_ACTION_RESCUE_KEY + "')");
            tFile.AppendLine("{");


            tFile.AppendLine("}");
            tFile.AppendLine("//---- SIGN UP ----");
            tFile.AppendLine("// I sign up with the value");
            tFile.AppendLine("if ($action == '" + NWD.K_WEB_ACTION_SIGNUP_KEY + "')");
            tFile.AppendLine("{");
            tFile.AppendLine("if (paramValue('sdkt', '" + NWD.K_WEB_SIGN_UP_TYPE_Key + "', " + NWD.K_WEB_EREG_SDKT + ", '" + NWDError.NWDError_SHS01.Code + "', '" + NWDError.NWDError_SHS02.Code + "'))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (paramValue('sdkv', '" + NWD.K_WEB_SIGN_UP_VALUE_Key + "', " + NWD.K_WEB_EREG_SDKI + ", '" + NWDError.NWDError_SHS01.Code + "', '" + NWDError.NWDError_SHS02.Code + "'))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (paramValue('sdkr', '" + NWD.K_WEB_SIGN_UP_RESCUE_Key + "', " + NWD.K_WEB_EREG_SDKR + ", '" + NWDError.NWDError_SHS01.Code + "', '" + NWDError.NWDError_SHS02.Code + "'))");
            tFile.AppendLine("{");
            tFile.AppendLine("FindSDKI($sdkt, $sdkv, $sdkr);");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SHS01));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SHS01));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SHS01));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("//---- SIGN IN ----");
            tFile.AppendLine("// I sign in with the good value");
            tFile.AppendLine("if ($action == '" + NWD.K_WEB_ACTION_SIGNIN_KEY + "')");
            tFile.AppendLine("{");
            tFile.AppendLine("if (paramValue('sdki', '" + NWD.K_WEB_SIGN_Key + "', " + NWD.K_WEB_EREG_SDKI + ", '" + NWDError.NWDError_SHS01.Code + "', '" + NWDError.NWDError_SHS02.Code + "'))");
            tFile.AppendLine("{");
            tFile.AppendLine("$tOldUuid = $uuid;");
            tFile.AppendLine("$tNewUuid = FindAccount($tOldUuid, $sdki, false);");
            tFile.AppendLine("if ($tOldUuid != $tNewUuid)");
            tFile.AppendLine("{");
            tFile.AppendLine("// respondUUID($tNewUuid);");
            tFile.AppendLine("NWDRequestTokenDeleteAllToken($tOldUuid); // delete old tokens");
            //tFile.AppendLine("respond_SignIn();");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SGN17));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SHS01));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("//---- SIGN OUT ----");
            tFile.AppendLine("// I sign out with the good value");
            tFile.AppendLine("if ($action == '" + NWD.K_WEB_ACTION_SIGNOUT_KEY + "')");
            tFile.AppendLine("{");
            tFile.AppendLine("if (paramValue('sdki', '" + NWD.K_WEB_SIGN_Key + "', " + NWD.K_WEB_EREG_SDKI + ", '" + NWDError.NWDError_SHS01.Code + "', '" + NWDError.NWDError_SHS02.Code + "'))");
            tFile.AppendLine("{");
            tFile.AppendLine("$tOldUuid = $uuid;");
            //tFile.AppendLine("$tNewUuid = FindAccount($tOldUuid, $sdki, true);");
            tFile.AppendLine("$tNewUuid = FindAccount(time().'T', $sdki, true);");
            tFile.AppendLine("if ($tOldUuid != $tNewUuid)");
            tFile.AppendLine("{");
            tFile.AppendLine("NWDRequestTokenDeleteAllToken($tOldUuid);  // delete old tokens");
            //tFile.AppendLine("respond_SignOut();");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SGN20));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SHS01));
            tFile.AppendLine("}");
            tFile.AppendLine("}");


            //tFile.AppendLine("if (!"+NWDError.FUNCTIONPHP_errorDetected+"())");
            //tFile.AppendLine("{");
            //// I need include ALL tables management files to manage ALL tables
            //foreach (Type tType in NWDDataManager.SharedInstance().mTypeSynchronizedList)
            //{
            //    tFile.AppendLine("if (isset($dico['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']))\n{");
            //    tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
            //    tFile.AppendLine("" + NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_SYNCHRONIZE() + " ($dico, $uuid, $admin);");
            //    tFile.AppendLine("}");
            //}
            //tFile.AppendLine("}");

            //// I need to prevent Non synchronized class from editor
            //if (this == NWDAppConfiguration.SharedInstance().DevEnvironment || this == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            //{

            //    tFile.AppendLine("if ($admin == true)\n{");
            //    foreach (Type tType in NWDDataManager.SharedInstance().mTypeUnSynchronizedList)
            //    {
            //        tFile.AppendLine("if (isset($dico['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']))\n{");
            //        tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
            //        tFile.AppendLine("" + NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_SYNCHRONIZE() + " ($dico, $uuid, $admin);");
            //        tFile.AppendLine("}");
            //    }
            //    tFile.AppendLine("}");
            //}

            tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
            tFile.AppendLine("{");
            tFile.AppendLine("if(TestBanAccount($uuid) == false)");
            tFile.AppendLine("{");
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeAccountDependantList)
            {
                tFile.AppendLine("if (isset($dico['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']))");
                tFile.AppendLine("{");
                tFile.AppendLine(NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_SYNCHRONIZE() + " ($dico, $uuid, $admin);");
                tFile.AppendLine("}");
            }
            tFile.AppendLine("}");
            tFile.AppendLine("}");




            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// finish the generic process");
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FINISH_PHP + "');");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EnvFolder(sWriteOnDisk) + NWD.K_AUTHENTIFICATION_PHP, tFileFormatted);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPBlankFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            if (LogMode == true)
            {
                tFile.AppendLine("error_reporting (E_ALL);");
                tFile.AppendLine("ini_set ('display_errors', 1);");
            }
            tFile.AppendLine("// BLANK");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// Determine the file tree path");
            tFile.AppendLine("" + NWD.K_PATH_BASE + " = dirname(__DIR__);");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_CONSTANTS_FILE + "');");
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_START_PHP + "');");
            //tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_WS_INSIDE_FILE + "');");
            tFile.AppendLine("mysqli_close (" + NWD.K_SQL_CON + ");");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EnvFolder(sWriteOnDisk) + NWD.K_BLANK_PHP, tFileFormatted);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //private void CreatePHPRescueFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        //{
        //    //NWEBenchmark.Start();
        //    StringBuilder tFile = new StringBuilder(string.Empty);
        //    tFile.AppendLine("<?php");
        //    tFile.AppendLine(Headlines());
        //    if (LogMode == true)
        //    {
        //        tFile.AppendLine("error_reporting (E_ALL);");
        //        tFile.AppendLine("ini_set ('display_errors', 1);");
        //    }
        //    tFile.AppendLine("// RESCUE");
        //    tFile.AppendLine("$NWD_TMA = microtime(true);");
        //    tFile.AppendLine(NWD.K_CommentSeparator);
        //    tFile.AppendLine("// Determine the file tree path");
        //    tFile.AppendLine("" + NWD.K_PATH_BASE + " = dirname(__DIR__);");
        //    tFile.AppendLine(NWD.K_CommentSeparator);
        //    tFile.AppendLine("" + NWD.K_ENV + " = '" + Environment + "';");
        //    tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_RESCUE_PHP + "');");
        //    tFile.AppendLine(NWD.K_CommentSeparator);
        //    tFile.AppendLine("?>");
        //    string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
        //    sFilesAndDatas.Add(EnvFolder(sWriteOnDisk) + NWD.K_RESCUE_PHP, tFileFormatted);
        //    //NWEBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPIndexFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            if (LogMode == true)
            {
                tFile.AppendLine("error_reporting (E_ALL);");
                tFile.AppendLine("ini_set ('display_errors', 1);");
            }
            tFile.AppendLine("// INDEX");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EnvFolder(sWriteOnDisk) + NWD.K_INDEX_PHP, tFileFormatted);
            sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_INDEX_PHP, tFileFormatted);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPMaintenanceFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            if (LogMode == true)
            {
                tFile.AppendLine("error_reporting (E_ALL);");
                tFile.AppendLine("ini_set ('display_errors', 1);");
            }
            tFile.AppendLine("// " + NWD.K_MAINTENANCE_ERROR + "");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("header('" + NWD.K_MAINTENANCE_HEADER_KEY + ": true');");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            tFile.AppendLine("{\"" + NWD.K_MAINTENANCE_HEADER_KEY + "\":\"true\"}");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EnvFolder(sWriteOnDisk) + NWD.K_MAINTENANCE_PHP, tFileFormatted);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPObsoleteFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            if (LogMode == true)
            {
                tFile.AppendLine("error_reporting (E_ALL);");
                tFile.AppendLine("ini_set ('display_errors', 1);");
            }
            tFile.AppendLine("// " + NWD.K_OBSOLETE_ERROR + "");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("header('" + NWD.K_OBSOLETE_HEADER_KEY + ": true');");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            tFile.AppendLine("{\"" + NWD.K_OBSOLETE_HEADER_KEY + "\":\"true\"}");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EnvFolder(sWriteOnDisk) + NWD.K_OBSOLETE_PHP, tFileFormatted);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPDotHTAccessFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            if (sWriteOnDisk == false)
            {
                sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_HTACCESS, "deny from all");
                sFilesAndDatas.Add(DBFolder(sWriteOnDisk) + NWD.K_HTACCESS, "deny from all");
            }
            else
            {
                sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_DOT_HTACCESS, "deny from all");
                sFilesAndDatas.Add(DBFolder(sWriteOnDisk) + NWD.K_DOT_HTACCESS, "deny from all");
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
