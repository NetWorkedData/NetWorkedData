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
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;
//=====================================================================================================================
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDBasisHelperTableEngine
    {
        InnoDB = 0,
        MyISAM = 1,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        NWDBasisHelperTableEngine TableEngine = NWDBasisHelperTableEngine.MyISAM;
        //-------------------------------------------------------------------------------------------------------------
        public string PHP_FUNCTION_CONSTANTS() { return ClassNamePHP + "Constants"; }
        public string PHP_FUNCTION_INTEGRITY_TEST() { return ClassNamePHP + "IntegrityTest"; }
        public string PHP_FUNCTION_INTERGRITY_REPLACE() { return ClassNamePHP + "IntegrityReplace"; }
        public string PHP_FUNCTION_INTERGRITY_REPLACES() { return ClassNamePHP + "IntegrityReplaces"; }
        public string PHP_FUNCTION_PREPARE_DATA() { return ClassNamePHP + "PrepareData"; }
        public string PHP_FUNCTION_LOG() { return ClassNamePHP + "Log"; }
        public string PHP_FUNCTION_INTEGRITY_SERVER_GENERATE() { return ClassNamePHP + "IntegrityServerGenerate"; }
        public string PHP_FUNCTION_INTEGRITY_GENERATE() { return ClassNamePHP + "IntegrityGenerate"; }
        public string PHP_FUNCTION_INTEGRITY_REEVALUATE() { return ClassNamePHP + "IntegrityReevalue"; }
        public string PHP_FUNCTION_MODIFY_AND_INTEGRATE() { return ClassNamePHP + "ModifyAndIntegrityReevalue"; }
        public string PHP_FUNCTION_MODIFY_WHERE_INTEGRATE() { return ClassNamePHP + "ModifyWhereIntegrityReevalue"; }
        public string PHP_FUNCTION_INTEGRITY_SERVER_VALIDATE() { return ClassNamePHP + "IntegrityServerValidate"; }
        public string PHP_FUNCTION_INTEGRITY_SERVER_VALIDATE_BY_ROW() { return ClassNamePHP + "IntegrityServerValidateByRow"; }
        public string PHP_FUNCTION_INTEGRITY_VALIDATE() { return ClassNamePHP + "IntegrityValidate"; }
        public string PHP_FUNCTION_INTEGRITY_VALIDATE_BY_ROW() { return ClassNamePHP + "IntegrityValidateByRow"; }
        public string PHP_FUNCTION_UPDATE_DATA() { return ClassNamePHP + "UpdateData"; }
        public string PHP_FUNCTION_FLUSH_TRASH_DATAS() { return ClassNamePHP + "FlushTrashedDatas"; }
        public string PHP_FUNCTION_GET_DATA_BY_REFERENCE() { return ClassNamePHP + "GetDataByReference"; }
        public string PHP_FUNCTION_GET_DATAS_BY_REFERENCES() { return ClassNamePHP + "GetDatasByReferences"; }
        public string PHP_FUNCTION_GET_DATAS() { return ClassNamePHP + "GetDatas"; }
        public string PHP_FUNCTION_GET_DATAS_BY_GAMESAVE() { return ClassNamePHP + "GetDatasByGameSave"; }
        public string PHP_FUNCTION_GET_DATAS_BY_ACCOUNT() { return ClassNamePHP + "GetDatasByAccounts"; }
        public string PHP_FUNCTION_DELETE_DATAS_BY_ACCOUNT() { return ClassNamePHP + "DeleteDatasByAccount"; }
        public string PHP_FUNCTION_SPECIAL() { return ClassNamePHP + "Special"; }
        public string PHP_FUNCTION_SYNCHRONIZE() { return ClassNamePHP + "Synchronize"; }
        public string PHP_FUNCTION_CREATE_TABLE() { return ClassNamePHP + "CreateTable"; }
        public string PHP_FUNCTION_CREATE_INDEX() { return ClassNamePHP + "CreateIndex"; }
        public string PHP_FUNCTION_CHANGE_TABLE_ENGINE() { return ClassNamePHP + "AlterTableEngine"; }
        public string PHP_FUNCTION_DEFRAGMENT_TABLE() { return ClassNamePHP + "DefragmentTable"; }
        public string PHP_FUNCTION_DROP_TABLE() { return ClassNamePHP + "DropTable"; }
        public string PHP_FUNCTION_FLUSH_TABLE() { return ClassNamePHP + "FlushTable"; }
        public string PHP_CONSTANT_SALT_A() { return "$" + ClassNamePHP + "SaltA"; }
        public string PHP_CONSTANT_SALT_B() { return "$" + ClassNamePHP + "SaltB"; }
        public string PHP_CONSTANT_WEBSERVICE() { return "$" + ClassNamePHP + "WebService"; }
        public string PHP_CONSTANT_SIGN() { return "$" + ClassNamePHP + "Sign"; }
        //-------------------------------------------------------------------------------------------------------------
        //public string PHP_CONSTANT_TABLENAME() { return "'." + NWD.K_ENV + ".'_" + ClassTableName; }
        public string PHP_ENV_SYNC(NWDAppEnvironment sEnvironment)
        {
            string rReturn = "ERROR";
            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                rReturn = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DevSync);
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                rReturn = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().PreprodSync);
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                rReturn = NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ProdSync);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string PHP_FILE_FUNCTION_PATH(NWDAppEnvironment sEnvironment) { return "" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FUNCTIONS_PHP + "'"; }
        public static string PHP_FILE_ENGINE_PATH(NWDAppEnvironment sEnvironment) { return "" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_ENG + "/" + NWD.K_WS_ENGINE + "'"; }
        public static string PHP_FILE_WS_PATH(NWDAppEnvironment sEnvironment) { return "" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_ENG + "/" + NWD.K_WS_FILE + "'"; }
        public static string PHP_FILE_SYNCHRONISATION_PATH(NWDAppEnvironment sEnvironment) { return "" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_ENG + "/" + NWD.K_WS_SYNCHRONISATION + "'"; }
        public string PHP_ENGINE_PATH(NWDAppEnvironment sEnvironment) { return "" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + ClassNamePHP + "/" + NWD.K_WS_ENGINE + "'"; }
        public string PHP_CONSTANTS_PATH(NWDAppEnvironment sEnvironment) { return "" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + ClassNamePHP + "/" + NWD.K_CONSTANTS_FILE + "'"; }
        public string PHP_SYNCHRONISATION_PATH(NWDAppEnvironment sEnvironment) { return "" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION + "'"; }
        //-------------------------------------------------------------------------------------------------------------
        #region FILE CONSTANT
        public Dictionary<string, string> CreatePHPConstant(NWDAppEnvironment sEnvironment)
        {
            //NWDBenchmark.Start();
            Dictionary<string, string> rReturn = new Dictionary<string, string>();

            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
            {
                StringBuilder tFile = new StringBuilder(string.Empty);
                tFile.AppendLine("<?php");
                tFile.AppendLine(sEnvironment.Headlines());
                tFile.AppendLine(NWD.K_CommentSeparator);
                tFile.AppendLine("// CONSTANTS");
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(sEnvironment, ClassNamePHP + "_CONSTANTS"));
                tFile.AppendLine(NWD.K_CommentSeparator);
                tFile.AppendLine("include_once (" + NWDBasisHelper.PHP_FILE_FUNCTION_PATH(sEnvironment) + ");");
                tFile.AppendLine(NWD.K_CommentSeparator);
                // to bypass the global limitation of PHP in internal include : use function :-) 
                tFile.AppendLine("function " + PHP_FUNCTION_CONSTANTS() + "()");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                    tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ", " + PHP_CONSTANT_WEBSERVICE() + ", " + PHP_CONSTANT_SIGN() + ";");
                    tFile.AppendLine("" + PHP_CONSTANT_SALT_A() + " = '" + SaltStart + "';");
                    tFile.AppendLine("" + PHP_CONSTANT_SALT_B() + " = '" + SaltEnd + "';");
                    tFile.AppendLine("" + PHP_CONSTANT_WEBSERVICE() + " = " + LastWebBuild + "; // last build for this model is " + LastWebBuild + " / " + NWDAppConfiguration.SharedInstance().WebBuild + "");

                    tFile.AppendLine("// Add sign of model, based on string : " + WebServiceOrder(LastWebBuild) + " ");
                    tFile.AppendLine("" + PHP_CONSTANT_SIGN() + " = '" + WebServiceSign(LastWebBuild) + "';");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);
                tFile.AppendLine("//Run function to install these global constants");
                tFile.AppendLine(PHP_FUNCTION_CONSTANTS() + "();");
                tFile.AppendLine(NWD.K_CommentSeparator);
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(sEnvironment, ClassNamePHP + "_CONSTANTS"));
                tFile.AppendLine("?>");
                string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
                rReturn.Add(ClassNamePHP + "/" + NWD.K_CONSTANTS_FILE, tFileFormatted);
                //NWDBenchmark.Finish();
            }
            return rReturn;
        }
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, string> CreatePHPEngine(NWDAppEnvironment sEnvironment)
        {
            //NWDBenchmark.Start();
            Dictionary<string, string> rReturn = new Dictionary<string, string>();

            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
            {
                StringBuilder tFile = new StringBuilder(string.Empty);
                tFile.AppendLine(AddonPhpEngineCalculate(sEnvironment));
                string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
                rReturn.Add(ClassNamePHP + "/" + NWD.K_WS_ENGINE, tFileFormatted);
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private string PropertyInfoToSQLType(PropertyInfo sPropertyInfo)
        {
            string rReturn = "TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT \\'\\' ";
            Type tTypeOfThis = sPropertyInfo.PropertyType;

            if (tTypeOfThis == typeof(int) ||
                tTypeOfThis == typeof(long) ||
                tTypeOfThis == typeof(Int16) ||
                tTypeOfThis == typeof(Int32) ||
                tTypeOfThis == typeof(Int64) ||
                tTypeOfThis.IsEnum
                )
            {
                rReturn = " INT(11) NOT NULL DEFAULT 0";
            }
            else if (tTypeOfThis == typeof(float) ||
                tTypeOfThis == typeof(double) ||
                tTypeOfThis == typeof(Double))
            {
                rReturn = "DOUBLE NOT NULL DEFAULT 0";
            }
            else if (tTypeOfThis == typeof(bool))
            {
                rReturn = "INT(1) NOT NULL DEFAULT 0";
            }
            else if (tTypeOfThis == typeof(string) || tTypeOfThis == typeof(String))
            {
                if (sPropertyInfo.GetCustomAttribute<NWDVarChar>() != null)
                {
                    NWDVarChar tNWDVarChar = sPropertyInfo.GetCustomAttribute<NWDVarChar>();
                    rReturn = "VARCHAR(" + tNWDVarChar.CharNumber.ToString() + ") CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT \\'\\' ";
                }
                else
                {
                    rReturn = "TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT \\'\\' ";
                }
            }
            else
            {
                if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
                {
                    if (sPropertyInfo.GetCustomAttribute<NWDVarChar>() != null)
                    {
                        NWDVarChar tNWDVarChar = sPropertyInfo.GetCustomAttribute<NWDVarChar>();
                        rReturn = "VARCHAR(" + tNWDVarChar.CharNumber.ToString() + ") CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT \\'\\' ";
                    }
                    else
                    {
                        rReturn = "TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci DEFAULT \\'\\' ";
                    }
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)))
                {
                    rReturn = " INT(11) NOT NULL DEFAULT 0";
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)))
                {
                    rReturn = "DOUBLE NOT NULL DEFAULT 0";
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)))
                {
                    rReturn = " INT(11) NOT NULL DEFAULT 0";
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
                {
                    rReturn = " INT(11) NOT NULL DEFAULT 0";
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private string PropertyInfoToSQLIndex(PropertyInfo sPropertyInfo)
        {
            string rReturn = " (24)";
            Type tTypeOfThis = sPropertyInfo.PropertyType;

            if (tTypeOfThis == typeof(int) ||
                tTypeOfThis == typeof(long) ||
                tTypeOfThis == typeof(Int16) ||
                tTypeOfThis == typeof(Int32) ||
                tTypeOfThis == typeof(Int64) ||
                tTypeOfThis.IsEnum
                )
            {
                rReturn = "";
            }
            else if (tTypeOfThis == typeof(float) ||
                tTypeOfThis == typeof(double) ||
                tTypeOfThis == typeof(Double))
            {
                rReturn = "";
            }
            else if (tTypeOfThis == typeof(bool))
            {
                rReturn = "";
            }
            else if (tTypeOfThis == typeof(string) || tTypeOfThis == typeof(String))
            {
                rReturn = " (24)";
            }
            else
            {
                if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
                {
                    rReturn = " (24)";
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)))
                {
                    rReturn = "";
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)))
                {
                    rReturn = "";
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)))
                {
                    rReturn = "";
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
                {
                    rReturn = "";
                }
            }
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, string> CreatePHPManagement(NWDAppEnvironment sEnvironment)
        {
            //NWDBenchmark.Start();
            Dictionary<string, string> rReturn = new Dictionary<string, string>();

            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
            {
                //string tClassName = ClassNamePHP;
                //string tTrigramme = ClassTrigramme;
                //Type tType = ClassType;
                //TableMapping tTableMapping = new TableMapping(ClassType);
                //string tTableName = tTableMapping.TableName;
                //========= MANAGEMENT TABLE FUNCTIONS FILE
                StringBuilder tFile = new StringBuilder(string.Empty);
                tFile.AppendLine("<?php");
                tFile.AppendLine(sEnvironment.Headlines());
                tFile.AppendLine(NWD.K_CommentSeparator);
                tFile.AppendLine("// TABLE MANAGEMENT");
                tFile.AppendLine(NWD.K_CommentSeparator);

                //---------------------------------------
                tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FUNCTIONS_PHP + "');");
                tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + ClassNamePHP + "/" + NWD.K_CONSTANTS_FILE + "');");
                tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDError>().ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
                tFile.AppendLine(NWD.K_CommentSeparator);
                //---------------------------------------

                tFile.AppendLine("ConnectAllDatabases();");
                tFile.AppendLine(NWD.K_CommentSeparator);

                //---------------------------------------
                tFile.AppendLine("function " + PHP_FUNCTION_CREATE_TABLE() + "()");
                tFile.AppendLine("{");
                tFile.AppendLine("global " + NWD.K_SQL_CON_EDITOR + ", " + NWD.K_ENV + ";");
                var tQuery = "CREATE TABLE IF NOT EXISTS `" + PHP_TABLENAME(sEnvironment) + "` (";
                List<string> PropertiesSQL = new List<string>();
                foreach (PropertyInfo tPropertyInfo in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (tPropertyInfo != null)
                    {
                        PropertiesSQL.Add("`" + tPropertyInfo.Name + "` " + PropertyInfoToSQLType(tPropertyInfo));
                    }
                }
                tQuery += string.Join(",", PropertiesSQL.ToArray());
                tQuery += ") ENGINE=" + TableEngine.ToString() + " DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
                tFile.AppendLine("$tQuery = '" + tQuery + "';");

                //tFile.AppendLine("$tQueryArray = array();");
                //tFile.AppendLine("$tQueryArray['" + tQuery + "']['" + NWDError.GetErrorCode(NWDError.NWDError_XXx01) + "'];");


                tFile.AppendLine("foreach(" + NWD.K_SQL_CON_EDITOR + " as $tRange=>$Connexion)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$tResult = $Connexion->query($tQuery);");
                    tFile.AppendLine("if (!$tResult)");
                    tFile.AppendLine("{");
                    //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery", "$Connexion"));
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx01, ClassNamePHP));
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");

                tFile.AppendLine("$tQuery = 'ALTER TABLE `" + PHP_TABLENAME(sEnvironment) + "` ADD PRIMARY KEY (`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ID) + "`), ADD UNIQUE KEY `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ID) + "` (`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ID) + "`);';");

                tFile.AppendLine("foreach(" + NWD.K_SQL_CON_EDITOR + " as $tRange=>$Connexion)");
                tFile.AppendLine("{");
                {

                    tFile.AppendLine("$tResult = $Connexion->query($tQuery);");
                    //tFile.AppendLine("if (!$tResult)");
                    //tFile.AppendLine("{");
                    //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
                    //tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx02, ClassNamePHP));
                    //tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("$tQuery = 'ALTER TABLE `" + PHP_TABLENAME(sEnvironment) + "` MODIFY IF EXISTS `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ID) + "` int(11) NOT NULL AUTO_INCREMENT;';");
                tFile.AppendLine("foreach(" + NWD.K_SQL_CON_EDITOR + " as $tRange=>$Connexion)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$tResult = $Connexion->query($tQuery);");
                    //tFile.AppendLine("if (!$tResult)");
                    //tFile.AppendLine("{");
                    //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
                    //tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx03, ClassNamePHP));
                    //tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("");


                tFile.AppendLine("// Alter all existing table with new columns or change type columns");

                Dictionary<string, List<PropertyInfo>> tIndexesDico = new Dictionary<string, List<PropertyInfo>>();
                tIndexesDico.Add(NWD.K_BASIS_INDEX, new List<PropertyInfo>());
                //tIndexesDico.Add(NWD.K_ACCOUNT_INDEX, new List<PropertyInfo>());


                foreach (PropertyInfo tPropertyInfo in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (tPropertyInfo != null)
                    {
                        foreach (NWDIndexedAttribut tAttribut in tPropertyInfo.GetCustomAttributes<NWDIndexedAttribut>())
                        {
                            if (tIndexesDico.ContainsKey(tAttribut.IndexName) == false)
                            {
                                tIndexesDico.Add(tAttribut.IndexName, new List<PropertyInfo>());
                            }
                            if (tIndexesDico[tAttribut.IndexName].Contains(tPropertyInfo) == false)
                            {
                                tIndexesDico[tAttribut.IndexName].Add(tPropertyInfo);
                            }
                        }
                        //Type tTypeOfThis = tPropertyInfo.PropertyType;
                        //if (tTypeOfThis.IsSubclassOf(typeof(NWDReference)) && tTypeOfThis.IsGenericType)
                        //{
                        //    Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                        //    if (tSubType == typeof(NWDAccount) || tSubType == typeof(NWDGameSave))
                        //    {
                        //        if (tPropertyInfo.GetCustomAttribute(typeof(NWDDisableAccountDependence), true) == null)
                        //        {
                        //            if (tIndexesDico[NWD.K_BASIS_INDEX].Contains(tPropertyInfo) == false)
                        //            {
                        //                tIndexesDico[NWD.K_BASIS_INDEX].Add(tPropertyInfo);
                        //            }
                        //        }
                        //    }
                        //    if (tSubType == typeof(NWDAccount))
                        //    {
                        //        if (tPropertyInfo.GetCustomAttribute(typeof(NWDDisableAccountDependence), true) == null)
                        //        {
                        //            if (tIndexesDico[NWD.K_ACCOUNT_INDEX].Contains(tPropertyInfo) == false)
                        //            {
                        //                tIndexesDico[NWD.K_ACCOUNT_INDEX].Add(tPropertyInfo);
                        //            }
                        //        }
                        //    }
                        //}

                        if (tPropertyInfo.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ID)
                           //&& tPropertyInfo.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference)
                           && tPropertyInfo.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DM)
                           && tPropertyInfo.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DC)
                           && tPropertyInfo.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().AC)
                           && tPropertyInfo.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DD)
                           && tPropertyInfo.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS)
                           && tPropertyInfo.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().XX)
                           )
                        {
                            tFile.AppendLine("$tQuery ='ALTER TABLE `" + PHP_TABLENAME(sEnvironment) + "` ADD COLUMN IF NOT EXISTS `" + tPropertyInfo.Name + "` " + PropertyInfoToSQLType(tPropertyInfo) + ";';");
                            tFile.AppendLine("foreach(" + NWD.K_SQL_CON_EDITOR + " as $tRange=>$Connexion)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine("$tResult = $Connexion->query($tQuery);");
                                tFile.AppendLine("if (!$tResult)");
                                tFile.AppendLine("{");
                                //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery", "$Connexion"));
                                tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx11, ClassNamePHP));
                                tFile.AppendLine("}");
                            }
                            tFile.AppendLine("}");
                            tFile.AppendLine("$tQuery ='ALTER TABLE `" + PHP_TABLENAME(sEnvironment) + "` MODIFY IF EXISTS `" + tPropertyInfo.Name + "` " + PropertyInfoToSQLType(tPropertyInfo) + ";';");
                            tFile.AppendLine("foreach(" + NWD.K_SQL_CON_EDITOR + " as $tRange=>$Connexion)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine("$tResult = $Connexion->query($tQuery);");
                                tFile.AppendLine("if (!$tResult)");
                                tFile.AppendLine("{");
                                //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery", "$Connexion"));
                                tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx12, ClassNamePHP));
                                tFile.AppendLine("}");
                                tFile.AppendLine("}");
                            }
                        }
                    }
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);

                //---------------------------------------
                tFile.AppendLine("function " + PHP_FUNCTION_CREATE_INDEX() + "()");
                tFile.AppendLine("{");
                tFile.AppendLine("global " + NWD.K_SQL_CON_EDITOR + ", " + NWD.K_ENV + ";");

                //Dictionary<string, List<PropertyInfo>> tIndexesDico
                foreach (KeyValuePair<string, List<PropertyInfo>> tIndex in tIndexesDico)
                {
                    tFile.AppendLine("$tRemove" + tIndex.Key + "Query = 'DROP INDEX IF EXISTS `" + tIndex.Key + "` ON `" + PHP_TABLENAME(sEnvironment) + "`;';");
                    tFile.AppendLine("foreach(" + NWD.K_SQL_CON_EDITOR + " as $tRange=>$Connexion)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$tRemove" + tIndex.Key + "Result = $Connexion->query($tRemove" + tIndex.Key + "Query);");
                        tFile.AppendLine("if (!$tRemove" + tIndex.Key + "Result)");
                        tFile.AppendLine("{");
                        //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tRemove" + tIndex.Key + "Query", "$Connexion"));
                        //tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx05, ClassNamePHP));
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                    List<string> tColumnNamesFinalList = new List<string>();
                    foreach (PropertyInfo PropertyInfo in tIndex.Value)
                    {
                        tColumnNamesFinalList.Add("`" + PropertyInfo.Name + "`" + PropertyInfoToSQLIndex(PropertyInfo));
                    }
                    if (tColumnNamesFinalList.Count > 0)
                    {
                        tFile.AppendLine("$tCreate" + tIndex.Key + "Query = 'CREATE INDEX `" + tIndex.Key + "`ON `" + PHP_TABLENAME(sEnvironment) + "` (" + string.Join(", ", tColumnNamesFinalList) + ");';");
                        tFile.AppendLine("foreach(" + NWD.K_SQL_CON_EDITOR + " as $tRange=>$Connexion)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("$tCreate" + tIndex.Key + "Result = $Connexion->query($tCreate" + tIndex.Key + "Query);");
                            tFile.AppendLine("if (!$tCreate" + tIndex.Key + "Result)");
                            tFile.AppendLine("{");
                            //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tCreate" + tIndex.Key + "Query", "$Connexion"));
                            //tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx05, ClassNamePHP));
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");
                    }
                }

                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);

                //---------------------------------------
                tFile.AppendLine("function " + PHP_FUNCTION_CHANGE_TABLE_ENGINE() + " ()");
                tFile.AppendLine("{");
                //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("global " + NWD.K_SQL_CON_EDITOR + ", " + NWD.K_ENV + ";");
                tFile.AppendLine("$tQuery = 'ALTER TABLE `" + PHP_TABLENAME(sEnvironment) + "` ENGINE=" + TableEngine.ToString() + ";';");
                tFile.AppendLine("foreach(" + NWD.K_SQL_CON_EDITOR + " as $tRange=>$Connexion)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$tResult = $Connexion->query($tQuery);");
                    tFile.AppendLine("if (!$tResult)");
                    tFile.AppendLine("{");
                    {
                        //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery", "$Connexion"));
                        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx07, ClassNamePHP));
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);

                //---------------------------------------
                tFile.AppendLine("function " + PHP_FUNCTION_DEFRAGMENT_TABLE() + " ()");
                tFile.AppendLine("{");
                //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("global " + NWD.K_SQL_CON_EDITOR + ", " + NWD.K_ENV + ";");
                tFile.AppendLine("$tQuery = 'ALTER TABLE `" + PHP_TABLENAME(sEnvironment) + "` ENGINE=" + TableEngine.ToString() + ";';");
                tFile.AppendLine("foreach(" + NWD.K_SQL_CON_EDITOR + " as $tRange=>$Connexion)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$tResult = $Connexion->query($tQuery);");
                    tFile.AppendLine("if (!$tResult)");
                    tFile.AppendLine("{");
                    //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery", "$Connexion"));
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx07, ClassNamePHP));
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);

                //---------------------------------------
                tFile.AppendLine("function " + PHP_FUNCTION_DROP_TABLE() + " ()");
                tFile.AppendLine("{");
                //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("global " + NWD.K_SQL_CON_EDITOR + ", " + NWD.K_ENV + ";");
                tFile.AppendLine("$tQuery = 'DROP TABLE `" + PHP_TABLENAME(sEnvironment) + "`;';");
                tFile.AppendLine("foreach(" + NWD.K_SQL_CON_EDITOR + " as $tRange=>$Connexion)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$tResult = $Connexion->query($tQuery);");
                    tFile.AppendLine("if (!$tResult)");
                    tFile.AppendLine("{");
                    //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery", "$Connexion"));
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx08, ClassNamePHP));
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);

                //---------------------------------------
                tFile.AppendLine("function " + PHP_FUNCTION_FLUSH_TABLE() + " ()");
                tFile.AppendLine("{");
                //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("global " + NWD.K_SQL_CON_EDITOR + ", " + NWD.K_ENV + ";");
                tFile.AppendLine("$tQuery = 'FLUSH TABLE `" + PHP_TABLENAME(sEnvironment) + "`;';");
                tFile.AppendLine("foreach(" + NWD.K_SQL_CON_EDITOR + " as $tRange=>$Connexion)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("$tResult = $Connexion->query($tQuery);");
                    tFile.AppendLine("if (!$tResult)");
                    tFile.AppendLine("{");
                    //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery", "$Connexion"));
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx09, ClassNamePHP));
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);

                //---------------------------------------
                tFile.AppendLine("// " + PHP_FUNCTION_FLUSH_TRASH_DATAS() + " ");
                tFile.AppendLine("function " + PHP_FUNCTION_FLUSH_TRASH_DATAS() + " ()");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                    tFile.AppendLine("global $admin;");
                    tFile.AppendLine("$tQuery = 'DELETE FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().XX) + ">0';");
                    tFile.AppendLine("if ($admin == true)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("// we are admin");
                        tFile.AppendLine("$tResult = ExecuteInAllConnexions($tQuery,'" + NWDError.GetErrorCode(NWDError.NWDError_XXx40) + "', '" + ClassNamePHP + "', true);");
                        tFile.AppendLine("if ($tResult['error'] == true)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx40, ClassNamePHP));
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);

                //---------------------------------------
                tFile.AppendLine("?>");
                string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
                rReturn.Add(ClassNamePHP + "/" + NWD.K_MANAGEMENT_FILE, tFileFormatted);
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual Dictionary<string, string> CreatePHPAddonFiles(NWDAppEnvironment sEnvironment, bool sWriteOnDisk = true)
        {
            Dictionary<string, string> rReturn = new Dictionary<string, string>();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, string> CreatePHPSynchronisation(NWDAppEnvironment sEnvironment)
        {
            //NWDBenchmark.Start();
            Dictionary<string, string> rReturn = new Dictionary<string, string>();
            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
            {
                //string tClassName = ClassNamePHP;
                //string tTableName = ClassTableName;
                //string tTrigramme = ClassTrigramme;
                //Type tType = ClassType;
                StringBuilder tFile = new StringBuilder(string.Empty);
                //========= SYNCHRONIZATION FUNCTIONS FILE
                // if need Account reference I prepare the restriction
                List<string> tAccountReferenceUpdate = new List<string>();
                List<string> tAccountReference = new List<string>();
                List<string> tGameSaveReference = new List<string>();
                List<string> tAccountReferences = new List<string>();
                foreach (var tProp in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
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
                                    if (tProp.GetCustomAttribute(typeof(NWDDisableAccountDependence), true) == null)
                                    {
                                        tAccountReference.Add("`" + tProp.Name + "` LIKE \\''.EscapeString($sAccountReference).'\\' ");
                                        tAccountReferences.Add("`" + tProp.Name + "` IN (\\''.implode('\\', \\'', $sAccountReferences).'\\') ");
                                        // only if Not locked property
                                        if (tProp.GetCustomAttributes(typeof(NWDLockUpdateForAccountAttribute), true).Length == 0)
                                        {
                                            tAccountReferenceUpdate.Add("`" + tProp.Name + "` LIKE \\''.EscapeString($sAccountReference).'\\' ");
                                        }
                                    }
                                }
                                if (tSubType == typeof(NWDGameSave))
                                {
                                    tGameSaveReference.Add("`" + tProp.Name + "` LIKE \\''.EscapeString($sGameSaveReference).'\\' ");
                                }
                            }
                            else if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceHashType<>))
                            {
                                Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                                if (tSubType == typeof(NWDAccount))
                                {
                                    if (tProp.GetCustomAttribute(typeof(NWDDisableAccountDependence), true) == null)
                                    {
                                        tAccountReference.Add("`" + tProp.Name + "` LIKE \\''.EscapeString(md5($sAccountReference." + PHP_CONSTANT_SALT_A() + ")).'\\' ");
                                        // only if Not locked property
                                        if (tProp.GetCustomAttributes(typeof(NWDLockUpdateForAccountAttribute), true).Length == 0)
                                        {
                                            tAccountReferenceUpdate.Add("`" + tProp.Name + "` LIKE \\''.EscapeString(md5($sAccountReference." + PHP_CONSTANT_SALT_A() + ")).'\\' ");
                                        }
                                    }
                                }
                            }
                            else if (tTypeOfThis.IsSubclassOf(typeof(NWDReferenceMultiple)))
                            {
                                Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                                if (tSubType == typeof(NWDAccount))
                                {
                                    if (tProp.GetCustomAttribute(typeof(NWDDisableAccountDependence), true) == null)
                                    {
                                        tAccountReference.Add("`" + tProp.Name + "` LIKE \\'%'.EscapeString($sAccountReference).'%\\'");
                                        // only if Not locked property
                                        if (tProp.GetCustomAttributes(typeof(NWDLockUpdateForAccountAttribute), true).Length == 0)
                                        {
                                            tAccountReferenceUpdate.Add("`" + tProp.Name + "` LIKE \\'%'.EscapeString($sAccountReference).'%\\'");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                bool tINeedAdminAccount = true;
                bool tINeedMultiAccount = false;
                if (tAccountReference.Count == 0)
                {
                    if (tAccountReference.Count > 1)
                    {
                        tINeedMultiAccount = true;
                    }
                }
                else
                {
                    tINeedAdminAccount = false;
                }

                string SLQIntegrityOrderToSelect = "***";
                foreach (string tPropertyName in SLQIntegrityOrder())
                {
                    PropertyInfo tPropertyInfo = ClassType.GetProperty(tPropertyName, BindingFlags.Public | BindingFlags.Instance);
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

                tFile.AppendLine("<?php");
                tFile.AppendLine(sEnvironment.Headlines());
                tFile.AppendLine(NWD.K_CommentSeparator);
                tFile.AppendLine("// SYNCHRONIZATION FUNCTIONS");
                tFile.AppendLine(NWD.K_CommentSeparator);
                string tSpecialAdd = string.Empty;
                tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FUNCTIONS_PHP + "');");
                tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + ClassNamePHP + "/" + NWD.K_CONSTANTS_FILE + "');");
                tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
                if (ClassNamePHP != NWDBasisHelper.BasisHelper<NWDError>().ClassNamePHP)
                {
                    tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDError>().ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
                }
                tFile.AppendLine(NWD.K_CommentSeparator);

                // --------------------------------------
                tFile.AppendLine("// " + PHP_FUNCTION_INTEGRITY_TEST() + "");
                tFile.AppendLine("function " + PHP_FUNCTION_INTEGRITY_TEST() + " ($sCsv, $sSaltUI)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                    tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ";");
                    tFile.AppendLine("$rReturn = true;");
#if NWD_INTEGRITY_NONE
#else
                    tFile.AppendLine("$tCsvList = explode('" + NWDConstants.kStandardSeparator + "',$sCsv);");
                    tFile.AppendLine("$tIntegrity = array_pop($tCsvList);");
                    tFile.AppendLine("unset($tCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS) + "");
                    tFile.AppendLine("unset($tCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DevSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DevSync) + "");
                    tFile.AppendLine("unset($tCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().PreprodSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().PreprodSync) + "");
                    tFile.AppendLine("unset($tCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ProdSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ProdSync) + "");
                    tFile.AppendLine("unset($tCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().RangeAccess)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().RangeAccess) + "");
                    tFile.AppendLine("$tDataString = implode('',$tCsvList);");
                    //tFile.AppendLine(NWDError.PHP_log(sEnvironment, "tDataString : '.$tDataString.'"));
                    if (TemplateHelper.NeedUseAccountSalt())
                    {
                        tFile.AppendLine("$tCalculate = str_replace('" + NWDConstants.kStandardSeparator + "', '', md5(" + PHP_CONSTANT_SALT_A() + ".$tDataString.$sSaltUI));");
                    }
                    else
                    {
                        tFile.AppendLine("$tCalculate = str_replace('" + NWDConstants.kStandardSeparator + "', '', md5(" + PHP_CONSTANT_SALT_A() + ".$tDataString." + PHP_CONSTANT_SALT_B() + "));");
                    }
                    tFile.AppendLine("if ($tCalculate!=$tIntegrity)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$rReturn = false;");
                        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx88, ClassNamePHP));
                    }
                    tFile.AppendLine("}");
#endif
                    tFile.AppendLine("return $rReturn;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);

                // --------------------------------------
                tFile.AppendLine("//" + PHP_FUNCTION_INTERGRITY_REPLACE() + "");
                tFile.AppendLine("function " + PHP_FUNCTION_INTERGRITY_REPLACE() + " ($sCsvArray, $sIndex, $sValue, $sSaltUI)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                    tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ";");
#if NWD_INTEGRITY_NONE
                    tFile.AppendLine("array_pop($sCsvArray);");
                    tFile.AppendLine("$sCsvArray[] = '';");
#else
                    tFile.AppendLine("$sCsvList = $sCsvArray;");
                    tFile.AppendLine("$sCsvList[$sIndex] = $sValue;");
                    tFile.AppendLine("$tIntegrity = array_pop($sCsvList);");
                    tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS) + "");
                    tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DevSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DevSync) + "");
                    tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().PreprodSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().PreprodSync) + "");
                    tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ProdSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ProdSync) + "");
                    tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().RangeAccess)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().RangeAccess) + "");
                    tFile.AppendLine("$sDataString = implode('',$sCsvList);");
                    tFile.AppendLine(NWDError.PHP_log(sEnvironment, "sDataString : '.$sDataString.'"));

                    if (TemplateHelper.NeedUseAccountSalt())
                    {
                        tFile.AppendLine("$tCalculate = str_replace('|', '', md5(" + PHP_CONSTANT_SALT_A() + ".$sDataString.$sSaltUI));");
                    }
                    else
                    {
                        tFile.AppendLine("$tCalculate = str_replace('|', '', md5(" + PHP_CONSTANT_SALT_A() + ".$sDataString." + PHP_CONSTANT_SALT_B() + "));");
                    }
                    tFile.AppendLine("$sCsvArray[$sIndex] = $sValue;");
                    tFile.AppendLine("array_pop($sCsvArray);");
                    tFile.AppendLine("$sCsvArray[] = $tCalculate;");
#endif
                    tFile.AppendLine("return $sCsvArray;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);

                // --------------------------------------
                tFile.AppendLine("// " + PHP_FUNCTION_INTERGRITY_REPLACES() + "");
                tFile.AppendLine("function " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvArray, $sIndexesAndValues, $sSaltUI)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                    tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ";");

#if NWD_INTEGRITY_NONE
                    tFile.AppendLine("foreach(array_keys($sIndexesAndValues) as $tKey)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$sCsvArray[$tKey] = $sIndexesAndValues[$tKey];");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("array_pop($sCsvArray);");
                    tFile.AppendLine("$sCsvArray[] = '';");
#else
                    tFile.AppendLine("$sCsvList = $sCsvArray;");
                    tFile.AppendLine("foreach(array_keys($sIndexesAndValues) as $tKey)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$sCsvList[$tKey] = $sIndexesAndValues[$tKey];");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("$tIntegrity = array_pop($sCsvList);");
                    tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS) + "");
                    tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DevSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DevSync) + "");
                    tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().PreprodSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().PreprodSync) + "");
                    tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ProdSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ProdSync) + "");
                    tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().RangeAccess)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().RangeAccess) + "");
                    tFile.AppendLine("$sDataString = implode('',$sCsvList);");
                    tFile.AppendLine(NWDError.PHP_log(sEnvironment, "sDataString : '.$sDataString.'"));

                    if (TemplateHelper.NeedUseAccountSalt())
                    {
                        tFile.AppendLine("$tCalculate = str_replace('|', '', md5(" + PHP_CONSTANT_SALT_A() + ".$sDataString.$sSaltUI));");
                    }
                    else
                    {
                        tFile.AppendLine("$tCalculate = str_replace('|', '', md5(" + PHP_CONSTANT_SALT_A() + ".$sDataString." + PHP_CONSTANT_SALT_B() + "));");
                    }

                    tFile.AppendLine("foreach(array_keys($sIndexesAndValues) as $tKey)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$sCsvArray[$tKey] = $sIndexesAndValues[$tKey];");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("array_pop($sCsvArray);");
                    tFile.AppendLine("$sCsvArray[] = $tCalculate;");
#endif
                    tFile.AppendLine("return $sCsvArray;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);

                // --------------------------------------
                tFile.AppendLine("// " + PHP_FUNCTION_PREPARE_DATA() + "");
                tFile.AppendLine("function " + PHP_FUNCTION_PREPARE_DATA() + " ($sCsv)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                    tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ", " + NWD.K_PHP_TIME_SYNC + ";");
                    tFile.AppendLine("$sCsvList = explode('" + NWDConstants.kStandardSeparator + "',$sCsv);");
                    tFile.AppendLine("$sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS)) + "] = " + NWD.K_PHP_TIME_SYNC + ";// change DS");
                    tFile.AppendLine("if ($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DM)) + "]<" + NWD.K_PHP_TIME_SYNC + ")");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("$sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS)) + "] = $sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DM)) + "];");
                    }
                    tFile.AppendLine("}");
                    if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
                    {
                        tFile.AppendLine("$sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DevSync)) + "] = " + NWD.K_PHP_TIME_SYNC + ";// change DevSync");
                    }
                    else if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
                    {
                        tFile.AppendLine("$sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().PreprodSync)) + "] = " + NWD.K_PHP_TIME_SYNC + ";// change PreprodSync");
                    }
                    else if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
                    {
                        tFile.AppendLine("$sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ProdSync)) + "] = " + NWD.K_PHP_TIME_SYNC + ";// change ProdSync");
                    }
                    tFile.AppendLine("return $sCsvList;");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);

                // --------------------------------------
                tFile.AppendLine("function " + PHP_FUNCTION_INTEGRITY_SERVER_GENERATE() + " ($sRow, $sSaltUI)");
                tFile.AppendLine("{");
                {

                    tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                    tFile.AppendLine("global " + NWD.K_NWD_SLT_SRV + ";");
                    tFile.Append("$sDataServerString =''");
                    foreach (string tPropertyName in SLQIntegrityServerOrder())
                    {
                        tFile.Append(".$sRow['" + tPropertyName + "']");
                    }
                    tFile.AppendLine(";");
                    tFile.AppendLine(NWDError.PHP_log(sEnvironment, "sDataServerString : '.$sDataServerString.'"));

                    tFile.AppendLine("return str_replace('" + NWDConstants.kStandardSeparator + "', '', md5(" + NWD.K_NWD_SLT_SRV + ".$sDataServerString." + NWD.K_NWD_SLT_SRV + "));");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);
                // --------------------------------------

                // DATA Integrity generate
                tFile.AppendLine("function " + PHP_FUNCTION_INTEGRITY_GENERATE() + " ($sRow, $sSaltUI)");
                tFile.AppendLine("{");
                {
#if NWD_INTEGRITY_NONE
                    tFile.AppendLine("return '';");
#else
                    tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                    tFile.AppendLine("global " + NWD.K_ENV + ", " + NWD.K_NWD_SLT_SRV + ";");
                    tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ";");
                    tFile.Append("$sDataString =''");
                    foreach (string tPropertyName in SLQIntegrityOrder())
                    {
                        tFile.Append(".$sRow['" + tPropertyName + "']");
                    }
                    tFile.AppendLine(";");
                    tFile.AppendLine(NWDError.PHP_log(sEnvironment, "sDataString : '.$sDataString.'"));
                    if (TemplateHelper.NeedUseAccountSalt())
                    {
                        tFile.AppendLine("return str_replace('" + NWDConstants.kStandardSeparator + "', '', md5(" + PHP_CONSTANT_SALT_A() + ".$sDataString.$sSaltUI));");
                    }
                    else
                    {
                        tFile.AppendLine("return str_replace('" + NWDConstants.kStandardSeparator + "', '', md5(" + PHP_CONSTANT_SALT_A() + ".$sDataString." + PHP_CONSTANT_SALT_B() + "));");
                    }
#endif
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);

                // --------------------------------------
                // TODO refactor to be more easy to generate
                tFile.AppendLine("function " + PHP_FUNCTION_INTEGRITY_REEVALUATE() + " ($sConnexion, $sReference)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                    tFile.AppendLine("if (empty($sReference)){return;}");
                    tFile.AppendLine("$tConnexion = $sConnexion;");
                    tFile.AppendLine("global $WSBUILD, " + NWD.K_ENV + ", " + NWD.K_NWD_SLT_SRV + ", " + NWD.K_PHP_TIME_SYNC + ", $NWD_FLOAT_FORMAT;");
                    tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ", " + PHP_CONSTANT_WEBSERVICE() + ";");
                    tFile.AppendLine("$tQuery = 'SELECT " + SLQIntegrityOrderToSelect + " FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "` = \\''.EscapeString($sReference).'\\';';");
                    //tFile.AppendLine("$tResult = $sConnexion->query($tQuery);");
                    tFile.AppendLine("$tResult = SelectFromConnexion($sConnexion, $tQuery);");
                    tFile.AppendLine("if ($tResult['error'] == true)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tResult['error_log'].'"));
                        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx31, ClassNamePHP));
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("if ($tResult['count'] == 1)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("// I calculate the integrity and reinject the good value");
                            tFile.AppendLine("$tRow = NULL;");
                            tFile.AppendLine("foreach($tResult['connexions'] as $tConnexionKey => $tConnexionSub)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine("foreach($tResult['datas'][$tConnexionKey] as $tRowFinal)");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine("$tRow = $tRowFinal;");
                                }
                                tFile.AppendLine("}");
                            }
                            tFile.AppendLine("}");
                            tFile.AppendLine("$tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().WebModel) + "'] = " + PHP_CONSTANT_WEBSERVICE() + ";");
                            if (TemplateHelper.NeedUseAccountSalt())
                            {
                                tFile.AppendLine("$tSaltUI = GetAccountSalt($tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountInfos>().Account) + "']);");
                            }
                            else
                            {
                                tFile.AppendLine("$tSaltUI = '';");
                            }
                            tFile.AppendLine("$tCalculate = " + PHP_FUNCTION_INTEGRITY_GENERATE() + " ($tRow, $tSaltUI);");
                            tFile.AppendLine("$tCalculateServer = " + PHP_FUNCTION_INTEGRITY_SERVER_GENERATE() + " ($tRow, $tSaltUI);");

                            tFile.Append("$tUpdate = 'UPDATE `" + PHP_TABLENAME(sEnvironment) + "` SET `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Integrity) + "` = \\''.EscapeString($tCalculate).'\\',");
                            tFile.Append(" `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ServerHash) + "` = \\''.EscapeString($tCalculateServer).'\\',");
                            tFile.Append(" `" + PHP_ENV_SYNC(sEnvironment) + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\' ,");
                            tFile.AppendLine(" `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().WebModel) + "` = \\''." + PHP_CONSTANT_WEBSERVICE() + ".'\\'" + " WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "` = \\''.EscapeString($tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "']).'\\';';");

                            if (tINeedAdminAccount == false && tINeedMultiAccount == false)
                            {
                                tFile.AppendLine("$tUpdateResult = ExecuteInConnexion($sConnexion, $tUpdate);");
                            }
                            else
                            {
                                tFile.AppendLine("$tUpdateResult = ExecuteInAllConnexions($tUpdate);");

                            }
                            tFile.AppendLine("if ($tUpdateResult['error'] == true)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tUpdateResult['error_log'].'"));
                                tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx91, ClassNamePHP));
                            }
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);

                // --------------------------------------
                //// TODO refactor to be more easy to generate
                //tFile.AppendLine("function " + PHP_FUNCTION_INTEGRITY_SERVER_VALIDATE_BY_ROW() + " ($sRow)");
                //tFile.AppendLine("{");
                //{
                //    tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                //    tFile.AppendLine("global " + NWD.K_NWD_SLT_SRV + ";");
                //    tFile.AppendLine("$tCalculateServer = " + PHP_FUNCTION_INTEGRITY_SERVER_GENERATE() + " ($sRow);");
                //    tFile.AppendLine("if ($tCalculateServer == $sRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ServerHash) + "'])");
                //    tFile.AppendLine("{");
                //    {
                //        tFile.AppendLine("return true;");
                //    }
                //    tFile.AppendLine("}");
                //    tFile.AppendLine("return false;");
                //}
                //tFile.AppendLine("}");
                //tFile.AppendLine(NWD.K_CommentSeparator);

                // --------------------------------------
                tFile.AppendLine("// " + PHP_FUNCTION_UPDATE_DATA() + " ");
                tFile.AppendLine("function " + PHP_FUNCTION_UPDATE_DATA() + " ($sCsv, $sTimeStamp, $sAccountReference, $sSaltUI, $sAdmin)");
                tFile.AppendLine("{");
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                List<string> tModify = new List<string>();
                List<string> tColumnNameList = new List<string>();
                List<string> tColumnValueList = new List<string>();
                tColumnNameList.Add("`Reference`");
                tColumnValueList.Add("\\''.EscapeString($sCsvList[0]).'\\'");
                int tIndex = 1;
                foreach (string tProperty in SLQAssemblyOrderArray())
                {
                    tModify.Add("`" + tProperty + "` = \\''.EscapeString($sCsvList[" + tIndex.ToString() + "]).'\\'");
                    tColumnNameList.Add("`" + tProperty + "`");
                    tColumnValueList.Add("\\''.EscapeString($sCsvList[" + tIndex.ToString() + "]).'\\'");
                    tIndex++;
                }
                tFile.AppendLine("global $WSBUILD, " + NWD.K_ENV + ", " + NWD.K_NWD_SLT_SRV + ", " + NWD.K_PHP_TIME_SYNC + ", $NWD_FLOAT_FORMAT, $ACC_NEEDED, " + NWD.K_PATH_BASE + ", $REF_NEEDED, $REP;");
                tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + "," + PHP_CONSTANT_WEBSERVICE() + ", " + NWD.K_PATH_BASE + ";");
                tFile.AppendLine("global $admin, $uuid;");
                tFile.AppendLine("$sCsvList = " + PHP_FUNCTION_PREPARE_DATA() + "($sCsv);");
                tFile.AppendLine("if ($admin == true)");
                tFile.AppendLine("{");
                if (TemplateHelper.NeedUseAccountSalt())
                {
                    tFile.AppendLine("$sSaltUI = GetAccountSalt($sCsvList[" + CSV_IndexOf(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDAccountInfos>().Account)) + "]);");
                }
                else
                {
                    tFile.AppendLine("$sSaltUI = '';");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("if (" + PHP_FUNCTION_INTEGRITY_TEST() + " ($sCsv, $sSaltUI) == true)");
                tFile.AppendLine("{");
                tFile.AppendLine("if (count ($sCsvList) != " + tColumnNameList.Count.ToString() + ")");
                tFile.AppendLine("{");
                tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx99, ClassNamePHP));
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                tFile.AppendLine("$tReference = $sCsvList[" + CSV_IndexOf(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference)) + "];");

                tFile.AppendLine("if (empty($tReference)){return;}");

                tFile.AppendLine("// find the good database");
                tFile.AppendLine(NWDError.PHP_log(sEnvironment, " I will try to update the data '.$tReference.'"));


                tFile.AppendLine("// find solution for pre calculate on server");
                tFile.AppendLine("if ($sCsvList[" + CSV_IndexOf(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().XX)) + "] == 1)");
                tFile.AppendLine("{");
                tFile.AppendLine("// replace XX by " + NWD.K_PHP_TIME_SYNC + "");
                tFile.AppendLine("$sCsvList = " + PHP_FUNCTION_INTERGRITY_REPLACE() + "($sCsvList, " + CSV_IndexOf(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().XX)) + ", " + NWD.K_PHP_TIME_SYNC + ", $sSaltUI);");
                tFile.AppendLine("}");
                tFile.Append(AddonPhpPreCalculate(sEnvironment));
                tFile.AppendLine("$tQuery = 'SELECT `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "`, `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DM) + "`, `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().XX) + "` FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "` = \\''.EscapeString($tReference).'\\';';");
                tFile.AppendLine("$tResult = SelectFromCurrentConnexion($tQuery);");
                tFile.AppendLine("if ($tResult['error'] == true)");
                tFile.AppendLine("{");
                tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tResult['error_log'].'"));
                tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx31, ClassNamePHP));
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                tFile.AppendLine("if ($tResult['count'] <= 1)");
                tFile.AppendLine("{");
                tFile.AppendLine("if ($tResult['count'] == 0)");
                tFile.AppendLine("{");
                tFile.AppendLine("$tInsert = 'INSERT INTO `" + PHP_TABLENAME(sEnvironment) + "` (" + string.Join(", ", tColumnNameList.ToArray()) + ") VALUES (" + string.Join(", ", tColumnValueList.ToArray()) + ");';");
                if (tINeedAdminAccount == false && tINeedMultiAccount == false)
                {
                    tFile.AppendLine("$tInsertResult = ExecuteInCurrentConnexion($tInsert);");
                }
                else
                {
                    tFile.AppendLine("$tInsertResult = ExecuteInAllConnexions($tInsert);");
                }
                tFile.AppendLine("if ($tInsertResult['error'] == true)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tInsertResult['error_log'].'"));
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx32, ClassNamePHP));
                }
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                //if (kAccountDependent == true)
                if (TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
                {
                    tFile.AppendLine("$tRow = NULL;");
                    tFile.AppendLine("foreach($tResult['connexions'] as $tConnexionKey => $tConnexionSub)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("foreach($tResult['datas'][$tConnexionKey] as $tRowFinal)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("$tRow = $tRowFinal;");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("if ($tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().XX) + "'] > '0')");
                    tFile.AppendLine("{");
                    tFile.AppendLine(NWDError.PHP_log(sEnvironment, "Data is trashed in database ... it is not possible to update untrash account dependent!"));
                    tFile.AppendLine(NWDError.PHP_log(sEnvironment, "Data is download again ... and please DELETE IT"));
                    tFile.AppendLine(PHP_FUNCTION_GET_DATA_BY_REFERENCE() + "($tConnexionSub, $tRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "']);");
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                }
                //"while($tRow = $tResult->fetch_row())"+
                //"{" );
                tFile.Append("$tUpdate = 'UPDATE `" + PHP_TABLENAME(sEnvironment) + "` SET ");
                tFile.Append(string.Join(", ", tModify.ToArray()) + " WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "` = \\''.EscapeString($tReference).'\\' ");
                tFile.AppendLine("';");
                if (tAccountReference.Count == 0)
                {
                    tFile.AppendLine("$tUpdateRestriction = '';");
                }
                else
                {
                    tFile.AppendLine("$tUpdateRestriction = 'AND (" + string.Join(" OR ", tAccountReferenceUpdate.ToArray()) + ") ';");
                }
                tFile.AppendLine("if ($admin == false)");
                tFile.AppendLine("{");
                tFile.AppendLine("$tUpdate = $tUpdate.$tUpdateRestriction.' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().WebModel) + "` <= '.$WSBUILD.'';");
                tFile.AppendLine("}");

                if (tINeedAdminAccount == false && tINeedMultiAccount == false)
                {
                    tFile.AppendLine("$tUpdateResult = ExecuteInCurrentConnexion($tUpdate);");
                    tFile.AppendLine("if ($tUpdateResult['error'] == true)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tUpdateResult['error_log'].'"));
                        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx38, ClassNamePHP));
                    }
                    tFile.AppendLine("}");
                }
                else
                {
                    tFile.AppendLine("$tUpdateResult  = ExecuteInAllConnexions($tUpdate);");
                    tFile.AppendLine("if ($tUpdateResult['error'] == true)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tUpdateResult['error_log'].'"));
                        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx38, ClassNamePHP));
                    }
                    tFile.AppendLine("}");
                }
                //"}" );
                tFile.AppendLine("}");

                tFile.AppendLine("// Update is finished!");

                //if (kAccountDependent == true)
                if (TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
                {
                    tFile.AppendLine("}");
                }

                tFile.AppendLine("// Solution for post calculate on server");
                tFile.Append(AddonPhpPostCalculate(sEnvironment));

                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                tFile.AppendLine(NWDError.PHP_log(sEnvironment, " erreur in result of '.$tQuery.'"));
                tFile.AppendLine(NWDError.PHP_log(sEnvironment, " row number is '.$tResult->num_rows.'"));
                tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx39, ClassNamePHP));
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);

                //---------------------------------------
                tFile.AppendLine("// " + PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ");
                tFile.AppendLine("function " + PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($sConnexion, $sReference)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                    //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                    tFile.AppendLine("global $WSBUILD, " + NWD.K_ENV + ", $REF_NEEDED, $ACC_NEEDED, $uuid;");
                    tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + "," + PHP_CONSTANT_WEBSERVICE() + ", " + NWD.K_PATH_BASE + ";");
                    tFile.AppendLine("global $REP;");
                    tFile.AppendLine("global $admin;");
                    // TODO improve this test
                    tFile.AppendLine("if (empty($sReference)){return;}");
                    //"$tPage = $sPage*$sLimit;" );
                    tFile.AppendLine("$tQuery = 'SELECT " + SLQSelect() + " FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "` = \\''.EscapeString($sReference).'\\'';");
                    tFile.AppendLine("if ($admin == false)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("// we are not admin, just an player");
                        tFile.AppendLine("$tQuery = $tQuery.' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().WebModel) + "` <= '.$WSBUILD.';';");

                        tFile.AppendLine("$tResult = SelectFromCurrentConnexion($tQuery);");
                        tFile.AppendLine("if ($tResult['error'] == true)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tResult['error_log'].'"));
                            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx33, ClassNamePHP));
                        }
                        tFile.AppendLine("}");
                        tFile.AppendLine("else");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("foreach($tResult['connexions'] as $tConnexionKey => $tConnexionSub)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine("foreach($tResult['datas'][$tConnexionKey] as $tRow)");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine("$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");

                                    tFile.Append(AddonPhpGetCalculate(sEnvironment));
                                }
                                tFile.AppendLine("}");
                            }
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("// we are admin");
                        if (tINeedAdminAccount == false || tINeedMultiAccount == false)
                        {
                            tFile.AppendLine("$tResult = SelectFromAllConnexions($tQuery, '" + NWDError.GetErrorCode(NWDError.NWDError_XXx33) + "', '', true);");
                        }
                        else
                        {
                            tFile.AppendLine("$tResult = SelectFromConnexion($sConnexion, $tQuery, '" + NWDError.GetErrorCode(NWDError.NWDError_XXx33) + "', '', true);");
                        }
                        tFile.AppendLine("if ($tResult['error'] == true)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tResult['error_log'].'"));
                            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx33, ClassNamePHP));
                        }
                        tFile.AppendLine("}");
                        tFile.AppendLine("else");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("foreach($tResult['connexions'] as $tConnexionKey => $tConnexionSub)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine("foreach($tResult['datas'][$tConnexionKey] as $tRow)");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine("if (isset($REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "']) == false) {$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'] = array(); }");
                                    tFile.AppendLine("$r = implode('|',$tRow); if (in_array($r,$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "']) == false){$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'][] = $r;};");

                                    tFile.Append(AddonPhpGetCalculate(sEnvironment));
                                }
                                tFile.AppendLine("}");
                            }
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);
                //---------------------------------------
                tFile.AppendLine("// " + PHP_FUNCTION_GET_DATAS_BY_REFERENCES() + " ");
                tFile.AppendLine("function " + PHP_FUNCTION_GET_DATAS_BY_REFERENCES() + " ($sReferences)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                    tFile.AppendLine("global $WSBUILD, " + NWD.K_ENV + ", $REF_NEEDED, $ACC_NEEDED, $uuid;");
                    tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + "," + PHP_CONSTANT_WEBSERVICE() + ", " + NWD.K_PATH_BASE + ";");
                    tFile.AppendLine("global $REP;");
                    tFile.AppendLine("global $admin;");
                    // TODO improve this test
                    tFile.AppendLine("if (empty($sReferences)){return;}");
                    //tFile.AppendLine("if (is_array($sReferences) == false){return;}");
                    //tFile.AppendLine("if ($sReferences == ''){return;}");

                    //"$tPage = $sPage*$sLimit;" );
                    tFile.AppendLine("$tQuery = 'SELECT " + SLQSelect() + " FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "` IN ( \\''.implode('\\', \\'', EscapeString($sReferences)).'\\')';");
                    tFile.AppendLine("if ($admin == false)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("// we are not admin, just an player");
                        tFile.AppendLine("$tQuery = $tQuery.' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().WebModel) + "` <= '.$WSBUILD.';';");

                        tFile.AppendLine("$tResult = SelectFromCurrentConnexion($tQuery);");
                        tFile.AppendLine("if ($tResult['error'] == true)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tResult['error_log'].'"));
                            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx33, ClassNamePHP));
                        }
                        tFile.AppendLine("}");
                        tFile.AppendLine("else");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("foreach($tResult['connexions'] as $tConnexionKey => $tConnexionSub)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine("foreach($tResult['datas'][$tConnexionKey] as $tRow)");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine("$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");

                                    tFile.Append(AddonPhpGetCalculate(sEnvironment));
                                }
                                tFile.AppendLine("}");
                            }
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("// we are admin");
                        if (tINeedAdminAccount == false || tINeedMultiAccount == false)
                        {
                            tFile.AppendLine("$tResult = SelectFromAllConnexions($tQuery);");
                        }
                        else
                        {
                            tFile.AppendLine("$tResult = SelectFromConnexion($sConnexion);");
                        }
                        tFile.AppendLine("if ($tResult['error'] == true)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tResult['error_log'].'"));
                            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx33, ClassNamePHP));
                        }
                        tFile.AppendLine("}");
                        tFile.AppendLine("else");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("foreach($tResult['connexions'] as $tConnexionKey => $tConnexionSub)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine("foreach($tResult['datas'][$tConnexionKey] as $tRow)");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine("if (isset($REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "']) == false) {$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'] = array(); }");
                                    tFile.AppendLine("$r = implode('|',$tRow); if (in_array($r,$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "']) == false){$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'][] = $r;};");

                                    tFile.Append(AddonPhpGetCalculate(sEnvironment));
                                }
                                tFile.AppendLine("}");
                            }
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);

                //---------------------------------------
                tFile.AppendLine("// " + PHP_FUNCTION_GET_DATAS() + " ");
                tFile.AppendLine("function " + PHP_FUNCTION_GET_DATAS() + " ($sTimeStamp, $sAccountReference)");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                    //tFile.AppendLine("$sAccountReferenceSure = str_replace('" + NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE + "','',str_replace('" + NWDAccount.K_ACCOUNT_PREFIX_TRIGRAM + "','',$sAccountReference));");
                    //tFile.AppendLine("$sAccountReferenceSure = str_replace('%','',str_replace('_','',$sAccountReferenceSure));");

                    tFile.AppendLine("global $WSBUILD, " + NWD.K_ENV + ", $REF_NEEDED, $ACC_NEEDED, $uuid;");
                    tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + "," + PHP_CONSTANT_WEBSERVICE() + ", " + NWD.K_PATH_BASE + ";");
                    tFile.AppendLine("global $REP;");
                    tFile.AppendLine("global $admin;");
                    if (sEnvironment.LogMode != NWDEnvironmentLogMode.NoLog)
                    {
                        tFile.AppendLine("$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "_list'][] = $sTimeStamp;");
                    }
                    //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                    //"$tPage = $sPage*$sLimit;" );
                    tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                    tFile.Append("$tQuery = 'SELECT " + SLQSelect() + " FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE ");
                    //"(`'."+NWD.K_ENV+".'Sync` >= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimeStamp).'\\' OR `DS` >= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimeStamp).'\\')";
                    tFile.Append("(`" + PHP_ENV_SYNC(sEnvironment) + "` >= \\''.EscapeString($sTimeStamp).'\\')");
                    // if need Account reference
                    if (tAccountReference.Count > 0)
                    {
                        tFile.Append("AND (" + string.Join("OR ", tAccountReference.ToArray()) + ") ");
                    }
                    tFile.AppendLine("';");
                    tFile.AppendLine("if ($admin == false)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("// we are not admin, just an player");
                        tFile.AppendLine("$tQuery = $tQuery.' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().WebModel) + "` <= '.$WSBUILD.';';");

                        tFile.AppendLine("$tResult = SelectFromCurrentConnexion($tQuery);");
                        tFile.AppendLine("if ($tResult['error'] == true)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tResult['error_log'].'"));
                            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx33, ClassNamePHP));
                        }
                        tFile.AppendLine("}");
                        tFile.AppendLine("else");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("foreach($tResult['connexions'] as $tConnexionKey => $tConnexionSub)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine("foreach($tResult['datas'][$tConnexionKey] as $tRow)");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine("$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");

                                    tFile.Append(AddonPhpGetCalculate(sEnvironment));
                                }
                                tFile.AppendLine("}");
                            }
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("// we are admin");
                        if (tINeedAdminAccount == false || tINeedMultiAccount == false)
                        {
                            tFile.AppendLine("$tResult = SelectFromAllConnexions($tQuery);");
                        }
                        else
                        {
                            tFile.AppendLine("$tResult = SelectFromCurrentConnexion($tQuery);");
                        }
                        tFile.AppendLine("if ($tResult['error'] == true)");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "'.$tResult['error_log'].'"));
                            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx33, ClassNamePHP));
                        }
                        tFile.AppendLine("}");
                        tFile.AppendLine("else");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("foreach($tResult['connexions'] as $tConnexionKey => $tConnexionSub)");
                            tFile.AppendLine("{");
                            {
                                tFile.AppendLine("foreach($tResult['datas'][$tConnexionKey] as $tRow)");
                                tFile.AppendLine("{");
                                {
                                    tFile.AppendLine("if (isset($REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "']) == false) {$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'] = array(); }");
                                    tFile.AppendLine("$r = implode('|',$tRow); if (in_array($r,$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "']) == false){$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'][] = $r;};");

                                    tFile.Append(AddonPhpGetCalculate(sEnvironment));
                                }
                                tFile.AppendLine("}");
                            }
                            tFile.AppendLine("}");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");

                }
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);

                //---------------------------------------


                tFile.AppendLine("// " + PHP_FUNCTION_SPECIAL() + "");
                tFile.AppendLine("function " + PHP_FUNCTION_SPECIAL() + " ($sAccountReferences)");
                tFile.AppendLine("{");
                //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("global $WSBUILD, " + NWD.K_ENV + ", " + NWD.K_NWD_SLT_SRV + ", " + NWD.K_PHP_TIME_SYNC + ", $NWD_FLOAT_FORMAT, $ACC_NEEDED, " + NWD.K_PATH_BASE + ", $REF_NEEDED, $REP;");
                tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + "," + PHP_CONSTANT_WEBSERVICE() + ", " + NWD.K_PATH_BASE + ";");
                tFile.AppendLine("global $admin, $uuid;");

                tFile.Append(AddonPhpSpecialCalculate(sEnvironment));

                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);
                //---------------------------------------

                string tFunctionsAdd = AddonPhpFunctions(sEnvironment);
                if (string.IsNullOrEmpty(tFunctionsAdd) == false)
                {
                    tFile.AppendLine(tFunctionsAdd);
                    tFile.AppendLine(NWD.K_CommentSeparator);
                }
                //---------------------------------------
                tFile.AppendLine("// " + PHP_FUNCTION_SYNCHRONIZE() + "");
                tFile.AppendLine("function " + PHP_FUNCTION_SYNCHRONIZE() + " ($sJsonDico, $sAccountReference, $sSaltUI, $sAdmin)");
                tFile.AppendLine("{");
                tFile.AppendLine(NWDError.PHP_BenchmarkStart(sEnvironment, PHP_FUNCTION_SYNCHRONIZE()));
                tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
                tFile.AppendLine("$tAccountReferenceSure = str_replace('%','',str_replace('_','',$sAccountReference));");
                //todo test lenght ?
                //todo test ereg structure?
                //tFile.AppendLine("$tAccountReferenceSure = str_replace('" + NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE + "','',str_replace('" + NWDAccount.K_ACCOUNT_PREFIX_TRIGRAM + "','',$sAccountReference));");
                //tFile.AppendLine("$tAccountReferenceSure = '" + NWDAccount.K_ACCOUNT_PREFIX_TRIGRAM + "'.$tAccountReferenceSure.'" + NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE + "';");
                tFile.AppendLine("global $token_FirstUse, " + NWD.K_PATH_BASE + ", " + NWD.K_PHP_TIME_SYNC + ", $REP, $CHANGE_USER, " + PHP_CONSTANT_SIGN() + ";");
                tFile.AppendLine("if (" + PHP_CONSTANT_SIGN() + " == $sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_WEBSIGN_KEY + "'])");
                tFile.AppendLine("{");
                if (ClassType.GetCustomAttributes(typeof(NWDForceSecureDataAttribute), true).Length > 0)
                {
                    tFile.AppendLine("respondAdd('" + NWD.K_JSON_SECURE_KEY + "',true);");
                }
                NWDOperationSpecial tOperation = NWDOperationSpecial.None;
                tFile.AppendLine("if ($sAdmin == true)");
                tFile.AppendLine("{");
                tFile.AppendLine("$tAccountReferenceSure = '%'; // bypass account limit for the admin");
                //Special?
                tOperation = NWDOperationSpecial.Special;
                tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']['" + tOperation.ToString().ToLower() + "']))");
                tFile.AppendLine("{");
                tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
                tFile.AppendLine("{");
                tFile.AppendLine("" + PHP_FUNCTION_SPECIAL() + " ($tAccountReferenceSure);");
                tFile.AppendLine(NWDError.PHP_log(sEnvironment, "SPECIAL : SPECIAL"));
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                // Clean data?
                tOperation = NWDOperationSpecial.Clean;
                tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']['" + tOperation.ToString().ToLower() + "']))");
                tFile.AppendLine("{");
                tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
                tFile.AppendLine("{");
                tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + ClassNamePHP + "/" + NWD.K_MANAGEMENT_FILE + "');");
                tFile.AppendLine("" + PHP_FUNCTION_FLUSH_TRASH_DATAS() + " ();");
                tFile.AppendLine(NWDError.PHP_log(sEnvironment, "SPECIAL : CLEAN"));
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                //Upgrade?
                tOperation = NWDOperationSpecial.Upgrade;
                tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']['" + tOperation.ToString().ToLower() + "']))");
                tFile.AppendLine("{");
                tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
                tFile.AppendLine("{");
                tFile.AppendLine("unset($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "']); ");
                tFile.AppendLine("unset($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "']); ");
                tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + ClassNamePHP + "/" + NWD.K_MANAGEMENT_FILE + "');");
                tFile.AppendLine("" + PHP_FUNCTION_CREATE_TABLE() + "();");
                tFile.AppendLine(NWDError.PHP_log(sEnvironment, "SPECIAL : UPGRADE"));
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                //Indexes?
                tOperation = NWDOperationSpecial.Indexes;
                tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']['" + tOperation.ToString().ToLower() + "']))");
                tFile.AppendLine("{");
                tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
                tFile.AppendLine("{");
                tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + ClassNamePHP + "/" + NWD.K_MANAGEMENT_FILE + "');");
                tFile.AppendLine("" + PHP_FUNCTION_CREATE_INDEX() + " ();");
                tFile.AppendLine(NWDError.PHP_log(sEnvironment, "SPECIAL : INDEXES"));
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                //Optimize?
                tOperation = NWDOperationSpecial.Optimize;
                tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']['" + tOperation.ToString().ToLower() + "']))");
                tFile.AppendLine("{");
                tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
                tFile.AppendLine("{");
                tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + ClassNamePHP + "/" + NWD.K_MANAGEMENT_FILE + "');");
                tFile.AppendLine("" + PHP_FUNCTION_DEFRAGMENT_TABLE() + " ();");
                tFile.AppendLine("" + PHP_FUNCTION_CHANGE_TABLE_ENGINE() + " ();");
                tFile.AppendLine("" + PHP_FUNCTION_CREATE_INDEX() + " ();");
                tFile.AppendLine(NWDError.PHP_log(sEnvironment, "SPECIAL : OPTIMIZE"));
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                // ENDIF ADMIN OPERATION
                tFile.AppendLine("}");
                tFile.AppendLine("if ($token_FirstUse == true)");
                tFile.AppendLine("{");
                if (tINeedAdminAccount == false)
                {
                    tFile.AppendLine("if ($CHANGE_USER == true)");
                    tFile.AppendLine("{");
                    {
                        //tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']))");
                        //tFile.AppendLine("{");
                        //{
                        //    tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "']))");
                        //    tFile.AppendLine("{");
                        //    {
                        //        tFile.AppendLine("foreach ($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'] as $sCsvValue)");
                        //        tFile.AppendLine("{");
                        //        {
                        //            tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
                        //            tFile.AppendLine("{");
                        //            {
                        //                tFile.AppendLine("" + PHP_FUNCTION_ANTICHEAT_DATA() + " ($sCsvValue, $sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "'], $tAccountReferenceSure, $sAdmin);");
                        //            }
                        //            tFile.AppendLine("}");
                        //            tFile.AppendLine("}");
                        //        }
                        //        tFile.AppendLine("}");
                        //    }
                        //    tFile.AppendLine("}");
                        //}
                        tFile.AppendLine("unset($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "']);");
                        tFile.AppendLine("$sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "'] = 0;");
                        //tFile.AppendLine("" + PHP_FUNCTION_GET_DATAS() + " (0, $sAccountReference);");
                        //tFile.AppendLine("$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "'] = "+NWD.K_PHP_TIME_SYNC+";");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("else");
                    tFile.AppendLine("{");
                }
                else
                {
                    tFile.AppendLine("if ($sAdmin == true)");
                    tFile.AppendLine("{");
                }
                tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']))");
                tFile.AppendLine("{");
                tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "']))");
                tFile.AppendLine("{");
                tFile.AppendLine("foreach ($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'] as $sCsvValue)");
                tFile.AppendLine("{");
                tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
                tFile.AppendLine("{");
                tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "']))");
                tFile.AppendLine("{");
                tFile.AppendLine("" + PHP_FUNCTION_UPDATE_DATA() + " ($sCsvValue, $sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "'], $tAccountReferenceSure, $sSaltUI, $sAdmin);");
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                tFile.AppendLine("" + PHP_FUNCTION_UPDATE_DATA() + " ($sCsvValue, " + NWD.K_PHP_TIME_SYNC + ", $tAccountReferenceSure, $sSaltUI, $sAdmin);");
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                if (tINeedAdminAccount == true)
                {
                    tFile.AppendLine("}");
                }
                else
                {
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                tFile.AppendLine("}");
                tFile.AppendLine("// any way get data");
                tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']))");
                tFile.AppendLine("{");
                {
                    tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "']))");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("" + PHP_FUNCTION_GET_DATAS() + " ($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "'], $tAccountReferenceSure);");
                            tFile.AppendLine("$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "'] = " + NWD.K_PHP_TIME_SYNC + ";");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                    tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_REF_KEY + "']) && $sAdmin == true)");
                    tFile.AppendLine("{");
                    {
                        tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
                        tFile.AppendLine("{");
                        {
                            tFile.AppendLine("" + PHP_FUNCTION_GET_DATAS_BY_REFERENCES() + " ($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_REF_KEY + "'], $tAccountReferenceSure);");
                            tFile.AppendLine("$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "'] = " + NWD.K_PHP_TIME_SYNC + ";");
                        }
                        tFile.AppendLine("}");
                    }
                    tFile.AppendLine("}");
                }
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
                tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx98, ClassNamePHP));
                tFile.AppendLine("}");
                tFile.AppendLine(NWDError.PHP_BenchmarkFinish(sEnvironment, PHP_FUNCTION_SYNCHRONIZE()));
                tFile.AppendLine("}");
                tFile.AppendLine(NWD.K_CommentSeparator);
                tFile.AppendLine("?>");
                string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
                rReturn.Add(ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION, tFileFormatted);
                //NWDBenchmark.Finish();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, string> CreatePHP(NWDAppEnvironment sEnvironment, bool sPrepareOrder = true)
        {
            //NWDBenchmark.Start();
            Dictionary<string, string> rReturn = new Dictionary<string, string>();
            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
            {
                PrefLoad();
                if (sPrepareOrder == true)
                {
                    PrepareOrders();
                }
                DetermineLast();
                foreach (KeyValuePair<string, string> tKeyValue in CreatePHPConstant(sEnvironment))
                {
                    rReturn.Add(tKeyValue.Key, tKeyValue.Value);
                }
                foreach (KeyValuePair<string, string> tKeyValue in CreatePHPEngine(sEnvironment))
                {
                    rReturn.Add(tKeyValue.Key, tKeyValue.Value);
                }
                foreach (KeyValuePair<string, string> tKeyValue in CreatePHPManagement(sEnvironment))
                {
                    rReturn.Add(tKeyValue.Key, tKeyValue.Value);
                }
                foreach (KeyValuePair<string, string> tKeyValue in CreatePHPSynchronisation(sEnvironment))
                {
                    rReturn.Add(tKeyValue.Key, tKeyValue.Value);
                }
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
