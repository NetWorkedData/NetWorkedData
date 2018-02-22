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
using ColoredAdvancedDebug;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDDataManager
	{
        //-------------------------------------------------------------------------------------------------------------
        public static List<object> kObjectToUpdateQueue = new List<object>();
        //-------------------------------------------------------------------------------------------------------------
		public void ConnectToDatabase ()
		{
			if (kConnectedToDatabase == false)
            {
				kConnectedToDatabase = true;
#if UNITY_EDITOR
				// create the good folder
				if (AssetDatabase.IsValidFolder (DatabasePathEditor) == false) 
                {
					AssetDatabase.CreateFolder ("Assets", "StreamingAssets");
				}
				// path for base editor
                string tDatabasePathEditor = DatabasePathEditor + "/" + DatabaseNameEditor;
                string tDatabasePathAccount = DatabasePathAccount + "/" + DatabaseNameAccount;

#else
				// Get saved App version from pref
				int tBuildTimeStamp = NWDAppConfiguration.SharedInstance.SelectedEnvironment ().BuildTimestamp;
				int tBuildTimeStampActual = BTBPrefsManager.ShareInstance ().getInt ("APP_VERSION");

				// Check build version
				bool tForceCopy = false;
				if (tBuildTimeStamp > tBuildTimeStampActual)
                {
					NeedCopy = true;
					tForceCopy = true;
				}

				// check if file exists in Application.persistentDataPath
				string tPathEditor = string.Format ("{0}/{1}", Application.persistentDataPath, DatabaseNameEditor);
				string tPathAccount = string.Format ("{0}/{1}", Application.persistentDataPath, DatabaseNameAccount);

                //Debug.Log("tPathEditor = " + tPathEditor);
                //Debug.Log("tPathAccount = " + tPathAccount);

                // if must be update by build version : delete old editor data!
                if (tForceCopy == true)
                {
                    File.Delete(tPathEditor);
                }
                // Write editor database
				if (!File.Exists (tPathEditor))
                {
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
#elif UNITY_STANDALONE_OSX
                    var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseNameEditor;
                    // then save to Application.persistentDataPath
                    File.Copy(loadDb, tPathEditor);
#else
                    var loadDb = Application.dataPath + "/Resources/StreamingAssets/" + DatabaseNameEditor;
				    // then save to Application.persistentDataPath
				    File.Copy(loadDb, tPathEditor);
#endif
					// Save App version in pref for futur used
					BTBPrefsManager.ShareInstance ().set ("APP_VERSION", tBuildTimeStamp);
				}

				string tDatabasePathEditor = tPathEditor;
                string tDatabasePathAccount = tPathAccount;
                //Debug.Log("Application.dataPath = "+Application.dataPath);
                //Debug.Log("tDatabasePathEditor = " + tDatabasePathEditor);
                //Debug.Log("tPathAccount = " + tPathAccount);
#endif
                SQLiteConnectionEditor = new SQLiteConnection (tDatabasePathEditor, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
				SQLiteConnectionAccount = new SQLiteConnection (tDatabasePathAccount, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
			}
		}
        //-------------------------------------------------------------------------------------------------------------
		public void InsertObject (object sObject, bool sAccountConnected)
		{
			if (sAccountConnected)
            {
				SQLiteConnectionAccount.Insert (sObject);
			}
            else
            {
				SQLiteConnectionEditor.Insert (sObject);
			}
		}
        //-------------------------------------------------------------------------------------------------------------
		public void UpdateObject (object sObject, bool sAccountConnected)
		{
			if (sAccountConnected) {
				SQLiteConnectionAccount.Update (sObject);
			} else {
				SQLiteConnectionEditor.Update (sObject);
			}
		}
        //-------------------------------------------------------------------------------------------------------------
		public void DeleteObject (object sObject, bool sAccountConnected)
		{
			//  update disable with date to delete
			if (sAccountConnected) {
				SQLiteConnectionAccount.Delete (sObject);
			} else {
				SQLiteConnectionEditor.Delete (sObject);
			}
		}
        //-------------------------------------------------------------------------------------------------------------
		public void AddObjectToUpdateQueue (object sObject)
		{
			if (kObjectToUpdateQueue.Contains (sObject) == false) {
				kObjectToUpdateQueue.Add (sObject);
			}
		}
        //-------------------------------------------------------------------------------------------------------------
		public void UpdateQueueExecute ()
		{
			foreach (object tObject in kObjectToUpdateQueue) {
				Type tType = tObject.GetType ();
				var tMethodInfo = tType.GetMethod ("UpdateMe", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) {
					tMethodInfo.Invoke (tObject, new object[]{ true });
				}
			}
			kObjectToUpdateQueue = new List<object> ();
		}
        //-------------------------------------------------------------------------------------------------------------
		// Table management
		public void CreateAllTablesLocal ()
		{
			foreach (Type tType in mTypeList) {
				var tMethodInfo = tType.GetMethod ("CreateTable", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) {
					tMethodInfo.Invoke (null, null);
				}
			}
		}
        //-------------------------------------------------------------------------------------------------------------
		public void CleanAllTablesLocal ()
		{
		    foreach (Type tType in mTypeList) {
		        var tMethodInfo = tType.GetMethod ("CleanTable", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
		        if (tMethodInfo != null) {
		            tMethodInfo.Invoke (null, null);
		        }
		    }
		}
        //-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		public void CreateAllTablesServer (NWDAppEnvironment sEnvironment)
		{
			NWDOperationWebManagement.AddOperation ("Create table on server", null, null, null, null, sEnvironment, true);
		}
		#endif
        //-------------------------------------------------------------------------------------------------------------
		public void CreateTable (Type sType, bool sAccountConnected)
		{
			ConnectToDatabase ();

			if (sAccountConnected) {
				SQLiteConnectionAccount.CreateTableByType (sType);
			} else {
				SQLiteConnectionEditor.CreateTableByType (sType);
			}

			PopulateTable (sType, sAccountConnected);
		}
        //-------------------------------------------------------------------------------------------------------------
		public void PopulateTable (Type sType, bool sAccountConnected)
		{
            //TODO implement code
		}
        //-------------------------------------------------------------------------------------------------------------
		public void EmptyTable (Type sType, bool sAccountConnected)
		{
            //TODO implement code
		}
        //-------------------------------------------------------------------------------------------------------------
		public void DropTable (Type sType, bool sAccountConnected)
		{
			if (sAccountConnected)
            {
				SQLiteConnectionAccount.DropTableByType (sType);
			}
            else
            {
				SQLiteConnectionEditor.DropTableByType (sType);
			}
		}
        //-------------------------------------------------------------------------------------------------------------
		public void ReInitializeTable (Type sType, bool sAccountConnected)
		{
			EmptyTable (sType, sAccountConnected);
			PopulateTable (sType, sAccountConnected);
		}
        //-------------------------------------------------------------------------------------------------------------
		public void ResetTable (Type sType, bool sAccountConnected)
		{
			DropTable (sType, sAccountConnected);
			CreateTable (sType, sAccountConnected);
		}
        //-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================