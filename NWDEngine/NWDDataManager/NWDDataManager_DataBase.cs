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
using System.Threading;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;

using BasicToolBox;
//using ColoredAdvancedDebug;

//=====================================================================================================================
namespace NetWorkedData
{
    public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        public static List<object> kObjectToUpdateQueue = new List<object>();
        //-------------------------------------------------------------------------------------------------------------
        public void ConnectToDatabase()
        {
            if (kConnectedToDatabase == false)
            {
                //BTBBenchmark.Start();
                kConnectedToDatabase = true;
#if UNITY_EDITOR
                // create the good folder
                if (AssetDatabase.IsValidFolder(DatabasePathEditor) == false)
                {
                    AssetDatabase.CreateFolder("Assets", "StreamingAssets");
                }
                // path for base editor
                string tDatabasePathEditor = DatabasePathEditor + "/" + DatabaseNameEditor;
                string tDatabasePathAccount = DatabasePathAccount + "/" + DatabaseNameAccount;

#else
				// Get saved App version from pref
				// check if file exists in Application.persistentDataPath
				string tPathEditor = string.Format ("{0}/{1}", Application.persistentDataPath, DatabaseNameEditor);
				string tPathAccount = string.Format ("{0}/{1}", Application.persistentDataPath, DatabaseNameAccount);
                // if must be update by build version : delete old editor data!
                if (UpdateBuildTimestamp() == true) // must update the editor base
                {
                    //Debug.Log("Application must be updated with the database from bundle! Copy the New database");
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
#elif UNITY_TVOS
                    var loadDb = Application.dataPath + "/Raw/" + DatabaseNameEditor;  // this is the path to your StreamingAssets in iOS
                    // then save to Application.persistentDataPath
                    File.Copy (loadDb, tPathEditor);
#elif UNITY_STANDALONE_OSX
                    var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseNameEditor;
                    // then save to Application.persistentDataPath
                    File.Copy(loadDb, tPathEditor);
#elif UNITY_WP8
                    var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseNameEditor;
				    // then save to Application.persistentDataPath
				    File.Copy(loadDb, tPathEditor);
#elif UNITY_WINRT
                    var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseNameEditor;
				    // then save to Application.persistentDataPath
				    File.Copy(loadDb, tPathEditor);
#elif UNITY_WSA_10_0
                    var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseNameEditor;
                    // then save to Application.persistentDataPath
                    File.Copy(loadDb, tPathEditor);
#elif UNITY_STANDALONE_WIN
                    var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseNameEditor;
                    // then save to Application.persistentDataPath
                    File.Copy(loadDb, tPathEditor);
#elif UNITY_STANDALONE_LINUX
                    var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseNameEditor;
                    // then save to Application.persistentDataPath
                    File.Copy(loadDb, tPathEditor);
#else
                    var loadDb = Application.dataPath + "/Resources/StreamingAssets/" + DatabaseNameEditor;
				    // then save to Application.persistentDataPath
				    File.Copy(loadDb, tPathEditor);
#endif
					// Save App version in pref for futur used
					//BTBPrefsManager.ShareInstance ().set ("APP_VERSION", tBuildTimeStamp);
				}

				string tDatabasePathEditor = tPathEditor;
                string tDatabasePathAccount = tPathAccount;
                //Debug.Log("Application.dataPath = "+Application.dataPath);
                //Debug.Log("tDatabasePathEditor = " + tDatabasePathEditor);
                //Debug.Log("tPathAccount = " + tPathAccount);
#endif
                SQLiteConnectionEditor = new SQLiteConnection(tDatabasePathEditor, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
                SQLiteConnectionAccount = new SQLiteConnection(tDatabasePathAccount, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
                //BTBBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool UpdateBuildTimestamp()
        {
            bool rReturn = false;
            // Get saved App version from pref
            int tBuildTimeStamp = NWDAppConfiguration.SharedInstance().SelectedEnvironment().BuildTimestamp;
            int tBuildTimeStampActual = BTBPrefsManager.ShareInstance().getInt("APP_VERSION");
            // test version
            if (tBuildTimeStamp > tBuildTimeStampActual)
            {
                rReturn = true;
                // delete all sync of data 
                foreach (Type tType in NWDDataManager.SharedInstance().mTypeNotAccountDependantList)
                {
                    var tMethodInfo = tType.GetMethod("SynchronizationUpadteTimestamp", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        tMethodInfo.Invoke(null, null);
                    }
                }
                BTBPrefsManager.ShareInstance().set("APP_VERSION", tBuildTimeStamp);
            }
            // Save App version in pref for futur used
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertObject(object sObject, bool sAccountConnected)
        {
            //BTBBenchmark.Start();
            if (NWDAppEnvironment.SelectedEnvironment().ThreadPoolSQLActive == true)
            {
                ThreadPool.QueueUserWorkItem(InsertObjectThread, new object[] { sObject, sAccountConnected });
            }
            else
            {
                InsertObjectDirect(sObject, sAccountConnected);
            }
            //BTBBenchmark.Finish();
        }

        //-------------------------------------------------------------------------------------------------------------
        public void InsertObjectDirect(object sObject, bool sAccountConnected)
        {
            //BTBBenchmark.Start();
            if (sAccountConnected)
            {
                SQLiteConnectionAccount.Insert(sObject);
            }
            else
            {
                SQLiteConnectionEditor.Insert(sObject);
            }
            BTBNotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_INSERT);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertObjectThread(object sState)
        {
            //BTBBenchmark.Start();
            object[] tParam = sState as object[];
            object sObject = tParam[0];
            bool sAccountConnected = (bool)tParam[1];
            if (sAccountConnected)
            {
                SQLiteConnectionAccount.Insert(sObject);
            }
            else
            {
                SQLiteConnectionEditor.Insert(sObject);
            }
            BTBNotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_INSERT);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateObject(object sObject, bool sAccountConnected)
        {
            //BTBBenchmark.Start();
            if (NWDAppEnvironment.SelectedEnvironment().ThreadPoolSQLActive == true)
            {
                ThreadPool.QueueUserWorkItem(UpdateObjectThread, new object[] { sObject, sAccountConnected });
            }
            else
            {
                UpdateObjectDirect(sObject, sAccountConnected);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateObjectDirect(object sObject, bool sAccountConnected)
        {
            //BTBBenchmark.Start();
            if (sAccountConnected)
            {
                SQLiteConnectionAccount.Update(sObject);
            }
            else
            {
                SQLiteConnectionEditor.Update(sObject);
            }
            BTBNotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_UPDATE);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateObjectThread(object sState)
        {
            //BTBBenchmark.Start();
            object[] tParam = sState as object[];
            object sObject = tParam[0];
            bool sAccountConnected = (bool)tParam[1];
            if (sAccountConnected)
            {
                SQLiteConnectionAccount.Update(sObject);
            }
            else
            {
                SQLiteConnectionEditor.Update(sObject);
            }
            BTBNotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_UPDATE);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteObject(object sObject, bool sAccountConnected)
        {
            //BTBBenchmark.Start();
            if (NWDAppEnvironment.SelectedEnvironment().ThreadPoolSQLActive == true)
            {
                ThreadPool.QueueUserWorkItem(DeleteObjectThread, new object[] { sObject, sAccountConnected });
            }
            else
            {
                DeleteObjectDirect(sObject, sAccountConnected);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteObjectDirect(object sObject, bool sAccountConnected)
        {
            //BTBBenchmark.Start();
            //  update disable with date to delete
            if (sAccountConnected)
            {
                SQLiteConnectionAccount.Delete(sObject);
            }
            else
            {
                SQLiteConnectionEditor.Delete(sObject);
            }
            BTBNotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_DELETE);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteObjectThread(object sState)
        {
            //BTBBenchmark.Start();
            object[] tParam = sState as object[];
            object sObject = tParam[0];
            bool sAccountConnected = (bool)tParam[1];
            //  update disable with date to delete
            if (sAccountConnected)
            {
                SQLiteConnectionAccount.Delete(sObject);
            }
            else
            {
                SQLiteConnectionEditor.Delete(sObject);
            }
            BTBNotificationManager.SharedInstance().PostNotification(sObject, NWDNotificationConstants.K_DATA_LOCAL_DELETE);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddObjectToUpdateQueue(object sObject)
        {
            //BTBBenchmark.Start();
            if (kObjectToUpdateQueue.Contains(sObject) == false)
            {
                kObjectToUpdateQueue.Add(sObject);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public int UpdateQueueCounter()
        {
            return kObjectToUpdateQueue.Count();
        }

        //-------------------------------------------------------------------------------------------------------------
        public void UpdateQueueExecute()
        {
            //BTBBenchmark.Start();
            if (NWDAppEnvironment.SelectedEnvironment().ThreadPoolSQLActive == true)
            {
                ThreadPool.QueueUserWorkItem(UpdateQueueExecuteThread, null);
            }
            else
            {
                UpdateQueueExecuteDirect();
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateQueueExecuteDirect()
        {
            //BTBBenchmark.Start();
            NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
            NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
            foreach (object tObject in kObjectToUpdateQueue)
            {
                Type tType = tObject.GetType();
                var tMethodInfo = tType.GetMethod("UpdateMeQueue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    tMethodInfo.Invoke(tObject, new object[] { true });
                }
            }
            kObjectToUpdateQueue = new List<object>();
            NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
            NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateQueueExecuteThread(object sState)
        {
            //BTBBenchmark.Start();
            NWDDataManager.SharedInstance().SQLiteConnectionAccount.BeginTransaction();
            NWDDataManager.SharedInstance().SQLiteConnectionEditor.BeginTransaction();
            foreach (object tObject in kObjectToUpdateQueue)
            {
                Type tType = tObject.GetType();
                var tMethodInfo = tType.GetMethod("UpdateMeQueue", BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    tMethodInfo.Invoke(tObject, new object[] { true });
                }
            }
            kObjectToUpdateQueue = new List<object>();
            NWDDataManager.SharedInstance().SQLiteConnectionAccount.Commit();
            NWDDataManager.SharedInstance().SQLiteConnectionEditor.Commit();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        // Table management
        public void CreateAllTablesLocal()
        {
            foreach (Type tType in mTypeList)
            {
                var tMethodInfo = tType.GetMethod("CreateTable", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    tMethodInfo.Invoke(null, null);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CleanAllTablesLocal()
        {
            foreach (Type tType in mTypeList)
            {
                var tMethodInfo = tType.GetMethod("CleanTable", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    tMethodInfo.Invoke(null, null);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateAllTablesLocal()
        {
            foreach (Type tType in mTypeList)
            {
                var tMethodInfo = tType.GetMethod("UpdateDataTable", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    tMethodInfo.Invoke(null, null);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetAllTablesLocal()
        {
            foreach (Type tType in mTypeList)
            {
                var tMethodInfo = tType.GetMethod("ResetTable", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    tMethodInfo.Invoke(null, null);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        public void CreateAllTablesServer(NWDAppEnvironment sEnvironment)
        {
            NWDOperationWebManagement.AddOperation("Create table on server", null, null, null, null, sEnvironment, true);
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public void CreateTable(Type sType, bool sAccountConnected)
        {
            ConnectToDatabase();

            if (sAccountConnected)
            {
                SQLiteConnectionAccount.CreateTableByType(sType);
            }
            else
            {
                SQLiteConnectionEditor.CreateTableByType(sType);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void MigrateTable(Type sType, bool sAccountConnected)
        {
            ConnectToDatabase();

            if (sAccountConnected)
            {
                SQLiteConnectionAccount.MigrateTableByType(sType);
            }
            else
            {
                SQLiteConnectionEditor.MigrateTableByType(sType);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void EmptyTable(Type sType, bool sAccountConnected)
        {
            if (sAccountConnected)
            {
                SQLiteConnectionAccount.TruncateTableByType(sType);
            }
            else
            {
                SQLiteConnectionEditor.TruncateTableByType(sType);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DropTable(Type sType, bool sAccountConnected)
        {
            if (sAccountConnected)
            {
                SQLiteConnectionAccount.DropTableByType(sType);
            }
            else
            {
                SQLiteConnectionEditor.DropTableByType(sType);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReInitializeTable(Type sType, bool sAccountConnected)
        {
            EmptyTable(sType, sAccountConnected);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetTable(Type sType, bool sAccountConnected)
        {
            DropTable(sType, sAccountConnected);
            CreateTable(sType, sAccountConnected);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================