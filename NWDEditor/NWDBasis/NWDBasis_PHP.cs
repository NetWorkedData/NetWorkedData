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

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		public static string ClassNamePHP ()
		{
			Type tType = ClassType ();
			TableMapping tTableMapping = new TableMapping (tType);
			string rClassName = tTableMapping.TableName;
			return rClassName;
		}

		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public static void CreateAllPHP ()
		{
			foreach (NWDAppEnvironment tEnvironement in  NWDAppConfiguration.SharedInstance.AllEnvironements()) {
				CreatePHP (tEnvironement);
			}
//
//			Type tType = ClassType ();
//			var tMethodInfo = tType.GetMethod ("CreateSpecialAllPHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
//			if (tMethodInfo != null) {
//				tMethodInfo.Invoke (null, null);
//			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void CreatePHP (NWDAppEnvironment sEnvironment)
		{
			string tEnvironmentFolder = sEnvironment.Environment;
			Type tType = ClassType ();
			TableMapping tTableMapping = new TableMapping (tType);
			string tTableName = tEnvironmentFolder + "_" + tTableMapping.TableName;
			string tClassName = tTableMapping.TableName;
			string tTrigramme = ClassTrigramme ();
			DateTime tTime = DateTime.UtcNow;
			string tDateTimeString = tTime.ToString ("yyyy-MM-dd");
			string tYearString = tTime.ToString ("yyyy");

			PrefLoad ();

			// Create folders

			string tServerRootFolder = "Assets/NetWorkedDataServer/Environment/" + tEnvironmentFolder;
			string tServerDatabaseFolder = tServerRootFolder + "/Engine/Database/" + tClassName;

			if (AssetDatabase.IsValidFolder ("Assets/NetWorkedDataServer") == false) {
				AssetDatabase.CreateFolder ("Assets/", "NetWorkedDataServer");
				AssetDatabase.ImportAsset ("Assets/NetWorkedDataServer");
			}
			if (AssetDatabase.IsValidFolder ("Assets/NetWorkedDataServer/Environment") == false) {
				//Debug.Log ("CreatePHP error 02 Assets/NetWorkedDataServer/"+sEnvironement+" not exists ");
				AssetDatabase.CreateFolder ("Assets/NetWorkedDataServer", "Environment");
				AssetDatabase.ImportAsset ("Assets/NetWorkedDataServer/Environment");
			}
			if (AssetDatabase.IsValidFolder ("Assets/NetWorkedDataServer/Environment/" + tEnvironmentFolder) == false) {
				//Debug.Log ("CreatePHP error 02 Assets/NetWorkedDataServer/"+sEnvironement+" not exists ");
				AssetDatabase.CreateFolder ("Assets/NetWorkedDataServer", tEnvironmentFolder);
				AssetDatabase.ImportAsset ("Assets/NetWorkedDataServer/Environment/" + tEnvironmentFolder);
			}
			if (AssetDatabase.IsValidFolder ("Assets/NetWorkedDataServer/Environment/" + tEnvironmentFolder + "/Engine") == false) {
				//Debug.Log ("CreatePHP error  03 Assets/NetWorkedDataServer/"+sEnvironement+"/Engine not exists ");
				AssetDatabase.CreateFolder ("Assets/NetWorkedDataServer/Environment/" + tEnvironmentFolder, "Engine");
				AssetDatabase.ImportAsset ("Assets/NetWorkedDataServer/Environment/" + tEnvironmentFolder + "/Engine");
			}
			if (AssetDatabase.IsValidFolder ("Assets/NetWorkedDataServer/Environment/" + tEnvironmentFolder + "/Engine/Database") == false) {
				//Debug.Log ("CreatePHP error 04 Assets/NetWorkedDataServer/"+sEnvironement+"/Engine/Database not exists ");
				AssetDatabase.CreateFolder ("Assets/NetWorkedDataServer/Environment/" + tEnvironmentFolder + "/Engine", "Database");
				AssetDatabase.ImportAsset ("Assets/NetWorkedDataServer/Environment/" + tEnvironmentFolder + "/Engine/Database");
			}
			if (AssetDatabase.IsValidFolder ("Assets/NetWorkedDataServer/Environment/" + tEnvironmentFolder + "/Engine/Database/" + tClassName) == false) {
				//Debug.Log ("CreatePHP error 05 Assets/NetWorkedDataServer/"+sEnvironement+"/Engine/Database/"+ tClassName+" not exists ");
				AssetDatabase.CreateFolder ("Assets/NetWorkedDataServer/Environment/" + tEnvironmentFolder + "/Engine/Database", tClassName);
				AssetDatabase.ImportAsset ("Assets/NetWorkedDataServer/Environment/" + tEnvironmentFolder + "/Engine/Database/" + tClassName);
			}
			if (AssetDatabase.IsValidFolder (tServerDatabaseFolder) == false) {
				Debug.Log ("CreatePHP error : tServerDatabaseFolder not exists (" + tServerDatabaseFolder + ")");
			}

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
			                        "$SQL_" + tClassName + "_SaltA = '" + PrefSaltA () + "';\n" +
			                        "$SQL_" + tClassName + "_SaltB = '" + PrefSaltB () + "';\n" +
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

			NWDError.CreateGenericError (tClassName, tTrigramme + "x01", "Error in" + tClassName, "error in request creation in " + tClassName + "");
			NWDError.CreateGenericError (tClassName, tTrigramme + "x02", "Error in" + tClassName, "error in request creation add primary key in " + tClassName + "");
			NWDError.CreateGenericError (tClassName, tTrigramme + "x03", "Error in" + tClassName, "error in request creation add autoincrement modify in " + tClassName + "");
			NWDError.CreateGenericError (tClassName, tTrigramme + "x05", "Error in" + tClassName, "error in sql index creation in " + tClassName + "");
			NWDError.CreateGenericError (tClassName, tTrigramme + "x07", "Error in" + tClassName, "error in sql defragment in " + tClassName + "");
			NWDError.CreateGenericError (tClassName, tTrigramme + "x08", "Error in" + tClassName, "error in sql drop in " + tClassName + "");
			NWDError.CreateGenericError (tClassName, tTrigramme + "x09", "Error in" + tClassName, "error in sql Flush in " + tClassName + "");
			NWDError.CreateGenericError (tClassName, tTrigramme + "x11", "Error in" + tClassName, "error in sql add columns in " + tClassName + "");
			NWDError.CreateGenericError (tClassName, tTrigramme + "x12", "Error in" + tClassName, "error in sql alter columns in " + tClassName + "");
			NWDError.CreateGenericError (tClassName, tTrigramme + "x31", "Error in" + tClassName, "error in request insert new datas before update in " + tClassName + " (update table?)");
			NWDError.CreateGenericError (tClassName, tTrigramme + "x32", "Error in" + tClassName, "error in request select datas to update in " + tClassName + " (update table?)");
			NWDError.CreateGenericError (tClassName, tTrigramme + "x33", "Error in" + tClassName, "error in request select updatable datas in " + tClassName + " (update table?)");
			NWDError.CreateGenericError (tClassName, tTrigramme + "x38", "Error in" + tClassName, "error in request update datas in " + tClassName + " (update table?)");
			NWDError.CreateGenericError (tClassName, tTrigramme + "x39", "Error in" + tClassName, "error more than one row for this reference in  " + tClassName + "");
			NWDError.CreateGenericError (tClassName, tTrigramme + "x40", "Error in" + tClassName, "error in flush trashed in  " + tClassName + "");
			NWDError.CreateGenericError (tClassName, tTrigramme + "x91", "Error in" + tClassName, "error update integrity in " + tClassName + " (update table?)");
			NWDError.CreateGenericError (tClassName, tTrigramme + "x88", "Error in" + tClassName, "integrity of one datas is false, break in " + tClassName + "");

			tConstantsFile += "" +
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
			"\n" +
			"//-------------------- \n" +
			"?>\n";
			File.WriteAllText (tServerDatabaseFolder + "/constants.php", tConstantsFile);
			// force to import this file by Unity3D
			AssetDatabase.ImportAsset (tServerDatabaseFolder + "/constants.php");

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
			                         "global $SQL_CON;\n" +
			                         "";
			var tQuery = "CREATE TABLE IF NOT EXISTS `" + tTableName + "` (";
			var tDeclarations = tTableMapping.Columns.Select (p => Orm.SqlDecl (p, true));
			var tDeclarationsJoined = string.Join (",", tDeclarations.ToArray ());
			tDeclarationsJoined = tDeclarationsJoined.Replace ('"', '`');
			tDeclarationsJoined = tDeclarationsJoined.Replace ("`ID` integer", "`ID` int(11) NOT NULL");
			tDeclarationsJoined = tDeclarationsJoined.Replace ("`DC` integer", "`DC` int(11) NOT NULL DEFAULT 0");
//			tDeclarationsJoined = tDeclarationsJoined.Replace ("`AC` integer", "`AC` int(11) NOT NULL DEFAULT 1");
			tDeclarationsJoined = tDeclarationsJoined.Replace ("`DM` integer", "`DM` int(11) NOT NULL DEFAULT 0");
			tDeclarationsJoined = tDeclarationsJoined.Replace ("`DD` integer", "`DD` int(11) NOT NULL DEFAULT 0");
			tDeclarationsJoined = tDeclarationsJoined.Replace ("`DS` integer", "`DS` int(11) NOT NULL DEFAULT 0");
			tDeclarationsJoined = tDeclarationsJoined.Replace ("`XX` integer", "`XX` int(11) NOT NULL DEFAULT 0");
			tDeclarationsJoined = tDeclarationsJoined.Replace ("varchar", "text CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL");
			tDeclarationsJoined = tDeclarationsJoined.Replace ("primary key autoincrement not null", "");
			tQuery += tDeclarationsJoined;
			tQuery += ") ENGINE=InnoDB DEFAULT CHARSET=utf8 COLLATE=utf8_unicode_ci;";
			tManagementFile += "" +
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
			"$tQuery = 'ALTER TABLE `" + tTableName + "` ADD PRIMARY KEY (`ID`), ADD UNIQUE KEY `ID` (`ID`);';\n" +
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
			"$tQuery = 'ALTER TABLE `" + tTableName + "` MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;';\n" +
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
			foreach (TableMapping.Column tColumn in tTableMapping.Columns) {

				if (tColumn.Name != "ID" &&
				    tColumn.Name != "Reference" &&
				    tColumn.Name != "DM" &&
				    tColumn.Name != "DC" &&
//					tColumn.Name != "AC" &&
				    tColumn.Name != "DD" &&
				    tColumn.Name != "DS" &&
				    tColumn.Name != "XX") {
					tManagementFile +=
					"$tQuery ='ALTER TABLE `" + tTableName + "` ADD " + Orm.SqlDecl (tColumn, true).Replace ("varchar", " TEXT CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL") + ";';\n" +
					"$tResult = $SQL_CON->query($tQuery);\n" +
					//"if (!$tResult)\n" +
					//"{\n" +
						//"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
//						"error('" + tTrigramme + "x11');\n" +
					//"}\n" +
					"";
					tManagementFile +=
//					"$tQuery ='ALTER TABLE `" + tTableName + "` CHANGE `" + tColumn.Name + "` " + Orm.SqlDecl (tColumn, true).Replace ("varchar", "text CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL") + ";';\n" +
					"$tQuery ='ALTER TABLE `" + tTableName + "` MODIFY " + Orm.SqlDecl (tColumn, true).Replace ("varchar", "text CHARACTER SET utf8 COLLATE utf8_unicode_ci NOT NULL") + ";';\n" +
					"$tResult = $SQL_CON->query($tQuery);\n" +
					//"if (!$tResult)\n" +
					//"{\n" +
//					"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
//					"error('" + tTrigramme + "x12');\n" +
					//"}\n" +
					"";
				}
			}
			tManagementFile += "\n";

			var indexes = new Dictionary<string, SQLite4Unity3d.SQLiteConnection.IndexInfo> ();
			foreach (var c in tTableMapping.Columns) {
				foreach (var i in c.Indices) {
					var iname = i.Name ?? tTableName + "_" + c.Name;
					SQLite4Unity3d.SQLiteConnection.IndexInfo iinfo;
					if (!indexes.TryGetValue (iname, out iinfo)) {
						iinfo = new SQLite4Unity3d.SQLiteConnection.IndexInfo {
							IndexName = iname,
							TableName = tTableName,
							Unique = i.Unique,
							Columns = new List<SQLite4Unity3d.SQLiteConnection.IndexedColumn> ()
						};
						indexes.Add (iname, iinfo);
					}
					if (i.Unique != iinfo.Unique)
						throw new Exception ("All the columns in an index must have the same value for their Unique property");
					iinfo.Columns.Add (new SQLite4Unity3d.SQLiteConnection.IndexedColumn {
						Order = i.Order,
						ColumnName = c.Name
					});
				}
			}

			foreach (var indexName in indexes.Keys) {
				var index = indexes [indexName];
				string[] columnNames = new string[index.Columns.Count];
				if (index.Columns.Count == 1) {
					columnNames [0] = index.Columns [0].ColumnName;
				} else {
					index.Columns.Sort ((lhs, rhs) => {
						return lhs.Order - rhs.Order;
					});
					for (int i = 0, end = index.Columns.Count; i < end; ++i) {
						columnNames [i] = index.Columns [i].ColumnName;
					}
				}

				List <string> columnNamesFinalList = new List <string> ();

				foreach (string tName in columnNames) {
					PropertyInfo tColumnInfos = tType.GetProperty (tName);
					Type tColumnType = tColumnInfos.PropertyType;
					//Debug.Log ("tColumnType `"+tName+"` = `" + tColumnType.Name+"`");


					if (tColumnType.IsSubclassOf (typeof(BTBDataType))) {
						columnNamesFinalList.Add ("`" + tName + "`(24)");
					} else if (tColumnType == typeof(string)) {
						columnNamesFinalList.Add ("`" + tName + "`(24)");
					} else if (tColumnType == typeof(string)) {
						columnNamesFinalList.Add ("`" + tName + "`(32)");
					} else {
						columnNamesFinalList.Add ("`" + tName + "`");
					}
				}

				string[] columnNamesFinal = columnNamesFinalList.ToArray<string> ();
					
				tManagementFile += "" +
				"$tRemoveIndexQuery = 'DROP INDEX `" + indexName + "` ON `" + index.TableName + "`;';\n" +
				"$tRemoveIndexResult = $SQL_CON->query($tRemoveIndexQuery);\n" +
					//"if (!$tRemoveIndexResult){};\n" +
					//"{\n" +
					//"myLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tRemoveIndexQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
					//	"error('" + tTrigramme + "x04');\n" +
					//"}\n" +
				"";
				
				const string sqlFormat = "CREATE {2}INDEX `{3}` ON `{0}` ({1});";
				var sql = String.Format (sqlFormat, index.TableName, string.Join (", ", columnNamesFinal), index.Unique ? "UNIQUE " : "", indexName);
//				sql = sql.Replace ("`Reference`", "`Reference`(32)");
				tManagementFile += "" +
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
				"";

			}

			tManagementFile += "}\n" +
			"//-------------------- \n" +
			"function Defragment" + tClassName + "Table ()\n" +
			"{\n" +
			"global $SQL_CON;\n" +
			"$tQuery = 'ALTER TABLE `" + tTableName + "` ENGINE=InnoDB;';\n" +
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
			"global $SQL_CON;\n" +
			"$tQuery = 'DROP TABLE `" + tTableName + "`;';\n" +
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
			"global $SQL_CON;\n" +
			"$tQuery = 'FLUSH TABLE `" + tTableName + "`;';\n" +
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
			File.WriteAllText (tServerDatabaseFolder + "/management.php", tManagementFile);
			// force to import this file by Unity3D
			AssetDatabase.ImportAsset (tServerDatabaseFolder + "/management.php");

			//========= SYNCHRONIZATION FUNCTIONS FILE

			// if need Account reference I prepare the restriction
			List<string> tAccountReference = new List<string> ();
			foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
				Type tTypeOfThis = tProp.PropertyType;
				if (tTypeOfThis != null) {
					if (tTypeOfThis.IsGenericType) {
						
						if (tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferenceType<>)) {
							Type tSubType = tTypeOfThis.GetGenericArguments () [0];
							if (tSubType == typeof(NWDAccount)) {
								tAccountReference.Add ("`" + tProp.Name + "` LIKE \\''.$SQL_CON->real_escape_string($sAccountReference).'\\' ");
							}
						} else if (tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferenceHashType<>)) {
							Type tSubType = tTypeOfThis.GetGenericArguments () [0];
							if (tSubType == typeof(NWDAccount)) {
								tAccountReference.Add ("`" + tProp.Name + "` LIKE \\''.$SQL_CON->real_escape_string(md5($sAccountReference.$SQL_" + tClassName + "_SaltA)).'\\' ");
							}
						}
					}
				}
			}
			bool tINeedAdminAccount = true;
			if (tAccountReference.Count == 0) {
			} else {
				tINeedAdminAccount = false;
			}

			string tSynchronizationFile = "";
			tSynchronizationFile += "" +
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
			"function Prepare" + tClassName + "Data ($sCsv)\n" +
			"\t{\n" +
			"\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;\n" +
			"\t\t$sCsvList = explode('" + NWDConstants.kStandardSeparator + "',$sCsv);\n" +
			"\t\t$sCsvList[2] = time();// change DS\n";
			if (sEnvironment == NWDAppConfiguration.SharedInstance.DevEnvironment) {
				tSynchronizationFile += "\t\t$sCsvList[3] = time();// change DevSync\n";
			} else if (sEnvironment == NWDAppConfiguration.SharedInstance.PreprodEnvironment) {
				tSynchronizationFile += "\t\t$sCsvList[4] = time();// change PreprodSync\n";
			} else if (sEnvironment == NWDAppConfiguration.SharedInstance.ProdEnvironment) {
				tSynchronizationFile += "\t\t$sCsvList[5] = time();// change ProdSync\n";
			}
			tSynchronizationFile += "" +
//			"\t\t$tIntegrity = array_pop($sCsvList);\n" +
//			"\t\t$sDataString = implode('',$sCsvList);\n" +
//			"\t\t$tIntegrity = str_replace('" + NWDConstants.kStandardSeparator + "', '', md5($SQL_" + tClassName + "_SaltA.$sDataString.$SQL_" + tClassName + "_SaltB));\n" +
//			"\t\t$sCsvList[] = $tIntegrity;\n" +
			"\t\treturn $sCsvList;\n" +
			"\t}\n" +
			"//-------------------- \n" +
			"function Integrity" + tClassName + "Reevalue ($sReference)\n" +
			"\t{\n" +
			"\t\tglobal $SQL_CON;\n" +
			"\t\tglobal $SQL_NWDAccount_SaltA, $SQL_NWDAccount_SaltB;\n" +
			"\t\t$tQuery = 'SELECT * FROM `" + tTableName + "` WHERE `Reference` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';\n" +
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
			"\t\t\t\t\t\t$sDataString ='';\n";
			foreach (string tPropertyName in SLQIntegrityOrder ()) {
				tSynchronizationFile += "" +
				"\t\t\t\t\t\t$sDataString .= $tRow['" + tPropertyName + "'];\n";
			}
			tSynchronizationFile += "" +
			"\t\t\t\t\t\t$tCalculate = str_replace('" + NWDConstants.kStandardSeparator + "', '', md5($SQL_" + tClassName + "_SaltA.$sDataString.$SQL_" + tClassName + "_SaltB));\n" +
			"\t\t\t\t\t\t$tUpdate = 'UPDATE `" + tTableName + "` SET `Integrity` = \\''.$SQL_CON->real_escape_string($tCalculate).'\\' WHERE `Reference` = \\''.$SQL_CON->real_escape_string($sReference).'\\';';\n" +
			"\t\t\t\t\t\t$tUpdateResult = $SQL_CON->query($tUpdate);\n" +
			"\t\t\t\t\t\tif (!$tUpdateResult)\n" +
			"\t\t\t\t\t\t\t{\n" +
			"\t\t\t\t\t\t\t\terror('" + tTrigramme + "x91');\n" +
			"\t\t\t\t\t\t\t\tmyLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tUpdate.'', __FILE__, __FUNCTION__, __LINE__);\n" +
			"\t\t\t\t\t\t\t}\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t}\n" +
			"\t}\n" +
			"//-------------------- \n";


			List <string> tModify = new List<string> ();
			List <string> tColumnNameList = new List<string> ();
			List <string> tColumnValueList = new List<string> ();

			tColumnNameList.Add ("`Reference`");
			tColumnValueList.Add ("\\''.$SQL_CON->real_escape_string($sCsvList[0]).'\\'");

			int tIndex = 1;
			foreach (string tProperty in SLQAssemblyOrderArray ()) {
				tModify.Add ("`" + tProperty + "` = \\''.$SQL_CON->real_escape_string($sCsvList[" + tIndex.ToString () + "]).'\\'");
				tColumnNameList.Add ("`" + tProperty + "`");
				tColumnValueList.Add ("\\''.$SQL_CON->real_escape_string($sCsvList[" + tIndex.ToString () + "]).'\\'");
				tIndex++;
			}
			;

			tSynchronizationFile += "function UpdateData" + tClassName + " ($sCsv, $sTimeStamp, $sAccountReference, $sAdmin)\n" +
			"\t{\n" +
				//"" + tGlobal +
			"\t\tglobal $SQL_CON;\n" +
			"\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;\n" +
			"\t\tglobal $admin;\n" +
			"\t\tif (Integrity" + tClassName + "Test ($sCsv) == true)\n" +
			"\t\t\t{\n" +
			//"$sCsvList = explode('"+NWDConstants.kStandardSeparator+"',$sCsv);\n" +
			"\t\t\t\t$sCsvList = Prepare" + tClassName + "Data($sCsv);\n" +
			"\t\t\t\tif (count ($sCsvList) != "+tColumnNameList.Count.ToString()+")\n" +
			"\t\t\t\t{\n" +
			"\t\t\t\t\t\terror('" + tTrigramme + "x99');\n" +
			"\t\t\t\t}\n" +
			"\t\t\t\telse\n" +
			"\t\t\t\t{\n" +
			"\t\t\t\t$tReference = $sCsvList[0];\n" +
			"\t\t\t\t$tDM = $sCsvList[1];\n" +
			"\t\t\t\t$tQuery = 'SELECT `Reference` FROM `" + tTableName + "` WHERE `Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\';';\n" +
			"\t\t\t\t$tResult = $SQL_CON->query($tQuery);\n" +
			"\t\t\t\tif (!$tResult)\n" +
			"\t\t\t\t\t{\n" +
			"\t\t\t\t\t\terror('" + tTrigramme + "x31');\n" +
			"\t\t\t\t\t\tmyLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tQuery.'', __FILE__, __FUNCTION__, __LINE__);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\telse\n" +
			"\t\t\t\t\t{\n" +
			"\t\t\t\t\t\tif ($tResult->num_rows <= 1)\n" +
			"\t\t\t\t\t\t\t{\n" +
			"\t\t\t\t\t\t\t\tif ($tResult->num_rows == 0)\n" +
			"\t\t\t\t\t\t\t\t\t{\n" +
			"\t\t\t\t\t\t\t\t\t\t$tInsert = 'INSERT INTO `" + tTableName + "` (" + string.Join (", ", tColumnNameList.ToArray ()) + ") VALUES (" + string.Join (", ", tColumnValueList.ToArray ()) + ");';\n" +
			"\t\t\t\t\t\t\t\t\t\t$tInsertResult = $SQL_CON->query($tInsert);\n" +
			"\t\t\t\t\t\t\t\t\t\tif (!$tInsertResult)\n" +
			"\t\t\t\t\t\t\t\t\t\t\t{\n" +
			"\t\t\t\t\t\t\t\t\t\t\t\terror('" + tTrigramme + "x32');\n" +
			"\t\t\t\t\t\t\t\t\t\t\t\tmyLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tInsertResult.'', __FILE__, __FUNCTION__, __LINE__);\n" +
			"\t\t\t\t\t\t\t\t\t\t\t}\n" +
			"\t\t\t\t\t\t\t\t\t}\n" +
			"\t\t\t\t\t\t\t\telse\n" +
			"\t\t\t\t\t\t\t\t\t{\n" +
			"\t\t\t\t\t\t\t\t\t\t$tUpdate = 'UPDATE `" + tTableName + "` SET ";
			tSynchronizationFile += string.Join (", ", tModify.ToArray ()) + " " +
			"WHERE `Reference` = \\''.$SQL_CON->real_escape_string($tReference).'\\' " +
			"AND `DM` < \\''.$SQL_CON->real_escape_string($tDM).'\\' ';\n";
			if (tAccountReference.Count == 0) {
				tSynchronizationFile += "$tUpdateRestriction = '';\n";
			} else {
				tSynchronizationFile += "$tUpdateRestriction = 'AND (" + string.Join (" OR ", tAccountReference.ToArray ()) + ") ';\n";
			}
			tSynchronizationFile += "" +
			"\t\t\t\t\t\t\t\t\t\tif ($admin == false)\n" +
			"\t\t\t\t\t\t\t\t\t\t\t{\n" +
			"\t\t\t\t\t\t\t\t\t\t\t\t$tUpdate = $tUpdate.$tUpdateRestriction;\n" +
			"\t\t\t\t\t\t\t\t\t\t\t}\n" +
			"\t\t\t\t\t\t\t\t\t\t$tUpdateResult = $SQL_CON->query($tUpdate);\n" +
			"\t\t\t\t\t\t\t\t\t\tif (!$tUpdateResult)\n" +
			"\t\t\t\t\t\t\t\t\t\t\t{\n " +
			"\t\t\t\t\t\t\t\t\t\t\t\terror('" + tTrigramme + "x38');\n" +
			"\t\t\t\t\t\t\t\t\t\t\t\tmyLog('error in mysqli request : ('. $SQL_CON->errno.')'. $SQL_CON->error.'  in : '.$tUpdate.'', __FILE__, __FUNCTION__, __LINE__);\n" +
			"\t\t\t\t\t\t\t\t\t\t\t}\n" +
			"\t\t\t\t\t\t\t\t\t}\n" +
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
			"\t\tglobal $SQL_CON;\n" +
			"\t\t$tQuery = 'DELETE FROM `" + tTableName + "` WHERE XX>0';\n" +
			"\t\t$tResult = $SQL_CON->query($tQuery);\n" +
			"\t\tif (!$tResult)\n" +
			"\t\t\t{\n" +
			"\t\t\t\terror('" + tTrigramme + "x40');\n" +
			"\t\t\t}\n" +
			"}" +
			"//-------------------- \n" +
			"function GetDatas" + tClassName + " ($sTimeStamp, $sAccountReference, $sPage, $sLimit)\n" +
			"\t{\n" +
			"\t\tglobal $SQL_CON;\n" +
			"\t\tglobal $SQL_" + tClassName + "_SaltA, $SQL_" + tClassName + "_SaltB;\n" +
			"\t\tglobal $REP;\n" +
			"\t\t$tPage = $sPage*$sLimit;\n" +
			"\t\t$tQuery = 'SELECT " + SLQAssemblyOrder () + " FROM `" + tTableName + "` WHERE ";
			if (sEnvironment == NWDAppConfiguration.SharedInstance.DevEnvironment) {
				tSynchronizationFile += "`DevSync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\' ";
			} else if (sEnvironment == NWDAppConfiguration.SharedInstance.PreprodEnvironment) {
				tSynchronizationFile += "`PreprodSync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\' ";
			} else if (sEnvironment == NWDAppConfiguration.SharedInstance.ProdEnvironment) {
				tSynchronizationFile += "`ProdSync` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\' ";
			} else {
				tSynchronizationFile += "`DM` >= \\''.$SQL_CON->real_escape_string($sTimeStamp).'\\' ";
			}
			// if need Account reference
			if (tAccountReference.Count == 0) {
			} else {
				tSynchronizationFile += "AND (" + string.Join ("OR ", tAccountReference.ToArray ()) + ") ";
			}
			//tSynchronizationFile += "LIMIT '.$tPage.','.$sLimit.' ";
			tSynchronizationFile += ";';\n";
			// I do the result operation
			tSynchronizationFile += "\t\t$tResult = $SQL_CON->query($tQuery);\n" +
			"\t\tif (!$tResult)\n" +
			"\t\t\t{\n" +
			"\t\t\t\terror('" + tTrigramme + "x33');" +
			"\t\t\t}\n" +
			"\t\telse\n" +
			"\t\t\t{\n" +
			"\t\t\t\twhile($tRow = $tResult->fetch_row())\n" +
			"\t\t\t\t\t{\n" +
			"\t\t\t\t\t\t$REP['" + tClassName + "'][] = implode('" + NWDConstants.kStandardSeparator + "',$tRow);\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\tmysqli_free_result($tResult);\n" +
			"\t\t\t}\n" +
			"\t}\n" +
			"//-------------------- \n" +
			"function Synchronize" + tClassName + " ($sJsonDico, $sTimeStamp, $sAccountReference, $sAdmin, $sPage, $sLimit) " +
			"\t{\n";
			if (tINeedAdminAccount == true) {
				tSynchronizationFile += "\t\tif ($sAdmin == true){\n";
			}
			tSynchronizationFile += "" +
			"\t\tif ($sAdmin == true)\n" +
			"\t\t\t{\n" +
			"\t\t\t\t$sAccountReference = '%';\n" +
			"\t\t\t}\n" +
			"\t\tif (isset($sJsonDico['" + tClassName + "']))\n" +
			"\t\t\t{\n" +
			"\t\t\t\tif (isset($sJsonDico['" + tClassName + "']['data']))\n" +
			"\t\t\t\t\t{\n" +
			"\t\t\t\t\t\tforeach ($sJsonDico['" + tClassName + "']['data'] as $sCsvValue)\n" +
			"\t\t\t\t\t\t\t{\n" +
			"\t\t\t\t\t\t\t\tif (!errorDetected())\n" +
			"\t\t\t\t\t\t\t\t\t{\n" +
			"\t\t\t\t\t\t\t\t\t\tUpdateData" + tClassName + " ($sCsvValue, $sTimeStamp, $sAccountReference, $sAdmin);\n" +
			"\t\t\t\t\t\t\t\t\t}\n" +
			"\t\t\t\t\t\t\t}\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t}\n";
			if (tINeedAdminAccount == true) {
				tSynchronizationFile += "\t\t}\n";
			}
			tSynchronizationFile += "\t\tif (isset($sJsonDico['" + tClassName + "']))\n" +
			"\t\t\t{\n" +
			"\t\t\t\tif (isset($sJsonDico['" + tClassName + "']['clean']))\n" +
			"\t\t\t\t\t{\n" +
			"\t\t\t\t\t\tif (!errorDetected())\n" +
			"\t\t\t\t\t\t\t{\n" +
			"\t\t\t\t\t\t\t\tFlushTrashedDatas" + tClassName + " ();\n" +
			"\t\t\t\t\t\t\t}\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t\tif (isset($sJsonDico['" + tClassName + "']['sync']))\n" +
			"\t\t\t\t\t{\n" +
			"\t\t\t\t\t\tif (!errorDetected())\n" +
			"\t\t\t\t\t\t\t{\n" +
			"\t\t\t\t\t\t\t\tGetDatas" + tClassName + " ($sJsonDico['" + tClassName + "']['sync'], $sAccountReference, $sPage, $sLimit);\n" +
			"\t\t\t\t\t\t\t}\n" +
			"\t\t\t\t\t}\n" +
			"\t\t\t}\n" +
			"\t}\n" +
			"//-------------------- \n" +
			"?>";
			File.WriteAllText (tServerDatabaseFolder + "/synchronization.php", tSynchronizationFile);
			// force to import this file by Unity3D
			AssetDatabase.ImportAsset (tServerDatabaseFolder + "/synchronization.php");

			//========= WEBSERVICE FILE
			/*
			string tWebServices = "" +
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
			"if (!errorDetected())\n" +
			"\t{\n" +
			"\t\tinclude_once ($PATH_BASE.'/Environment/" + sEnvironment.Environment + "/Engine/Database/" + tClassName + "/synchronization.php');\n" +
			"\t\t$tPage = 0; if (isset($dico['page'])) { $tPage = $dico['page'];};\n" +
			"\t\t$tLimit = 1000; if (isset($dico['limit'])) { $tLimit = $dico['limit'];};\n" +
			"\t\t$tDate = time()-3600; if (isset($dico['date'])) { $tDate = $dico['date'];};\n" +
			"\t\t// start synchronize\n" +
			"\t\tSynchronize" + tClassName + " ($dico, $tDate, $uuid, $admin, $tPage, $tLimit);\n" +
			"\t}\n" +
			"//--------------------\n" +
			"// script is finished\n" +
			"// finish the generic process\n" +
			"include_once ($PATH_BASE.'/Engine/finish.php');\n" +
			"\n" +
			"?>";
			File.WriteAllText (tServerRootFolder + "/" + tClassName.ToLower () + ".php", tWebServices);
			// force to import this file by Unity3D
			AssetDatabase.ImportAsset (tServerRootFolder + "/" + tClassName.ToLower () + ".php");
			*/
			// try to create special file for the special operation in PHP

		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================