//=====================================================================================================================
//
// ideMobi copyright 2019 
// All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using SQLite4Unity3d;
using BasicToolBox;
using UnityEditor;
using System.Text;
using SQLite4Unity3d;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, string> New_CreatePHPConstant(NWDAppEnvironment sEnvironment)
        {
            Dictionary<string, string> rReturn = new Dictionary<string, string>();

            string tClassName = ClassNamePHP;
            string tTrigramme = ClassTrigramme;
            int tWebBuildUsed = LastWebBuild;

            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// CONSTANTS");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("include_once ($PATH_BASE.'/" + sEnvironment.Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FUNCTIONS_PHP + "');");
            tFile.AppendLine(NWD.K_CommentSeparator);
            // to bypass the global limitation of PHP in internal include : use function :-) 
            tFile.AppendLine("function " + tClassName + "Constants ()");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $SQL_" + tClassName + "_WebService;");
            tFile.AppendLine("$SQL_" + tClassName + "_SaltA = '" + SaltStart + "';");
            tFile.AppendLine("$SQL_" + tClassName + "_SaltB = '" + SaltEnd + "';");
            tFile.AppendLine("$SQL_" + tClassName + "_WebService = " + tWebBuildUsed + ";");
            tFile.AppendLine("}");
            tFile.AppendLine("//Run this function to install globals of theses datas!");
            tFile.AppendLine(tClassName + "Constants();");
            tFile.AppendLine(NWD.K_CommentSeparator);
            // craete constants of erro in php
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x01', 'error in request creation in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x02', 'error in request creation add primary key in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x03', 'error in request creation add autoincrement modify in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x04', 'error in sql index remove in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x05', 'error in sql index creation in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x07', 'error in sql defragment in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x08', 'error in sql drop in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x09', 'error in sql Flush in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x11', 'error in sql add columns in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x12', 'error in sql alter columns in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x31', 'error in request insert new datas before update in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x32', 'error in request select datas to update in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x33', 'error in request select updatable datas in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x38', 'error in request update datas in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x39', 'error too much datas for this reference in  " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x88', 'integrity of one datas is false, break in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x91', 'error update integrity in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x99', 'error columns number in " + tClassName + "');");
            tFile.AppendLine("errorDeclaration('" + tTrigramme + "x77', 'error update log in " + tClassName + "');");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            rReturn.Add(tClassName + "/" + NWD.K_CONSTANTS_FILE, tFileFormatted);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, string> New_CreatePHPManagement(NWDAppEnvironment sEnvironment)
        {
            Dictionary<string, string> rReturn = new Dictionary<string, string>();
            string tClassName = ClassNamePHP;
            string tTrigramme = ClassTrigramme;
            Type tType = ClassType;
            TableMapping tTableMapping = new TableMapping(tType);
            string tTableName = tTableMapping.TableName;
            //========= MANAGEMENT TABLE FUNCTIONS FILE
            StringBuilder tFile = new StringBuilder(string.Empty);
            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// TABLE MANAGEMENT");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("include_once ( $PATH_BASE.'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + tClassName + "/" + NWD.K_CONSTANTS_FILE + "');");
            tFile.AppendLine("include_once ( $PATH_BASE.'/" + sEnvironment.Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FUNCTIONS_PHP + "');");
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("function Create" + tClassName + "Table () {");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("global $SQL_CON, $ENV;");
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
            tFile.AppendLine("$tQuery = '" + tQuery + "';");
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine("error('" + tTrigramme + "x01');");
            tFile.AppendLine("}");
            tFile.AppendLine("$tQuery = 'ALTER TABLE `'.$ENV.'_" + tTableName + "` ADD PRIMARY KEY (`ID`), ADD UNIQUE KEY `ID` (`ID`);';");
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("$tQuery = 'ALTER TABLE `'.$ENV.'_" + tTableName + "` MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;';");
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("");
            tFile.AppendLine("// Alter all existing table with new columns or change type columns");
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
                    tFile.Append("$tQuery ='ALTER TABLE `'.$ENV.'_" + tTableName + "` ADD " +
                        Orm.SqlDecl(tColumn, true).Replace(" varchar ", " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL ").Replace(" float ", " double ").Replace("\"", "`") +
                        ";';");
                    tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
                    tFile.AppendLine("$tQuery ='ALTER TABLE `'.$ENV.'_" + tTableName + "` MODIFY " +
                        Orm.SqlDecl(tColumn, true).Replace(" varchar ", " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL ").Replace(" float ", " double ").Replace("\"", "`") +
                        ";';");
                    tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
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
                string[] columnNamesRought = new string[index.Columns.Count];
                if (index.Columns.Count == 1)
                {
                    columnNamesRought[0] = index.Columns[0].ColumnName;
                }
                else
                {
                    index.Columns.Sort((lhs, rhs) =>
                    {
                        return lhs.Order - rhs.Order;
                    });
                    for (int i = 0, end = index.Columns.Count; i < end; ++i)
                    {
                        columnNamesRought[i] = index.Columns[i].ColumnName;
                    }
                }

                List<string> columnNames = new List<string>(columnNamesRought);

                // Add special index
                foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    foreach (NWDAddIndexed tIndex in tProp.GetCustomAttributes(typeof(NWDAddIndexed), true))
                    {
                        if (indexName == tIndex.IndexName)
                        {
                            if (columnNames.Contains(tIndex.IndexColumn) == false)
                            {
                                columnNames.Add(tIndex.IndexColumn);
                            }
                        }
                    }
                }

                // Add account columnnames in K_BASIS_INDEX
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
                    else if (tColumnType.IsSubclassOf(typeof(BTBDataTypeEnum)))
                    {
                        columnNamesFinalList.Add("`" + tName + "`");
                    }
                    else if (tColumnType.IsSubclassOf(typeof(BTBDataTypeMask)))
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


                if (indexName == NWDTypeClass.K_BASIS_INDEX)
                {
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
                                        if (columnNames.Contains(tProp.Name) == false)
                                        {
                                            columnNamesFinalList.Add("`" + tProp.Name + "`(24)");
                                        }
                                    }
                                    if (tSubType == typeof(NWDGameSave))
                                    {
                                        if (columnNames.Contains(tProp.Name) == false)
                                        {
                                            columnNamesFinalList.Add("`" + tProp.Name + "`(24)");
                                        }
                                    }
                                }
                                else if (tTypeOfThis.GetGenericTypeDefinition() == typeof(NWDReferenceHashType<>))
                                {
                                    Type tSubType = tTypeOfThis.GetGenericArguments()[0];
                                    if (tSubType == typeof(NWDAccount))
                                    {
                                        if (columnNames.Contains(tProp.Name) == false)
                                        {
                                            columnNamesFinalList.Add("`" + tProp.Name + "`(24)");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                string[] columnNamesFinal = columnNamesFinalList.ToArray<string>();
                tFile.AppendLine("$tRemoveIndexQuery = 'DROP INDEX `" + indexName + "` ON `'.$ENV.'_" + index.TableName + "`;';");
                tFile.AppendLine("$tRemoveIndexResult = $SQL_CON->query($tRemoveIndexQuery);");
                const string sqlFormat = "CREATE {2}INDEX `{3}` ON `'.$ENV.'_{0}` ({1});";
                var sql = String.Format(sqlFormat, index.TableName, string.Join(", ", columnNamesFinal), index.Unique ? "UNIQUE " : "", indexName);
                tFile.AppendLine("$tQuery = '" + sql + "';");
                tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
                tFile.AppendLine("if (!$tResult)");
                tFile.AppendLine("{");
                if (sEnvironment.LogMode == true)
                {
                    tFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
                }
                tFile.AppendLine("error('" + tTrigramme + "x05');");
                tFile.AppendLine("}");
            }
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function Defragment" + tClassName + "Table ()");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("global $SQL_CON, $ENV;");
            tFile.AppendLine("$tQuery = 'ALTER TABLE `'.$ENV.'_" + tTableName + "` ENGINE=InnoDB;';");
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine("error('" + tTrigramme + "x07');");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function Drop" + tClassName + "Table ()");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("global $SQL_CON, $ENV;");
            tFile.AppendLine("$tQuery = 'DROP TABLE `'.$ENV.'_" + tTableName + "`;';");
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine("error('" + tTrigramme + "x08');");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function Flush" + tClassName + "Table ()");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("global $SQL_CON, $ENV;");
            tFile.AppendLine("$tQuery = 'FLUSH TABLE `'.$ENV.'_" + tTableName + "`;';");
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine("error('" + tTrigramme + "x09');");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("?>");
            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            rReturn.Add(tClassName + "/" + NWD.K_MANAGEMENT_FILE, tFileFormatted);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, string> New_CreatePHPSynchronisation(NWDAppEnvironment sEnvironment)
        {
            Dictionary<string, string> rReturn = new Dictionary<string, string>();
            string tClassName = ClassNamePHP;
            string tTableName = ClassNamePHP;
            string tTrigramme = ClassTrigramme;
            Type tType = ClassType;
            StringBuilder tFile = new StringBuilder(string.Empty);
            //========= SYNCHRONIZATION FUNCTIONS FILE
            // if need Account reference I prepare the restriction
            List<string> tAccountReference = new List<string>();
            List<string> tGameSaveReference = new List<string>();
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
                            if (tSubType == typeof(NWDGameSave))
                            {
                                tGameSaveReference.Add("`" + tProp.Name + "` LIKE \\''.$SQL_CON->real_escape_string($sGameSaveReference).'\\' ");
                                //tGameSaveReference.Add("`" + tProp.Name + "` IN (\\''.implode('\\', \\'', $sGameSaveReferences).'\\') ");
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

            string SLQIntegrityOrderToSelect = "***";
            foreach (string tPropertyName in New_SLQIntegrityOrder())
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

            tFile.AppendLine("<?php");
            tFile.AppendLine(sEnvironment.Headlines());
            tFile.AppendLine(NWD.K_CommentSeparator);
            tFile.AppendLine("// SYNCHRONIZATION FUNCTIONS");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("include_once ( $PATH_BASE.'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + tClassName + "/" + NWD.K_CONSTANTS_FILE + "');");
            tFile.AppendLine("include_once ( $PATH_BASE.'/" + sEnvironment.Environment + "/" + NWD.K_ENG + "/" + NWD.K_STATIC_FUNCTIONS_PHP + "');");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function Integrity" + tClassName + "Test ($sCsv)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;");
            tFile.AppendLine("$rReturn = true;");
            tFile.AppendLine("$sCsvList = explode('" + NWDConstants.kStandardSeparator + "',$sCsv);");
            tFile.AppendLine("$tIntegrity = array_pop($sCsvList);");
            tFile.AppendLine("unset($sCsvList[2]);//remove DS");
            tFile.AppendLine("unset($sCsvList[3]);//remove DevSync");
            tFile.AppendLine("unset($sCsvList[4]);//remove PreprodSync");
            tFile.AppendLine("unset($sCsvList[5]);//remove ProdSync");
            tFile.AppendLine("$sDataString = implode('',$sCsvList);");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('sDataString : '.$sDataString.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("$tCalculate = str_replace('" + NWDConstants.kStandardSeparator + "', '', md5($SQL_" + tClassName + "_SaltA.$sDataString.$SQL_" + tClassName + "_SaltB));");
            tFile.AppendLine("if ($tCalculate!=$tIntegrity)");
            tFile.AppendLine("{");
            tFile.AppendLine("$rReturn = false;");
            tFile.AppendLine("error('" + tTrigramme + "x88');");
            tFile.AppendLine("}");
            tFile.AppendLine("return $rReturn;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function Integrity" + tClassName + "Replace ($sCsvArray, $sIndex, $sValue)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;");
            tFile.AppendLine("$sCsvList = $sCsvArray;");
            tFile.AppendLine("$sCsvList[$sIndex] = $sValue;");
            tFile.AppendLine("$tIntegrity = array_pop($sCsvList);");
            tFile.AppendLine("unset($sCsvList[2]);//remove DS");
            tFile.AppendLine("unset($sCsvList[3]);//remove DevSync");
            tFile.AppendLine("unset($sCsvList[4]);//remove PreprodSync");
            tFile.AppendLine("unset($sCsvList[5]);//remove ProdSync");
            tFile.AppendLine("$sDataString = implode('',$sCsvList);");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('sDataString : '.$sDataString.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("$tCalculate = str_replace('|', '', md5($SQL_" + tClassName + "_SaltA.$sDataString.$SQL_" + tClassName + "_SaltB));");
            tFile.AppendLine("$sCsvArray[$sIndex] = $sValue;");
            tFile.AppendLine("array_pop($sCsvArray);");
            tFile.AppendLine("$sCsvArray[] = $tCalculate;");
            tFile.AppendLine("return $sCsvArray;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function Integrity" + tClassName + "Replaces ($sCsvArray, $sIndexesAndValues)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;");
            tFile.AppendLine("$sCsvList = $sCsvArray;");
            tFile.AppendLine("foreach(array_keys($sIndexesAndValues) as $tKey)");
            tFile.AppendLine("{");
            tFile.AppendLine("$sCsvList[$tKey] = $sIndexesAndValues[$tKey];");
            tFile.AppendLine("}");
            tFile.AppendLine("$tIntegrity = array_pop($sCsvList);");
            tFile.AppendLine("unset($sCsvList[2]);//remove DS");
            tFile.AppendLine("unset($sCsvList[3]);//remove DevSync");
            tFile.AppendLine("unset($sCsvList[4]);//remove PreprodSync");
            tFile.AppendLine("unset($sCsvList[5]);//remove ProdSync");
            tFile.AppendLine("$sDataString = implode('',$sCsvList);");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('sDataString : '.$sDataString.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("$tCalculate = str_replace('|', '', md5($SQL_" + tClassName + "_SaltA.$sDataString.$SQL_" + tClassName + "_SaltB));");
            tFile.AppendLine("foreach(array_keys($sIndexesAndValues) as $tKey)");
            tFile.AppendLine("{");
            tFile.AppendLine("$sCsvArray[$tKey] = $sIndexesAndValues[$tKey];");
            tFile.AppendLine("}");
            tFile.AppendLine("array_pop($sCsvArray);");
            tFile.AppendLine("$sCsvArray[] = $tCalculate;");
            tFile.AppendLine("return $sCsvArray;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function Prepare" + tClassName + "Data ($sCsv)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $TIME_SYNC;");
            tFile.AppendLine("$sCsvList = explode('" + NWDConstants.kStandardSeparator + "',$sCsv);");
            tFile.AppendLine("$sCsvList[2] = $TIME_SYNC;// change DS");
            tFile.AppendLine("if ($sCsvList[1]<$TIME_SYNC)");
            tFile.AppendLine("{");
            tFile.AppendLine("$sCsvList[2] = $sCsvList[1];");
            tFile.AppendLine("}");
            if (sEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                tFile.AppendLine("$sCsvList[3] = $TIME_SYNC;// change DevSync");
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                tFile.AppendLine("$sCsvList[4] = $TIME_SYNC;// change PreprodSync");
            }
            else if (sEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                tFile.AppendLine("$sCsvList[5] = $TIME_SYNC;// change ProdSync");
            }
            tFile.AppendLine("return $sCsvList;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);


            tFile.AppendLine("function Log" + tClassName + " ($sReference, $sLog)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $SQL_CON, $ENV;");
            tFile.AppendLine("$tUpdate = 'UPDATE `'.$ENV.'_" + tTableName + "` SET `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().ServerLog) + "` = CONCAT(`" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().ServerLog) + "`, \\' ; '.$SQL_CON->real_escape_string($sLog).'\\') WHERE `Reference` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';");
            tFile.AppendLine("$tUpdateResult = $SQL_CON->query($tUpdate);");
            tFile.AppendLine("if (!$tUpdateResult)");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tUpdate.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("error('" + tTrigramme + "x77');");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // SERVER Integrity generate
            tFile.AppendLine("function IntegrityServer" + tClassName + "Generate ($sRow)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $NWD_SLT_SRV;");
            tFile.Append("$sDataServerString =''");
            foreach (string tPropertyName in New_SLQIntegrityServerOrder())
            {
                tFile.Append(".$sRow['" + tPropertyName + "']");
            }
            tFile.AppendLine(";");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('sDataServerString : '.$sDataServerString.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("return str_replace('" + NWDConstants.kStandardSeparator + "', '', md5($NWD_SLT_SRV.$sDataServerString.$NWD_SLT_SRV));");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // DATA Integrity generate
            tFile.AppendLine("function Integrity" + tClassName + "Generate ($sRow)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $SQL_CON, $ENV, $NWD_SLT_SRV;");
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;");
            tFile.Append("$sDataString =''");
            foreach (string tPropertyName in New_SLQIntegrityOrder())
            {
                tFile.Append(".$sRow['" + tPropertyName + "']");
            }
            tFile.AppendLine(";");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('sDataString : '.$sDataString.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("return str_replace('" + NWDConstants.kStandardSeparator + "', '', md5($SQL_" + tClassName + "_SaltA.$sDataString.$SQL_" + tClassName + "_SaltB));");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // TODO refactor to be more easy to generate
            tFile.AppendLine("function Integrity" + tClassName + "Reevalue ($sReference)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $NWD_SLT_SRV, $TIME_SYNC, $NWD_FLOAT_FORMAT;");
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $SQL_" + tClassName + "_WebService;");
            tFile.AppendLine("$tQuery = 'SELECT " + SLQIntegrityOrderToSelect + " FROM `'.$ENV.'_" + tTableName + "` WHERE `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().Reference) + "` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';");
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("error('" + tTrigramme + "x31');");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("// I calculate the integrity and reinject the good value");
            tFile.AppendLine("$tRow = $tResult->fetch_assoc();");
            //"$tRow['WebServiceVersion'] = $WSBUILD;" );
            tFile.AppendLine("$tRow['WebModel'] = $SQL_" + tClassName + "_WebService;");
            tFile.AppendLine("$tCalculate = Integrity" + tClassName + "Generate ($tRow);");
            tFile.AppendLine("$tCalculateServer = IntegrityServer" + tClassName + "Generate ($tRow);");
            tFile.Append("$tUpdate = 'UPDATE `'.$ENV.'_" + tTableName + "` SET `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().Integrity) + "` = \\''.$SQL_CON->real_escape_string($tCalculate).'\\',");
            tFile.Append(" `ServerHash` = \\''.$SQL_CON->real_escape_string($tCalculateServer).'\\',");
            tFile.Append(" `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\' ,");
            //tSynchronizationFile.Append(" `WebModel` = \\''.$WSBUILD.'\\'" );
            tFile.AppendLine(" `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().WebModel) + "` = \\''.$SQL_" + tClassName + "_WebService.'\\'" + " WHERE `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().Reference) + "` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';");
            tFile.AppendLine("$tUpdateResult = $SQL_CON->query($tUpdate);");
            tFile.AppendLine("if (!$tUpdateResult)");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tUpdate.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("error('" + tTrigramme + "x91');");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // TODO refactor to be more easy to generate
            tFile.AppendLine("function IntegrityServer" + tClassName + "Validate ($sReference)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $SQL_CON, $ENV, $NWD_SLT_SRV;");
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;");
            tFile.AppendLine("$tQuery = 'SELECT " + New_SLQSelect() + " FROM `'.$ENV.'_" + tTableName + "` WHERE `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().Reference) + "` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';");
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("error('" + tTrigramme + "x31');");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("// I calculate the integrity and reinject the good value");
            tFile.AppendLine("$tRow = $tResult->fetch_assoc();");
            tFile.AppendLine("$tCalculateServer = IntegrityServer" + tClassName + "Generate ($tRow);");
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
            tFile.AppendLine("function IntegrityServer" + tClassName + "ValidateByRow ($sRow)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $NWD_SLT_SRV;");
            tFile.AppendLine("$tCalculateServer = IntegrityServer" + tClassName + "Generate ($sRow);");
            tFile.AppendLine("if ($tCalculateServer == $sRow['" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().ServerHash) + "'])");
            tFile.AppendLine("{");
            tFile.AppendLine("return true;");
            tFile.AppendLine("}");
            tFile.AppendLine("return false;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            // TODO refactor to be more easy to generate
            tFile.AppendLine("function Integrity" + tClassName + "Validate ($sReference)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $SQL_CON, $ENV, $NWD_SLT_SRV;");
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;");
            tFile.AppendLine("$tQuery = 'SELECT " + New_SLQSelect() + " FROM `'.$ENV.'_" + tTableName + "` WHERE `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().Reference) + "` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';");
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("error('" + tTrigramme + "x31');");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("// I calculate the integrity and reinject the good value");
            tFile.AppendLine("$tRow = $tResult->fetch_assoc();");
            tFile.AppendLine("$tCalculate =Integrity" + tClassName + "Generate ($tRow);");
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

            tFile.AppendLine("function Integrity" + tClassName + "ValidateByRow ($sRow)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $NWD_SLT_SRV;");
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB,$SQL_" + tClassName + "_WebService;");
            tFile.AppendLine("$tCalculate =Integrity" + tClassName + "Generate ($sRow);");
            tFile.AppendLine("if ($tCalculate == $sRow['" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().Integrity) + "'])");
            tFile.AppendLine("{");
            tFile.AppendLine("return true;");
            tFile.AppendLine("}");
            tFile.AppendLine("return false;");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function UpdateData" + tClassName + " ($sCsv, $sTimeStamp, $sAccountReference, $sAdmin)");
            List<string> tModify = new List<string>();
            List<string> tColumnNameList = new List<string>();
            List<string> tColumnValueList = new List<string>();
            tColumnNameList.Add("`Reference`");
            tColumnValueList.Add("\\''.$SQL_CON->real_escape_string($sCsvList[0]).'\\'");
            int tIndex = 1;
            foreach (string tProperty in New_SLQAssemblyOrderArray())
            {
                tModify.Add("`" + tProperty + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + tIndex.ToString() + "]).'\\'");
                tColumnNameList.Add("`" + tProperty + "`");
                tColumnValueList.Add("\\''.$SQL_CON->real_escape_string($sCsvList[" + tIndex.ToString() + "]).'\\'");
                tIndex++;
            }

            //MethodInfo tMethodDeclareFunctions = NWDAliasMethod.GetMethod(tType, NWDConstants.M_AddonPhpFunctions, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            //if (tMethodDeclareFunctions != null)
            //{
            //    tFile.Append((string)tMethodDeclareFunctions.Invoke(null, new object[] { sEnvironment }));
            //}
            tFile.Append(New_AddonPhpFunctions(sEnvironment));

            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $NWD_SLT_SRV, $TIME_SYNC, $NWD_FLOAT_FORMAT, $ACC_NEEDED, $PATH_BASE, $REF_NEEDED, $REP;");
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $SQL_" + tClassName + "_WebService;");
            tFile.AppendLine("global $admin, $uuid;");
            tFile.AppendLine("if (Integrity" + tClassName + "Test ($sCsv) == true)");
            tFile.AppendLine("{");
            tFile.AppendLine("$sCsvList = Prepare" + tClassName + "Data($sCsv);");
            tFile.AppendLine("if (count ($sCsvList) != " + tColumnNameList.Count.ToString() + ")");
            tFile.AppendLine("{");
            tFile.AppendLine("error('" + tTrigramme + "x99');");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("$tReference = $sCsvList[0];");
            tFile.AppendLine("// find solution for pre calculate on server");

            //MethodInfo tMethodDeclarePre = NWDAliasMethod.GetMethod(tType, NWDConstants.M_AddonPhpPreCalculate, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            //if (tMethodDeclarePre != null)
            //{
            //    tFile.Append((string)tMethodDeclarePre.Invoke(null, new object[] { sEnvironment }));
            //}
            tFile.Append(New_AddonPhpPreCalculate(sEnvironment));
            tFile.AppendLine("$tQuery = 'SELECT `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().Reference) + "`, `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().DM) + "` FROM `'.$ENV.'_" + tTableName + "` WHERE `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().Reference) + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\';';");
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("error('" + tTrigramme + "x31');");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows <= 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("if ($tResult->num_rows == 0)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tInsert = 'INSERT INTO `'.$ENV.'_" + tTableName + "` (" + string.Join(", ", tColumnNameList.ToArray()) + ") VALUES (" + string.Join(", ", tColumnValueList.ToArray()) + ");';");
            tFile.AppendLine("$tInsertResult = $SQL_CON->query($tInsert);");
            tFile.AppendLine("if (!$tInsertResult)");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tInsertResult.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("error('" + tTrigramme + "x32');");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            //"while($tRow = $tResult->fetch_row())"+
            //"{" );
            tFile.Append("$tUpdate = 'UPDATE `'.$ENV.'_" + tTableName + "` SET ");
            tFile.Append(string.Join(", ", tModify.ToArray()) + " WHERE `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().Reference) + "` = \\''.$SQL_CON->real_escape_string($tReference).'\\' ");
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
            tFile.AppendLine("$tUpdate = $tUpdate.$tUpdateRestriction.' AND `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().WebModel) + "` <= '.$WSBUILD.'';");
            tFile.AppendLine("}");
            //"else" );
            //"{" );
            //"//$tUpdate = $tUpdate.' AND `DM`<= \\''.$SQL_CON->real_escape_string($sCsvList[1]).'\\'';" );
            //"}" );
            tFile.AppendLine("$tUpdateResult = $SQL_CON->query($tUpdate);");
            tFile.AppendLine("if (!$tUpdateResult)");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tUpdate.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("error('" + tTrigramme + "x38');");
            tFile.AppendLine("}");
            //"}" );
            tFile.AppendLine("}");
            tFile.AppendLine("// Solution for post calculate on server");
            //MethodInfo tMethodDeclarePost = NWDAliasMethod.GetMethod(tType, NWDConstants.M_AddonPhpPostCalculate, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            //if (tMethodDeclarePost != null)
            //{
            //    tFile.Append((string)tMethodDeclarePost.Invoke(null, new object[] { sEnvironment }));
            //}
            tFile.Append(New_AddonPhpPostCalculate(sEnvironment));

            tFile.AppendLine("// Update is finished!");
            /*
            tFile.AppendLine("$tLigneAffected = $SQL_CON->affected_rows;");
            //"myLog('tLigneAffected = '.$tLigneAffected, __FILE__, __FUNCTION__, __LINE__);" );
            tFile.AppendLine("if ($tLigneAffected == 1)");
            tFile.AppendLine("{");
            tFile.AppendLine("// je transmet la sync à tout le monde");
            tFile.AppendLine("if ($sCsvList[3] != -1)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tUpdate = 'UPDATE `Dev_" + tTableName + "` SET `DS` = \\''.$TIME_SYNC.'\\',  `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\' WHERE `Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\'';");
            tFile.AppendLine("$tUpdateResult = $SQL_CON->query($tUpdate);");
            tFile.AppendLine("}");
            tFile.AppendLine("if ($sCsvList[4] != -1)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tUpdate = 'UPDATE `Preprod_" + tTableName + "` SET `DS` = \\''.$TIME_SYNC.'\\',  `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\' WHERE `Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\'';");
            tFile.AppendLine("$tUpdateResult = $SQL_CON->query($tUpdate);");
            tFile.AppendLine("}");
            tFile.AppendLine("if ($sCsvList[5] != -1)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tUpdate = 'UPDATE `Prod_" + tTableName + "` SET `DS` = \\''.$TIME_SYNC.'\\',  `'.$ENV.'Sync` = \\''.$TIME_SYNC.'\\' WHERE `Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\'';");
            tFile.AppendLine("$tUpdateResult = $SQL_CON->query($tUpdate);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            */
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("error('" + tTrigramme + "x39');");
            tFile.AppendLine("}");
            tFile.AppendLine("mysqli_free_result($tResult);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function FlushTrashedDatas" + tClassName + " ()");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("global $SQL_CON, $ENV;");
            tFile.AppendLine("$tQuery = 'DELETE FROM `'.$ENV.'_" + tTableName + "` WHERE " + NWDToolbox.PropertyName(() => NWDExample.FictiveData().XX) + ">0';");
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            tFile.AppendLine("error('" + tTrigramme + "x40');");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function GetDatas" + tClassName + "ByReference ($sReference)");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $REF_NEEDED, $ACC_NEEDED, $uuid;");
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB,$SQL_" + tClassName + "_WebService;");
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("global $admin;");
            //"$tPage = $sPage*$sLimit;" );
            tFile.AppendLine("$tQuery = 'SELECT " + New_SLQSelect() + " FROM `'.$ENV.'_" + tTableName + "` WHERE `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().Reference) + "` = \\''.$SQL_CON->real_escape_string($sReference).'\\'';");
            tFile.AppendLine("if ($admin == false)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tQuery = $tQuery.' AND `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().WebModel) + "` <= '.$WSBUILD.';';");
            tFile.AppendLine("}");
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("error('" + tTrigramme + "x33');");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("while($tRow = $tResult->fetch_row())");
            tFile.AppendLine("{");
            tFile.AppendLine("$REP['" + tClassName + "']['" + SynchronizeKeyData + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");

            //MethodInfo tMethodDeclareGet = NWDAliasMethod.GetMethod(tType, NWDConstants.M_AddonPhpGetCalculate, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            //if (tMethodDeclareGet != null)
            //{
            //    tFile.Append((string)tMethodDeclareGet.Invoke(null, new object[] { sEnvironment }));
            //}

            tFile.Append(New_AddonPhpGetCalculate(sEnvironment));

            tFile.AppendLine("}");
            string tSpecialAdd = string.Empty;
            foreach (PropertyInfo tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
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

            tFile.AppendLine("function GetDatas" + tClassName + "ByReferences ($sReferences)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $REF_NEEDED, $ACC_NEEDED, $uuid;");
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB,$SQL_" + tClassName + "_WebService;");
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("global $admin;");
            //"$tPage = $sPage*$sLimit;" );
            tFile.AppendLine("$tQuery = 'SELECT " + New_SLQSelect() + " FROM `'.$ENV.'_" + tTableName + "` WHERE `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().Reference) + "` IN ( \\''.implode('\\', \\'', $sReferences).'\\')';");
            tFile.AppendLine("if ($admin == false)");
            tFile.AppendLine("{");
            tFile.AppendLine("$tQuery = $tQuery.' AND `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().WebModel) + "` <= '.$WSBUILD.';';");
            tFile.AppendLine("}");
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("error('" + tTrigramme + "x33');");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("while($tRow = $tResult->fetch_row())");
            tFile.AppendLine("{");
            tFile.AppendLine("$REP['" + tClassName + "']['" + SynchronizeKeyData + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");
            //if (tMethodDeclareGet != null)
            //{
            //    tFile.Append((string)tMethodDeclareGet.Invoke(null, new object[] { sEnvironment }));
            //}
            tFile.Append(New_AddonPhpGetCalculate(sEnvironment));
            tFile.AppendLine("}");
            if (tSpecialAdd != string.Empty)
            {
                tFile.AppendLine("$tResult->data_seek(0);while($tRow = $tResult->fetch_assoc()){" + tSpecialAdd + "}");
            }

            tFile.AppendLine("mysqli_free_result($tResult);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function GetDatas" + tClassName + " ($sTimeStamp, $sAccountReference)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $REF_NEEDED, $ACC_NEEDED, $uuid;");
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB,$SQL_" + tClassName + "_WebService;");
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("global $admin;");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('GetDatas" + tClassName + " for '.$sAccountReference.' at '.$sTimeStamp.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            //"$tPage = $sPage*$sLimit;" );
            tFile.Append("$tQuery = 'SELECT " + New_SLQSelect() + " FROM `'.$ENV.'_" + tTableName + "` WHERE ");
            //"(`'.$ENV.'Sync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\' OR `DS` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\')";
            tFile.Append("(`'.$ENV.'Sync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\')");
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
            tFile.AppendLine("$tQuery = $tQuery.' AND `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().WebModel) + "` <= '.$WSBUILD.';';");
            tFile.AppendLine("}");
            // I do the result operation
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("error('" + tTrigramme + "x33');");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("while($tRow = $tResult->fetch_row())");
            tFile.AppendLine("{");
            tFile.AppendLine("$REP['" + tClassName + "']['" + SynchronizeKeyData + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");
            //if (tMethodDeclareGet != null)
            //{
            //    tFile.Append((string)tMethodDeclareGet.Invoke(null, new object[] { sEnvironment }));
            //}
            tFile.Append(New_AddonPhpGetCalculate(sEnvironment));
            tFile.AppendLine("}");
            if (tSpecialAdd != string.Empty)
            {
                tFile.AppendLine("$tResult->data_seek(0);while($tRow = $tResult->fetch_assoc()){" + tSpecialAdd + "}");
            }
            tFile.AppendLine("mysqli_free_result($tResult);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);


            tFile.AppendLine("function GetDatasByGameSave" + tClassName + " ($sTimeStamp, $sAccountReference, $sGameSaveReference)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $REF_NEEDED, $ACC_NEEDED, $uuid;");
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB,$SQL_" + tClassName + "_WebService;");
            tFile.AppendLine("global $REP;");
            tFile.AppendLine("global $admin;");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('GetDatasByGameSave" + tClassName + " for '.$sAccountReference.' in '.$sGameSaveReference.' at '.$sTimeStamp.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            //"$tPage = $sPage*$sLimit;" );
            tFile.Append("$tQuery = 'SELECT " + New_SLQSelect() + " FROM `'.$ENV.'_" + tTableName + "` WHERE ");
            //"(`'.$ENV.'Sync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\' OR `DS` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\')";
            tFile.Append("(`'.$ENV.'Sync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\')");
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
            tFile.AppendLine("$tQuery = $tQuery.' AND `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().WebModel) + "` <= '.$WSBUILD.';';");
            tFile.AppendLine("}");
            // I do the result operation
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("error('" + tTrigramme + "x33');");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("while($tRow = $tResult->fetch_row())");
            tFile.AppendLine("{");
            tFile.AppendLine("$REP['" + tClassName + "']['" + SynchronizeKeyData + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");
            //if (tMethodDeclareGet != null)
            //{
            //    tFile.Append((string)tMethodDeclareGet.Invoke(null, new object[] { sEnvironment }));
            //}
            tFile.Append(New_AddonPhpGetCalculate(sEnvironment));
            tFile.AppendLine("}");
            if (tSpecialAdd != string.Empty)
            {
                tFile.AppendLine("$tResult->data_seek(0);while($tRow = $tResult->fetch_assoc()){" + tSpecialAdd + "}");
            }
            tFile.AppendLine("mysqli_free_result($tResult);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);






            tFile.AppendLine("function GetDatas" + tClassName + "ByAccounts ($sTimeStamp, $sAccountReferences)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $REF_NEEDED, $ACC_NEEDED, $uuid;");
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $SQL_" + tClassName + "_WebService;");
            tFile.AppendLine("global $REP;");
            //"$tPage = $sPage*$sLimit;" );
            tFile.Append("$tQuery = 'SELECT " + New_SLQSelect() + " FROM `'.$ENV.'_" + tTableName + "` WHERE ");
            //"(`'.$ENV.'Sync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\' OR `DS` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\')";
            tFile.Append("(`'.$ENV.'Sync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\')");
            // if need Account reference
            if (tAccountReferences.Count == 0)
            {
            }
            else
            {
                tFile.Append("AND (" + string.Join("OR ", tAccountReferences.ToArray()) + ") ");
            }
            tFile.AppendLine(" AND `" + NWDToolbox.PropertyName(() => NWDExample.FictiveData().WebModel) + "` <= '.$SQL_" + tClassName + "_WebService.';';");
            // I do the result operation
            tFile.AppendLine("$tResult = $SQL_CON->query($tQuery);");
            tFile.AppendLine("if (!$tResult)");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("error('" + tTrigramme + "x33');");
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            tFile.AppendLine("while($tRow = $tResult->fetch_row())");
            tFile.AppendLine("{");
            tFile.AppendLine("$REP['" + tClassName + "']['" + SynchronizeKeyData + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);");
            //if (tMethodDeclareGet != null)
            //{
            //    tFile.Append((string)tMethodDeclareGet.Invoke(null, new object[] { sEnvironment }));
            //}
            tFile.Append(New_AddonPhpGetCalculate(sEnvironment));
            tFile.AppendLine("}");
            if (tSpecialAdd != string.Empty)
            {
                tFile.AppendLine("$tResult->data_seek(0);while($tRow = $tResult->fetch_assoc()){" + tSpecialAdd + "}");
            }
            tFile.AppendLine("mysqli_free_result($tResult);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("function Special" + tClassName + " ($sTimeStamp, $sAccountReferences)");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('DEBUG TRACE', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("global $SQL_CON, $WSBUILD, $ENV, $NWD_SLT_SRV, $TIME_SYNC, $NWD_FLOAT_FORMAT, $ACC_NEEDED, $PATH_BASE, $REF_NEEDED, $REP;");
            tFile.AppendLine("global $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB, $SQL_" + tClassName + "_WebService;");
            tFile.AppendLine("global $admin, $uuid;");
            //MethodInfo tMethodDeclareSpecial = NWDAliasMethod.GetMethod(tType, NWDConstants.M_AddonPhpSpecialCalculate, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            //if (tMethodDeclareSpecial != null)
            //{
            //    tFile.Append((string)tMethodDeclareSpecial.Invoke(null, new object[] { sEnvironment }));
            //}

            tFile.Append(New_AddonPhpSpecialCalculate(sEnvironment));

            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);


            tFile.AppendLine("function Synchronize" + tClassName + " ($sJsonDico, $sAccountReference, $sAdmin)");
            tFile.AppendLine("{");
            tFile.AppendLine("global $token_FirstUse, $PATH_BASE, $TIME_SYNC, $REP;");

            if (tType.GetCustomAttributes(typeof(NWDForceSecureDataAttribute), true).Length > 0)
            {
                tFile.AppendLine("respondAdd('securePost',true);");
            }

            NWDOperationSpecial tOperation = NWDOperationSpecial.None;
            tFile.AppendLine("if ($sAdmin == true)");
            tFile.AppendLine("{");
            tFile.AppendLine("$sAccountReference = '%';");

            // Clean data?
            tOperation = NWDOperationSpecial.Clean;
            tFile.AppendLine("if (isset($sJsonDico['" + tClassName + "']['" + tOperation.ToString().ToLower() + "']))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (!errorDetected())");
            tFile.AppendLine("{");
            tFile.AppendLine("FlushTrashedDatas" + tClassName + " ();");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('SPECIAL : CLEAN', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine("}");

            //Special?
            tOperation = NWDOperationSpecial.Special;
            tFile.AppendLine("if (isset($sJsonDico['" + tClassName + "']['" + tOperation.ToString().ToLower() + "']))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (!errorDetected())");
            tFile.AppendLine("{");
            tFile.AppendLine("Special" + tClassName + " ($sJsonDico['" + tClassName + "']['sync'], $sAccountReference);");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('SPECIAL : SPECIAL', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine("}");

            //Upgrade?
            tOperation = NWDOperationSpecial.Upgrade;
            tFile.AppendLine("if (isset($sJsonDico['" + tClassName + "']['" + tOperation.ToString().ToLower() + "']))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (!errorDetected())");
            tFile.AppendLine("{");
            tFile.AppendLine("unset($sJsonDico['" + tClassName + "']['data']); ");
            tFile.AppendLine("unset($sJsonDico['" + tClassName + "']['sync']); ");
            tFile.AppendLine("include_once ($PATH_BASE.'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + tClassName + "/" + NWD.K_MANAGEMENT_FILE + "');");
            tFile.AppendLine("Create" + tClassName + "Table ();");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('SPECIAL : UPGRADE OR CREATE TABLE', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine("}");

            //Optimize?
            tOperation = NWDOperationSpecial.Optimize;
            tFile.AppendLine("if (isset($sJsonDico['" + tClassName + "']['" + tOperation.ToString().ToLower() + "']))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (!errorDetected())");
            tFile.AppendLine("{");
            tFile.AppendLine("include_once ($PATH_BASE.'/" + sEnvironment.Environment + "/" + NWD.K_DB + "/" + tClassName + "/" + NWD.K_MANAGEMENT_FILE + "');");
            tFile.AppendLine("Defragment" + tClassName + "Table ();");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('SPECIAL : OPTIMIZE AND DEFRAGMENT TABLE', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine("}");

            // ENDIF ADMIN OPERATION
            tFile.AppendLine("}");
            tFile.AppendLine("if ($token_FirstUse == true)");
            tFile.AppendLine("{");

            if (tINeedAdminAccount == true)
            {
                tFile.AppendLine("if ($sAdmin == true){");
            }
            tFile.AppendLine("if (isset($sJsonDico['" + tClassName + "']))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (isset($sJsonDico['" + tClassName + "']['data']))");
            tFile.AppendLine("{");
            tFile.AppendLine("foreach ($sJsonDico['" + tClassName + "']['data'] as $sCsvValue)");
            tFile.AppendLine("{");
            tFile.AppendLine("if (!errorDetected())");
            tFile.AppendLine("{");
            tFile.AppendLine("UpdateData" + tClassName + " ($sCsvValue, $sJsonDico['" + tClassName + "']['sync'], $sAccountReference, $sAdmin);");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            if (tINeedAdminAccount == true)
            {
                tFile.AppendLine("}");
            }
            tFile.AppendLine("}");
            tFile.AppendLine("else");
            tFile.AppendLine("{");
            if (sEnvironment.LogMode == true)
            {
                tFile.AppendLine("myLog('NOT UPDATE, SPECIAL OR CLEAN ACTION ... YOU USE OLDEST TOKEN', __FILE__, __FUNCTION__, __LINE__);");
            }
            tFile.AppendLine("}");
            tFile.AppendLine("if (isset($sJsonDico['" + tClassName + "']))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (isset($sJsonDico['" + tClassName + "']['sync']))");
            tFile.AppendLine("{");
            tFile.AppendLine("if (!errorDetected())");
            tFile.AppendLine("{");
            tFile.AppendLine("GetDatas" + tClassName + " ($sJsonDico['" + tClassName + "']['sync'], $sAccountReference);");
            tFile.AppendLine("$REP['" + tClassName + "']['" + SynchronizeKeyTimestamp + "'] = $TIME_SYNC;");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine("}");
            tFile.AppendLine(NWD.K_CommentSeparator);

            tFile.AppendLine("?>");

            string tFileFormatted = NWDToolbox.CSharpFormat(tFile.ToString());
            rReturn.Add(tClassName + "/" + NWD.K_WS_SYNCHRONISATION, tFileFormatted);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_BasisCreatePHP)]
        public Dictionary<string, string> New_CreatePHP(NWDAppEnvironment sEnvironment, bool sPrepareOrder = true)
        {
            Dictionary<string, string> rReturn = new Dictionary<string, string>();
            PrefLoad();
            if (sPrepareOrder == true)
            {
                New_PrepareOrders();
            }
            foreach (KeyValuePair<string, string> tKeyValue in New_CreatePHPConstant(sEnvironment))
            {
                rReturn.Add(tKeyValue.Key, tKeyValue.Value);
            }
            foreach (KeyValuePair<string, string> tKeyValue in New_CreatePHPManagement(sEnvironment))
            {
                rReturn.Add(tKeyValue.Key, tKeyValue.Value);
            }
            foreach (KeyValuePair<string, string> tKeyValue in New_CreatePHPSynchronisation(sEnvironment))
            {
                rReturn.Add(tKeyValue.Key, tKeyValue.Value);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif