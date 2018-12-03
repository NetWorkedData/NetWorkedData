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
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDDatasRows
    {
        //-------------------------------------------------------------------------------------------------------------
        public string ObjectClass
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Reference
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Datas
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Integrity
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDDatasRows(string sObjectClass, string sReference, string sDatas, string sIntegrity)
        {
            ObjectClass = sObjectClass;
            Reference = sReference;
            Datas = sDatas;
            Integrity = sIntegrity;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        public static List<object> kObjectToUpdateQueue = new List<object>();
        //-------------------------------------------------------------------------------------------------------------
        public void ConnectToDatabase()
        {
            //Debug.Log("ConnectToDatabase ()");
            //BTBBenchmark.Start();
            //if (kConnectedToDatabase == true)
            //{
            //    if (SQLiteConnectionAccount.Trace )
            //    {
            //    }
            //}
            if (kConnectedToDatabase == false)
            {
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
                //SQLiteConnection conn = new SQLiteConnection("Data Source=MyDatabase.sqlite;Version=3;");
                //conn.("password");
                //Debug.Log("ConnectToDatabase () CONNECTION PREPARE");
                //Debug.Log("ConnectToDatabase () tDatabasePathEditor : " + tDatabasePathEditor);
                //Debug.Log("ConnectToDatabase () tDatabasePathAccount : " + tDatabasePathAccount);

                string tAccountPass = NWDAppConfiguration.SharedInstance().GetAccountPass();
                //Debug.Log("ConnectToDatabase () tAccountPass : " + tAccountPass);
                string tEditorPass = NWDAppConfiguration.SharedInstance().GetEditorPass();
                //Debug.Log("ConnectToDatabase () tEditorPass : " + tEditorPass);

                //Debug.Log("ConnectToDatabase () CONNECTION SQLiteConnectionEditor");
                SQLiteConnectionEditor = new SQLiteConnection(tDatabasePathEditor,
                tEditorPass,
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

                //Debug.Log("ConnectToDatabase () CONNECTION SQLiteConnectionAccount");
                SQLiteConnectionAccount = new SQLiteConnection(tDatabasePathAccount,
                tAccountPass,
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

                //Debug.Log("ConnectToDatabase () CONNECTION SQLiteConnectionAccount.BusyTimeout" + SQLiteConnectionAccount.BusyTimeout.ToString());
                // waiting the tables and file will be open...
                // TODO: REAL DISPO! MARCHE PAS?!
                double tSeconds = SQLiteConnectionAccount.BusyTimeout.TotalSeconds +
                SQLiteConnectionEditor.BusyTimeout.TotalSeconds;
                //Debug.Log("ConnectToDatabase () CONNECTION tSeconds : " + tSeconds.ToString());
                // BYPASS 
                // tSeconds = 0.2;

                DateTime t = DateTime.Now;
                DateTime tf = DateTime.Now.AddSeconds(tSeconds);
                while (t < tf)
                {
                    t = DateTime.Now;
                }

                // TEST WHILE / MARCHE PAS 
                while (SQLiteConnectionAccount.IsOpen() == false && SQLiteConnectionEditor.IsOpen() == false)
                {
                    // waiting
                    // TODO : timeout and Mesaage d'erreur : desinstaller app et reinstaller
                    // TODO : Detruire fichier et reinstaller ? 
                }


                //SQLiteConnectionEditorV4 = new SQLiteConnection(tDatabasePathEditor + "ssl", tEditorPass, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

                //SQLiteConnectionAccountV4 = new SQLiteConnection(tDatabasePathAccount + "ssl", tAccountPass, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteDatabase()
        {
            //Debug.Log("DeleteDatabase ()");
            if (SQLiteConnectionAccount != null)
            {
                SQLiteConnectionAccount.Close();
            }
            if (SQLiteConnectionEditor != null)
            {
                SQLiteConnectionEditor.Close();
            }
            kConnectedToDatabase = false;
#if UNITY_EDITOR
            if (AssetDatabase.IsValidFolder(DatabasePathEditor) == false)
            {
                AssetDatabase.CreateFolder("Assets", "StreamingAssets");
            }
            // path for base editor
            string tDatabasePathEditor = DatabasePathEditor + "/" + DatabaseNameEditor;
            string tDatabasePathAccount = DatabasePathAccount + "/" + DatabaseNameAccount;
            File.Delete(tDatabasePathEditor);
            File.Delete(tDatabasePathAccount);
#else
				string tPathEditor = string.Format ("{0}/{1}", Application.persistentDataPath, DatabaseNameEditor);
				string tPathAccount = string.Format ("{0}/{1}", Application.persistentDataPath, DatabaseNameAccount);
                File.Delete(tPathEditor);
                File.Delete(tPathAccount);
#endif
        }
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public void RecreateDatabase(bool sRegeneratePassword = false, bool sRegenerateDeviceSalt = false)
        {
            //Debug.Log("RecreateDatabase ()");
            kConnectedToDatabase = false;
            //Close SLQite
            SQLiteConnectionEditor.Close();
            SQLiteConnectionEditor = null;
            SQLiteConnectionAccount.Close();
            SQLiteConnectionAccount = null;
            // reload empty object
            NWDDataManager.SharedInstance().ReloadAllObjects();
            // delete DataBase
            DeleteDatabase();
            bool tCSharpRegenerate = false;
            if (sRegeneratePassword == true)
            {
                NWDAppConfiguration.SharedInstance().EditorPass = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(24, 36));
                NWDAppConfiguration.SharedInstance().EditorPassA = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(12, 18));
                NWDAppConfiguration.SharedInstance().EditorPassB = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(12, 18));
                tCSharpRegenerate = true;
            }
            if (sRegeneratePassword == true)
            {
                NWDAppConfiguration.SharedInstance().AccountHashSalt = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(24, 36));
                NWDAppConfiguration.SharedInstance().AccountHashSaltA = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(12, 18));
                NWDAppConfiguration.SharedInstance().AccountHashSaltB = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(12, 18));
                tCSharpRegenerate = true;
            }
            //ConnectToDatabase(); // Do by tCSharpRegenerate
            if (tCSharpRegenerate == true)
            {
                NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            }
        }
#endif
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
                //SQLiteConnectionAccountV4.CreateTableByType(typeof(NWDDatasRows));
            }
            else
            {
                SQLiteConnectionEditor.CreateTableByType(sType);
                //SQLiteConnectionEditorV4.CreateTableByType(typeof(NWDDatasRows));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void MigrateTable(Type sType, bool sAccountConnected)
        {
            ConnectToDatabase();

            if (sAccountConnected)
            {
                SQLiteConnectionAccount.MigrateTableByType(sType);
                //SQLiteConnectionAccountV4.MigrateTableByType(typeof(NWDDatasRows));
            }
            else
            {
                SQLiteConnectionEditor.MigrateTableByType(sType);
                //SQLiteConnectionEditorV4.MigrateTableByType(typeof(NWDDatasRows));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void EmptyTable(Type sType, bool sAccountConnected)
        {
            if (sAccountConnected)
            {
                SQLiteConnectionAccount.TruncateTableByType(sType);
                //SQLiteConnectionAccountV4.TruncateTableByType(typeof(NWDDatasRows));
            }
            else
            {
                SQLiteConnectionEditor.TruncateTableByType(sType);
                //SQLiteConnectionEditorV4.TruncateTableByType(typeof(NWDDatasRows));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DropTable(Type sType, bool sAccountConnected)
        {
            if (sAccountConnected)
            {
                SQLiteConnectionAccount.DropTableByType(sType);
                //SQLiteConnectionAccountV4.DropTableByType(typeof(NWDDatasRows));
            }
            else
            {
                SQLiteConnectionEditor.DropTableByType(sType);
                //SQLiteConnectionEditorV4.DropTableByType(typeof(NWDDatasRows));
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
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================