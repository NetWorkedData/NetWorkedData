//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using BasicToolBox;

//=====================================================================================================================

namespace NetWorkedData
{
	/// <summary>
	/// NWD account panel. A script for NWDAccountPanel refab to show the informations about wiwh account is use in the 
	/// game. 
	/// </summary>
	public class NWDAccountPanel : MonoBehaviour
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The name of environment.
		/// </summary>
		public Text TextEnvironment;
		/// <summary>
		/// The account's reference.
		/// </summary>
		public Text TextAccount;
		/// <summary>
		/// The token's value.
		/// </summary>
		public Text TextToken;
		/// <summary>
		/// The anonymous account's reference.
		/// </summary>
		public Text TextAnonymousAccount;
		/// <summary>
		/// The text anonymous token ? .
		/// </summary>
		public Text TextAnonymousToken;
		/// <summary>
		/// The text of web result.
		/// </summary>
		public Text TextWebResult;
		/// <summary>
		/// The text of network statut.
		/// </summary>
		public Text TextNetworkResult;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Use to test Synchronization
		/// </summary>
		public void SynchronizeTest ()
		{
			NWDDataManager.SharedInstance.AddWebRequestAllSynchronizationWithBlock (
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
					NWDOperationResult tResult = (NWDOperationResult) sResult;
					string tDescription =  "";
					if (tResult.errorDesc!=null)
					{
						tDescription = ": (" +tResult.errorCode + ") " +tResult.errorDesc.LocalizedDescription.GetLocalString ();
					}
					TextWebResult.text = " Synchronize Success " + tDescription;
				},
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
					NWDOperationResult tResult = (NWDOperationResult) sResult;
					string tDescription =  "";
					if (tResult.errorDesc!=null)
					{
						tDescription = ": (" +tResult.errorCode + ") " +tResult.errorDesc.LocalizedDescription.GetLocalString ();
					}
					TextWebResult.text = " Synchronize Error " + tDescription;
				},
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
					NWDOperationResult tResult = (NWDOperationResult) sResult;
					string tDescription =  "";
					if (tResult.errorDesc!=null)
					{
						tDescription = ": (" +tResult.errorCode + ") " +tResult.errorDesc.LocalizedDescription.GetLocalString ();
					}
					TextWebResult.text = " Synchronize Cancel " + tDescription;
				},
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
					NWDOperationResult tResult = (NWDOperationResult) sResult;
					string tDescription =  "";
					if (tResult.errorDesc!=null)
					{
						tDescription = ": (" +tResult.errorCode + ") " +tResult.errorDesc.LocalizedDescription.GetLocalString ();
					}
					TextWebResult.text = " Synchronize Progress " + tDescription;
				}
			);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Use to test Log-in
		/// </summary>
		public void LogInTest ()
		{
			NWDDataManager.SharedInstance.AddWebRequestSignInWithBlock ("Kortex", "Xetrok",
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
					NWDOperationResult tResult = (NWDOperationResult) sResult;
					string tDescription =  "";
					if (tResult.errorDesc!=null)
					{
						tDescription = ": (" +tResult.errorCode + ") " +tResult.errorDesc.LocalizedDescription.GetLocalString ();
					}
					TextWebResult.text = " Sign In Success " + tDescription;
				},
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
					NWDOperationResult tResult = (NWDOperationResult) sResult;
					string tDescription =  "";
					if (tResult.errorDesc!=null)
					{
						tDescription = ": (" +tResult.errorCode + ") " +tResult.errorDesc.LocalizedDescription.GetLocalString ();
					}
					TextWebResult.text = " Sign In Error " + tDescription;
				},
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
					NWDOperationResult tResult = (NWDOperationResult) sResult;
					string tDescription =  "";
					if (tResult.errorDesc!=null)
					{
						tDescription = ": (" +tResult.errorCode + ") " +tResult.errorDesc.LocalizedDescription.GetLocalString ();
					}
					TextWebResult.text = " Sign In Cancel " + tDescription;
				},
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
					NWDOperationResult tResult = (NWDOperationResult) sResult;
					string tDescription =  "";
					if (tResult.errorDesc!=null)
					{
						tDescription = ": (" +tResult.errorCode + ") " +tResult.errorDesc.LocalizedDescription.GetLocalString ();
					}
					TextWebResult.text = " Sign In Progress " + tDescription;
				});
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Use to test Log-out
		/// </summary>
		public void LogOutTest ()
		{
			NWDDataManager.SharedInstance.AddWebRequestSignOutWithBlock (
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
					NWDOperationResult tResult = (NWDOperationResult) sResult;
					string tDescription =  "";
					if (tResult.errorDesc!=null)
					{
						tDescription = ": (" +tResult.errorCode + ") " +tResult.errorDesc.LocalizedDescription.GetLocalString ();
					}
					TextWebResult.text = " Sign Out Success " + tDescription;
				},
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
					NWDOperationResult tResult = (NWDOperationResult) sResult;
					string tDescription =  "";
					if (tResult.errorDesc!=null)
					{
						tDescription = ": (" +tResult.errorCode + ") " +tResult.errorDesc.LocalizedDescription.GetLocalString ();
					}
					TextWebResult.text = " Sign Out Error " + tDescription;
				},
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
					NWDOperationResult tResult = (NWDOperationResult) sResult;
					string tDescription =  "";
					if (tResult.errorDesc!=null)
					{
						tDescription = ": (" +tResult.errorCode + ") " +tResult.errorDesc.LocalizedDescription.GetLocalString ();
					}
					TextWebResult.text = " Sign Out Cancel " + tDescription;
				},
				delegate (BTBOperation bOperation, float bProgress, BTBOperationResult sResult) {
					NWDOperationResult tResult = (NWDOperationResult) sResult;
					string tDescription =  "";
					if (tResult.errorDesc!=null)
					{
						tDescription = ": (" +tResult.errorCode + ") " +tResult.errorDesc.LocalizedDescription.GetLocalString ();
					}
					TextWebResult.text = " Sign Out Progress " + tDescription;
				});
		}
		//-------------------------------------------------------------------------------------------------------------
		// Use this for initialization
		void Start ()
		{
			Debug.Log("START NWDAccountPanel");
			BTBNotificationManager.SharedInstance.AddObserver (this, NWDGameDataManager.NOTIFICATION_NETWORK_ONLINE, delegate (BTBNotification sNotification) {
				if (TextNetworkResult!=null)
				{
				TextNetworkResult.text = "ON LINE";
				}
			});
			BTBNotificationManager.SharedInstance.AddObserver (this, NWDGameDataManager.NOTIFICATION_NETWORK_OFFLINE, delegate (BTBNotification sNotification) {
				if (TextNetworkResult!=null)
				{
				TextNetworkResult.text = "<color=red><b>OFF LINE</b></color>";
				}
			});
			BTBNotificationManager.SharedInstance.AddObserver (this, NWDGameDataManager.NOTIFICATION_NETWORK_UNKNOW, delegate (BTBNotification sNotification) {
				if (TextNetworkResult!=null)
				{
					TextNetworkResult.text = "<color=orange><b>????</b></color>";
				}
			});
		}
		//-------------------------------------------------------------------------------------------------------------
		// Use this for destroy
		void OnDestroy ()
		{
			BTBNotificationManager.SharedInstance.RemoveObserverEveryWhere (this);
		}
		//-------------------------------------------------------------------------------------------------------------
		// Update is called once per frame
		void Update ()
		{
			TextEnvironment.text = NWDAppConfiguration.SharedInstance.SelectedEnvironment ().Environment;
			TextAccount.text = NWDAppConfiguration.SharedInstance.SelectedEnvironment ().PlayerAccountReference;
			TextToken.text = NWDAppConfiguration.SharedInstance.SelectedEnvironment ().RequesToken;
			TextAnonymousAccount.text = NWDAppConfiguration.SharedInstance.SelectedEnvironment ().AnonymousPlayerAccountReference;
			TextAnonymousToken.text = "????";
		}
        //-------------------------------------------------------------------------------------------------------------
        public void ShowAccount()
        {
            gameObject.SetActive(!gameObject.activeInHierarchy);
        }
    }
}
//=====================================================================================================================
