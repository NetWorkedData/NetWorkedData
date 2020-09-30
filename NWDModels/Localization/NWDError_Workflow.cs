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
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDErrorHelper : NWDHelper<NWDError>
    {
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateBasisError()
        {
            // TODO : too long ... thaht take 0.501 seconds
            //NWDBenchmark.Start();
            if (IsLoaded() == true)
            {
                GenerateServerErreur();
                GenerateGenericErreur();
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateServerErreur()
        {
            //NWDBenchmark.Start();
            NWDError.CreateGenericError(NWDError.NWDError_ADMIN, "ADMIN", "error your are not admin", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ERR01, "ERROR error", "Invalid Request.", "OK", NWDErrorType.Alert);
            NWDError.CreateGenericError(NWDError.NWDError_RESC04, "ERROR error", "Invalid Reference match.", "OK", NWDErrorType.Alert);
            NWDError.CreateGenericError(NWDError.NWDError_RESC03, "Rescue error", "Too much match.", "OK", NWDErrorType.Alert);
            NWDError.CreateGenericError(NWDError.NWDError_RESC02, "Rescue error", "No match.", "OK", NWDErrorType.Alert);
            NWDError.CreateGenericError(NWDError.NWDError_RESC01, "Rescue error", "Invalid reqest.", "OK", NWDErrorType.Alert);


            NWDError.CreateGenericError(NWDError.NWDError_WEB01, "Network", "no network or time out", "OK", NWDErrorType.InGame, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_WEB02, "Network", "http error", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_WEB03, "Network", "http respond is empty", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_WEB04, "Network", "http respond is not valid format", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError(NWDError.NWDError_UIG00, "ID", "error in unique generate", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SQL00, "SQL", "error SQL CONNEXION IMPOSSIBLE", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_DISKFULL, "SQL", "error SQL ... no place for new user", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError(NWDError.NWDError_HEA01, "header error", "os is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_HEA02, "header error", "version is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_HEA03, "header error", "lang is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_HEA04, "header error", "uuid is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_HEA05, "header error", "hash is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_HEA11, "header error", "os is invalid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_HEA12, "header error", "version is invalid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_HEA13, "header error", "lang is invalid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_HEA14, "header error", "uuid is invalid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_HEA15, "header error", "hash is invalid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_HEA90, "header error", "hash error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError(NWDError.NWDError_PAR97, "param error", "not json valid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_PAR98, "param error", "json digest is false", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_PAR99, "param error", "json null", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError(NWDError.NWDError_GVA00, "version error", "error in sql select Version", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_GVA01, "version error", "stop : update app", "OK", NWDErrorType.Upgrade, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_GVA02, "version error", "stop unknow version : update app", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_GVA99, "version error", "block data", "OK", NWDErrorType.Upgrade, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError(NWDError.NWDError_ACC01, "Account error", "action is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC02, "Account error", "action is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC03, "Account error", "appname is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC04, "Account error", "appname is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC05, "Account error", "appmail is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC06, "Account error", "appmail is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC10, "Account error", "email is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC11, "Account error", "password is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC12, "Account error", "confirm password is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC13, "Account error", "old password is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC14, "Account error", "new password is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC22, "Account error", "sign-up password is different to confirm password", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC24, "Account error", "sign-up new password is different to confirm password", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC40, "Account error", "email is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC41, "Account error", "password is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC42, "Account error", "confirm password is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC43, "Account error", "old password is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC44, "Account error", "new password is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC55, "Account error", "email or login unknow", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC56, "Account error", "multi-account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC71, "Account error", "GoogleID is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC72, "Account error", "GoogleID is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC73, "Account error", "Google Graph error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC74, "Account error", "Google SDK error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC75, "Account error", "Google sql select error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC76, "Account error", "Google sql update error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC77, "Account error", "Google multi-account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC78, "Account error", "Google singin error already log with this account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC81, "Account error", "FacebookID is empty", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC82, "Account error", "FacebookID is invalid format", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC83, "Account error", "Facebook Graph error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC84, "Account error", "Facebook SDK error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC85, "Account error", "Facebook sql select error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC86, "Account error", "Facebook sql update error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC87, "Account error", "Facebook multi-account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC88, "Account error", "Facebook singin error already log with this account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC90, "Account error", "error in request select in Account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC91, "Account error", "error in request insert anonymous Account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC92, "Account error", "unknow account but not temporary … it's not possible … maybe destroyed account", "OK", NWDErrorType.Alert);
            NWDError.CreateGenericError(NWDError.NWDError_ACC95, "Account error", "user is multiple", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC97, "Account error", "create only for temporary UUID", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC98, "Account error", "user is banned, no sign-in", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_ACC99, "Account error", "user is banned", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN01, "Account sign error", "sign-up error in select valid account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN20, "Account sign error", "sign-up error in select account by uuid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN03, "Account sign error", "sign-up error in update account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN04, "Account sign error", "sign-up error account already linked with another email", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN05, "Account sign error", "sign-up error multi-account by uuid", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN06, "Account sign error", "sign-up error account already linked with this email", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN07, "Account sign error", "sign-up error another account already linked with signature", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN08, "Account sign error", "sign-up error multi-account already linked with this email", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN09, "Account sign error", "modify error in select valid account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN10, "Account sign error", "modify error unknow account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN11, "Account sign error", "sign-up error in update account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN12, "Account sign error", "modify error multi-account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN13, "Account sign error", "modify error in select valid account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN14, "Account sign error", "modify error email already use in another account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN15, "Account sign error", "sign error in request account ", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN16, "Account sign error", "signin error no account ", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN17, "Account sign error", "signin error already log with this account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN18, "Account sign error", "signin error multi-account ", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN19, "Account sign error", "delete error in update account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN20, "Account sign error", "already sign with default account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN33, "Account sign error", "signout impossible with anonymous account equal to restaured account", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN70, "Account sign error", "rescue select error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN71, "Account sign error", "rescue unknow user", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN72, "Account sign error", "rescue multi-user", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN80, "Account sign error", "session select error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN81, "Account sign error", "impossible unknow user", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_SGN82, "Account sign error", "impossible multi-users", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError(NWDError.NWDError_SHS01, "secret key error", "secret key error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError(NWDError.NWDError_SHS02, "secret key error", "invalid secret key error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError(NWDError.NWDError_RQT01, "Token error", "error in request token creation", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RQT11, "Token error", "new token is not in base", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RQT12, "Token error", "error in token select", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RQT13, "Token error", "error in all token delete", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RQT14, "Token error", "error in old token delete", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RQT90, "Token error", "session not exists", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RQT91, "Token error", "session expired", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RQT92, "Token error", "token not in base", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RQT93, "Token error", "too much tokens in base ... reconnect you", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RQT94, "Token error", "too much tokens in base ... reconnect you", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RQT95, "Token error", "token already used...", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RQT96, "Token error", "token integrity error...", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RQT97, "Token error", "token != token error...", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RQT98, "Security error", "Security error one...", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RQT99, "Security error", "Security error two...", "OK", NWDErrorType.Critical, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError(NWDError.NWDError_MAINTENANCE, NWD.K_MAINTENANCE_ERROR, NWD.K_MAINTENANCE_ERROR, "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_OBSOLETE, NWD.K_OBSOLETE_ERROR, NWD.K_OBSOLETE_ERROR, "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError(NWDError.NWDError_SERVER, "Server ", "server error", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError(NWDError.NWDError_IPB01, "IP Ban error", "IP is banned", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);

            NWDError.CreateGenericError(NWDError.NWDError_RescueRequest, "{APP} Rescue request", "{APP} rescue instruction : click here for rescue {URL} ", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RescuePage, "{APP} Rescue request", "{APP} rescue an email with new credential was send!", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RescuePageError, "{APP} Rescue request", "{APP} rescue error!", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RescueAnswerLogin, "{APP} Rescue Answer Login", "{APP} rescue informations new login is : {LOGIN} new password is : {PASSWORD}", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            NWDError.CreateGenericError(NWDError.NWDError_RescueAnswerEmail, "{APP} Rescue Answer Email", "{APP} rescue informations new password is : {PASSWORD} ", "OK", NWDErrorType.Alert, NWDBasisTag.TagInternal);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void GenerateGenericErreur()
        {
            //NWDBenchmark.Start();
            NWDError.CreateGenericError(NWDError.NWDError_XXx01, "Error in  model XXX", "error in request creation in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx02, "Error in  model XXX", "error in request creation add primary key in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx03, "Error in  model XXX", "error in request creation add autoincrement modify in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx05, "Error in  model XXX", "error in sql index creation in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx07, "Error in  model XXX", "error in sql defragment in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx08, "Error in  model XXX", "error in sql drop in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx09, "Error in  model XXX", "error in sql Flush in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx11, "Error in  model XXX", "error in sql add columns in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx12, "Error in  model XXX", "error in sql alter columns in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx31, "Error in  model XXX", "error in request insert new datas before update in XXX (upgrade table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx32, "Error in  model XXX", "error in request select datas to update in XXX (upgrade table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx33, "Error in  model XXX", "error in request select updatable datas in XXX (upgrade table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx38, "Error in  model XXX", "error in request update datas in XXX (upgrade table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx39, "Error in  model XXX", "error more than one row for this reference in  XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx40, "Error in  model XXX", "error in flush trashed in  XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx91, "Error in  model XXX", "error update integrity in XXX (upgrade table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx98, "Error in  model XXX", "error columns header sign in XXX (update webservice?)", "OK", NWDErrorType.UnityEditor, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx99, "Error in  model XXX", "error columns number in XXX (upgrade table?)", "OK", NWDErrorType.UnityEditor, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx88, "Error in  model XXX", "integrity of one datas is false, break in XXX", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            NWDError.CreateGenericError(NWDError.NWDError_XXx77, "Error in  model XXX", "error update log in XXX (upgrade table?)", "OK", NWDErrorType.LogVerbose, NWDBasisTag.TagServerCreated);
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void ClassDatasAreLoaded()
        {
            //NWDBenchmark.Start();
            base.ClassDatasAreLoaded();
            //GenerateBasisError();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDError : NWDBasis
    {
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
        public static NWDError GetErrorReference(string sReference)
        {
            NWDError rReturn = null;
            rReturn = NWDBasisHelper.BasisHelper<NWDError>().GetDataByReference(sReference) as NWDError;
            if (rReturn == null)
            {
                rReturn = new NWDError(false);
                rReturn.Type = NWDErrorType.Alert;
                rReturn.Code = "error";
                rReturn.Domain = "error";
                rReturn.Title = new NWDLocalizableStringType();
                rReturn.Description = new NWDLocalizableTextType();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDError GetErrorDomainCode(string sDomain, string sCode)
        {
            return GetErrorDomainCode(sDomain + NWEConstants.K_MINUS + sCode);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDError GetErrorDomainCode(string sDomainsCode)
        {
            NWDError rReturn = null;
            rReturn = NWDBasisHelper.BasisHelper<NWDError>().GetDataByReference(GetBasisHelper().ClassTrigramme + NWEConstants.K_MINUS + sDomainsCode) as NWDError;
            if (rReturn == null)
            {
                rReturn = new NWDError(false);
                rReturn.Type = NWDErrorType.Alert;
                rReturn.Code = "error";
                rReturn.Domain = "error";
                rReturn.Title = new NWDLocalizableStringType();
                rReturn.Description = new NWDLocalizableTextType();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ShowAlert(string sInfo, NWDUserNotificationDelegate sCompleteBlock = null)
        {
            NWDErrorType tType = Type;
#if UNITY_EDITOR
            if (Application.isPlaying == false)
            {
                // for editor alert!
                tType = NWDErrorType.UnityEditor;
            }
#endif
            NWDUserNotification tErrorNotification = new NWDUserNotification(this, sInfo, sCompleteBlock);
            switch (tType)
            {
                case NWDErrorType.LogVerbose:
                    {
                        Debug.Log(tErrorNotification.Title());
                    }
                    break;
                case NWDErrorType.LogWarning:
                    {
                        Debug.LogWarning(tErrorNotification.Title() + "\n" + tErrorNotification.Description());
                    }
                    break;
                case NWDErrorType.Alert:
                case NWDErrorType.Critical:
                case NWDErrorType.Upgrade:
                case NWDErrorType.Ignore:
                case NWDErrorType.InGame:
                    {
                        tErrorNotification.Post();
                    }
                    break;
                case NWDErrorType.UnityEditor:
                    {
#if UNITY_EDITOR
                        // if editor alert!
                        if (EditorUtility.DisplayDialog(tErrorNotification.Title(), tErrorNotification.Description(), tErrorNotification.TextValidate()) == true)
                        {
                            tErrorNotification.Validate();
                        }
#endif
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
