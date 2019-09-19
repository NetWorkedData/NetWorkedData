//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:27:6
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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

//using BasicToolBox;
//using ColoredAdvancedDebug;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        public static List<object> kObjectToUpdateQueue = new List<object>();
        //-------------------------------------------------------------------------------------------------------------
        public bool ConnectToDatabaseEditor()
        {
            // waiting ... do nothing
            bool rReturn = true;
            NWEBenchmark.Start();
            if (DataEditorConnected == false && DataEditorConnectionInProgress == false)
            {
                DataEditorConnectionInProgress = true;
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
                // path for base editor
                string tDatabasePathEditor = "Assets/" + DatabasePathEditor + "/" + DatabaseNameEditor;
#else
                // Get saved App version from pref
                // check if file exists in Application.persistentDataPath
                string tPathEditor = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseNameEditor);
                // if must be update by build version : delete old editor data!
                if (UpdateBuildTimestamp() == true) // must update the editor base
                {
                    File.Delete(tPathEditor);
                }
                // Write editor database
                if (!File.Exists(tPathEditor))
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
                }

                string tDatabasePathEditor = tPathEditor;
#endif
                string tEditorPass = NWDAppConfiguration.SharedInstance().GetEditorPass();
                if (NWDAppEnvironment.SelectedEnvironment() == NWDAppConfiguration.SharedInstance().DevEnvironment ||
                    NWDAppEnvironment.SelectedEnvironment() == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
                {
                    Debug.Log("ConnectToDatabaseEditor () tDatabasePathEditor : " + tDatabasePathEditor + " : " + tEditorPass);
                }

                SQLiteConnectionEditor = new SQLiteConnection(tDatabasePathEditor, tEditorPass, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

                double tSeconds = SQLiteConnectionEditor.BusyTimeout.TotalSeconds;
                DateTime t = DateTime.Now;
                DateTime tf = DateTime.Now.AddSeconds(tSeconds);
                while (t < tf)
                {
                    t = DateTime.Now;
                }
                //waiting the tables and file will be open...
                while (SQLiteConnectionEditor.IsOpen() == false)
                {
                    //Debug.LogWarning("SQLiteConnectionEditor is not opened!");
                }
                
                // finish test opened database
                rReturn = SQLiteConnectionEditor.IsValid();
                if (rReturn == true)
                {
                    DataEditorConnected = true;
                    //Debug.LogWarning("SQLiteConnectionEditor is valid!");
                }
                else
                {
                    DataEditorConnected = false;
                    Debug.LogWarning("SQLiteConnectionEditor is not valid!");
                }
                DataEditorConnectionInProgress = false;
            }
            else
            {
                if (DataEditorConnected == true)
                {
                    Debug.LogWarning("SQLiteConnectionEditor allready connected");
                }
                if (DataEditorConnectionInProgress == true)
                {
                    Debug.LogWarning("SQLiteConnectionEditor connexion in progress");
                }
            }
            NWEBenchmark.Finish();

            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string PathDatabaseAccount()
        {
            string rReturn = string.Empty;
#if UNITY_EDITOR
            // path for base editor
            //rReturn = Application.dataPath + "/" + NWDAppConfiguration.SharedInstance().DatabasePrefix + DatabaseNameAccount;
            rReturn = "Assets/" + NWDAppConfiguration.SharedInstance().DatabasePrefix + DatabaseNameAccount;
#else
            rReturn = string.Format("{0}/{1}", Application.persistentDataPath, NWDAppConfiguration.SharedInstance().DatabasePrefix + DatabaseNameAccount);
#endif
            //Debug.Log("<color=orange>PathDatabaseAccount return :" + rReturn + "</color>");
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool DatabaseAccountExists()
        {
            bool rReturn = false;
            rReturn = File.Exists(PathDatabaseAccount());
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ConnectToDatabaseAccount(string sSurProtection)
        {
            bool rReturn = true;
            NWEBenchmark.Start();
            //Debug.LogWarning("ConnectToDatabaseAccount (" + sSurProtection + ")");
            if (DataAccountConnected == false && DataAccountConnectionInProgress == false)
            {
                DataAccountConnectionInProgress = true;
                string tDatabasePathAccount = PathDatabaseAccount();
                string tAccountPass = NWDAppConfiguration.SharedInstance().GetAccountPass(sSurProtection);
                if (NWDAppEnvironment.SelectedEnvironment() == NWDAppConfiguration.SharedInstance().DevEnvironment
                || NWDAppEnvironment.SelectedEnvironment() == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
                {
                    Debug.Log("ConnectToDatabaseAccount () tDatabasePathAccount : " + tDatabasePathAccount + " : " + tAccountPass);
                }

                //if (NWDAppConfiguration.SharedInstance().SurProtected == true)
                //{
                //    if (!File.Exists(tDatabasePathAccount) && string.IsNullOrEmpty(sSurProtection))
                //    {
                //        Debug.LogWarning("NEED NEW DATABASE ACCOUNT");
                //        NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_NEEDED);
                //        NWDTypeLauncher.CodePinCreationNeeded = true;
                //        rReturn = false;
                //    }
                //}
                //if (rReturn == true)
                //{
                SQLiteConnectionAccount = new SQLiteConnection(tDatabasePathAccount, tAccountPass, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

                double tSeconds = SQLiteConnectionAccount.BusyTimeout.TotalSeconds;
                DateTime t = DateTime.Now;
                DateTime tf = DateTime.Now.AddSeconds(tSeconds);
                while (t < tf)
                {
                    t = DateTime.Now;
                }
                //waiting the tables and file will be open...
                while (SQLiteConnectionAccount.IsOpen() == false)
                {
                    //Debug.LogWarning("SQLiteConnectionAccount is not opened!");
                }
                // finish test opened database
                rReturn = SQLiteConnectionAccount.IsValid();
                if (rReturn == true)
                {
                    DataAccountConnected = true;
                    //Debug.LogWarning("SQLiteConnectionAccount is valid!");
                }
                else
                {
                    DataAccountConnected = false;
                    Debug.LogWarning("SQLiteConnectionAccount is not valid!");
                }
                DataAccountConnectionInProgress = false;
                //}
            }
            else
            {
                if (DataAccountConnected == true)
                {
                    Debug.LogWarning("SQLiteConnectionAccount allready connected");
                }
                if (DataAccountConnectionInProgress == true)
                {
                    Debug.LogWarning("SQLiteConnectionAccount connexion in progress");
                }
            }
            NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeconnectFromDatabaseAccount()
        {
            NWDLauncher.DeconnectFromDatabaseAccount();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteDatabaseAccount()
        {
            NWDLauncher.DeleteDatabaseAccount();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteDatabaseEditor()
        {
            //Debug.Log("DeleteDatabase ()");
            DataEditorConnected = false;
            //Close SLQite
            if (SQLiteConnectionEditor != null)
            {
                //SQLiteConnectionEditor.Commit();
                //SQLiteConnectionEditor.Dispose();
                SQLiteConnectionEditor.Close();
            }
            SQLiteConnectionEditor = null;
            // reload empty object
            ReloadAllObjectsEditor();
            // database is not connected
#if UNITY_EDITOR
            if (AssetDatabase.IsValidFolder("Assets/" + DatabasePathEditor) == false)
            {
                AssetDatabase.CreateFolder("Assets", DatabasePathEditor);
            }
            string tDatabasePathEditor = "Assets/" + DatabasePathEditor + "/" + DatabaseNameEditor;

            File.Delete(tDatabasePathEditor);
#else
            string tPathEditor = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseNameEditor);
            File.Delete(tPathEditor);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteDatabase()
        {
            //Debug.Log("DeleteDatabase ()");
            DeleteDatabaseAccount();
            DeleteDatabaseEditor();
        }
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public void RecreateDatabase(bool sRegeneratePassword = false, bool sRegenerateDeviceSalt = false)
        {
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
                NWDAppConfiguration.SharedInstance().DatabasePrefix = "NWD" + NWEDateHelper.ConvertToTimestamp(DateTime.Now).ToString("F0");
                NWDAppConfiguration.SharedInstance().AccountHashSalt = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(24, 36));
                NWDAppConfiguration.SharedInstance().AccountHashSaltA = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(12, 18));
                NWDAppConfiguration.SharedInstance().AccountHashSaltB = NWDToolbox.RandomStringCypher(UnityEngine.Random.Range(12, 18));
                tCSharpRegenerate = true;
            }
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
            int tBuildTimeStampActual = NWEPrefsManager.ShareInstance().getInt("APP_VERSION");
            // test version
            if (tBuildTimeStamp > tBuildTimeStampActual)
            {
                rReturn = true;

                // !!! NOT POSSIBLLE BECAUSE EDITOR DATABASE IS NOT LOADED!!!
                // delete all sync of data 
                //foreach (Type tType in NWDDataManager.SharedInstance().mTypeNotAccountDependantList)
                //{
                //    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                //    tHelper.New_SynchronizationSetNewTimestamp(NWDAppConfiguration.SharedInstance().SelectedEnvironment(), tBuildTimeStamp);
                //}
                NWEPrefsManager.ShareInstance().set("APP_VERSION", tBuildTimeStamp);
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
        public void CreateAllTablesLocalAccount()
        {
            if (DataAccountConnected == true && DataAccountConnectionInProgress == false)
            {
                foreach (Type tType in mTypeAccountDependantList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.CreateTable();
                    //Debug.Log("<color=orange>CreateAllTablesLocalAccount() create Datas </color>");
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_CreateTable);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CleanAllTablesLocalAccount()
        {
            if (DataAccountConnected == true && DataAccountConnectionInProgress == false)
            {
                foreach (Type tType in mTypeAccountDependantList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.CleanTable();
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_CleanTable);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PurgeAllTablesLocalAccount()
        {
            if (DataAccountConnected == true && DataAccountConnectionInProgress == false)
            {
                foreach (Type tType in mTypeAccountDependantList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.PurgeTable();
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_PurgeTable);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateAllTablesLocalAccount()
        {
            if (DataAccountConnected == true && DataAccountConnectionInProgress == false)
            {
                foreach (Type tType in mTypeAccountDependantList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.UpdateDataTable();
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_UpdateDataTable);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetAllTablesLocalAccount()
        {
            if (DataAccountConnected == true && DataAccountConnectionInProgress == false)
            {
                foreach (Type tType in mTypeAccountDependantList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.ResetTable();
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_ResetTable);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CreateAllTablesLocalEditor()
        {
            if (DataEditorConnected == true && DataEditorConnectionInProgress == false)
            {
                foreach (Type tType in mTypeNotAccountDependantList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.CreateTable();
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_CreateTable);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CleanAllTablesLocalEditor()
        {
            if (DataEditorConnected == true && DataEditorConnectionInProgress == false)
            {
                foreach (Type tType in mTypeNotAccountDependantList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.CleanTable();
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_CleanTable);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PurgeAllTablesLocalEditor()
        {
            if (DataEditorConnected == true && DataEditorConnectionInProgress == false)
            {
                foreach (Type tType in mTypeNotAccountDependantList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.PurgeTable();
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_PurgeTable);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UpdateAllTablesLocalEditor()
        {
            if (DataEditorConnected == true && DataEditorConnectionInProgress == false)
            {
                foreach (Type tType in mTypeNotAccountDependantList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.UpdateDataTable();
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_UpdateDataTable);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetAllTablesLocalEditor()
        {
            if (DataEditorConnected == true && DataEditorConnectionInProgress == false)
            {
                foreach (Type tType in mTypeNotAccountDependantList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.ResetTable();
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_ResetTable);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CreateTable(Type sType, bool sAccountConnected)
        {

            if (sAccountConnected)
            {
                if (SQLiteConnectionAccountIsValid())
                {
                    //Debug.Log("<color=green>CreateTable() account" + sType.Name + " </color>");
                    SQLiteConnectionAccount.CreateTableByType(sType);
                }
            }
            else
            {
                if (SQLiteConnectionEditorIsValid())
                {
                    //Debug.Log("<color=green>CreateTable() editor" + sType.Name + " </color>");
                    SQLiteConnectionEditor.CreateTableByType(sType);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void MigrateTable(Type sType, bool sAccountConnected)
        {

            if (sAccountConnected)
            {
                //if (DataAccountConnected == true && DataAccountConnectionInProgress == false)
                //{
                if (SQLiteConnectionAccountIsValid())
                {
                    SQLiteConnectionAccount.MigrateTableByType(sType);
                }
                //}
            }
            else
            {
                //if (DataEditorConnected == true && DataEditorConnectionInProgress == false)
                //{
                if (SQLiteConnectionEditorIsValid())
                {
                    SQLiteConnectionEditor.MigrateTableByType(sType);
                }
                //}
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void EmptyTable(Type sType, bool sAccountConnected)
        {

            if (sAccountConnected)
            {
                //if (DataAccountConnected == true && DataAccountConnectionInProgress == false)
                //{
                if (SQLiteConnectionAccountIsValid())
                {
                    SQLiteConnectionAccount.TruncateTableByType(sType);
                }
                //}
            }
            else
            {
                //if (DataEditorConnected == true && DataEditorConnectionInProgress == false)
                //{
                if (SQLiteConnectionEditorIsValid())
                {
                    SQLiteConnectionEditor.TruncateTableByType(sType);
                }
                //}
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DropTable(Type sType, bool sAccountConnected)
        {

            if (sAccountConnected)
            {
                //if (DataAccountConnected == true && DataAccountConnectionInProgress == false)
                //{
                if (SQLiteConnectionAccountIsValid())
                {
                    SQLiteConnectionAccount.DropTableByType(sType);
                }
                //}
            }
            else
            {
                //if (DataEditorConnected == true && DataEditorConnectionInProgress == false)
                //{
                if (SQLiteConnectionEditorIsValid())
                {
                    SQLiteConnectionEditor.DropTableByType(sType);
                }
                //}
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
        public bool SQLiteConnectionEditorIsValid()
        {
            bool rReturn = true;
            if (SQLiteConnectionEditor != null)
            {
                rReturn = SQLiteConnectionEditor.IsValid();
            }
            else
            {
                rReturn = false;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool SQLiteConnectionAccountIsValid()
        {
            bool rReturn = true;
            if (SQLiteConnectionAccount != null)
            {
                rReturn = SQLiteConnectionAccount.IsValid();
            }
            else
            {
                rReturn = false;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================