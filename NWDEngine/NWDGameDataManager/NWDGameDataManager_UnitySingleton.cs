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
		/// <summary>
		/// The unity singleton. (destroy other instance)
		/// </summary>
		private static NWDGameDataManager kUnitySingleton = null;
		/// <summary>
		/// The application stand by.
		/// </summary>
		private bool ApplicationStandBy = false;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the runtime method load event.
		/// </summary>
		[RuntimeInitializeOnLoadMethod]
		static void OnRuntimeMethodLoad ()
		{
			//BTBDebug.LogVerbose ("NWDGameDataManager OnRuntimeMethodLoad");
			UnitySingleton ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Unity the singleton game object.
		/// </summary>
		/// <returns>The singleton game object.</returns>
		public static GameObject UnitySingletonGameObject ()
		{
			return UnitySingleton ().gameObject;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Unity singleton.
		/// </summary>
		/// <returns>The singleton.</returns>
		public static NWDGameDataManager UnitySingleton ()
		{
			//BTBDebug.LogVerbose ("NWDGameDataManager Singleton");
			if (kUnitySingleton == null) {
				// I need to create singleton
				GameObject tObjToSpawn;
				//spawn object
				tObjToSpawn = new GameObject ("NWDGameDataManagerUnitySingleton");
				//Add Components
				tObjToSpawn.AddComponent<NWDGameDataManager> ();
				// keep k_Singleton
				kUnitySingleton = tObjToSpawn.GetComponent<NWDGameDataManager> ();
				// check real Singleton
				NWDGameDataManager[] tOtherInstances = FindObjectsOfType<NWDGameDataManager> ();
				foreach (NWDGameDataManager tOtherInstance in tOtherInstances) {
					if (tOtherInstance.gameObject != tObjToSpawn) {
						#if UNITY_EDITOR
						DestroyImmediate (tOtherInstance.gameObject);
						#else
						Destroy (tOtherInstance.gameObject);
						#endif
					}
				}
			}
			return kUnitySingleton;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Start this instance.
		/// </summary>
		void Start ()
		{
			//BTBDebug.LogVerbose ("NWDGameDataManager Start");
			DataManager = SharedInstanceAsSingleton.kSharedInstaceAsSingleton.DataManager;
			AppConfiguration = SharedInstanceAsSingleton.kSharedInstaceAsSingleton.AppConfiguration;
			NotificationCenter = SharedInstanceAsSingleton.kSharedInstaceAsSingleton.NotificationCenter;

			//Network is unknow at start
			NetworkStatutChange (NWDNetworkState.Unknow);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the destroy event.
		/// </summary>
		void OnDestroy()
		{
			//BTBDebug.LogVerbose ("NWDGameDataManager Destroy");
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Awake this instance.
		/// </summary>
		void Awake ()
		{
			//BTBDebug.LogVerbose ("NWDGameDataManager Awake");
			//Check if there is already an instance
			if (kUnitySingleton == null) {
				//if not, set it to this.
				kUnitySingleton = this;
				#if UNITY_EDITOR
				if (Application.isEditor && Application.isPlaying == true) {
					DontDestroyOnLoad (gameObject);
				}
				#else
				DontDestroyOnLoad (gameObject);
				#endif
			}
			//If instance already exists:
			else if (kUnitySingleton != this) {
				//Destroy this, this enforces our singleton pattern so there can only be one instance.
				#if UNITY_EDITOR
				DestroyImmediate (this.gameObject);
				#else
				Destroy (this.gameObject);
				#endif
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the application focus event.
		/// </summary>
		/// <param name="sHasFocus">If set to <c>true</c> s has focus.</param>
		void OnApplicationFocus (bool sHasFocus)
		{
			ApplicationStandBy = !sHasFocus;
			if (ApplicationStandBy == false) {
				//BTBDebug.LogVerbose ("OnApplicationFocus Focus is ON");
			} else {
				//BTBDebug.LogVerbose ("OnApplicationFocus Focus is OFF");
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Raises the application pause event.
		/// </summary>
		/// <param name="sPauseStatus">If set to <c>true</c> s pause status.</param>
		void OnApplicationPause (bool sPauseStatus)
		{
			ApplicationStandBy = sPauseStatus;
			if (ApplicationStandBy == false) {
				//BTBDebug.LogVerbose ("OnApplicationPause Pause is OFF");
			} else {
				//BTBDebug.LogVerbose ("OnApplicationPause Pause is ON");
			}
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================