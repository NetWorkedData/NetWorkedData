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
#endif
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;


//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDError : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// WEB01 no network.
        /// </summary>
        public const string NWDError_ADMIN = "ADMIN" + NWEConstants.K_MINUS + "WAR";
        public const string NWDError_DISKFULL = "sql" + NWEConstants.K_MINUS + "SQL99";
        public const string NWDError_ERR01 = "ERR" + NWEConstants.K_MINUS + "ERR01";
        public const string NWDError_RESC04 = "ERR" + NWEConstants.K_MINUS + "RESC04";
        public const string NWDError_RESC03 = "ERR" + NWEConstants.K_MINUS + "RESC03";
        public const string NWDError_RESC02 = "ERR" + NWEConstants.K_MINUS + "RESC02";
        public const string NWDError_RESC01 = "ERR" + NWEConstants.K_MINUS + "RESC01";

        public const string NWDError_WEB01 = "webrequest" + NWEConstants.K_MINUS + "WEB01";
        public const string NWDError_WEB02 = "webrequest" + NWEConstants.K_MINUS + "WEB02";
        public const string NWDError_WEB03 = "webrequest" + NWEConstants.K_MINUS + "WEB03";
        public const string NWDError_WEB04 = "webrequest" + NWEConstants.K_MINUS + "WEB04";
        public const string NWDError_UIG00 = "sql" + NWEConstants.K_MINUS + "UIG00";
        public const string NWDError_SQL00 = "sql" + NWEConstants.K_MINUS + "SQL00";
        public const string NWDError_HEA01 = "header" + NWEConstants.K_MINUS + "HEA01";
        public const string NWDError_HEA02 = "header" + NWEConstants.K_MINUS + "HEA02";
        public const string NWDError_HEA03 = "header" + NWEConstants.K_MINUS + "HEA03";
        public const string NWDError_HEA04 = "header" + NWEConstants.K_MINUS + "HEA04";
        public const string NWDError_HEA05 = "header" + NWEConstants.K_MINUS + "HEA05";
        public const string NWDError_HEA11 = "header" + NWEConstants.K_MINUS + "HEA11";
        public const string NWDError_HEA12 = "header" + NWEConstants.K_MINUS + "HEA12";
        public const string NWDError_HEA13 = "header" + NWEConstants.K_MINUS + "HEA13";
        public const string NWDError_HEA14 = "header" + NWEConstants.K_MINUS + "HEA14";
        public const string NWDError_HEA15 = "header" + NWEConstants.K_MINUS + "HEA15";
        public const string NWDError_HEA90 = "header" + NWEConstants.K_MINUS + "HEA90";
        public const string NWDError_PAR97 = "param" + NWEConstants.K_MINUS + "PAR97";
        public const string NWDError_PAR98 = "param" + NWEConstants.K_MINUS + "PAR98";
        public const string NWDError_PAR99 = "param" + NWEConstants.K_MINUS + "PAR99";
        public const string NWDError_GVA00 = "gameversion" + NWEConstants.K_MINUS + "GVA00";
        public const string NWDError_GVA01 = "gameversion" + NWEConstants.K_MINUS + "GVA01";
        public const string NWDError_GVA02 = "gameversion" + NWEConstants.K_MINUS + "GVA02";
        public const string NWDError_GVA99 = "gameversion" + NWEConstants.K_MINUS + "GVA99";
        public const string NWDError_ACC01 = "account" + NWEConstants.K_MINUS + "ACC01";
        public const string NWDError_ACC02 = "account" + NWEConstants.K_MINUS + "ACC02";
        public const string NWDError_ACC03 = "account" + NWEConstants.K_MINUS + "ACC03";
        public const string NWDError_ACC04 = "account" + NWEConstants.K_MINUS + "ACC04";
        public const string NWDError_ACC05 = "account" + NWEConstants.K_MINUS + "ACC05";
        public const string NWDError_ACC06 = "account" + NWEConstants.K_MINUS + "ACC06";
        public const string NWDError_ACC10 = "account" + NWEConstants.K_MINUS + "ACC10";
        public const string NWDError_ACC11 = "account" + NWEConstants.K_MINUS + "ACC11";
        public const string NWDError_ACC12 = "account" + NWEConstants.K_MINUS + "ACC12";
        public const string NWDError_ACC13 = "account" + NWEConstants.K_MINUS + "ACC13";
        public const string NWDError_ACC14 = "account" + NWEConstants.K_MINUS + "ACC14";
        public const string NWDError_ACC22 = "account" + NWEConstants.K_MINUS + "ACC22";
        public const string NWDError_ACC24 = "account" + NWEConstants.K_MINUS + "ACC24";
        public const string NWDError_ACC40 = "account" + NWEConstants.K_MINUS + "ACC40";
        public const string NWDError_ACC41 = "account" + NWEConstants.K_MINUS + "ACC41";
        public const string NWDError_ACC42 = "account" + NWEConstants.K_MINUS + "ACC42";
        public const string NWDError_ACC43 = "account" + NWEConstants.K_MINUS + "ACC43";
        public const string NWDError_ACC44 = "account" + NWEConstants.K_MINUS + "ACC44";
        public const string NWDError_ACC55 = "account" + NWEConstants.K_MINUS + "ACC55";
        public const string NWDError_ACC56 = "account" + NWEConstants.K_MINUS + "ACC56";
        public const string NWDError_ACC71 = "account" + NWEConstants.K_MINUS + "ACC71";
        public const string NWDError_ACC72 = "account" + NWEConstants.K_MINUS + "ACC72";
        public const string NWDError_ACC73 = "account" + NWEConstants.K_MINUS + "ACC73";
        public const string NWDError_ACC74 = "account" + NWEConstants.K_MINUS + "ACC74";
        public const string NWDError_ACC75 = "account" + NWEConstants.K_MINUS + "ACC75";
        public const string NWDError_ACC76 = "account" + NWEConstants.K_MINUS + "ACC76";
        public const string NWDError_ACC77 = "account" + NWEConstants.K_MINUS + "ACC77";
        public const string NWDError_ACC78 = "account" + NWEConstants.K_MINUS + "ACC78";
        public const string NWDError_ACC81 = "account" + NWEConstants.K_MINUS + "ACC81";
        public const string NWDError_ACC82 = "account" + NWEConstants.K_MINUS + "ACC82";
        public const string NWDError_ACC83 = "account" + NWEConstants.K_MINUS + "ACC83";
        public const string NWDError_ACC84 = "account" + NWEConstants.K_MINUS + "ACC84";
        public const string NWDError_ACC85 = "account" + NWEConstants.K_MINUS + "ACC85";
        public const string NWDError_ACC86 = "account" + NWEConstants.K_MINUS + "ACC86";
        public const string NWDError_ACC87 = "account" + NWEConstants.K_MINUS + "ACC87";
        public const string NWDError_ACC88 = "account" + NWEConstants.K_MINUS + "ACC88";
        public const string NWDError_ACC90 = "account" + NWEConstants.K_MINUS + "ACC90";
        public const string NWDError_ACC91 = "account" + NWEConstants.K_MINUS + "ACC91";
        public const string NWDError_ACC92 = "account" + NWEConstants.K_MINUS + "ACC92";
        public const string NWDError_ACC95 = "account" + NWEConstants.K_MINUS + "ACC95";
        public const string NWDError_ACC97 = "account" + NWEConstants.K_MINUS + "ACC97";
        public const string NWDError_ACC98 = "account" + NWEConstants.K_MINUS + "ACC98";
        public const string NWDError_ACC99 = "account" + NWEConstants.K_MINUS + "ACC99";
        public const string NWDError_SGN01 = "account" + NWEConstants.K_MINUS + "SGN01";
        public const string NWDError_SGN02 = "account" + NWEConstants.K_MINUS + "SGN02";
        public const string NWDError_SGN03 = "account" + NWEConstants.K_MINUS + "SGN03";
        public const string NWDError_SGN04 = "account" + NWEConstants.K_MINUS + "SGN04";
        public const string NWDError_SGN05 = "account" + NWEConstants.K_MINUS + "SGN05";
        public const string NWDError_SGN06 = "account" + NWEConstants.K_MINUS + "SGN06";
        public const string NWDError_SGN07 = "account" + NWEConstants.K_MINUS + "SGN07";
        public const string NWDError_SGN08 = "account" + NWEConstants.K_MINUS + "SGN08";
        public const string NWDError_SGN09 = "account" + NWEConstants.K_MINUS + "SGN09";
        public const string NWDError_SGN10 = "account" + NWEConstants.K_MINUS + "SGN10";
        public const string NWDError_SGN11 = "account" + NWEConstants.K_MINUS + "SGN11";
        public const string NWDError_SGN12 = "account" + NWEConstants.K_MINUS + "SGN12";
        public const string NWDError_SGN13 = "account" + NWEConstants.K_MINUS + "SGN13";
        public const string NWDError_SGN14 = "account" + NWEConstants.K_MINUS + "SGN14";
        public const string NWDError_SGN15 = "account" + NWEConstants.K_MINUS + "SGN15";
        public const string NWDError_SGN16 = "account" + NWEConstants.K_MINUS + "SGN16";
        public const string NWDError_SGN17 = "account" + NWEConstants.K_MINUS + "SGN17";
        public const string NWDError_SGN18 = "account" + NWEConstants.K_MINUS + "SGN18";
        public const string NWDError_SGN19 = "account" + NWEConstants.K_MINUS + "SGN19";
        public const string NWDError_SGN20 = "account" + NWEConstants.K_MINUS + "SGN20";
        public const string NWDError_SGN33 = "account" + NWEConstants.K_MINUS + "SGN33";
        public const string NWDError_SGN70 = "account" + NWEConstants.K_MINUS + "SGN70";
        public const string NWDError_SGN71 = "account" + NWEConstants.K_MINUS + "SGN71";
        public const string NWDError_SGN72 = "account" + NWEConstants.K_MINUS + "SGN72";
        public const string NWDError_SGN80 = "account" + NWEConstants.K_MINUS + "SGN80";
        public const string NWDError_SGN81 = "account" + NWEConstants.K_MINUS + "SGN81";
        public const string NWDError_SGN82 = "account" + NWEConstants.K_MINUS + "SGN82";
        public const string NWDError_SHS01 = "account" + NWEConstants.K_MINUS + "SHS01";
        public const string NWDError_SHS02 = "account" + NWEConstants.K_MINUS + "SHS02";
        public const string NWDError_RQT01 = "token" + NWEConstants.K_MINUS + "RQT01";
        public const string NWDError_RQT11 = "token" + NWEConstants.K_MINUS + "RQT11";
        public const string NWDError_RQT12 = "token" + NWEConstants.K_MINUS + "RQT12";
        public const string NWDError_RQT13 = "token" + NWEConstants.K_MINUS + "RQT13";
        public const string NWDError_RQT14 = "token" + NWEConstants.K_MINUS + "RQT14";
        public const string NWDError_RQT90 = "token" + NWEConstants.K_MINUS + "RQT90";
        public const string NWDError_RQT91 = "token" + NWEConstants.K_MINUS + "RQT91";
        public const string NWDError_RQT92 = "token" + NWEConstants.K_MINUS + "RQT92";
        public const string NWDError_RQT93 = "token" + NWEConstants.K_MINUS + "RQT93";
        public const string NWDError_RQT94 = "token" + NWEConstants.K_MINUS + "RQT94";
        public const string NWDError_RQT95 = "token" + NWEConstants.K_MINUS + "RQT95";
        public const string NWDError_RQT96 = "token" + NWEConstants.K_MINUS + "RQT96";
        public const string NWDError_RQT97 = "token" + NWEConstants.K_MINUS + "RQT97";
        public const string NWDError_RQT98 = "token" + NWEConstants.K_MINUS + "RQT98";
        public const string NWDError_RQT99 = "token" + NWEConstants.K_MINUS + "RQT99";
        public const string NWDError_MAINTENANCE = "webrequest" + NWEConstants.K_MINUS + NWD.K_MAINTENANCE_ERROR;
        public const string NWDError_OBSOLETE = "webrequest" + NWEConstants.K_MINUS + NWD.K_OBSOLETE_ERROR;
        public const string NWDError_SERVER = "Server" + NWEConstants.K_MINUS + "SERVER";
        public const string NWDError_IPB01 = "IPBan" + NWEConstants.K_MINUS + "IPB01";
        public const string NWDError_RescueRequest = "RESC" + NWEConstants.K_MINUS + "RescueRequest";
        public const string NWDError_RescuePage = "RESC" + NWEConstants.K_MINUS + "NWDError_RescuePage";
        public const string NWDError_RescuePageError = "RESC" + NWEConstants.K_MINUS + "NWDError_RescuePageError";
        public const string NWDError_RescueAnswerLogin = "RESC" + NWEConstants.K_MINUS + "RescueAnswerLogin";
        public const string NWDError_RescueAnswerEmail = "RESC" + NWEConstants.K_MINUS + "RescueAnswerEmail";
        public const string NWDError_XXx01 = "XXX" + NWEConstants.K_MINUS + "XXXx01";
        public const string NWDError_XXx02 = "XXX" + NWEConstants.K_MINUS + "XXXx02";
        public const string NWDError_XXx03 = "XXX" + NWEConstants.K_MINUS + "XXXx03";
        public const string NWDError_XXx05 = "XXX" + NWEConstants.K_MINUS + "XXXx05";
        public const string NWDError_XXx07 = "XXX" + NWEConstants.K_MINUS + "XXXx07";
        public const string NWDError_XXx08 = "XXX" + NWEConstants.K_MINUS + "XXXx08";
        public const string NWDError_XXx09 = "XXX" + NWEConstants.K_MINUS + "XXXx09";
        public const string NWDError_XXx11 = "XXX" + NWEConstants.K_MINUS + "XXXx11";
        public const string NWDError_XXx12 = "XXX" + NWEConstants.K_MINUS + "XXXx12";
        public const string NWDError_XXx31 = "XXX" + NWEConstants.K_MINUS + "XXXx31";
        public const string NWDError_XXx32 = "XXX" + NWEConstants.K_MINUS + "XXXx32";
        public const string NWDError_XXx33 = "XXX" + NWEConstants.K_MINUS + "XXXx33";
        public const string NWDError_XXx38 = "XXX" + NWEConstants.K_MINUS + "XXXx38";
        public const string NWDError_XXx39 = "XXX" + NWEConstants.K_MINUS + "XXXx39";
        public const string NWDError_XXx40 = "XXX" + NWEConstants.K_MINUS + "XXXx40";
        public const string NWDError_XXx91 = "XXX" + NWEConstants.K_MINUS + "XXXx91";
        public const string NWDError_XXx98 = "XXX" + NWEConstants.K_MINUS + "XXXx98";
        public const string NWDError_XXx99 = "XXX" + NWEConstants.K_MINUS + "XXXx99";
        public const string NWDError_XXx88 = "XXX" + NWEConstants.K_MINUS + "XXXx88";
        public const string NWDError_XXx77 = "XXX" + NWEConstants.K_MINUS + "XXXx77";
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================