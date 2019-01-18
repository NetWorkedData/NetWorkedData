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
    //public class NWDDatasRows
    //{
    //    //-------------------------------------------------------------------------------------------------------------
    //    public string ObjectClass
    //    {
    //        get; set;
    //    }
    //    //-------------------------------------------------------------------------------------------------------------
    //    public string Reference
    //    {
    //        get; set;
    //    }
    //    //-------------------------------------------------------------------------------------------------------------
    //    public string Datas
    //    {
    //        get; set;
    //    }
    //    //-------------------------------------------------------------------------------------------------------------
    //    public string Integrity
    //    {
    //        get; set;
    //    }
    //    //-------------------------------------------------------------------------------------------------------------
    //    public NWDDatasRows(string sObjectClass, string sReference, string sDatas, string sIntegrity)
    //    {
    //        ObjectClass = sObjectClass;
    //        Reference = sReference;
    //        Datas = sDatas;
    //        Integrity = sIntegrity;
    //    }
    //    //-------------------------------------------------------------------------------------------------------------
    //}
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        public static List<object> kObjectToUpdateQueue = new List<object>();
        //-------------------------------------------------------------------------------------------------------------
        public void ConnectToDatabase()
        {
            //Debug.Log("NWDDataManager ConnectToDatabase ()");
            //BTBBenchmark.Start();
            //if (kConnectedToDatabase == true)
            //{
            //    if (SQLiteConnectionAccount.Trace )
            //    {
            //    }
            //}
            if (kConnectedToDatabase == false && kConnectedToDatabaseIsProgress == false)
            {
                kConnectedToDatabaseIsProgress = true;
                //Debug.Log("NWDDataManager ConnectToDatabase () kConnectedToDatabase is false ... connect me!");
#if UNITY_EDITOR
                // create the good folder
                string tAccessPath = Application.dataPath;
                if (Directory.Exists(tAccessPath + "/" + DatabasePathEditor) == false)
                {
                    //Debug.Log("NWDDataManager ConnectToDatabase () path : " + tAccessPath + "/" + DatabasePathEditor);
                    AssetDatabase.CreateFolder("Assets", DatabasePathEditor);
                    AssetDatabase.ImportAsset("Assets/" + DatabasePathEditor);
                    AssetDatabase.Refresh();
                }
                //if (Directory.Exists(tAccessPath + "/" + DatabasePathEditor + "/" + DatabasePathAccount) == false)
                //{
                //    //Debug.Log("NWDDataManager ConnectToDatabase () path : " + tAccessPath + "/" + DatabasePathEditor + "/" + DatabasePathAccount);
                //    AssetDatabase.CreateFolder("Assets/" + DatabasePathEditor, DatabasePathAccount);
                //    AssetDatabase.ImportAsset("Assets/" + DatabasePathEditor + "/" + DatabasePathAccount);
                //    AssetDatabase.Refresh();
                //}

                /*int tTestNumber = 100;
                while (AssetDatabase.IsValidFolder("Assets/" + DatabasePathEditor + "/" + DatabasePathAccount) == false && tTestNumber > 0)
                {
                    tTestNumber--;
                    if (AssetDatabase.IsValidFolder("Assets/" + DatabasePathEditor) == false)
                    {
                        AssetDatabase.CreateFolder("Assets", DatabasePathEditor);
                        AssetDatabase.ImportAsset("Assets/" + DatabasePathEditor);
                        AssetDatabase.Refresh();
                    }
                    if (AssetDatabase.IsValidFolder("Assets/" + DatabasePathEditor + "/" + DatabasePathAccount) == false)
                    {
                        AssetDatabase.CreateFolder("Assets/" + DatabasePathEditor, DatabasePathAccount);
                        AssetDatabase.ImportAsset("Assets/" + DatabasePathEditor + "/" + DatabasePathAccount);
                        AssetDatabase.Refresh();
                    }
                }
                if (tTestNumber == 0)
                {
                    Debug.LogWarning("Impossible to create Database's folder");
                }*/
                // path for base editor
                string tDatabasePathEditor = "Assets/" + DatabasePathEditor + "/" + DatabaseNameEditor;
                //string tDatabasePathAccount = "Assets/" + DatabasePathEditor + "/" + DatabasePathAccount + "/" +
                //NWDAppConfiguration.SharedInstance().DatabasePrefix + DatabaseNameAccount;
                string tDatabasePathAccount = "Assets/" +
                NWDAppConfiguration.SharedInstance().DatabasePrefix + DatabaseNameAccount;
#else
                // Get saved App version from pref
                // check if file exists in Application.persistentDataPath
                string tPathEditor = string.Format ("{0}/{1}", Application.persistentDataPath, DatabaseNameEditor);
                string tPathAccount = string.Format ("{0}/{1}", Application.persistentDataPath, NWDAppConfiguration.SharedInstance().DatabasePrefix + DatabaseNameAccount);
                // if must be update by build version : delete old editor data!
                if (UpdateBuildTimestamp() == true) // must update the editor base
                {
                    Debug.Log("#DATABASE# Application must be updated with the database from bundle! Copy the New database");
                    Debug.Log("#DATABASE# Application will delete database : " + tPathEditor);
                    File.Delete(tPathEditor);
                    Debug.Log("#DATABASE# Application has delete database : " + tPathEditor);
                }
                // Write editor database
                if (!File.Exists (tPathEditor))
                {
                    // if it doesn't ->
                    // open StreamingAssets directory and load the db ->
                    Debug.Log("#DATABASE# Application will copy database : " + tPathEditor);
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
                    Debug.Log("#DATABASE# Application was copied database : " + tPathEditor);
                    Debug.Log("#DATABASE# Database Editor just created");
                    // Save App version in pref for futur used
                    //BTBPrefsManager.ShareInstance ().set ("APP_VERSION", tBuildTimeStamp);
                }
                else
                {
                    Debug.Log("#DATABASE# Database allready exists");
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

                //Debug.Log("#DATABASE# ConnectToDatabase () CONNECTION SQLiteConnectionEditor at " + tDatabasePathEditor);
                SQLiteConnectionEditor = new SQLiteConnection(tDatabasePathEditor,
                tEditorPass,
                SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);


                // RESET TOKEN SYNC OF USER 'S DATAS TO ZERO!
                if (File.Exists(tDatabasePathAccount) == false)
                {
                    // restart with new user!
                    NWDAppEnvironment.SelectedEnvironment().ResetSession();
                    //Debug.Log("#DATABASE#ConnectToDatabase () CONNECTION SQLiteConnectionAccount not exist ");
                    foreach (Type tType in NWDDataManager.SharedInstance().mTypeAccountDependantList)
                    {
                        //Debug.Log("#DATABASE#ConnectToDatabase () CONNECTION SQLiteConnectionAccount reset class sync  to Zero" + tType.FullName);
                        var tMethodInfo = tType.GetMethod("SynchronizationSetToZeroTimestamp", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                        if (tMethodInfo != null)
                        {
                            tMethodInfo.Invoke(null, null);
                        }
                    }
                }


                //Debug.Log("#DATABASE#ConnectToDatabase () CONNECTION SQLiteConnectionAccount at " + tDatabasePathAccount);
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
                while (SQLiteConnectionEditor.IsOpen() == false)
                {
                    Debug.LogWarning("SQLiteConnectionEditor is not opened!");
                    // waiting
                    // TODO : timeout and Mesaage d'erreur : desinstaller app et reinstaller
                    // TODO : Detruire fichier et reinstaller ? 
                }
                while (SQLiteConnectionAccount.IsOpen() == false)
                {
                    Debug.LogWarning("SQLiteConnectionAccount is not opened!");
                    // waiting
                    // TODO : timeout and Mesaage d'erreur : desinstaller app et reinstaller
                    // TODO : Detruire fichier et reinstaller ? 
                }
                // finish
                kConnectedToDatabase = true;
                kConnectedToDatabaseIsProgress = false;
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteDatabase()
        {
            //Debug.Log("DeleteDatabase ()");
            kConnectedToDatabase = false;
            //Close SLQite
            if (SQLiteConnectionEditor != null)
            {
                SQLiteConnectionEditor.Close();
            }
            SQLiteConnectionEditor = null;
            if (SQLiteConnectionAccount != null)
            {
                SQLiteConnectionAccount.Close();
            }
            SQLiteConnectionAccount = null;
            // reload empty object
            NWDDataManager.SharedInstance().ReloadAllObjects();
            // database is not connected
            kConnectedToDatabase = false;
#if UNITY_EDITOR
            if (AssetDatabase.IsValidFolder("Assets/" + DatabasePathEditor) == false)
            {
                AssetDatabase.CreateFolder("Assets", DatabasePathEditor);
            }
            // path for base editor
            //if (AssetDatabase.IsValidFolder("Assets/" + DatabasePathEditor + "/" + DatabasePathAccount) == false)
            //{
            //    AssetDatabase.CreateFolder("Assets/" + DatabasePathEditor, DatabasePathAccount);
            //}
            // path for base editor
            string tDatabasePathEditor = "Assets/" + DatabasePathEditor + "/" + DatabaseNameEditor;
            //string tDatabasePathAccount = "Assets/" + DatabasePathEditor + "/" + DatabasePathAccount + "/" +
             //NWDAppConfiguration.SharedInstance().DatabasePrefix + DatabaseNameAccount;
            string tDatabasePathAccount = "Assets/" +
             NWDAppConfiguration.SharedInstance().DatabasePrefix + DatabaseNameAccount;

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
            // delete DataBase
            DeleteDatabase();
            bool tCSharpRegenerate = false;
            if (sRegeneratePassword == true)
            {
                NWDAppConfiguration.SharedInstance().EditorPass = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(24, 36));
                NWDAppConfiguration.SharedInstance().EditorPassA = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(12, 18));
                NWDAppConfiguration.SharedInstance().EditorPassB = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(12, 18));
                // force data base to be copy on next install!
                int tTimeStamp = NWDToolbox.Timestamp();
                NWDAppConfiguration.SharedInstance().DevEnvironment.BuildTimestamp = tTimeStamp;
                NWDAppConfiguration.SharedInstance().PreprodEnvironment.BuildTimestamp = tTimeStamp;
                NWDAppConfiguration.SharedInstance().ProdEnvironment.BuildTimestamp = tTimeStamp;
                tCSharpRegenerate = true;
            }
            if (sRegenerateDeviceSalt == true)
            {
                //NWDAppConfiguration.SharedInstance().DatabasePrefix = "NWD_" +
                //NWDAppConfiguration.SharedInstance().DatabasePrefix.Replace("NWD_","") +
                //NWDToolbox.RandomStringAlpha(UnityEngine.Random.Range(1, 1));
                NWDAppConfiguration.SharedInstance().DatabasePrefix = "NWD" + BTBDateHelper.ConvertToTimestamp(DateTime.Now).ToString("F0");
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

                //Debug.Log("#DATABASE# Database must upadte by bundle mTypeNotAccountDependantList count " + mTypeNotAccountDependantList.Count());

                foreach (Type tType in NWDDataManager.SharedInstance().mTypeNotAccountDependantList)
                {
                    //Debug.Log("#DATABASE#ConnectToDatabase () CONNECTION SQLiteConnectionEditor reset class sync " + tType.FullName);
                    var tMethodInfo = tType.GetMethod("SynchronizationUpadteTimestamp", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        tMethodInfo.Invoke(null, null);
                    }
                }
                BTBPrefsManager.ShareInstance().set("APP_VERSION", tBuildTimeStamp);
            }
            // Save App version in pref for futur used
            if (rReturn == true)
            {
                //Debug.Log("#DATABASE# Database must upadte by bundle");
            }
            else
            {
                //Debug.Log("#DATABASE# Database is ok (no update needed)");
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CreateAllTablesLocal()
        {
            if (kConnectedToDatabase == true && kConnectedToDatabaseIsProgress == false)
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
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CleanAllTablesLocal()
        {
            if (kConnectedToDatabase == true && kConnectedToDatabaseIsProgress == false)
            {
                foreach (Type tType in mTypeList)
                {
                    string tMethodName  = NWDAliasMethod.FindAliasName(tType, "CleanTable");
                    var tMethodInfo = tType.GetMethod(tMethodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        tMethodInfo.Invoke(null, null);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PurgeAllTablesLocal()
        {
            if (kConnectedToDatabase == true && kConnectedToDatabaseIsProgress == false)
            {
                foreach (Type tType in mTypeList)
                {
                    string tMethodName = NWDAliasMethod.FindAliasName(tType, "PurgeTable");
                    var tMethodInfo = tType.GetMethod(tMethodName, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                    if (tMethodInfo != null)
                    {
                        tMethodInfo.Invoke(null, null);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateAllTablesLocal()
        {
            if (kConnectedToDatabase == true && kConnectedToDatabaseIsProgress == false)
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
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetAllTablesLocal()
        {
            if (kConnectedToDatabase == true && kConnectedToDatabaseIsProgress == false)
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
            if (kConnectedToDatabase == true && kConnectedToDatabaseIsProgress == false)
            {
                if (sAccountConnected)
                {
                    if (SQLiteConnectionAccount != null)
                    {
                        SQLiteConnectionAccount.CreateTableByType(sType);
                        //SQLiteConnectionAccountV4.CreateTableByType(typeof(NWDDatasRows));
                    }
                }
                else
                {
                    if (SQLiteConnectionEditor != null)
                    {
                        SQLiteConnectionEditor.CreateTableByType(sType);
                        //SQLiteConnectionEditorV4.CreateTableByType(typeof(NWDDatasRows));
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void MigrateTable(Type sType, bool sAccountConnected)
        {
            if (kConnectedToDatabase == true && kConnectedToDatabaseIsProgress == false)
            {
                if (sAccountConnected)
                {
                    if (SQLiteConnectionAccount != null)
                    {
                        SQLiteConnectionAccount.MigrateTableByType(sType);
                        //SQLiteConnectionAccountV4.MigrateTableByType(typeof(NWDDatasRows));
                    }
                }
                else
                {
                    if (SQLiteConnectionEditor != null)
                    {
                        SQLiteConnectionEditor.MigrateTableByType(sType);
                        //SQLiteConnectionEditorV4.MigrateTableByType(typeof(NWDDatasRows));
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void EmptyTable(Type sType, bool sAccountConnected)
        {
            if (kConnectedToDatabase == true && kConnectedToDatabaseIsProgress == false)
            {
                if (sAccountConnected)
                {
                    if (SQLiteConnectionAccount != null)
                    {
                        SQLiteConnectionAccount.TruncateTableByType(sType);
                        //SQLiteConnectionAccountV4.TruncateTableByType(typeof(NWDDatasRows));
                    }
                }
                else
                {
                    if (SQLiteConnectionEditor != null)
                    {
                        SQLiteConnectionEditor.TruncateTableByType(sType);
                        //SQLiteConnectionEditorV4.TruncateTableByType(typeof(NWDDatasRows));
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DropTable(Type sType, bool sAccountConnected)
        {
            if (kConnectedToDatabase == true && kConnectedToDatabaseIsProgress == false)
            {
                if (sAccountConnected)
                {
                    if (SQLiteConnectionAccount != null)
                    {
                        SQLiteConnectionAccount.DropTableByType(sType);
                        //SQLiteConnectionAccountV4.DropTableByType(typeof(NWDDatasRows));
                    }
                }
                else
                {
                    if (SQLiteConnectionEditor != null)
                    {
                        SQLiteConnectionEditor.DropTableByType(sType);
                        //SQLiteConnectionEditorV4.DropTableByType(typeof(NWDDatasRows));
                    }
                }
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