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

				if (ManagementType != NWDTypeService.ServerOnly) {
#if UNITY_EDITOR
					if (AssetDatabase.IsValidFolder (mDatabasePath) == false)
                    {
						AssetDatabase.CreateFolder ("Assets", "StreamingAssets");
					}
					var dbPath = mDatabasePath + "/" + mDatabaseName;
                    //var dbPath = string.Format(@"Assets/StreamingAssets/{0}", mDatabaseName);
#else
					// check if file exists in Application.persistentDataPath
					var filepath = string.Format ("{0}/{1}", Application.persistentDataPath, mDatabaseName);
					BTBDebug.Log ("Persistent path:" + filepath);
					if (!File.Exists (filepath)) {
						BTBDebug.Log ("Database not in Persistent path");
						// if it doesn't ->
						// open StreamingAssets directory and load the db ->
#if UNITY_ANDROID
					    var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + mDatabaseName);  // this is the path to your StreamingAssets in android
					    while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
					    // then save to Application.persistentDataPath
					    File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
						var loadDb = Application.dataPath + "/Raw/" + mDatabaseName;  // this is the path to your StreamingAssets in iOS
						// then save to Application.persistentDataPath
						File.Copy (loadDb, filepath);
#elif UNITY_WP8
					    var loadDb = Application.dataPath + "/Resources/" + mDatabaseName;
					    // then save to Application.persistentDataPath
					    File.Copy(loadDb, filepath);
#elif UNITY_WINRT
					    var loadDb = Application.dataPath + "/Resources/" + mDatabaseName;
					    // then save to Application.persistentDataPath
					    File.Copy(loadDb, filepath);
#else
					    var loadDb = Application.dataPath + "/Resources/" + mDatabaseName;
					    // then save to Application.persistentDataPath
					    File.Copy(loadDb, filepath);
#endif
						// Save App version in pref for futur used
						string tBuildTimeStamp = NWDAppConfiguration.SharedInstance.SelectedEnvironment ().BuildTimestamp.ToString ();
						BTBPrefsManager.ShareInstance ().set ("APP_VERSION", tBuildTimeStamp);

						BTBDebug.Log ("Database written");
						BTBDebug.Log ("Path:" + loadDb);
					} else {

						Debug.Log ("#### BASE ALLREDAY EXISTS");
						// Get saved App version from pref
						string tBuildTimeStamp = NWDAppConfiguration.SharedInstance.SelectedEnvironment ().BuildTimestamp.ToString ();
						string tBuildTimeStampActual = BTBPrefsManager.ShareInstance ().getString ("APP_VERSION");
						// Check build version
						if (tBuildTimeStamp.Equals (tBuildTimeStampActual) == false) {

							Debug.Log ("#### BASE ALLREDAY EXISTS AND NEED UPDATE FROM BUNDLE DATABASE");
							NeedCopy = true;
							// Set temporary patch for copy the database localy
							PathDatabaseFromBundleCopy = string.Format ("{0}/{1}", Application.persistentDataPath, "TempDB.prp");

							// Copy data from the bundle database into temporary location
#if UNITY_ANDROID
                            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + mDatabaseName);  // this is the path to your StreamingAssets in android
                            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
                            // then save to Application.persistentDataPath
					File.WriteAllBytes(PathDatabaseFromBundleCopy, loadDb.bytes);
#elif UNITY_IOS
							var loadDb = Application.dataPath + "/Raw/" + mDatabaseName;  // this is the path to your StreamingAssets in iOS
							// then save to Application.persistentDataPath
							File.Copy (loadDb, PathDatabaseFromBundleCopy);
#elif UNITY_WP8
					        var loadDb = Application.dataPath + "/Resources/" + mDatabaseName;
					        // then save to Application.persistentDataPath
					File.Copy(loadDb, PathDatabaseFromBundleCopy);
#elif UNITY_WINRT
					        var loadDb = Application.dataPath + "/Resources/" + mDatabaseName;
					        // then save to Application.persistentDataPath
					File.Copy(loadDb, PathDatabaseFromBundleCopy);
#endif
							// Load the temporay database
					SQLiteConnectionFromBundleCopy = new SQLiteConnection (PathDatabaseFromBundleCopy, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

							// Update the actual database

//                            List<NWDItem> query = tSQLiteTemp.Query<NWDItem>("Select * From NWDItem");
							//select * from db2.table where not exists (select * from db1.table where db1.table.column1 = db2.table.column1);



							// Update sync timestamp in pref
//
//                            // Close both database
//					SQLiteConnectionFromBundleCopy.Close();
//
//                            // Remove temporary database
//					File.Delete(PathDatabaseFromBundleCopy);

							// Save App version in pref
//					BTBPrefsManager.ShareInstance ().set("APP_VERSION", Application.version);
						}
					else
					{
					Debug.Log ("#### BASE ALLREDAY EXISTS BUT NOT NEED UPDATE !!! ");
					}
					}
					var dbPath = filepath;
#endif
					SQLiteConnection = new SQLiteConnection (dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
					BTBDebug.Log ("Final PATH: " + dbPath);

					//TODO : Find build NWD in preference (user pref)
					//TODO : find build NWD in this IPA
					//TODO : if build different copy the not connected table and reccord the new build verison in user prefs
					//TODO : check all table of notaccountconnected class 
					//TODO : select all in the bundle database
					//TODO : copy in the document database each row which is newer than the used row

				} else {
					//TODO : with webservice only
				}
			}
		}

		public void InsertObject (object sObject)
		{
			if (ManagementType != NWDTypeService.ServerOnly) {
				// local insert
				SQLiteConnection.Insert (sObject);
			}
			if (ManagementType != NWDTypeService.LocalOnly) {
				// TO DO Synchronization Queue
			}
		}

		public void UpdateObject (object sObject)
		{
			if (ManagementType != NWDTypeService.ServerOnly) {
				// local update
				//Debug.Log ("object is update on database without DM and Integrity");
				SQLiteConnection.Update (sObject);
			}
			if (ManagementType != NWDTypeService.LocalOnly) {
				// TO DO Synchronization Queue
			}
		}

		public void DeleteObject (object sObject)
		{
			//  update disable with date to delete
			//UpdateObject (sObject);
			if (ManagementType != NWDTypeService.ServerOnly) {
				SQLiteConnection.Delete (sObject);
			}
			if (ManagementType != NWDTypeService.LocalOnly) {
				// TO DO Synchronization Queue with UPDATE XX
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
			if (ManagementType != NWDTypeService.ServerOnly) {
				SQLiteConnection = new SQLiteConnection (mDatabasePath + mDatabaseName, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
				#if UNITY_EDITOR
				AssetDatabase.ImportAsset (mDatabasePath + mDatabaseName);
				#endif
			}

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

		public void CreateTable (Type sType)
		{
			//Debug.Log ("CreateTable " + sType.Name);
			if (ManagementType != NWDTypeService.ServerOnly) {
				//Debug.Log ("Go to CreateTableByType " + sType.Name);
				ConnectToDatabase ();
				SQLiteConnection.CreateTableByType (sType);
			}
			if (ManagementType != NWDTypeService.LocalOnly) {
				// TO DO Creation on server
			}
			PopulateTable (sType);
		}

		public void PopulateTable (Type sType)
		{
			if (ManagementType != NWDTypeService.ServerOnly) {
			}
			if (ManagementType != NWDTypeService.LocalOnly) {
			}
		}


		public void EmptyTable (Type sType)
		{
			if (ManagementType != NWDTypeService.ServerOnly) {
			}
			if (ManagementType != NWDTypeService.LocalOnly) {
				// TO DO Creation on server
			}
		}

		public void DropTable (Type sType)
		{
			if (ManagementType != NWDTypeService.ServerOnly) {
				SQLiteConnection.DropTableByType (sType);
			}
			if (ManagementType != NWDTypeService.LocalOnly) {
				// TO DO Creation on server
			}
		}

		public void ReInitializeTable (Type sType)
		{
			EmptyTable (sType);
			PopulateTable (sType);
		}

		public void ResetTable (Type sType)
		{
			DropTable (sType);
			CreateTable (sType);
		}
		#if UNITY_EDITOR
		#endif
	}
}
//=====================================================================================================================