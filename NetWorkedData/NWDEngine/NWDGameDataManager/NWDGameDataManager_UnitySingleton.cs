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
	}
}
//=====================================================================================================================
