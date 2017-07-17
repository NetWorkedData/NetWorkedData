﻿/// <summary>
/// NWD app environment.
/// </summary>
/// 
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDAppEnvironment
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Creates the PHP files .
		/// </summary>
		public void CreatePHP ()
		{
			DateTime tTime = DateTime.UtcNow;
			string tDateTimeString = tTime.ToString ("yyyy-MM-dd");
			string tYearString = tTime.ToString ("yyyy");
			// Create folders
			string tServerRootFolder = "Assets/NetWorkedDataServer/Environment/" + Environment;
			string tServerDatabaseFolder = tServerRootFolder + "/Engine";

			if (AssetDatabase.IsValidFolder ("Assets/NetWorkedDataServer") == false) {
				AssetDatabase.CreateFolder ("Assets", "NetWorkedDataServer");
			}
			if (AssetDatabase.IsValidFolder ("Assets/NetWorkedDataServer/Environment") == false) {
				AssetDatabase.CreateFolder ("Assets/NetWorkedDataServer", "Environment");
			}
			if (AssetDatabase.IsValidFolder ("Assets/NetWorkedDataServer/Environment/" + Environment) == false) {
				AssetDatabase.CreateFolder ("Assets/NetWorkedDataServer/Environment", Environment);
			}
			if (AssetDatabase.IsValidFolder ("Assets/NetWorkedDataServer/Environment/" + Environment + "/Engine") == false) {
				AssetDatabase.CreateFolder ("Assets/NetWorkedDataServer/Environment/" + Environment, "Engine");
			}
			if (AssetDatabase.IsValidFolder ("Assets/NetWorkedDataServer/Environment/" + Environment + "/Engine/Database") == false) {
				AssetDatabase.CreateFolder ("Assets/NetWorkedDataServer/Environment/" + Environment + "/Engine", "Database");
			}

			//========= CONSTANTS FILE

			string tConstantsFile = "";
			tConstantsFile += "<?php\n" +
				"\t\t//NWD Autogenerate File at " + tDateTimeString + "\n" +
				"\t\t//Copyright NetWorkedDatas ideMobi " + tYearString + "\n" +
				"\t\t//Created by Jean-François CONTART\n" +
				"\t\t//-------------------- \n" +
				"\t\t// CONSTANTS \n" +
				"\t\t//-------------------- \n";
			if (NWDAppConfiguration.SharedInstance.DevEnvironment == this) {
				tConstantsFile += "\terror_reporting(E_ALL);\n" +
					"\tini_set('display_errors', 1);\n" +
					"\t$NWD_LOG = true;\n";
			} else {
				tConstantsFile += "\t$LOG = false;\n";
			}
			tConstantsFile += "\t$TimeIn = microtime();\n" +
				"\t\t//-------------------- \n" +
				"\t\t// CONSTANT FOR SHA512\n" +
				"\t$NWD_SHA_SEC = '" + DataSHAPassword.Replace ("'", "\'") + "';\n" +
				"\t$NWD_SHA_VEC = '" + DataSHAVector.Replace ("'", "'") + "';\n" +
				"\t$NWD_SLT_STR = '" + SaltStart.Replace ("'", "\'") + "';\n" +
				"\t$NWD_SLT_END = '" + SaltEnd.Replace ("'", "\'") + "';\n" +
				"\t\t//-------------------- \n" +
				"\t\t// CONSTANT FOR TEMPORAL SALT\n" +
				"\t$NWD_SLT_TMP = " + SaltFrequency.ToString () + ";\n" +
				"\t\t//-------------------- \n" +
				"\t\t// CONSTANT TO CONNECT TO SQL DATABASE\n" +
				"\t$SQL_HOT = '" + ServerHost.Replace ("'", "\'") + "';\n" +
				"\t$SQL_USR = '" + ServerUser.Replace ("'", "\'") + "';\n" +
				"\t$SQL_PSW = '" + ServerPassword.Replace ("'", "\'") + "';\n" +
				"\t$SQL_BSE = '" + ServerBase.Replace ("'", "\'") + "';\n" +
				"\t\t//connexion to mysql socket\n" +
				"\t$SQL_CON = '';\n" +
				"\t$SQL_CONDB = '';\n" +
				"\t$SQL_MNG = false;\n" +
				"\t\t//-------------------- \n" +
				"\t\t// ADMIN SECRET KEY\n" +
				"\t$NWD_ADM_KEY = '" + AdminKey.Replace ("'", "\'") + "';\n" +
				"\t\t//-------------------- \n" +
				"\t\t// RESCUE EMAIL\n" +
				"\t$NWD_RES_MAIL = '" + RescueEmail + "';\n" +
				"\t$NWD_APP_NAM = '" + AppName.Replace ("'", "\'") + "';\n" +
				"\t\t//-------------------- \n" +
				"\t\t// SOCIALS APP KEY AND SECRET KEY\n" +
				"\t\t// -- facebook\n" +
				"\t$NWD_FCB_AID = '" + FacebookAppID.Replace ("'", "\'") + "'; // for " + Environment + "\n" +
				"\t$NWD_FCB_SRT = '" + FacebookAppSecret.Replace ("'", "\'") + "'; // for " + Environment + "\n" +
				"\t\t// -- google\n" +
				"\t$NWD_GGO_AID = '" + GoogleAppKey.Replace ("'", "\'") + "';\n" +
				"\t\t//-------------------- \n" +
				"\t$ENV = '" + Environment + "';\n" +
				"\t\t//-------------------- \n" +
				"\t?>\n";
			File.WriteAllText (tServerDatabaseFolder + "/constants.php", tConstantsFile);
			AssetDatabase.ImportAsset (tServerDatabaseFolder + "/constants.php");

			//========= MANAGEMENT TABLE FUNCTIONS FILE

			string tManagementFile = "";
			tManagementFile +=	"<?php\n" +
				"//NWD Autogenerate File at " + tDateTimeString + "\n" +
				"//Copyright NetWorkedDatas ideMobi " + tYearString + "\n" +
				"//Created by Jean-François CONTART\n" +
				"//--------------------\n" +
				"// MANAGEMENT\n" +
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
			foreach (Type tType in NWDDataManager.SharedInstance.mTypeList) {
				var tMethodInfo = tType.GetMethod ("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) {
					string tClassName = tMethodInfo.Invoke (null, null) as string;
					tManagementFile += "include_once ($PATH_BASE.'/Environment/" + Environment + "/Engine/Database/" + tClassName + "/management.php');\n";
				}
			}
			tManagementFile += "//-------------------- \n" +
				"function CreateAllTables ()\n" +
				"\t{\n" +
				"\t\tmyLog('CREATE ALL TABALES ON SERVER', __FILE__, __FUNCTION__, __LINE__);\n";
			foreach (Type tType in NWDDataManager.SharedInstance.mTypeList) {
				var tMethodInfo = tType.GetMethod ("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) {
					string tClassName = tMethodInfo.Invoke (null, null) as string;
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
			File.WriteAllText (tServerRootFolder + "/management.php", tManagementFile);
			AssetDatabase.ImportAsset (tServerRootFolder + "/management.php");

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
				"if (!errorDetected())\n" +
				"{\n";
			tWebServices += "\n" +
				"$tPage = 0;\n" +
				"if (isset($dico['page'])) { $tPage = $dico['page'];};\n" +
				"$tLimit = 1000;\n" +
				"if (isset($dico['limit'])) { $tLimit = $dico['limit'];};\n" +
				"$tDate = time()-36000000; // check just one hour by default\n" +
				"$tDate = 0; // check just one hour by default\n" +
				"if (isset($dico['date'])) { $tDate = $dico['date'];};\n";


			// I need include ALL tables management files to manage ALL tables
			foreach (Type tType in NWDDataManager.SharedInstance.mTypeSynchronizedList) {
				var tMethodInfo = tType.GetMethod ("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) {
					string tClassName = tMethodInfo.Invoke (null, null) as string;
					tWebServices += "if (isset($dico['"+tClassName+"'])){\n";
					tWebServices += "include_once ( $PATH_BASE.'/Environment/" + Environment + "/Engine/Database/" + tClassName + "/synchronization.php');\n";
					tWebServices += "Synchronize" + tClassName + " ($dico, $tDate, $uuid, $admin, $tPage, $tLimit);\n";
					tWebServices += "}\n";
				}
			}
			// I need to prevent Non synchronized class from editor
			if (this == NWDAppConfiguration.SharedInstance.DevEnvironment || this == NWDAppConfiguration.SharedInstance.PreprodEnvironment) {

				tWebServices += "if ($admin == true)\n{\n";
				foreach (Type tType in NWDDataManager.SharedInstance.mTypeUnSynchronizedList) {
					var tMethodInfo = tType.GetMethod ("ClassNamePHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
					if (tMethodInfo != null) {
						string tClassName = tMethodInfo.Invoke (null, null) as string;
						tWebServices += "if (isset($dico['"+tClassName+"'])){\n";
						tWebServices += "include_once ( $PATH_BASE.'/Environment/" + Environment + "/Engine/Database/" + tClassName + "/synchronization.php');\n";
						tWebServices += "Synchronize" + tClassName + " ($dico, $tDate, $uuid, $admin, $tPage, $tLimit);\n";
						tWebServices += "}\n";
					}
				}
				tWebServices += "}\n";
			}
			tWebServices += "//--------------------\n" +
				"// script is finished\n" +
				"}\n" +
				"// finish the generic process\n" +
				"include_once ($PATH_BASE.'/Engine/finish.php');\n" +
				"//-------------------- \n" +
				"?>\n";
			File.WriteAllText (tServerRootFolder + "/webservices.php", tWebServices);
			AssetDatabase.ImportAsset (tServerRootFolder + "/webservices.php");
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif