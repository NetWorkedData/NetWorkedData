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
					if (AssetDatabase.IsValidFolder (mDatabasePath) == false) {
						AssetDatabase.CreateFolder ("Assets", "StreamingAssets");
					}
					var dbPath = mDatabasePath + "/" + mDatabaseName;
                    //var dbPath = string.Format(@"Assets/StreamingAssets/{0}", mDatabaseName);
#else
					// check if file exists in Application.persistentDataPath
					var filepath = string.Format("{0}/{1}", Application.persistentDataPath, mDatabaseName);
                    BTBDebug.Log("Persistent path:" + filepath);

					if (!File.Exists(filepath))
					{
					BTBDebug.Log("Database not in Persistent path");
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
					File.Copy(loadDb, filepath);
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
					BTBDebug.Log("Database written");
                    BTBDebug.Log("Path:" + loadDb);
					}
					var dbPath = filepath;
#endif

                    SQLiteConnection = new SQLiteConnection (dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
					BTBDebug.Log ("Final PATH: " + dbPath);
				} else {
					// TO DO with webservice only
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
			UpdateObject (sObject);
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
			NWDOperationWebManagement.AddOperation ("Create table on server",null,null,null,null, sEnvironment, true);
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

		#if UNITY_EDITOR

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
		#endif
	}
			}
			//=====================================================================================================================