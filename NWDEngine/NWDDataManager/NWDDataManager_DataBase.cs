//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#endif
//=====================================================================================================================

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System.Text;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
            if (NWDLauncher.ActiveBenchmark)
            {
                NWDBenchmark.Start();
                NWDBenchmark.Log("LibVersionNumber() = " + SQLite3.LibVersionNumber());
            }
            bool rReturn = false;
            if (EditorDatabaseConnected == false && EditorDatabaseConnectionInProgress == false)
            {
                EditorDatabaseConnectionInProgress = true;
#if UNITY_EDITOR
                // create the good folder
                string tAccessPath = Application.dataPath;
                if (Directory.Exists(tAccessPath + "/" + NWD.K_StreamingAssets) == false)
                {
                    AssetDatabase.CreateFolder(NWD.K_Assets, NWD.K_StreamingAssets);
                    AssetDatabase.ImportAsset(NWD.K_Assets+"/" + NWD.K_StreamingAssets);
                    AssetDatabase.Refresh();
                }
                // path for base editor
                string tDatabasePathEditor = NWD.K_Assets+"/" + NWD.K_StreamingAssets + "/" + DatabaseEditorName();
#else
                // Get saved App version from pref
                // check if file exists in Application.persistentDataPath
#if (UNITY_TVOS)
                string tPathEditor = string.Format("{0}/{1}", Application.temporaryCachePath, DatabaseEditorName());
#else
                string tPathEditor = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseEditorName());
#endif

                // if must be update by build version : delete old editor data!
                if (UpdateBuildTimestamp() == true) // must update the editor base
                {
                    File.Delete(tPathEditor);
                }
                // Write editor database
                if (!File.Exists(tPathEditor))
                {
                    if (NWDLauncher.ActiveBenchmark)
                    {
                        NWEBenchmark.Start("Copy editor");
                    }
                    // if it doesn't ->
                    // open StreamingAssets directory and load the db ->
                    if (NWDLauncher.ActiveBenchmark)
                    {
                        NWEBenchmark.Log("Application will copy editor database : " + tPathEditor);
                    }
                    NWDLauncher.CopyDatabase = true;
#if UNITY_ANDROID
                    var tLoadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseEditorName());  // this is the path to your StreamingAssets in android
                    while (!tLoadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
                    // then save to Application.persistentDataPath
                    File.WriteAllBytes(tPathEditor, tLoadDb.bytes);
#elif (UNITY_IOS)
                    var tLoadDb = Application.dataPath + "/Raw/" + DatabaseEditorName();  // this is the path to your StreamingAssets in iOS
                    File.Copy(tLoadDb, tPathEditor);
#elif (UNITY_TVOS)
                    string tLoadDb = Application.dataPath + "/Raw/" + DatabaseEditorName();  // this is the path to your StreamingAssets in tvOS
                    Debug.Log(" read file in " + tLoadDb);
                    Debug.Log(" copy file to " + tPathEditor);
                    File.Copy(tLoadDb, tPathEditor);
#elif (UNITY_STANDALONE_OSX)
                    var tLoadDb = Application.dataPath + "/" + NWD.K_Resources + "/Data/"+NWD.K_StreamingAssets + "/" + DatabaseEditorName();
                    File.Copy(tLoadDb, tPathEditor);
#elif (UNITY_WP8 || UNITY_WINRT || UNITY_WSA_10_0 || UNITY_STANDALONE_WIN || UNITY_STANDALONE_LINUX)
                    var tLoadDb = Application.dataPath + "/" + NWD.K_StreamingAssets + "/" + DatabaseEditorName();
                    File.Copy(tLoadDb, tPathEditor);
#else
                    var tLoadDb = Application.dataPath + "/" + NWD.K_Resources + "/" + NWD.K_StreamingAssets + "/" + DatabaseEditorName();
                    File.Copy(tLoadDb, tPathEditor);
#endif
                    if (NWDLauncher.ActiveBenchmark)
                    {
                        NWEBenchmark.Finish("Copy editor");
                    }
                }
                string tDatabasePathEditor = tPathEditor;
#endif
                //Debug.Log(" tDatabasePathEditor =  " + tDatabasePathEditor);

                byte[] tDatabasePathAsBytes = GetNullTerminatedUtf8(tDatabasePathEditor);
                SQLite3.Result tResult = SQLite3.Open(tDatabasePathAsBytes, out SQLiteEditorHandle, (int)(SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create), IntPtr.Zero);
                if (tResult != SQLite3.Result.OK)
                {
                    throw SQLiteException.New(tResult, string.Format("Could not open database file: {0} ({1})", tDatabasePathEditor, tResult));
                }
                else
                {

                    DatabaseEditorOpenKey(tDatabasePathEditor);

                    EditorDatabaseConnected = true;
                    EditorDatabaseConnectionInProgress = false;
                    //IntPtr stmtpragmaX = SQLite3.Prepare2(SQLiteEditorHandle, "PRAGMA cipher_memory_security = OFF;");
                    //SQLite3.Step(stmtpragmaX);
                    //SQLite3.Finalize(stmtpragmaX);
                    IntPtr stmtpragma = SQLite3.Prepare2(SQLiteEditorHandle, "PRAGMA synchronous = OFF;");
                    SQLite3.Step(stmtpragma);
                    SQLite3.Finalize(stmtpragma);
                    IntPtr stmtpragmaB = SQLite3.Prepare2(SQLiteEditorHandle, "PRAGMA journal_mode = MEMORY");
                    SQLite3.Step(stmtpragmaB);
                    SQLite3.Finalize(stmtpragmaB);
                    //Sqlite3DatabaseHandle stmtpragmaC = SQLite3.Prepare2(SQLiteEditorHandle, "PRAGMA cache_size = 1000000");
                    //SQLite3.Step(stmtpragmaC);
                    //SQLite3.Finalize(stmtpragmaC);
                    //Sqlite3DatabaseHandle stmtpragmaD = SQLite3.Prepare2(SQLiteEditorHandle, "PRAGMA temp_store = MEMORY");
                    //SQLite3.Step(stmtpragmaD);
                    //SQLite3.Finalize(stmtpragmaD);

                    if (NWDAppEnvironment.SelectedEnvironment() == NWDAppConfiguration.SharedInstance().DevEnvironment ||
                        NWDAppEnvironment.SelectedEnvironment() == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
                    {
                        var fileInfo = new System.IO.FileInfo(tDatabasePathEditor);
                        if (IsSecure())
                        {
                            NWDBenchmark.Log("ConnectToDatabaseEditor () tDatabasePathEditor : " + tDatabasePathEditor + " (" + fileInfo.Length + " octets) : " + NWDAppConfiguration.SharedInstance().GetEditorPass());
                        }
                        else
                        {
                            NWDBenchmark.Log("ConnectToDatabaseEditor () tDatabasePathEditor : " + tDatabasePathEditor + " (" + fileInfo.Length + " octets)");
                        }
                    }
                }
            }
            else
            {
                if (EditorDatabaseConnected == true)
                {
                    Debug.LogWarning("SQLiteConnectionEditor already connected");
                }
                if (EditorDatabaseConnectionInProgress == true)
                {
                    Debug.LogWarning("SQLiteConnectionEditor connexion in progress");
                }
            }

            if (NWDLauncher.ActiveBenchmark)
            {
                NWDBenchmark.Finish();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string PathDatabaseAccount()
        {
            string rReturn = string.Empty;
#if UNITY_EDITOR
            // path for base editor
            //rReturn = Application.dataPath + "/" + NWDAppConfiguration.SharedInstance().DatabasePrefix + DatabaseAccountName();
            rReturn = NWD.K_Assets+"/" + DatabaseAccountName();
#elif (UNITY_TVOS)
            rReturn = string.Format("{0}/{1}", Application.temporaryCachePath, DatabaseAccountName());
#else
            rReturn = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseAccountName());

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
        private static byte[] GetNullTerminatedUtf8(string s)
        {
            int utf8Length = Encoding.UTF8.GetByteCount(s);
            byte[] bytes = new byte[utf8Length + 1];
            utf8Length = Encoding.UTF8.GetBytes(s, 0, s.Length, bytes, 0);
            return bytes;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ConnectToDatabaseAccount(string sSurProtection)
        {
            if (NWDLauncher.ActiveBenchmark)
            {
                NWDBenchmark.Start();
                NWDBenchmark.Log("LibVersionNumber() = " + SQLite3.LibVersionNumber());
            }
            bool rReturn = true;
            //Debug.LogWarning("ConnectToDatabaseAccount (" + sSurProtection + ")");
            if (DeviceDatabaseConnected == false && DeviceDatabasConnectionInProgress == false)
            {
                DeviceDatabasConnectionInProgress = true;
                string tDatabasePathAccount = PathDatabaseAccount();
                byte[] tDatabasePathAsBytes = GetNullTerminatedUtf8(tDatabasePathAccount);
                SQLite3.Result tResult = SQLite3.Open(tDatabasePathAsBytes, out SQLiteDeviceHandle, (int)(SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create), IntPtr.Zero);
                if (tResult != SQLite3.Result.OK)
                {
                    throw SQLiteException.New(tResult, string.Format("Could not open database file: {0} ({1})", tDatabasePathAccount, tResult));
                }
                else
                {
                    DatabaseAccountOpenKey(tDatabasePathAccount, sSurProtection);

                    DeviceDatabaseConnected = true;
                    DeviceDatabasConnectionInProgress = false;
                    //IntPtr stmtpragmaX = SQLite3.Prepare2(SQLiteAccountHandle, "PRAGMA cipher_memory_security = OFF;");
                    //SQLite3.Step(stmtpragmaX);
                    //SQLite3.Finalize(stmtpragmaX);
                    IntPtr stmtpragma = SQLite3.Prepare2(SQLiteDeviceHandle, "PRAGMA synchronous = OFF;");
                    SQLite3.Step(stmtpragma);
                    SQLite3.Finalize(stmtpragma);
                    IntPtr stmtpragmaB = SQLite3.Prepare2(SQLiteDeviceHandle, "PRAGMA journal_mode = MEMORY");
                    SQLite3.Step(stmtpragmaB);
                    SQLite3.Finalize(stmtpragmaB);
                    //Sqlite3DatabaseHandle stmtpragmaC = SQLite3.Prepare2(SQLiteAccountHandle, "PRAGMA cache_size = 1000000");
                    //SQLite3.Step(stmtpragmaC);
                    //SQLite3.Finalize(stmtpragmaC);
                    //Sqlite3DatabaseHandle stmtpragmaD = SQLite3.Prepare2(SQLiteAccountHandle, "PRAGMA temp_store = MEMORY");
                    //SQLite3.Step(stmtpragmaD);
                    //SQLite3.Finalize(stmtpragmaD);

                    if (NWDAppEnvironment.SelectedEnvironment() == NWDAppConfiguration.SharedInstance().DevEnvironment
                    || NWDAppEnvironment.SelectedEnvironment() == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
                    {

                        var fileInfo = new System.IO.FileInfo(tDatabasePathAccount);
                        if (IsSecure())
                        {
                            NWDBenchmark.Log("ConnectToDatabaseAccount () tDatabasePathAccount : " + tDatabasePathAccount + " (" + fileInfo.Length + " octets) : " + NWDAppConfiguration.SharedInstance().GetAccountPass(sSurProtection));
                        }
                        else
                        {
                            NWDBenchmark.Log("ConnectToDatabaseAccount () tDatabasePathAccount : " + tDatabasePathAccount + " (" + fileInfo.Length + " octets) ");
                        }
                    }
                }
            }
            else
            {
                if (DeviceDatabaseConnected == true)
                {
                    Debug.LogWarning("SQLiteConnectionAccount already connected");
                }
                if (DeviceDatabasConnectionInProgress == true)
                {
                    Debug.LogWarning("SQLiteConnectionAccount connexion in progress");
                }
            }
            if (NWDLauncher.ActiveBenchmark)
            {
                NWDBenchmark.Finish();
            }
            return rReturn;
        }
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
            //NWEBenchmark.Start();
            IntPtr stmt = SQLite3.Prepare2(SQLiteDeviceHandle, "BEGIN TRANSACTION");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            if (DeviceDatabaseConnected == true && DeviceDatabasConnectionInProgress == false)
            {
                foreach (Type tType in ClassInDeviceDatabaseList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    if (tHelper.TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseAccount)
                    {
                        NWDSQLiteTableState tState = tHelper.TableSQLiteState();
                        foreach (string tQuery in tHelper.CreateTableSQLite(tState))
                        {
                            Debug.Log(tQuery);
                            stmt = SQLite3.Prepare2(SQLiteDeviceHandle, tQuery);
                            SQLite3.Result tResult = SQLite3.Step(stmt);
                            if (tResult != SQLite3.Result.Done)
                            {
                                Debug.Log("error in execution of query " + tResult.ToString() + ": " + tQuery);
                            }
                            else
                            {
#if UNITY_EDITOR
                                Debug.Log("success query : " + tQuery);
#endif
                            }
                            SQLite3.Finalize(stmt);
                        }
                        string tIndexA = tHelper.CreateIndexSQLite(tState);
                        if (string.IsNullOrEmpty(tIndexA) == false)
                        {
                            stmt = SQLite3.Prepare2(SQLiteDeviceHandle, tIndexA);
                            SQLite3.Result tResult = SQLite3.Step(stmt);
                            if (tResult != SQLite3.Result.Done)
                            {
                                Debug.Log("error in execution of query " + tResult.ToString() + ": " + tIndexA);
                            }
                            else
                            {
#if UNITY_EDITOR
                                Debug.Log("success query : " + tIndexA);
#endif
                            }
                            SQLite3.Finalize(stmt);
                        }
                        string tIndexC = tHelper.CreateIndexBundleSQLite(tState);
                        if (string.IsNullOrEmpty(tIndexC) == false)
                        {
                            stmt = SQLite3.Prepare2(SQLiteDeviceHandle, tIndexC);
                            SQLite3.Result tResult = SQLite3.Step(stmt);
                            if (tResult != SQLite3.Result.Done)
                            {
                                Debug.Log("error in execution of query " + tResult.ToString() + ": " + tIndexC);
                            }
                            else
                            {
#if UNITY_EDITOR
                                Debug.Log("success query : " + tIndexC);
#endif
                            }
                            SQLite3.Finalize(stmt);
                        }
                        if (tHelper.TemplateHelper.GetBundlisable() == NWDTemplateBundlisable.Bundlisable)
                        {
                            string tIndexB = tHelper.CreateIndexBundleSQLite(tState);
                            if (string.IsNullOrEmpty(tIndexB) == false)
                            {
                                stmt = SQLite3.Prepare2(SQLiteDeviceHandle, tIndexB);
                                SQLite3.Result tResult = SQLite3.Step(stmt);
                                if (tResult != SQLite3.Result.Done)
                                {
                                    Debug.Log("error in execution of query " + tResult.ToString() + ": " + tIndexB);
                                }
                                else
                                {
#if UNITY_EDITOR
                                    Debug.Log("success query : " + tIndexB);
#endif
                                }
                                SQLite3.Finalize(stmt);
                            }
                        }
                    }
                }
            }
            stmt = SQLite3.Prepare2(SQLiteDeviceHandle, "COMMIT");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void CleanAllTablesLocalAccount()
        //{
        //    if (DataAccountConnected == true && DataAccountConnectionInProgress == false)
        //    {
        //        foreach (Type tType in mTypeAccountDependantList)
        //        {
        //            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
        //            tHelper.CleanTable();
        //        }
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void PurgeAllTablesLocalAccount()
        //{
        //    if (DataAccountConnected == true && DataAccountConnectionInProgress == false)
        //    {
        //        foreach (Type tType in mTypeAccountDependantList)
        //        {
        //            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
        //            tHelper.PurgeTable();
        //            //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_PurgeTable);
        //        }
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void ResetAllTablesLocalAccount()
        //{
        //    if (DataAccountConnected == true && DataAccountConnectionInProgress == false)
        //    {
        //        foreach (Type tType in mTypeAccountDependantList)
        //        {
        //            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
        //            tHelper.ResetTable();
        //            //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_ResetTable);
        //        }
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void CreateAllTablesLocalEditor()
        {
            //NWEBenchmark.Start();
            IntPtr stmt = SQLite3.Prepare2(SQLiteEditorHandle, "BEGIN TRANSACTION");
            SQLite3.Step(stmt);
            //SQLite3.Finalize(stmt);
            if (EditorDatabaseConnected == true && EditorDatabaseConnectionInProgress == false)
            {
                foreach (Type tType in ClassInEditorDatabaseList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    if (tHelper.TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseEditor)
                    {
                        NWDSQLiteTableState tState = tHelper.TableSQLiteState();
                        foreach (string tQuery in tHelper.CreateTableSQLite(tState))
                        {
                            Debug.Log(tQuery);
                            stmt = SQLite3.Prepare2(SQLiteEditorHandle, tQuery);
                            SQLite3.Result tResult = SQLite3.Step(stmt);
                            if (tResult != SQLite3.Result.Done)
                            {
                                Debug.Log("error in execution of query " + tResult.ToString() + ": " + tQuery);
                            }
                            else
                            {
#if UNITY_EDITOR
                                Debug.Log("success query : " + tQuery);
#endif
                            }
                            SQLite3.Finalize(stmt);
                        }
                        string tIndexA = tHelper.CreateIndexSQLite(tState);
                        if (string.IsNullOrEmpty(tIndexA) == false)
                        {
                            stmt = SQLite3.Prepare2(SQLiteEditorHandle, tIndexA);
                            SQLite3.Result tResult = SQLite3.Step(stmt);
                            if (tResult != SQLite3.Result.Done)
                            {
                                Debug.Log("error in execution of query " + tResult.ToString() + ": " + tIndexA);
                            }
                            else
                            {
#if UNITY_EDITOR
                                Debug.Log("success query : " + tIndexA);
#endif
                            }
                            SQLite3.Finalize(stmt);
                        }

                        string tIndexC = tHelper.CreateIndexModifiySQLite(tState);
                        if (string.IsNullOrEmpty(tIndexC) == false)
                        {
                            stmt = SQLite3.Prepare2(SQLiteEditorHandle, tIndexC);
                            SQLite3.Result tResult = SQLite3.Step(stmt);
                            if (tResult != SQLite3.Result.Done)
                            {
                                Debug.Log("error in execution of query " + tResult.ToString() + ": " + tIndexC);
                            }
                            else
                            {
#if UNITY_EDITOR
                                Debug.Log("success query : " + tIndexC);
#endif
                            }
                            SQLite3.Finalize(stmt);
                        }
                        if (tHelper.TemplateHelper.GetBundlisable() == NWDTemplateBundlisable.Bundlisable)
                        {
                            string tIndexB = tHelper.CreateIndexBundleSQLite(tState);
                            if (string.IsNullOrEmpty(tIndexB) == false)
                            {
                                stmt = SQLite3.Prepare2(SQLiteEditorHandle, tIndexB);
                                SQLite3.Result tResult = SQLite3.Step(stmt);
                                if (tResult != SQLite3.Result.Done)
                                {
                                    Debug.Log("error in execution of query " + tResult.ToString() + ": " + tIndexB);
                                }
                                else
                                {
#if UNITY_EDITOR
                                    Debug.Log("success query : " + tIndexB);
#endif
                                }
                                SQLite3.Finalize(stmt);
                            }
                        }
                    }
                }
            }
            stmt = SQLite3.Prepare2(SQLiteEditorHandle, "COMMIT");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CleanAllTablesLocalEditor()
        {
            if (EditorDatabaseConnected == true && EditorDatabaseConnectionInProgress == false)
            {
                foreach (Type tType in ClassInEditorDatabaseList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.CleanTable();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PurgeAllTablesLocalEditor()
        {
            if (EditorDatabaseConnected == true && EditorDatabaseConnectionInProgress == false)
            {
                foreach (Type tType in ClassInEditorDatabaseList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.PurgeTable();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetAllTablesLocalEditor()
        {
            if (EditorDatabaseConnected == true && EditorDatabaseConnectionInProgress == false)
            {
                foreach (Type tType in ClassInEditorDatabaseList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.ResetTable();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool SQLiteConnectionEditorIsValid()
        {
            bool rReturn = true;
            rReturn = EditorDatabaseConnected;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool SQLiteConnectionAccountIsValid()
        {
            bool rReturn = true;
            rReturn = DeviceDatabaseConnected;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================