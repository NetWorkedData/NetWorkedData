//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Text;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
        public void CreatePHP(List<Type> sTypeList, bool sCreateAll = true, bool sWriteOnDisk = true, NWDServerAuthentication sConn = null)
        {
            NWEBenchmark.Start();
            List<string> tFolders = CreatePHPFolder(sWriteOnDisk);
            Dictionary<string, string> tFilesAndDatas = new Dictionary<string, string>();
            CreatePHPManagementFile(tFilesAndDatas, sWriteOnDisk);
            CreatePHPWebservicesFile(tFilesAndDatas, sWriteOnDisk);
            if (sCreateAll == true)
            {
                CreatePHPErrorGenerate();
                CreatePHPConstantsFile(tFilesAndDatas, sWriteOnDisk);
                CreatePHPAuthenticationFile(tFilesAndDatas, sWriteOnDisk);
                CreatePHPRescueFile(tFilesAndDatas, sWriteOnDisk);
                CreatePHPBlankFile(tFilesAndDatas, sWriteOnDisk);
                CreatePHPIndexFile(tFilesAndDatas, sWriteOnDisk);
                CreatePHPDotHTAccessFile(tFilesAndDatas, sWriteOnDisk);
                CreatePHPMaintenanceFile(tFilesAndDatas, sWriteOnDisk);
                CreatePHPObsoleteFile(tFilesAndDatas, sWriteOnDisk);
                CreatePHP_StaticFinishFile(tFilesAndDatas, sWriteOnDisk);
                CreatePHP_StaticFunctionsFile(tFilesAndDatas, sWriteOnDisk);
                CreatePHP_StaticRequestFile(tFilesAndDatas, sWriteOnDisk);
                CreatePHP_StaticRespondFile(tFilesAndDatas, sWriteOnDisk);
                CreatePHP_StaticStartFile(tFilesAndDatas, sWriteOnDisk);
                CreatePHP_StaticValuesFile(tFilesAndDatas, sWriteOnDisk);
            }
            foreach (Type tType in sTypeList)
            {
                NWDBasisHelper tDatas = NWDBasisHelper.FindTypeInfos(tType);
                tFolders.Add(DBFolder(sWriteOnDisk) + tDatas.ClassNamePHP);

                Dictionary<string, string> tResult = NWDBasisHelper.FindTypeInfos(tType).CreatePHP(this, true);
                foreach (KeyValuePair<string, string> tKeyValue in tResult)
                {
                    tFilesAndDatas.Add(DBFolder(sWriteOnDisk) + tKeyValue.Key, tKeyValue.Value);
                }
                Dictionary<string, string> tResultAddOn = NWDBasisHelper.FindTypeInfos(tType).CreatePHPAddonFiles(this, sWriteOnDisk);
                foreach (KeyValuePair<string, string> tKeyValue in tResultAddOn)
                {
                    tFilesAndDatas.Add(tKeyValue.Key, tKeyValue.Value);
                }
            }
            if (sWriteOnDisk == true)
            {
                WriteFolderAndFiles(tFolders, tFilesAndDatas);
            }
            else
            {
                if (sConn == null)
                {
                    SendFolderAndFiles(tFolders, tFilesAndDatas, false);
                }
                else
                {
                    sConn.SendFolderAndFiles(tFolders, tFilesAndDatas, false);
                }
            }
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
        public string RootFolder(bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            string rReturn = string.Empty;
            if (sWriteOnDisk == true)
            {
                rReturn = NWDToolbox.FindOwnerServerFolder();
            }
            else
            {
            }
            //NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string EnvFolder(bool sWriteOnDisk = true)
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
        public string EngFolder(bool sWriteOnDisk = true)
        {
            //NWEBenchmark.Start();
            string rReturn = EnvFolder(sWriteOnDisk) + NWD.K_ENG + "/";
            //NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string DBFolder(bool sWriteOnDisk = true)
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
            if (LogMode == true)
            {
                tConstantsFile.AppendLine("// GLOBAL FOR DATABASE ACCESS ANALYZE");
                tConstantsFile.AppendLine("global $SQL_ACCESS_COUNT;");
                tConstantsFile.AppendLine("global $SQL_ACCESS_SQL;");
                tConstantsFile.AppendLine("$SQL_ACCESS_COUNT = 0;");
                tConstantsFile.AppendLine("$SQL_ACCESS_SQL = array();");
            }
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            tConstantsFile.AppendLine("// CONSTANT FOR EREG");
            tConstantsFile.AppendLine("$ereg_os = '/^(editor|unity|ios|osx|android|web|win|wp8|ps3|ps4|psp|switch)$/';");
            tConstantsFile.AppendLine("$ereg_version = '/^([0-9]{1,2})+(\\.[0-9]{1,3})*$/';");
            tConstantsFile.AppendLine("$ereg_lang = '/^([A-Z\\_\\-a-z]{2,7})$/';");
            tConstantsFile.AppendLine("$ereg_UUID = '/^([A-Za-z0-9\\-]{15,48})$/';");
            tConstantsFile.AppendLine("$ereg_hash = '/^(.*)$/';");
            tConstantsFile.AppendLine("$ereg_token = '/^(.*)$/';");
            tConstantsFile.AppendLine("// CONSTANT FOR WEB");
            tConstantsFile.AppendLine("$NWD_FLOAT_FORMAT = " + NWDConstants.FloatSQLFormat + ";");
            tConstantsFile.AppendLine("$NWD_DOUBLE_FORMAT = " + NWDConstants.DoubleSQLFormat + ";");
            tConstantsFile.AppendLine("$HTTP_URL = '" + GetConfigurationServerHTTPS() + "/" + NWDAppConfiguration.SharedInstance().WebServiceFolder() + "';");
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
            //tConstantsFile.AppendLine("$SMTP_DOMAIN = '" + MailDomain.Trim().Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$SMTP_FROM = '" + MailFrom.Trim().Replace("'", "\'") + "';");
            //tConstantsFile.AppendLine("$SMTP_REPLY = '" + MailReplyTo.Trim().Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$SMTP_USER = '" + MailUserName.Trim().Replace("'", "\'") + "';");
            tConstantsFile.AppendLine("$SMTP_PSW = '" + MailPassword.Trim().Replace("'", "\'") + "';");
            //tConstantsFile.AppendLine("$SMTP_AUT = '" + MailAuthentication.Trim().Replace("'", "\'") + "';");
            //tConstantsFile.AppendLine("$SMTP_STARTTLS = '" + MailEnableStarttlsAuto.Trim().Replace("'", "\'") + "';");
            //tConstantsFile.AppendLine("$SMTP_OPENSSL = '" + MailOpenSSLVerifyMode.Trim().Replace("'", "\'") + "';");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            tConstantsFile.AppendLine("// CONSTANT TO CONNECT TO SQL DATABASE");
            tConstantsFile.AppendLine("global $K_ConnectAllDatabases;");
            tConstantsFile.AppendLine("$K_ConnectAllDatabases = false;");
            //tConstantsFile.AppendLine("$SQL_HOT = '" + ServerHost.Replace("'", "\'") + "';");
            //tConstantsFile.AppendLine("$SQL_USR = '" + ServerUser.Replace("'", "\'") + "';");
            //tConstantsFile.AppendLine("$SQL_PSW = '" + ServerPassword.Replace("'", "\'") + "';");
            //tConstantsFile.AppendLine("$SQL_BSE = '" + ServerBase.Replace("'", "\'") + "';");
            foreach (NWDServerDatabaseAuthentication tServerDatabase in NWDServerDatas.GetAllConfigurationServerDatabase(this))
            {
                tConstantsFile.AppendLine("// Constant for ServerDatabase " + tServerDatabase.Title);
                tConstantsFile.AppendLine("$SQL_LIST['" + tServerDatabase.Range + "']['title'] = '" + tServerDatabase.Title.Replace("'", "\'") + "';");
                tConstantsFile.AppendLine("$SQL_LIST['" + tServerDatabase.Range + "']['id'] = '" + tServerDatabase.NameID.Replace("'", "\'") + "';");
                tConstantsFile.AppendLine("$SQL_LIST['" + tServerDatabase.Range + "']['host'] = '" + tServerDatabase.Host.Replace("'", "\'") + "';");
                tConstantsFile.AppendLine("$SQL_LIST['" + tServerDatabase.Range + "']['port'] = " + tServerDatabase.Port.ToString() + ";");
                tConstantsFile.AppendLine("$SQL_LIST['" + tServerDatabase.Range + "']['user'] = '" + tServerDatabase.User.Replace("'", "\'") + "';");
                tConstantsFile.AppendLine("$SQL_LIST['" + tServerDatabase.Range + "']['password'] = '" + tServerDatabase.Password.Replace("'", "\'") + "';");
                tConstantsFile.AppendLine("$SQL_LIST['" + tServerDatabase.Range + "']['database'] = '" + tServerDatabase.Database.Replace("'", "\'") + "';");
                tConstantsFile.AppendLine("$SQL_LIST['" + tServerDatabase.Range + "']['maxuser'] = " + tServerDatabase.MaxUser.Replace("'", "\'") + ";");
                tConstantsFile.AppendLine("$SQL_LIST['" + tServerDatabase.Range + "']['max'] = " + tServerDatabase.RangeMax.ToString() + ";");
                tConstantsFile.AppendLine("$SQL_LIST['" + tServerDatabase.Range + "']['min'] = " + tServerDatabase.RangeMin.ToString() + ";");
            }
            if (LogMode==true)
            {
                tConstantsFile.AppendLine("// add random for test");
                tConstantsFile.AppendLine("shuffle($SQL_LIST);");
            }
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
            tConstantsFile.AppendLine("// global variables");
            tConstantsFile.AppendLine("global $SQL_CURRENT_DATABASE, $SQL_CURRENT_ACCESSRANGE, $SQL_FAKE_ACCESSRANGE ;");
            tConstantsFile.AppendLine("global " + NWD.K_SQL_CON_EDITOR + ";");
            tConstantsFile.AppendLine("$SQL_CURRENT_DATABASE = NULL;");
            tConstantsFile.AppendLine("$SQL_CURRENT_ACCESSRANGE = -1;");
            //tConstantsFile.AppendLine("$SQL_FAKE_ACCESSRANGE = -1;");
            tConstantsFile.AppendLine("global $UserRange;");
            tConstantsFile.AppendLine("$UserRange = -1;");
            tConstantsFile.AppendLine("global $admin;");
            tConstantsFile.AppendLine("$admin = false;");
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
        //    StringBuilder tWebServicesAddon = new StringBuilder(string.Empty);
        //    tWebServicesAddon.AppendLine("<?php");
        //    tWebServicesAddon.AppendLine(Headlines());
        //    tWebServicesAddon.AppendLine("// WEBSERVICES ADDON");
        //    tWebServicesAddon.AppendLine(NWD.K_CommentSeparator);
        //    tWebServicesAddon.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
        //    tWebServicesAddon.AppendLine("{");
        //    // I need include ALL tables management files to manage ALL tables
        //    foreach (Type tType in NWDDataManager.SharedInstance().mTypeSynchronizedList)
        //    {
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
        //        }
        //    }
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
        private void CreatePHPRescueFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
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
            tFile.AppendLine("// RESCUE");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// Determine the file tree path");
            tFile.AppendLine("" + NWD.K_PATH_BASE + " = dirname(__DIR__);");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// include all necessary files");
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_CONSTANTS_FILE + "');");
            tFile.AppendLine("$TIME_MICRO = microtime(true); // perhaps use in instance of $TIME_STAMP in sync ");
            tFile.AppendLine("settype($TIME_MICRO, \"float\");");
            tFile.AppendLine("" + NWD.K_PHP_TIME_SYNC + " = intval($TIME_MICRO);");
            tFile.AppendLine("settype(" + NWD.K_PHP_TIME_SYNC + ", \"integer\");");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// use functions library");
            tFile.AppendLine("include_once ($PATH_BASE.'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FUNCTIONS_PHP + "');");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// connect MYSQL");
            //tFile.AppendLine(NWDError.PHP_log(sEnvironment, "Error in MySQL connexion on '.$tValue['host'].' for '.$tValue['user'].' with password ••••••••••••"));

            tFile.AppendLine("" + NWD.K_SQL_CON + " = new mysqli($SQL_HOT,$SQL_USR,$SQL_PSW, $SQL_BSE);");
            tFile.AppendLine("if (" + NWD.K_SQL_CON + "->connect_errno)");
            tFile.AppendLine("{");
            tFile.AppendLine("echo('internal error');");
            tFile.AppendLine("exit;");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("include_once ($PATH_BASE.'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDAccountSign>().ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
            tFile.AppendLine("include_once ($PATH_BASE.'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDError>().ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
            tFile.AppendLine("include_once ($PATH_BASE.'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDAccountSign>().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
            tFile.AppendLine("include_once ($PATH_BASE.'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDError>().ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
            tFile.AppendLine("if (getValue('rescueemail', '" + NWD.K_WEB_RESCUE_EMAIL_Key + "', " + NWD.K_WEB_EREG_SDKR + ", '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS01).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS02).Code + "'))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (getValue('rescuelang', '" + NWD.K_WEB_RESCUE_LANGUAGE_Key + "', " + NWD.K_WEB_EREG_SDKR + ", '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS01).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS02).Code + "'))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (getValue('fyr', '" + NWD.K_WEB_RESCUE_PROOF_Key + "', " + NWD.K_WEB_EREG_SDKR + ", '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS01).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS02).Code + "'))");
            tFile.AppendLine("{");
            tFile.AppendLine("include_once (" + NWDBasisHelper.BasisHelper<NWDAccountSign>().PHP_ENGINE_PATH(this) + ");");
            tFile.AppendLine("RescueSignProceed($rescueemail, $rescuelang, $fyr);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EnvFolder(sWriteOnDisk) + NWD.K_RESCUE_PHP, tFileFormatted);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPAuthenticationFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
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
            tFile.AppendLine("if (paramValue('action', '" + NWD.K_WEB_ACTION_KEY + "', " + NWD.K_WEB_EREG_ACTION + ", '" + NWDError.GetErrorDomainCode(NWDError.NWDError_ACC01).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_ACC02).Code + "')) // test if action is valid");
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

            tFile.AppendLine("if (paramValue('rescueemail', '" + NWD.K_WEB_RESCUE_EMAIL_Key + "', " + NWD.K_WEB_EREG_SDKR + ", '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS01).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS02).Code + "'))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (paramValue('rescuelang', '" + NWD.K_WEB_RESCUE_LANGUAGE_Key + "', " + NWD.K_WEB_EREG_SDKR + ", '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS01).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS02).Code + "'))");
            tFile.AppendLine("{");
            tFile.AppendLine("include_once (" + NWDBasisHelper.BasisHelper<NWDAccountSign>().PHP_ENGINE_PATH(this) + ");");
            tFile.AppendLine("RescueSign($rescueemail, $rescuelang);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");


            tFile.AppendLine("}");
            tFile.AppendLine("//---- SIGN UP ----");
            tFile.AppendLine("// I sign up with the value");
            tFile.AppendLine("if ($action == '" + NWD.K_WEB_ACTION_SIGNUP_KEY + "')");
            tFile.AppendLine("{");
            tFile.AppendLine("if (paramValue('sdkt', '" + NWD.K_WEB_SIGN_UP_TYPE_Key + "', " + NWD.K_WEB_EREG_SDKT + ", '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS01).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS02).Code + "'))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (paramValue('sdkv', '" + NWD.K_WEB_SIGN_UP_VALUE_Key + "', " + NWD.K_WEB_EREG_SDKI + ", '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS01).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS02).Code + "'))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (paramValue('sdkr', '" + NWD.K_WEB_SIGN_UP_RESCUE_Key + "', " + NWD.K_WEB_EREG_SDKR + ", '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS01).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS02).Code + "'))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (paramValue('sdkl', '" + NWD.K_WEB_SIGN_UP_LOGIN_Key + "', " + NWD.K_WEB_EREG_SDKR + ", '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS01).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS02).Code + "'))");
            tFile.AppendLine("{");
            tFile.AppendLine("FindSDKI($sdkt, $sdkv, $sdkr, $sdkl);");
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
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_SHS01));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("//---- SIGN IN ----");
            tFile.AppendLine("// I sign in with the good value");
            tFile.AppendLine("if ($action == '" + NWD.K_WEB_ACTION_SIGNIN_KEY + "')");
            tFile.AppendLine("{");
            tFile.AppendLine("if (paramValue('sdki', '" + NWD.K_WEB_SIGN_Key + "', " + NWD.K_WEB_EREG_SDKI + ", '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS01).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS02).Code + "'))");
            tFile.AppendLine("{");
            //tFile.AppendLine("$tOldUuid = $uuid;");
            tFile.AppendLine("$tNewUuid = FindAccount($HeaderUUID, $sdki, false);");
            //tFile.AppendLine("if ($tOldUuid != $tNewUuid)");
            tFile.AppendLine("if ($HeaderUUID != $tNewUuid)");
            tFile.AppendLine("{");
            tFile.AppendLine("// respondUUID($tNewUuid);");
            //tFile.AppendLine("NWDRequestTokenDeleteAllToken($tOldUuid); // delete old tokens");
            tFile.AppendLine("NWDRequestTokenDeleteAllToken($HeaderUUID); // delete old tokens");
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
            tFile.AppendLine("if (paramValue('sdki', '" + NWD.K_WEB_SIGN_Key + "', " + NWD.K_WEB_EREG_SDKI + ", '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS01).Code + "', '" + NWDError.GetErrorDomainCode(NWDError.NWDError_SHS02).Code + "'))");
            tFile.AppendLine("{");
            //tFile.AppendLine("$tOldUuid = $uuid;");
            //tFile.AppendLine("$tNewUuid = FindAccount($tOldUuid, $sdki, true);");
            tFile.AppendLine("$tNewUuid = FindAccount(time().'" + NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE + "', $sdki, true);");
            //tFile.AppendLine("if ($tOldUuid != $tNewUuid)");
            tFile.AppendLine("if ($HeaderUUID != $tNewUuid)");
            tFile.AppendLine("{");
            //tFile.AppendLine("NWDRequestTokenDeleteAllToken($tOldUuid);  // delete old tokens");
            tFile.AppendLine("NWDRequestTokenDeleteAllToken($HeaderUUID);  // delete old tokens");
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

            tFile.AppendLine("// get all datas for this account with reset sync... as force pull all!");

            //foreach (Type tType in NWDDataManager.SharedInstance().mTypeAccountDependantList)
            //{
            //tFile.AppendLine("if (isset($dico['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']))");
            //tFile.AppendLine("{");
            //tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
            //tFile.AppendLine(NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_SYNCHRONIZE() + " ($dico, $uuid, $admin);");
            //tFile.AppendLine("}");
            //}

            foreach (Type tType in NWDDataManager.SharedInstance().mTypeAccountDependantList)
            {
                if (NWDDataManager.SharedInstance().mTypeSynchronizedList.Contains(tType))
                {
                    tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
                    tFile.AppendLine(NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_GET_DATAS() + " (0, $uuid);");
                }
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
            sFilesAndDatas.Add(EnvFolder(sWriteOnDisk) + NWD.K_AUTHENTICATION_PHP, tFileFormatted);
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
