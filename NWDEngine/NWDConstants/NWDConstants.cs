//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
#if UNITY_EDITOR
    [InitializeOnLoad]
    [ExecuteInEditMode]
#endif
    public partial class NWDConstants
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string K_DEVELOPMENT = "Development";
        public static string K_PREPRODUCTION = "PreProduction";
        public static string K_PRODUCTION = "Production";
        //-------------------------------------------------------------------------------------------------------------
        public static string K_DEVELOPMENT_NAME = "Dev";
        public static string K_PREPRODUCTION_NAME = "Preprod";
        public static string K_PRODUCTION_NAME = "Prod";
        //-------------------------------------------------------------------------------------------------------------
        public static string K_APP_CHOOSER_ENVIRONMENT = "Select Environment used in Player Mode (Game panel)";
        public static string K_APP_CHOOSER_ENVIRONMENT_TITLE = "Environment chooser";
        public static string K_APP_SYNC_ENVIRONMENT = "Synchronize your datas in the good environment";
        public static string K_APP_SYNC_ENVIRONMENT_TITLE = "Environment synchronize";
        public static string K_VERSION_LABEL = "Version";
        //-------------------------------------------------------------------------------------------------------------
        public static string kStandardSeparator = "|";
        public static string kStandardSeparatorSubstitute = "@0#";
        //-------------------------------------------------------------------------------------------------------------
        public static string kFieldSeparatorA = "•";
        public static string kFieldSeparatorB = ":";
        public static string kFieldSeparatorC = "_";
        public static string kFieldSeparatorASubstitute = "@1#";
        public static string kFieldSeparatorBSubstitute = "@2#";
        public static string kFieldSeparatorCSubstitute = "@3#";
        //-------------------------------------------------------------------------------------------------------------
        public static string kAlertSaltShortError = "ALERT SALT ARE NOT MEMORIZE : RECCORD CONFIGURATIONS AND RECOMPILE!";
        public static string K_APP_CLASS_SALT_REGENERATE = "Generate salts";
        public static float kFieldMarge = 5.0f;

        public static int kLongString = 5;
        public static int kVeryLongString = 15;
        public static float kPrefabSize = 80.0f;
        public static float kIntWidth = 36.0f;
        public static float kConWidth = 42.0f;
        public static float kIconWidth = 36.0f;
        public static float kEditWidth = 16.0f;
        public static float kEditWidthHalf = 8.0f;
        public static float kEditWidthMini = 12.0f;
        public static float kEditWidthMiniHalf = 6.0f;
        public static float kLangWidth = 50.0f;
        public static float kConnectionIndent = 10.0f;
        //-------------------------------------------------------------------------------------------------------------
        // NetWorked synchronize alert
        public const string K_SYNC_ALERT_TITLE = "ALERT PRODUCTION";
        public const string K_SYNC_ALERT_MESSAGE = "YOU WILL SYNCHRONIZE ON THE PRODUCTION ENVIRONEMENT";
        public const string K_SYNC_ALERT_OK = "Ok";
        public const string K_SYNC_ALERT_CANCEL = "Cancel";
        //-------------------------------------------------------------------------------------------------------------
        // NetWorked synchronize alert
        public const string K_CLEAN_ALERT_TITLE = "CLEAN ALERT";
        public const string K_CLEAN_ALERT_MESSAGE = "You will flush all objects trashed. Only objects sync in dev, preprod and prod will be delete. So, clean dev, preprod and prod must be execute before.";
        public const string K_CLEAN_ALERT_OK = "Clean";
        public const string K_CLEAN_ALERT_CANCEL = "Cancel";
        //-------------------------------------------------------------------------------------------------------------
        // Idemobi alert Strings
        public const string K_ALERT_IDEMOBI_TITLE = "NetWorkedData";
        public const string K_ALERT_IDEMOBI_MESSAGE = "NetWorkedData is an idéMobi module to create and manage datas in your game. You  can create your owner classes and manage like standard NetWorkedDatas";
        public const string K_ALERT_IDEMOBI_OK = "Thanks!";
        public const string K_ALERT_IDEMOBI_SEE_DOC = "See online docs";
        public const string K_ALERT_IDEMOBI_DOC_HTTP = "http://www.idemobi.com/networkeddata";
        //		public const string K_ALERT_IDEMOBI_DOC_HTTP = 				"http://idemobi.com/networkeddata/";
        //-------------------------------------------------------------------------------------------------------------
        // Menu Strings

        public const string K_MENU_BASE = "NetWorkedDatas/";
        public const string K_MENU_IDEMOBI = K_MENU_BASE + "Developed by ideMobi";
        public const string K_MENU_EDITOR_PREFERENCES = K_MENU_BASE + "Editor preferences";
        public const string K_MENU_EDITOR_NODAL = K_MENU_BASE + "Editor Nodal";
        public const string K_BUTTON_EDITOR_NODAL = "Nodal view";
        public const string K_MENU_EDITOR_NEWCLASS = K_MENU_BASE + "Create New NWDBasis Class";
        public const string K_MENU_EDITOR_NEWWINDOW = K_MENU_BASE + "Create New Window NWD management";

        public const string K_MENU_ENVIRONMENT_EDIT = K_MENU_BASE + "Environment configurations";
        public const string K_MENU_ENVIRONMENT = K_MENU_BASE + "Environment player chooser";
        public const string K_MENU_ENVIRONMENT_SYNC = K_MENU_BASE + "Environment synchronize";
        public const string K_MENU_GAME = K_MENU_BASE + "App's configurations";
        public const string K_MENU_ALL_CLASSES = K_MENU_BASE + "All Data's Classes (herited from NWDBasis<K>)";

        public const string K_MENU_CREATE_PHP = "Creation of files for server";
        public const string K_MENU_CREATE_PHP_FILES = K_MENU_BASE + K_MENU_CREATE_PHP + "/Create PHP Files";
        public const string K_MENU_CREATE_PHP_EXPORT_WEB_SITE = K_MENU_BASE + K_MENU_CREATE_PHP + "/Export website(s)";

        public const string K_MENU_CREATE_CSHARP = "Creation of  files of workflow";
        public const string K_MENU_CREATE_CSHARP_FILES = K_MENU_BASE + K_MENU_CREATE_CSHARP + "/Create CSharp Files Workflow";

        public const string K_MENU_LOCALIZATION = "Localization";
        public const string K_MENU_LOCALIZATION_CONFIG = K_MENU_BASE + K_MENU_LOCALIZATION + "/Localization configuration";
        public const string K_MENU_LOCALIZATION_REORDER = K_MENU_BASE + K_MENU_LOCALIZATION + "/Reorder language in all objects";
        public const string K_MENU_LOCALIZATION_EXPORT = K_MENU_BASE + K_MENU_LOCALIZATION + "/Export in csv";
        public const string K_MENU_LOCALIZATION_IMPORT = K_MENU_BASE + K_MENU_LOCALIZATION + "/Import from csv";

        public const string K_MENU_DEV = "Dev";
        public const string K_MENU_PREPROD = "Preprod";
        public const string K_MENU_PROD = "Prod";
        public const string K_MENU_LOCAL = "Local";

        public const string K_MENU_DEV_CREATE_TABLES = K_MENU_BASE + K_MENU_DEV + "/Update all server's tables";
        public const string K_MENU_DEV_SYNCHRONIZE_DATAS = K_MENU_BASE + K_MENU_DEV + "/Synchronize datas on server";
        public const string K_MENU_DEV_FORCE_SYNCHRONIZE = K_MENU_BASE + K_MENU_DEV + "/Force synchronization on server";
        public const string K_MENU_DEV_RESET_CONNEXION = K_MENU_BASE + K_MENU_DEV + "/Reset connection with server";
        public const string K_MENU_DEV_FLUSH_CONNEXION = K_MENU_BASE + K_MENU_DEV + "/Flush Web queue";

        public const string K_MENU_PREPROD_CREATE_TABLES = K_MENU_BASE + K_MENU_PREPROD + "/Update all server's tables";
        public const string K_MENU_PREPROD_SYNCHRONIZE_DATAS = K_MENU_BASE + K_MENU_PREPROD + "/Synchronize datas on server";
        public const string K_MENU_PREPROD_FORCE_SYNCHRONIZE = K_MENU_BASE + K_MENU_PREPROD + "/Force synchronization on server";
        public const string K_MENU_PREPROD_RESET_CONNEXION = K_MENU_BASE + K_MENU_PREPROD + "/Reset connection with server";
        public const string K_MENU_PREPROD_FLUSH_CONNEXION = K_MENU_BASE + K_MENU_PREPROD + "/Flush Web queue";

        public const string K_MENU_PROD_CREATE_TABLES = K_MENU_BASE + K_MENU_PROD + "/Update all server's tables";
        public const string K_MENU_PROD_SYNCHRONIZE_DATAS = K_MENU_BASE + K_MENU_PROD + "/Synchronize datas on server";
        public const string K_MENU_PROD_FORCE_SYNCHRONIZE = K_MENU_BASE + K_MENU_PROD + "/Force synchronization on server";
        public const string K_MENU_PROD_RESET_CONNEXION = K_MENU_BASE + K_MENU_PROD + "/Reset connection with server";
        public const string K_MENU_PROD_FLUSH_CONNEXION = K_MENU_BASE + K_MENU_PROD + "/Flush Web queue";

        public const string K_MENU_LOCAL_CREATE_TABLES = K_MENU_BASE + K_MENU_LOCAL + "/Create all tables on local";
        public const string K_MENU_LOCAL_RELOAD_DATAS = K_MENU_BASE + K_MENU_LOCAL + "/Reload local datas on local";
        public const string K_MENU_LOCAL_CLEAN_TRASHED_DATAS = K_MENU_BASE + K_MENU_LOCAL + "/Clean Trashed in all tables datas on local";
        public const string K_MENU_LOCAL_RESET_DATAS = K_MENU_BASE + K_MENU_LOCAL + "/Reset all tables datas on local";
        public const string K_MENU_LOCAL_UPDATE_DATAS = K_MENU_BASE + K_MENU_LOCAL + "/Update all tables datas on local";

        public const string K_MENU_BASIS_WINDOWS_MANAGEMENT = " management";

        public const string K_ENVIRONMENT_CHOOSER_VERSION_BUNDLE = "Version bundle";
        public const string K_ENVIRONMENT_CHOOSER_ACCOOUNT_REFERENCE = "Account Reference";
        public const string K_ENVIRONMENT_CHOOSER_ACCOOUNT_INTERNALKEY = "Account Internal Key";
        public const string K_ENVIRONMENT_CHOOSER_ACCOOUNT_SELECT = "Account Select";

        //-------------------------------------------------------------------------------------------------------------
        // App Configurations Strings
        public const string K_APP_CONFIGURATION_HELPBOX = "Project configuration for connection with server";
        public const string K_APP_CONFIGURATION_MENU_NAME = "Environments configurations";
        public const string K_APP_CONFIGURATION_DEV = "Development";
        public const string K_APP_CONFIGURATION_PREPROD = "PreProduction";
        public const string K_APP_CONFIGURATION_PROD = "Production";
        public const string K_APP_CONFIGURATION_SAVE_BUTTON = "Save configurations";
        public const string K_APP_CONFIGURATION_LANGUAGE_AREA = "Project's localization";
        public const string K_APP_CONFIGURATION_ENVIRONMENT_AREA = "Environment's configurations";
        public const string K_APP_CONFIGURATION_BUNDLENAMEE_AREA = "Bundle name's localization";
        public const string K_APP_CONFIGURATION_DEV_LOCALALIZATION_AREA = "Project localization";
        public const string K_APP_CONFIGURATION_DEV_LOCALALIZATION_CHOOSE = "Choose project localization";
        //-------------------------------------------------------------------------------------------------------------
        // App Enviromnent Strings
        public const string K_APP_ENVIRONMENT_MENU_NAME = "Environment configuration";
        //-------------------------------------------------------------------------------------------------------------
        // Basis Interface Strings
        public const string K_APP_BASIS_xxx = "";

        public const string K_APP_BASIS_NO_OBJECT = "no object";

        public const string K_APP_BASIS_ONE_OBJECT = "object";

        public const string K_APP_BASIS_X_OBJECTS = "objects";

        public const string K_APP_BASIS_INTEGRITY_IS_FALSE = "INTEGRITY IS FALSE";
        public const string K_APP_BASIS_INTEGRITY_HELPBOX = "Integrity of this object is false! Perhaps it's damaged or hacked. Becarefully check its data.";
        public const string K_APP_BASIS_INTEGRITY_REEVAL = "Click to reeval integrity";
        public const string K_APP_BASIS_INTEGRITY_WARNING = "Warning";
        public const string K_APP_BASIS_INTEGRITY_WARNING_MESSAGE = "Are-you sure to reeval integrity and then accept this object in your database?";
        public const string K_APP_BASIS_INTEGRITY_OK = "OK";
        public const string K_APP_BASIS_INTEGRITY_CANCEL = "CANCEL";

        public const string K_APP_BASIS_IN_TRASH = "OBJECT IN TRASH";
        public const string K_APP_BASIS_IN_TRASH_HELPBOX = "This object is in trash! It'll be delete in the users' database as soon as possible. If you didn't synchronize you can untrash it without consequence!";
        public const string K_APP_BASIS_UNTRASH = "Click to untrash";
        public const string K_APP_BASIS_UNTRASH_WARNING = "Warning";
        public const string K_APP_BASIS_UNTRASH_WARNING_MESSAGE = "Are-you sure to untrash this object?";
        public const string K_APP_BASIS_UNTRASH_OK = "Yes";
        public const string K_APP_BASIS_UNTRASH_CANCEL = "No";

        public const string K_APP_BASIS_PREVIEW = "Preview";
        public const string K_APP_BASIS_INFORMATIONS = "Informations";
        public const string K_APP_BASIS_REFERENCE = "";
        public const string K_APP_BASIS_DC = "Created ";
        public const string K_APP_BASIS_DM = "Modified ";
        public const string K_APP_BASIS_Sync = "Sync ";
        public const string K_APP_BASIS_DevSync = "Sync Dev ";
        public const string K_APP_BASIS_PreprodSync = "Sync Preprod ";
        public const string K_APP_BASIS_ProdSync = "Sync Prod ";
        public const string K_APP_BASIS_xx = "trashed? ";
        public const string K_APP_BASIS_ac = "actived? ";
        public const string K_APP_BASIS_DUPPLICATE = "Dupplicate";
        public const string K_APP_BASIS_UPDATE = "Update";

        public const string K_APP_BASIS_DISACTIVED = "Disactived";
        public const string K_APP_BASIS_INACTIVED = "Inactived :";
        public const string K_APP_BASIS_REACTIVE = "ReActive";
        public const string K_APP_BASIS_ACTIVE = "Active";
        public const string K_APP_BASIS_DISACTIVE = "Disactive";

        public const string K_APP_BASIS_PREVIEW_GAMEOBJECT = "Preview GameObject";
        public const string K_APP_BASIS_INTERNAL_KEY = "Internal Key";
        public const string K_APP_BASIS_INTERNAL_DESCRIPTION = "Internal Description";
        public const string K_APP_BASIS_INTERNAL_TAG = "Internal Tag";

        public const string K_WARNING = "WARNING ";
        public const string K_NOT_IN_RESOURCES_FOLDER = "NOT IN \"Resources\"";
        public const string K_APP_BASIS_ASSET_MUST_BE_DOWNLOAD = "Asset must be download";
        public const string K_APP_BASIS_ASSET_DELETE = "Delete";
        public const string K_APP_BASIS_REFERENCE_ERROR = "Reference error";
        public const string K_APP_BASIS_REFERENCE_LIST_ERROR = "Reference(s) error";
        public const string K_APP_BASIS_REFERENCE_QUANTITY_ERROR = "Reference(s) error";
        public const string K_APP_BASIS_REFERENCE_CLEAN = "Clean";

        public const string K_APP_BASIS_TRASH_ZONE = "TRASH ZONE";
        public const string K_APP_BASIS_ACTION_ZONE = "ACTION ZONE";
        public const string K_APP_BASIS_PUT_IN_TRASH = "Trash-it";
        public const string K_APP_BASIS_PUT_IN_TRASH_WARNING = "Warning";
        public const string K_APP_BASIS_PUT_IN_TRASH_MESSAGE = "Do you want to trash this object?";
        public const string K_APP_BASIS_PUT_IN_TRASH_OK = "Trash it";
        public const string K_APP_BASIS_PUT_IN_TRASH_CANCEL = "Cancel";

        public const string K_APP_BASIS_WARNING_ZONE = "WARNING ZONE";
        public const string K_APP_BASIS_DELETE = "Local delete";
        public const string K_APP_BASIS_DELETE_WARNING = "Warning";
        public const string K_APP_BASIS_DELETE_MESSAGE = "Do you want to delete this object?";
        public const string K_APP_BASIS_DELETE_OK = "Delete it";
        public const string K_APP_BASIS_DELETE_CANCEL = "Cancel";

        public const string K_APP_BASIS_NEW_REFERENCE = "New reference";
        public const string K_APP_BASIS_NEW_REFERENCE_WARNING = "Warning";
        public const string K_APP_BASIS_NEW_REFERENCE_MESSAGE = "Do you want to change the reference of this object?";
        public const string K_APP_BASIS_NEW_REFERENCE_OK = "Change it's reference";
        public const string K_APP_BASIS_NEW_REFERENCE_CANCEL = "Cancel";

        public const string K_APP_BASIS_INSPECTOR_FOLDOUT = "See form";
        public const string K_APP_BASIS_INSPECTOR_FOLDOUT_CLOSE = "Hidde form";

        public const string K_APP_BASIS_INTEGRITY_VALUE = "Integrity value";

        public const string K_APP_BASIS_CLASS_DESCRIPTION = "Description";
        public const string K_APP_BASIS_CLASS_DEV = "Dev Environment";
        public const string K_APP_BASIS_CLASS_PREPROD = "PreProd Environment";
        public const string K_APP_BASIS_CLASS_PROD = "Prod Environment";
        public const string K_APP_BASIS_CLASS_SYNC_FORCE = "Force All";
        public const string K_APP_BASIS_CLASS_SYNC = "Synchronize table";
        public const string K_APP_BASIS_CLASS_DATAS = " Datas";
        public const string K_APP_BASIS_CLASS_SYNC_ALL_DATAS = "Synchronize ALL Datas";
        public const string K_APP_BASIS_CLASS_WARNING_ZONE = "WARNING SETTINGS";
        public const string K_APP_BASIS_CLASS_WARNING_HELPBOX = "Change these settings can detroyed all compatibilities with your actual game distribution! Use carrefully!";
        public const string K_APP_BASIS_CLASS_RESET_TABLE = "RESET TABLE";
        public const string K_APP_BASIS_CLASS_FIRST_SALT = "First Salt";
        public const string K_APP_BASIS_CLASS_SECOND_SALT = "Second Salt";
        public const string K_APP_BASIS_CLASS_REGENERATE = "Regenerate";
        public const string K_APP_BASIS_CLASS_SEE_WORKFLOW = "See Workflow Script";
        public const string K_APP_BASIS_CLASS_INTEGRITY_REEVALUE = "Integrity re-evaluate";
        public const string K_APP_BASIS_CLASS_PHP_GENERATE = "Generate PHP files for";
        public const string K_APP_BASIS_CLASS_CSHARP_GENERATE = "Generate C# files for";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_APP_TABLE_SEARCH_ZONE = "Search";
        public const string K_APP_TABLE_FILTER_ZONE = "Filters";
        public const string K_APP_TABLE_SHORTCUT_ZONE_A = "Tape 's' to select.";
        public const string K_APP_TABLE_SHORTCUT_ZONE_B = "Use arrows to navigate throw lines or pages.";
        public const string K_APP_TABLE_SHORTCUT_ZONE_C = "Use 'shift' + 'tab' to navigate throw tabs.";
        public const string K_APP_TABLE_SEARCH_REFERENCE = "Reference";
        public const string K_APP_TABLE_SEARCH_NAME = "Internal Key";
        public const string K_APP_TABLE_SEARCH_DESCRIPTION = "Internal Description";
        public const string K_APP_TABLE_SEARCH_TAG = "Internal Tag";
        public const string K_APP_TABLE_SEARCH_REMOVE_FILTER = "Remove filter";
        public const string K_APP_TABLE_SEARCH_FILTER = "Filter";
        public const string K_APP_TABLE_SEARCH_SORT = "Sort by name";
        public const string K_APP_TABLE_SEARCH_RELOAD = "Reload all datas";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_APP_TABLE_NO_SELECTED_OBJECT = "No selected object";
        public const string K_APP_TABLE_ONE_SELECTED_OBJECT = "1 selected object";
        public const string K_APP_TABLE_XX_SELECTED_OBJECT = " selected objects";
        public const string K_APP_TABLE_SELECT_ALL = "Select all";
        public const string K_APP_TABLE_DESELECT_ALL = "Deselect all";
        public const string K_APP_TABLE_INVERSE = "Inverse";
        public const string K_APP_TABLE_SELECT_ENABLED = "Select all enabled";
        public const string K_APP_TABLE_SELECT_DISABLED = "Select all disabled";
        public const string K_APP_TABLE_ACTIONS = "Actions";
        public const string K_APP_TABLE_REACTIVE = "Re-active";
        public const string K_APP_TABLE_DISACTIVE = "Disactive";
        public const string K_APP_TABLE_DUPPLICATE = "Dupplicate";
        public const string K_APP_TABLE_UPDATE = "Update";
        public const string K_APP_TABLE_DELETE_WARNING = "Warning";
        public const string K_APP_TABLE_DELETE_BUTTON = "Delete";
        public const string K_APP_TABLE_DELETE_NO_OBJECT = "No object to delete !?";
        public const string K_APP_TABLE_DELETE_ONE_OBJECT = "Do you want to delete this object? Delete object affect only the local database and not the servers and players. Loacl delete can be restaure exept forcing the sync. Prefer trash object to trash it everywhere";
        public const string K_APP_TABLE_DELETE_X_OBJECTS_A = "Do you want to delete these ";
        public const string K_APP_TABLE_DELETE_X_OBJECTS_B = " objects?";
        public const string K_APP_TABLE_DELETE_ALERT = "Warning";
        public const string K_APP_TABLE_DELETE_YES = "Yes";
        public const string K_APP_TABLE_DELETE_NO = "No";
        public const string K_APP_TABLE_TRASH_ZONE = "Trash";
        public const string K_APP_TABLE_TRASH_NO_OBJECT = "No object to put in trash !?";
        public const string K_APP_TABLE_TRASH_ONE_OBJECT = "Do you want to put in trash this object?";
        public const string K_APP_TABLE_TRASH_X_OBJECT_A = "Do you want to put in trash these ";
        public const string K_APP_TABLE_TRASH_X_OBJECT_B = " objects?";
        public const string K_APP_TABLE_TRASH_ALERT = "Warning";
        public const string K_APP_TABLE_TRASH_YES = "Yes";
        public const string K_APP_TABLE_TRASH_NO = "No";
        public const string K_APP_TABLE_PAGINATION = "Pagination";
        public const string K_APP_TABLE_NO_OBJECT = "No object in database";
        public const string K_APP_TABLE_ONE_OBJECT = "Only 1 object in database";
        public const string K_APP_TABLE_X_OBJECTS = " objects in database";
        public const string K_APP_TABLE_NO_OBJECT_FILTERED = "No object match";
        public const string K_APP_TABLE_ONE_OBJECT_FILTERED = "1 object in result";
        public const string K_APP_TABLE_X_OBJECTS_FILTERED = " objects in result";
        public const string K_APP_TABLE_TOOLS_ZONE = "Table Tools";
        public const string K_APP_TABLE_DATAS_ARE_LOADING_ZONE = "DATAS ARE LOADING";
        public const string K_APP_TABLE_SHOW_TOOLS = "Show tools";
        public const string K_APP_TABLE_ADD_ZONE = "New object";
        public const string K_APP_TABLE_ADD_ROW = "Add new object";

        public const string K_APP_TABLE_SHOW_ENABLE_DATAS = "Enable datas";
        public const string K_APP_TABLE_SHOW_DISABLE_DATAS = "Disable datas";
        public const string K_APP_TABLE_SHOW_TRASHED_DATAS = "Trashed datas";
        public const string K_APP_TABLE_SHOW_INTEGRITY_ERROR_DATAS = "Corrupted datas";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_APP_TABLE_HEADER_SELECT = " ";
        public const string K_APP_TABLE_HEADER_ID = "ID";
        public const string K_APP_TABLE_HEADER_PREFAB = "PrF";
        public const string K_APP_TABLE_HEADER_DESCRIPTION = "Description";
        public const string K_APP_TABLE_HEADER_SYNCHRO = "Sync";
        public const string K_APP_TABLE_HEADER_DEVSYNCHRO = "Dev";
        public const string K_APP_TABLE_HEADER_PREPRODSYNCHRO = "PrProd";
        public const string K_APP_TABLE_HEADER_PRODSYNCHRO = "Prod";
        public const string K_APP_TABLE_HEADER_STATUT = "Statut";
        public const string K_APP_TABLE_HEADER_REFERENCE = "Reference";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_APP_TABLE_ROW_OBJECT_OK = " ";
        public const string K_APP_TABLE_ROW_OBJECT_ERROR = "Error";
        public const string K_APP_TABLE_ROW_OBJECT_INTEGRITY_ERROR = "Corrupted!";
        public const string K_APP_TABLE_ROW_OBJECT_WEBSERVICE_ERROR = "OverGrade";
        public const string K_APP_TABLE_ROW_OBJECT_TRASH = "Trashed";
        public const string K_APP_TABLE_ROW_OBJECT_DISACTIVE = "Desactived";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_APP_CONNEXION_EDIT = "edit";
        public const string K_APP_CONNEXION_NEW = "new";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_EDITOR_NODE_WINDOW_TITLE = "Nodal view";
        public const string K_EDITOR_NODE_CHOOSE_LANGUAGE =  "Choose language";
        public const string K_EDITOR_NODE_ONLY_USED_PROPERTIES = "Only used properties";
        public const string K_EDITOR_NODE_GROUP_PROPERTIES = "Group properties";
        public const string K_EDITOR_NODE_ANALYZE_NONE = "Analyze nothing";
        public const string K_EDITOR_NODE_ANALYZE_ALL = "Analyze everything";
        public const string K_EDITOR_NODE_SHOW_SELECTED_OBJECT = "Show Selected Object";
        public const string K_EDITOR_NODE_MASK_ALL = "Show nothing";
        public const string K_EDITOR_NODE_SHOW_ALL = "Show everything";
        public const string K_EDITOR_NODE_LIST = "Classes show/analyze/new";
        //-------------------------------------------------------------------------------------------------------------
        public const string K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE = "ALERT";
        public const string K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE = "You are in playing mode, all sync not effective!";
        public const string K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK = "OK";
        //-------------------------------------------------------------------------------------------------------------

        public static Color K_RED_BUTTON_COLOR = new Color(0.9F, 0.7F, 0.7F, 1.0F); // invert color from white to fusion over button

        // todo test the color of button
        public static Color K_GREEN_BUTTON_COLOR = new Color(0.7F, 0.9F, 0.7F, 1.0F); // invert color from white to fusion over button
        public static Color K_BLUE_BUTTON_COLOR = new Color(0.7F, 0.7F, 0.9F, 1.0F); // invert color from white to fusion over button
        public static Color K_ORANGE_BUTTON_COLOR = new Color(0.9F, 0.8F, 0.7F, 1.0F); // invert color from white to fusion over button
        public static Color K_GRAY_BUTTON_COLOR = new Color(0.9F, 0.9F, 0.9F, 1.0F); // invert color from white to fusion over button
        public static Color K_DARKGRAY_BUTTON_COLOR = new Color(0.7F, 0.7F, 0.7F, 1.0F); // invert color from white to fusion over button
        //-------------------------------------------------------------------------------------------------------------
        public static string[] K_VERSION_MAJOR_ARRAY = new string[] {
            "0", "1", "2", "3", "4", "5", "6", "7", "8", "9"
        };
        //-------------------------------------------------------------------------------------------------------------
        public static string[] K_VERSION_MINOR_ARRAY = new string[] {
            "00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
            "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
            "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
            "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
            "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
            "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
            "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
            "70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
            "80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
            "90", "91", "92", "93", "94", "95", "96", "97", "98", "99"
        };
        //-------------------------------------------------------------------------------------------------------------
        public static string[] K_VERSION_BUILD_ARRAY = new string[] {
            "00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
            "10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
            "20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
            "30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
            "40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
            "50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
            "60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
            "70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
            "80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
            "90", "91", "92", "93", "94", "95", "96", "97", "98", "99"
        };
        //-------------------------------------------------------------------------------------------------------------
        //public static string K_NWD_WS_BUILD = "NWD_WS_BUILD";
        //-------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------

#if UNITY_EDITOR
        // images
        static public Texture2D kImageShortcutsToolsEditor = null;

        static public Texture2D kImageSelectionUpdate = null;
        static public Texture2D kImageMarkupUpdate = null;
        static public Texture2D kImageDelete = null;
        static public Texture2D kImageUp = null;
        static public Texture2D kImageDown = null;
        static public Texture2D kImageUpdate = null;
        static public Texture2D kImageMove = null;
        static public Texture2D kImageSelection = null;
        static public Texture2D kImageMarkup = null;
        static public Texture2D kImageMarkupGizmos = null;
        static public Texture2D kImageAction = null;
        static public Texture2D kImageScene = null;
        static public Texture2D kImageCamera = null;
        static public Texture2D kImageCameraPosition = null;
        static public Texture2D kImageOrthographic = null;
        static public Texture2D kImagePerspective = null;
        static public Texture2D kImageTwoDimension = null;
        static public Texture2D kImageEdit = null;
        static public Texture2D kImageNew = null;
        static public Texture2D kImageTabOptions = null;
        static public Texture2D kImageTabCreate = null;
        static public Texture2D kImageTabEdit = null;
        static public Texture2D kImageTabReduce = null;

        static public Texture2D kImageNodalCard = null;


        static public Texture2D kImageBezierTexture = null;
        //-------------------------------------------------------------------------------------------------------------
        // Styles and dimensions for GUI
        static public GUIStyle StyleButton;
        static public float HeightButton;
        static public GUIStyle StyleMiniButton;
        static public float HeightMiniButton;

        static public float SizeSlider = 18.0F;
        static public float Indent = 3.0F;
        static public float CharWidth = 7.0F;
        static public float BoxMarge = 18.0F;
        static public float ElementMarge = 6.0F;
        static public Color GUIBackgroundColor;
        static public int kMaxTabInbasisWindow = 16;
        //= GUI.backgroundColor;
        static public Color GUIContentColor;
        // = GUI.contentColor;

        static private bool StyleLoaded = false;
        static private bool ImageLoaded = false;


        static public Color kNodeLineColor;
        static public Color kNodeOverLineColor;
        //-------------------------------------------------------------------------------------------------------------
        static public Color kIdentityColor;
        static public Color kPropertyColor;


        public static float kHeaderHeight = 20.0f;
        public static float kHeaderHeightSpace = 6.0f;
        public static float kHeaderLineStroke = 1.0f;
        public static Color kHeaderColorBackground = new Color (0.0f, 0.0f, 0.0f, 0.35f);
        public static Color kHeaderColorLine = new Color (0.0f, 0.0f, 0.0f, 0.55f);
        //-------------------------------------------------------------------------------------------------------------
        // Row design
        public static float kRowOutMarge = 25.0f;
        public static float kRowHeight = 30.0f;
        public static float kRowHeightImage = 20.0f;
        public static float kRowHeightSpace = 5.0f;
        //static Color kRowColorNormal = new Color (0.0f, 0.0f, 0.0f, 0.30f);
        public static Color kRowColorSelected = new Color (0.55f, 0.55f, 1.00f, 0.25f);
        public static Color kRowColorError = new Color (1.00f, 0.00f, 0.00f, 0.55f);
        public static Color kRowColorWarning = new Color(1.00f, 0.50f, 0.00f, 0.55f);
        public static Color kRowColorTrash = new Color (0.00f, 0.00f, 0.00f, 0.45f);
        public static Color kRowColorDisactive = new Color (0.00f, 0.00f, 0.00f, 0.35f);
        public static float kRowLineStroke = 1.0f;
        public static Color kRowColorLine = new Color (0.0f, 0.0f, 0.0f, 0.25f);
        //-------------------------------------------------------------------------------------------------------------
        // Columns Size
        public static float kOriginWidth = 1.0f;
        public static float kSelectWidth = 20.0f;
        public static float kIDWidth = 45.0f;
        public static float kPrefabWidth = 30.0f;
        public static float kDescriptionMinWidth = 200.0f;
        public static float kSyncWidth = 40.0f;
        public static float kDevSyncWidth = 40.0f;
        public static float kPreprodSyncWidth = 40.0f;
        public static float kProdSyncWidth = 40.0f;
        public static float kActiveWidth = 70.0f;
        public static float kReferenceWidth = 230.0f;
        //-------------------------------------------------------------------------------------------------------------
        // Icons for Sync
        public static Texture2D kImageRed = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDRed.psd"));
        public static Texture2D kImageGreen = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDGreen.psd"));
        public static Texture2D kImageOrange = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDOrange.psd"));
        public static Texture2D kImageForbidden = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDForbidden.psd"));
        public static Texture2D kImageForbiddenOrange = AssetDatabase.LoadAssetAtPath<Texture2D> (NWDFindPackage.PathOfPackage ("/NWDEditor/NWDNativeImages/NWDForbiddenOrange.psd"));
        public static Texture2D kImageEmpty = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/NWDEditor/NWDNativeImages/NWDEmpty.psd"));
        public static Texture2D kImageWaiting = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/NWDEditor/NWDNativeImages/NWDWaiting.psd"));
        public static Texture2D kImageWarning = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/NWDEditor/NWDNativeImages/NWDWarning.psd"));


        static public Color KTAB_BAR_BACK_COLOR = new Color(0.6f, 0.6f, 0.6f, 1.0f);
        static public Color KTAB_BAR_LINE_COLOR = new Color(0.6f, 0.6f, 0.6f, 1.0f);
        static public Color KTAB_BAR_HIGHLIGHT_COLOR = new Color(0.5f, 0.5f, 0.5f, 1.0f);



        static public GUIStyle kLabelStyle;
        static public GUIStyle kLabelRightStyle;
        static public GUIStyle kBoldLabelStyle;
        static public GUIStyle kPopupdStyle;
        static public GUIStyle kTextFieldStyle;
        static public GUIStyle kTextAreaStyle;
        static public GUIStyle kRedLabelStyle;
        static public GUIStyle kGrayLabelStyle;
        static public GUIStyle kPopupButtonStyle;
        static public GUIStyle kMiniButtonStyle;
        static public GUIStyle kDeleteButtonStyle;
        static public GUIStyle kSeparatorStyle;
        //-------------------------------------------------------------------------------------------------------------
        static NWDConstants()
        {
            //Debug.Log("STATIC STEConstants constructor");
            //LoadStyles (); // Must be call from GUI
            LoadImages();
            //Memorize color
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void LoadStyles()
        {
            //Debug.Log ("STATIC STEConstants LoadStyles()");
            if (StyleLoaded == false)
            {
                StyleLoaded = true;

                GUIBackgroundColor = GUI.backgroundColor;
                GUIContentColor = GUI.contentColor;

                StyleButton = new GUIStyle(GUI.skin.button);
                StyleButton.alignment = TextAnchor.MiddleLeft;
                StyleButton.padding = new RectOffset(4, -2, 2, 2);
                StyleButton.wordWrap = true;
                //StyleButton.richText = true;

                StyleMiniButton = new GUIStyle(GUI.skin.button);
                StyleMiniButton.padding = new RectOffset(2, 2, 3, 3);

                HeightButton = StyleButton.CalcHeight(new GUIContent("-"), 100.0f);
                HeightMiniButton = StyleMiniButton.CalcHeight(new GUIContent("-"), 100.0f);

                HeightMiniButton = HeightButton;

                SizeSlider = Mathf.Min(HeightButton, HeightMiniButton);

                // used in form of inspector

                kLabelStyle = new GUIStyle(EditorStyles.label);
                kLabelStyle.fixedHeight = kLabelStyle.CalcHeight(new GUIContent("A"), 100.0F);

                kLabelRightStyle = new GUIStyle(EditorStyles.label);
                kLabelRightStyle.alignment = TextAnchor.MiddleRight;
                kLabelRightStyle.fixedHeight = kLabelRightStyle.CalcHeight(new GUIContent("A"), 100.0F);

                kBoldLabelStyle = new GUIStyle(EditorStyles.boldLabel);
                kBoldLabelStyle.fixedHeight = kBoldLabelStyle.CalcHeight(new GUIContent("A"), 100.0F);

                kPopupdStyle = new GUIStyle(EditorStyles.popup);
                kPopupdStyle.fixedHeight = kPopupdStyle.CalcHeight(new GUIContent("A"), 100.0F);

                kPopupButtonStyle = new GUIStyle(EditorStyles.miniButton);
                kPopupButtonStyle.fixedHeight = kPopupdStyle.fixedHeight; // use kPopupdStyle to fixe the good size of button from popup
                kPopupButtonStyle.padding = new RectOffset(2, 2, 2, 2);

                kTextFieldStyle = new GUIStyle(EditorStyles.textField);
                kTextFieldStyle.fixedHeight = kTextFieldStyle.CalcHeight(new GUIContent("A"), 100.0f);

                //kTextAreaStyle = new GUIStyle(EditorStyles.textArea);
                kTextAreaStyle = new GUIStyle(EditorStyles.textField);
                kTextAreaStyle.wordWrap = true;


                kRedLabelStyle = new GUIStyle(EditorStyles.label);
                kRedLabelStyle.fixedHeight = kRedLabelStyle.CalcHeight(new GUIContent("A"), 100.0F);
                kRedLabelStyle.normal.textColor = Color.red;

                kGrayLabelStyle = new GUIStyle(EditorStyles.label);
                kGrayLabelStyle.fontSize = 9;
                kGrayLabelStyle.fixedHeight = kGrayLabelStyle.CalcHeight(new GUIContent("A"), 100.0F);
                kGrayLabelStyle.normal.textColor = Color.gray;

                kMiniButtonStyle = new GUIStyle(EditorStyles.miniButton);
                kMiniButtonStyle.fixedHeight = kMiniButtonStyle.CalcHeight(new GUIContent("A"), 100.0F);

                kDeleteButtonStyle = new GUIStyle(EditorStyles.miniButton);
                kDeleteButtonStyle.fixedHeight = kDeleteButtonStyle.CalcHeight(new GUIContent("A"), 100.0F);

                kSeparatorStyle = new GUIStyle(EditorStyles.label);
                kSeparatorStyle.alignment = TextAnchor.MiddleCenter;
                kSeparatorStyle.fixedHeight = kSeparatorStyle.CalcHeight(new GUIContent("A"), 100.0F);
            }

            kNodeLineColor = new Color(1.0F, 1.0F, 1.0F, 0.40F);
            kNodeOverLineColor = new Color(1.0F, 1.0F, 1.0F, 0.70F);
            kIdentityColor = new Color(0.7f, 0.7f, 0.7f, 0.4f);
            kPropertyColor = new Color(0.8f, 0.8f, 0.8f, 0.3f);

            K_GREEN_BUTTON_COLOR = new Color(0.7F, 0.9F, 0.7F, 1.0F); // invert color from white to fusion over button
            K_BLUE_BUTTON_COLOR = new Color(0.7F, 0.7F, 0.9F, 1.0F); // invert color from white to fusion over button
            K_ORANGE_BUTTON_COLOR = new Color(0.9F, 0.8F, 0.7F, 1.0F); // invert color from white to fusion over button
            K_GRAY_BUTTON_COLOR = new Color(0.9F, 0.9F, 0.9F, 1.0F); // invert color from white to fusion over button
            K_DARKGRAY_BUTTON_COLOR = new Color(0.7F, 0.7F, 0.7F, 1.0F); // invert color from white to fusion over button



#if UNITY_EDITOR
            if ( EditorGUIUtility.isProSkin)
            {
                kIdentityColor = new Color(0.3f, 0.3f, 0.3f, 0.4f);
                kPropertyColor = new Color(0.2f, 0.2f, 0.2f, 0.2f);
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void LoadImages()
        {
            //Debug.Log("STATIC STEConstants LoadImages()");
           if (ImageLoaded == false)
            {
                ImageLoaded = true;
                if (kImageSelectionUpdate == null)
                {
                    kImageSelectionUpdate = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceSelectionUpdate.psd"));
                }
                if (kImageMarkupUpdate == null)
                {
                    kImageMarkupUpdate = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceMarkupUpdate.psd"));
                }
                if (kImageDelete == null)
                {
                    kImageDelete = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceDelete.psd"));
                }
                if (kImageUp == null)
                {
                    kImageUp = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceUp.psd"));
                }
                if (kImageDown == null)
                {
                    kImageDown = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceDown.psd"));
                }
                if (kImageMove == null)
                {
                    kImageMove = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceMove.psd"));
                }
                if (kImageUpdate == null)
                {
                    kImageUpdate = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceUpdate.psd"));
                }
                if (kImageShortcutsToolsEditor == null)
                {
                    kImageShortcutsToolsEditor = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceShorcutsToolsEditor.psd"));
                }
                if (kImageSelection == null)
                {
                    kImageSelection = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceSelection.psd"));
                }
                if (kImageMarkup == null)
                {
                    kImageMarkup = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceMarkup.psd"));
                }
                if (kImageMarkupGizmos == null)
                {
                    kImageMarkupGizmos = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceMarkupGizmos.psd"));
                }
                if (kImageAction == null)
                {
                    kImageAction = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceAction.psd"));
                }
                if (kImageScene == null)
                {
                    kImageScene = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceScene.psd"));
                }
                if (kImageCamera == null)
                {
                    kImageCamera = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceCamera.psd"));
                }
                if (kImageCameraPosition == null)
                {
                    kImageCameraPosition = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceCameraPosition.psd"));
                }
                if (kImageOrthographic == null)
                {
                    kImageOrthographic = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceOrthographic.psd"));
                }
                if (kImagePerspective == null)
                {
                    kImagePerspective = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfacePerspective.psd"));
                }
                if (kImageTwoDimension == null)
                {
                    kImageTwoDimension = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceTwoDimension.psd"));
                }
                if (kImageEdit == null)
                {
                    kImageEdit = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceEdit.psd"));
                }
                if (kImageNew == null)
                {
                    kImageNew = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceNew.psd"));
                }
                if (kImageTabOptions == null)
                {
                    kImageTabOptions = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceTabOptions.psd"));
                }
                if (kImageTabCreate == null)
                {
                    kImageTabCreate = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceTabCreate.psd"));
                }
                if (kImageTabEdit == null)
                {
                    kImageTabEdit = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceTabEdit.psd"));
                }
                if (kImageTabReduce == null)
                {
                    kImageTabReduce = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDInterfaceTabReduce.psd"));
                }
                if (kImageNodalCard == null)
                {
                    kImageNodalCard = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDCardBack.psd"));
                }
                if (kImageBezierTexture == null)
                {
                    kImageBezierTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(NWDFindPackage.PathOfPackage("/Editor/Resources/Textures/NWDBezierTexture.psd"));
                }
            }
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================