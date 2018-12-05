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
        public const string K_DATABASE_CONNECTED = "K_DATABASE_CONNECTED_de4be4t"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        // Load datas
        public const string K_DATAS_START_LOADING = "K_DATAS_START_LOADING_Mr524ztr"; // OK Needed by test & verify
        public const string K_DATAS_PARTIAL_LOADED = "K_DATAS_PARTIAL_LOADED_fZt875df"; // OK Needed by test & verify
        public const string K_DATAS_LOADED = "K_DATAS_LOADED_er468rez"; // OK Needed by test & verify
        //-------------------------------------------------------------------------------------------------------------
        public const string K_EDITOR_REFRESH = "K_EDITOR_REFRESH_g54D55hs"; // OK Needed by test & verify
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
        // receipt Error
        public const string K_ERROR = "K_ERROR_Okk4ez6S";
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================