//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

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

#if UNITY_EDITOR
using UnityEditor;

using Renci.SshNet;
using Renci.SshNet.Common;
using Renci.SshNet.Sftp;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        //public static Dictionary<string, string> kTableNameList = new Dictionary<string, string>();
        ////-------------------------------------------------------------------------------------------------------------
        //public static string ClassNamePHP()
        //{
        //    string rReturn = "";
        //    if (kTableNameList.ContainsKey(ClassID()))
        //    {
        //        rReturn = kTableNameList[ClassID()];
        //    }
        //    else
        //    {
        //        Type tType = ClassType();
        //        TableMapping tTableMapping = new TableMapping(tType);
        //        string rClassName = tTableMapping.TableName;
        //        rReturn = rClassName;
        //        kTableNameList.Add(ClassID(), rClassName);
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
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

        }//-------------------------------------------------------------------------------------------------------------
        public static void CreateDevPHPForOnlyThisClass()
        {
            BTBBenchmark.Start();
            CreateAllError();
            bool rRegenrateCSharp = false;
           if (ModelChanged() == true)
           {
                //PrepareOrders();
                rRegenrateCSharp = true;
                Debug.LogWarning(NWDConstants.K_APP_BASIS_WARNING_MODEL);

            //    //TODO clean integrity fonction and regenerate PHP
            //    kPropertiesOrderArray.Remove(ClassID());
            //kCSVAssemblyOrderArray.Remove(ClassID());
            //kSLQAssemblyOrderArray.Remove(ClassID());
            //kSLQAssemblyOrder.Remove(ClassID());
            //kSLQIntegrityOrder.Remove(ClassID());
            //kSLQIntegrityServerOrder.Remove(ClassID());
            //kDataAssemblyPropertiesList.Remove(ClassID());

            //if (NWDAppConfiguration.SharedInstance().kWebBuildkCSVAssemblyOrderArray.ContainsKey(NWDAppConfiguration.SharedInstance().WebBuild))
            //{
            //    NWDAppConfiguration.SharedInstance().kWebBuildkCSVAssemblyOrderArray[NWDAppConfiguration.SharedInstance().WebBuild].Remove(ClassID());
            //}
            //if (NWDAppConfiguration.SharedInstance().kWebBuildkSLQAssemblyOrderArray.ContainsKey(NWDAppConfiguration.SharedInstance().WebBuild))
            //{
            //    NWDAppConfiguration.SharedInstance().kWebBuildkSLQAssemblyOrderArray[NWDAppConfiguration.SharedInstance().WebBuild].Remove(ClassID());
            //}
            //if (NWDAppConfiguration.SharedInstance().kWebBuildkSLQAssemblyOrder.ContainsKey(NWDAppConfiguration.SharedInstance().WebBuild))
            //{
            //    NWDAppConfiguration.SharedInstance().kWebBuildkSLQAssemblyOrder[NWDAppConfiguration.SharedInstance().WebBuild].Remove(ClassID());
            //}
            //if (NWDAppConfiguration.SharedInstance().kWebBuildkSLQIntegrityOrder.ContainsKey(NWDAppConfiguration.SharedInstance().WebBuild))
            //{
            //    NWDAppConfiguration.SharedInstance().kWebBuildkSLQIntegrityOrder[NWDAppConfiguration.SharedInstance().WebBuild].Remove(ClassID());
            //}
            //if (NWDAppConfiguration.SharedInstance().kWebBuildkSLQIntegrityServerOrder.ContainsKey(NWDAppConfiguration.SharedInstance().WebBuild))
            //{
            //    NWDAppConfiguration.SharedInstance().kWebBuildkSLQIntegrityServerOrder[NWDAppConfiguration.SharedInstance().WebBuild].Remove(ClassID());
            //}
            //if (NWDAppConfiguration.SharedInstance().kWebBuildkDataAssemblyPropertiesList.ContainsKey(NWDAppConfiguration.SharedInstance().WebBuild))
            //{
            //    NWDAppConfiguration.SharedInstance().kWebBuildkDataAssemblyPropertiesList[NWDAppConfiguration.SharedInstance().WebBuild].Remove(ClassID());
            //}

            }
            bool sFromFileWrting = false;
            string tTitle = Datas().ClassTableName + " WS Regenerate";
            string tMessage = "...?...";
            EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.0F);
            if (sFromFileWrting == true)
            {
                tMessage = "Generate file on disk and copy result on server.";
                CreateAllPHP("_modify", true, false);
                EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.5f);
                NWDAppConfiguration.SharedInstance().DevEnvironment.SendFileWS("_modify", Datas().ClassTableName);
                tMessage = "Copy result on server Dev.";
                EditorUtility.DisplayProgressBar(tTitle, tMessage, 1.00f);
            }
            else
            {
                tMessage = "Generate and directly copy on server.";
                EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.5f);
                Dictionary<string, string> tResultDev = CreatePHP(NWDAppConfiguration.SharedInstance().DevEnvironment, "", false, false);
                List<string> tFoldersDev = new List<string>();
                tFoldersDev.Add("Environment/" + NWDAppConfiguration.SharedInstance().DevEnvironment.Environment + "/Engine/Database/" + Datas().ClassNamePHP);
                NWDAppConfiguration.SharedInstance().DevEnvironment.SendFolderAndFiles(tFoldersDev, tResultDev, true);
                EditorUtility.DisplayProgressBar(tTitle, tMessage, 1.00f);
            }

            EditorUtility.ClearProgressBar();
            BTBBenchmark.Finish();
            // RECOMPILE WITH THE NEW DATAS!
            if (rRegenrateCSharp == true)
            {
                NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void CreateAllPHPForOnlyThisClass()
        {
            BTBBenchmark.Start();
            CreateAllError();
            bool rRegenrateCSharp = false;
            if (ModelChanged() == true)
            {
                //PrepareOrders();
                rRegenrateCSharp = true;
                Debug.LogWarning(NWDConstants.K_APP_BASIS_WARNING_MODEL);

                ////TODO clean integrity fonction and regenerate PHP
                //kPropertiesOrderArray.Remove(ClassID());
                //kCSVAssemblyOrderArray.Remove(ClassID());
                //kSLQAssemblyOrderArray.Remove(ClassID());
                //kSLQAssemblyOrder.Remove(ClassID());
                //kSLQIntegrityOrder.Remove(ClassID());
                //kSLQIntegrityServerOrder.Remove(ClassID());
                //kDataAssemblyPropertiesList.Remove(ClassID());

                //if (NWDAppConfiguration.SharedInstance().kWebBuildkCSVAssemblyOrderArray.ContainsKey(NWDAppConfiguration.SharedInstance().WebBuild))
                //{
                //    NWDAppConfiguration.SharedInstance().kWebBuildkCSVAssemblyOrderArray[NWDAppConfiguration.SharedInstance().WebBuild].Remove(ClassID());
                //}
                //if (NWDAppConfiguration.SharedInstance().kWebBuildkSLQAssemblyOrderArray.ContainsKey(NWDAppConfiguration.SharedInstance().WebBuild))
                //{
                //    NWDAppConfiguration.SharedInstance().kWebBuildkSLQAssemblyOrderArray[NWDAppConfiguration.SharedInstance().WebBuild].Remove(ClassID());
                //}
                //if (NWDAppConfiguration.SharedInstance().kWebBuildkSLQAssemblyOrder.ContainsKey(NWDAppConfiguration.SharedInstance().WebBuild))
                //{
                //    NWDAppConfiguration.SharedInstance().kWebBuildkSLQAssemblyOrder[NWDAppConfiguration.SharedInstance().WebBuild].Remove(ClassID());
                //}
                //if (NWDAppConfiguration.SharedInstance().kWebBuildkSLQIntegrityOrder.ContainsKey(NWDAppConfiguration.SharedInstance().WebBuild))
                //{
                //    NWDAppConfiguration.SharedInstance().kWebBuildkSLQIntegrityOrder[NWDAppConfiguration.SharedInstance().WebBuild].Remove(ClassID());
                //}
                //if (NWDAppConfiguration.SharedInstance().kWebBuildkSLQIntegrityServerOrder.ContainsKey(NWDAppConfiguration.SharedInstance().WebBuild))
                //{
                //    NWDAppConfiguration.SharedInstance().kWebBuildkSLQIntegrityServerOrder[NWDAppConfiguration.SharedInstance().WebBuild].Remove(ClassID());
                //}
                //if (NWDAppConfiguration.SharedInstance().kWebBuildkDataAssemblyPropertiesList.ContainsKey(NWDAppConfiguration.SharedInstance().WebBuild))
                //{
                //    NWDAppConfiguration.SharedInstance().kWebBuildkDataAssemblyPropertiesList[NWDAppConfiguration.SharedInstance().WebBuild].Remove(ClassID());
                //}
            }
            bool sFromFileWrting = false;
            string tTitle = Datas().ClassTableName+" WS Regenerate";
            string tMessage = "...?...";
            EditorUtility.DisplayProgressBar(tTitle, tMessage,0.0F);
            if (sFromFileWrting == true)
            {
                tMessage = "Generate file on disk and copy result on server.";
                CreateAllPHP("_modify", true, false);
                EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.25f);
                NWDAppConfiguration.SharedInstance().DevEnvironment.SendFileWS("_modify", Datas().ClassTableName);
                tMessage = "Copy result on server Dev.";
                EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.50f);
                NWDAppConfiguration.SharedInstance().PreprodEnvironment.SendFileWS("_modify", Datas().ClassTableName);
                tMessage = "Copy result on server Preprod.";
                EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.75f);
                NWDAppConfiguration.SharedInstance().ProdEnvironment.SendFileWS("_modify", Datas().ClassTableName);
                tMessage = "Copy result on server Prod.";
                EditorUtility.DisplayProgressBar(tTitle, tMessage, 1.00f);
            }
            else
            {
                tMessage = "Generate and directly copy on server.";
                EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.25f);
                Dictionary<string, string> tResultDev = CreatePHP(NWDAppConfiguration.SharedInstance().DevEnvironment, "", false, false);
                List<string> tFoldersDev = new List<string>();
                tFoldersDev.Add("Environment/" + NWDAppConfiguration.SharedInstance().DevEnvironment.Environment + "/Engine/Database/" + Datas().ClassNamePHP);
                NWDAppConfiguration.SharedInstance().DevEnvironment.SendFolderAndFiles(tFoldersDev, tResultDev, true);
                EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.50f);
                Dictionary<string, string> tResultPreprod = CreatePHP(NWDAppConfiguration.SharedInstance().PreprodEnvironment, "", false);
                List<string> tFoldersPreprod = new List<string>();
                tFoldersPreprod.Add("Environment/" + NWDAppConfiguration.SharedInstance().PreprodEnvironment.Environment + "/Engine/Database/" + Datas().ClassNamePHP);
                NWDAppConfiguration.SharedInstance().PreprodEnvironment.SendFolderAndFiles(tFoldersPreprod, tResultPreprod, true);
                EditorUtility.DisplayProgressBar(tTitle, tMessage, 0.75f);
                Dictionary<string, string> tResultProd = CreatePHP(NWDAppConfiguration.SharedInstance().ProdEnvironment, "", false);
                List<string> tFoldersProd = new List<string>();
                tFoldersProd.Add("Environment/" + NWDAppConfiguration.SharedInstance().ProdEnvironment.Environment + "/Engine/Database/" + Datas().ClassNamePHP);
                NWDAppConfiguration.SharedInstance().ProdEnvironment.SendFolderAndFiles(tFoldersProd, tResultProd, true);
                EditorUtility.DisplayProgressBar(tTitle, tMessage, 1.00f);
            }

            EditorUtility.ClearProgressBar();
            BTBBenchmark.Finish();
            // RECOMPILE WITH THE NEW DATAS!
            if (rRegenrateCSharp == true)
            {
                NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void CreateAllPHP(string sFolderAdd, bool sWriteOnDisk = true, bool sPrepareOrder = true)
        {
            //BTBBenchmark.Start();
            foreach (NWDAppEnvironment tEnvironement in NWDAppConfiguration.SharedInstance().AllEnvironements())
            {
                CreatePHP(tEnvironement, sFolderAdd, sWriteOnDisk, sPrepareOrder);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
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
            string tDateTimeString = tTime.ToString("yyyy-MM-dd", NWDConstants.FormatCountry);
            string tYearString = tTime.ToString("yyyy", NWDConstants.FormatCountry);
            bool tInDevEnviroment = false;
            if (NWDAppConfiguration.SharedInstance().DevEnvironment == sEnvironment)
            {
                tInDevEnviroment = true;
            }
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

            string tOwnerFolderServer = NWDToolbox.FindOwnerServerFolder();
            string tServerRootFolder = tOwnerFolderServer + "/" + tWebServiceFolder + sFolderAdd + "/Environment/" + tEnvironmentFolder;
            string tServerDatabaseFolder = tServerRootFolder + "/Engine/Database/" + tClassName;
            if (sWriteOnDisk)
            {
                Directory.CreateDirectory(tServerDatabaseFolder);
                AssetDatabase.ImportAsset(tServerDatabaseFolder);
            }
            /*
            if (AssetDatabase.IsValidFolder(tOwnerFolderServer + "/" + tWebServiceFolder) == false)
            {
                AssetDatabase.CreateFolder(tOwnerFolderServer, tWebServiceFolder);
                AssetDatabase.ImportAsset(tOwnerFolderServer + "/" + tWebServiceFolder);
            }
            if (AssetDatabase.IsValidFolder(tOwnerFolderServer + "/" + tWebServiceFolder + "/Environment") == false)
            {
                AssetDatabase.CreateFolder(tOwnerFolderServer + "/" + tWebServiceFolder, "Environment");
                AssetDatabase.ImportAsset(tOwnerFolderServer + "/" + tWebServiceFolder + "/Environment");
            }
            if (AssetDatabase.IsValidFolder(tOwnerFolderServer + "/" + tWebServiceFolder + "/Environment/" + tEnvironmentFolder) == false)
            {
                AssetDatabase.CreateFolder(tOwnerFolderServer + "/" + tWebServiceFolder + "/Environment/", tEnvironmentFolder);
                AssetDatabase.ImportAsset(tOwnerFolderServer + "/" + tWebServiceFolder + "/Environment/" + tEnvironmentFolder);
            }
            // tServerRootFolder is created 
            if (AssetDatabase.IsValidFolder(tServerRootFolder + "/Engine") == false)
            {
                AssetDatabase.CreateFolder(tServerRootFolder, "Engine");
                AssetDatabase.ImportAsset(tServerRootFolder + "/Engine");
            }
            if (AssetDatabase.IsValidFolder(tServerRootFolder + "/Engine/Database") == false)
            {
                AssetDatabase.CreateFolder(tServerRootFolder + "/Engine", "Database");
                AssetDatabase.ImportAsset(tServerRootFolder + "/Engine/Database");
            }
            if (AssetDatabase.IsValidFolder(tServerRootFolder + "/Engine/Database/" + tClassName) == false)
            {
                AssetDatabase.CreateFolder(tServerRootFolder + "/Engine/Database", tClassName);
                AssetDatabase.ImportAsset(tServerRootFolder + "/Engine/Database/" + tClassName);
            }
            */
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
                    SLQIntegrityOrderToSelect += ", REPLACE(`" + tPropertyName + "`,\",\",\"\") as " + tPropertyName;
                }
                else if (tTypeOfThis == typeof(float) || tTypeOfThis == typeof(double))
                {
                    SLQIntegrityOrderToSelect += ", REPLACE(FORMAT(`" + tPropertyName + "`," + NWDConstants.FloatSQLFormat + "),\",\",\"\") as " + tPropertyName;
                }
                else
                {
                    SLQIntegrityOrderToSelect += ", `" + tPropertyName+"`";
                }
            }
            SLQIntegrityOrderToSelect = SLQIntegrityOrderToSelect.Replace("***, ","");


            //========= CONSTANTS FILE
            string tConstantsFile = "<?php\n" +
                                    "//NWD Autogenerate File at " + tDateTimeString + "\n" +
                                    "//Copyright NetWorkedDatas ideMobi " + tYearString + "\n" +
                                    "//Created by Jean-François CONTART\n" +
                                    "//-------------------- \n" +
                                    "// CONSTANTS \n" +
                                    "//-------------------- \n" +
                                    "include_once ($PATH_BASE.'/Engine/functions.php');\n" +
                                    "//-------------------- \n" +
                                    // to bypass the global limitation of PHP in internal include : use function :-) 
                                    "function " + tClassName + "Constants ()\n" +
                                    "{\n" +
                                    "myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);\n" +
                                    "global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $SQL_" + tClassName + "_WebService;\n" +
                                    "$SQL_" + tClassName + "_SaltA = '" + Datas().SaltA + "';\n" +
                                    "$SQL_" + tClassName + "_SaltB = '" + Datas().SaltB + "';\n" +
                                    "";



            //if (NWDAppConfiguration.SharedInstance().kLastWebBuildClass.ContainsKey(ClassType()))
            //{
            //    tWebBuildUsed = NWDAppConfiguration.SharedInstance().kLastWebBuildClass[ClassType()];
            //}

            //tWebBuildUsed = NWDAppConfiguration.SharedInstance().WebBuild;

            tConstantsFile += "$SQL_" + tClassName + "_WebService = " + tWebBuildUsed + ";\n" +
                "}\n" +
                "//Run this function to install globals of theses datas!\n" +
                "" + tClassName + "Constants();\n" +
                "//-------------------- \n";

            //string tGlobal = "global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, ";

            //			// TO DO  : Change the name insertion in all file and COMMENT LATER {
            //			tConstantsFile += "$SQL_" + tClassName + "_Table = '" + tTableName + "';\n";
            //			tGlobal += "$SQL_" + tClassName + "_Table, ";
            //			foreach (TableMapping.Column tColumn in tTableMapping.Columns) {
            //				tConstantsFile += "$SQL_" + tClassName + "_" + tColumn.Name + " = '" + tColumn.Name + "'; // type " + tColumn.ColumnType.Name;
            //				tConstantsFile += "\n";
            //				tGlobal += "$SQL_" + tClassName + "_" + tColumn.Name + ",";
            //			}
            //			tGlobal = tGlobal.TrimEnd (new char[]{ ',' }) + ";\n";
            //			tConstantsFile += "//-------------------- \n";
            //			// }

            // Create error in local data base
            NWDError.CreateGenericError("RESCUE", "01", "{APP} : Forgotten password", "Hello,\r\n" +
                                        "You forgot your password for the App {APP}'s account and ask to reset it." +
                                        "If you didn't ask the reset, ignore it.\r\n" +
                                        "Else, just click on this link to reset your password and receipt a new password by email: \r\n" +
                                        "\r\n" +
                                        "reset my password: {URL}\r\n" +
                                        "\r\n" +
                                        "Best regards,\r\n" +
                                        "The {APP}'s team.", "OK");

            NWDError.CreateGenericError("RESCUE", "02", "{APP} : Password rescue", "Hello,\r\n" +
                                        "Your password was resetted!" +
                                        "\r\n" +
                                        "Best regards,\r\n" +
                                        "The {APP}'s team.", "OK");

            NWDError.CreateGenericError("RESCUE", "03", "{APP} : Password Resetted", "Hello,\r\n" +
                                        "Your password for the App {APP}'s account was resetted to : \r\n" +
                                        "\r\n" +
                                        "{PASSWORD}\r\n" +
                                        "\r\n" +
                                        "Best regards,\r\n" +
                                        "The {APP}'s team.", "OK");



            // craete constants of erro in php
            tConstantsFile += string.Empty +
            "errorDeclaration('" + tTrigramme + "x01', 'error in request creation in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x02', 'error in request creation add primary key in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x03', 'error in request creation add autoincrement modify in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x04', 'error in sql index remove in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x05', 'error in sql index creation in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x07', 'error in sql defragment in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x08', 'error in sql drop in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x09', 'error in sql Flush in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x11', 'error in sql add columns in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x12', 'error in sql alter columns in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x31', 'error in request insert new datas before update in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x32', 'error in request select datas to update in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x33', 'error in request select updatable datas in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x38', 'error in request update datas in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x39', 'error too much datas for this reference in  " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x88', 'integrity of one datas is false, break in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x91', 'error update integrity in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x99', 'error columns number in " + tClassName + "');\n" +
            "errorDeclaration('" + tTrigramme + "x77', 'error update log in " + tClassName + "');\n" +
            "\n" +
                "//-------------------- \n";
            tConstantsFile += "?>\n";
            if (sWriteOnDisk)
            {
                File.WriteAllText(tServerDatabaseFolder + "/constants.php", tConstantsFile);
            }
            rReturn.Add("Environment/" + tEnvironmentFolder + "/Engine/Database/" + tClassName + "/constants.php", tConstantsFile);
            // force to import this file by Unity3D
            // AssetDatabase.ImportAsset(tServerDatabaseFolder + "/constants.php");

            //========= MANAGEMENT TABLE FUNCTIONS FILE

            string tManagementFile = "<?php\n" +
                                     "//NWD Autogenerate File at " + tDateTimeString + "\n" +
                                     "//Copyright NetWorkedDatas ideMobi " + tYearString + "\n" +
                                     "//Created by Jean-François CONTART\n" +
                                     "//-------------------- \n" +
                                     "// TABLE MANAGEMENT \n" +
                                     "//-------------------- \n" +
                                     "include_once ( $PATH_BASE.'/Environment/" + sEnvironment.Environment + "/Engine/Database/" + tClassName + "/constants.php');\n" +
                                     "include_once ( $PATH_BASE.'/Engine/functions.php');\n" +
                                     "//-------------------- \n" +
                                     "function Create" + tClassName + "Table () {\n" +
                                    "myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);\n" +
            "global $SQL_CON, $ENV;\n" +
                                     string.Empty;
            var tQuery = "CREATE TABLE IF NOT EXISTS `'.$ENV.'_" + tTableName + "` (";
            var tDeclarations = tTableMapping.Columns.Select(p => Orm.SqlDecl(p, true));
            var tDeclarationsJoined = string.Join(",", tDeclarations.ToArray());
            tDeclarationsJoined = tDeclarationsJoined.Replace('"', '`');
            tDeclarationsJoined = tDeclarationsJoined.Replace("`ID` integer", "`ID` int(11) NOT NULL");
            tDeclarationsJoined = tDeclarationsJoined.Replace("`DC` integer", "`DC` int(11) NOT NULL DEFAULT 0");
            //			tDeclarationsJoined = tDeclarationsJoined.Replace ("`AC` integer", "`AC` int(11) NOT NULL DEFAULT 1");
            tDeclarationsJoined = tDeclarationsJoined.Replace("`DM` integer", "`DM` int(11) NOT NULL DEFAULT 0");
            tDeclarationsJoined = tDeclarationsJoined.Replace("`DD` integer", "`DD` int(11) NOT NULL DEFAULT 0");
            tDeclarationsJoined = tDeclarationsJoined.Replace("`DS` integer", "`DS` int(11) NOT NULL DEFAULT 0");
            tDeclarationsJoined = tDeclarationsJoined.Replace("`XX` integer", "`XX` int(11) NOT NULL DEFAULT 0");
            tDeclarationsJoined = tDeclarationsJoined.Replace("varchar", "text CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL");
            tDeclarationsJoined = tDeclarationsJoined.Replace("primary key autoincrement not null", string.Empty);
            tQuery += tDeclarationsJoined;
            tQuery += ") ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
            tManagementFile += string.Empty +
            "$tQuery = '" + tQuery + "';\n" +
            "$tResult = $SQL_CON->query($tQuery);\n" +
            "if (!$tResult)\n" +
            "{\n" +
            //						"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
            "error('" + tTrigramme + "x01');\n" +
            "}\n" +
                //				"else\n" +
                //					"{\n" +
                //						"myLog(' -> '.$tQuery.' is ok', __FILE__, __FUNCTION__, __LINE__);\n" +
                //					"}\n" +
                "$tQuery = 'ALTER TABLE `'.$ENV.'_" + tTableName + "` ADD PRIMARY KEY (`ID`), ADD UNIQUE KEY `ID` (`ID`);';\n" +
            "$tResult = $SQL_CON->query($tQuery);\n" +
                //"if (!$tResult)\n" +
                //"{\n" +
                //						"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                //"error('" + tTrigramme + "x02');\n" +
                //"}\n" +
                //				"else\n" +
                //					"{\n" +
                //						"myLog(' -> '.$tQuery.' is ok', __FILE__, __FUNCTION__, __LINE__);\n" +
                //					"}\n" +
                "$tQuery = 'ALTER TABLE `'.$ENV.'_" + tTableName + "` MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;';\n" +
            "$tResult = $SQL_CON->query($tQuery);\n" +
            //"if (!$tResult)\n" +
            //"{\n" +
            //						"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
            //"error('" + tTrigramme + "x03');\n" +
            //"}\n" +
            //				"else\n" +
            //					"{\n" +
            //					"myLog(' -> '.$tQuery.' is ok', __FILE__, __FUNCTION__, __LINE__);\n" +
            //					"}\n" +
            "\n" +
            "// Alter all existing table with new columns or change type columns\n";


            //			var tAddTable = tTableMapping.Columns.Select (p => Orm.SqlDecl (p, true));
            foreach (TableMapping.Column tColumn in tTableMapping.Columns)
            {

                if (tColumn.Name != "ID" &&
                    tColumn.Name != "Reference" &&
                    tColumn.Name != "DM" &&
                    tColumn.Name != "DC" &&
                    //					tColumn.Name != "AC" &&
                    tColumn.Name != "DD" &&
                    tColumn.Name != "DS" &&
                    tColumn.Name != "XX")
                {
                    tManagementFile +=
                        "$tQuery ='ALTER TABLE `'.$ENV.'_" + tTableName + "` ADD " +
                        Orm.SqlDecl(tColumn, true).Replace(" varchar ", " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL ").Replace(" float ", " double ").Replace("\"", "`") + ";';\n" +
                    "$tResult = $SQL_CON->query($tQuery);\n" +
                    //"if (!$tResult)\n" +
                    //"{\n" +
                    //"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                    //						"error('" + tTrigramme + "x11');\n" +
                    //"}\n" +
                    string.Empty;
                    tManagementFile +=
                        //					"$tQuery ='ALTER TABLE `'.$ENV.'" + tTableName + "` CHANGE `" + tColumn.Name + "` " + Orm.SqlDecl (tColumn, true).Replace ("varchar", "text CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL") + ";';\n" +
                        "$tQuery ='ALTER TABLE `'.$ENV.'_" + tTableName + "` MODIFY " +
                        Orm.SqlDecl(tColumn, true).Replace(" varchar ", " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL ").Replace(" float ", " double ").Replace("\"", "`") + ";';\n" +
                    "$tResult = $SQL_CON->query($tQuery);\n" +
                    //"if (!$tResult)\n" +
                    //"{\n" +
                    //					"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                    //					"error('" + tTrigramme + "x12');\n" +
                    //"}\n" +
                    string.Empty;
                }
            }
            tManagementFile += "\n";

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
                    //Debug.Log ("tColumnType `"+tName+"` = `" + tColumnType.Name+"`");


                    if (tColumnType.IsSubclassOf(typeof(BTBDataType)))
                    {
                        columnNamesFinalList.Add("`" + tName + "`(24)");
                    }
                    else if (tColumnType.IsSubclassOf(typeof(BTBDataTypeInt)))
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

                tManagementFile += string.Empty +
                    "$tRemoveIndexQuery = 'DROP INDEX `" + indexName + "` ON `'.$ENV.'_" + index.TableName + "`;';\n" +
                "$tRemoveIndexResult = $SQL_CON->query($tRemoveIndexQuery);\n" +
                //"if (!$tRemoveIndexResult){};\n" +
                //"{\n" +
                //"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tRemoveIndexQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                //	"error('" + tTrigramme + "x04');\n" +
                //"}\n" +
                string.Empty;

                const string sqlFormat = "CREATE {2}INDEX `{3}` ON `'.$ENV.'_{0}` ({1});";
                var sql = String.Format(sqlFormat, index.TableName, string.Join(", ", columnNamesFinal), index.Unique ? "UNIQUE " : "", indexName);
                //				sql = sql.Replace ("`Reference`", "`Reference`(32)");
                tManagementFile += string.Empty +
                "$tQuery = '" + sql + "';\n" +
                "$tResult = $SQL_CON->query($tQuery);\n" +
                "if (!$tResult)\n" +
                "{\n" +
                "myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
                "error('" + tTrigramme + "x05');\n" +
                "}\n" +
                //				"else\n" +
                //					"{\n" +
                //						"myLog(' -> '.$tQuery.' is ok', __FILE__, __FUNCTION__, __LINE__);\n" +
                //					"}\n" +
                string.Empty;

            }

            tManagementFile += "}\n" +
            "//-------------------- \n" +
            "function Defragment" + tClassName + "Table ()\n" +
            "{\n" +
            "myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);\n" +
            "global $SQL_CON, $ENV;\n" +
            "$tQuery = 'ALTER TABLE `'.$ENV.'_" + tTableName + "` ENGINE=InnoDB;';\n" +
            "$tResult = $SQL_CON->query($tQuery);\n" +
            "if (!$tResult)\n" +
            "{\n" +
            //							"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
            "error('" + tTrigramme + "x07');\n" +
            "}\n" +
            //					"else\n" +
            //						"{\n" +
            //							"myLog(' -> '.$tQuery.' is ok', __FILE__, __FUNCTION__, __LINE__);\n" +
            //						"}\n" +
            "}\n" +
            "//-------------------- \n" +
            "function Drop" + tClassName + "Table ()\n" +
            "{\n" +
            "myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);\n" +
            "global $SQL_CON, $ENV;\n" +
            "$tQuery = 'DROP TABLE `'.$ENV.'_" + tTableName + "`;';\n" +
            "$tResult = $SQL_CON->query($tQuery);\n" +
            "if (!$tResult)\n" +
            "{\n" +
            //						"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
            "error('" + tTrigramme + "x08');\n" +
            "}\n" +
            //				"else\n" +
            //					"{\n" +
            //						"myLog(' -> '.$tQuery.' is ok', __FILE__, __FUNCTION__, __LINE__);\n" +
            //					"}\n" +
            "}\n" +
            "//-------------------- \n" +
            "function Flush" + tClassName + "Table ()\n" +
            "{\n" +
            "myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);\n" +
            "global $SQL_CON, $ENV;\n" +
            "$tQuery = 'FLUSH TABLE `'.$ENV.'_" + tTableName + "`;';\n" +
            "$tResult = $SQL_CON->query($tQuery);\n" +
            "if (!$tResult)\n" +
            "{\n" +
            //						"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
            "error('" + tTrigramme + "x09');\n" +
            "}\n" +
            //				"else\n" +
            //					"{\n" +
            //						"myLog(' -> '.$tQuery.' is ok', __FILE__, __FUNCTION__, __LINE__);\n" +
            //					"}\n" +
            "}\n" +
            "//-------------------- \n" +
            "?>\n";
            if (sWriteOnDisk)
            {
                File.WriteAllText(tServerDatabaseFolder + "/management.php", tManagementFile);
            }
            rReturn.Add("Environment/" + tEnvironmentFolder + "/Engine/Database/" + tClassName + "/management.php", tManagementFile);
            // force to import this file by Unity3D
            //AssetDatabase.ImportAsset(tServerDatabaseFolder + "/management.php");

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

            string tSynchronizationFile = string.Empty;
            tSynchronizationFile += string.Empty +
            "<?php\n" +
            "//NWD Autogenerate File at " + tDateTimeString + "\n" +
            "//Copyright NetWorkedDatas ideMobi " + tYearString + "\n" +
            "//Created by Jean-François CONTART\n" +
            "//--------------------\n" +
            "// SYNCHRONIZATION FUNCTIONS\n" +
            "//-------------------- \n" +
            "include_once ($PATH_BASE.'/Environment/" + sEnvironment.Environment + "/Engine/Database/" + tClassName + "/constants.php');\n" +
            "include_once ($PATH_BASE.'/Engine/functions.php');\n" +
            "//-------------------- \n" +
            "function Integrity" + tClassName + "Test ($sCsv)\n" +
            "\t{\n" +
            "\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;\n" +
            "\t\t$rReturn = true;\n" +
            "\t\t$sCsvList = explode('" + NWDConstants.kStandardSeparator + "',$sCsv);\n" +
            "\t\t$tIntegrity = array_pop($sCsvList);\n" +
            "\t\tunset($sCsvList[2]);//remove DS\n" +
            "\t\tunset($sCsvList[3]);//remove DevSync\n" +
            "\t\tunset($sCsvList[4]);//remove PreprodSync\n" +
            "\t\tunset($sCsvList[5]);//remove ProdSync\n" +
            "\t\t$sDataString = implode('',$sCsvList);\n" +
            "\t\t$tCalculate = str_replace('" + NWDConstants.kStandardSeparator + "', '', md5($SQL_" + tClassName + "_SaltA.$sDataString.$SQL_" + tClassName + "_SaltB));\n" +
            "\t\tif ($tCalculate!=$tIntegrity)\n" +
            "\t\t\t{\n" +
            "\t\t\t\t$rReturn = false;\n" +
            "\t\t\t\terror('" + tTrigramme + "x88');\n" +
            "\t\t\t}\n" +
            "\t\treturn $rReturn;\n" +
            "\t}\n" +
            "//-------------------- \n" +
            "function Integrity" + tClassName + "Replace ($sCsvArray, $sIndex, $sValue)\n" +
            "\t{\n" +
            "\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;\n" +
            "\t\t$sCsvList = $sCsvArray;\n" +
            "\t\t$sCsvList[$sIndex] = $sValue;\n" +
            "\t\t$tIntegrity = array_pop($sCsvList);\n" +
            "\t\tunset($sCsvList[2]);//remove DS\n" +
            "\t\tunset($sCsvList[3]);//remove DevSync\n" +
            "\t\tunset($sCsvList[4]);//remove PreprodSync\n" +
            "\t\tunset($sCsvList[5]);//remove ProdSync\n" +
            "\t\t$sDataString = implode('',$sCsvList);\n" +
            "\t\t$tCalculate = str_replace('|', '', md5($SQL_" + tClassName + "_SaltA.$sDataString.$SQL_" + tClassName + "_SaltB));\n" +
            "\t\t$sCsvArray[$sIndex] = $sValue;\n" +
            "\t\tarray_pop($sCsvArray);\n" +
            "\t\t$sCsvArray[] = $tCalculate;\n" +
            "\t\treturn $sCsvArray;\n" +
            "\t}\n" +
            "//-------------------- \n" +
            "function Integrity" + tClassName + "Replaces ($sCsvArray, $sIndexesAndValues)\n" +
            "\t{\n" +
            "\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;\n" +
            "\t\t$sCsvList = $sCsvArray;\n" +
            "\t\tforeach(array_keys($sIndexesAndValues) as $tKey)\n" +
            "\t\t\t{\n" +
            "\t\t\t\t$sCsvList[$tKey] = $sIndexesAndValues[$tKey];\n" +
            "\t\t\t}\n" +
            "\t\t$tIntegrity = array_pop($sCsvList);\n" +
            "\t\tunset($sCsvList[2]);//remove DS\n" +
            "\t\tunset($sCsvList[3]);//remove DevSync\n" +
            "\t\tunset($sCsvList[4]);//remove PreprodSync\n" +
            "\t\tunset($sCsvList[5]);//remove ProdSync\n" +
            "\t\t$sDataString = implode('',$sCsvList);\n" +
            "\t\t$tCalculate = str_replace('|', '', md5($SQL_" + tClassName + "_SaltA.$sDataString.$SQL_" + tClassName + "_SaltB));\n" +
            "\t\tforeach(array_keys($sIndexesAndValues) as $tKey)\n" +
            "\t\t\t{\n" +
            "\t\t\t\t$sCsvArray[$tKey] = $sIndexesAndValues[$tKey];\n" +
            "\t\t\t}\n" +
            "\t\tarray_pop($sCsvArray);\n" +
            "\t\t$sCsvArray[] = $tCalculate;\n" +
            "\t\treturn $sCsvArray;\n" +
            "\t}\n" +
            "//-------------------- \n" +
            "function Prepare" + tClassName + "Data ($sCsv)\n" +
            "\t{\n" +
            "\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $TIME_SYNC;\n" +
            "\t\t$sCsvList = explode('" + NWDConstants.kStandardSeparator + "',$sCsv);\n" +
            "\t\t$sCsvList[2] = $TIME_SYNC;// change DS\n" +
            "\t\tif ($sCsvList[1]<$TIME_SYNC)\n" +
            "\t\t\t{\n" +
            "\t\t\t\t$sCsvList[2] = $sCsvList[1];\n" +
            "\t\t\t}\n";
            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                tSynchronizationFile += "\t\t$sCsvList[3] = $TIME_SYNC;// change DevSync\n";
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                tSynchronizationFile += "\t\t$sCsvList[4] = $TIME_SYNC;// change PreprodSync\n";
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                tSynchronizationFile += "\t\t$sCsvList[5] = $TIME_SYNC;// change ProdSync\n";
            }
            tSynchronizationFile += string.Empty +
            //			"\t\t$tIntegrity = array_pop($sCsvList);\n" +
            //			"\t\t$sDataString = implode('',$sCsvList);\n" +
            //			"\t\t$tIntegrity = str_replace('" + NWDConstants.kStandardSeparator + "', '', md5($SQL_" + tClassName + "_SaltA.$sDataString.$SQL_" + tClassName + "_SaltB));\n" +
            //			"\t\t$sCsvList[] = $tIntegrity;\n" +
            "\t\treturn $sCsvList;\n" +
            "\t}\n" +
            "//-------------------- \n";

            // Add Log in server database
            tSynchronizationFile += "function Log" + tClassName + " ($sReference, $sLog)\n" +
            "\t{\n" +
            "\t\tglobal $SQL_CON, $ENV;\n" +
            "\t\t$tUpdate = 'UPDATE `'.$ENV.'_" + tTableName + "` SET `ServerLog` = CONCAT(`ServerLog`, \\' ; '.$SQL_CON->real_escape_string($sLog).'\\') WHERE `Reference` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';\n" +
            "\t\t$tUpdateResult = $SQL_CON->query($tUpdate);\n" +
            "\t\tif (!$tUpdateResult)\n" +
            "\t\t\t{\n" +
            "\t\t\t\tmyLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tUpdate.'', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\t\t\terror('" + tTrigramme + "x77');\n" +
            "\t\t\t}\n" +
            "\t}\n" +
            "//-------------------- \n";



            // SERVER Integrity generate
            tSynchronizationFile += "function IntegrityServer" + tClassName + "Generate ($sRow)\n" +
            "\t{\n" +
            "\t\tglobal $NWD_SLT_SRV;\n" +
            "\t\t$sDataServerString =''";
            foreach (string tPropertyName in SLQIntegrityServerOrder())
            {
                tSynchronizationFile += ".$sRow['" + tPropertyName + "']";
            }
            tSynchronizationFile += ";\n" +
            "\t\treturn str_replace('" + NWDConstants.kStandardSeparator + "', '', md5($NWD_SLT_SRV.$sDataServerString.$NWD_SLT_SRV));\n" +
            "\t}\n" +
            "//-------------------- \n";

            // DATA Integrity generate
            tSynchronizationFile += "function Integrity" + tClassName + "Generate ($sRow)\n" +
            "\t{\n" +
            "\t\tglobal $SQL_CON, $ENV, $NWD_SLT_SRV;\n" +
            "\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;\n" +
            "\t\t$sDataString =''";
            foreach (string tPropertyName in SLQIntegrityOrder())
            {
                tSynchronizationFile += ".$sRow['" + tPropertyName + "']";
            }
            tSynchronizationFile += ";\n" +
            "\t\tmyLog('sDataString : '.$sDataString.'', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\treturn str_replace('" + NWDConstants.kStandardSeparator + "', '', md5($SQL_" + tClassName + "_SaltA.$sDataString.$SQL_" + tClassName + "_SaltB));\n" +
            "\t}\n" +
            "//-------------------- \n";

            // TODO refactor to be more easy to generate

            tSynchronizationFile += "function Integrity" + tClassName + "Reevalue ($sReference)\n" +
            "\t{\n" +
            "\t\tglobal $SQL_CON, $WSBUILD, $ENV, $NWD_SLT_SRV, $TIME_SYNC, $NWD_FLOAT_FORMAT;\n" +
            "\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $SQL_" + tClassName + "_WebService;\n" +
            "\t\t$tQuery = 'SELECT " + SLQIntegrityOrderToSelect + " FROM `'.$ENV.'_" + tTableName + "` WHERE `Reference` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';\n" +
            "\t\t$tResult = $SQL_CON->query($tQuery);\n" +
            "\t\tif (!$tResult)\n" +
            "\t\t\t{\n" +
            "\t\t\t\tmyLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\t\t\terror('" + tTrigramme + "x31');\n" +
            "\t\t\t}\n" +
            "\t\telse\n" +
            "\t\t\t{\n" +
            "\t\t\t\tif ($tResult->num_rows == 1)\n" +
            "\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t// I calculate the integrity and reinject the good value\n" +
            "\t\t\t\t\t\t$tRow = $tResult->fetch_assoc();\n" +
            //"\t\t\t\t\t\t$tRow['WebServiceVersion'] = $WSBUILD;\n" +
            "\t\t\t\t\t\t$tRow['WebServiceVersion'] = $SQL_" + tClassName + "_WebService;\n" +
            "\t\t\t\t\t\t$tCalculate = Integrity" + tClassName + "Generate ($tRow);\n" +
            "\t\t\t\t\t\t$tCalculateServer = IntegrityServer" + tClassName + "Generate ($tRow);\n" +
            "\t\t\t\t\t\t$tUpdate = 'UPDATE `'.$ENV.'_" + tTableName + "` SET `Integrity` = \\''.$SQL_CON->real_escape_string($tCalculate).'\\'," +
            " `ServerHash` = \\''.$SQL_CON->real_escape_string($tCalculateServer).'\\'," +
            " `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\' ," +
            //" `WebServiceVersion` = \\''.$WSBUILD.'\\'" +
            " `WebServiceVersion` = \\''.$SQL_" + tClassName + "_WebService.'\\'" +
            " WHERE `Reference` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';\n" +
            "\t\t\t\t\t\t$tUpdateResult = $SQL_CON->query($tUpdate);\n" +
            "\t\t\t\t\t\tif (!$tUpdateResult)\n" +
            "\t\t\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t\t\tmyLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tUpdate.'', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\t\t\t\t\t\t\terror('" + tTrigramme + "x91');\n" +
            "\t\t\t\t\t\t\t}\n" +
            "\t\t\t\t\t}\n" +
            "\t\t\t}\n" +
            "\t}\n" +
            "//-------------------- \n";

            // TODO refactor to be more easy to generate

            tSynchronizationFile += "function IntegrityServer" + tClassName + "Validate ($sReference)\n" +
            "\t{\n" +
            "\t\tglobal $SQL_CON, $ENV, $NWD_SLT_SRV;\n" +
            "\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;\n" +
            "\t\t$tQuery = 'SELECT " + SLQIntegrityOrderToSelect + " FROM `'.$ENV.'_" + tTableName + "` WHERE `Reference` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';\n" +
            "\t\t$tResult = $SQL_CON->query($tQuery);\n" +
            "\t\tif (!$tResult)\n" +
            "\t\t\t{\n" +
            "\t\t\t\tmyLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\t\t\terror('" + tTrigramme + "x31');\n" +
            "\t\t\t}\n" +
            "\t\telse\n" +
            "\t\t\t{\n" +
            "\t\t\t\tif ($tResult->num_rows == 1)\n" +
            "\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t// I calculate the integrity and reinject the good value\n" +
            "\t\t\t\t\t\t$tRow = $tResult->fetch_assoc();\n" +
            "\t\t\t\t\t\t$tCalculateServer =IntegrityServer" + tClassName + "Generate ($tRow);\n" +
            "\t\t\t\t\t\tif ($tCalculateServer == $tRow['ServerHash'])\n" +
            "\t\t\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t\t\treturn true;\n" +
            "\t\t\t\t\t\t\t}\n" +
            "\t\t\t\t\t}\n" +
            "\t\t\t}\n" +
            "\t\treturn false;\n" +
            "\t}\n" +
            "//-------------------- \n";



            // TODO refactor to be more easy to generate

            tSynchronizationFile += "function IntegrityServer" + tClassName + "ValidateByRow ($sRow)\n" +
            "\t{\n" +
            "\t\tglobal $NWD_SLT_SRV;\n" +
            "\t\t$tCalculateServer = IntegrityServer" + tClassName + "Generate ($sRow);\n" +
            "\t\tif ($tCalculateServer == $sRow['ServerHash'])\n" +
            "\t\t\t{\n" +
            "\t\t\t\treturn true;\n" +
            "\t\t\t}\n" +
            "\t\treturn false;\n" +
            "\t}\n" +
            "//-------------------- \n";

            // TODO refactor to be more easy to generate



            tSynchronizationFile += "function Integrity" + tClassName + "Validate ($sReference)\n" +
            "\t{\n" +
            "\t\tglobal $SQL_CON, $ENV, $NWD_SLT_SRV;\n" +
            "\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;\n" +
            "\t\t$tQuery = 'SELECT " + SLQIntegrityOrderToSelect + " FROM `'.$ENV.'_" + tTableName + "` WHERE `Reference` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';\n" +
            "\t\t$tResult = $SQL_CON->query($tQuery);\n" +
            "\t\tif (!$tResult)\n" +
            "\t\t\t{\n" +
            "\t\t\t\terror('" + tTrigramme + "x31');\n" +
            "\t\t\t\tmyLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\t\t}\n" +
            "\t\telse\n" +
            "\t\t\t{\n" +
            "\t\t\t\tif ($tResult->num_rows == 1)\n" +
            "\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t// I calculate the integrity and reinject the good value\n" +
            "\t\t\t\t\t\t$tRow = $tResult->fetch_assoc();\n" +
            "\t\t\t\t\t\t$tCalculate =Integrity" + tClassName + "Generate ($tRow);\n" +
            "\t\t\t\t\t\tif ($tCalculate == $tRow['Integrity'])\n" +
            "\t\t\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t\t\treturn true;\n" +
            "\t\t\t\t\t\t\t}\n" +
            "\t\t\t\t\t}\n" +
            "\t\t\t}\n" +
            "\t\treturn false;\n" +
            "\t}\n" +
            "//-------------------- \n";



            // TODO refactor to be more easy to generate

            tSynchronizationFile += "function Integrity" + tClassName + "ValidateByRow ($sRow)\n" +
            "\t{\n" +
            "\t\tglobal $SQL_CON, $WSBUILD, $ENV, $NWD_SLT_SRV;\n" +
                "\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB,$SQL_" + tClassName + "_WebService;\n" +
            "\t\t$tCalculate =Integrity" + tClassName + "Generate ($sRow);\n" +
            "\t\tif ($tCalculate == $sRow['Integrity'])\n" +
            "\t\t\t{\n" +
            "\t\t\t\treturn true;\n" +
            "\t\t\t}\n" +
            "\t\treturn false;\n" +
            "\t}\n" +
            "//-------------------- \n";



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

			var tMethodDeclareFunctions = tType.GetMethod("AddonPhpFunctions", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
			if (tMethodDeclareFunctions != null)
			{
				tSynchronizationFile += (string)tMethodDeclareFunctions.Invoke(null, new object[] { sEnvironment });
			}

			tSynchronizationFile += "function UpdateData" + tClassName + " ($sCsv, $sTimeStamp, $sAccountReference, $sAdmin)\n" +
            "\t{\n" +
            "myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\tglobal $SQL_CON, $WSBUILD, $ENV, $NWD_SLT_SRV, $TIME_SYNC, $NWD_FLOAT_FORMAT, $ACC_NEEDED, $PATH_BASE, $REF_NEEDED, $REP;\n" +
            "\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $SQL_" + tClassName + "_WebService;\n" +
            "\t\tglobal $admin, $uuid;\n" +
            "\t\tif (Integrity" + tClassName + "Test ($sCsv) == true)\n" +
            "\t\t\t{\n" +
            "\t\t\t\t$sCsvList = Prepare" + tClassName + "Data($sCsv);\n" +
            "\t\t\t\tif (count ($sCsvList) != " + tColumnNameList.Count.ToString() + ")\n" +
            "\t\t\t\t{\n" +
            "\t\t\t\t\t\terror('" + tTrigramme + "x99');\n" +
            "\t\t\t\t}\n" +
            "\t\t\t\telse\n" +
            "\t\t\t\t{\n" +
            "\t\t\t\t$tReference = $sCsvList[0];\n" +
            "\t\t\t\t\t\t\t\t\t// find solution for pre calculate on server\n";


            var tMethodDeclarePre = tType.GetMethod("AddonPhpPreCalculate", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (tMethodDeclarePre != null)
            {
                tSynchronizationFile += (string)tMethodDeclarePre.Invoke(null, new object[] { sEnvironment });
            }
            //if (tType.GetCustomAttributes(typeof(NWDClassPhpPreCalculateAttribute), true).Length > 0)
            //{
            //    NWDClassPhpPreCalculateAttribute tScriptNameAttribut = (NWDClassPhpPreCalculateAttribute)tType.GetCustomAttributes(typeof(NWDClassPhpPreCalculateAttribute), true)[0];
            //    tSynchronizationFile += AddonPhpPreCalculate();
            //}


            tSynchronizationFile += "\n" +
            "\t\t\t\t$tQuery = 'SELECT `Reference`, `DM` FROM `'.$ENV.'_" + tTableName + "` WHERE `Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\';';\n" +
            "\t\t\t\t$tResult = $SQL_CON->query($tQuery);\n" +
            "\t\t\t\tif (!$tResult)\n" +
            "\t\t\t\t\t{\n" +
            "\t\t\t\t\t\tmyLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\t\t\t\t\terror('" + tTrigramme + "x31');\n" +
            "\t\t\t\t\t}\n" +
            "\t\t\t\telse\n" +
            "\t\t\t\t\t{\n" +
            "\t\t\t\t\t\tif ($tResult->num_rows <= 1)\n" +
            "\t\t\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t\t\tif ($tResult->num_rows == 0)\n" +
            "\t\t\t\t\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t\t\t\t\t$tInsert = 'INSERT INTO `'.$ENV.'_" + tTableName + "` (" + string.Join(", ", tColumnNameList.ToArray()) + ") VALUES (" + string.Join(", ", tColumnValueList.ToArray()) + ");';\n" +
            "\t\t\t\t\t\t\t\t\t\t$tInsertResult = $SQL_CON->query($tInsert);\n" +
            "\t\t\t\t\t\t\t\t\t\tif (!$tInsertResult)\n" +
            "\t\t\t\t\t\t\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t\t\t\t\t\t\tmyLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tInsertResult.'', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\t\t\t\t\t\t\t\t\t\t\terror('" + tTrigramme + "x32');\n" +
            "\t\t\t\t\t\t\t\t\t\t\t}\n" +
            "\t\t\t\t\t\t\t\t\t}\n" +
            "\t\t\t\t\t\t\t\telse\n" +
            "\t\t\t\t\t\t\t\t\t{\n" +
            //"\t\t\t\t\t\t\t\t\t\twhile($tRow = $tResult->fetch_row())\n"+
            //"\t\t\t\t\t\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t\t\t\t\t$tUpdate = 'UPDATE `'.$ENV.'_" + tTableName + "` SET ";
            tSynchronizationFile += string.Join(", ", tModify.ToArray()) + " WHERE `Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\' ";
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
            tSynchronizationFile += "';\n";
            if (tAccountReference.Count == 0)
            {
                tSynchronizationFile += "$tUpdateRestriction = '';\n";
            }
            else
            {
                tSynchronizationFile += "$tUpdateRestriction = 'AND (" + string.Join(" OR ", tAccountReference.ToArray()) + ") ';\n";
            }
            tSynchronizationFile += string.Empty +
            "\t\t\t\t\t\t\t\t\t\tif ($admin == false)\n" +
            "\t\t\t\t\t\t\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t\t\t\t\t\t\t$tUpdate = $tUpdate.$tUpdateRestriction.' AND `WebServiceVersion` <= '.$WSBUILD.'';\n" +
            "\t\t\t\t\t\t\t\t\t\t\t}\n" +
            //"\t\t\t\t\t\t\t\t\t\telse\n" +
            //"\t\t\t\t\t\t\t\t\t\t\t{\n" +
            //"\t\t\t\t\t\t\t\t\t\t\t\t//$tUpdate = $tUpdate.' AND `DM`<= \\''.$SQL_CON->real_escape_string($sCsvList[1]).'\\'';\n" +
            //"\t\t\t\t\t\t\t\t\t\t\t}\n" +
            "\t\t\t\t\t\t\t\t\t\t$tUpdateResult = $SQL_CON->query($tUpdate);\n" +
            "\t\t\t\t\t\t\t\t\t\tif (!$tUpdateResult)\n" +
            "\t\t\t\t\t\t\t\t\t\t\t{\n " +
            "\t\t\t\t\t\t\t\t\t\t\t\tmyLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tUpdate.'', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\t\t\t\t\t\t\t\t\t\t\terror('" + tTrigramme + "x38');\n" +
            "\t\t\t\t\t\t\t\t\t\t\t}\n" +
            //"\t\t\t\t\t\t\t\t\t}\n" +
            "\t\t\t\t\t\t\t\t\t}\n" +
            "\t\t\t\t\t\t\t\t\t// find solution for post calculate on server\n";

            var tMethodDeclarePost = tType.GetMethod("AddonPhpPostCalculate", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (tMethodDeclarePost != null)
            {
                tSynchronizationFile += (string)tMethodDeclarePost.Invoke(null, new object[] {sEnvironment });
            }

            //if (tType.GetCustomAttributes(typeof(NWDClassPhpPostCalculateAttribute), true).Length > 0)
            //{
            //    NWDClassPhpPostCalculateAttribute tScriptNameAttribut = (NWDClassPhpPostCalculateAttribute)tType.GetCustomAttributes(typeof(NWDClassPhpPostCalculateAttribute), true)[0];
            //    tSynchronizationFile += tScriptNameAttribut.Script;
            //}
            tSynchronizationFile += "\n" +
                "$tLigneAffceted = $SQL_CON->affected_rows;\n" +
                //"myLog('tLigneAffceted = '.$tLigneAffceted, __FILE__, __FUNCTION__, __LINE__);\n" +
                "if ($tLigneAffceted == 1)\n" +
                "{\n" +
                "// je transmet la sync à tout le monde\n" +
                "if ($sCsvList[3] != -1)\n" +
                "{\n" +
                "$tUpdate = 'UPDATE `Dev_" + tTableName + "` SET `DS` = \\''.$TIME_SYNC.'\\',  `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\' WHERE `Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\'';\n" +
                "$tUpdateResult = $SQL_CON->query($tUpdate);\n" +
                "}\n" +
                "if ($sCsvList[4] != -1)\n" +
                "{\n" +
                "$tUpdate = 'UPDATE `Preprod_" + tTableName + "` SET `DS` = \\''.$TIME_SYNC.'\\',  `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\' WHERE `Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\'';\n" +
                "$tUpdateResult = $SQL_CON->query($tUpdate);\n" +
                "}\n" +
                "if ($sCsvList[5] != -1)\n" +
                "{\n" +
                "$tUpdate = 'UPDATE `Prod_" + tTableName + "` SET `DS` = \\''.$TIME_SYNC.'\\',  `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\' WHERE `Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\'';\n" +
                "$tUpdateResult = $SQL_CON->query($tUpdate);\n" +
                "}\n" +
                "}\n" +
                "\n";

            tSynchronizationFile += "\n\n\n" +
            "\t\t\t\t\t\t\t}\n" +
            "\t\t\t\t\t\telse\n" +
            "\t\t\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t\t\terror('" + tTrigramme + "x39');\n" +
            "\t\t\t\t\t\t\t}\n" +
            "\t\t\t\t\t\tmysqli_free_result($tResult);\n" +
            "\t\t\t\t\t}\n" +
            "\t\t\t}\n" +
            "\t\t\t}\n" +
            "\t}\n" +
            "//-------------------- \n" +
            "function FlushTrashedDatas" + tClassName + " ()\n" +
            "\t{\n" +
            "myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\tglobal $SQL_CON, $ENV;\n" +
            "\t\t$tQuery = 'DELETE FROM `'.$ENV.'_" + tTableName + "` WHERE XX>0';\n" +
            "\t\t$tResult = $SQL_CON->query($tQuery);\n" +
            "\t\tif (!$tResult)\n" +
            "\t\t\t{\n" +
            "\t\t\t\terror('" + tTrigramme + "x40');\n" +
            "\t\t\t}\n" +
            "}" +
            "//-------------------- \n" +
            "function GetDatas" + tClassName + "ByReference ($sReference)\n" +
            "\t{\n" +
            "myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\tglobal $SQL_CON, $WSBUILD, $ENV, $REF_NEEDED, $ACC_NEEDED, $uuid;\n" +
            "\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB,$SQL_" + tClassName + "_WebService;\n" +
                "\t\tglobal $REP;\n" +
            "\t\tglobal $admin;\n" +
            //"\t\t$tPage = $sPage*$sLimit;\n" +
                "\t\t$tQuery = 'SELECT " + SLQIntegrityOrderToSelect + " FROM `'.$ENV.'_" + tTableName + "` WHERE Reference = \\''.$SQL_CON->real_escape_string($sReference).'\\'';\n" +
            "\t\t\t\t\t\t\t\t\t\tif ($admin == false)\n" +
            "\t\t\t\t\t\t\t\t\t\t\t{\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t$tQuery = $tQuery.' AND `WebServiceVersion` <= '.$WSBUILD.';';\n" +
            "\t\t\t\t\t\t\t\t\t\t\t}\n" +
            "\t\t$tResult = $SQL_CON->query($tQuery);\n" +
            "\t\tif (!$tResult)\n" +
            "\t\t\t{\n" +
            "\t\t\t\terror('" + tTrigramme + "x33');\n" +
            "\t\t\t}\n" +
            "\t\telse\n" +
            "\t\t\t{\n" +
            "\t\t\t\twhile($tRow = $tResult->fetch_row())\n" +
            "\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t$REP['" + tClassName + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);\n" +
            "\t\t\t\t\t}\n";
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
                tSynchronizationFile += "\t\t\t\t$tResult->data_seek(0);\n\t\t\t\twhile($tRow = $tResult->fetch_assoc())\n\t\t\t\t\t{\n" + tSpecialAdd + "\n\t\t\t\t\t}\n";
            }

            tSynchronizationFile += "\t\t\t\tmysqli_free_result($tResult);\n" +
            "\t\t\t}\n" +
            "\t}\n" +
            "//-------------------- \n" +
            "function GetDatas" + tClassName + "ByReferences ($sReferences)\n" +
            "\t{\n" +
            "\t\tglobal $SQL_CON, $WSBUILD, $ENV, $REF_NEEDED, $ACC_NEEDED, $uuid;\n" +
             "\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB,$SQL_" + tClassName + "_WebService;\n" +
                "\t\tglobal $REP;\n" +
            "\t\tglobal $admin;\n" +
            //"\t\t$tPage = $sPage*$sLimit;\n" +
                "\t\t$tQuery = 'SELECT " + SLQIntegrityOrderToSelect + " FROM `'.$ENV.'_" + tTableName + "` WHERE Reference IN ( \\''.implode('\\', \\'', $sReferences).'\\')';\n" +
            "\t\t\t\t\t\t\t\t\t\tif ($admin == false)\n" +
            "\t\t\t\t\t\t\t\t\t\t\t{\n" +
                "\t\t\t\t\t\t\t\t\t\t\t\t$tQuery = $tQuery.' AND `WebServiceVersion` <= '.$WSBUILD.';';\n" +
            "\t\t\t\t\t\t\t\t\t\t\t}\n" +
            "\t\t$tResult = $SQL_CON->query($tQuery);\n" +
            "\t\tif (!$tResult)\n" +
            "\t\t\t{\n" +
            "\t\t\t\terror('" + tTrigramme + "x33');\n" +
            "\t\t\t}\n" +
            "\t\telse\n" +
            "\t\t\t{\n" +
            "\t\t\t\twhile($tRow = $tResult->fetch_row())\n" +
            "\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t$REP['" + tClassName + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);\n" +
            "\t\t\t\t\t}\n";
            if (tSpecialAdd != string.Empty)
            {
                tSynchronizationFile += "\t\t\t\t$tResult->data_seek(0);\n\t\t\t\twhile($tRow = $tResult->fetch_assoc())\n\t\t\t\t\t{\n" + tSpecialAdd + "\n\t\t\t\t\t}\n";
            }

            tSynchronizationFile += "\t\t\t\tmysqli_free_result($tResult);\n" +
            "\t\t\t}\n" +
            "\t}\n" +
            "//-------------------- \n" +
            "function GetDatas" + tClassName + " ($sTimeStamp, $sAccountReference)\n" +
            "\t{\n" +
            "\t\tglobal $SQL_CON, $WSBUILD, $ENV, $REF_NEEDED, $ACC_NEEDED, $uuid;\n" +
            "\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB,$SQL_" + tClassName + "_WebService;\n" +
                "\t\tglobal $REP;\n" +
            "\t\tglobal $admin;\n" +
            //"\t\t$tPage = $sPage*$sLimit;\n" +
            "\t\t$tQuery = 'SELECT " + SLQIntegrityOrderToSelect + " FROM `'.$ENV.'_" + tTableName + "` WHERE " +
            //"(`'.$ENV.'Sync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\' OR `DS` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\')";
            "(`'.$ENV.'Sync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\')";
            // if need Account reference
            if (tAccountReference.Count == 0)
            {
            }
            else
            {
                tSynchronizationFile += "AND (" + string.Join("OR ", tAccountReference.ToArray()) + ") ";
            }
            tSynchronizationFile += "';\n" +
            "\t\t\t\t\t\t\t\t\t\tif ($admin == false)\n" +
            "\t\t\t\t\t\t\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t\t\t\t\t\t\t$tQuery = $tQuery.' AND `WebServiceVersion` <= '.$WSBUILD.';';\n" +
            "\t\t\t\t\t\t\t\t\t\t\t}\n" +
            string.Empty;
            // I do the result operation
            tSynchronizationFile += "\t\t$tResult = $SQL_CON->query($tQuery);\n" +
            "\t\tif (!$tResult)\n" +
            "\t\t\t{\n" +
            "\t\t\t\terror('" + tTrigramme + "x33');\n" +
            "\t\t\t}\n" +
            "\t\telse\n" +
            "\t\t\t{\n" +
            "\t\t\t\twhile($tRow = $tResult->fetch_row())\n" +
            "\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t$REP['" + tClassName + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);\n" +
            "\t\t\t\t\t}\n";
            if (tSpecialAdd != string.Empty)
            {
                tSynchronizationFile += "\t\t\t\t$tResult->data_seek(0);\n\t\t\t\twhile($tRow = $tResult->fetch_assoc())\n\t\t\t\t\t{\n" + tSpecialAdd + "\n\t\t\t\t\t}\n";
            }
            tSynchronizationFile += "\t\t\t\tmysqli_free_result($tResult);\n" +
            "\t\t\t}\n" +
            "\t}\n" +
            "//-------------------- \n" +
            "function GetDatas" + tClassName + "ByAccounts ($sTimeStamp, $sAccountReferences)\n" +
            "\t{\n" +
            "\t\tglobal $SQL_CON, $WSBUILD, $ENV, $REF_NEEDED, $ACC_NEEDED, $uuid;\n" +
            "\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $SQL_" + tClassName + "_WebService;\n" +
            "\t\tglobal $REP;\n" +
            //"\t\t$tPage = $sPage*$sLimit;\n" +
                "\t\t$tQuery = 'SELECT " + SLQIntegrityOrderToSelect + " FROM `'.$ENV.'_" + tTableName + "` WHERE " +
            //"(`'.$ENV.'Sync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\' OR `DS` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\')";
            "(`'.$ENV.'Sync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\')";
            // if need Account reference
            if (tAccountReferences.Count == 0)
            {
            }
            else
            {
                tSynchronizationFile += "AND (" + string.Join("OR ", tAccountReferences.ToArray()) + ") ";
            }
            tSynchronizationFile += " AND `WebServiceVersion` <= '.$SQL_" + tClassName + "_WebService.';';\n";
            // I do the result operation
            tSynchronizationFile += "\t\t$tResult = $SQL_CON->query($tQuery);\n" +
            "\t\tif (!$tResult)\n" +
            "\t\t\t{\n" +
            "\t\t\t\terror('" + tTrigramme + "x33');\n" +
            "\t\t\t}\n" +
            "\t\telse\n" +
            "\t\t\t{\n" +
            "\t\t\t\twhile($tRow = $tResult->fetch_row())\n" +
            "\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t$REP['" + tClassName + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);\n" +
            "\t\t\t\t\t}\n";
            if (tSpecialAdd != string.Empty)
            {
                tSynchronizationFile += "\t\t\t\t$tResult->data_seek(0);\n\t\t\t\twhile($tRow = $tResult->fetch_assoc())\n\t\t\t\t\t{\n" + tSpecialAdd + "\n\t\t\t\t\t}\n";
            }
            tSynchronizationFile += "\t\t\t\tmysqli_free_result($tResult);\n" +
            "\t\t\t}\n" +
            "\t}\n" +
            "//-------------------- \n" +
            "function Special" + tClassName + " ($sTimeStamp, $sAccountReferences)\n" +
            "\t{\n" +
            "myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\tglobal $SQL_CON, $WSBUILD, $ENV, $NWD_SLT_SRV, $TIME_SYNC, $NWD_FLOAT_FORMAT, $ACC_NEEDED, $PATH_BASE, $REF_NEEDED, $REP;\n" +
            "\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $SQL_" + tClassName + "_WebService;\n" +
            "\t\tglobal $admin, $uuid;\n" +
            "";

            var tMethodDeclareSpecial = tType.GetMethod("AddonPhpSpecialCalculate", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (tMethodDeclareSpecial != null)
            {
                tSynchronizationFile += (string)tMethodDeclareSpecial.Invoke(null, new object[] { sEnvironment });
            }

            //if (tType.GetCustomAttributes(typeof(NWDClassPhpSpecialCalculateAttribute), true).Length > 0)
            //{
            //    NWDClassPhpSpecialCalculateAttribute tScriptNameAttribut = (NWDClassPhpSpecialCalculateAttribute)tType.GetCustomAttributes(typeof(NWDClassPhpSpecialCalculateAttribute), true)[0];
            //    tSynchronizationFile += tScriptNameAttribut.Script;
            //}
            tSynchronizationFile += "\n" +
            "\t}\n" +
            "//-------------------- \n" +


            "function Synchronize" + tClassName + " ($sJsonDico, $sAccountReference, $sAdmin)\n" +
            "\t{\n" +
            "\tglobal $token_FirstUse,$PATH_BASE;\n";

            if (tType.GetCustomAttributes(typeof(NWDForceSecureDataAttribute), true).Length > 0)
            {
                tSynchronizationFile += "respondAdd('securePost',true);\n";
            }

            NWDOperationSpecial tOperation = NWDOperationSpecial.None;
            tSynchronizationFile += "" +
            // IF ADMIN OPERATION
            "\tif ($sAdmin == true)\n" +
            "\t\t{\n" +
            "\t\t\t$sAccountReference = '%';\n";

            // Clean data?
            tOperation = NWDOperationSpecial.Clean;
            tSynchronizationFile += "" +
            "\t\t\tif (isset($sJsonDico['" + tClassName + "']['"+tOperation.ToString().ToLower()+"']))\n" +
            "\t\t\t\t{\n" +
            "\t\t\t\t\tif (!errorDetected())\n" +
            "\t\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t\tFlushTrashedDatas" + tClassName + " ();\n" +
            "\t\t\tmyLog('SPECIAL : CLEAN', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\t\t\t\t\t}\n" +
            "\t\t\t\t}\n";

            //Special?
            tOperation = NWDOperationSpecial.Special;
            tSynchronizationFile += "" +
            "\t\t\tif (isset($sJsonDico['" + tClassName + "']['" + tOperation.ToString().ToLower() + "']))\n" +
            "\t\t\t\t{\n" +
            "\t\t\t\t\tif (!errorDetected())\n" +
            "\t\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t\tSpecial" + tClassName + " ($sJsonDico['" + tClassName + "']['sync'], $sAccountReference);\n" +
            "\t\t\tmyLog('SPECIAL : SPECIAL', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\t\t\t\t\t}\n" +
            "\t\t\t\t}\n";

            //Upgrade?
            tOperation = NWDOperationSpecial.Upgrade;
            tSynchronizationFile += "" +
            "\t\t\tif (isset($sJsonDico['" + tClassName + "']['" + tOperation.ToString().ToLower() + "']))\n" +
            "\t\t\t\t{\n" +
            "\t\t\t\t\tif (!errorDetected())\n" +
            "\t\t\t\t\t\t{\n" +
            "include_once ($PATH_BASE.'/Environment/"+sEnvironment.Environment + "/Engine/Database/" + tClassName + "/management.php');" +
            "\t\t\t\t\t\t\tCreate" + tClassName + "Table ();\n" +
            "\t\t\tmyLog('SPECIAL : UPGRADE OR CREATE TABLE', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\t\t\t\t\t}\n" +
            "\t\t\t\t}\n";

            //Optimize?
            tOperation = NWDOperationSpecial.Optimize;
            tSynchronizationFile += "" +
            "\t\t\tif (isset($sJsonDico['" + tClassName + "']['" + tOperation.ToString().ToLower() + "']))\n" +
            "\t\t\t\t{\n" +
            "\t\t\t\t\tif (!errorDetected())\n" +
            "\t\t\t\t\t\t{\n" +
            "include_once ($PATH_BASE.'/Environment/" + sEnvironment.Environment + "/Engine/Database/" + tClassName + "/management.php');" +
            "\t\t\t\t\t\t\tDefragment" + tClassName + "Table ();\n" +
            "\t\t\tmyLog('SPECIAL : OPTIMIZE AND DEFRAGMENT TABLE', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\t\t\t\t\t}\n" +
            "\t\t\t\t}\n";

            tSynchronizationFile += "" +
            // ENDIF ADMIN OPERATION
            "\t\t}\n" +
            "\tif ($token_FirstUse == true)\n" +
            "\t\t{\n";

            if (tINeedAdminAccount == true)
            {
                tSynchronizationFile += "\t\tif ($sAdmin == true)\n\t\t\t{\n";
            }
            tSynchronizationFile += string.Empty +
            "\t\tif (isset($sJsonDico['" + tClassName + "']))\n" +
            "\t\t\t{\n" +
            "\t\t\t\tif (isset($sJsonDico['" + tClassName + "']['data']))\n" +
            "\t\t\t\t\t{\n" +
            "\t\t\t\t\t\tforeach ($sJsonDico['" + tClassName + "']['data'] as $sCsvValue)\n" +
            "\t\t\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t\t\tif (!errorDetected())\n" +
            "\t\t\t\t\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t\t\t\t\tUpdateData" + tClassName + " ($sCsvValue, $sJsonDico['" + tClassName + "']['sync'], $sAccountReference, $sAdmin);\n" +
            "\t\t\t\t\t\t\t\t\t}\n" +
            "\t\t\t\t\t\t\t}\n" +
            "\t\t\t\t\t}\n" +
            "\t\t\t}\n";
            if (tINeedAdminAccount == true)
            {
                tSynchronizationFile += "\t\t\t}\n";
            }
            tSynchronizationFile += "" +
            "\t\t}\n" +
            "\telse\n" +
            "\t\t{\n" +
            "\t\t\tmyLog('NOT UPDATE, SPECIAL OR CLEAN ACTION ... YOU USE OLDEST TOKEN', __FILE__, __FUNCTION__, __LINE__);\n" +
            "\t\t}\n" +
            "\tif (isset($sJsonDico['" + tClassName + "']))\n" +
            "\t\t{\n" +
            "\t\t\tif (isset($sJsonDico['" + tClassName + "']['sync']))\n" +
            "\t\t\t\t{\n" +
            "\t\t\t\t\tif (!errorDetected())\n" +
            "\t\t\t\t\t\t{\n" +
            "\t\t\t\t\t\t\tGetDatas" + tClassName + " ($sJsonDico['" + tClassName + "']['sync'], $sAccountReference);\n" +
            "\t\t\t\t\t\t}\n" +
            "\t\t\t\t}\n" +
            "\t\t}\n" +
            "\t}\n" +
            "//-------------------- \n" +
            "?>";
            if (sWriteOnDisk)
            {
                File.WriteAllText(tServerDatabaseFolder + "/synchronization.php", tSynchronizationFile);
            }
            rReturn.Add("Environment/" + tEnvironmentFolder + "/Engine/Database/" + tClassName + "/synchronization.php", tSynchronizationFile);
            // force to import this file by Unity3D
            // AssetDatabase.ImportAsset(tServerDatabaseFolder + "/synchronization.php");


            /*
            //========= WEBSERVICE FILE
            string tWebService = "" +
            "<?php\n" +
            "//NWD Autogenerate File at " + tDateTimeString + "\n" +
            "//Copyright NetWorkedDatas ideMobi " + tYearString + "\n" +
            "//Created by Jean-François CONTART\n" +
            "//--------------------\n" +
            "// WEBSERVICES FUNCTIONS\n" +
            "//--------------------\n" +
            "// Determine the file tree path\n" +
            "$PATH_BASE = dirname(dirname(__DIR__));\n" +
            "// include all necessary files\n" +
            "include_once ($PATH_BASE.'/Environment/" + sEnvironment.Environment + "/Engine/constants.php');\n" +
            "// start the generic process\n" +
            "include_once ($PATH_BASE.'/Engine/start.php');\n" +
            "// start the script\n" +
            "//--------------------\n" +
            "global $dico, $uuid;\n" +
            "//--------------------\n" +
            "// Ok I create a permanent account if temporary before\n" +
            "AccountAnonymeGenerate();\n" +
            "//--------------------\n" +
            "if ($ban == true)\n" +
            "\t{\n" +
            "\t\terror('ACC99');\n" +
            "\t}\n" +
            "//--------------------\n" +
            "if (!errorDetected())\n" +
            "\t{\n" +
            "\t\tinclude_once ($PATH_BASE.'/Environment/" + sEnvironment.Environment + "/Engine/Database/" + tClassName + "/synchronization.php');\n" +
            "\t\tSynchronize" + tClassName + " ($dico, $uuid, $admin);\n" +
            "\t}\n" +
            "//--------------------\n" +
            "// script is finished\n" +
            "// finish the generic process\n" +
            "include_once ($PATH_BASE.'/Engine/finish.php');\n" +
            "\n" +
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