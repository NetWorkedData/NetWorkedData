// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:26:42
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using BasicToolBox;
using System.Globalization;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWD
    {
        //-------------------------------------------------------------------------------------------------------------
        public const string K_Resources = "Resources";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_DevEnvironment = "K_DevEnvironment";
        public const string K_PreprodEnvironment = "K_PreprodEnvironment";
        public const string K_ProdEnvironment = "K_ProdEnvironment";
        //-------------------------------------------------------------------------------------------------------------
        public static string K_ReturnLine = "\n";
        public static string K_CommentSeparator = "//--------------------";
        public static string K_CommentAutogenerate = "//NWD Autogenerate File at ";
        public static string K_CommentCopyright = "//Copyright NetWorkedDatas ideMobi ";
        public static string K_CommentCreator = "//Created by Jean-François CONTART ";
        //-------------------------------------------------------------------------------------------------------------
        //public const string K_ENV = "Environment";
        public const string K_ENG = "ENG";
        public const string K_DB = "DB";
        public const string K_CONSTANTS_FILE = "constants.php";
        public const string K_MANAGEMENT_FILE = "management.php";
        public const string K_WS_FILE = "webservices.php";
        public const string K_WS_INSIDE_FILE = "webservices_inside.php";
        public const string K_WS_FILE_ADDON = "webservices_addon.php";
        public const string K_WS_ACCOUNT_ADDON = "accountservices.php";
        public const string K_WS_SYNCHRONISATION = "synchronization.php";
        public const string K_DOT_HTACCESS = "dot_htaccess.txt";
        public const string K_HTACCESS = ".htaccess";
        public const string K_AUTHENTIFICATION_PHP = "authentification.php";
        public const string K_BLANK_PHP = "blank.php";
        public const string K_INDEX_PHP = "index.php";
        public const string K_RESCUE_PHP = "rescue.php";
        public const string K_MAINTENANCE_PHP = "maintenance.php";
        public const string K_OBSOLETE_PHP = "obsolete.php";
        public const string K_NO_PAGE_PHP = "NoPage.php";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_STATIC_ACCOUNT_PHP = "account.php";
        public const string K_STATIC_ERROR_PHP = "error.php";
        public const string K_STATIC_FINISH_PHP = "finish.php";
        public const string K_STATIC_FUNCTIONS_PHP = "functions.php";
        public const string K_STATIC_REQUEST_PHP = "request.php";
        public const string K_STATIC_REQUEST_TOKEN_PHP = "requesttoken.php";
        public const string K_STATIC_RELATIONSHIP_PHP = "relationship.php";
        public const string K_STATIC_RESCUE_PHP = "rescue.php";
        public const string K_STATIC_RESPOND_PHP = "respond.php";
        public const string K_STATIC_START_PHP = "start.php";
        public const string K_STATIC_VALUES_PHP = "values.php";
        public const string K_STATIC_FLASH_PHP = "FlashMyApp.php";
        public const string K_STATIC_FLASH_CSS = "FlashMyApp.css";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_LOADER = "Loader";
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDConstants
    {
        //-------------------------------------------------------------------------------------------------------------
        public static CultureInfo FormatCountry = CultureInfo.InvariantCulture;
        //-------------------------------------------------------------------------------------------------------------
        public static string FloatFormat = "F5";
        public static string FloatSQLFormat = "5";
        public static string DoubleFormat = "F5";
        public static string DoubleSQLFormat = "5";
        //-------------------------------------------------------------------------------------------------------------
        public static string K_DEVELOPMENT_NAME = "Dev";
        public static string K_PREPRODUCTION_NAME = "Preprod";
        public static string K_PRODUCTION_NAME = "Prod";
        //-------------------------------------------------------------------------------------------------------------
        public static string kStandardSeparator = "|";
        public static string kStandardSeparatorSubstitute = "@0#";
        //-------------------------------------------------------------------------------------------------------------
        //public static string kFieldNone = "<color=gray>none</color>";
        public static string kFieldNone = "none";
        public static string kFieldEmpty = "empty";
        public static string kFieldNotEmpty = "not empty";
        public static string kFieldSeparatorA = "•";
        public static string kFieldSeparatorB = ":";
        public static string kFieldSeparatorC = "_";
        public static string kFieldSeparatorD = "∆";
        public static string kFieldSeparatorE = "∂";
        public static string kFieldSeparatorASubstitute = "@1#";
        public static string kFieldSeparatorBSubstitute = "@2#";
        public static string kFieldSeparatorCSubstitute = "@3#";
        public static string kFieldSeparatorDSubstitute = "@4#";
        public static string kFieldSeparatorESubstitute = "@5#";
        //-------------------------------------------------------------------------------------------------------------
        static public string kPrefSaltValidKey = "SaltValid";
        static public string kPrefSaltAKey = "SaltA";
        static public string kPrefSaltBKey = "SaltB";
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================