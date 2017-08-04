//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SocialPlatforms;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{

	//-------------------------------------------------------------------------------------------------------------
	public enum NWDNetworkState
	{
		Unknow,
		OnLine,
		OffLine,
	}
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWD game data manager.
	/// Extends class to use as singleton in unity3D
	/// </summary>
	public partial class NWDGameDataManager : MonoBehaviour
	{
		//-------------------------------------------------------------------------------------------------------------
		public const string NOTIFICATION_UPLOAD_IN_PROGRESS = "NOTIFICATION_UPLOAD_IN_PROGRESS";
		//-------------------------------------------------------------------------------------------------------------
		public const string NOTIFICATION_DOWNLOAD_IN_PROGRESS = "NOTIFICATION_DOWNLOAD_IN_PROGRESS";
		//-------------------------------------------------------------------------------------------------------------
		public const string NOTIFICATION_DOWNLOAD_IS_DONE = "NOTIFICATION_DOWNLOAD_IS_DONE";
		//-------------------------------------------------------------------------------------------------------------
		public const string NOTIFICATION_ERROR = "NOTIFICATION_ERROR";
		//-------------------------------------------------------------------------------------------------------------
		public const string NOTIFICATION_SESSION_EXPIRED = "NOTIFICATION_SESSION_EXPIRED";
		//-------------------------------------------------------------------------------------------------------------
		public const string NOTIFICATION_DOWNLOAD_SUCCESSED = "NOTIFICATION_DOWNLOAD_SUCCESSED";
		//-------------------------------------------------------------------------------------------------------------
		public const string NOTIFICATION_NETWORK_OFFLINE = "NOTIFICATION_NETWORK_OFFLINE";
		//-------------------------------------------------------------------------------------------------------------
		public const string NOTIFICATION_NETWORK_ONLINE = "NOTIFICATION_NETWORK_ONLINE";
		//-------------------------------------------------------------------------------------------------------------
		public const string NOTIFICATION_NETWORK_UNKNOW = "NOTIFICATION_NETWORK_UNKNOW";
		//-------------------------------------------------------------------------------------------------------------

		public NWDNetworkState NetworkStatut = NWDNetworkState.Unknow;
		//-------------------------------------------------------------------------------------------------------------
		// Test network connexion
		public void NetworkStatutChange (NWDNetworkState sNewNetWorkStatut)
		{
			if (sNewNetWorkStatut != NetworkStatut) {
				NetworkStatut = sNewNetWorkStatut;
				if (NetworkStatut == NWDNetworkState.OffLine) {
					BTBNotificationManager.SharedInstance.PostNotification (new BTBNotification (NWDGameDataManager.NOTIFICATION_NETWORK_OFFLINE, null));
				} else if (NetworkStatut == NWDNetworkState.OnLine) {
					BTBNotificationManager.SharedInstance.PostNotification (new BTBNotification (NWDGameDataManager.NOTIFICATION_NETWORK_ONLINE, null));
				} else {
					BTBNotificationManager.SharedInstance.PostNotification (new BTBNotification (NWDGameDataManager.NOTIFICATION_NETWORK_UNKNOW, null));
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool HasNetWork ()
		{
			return NetworkStatut == NWDNetworkState.OnLine;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
