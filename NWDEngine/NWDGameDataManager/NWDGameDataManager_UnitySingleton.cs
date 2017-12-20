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

#if UNITY_EDITOR
using UnityEditor;
#endif

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
			Debug.Log ("NWDGameDataManager OnRuntimeMethodLoad");
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
            Debug.Log ("NWDGameDataManager UnitySingleton");
			if (kUnitySingleton == null) {
				
				// check real Singleton
				NWDGameDataManager[] tOtherInstances = FindObjectsOfType<NWDGameDataManager> ();

                List<NWDGameDataManager> tOtherList = new List<NWDGameDataManager>();

                foreach (NWDGameDataManager tOtherInstance in tOtherInstances)
                {
                    tOtherList.Add(tOtherInstance);
                }
                if (tOtherList.Count == 0)
                {
                    // I need to create singleton
                    GameObject tObjToSpawn;
                    //spawn object
                    tObjToSpawn = new GameObject("NWDGameDataManagerUnitySingleton");
                    //Add Components
                    tObjToSpawn.AddComponent<NWDGameDataManager>();



                    // keep k_Singleton
                    kUnitySingleton = tObjToSpawn.GetComponent<NWDGameDataManager>();
                }
                else  if (tOtherList.Count == 1)
                {
                    kUnitySingleton = tOtherList[0];
                }
                else
                {
                    while (tOtherList.Count > 0)
                    {
                        NWDGameDataManager tLast = tOtherList[0];
                        tOtherList.Remove(tLast);
#if UNITY_EDITOR
                        if (EditorApplication.isPlaying == true)
                        {
                            Debug.Log("NWDGameDataManager A");
                            Destroy(tLast.gameObject);
                        }
                        else
                        {
                            Debug.Log("NWDGameDataManager B");
                            DestroyImmediate(tLast.gameObject);
                        }
#else
                        Debug.Log("NWDGameDataManager C");
                    Destroy (tLast.gameObject);
#endif
                    }

                }


				//foreach (NWDGameDataManager tOtherInstance in tOtherInstances) {
				//	if (tOtherInstance.gameObject != tObjToSpawn) {
    //                    #if UNITY_EDITOR
    //                    if (EditorApplication.isPlaying == true)
    //                    {
    //                        Debug.Log("NWDGameDataManager A");
    //                        //DestroyImmediate (tOtherInstance.gameObject); // change with unity 2017.2.1f1
    //                        Destroy(tOtherInstance.gameObject);
    //                    }
    //                    else
    //                    {
    //                        Debug.Log("NWDGameDataManager B");
    //                        DestroyImmediate(tOtherInstance.gameObject); // change with unity 2017.2.1f1
    //                        //Destroy(tOtherInstance.gameObject);
    //                    }
    //                    #else
    //                    Debug.Log("NWDGameDataManager C");
				//		Destroy (tOtherInstance.gameObject);
				//		#endif
				//	}
				//}
			}
			return kUnitySingleton;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Start this instance.
		/// </summary>
		void Start ()
		{
			//Debug.LogVerbose ("NWDGameDataManager Start");
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
			//Debug.LogVerbose ("NWDGameDataManager Destroy");
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Awake this instance.
		/// </summary>
		void Awake ()
		{
			//Debug.LogVerbose ("NWDGameDataManager Awake");
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
				//Debug.LogVerbose ("OnApplicationFocus Focus is ON");
			} else {
				//Debug.LogVerbose ("OnApplicationFocus Focus is OFF");
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
				//Debug.LogVerbose ("OnApplicationPause Pause is OFF");
			} else {
				//Debug.LogVerbose ("OnApplicationPause Pause is ON");
			}
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================