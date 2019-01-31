//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using SQLite4Unity3d;
using UnityEngine;
using BasicToolBox;
using System.Globalization;
using UnityEditor;
using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;
using System.Text;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_CreateAllError)]
        public static void CreateAllError()
        {
            // Create error in local data base
            string tClassName = Datas().ClassTableName;
            string tTrigramme = Datas().ClassTrigramme;
            NWDError.CreateGenericError(tClassName, tTrigramme + "x01", "Error in " + tClassName, "error in request creation in " + tClassName + "", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x02", "Error in " + tClassName, "error in request creation add primary key in " + tClassName + "", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x03", "Error in " + tClassName, "error in request creation add autoincrement modify in " + tClassName + "", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x05", "Error in " + tClassName, "error in sql index creation in " + tClassName + "", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x07", "Error in " + tClassName, "error in sql defragment in " + tClassName + "", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x08", "Error in " + tClassName, "error in sql drop in " + tClassName + "", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x09", "Error in " + tClassName, "error in sql Flush in " + tClassName + "", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x11", "Error in " + tClassName, "error in sql add columns in " + tClassName + "", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x12", "Error in " + tClassName, "error in sql alter columns in " + tClassName + "", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x31", "Error in " + tClassName, "error in request insert new datas before update in " + tClassName + " (update table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x32", "Error in " + tClassName, "error in request select datas to update in " + tClassName + " (update table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x33", "Error in " + tClassName, "error in request select updatable datas in " + tClassName + " (update table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x38", "Error in " + tClassName, "error in request update datas in " + tClassName + " (update table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x39", "Error in " + tClassName, "error more than one row for this reference in  " + tClassName + "", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x40", "Error in " + tClassName, "error in flush trashed in  " + tClassName + "", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x91", "Error in " + tClassName, "error update integrity in " + tClassName + " (update table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x99", "Error in " + tClassName, "error columns number in " + tClassName + " (update table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x88", "Error in " + tClassName, "integrity of one datas is false, break in " + tClassName + "", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(tClassName, tTrigramme + "x77", "Error in " + tClassName, "error update log in " + tClassName + " (update table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);

        }
        //-------------------------------------------------------------------------------------------------------------
        //public static void CreateDevPHPForOnlyThisClass()
        //{
        //    BTBBenchmark.Start();
        //    /*
        //    CreateAllError();
        //    bool rRegenrateCSharp = false;
        //    if (ModelChanged() == true)
        //    {
        //        rRegenrateCSharp = true;
        //        Debug.LogWarning(NWDConstants.K_APP_BASIS_WARNING_MODEL);
        //    }
        //    bool sFromFileWrting = false;
        //    string tTitle = Datas().ClassTableName + " WS Regenerate";
        //    string tMessage = "...?...";
        //    EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.0F);
        //    if (sFromFileWrting == true)
        //    {
        //        tMessage = "Generate file on disk and copy result on server.";
        //        CreateAllPHP("_modify", true, false);
        //        EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.5f);
        //        NWDAppConfiguration.SharedInstance().DevEnvironment.SendFileWS("_modify", Datas().ClassTableName);
        //        tMessage = "Copy result on server Dev.";
        //        EditorUtility.DisplayProgressBar(tTitle, tMessage, 1.00f);
        //    }
        //    else
        //    {
        //        tMessage = "Generate and directly copy on server.";
        //        EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.5f);
        //        Dictionary<string, string> tResultDev = CreatePHP(NWDAppConfiguration.SharedInstance().DevEnvironment, "", false, false);
        //        List<string> tFoldersDev = new List<string>();
        //        tFoldersDev.Add(NWD.K_ENV + "/" + NWDAppConfiguration.SharedInstance().DevEnvironment.Environment + "/" + NWD.K_ENV + "/" + NWD.K_DB + "/" + Datas().ClassNamePHP);
        //        NWDAppConfiguration.SharedInstance().DevEnvironment.SendFolderAndFiles(tFoldersDev, tResultDev, true);
        //        EditorUtility.DisplayProgressBar(tTitle, tMessage, 1.00f);
        //    }
        //    EditorUtility.ClearProgressBar();
        //    // RECOMPILE WITH THE NEW DATAS!
        //    if (rRegenrateCSharp == true)
        //    {
        //        NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
        //    }
        //    */
        //    BTBBenchmark.Finish();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void CreateAllPHPForOnlyThisClass()
        //{
        //    BTBBenchmark.Start();
        //    /*
        //    CreateAllError();
        //    bool rRegenrateCSharp = false;
        //    if (ModelChanged() == true)
        //    {
        //        rRegenrateCSharp = true;
        //        Debug.LogWarning(NWDConstants.K_APP_BASIS_WARNING_MODEL);
        //    }
        //    bool sFromFileWrting = false;
        //    string tTitle = Datas().ClassTableName + " WS Regenerate";
        //    string tMessage = "...?...";
        //    EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.0F);
        //    if (sFromFileWrting == true)
        //    {
        //        tMessage = "Generate file on disk and copy result on server.";
        //        CreateAllPHP("_modify", true, false);
        //        EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.25f);
        //        NWDAppConfiguration.SharedInstance().DevEnvironment.SendFileWS("_modify", Datas().ClassTableName);
        //        tMessage = "Copy result on server Dev.";
        //        EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.50f);
        //        NWDAppConfiguration.SharedInstance().PreprodEnvironment.SendFileWS("_modify", Datas().ClassTableName);
        //        tMessage = "Copy result on server Preprod.";
        //        EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.75f);
        //        NWDAppConfiguration.SharedInstance().ProdEnvironment.SendFileWS("_modify", Datas().ClassTableName);
        //        tMessage = "Copy result on server Prod.";
        //        EditorUtility.DisplayProgressBar(tTitle, tMessage, 1.00f);
        //    }
        //    else
        //    {
        //        tMessage = "Generate and directly copy on server.";
        //        EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.25f);
        //        Dictionary<string, string> tResultDev = CreatePHP(NWDAppConfiguration.SharedInstance().DevEnvironment, "", false, false);
        //        List<string> tFoldersDev = new List<string>();
        //        tFoldersDev.Add(NWD.K_ENV + "/" + NWDAppConfiguration.SharedInstance().DevEnvironment.Environment + "/" + NWD.K_ENV + "/" + NWD.K_DB + "/" + Datas().ClassNamePHP);
        //        NWDAppConfiguration.SharedInstance().DevEnvironment.SendFolderAndFiles(tFoldersDev, tResultDev, true);
        //        EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.50f);
        //        Dictionary<string, string> tResultPreprod = CreatePHP(NWDAppConfiguration.SharedInstance().PreprodEnvironment, "", false);
        //        List<string> tFoldersPreprod = new List<string>();
        //        tFoldersPreprod.Add(NWD.K_ENV + "/" + NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment + "/" + NWD.K_ENV + "/" + NWD.K_DB + "/" + Datas().ClassNamePHP);
        //        NWDAppConfiguration.SharedInstance().PreprodEnvironment.SendFolderAndFiles(tFoldersPreprod, tResultPreprod, true);
        //        EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.75f);
        //        Dictionary<string, string> tResultProd = CreatePHP(NWDAppConfiguration.SharedInstance().ProdEnvironment, "", false);
        //        List<string> tFoldersProd = new List<string>();
        //        tFoldersProd.Add(NWD.K_ENV + "/" + NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment + "/" + NWD.K_ENV + "/" + NWD.K_DB + "/" + Datas().ClassNamePHP);
        //        NWDAppConfiguration.SharedInstance().ProdEnvironment.SendFolderAndFiles(tFoldersProd, tResultProd, true);
        //        EditorUtility.DisplayProgressBar(tTitle, tMessage, 1.00f);
        //    }

        //    EditorUtility.ClearProgressBar();
        //    // RECOMPILE WITH THE NEW DATAS!
        //    if (rRegenrateCSharp == true)
        //    {
        //        NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
        //    }
        //    */
        //    BTBBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_CreateAllPHP)]
        //public static void CreateAllPHP(string sFolderAdd, bool sWriteOnDisk = true, bool sPrepareOrder = true)
        //{
        //    //BTBBenchmark.Start();
        //    foreach (NWDAppEnvironment tEnvironement in NWDAppConfiguration.SharedInstance().AllEnvironements())
        //    {
        //        CreatePHP(tEnvironement, sFolderAdd, sWriteOnDisk, sPrepareOrder);
        //    }
        //    //BTBBenchmark.Finish();
        //}
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_BasisCreatePHP)]
        public static Dictionary<string, string> CreatePHP(NWDAppEnvironment sEnvironment, string sFolderAdd = "", bool sWriteOnDisk = true, bool sPrepareOrder = true)
        {
            //BTBBenchmark.Start();
            Dictionary<string, string> rReturn = new Dictionary<string, string>();
            string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            string tEnvironmentFolder = sEnvironment.Environment;
            Type tType = ClassType();
            TableMapping tTableMapping = new TableMapping(tType);
            string tTableName = /*tEnvironmentFolder + "_" + */tTableMapping.TableName;
            string tClassName = tTableMapping.TableName;
            string tTrigramme = Datas().ClassTrigramme;
            DateTime tTime = DateTime.UtcNow;
            string tDateTimeString = NWDToolbox.DateTimeYYYYMMdd(tTime);
            string tYearString = NWDToolbox.DateTimeYYYY(tTime);
            //bool tInDevEnviroment = false;
            //if (NWDAppConfiguration.SharedInstance().DevEnvironment == sEnvironment)
            //{
            //    tInDevEnviroment = true;
            //}
            // Debug.Log("Create PHP file for " + tClassName + " in Environment " + sEnvironment.Environment);
            Datas().PrefLoad();
            if (sPrepareOrder == true)
            {
                PrepareOrders();
            }
            Dictionary<string, int> tResult = new Dictionary<string, int>();
            foreach (KeyValuePair<int, Dictionary<string, string>> tKeyValue in NWDAppConfiguration.SharedInstance().kWebBuildkSLQAssemblyOrder.OrderBy(x => x.Key))
            {
                if (NWDAppConfiguration.SharedInstance().WSList.ContainsKey(tKeyValue.Key) == true)
                {
                    if (NWDAppConfiguration.SharedInstance().WSList[tKeyValue.Key] == true)
                    {
                        foreach (KeyValuePair<string, string> tSubKeyValue in tKeyValue.Value.OrderBy(x => x.Key))
                        {
                            if (tResult.ContainsKey(tSubKeyValue.Key))
                            {
                                if (tResult[tSubKeyValue.Key] < tKeyValue.Key)
                                {
                                    tResult[tSubKeyValue.Key] = tKeyValue.Key;
                                }
                            }
                            else
                            {
                                tResult.Add(tSubKeyValue.Key, tKeyValue.Key);
                            }
                        }
                    }
                }
            }
            int tWebBuildUsed = NWDAppConfiguration.SharedInstance().WebBuild;
            //NWDAppConfiguration.SharedInstance().kLastWebBuildClass = new Dictionary<Type, int>();
            foreach (KeyValuePair<string, int> tKeyValue in tResult.OrderBy(x => x.Key))
            {
                if (tKeyValue.Key == Datas().ClassNamePHP)
                {
                    tWebBuildUsed = tKeyValue.Value;
                }
            }
            // Create folders
            //string tOwnerFolderServer = NWDToolbox.FindOwnerServerFolder();
            //string tServerRootFolder = tOwnerFolderServer + "/" + tWebServiceFolder + sFolderAdd + "/" + NWD.K_ENV + "/" + tEnvironmentFolder;
            //string tServerDatabaseFolder = tServerRootFolder + "/" + NWD.K_ENG + "/" + NWD.K_DB + "/" + tClassName;
            string tServerDatabaseFolder = tClassName;
            if (sWriteOnDisk)
            {
                //Directory.CreateDirectory(tServerDatabaseFolder);
                //AssetDatabase.ImportAsset(tServerDatabaseFolder);
            }
            if (AssetDatabase.IsValidFolder(tServerDatabaseFolder) == false)
            {
                // Debug.LogWarning("CreatePHP error : tServerDatabaseFolder not exists (" + tServerDatabaseFolder + ")");
            }
            string SLQIntegrityOrderToSelect = "***";
            foreach (string tPropertyName in SLQIntegrityOrder())
            {
                PropertyInfo tPropertyInfo = tType.GetProperty(tPropertyName, BindingFlags.Public | BindingFlags.Instance);
                Type tTypeOfThis = tPropertyInfo.PropertyType;
                if (tTypeOfThis == typeof(int) || tTypeOfThis == typeof(long))
                {
                    SLQIntegrityOrderToSelect += ", REPLACE(`" + tPropertyName + "`,\",\",\"\") as `" + tPropertyName + "`";
                }
                else if (tTypeOfThis == typeof(float))
                {
                    SLQIntegrityOrderToSelect += ", REPLACE(FORMAT(`" + tPropertyName + "`," + NWDConstants.FloatSQLFormat + "),\",\",\"\") as `" + tPropertyName + "`";
                }
                else if (tTypeOfThis == typeof(double))
                {
                    SLQIntegrityOrderToSelect += ", REPLACE(FORMAT(`" + tPropertyName + "`," + NWDConstants.DoubleSQLFormat + "),\",\",\"\") as `" + tPropertyName + "`";
                }
                else
                {
                    SLQIntegrityOrderToSelect += ", `" + tPropertyName + "`";
                }
            }
            SLQIntegrityOrderToSelect = SLQIntegrityOrderToSelect.Replace("***, ", "");

            //========= CONSTANTS FILE
            StringBuilder tConstantsFile = new StringBuilder(string.Empty);
            tConstantsFile.AppendLine("<?php");
            tConstantsFile.AppendLine(NWD.K_CommentAutogenerate + tDateTimeString);
            tConstantsFile.AppendLine(NWD.K_CommentCopyright + tYearString);
            tConstantsFile.AppendLine(NWD.K_CommentCreator);
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);

            tConstantsFile.AppendLine("// CONSTANTS");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);

            tConstantsFile.AppendLine("include_once ($PATH_BASE.'/" + NWD.K_ENG + "/functions.php');");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);

            // to bypass the global limitation of PHP in internal include : use function :-) 
            tConstantsFile.AppendLine("function " + tClassName + "Constants ()");
            tConstantsFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tConstantsFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            }
            tConstantsFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $SQL_" + tClassName + "_WebService;");
            tConstantsFile.AppendLine("$SQL_" + tClassName + "_SaltA = '" + Datas().SaltA + "';");
            tConstantsFile.AppendLine("$SQL_" + tClassName + "_SaltB = '" + Datas().SaltB + "';");
            tConstantsFile.AppendLine("$SQL_" + tClassName + "_WebService = " + tWebBuildUsed + ";");
            tConstantsFile.AppendLine("}");
            tConstantsFile.AppendLine("//Run this function to install globals of theses datas!");
            tConstantsFile.AppendLine(tClassName + "Constants();");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            // Create error in local data base
            //NWDError.CreateGenericError("RESCUE", "01", "{APP} : Forgotten password", "Hello,\r\n" +
            //                            "You forgot your password for the App {APP}'s account and ask to reset it." +
            //                            "If you didn't ask the reset, ignore it.\r\n" +
            //                            "Else, just click on this link to reset your password and receipt a new password by email: \r\n" +
            //                            "\r\n" +
            //                            "reset my password: {URL}\r\n" +
            //                            "\r\n" +
            //                            "Best regards,\r\n" +
            //                            "The {APP}'s team.", "OK");

            //NWDError.CreateGenericError("RESCUE", "02", "{APP} : Password rescue", "Hello,\r\n" +
            //                            "Your password was resetted!\r\n" +
            //                            "Best regards,\r\n" +
            //                            "The {APP}'s team.", "OK");

            //NWDError.CreateGenericError("RESCUE", "03", "{APP} : Password Resetted", "Hello,\r\n" +
            //"Your password for the App {APP}'s account was resetted to : \r\n" +
            //"\r\n" +
            //"{PASSWORD}\r\n" +
            //"\r\n" +
            //"Best regards,\r\n" +
            //"The {APP}'s team.", "OK");
            // craete constants of erro in php
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x01', 'error in request creation in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x02', 'error in request creation add primary key in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x03', 'error in request creation add autoincrement modify in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x04', 'error in sql index remove in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x05', 'error in sql index creation in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x07', 'error in sql defragment in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x08', 'error in sql drop in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x09', 'error in sql Flush in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x11', 'error in sql add columns in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x12', 'error in sql alter columns in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x31', 'error in request insert new datas before update in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x32', 'error in request select datas to update in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x33', 'error in request select updatable datas in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x38', 'error in request update datas in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x39', 'error too much datas for this reference in  " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x88', 'integrity of one datas is false, break in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x91', 'error update integrity in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x99', 'error columns number in " + tClassName + "');");
            tConstantsFile.AppendLine("errorDeclaration('" + tTrigramme + "x77', 'error update log in " + tClassName + "');");
            tConstantsFile.AppendLine(NWD.K_CommentSeparator);
            tConstantsFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tConstantsFile.ToString());
            //if (sWriteOnDisk)
            //{
            //    File.WriteAllText(tServerDatabaseFolder + "/" + NWD.K_CONSTANTS_FILE, tFileFormatted);
            //}
            rReturn.Add(/*NWD.K_ENV + "/" + tEnvironmentFolder + "/" + NWD.K_ENG + "/" + NWD.K_DB + "/" +*/ tClassName + "/" + NWD.K_CONSTANTS_FILE, tFileFormatted);

            //========= MANAGEMENT TABLE FUNCTIONS FILE
            StringBuilder tManagementFile = new StringBuilder(string.Empty);
            tManagementFile.AppendLine("<?php");
            tManagementFile.AppendLine(NWD.K_CommentAutogenerate + tDateTimeString);
            tManagementFile.AppendLine(NWD.K_CommentCopyright + tYearString);
            tManagementFile.AppendLine(NWD.K_CommentCreator);
            tManagementFile.AppendLine(NWD.K_CommentSeparator);

            tManagementFile.AppendLine("// TABLE MANAGEMENT");
            tManagementFile.AppendLine(NWD.K_CommentSeparator);

            tManagementFile.AppendLine("include_once ( $PATH_BASE.'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + tClassName + "/" + NWD.K_CONSTANTS_FILE + "');");
            tManagementFile.AppendLine("include_once ( $PATH_BASE.'/" + NWD.K_ENG + "/functions.php');");
            tManagementFile.AppendLine(NWD.K_CommentSeparator);

            tManagementFile.AppendLine("function Create" + tClassName + "Table () {");
            tManagementFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            tManagementFile.AppendLine("global $SQL_CON, $ENV;");
            var tQuery = "CREATE TABLE IF NOT EXISTS `'.$ENV.'_" + tTableName + "` (";
            var tDeclarations = tTableMapping.Columns.Select(p => Orm.SqlDecl(p, true));
            var tDeclarationsJoined = string.Join(",", tDeclarations.ToArray());
            tDeclarationsJoined = tDeclarationsJoined.Replace('"', '`');
            tDeclarationsJoined = tDeclarationsJoined.Replace("`ID` integer", "`ID` int(11) NOT NULL");
            tDeclarationsJoined = tDeclarationsJoined.Replace("`DC` integer", "`DC` int(11) NOT NULL DEFAULT 0");
            //tDeclarationsJoined = tDeclarationsJoined.Replace ("`AC` integer", "`AC` int(11) NOT NULL DEFAULT 1");
            tDeclarationsJoined = tDeclarationsJoined.Replace("`DM` integer", "`DM` int(11) NOT NULL DEFAULT 0");
            tDeclarationsJoined = tDeclarationsJoined.Replace("`DD` integer", "`DD` int(11) NOT NULL DEFAULT 0");
            tDeclarationsJoined = tDeclarationsJoined.Replace("`DS` integer", "`DS` int(11) NOT NULL DEFAULT 0");
            tDeclarationsJoined = tDeclarationsJoined.Replace("`XX` integer", "`XX` int(11) NOT NULL DEFAULT 0");
            tDeclarationsJoined = tDeclarationsJoined.Replace("varchar", "text CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL");
            tDeclarationsJoined = tDeclarationsJoined.Replace("primary key autoincrement not null", string.Empty);
            tQuery += tDeclarationsJoined;
            tQuery += ") ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
            tManagementFile.AppendLine("$tQuery = '" + tQuery + "';");
            tManagementFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tManagementFile.AppendLine("if (!$tResult)");
            tManagementFile.AppendLine("{");
            tManagementFile.AppendLine("error('" + tTrigramme + "x01');");
            tManagementFile.AppendLine("}");
            tManagementFile.AppendLine("$tQuery = 'ALTER TABLE `'.$ENV.'_" + tTableName + "` ADD PRIMARY KEY (`ID`), ADD UNIQUE KEY `ID` (`ID`);';");
            tManagementFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tManagementFile.AppendLine("$tQuery = 'ALTER TABLE `'.$ENV.'_" + tTableName + "` MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;';");
            tManagementFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tManagementFile.AppendLine("");
            tManagementFile.AppendLine("// Alter all existing table with new columns or change type columns");
            foreach (TableMapping.Column tColumn in tTableMapping.Columns)
            {
                if (tColumn.Name != "ID" &&
                    tColumn.Name != "Reference" &&
                    tColumn.Name != "DM" &&
                    tColumn.Name != "DC" &&
                    //                  tColumn.Name != "AC" &&
                    tColumn.Name != "DD" &&
                    tColumn.Name != "DS" &&
                    tColumn.Name != "XX")
                {
                    tManagementFile.Append("$tQuery ='ALTER TABLE `'.$ENV.'_" + tTableName + "` ADD " +
                        Orm.SqlDecl(tColumn, true).Replace(" varchar ", " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL ").Replace(" float ", " double ").Replace("\"", "`") +
                        ";';");
                    tManagementFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
                    tManagementFile.AppendLine("$tQuery ='ALTER TABLE `'.$ENV.'_" + tTableName + "` MODIFY " +
                        Orm.SqlDecl(tColumn, true).Replace(" varchar ", " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL ").Replace(" float ", " double ").Replace("\"", "`") +
                        ";';");
                    tManagementFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
                }
            }
            var indexes = new Dictionary<string, SQLite4Unity3d.SQLiteConnection.IndexInfo>();
            foreach (var c in tTableMapping.Columns)
            {
                foreach (var i in c.Indices)
                {
                    var iname = i.Name ?? tTableName + "_" + c.Name;
                    SQLite4Unity3d.SQLiteConnection.IndexInfo iinfo;
                    if (!indexes.TryGetValue(iname, out iinfo))
                    {
                        iinfo = new SQLite4Unity3d.SQLiteConnection.IndexInfo
                        {
                            IndexName = iname,
                            TableName = tTableName,
                            Unique = i.Unique,
                            Columns = new List<SQLite4Unity3d.SQLiteConnection.IndexedColumn>()
                        };
                        indexes.Add(iname, iinfo);
                    }
                    if (i.Unique != iinfo.Unique)
                        throw new Exception("All the columns in an index must have the same value for their Unique property");
                    iinfo.Columns.Add(new SQLite4Unity3d.SQLiteConnection.IndexedColumn
                    {
                        Order = i.Order,
                        ColumnName = c.Name
                    });
                }
            }
            foreach (var indexName in indexes.Keys)
            {
                var index = indexes[indexName];
                string[] columnNames = new string[index.Columns.Count];
                if (index.Columns.Count == 1)
                {
                    columnNames[0] = index.Columns[0].ColumnName;
                }
                else
                {
                    index.Columns.Sort((lhs, rhs) =>
                    {
                        return lhs.Order - rhs.Order;
                    });
                    for (int i = 0, end = index.Columns.Count; i < end; ++i)
                    {
                        columnNames[i] = index.Columns[i].ColumnName;
                    }
                }
                List<string> columnNamesFinalList = new List<string>();
                foreach (string tName in columnNames)
                {
                    PropertyInfo tColumnInfos = tType.GetProperty(tName);
                    Type tColumnType = tColumnInfos.PropertyType;
                    if (tColumnType.IsSubclassOf(typeof(BTBDataType)))
                    {
                        columnNamesFinalList.Add("`" + tName + "`(24)");
                    }
                    else if (tColumnType.IsSubclassOf(typeof(BTBDataTypeInt)))
                    {
                        columnNamesFinalList.Add("`" + tName + "`");
                    }
                    else if (tColumnType.IsSubclassOf(typeof(BTBDataTypeFloat)))
                    {
                        columnNamesFinalList.Add("`" + tName + "`");
                    }
                    else if (tColumnType == typeof(string))
                    {
                        columnNamesFinalList.Add("`" + tName + "`(24)");
                    }
                    else if (tColumnType == typeof(string))
                    {
                        columnNamesFinalList.Add("`" + tName + "`(32)");
                    }
                    else
                    {
                        columnNamesFinalList.Add("`" + tName + "`");
                    }
                }
                string[] columnNamesFinal = columnNamesFinalList.ToArray<string>();
                tManagementFile.AppendLine("$tRemoveIndexQuery = 'DROP INDEX `" + indexName + "` ON `'.$ENV.'_" + index.TableName + "`;';");
                tManagementFile.AppendLine("$tRemoveIndexResult = $SQL_CON->query($tRemoveIndexQuery);");
                const string sqlFormat = "CREATE {2}INDEX `{3}` ON `'.$ENV.'_{0}` ({1});";
                var sql = String.Format(sqlFormat, index.TableName, string.Join(", ", columnNamesFinal), index.Unique ? "UNIQUE " : "", indexName);
                tManagementFile.AppendLine("$tQuery = '" + sql + "';");
                tManagementFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
                tManagementFile.AppendLine("if (!$tResult)");
                tManagementFile.AppendLine("{");
                tManagementFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
                tManagementFile.AppendLine("error('" + tTrigramme + "x05');");
                tManagementFile.AppendLine("}");
            }
            tManagementFile.AppendLine("}");
            tManagementFile.AppendLine(NWD.K_CommentSeparator);

            tManagementFile.AppendLine("function Defragment" + tClassName + "Table ()");
            tManagementFile.AppendLine("{");
            tManagementFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            tManagementFile.AppendLine("global $SQL_CON, $ENV;");
            tManagementFile.AppendLine("$tQuery = 'ALTER TABLE `'.$ENV.'_" + tTableName + "` ENGINE=InnoDB;';");
            tManagementFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tManagementFile.AppendLine("if (!$tResult)");
            tManagementFile.AppendLine("{");
            tManagementFile.AppendLine("error('" + tTrigramme + "x07');");
            tManagementFile.AppendLine("}");
            tManagementFile.AppendLine("}");
            tManagementFile.AppendLine(NWD.K_CommentSeparator);

            tManagementFile.AppendLine("function Drop" + tClassName + "Table ()");
            tManagementFile.AppendLine("{");
            tManagementFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            tManagementFile.AppendLine("global $SQL_CON, $ENV;");
            tManagementFile.AppendLine("$tQuery = 'DROP TABLE `'.$ENV.'_" + tTableName + "`;';");
            tManagementFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tManagementFile.AppendLine("if (!$tResult)");
            tManagementFile.AppendLine("{");
            tManagementFile.AppendLine("error('" + tTrigramme + "x08');");
            tManagementFile.AppendLine("}");
            tManagementFile.AppendLine("}");
            tManagementFile.AppendLine(NWD.K_CommentSeparator);

            tManagementFile.AppendLine("function Flush" + tClassName + "Table ()");
            tManagementFile.AppendLine("{");
            tManagementFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            tManagementFile.AppendLine("global $SQL_CON, $ENV;");
            tManagementFile.AppendLine("$tQuery = 'FLUSH TABLE `'.$ENV.'_" + tTableName + "`;';");
            tManagementFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tManagementFile.AppendLine("if (!$tResult)");
            tManagementFile.AppendLine("{");
            tManagementFile.AppendLine("error('" + tTrigramme + "x09');");
            tManagementFile.AppendLine("}");
            tManagementFile.AppendLine("}");
            tManagementFile.AppendLine(NWD.K_CommentSeparator);

            tManagementFile.AppendLine("?>");
            string tManagementFileFormatted = NWDToolbox.CSharpFormat(tManagementFile.ToString());
            //if (sWriteOnDisk)
            //{
            //    File.WriteAllText(tServerDatabaseFolder + "/" + NWD.K_MANAGEMENT_FILE + "", tManagementFileFormatted);
            //}
            rReturn.Add(/*NWD.K_ENV + "/" + tEnvironmentFolder + "/" + NWD.K_ENG + "/" + NWD.K_DB + "/" +*/ tClassName + "/" + NWD.K_MANAGEMENT_FILE + "", tManagementFileFormatted);

            //========= SYNCHRONIZATION FUNCTIONS FILE
            // if need Account reference I prepare the restriction
            List<string> tAccountReference = new List<string>();
            List<string> tAccountReferences = new List<string>();
            foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                Type tTypeOfThis = tProp.PropertyType;
                if (tTypeOfThis != null)
                {
                    if (tTypeOfThis.IsGenericType)
                    {

                        if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceType<>))
                        {
                            Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                            if (tSubType == typeof(NWDAccount))
                            {
                                tAccountReference.Add("`" + tProp.Name + "` LIKE \\''.$SQL_CON->real_escape_string($sAccountReference).'\\' ");
                                tAccountReferences.Add("`" + tProp.Name + "` IN (\\''.implode('\\', \\'', $sAccountReferences).'\\') ");
                            }
                        }
                        else if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceHashType<>))
                        {
                            Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                            if (tSubType == typeof(NWDAccount))
                            {
                                tAccountReference.Add("`" + tProp.Name + "` LIKE \\''.$SQL_CON->real_escape_string(md5($sAccountReference.$SQL_" + tClassName + "_SaltA)).'\\' ");
                                tAccountReferences.Add("`" + tProp.Name + "` IN (\\''.implode('\\', \\'', $sAccountReferences).'\\') ");
                            }
                        }
                    }
                }
            }
            bool tINeedAdminAccount = true;
            if (tAccountReference.Count == 0)
            {
            }
            else
            {
                tINeedAdminAccount = false;
            }

            StringBuilder tSynchronizationFile = new StringBuilder(string.Empty);
            tSynchronizationFile.AppendLine("<?php");
            tSynchronizationFile.AppendLine(NWD.K_CommentAutogenerate + tDateTimeString);
            tSynchronizationFile.AppendLine(NWD.K_CommentCopyright + tYearString);
            tSynchronizationFile.AppendLine(NWD.K_CommentCreator);
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            tSynchronizationFile.AppendLine("// SYNCHRONIZATION FUNCTIONS");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            tSynchronizationFile.AppendLine("include_once ( $PATH_BASE.'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + tClassName + "/" + NWD.K_CONSTANTS_FILE + "');");
            tSynchronizationFile.AppendLine("include_once ($PATH_BASE.'/" + NWD.K_ENG + "/functions.php');");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            tSynchronizationFile.AppendLine("function Integrity" + tClassName + "Test ($sCsv)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;");
            tSynchronizationFile.AppendLine("$rReturn = true;");
            tSynchronizationFile.AppendLine("$sCsvList = explode('" + NWDConstants.kStandardSeparator + "',$sCsv);");
            tSynchronizationFile.AppendLine("$tIntegrity = array_pop($sCsvList);");
            tSynchronizationFile.AppendLine("unset($sCsvList[2]);//remove DS");
            tSynchronizationFile.AppendLine("unset($sCsvList[3]);//remove DevSync");
            tSynchronizationFile.AppendLine("unset($sCsvList[4]);//remove PreprodSync");
            tSynchronizationFile.AppendLine("unset($sCsvList[5]);//remove ProdSync");
            tSynchronizationFile.AppendLine("$sDataString = implode('',$sCsvList);");
            tSynchronizationFile.AppendLine("$tCalculate = str_replace('" + NWDConstants.kStandardSeparator + "', '', md5($SQL_" + tClassName + "_SaltA.$sDataString.$SQL_" + tClassName + "_SaltB));");
            tSynchronizationFile.AppendLine("if ($tCalculate!=$tIntegrity)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$rReturn = false;");
            tSynchronizationFile.AppendLine("error('" + tTrigramme + "x88');");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("return $rReturn;");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            tSynchronizationFile.AppendLine("function Integrity" + tClassName + "Replace ($sCsvArray, $sIndex, $sValue)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;");
            tSynchronizationFile.AppendLine("$sCsvList = $sCsvArray;");
            tSynchronizationFile.AppendLine("$sCsvList[$sIndex] = $sValue;");
            tSynchronizationFile.AppendLine("$tIntegrity = array_pop($sCsvList);");
            tSynchronizationFile.AppendLine("unset($sCsvList[2]);//remove DS");
            tSynchronizationFile.AppendLine("unset($sCsvList[3]);//remove DevSync");
            tSynchronizationFile.AppendLine("unset($sCsvList[4]);//remove PreprodSync");
            tSynchronizationFile.AppendLine("unset($sCsvList[5]);//remove ProdSync");
            tSynchronizationFile.AppendLine("$sDataString = implode('',$sCsvList);");
            tSynchronizationFile.AppendLine("$tCalculate = str_replace('|', '', md5($SQL_" + tClassName + "_SaltA.$sDataString.$SQL_" + tClassName + "_SaltB));");
            tSynchronizationFile.AppendLine("$sCsvArray[$sIndex] = $sValue;");
            tSynchronizationFile.AppendLine("array_pop($sCsvArray);");
            tSynchronizationFile.AppendLine("$sCsvArray[] = $tCalculate;");
            tSynchronizationFile.AppendLine("return $sCsvArray;");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            tSynchronizationFile.AppendLine("function Integrity" + tClassName + "Replaces ($sCsvArray, $sIndexesAndValues)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;");
            tSynchronizationFile.AppendLine("$sCsvList = $sCsvArray;");
            tSynchronizationFile.AppendLine("foreach(array_keys($sIndexesAndValues) as $tKey)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$sCsvList[$tKey] = $sIndexesAndValues[$tKey];");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("$tIntegrity = array_pop($sCsvList);");
            tSynchronizationFile.AppendLine("unset($sCsvList[2]);//remove DS");
            tSynchronizationFile.AppendLine("unset($sCsvList[3]);//remove DevSync");
            tSynchronizationFile.AppendLine("unset($sCsvList[4]);//remove PreprodSync");
            tSynchronizationFile.AppendLine("unset($sCsvList[5]);//remove ProdSync");
            tSynchronizationFile.AppendLine("$sDataString = implode('',$sCsvList);");
            tSynchronizationFile.AppendLine("$tCalculate = str_replace('|', '', md5($SQL_" + tClassName + "_SaltA.$sDataString.$SQL_" + tClassName + "_SaltB));");
            tSynchronizationFile.AppendLine("foreach(array_keys($sIndexesAndValues) as $tKey)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$sCsvArray[$tKey] = $sIndexesAndValues[$tKey];");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("array_pop($sCsvArray);");
            tSynchronizationFile.AppendLine("$sCsvArray[] = $tCalculate;");
            tSynchronizationFile.AppendLine("return $sCsvArray;");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            tSynchronizationFile.AppendLine("function Prepare" + tClassName + "Data ($sCsv)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $TIME_SYNC;");
            tSynchronizationFile.AppendLine("$sCsvList = explode('" + NWDConstants.kStandardSeparator + "',$sCsv);");
            tSynchronizationFile.AppendLine("$sCsvList[2] = $TIME_SYNC;// change DS");
            tSynchronizationFile.AppendLine("if ($sCsvList[1]<$TIME_SYNC)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$sCsvList[2] = $sCsvList[1];");
            tSynchronizationFile.AppendLine("}");
            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                tSynchronizationFile.AppendLine("$sCsvList[3] = $TIME_SYNC;// change DevSync");
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                tSynchronizationFile.AppendLine("$sCsvList[4] = $TIME_SYNC;// change PreprodSync");
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                tSynchronizationFile.AppendLine("$sCsvList[5] = $TIME_SYNC;// change ProdSync");
            }
            tSynchronizationFile.AppendLine("return $sCsvList;");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);


            tSynchronizationFile.AppendLine("function Log" + tClassName + " ($sReference, $sLog)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("global $SQL_CON, $ENV;");
            tSynchronizationFile.AppendLine("$tUpdate = 'UPDATE `'.$ENV.'_" + tTableName + "` SET `ServerLog` = CONCAT(`ServerLog`, \\' ; '.$SQL_CON->real_escape_string($sLog).'\\') WHERE `Reference` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';");
            tSynchronizationFile.AppendLine("$tUpdateResult = $SQL_CON->query($tUpdate);");
            tSynchronizationFile.AppendLine("if (!$tUpdateResult)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tUpdate.'', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("error('" + tTrigramme + "x77');");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            // SERVER Integrity generate
            tSynchronizationFile.AppendLine("function IntegrityServer" + tClassName + "Generate ($sRow)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("global $NWD_SLT_SRV;");
            tSynchronizationFile.Append("$sDataServerString =''");
            foreach (string tPropertyName in SLQIntegrityServerOrder())
            {
                tSynchronizationFile.Append(".$sRow['" + tPropertyName + "']");
            }
            tSynchronizationFile.AppendLine(";");
            tSynchronizationFile.AppendLine("return str_replace('" + NWDConstants.kStandardSeparator + "', '', md5($NWD_SLT_SRV.$sDataServerString.$NWD_SLT_SRV));");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            // DATA Integrity generate
            tSynchronizationFile.AppendLine("function Integrity" + tClassName + "Generate ($sRow)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("global $SQL_CON, $ENV, $NWD_SLT_SRV;");
            tSynchronizationFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;");
            tSynchronizationFile.Append("$sDataString =''");
            foreach (string tPropertyName in SLQIntegrityOrder())
            {
                tSynchronizationFile.Append(".$sRow['" + tPropertyName + "']");
            }
            tSynchronizationFile.AppendLine(";");
            tSynchronizationFile.AppendLine("myLog('sDataString : '.$sDataString.'', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("return str_replace('" + NWDConstants.kStandardSeparator + "', '', md5($SQL_" + tClassName + "_SaltA.$sDataString.$SQL_" + tClassName + "_SaltB));");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            // TODO refactor to be more easy to generate
            tSynchronizationFile.AppendLine("function Integrity" + tClassName + "Reevalue ($sReference)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $NWD_SLT_SRV, $TIME_SYNC, $NWD_FLOAT_FORMAT;");
            tSynchronizationFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $SQL_" + tClassName + "_WebService;");
            tSynchronizationFile.AppendLine("$tQuery = 'SELECT " + SLQIntegrityOrderToSelect + " FROM `'.$ENV.'_" + tTableName + "` WHERE `Reference` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';");
            tSynchronizationFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tSynchronizationFile.AppendLine("if (!$tResult)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("error('" + tTrigramme + "x31');");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("else");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("if ($tResult->num_rows == 1)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("// I calculate the integrity and reinject the good value");
            tSynchronizationFile.AppendLine("$tRow = $tResult->fetch_assoc();");
            //"$tRow['WebServiceVersion'] = $WSBUILD;" );
            tSynchronizationFile.AppendLine("$tRow['WebServiceVersion'] = $SQL_" + tClassName + "_WebService;");
            tSynchronizationFile.AppendLine("$tCalculate = Integrity" + tClassName + "Generate ($tRow);");
            tSynchronizationFile.AppendLine("$tCalculateServer = IntegrityServer" + tClassName + "Generate ($tRow);");
            tSynchronizationFile.AppendLine("$tUpdate = 'UPDATE `'.$ENV.'_" + tTableName + "` SET `Integrity` = \\''.$SQL_CON->real_escape_string($tCalculate).'\\',");
            tSynchronizationFile.Append(" `ServerHash` = \\''.$SQL_CON->real_escape_string($tCalculateServer).'\\',");
            tSynchronizationFile.Append(" `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\' ,");
            //tSynchronizationFile.Append(" `WebServiceVersion` = \\''.$WSBUILD.'\\'" );
            tSynchronizationFile.AppendLine(" `WebServiceVersion` = \\''.$SQL_" + tClassName + "_WebService.'\\'" + " WHERE `Reference` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';");
            tSynchronizationFile.AppendLine("$tUpdateResult = $SQL_CON->query($tUpdate);");
            tSynchronizationFile.AppendLine("if (!$tUpdateResult)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tUpdate.'', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("error('" + tTrigramme + "x91');");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            // TODO refactor to be more easy to generate
            tSynchronizationFile.AppendLine("function IntegrityServer" + tClassName + "Validate ($sReference)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("global $SQL_CON, $ENV, $NWD_SLT_SRV;");
            tSynchronizationFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;");
            tSynchronizationFile.AppendLine("$tQuery = 'SELECT " + SLQAssemblyOrder() + " FROM `'.$ENV.'_" + tTableName + "` WHERE `Reference` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';");
            tSynchronizationFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tSynchronizationFile.AppendLine("if (!$tResult)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("error('" + tTrigramme + "x31');");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("else");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("if ($tResult->num_rows == 1)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("// I calculate the integrity and reinject the good value");
            tSynchronizationFile.AppendLine("$tRow = $tResult->fetch_assoc();");
            tSynchronizationFile.AppendLine("$tCalculateServer = IntegrityServer" + tClassName + "Generate ($tRow);");
            tSynchronizationFile.AppendLine("if ($tCalculateServer == $tRow['ServerHash'])");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("return true;");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("return false;");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            // TODO refactor to be more easy to generate
            tSynchronizationFile.AppendLine("function IntegrityServer" + tClassName + "ValidateByRow ($sRow)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("global $NWD_SLT_SRV;");
            tSynchronizationFile.AppendLine("$tCalculateServer = IntegrityServer" + tClassName + "Generate ($sRow);");
            tSynchronizationFile.AppendLine("if ($tCalculateServer == $sRow['ServerHash'])");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("return true;");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("return false;");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            // TODO refactor to be more easy to generate
            tSynchronizationFile.AppendLine("function Integrity" + tClassName + "Validate ($sReference)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("global $SQL_CON, $ENV, $NWD_SLT_SRV;");
            tSynchronizationFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;");
            tSynchronizationFile.AppendLine("$tQuery = 'SELECT " + SLQAssemblyOrder() + " FROM `'.$ENV.'_" + tTableName + "` WHERE `Reference` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';");
            tSynchronizationFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tSynchronizationFile.AppendLine("if (!$tResult)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("error('" + tTrigramme + "x31');");
            tSynchronizationFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("else");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("if ($tResult->num_rows == 1)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("// I calculate the integrity and reinject the good value");
            tSynchronizationFile.AppendLine("$tRow = $tResult->fetch_assoc();");
            tSynchronizationFile.AppendLine("$tCalculate =Integrity" + tClassName + "Generate ($tRow);");
            tSynchronizationFile.AppendLine("if ($tCalculate == $tRow['Integrity'])");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("return true;");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("return false;");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);



            // TODO refactor to be more easy to generate

            tSynchronizationFile.AppendLine("function Integrity" + tClassName + "ValidateByRow ($sRow)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $NWD_SLT_SRV;");
            tSynchronizationFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB,$SQL_" + tClassName + "_WebService;");
            tSynchronizationFile.AppendLine("$tCalculate =Integrity" + tClassName + "Generate ($sRow);");
            tSynchronizationFile.AppendLine("if ($tCalculate == $sRow['Integrity'])");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("return true;");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("return false;");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);



            tSynchronizationFile.AppendLine("function UpdateData" + tClassName + " ($sCsv, $sTimeStamp, $sAccountReference, $sAdmin)");
            List<string> tModify = new List<string>();
            List<string> tColumnNameList = new List<string>();
            List<string> tColumnValueList = new List<string>();
            tColumnNameList.Add("`Reference`");
            tColumnValueList.Add("\\''.$SQL_CON->real_escape_string($sCsvList[0]).'\\'");
            int tIndex = 1;
            foreach (string tProperty in SLQAssemblyOrderArray())
            {
                tModify.Add("`" + tProperty + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + tIndex.ToString() + "]).'\\'");
                tColumnNameList.Add("`" + tProperty + "`");
                tColumnValueList.Add("\\''.$SQL_CON->real_escape_string($sCsvList[" + tIndex.ToString() + "]).'\\'");
                tIndex++;
            }

            MethodInfo tMethodDeclareFunctions = NWDAliasMethod.GetMethod(tType, NWDConstants.M_AddonPhpFunctions, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (tMethodDeclareFunctions != null)
            {
                tSynchronizationFile.Append((string)tMethodDeclareFunctions.Invoke(null, new object[] { sEnvironment }));
            }
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $NWD_SLT_SRV, $TIME_SYNC, $NWD_FLOAT_FORMAT, $ACC_NEEDED, $PATH_BASE, $REF_NEEDED, $REP;");
            tSynchronizationFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $SQL_" + tClassName + "_WebService;");
            tSynchronizationFile.AppendLine("global $admin, $uuid;");
            tSynchronizationFile.AppendLine("if (Integrity" + tClassName + "Test ($sCsv) == true)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$sCsvList = Prepare" + tClassName + "Data($sCsv);");
            tSynchronizationFile.AppendLine("if (count ($sCsvList) != " + tColumnNameList.Count.ToString() + ")");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("error('" + tTrigramme + "x99');");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("else");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$tReference = $sCsvList[0];");
            tSynchronizationFile.AppendLine("// find solution for pre calculate on server");

            MethodInfo tMethodDeclarePre = NWDAliasMethod.GetMethod(tType, NWDConstants.M_AddonPhpPreCalculate, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (tMethodDeclarePre != null)
            {
                tSynchronizationFile.Append((string)tMethodDeclarePre.Invoke(null, new object[] { sEnvironment }));
            }
            tSynchronizationFile.AppendLine("$tQuery = 'SELECT `Reference`, `DM` FROM `'.$ENV.'_" + tTableName + "` WHERE `Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\';';");
            tSynchronizationFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tSynchronizationFile.AppendLine("if (!$tResult)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("error('" + tTrigramme + "x31');");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("else");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("if ($tResult->num_rows <= 1)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("if ($tResult->num_rows == 0)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$tInsert = 'INSERT INTO `'.$ENV.'_" + tTableName + "` (" + string.Join(", ", tColumnNameList.ToArray()) + ") VALUES (" + string.Join(", ", tColumnValueList.ToArray()) + ");';");
            tSynchronizationFile.AppendLine("$tInsertResult = $SQL_CON->query($tInsert);");
            tSynchronizationFile.AppendLine("if (!$tInsertResult)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tInsertResult.'', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("error('" + tTrigramme + "x32');");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("else");
            tSynchronizationFile.AppendLine("{");
            //"while($tRow = $tResult->fetch_row())"+
            //"{" );
            tSynchronizationFile.Append("$tUpdate = 'UPDATE `'.$ENV.'_" + tTableName + "` SET ");
            tSynchronizationFile.Append(string.Join(", ", tModify.ToArray()) + " WHERE `Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\' ");
            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                // tSynchronizationFile += "AND (`DevSync`<= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\') "; 
                //no test the last is the winner!
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                //tSynchronizationFile += "AND (`DevSync`<= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\' || `PreprodSync`<= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\') ";
                //tSynchronizationFile += "AND `PreprodSync`<= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\' ";
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                //tSynchronizationFile += "AND (`DevSync`<= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\' || `PreprodSync`<= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\' || `ProdSync`<= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\') ";
                //tSynchronizationFile += "AND `ProdSync`<= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\' ";
            }
            tSynchronizationFile.AppendLine("';");
            if (tAccountReference.Count == 0)
            {
                tSynchronizationFile.AppendLine("$tUpdateRestriction = '';");
            }
            else
            {
                tSynchronizationFile.AppendLine("$tUpdateRestriction = 'AND (" + string.Join(" OR ", tAccountReference.ToArray()) + ") ';");
            }
            tSynchronizationFile.AppendLine("if ($admin == false)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$tUpdate = $tUpdate.$tUpdateRestriction.' AND `WebServiceVersion` <= '.$WSBUILD.'';");
            tSynchronizationFile.AppendLine("}");
            //"else" );
            //"{" );
            //"//$tUpdate = $tUpdate.' AND `DM`<= \\''.$SQL_CON->real_escape_string($sCsvList[1]).'\\'';" );
            //"}" );
            tSynchronizationFile.AppendLine("$tUpdateResult = $SQL_CON->query($tUpdate);");
            tSynchronizationFile.AppendLine("if (!$tUpdateResult)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tUpdate.'', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("error('" + tTrigramme + "x38');");
            tSynchronizationFile.AppendLine("}");
            //"}" );
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("// find solution for post calculate on server");
            MethodInfo tMethodDeclarePost = NWDAliasMethod.GetMethod(tType, NWDConstants.M_AddonPhpPostCalculate, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (tMethodDeclarePost != null)
            {
                tSynchronizationFile.Append((string)tMethodDeclarePost.Invoke(null, new object[] { sEnvironment }));
            }
            tSynchronizationFile.AppendLine("");
            tSynchronizationFile.AppendLine("$tLigneAffceted = $SQL_CON->affected_rows;");
            //"myLog('tLigneAffceted = '.$tLigneAffceted, __FILE__, __FUNCTION__, __LINE__);" );
            tSynchronizationFile.AppendLine("if ($tLigneAffceted == 1)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("// je transmet la sync à tout le monde");
            tSynchronizationFile.AppendLine("if ($sCsvList[3] != -1)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$tUpdate = 'UPDATE `Dev_" + tTableName + "` SET `DS` = \\''.$TIME_SYNC.'\\',  `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\' WHERE `Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\'';");
            tSynchronizationFile.AppendLine("$tUpdateResult = $SQL_CON->query($tUpdate);");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("if ($sCsvList[4] != -1)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$tUpdate = 'UPDATE `Preprod_" + tTableName + "` SET `DS` = \\''.$TIME_SYNC.'\\',  `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\' WHERE `Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\'';");
            tSynchronizationFile.AppendLine("$tUpdateResult = $SQL_CON->query($tUpdate);");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("if ($sCsvList[5] != -1)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$tUpdate = 'UPDATE `Prod_" + tTableName + "` SET `DS` = \\''.$TIME_SYNC.'\\',  `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\' WHERE `Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\'';");
            tSynchronizationFile.AppendLine("$tUpdateResult = $SQL_CON->query($tUpdate);");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");

            tSynchronizationFile.AppendLine("");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("else");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("error('" + tTrigramme + "x39');");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("mysqli_free_result($tResult);");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            tSynchronizationFile.AppendLine("function FlushTrashedDatas" + tClassName + " ()");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("global $SQL_CON, $ENV;");
            tSynchronizationFile.AppendLine("$tQuery = 'DELETE FROM `'.$ENV.'_" + tTableName + "` WHERE XX>0';");
            tSynchronizationFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tSynchronizationFile.AppendLine("if (!$tResult)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("error('" + tTrigramme + "x40');");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            tSynchronizationFile.AppendLine("function GetDatas" + tClassName + "ByReference ($sReference)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $REF_NEEDED, $ACC_NEEDED, $uuid;");
            tSynchronizationFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB,$SQL_" + tClassName + "_WebService;");
            tSynchronizationFile.AppendLine("global $REP;");
            tSynchronizationFile.AppendLine("global $admin;");
            //"$tPage = $sPage*$sLimit;" );
            tSynchronizationFile.AppendLine("$tQuery = 'SELECT " + SLQAssemblyOrder() + " FROM `'.$ENV.'_" + tTableName + "` WHERE Reference = \\''.$SQL_CON->real_escape_string($sReference).'\\'';");
            tSynchronizationFile.AppendLine("if ($admin == false)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$tQuery = $tQuery.' AND `WebServiceVersion` <= '.$WSBUILD.';';");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tSynchronizationFile.AppendLine("if (!$tResult)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("error('" + tTrigramme + "x33');");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("else");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("while($tRow = $tResult->fetch_row())");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$REP['" + tClassName + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");
            tSynchronizationFile.AppendLine("}");
            string tSpecialAdd = string.Empty;
            foreach (PropertyInfo tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (tProp.GetCustomAttributes(typeof(NWDNeedUserAvatarAttribute), true).Length > 0)
                {
                    tSpecialAdd += NWDNeedUserAvatarAttribute.PHPstring(tProp.Name);
                }
                if (tProp.GetCustomAttributes(typeof(NWDNeedAccountNicknameAttribute), true).Length > 0)
                {
                    tSpecialAdd += NWDNeedAccountNicknameAttribute.PHPstring(tProp.Name);
                }
                if (tProp.GetCustomAttributes(typeof(NWDNeedReferenceAttribute), true).Length > 0)
                {
                    foreach (NWDNeedReferenceAttribute tReference in tProp.GetCustomAttributes(typeof(NWDNeedReferenceAttribute), true))
                    {
                        tSpecialAdd += tReference.PHPstring(tProp.Name);
                    }
                }
            }
            if (tSpecialAdd != string.Empty)
            {
                tSynchronizationFile.AppendLine("$tResult->data_seek(0);while($tRow = $tResult->fetch_assoc()){" + tSpecialAdd + "}");
            }
            tSynchronizationFile.AppendLine("mysqli_free_result($tResult);");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            tSynchronizationFile.AppendLine("function GetDatas" + tClassName + "ByReferences ($sReferences)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $REF_NEEDED, $ACC_NEEDED, $uuid;");
            tSynchronizationFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB,$SQL_" + tClassName + "_WebService;");
            tSynchronizationFile.AppendLine("global $REP;");
            tSynchronizationFile.AppendLine("global $admin;");
            //"$tPage = $sPage*$sLimit;" );
            tSynchronizationFile.AppendLine("$tQuery = 'SELECT " + SLQAssemblyOrder() + " FROM `'.$ENV.'_" + tTableName + "` WHERE Reference IN ( \\''.implode('\\', \\'', $sReferences).'\\')';");
            tSynchronizationFile.AppendLine("if ($admin == false)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$tQuery = $tQuery.' AND `WebServiceVersion` <= '.$WSBUILD.';';");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tSynchronizationFile.AppendLine("if (!$tResult)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("error('" + tTrigramme + "x33');");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("else");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("while($tRow = $tResult->fetch_row())");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$REP['" + tClassName + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");
            tSynchronizationFile.AppendLine("}");
            if (tSpecialAdd != string.Empty)
            {
                tSynchronizationFile.AppendLine("$tResult->data_seek(0);while($tRow = $tResult->fetch_assoc()){" + tSpecialAdd + "}");
            }

            tSynchronizationFile.AppendLine("mysqli_free_result($tResult);");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            tSynchronizationFile.AppendLine("function GetDatas" + tClassName + " ($sTimeStamp, $sAccountReference)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $REF_NEEDED, $ACC_NEEDED, $uuid;");
            tSynchronizationFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB,$SQL_" + tClassName + "_WebService;");
            tSynchronizationFile.AppendLine("global $REP;");
            tSynchronizationFile.AppendLine("global $admin;");
            //"$tPage = $sPage*$sLimit;" );
            tSynchronizationFile.Append("$tQuery = 'SELECT " + SLQAssemblyOrder() + " FROM `'.$ENV.'_" + tTableName + "` WHERE ");
            //"(`'.$ENV.'Sync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\' OR `DS` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\')";
            tSynchronizationFile.Append("(`'.$ENV.'Sync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\')");
            // if need Account reference
            if (tAccountReference.Count == 0)
            {
            }
            else
            {
                tSynchronizationFile.Append("AND (" + string.Join("OR ", tAccountReference.ToArray()) + ") ");
            }
            tSynchronizationFile.AppendLine("';");
            tSynchronizationFile.AppendLine("if ($admin == false)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$tQuery = $tQuery.' AND `WebServiceVersion` <= '.$WSBUILD.';';");
            tSynchronizationFile.AppendLine("}");
            // I do the result operation
            tSynchronizationFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tSynchronizationFile.AppendLine("if (!$tResult)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("error('" + tTrigramme + "x33');");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("else");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("while($tRow = $tResult->fetch_row())");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$REP['" + tClassName + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");
            tSynchronizationFile.AppendLine("}");
            if (tSpecialAdd != string.Empty)
            {
                tSynchronizationFile.AppendLine("$tResult->data_seek(0);while($tRow = $tResult->fetch_assoc()){" + tSpecialAdd + "}");
            }
            tSynchronizationFile.AppendLine("mysqli_free_result($tResult);");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            tSynchronizationFile.AppendLine("function GetDatas" + tClassName + "ByAccounts ($sTimeStamp, $sAccountReferences)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $REF_NEEDED, $ACC_NEEDED, $uuid;");
            tSynchronizationFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $SQL_" + tClassName + "_WebService;");
            tSynchronizationFile.AppendLine("global $REP;");
            //"$tPage = $sPage*$sLimit;" );
            tSynchronizationFile.Append("$tQuery = 'SELECT " + SLQAssemblyOrder() + " FROM `'.$ENV.'_" + tTableName + "` WHERE ");
            //"(`'.$ENV.'Sync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\' OR `DS` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\')";
            tSynchronizationFile.Append("(`'.$ENV.'Sync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\')");
            // if need Account reference
            if (tAccountReferences.Count == 0)
            {
            }
            else
            {
                tSynchronizationFile.Append("AND (" + string.Join("OR ", tAccountReferences.ToArray()) + ") ");
            }
            tSynchronizationFile.AppendLine(" AND `WebServiceVersion` <= '.$SQL_" + tClassName + "_WebService.';';");
            // I do the result operation
            tSynchronizationFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tSynchronizationFile.AppendLine("if (!$tResult)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("error('" + tTrigramme + "x33');");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("else");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("while($tRow = $tResult->fetch_row())");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$REP['" + tClassName + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");
            tSynchronizationFile.AppendLine("}");
            if (tSpecialAdd != string.Empty)
            {
                tSynchronizationFile.AppendLine("$tResult->data_seek(0);while($tRow = $tResult->fetch_assoc()){" + tSpecialAdd + "}");
            }
            tSynchronizationFile.AppendLine("mysqli_free_result($tResult);");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            tSynchronizationFile.AppendLine("function Special" + tClassName + " ($sTimeStamp, $sAccountReferences)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $NWD_SLT_SRV, $TIME_SYNC, $NWD_FLOAT_FORMAT, $ACC_NEEDED, $PATH_BASE, $REF_NEEDED, $REP;");
            tSynchronizationFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $SQL_" + tClassName + "_WebService;");
            tSynchronizationFile.AppendLine("global $admin, $uuid;");
            MethodInfo tMethodDeclareSpecial = NWDAliasMethod.GetMethod(tType, NWDConstants.M_AddonPhpSpecialCalculate, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (tMethodDeclareSpecial != null)
            {
                tSynchronizationFile.Append((string)tMethodDeclareSpecial.Invoke(null, new object[] { sEnvironment }));
            }
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);


            tSynchronizationFile.AppendLine("function Synchronize" + tClassName + " ($sJsonDico, $sAccountReference, $sAdmin)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("global $token_FirstUse,$PATH_BASE;");

            if (tType.GetCustomAttributes(typeof(NWDForceSecureDataAttribute), true).Length > 0)
            {
                tSynchronizationFile.AppendLine("respondAdd('securePost',true);");
            }

            NWDOperationSpecial tOperation = NWDOperationSpecial.None;
            tSynchronizationFile.AppendLine("if ($sAdmin == true)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("$sAccountReference = '%';");

            // Clean data?
            tOperation = NWDOperationSpecial.Clean;
            tSynchronizationFile.AppendLine("if (isset($sJsonDico['" + tClassName + "']['" + tOperation.ToString().ToLower() + "']))");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("if (!errorDetected())");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("FlushTrashedDatas" + tClassName + " ();");
            tSynchronizationFile.AppendLine("myLog('SPECIAL : CLEAN', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");

            //Special?
            tOperation = NWDOperationSpecial.Special;
            tSynchronizationFile.AppendLine("if (isset($sJsonDico['" + tClassName + "']['" + tOperation.ToString().ToLower() + "']))");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("if (!errorDetected())");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("Special" + tClassName + " ($sJsonDico['" + tClassName + "']['sync'], $sAccountReference);");
            tSynchronizationFile.AppendLine("myLog('SPECIAL : SPECIAL', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");

            //Upgrade?
            tOperation = NWDOperationSpecial.Upgrade;
            tSynchronizationFile.AppendLine("if (isset($sJsonDico['" + tClassName + "']['" + tOperation.ToString().ToLower() + "']))");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("if (!errorDetected())");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("include_once ($PATH_BASE.'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + tClassName + "/" + NWD.K_MANAGEMENT_FILE + "');");
            tSynchronizationFile.AppendLine("Create" + tClassName + "Table ();");
            tSynchronizationFile.AppendLine("myLog('SPECIAL : UPGRADE OR CREATE TABLE', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");

            //Optimize?
            tOperation = NWDOperationSpecial.Optimize;
            tSynchronizationFile.AppendLine("if (isset($sJsonDico['" + tClassName + "']['" + tOperation.ToString().ToLower() + "']))");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("if (!errorDetected())");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("include_once ($PATH_BASE.'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + tClassName + "/" + NWD.K_MANAGEMENT_FILE + "');");
            tSynchronizationFile.AppendLine("Defragment" + tClassName + "Table ();");
            tSynchronizationFile.AppendLine("myLog('SPECIAL : OPTIMIZE AND DEFRAGMENT TABLE', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");

            // ENDIF ADMIN OPERATION
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("if ($token_FirstUse == true)");
            tSynchronizationFile.AppendLine("{");

            if (tINeedAdminAccount == true)
            {
                tSynchronizationFile.AppendLine("if ($sAdmin == true){");
            }
            tSynchronizationFile.AppendLine("if (isset($sJsonDico['" + tClassName + "']))");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("if (isset($sJsonDico['" + tClassName + "']['data']))");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("foreach ($sJsonDico['" + tClassName + "']['data'] as $sCsvValue)");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("if (!errorDetected())");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("UpdateData" + tClassName + " ($sCsvValue, $sJsonDico['" + tClassName + "']['sync'], $sAccountReference, $sAdmin);");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            if (tINeedAdminAccount == true)
            {
                tSynchronizationFile.AppendLine("}");
            }
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("else");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("myLog('NOT UPDATE, SPECIAL OR CLEAN ACTION ... YOU USE OLDEST TOKEN', __FILE__, __FUNCTION__, __LINE__);");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("if (isset($sJsonDico['" + tClassName + "']))");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("if (isset($sJsonDico['" + tClassName + "']['sync']))");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("if (!errorDetected())");
            tSynchronizationFile.AppendLine("{");
            tSynchronizationFile.AppendLine("GetDatas" + tClassName + " ($sJsonDico['" + tClassName + "']['sync'], $sAccountReference);");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine("}");
            tSynchronizationFile.AppendLine(NWD.K_CommentSeparator);

            tSynchronizationFile.AppendLine("?>");

            string tSynchronizationFileFormatted = NWDToolbox.CSharpFormat(tSynchronizationFile.ToString());

            //if (sWriteOnDisk)
            //{
            //    File.WriteAllText(tServerDatabaseFolder + "/" + NWD.K_WS_SYNCHRONISATION, tSynchronizationFileFormatted);
            //}
            rReturn.Add(/*NWD.K_ENV + "/" + tEnvironmentFolder + "/" + NWD.K_ENG + "/" + NWD.K_DB + "/" +*/ tClassName + "/" + NWD.K_WS_SYNCHRONISATION, tSynchronizationFileFormatted);
            // force to import this file by Unity3D
            // AssetDatabase.ImportAsset(tServerDatabaseFolder + "/"+NWDAppEnvironment.K_WS_SYNCHRONISATION+"");


            /*
            //========= WEBSERVICE FILE
            string tWebService = "" +
            "<?php" );
            "//NWD Autogenerate File at " + tDateTimeString + "" );
            "//Copyright NetWorkedDatas ideMobi " + tYearString + "" );
            "//Created by Jean-François CONTART" );
            "//--------------------" );
            "// WEBSERVICES FUNCTIONS" );
            "//--------------------" );
            "// Determine the file tree path" );
            "$PATH_BASE = dirname(dirname(__DIR__));" );
            "// include all necessary files" );
            "include_once ($PATH_BASE.'/Environment/" + sEnvironment.Environment + "/Engine/"+ NWDAppEnvironment.K_CONSTANTS_FILE + "');" );
            "// start the generic process" );
            "include_once ($PATH_BASE.'/Engine/start.php');" );
            "// start the script" );
            "//--------------------" );
            "global $dico, $uuid;" );
            "//--------------------" );
            "// Ok I create a permanent account if temporary before" );
            "AccountAnonymeGenerate();" );
            "//--------------------" );
            "if ($ban == true)" );
            "{" );
            "error('ACC99');" );
            "}" );
            "//--------------------" );
            "if (!errorDetected())" );
            "{" );
            "include_once ($PATH_BASE.'/Environment/" + sEnvironment.Environment + "/Engine/Database/" + tClassName + "/synchronization.php');" );
            "Synchronize" + tClassName + " ($dico, $uuid, $admin);" );
            "}" );
            "//--------------------" );
            "// script is finished" );
            "// finish the generic process" );
            "include_once ($PATH_BASE.'/Engine/finish.php');" );
            "" );
            "?>";
            File.WriteAllText(tServerRootFolder + "/" + tClassName + "Webservice.php", tWebService);
            // force to import this file by Unity3D
            AssetDatabase.ImportAsset(tServerRootFolder + "/" + tClassName + "Webservice.php");
            // try to create special file for the special operation in PHP
            */

            //BTBBenchmark.Finish();

            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif