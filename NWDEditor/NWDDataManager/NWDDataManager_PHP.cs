//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDDataManager
	{
		//-------------------------------------------------------------------------------------------------------------
		public void CreatePHPAllClass ()
		{
			string tProgressBarTitle = "NetWorkedData Create all php files";
			float tCountClass = mTypeList.Count + 1;
			float tOperation = 1;
			EditorUtility.DisplayProgressBar (tProgressBarTitle, "Create general error index", tOperation / tCountClass);
			tOperation++;

			NWDError.CreateGenericError ("webrequest", "WEB01", "Network", "no network", "warning");
			NWDError.CreateGenericError ("webrequest", "WEB02", "Network", "http error", "warning");
			NWDError.CreateGenericError ("webrequest", "WEB03", "Network", "http respond is empty", "warning");
			NWDError.CreateGenericError ("webrequest", "WEB04", "Network", "http respond is not valid format", "critical");

			NWDError.CreateGenericError ("sql", "UIG00", "ID", "error in unique generate", "alert");
			NWDError.CreateGenericError ("sql", "SQL00", "SQL", "error SQL CONNEXION IMPOSSIBLE", "alert");

			NWDError.CreateGenericError ("header", "HEA01", "header error", "os is empty", "alert");
			NWDError.CreateGenericError ("header", "HEA02", "header error", "version is empty", "alert");
			NWDError.CreateGenericError ("header", "HEA03", "header error", "lang is empty", "alert");
			NWDError.CreateGenericError ("header", "HEA04", "header error", "uuid is empty", "alert");
			NWDError.CreateGenericError ("header", "HEA05", "header error", "hash is empty", "alert");
			NWDError.CreateGenericError ("header", "HEA11", "header error", "os is invalid", "alert");
			NWDError.CreateGenericError ("header", "HEA12", "header error", "version is invalid", "alert");
			NWDError.CreateGenericError ("header", "HEA13", "header error", "lang is invalid", "alert");
			NWDError.CreateGenericError ("header", "HEA14", "header error", "uuid is invalid", "alert");
			NWDError.CreateGenericError ("header", "HEA15", "header error", "hash is invalid", "alert");
			NWDError.CreateGenericError ("header", "HEA90", "header error", "hash error", "alert");

			NWDError.CreateGenericError ("param", "PAR97", "param error", "not json valid", "alert");
			NWDError.CreateGenericError ("param", "PAR98", "param error", "json digest is false", "alert");
			NWDError.CreateGenericError ("param", "PAR99", "param error", "json null", "alert");

			NWDError.CreateGenericError ("gameversion", "GVA00", "version error", "error in sql select Version", "alert");
			NWDError.CreateGenericError ("gameversion", "GVA01", "version error", "stop : update app", "alert");
			NWDError.CreateGenericError ("gameversion", "GVA02", "version error", "stop unknow version : update app", "alert");
			NWDError.CreateGenericError ("gameversion", "GVA99", "version error", "block data", "alert");

			NWDError.CreateGenericError ("account", "ACC01", "Account error", "action is empty", "alert");
			NWDError.CreateGenericError ("account", "ACC02", "Account error", "action is invalid format", "alert");
			NWDError.CreateGenericError ("account", "ACC03", "Account error", "appname is empty", "alert");
			NWDError.CreateGenericError ("account", "ACC04", "Account error", "appname is invalid format", "alert");
			NWDError.CreateGenericError ("account", "ACC05", "Account error", "appmail is empty", "alert");
			NWDError.CreateGenericError ("account", "ACC06", "Account error", "appmail is invalid format", "alert");
			NWDError.CreateGenericError ("account", "ACC10", "Account error", "email is empty", "alert");
			NWDError.CreateGenericError ("account", "ACC11", "Account error", "password is empty", "alert");
			NWDError.CreateGenericError ("account", "ACC12", "Account error", "confirm password is empty", "alert");
			NWDError.CreateGenericError ("account", "ACC13", "Account error", "old password is empty", "alert");
			NWDError.CreateGenericError ("account", "ACC14", "Account error", "new password is empty", "alert");
			NWDError.CreateGenericError ("account", "ACC22", "Account error", "sign-up password is different to confirm password", "alert");
			NWDError.CreateGenericError ("account", "ACC24", "Account error", "sign-up new password is different to confirm password", "alert");
			NWDError.CreateGenericError ("account", "ACC40", "Account error", "email is invalid format", "alert");
			NWDError.CreateGenericError ("account", "ACC41", "Account error", "password is invalid format", "alert");
			NWDError.CreateGenericError ("account", "ACC42", "Account error", "confirm password is invalid format", "alert");
			NWDError.CreateGenericError ("account", "ACC43", "Account error", "old password is invalid format", "alert");
			NWDError.CreateGenericError ("account", "ACC44", "Account error", "new password is invalid format", "alert");
			NWDError.CreateGenericError ("account", "ACC55", "Account error", "email or login unknow", "alert");
			NWDError.CreateGenericError ("account", "ACC56", "Account error", "multi-account", "alert");
			NWDError.CreateGenericError ("account", "ACC71", "Account error", "GoogleID is empty", "alert");
			NWDError.CreateGenericError ("account", "ACC72", "Account error", "GoogleID is invalid format", "alert");
			NWDError.CreateGenericError ("account", "ACC73", "Account error", "Google Graph error", "alert");
			NWDError.CreateGenericError ("account", "ACC74", "Account error", "Google SDK error", "alert");
			NWDError.CreateGenericError ("account", "ACC75", "Account error", "Google sql select error", "alert");
			NWDError.CreateGenericError ("account", "ACC76", "Account error", "Google sql update error", "alert");
			NWDError.CreateGenericError ("account", "ACC77", "Account error", "Google multi-account", "alert");
			NWDError.CreateGenericError ("account", "ACC78", "Account error", "Google singin error allready log with this account", "alert");
			NWDError.CreateGenericError ("account", "ACC81", "Account error", "FacebookID is empty", "alert");
			NWDError.CreateGenericError ("account", "ACC82", "Account error", "FacebookID is invalid format", "alert");
			NWDError.CreateGenericError ("account", "ACC83", "Account error", "Facebook Graph error", "alert");
			NWDError.CreateGenericError ("account", "ACC84", "Account error", "Facebook SDK error", "alert");
			NWDError.CreateGenericError ("account", "ACC85", "Account error", "Facebook sql select error", "alert");
			NWDError.CreateGenericError ("account", "ACC86", "Account error", "Facebook sql update error", "alert");
			NWDError.CreateGenericError ("account", "ACC87", "Account error", "Facebook multi-account", "alert");
			NWDError.CreateGenericError ("account", "ACC88", "Account error", "Facebook singin error allready log with this account", "alert");
			NWDError.CreateGenericError ("account", "ACC90", "Account error", "error in request select in Account", "alert");
            NWDError.CreateGenericError ("account", "ACC91", "Account error", "error in request insert anonymous Account", "alert");
            NWDError.CreateGenericError ("account", "ACC92", "Account error", "unknow account but not temporary … it's not possible … maybe destroyed account", "alert");
			NWDError.CreateGenericError ("account", "ACC95", "Account error", "user is multiple", "alert");
			NWDError.CreateGenericError ("account", "ACC98", "Account error", "user is banned, no sign-in", "alert");
			NWDError.CreateGenericError ("account", "ACC99", "Account error", "user is banned", "alert");
			NWDError.CreateGenericError ("account", "SGN01", "Account sign error", "sign-up error in select valid account", "alert");
			NWDError.CreateGenericError ("account", "SGN02", "Account sign error", "sign-up error in select account by uuid", "alert");
			NWDError.CreateGenericError ("account", "SGN03", "Account sign error", "sign-up error in update account", "alert");
			NWDError.CreateGenericError ("account", "SGN04", "Account sign error", "sign-up error account allready linked with another email", "alert");
			NWDError.CreateGenericError ("account", "SGN05", "Account sign error", "sign-up error multi-account by uuid", "alert");
			NWDError.CreateGenericError ("account", "SGN06", "Account sign error", "sign-up error account allready linked with this email", "alert");
			NWDError.CreateGenericError ("account", "SGN07", "Account sign error", "sign-up error another account allready linked with this email", "alert");
			NWDError.CreateGenericError ("account", "SGN08", "Account sign error", "sign-up error multi-account allready linked with this email", "alert");
			NWDError.CreateGenericError ("account", "SGN09", "Account sign error", "modify error in select valid account", "alert");
			NWDError.CreateGenericError ("account", "SGN10", "Account sign error", "modify error unknow account", "alert");
			NWDError.CreateGenericError ("account", "SGN11", "Account sign error", "sign-up error in update account", "alert");
			NWDError.CreateGenericError ("account", "SGN12", "Account sign error", "modify error multi-account", "alert");
			NWDError.CreateGenericError ("account", "SGN13", "Account sign error", "modify error in select valid account", "alert");
			NWDError.CreateGenericError ("account", "SGN14", "Account sign error", "modify error email allready use in another account", "alert");
			NWDError.CreateGenericError ("account", "SGN15", "Account sign error", "singin error in request account ", "alert");
			NWDError.CreateGenericError ("account", "SGN16", "Account sign error", "singin error no account ", "alert");
			NWDError.CreateGenericError ("account", "SGN17", "Account sign error", "singin error allready log with this account", "alert");
			NWDError.CreateGenericError ("account", "SGN18", "Account sign error", "singin error multi-account ", "alert");
			NWDError.CreateGenericError ("account", "SGN19", "Account sign error", "delete error in update account", "alert");

			NWDError.CreateGenericError ("account", "SGN25", "Account sign error", "signanonymous error in request account", "alert");
			NWDError.CreateGenericError ("account", "SGN26", "Account sign error", "signanonymous error no account", "alert");
			NWDError.CreateGenericError ("account", "SGN27", "Account sign error", "signanonymous error allready log with this account", "alert");
			NWDError.CreateGenericError ("account", "SGN28", "Account sign error", "signanonymous error multi-account", "alert");

			NWDError.CreateGenericError ("account", "SGN33", "Account sign error", "signout impossible with anonymous account equal to restaured account", "alert");

			NWDError.CreateGenericError ("account", "SGN70", "Account sign error", "rescue select error", "alert");
			NWDError.CreateGenericError ("account", "SGN71", "Account sign error", "rescue unknow user", "alert");
			NWDError.CreateGenericError ("account", "SGN72", "Account sign error", "rescue multi-user", "alert");
			NWDError.CreateGenericError ("account", "SGN80", "Account sign error", "session select error", "alert");
			NWDError.CreateGenericError ("account", "SGN81", "Account sign error", "impossible unknow user", "alert");
			NWDError.CreateGenericError ("account", "SGN82", "Account sign error", "impossible multi-users", "alert");

			NWDError.CreateGenericError ("token", "RQT01", "Token error", "error in request token creation", "alert");
			NWDError.CreateGenericError ("token", "RQT11", "Token error", "new token is not in base", "alert");
			NWDError.CreateGenericError ("token", "RQT12", "Token error", "error in token select", "alert");
			NWDError.CreateGenericError ("token", "RQT13", "Token error", "error in all token delete", "alert");
			NWDError.CreateGenericError ("token", "RQT14", "Token error", "error in old token delete", "alert");

			NWDError.CreateGenericError ("token", "RQT90", "Token error", "session not exists", "alert");
			NWDError.CreateGenericError ("token", "RQT91", "Token error", "session expired", "alert");
			NWDError.CreateGenericError ("token", "RQT92", "Token error", "token not in base", "alert");
			NWDError.CreateGenericError ("token", "RQT93", "Token error", "too much tokens in base ... reconnect you", "alert");
			NWDError.CreateGenericError ("token", "RQT94", "Token error", "too much tokens in base ... reconnect you", "alert");

			int tPHPBuild = BTBConfigManager.SharedInstance ().GetInt (NWDConstants.K_NWD_WS_BUILD, 0);
			tPHPBuild++;
			BTBConfigManager.SharedInstance ().Set (NWDConstants.K_NWD_WS_BUILD, tPHPBuild);
			BTBConfigManager.SharedInstance ().Save ();
			CreateAllPHP ();
			foreach (Type tType in mTypeList) {
				EditorUtility.DisplayProgressBar (tProgressBarTitle, "Create " + tType.Name + " files", tOperation / tCountClass);
				tOperation++;
				var tMethodInfo = tType.GetMethod ("CreateAllPHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) {
					tMethodInfo.Invoke (null, null);
				}
			}
			EditorUtility.DisplayProgressBar (tProgressBarTitle, "Finish", 1.0F);
			EditorUtility.ClearProgressBar ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void CreateAllPHP ()
		{
			CopyEnginePHP ();
			foreach (NWDAppEnvironment tEnvironement in  NWDAppConfiguration.SharedInstance.AllEnvironements()) {
				tEnvironement.CreatePHP ();
			}
		}
        //-------------------------------------------------------------------------------------------------------------
        static private int kCounterExport=0;
        //-------------------------------------------------------------------------------------------------------------
		public void ExportWebSites ()
		{
			string tPath = EditorUtility.SaveFolderPanel ("Export WebSite(s)", "", "NetWorkedDataServer");
            kCounterExport++;
			if (tPath != null) {
                if (Directory.Exists (tPath + "/NetWorkedDataServer_"+kCounterExport.ToString("0000")) == false) {
                    Directory.CreateDirectory (tPath + "/NetWorkedDataServer_"+ kCounterExport.ToString("0000"));
				}
                if (Directory.Exists (tPath + "/NetWorkedDataServer_"+ kCounterExport.ToString("0000")) == true) {
                    NWDToolbox.ExportCopyFolderFiles ("Assets/NetWorkedDataServer", tPath + "/NetWorkedDataServer_"+ kCounterExport.ToString("0000"));
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void CopyEnginePHP ()
		{
			string tFolder = NWDFindPackage.SharedInstance ().ScriptFolderFromAssets + "/NWDServer";
			Debug.Log ("tFolder = " + tFolder);
			if (AssetDatabase.IsValidFolder ("Assets/NetWorkedDataServer") == false) {
				AssetDatabase.CreateFolder ("Assets", "NetWorkedDataServer");
			}
			NWDToolbox.CopyFolderFiles (tFolder, "Assets/NetWorkedDataServer");
		}
		//-------------------------------------------------------------------------------------------------------------
	
	}
}
//=====================================================================================================================
#endif