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
        public void CreatePHPAllClass()
        {

            //int tPHPBuild = BTBConfigManager.SharedInstance().GetInt(NWDConstants.K_NWD_WS_BUILD, 0);
            //tPHPBuild++;
            //BTBConfigManager.SharedInstance().Set(NWDConstants.K_NWD_WS_BUILD, tPHPBuild);
            //BTBConfigManager.SharedInstance().Save();

            //NWDAppConfiguration.SharedInstance().WebBuild = tPHPBuild;
            NWDAppConfiguration.SharedInstance().WebBuild++;
            NWDAppConfiguration.SharedInstance().WSList.Add(NWDAppConfiguration.SharedInstance().WebBuild, true);
            //TODO RECALCULATE THE NEW ORDER FOR THE WEBSERVICE
            //TODO Class by class re-random the order of property for each class for webservice
            //TODO memorize in Table by webbuild the new order

            //TODO reccord the new Configuration;

            string tProgressBarTitle = "NetWorkedData Create all php files";
            float tCountClass = mTypeList.Count + 1;
            float tOperation = 1;
            EditorUtility.DisplayProgressBar(tProgressBarTitle, "Create general error index", tOperation / tCountClass);
            tOperation++;

            NWDError.CreateGenericError("webrequest", "WEB01", "Network", "no network", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("webrequest", "WEB02", "Network", "http error", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("webrequest", "WEB03", "Network", "http respond is empty", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("webrequest", "WEB04", "Network", "http respond is not valid format", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError("sql", "UIG00", "ID", "error in unique generate", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("sql", "SQL00", "SQL", "error SQL CONNEXION IMPOSSIBLE", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError("header", "HEA01", "header error", "os is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("header", "HEA02", "header error", "version is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("header", "HEA03", "header error", "lang is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("header", "HEA04", "header error", "uuid is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("header", "HEA05", "header error", "hash is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("header", "HEA11", "header error", "os is invalid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("header", "HEA12", "header error", "version is invalid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("header", "HEA13", "header error", "lang is invalid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("header", "HEA14", "header error", "uuid is invalid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("header", "HEA15", "header error", "hash is invalid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("header", "HEA90", "header error", "hash error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError("param", "PAR97", "param error", "not json valid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("param", "PAR98", "param error", "json digest is false", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("param", "PAR99", "param error", "json null", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError("gameversion", "GVA00", "version error", "error in sql select Version", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("gameversion", "GVA01", "version error", "stop : update app", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("gameversion", "GVA02", "version error", "stop unknow version : update app", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("gameversion", "GVA99", "version error", "block data", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError("account", "ACC01", "Account error", "action is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC02", "Account error", "action is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC03", "Account error", "appname is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC04", "Account error", "appname is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC05", "Account error", "appmail is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC06", "Account error", "appmail is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC10", "Account error", "email is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC11", "Account error", "password is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC12", "Account error", "confirm password is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC13", "Account error", "old password is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC14", "Account error", "new password is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC22", "Account error", "sign-up password is different to confirm password", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC24", "Account error", "sign-up new password is different to confirm password", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC40", "Account error", "email is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC41", "Account error", "password is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC42", "Account error", "confirm password is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC43", "Account error", "old password is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC44", "Account error", "new password is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC55", "Account error", "email or login unknow", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC56", "Account error", "multi-account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC71", "Account error", "GoogleID is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC72", "Account error", "GoogleID is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC73", "Account error", "Google Graph error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC74", "Account error", "Google SDK error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC75", "Account error", "Google sql select error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC76", "Account error", "Google sql update error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC77", "Account error", "Google multi-account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC78", "Account error", "Google singin error allready log with this account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC81", "Account error", "FacebookID is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC82", "Account error", "FacebookID is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC83", "Account error", "Facebook Graph error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC84", "Account error", "Facebook SDK error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC85", "Account error", "Facebook sql select error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC86", "Account error", "Facebook sql update error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC87", "Account error", "Facebook multi-account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC88", "Account error", "Facebook singin error allready log with this account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC90", "Account error", "error in request select in Account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC91", "Account error", "error in request insert anonymous Account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC92", "Account error", "unknow account but not temporary … it's not possible … maybe destroyed account", "OK", NWDErrorType.Alert);
            NWDError.CreateGenericError("account", "ACC95", "Account error", "user is multiple", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC98", "Account error", "user is banned, no sign-in", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "ACC99", "Account error", "user is banned", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN01", "Account sign error", "sign-up error in select valid account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN02", "Account sign error", "sign-up error in select account by uuid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN03", "Account sign error", "sign-up error in update account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN04", "Account sign error", "sign-up error account allready linked with another email", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN05", "Account sign error", "sign-up error multi-account by uuid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN06", "Account sign error", "sign-up error account allready linked with this email", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN07", "Account sign error", "sign-up error another account allready linked with this email", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN08", "Account sign error", "sign-up error multi-account allready linked with this email", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN09", "Account sign error", "modify error in select valid account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN10", "Account sign error", "modify error unknow account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN11", "Account sign error", "sign-up error in update account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN12", "Account sign error", "modify error multi-account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN13", "Account sign error", "modify error in select valid account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN14", "Account sign error", "modify error email allready use in another account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN15", "Account sign error", "singin error in request account ", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN16", "Account sign error", "singin error no account ", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN17", "Account sign error", "singin error allready log with this account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN18", "Account sign error", "singin error multi-account ", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN19", "Account sign error", "delete error in update account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError("account", "SGN25", "Account sign error", "signanonymous error in request account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN26", "Account sign error", "signanonymous error no account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN27", "Account sign error", "signanonymous error allready log with this account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN28", "Account sign error", "signanonymous error multi-account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError("account", "SGN33", "Account sign error", "signout impossible with anonymous account equal to restaured account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError("account", "SGN70", "Account sign error", "rescue select error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN71", "Account sign error", "rescue unknow user", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN72", "Account sign error", "rescue multi-user", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN80", "Account sign error", "session select error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN81", "Account sign error", "impossible unknow user", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN82", "Account sign error", "impossible multi-users", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError("token", "RQT01", "Token error", "error in request token creation", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT11", "Token error", "new token is not in base", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT12", "Token error", "error in token select", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT13", "Token error", "error in all token delete", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT14", "Token error", "error in old token delete", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError("token", "RQT90", "Token error", "session not exists", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT91", "Token error", "session expired", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT92", "Token error", "token not in base", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT93", "Token error", "too much tokens in base ... reconnect you", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT94", "Token error", "too much tokens in base ... reconnect you", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            if (CreateAllEnvironmentPHP())
            {
                foreach (Type tType in mTypeList)
                {
                    EditorUtility.DisplayProgressBar(tProgressBarTitle, "Create " + tType.Name + " files", tOperation / tCountClass);
                    tOperation++;
                    var tMethodInfo = tType.GetMethod("CreateAllPHP", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        tMethodInfo.Invoke(null, null);
                    }
                }
            }
            EditorUtility.DisplayProgressBar(tProgressBarTitle, "Finish", 1.0F);
            EditorUtility.ClearProgressBar();

            // TODO reccord the new Configuration;
            NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppEnvironment.SelectedEnvironment());
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool CreateAllEnvironmentPHP()
        {
            bool rReturn = false;
            if (CopyEnginePHP())
            {
                foreach (NWDAppEnvironment tEnvironement in NWDAppConfiguration.SharedInstance().AllEnvironements())
                {
                    tEnvironement.CreatePHP();
                }
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //static private int kCounterExport=0;
        //-------------------------------------------------------------------------------------------------------------
        public void ExportWebSites()
        {
            string tPath = EditorUtility.SaveFolderPanel("Export WebSite(s)", "", "NetWorkedDataServer");
            //int tPHPBuild = BTBConfigManager.SharedInstance().GetInt(NWDConstants.K_NWD_WS_BUILD, 0);
            string tFolder = NWDAppConfiguration.SharedInstance().WebFolder;
            //string tFolder = "NetWorkedData";
            //tFolder = new DirectoryInfo(NWDAppEnvironment.SelectedEnvironment().ServerHTTPS).Name;
            //string tFolder = Path.get(NWDAppEnvironment.SelectedEnvironment().ServerHTTPS);
            //kCounterExport++;
            if (tPath != null)
            {
                if (tPath.Length != 0)
                {
                    if (Directory.Exists(tPath + "/" + tFolder + "_AllVersions") == false)
                    {
                        Directory.CreateDirectory(tPath + "/" + tFolder + "_AllVersions");
                    }
                    if (Directory.Exists(tPath + "/" + tFolder + "_AllVersions") == true)
                    {

                        string tOwnerFolderServer = NWDToolbox.FindOwnerServerFolder();

                        NWDToolbox.ExportCopyFolderFiles(tOwnerFolderServer + "/", tPath + "/" + tFolder + "_AllVersions");
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool  CopyEnginePHP()
        {
           bool rReturn  = false;
            string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();

            string tFolderScript = NWDFindPackage.SharedInstance().ScriptFolderFromAssets + "/Editor/NWDServer";
            string tOwnerFolderServer = NWDToolbox.FindOwnerServerFolder();
            //Debug.Log ("tWebServiceFolder = " + tWebServiceFolder);

            if (AssetDatabase.IsValidFolder(tOwnerFolderServer + "/" + tWebServiceFolder) == false)
            {
                Debug.LogWarning(tOwnerFolderServer + "/" + tWebServiceFolder + " MUST BE CREATE !!!");
                AssetDatabase.CreateFolder(tOwnerFolderServer, tWebServiceFolder);
                AssetDatabase.ImportAsset(tOwnerFolderServer + "/" + tWebServiceFolder);
            }
            if (AssetDatabase.IsValidFolder(tOwnerFolderServer + "/" + tWebServiceFolder) == true)
            {
                NWDToolbox.CopyFolderFiles(tFolderScript, tOwnerFolderServer + "/" + tWebServiceFolder);
                rReturn = true;
            }
            else
            {
                Debug.LogWarning(tOwnerFolderServer + "/" + tWebServiceFolder + " NOT CREATED !!!");
            }
            // TODO Copy the Special file too 
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------

    }
}
//=====================================================================================================================
#endif