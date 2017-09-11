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

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDDataManager
	{
		public void ConnectToDatabase ()
		{
			if (kConnectedToDatabase == false) {
				kConnectedToDatabase = true;







#if UNITY_EDITOR
				// create the good folder
				if (AssetDatabase.IsValidFolder (DatabasePathEditor) == false) {
					AssetDatabase.CreateFolder ("Assets", "StreamingAssets");
				}
				// path for base editor
				var tDatabasePathEditor = DatabasePathEditor + "/" + DatabaseNameEditor;
				var tDatabasePathAccount = DatabasePathAccount + "/" + DatabaseNameAccount;

#else


				// Get saved App version from pref
				int tBuildTimeStamp = NWDAppConfiguration.SharedInstance.SelectedEnvironment ().BuildTimestamp;
				int tBuildTimeStampActual = BTBPrefsManager.ShareInstance ().getInt ("APP_VERSION");
				// Check build version
				bool tForceCopy = false;
				if (tBuildTimeStamp > tBuildTimeStampActual) {
					NeedCopy = true;
					tForceCopy = true;
				}




					// check if file exists in Application.persistentDataPath
				var tPathEditor = string.Format ("{0}/{1}", Application.persistentDataPath, DatabaseNameEditor);
				var tPathAccount = string.Format ("{0}/{1}", Application.persistentDataPath, DatabaseNameAccount);
				BTBDebug.Log ("Persistent path:" + tPathEditor);
				if (!File.Exists (tPathEditor) || tForceCopy) {
						BTBDebug.Log ("Database not in Persistent path");
						// if it doesn't ->
						// open StreamingAssets directory and load the db ->
#if UNITY_ANDROID
				var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseNameEditor);  // this is the path to your StreamingAssets in android
					    while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
					    // then save to Application.persistentDataPath
				File.WriteAllBytes(tPathEditor, loadDb.bytes);
#elif UNITY_IOS
					var loadDb = Application.dataPath + "/Raw/" + DatabaseNameEditor;  // this is the path to your StreamingAssets in iOS
						// then save to Application.persistentDataPath
					File.Copy (loadDb, tPathEditor);
#elif UNITY_WP8
				var loadDb = Application.dataPath + "/Resources/" + DatabaseNameEditor;
					    // then save to Application.persistentDataPath
				File.Copy(loadDb, tPathEditor);
#elif UNITY_WINRT
				var loadDb = Application.dataPath + "/Resources/" + DatabaseNameEditor;
					    // then save to Application.persistentDataPath
				File.Copy(loadDb, tPathEditor);
#else
				var loadDb = Application.dataPath + "/Resources/" + DatabaseNameEditor;
					    // then save to Application.persistentDataPath
				File.Copy(loadDb, tPathEditor);
#endif
						// Save App version in pref for futur used

						BTBPrefsManager.ShareInstance ().set ("APP_VERSION", tBuildTimeStamp);

						BTBDebug.Log ("Database written");
						BTBDebug.Log ("Path:" + loadDb);
					} else {

//						Debug.Log ("#### BASE ALLREDAY EXISTS");
//						// Get saved App version from pref
//						string tBuildTimeStamp = NWDAppConfiguration.SharedInstance.SelectedEnvironment ().BuildTimestamp;
//						string tBuildTimeStampActual = BTBPrefsManager.ShareInstance ().getString ("APP_VERSION");
//						// Check build version
//						if (tBuildTimeStamp > tBuildTimeStampActual) {
//
//
//							Debug.Log ("#### BASE ALLREDAY NEED DELETE");
//
//							Debug.Log ("#### BASE ALLREDAY EXISTS AND NEED UPDATE FROM BUNDLE DATABASE");
//							NeedCopy = true;
//							// Set temporary patch for copy the database localy
//							PathDatabaseFromBundleCopy = string.Format ("{0}/{1}", Application.persistentDataPath, "TempDB.prp");
//
//							// Copy data from the bundle database into temporary location
//#if UNITY_ANDROID
//                            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + mDatabaseName);  // this is the path to your StreamingAssets in android
//                            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
//                            // then save to Application.persistentDataPath
//					File.WriteAllBytes(PathDatabaseFromBundleCopy, loadDb.bytes);
//#elif UNITY_IOS
//							var loadDb = Application.dataPath + "/Raw/" + mDatabaseName;  // this is the path to your StreamingAssets in iOS
//							// then save to Application.persistentDataPath
//							File.Copy (loadDb, PathDatabaseFromBundleCopy);
//#elif UNITY_WP8
//					        var loadDb = Application.dataPath + "/Resources/" + mDatabaseName;
//					        // then save to Application.persistentDataPath
//					File.Copy(loadDb, PathDatabaseFromBundleCopy);
//#elif UNITY_WINRT
//					        var loadDb = Application.dataPath + "/Resources/" + mDatabaseName;
//					        // then save to Application.persistentDataPath
//					File.Copy(loadDb, PathDatabaseFromBundleCopy);
//#endif
//							// Load the temporay database
//					SQLiteConnectionFromBundleCopy = new SQLiteConnection (PathDatabaseFromBundleCopy, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
//
//							// Update the actual database
//
////                            List<NWDItem> query = tSQLiteTemp.Query<NWDItem>("Select * From NWDItem");
//							//select * from db2.table where not exists (select * from db1.table where db1.table.column1 = db2.table.column1);
//
//
//
//							// Update sync timestamp in pref
////
////                            // Close both database
////					SQLiteConnectionFromBundleCopy.Close();
////
////                            // Remove temporary database
////					File.Delete(PathDatabaseFromBundleCopy);
//
//							// Save App version in pref
////					BTBPrefsManager.ShareInstance ().set("APP_VERSION", Application.version);
//						}
//					else
//					{
//					Debug.Log ("#### BASE ALLREDAY EXISTS BUT NOT NEED UPDATE !!! ");
//					}
					}
				var tDatabasePathEditor = tPathEditor;
				var tDatabasePathAccount = tPathAccount;
#endif
				SQLiteConnectionEditor = new SQLiteConnection (tDatabasePathEditor, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
				SQLiteConnectionAccount = new SQLiteConnection (tDatabasePathAccount, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
				#if UNITY_EDITOR
//				AssetDatabase.ImportAsset (tDatabasePathEditor);
//				AssetDatabase.ImportAsset (tDatabasePathAccount);
				#endif
			}
		}

		public void InsertObject (object sObject, bool sAccountConnected)
		{
			if (sAccountConnected) {
				SQLiteConnectionAccount.Insert (sObject);
			} else {
				SQLiteConnectionEditor.Insert (sObject);
			}
		}

		public void UpdateObject (object sObject, bool sAccountConnected)
		{
			if (sAccountConnected) {
				SQLiteConnectionAccount.Update (sObject);
			} else {
				SQLiteConnectionEditor.Update (sObject);
			}
		}

		public void DeleteObject (object sObject, bool sAccountConnected)
		{
			//  update disable with date to delete
			if (sAccountConnected) {
				SQLiteConnectionAccount.Delete (sObject);
			} else {
				SQLiteConnectionEditor.Delete (sObject);
			}
		}

		//NWDDataManager.SharedInstance.UpdateQueueExecute();
		public static List<object> kObjectToUpdateQueue = new List<object> ();

		public void AddObjectToUpdateQueue (object sObject)
		{
			//Debug.Log ("AddObjectToUpdateQueue … ");
			if (kObjectToUpdateQueue.Contains (sObject) == false) {
				kObjectToUpdateQueue.Add (sObject);
			}
		}

		public void UpdateQueueExecute ()
		{
			//Debug.Log ("UpdateQueueExecute with " + kObjectToUpdateQueue.Count + "element(s)");
			foreach (object tObject in kObjectToUpdateQueue) {
				Type tType = tObject.GetType ();
				var tMethodInfo = tType.GetMethod ("UpdateMe", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) {
					tMethodInfo.Invoke (tObject, new object[]{ true });
				} else {
				}
			}
			kObjectToUpdateQueue = new List<object> ();
		}

		// Table management
		public void CreateAllTablesLocal ()
		{
//			if (ManagementType != NWDTypeService.ServerOnly) {
//				SQLiteConnection = new SQLiteConnection (mDatabasePath + mDatabaseName, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
//				#if UNITY_EDITOR
//				AssetDatabase.ImportAsset (mDatabasePath + mDatabaseName);
//				#endif
//			}

			foreach (Type tType in mTypeList) {
				var tMethodInfo = tType.GetMethod ("CreateTable", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) {
					tMethodInfo.Invoke (null, null);
				}
			}
		}

		#if UNITY_EDITOR
		public void CreateAllTablesServer (NWDAppEnvironment sEnvironment)
		{
			NWDOperationWebManagement.AddOperation ("Create table on server", null, null, null, null, sEnvironment, true);
		}
		#endif

		public void CreateTable (Type sType, bool sAccountConnected)
		{
			//Debug.Log ("CreateTable " + sType.Name);
			//Debug.Log ("Go to CreateTableByType " + sType.Name);
			ConnectToDatabase ();

			if (sAccountConnected) {
				SQLiteConnectionAccount.CreateTableByType (sType);
			} else {
				SQLiteConnectionEditor.CreateTableByType (sType);
			}

			PopulateTable (sType, sAccountConnected);
		}

		public void PopulateTable (Type sType, bool sAccountConnected)
		{

			if (sAccountConnected) {
			} else {
			}
		}


		public void EmptyTable (Type sType, bool sAccountConnected)
		{
			if (sAccountConnected) {
			} else {
			}
			
		}

		public void DropTable (Type sType, bool sAccountConnected)
		{
			if (sAccountConnected) {
				SQLiteConnectionAccount.DropTableByType (sType);
			} else {
				SQLiteConnectionEditor.DropTableByType (sType);
			}
			
		}

		public void ReInitializeTable (Type sType, bool sAccountConnected)
		{
			EmptyTable (sType, sAccountConnected);
			PopulateTable (sType, sAccountConnected);
		}

		public void ResetTable (Type sType, bool sAccountConnected)
		{
			DropTable (sType, sAccountConnected);
			CreateTable (sType, sAccountConnected);
		}
		#if UNITY_EDITOR
		#endif
	}
}
//=====================================================================================================================