// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:41
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        public void CreateErrorsAndMessagesAllClasses()
        {
            //BTBBenchmark.Start();
            string tProgressBarTitle = "NetWorkedData Create error";
            float tCountClass = mTypeList.Count + 1;
            float tOperation = 1;
            EditorUtility.DisplayProgressBar(tProgressBarTitle, "Create general error index", tOperation / tCountClass);
            tOperation++;

            CreateErrorsAndMessagesEngine();

            foreach (Type tType in mTypeList)
            {
                EditorUtility.DisplayProgressBar(tProgressBarTitle, "Create " + tType.Name + " errors and messages", tOperation / tCountClass);
                tOperation++;
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.New_CreateErrorAndMessage();
                tHelper.New_ErrorRegenerate();
                //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_CreateErrorsAndMessages);
                //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_ErrorRegenerate);
            }

            NWDDataManager.SharedInstance().DataQueueExecute();
            EditorUtility.DisplayProgressBar(tProgressBarTitle, "Finish", 1.0F);
            EditorUtility.ClearProgressBar();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CreateErrorsAndMessagesEngine()
        {
            //BTBBenchmark.Start();
            NWDError.CreateGenericError("webrequest", "WEB01", "Network", "no network or time out", "OK", NWDErrorType.InGame, NWDBasisTag.TagInternal);
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

            NWDError.CreateGenericError("gameversion", "GVA00", "version error", "error in sql select Version", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("gameversion", "GVA01", "version error", "stop : update app", "OK", NWDErrorType.Upgrade, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("gameversion", "GVA02", "version error", "stop unknow version : update app", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("gameversion", "GVA99", "version error", "block data", "OK", NWDErrorType.Upgrade, NWDBasisTag.TagInternal);

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

            //NWDError.CreateGenericError("account", "SGN25", "Account sign error", "signanonymous error in request account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            //NWDError.CreateGenericError("account", "SGN26", "Account sign error", "signanonymous error no account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            //NWDError.CreateGenericError("account", "SGN27", "Account sign error", "signanonymous error allready log with this account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            //NWDError.CreateGenericError("account", "SGN28", "Account sign error", "signanonymous error multi-account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError("account", "SGN33", "Account sign error", "signout impossible with anonymous account equal to restaured account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError("account", "SGN70", "Account sign error", "rescue select error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN71", "Account sign error", "rescue unknow user", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN72", "Account sign error", "rescue multi-user", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN80", "Account sign error", "session select error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN81", "Account sign error", "impossible unknow user", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("account", "SGN82", "Account sign error", "impossible multi-users", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError("account", "SHS01", "secret key error", "secret key error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError("token", "RQT01", "Token error", "error in request token creation", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT11", "Token error", "new token is not in base", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT12", "Token error", "error in token select", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT13", "Token error", "error in all token delete", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT14", "Token error", "error in old token delete", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT90", "Token error", "session not exists", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT91", "Token error", "session expired", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT92", "Token error", "token not in base", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT93", "Token error", "too much tokens in base ... reconnect you", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT94", "Token error", "too much tokens in base ... reconnect you", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT95", "Token error", "token allready used...", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT96", "Token error", "token integrity error...", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT97", "Token error", "token != token error...", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT98", "Security error", "Security error one...", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("token", "RQT99", "Security error", "Security error two...", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError("webrequest", NWD.K_MAINTENANCE_ERROR, NWD.K_MAINTENANCE_ERROR, NWD.K_MAINTENANCE_ERROR, "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError("webrequest", NWD.K_OBSOLETE_ERROR, NWD.K_OBSOLETE_ERROR, NWD.K_OBSOLETE_ERROR, "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError("Server", "SERVER", "Server ", "server error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

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
                                        "Your password was resetted!\r\n" +
                                        "Best regards,\r\n" +
                                        "The {APP}'s team.", "OK");

            NWDError.CreateGenericError("RESCUE", "03", "{APP} : Password Resetted", "Hello,\r\n" +
                                        "Your password for the App {APP}'s account was resetted to : \r\n" +
                                        "\r\n" +
                                        "{PASSWORD}\r\n" +
                                        "\r\n" +
                                        "Best regards,\r\n" +
                                        "The {APP}'s team.", "OK");

            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CreatePHPAllClass(NWDAppEnvironment sEnvironment ,bool sIncrement = true, bool sWriteOnDisk = true)
        {
            //BTBBenchmark.Start();
            if (sIncrement == true)
            {
                NWDAppConfiguration.SharedInstance().WebBuildMax++;
                NWDAppConfiguration.SharedInstance().WebBuild = NWDAppConfiguration.SharedInstance().WebBuildMax;
                NWDAppConfiguration.SharedInstance().WSList.Add(NWDAppConfiguration.SharedInstance().WebBuildMax, true);
            }
            else
            {
                string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();
                string tOwnerServerFolderPath = NWDToolbox.FindOwnerServerFolder();
                if (AssetDatabase.IsValidFolder(tOwnerServerFolderPath + "/" + tWebServiceFolder) == false)
                {
                    AssetDatabase.DeleteAsset(tOwnerServerFolderPath);
                }
            }
            sEnvironment.CreatePHP(NWDDataManager.SharedInstance().mTypeList, true, sWriteOnDisk);
            NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppEnvironment.SelectedEnvironment());
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CreatePHPAllClass(bool sIncrement = true, bool sWriteOnDisk = true)
        {
            //BTBBenchmark.Start();
            if (sIncrement == true)
            {
                NWDAppConfiguration.SharedInstance().WebBuildMax++;
                NWDAppConfiguration.SharedInstance().WebBuild = NWDAppConfiguration.SharedInstance().WebBuildMax;
                NWDAppConfiguration.SharedInstance().WSList.Add(NWDAppConfiguration.SharedInstance().WebBuildMax, true);
            }
            else
            {
                string tWebServiceFolder = NWDAppConfiguration.SharedInstance().WebServiceFolder();
                string tOwnerServerFolderPath = NWDToolbox.FindOwnerServerFolder();
                if (AssetDatabase.IsValidFolder(tOwnerServerFolderPath + "/" + tWebServiceFolder) == false)
                {
                    AssetDatabase.DeleteAsset(tOwnerServerFolderPath);
                }
            }
            foreach (NWDAppEnvironment tEnvironement in NWDAppConfiguration.SharedInstance().AllEnvironements())
            {
                tEnvironement.CreatePHP(NWDDataManager.SharedInstance().mTypeList, true, sWriteOnDisk);
            }
            NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppEnvironment.SelectedEnvironment());
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ModelResetAllClass()
        {
            //BTBBenchmark.Start();
            string tProgressBarTitle = "NetWorkedData Models Resets";
            float tCountClass = mTypeList.Count + 1;
            float tOperation = 1;
            EditorUtility.DisplayProgressBar(tProgressBarTitle, "Reset Model index", tOperation / tCountClass);
            tOperation++;
            foreach (Type tType in mTypeList)
            {
                EditorUtility.DisplayProgressBar(tProgressBarTitle, "Reset " + tType.Name + " model", tOperation / tCountClass);
                tOperation++;
                //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_ModelReset); 
                NWDBasisHelper.FindTypeInfos(tType).New_DeleteOldsModels();
            }
            EditorUtility.DisplayProgressBar(tProgressBarTitle, "Finish", 1.0F);
            EditorUtility.ClearProgressBar();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ExportWebSites()
        {
            //BTBBenchmark.Start();
            string tPath = EditorUtility.SaveFolderPanel("Export WebSite(s)", "", "NetWorkedDataServer");
            string tFolder = NWDAppConfiguration.SharedInstance().WebFolder;
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
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif