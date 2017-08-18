//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using SQLite4Unity3d;

using BasicToolBox;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		public static string SynchronizeKeyData = "data";
		public static string SynchronizeKeyDataCount = "rowCount";
		public static string SynchronizeKeyTimestamp = "sync";
		public static string SynchronizeKeyLastTimestamp = "last";
		public static string SynchronizeKeyInWaitingTimestamp = "waiting";
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Synchronizations the prefs key.
		/// </summary>
		/// <returns>The prefs key.</returns>
		public static string SynchronizationPrefsKey (NWDAppEnvironment sEnvironment)
		{
			// use the accountReference with prefbase key associated with environement and key time 
			if (AccountDependent ()) {
				return sEnvironment.PlayerAccountReference + PrefBaseKey () + sEnvironment.Environment + SynchronizeKeyLastTimestamp;
			} else {
				return PrefBaseKey () + sEnvironment.Environment + SynchronizeKeyLastTimestamp;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void SynchronizationResetTimestamp (NWDAppEnvironment sEnvironment)
		{
			#if UNITY_EDITOR
			EditorPrefs.SetInt (SynchronizationPrefsKey (sEnvironment), sEnvironment.BuildTimestamp);
			#else
			PlayerPrefs.SetInt (SynchronizationPrefsKey(sEnvironment), sEnvironment.BuildTimestamp);
			#endif
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Synchronizations the get last timestamp.
		/// </summary>
		/// <returns>The get last timestamp.</returns>
		public static int SynchronizationGetLastTimestamp (NWDAppEnvironment sEnvironment)
		{
			int rReturn = sEnvironment.BuildTimestamp;
			#if UNITY_EDITOR
			if (EditorPrefs.HasKey (SynchronizationPrefsKey (sEnvironment))) {
				rReturn = EditorPrefs.GetInt (SynchronizationPrefsKey (sEnvironment));
			}
			#else
			if (PlayerPrefs.HasKey(SynchronizationPrefsKey(sEnvironment)))
				{
			rReturn = PlayerPrefs.GetInt (SynchronizationPrefsKey(sEnvironment));
				};
			#endif
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Synchronizations the set new timestamp.
		/// </summary>
		/// <param name="sNewTimestamp">S new timestamp.</param>
		public static void SynchronizationSetNewTimestamp (NWDAppEnvironment sEnvironment, int sNewTimestamp)
		{
			#if UNITY_EDITOR
			EditorPrefs.SetInt (SynchronizationPrefsKey (sEnvironment), sNewTimestamp);
			#else
			PlayerPrefs.SetInt (SynchronizationPrefsKey(sEnvironment), sNewTimestamp);
			#endif
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Synchronizations the in waiting prefs key.
		/// </summary>
		/// <returns>The in waiting prefs key.</returns>
		public static string SynchronizationInWaitingPrefsKey (NWDAppEnvironment sEnvironment)
		{
			// use the accountReference with prefbase key associated with environement and key time 
			if (AccountDependent ()) {
				return sEnvironment.PlayerAccountReference + PrefBaseKey () + sEnvironment.Environment + SynchronizeKeyInWaitingTimestamp;
			} else {
				return PrefBaseKey () + sEnvironment.Environment + SynchronizeKeyInWaitingTimestamp;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Synchronizations the get in waiting timestamp.
		/// </summary>
		/// <returns>The get in waiting timestamp.</returns>
		public static int SynchronizationGetInWaitingTimestamp (NWDAppEnvironment sEnvironment)
		{
			int rReturn = sEnvironment.BuildTimestamp;
			#if UNITY_EDITOR
			if (EditorPrefs.HasKey (SynchronizationInWaitingPrefsKey (sEnvironment))) {
				rReturn = EditorPrefs.GetInt (SynchronizationInWaitingPrefsKey (sEnvironment));
			}
			#else
			if (PlayerPrefs.HasKey(SynchronizationInWaitingPrefsKey(sEnvironment)))
			{
			rReturn = PlayerPrefs.GetInt (SynchronizationInWaitingPrefsKey(sEnvironment));
			};
			#endif
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Synchronizations the set in waiting timestamp.
		/// </summary>
		/// <param name="sNewTimestamp">S new timestamp.</param>
		public static void SynchronizationSetInWaitingTimestamp (NWDAppEnvironment sEnvironment, int sNewTimestamp)
		{
			#if UNITY_EDITOR
			EditorPrefs.SetInt (SynchronizationInWaitingPrefsKey (sEnvironment), sNewTimestamp);
			#else
			PlayerPrefs.SetInt (SynchronizationInWaitingPrefsKey(sEnvironment), sNewTimestamp);
			#endif
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Synchronizations the timestamp validate.
		/// </summary>
		/// <param name="sTimestampServer">S timestamp server.</param>
		public static void SynchronizationTimestampValidate (NWDAppEnvironment sEnvironment, int sTimestampServer)
		{
			int tTimestamp = SynchronizationGetInWaitingTimestamp (sEnvironment);
			if (tTimestamp > sTimestampServer) {
				tTimestamp = sTimestampServer;
			}
			SynchronizationSetNewTimestamp (sEnvironment, tTimestamp);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Synchronizations the insert in base.
		/// </summary>
		/// <returns>The insert in base.</returns>
		/// <param name="sDataArray">S data array.</param>
		public static NWDBasis<K> SynchronizationInsertInBase (NWDAppEnvironment sEnvironment, string[] sDataArray)
		{
			//BTBDebug.Log ("SynchronizationInsertInBase ");
			string tReference = GetReferenceValueFromCSV (sDataArray);
			NWDBasis<K> tObject = InstanceByReference (tReference);
			if (tObject == null) {
				//BTBDebug.Log ("SynchronizationInsertInBase NEW OBJECT DETECTED");
				tObject = NewInstanceFromCSV (sEnvironment, sDataArray);
				AddObjectInListOfEdition (tObject);
			} else {
				// test if Modification is older than actual object
				//BTBDebug.Log ("SynchronizationInsertInBase JUST UPDATE OBJECT DETECTED");
				if (tObject.DM <= GetDMValueFromCSV (sDataArray)) {
					tObject.UpdateWithCSV (sEnvironment, sDataArray);
				}
			}
			return tObject;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Synchronizations the insert in memory.
		/// </summary>
		/// <param name="sDataArray">S data array.</param>
		public static void SynchronizationInsertInMemory (NWDAppEnvironment sEnvironment, string[] sDataArray)
		{
			//BTBDebug.Log ("SynchronizationInsertInMemory ");
			// if NWDject.reference allready in memory I must replace this object and distroy old object
			NWDBasis<K> tFindObject = null;
			string tReference = GetReferenceValueFromCSV (sDataArray);
			foreach (NWDBasis<K> tObject in ObjectsList) {
				if (tObject.Reference == tReference) {
					tFindObject = tObject;
				}
			}
			if (tFindObject != null) {
				if (tFindObject.DM <= GetDMValueFromCSV (sDataArray)) {
					tFindObject.UpdateWithCSV (sEnvironment, sDataArray);
				}
			} else {
				tFindObject = NewInstanceFromCSV (sEnvironment, sDataArray);
				tFindObject.UpdateWithCSV (sEnvironment, sDataArray);
				AddObjectInListOfEdition (tFindObject);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Synchronizations the try to use.
		/// </summary>
		/// <returns>The try to use.</returns>
		/// <param name="sData">S data.</param>
		/// <param name="sForceToUse">If set to <c>true</c> s force to use.</param>
		public static NWDBasis<K> SynchronizationTryToUse (NWDAppEnvironment sEnvironment, string sData, bool sForceToUse = false)
		{
			//BTBDebug.Log ("SynchronizationTryToUse ");
			NWDBasis<K> rReturn = null;
			string[] tDataArray = sData.Split (NWDConstants.kStandardSeparator.ToCharArray ());
			for (int tI = 0; tI < tDataArray.Length; tI++) {
				tDataArray [tI] = NWDToolbox.TextCSVUnprotect (tDataArray [tI]);
			}
			// I need to test the integrity of datas... 
			if (TestIntegrityValueFromCSV (tDataArray) == true || sForceToUse == true) {
				rReturn = SynchronizationInsertInBase (sEnvironment, tDataArray);
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Synchronizations the push data.
		/// </summary>
		/// <returns>The push data.</returns>
		/// <param name="sForceAll">If set to <c>true</c> s force all.</param>
		public static Dictionary<string, object> SynchronizationPushData (NWDAppEnvironment sEnvironment, bool sForceAll)
		{

			SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance.SQLiteConnectionEditor;
			if (AccountDependent ())
			{
				tSQLiteConnection = NWDDataManager.SharedInstance.SQLiteConnectionAccount;
			}

			//BTBDebug.Log ("SynchronizationPushData for table " + TableName ());
			// ok if sync will be ok this date will be the last sync for this table
			SynchronizationSetInWaitingTimestamp (sEnvironment, NWDToolbox.Timestamp ());
			// create respond object
			Dictionary<string, object> rSend = new Dictionary<string, object> ();
			// create dictionnary for this tablename and insert in the respond
			Dictionary<string, object> rSendDatas = new Dictionary<string, object> ();
			rSend.Add (TableName (), rSendDatas);
			// create List with all object to synchron on the server
			// create List 
			List<object> tDatas = new List<object> ();
			// get last synchro
			int tLastSynchronization = SynchronizationGetLastTimestamp (sEnvironment);
			// I get all objects 
			IEnumerable<K> tResults = null;
			//TODO: BUT IF SYNC = TIME ?
			if (sForceAll == true) {
				tLastSynchronization = 0; // ok you force, then, upload and then download ALL datas since 1970 (0)
				tResults = tSQLiteConnection.Table<K> ().Where (x => x.DM >= tLastSynchronization);
			} else if (sEnvironment == NWDAppConfiguration.SharedInstance.DevEnvironment) {
				tResults = tSQLiteConnection.Table<K> ().Where (x => x.DevSync == 0);
			} else if (sEnvironment == NWDAppConfiguration.SharedInstance.PreprodEnvironment) {
				tResults = tSQLiteConnection.Table<K> ().Where (x => x.PreprodSync == 0);
			} else if (sEnvironment == NWDAppConfiguration.SharedInstance.ProdEnvironment) {
				tResults = tSQLiteConnection.Table<K> ().Where (x => x.ProdSync == 0);
			}
			if (tResults != null) {
				foreach (NWDBasis<K> tItem in tResults) {
					if (tItem.TestIntegrity () && tItem.IsLockedObject () == false) {
						tDatas.Add (tItem.DataAssembly (true));
					}
				}
				// But I insert the datas only if I had one object or more to insert/update on the server
				if (tDatas.Count > 0) {
					rSendDatas.Add (SynchronizeKeyDataCount, tDatas.Count);
					rSendDatas.Add (SynchronizeKeyData, tDatas);
				}
			}
			rSendDatas.Add (SynchronizeKeyTimestamp, tLastSynchronization);
			// return the data
			//BTBDebug.Log ("SynchronizationPushData for table " + TableName () +" rSend = " + rSend.ToString ());
			return rSend;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Synchronizations the pull data.
		/// </summary>
		/// <param name="sData">S data.</param>
		public static string SynchronizationPullData (NWDAppEnvironment sEnvironment, Dictionary<string, object> sData)
		{
			string rReturn = "";
			//BTBDebug.Log ("SynchronizationPullData for table " + TableName () + " with datas receipted");
			// Ok I receive data ... so I can reccord the last waiting timestamp as the good sync date
			if (sData.ContainsKey ("error")) {
				// error to show on Device
			} else {
				int tTimestampServer = NWDToolbox.Timestamp ();
				if (sData.ContainsKey ("timestamp")) {
					tTimestampServer = int.Parse (sData ["timestamp"].ToString ());
				} else {
					BTBDebug.Warning ("BIG ERROR NO TIMESTAMP");
				}
				SynchronizationTimestampValidate (sEnvironment, tTimestampServer);
				// now i need get only datas for this class tablename
				string tTableName = TableName ();
				// Ok I need to compute all datas for this Class tablename
				if (sData.ContainsKey (tTableName)) {
					//BTBDebug.Log (tTableName + "'s Datas found! it's a " + sData [tTableName].GetType ().Name);
					List<object> tListOfRows = sData [tTableName] as List<object>;
					//BTBDebug.Log ("tListOfRows found! with " + tListOfRows.Count + " row(s)");
					if (tListOfRows.Count > 0) {
						foreach (object tCsvValue in tListOfRows) {
							string tCsvValueString = tCsvValue as string;
							//BTBDebug.Log ("Datas found : '" + tCsvValueString + "'");
							// I try to use this data to ... insert/update/delete/... ?
							bool tForceToUse = false;
							#if UNITY_EDITOR
							tForceToUse = true;
							#endif
							NWDBasis<K> tObject = SynchronizationTryToUse (sEnvironment, tCsvValueString, tForceToUse);
							// trash this object ?
							FlushTrash (tObject);
						}
						rReturn = "YES";
						//NWDDataManager.SharedInstance.NotificationCenter.PostNotification (new BTBNotification (NWDNotificationConstants.K_DATAS_UPDATED, null));
						#if UNITY_EDITOR
						//	ReloadAllObjects ();
						FilterTableEditor ();
						#endif
					}
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Trash from web service.
		/// </summary>
		/// <param name="sForceAll">If set to <c>true</c> s force all.</param>
		public static bool TrashFromWebService (NWDAppEnvironment sEnvironment)
		{
			bool rReturn = false;

			rReturn = true;
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Synchronizations from web service.
		/// </summary>
		/// <param name="sForceAll">If set to <c>true</c> s force all.</param>
		public static bool SynchronizationFromWebService (bool sForceAll, NWDAppEnvironment sEnvironment)
		{
			bool rReturn = false;
			if (sForceAll == true) {
				#if UNITY_EDITOR
				NWDEditorMenu.EnvironementSync ().SynchronizationForce (new List<Type>{ ClassType () }, sEnvironment);
				#else
				NWDDataManager.SharedInstance.AddWebRequestSynchronizationForce (new List<Type>{ClassType ()}, true, sEnvironment);
				#endif
			} else {
				#if UNITY_EDITOR
				NWDEditorMenu.EnvironementSync ().Synchronization (new List<Type>{ ClassType () }, sEnvironment);
				#else
				NWDDataManager.SharedInstance.AddWebRequestSynchronization (new List<Type>{ClassType ()}, true, sEnvironment);
				#endif
			}
			rReturn = true;
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void DeleteUser (NWDAppEnvironment sEnvironment)
		{
			if (AccountDependent () == true) {
				// reset last sync to zero
				SynchronizationSetNewTimestamp (sEnvironment, 0); // set to 0 ... only for data AccountDependent, so that's not affect the not connected data (game's data)
				// delete all datas for this user
				foreach (NWDBasis<K> tObject in ObjectsList) {
					if (tObject.IsReacheableByAccount (NWDAppConfiguration.SharedInstance.SelectedEnvironment ().PlayerAccountReference)) {
						tObject.DeleteMe ();
					}
				}
				// need to reload this data now : to remove all tObjects from memory!
				LoadTableEditor ();
			}
		}
	}
}
//=====================================================================================================================