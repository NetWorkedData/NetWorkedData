//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:40
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
using System.Linq;
using System.Reflection;
using SQLite4Unity3d;
//using BasicToolBox;
using System.Text;

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
        NWDBasisHelperTableEngine TableEngine = NWDBasisHelperTableEngine.InnoDB;
        //-------------------------------------------------------------------------------------------------------------
        public string PHP_FUNCTION_CONSTANTS() { return ClassNamePHP + "Constants"; }
        public string PHP_FUNCTION_INTERGRITY_TEST() { return ClassNamePHP + "IntegrityTest"; }
        public string PHP_FUNCTION_INTERGRITY_REPLACE() { return ClassNamePHP + "IntegrityReplace"; }
        public string PHP_FUNCTION_INTERGRITY_REPLACES() { return ClassNamePHP + "IntegrityReplaces"; }
        public string PHP_FUNCTION_PREPARE_DATA() { return ClassNamePHP + "PrepareData"; }
        public string PHP_FUNCTION_LOG() { return ClassNamePHP + "Log"; }
        public string PHP_FUNCTION_INTEGRITY_SERVER_GENERATE() { return ClassNamePHP + "IntegrityServerGenerate"; }
        public string PHP_FUNCTION_INTEGRITY_GENERATE() { return ClassNamePHP + "IntegrityGenerate"; }
        public string PHP_FUNCTION_INTEGRITY_REEVALUATE() { return ClassNamePHP + "IntegrityReevalue"; }
        public string PHP_FUNCTION_INTEGRITY_SERVER_VALIDATE() { return ClassNamePHP + "IntegrityServerValidate"; }
        public string PHP_FUNCTION_INTEGRITY_SERVER_VALIDATE_BY_ROW() { return ClassNamePHP + "IntegrityServerValidateByRow"; }
        public string PHP_FUNCTION_INTEGRITY_VALIDATE() { return ClassNamePHP + "IntegrityValidate"; }
        public string PHP_FUNCTION_INTEGRITY_VALIDATE_BY_ROW() { return ClassNamePHP + "IntegrityValidateByRow"; }
        public string PHP_FUNCTION_UPDATE_DATA() { return ClassNamePHP + "UpdateData"; }
        public string PHP_FUNCTION_ANTICHEAT_DATA() { return ClassNamePHP + "AntiCheatData"; }
        public string PHP_FUNCTION_FLUSH_TRASH_DATAS() { return ClassNamePHP + "FlushTrashedDatas"; }
        public string PHP_FUNCTION_GET_DATA_BY_REFERENCE() { return ClassNamePHP + "GetDataByReference"; }
        public string PHP_FUNCTION_GET_DATAS_BY_REFERENCES() { return ClassNamePHP + "GetDatasByReferences"; }
        public string PHP_FUNCTION_GET_DATAS() { return ClassNamePHP + "GetDatas"; }
        public string PHP_FUNCTION_GET_DATAS_BY_GAMESAVE() { return ClassNamePHP + "GetDatasByGameSave"; }
        public string PHP_FUNCTION_GET_DATAS_BY_ACCOUNT() { return ClassNamePHP + "GetDatasByAccounts"; }
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
            //NWEBenchmark.Start();
            Dictionary<string, string> rReturn = new Dictionary<string, string>();
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// CONSTANTS");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("include_once (" + NWDBasisHelper.PHP_FILE_FUNCTION_PATH(sEnvironment) + ");");
            tFile.AppendLine(NWD.K_CommentSeparator);
            // to bypass the global limitation of PHP in internal include : use function :-) 
            tFile.AppendLine("function " + PHP_FUNCTION_CONSTANTS() + "()");
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ", " + PHP_CONSTANT_WEBSERVICE() + "," + PHP_CONSTANT_SIGN() + ";");
            tFile.AppendLine("" + PHP_CONSTANT_SALT_A() + " = '" + SaltStart + "';");
            tFile.AppendLine("" + PHP_CONSTANT_SALT_B() + " = '" + SaltEnd + "';");
            tFile.AppendLine("" + PHP_CONSTANT_WEBSERVICE() + " = " + LastWebBuild + "; // last build for this model is " + LastWebBuild + " / " + NWDAppConfiguration.SharedInstance().WebBuild + "");

            tFile.AppendLine("// Add sign of " + WebServiceOrder(LastWebBuild) + " ");
            tFile.AppendLine("" + PHP_CONSTANT_SIGN() + " = '" + WebServiceSign(LastWebBuild) + "';");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("//Run this function to install globals of theses datas!");
            tFile.AppendLine(PHP_FUNCTION_CONSTANTS() + "();");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            rReturn.Add(ClassNamePHP + "/" + NWD.K_CONSTANTS_FILE, tFileFormatted);
            //NWEBenchmark.Finish();
            return rReturn;
        }
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, string> CreatePHPEngine(NWDAppEnvironment sEnvironment)
        {
            //NWEBenchmark.Start();
            Dictionary<string, string> rReturn = new Dictionary<string, string>();
            StringBuilder tFile = new StringBuilder(string.Empty);
            //tFile.AppendLine("<?php");
            //tFile.AppendLine(sEnvironment.Headlines());
            //tFile.AppendLine(NWD.K_CommentSeparator);
            //tFile.AppendLine("// ENGINE");
            //tFile.AppendLine(NWD.K_CommentSeparator);
            //tFile.AppendLine("include_once (" + NWDBasisHelper.PHP_FILE_FUNCTION_PATH(sEnvironment) + ");");
            //tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + ClassNamePHP + "/" + NWD.K_CONSTANTS_FILE + "');");
            //if (ClassNamePHP != NWDBasisHelper.BasisHelper<NWDError>().ClassNamePHP)
            //{
            //    tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDError>().ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
            //}
            //tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine(AddonPhpEngineCalculate(sEnvironment));
            //tFile.AppendLine(NWD.K_CommentSeparator);
            //tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            rReturn.Add(ClassNamePHP + "/" + NWD.K_WS_ENGINE, tFileFormatted);
            //NWEBenchmark.Finish();
            return rReturn;
        }

        private string PropertyInfoToSQLType(PropertyInfo sPropertyInfo)
        {
            string rReturn = "TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL";
            Type tTypeOfThis = sPropertyInfo.PropertyType;

            if (tTypeOfThis == typeof(int) ||
                tTypeOfThis == typeof(long) ||
                tTypeOfThis == typeof(Int16) ||
                tTypeOfThis == typeof(Int32) ||
                tTypeOfThis == typeof(Int64) ||
                tTypeOfThis.IsEnum
                )
            {
                rReturn = " INT(11) NOT NULL default 0";
            }
            else if (tTypeOfThis == typeof(float) ||
                tTypeOfThis == typeof(double) ||
                tTypeOfThis == typeof(Double))
            {
                rReturn = "DOUBLE NOT NULL default 0";
            }
            else if (tTypeOfThis == typeof(bool))
            {
                rReturn = "INT(1) NOT NULL default 0";
            }
            else if (tTypeOfThis == typeof(string) || tTypeOfThis == typeof(String))
            {
                rReturn = "TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL";
            }
            else
            {
                if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
                {
                    rReturn = "TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL";
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)))
                {
                    rReturn = " INT(11) NOT NULL default 0";
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)))
                {
                    rReturn = "DOUBLE NOT NULL default 0";
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)))
                {
                    rReturn = " INT(11) NOT NULL default 0";
                }
                else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
                {
                    rReturn = " INT(11) NOT NULL default 0";
                }
            }
            return rReturn;
        }
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
            //NWEBenchmark.Start();
            Dictionary<string, string> rReturn = new Dictionary<string, string>();
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
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FUNCTIONS_PHP + "');");
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + ClassNamePHP + "/" + NWD.K_CONSTANTS_FILE + "');");
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDError>().ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("function " + PHP_FUNCTION_CREATE_TABLE() + "()");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", " + NWD.K_ENV + ";");
            var tQuery = "CREATE TABLE IF NOT EXISTS `" + PHP_TABLENAME(sEnvironment) + "` (";
            //var tDeclarations = tTableMapping.Columns.Select(p => Orm.SqlDecl(p, true));
            //var tDeclarationsJoined = string.Join(",", tDeclarations.ToArray());
            //tDeclarationsJoined = tDeclarationsJoined.Replace('"', '`');
            //tDeclarationsJoined = tDeclarationsJoined.Replace("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ID) + "` integer", "`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ID) + "`   int(11) NOT NULL");
            ////tDeclarationsJoined = tDeclarationsJoined.Replace("`DC` integer", "`DC` int(11) NOT NULL DEFAULT 0");
            //tDeclarationsJoined = tDeclarationsJoined.Replace("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().AC) + "` integer", "`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().AC) + "`   int(11) NOT NULL DEFAULT 1");
            ////tDeclarationsJoined = tDeclarationsJoined.Replace("`DM` integer", "`DM` int(11) NOT NULL DEFAULT 0");
            ////tDeclarationsJoined = tDeclarationsJoined.Replace("`DD` integer", "`DD` int(11) NOT NULL DEFAULT 0");
            ////tDeclarationsJoined = tDeclarationsJoined.Replace("`DS` integer", "`DS` int(11) NOT NULL DEFAULT 0");
            ////tDeclarationsJoined = tDeclarationsJoined.Replace("`XX` integer", "`XX` int(11) NOT NULL DEFAULT 0");
            //foreach (TableMapping.Column tColumn in tTableMapping.Columns)
            //{
            //    if (tColumn.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ID) &&
            //        tColumn.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().AC))
            //    {
            //        tDeclarationsJoined = tDeclarationsJoined.Replace("`" + tColumn.Name + "` integer", "`" + tColumn.Name + "` int");
            //        tDeclarationsJoined = tDeclarationsJoined.Replace("`" + tColumn.Name + "` int", "`" + tColumn.Name + "` int(11) NOT NULL DEFAULT 0");
            //    }
            //}
            //tDeclarationsJoined = tDeclarationsJoined.Replace("varchar", "text CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL");
            //tDeclarationsJoined = tDeclarationsJoined.Replace("primary key autoincrement not null", string.Empty);
            //tQuery += tDeclarationsJoined;
            List<string> PropertiesSQL = new List<string>();
            foreach (PropertyInfo tPropertyInfo in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (tPropertyInfo != null)
                {
                    PropertiesSQL.Add("`" + tPropertyInfo.Name + "` " + PropertyInfoToSQLType(tPropertyInfo));
                }
            }
            tQuery += string.Join(",", PropertiesSQL.ToArray());
            tQuery += ") ENGINE="+ TableEngine.ToString()+" DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
            tFile.AppendLine("$tQuery = '" + tQuery + "';");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx01, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("$tQuery = 'ALTER TABLE `" + PHP_TABLENAME(sEnvironment) + "` ADD PRIMARY KEY (`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ID) + "`), ADD UNIQUE KEY `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ID) + "` (`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ID) + "`);';");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            //tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx02, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("$tQuery = 'ALTER TABLE `" + PHP_TABLENAME(sEnvironment) + "` MODIFY IF EXISTS `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ID) + "` int(11) NOT NULL AUTO_INCREMENT;';");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            //tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx03, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("");
            tFile.AppendLine("// Alter all existing table with new columns or change type columns");



            Dictionary<string, List<PropertyInfo>> tIndexesDico = new Dictionary<string, List<PropertyInfo>>();
            tIndexesDico.Add(NWD.K_BASIS_INDEX, new List<PropertyInfo>());
            tIndexesDico.Add(NWD.K_ACCOUNT_INDEX, new List<PropertyInfo>());


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
                    Type tTypeOfThis = tPropertyInfo.PropertyType;
                    if (tTypeOfThis.IsSubclassOf(typeof(NWDReference)) && tTypeOfThis.IsGenericType)
                    {
                        Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                        if (tSubType == typeof(NWDAccount) || tSubType == typeof(NWDGameSave))
                        {
                            if (tIndexesDico[NWD.K_BASIS_INDEX].Contains(tPropertyInfo) == false)
                            {
                                tIndexesDico[NWD.K_BASIS_INDEX].Add(tPropertyInfo);
                            }
                        }
                        if (tSubType == typeof(NWDAccount))
                        {
                            if (tIndexesDico[NWD.K_ACCOUNT_INDEX].Contains(tPropertyInfo) == false)
                            {
                                tIndexesDico[NWD.K_ACCOUNT_INDEX].Add(tPropertyInfo);
                            }
                        }
                    }

                    if (tPropertyInfo.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ID)
                        && tPropertyInfo.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference)
                       && tPropertyInfo.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DM)
                       && tPropertyInfo.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DC)
                       && tPropertyInfo.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().AC)
                       && tPropertyInfo.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DD)
                       && tPropertyInfo.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS)
                       && tPropertyInfo.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().XX)
                       )
                    {
                        tFile.AppendLine("$tQuery ='ALTER TABLE `" + PHP_TABLENAME(sEnvironment) + "` ADD COLUMN IF NOT EXISTS `" + tPropertyInfo.Name + "` " + PropertyInfoToSQLType(tPropertyInfo) + ";';");
                        tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
                        tFile.AppendLine("if (!$tResult)");
                        tFile.AppendLine("{");
                        tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
                        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx11, ClassNamePHP));
                        tFile.AppendLine("}");
                        tFile.AppendLine("$tQuery ='ALTER TABLE `" + PHP_TABLENAME(sEnvironment) + "` MODIFY IF EXISTS `" + tPropertyInfo.Name + "` " + PropertyInfoToSQLType(tPropertyInfo) + ";';");
                        tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
                        tFile.AppendLine("if (!$tResult)");
                        tFile.AppendLine("{");
                        tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
                        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx12, ClassNamePHP));
                        tFile.AppendLine("}");
                    }
                }
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("function " + PHP_FUNCTION_CREATE_INDEX() + "()");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", " + NWD.K_ENV + ";");

            //Dictionary<string, List<PropertyInfo>> tIndexesDico
            foreach (KeyValuePair<string, List<PropertyInfo>> tIndex in tIndexesDico)
            {
                tFile.AppendLine("$tRemove" + tIndex.Key + "Query = 'DROP INDEX IF EXISTS `" + tIndex.Key + "` ON `" + PHP_TABLENAME(sEnvironment) + "`;';");
                tFile.AppendLine("$tRemove" + tIndex.Key + "Result = " + NWD.K_SQL_CON + "->query($tRemove" + tIndex.Key + "Query);");
                tFile.AppendLine("if (!$tRemove" + tIndex.Key + "Result)");
                tFile.AppendLine("{");
                tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tRemove" + tIndex.Key + "Query"));
                tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx05, ClassNamePHP));
                tFile.AppendLine("}");
                List<string> tColumnNamesFinalList = new List<string>();
                foreach (PropertyInfo PropertyInfo in tIndex.Value)
                {
                    tColumnNamesFinalList.Add("`" + PropertyInfo.Name + "`" + PropertyInfoToSQLIndex(PropertyInfo));
                }
                if (tColumnNamesFinalList.Count > 0)
                {
                    tFile.AppendLine("$tCreate" + tIndex.Key + "Query = 'CREATE INDEX `" + tIndex.Key + "`ON `" + PHP_TABLENAME(sEnvironment) + "` (" + string.Join(", ", tColumnNamesFinalList) + ");';");
                    tFile.AppendLine("$tCreate" + tIndex.Key + "Result = " + NWD.K_SQL_CON + "->query($tCreate" + tIndex.Key + "Query);");
                    tFile.AppendLine("if (!$tCreate" + tIndex.Key + "Result)");
                    tFile.AppendLine("{");
                    tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tCreate" + tIndex.Key + "Query"));
                    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx05, ClassNamePHP));
                    tFile.AppendLine("}");
                }
            }

            //foreach (TableMapping.Column tColumn in tTableMapping.Columns)
            //{
            //    if (tColumn.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ID) &&
            //        tColumn.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) &&
            //        tColumn.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DM) &&
            //        tColumn.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DC) &&
            //        //tColumn.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().AC) &&
            //        tColumn.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DD) &&
            //        tColumn.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS) &&
            //        tColumn.Name != NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().XX))
            //    {
            //        tFile.AppendLine("$tQuery ='ALTER TABLE `" + PHP_TABLENAME(sEnvironment) + "` ADD COLUMN IF NOT EXISTS " +
            //            Orm.SqlDecl(tColumn, true).Replace(" varchar ", " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL ").Replace(" float ", " double ").Replace("\"", "`") +
            //            ";';");
            //        tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            //        tFile.AppendLine("if (!$tResult)");
            //        tFile.AppendLine("{");
            //        tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            //        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx11, ClassNamePHP));
            //        tFile.AppendLine("}");
            //        tFile.AppendLine("$tQuery ='ALTER TABLE `" + PHP_TABLENAME(sEnvironment) + "` MODIFY IF EXISTS " +
            //            Orm.SqlDecl(tColumn, true).Replace(" varchar ", " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL ").Replace(" float ", " double ").Replace("\"", "`") +
            //            ";';");
            //        tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            //        tFile.AppendLine("if (!$tResult)");
            //        tFile.AppendLine("{");
            //        tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            //        tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx12, ClassNamePHP));
            //        tFile.AppendLine("}");
            //    }
            //}


            //var indexes = new Dictionary<string, SQLite4Unity3d.SQLiteConnection.IndexInfo>();
            //foreach (var c in tTableMapping.Columns)
            //{
            //    foreach (var i in c.Indices)
            //    {
            //        var iname = i.Name ?? ClassTableName + "_" + c.Name;
            //        SQLite4Unity3d.SQLiteConnection.IndexInfo iinfo;
            //        if (!indexes.TryGetValue(iname, out iinfo))
            //        {
            //            iinfo = new SQLite4Unity3d.SQLiteConnection.IndexInfo
            //            {
            //                IndexName = iname,
            //                TableName = ClassTableName,
            //                Unique = i.Unique,
            //                Columns = new List<SQLite4Unity3d.SQLiteConnection.IndexedColumn>()
            //            };
            //            indexes.Add(iname, iinfo);
            //        }
            //        if (i.Unique != iinfo.Unique)
            //            throw new Exception("All the columns in an index must have the same value for their Unique property");
            //        iinfo.Columns.Add(new SQLite4Unity3d.SQLiteConnection.IndexedColumn
            //        {
            //            Order = i.Order,
            //            ColumnName = c.Name
            //        });
            //    }
            //}
            //foreach (var indexName in indexes.Keys)
            //{
            //    var index = indexes[indexName];
            //    string[] columnNamesRought = new string[index.Columns.Count];
            //    if (index.Columns.Count == 1)
            //    {
            //        columnNamesRought[0] = index.Columns[0].ColumnName;
            //    }
            //    else
            //    {
            //        index.Columns.Sort((lhs, rhs) =>
            //        {
            //            return lhs.Order - rhs.Order;
            //        });
            //        for (int i = 0, end = index.Columns.Count; i < end; ++i)
            //        {
            //            columnNamesRought[i] = index.Columns[i].ColumnName;
            //        }
            //    }

            //    List<string> columnNames = new List<string>(columnNamesRought);

            //    // Add special index
            //    foreach (var tProp in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            //    {
            //        foreach (NWDAddIndexed tIndex in tProp.GetCustomAttributes(typeof(NWDAddIndexed), true))
            //        {
            //            if (indexName == tIndex.IndexName)
            //            {
            //                if (columnNames.Contains(tIndex.IndexColumn) == false)
            //                {
            //                    columnNames.Add(tIndex.IndexColumn);
            //                }
            //            }
            //        }
            //    }

            //    // Add account columnnames in K_BASIS_INDEX
            //    List<string> columnNamesFinalList = new List<string>();
            //    foreach (string tName in columnNames)
            //    {
            //        PropertyInfo tColumnInfos = ClassType.GetProperty(tName);
            //        Type tColumnType = tColumnInfos.PropertyType;
            //        if (tColumnType.IsSubclassOf(typeof(NWEDataType)))
            //        {
            //            columnNamesFinalList.Add("`" + tName + "`(24)");
            //        }
            //        else if (tColumnType.IsSubclassOf(typeof(NWEDataTypeInt)))
            //        {
            //            columnNamesFinalList.Add("`" + tName + "`");
            //        }
            //        else if (tColumnType.IsSubclassOf(typeof(NWEDataTypeFloat)))
            //        {
            //            columnNamesFinalList.Add("`" + tName + "`");
            //        }
            //        else if (tColumnType.IsSubclassOf(typeof(NWEDataTypeEnum)))
            //        {
            //            columnNamesFinalList.Add("`" + tName + "`");
            //        }
            //        else if (tColumnType.IsSubclassOf(typeof(NWEDataTypeMask)))
            //        {
            //            columnNamesFinalList.Add("`" + tName + "`");
            //        }
            //        else if (tColumnType == typeof(string))
            //        {
            //            columnNamesFinalList.Add("`" + tName + "`(24)");
            //        }
            //        else if (tColumnType == typeof(string))
            //        {
            //            columnNamesFinalList.Add("`" + tName + "`(32)");
            //        }
            //        else
            //        {
            //            columnNamesFinalList.Add("`" + tName + "`");
            //        }
            //    }





            //    if (indexName == NWD.K_BASIS_INDEX)
            //    {
            //        foreach (var tProp in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            //        {

            //            Type tTypeOfThis = tProp.PropertyType;
            //            if (tTypeOfThis != null)
            //            {
            //                if (tTypeOfThis.IsGenericType)
            //                {

            //                    if (tTypeOfThis.IsSubclassOf(typeof(NWDReference)) && tTypeOfThis.IsGenericType)
            //                    {
            //                        Type tSubType = tTypeOfThis.GetGenericArguments()[0];
            //                        if (tSubType == typeof(NWDAccount) || tSubType == typeof(NWDGameSave))
            //                        {
            //                            columnNamesFinalList.Add("`" + tProp.Name + "`(24) ");
            //                        }
            //                    }
            //                    //    if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceType<>))
            //                    //{
            //                    //    Type tSubType = tTypeOfThis.GetGenericArguments()[0];
            //                    //    if (tSubType == typeof(NWDAccount))
            //                    //    {
            //                    //        if (columnNames.Contains(tProp.Name) == false)
            //                    //        {
            //                    //            columnNamesFinalList.Add("`" + tProp.Name + "`(24)");
            //                    //        }
            //                    //    }
            //                    //    if (tSubType == typeof(NWDGameSave))
            //                    //    {
            //                    //        if (columnNames.Contains(tProp.Name) == false)
            //                    //        {
            //                    //            columnNamesFinalList.Add("`" + tProp.Name + "`(24)");
            //                    //        }
            //                    //    }
            //                    //}
            //                    //else if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceHashType<>))
            //                    //{
            //                    //    Type tSubType = tTypeOfThis.GetGenericArguments()[0];
            //                    //    if (tSubType == typeof(NWDAccount))
            //                    //    {
            //                    //        if (columnNames.Contains(tProp.Name) == false)
            //                    //        {
            //                    //            columnNamesFinalList.Add("`" + tProp.Name + "`(24)");
            //                    //        }
            //                    //    }
            //                    //}
            //                }
            //            }
            //        }
            //    }

            //    string[] columnNamesFinal = columnNamesFinalList.ToArray<string>();
            //    tFile.AppendLine("$tRemoveIndexQuery = 'DROP INDEX `" + indexName + "` ON `" + PHP_TABLENAME(sEnvironment) + "`;';");
            //    tFile.AppendLine("$tRemoveIndexResult = " + NWD.K_SQL_CON + "->query($tRemoveIndexQuery);");
            //    string sqlFormat = "CREATE {2}INDEX `{3}` ON `{0}` ({1});";
            //    var sql = String.Format(sqlFormat, PHP_TABLENAME(sEnvironment), string.Join(", ", columnNamesFinal), index.Unique ? "UNIQUE " : "", indexName);
            //    tFile.AppendLine("$tQuery = '" + sql + "';");
            //    tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            //    tFile.AppendLine("if (!$tResult)");
            //    tFile.AppendLine("{");
            //    tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            //    tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx05, ClassNamePHP));
            //    tFile.AppendLine("}");
            //}



            //// List account properties for Special generic index
            //List<string> tAccountReference = new List<string>();
            //foreach (var tProp in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            //{
            //    Type tTypeOfThis = tProp.PropertyType;
            //    if (tTypeOfThis != null)
            //    {
            //        if (tTypeOfThis.IsGenericType)
            //        {
            //            if (tTypeOfThis.IsSubclassOf(typeof(NWDReference)) && tTypeOfThis.IsGenericType)
            //            {
            //                Type tSubType = tTypeOfThis.GetGenericArguments()[0];
            //                if (tSubType == typeof(NWDAccount))
            //                {
            //                    tAccountReference.Add("`" + tProp.Name + "`(24) ");
            //                }
            //            }

            //            //if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceType<>))
            //            //{
            //            //    Type tSubType = tTypeOfThis.GetGenericArguments()[0];
            //            //    if (tSubType == typeof(NWDAccount))
            //            //    {
            //            //        tAccountReference.Add("`" + tProp.Name + "`(24) ");
            //            //    }
            //            //}
            //            //else if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceHashType<>))
            //            //{
            //            //    Type tSubType = tTypeOfThis.GetGenericArguments()[0];
            //            //    if (tSubType == typeof(NWDAccount))
            //            //    {
            //            //        tAccountReference.Add("`" + tProp.Name + "`(24) ");
            //            //    }
            //            //}
            //        }
            //    }
            //}

            //// create generic index
            //// TODO : improve this index
            //List<string> tNWDIndexIndex = new List<string>(tAccountReference);
            //string tGenericIndex = "GenericIndex";
            //tNWDIndexIndex.Add("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "`(24)");
            //tNWDIndexIndex.Add("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().WebModel) + "`");
            //tNWDIndexIndex.Add("`" + PHP_ENV_SYNC(sEnvironment) + "`");
            //tFile.AppendLine("$tRemoveIndexQuery = 'DROP INDEX `" + tGenericIndex + "` ON `" + PHP_TABLENAME(sEnvironment) + "`;';");
            //tFile.AppendLine("$tRemoveIndexResult = " + NWD.K_SQL_CON + "->query($tRemoveIndexQuery);");
            //tFile.AppendLine("$tQuery = 'CREATE INDEX `" + tGenericIndex + "` ON `" + PHP_TABLENAME(sEnvironment) + "` ("
            //    + string.Join(", ", tNWDIndexIndex) +
            //    ");';");
            ////tFile.AppendLine(NWDError.PHP_log(sEnvironment,"----"+ string.Join(", ", tNWDIndexIndex) + "----"));
            //tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            //tFile.AppendLine("if (!$tResult)");
            //tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            //tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx05, ClassNamePHP));
            //tFile.AppendLine("}");
            ////


            //// create other generic index
            //// TODO : improve this index
            //List<string> tNWTrashIndex = new List<string>();
            //string tTrashIndex = "TrashIndex";
            //tNWTrashIndex.Add("`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().XX) + "`");
            //tFile.AppendLine("$tRemoveIndexQuery = 'DROP INDEX `" + tTrashIndex + "` ON `" + PHP_TABLENAME(sEnvironment) + "`;';");
            //tFile.AppendLine("$tRemoveIndexResult = " + NWD.K_SQL_CON + "->query($tRemoveIndexQuery);");
            //tFile.AppendLine("$tQuery = 'CREATE INDEX `" + tTrashIndex + "` ON `" + PHP_TABLENAME(sEnvironment) + "` ("
            //    + string.Join(", ", tNWTrashIndex) +
            //    ");';");
            //tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            //tFile.AppendLine("if (!$tResult)");
            //tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            //tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx05, ClassNamePHP));
            //tFile.AppendLine("}");
            ////



            //// create other generic index






            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + PHP_FUNCTION_CHANGE_TABLE_ENGINE() + " ()");
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", " + NWD.K_ENV + ";");
            tFile.AppendLine("$tQuery = 'ALTER TABLE `" + PHP_TABLENAME(sEnvironment) + "` ENGINE=" + TableEngine.ToString() + ";';");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx07, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + PHP_FUNCTION_DEFRAGMENT_TABLE() + " ()");
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", " + NWD.K_ENV + ";");
            tFile.AppendLine("$tQuery = 'ALTER TABLE `" + PHP_TABLENAME(sEnvironment) + "` ENGINE=" + TableEngine.ToString() + ";';");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx07, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + PHP_FUNCTION_DROP_TABLE() + " ()");
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", " + NWD.K_ENV + ";");
            tFile.AppendLine("$tQuery = 'DROP TABLE `" + PHP_TABLENAME(sEnvironment) + "`;';");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx08, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + PHP_FUNCTION_FLUSH_TABLE() + " ()");
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", " + NWD.K_ENV + ";");
            tFile.AppendLine("$tQuery = 'FLUSH TABLE `" + PHP_TABLENAME(sEnvironment) + "`;';");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx09, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            rReturn.Add(ClassNamePHP + "/" + NWD.K_MANAGEMENT_FILE, tFileFormatted);
            //NWEBenchmark.Finish();
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
            //NWEBenchmark.Start();
            Dictionary<string, string> rReturn = new Dictionary<string, string>();
            //string tClassName = ClassNamePHP;
            //string tTableName = ClassTableName;
            //string tTrigramme = ClassTrigramme;
            //Type tType = ClassType;
            StringBuilder tFile = new StringBuilder(string.Empty);
            //========= SYNCHRONIZATION FUNCTIONS FILE
            // if need Account reference I prepare the restriction
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
                                tAccountReference.Add("`" + tProp.Name + "` LIKE \\''." + NWD.K_SQL_CON + "->real_escape_string($sAccountReference).'\\' ");
                                tAccountReferences.Add("`" + tProp.Name + "` IN (\\''.implode('\\', \\'', $sAccountReferences).'\\') ");
                            }
                            if (tSubType == typeof(NWDGameSave))
                            {
                                tGameSaveReference.Add("`" + tProp.Name + "` LIKE \\''." + NWD.K_SQL_CON + "->real_escape_string($sGameSaveReference).'\\' ");
                                //tGameSaveReference.Add("`" + tProp.Name + "` IN (\\''.implode('\\', \\'', $sGameSaveReferences).'\\') ");
                            }
                        }
                        else if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceHashType<>))
                        {
                            Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                            if (tSubType == typeof(NWDAccount))
                            {
                                tAccountReference.Add("`" + tProp.Name + "` LIKE \\''." + NWD.K_SQL_CON + "->real_escape_string(md5($sAccountReference." + PHP_CONSTANT_SALT_A() + ")).'\\' ");
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

            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FUNCTIONS_PHP + "');");
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + ClassNamePHP + "/" + NWD.K_CONSTANTS_FILE + "');");
            tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
            if (ClassNamePHP != NWDBasisHelper.BasisHelper<NWDError>().ClassNamePHP)
            {
                tFile.AppendLine("include_once (" + NWD.K_PATH_BASE + ".'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + NWDBasisHelper.BasisHelper<NWDError>().ClassNamePHP + "/" + NWD.K_WS_ENGINE + "');");
            }
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("function " + PHP_FUNCTION_INTERGRITY_TEST() + " ($sCsv)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ";");
            tFile.AppendLine("$rReturn = true;");
            tFile.AppendLine("$tCsvList = explode('" + NWDConstants.kStandardSeparator + "',$sCsv);");
            tFile.AppendLine("$tIntegrity = array_pop($tCsvList);");
            tFile.AppendLine("unset($tCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS) + "");
            tFile.AppendLine("unset($tCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DevSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DevSync) + "");
            tFile.AppendLine("unset($tCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().PreprodSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().PreprodSync) + "");
            tFile.AppendLine("unset($tCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ProdSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ProdSync) + "");
            tFile.AppendLine("$tDataString = implode('',$tCsvList);");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "tDataString : '.$tDataString.'"));
            tFile.AppendLine("$tCalculate = str_replace('" + NWDConstants.kStandardSeparator + "', '', md5(" + PHP_CONSTANT_SALT_A() + ".$tDataString." + PHP_CONSTANT_SALT_B() + "));");
            tFile.AppendLine("if ($tCalculate!=$tIntegrity)");
            tFile.AppendLine("{");
            tFile.AppendLine("$rReturn = false;");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx88, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("return $rReturn;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + PHP_FUNCTION_INTERGRITY_REPLACE() + " ($sCsvArray, $sIndex, $sValue)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ";");
            tFile.AppendLine("$sCsvList = $sCsvArray;");
            tFile.AppendLine("$sCsvList[$sIndex] = $sValue;");
            tFile.AppendLine("$tIntegrity = array_pop($sCsvList);");
            tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS) + "");
            tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DevSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DevSync) + "");
            tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().PreprodSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().PreprodSync) + "");
            tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ProdSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ProdSync) + "");
            tFile.AppendLine("$sDataString = implode('',$sCsvList);");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "sDataString : '.$sDataString.'"));
            tFile.AppendLine("$tCalculate = str_replace('|', '', md5(" + PHP_CONSTANT_SALT_A() + ".$sDataString." + PHP_CONSTANT_SALT_B() + "));");
            tFile.AppendLine("$sCsvArray[$sIndex] = $sValue;");
            tFile.AppendLine("array_pop($sCsvArray);");
            tFile.AppendLine("$sCsvArray[] = $tCalculate;");
            tFile.AppendLine("return $sCsvArray;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + PHP_FUNCTION_INTERGRITY_REPLACES() + " ($sCsvArray, $sIndexesAndValues)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ";");
            tFile.AppendLine("$sCsvList = $sCsvArray;");
            tFile.AppendLine("foreach(array_keys($sIndexesAndValues) as $tKey)");
            tFile.AppendLine("{");
            tFile.AppendLine("$sCsvList[$tKey] = $sIndexesAndValues[$tKey];");
            tFile.AppendLine("}");
            tFile.AppendLine("$tIntegrity = array_pop($sCsvList);");
            tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS) + "");
            tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DevSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DevSync) + "");
            tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().PreprodSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().PreprodSync) + "");
            tFile.AppendLine("unset($sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ProdSync)) + "]);//remove " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ProdSync) + "");
            tFile.AppendLine("$sDataString = implode('',$sCsvList);");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "sDataString : '.$sDataString.'"));
            tFile.AppendLine("$tCalculate = str_replace('|', '', md5(" + PHP_CONSTANT_SALT_A() + ".$sDataString." + PHP_CONSTANT_SALT_B() + "));");
            tFile.AppendLine("foreach(array_keys($sIndexesAndValues) as $tKey)");
            tFile.AppendLine("{");
            tFile.AppendLine("$sCsvArray[$tKey] = $sIndexesAndValues[$tKey];");
            tFile.AppendLine("}");
            tFile.AppendLine("array_pop($sCsvArray);");
            tFile.AppendLine("$sCsvArray[] = $tCalculate;");
            tFile.AppendLine("return $sCsvArray;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + PHP_FUNCTION_PREPARE_DATA() + " ($sCsv)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ", " + NWD.K_PHP_TIME_SYNC + ";");
            tFile.AppendLine("$sCsvList = explode('" + NWDConstants.kStandardSeparator + "',$sCsv);");
            tFile.AppendLine("$sCsvList[2] = " + NWD.K_PHP_TIME_SYNC + ";// change DS");
            tFile.AppendLine("if ($sCsvList[1]<" + NWD.K_PHP_TIME_SYNC + ")");
            tFile.AppendLine("{");
            tFile.AppendLine("$sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DS)) + "] = $sCsvList[" + NWDBasisHelper.CSV_IndexOf<NWDExample>(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DM)) + "];");
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
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + PHP_FUNCTION_LOG() + " ($sReference, $sLog)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", " + NWD.K_ENV + ";");
            tFile.AppendLine("$tUpdate = 'UPDATE `" + PHP_TABLENAME(sEnvironment) + "` SET `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ServerLog) + "` = CONCAT(`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ServerLog) + "`, \\' ; '." + NWD.K_SQL_CON + "->real_escape_string($sLog).'\\') WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sReference).'\\';';");
            tFile.AppendLine("$tUpdateResult = " + NWD.K_SQL_CON + "->query($tUpdate);");
            tFile.AppendLine("if (!$tUpdateResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tUpdate"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx77, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // SERVER Integrity generate
            tFile.AppendLine("function " + PHP_FUNCTION_INTEGRITY_SERVER_GENERATE() + " ($sRow)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + NWD.K_NWD_SLT_SRV + ";");
            tFile.Append("$sDataServerString =''");
            foreach (string tPropertyName in SLQIntegrityServerOrder())
            {
                tFile.Append(".$sRow['" + tPropertyName + "']");
            }
            tFile.AppendLine(";");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "sDataServerString : '.$sDataServerString.'"));
            tFile.AppendLine("return str_replace('" + NWDConstants.kStandardSeparator + "', '', md5(" + NWD.K_NWD_SLT_SRV + ".$sDataServerString." + NWD.K_NWD_SLT_SRV + "));");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // DATA Integrity generate
            tFile.AppendLine("function " + PHP_FUNCTION_INTEGRITY_GENERATE() + " ($sRow)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", " + NWD.K_ENV + ", " + NWD.K_NWD_SLT_SRV + ";");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ";");
            tFile.Append("$sDataString =''");
            foreach (string tPropertyName in SLQIntegrityOrder())
            {
                tFile.Append(".$sRow['" + tPropertyName + "']");
            }
            tFile.AppendLine(";");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "sDataString : '.$sDataString.'"));
            tFile.AppendLine("return str_replace('" + NWDConstants.kStandardSeparator + "', '', md5(" + PHP_CONSTANT_SALT_A() + ".$sDataString." + PHP_CONSTANT_SALT_B() + "));");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // TODO refactor to be more easy to generate
            tFile.AppendLine("function " + PHP_FUNCTION_INTEGRITY_REEVALUATE() + " ($sReference)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", $WSBUILD, " + NWD.K_ENV + ", " + NWD.K_NWD_SLT_SRV + ", " + NWD.K_PHP_TIME_SYNC + ", $NWD_FLOAT_FORMAT;");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ", " + PHP_CONSTANT_WEBSERVICE() + ";");
            tFile.AppendLine("$tQuery = 'SELECT " + SLQIntegrityOrderToSelect + " FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sReference).'\\';';");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx31, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("// I calculate the integrity and reinject the good value");
            tFile.AppendLine("$tRow = $tResult->fetch_assoc();");
            //"$tRow['WebServiceVersion'] = $WSBUILD;" );
            tFile.AppendLine("$tRow['WebModel'] = " + PHP_CONSTANT_WEBSERVICE() + ";");
            tFile.AppendLine("$tCalculate = " + PHP_FUNCTION_INTEGRITY_GENERATE() + " ($tRow);");
            tFile.AppendLine("$tCalculateServer = " + PHP_FUNCTION_INTEGRITY_SERVER_GENERATE() + " ($tRow);");
            tFile.Append("$tUpdate = 'UPDATE `" + PHP_TABLENAME(sEnvironment) + "` SET `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Integrity) + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tCalculate).'\\',");
            tFile.Append(" `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ServerHash) + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tCalculateServer).'\\',");
            tFile.Append(" `" + PHP_ENV_SYNC(sEnvironment) + "` = \\''." + NWD.K_PHP_TIME_SYNC + ".'\\' ,");
            //tSynchronizationFile.Append(" `WebModel` = \\''.$WSBUILD.'\\'" );
            tFile.AppendLine(" `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().WebModel) + "` = \\''." + PHP_CONSTANT_WEBSERVICE() + ".'\\'" + " WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sReference).'\\';';");
            tFile.AppendLine("$tUpdateResult = " + NWD.K_SQL_CON + "->query($tUpdate);");
            tFile.AppendLine("if (!$tUpdateResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tUpdate"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx91, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // TODO refactor to be more easy to generate
            tFile.AppendLine("function " + PHP_FUNCTION_INTEGRITY_SERVER_VALIDATE() + " ($sReference)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", " + NWD.K_ENV + ", " + NWD.K_NWD_SLT_SRV + ";");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ";");
            tFile.AppendLine("$tQuery = 'SELECT " + SLQSelect() + " FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sReference).'\\';';");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx31, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("// I calculate the integrity and reinject the good value");
            tFile.AppendLine("$tRow = $tResult->fetch_assoc();");
            tFile.AppendLine("$tCalculateServer = " + PHP_FUNCTION_INTEGRITY_SERVER_GENERATE() + " ($tRow);");
            tFile.AppendLine("if ($tCalculateServer == $tRow['ServerHash'])");
            tFile.AppendLine("{");
            tFile.AppendLine("return true;");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("return false;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // TODO refactor to be more easy to generate
            tFile.AppendLine("function " + PHP_FUNCTION_INTEGRITY_SERVER_VALIDATE_BY_ROW() + " ($sRow)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + NWD.K_NWD_SLT_SRV + ";");
            tFile.AppendLine("$tCalculateServer = " + PHP_FUNCTION_INTEGRITY_SERVER_GENERATE() + " ($sRow);");
            tFile.AppendLine("if ($tCalculateServer == $sRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ServerHash) + "'])");
            tFile.AppendLine("{");
            tFile.AppendLine("return true;");
            tFile.AppendLine("}");
            tFile.AppendLine("return false;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // TODO refactor to be more easy to generate
            tFile.AppendLine("function " + PHP_FUNCTION_INTEGRITY_VALIDATE() + " ($sReference)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", " + NWD.K_ENV + ", " + NWD.K_NWD_SLT_SRV + ";");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ";");
            tFile.AppendLine("$tQuery = 'SELECT " + SLQSelect() + " FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sReference).'\\';';");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx31, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("// I calculate the integrity and reinject the good value");
            tFile.AppendLine("$tRow = $tResult->fetch_assoc();");
            tFile.AppendLine("$tCalculate = " + PHP_FUNCTION_INTEGRITY_GENERATE() + " ($tRow);");
            tFile.AppendLine("if ($tCalculate == $tRow['Integrity'])");
            tFile.AppendLine("{");
            tFile.AppendLine("return true;");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("return false;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // TODO refactor to be more easy to generate

            tFile.AppendLine("function " + PHP_FUNCTION_INTEGRITY_VALIDATE_BY_ROW() + " ($sRow)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", $WSBUILD, " + NWD.K_ENV + ", " + NWD.K_NWD_SLT_SRV + ";");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + "," + PHP_CONSTANT_WEBSERVICE() + ";");
            tFile.AppendLine("$tCalculate = " + PHP_FUNCTION_INTEGRITY_GENERATE() + " ($sRow);");
            tFile.AppendLine("if ($tCalculate == $sRow['" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Integrity) + "'])");
            tFile.AppendLine("{");
            tFile.AppendLine("return true;");
            tFile.AppendLine("}");
            tFile.AppendLine("return false;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + PHP_FUNCTION_UPDATE_DATA() + " ($sCsv, $sTimeStamp, $sAccountReference, $sAdmin)");
            List<string> tModify = new List<string>();
            List<string> tColumnNameList = new List<string>();
            List<string> tColumnValueList = new List<string>();
            tColumnNameList.Add("`Reference`");
            tColumnValueList.Add("\\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[0]).'\\'");
            int tIndex = 1;
            foreach (string tProperty in SLQAssemblyOrderArray())
            {
                tModify.Add("`" + tProperty + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + tIndex.ToString() + "]).'\\'");
                tColumnNameList.Add("`" + tProperty + "`");
                tColumnValueList.Add("\\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + tIndex.ToString() + "]).'\\'");
                tIndex++;
            }
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", $WSBUILD, " + NWD.K_ENV + ", " + NWD.K_NWD_SLT_SRV + ", " + NWD.K_PHP_TIME_SYNC + ", $NWD_FLOAT_FORMAT, $ACC_NEEDED, " + NWD.K_PATH_BASE + ", $REF_NEEDED, $REP;");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ", " + PHP_CONSTANT_WEBSERVICE() + ";");
            tFile.AppendLine("global $admin, $uuid;");
            tFile.AppendLine("if (" + PHP_FUNCTION_INTERGRITY_TEST() + " ($sCsv) == true)");
            tFile.AppendLine("{");
            tFile.AppendLine("$sCsvList = " + PHP_FUNCTION_PREPARE_DATA() + "($sCsv);");
            tFile.AppendLine("if (count ($sCsvList) != " + tColumnNameList.Count.ToString() + ")");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx99, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$tReference = $sCsvList[0];");
            tFile.AppendLine("// find solution for pre calculate on server");

            tFile.Append(AddonPhpPreCalculate(sEnvironment));
            tFile.AppendLine("$tQuery = 'SELECT `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "`, `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DM) + "` FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\';';");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx31, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows <= 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows == 0)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tInsert = 'INSERT INTO `" + PHP_TABLENAME(sEnvironment) + "` (" + string.Join(", ", tColumnNameList.ToArray()) + ") VALUES (" + string.Join(", ", tColumnValueList.ToArray()) + ");';");
            tFile.AppendLine("$tInsertResult = " + NWD.K_SQL_CON + "->query($tInsert);");
            tFile.AppendLine("if (!$tInsertResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx32, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            //"while($tRow = $tResult->fetch_row())"+
            //"{" );
            tFile.Append("$tUpdate = 'UPDATE `" + PHP_TABLENAME(sEnvironment) + "` SET ");
            tFile.Append(string.Join(", ", tModify.ToArray()) + " WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\' ");
            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                // tSynchronizationFile += "AND (`DevSync`<= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimeStamp).'\\') "; 
                //no test the last is the winner!
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                //tSynchronizationFile += "AND (`DevSync`<= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimeStamp).'\\' || `PreprodSync`<= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimeStamp).'\\') ";
                //tSynchronizationFile += "AND `PreprodSync`<= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimeStamp).'\\' ";
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                //tSynchronizationFile += "AND (`DevSync`<= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimeStamp).'\\' || `PreprodSync`<= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimeStamp).'\\' || `ProdSync`<= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimeStamp).'\\') ";
                //tSynchronizationFile += "AND `ProdSync`<= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimeStamp).'\\' ";
            }
            tFile.AppendLine("';");
            if (tAccountReference.Count == 0)
            {
                tFile.AppendLine("$tUpdateRestriction = '';");
            }
            else
            {
                tFile.AppendLine("$tUpdateRestriction = 'AND (" + string.Join(" OR ", tAccountReference.ToArray()) + ") ';");
            }
            tFile.AppendLine("if ($admin == false)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tUpdate = $tUpdate.$tUpdateRestriction.' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().WebModel) + "` <= '.$WSBUILD.'';");
            tFile.AppendLine("}");

            tFile.AppendLine("$tUpdateResult = " + NWD.K_SQL_CON + "->query($tUpdate);");
            tFile.AppendLine("if (!$tUpdateResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tUpdate"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx38, ClassNamePHP));
            tFile.AppendLine("}");
            //"}" );
            tFile.AppendLine("}");
            tFile.AppendLine("// Solution for post calculate on server");

            tFile.Append(AddonPhpPostCalculate(sEnvironment));

            tFile.AppendLine("// Update is finished!");
            /*
            tFile.AppendLine("$tLigneAffected = "+NWD.K_SQL_CON+"->affected_rows;");
            tFile.AppendLine("if ($tLigneAffected == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("// je transmet la sync à tout le monde");
            tFile.AppendLine("if ($sCsvList[3] != -1)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tUpdate = 'UPDATE `Dev_" + tTableName + "` SET `DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\',  `'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\' WHERE `Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\'';");
            tFile.AppendLine("$tUpdateResult = "+NWD.K_SQL_CON+"->query($tUpdate);");
            tFile.AppendLine("}");
            tFile.AppendLine("if ($sCsvList[4] != -1)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tUpdate = 'UPDATE `Preprod_" + tTableName + "` SET `DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\',  `'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\' WHERE `Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\'';");
            tFile.AppendLine("$tUpdateResult = "+NWD.K_SQL_CON+"->query($tUpdate);");
            tFile.AppendLine("}");
            tFile.AppendLine("if ($sCsvList[5] != -1)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tUpdate = 'UPDATE `Prod_" + tTableName + "` SET `DS` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\',  `'."+NWD.K_ENV+".'Sync` = \\''."+NWD.K_PHP_TIME_SYNC+".'\\' WHERE `Reference` = \\''."+NWD.K_SQL_CON+"->real_escape_string($tReference).'\\'';");
            tFile.AppendLine("$tUpdateResult = "+NWD.K_SQL_CON+"->query($tUpdate);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            */
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx39, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("mysqli_free_result($tResult);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + PHP_FUNCTION_FLUSH_TRASH_DATAS() + " ()");
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", " + NWD.K_ENV + ";");
            tFile.AppendLine("$tQuery = 'DELETE FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE " + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().XX) + ">0';");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx40, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + PHP_FUNCTION_GET_DATA_BY_REFERENCE() + " ($sReference)");
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", $WSBUILD, " + NWD.K_ENV + ", $REF_NEEDED, $ACC_NEEDED, $uuid;");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + "," + PHP_CONSTANT_WEBSERVICE() + ";");
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("global $admin;");
            //"$tPage = $sPage*$sLimit;" );
            tFile.AppendLine("$tQuery = 'SELECT " + SLQSelect() + " FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sReference).'\\'';");
            tFile.AppendLine("if ($admin == false)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tQuery = $tQuery.' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().WebModel) + "` <= '.$WSBUILD.';';");
            tFile.AppendLine("}");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx33, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("while($tRow = $tResult->fetch_row())");
            tFile.AppendLine("{");
            tFile.AppendLine("$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");

            tFile.Append(AddonPhpGetCalculate(sEnvironment));

            tFile.AppendLine("}");
            string tSpecialAdd = string.Empty;
            foreach (PropertyInfo tProp in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (tProp.GetCustomAttributes(typeof(NWDNeedAccountAvatarAttribute), true).Length > 0)
                {
                    tSpecialAdd += NWDNeedAccountAvatarAttribute.PHPstring(tProp.Name);
                }
                if (tProp.GetCustomAttributes(typeof(NWDNeedUserAvatarAttribute), true).Length > 0)
                {
                    tSpecialAdd += NWDNeedUserAvatarAttribute.PHPstring(tProp.Name);
                }
                if (tProp.GetCustomAttributes(typeof(NWDNeedAccountNicknameAttribute), true).Length > 0)
                {
                    tSpecialAdd += NWDNeedAccountNicknameAttribute.PHPstring(tProp.Name);
                }
                if (tProp.GetCustomAttributes(typeof(NWDNeedUserNicknameAttribute), true).Length > 0)
                {
                    tSpecialAdd += NWDNeedUserNicknameAttribute.PHPstring(tProp.Name);
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
                tFile.AppendLine("$tResult->data_seek(0);while($tRow = $tResult->fetch_assoc()){" + tSpecialAdd + "}");
            }
            tFile.AppendLine("mysqli_free_result($tResult);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + PHP_FUNCTION_GET_DATAS_BY_REFERENCES() + " ($sReferences)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", $WSBUILD, " + NWD.K_ENV + ", $REF_NEEDED, $ACC_NEEDED, $uuid;");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + "," + PHP_CONSTANT_WEBSERVICE() + ";");
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("global $admin;");
            //"$tPage = $sPage*$sLimit;" );
            tFile.AppendLine("$tQuery = 'SELECT " + SLQSelect() + " FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "` IN ( \\''.implode('\\', \\'', $sReferences).'\\')';");
            tFile.AppendLine("if ($admin == false)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tQuery = $tQuery.' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().WebModel) + "` <= '.$WSBUILD.';';");
            tFile.AppendLine("}");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx33, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("while($tRow = $tResult->fetch_row())");
            tFile.AppendLine("{");
            tFile.AppendLine("$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");

            tFile.Append(AddonPhpGetCalculate(sEnvironment));
            tFile.AppendLine("}");
            if (tSpecialAdd != string.Empty)
            {
                tFile.AppendLine("$tResult->data_seek(0);while($tRow = $tResult->fetch_assoc()){" + tSpecialAdd + "}");
            }

            tFile.AppendLine("mysqli_free_result($tResult);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + PHP_FUNCTION_GET_DATAS() + " ($sTimeStamp, $sAccountReference)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", $WSBUILD, " + NWD.K_ENV + ", $REF_NEEDED, $ACC_NEEDED, $uuid;");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + "," + PHP_CONSTANT_WEBSERVICE() + ";");
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("global $admin;");
            //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            //"$tPage = $sPage*$sLimit;" );
            tFile.Append("$tQuery = 'SELECT " + SLQSelect() + " FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE ");
            //"(`'."+NWD.K_ENV+".'Sync` >= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimeStamp).'\\' OR `DS` >= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimeStamp).'\\')";
            tFile.Append("(`" + PHP_ENV_SYNC(sEnvironment) + "` >= \\''." + NWD.K_SQL_CON + "->real_escape_string($sTimeStamp).'\\')");
            // if need Account reference
            if (tAccountReference.Count == 0)
            {
            }
            else
            {
                tFile.Append("AND (" + string.Join("OR ", tAccountReference.ToArray()) + ") ");
            }
            tFile.AppendLine("';");
            tFile.AppendLine("if ($admin == false)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tQuery = $tQuery.' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().WebModel) + "` <= '.$WSBUILD.';';");
            tFile.AppendLine("}");
            // I do the result operation
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");

            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx33, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("while($tRow = $tResult->fetch_row())");
            tFile.AppendLine("{");
            tFile.AppendLine("$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");

            tFile.Append(AddonPhpGetCalculate(sEnvironment));
            tFile.AppendLine("}");
            if (tSpecialAdd != string.Empty)
            {
                tFile.AppendLine("$tResult->data_seek(0);while($tRow = $tResult->fetch_assoc()){" + tSpecialAdd + "}");
            }
            tFile.AppendLine("mysqli_free_result($tResult);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");










            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("function " + PHP_FUNCTION_ANTICHEAT_DATA() + " ($sCsv, $sTimeStamp, $sAccountReference, $sAdmin)");
            List<string> tColumnNameListCheat = new List<string>();
            List<string> tColumnValueListCheat = new List<string>();
            tColumnNameListCheat.Add("`Reference`");
            tColumnValueListCheat.Add("\\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[0]).'\\'");
            foreach (string tProperty in SLQAssemblyOrderArray())
            {
                tModify.Add("`" + tProperty + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + tIndex.ToString() + "]).'\\'");
                tColumnNameListCheat.Add("`" + tProperty + "`");
                tColumnValueListCheat.Add("\\''." + NWD.K_SQL_CON + "->real_escape_string($sCsvList[" + tIndex.ToString() + "]).'\\'");
                tIndex++;
            }
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", $WSBUILD, " + NWD.K_ENV + ", " + NWD.K_NWD_SLT_SRV + ", " + NWD.K_PHP_TIME_SYNC + ", $NWD_FLOAT_FORMAT, $ACC_NEEDED, " + NWD.K_PATH_BASE + ", $REF_NEEDED, $REP;");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ", " + PHP_CONSTANT_WEBSERVICE() + ";");
            tFile.AppendLine("global $admin, $uuid;");
            tFile.AppendLine("if (" + PHP_FUNCTION_INTERGRITY_TEST() + " ($sCsv) == true)");
            tFile.AppendLine("{");
            tFile.AppendLine("$sCsvList = " + PHP_FUNCTION_PREPARE_DATA() + "($sCsv);");
            tFile.AppendLine("if (count ($sCsvList) != " + tColumnNameListCheat.Count.ToString() + ")");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx99, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$tReference = $sCsvList[0];");
            tFile.AppendLine("$tQuery = 'SELECT `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "`, `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DM) + "` FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "` = \\''." + NWD.K_SQL_CON + "->real_escape_string($tReference).'\\';';");
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx31, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows <= 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows == 0)");
            tFile.AppendLine("{");
            tFile.AppendLine("// Trash Data ... it's not valid!");
            tFile.AppendLine(PHP_FUNCTION_INTERGRITY_REPLACE() + " ($sCsvList, " + CSV_IndexOf(NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().AC)) + ", '-1');");
            tFile.AppendLine("$tInsert = 'INSERT INTO `" + PHP_TABLENAME(sEnvironment) + "` (" + string.Join(", ", tColumnNameList.ToArray()) + ") VALUES (" + string.Join(", ", tColumnValueListCheat.ToArray()) + ");';");
            tFile.AppendLine("$tInsertResult = " + NWD.K_SQL_CON + "->query($tInsert);");
            tFile.AppendLine("if (!$tInsertResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx32, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx39, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("mysqli_free_result($tResult);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");



            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("function " + PHP_FUNCTION_GET_DATAS_BY_GAMESAVE() + " ($sTimeStamp, $sAccountReference, $sGameSaveReference)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", $WSBUILD, " + NWD.K_ENV + ", $REF_NEEDED, $ACC_NEEDED, $uuid;");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + "," + PHP_CONSTANT_WEBSERVICE() + ";");
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("global $admin;");
            //"$tPage = $sPage*$sLimit;" );
            tFile.Append("$tQuery = 'SELECT " + SLQSelect() + " FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE ");
            //"(`'."+NWD.K_ENV+".'Sync` >= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimeStamp).'\\' OR `DS` >= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimeStamp).'\\')";
            tFile.Append("(`" + PHP_ENV_SYNC(sEnvironment) + "` >= \\''." + NWD.K_SQL_CON + "->real_escape_string($sTimeStamp).'\\')");
            // if need Account reference
            if (tAccountReference.Count == 0)
            {
            }
            else
            {
                tFile.Append("AND (" + string.Join("OR ", tAccountReference.ToArray()) + ") ");
            }
            if (tGameSaveReference.Count == 0)
            {
            }
            else
            {
                tFile.Append("AND (" + string.Join("OR ", tGameSaveReference.ToArray()) + ") ");
            }
            tFile.AppendLine("';");
            tFile.AppendLine("if ($admin == false)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tQuery = $tQuery.' AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().WebModel) + "` <= '.$WSBUILD.';';");
            tFile.AppendLine("}");
            // I do the result operation
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx33, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("while($tRow = $tResult->fetch_row())");
            tFile.AppendLine("{");
            tFile.AppendLine("$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");

            tFile.Append(AddonPhpGetCalculate(sEnvironment));
            tFile.AppendLine("}");
            if (tSpecialAdd != string.Empty)
            {
                tFile.AppendLine("$tResult->data_seek(0);while($tRow = $tResult->fetch_assoc()){" + tSpecialAdd + "}");
            }
            tFile.AppendLine("mysqli_free_result($tResult);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function " + PHP_FUNCTION_GET_DATAS_BY_ACCOUNT() + " ($sTimeStamp, $sAccountReferences)");
            tFile.AppendLine("{");
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", $WSBUILD, " + NWD.K_ENV + ", $REF_NEEDED, $ACC_NEEDED, $uuid;");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ", " + PHP_CONSTANT_WEBSERVICE() + ";");
            tFile.AppendLine("global $REP;");
            //"$tPage = $sPage*$sLimit;" );
            tFile.Append("$tQuery = 'SELECT " + SLQSelect() + " FROM `" + PHP_TABLENAME(sEnvironment) + "` WHERE ");
            //"(`'."+NWD.K_ENV+".'Sync` >= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimeStamp).'\\' OR `DS` >= \\''."+NWD.K_SQL_CON+"->real_escape_string($sTimeStamp).'\\')";
            tFile.Append("(`" + PHP_ENV_SYNC(sEnvironment) + "` >= \\''." + NWD.K_SQL_CON + "->real_escape_string($sTimeStamp).'\\')");
            // if need Account reference
            if (tAccountReferences.Count == 0)
            {
            }
            else
            {
                tFile.Append("AND (" + string.Join("OR ", tAccountReferences.ToArray()) + ") ");
            }
            tFile.AppendLine(" AND `" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().WebModel) + "` <= '." + PHP_CONSTANT_WEBSERVICE() + ".';';");
            // I do the result operation
            tFile.AppendLine("$tResult = " + NWD.K_SQL_CON + "->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_ErrorSQL(sEnvironment, "$tQuery"));
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx33, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("while($tRow = $tResult->fetch_row())");
            tFile.AppendLine("{");
            tFile.AppendLine("$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");

            tFile.Append(AddonPhpGetCalculate(sEnvironment));
            tFile.AppendLine("}");
            if (tSpecialAdd != string.Empty)
            {
                tFile.AppendLine("$tResult->data_seek(0);while($tRow = $tResult->fetch_assoc()){" + tSpecialAdd + "}");
            }
            tFile.AppendLine("mysqli_free_result($tResult);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");

            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("function " + PHP_FUNCTION_SPECIAL() + " ($sTimeStamp, $sAccountReferences)");
            tFile.AppendLine("{");
            //tFile.AppendLine(NWDError.PHP_logTrace(sEnvironment));
            tFile.AppendLine("global " + NWD.K_SQL_CON + ", $WSBUILD, " + NWD.K_ENV + ", " + NWD.K_NWD_SLT_SRV + ", " + NWD.K_PHP_TIME_SYNC + ", $NWD_FLOAT_FORMAT, $ACC_NEEDED, " + NWD.K_PATH_BASE + ", $REF_NEEDED, $REP;");
            tFile.AppendLine("global " + PHP_CONSTANT_SALT_A() + ", " + PHP_CONSTANT_SALT_B() + ", " + PHP_CONSTANT_WEBSERVICE() + ";");
            tFile.AppendLine("global $admin, $uuid;");

            tFile.Append(AddonPhpSpecialCalculate(sEnvironment));

            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);
            string tFunctionsAdd = AddonPhpFunctions(sEnvironment);
            if (string.IsNullOrEmpty(tFunctionsAdd) == false)
            {
                tFile.AppendLine(tFunctionsAdd);
                tFile.AppendLine(NWD.K_CommentSeparator);
            }

            tFile.AppendLine("function " + PHP_FUNCTION_SYNCHRONIZE() + " ($sJsonDico, $sAccountReference, $sAdmin)");
            tFile.AppendLine("{");
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
            tFile.AppendLine("$sAccountReference = '%';");

            // Clean data?
            tOperation = NWDOperationSpecial.Clean;
            tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']['" + tOperation.ToString().ToLower() + "']))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
            tFile.AppendLine("{");
            tFile.AppendLine("" + PHP_FUNCTION_FLUSH_TRASH_DATAS() + " ();");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "SPECIAL : CLEAN"));
            tFile.AppendLine("}");
            tFile.AppendLine("}");

            //Special?
            tOperation = NWDOperationSpecial.Special;
            tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']['" + tOperation.ToString().ToLower() + "']))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
            tFile.AppendLine("{");
            tFile.AppendLine("" + PHP_FUNCTION_SPECIAL() + " ($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "'], $sAccountReference);");
            tFile.AppendLine(NWDError.PHP_log(sEnvironment, "SPECIAL : SPECIAL"));
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
                tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']))");
                tFile.AppendLine("{");
                tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "']))");
                tFile.AppendLine("{");
                tFile.AppendLine("foreach ($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "'] as $sCsvValue)");
                tFile.AppendLine("{");
                tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
                tFile.AppendLine("{");
                tFile.AppendLine("" + PHP_FUNCTION_ANTICHEAT_DATA() + " ($sCsvValue, $sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "'], $sAccountReference, $sAdmin);");
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                tFile.AppendLine("}");
                tFile.AppendLine("unset($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_DATA_KEY + "']);");
                tFile.AppendLine("$sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "'] = 0;");
                //tFile.AppendLine("" + PHP_FUNCTION_GET_DATAS() + " (0, $sAccountReference);");
                //tFile.AppendLine("$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "'] = "+NWD.K_PHP_TIME_SYNC+";");
                tFile.AppendLine("}");
                tFile.AppendLine("else");
                tFile.AppendLine("{");
            }
            if (tINeedAdminAccount == true)
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
            tFile.AppendLine("" + PHP_FUNCTION_UPDATE_DATA() + " ($sCsvValue, $sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "'], $sAccountReference, $sAdmin);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            if (tINeedAdminAccount == true)
            {
                tFile.AppendLine("}");
            }
            if (tINeedAdminAccount == false)
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
            tFile.AppendLine("if (isset($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "']))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (!" + NWDError.PHP_errorDetected() + "())");
            tFile.AppendLine("{");
            tFile.AppendLine("" + PHP_FUNCTION_GET_DATAS() + " ($sJsonDico['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "'], $sAccountReference);");
            tFile.AppendLine("$REP['" + ClassNamePHP + "']['" + NWD.K_WEB_ACTION_SYNC_KEY + "'] = " + NWD.K_PHP_TIME_SYNC + ";");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine(NWDError.PHP_Error(NWDError.NWDError_XXx98, ClassNamePHP));
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("?>");

            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            rReturn.Add(ClassNamePHP + "/" + NWD.K_WS_SYNCHRONISATION, tFileFormatted);
            //NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, string> CreatePHP(NWDAppEnvironment sEnvironment, bool sPrepareOrder = true)
        {
            //NWEBenchmark.Start();
            Dictionary<string, string> rReturn = new Dictionary<string, string>();
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
            //NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif