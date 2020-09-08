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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWD
    {
        //-------------------------------------------------------------------------------------------------------------
        // SQL INDEXATION
        //public const string K_REFERENCE_INDEX = "ReferenceIndex";
        public const string K_BASIS_INDEX = "BasisIndex";
        //public const string K_ACCOUNT_INDEX = "AccountIndex";
        //public const string K_INTERNAL_INDEX = "InternalIndex";
        //public const string K_EDITOR_INDEX = "EditorIndex";

        public const string K_REQUEST_TOKEN_INDEX = "TokenIndex";
        public const string K_IP_BAN_INDEX = "IPBanIndex";
        //-------------------------------------------------------------------------------------------------------------
        // INSPECTOR GROUP
        public const string K_INSPECTOR_BASIS = "Basis";
        //-------------------------------------------------------------------------------------------------------------
        // FOR PHP AUTO EDITION
        public const string K_ENG = "ENG";
        public const string K_DB = "DB";
        public const string K_HTACCESS = ".htaccess";
        public const string K_DOT_HTACCESS = "dot_htaccess.txt";
        public const string K_PATH_BASE= "$PATH_BASE";
        public const string K_CONSTANTS_FILE = "constants.php";
        public const string K_MANAGEMENT_FILE = "management.php";
        public const string K_WS_SYNCHRONISATION = "synchronization.php";
        public const string K_WS_ENGINE = "engine.php";
        public const string K_SQL_CON = "$SQL_CON";
        public const string K_SQL_CON_EDITOR = "$SQL_CON_EDITOR";
        public const string K_ENV = "$ENV";
        public const string K_NWD_SLT_SRV = "$NWD_SLT_SRV";
        public const string K_PHP_TIME_SYNC = "$TIME_SYNC";
        public const string K_JSON_SECURE_KEY = "securePost";
        public const string K_WEB_ACTION_KEY = "action";
        public const string K_WEB_ACTION_SIGNUP_KEY = "signup";
        public const string K_WEB_ACTION_SIGNIN_KEY = "signin";
        public const string K_WEB_ACTION_SIGNOUT_KEY = "signout";
        public const string K_WEB_ACTION_RESCUE_KEY = "rescue";
        public const string K_WEB_ACTION_RESTART_WEBSERVICE_KEY = "restartWebservice";
        public const string K_WEB_ACTION_USER_TRANSFERT_KEY = "usertransfert"; // used
        public const string K_WEB_ACTION_NEW_USER_KEY = "newuser";
        public const string K_WEB_ACTION_PREVIEW_USER_KEY = "preview_user";
        public const string K_WEB_ACTION_NEXT_USER_KEY = "next_user";
        public const string K_WEB_EREG_ACTION = "'/^("+K_WEB_ACTION_SIGNIN_KEY+"|"+K_WEB_ACTION_SIGNOUT_KEY+"|"+K_WEB_ACTION_RESCUE_KEY+"|"+K_WEB_ACTION_SIGNUP_KEY+")$/'";
        public const string K_WEB_EREG_SDKI = "'/^(.{1,64})$/'";
        public const string K_WEB_EREG_SDKT = "'/^(.{1,32})$/'";
        public const string K_WEB_EREG_SDKR = "'/^(.{0,128})$/'";
        //public const string K_WEB_EREG_EMAIL = "'/^(.{24,64})$/'";
        //public const string K_WEB_EREG_PASSWORD = "'/^(.{24,64})$/'";
        public const string K_WEB_WEBSIGN_KEY = "Wign";
        public const string K_WEB_ACTION_SYNC_KEY = "sync";
        public const string K_WEB_ACTION_REF_KEY = "ref";
        public const string K_WEB_SIGN_Key = "sdki";
        public const string K_WEB_SIGN_UP_TYPE_Key = "sdkt";
        public const string K_WEB_SIGN_UP_VALUE_Key = "sdkv";
        public const string K_WEB_SIGN_UP_RESCUE_Key = "sdkr";
        public const string K_WEB_SIGN_UP_LOGIN_Key = "sdkl";
        public const string K_WEB_LOG_Key = "log";
        public const string K_WEB_BENCHMARK_Key = "Benchmark";

        // if rescue
        public const string K_WEB_RESCUE_EMAIL_Key = "email";
        public const string K_WEB_RESCUE_LANGUAGE_Key = "language";
        public const string K_WEB_RESCUE_PROOF_Key = "fyr"; // use in URL of rescue email

        // for sync
        public const string K_WEB_DATA_KEY = "data";
        public const string K_WEB_DATA_ROW_COUNTER = "rowCount";
        public const string K_WEB_HEADER_OS_KEY = "os";
        public const string K_WEB_HEADER_LANG_KEY = "lang";
        public const string K_WEB_HEADER_VERSION_KEY = "version";
        public const string K_WEB_UUID_KEY = "uuid";
        public const string K_WEB_REQUEST_TOKEN_KEY = "token";
        public const string HashKey = "hash";
        public const string AdminHashKey = "adminHash";
        //-------------------------------------------------------------------------------------------------------------
        //public const string K_STATIC_ACCOUNT_PHP = "account.php";
        //public const string K_STATIC_ERROR_PHP = "error.php";
        public const string K_STATIC_FINISH_PHP = "finish.php";
        public const string K_STATIC_FUNCTIONS_PHP = "functions.php";
        public const string K_STATIC_REQUEST_PHP = "request.php";
        //public const string K_STATIC_REQUEST_TOKEN_PHP = "requesttoken.php";
        public const string K_STATIC_RELATIONSHIP_PHP = "relationship.php";
        public const string K_STATIC_RESCUE_PHP = "rescue.php";
        public const string K_STATIC_RESPOND_PHP = "respond.php";
        public const string K_STATIC_START_PHP = "start.php";
        public const string K_STATIC_VALUES_PHP = "values.php";
        public const string K_STATIC_FLASH_PHP = "https://www.show-my-app.com/r.php";
        //public const string K_STATIC_FLASH_CSS = "FlashMyApp.css";
        public const string K_PHP_WSBUILD = "$WSBUILD";
        //public const string K_WIP = "WIP_";
        //-------------------------------------------------------------------------------------------------------------
        //public const string K_ENV = "Environment";
        public const string K_WS_FILE = "webservices.php";
        //public const string K_WS_INSIDE_FILE = "webservices_inside.php";
        //public const string K_WS_FILE_ADDON = "webservices_addon.php";
        //public const string K_WS_ACCOUNT_ADDON = "accountservices.php";
        public const string K_AUTHENTICATION_PHP = "authentication.php";
        public const string K_BLANK_PHP = "blank.php";
        public const string K_INDEX_PHP = "index.php";
        public const string K_RESCUE_PHP = "rescue.php";
        public const string K_MAINTENANCE_PHP = "maintenance.php";
        public const string K_OBSOLETE_PHP = "obsolete.php";
        public const string K_NO_PAGE_PHP = "NoPage.php";
        public const string K_OBSOLETE_HEADER_KEY = "obsolete";
        public const string K_MAINTENANCE_HEADER_KEY = "maintenance";
        public const string K_OBSOLETE_ERROR = "OBSOLETE";
        public const string K_MAINTENANCE_ERROR = "MAINTENANCE";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_JSON_ERROR_KEY = "error";
        public const string K_JSON_ERROR_CODE_KEY = "error_code";
        public const string K_JSON_ERROR_INFOS_KEY= "error_infos";
        public const string K_JSON_WEB_SERVICE_KEY= "wsbuild";
        public const string K_JSON_PERFORM_KEY = "perform";
        public const string K_JSON_AVG_KEY = "avg";
        public const string K_JSON_PERFORM_REQUEST_KEY = "performRequest";
        public const string K_JSON_TIMESTAMP_KEY = "timestamp";
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
