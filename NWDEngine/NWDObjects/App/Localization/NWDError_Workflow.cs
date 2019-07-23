//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:33:46
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDErrorHelper : NWDHelper<NWDError>
    {
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateBasisError()
        {
            Debug.Log("<color=red>NWDErrorHelper New_GenerateBasisError()</color>");
            if (DatabaseIsLoaded())
            {
            }
            if (DatasAreLoaded() == true)
            {
                Debug.Log("<color=green>NWDErrorHelper New_GenerateBasisError() OK DATA ARE LOADING</color>");
                NWDError.NWDError_WEB01 = NWDError.CreateGenericError("webrequest", "WEB01", "Network", "no network or time out", "OK", NWDErrorType.InGame, NWDBasisTag.TagInternal);

                if (NWDError.NWDError_WEB01 == null)
                {
                    Debug.Log("<color=red>NWDErrorHelper New_GenerateBasisError() ERROR NOT LOADED</color>");
                }
                else
                {
                    Debug.Log("<color=green>NWDErrorHelper New_GenerateBasisError() LOADED</color>");
                }

                NWDError.NWDError_WEB02 = NWDError.CreateGenericError("webrequest", "WEB02", "Network", "http error", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_WEB03 = NWDError.CreateGenericError("webrequest", "WEB03", "Network", "http respond is empty", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_WEB04 = NWDError.CreateGenericError("webrequest", "WEB04", "Network", "http respond is not valid format", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);

                NWDError.NWDError_UIG00 = NWDError.CreateGenericError("sql", "UIG00", "ID", "error in unique generate", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SQL00 = NWDError.CreateGenericError("sql", "SQL00", "SQL", "error SQL CONNEXION IMPOSSIBLE", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

                NWDError.NWDError_HEA01 = NWDError.CreateGenericError("header", "HEA01", "header error", "os is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_HEA02 = NWDError.CreateGenericError("header", "HEA02", "header error", "version is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_HEA03 = NWDError.CreateGenericError("header", "HEA03", "header error", "lang is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_HEA04 = NWDError.CreateGenericError("header", "HEA04", "header error", "uuid is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_HEA05 = NWDError.CreateGenericError("header", "HEA05", "header error", "hash is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_HEA11 = NWDError.CreateGenericError("header", "HEA11", "header error", "os is invalid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_HEA12 = NWDError.CreateGenericError("header", "HEA12", "header error", "version is invalid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_HEA13 = NWDError.CreateGenericError("header", "HEA13", "header error", "lang is invalid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_HEA14 = NWDError.CreateGenericError("header", "HEA14", "header error", "uuid is invalid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_HEA15 = NWDError.CreateGenericError("header", "HEA15", "header error", "hash is invalid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_HEA90 = NWDError.CreateGenericError("header", "HEA90", "header error", "hash error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);


                NWDError.NWDError_PAR97 = NWDError.CreateGenericError("param", "PAR97", "param error", "not json valid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_PAR98 = NWDError.CreateGenericError("param", "PAR98", "param error", "json digest is false", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_PAR99 = NWDError.CreateGenericError("param", "PAR99", "param error", "json null", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

                NWDError.NWDError_GVA00 = NWDError.CreateGenericError("gameversion", "GVA00", "version error", "error in sql select Version", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_GVA01 = NWDError.CreateGenericError("gameversion", "GVA01", "version error", "stop : update app", "OK", NWDErrorType.Upgrade, NWDBasisTag.TagInternal);
                NWDError.NWDError_GVA02 = NWDError.CreateGenericError("gameversion", "GVA02", "version error", "stop unknow version : update app", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_GVA99 = NWDError.CreateGenericError("gameversion", "GVA99", "version error", "block data", "OK", NWDErrorType.Upgrade, NWDBasisTag.TagInternal);

                NWDError.NWDError_ACC01 = NWDError.CreateGenericError("account", "ACC01", "Account error", "action is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC02 = NWDError.CreateGenericError("account", "ACC02", "Account error", "action is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC03 = NWDError.CreateGenericError("account", "ACC03", "Account error", "appname is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC04 = NWDError.CreateGenericError("account", "ACC04", "Account error", "appname is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC05 = NWDError.CreateGenericError("account", "ACC05", "Account error", "appmail is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC06 = NWDError.CreateGenericError("account", "ACC06", "Account error", "appmail is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC10 = NWDError.CreateGenericError("account", "ACC10", "Account error", "email is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC11 = NWDError.CreateGenericError("account", "ACC11", "Account error", "password is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC12 = NWDError.CreateGenericError("account", "ACC12", "Account error", "confirm password is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC13 = NWDError.CreateGenericError("account", "ACC13", "Account error", "old password is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC14 = NWDError.CreateGenericError("account", "ACC14", "Account error", "new password is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC22 = NWDError.CreateGenericError("account", "ACC22", "Account error", "sign-up password is different to confirm password", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC24 = NWDError.CreateGenericError("account", "ACC24", "Account error", "sign-up new password is different to confirm password", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC40 = NWDError.CreateGenericError("account", "ACC40", "Account error", "email is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC41 = NWDError.CreateGenericError("account", "ACC41", "Account error", "password is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC42 = NWDError.CreateGenericError("account", "ACC42", "Account error", "confirm password is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC43 = NWDError.CreateGenericError("account", "ACC43", "Account error", "old password is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC44 = NWDError.CreateGenericError("account", "ACC44", "Account error", "new password is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC55 = NWDError.CreateGenericError("account", "ACC55", "Account error", "email or login unknow", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC56 = NWDError.CreateGenericError("account", "ACC56", "Account error", "multi-account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC71 = NWDError.CreateGenericError("account", "ACC71", "Account error", "GoogleID is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC72 = NWDError.CreateGenericError("account", "ACC72", "Account error", "GoogleID is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC73 = NWDError.CreateGenericError("account", "ACC73", "Account error", "Google Graph error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC74 = NWDError.CreateGenericError("account", "ACC74", "Account error", "Google SDK error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC75 = NWDError.CreateGenericError("account", "ACC75", "Account error", "Google sql select error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC76 = NWDError.CreateGenericError("account", "ACC76", "Account error", "Google sql update error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC77 = NWDError.CreateGenericError("account", "ACC77", "Account error", "Google multi-account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC78 = NWDError.CreateGenericError("account", "ACC78", "Account error", "Google singin error allready log with this account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC81 = NWDError.CreateGenericError("account", "ACC81", "Account error", "FacebookID is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC82 = NWDError.CreateGenericError("account", "ACC82", "Account error", "FacebookID is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC83 = NWDError.CreateGenericError("account", "ACC83", "Account error", "Facebook Graph error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC84 = NWDError.CreateGenericError("account", "ACC84", "Account error", "Facebook SDK error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC85 = NWDError.CreateGenericError("account", "ACC85", "Account error", "Facebook sql select error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC86 = NWDError.CreateGenericError("account", "ACC86", "Account error", "Facebook sql update error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC87 = NWDError.CreateGenericError("account", "ACC87", "Account error", "Facebook multi-account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC88 = NWDError.CreateGenericError("account", "ACC88", "Account error", "Facebook singin error allready log with this account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC90 = NWDError.CreateGenericError("account", "ACC90", "Account error", "error in request select in Account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC91 = NWDError.CreateGenericError("account", "ACC91", "Account error", "error in request insert anonymous Account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC92 = NWDError.CreateGenericError("account", "ACC92", "Account error", "unknow account but not temporary … it's not possible … maybe destroyed account", "OK", NWDErrorType.Alert);
                NWDError.NWDError_ACC95 = NWDError.CreateGenericError("account", "ACC95", "Account error", "user is multiple", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC97 = NWDError.CreateGenericError("account", "ACC97", "Account error", "create only for temporary UUID", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC98 = NWDError.CreateGenericError("account", "ACC98", "Account error", "user is banned, no sign-in", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_ACC99 = NWDError.CreateGenericError("account", "ACC99", "Account error", "user is banned", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN01 = NWDError.CreateGenericError("account", "SGN01", "Account sign error", "sign-up error in select valid account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN20 = NWDError.CreateGenericError("account", "SGN02", "Account sign error", "sign-up error in select account by uuid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN03 = NWDError.CreateGenericError("account", "SGN03", "Account sign error", "sign-up error in update account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN04 = NWDError.CreateGenericError("account", "SGN04", "Account sign error", "sign-up error account allready linked with another email", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN05 = NWDError.CreateGenericError("account", "SGN05", "Account sign error", "sign-up error multi-account by uuid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN06 = NWDError.CreateGenericError("account", "SGN06", "Account sign error", "sign-up error account allready linked with this email", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN07 = NWDError.CreateGenericError("account", "SGN07", "Account sign error", "sign-up error another account allready linked with this email", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN08 = NWDError.CreateGenericError("account", "SGN08", "Account sign error", "sign-up error multi-account allready linked with this email", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN09 = NWDError.CreateGenericError("account", "SGN09", "Account sign error", "modify error in select valid account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN10 = NWDError.CreateGenericError("account", "SGN10", "Account sign error", "modify error unknow account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN11 = NWDError.CreateGenericError("account", "SGN11", "Account sign error", "sign-up error in update account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN12 = NWDError.CreateGenericError("account", "SGN12", "Account sign error", "modify error multi-account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN13 = NWDError.CreateGenericError("account", "SGN13", "Account sign error", "modify error in select valid account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN14 = NWDError.CreateGenericError("account", "SGN14", "Account sign error", "modify error email allready use in another account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN15 = NWDError.CreateGenericError("account", "SGN15", "Account sign error", "singin error in request account ", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN16 = NWDError.CreateGenericError("account", "SGN16", "Account sign error", "singin error no account ", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN17 = NWDError.CreateGenericError("account", "SGN17", "Account sign error", "singin error allready log with this account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN18 = NWDError.CreateGenericError("account", "SGN18", "Account sign error", "singin error multi-account ", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN19 = NWDError.CreateGenericError("account", "SGN19", "Account sign error", "delete error in update account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

                NWDError.NWDError_SGN20 = NWDError.CreateGenericError("account", "SGN20", "Account sign error", "allready sign with default account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

                //NWDError.CreateGenericError("account", "SGN25", "Account sign error", "signanonymous error in request account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                //NWDError.CreateGenericError("account", "SGN26", "Account sign error", "signanonymous error no account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                //NWDError.CreateGenericError("account", "SGN27", "Account sign error", "signanonymous error allready log with this account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                //NWDError.CreateGenericError("account", "SGN28", "Account sign error", "signanonymous error multi-account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

                NWDError.NWDError_SGN33 = NWDError.CreateGenericError("account", "SGN33", "Account sign error", "signout impossible with anonymous account equal to restaured account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

                NWDError.NWDError_SGN70 = NWDError.CreateGenericError("account", "SGN70", "Account sign error", "rescue select error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN71 = NWDError.CreateGenericError("account", "SGN71", "Account sign error", "rescue unknow user", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN72 = NWDError.CreateGenericError("account", "SGN72", "Account sign error", "rescue multi-user", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN80 = NWDError.CreateGenericError("account", "SGN80", "Account sign error", "session select error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN81 = NWDError.CreateGenericError("account", "SGN81", "Account sign error", "impossible unknow user", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_SGN82 = NWDError.CreateGenericError("account", "SGN82", "Account sign error", "impossible multi-users", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

                NWDError.NWDError_SHS01 = NWDError.CreateGenericError("account", "SHS01", "secret key error", "secret key error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

                NWDError.NWDError_SHS02 = NWDError.CreateGenericError("account", "SHS02", "secret key error", "invalid secret key error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

                NWDError.NWDError_RQT01 = NWDError.CreateGenericError("token", "RQT01", "Token error", "error in request token creation", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_RQT11 = NWDError.CreateGenericError("token", "RQT11", "Token error", "new token is not in base", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_RQT12 = NWDError.CreateGenericError("token", "RQT12", "Token error", "error in token select", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_RQT13 = NWDError.CreateGenericError("token", "RQT13", "Token error", "error in all token delete", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_RQT14 = NWDError.CreateGenericError("token", "RQT14", "Token error", "error in old token delete", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_RQT90 = NWDError.CreateGenericError("token", "RQT90", "Token error", "session not exists", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_RQT91 = NWDError.CreateGenericError("token", "RQT91", "Token error", "session expired", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_RQT92 = NWDError.CreateGenericError("token", "RQT92", "Token error", "token not in base", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_RQT93 = NWDError.CreateGenericError("token", "RQT93", "Token error", "too much tokens in base ... reconnect you", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_RQT94 = NWDError.CreateGenericError("token", "RQT94", "Token error", "too much tokens in base ... reconnect you", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_RQT95 = NWDError.CreateGenericError("token", "RQT95", "Token error", "token allready used...", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_RQT96 = NWDError.CreateGenericError("token", "RQT96", "Token error", "token integrity error...", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_RQT97 = NWDError.CreateGenericError("token", "RQT97", "Token error", "token != token error...", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_RQT98 = NWDError.CreateGenericError("token", "RQT98", "Security error", "Security error one...", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
                NWDError.NWDError_RQT99 = NWDError.CreateGenericError("token", "RQT99", "Security error", "Security error two...", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);

                NWDError.NWDError_MAINTENANCE = NWDError.CreateGenericError("webrequest", NWD.K_MAINTENANCE_ERROR, NWD.K_MAINTENANCE_ERROR, NWD.K_MAINTENANCE_ERROR, "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
                NWDError.NWDError_OBSOLETE = NWDError.CreateGenericError("webrequest", NWD.K_OBSOLETE_ERROR, NWD.K_OBSOLETE_ERROR, NWD.K_OBSOLETE_ERROR, "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

                NWDError.NWDError_SERVER = NWDError.CreateGenericError("Server", "SERVER", "Server ", "server error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

                NWDError.NWDError_RESC01 = NWDError.CreateGenericError("RESCUE", "01", "{APP} : Forgotten password", "Hello,\r\n" +
                                             "You forgot your password for the App {APP}'s account and ask to reset it." +
                                             "If you didn't ask the reset, ignore it.\r\n" +
                                             "Else, just click on this link to reset your password and receipt a new password by email: \r\n" +
                                             "\r\n" +
                                             "reset my password: {URL}\r\n" +
                                             "\r\n" +
                                             "Best regards,\r\n" +
                                             "The {APP}'s team.", "OK");

                NWDError.NWDError_RESC02 = NWDError.CreateGenericError("RESCUE", "02", "{APP} : Password rescue", "Hello,\r\n" +
                                              "Your password was resetted!\r\n" +
                                              "Best regards,\r\n" +
                                              "The {APP}'s team.", "OK");

                NWDError.NWDError_RESC03 = NWDError.CreateGenericError("RESCUE", "03", "{APP} : Password Resetted", "Hello,\r\n" +
                                             "Your password for the App {APP}'s account was resetted to : \r\n" +
                                             "\r\n" +
                                             "{PASSWORD}\r\n" +
                                             "\r\n" +
                                             "Best regards,\r\n" +
                                             "The {APP}'s team.", "OK");


                NWDError.NWDError_XXx01 = NWDError.CreateGenericError("XXX", "XXXx01", "Error in  model XXX", "error in request creation in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx02 = NWDError.CreateGenericError("XXX", "XXXx02", "Error in  model XXX", "error in request creation add primary key in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx03 = NWDError.CreateGenericError("XXX", "XXXx03", "Error in  model XXX", "error in request creation add autoincrement modify in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx05 = NWDError.CreateGenericError("XXX", "XXXx05", "Error in  model XXX", "error in sql index creation in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx07 = NWDError.CreateGenericError("XXX", "XXXx07", "Error in  model XXX", "error in sql defragment in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx08 = NWDError.CreateGenericError("XXX", "XXXx08", "Error in  model XXX", "error in sql drop in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx09 = NWDError.CreateGenericError("XXX", "XXXx09", "Error in  model XXX", "error in sql Flush in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx11 = NWDError.CreateGenericError("XXX", "XXXx11", "Error in  model XXX", "error in sql add columns in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx12 = NWDError.CreateGenericError("XXX", "XXXx12", "Error in  model XXX", "error in sql alter columns in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx31 = NWDError.CreateGenericError("XXX", "XXXx31", "Error in  model XXX", "error in request insert new datas before update in XXX (update table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx32 = NWDError.CreateGenericError("XXX", "XXXx32", "Error in  model XXX", "error in request select datas to update in XXX (update table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx33 = NWDError.CreateGenericError("XXX", "XXXx33", "Error in  model XXX", "error in request select updatable datas in XXX (update table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx38 = NWDError.CreateGenericError("XXX", "XXXx38", "Error in  model XXX", "error in request update datas in XXX (update table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx39 = NWDError.CreateGenericError("XXX", "XXXx39", "Error in  model XXX", "error more than one row for this reference in  XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx40 = NWDError.CreateGenericError("XXX", "XXXx40", "Error in  model XXX", "error in flush trashed in  XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx91 = NWDError.CreateGenericError("XXX", "XXXx91", "Error in  model XXX", "error update integrity in XXX (update table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx99 = NWDError.CreateGenericError("XXX", "XXXx99", "Error in  model XXX", "error columns number in XXX (update table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx88 = NWDError.CreateGenericError("XXX", "XXXx88", "Error in  model XXX", "integrity of one datas is false, break in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
                NWDError.NWDError_XXx77 = NWDError.CreateGenericError("XXX", "XXXx77", "Error in  model XXX", "error update log in XXX (update table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);

                NWDError.NWDError_IPB01 = NWDError.CreateGenericError("IPBan", "IPB01", "IP Ban error", "IP is banned", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void ClassDatasAreLoaded()
        {
            base.ClassDatasAreLoaded();
            Debug.Log("ClassDatasAreLoaded() override method (" + GetType().FullName + ")");
            GenerateBasisError();

        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDError : NWDBasis<NWDError>
    {
        //-------------------------------------------------------------------------------------------------------------
        private const string XXX = "XXX";
        //-------------------------------------------------------------------------------------------------------------
        public NWDError()
        {
            //Debug.Log("NWDError Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDError(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDError Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string Enrichment(string sText, string sLanguage = null, bool sBold = true)
        {

            string rText = NWDLocalization.Enrichment(sText, sLanguage, sBold);
            rText = NWDUserNickname.Enrichment(rText, sBold);
            rText = NWDAccountNickname.Enrichment(rText, sLanguage, sBold);
            return rText;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PostNotificationError()
        {
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_ERROR, this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ShowAlert(string sInfo, BTBAlertOnCompleteBlock sCompleteBlock = null)
        {
            NWDErrorType tType = Type;
            // NWDErrorType set by compile environment
#if UNITY_EDITOR
            // NO CHANGE
            //tType = NWDErrorType.UnityEditor;
#elif UNITY_IOS
            // NO CHANGE
#elif UNITY_ANDROID
            // NO CHANGE
#elif UNITY_STANDALONE_OSX
            // NO CHANGE
            //tType = NWDErrorType.InGame;
#elif UNITY_STANDALONE_WIN
            tType = NWDErrorType.InGame;
#elif UNITY_STANDALONE_LINUX
            tType = NWDErrorType.InGame;
#else
            tType = NWDErrorType.InGame;
#endif
            switch (tType)
            {
                case NWDErrorType.Alert:
                    {
                        BTBAlert.Alert(Enrichment(Title.GetLocalString().Replace(XXX, sInfo)), Enrichment(Description.GetLocalString().Replace(XXX, sInfo)), Validation.GetLocalString().Replace(XXX, sInfo), sCompleteBlock);
                    }
                    break;
                case NWDErrorType.Critical:
                    {
                        BTBAlert.Alert(Enrichment(Title.GetLocalString().Replace(XXX, sInfo)), Enrichment(Description.GetLocalString().Replace(XXX, sInfo)), Validation.GetLocalString().Replace(XXX, sInfo), delegate (BTBMessageState state)
                           {
                               Application.Quit();
                           }
                        );
                    }
                    break;
                case NWDErrorType.Upgrade:
                    {
                        BTBAlert.Alert(Enrichment(Title.GetLocalString().Replace(XXX, sInfo)), Enrichment(Description.GetLocalString().Replace(XXX, sInfo)), Validation.GetLocalString().Replace(XXX, sInfo), delegate (BTBMessageState state)
                           {
                               string tURL = "https://www.google.fr/search?q=" + NWDAppEnvironment.SelectedEnvironment().AppName;
                               NWDVersion tVersion = NWDVersion.CurrentData();
#if UNITY_EDITOR
                            // NO CHANGE
#elif UNITY_IOS
                            if (string.IsNullOrEmpty(tVersion.IOSStoreURL) == false)
                            {
                                tURL = tVersion.IOSStoreURL;
                            }
#elif UNITY_ANDROID
                            if (string.IsNullOrEmpty(tVersion.GooglePlayURL) == false)
                            {
                                tURL = tVersion.GooglePlayURL;
                            }
#elif UNITY_STANDALONE_OSX
                            if (string.IsNullOrEmpty(tVersion.OSXStoreURL) == false)
                            {
                                tURL = tVersion.OSXStoreURL;
                            }
#elif UNITY_STANDALONE_WIN

#elif UNITY_STANDALONE_LINUX

#else

#endif
                               Application.OpenURL(tURL);
                               Application.Quit();
                           });
                        // TODO : redirection to Store

                    }
                    break;
                case NWDErrorType.Ignore:
                    {
                        // Do nothing
                    }
                    break;
                case NWDErrorType.InGame:
                    {
                        // Do nothing
                        PostNotificationError();
                    }
                    break;
                case NWDErrorType.LogVerbose:
                    {
                        Debug.Log("ALERT! " + Title.GetLocalString().Replace(XXX, sInfo) + " : " + Description.GetLocalString().Replace(XXX, sInfo));
                    }
                    break;
                case NWDErrorType.LogWarning:
                    {
                        Debug.LogWarning("WARNING! " + Title.GetLocalString().Replace(XXX, sInfo) + " : " + Description.GetLocalString().Replace(XXX, sInfo));
                    }
                    break;
                case NWDErrorType.UnityEditor:
                    {
                        BTBAlert.Alert(Title.GetLocalString().Replace(XXX, sInfo), Description.GetLocalString().Replace(XXX, sInfo), Validation.GetLocalString().Replace(XXX, sInfo), sCompleteBlock);
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================