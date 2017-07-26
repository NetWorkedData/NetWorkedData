using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDConstants
	{
		//-------------------------------------------------------------------------------------------------------------
		public static string K_DEVELOPMENT = 						"Development";
		public static string K_PREPRODUCTION = 						"PreProduction";
		public static string K_PRODUCTION = 						"Production";
		//-------------------------------------------------------------------------------------------------------------
		public static string K_DEVELOPMENT_NAME = 					"dev";
		public static string K_PREPRODUCTION_NAME = 				"preprod";
		public static string K_PRODUCTION_NAME = 					"prod";
		//-------------------------------------------------------------------------------------------------------------
		public static string K_APP_CHOOSER_ENVIRONMENT = 			"Select Environment used in Player Mode (Game panel)";
		public static string K_APP_CHOOSER_ENVIRONMENT_TITLE = 		"Environment chooser";
		public static string K_APP_SYNC_ENVIRONMENT = 				"Synchronize your datas in the good environment";
		public static string K_APP_SYNC_ENVIRONMENT_TITLE = 		"Environment synchronize";
		public static string K_VERSION_LABEL = 						"Version";
		//-------------------------------------------------------------------------------------------------------------
		public static string kStandardSeparator = 					"|";
		public static string kStandardSeparatorSubstitute = 		"@0#";
		//-------------------------------------------------------------------------------------------------------------
		public static string kFieldSeparatorA = 					"•";
		public static string kFieldSeparatorB = 					":";
		public static string kFieldSeparatorC = 					"_";
		public static string kFieldSeparatorASubstitute = 			"@1#";
		public static string kFieldSeparatorBSubstitute = 			"@2#";
		public static string kFieldSeparatorCSubstitute = 			"@3#";
		//-------------------------------------------------------------------------------------------------------------
		public static string kAlertSaltShortError = 				"ALERT SALT ARE NOT MEMORIZE : RECCORD CONFIGURATIONS AND RECOMPILE!";
		public static float kFieldMarge = 							5.0f;
		public static float kPrefabSize = 							40.0f;
		public static float kIntWidth = 							36.0f;
		public static float kEditWidth = 							12.0f;
		public static float kLangWidth = 							200.0f;
		//-------------------------------------------------------------------------------------------------------------
		// NetWorked synchronize alert
		public const string K_SYNC_ALERT_TITLE = 					"ALERT PRODUCTION";
		public const string K_SYNC_ALERT_MESSAGE = 					"YOU WILL SYNCHRONIZE ON THE PRODUCTION ENVIRONEMENT";
		public const string K_SYNC_ALERT_OK = 						"Ok";
		public const string K_SYNC_ALERT_CANCEL = 					"Cancel";
		//-------------------------------------------------------------------------------------------------------------
		// Idemobi alert Strings
		public const string K_ALERT_IDEMOBI_TITLE = 				"NetWorkedData";
		public const string K_ALERT_IDEMOBI_MESSAGE = 				"NetWorkedData is an idéMobi module to create and manage datas in your game. You  can create your owner classes and manage like standard NetWorkedDatas";
		public const string K_ALERT_IDEMOBI_OK = 					"Thanks!";
		public const string K_ALERT_IDEMOBI_SEE_DOC = 				"See online docs";
		public const string K_ALERT_IDEMOBI_DOC_HTTP = 				"http://www.idemobi.com/networkeddata";
//		public const string K_ALERT_IDEMOBI_DOC_HTTP = 				"http://idemobi.com/networkeddata/";
		//-------------------------------------------------------------------------------------------------------------
		// Menu Strings
		public const string K_MENU_BASE = 							"NetWorkedDatas/";
		public const string K_MENU_IDEMOBI = 						K_MENU_BASE+"Developped by ideMobi";

		public const string K_MENU_EDITOR_PREFERENCES = 			K_MENU_BASE+"Editor preferences";
		public const string K_MENU_EDITOR_NEWCLASS = 				K_MENU_BASE+"Create New NWD Data Class";
		public const string K_MENU_EDITOR_NEWWINDOW = 				K_MENU_BASE+"Create New Window of NWD Classes";

		public const string K_MENU_ENVIRONMENT_EDIT = 				K_MENU_BASE+"Environment configurations";
		public const string K_MENU_ENVIRONMENT = 					K_MENU_BASE+"Environment player chooser";
		public const string K_MENU_ENVIRONMENT_SYNC = 				K_MENU_BASE+"Environment synchronize";
		public const string K_MENU_GAME = 							K_MENU_BASE+"Game's configurations";
		public const string K_MENU_ALL_CLASSES = 					K_MENU_BASE+"All Data's Classes (herited from NWDBasis<K>)";

		public const string K_MENU_CREATE = 						"Creation of files/";
		public const string K_MENU_CREATE_PHP_FILES = 				K_MENU_BASE+K_MENU_CREATE+"Create PHP Files";
		public const string K_MENU_CREATE_PHP_EXPORT_WEB_SITE = 	K_MENU_BASE+K_MENU_CREATE+"Export website(s)";
		public const string K_MENU_CREATE_CSHARP_FILES = 			K_MENU_BASE+K_MENU_CREATE+"Create CSharp Files Workflow";

		public const string K_MENU_LOCALIZATION = 					"Localization/";
		public const string K_MENU_LOCALIZATION_CONFIG =			K_MENU_BASE+K_MENU_LOCALIZATION+"Localization configuration";
		public const string K_MENU_LOCALIZATION_REORDER =			K_MENU_BASE+K_MENU_LOCALIZATION+"Reorder language in all objects";
		public const string K_MENU_LOCALIZATION_EXPORT = 			K_MENU_BASE+K_MENU_LOCALIZATION+"Export in csv";
		public const string K_MENU_LOCALIZATION_IMPORT = 			K_MENU_BASE+K_MENU_LOCALIZATION+"Import from csv";

		public const string K_MENU_DEV = 							"Dev/";
		public const string K_MENU_PREPROD = 						"Preprod/";
		public const string K_MENU_PROD = 							"Prod/";
		public const string K_MENU_LOCAL = 							"Local/";

		public const string K_MENU_DEV_CREATE_TABLES = 				K_MENU_BASE+K_MENU_DEV+"Update all server's tables";
		public const string K_MENU_DEV_SYNCHRONIZE_DATAS = 			K_MENU_BASE+K_MENU_DEV+"Synchronize datas on server";
		public const string K_MENU_DEV_FORCE_SYNCHRONIZE = 			K_MENU_BASE+K_MENU_DEV+"Force synchronization on server";
		public const string K_MENU_DEV_RESET_CONNEXION = 			K_MENU_BASE+K_MENU_DEV+"Reset connexion with server";
		public const string K_MENU_DEV_FLUSH_CONNEXION = 			K_MENU_BASE+K_MENU_DEV+"Flush Web queue";

		public const string K_MENU_PREPROD_CREATE_TABLES = 			K_MENU_BASE+K_MENU_PREPROD+"Update all server's tables";
		public const string K_MENU_PREPROD_SYNCHRONIZE_DATAS = 		K_MENU_BASE+K_MENU_PREPROD+"Synchronize datas on server";
		public const string K_MENU_PREPROD_FORCE_SYNCHRONIZE = 		K_MENU_BASE+K_MENU_PREPROD+"Force synchronization on server";
		public const string K_MENU_PREPROD_RESET_CONNEXION = 		K_MENU_BASE+K_MENU_PREPROD+"Reset connexion with server";
		public const string K_MENU_PREPROD_FLUSH_CONNEXION = 		K_MENU_BASE+K_MENU_PREPROD+"Flush Web queue";

		public const string K_MENU_PROD_CREATE_TABLES = 			K_MENU_BASE+K_MENU_PROD+"Update all server's tables";
		public const string K_MENU_PROD_SYNCHRONIZE_DATAS = 		K_MENU_BASE+K_MENU_PROD+"Synchronize datas on server";
		public const string K_MENU_PROD_FORCE_SYNCHRONIZE = 		K_MENU_BASE+K_MENU_PROD+"Force synchronization on server";
		public const string K_MENU_PROD_RESET_CONNEXION = 			K_MENU_BASE+K_MENU_PROD+"Reset connexion with server";
		public const string K_MENU_PROD_FLUSH_CONNEXION = 			K_MENU_BASE+K_MENU_PROD+"Flush Web queue";

		public const string K_MENU_LOCAL_CREATE_TABLES =			K_MENU_BASE+K_MENU_LOCAL+"Create all tables on local";
		public const string K_MENU_LOCAL_RELOAD_DATAS = 			K_MENU_BASE+K_MENU_LOCAL+"Reload local datas on local";

		public const string K_MENU_BASIS_WINDOWS_MANAGEMENT = 		" management";

		//-------------------------------------------------------------------------------------------------------------
		// App Configurations Strings
		public const string K_APP_CONFIGURATION_HELPBOX  = 			"Project configuration for connexion with server";
		public const string K_APP_CONFIGURATION_MENU_NAME  = 		"Environments configurations";
		public const string K_APP_CONFIGURATION_DEV = 				"Development";
		public const string K_APP_CONFIGURATION_PREPROD =			"PreProduction";
		public const string K_APP_CONFIGURATION_PROD =				"Production";
		public const string K_APP_CONFIGURATION_SAVE_BUTTON = 		"Save configurations";
		public const string K_APP_CONFIGURATION_LANGUAGE_AREA = 	"Project's localization";
		public const string K_APP_CONFIGURATION_ENVIRONMENT_AREA = 	"Environment's configurations";
		//-------------------------------------------------------------------------------------------------------------
		// App Enviromnent Strings
		public const string K_APP_ENVIRONMENT_MENU_NAME = 			"Environment configuration";
		//-------------------------------------------------------------------------------------------------------------
		// Basis Interface Strings
		public const string K_APP_BASIS_xxx = 						"";

		public const string K_APP_BASIS_NO_OBJECT = 				"no object\n";

		public const string K_APP_BASIS_ONE_OBJECT = 				"object";

		public const string K_APP_BASIS_X_OBJECTS = 				"objects";

		public const string K_APP_BASIS_INTEGRITY_IS_FALSE = 		"INTEGRITY IS FALSE";
		public const string K_APP_BASIS_INTEGRITY_HELPBOX = 		"Integrity of this object is false! Perhaps it's damaged or hacked. Becarefully check its data.";
		public const string K_APP_BASIS_INTEGRITY_REEVAL = 			"Click to reaval integrity";
		public const string K_APP_BASIS_INTEGRITY_WARNING = 		"Warning";
		public const string K_APP_BASIS_INTEGRITY_WARNING_MESSAGE = "Are-you sure to reeval integrity and then accept this object in your database?";
		public const string K_APP_BASIS_INTEGRITY_OK = 				"OK";
		public const string K_APP_BASIS_INTEGRITY_CANCEL = 			"CANCEL";

		public const string K_APP_BASIS_IN_TRASH = 					"OBJECT IN TRASH";
		public const string K_APP_BASIS_IN_TRASH_HELPBOX = 			"This object is in trash! It'll be delete in the users' database as soon as possible. If you didn't synchronize you can untrash it without consequence!";
		public const string K_APP_BASIS_UNTRASH = 					"Click to untrash";
		public const string K_APP_BASIS_UNTRASH_WARNING = 			"Warning";
		public const string K_APP_BASIS_UNTRASH_WARNING_MESSAGE = 	"Are-you sure to untrash this object?";
		public const string K_APP_BASIS_UNTRASH_OK = 				"Yes";
		public const string K_APP_BASIS_UNTRASH_CANCEL= 			"No";

		public const string K_APP_BASIS_PREVIEW = 					"Preview";
		public const string K_APP_BASIS_INFORMATIONS = 				"Informations";
		public const string K_APP_BASIS_REFERENCE = 				"";
		public const string K_APP_BASIS_DC = 						"Created ";
		public const string K_APP_BASIS_DM = 						"Modified ";
		public const string K_APP_BASIS_DevSync = 					"Sync dev ";
		public const string K_APP_BASIS_PreprodSync = 				"Sync Preprod ";
		public const string K_APP_BASIS_ProdSync = 					"Sync Prod ";
		public const string K_APP_BASIS_DUPPLICATE = 				"Dupplicate";
		public const string K_APP_BASIS_UPDATE = 					"Update";

		public const string K_APP_BASIS_DISACTIVED = 				"Disactived";
		public const string K_APP_BASIS_INACTIVED = 				"Inactived :";
		public const string K_APP_BASIS_REACTIVE = 					"ReActive";
		public const string K_APP_BASIS_ACTIVE = 					"Active";
		public const string K_APP_BASIS_DISACTIVE = 				"Disactive";

		public const string K_APP_BASIS_PREVIEW_GAMEOBJECT = 		"Preview GameObject";
		public const string K_APP_BASIS_INTERNAL_KEY = 				"Internal Key";
		public const string K_APP_BASIS_INTERNAL_DESCRIPTION = 		"Internal Description";

		public const string K_APP_BASIS_TRASH_ZONE = 				"TRASH ZONE";
		public const string K_APP_BASIS_ACTION_ZONE = 				"ACTION ZONE";
		public const string K_APP_BASIS_PUT_IN_TRASH = 				"Trash-it";
		public const string K_APP_BASIS_PUT_IN_TRASH_WARNING =		"Warning";
		public const string K_APP_BASIS_PUT_IN_TRASH_MESSAGE =		"Do you want to trash this object?";
		public const string K_APP_BASIS_PUT_IN_TRASH_OK = 			"Trash it";
		public const string K_APP_BASIS_PUT_IN_TRASH_CANCEL = 		"Cancel";

		public const string K_APP_BASIS_WARNING_ZONE = 				"WARNING ZONE";
		public const string K_APP_BASIS_DELETE =					"Delete on local database";
		public const string K_APP_BASIS_DELETE_WARNING =			"Warning";
		public const string K_APP_BASIS_DELETE_MESSAGE =			"Do you want to delete this object?";
		public const string K_APP_BASIS_DELETE_OK = 				"Delete it";
		public const string K_APP_BASIS_DELETE_CANCEL = 			"Cancel";

		public const string K_APP_BASIS_NEW_REFERENCE =				"Regenerate Reference";
		public const string K_APP_BASIS_NEW_REFERENCE_WARNING =		"Warning";
		public const string K_APP_BASIS_NEW_REFERENCE_MESSAGE =		"Do you want to change the reference of this object?";
		public const string K_APP_BASIS_NEW_REFERENCE_OK = 			"Change it's reference";
		public const string K_APP_BASIS_NEW_REFERENCE_CANCEL = 		"Cancel";

		public const string K_APP_BASIS_INTEGRITY_VALUE = 			"Integrity value";

		public const string K_APP_BASIS_CLASS_DESCRIPTION = 		"Description";
		public const string K_APP_BASIS_CLASS_DEV = 				"Dev Environment";
		public const string K_APP_BASIS_CLASS_PREPROD = 			"PreProd Environment";
		public const string K_APP_BASIS_CLASS_PROD = 				"Prod Environment";
		public const string K_APP_BASIS_CLASS_SYNC_FORCE = 			"Force All";
		public const string K_APP_BASIS_CLASS_SYNC = 				"Synchronize table";
		public const string K_APP_BASIS_CLASS_DATAS = 				" Datas";
		public const string K_APP_BASIS_CLASS_SYNC_ALL_DATAS = 		"Synchronize ALL Datas";
		public const string K_APP_BASIS_CLASS_WARNING_ZONE = 		"WARNING SETTINGS";
		public const string K_APP_BASIS_CLASS_WARNING_HELPBOX = 	"Change these settings can detroyed all compatibilities with your actual game distribution! Use carrefully!";
		public const string K_APP_BASIS_CLASS_RESET_TABLE = 		"RESET TABLE";
		public const string K_APP_BASIS_CLASS_FIRST_SALT = 			"First Salt";
		public const string K_APP_BASIS_CLASS_SECOND_SALT = 		"Second Salt";
		public const string K_APP_BASIS_CLASS_REGENERATE = 			"Regenerate";
		public const string K_APP_BASIS_CLASS_SEE_WORKFLOW = 		"See Workflow Script";
		public const string K_APP_BASIS_CLASS_INTEGRITY_REEVALUE = 	"Integrity re-evaluate";
		public const string K_APP_BASIS_CLASS_PHP_GENERATE = 		"Generate PHP files for";
		public const string K_APP_BASIS_CLASS_CSHARP_GENERATE = 	"Generate C# files for";
		//-------------------------------------------------------------------------------------------------------------
		public const string K_APP_TABLE_SEARCH_ZONE = 				"Filter zone";
		public const string K_APP_TABLE_SHORTCUT_ZONE_A = "Tape 's' to select.";
		public const string K_APP_TABLE_SHORTCUT_ZONE_B = "Use arrows to navigate throw lines or pages.";
		public const string K_APP_TABLE_SHORTCUT_ZONE_C = "Use 'shift' + 'tab' to navigate throw tabs.";
		public const string K_APP_TABLE_SEARCH_NAME = 				"Internal name";
		public const string K_APP_TABLE_SEARCH_DESCRIPTION = 		"Intername description";
		public const string K_APP_TABLE_SEARCH_REMOVE_FILTER = 		"Remove filter";
		public const string K_APP_TABLE_SEARCH_FILTER = 			"Filter";
		public const string K_APP_TABLE_SEARCH_SORT = 				"Sort by name";
		public const string K_APP_TABLE_SEARCH_RELOAD = 			"Reload all datas";
		//-------------------------------------------------------------------------------------------------------------
		public const string K_APP_TABLE_NO_SELECTED_OBJECT = 		"No selected object";
		public const string K_APP_TABLE_ONE_SELECTED_OBJECT = 		"1 selected object";
		public const string K_APP_TABLE_XX_SELECTED_OBJECT = 		" selected objects";
		public const string K_APP_TABLE_SELECT_ALL = 				"Select all";
		public const string K_APP_TABLE_DESELECT_ALL = 				"Deselect all";
		public const string K_APP_TABLE_INVERSE = 					"Inverse";
		public const string K_APP_TABLE_SELECT_DISABLED = 			"Select Disabled";
		public const string K_APP_TABLE_ACTIONS =  					"Actions";
		public const string K_APP_TABLE_REACTIVE = 					"Re-active";
		public const string K_APP_TABLE_DISACTIVE = 				"Disactive";
		public const string K_APP_TABLE_DUPPLICATE = 				"Dupplicate";
		public const string K_APP_TABLE_UPDATE = 					"Update";
		public const string K_APP_TABLE_DELETE_WARNING = 			"Warning";
		public const string K_APP_TABLE_DELETE_BUTTON = 			"Delete";
		public const string K_APP_TABLE_DELETE_NO_OBJECT = 			"No object to delete !?";
		public const string K_APP_TABLE_DELETE_ONE_OBJECT = 		"Do you want to delete this object? Delete object affect only the local database and not the servers and players. Loacl delete can be restaure exept forcing the sync. Prefer trash object to trash it everywhere";
		public const string K_APP_TABLE_DELETE_X_OBJECTS_A = 		"Do you want to delete these ";
		public const string K_APP_TABLE_DELETE_X_OBJECTS_B = 		" objects?";
		public const string K_APP_TABLE_DELETE_ALERT = 				"Warning";
		public const string K_APP_TABLE_DELETE_YES = 				"Yes";
		public const string K_APP_TABLE_DELETE_NO = 				"No";
		public const string K_APP_TABLE_TRASH_ZONE = 				"Trash";
		public const string K_APP_TABLE_TRASH_NO_OBJECT = 			"No object to put in trash !?";
		public const string K_APP_TABLE_TRASH_ONE_OBJECT = 			"Do you want to put in trash this object?";
		public const string K_APP_TABLE_TRASH_X_OBJECT_A = 			"Do you want to put in trash these ";
		public const string K_APP_TABLE_TRASH_X_OBJECT_B = 			" objects?";
		public const string K_APP_TABLE_TRASH_ALERT = 				"Warning";
		public const string K_APP_TABLE_TRASH_YES = 				"Yes";
		public const string K_APP_TABLE_TRASH_NO = 					"No";
		public const string K_APP_TABLE_PAGINATION = 				"Pagination";
		public const string K_APP_TABLE_NO_OBJECT = 				"No object in database";
		public const string K_APP_TABLE_ONE_OBJECT = 				"Only 1 object in database";
		public const string K_APP_TABLE_X_OBJECTS = 				" objects in database";
		public const string K_APP_TABLE_NO_OBJECT_FILTERED =  		"No object match";
		public const string K_APP_TABLE_ONE_OBJECT_FILTERED =  		"1 object in result";
		public const string K_APP_TABLE_X_OBJECTS_FILTERED =  		" objects in result";
		public const string K_APP_TABLE_TOOLS_ZONE =  				"Table Tools";
		public const string K_APP_TABLE_SHOW_TOOLS=  				"Show tools";
		public const string K_APP_TABLE_ADD_ZONE =  				"New object";
		public const string K_APP_TABLE_ADD_ROW =  					"Add new object";
		//-------------------------------------------------------------------------------------------------------------
		public const string K_APP_TABLE_HEADER_SELECT = 			" ";
		public const string K_APP_TABLE_HEADER_ID = 				"ID";
		public const string K_APP_TABLE_HEADER_PREFAB =				"PrF";
		public const string K_APP_TABLE_HEADER_DESCRIPTION = 		"Description";
		public const string K_APP_TABLE_HEADER_SYNCHRO = 			"Sync";
		public const string K_APP_TABLE_HEADER_DEVSYNCHRO = 		"Dev";
		public const string K_APP_TABLE_HEADER_PREPRODSYNCHRO = 	"PrProd";
		public const string K_APP_TABLE_HEADER_PRODSYNCHRO = 		"Prod";
		public const string K_APP_TABLE_HEADER_STATUT = 			"Statut";
		public const string K_APP_TABLE_HEADER_REFERENCE = 			"Reference";
		//-------------------------------------------------------------------------------------------------------------
		public const string K_APP_TABLE_ROW_OBJECT_OK = 			" ";
		public const string K_APP_TABLE_ROW_OBJECT_ERROR = 			"Error";
		public const string K_APP_TABLE_ROW_OBJECT_TRASH = 			"Trashed";
		public const string K_APP_TABLE_ROW_OBJECT_DISACTIVE = 		"Desactived";
		//-------------------------------------------------------------------------------------------------------------
		public const string K_APP_CONNEXION_EDIT =					"edit";
		public const string K_APP_CONNEXION_NEW = 					"new";
		//-------------------------------------------------------------------------------------------------------------
		public static string[] K_VERSION_MAJOR_ARRAY = new string[] { 
			"0", "1", "2", "3", "4", "5", "6", "7", "8", "9" 
		};
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

		public static string K_NWD_WS_BUILD = "NWD_WS_BUILD";
	}
}
//=====================================================================================================================