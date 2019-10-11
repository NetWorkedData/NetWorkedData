//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:10
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDNotificationConstants is class to register constants notification key.
    /// </summary>
	public partial class NWDNotificationConstants
    {
        //-------------------------------------------------------------------------------------------------------------
        // Launch NWD engine
        public const string K_ENGINE_LAUNCH = "K_ENGINE_LAUNCH_Bbf8ke4t"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        // connect to editor database
        public const string K_DB_EDITOR_READY = "K_DATABASE_EDITOR_READY_de4be4t"; // OK Needed by test & verify
        public const string K_DB_EDITOR_START_ASYNC_LOADING = "K_DB_EDITOR_START_ASYNC_LOADING_drez4t"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        // connect to account database
        public const string K_DB_ACCOUNT_PINCODE_REQUEST = "K_DATABASE_PROTECTION_REQUEST_defeet"; // OK Needed by test & verify
        public const string K_DB_ACCOUNT_PINCODE_SUCCESS = "K_DATABASE_PROTECTION_SUCCESS_de89e4t"; // OK Needed by test & verify
        public const string K_DB_ACCOUNT_PINCODE_FAIL = "K_DATABASE_PROTECTION_FAIL_lki5dt"; // OK Needed by test & verify
        public const string K_DB_ACCOUNT_PINCODE_STOP = "K_DATABASE_PROTECTION_STOP_dAe44t"; // OK Needed by test & verify
        public const string K_DB_ACCOUNT_PINCODE_NEEDED = "K_DATABASE_PROTECTION_NEED_PINCODE_lki5dt"; // OK Needed by test & verify

        public const string K_DB_ACCOUNT_READY = "K_DATABASE_ACCOUNT_READY_de4be4t"; // OK Needed by test & verify
        public const string K_DB_ACCOUNT_START_ASYNC_LOADING = "K_DB_ACCOUNT_START_ASYNC_LOADING_dee48yu"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        // Load datas
        public const string K_DATA_EDITOR_START_LOADING = "K_DATAS_EDITOR_START_LOADING_Mr524ztr"; // OK Needed by test & verify
        public const string K_DATA_EDITOR_PARTIAL_LOADED = "K_DATAS_EDITOR_PARTIAL_LOADED_fZt875df"; // OK Needed by test & verify
        public const string K_DATA_EDITOR_LOADED = "K_DATAS_EDITOR_LOADED_er468rez"; // OK Needed by test & verify

        public const string K_DATA_ACCOUNT_START_LOADING = "K_DATAS_ACCOUNT_START_LOADING_Mr524ztr"; // OK Needed by test & verify
        public const string K_DATA_ACCOUNT_PARTIAL_LOADED = "K_DATAS_ACCOUNT_PARTIAL_LOADED_fZt875df"; // OK Needed by test & verify
        public const string K_DATA_ACCOUNT_LOADED = "K_DATAS_ACCOUNT_LOADED_er468rez"; // OK Needed by test & verify
        // global load
        public const string K_DATA_START_LOADING = "K_DATAS_START_LOADING_M7374ztr"; // OK Needed by test & verify
        public const string K_DATA_PARTIAL_LOADED = "K_DATAS_PARTIAL_LOADED_f77475df"; // OK Needed by test & verify
        public const string K_DATA_LOADED = "K_DATAS_LOADED_er47478z"; // OK Needed by test & verify
        // indexation
        public const string K_INDEXATION_START_ASYNC = "K_DB_INDEXATION_START_ASYNC_drez4t"; // OK Needed by test & verify
        public const string K_INDEXATION_START = "K_INDEXATION_START_er668rez"; // OK Needed by test & verify
        public const string K_INDEXATION_STEP = "K_INDEXATION_STEP_er4678ez"; // OK Needed by test & verify
        public const string K_INDEXATION_FINISH = "K_INDEXATION_FINISH_e7868rez"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        public const string K_ENGINE_READY = "K_ENGINE_READY_er468rez"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        //public const string K_EDITOR_REFRESH = "K_EDITOR_REFRESH_g54D55hs"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        // Change language in game
        public const string K_LANGUAGE_CHANGED = "K_LANGUAGE_CHANGED"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        // Datas modification from local
        public const string K_DATA_LOCAL_INSERT = "K_DATA_LOCAL_INSERTD_eBf75rez";  // OK Needed by test & verify
        public const string K_DATA_LOCAL_UPDATE = "K_DATA_LOCAL_UPDATE_eDe24rez";  // OK Needed by test & verify
        public const string K_DATA_LOCAL_DELETE = "K_DATA_LOCAL_DELETE_Ur4rerez";  // OK Needed by test & verify
        // Datas modification from web
        public const string K_DATAS_WEB_UPDATE = "K_DATAS_WEB_UPDATE_LOe245rz"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        // receipt Error message
        public const string K_ERROR = "K_ERROR_Okk4ez6S";
        public const string K_MESSAGE = "K_MESSAGE_77d4OkzS";
        //-------------------------------------------------------------------------------------------------------------
        // player/user change
        public const string K_ACCOUNT_CHANGE = "K_ACCOUNT_CHANGE_jhGe45di";// OK Needed by test & verify
        public const string K_ACCOUNT_SESSION_EXPIRED = "K_ACCOUNT_SESSION_EXPIRED_Kiue44Q9"; // OK Needed by test & verify
        public const string K_ACCOUNT_BANNED = "K_ACCOUNT_BANNED_o4dH1dKJ"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        // Web operation
        public const string K_WEB_OPERATION_ERROR = "K_OPERATION_WEB_ERROR_Oz6kk4eS"; // OK Needed by test & verify
        public const string K_WEB_OPERATION_UPLOAD_START = "K_OPERATION_WEB_UPLOAD_START_ee8yd4e"; // OK Needed by test & verify
        public const string K_WEB_OPERATION_UPLOAD_IN_PROGRESS = "K_OPERATION_WEB_UPLOAD_IN_PROGRESS_geuydhfe"; // OK Needed by test & verify
        public const string K_WEB_OPERATION_DOWNLOAD_IN_PROGRESS = "K_OPERATION_WEB_DOWNLOAD_IN_PROGRESS_ujehdtss"; // OK Needed by test & verify
        public const string K_WEB_OPERATION_DOWNLOAD_IS_DONE = "K_OPERATION_WEB_DOWNLOAD_IS_DONE_perleifhgss"; // OK Needed by test & verify
        public const string K_WEB_OPERATION_DOWNLOAD_FAILED = "K_OPERATION_WEB_DOWNLOAD_FAILED_iekdjyft"; // OK Needed by test & verify
        public const string K_WEB_OPERATION_DOWNLOAD_ERROR = "K_OPERATION_WEB_DOWNLOAD_ERROR_pemqtdfa"; // OK Needed by test & verify
        public const string K_WEB_OPERATION_DOWNLOAD_SUCCESSED = "K_OPERATION_WEB_DOWNLOAD_SUCCESSED_ruaashg4"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        // Network statut
        public const string K_NETWORK_OFFLINE = "K_NETWORK_OFFLINE_ds87s744r"; // OK Needed by test & verify
        public const string K_NETWORK_ONLINE = "K_NETWORK_ONLINE_FHGCve4e"; // OK Needed by test & verify
        public const string K_NETWORK_UNKNOW = "K_NETWORK_UNKNOW_za9a4z77"; // OK Needed by test & verify
        public const string K_NETWORK_CHECK = "K_NETWORK_CHECK_Ag8a8aq4"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        // generic notification
        public const string K_NOTIFICATION_KEY = "K_NOTIFICATION_KEY_87zffzer"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        public const string K_NEWS_NOTIFICATION = "K_NEWS_NOTIFICATION_ezr8e4t"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================