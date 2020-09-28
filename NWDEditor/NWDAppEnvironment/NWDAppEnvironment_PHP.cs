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
using System.Collections.Generic;
using System.Text;
using NetWorkedData.NWDEditor;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
        static void LoadDataNecessary()
        {
            NWDBenchmark.Start();
            NWDBasisHelper.BasisHelper<NWDError>().LoadDataForWorkflow();
            NWDBasisHelper.BasisHelper<NWDCluster>().LoadDataForWorkflow();
            NWDBasisHelper.BasisHelper<NWDServerDomain>().LoadDataForWorkflow();
            NWDBasisHelper.BasisHelper<NWDServer>().LoadDataForWorkflow();
            NWDBasisHelper.BasisHelper<NWDServerServices>().LoadDataForWorkflow();
            NWDBasisHelper.BasisHelper<NWDServerDatas>().LoadDataForWorkflow();
            NWDBasisHelper.BasisHelper<NWDServerOther>().LoadDataForWorkflow();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CreatePHP(List<Type> sTypeList, bool sCreateAll = true, bool sWriteOnDisk = true, NWDServerAuthentication sConn = null)
        {
            NWDBenchmark.Start();
            NWDCluster tNWDCluster = NWDCluster.SelectClusterforEnvironment(this, true);
            if (tNWDCluster == null)
            {
            }
            else
            {

                // save datas
                NWDProjectCredentialsManager.SharedInstance().ShowUtility();
                NWDDataManager.SharedInstance().DataQueueExecute();
                LoadDataNecessary();
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
                    if (tDatas.TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
                    {
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
                NWDOperationWebhook.NewWebService(this, sTypeList);
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Headlines()
        {
            //NWDBenchmark.Start();
            StringBuilder rReturn = new StringBuilder(string.Empty);
            string tWebService = NWDAppConfiguration.SharedInstance().WebServiceFolderMax();
            DateTime tTime = DateTime.UtcNow;
            string tDateTimeString = NWDToolbox.DateTimeYYYYMMddHHmmss(tTime);
            string tYearString = NWDToolbox.DateTimeYYYY(tTime);
            rReturn.AppendLine(NWD.K_CommentSeparator);
            rReturn.AppendLine(NWD.K_CommentCopyright + tYearString);
            rReturn.AppendLine(NWD.K_CommentCreator);

            rReturn.AppendLine(NWD.K_CommentSeparator);
            rReturn.AppendLine("// " + tWebService + " for " + Environment + " Environment");
            rReturn.AppendLine("// Generated at " + tDateTimeString);
            rReturn.AppendLine("// Generated by " + NWDProjectConfigurationManagerContent.SharedInstance().UserName);
            rReturn.AppendLine("// Generated on system " + SystemInfo.deviceUniqueIdentifier + " (deviceUniqueIdentifier)");

            rReturn.AppendLine(NWD.K_CommentSeparator);
            //NWDBenchmark.Finish();
            return rReturn.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string RootFolder(bool sWriteOnDisk = true)
        {
            //NWDBenchmark.Start();
            string rReturn = string.Empty;
            if (sWriteOnDisk == true)
            {
                rReturn = NWDToolbox.FindOwnerServerFolder();
            }
            else
            {
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string EnvFolder(bool sWriteOnDisk = true)
        {
            //NWDBenchmark.Start();
            string rReturn = string.Empty;
            string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolderMax();
            if (sWriteOnDisk == true)
            {
                string tWSFolderPath = NWDToolbox.FindOwnerServerFolder() + "/" + tWebServiceFolder + "/";
                rReturn = tWSFolderPath + Environment + "/";
            }
            else
            {
                rReturn = tWebServiceFolder + "/" + Environment + "/";
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string EngFolder(bool sWriteOnDisk = true)
        {
            //NWDBenchmark.Start();
            string rReturn = EnvFolder(sWriteOnDisk) + NWD.K_ENG + "/";
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string DBFolder(bool sWriteOnDisk = true)
        {
            //NWDBenchmark.Start();
            string rReturn = EnvFolder(sWriteOnDisk) + NWD.K_DB + "/";
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private List<string> CreatePHPFolder(bool sWriteOnDisk = true)
        {
            //NWDBenchmark.Start();
            List<string> rReturn = new List<string>();
            string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolderMax();
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
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPErrorGenerate(bool sWriteOnDisk = true)
        {
            //NWDBenchmark.Start();
            LoadDataNecessary();
            // regenerate basis error
            NWDErrorHelper tErrorHelper = NWDBasisHelper.BasisHelper<NWDError>() as NWDErrorHelper;
            tErrorHelper.GenerateBasisError();
            // regenerate
            //NWDDataManager.SharedInstance().CreateErrorsAndMessagesEngine();
            foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.ErrorRegenerate();
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPConstantsFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWDBenchmark.Start();
            LoadDataNecessary();
            StringBuilder tConstantsFile = new StringBuilder(string.Empty);
            tConstantsFile.AppendLine("<?php");
            tConstantsFile.AppendLine(Headlines());
            tConstantsFile.AppendLine("// CONSTANTS");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            tConstantsFile.AppendLine("$NWD_TMA = microtime (true);");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            if (LogMode != NWDEnvironmentLogMode.NoLog)
            {
                tConstantsFile.AppendLine("error_reporting (E_ALL);");
                tConstantsFile.AppendLine("ini_set ('display_errors', 1);");
            }
            if (LogMode != NWDEnvironmentLogMode.NoLog)
            {
                tConstantsFile.AppendLine("// GLOBAL FOR DATABASE ACCESS ANALYZE");
                tConstantsFile.AppendLine("global $SQL_ACCESS_COUNT;");
                tConstantsFile.AppendLine("global $SQL_ACCESS_SQL;");
                tConstantsFile.AppendLine("$SQL_ACCESS_COUNT = 0;");
                tConstantsFile.AppendLine("$SQL_ACCESS_SQL = array();");
            }

            NWDCluster tNWDCluster = NWDCluster.SelectClusterforEnvironment(this, true);
            if (tNWDCluster == null)
            {
            }
            else
            {
                // prevent null effect
                tNWDCluster.NotNullChecker();
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
                tConstantsFile.AppendLine("$HTTP_URL = '" + GetConfigurationServerHTTPS() + "/" + NWDAppConfiguration.SharedInstance().WebServiceFolderMax() + "';");
                tConstantsFile.AppendLine("$WS_DIR = '" + NWDAppConfiguration.SharedInstance().WebServiceFolderMax() + "';");
                tConstantsFile.AppendLine(NWD.K_CommentSeparator);
                tConstantsFile.AppendLine("// CONSTANT FOR SHA512");
                tConstantsFile.AppendLine("$NWD_SHA_SEC = '" + DataSHAPassword.Replace("'", "\'") + "';");
                tConstantsFile.AppendLine("$NWD_SHA_VEC = '" + DataSHAVector.Replace("'", "'") + "';");
                tConstantsFile.AppendLine("$NWD_SLT_STR = '" + SaltStart.Replace("'", "\'") + "';");
                tConstantsFile.AppendLine("$NWD_SLT_END = '" + SaltEnd.Replace("'", "\'") + "';");
                tConstantsFile.AppendLine("" + NWD.K_NWD_SLT_SRV + " = '" + tNWDCluster.SaltServer.Decrypt().Replace("'", "\'") + "';");
                tConstantsFile.AppendLine(NWD.K_CommentSeparator);
                tConstantsFile.AppendLine("// CONSTANT FOR TEMPORAL SALT");
                tConstantsFile.AppendLine("$NWD_SLT_TMP = " + SaltFrequency.ToString() + ";");
                tConstantsFile.AppendLine(NWD.K_CommentSeparator);
                tConstantsFile.AppendLine("// CONSTANT FOR SMTP");
                if (string.IsNullOrEmpty(tNWDCluster.MailHost))
                {
                    tConstantsFile.AppendLine("$SMTP_HOST = ''; // NOT DEFINED");
                }
                else
                {
                    tConstantsFile.AppendLine("$SMTP_HOST = '" + tNWDCluster.MailHost.Trim().Replace("'", "\'") + "';");
                }
                if (string.IsNullOrEmpty(tNWDCluster.MailPort.ToString()))
                {
                    tConstantsFile.AppendLine("$SMTP_PORT = ''; // NOT DEFINED");
                }
                else
                {
                    tConstantsFile.AppendLine("$SMTP_PORT = " + tNWDCluster.MailPort.ToString() + ";");
                }
                if (string.IsNullOrEmpty(tNWDCluster.MailFrom))
                {
                    tConstantsFile.AppendLine("$SMTP_FROM = ''; // NOT DEFINED");
                }
                else
                {
                    tConstantsFile.AppendLine("$SMTP_FROM = '" + tNWDCluster.MailFrom.Trim().Replace("'", "\'") + "';");
                }
                if (string.IsNullOrEmpty(tNWDCluster.MailUserName))
                {
                    tConstantsFile.AppendLine("$SMTP_USER = ''; // NOT DEFINED");
                }
                else
                {
                    tConstantsFile.AppendLine("$SMTP_PSW = '" + tNWDCluster.MailUserName.Trim().Replace("'", "\'") + "';");
                }
                if (string.IsNullOrEmpty(tNWDCluster.MailPassword.Decrypt()))
                {
                    tConstantsFile.AppendLine("$SMTP_PSW = ''; // NOT DEFINED");
                }
                else
                {
                    tConstantsFile.AppendLine("$SMTP_PSW = '" + tNWDCluster.MailPassword.Decrypt().Trim().Replace("'", "\'") + "';");
                }
                tConstantsFile.AppendLine(NWD.K_CommentSeparator);
                tConstantsFile.AppendLine("// CONSTANT TO CONNECT TO SQL DATABASE");
                tConstantsFile.AppendLine("global $K_ConnectAllDatabases;");
                tConstantsFile.AppendLine("$K_ConnectAllDatabases = false;");
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
                if (LogMode != NWDEnvironmentLogMode.NoLog)
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
                tConstantsFile.AppendLine("$NWD_ADM_KEY = '" + NWDCluster.SelectClusterforEnvironment(this, false).AdminKey.Decrypt().Replace("'", "\'") + "';");
                tConstantsFile.AppendLine(NWD.K_CommentSeparator);
                tConstantsFile.AppendLine("$NWD_RES_MAIL = '" + NWDCluster.SelectClusterforEnvironment(this, false).RescueEmail + "';");
                tConstantsFile.AppendLine("$NWD_APP_PRO = '" + AppProtocol.Replace("'", "\'") + "';");
                tConstantsFile.AppendLine("$NWD_APP_NAM = '" + AppName.Replace("'", "\'") + "';");
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
                tConstantsFile.AppendLine("global $UserRange;");
                tConstantsFile.AppendLine("$UserRange = -1;");
                tConstantsFile.AppendLine("global $admin;");
                tConstantsFile.AppendLine("$admin = false;");
                tConstantsFile.AppendLine(NWD.K_CommentSeparator);
                tConstantsFile.AppendLine("?>");
                string tFileFormatted = NWDToolbox.CSharpFormat(tConstantsFile.ToString());
                sFilesAndDatas.Add(EngFolder(sWriteOnDisk) + NWD.K_CONSTANTS_FILE, tFileFormatted);
                //NWDBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPManagementFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWDBenchmark.Start();
            LoadDataNecessary();
            StringBuilder tManagementFile = new StringBuilder(string.Empty);
            tManagementFile.AppendLine("<?php");
            tManagementFile.AppendLine(Headlines());
            if (LogMode != NWDEnvironmentLogMode.NoLog)
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
            foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                if (tHelper.TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
                {
                    string tClassName = NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP;
                    tManagementFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_DB + "/" + tClassName + "/" + NWD.K_MANAGEMENT_FILE + "');");
                    tManagementFile.AppendLine(NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_CREATE_TABLE() + "();");
                }
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPWebservicesFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWDBenchmark.Start();
            LoadDataNecessary();
            StringBuilder tWebServices = new StringBuilder(string.Empty);
            tWebServices.AppendLine("<?php");
            tWebServices.AppendLine(Headlines());
            if (LogMode != NWDEnvironmentLogMode.NoLog)
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
            tWebServices.AppendLine("$saltui = GetAccountSalt($uuid);");
            // I need include ALL tables management files to manage ALL tables
            foreach (Type tType in NWDDataManager.SharedInstance().ClassSynchronizeList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                if (tHelper.TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
                {
                    tWebServices.AppendLine("if (isset($dico['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']))\n{");
                    tWebServices.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
                    tWebServices.AppendLine("" + NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_SYNCHRONIZE() + " ($dico, $uuid, $saltui, $admin);");
                    tWebServices.AppendLine("}");
                }
            }
            tWebServices.AppendLine("}");
            tWebServices.AppendLine("}");
            tWebServices.AppendLine(NWD.K_CommentSeparator);
            tWebServices.AppendLine("// finish the generic process");
            tWebServices.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FINISH_PHP + "');");
            tWebServices.AppendLine(NWD.K_CommentSeparator);
            tWebServices.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tWebServices.ToString());
            sFilesAndDatas.Add(EnvFolder(sWriteOnDisk) + NWD.K_WS_FILE, tFileFormatted);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPRescueFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWDBenchmark.Start();
            LoadDataNecessary();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            if (LogMode != NWDEnvironmentLogMode.NoLog)
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPAuthenticationFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWDBenchmark.Start();
            LoadDataNecessary();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            if (LogMode != NWDEnvironmentLogMode.NoLog)
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
            tFile.AppendLine("$saltui = GetAccountSalt($uuid);");
            foreach (Type tType in NWDDataManager.SharedInstance().ClassSynchronizeList)
            {
                tFile.AppendLine("if (isset($dico['" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "']))\n{");
                tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.FindTypeInfos(tType).ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "');");
                tFile.AppendLine("" + NWDBasisHelper.FindTypeInfos(tType).PHP_FUNCTION_SYNCHRONIZE() + " ($dico, $uuid, $saltui, $admin);");
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
            tFile.AppendLine("$tNewUuid = FindAccount($HeaderUUID, $sdki, false);");
            tFile.AppendLine("if ($HeaderUUID != $tNewUuid)");
            tFile.AppendLine("{");
            tFile.AppendLine("// respondUUID($tNewUuid);");
            tFile.AppendLine("NWDRequestTokenDeleteAllToken($HeaderUUID); // delete old tokens");
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
            tFile.AppendLine("$tNewUuid = FindAccount('" + NWDAccount.ServerFakeAccount + "', $sdki, true);");
            tFile.AppendLine("if ($HeaderUUID != $tNewUuid)");
            tFile.AppendLine("{");
            tFile.AppendLine("NWDRequestTokenDeleteAllToken($HeaderUUID);  // delete old tokens");
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
            tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
            tFile.AppendLine("{");
            tFile.AppendLine("if(TestBanAccount($uuid) == false)");
            tFile.AppendLine("{");
            tFile.AppendLine("// get all datas for this account with reset sync... as force pull all!");

            foreach (Type tType in NWDDataManager.SharedInstance().ClassAccountDependentList)
            {
                if (NWDDataManager.SharedInstance().ClassSynchronizeList.Contains(tType))
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPBlankFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWDBenchmark.Start();
            LoadDataNecessary();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            if (LogMode != NWDEnvironmentLogMode.NoLog)
            {
                tFile.AppendLine("error_reporting (E_ALL);");
                tFile.AppendLine("ini_set ('display_errors', 1);");
            }
            tFile.AppendLine("// BLANK");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("$REP['blank'] = 'true';");
            tFile.AppendLine("$REP['" + NWD.K_JSON_PERFORM_REQUEST_KEY + "'] = microtime(true)-$_SERVER['REQUEST_TIME_FLOAT'];");
            tFile.AppendLine("$json = json_encode($REP);");
            tFile.AppendLine("echo($json);");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            sFilesAndDatas.Add(EnvFolder(sWriteOnDisk) + NWD.K_BLANK_PHP, tFileFormatted);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPIndexFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWDBenchmark.Start();
            LoadDataNecessary();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            if (LogMode != NWDEnvironmentLogMode.NoLog)
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPMaintenanceFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWDBenchmark.Start();
            LoadDataNecessary();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            if (LogMode != NWDEnvironmentLogMode.NoLog)
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPObsoleteFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWDBenchmark.Start();
            LoadDataNecessary();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(Headlines());
            if (LogMode != NWDEnvironmentLogMode.NoLog)
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void CreatePHPDotHTAccessFile(Dictionary<string, string> sFilesAndDatas, bool sWriteOnDisk = true)
        {
            //NWDBenchmark.Start();
            LoadDataNecessary();
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
