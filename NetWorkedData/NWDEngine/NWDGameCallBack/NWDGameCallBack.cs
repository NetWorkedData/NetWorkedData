//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	//	TODO : finish implementation  : add notifications key and callback method : Must be test
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWD game call back.
	/// Use in game object to connect the other gameobject to action in the NetWorkedData package 
	/// Each scene can be connect independently
	/// </summary>
	public partial class NWDGameCallBack : MonoBehaviour
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Installs the observer in the BTBNotification manager
		/// </summary>
		void InstallObserver ()
		{
			// get BTBNotificationManager shared instance from the NWDGameDataManager Singleton
			BTBNotificationManager tNotificationManager = NWDGameDataManager.UnitySingleton ().NotificationCenter;

			// Add all notifictions keys to observe for this 
			tNotificationManager.AddObserver (this, NWDNotificationConstants.K_DATAS_UPDATED, delegate (BTBNotification sNotification) {
				Dictionary<string,object> tDicoInformations = new Dictionary<string,object> ();
				if (sNotification.Datas != null) {
					tDicoInformations.Add (NWDNotificationConstants.K_NOTIFICATION_KEY, sNotification.Datas);
				}
				DataUpdatedAction (tDicoInformations);
			});
			tNotificationManager.AddObserver (this, NWDNotificationConstants.K_DATAS_SYNCHRONIZE_START, delegate (BTBNotification sNotification) {
				Dictionary<string,object> tDicoInformations = new Dictionary<string,object> ();
				if (sNotification.Datas != null) {
					tDicoInformations.Add (NWDNotificationConstants.K_NOTIFICATION_KEY, sNotification.Datas);
				}
				DataSynchronizeStartAction (tDicoInformations);
			});
			tNotificationManager.AddObserver (this, NWDNotificationConstants.K_DATAS_SYNCHRONIZE_ERROR, delegate (BTBNotification sNotification) {
				Dictionary<string,object> tDicoInformations = new Dictionary<string,object> ();
				if (sNotification.Datas != null) {
					tDicoInformations.Add (NWDNotificationConstants.K_NOTIFICATION_KEY, sNotification.Datas);
				}
				DataSynchronizeErrorAction (tDicoInformations);
			});
			tNotificationManager.AddObserver (this, NWDNotificationConstants.K_DATAS_SYNCHRONIZE_SUCCESS, delegate (BTBNotification sNotification) {
				Dictionary<string,object> tDicoInformations = new Dictionary<string,object> ();
				if (sNotification.Datas != null) {
					tDicoInformations.Add (NWDNotificationConstants.K_NOTIFICATION_KEY, sNotification.Datas);
				}
				DataSynchronizeSuccessAction (tDicoInformations);
			});
			tNotificationManager.AddObserver (this, NWDNotificationConstants.K_ACCOUNT_REQUEST_START, delegate (BTBNotification sNotification) {
				Dictionary<string,object> tDicoInformations = new Dictionary<string,object> ();
				if (sNotification.Datas != null) {
					tDicoInformations.Add (NWDNotificationConstants.K_NOTIFICATION_KEY, sNotification.Datas);
				}
				AccountRequestStartAction (tDicoInformations);
			});
			tNotificationManager.AddObserver (this, NWDNotificationConstants.K_ACCOUNT_REQUEST_ERROR, delegate (BTBNotification sNotification) {
				Dictionary<string,object> tDicoInformations = new Dictionary<string,object> ();
				if (sNotification.Datas != null) {
					tDicoInformations.Add (NWDNotificationConstants.K_NOTIFICATION_KEY, sNotification.Datas);
				}
				AccountRequestErrorAction (tDicoInformations);
			});
			tNotificationManager.AddObserver (this, NWDNotificationConstants.K_ACCOUNT_REQUEST_SUCCESS, delegate (BTBNotification sNotification) {
				Dictionary<string,object> tDicoInformations = new Dictionary<string,object> ();
				if (sNotification.Datas != null) {
					tDicoInformations.Add (NWDNotificationConstants.K_NOTIFICATION_KEY, sNotification.Datas);
				}
				AccountRequestSuccessAction (tDicoInformations);
			});
		}
		//-------------------------------------------------------------------------------------------------------------
		void RemoveObserver ()
		{
			// get BTBNotificationManager shared instance from the NWDGameDataManager Singleton
			BTBNotificationManager tNotificationManager = NWDGameDataManager.UnitySingleton ().NotificationCenter;

			// remove this from BTBNotificationManager
			tNotificationManager.RemoveObserverEveryWhere (this);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Start this instance.
		/// </summary>
		void Start ()
		{
			InstallObserver ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Awake this instance.
		/// </summary>
		void Awake ()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the destroy event.
		/// </summary>
		void OnDestroy ()
		{
			RemoveObserver ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The data updated Event 
		/// </summary>
		public NWDGameDatasUpdated DataUpdated;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Datas updated action.
		/// </summary>
		/// <param name="sDicoInformations">dico informations.</param>
		public void DataUpdatedAction (Dictionary<string,object> sDicoInformations = null)
		{
			if (DataUpdated != null) {
				if (sDicoInformations == null) {
					sDicoInformations = new Dictionary<string,object> ();
				}
				DataUpdated.Invoke (sDicoInformations);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The data synchronize start event.
		/// </summary>
		public NWDGameDatasStart DataSynchronizeStart;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Datas synchronize start action.
		/// </summary>
		/// <param name="sDicoInformations">dico informations.</param>
		public void DataSynchronizeStartAction (Dictionary<string,object> sDicoInformations = null)
		{
			if (DataSynchronizeStart != null) {
				if (sDicoInformations == null) {
					sDicoInformations = new Dictionary<string,object> ();
				}
				DataSynchronizeStart.Invoke (sDicoInformations);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The data synchronize error event.
		/// </summary>
		public NWDGameDatasError DataSynchronizeError;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Datas the synchronize error action.
		/// </summary>
		/// <param name="sDicoInformations">S dico informations.</param>
		public void DataSynchronizeErrorAction (Dictionary<string,object> sDicoInformations = null)
		{
			if (DataSynchronizeError != null) {
				if (sDicoInformations == null) {
					sDicoInformations = new Dictionary<string,object> ();
				}
                //TODO : implement the error from data synchronize
                NWDError sError = new NWDError();
				DataSynchronizeError.Invoke (sDicoInformations, sError);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The data synchronize success event.
		/// </summary>
		public NWDGameDatasSuccess DataSynchronizeSuccess;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Datas the synchronize success action.
		/// </summary>
		/// <param name="sDicoInformations">S dico informations.</param>
		public void DataSynchronizeSuccessAction (Dictionary<string,object> sDicoInformations = null)
		{
			if (DataSynchronizeSuccess != null) {
				if (sDicoInformations == null) {
					sDicoInformations = new Dictionary<string,object> ();
				}
				DataSynchronizeSuccess.Invoke (sDicoInformations);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The account request start event.
		/// </summary>
		public NWDGameAccountStart AccountRequestStart;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Account request start action.
		/// </summary>
		/// <param name="sDicoInformations">S dico informations.</param>
		public void AccountRequestStartAction (Dictionary<string,object> sDicoInformations = null)
		{
			if (AccountRequestStart != null) {
				if (sDicoInformations == null) {
					sDicoInformations = new Dictionary<string,object> ();
				}
				NWDAppEnvironmentPlayerStatut tPlayerStatut = NWDAppConfiguration.SharedInstance.SelectedEnvironment ().PlayerStatut;
				AccountRequestStart.Invoke (sDicoInformations, tPlayerStatut);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The account request error event.
		/// </summary>
		public NWDGameAccountError AccountRequestError;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Account request error action.
		/// </summary>
		/// <param name="sDicoInformations">S dico informations.</param>
		public void AccountRequestErrorAction (Dictionary<string,object> sDicoInformations = null)
		{
			if (AccountRequestError != null) {
				if (sDicoInformations == null) {
					sDicoInformations = new Dictionary<string,object> ();
				}
				//TODO : implement the error from data synchronize
				NWDError sError = new NWDError();
				NWDAppEnvironmentPlayerStatut tPlayerStatut = NWDAppConfiguration.SharedInstance.SelectedEnvironment ().PlayerStatut;
				AccountRequestError.Invoke (sDicoInformations, tPlayerStatut, sError);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The account request success event.
		/// </summary>
		public NWDGameAccountSuccess AccountRequestSuccess;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Account request success action.
		/// </summary>
		/// <param name="sDicoInformations">S dico informations.</param>
		public void AccountRequestSuccessAction (Dictionary<string,object> sDicoInformations = null)
		{
			if (AccountRequestSuccess != null) {
				if (sDicoInformations == null) {
					sDicoInformations = new Dictionary<string,object> ();
				}
				NWDAppEnvironmentPlayerStatut tPlayerStatut = NWDAppConfiguration.SharedInstance.SelectedEnvironment ().PlayerStatut;
				AccountRequestSuccess.Invoke (sDicoInformations, tPlayerStatut);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================