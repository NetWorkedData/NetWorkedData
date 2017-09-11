//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.IO;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
//	public enum NWDTypeService
//	{
//		LocalOnly,
//		// just localbase (example : device without network) => file database to manage data
//		LocalWithServer,
//		// Synchronize with server and copy to local to use offline (example : smartphone, computer, etc ) => file database and webservice to synchronize to manage data
//		ServerOnly,
//		// no copy to local, only data from webservices (example : facebook app) => webservice to manage data
//	}
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDDataManager
	{
		//		#if UNITY_EDITOR
		//		public static bool UseUnityInpector = false;
		//		#endif
		/// <summary>
		/// The singleton.
		/// </summary>
		private static readonly NWDDataManager kSharedInstance = new NWDDataManager ();

		public string PlayerLanguage = "en";
		//		public string PlayerAccountUUID;
		//		public string PlayerAccountToken;

		// Members properties for NOT account dependant database (data from editor)
		//public SQLiteConnection SQLiteConnection;
		public SQLiteConnection SQLiteConnectionEditor;
		public string DatabasePathEditor = "Assets/StreamingAssets";
		public string DatabaseNameEditor  = "NWDDatabaseEditor.prp";

		// Members properties for account dependant database (data from user)
		public SQLiteConnection SQLiteConnectionAccount;
		public string DatabasePathAccount = "Assets";
		public string DatabaseNameAccount = "NWDDatabaseAccount.prp";


//		public NWDTypeService ManagementType = NWDTypeService.LocalWithServer;
		private bool kConnectedToDatabase = false;

		public BTBNotificationManager NotificationCenter;


//		public SQLiteConnection SQLiteConnectionFromBundleCopy;
//		public string PathDatabaseFromBundleCopy;
		public bool NeedCopy = false;



		//-------------------------------------------------------------------------------------------------------------
		private NWDDataManager ()
		{
			NotificationCenter = BTBNotificationManager.SharedInstance;
			SystemLanguage tLocalLanguage = Application.systemLanguage;
			switch (tLocalLanguage) {
			case SystemLanguage.Afrikaans:
				PlayerLanguage = "aff";
				break;

			case SystemLanguage.French:
				PlayerLanguage = "fr";
				break;

			case SystemLanguage.English:
				PlayerLanguage = "en";
				break;

			default :
				PlayerLanguage = "en";
				break;
			}

			//Debug.Log ("NWDDataService");
			LoadPreferences (NWDAppConfiguration.SharedInstance.SelectedEnvironment ());
//			if (PlayerAccountUUID == null || PlayerAccountUUID == "")
//			{
//				PlayerAccountUUID = BTBSecurityTools.generateUniqueID();
//				PlayerAccountToken = "";
//				SavePreferences ();
//			}
		}
		//-------------------------------------------------------------------------------------------------------------
		~NWDDataManager ()
		{
			//Debug.Log ("NWDDataManager Destructor");
			if (NotificationCenter != null) {
				NotificationCenter.RemoveAll ();
				NotificationCenter = null;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public List<Type> mTypeLoadedList = new List<Type> ();
		public List<Type> mTypeList = new List<Type> ();
		public List<Type> mTypeSynchronizedList = new List<Type> ();
		public List<Type> mTypeUnSynchronizedList = new List<Type> ();

		public List<Type> mTypeAccountDependantList = new List<Type> ();
		public List<Type> mTypeNotAccountDependantList = new List<Type> ();

		public Dictionary<string, Type> mTrigramTypeDictionary = new Dictionary<string, Type> ();
		//-------------------------------------------------------------------------------------------------------------
//		public void AddClassToManage (Type sType, bool sServerSynchronize, string sClassTrigramme, string sMenuName, string sDescription = "")
//		{
//			if (mTypeList.Contains (sType) == false) {
//				mTypeList.Add (sType);
//			}
//			if (sServerSynchronize == true) {
//				if (mTypeSynchronizedList.Contains (sType) == false) {
//					mTypeSynchronizedList.Add (sType);
//				}
//				if (mTypeUnSynchronizedList.Contains (sType) == true) {
//					mTypeUnSynchronizedList.Remove (sType);
//				}
//			} else {
//				if (mTypeSynchronizedList.Contains (sType) == true) {
//					mTypeSynchronizedList.Remove (sType);
//				}
//				if (mTypeUnSynchronizedList.Contains (sType) == false) {
//					mTypeUnSynchronizedList.Add (sType);
//				}
//			}
//			if (mTypeLoadedList.Contains (sType) == false) {
//				var tMethodInfo = sType.GetMethod ("redefineClassToUse", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
//				if (tMethodInfo != null) {
//					tMethodInfo.Invoke (null, new object[]{ sType,sServerSynchronize, sClassTrigramme, sMenuName, sDescription });
//				}
//
//				if (mTrigramTypeDictionary.ContainsKey (sClassTrigramme)) {
//					Debug.Log ("ERROR this trigramme '" + sClassTrigramme + "' is allreday use by another class! (" + mTrigramTypeDictionary [sClassTrigramme] + ")");
//				} else {
//					mTrigramTypeDictionary.Add (sClassTrigramme, sType);
//				}
//
//				mTypeLoadedList.Add (sType);
//			}
//		}
		//-------------------------------------------------------------------------------------------------------------
		void ReLoadAllClass ()
		{
			//Debug.Log ("LoadAllClass");
			foreach (Type tType in mTypeList) {
				if (mTypeLoadedList.Contains (tType) == false) {
					var tMethodInfo = tType.GetMethod ("ClassLoad", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
					if (tMethodInfo != null) {
						tMethodInfo.Invoke (null, null);
					}
					mTypeLoadedList.Add (tType);
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool TestSaltMemorizationForAllClass ()
		{
			//Debug.Log ("LoadAllClass");
			bool rReturn = true;
			foreach (Type tType in mTypeList) {
				var tMethodInfo = tType.GetMethod ("PrefSalt", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) {
					string tR = tMethodInfo.Invoke (null, null) as string;
					if (tR != "ok") {
						rReturn = false;
					}
				}
			}
			if (rReturn == false) {
				// do reccord and recompile
				#if UNITY_EDITOR
				NWDAppConfiguration.SharedInstance.GenerateCSharpFile (NWDAppConfiguration.SharedInstance.SelectedEnvironment ());
				#else
				//TODO: ALERT USER ERROR IN APP DISTRIBUTION
				#endif
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDDataManager SharedInstance {
			get {
				//Debug.Log ("Singleton");
				return kSharedInstance; 
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void LoadedClass(Type sType, int sNumberOfClasses, int sIndexOfActualClass)
		{
			//Debug.Log (sType.Name + " loaded ("+ (sIndexOfActualClass) +" / "+ sNumberOfClasses +")");
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================