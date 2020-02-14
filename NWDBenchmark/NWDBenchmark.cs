//=====================================================================================================================
//
// ideMobi 2019©
//
// Date		2019-4-12 18:42:10
// Author		Kortex (Jean-François CONTART) 
// Email		jfcontart@idemobi.com
// Project 	NetWorkedData for Unity3D
//
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
using System.IO;

using Sqlite3DatabaseHandle = System.IntPtr;
using Sqlite3Statement = System.IntPtr;

using SQLite4Unity3d;
using System.Text;

using System.Reflection;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDBenchmark
    {
        //-------------------------------------------------------------------------------------------------------------
        private static NWDBenchmark kSharedInstance = new NWDBenchmark();
        const string kMenu = "NWDBenchmark/";
        const string DatabaseName = "DBBenchmark.prp";
        internal static readonly Sqlite3DatabaseHandle NullHandle = default(Sqlite3DatabaseHandle);
        //-------------------------------------------------------------------------------------------------------------
        public Sqlite3DatabaseHandle Handle;
        public string Path;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDBenchmark SharedInstance()
        {
            if (kSharedInstance == null)
            {
                kSharedInstance = new NWDBenchmark();
            }
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        [MenuItem(kMenu + "open database")]
#endif
        public static void TEST_OpenDatabase()
        {
            NWEBenchmark.Start();
            SharedInstance().OpenDatabase();
            NWEBenchmark.Finish(true, SharedInstance().Path);
            SharedInstance().CreateTable();
            SharedInstance().Close();
            SharedInstance().DeleteDataBase();
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        [MenuItem(kMenu + "close database")]
#endif
        public static void TEST_CloseDatabase()
        {
            SharedInstance().OpenDatabase();
            SharedInstance().CreateTable();
            NWEBenchmark.Start();
            SharedInstance().Close();
            NWEBenchmark.Finish(true, SharedInstance().Path);
            SharedInstance().DeleteDataBase();
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        [MenuItem(kMenu + " open and create 100 tables database")]
#endif
        public static void TEST_CreateHundredTableDatabase()
        {
            NWEBenchmark.Start();
            SharedInstance().OpenDatabase();
            for (int i = 0; i < 100; i++)
            {
                SharedInstance().CreateTable();
            }
            NWEBenchmark.Finish(true, SharedInstance().Path);
            SharedInstance().Close();
            SharedInstance().DeleteDataBase();
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        [MenuItem(kMenu + " open and create 100 tables database with transaction")]
#endif
        public static void TEST_CreateHundredTableDatabaseWithTransaction()
        {
            NWEBenchmark.Start();
            SharedInstance().OpenDatabase();
            SharedInstance().BeginTransaction();
            for (int i = 0; i < 100; i++)
            {
                SharedInstance().CreateTable();
            }
            SharedInstance().CommitTransaction();
            NWEBenchmark.Finish(true, SharedInstance().Path);
            SharedInstance().Close();
            SharedInstance().DeleteDataBase();
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        [MenuItem(kMenu + "delete database")]
#endif
        public static void TEST_DeleteDatabase()
        {
            SharedInstance().OpenDatabase();
            SharedInstance().CreateTable();
            SharedInstance().Close();
            NWEBenchmark.Start();
            SharedInstance().DeleteDataBase();
            NWEBenchmark.Finish(true, SharedInstance().Path);
        }

        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        [MenuItem(kMenu + " open and create 1 tables  and insert 1000 datas with transaction")]
#endif
        public static void TEST_CreateTableAndTenThousanDataWithTransaction()
        {
            int testInt = 1000;
            SharedInstance().OpenDatabase();
            SharedInstance().BeginTransaction();
            string tTable = SharedInstance().CreateTable();
            SharedInstance().CommitTransaction();

            SharedInstance().BeginTransaction();
            for (int i = 0; i < testInt; i++)
            {
                SharedInstance().InsertFakeDataInTable(tTable);
            }
            NWEBenchmark.Start();
            SharedInstance().CommitTransaction();
            NWEBenchmark.Finish(true, SharedInstance().Path);

            SharedInstance().InsertDataInEditorTable(testInt);
            SharedInstance().InsertDataInEditorTableSecond(testInt);

            SharedInstance().Close();
            SharedInstance().DeleteDataBase();
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        [MenuItem(kMenu + " open and insert 10 000 datas with transaction")]
#endif
        public static void TEST_CreateTableAndTenThousanDataWithTransactionB()
        {
            NWEBenchmark.Start();
            int testInt = 10000;
            SharedInstance().OpenDatabase();
            SharedInstance().InsertDataInEditorTableSecond(testInt);
            SharedInstance().Close();
            SharedInstance().DeleteDataBase();
            NWEBenchmark.Finish();
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
        public string PathDatabaseAccount()
        {
            string rReturn = string.Empty;
            string tPath = NWDToolbox.RandomStringUnix(8) + "_" + DatabaseName;
#if UNITY_EDITOR
            if (Directory.Exists("Assets/BenchmarkTest") == false)
            {
                Directory.CreateDirectory("Assets/BenchmarkTest");
            }
            rReturn = "Assets/BenchmarkTest/" + tPath;
#else
       rReturn = string.Format("{0}/{1}", Application.persistentDataPath, tPath);
#endif
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OpenDatabase()
        {
            Path = PathDatabaseAccount();
            byte[] tDatabasePathAsBytes = GetNullTerminatedUtf8(Path);
            SQLite3.Result tResult = SQLite3.Open(tDatabasePathAsBytes, out Handle, (int)(SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create), IntPtr.Zero);
            if (tResult != SQLite3.Result.OK)
            {
                throw SQLiteException.New(tResult, string.Format("Could not open database file: {0} ({1})", Path, tResult));
            }

            Sqlite3DatabaseHandle stmtpragma = SQLite3.Prepare2(Handle, "PRAGMA synchronous = OFF;");
            SQLite3.Step(stmtpragma);
            SQLite3.Finalize(stmtpragma);

            Sqlite3DatabaseHandle stmtpragmaB = SQLite3.Prepare2(Handle, "PRAGMA journal_mode = MEMORY");
            SQLite3.Step(stmtpragmaB);
            SQLite3.Finalize(stmtpragmaB);

            Sqlite3DatabaseHandle stmtpragmaC = SQLite3.Prepare2(Handle, "PRAGMA cache_size = 1000000");
            SQLite3.Step(stmtpragmaC);
            SQLite3.Finalize(stmtpragmaC);

            Sqlite3DatabaseHandle stmtpragmaD = SQLite3.Prepare2(Handle, "PRAGMA temp_store = MEMORY");
            SQLite3.Step(stmtpragmaD);
            SQLite3.Finalize(stmtpragmaD);

            string tTest = "SELECT name FROM sqlite_master WHERE type='table';";
            Sqlite3DatabaseHandle stmt;
            SQLite3.Result er = SQLite3.Prepare2(Handle, tTest, Encoding.UTF8.GetByteCount(tTest), out stmt, Sqlite3DatabaseHandle.Zero);
            if (er != SQLite3.Result.OK)
            {
                NWDDebug.Log("Fail open and list " + Path);
            }
            else
            {
                NWDDebug.Log("Success open and list " + Path);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Close()
        {
            if (this.Handle != NullHandle)
                try
                {
                    SQLite3.Result r = SQLite3.Close(Handle);
                    if (r != SQLite3.Result.OK)
                    {
                        string msg = SQLite3.GetErrmsg(Handle);
                        throw SQLiteException.New(r, msg);
                    }
                }
                finally
                {
                    Handle = NullHandle;
                }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DeleteDataBase()
        {
            if (File.Exists(Path))
            {
#if UNITY_EDITOR
                //File.Delete(Path);
#else
                File.Delete(Path);
#endif
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void BeginTransaction()
        {
            string tQuery = "begin transaction";
            Sqlite3DatabaseHandle stmt = SQLite3.Prepare2(Handle, tQuery);
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CommitTransaction()
        {
            string tQuery = "commit";
            Sqlite3DatabaseHandle stmt = SQLite3.Prepare2(Handle, tQuery);
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CreateTable()
        {
            string rTableName = NWDToolbox.RandomStringCypher(16);
            string tQuery = "CREATE TABLE \"" + rTableName + "\"(" +
//"\"ID\" integer primary key autoincrement not null default 0 ," +
"\"Reference\" varchar primary key," +
"\"CheckList\" int ," +
"\"WebModel\" integer ," +
"\"InternalKey\" varchar ," +
"\"InternalDescription\" varchar ," +
"\"Preview\" varchar ," +
"\"AC\" integer ," +
"\"DC\" integer ," +
"\"DM\" integer ," +
"\"DD\" integer ," +
"\"XX\" integer ," +
"\"Integrity\" varchar ," +
"\"DS\" integer ," +
"\"DevSync\" integer ," +
"\"PreprodSync\" integer ," +
"\"ProdSync\" integer ," +
"\"Tag\" integer ," +
"\"ServerHash\" varchar ," +
"\"ServerLog\" varchar ," +
"\"InError\" integer )";
            Sqlite3DatabaseHandle stmt = SQLite3.Prepare2(Handle, tQuery);
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            return rTableName;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertFakeDataInTable(string sTableName)
        {
            Dictionary<string, string> tKeyValue = new Dictionary<string, string>();
            tKeyValue.Add("Reference", NWDToolbox.GenerateUniqueID());
            tKeyValue.Add("CheckList", "1");
            tKeyValue.Add("WebModel", "2");
            tKeyValue.Add("InternalKey", NWDToolbox.RandomString(24));
            tKeyValue.Add("InternalDescription", NWDToolbox.RandomString(64));
            tKeyValue.Add("Preview", "2");
            tKeyValue.Add("AC", NWDToolbox.Timestamp().ToString());
            tKeyValue.Add("DC", NWDToolbox.Timestamp().ToString());
            tKeyValue.Add("DM", NWDToolbox.Timestamp().ToString());
            tKeyValue.Add("DD", NWDToolbox.Timestamp().ToString());
            tKeyValue.Add("XX", "2");
            tKeyValue.Add("Integrity", "2");
            tKeyValue.Add("DS", NWDToolbox.Timestamp().ToString());
            tKeyValue.Add("DevSync", NWDToolbox.Timestamp().ToString());
            tKeyValue.Add("PreprodSync", NWDToolbox.Timestamp().ToString());
            tKeyValue.Add("ProdSync", NWDToolbox.Timestamp().ToString());
            tKeyValue.Add("Tag", "2");
            tKeyValue.Add("ServerHash", "2");
            tKeyValue.Add("ServerLog", "2");
            tKeyValue.Add("InError", "2");
            List<string> tKeys = new List<string>();
            List<string> tValues = new List<string>();
            foreach (KeyValuePair<string, string> tT in tKeyValue)
            {
                tKeys.Add(tT.Key);
                tValues.Add(tT.Value);
            }
            string tQuery = "INSERT OR REPLACE INTO `" + sTableName + "` (`" + string.Join("`, `", tKeys) + "`) VALUES (\"" + string.Join("\", \"", tValues) + "\");";
            Sqlite3DatabaseHandle stmt = SQLite3.Prepare2(Handle, tQuery);
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
        }

        //-------------------------------------------------------------------------------------------------------------
        public void InsertDataInEditorTable(int sNumber)
        {
            string tTag = "NWDBasis insert " + sNumber.ToString() + " Data";
            List<NWDExample> tToDelete = new List<NWDExample>();
            for (int i = 0; i < sNumber; i++)
            {
                NWDExample tData = new NWDExample();
                tData.InsertData(true, NWDWritingMode.QueuedMainThread);
                tToDelete.Add(tData);
            }
            NWEBenchmark.Start(tTag);
            NWDDataManager.SharedInstance().DataQueueMainExecute();
            NWEBenchmark.Finish(tTag);


            foreach (NWDExample tD in tToDelete)
            {
                tD.DeleteData(NWDWritingMode.QueuedMainThread);
            }
            NWDDataManager.SharedInstance().DataQueueMainExecute();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InsertDataInEditorTableSecond(int sNumber)
        {
            NWEBenchmark.Start();


            string tTag = "NWDBasis insert " + sNumber.ToString() + " Data create table";
            NWEBenchmark.Start(tTag);
            string tQueryTable = "CREATE TABLE \"NWDBenchmarkExample\"(" +
//"\"ID\" integer not null default 0 ," +
"\"Reference\" varchar primary key," +
"\"CheckList\" int ," +
"\"WebModel\" integer ," +
"\"InternalKey\" varchar ," +
"\"InternalDescription\" varchar ," +
"\"Preview\" varchar ," +
"\"AC\" integer ," +
"\"DC\" integer ," +
"\"DM\" integer ," +
"\"DD\" integer ," +
"\"XX\" integer ," +
"\"Integrity\" varchar ," +
"\"DS\" integer ," +
"\"DevSync\" integer ," +
"\"PreprodSync\" integer ," +
"\"ProdSync\" integer ," +
"\"Tag\" integer ," +
"\"ServerHash\" varchar ," +
"\"ServerLog\" varchar ," +
"\"InError\" integer )";
            Sqlite3DatabaseHandle stmt = SQLite3.Prepare2(Handle, tQueryTable);
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            NWEBenchmark.Finish(tTag);

            tTag = "NWDBasis insert " + sNumber.ToString() + " Data create instance ";
            NWEBenchmark.Start(tTag);

            List<NWDBenchmarkExample> tToDelete = new List<NWDBenchmarkExample>();
            List<NWDBenchmarkExample> tTest = new List<NWDBenchmarkExample>();

            for (int i = 0; i < sNumber; i++)
            {
                NWDBenchmarkExample tData = new NWDBenchmarkExample();
                //tData.InternalKey = NWDToolbox.RandomString(24);
                //tData.InternalDescription = NWDToolbox.RandomString(64);
                tData.InternalKey = "test" + sNumber;
                tData.InternalDescription = "test" + sNumber;
                tTest.Add(tData);
                tToDelete.Add(tData);
            }
            NWEBenchmark.Finish(tTag);

            tTag = "NWDBasis insert " + sNumber.ToString() + " Data insert";
            //NWEBenchmark.Start(tTag);
            //BeginTransaction();
            //foreach (NWDBenchmarkExample tData in tTest)
            //{
            //    string tQuery = tData.SQLInsert();
            //    Sqlite3DatabaseHandle stmtb = SQLite3.Prepare2(Handle, tQuery);
            //    SQLite3.Step(stmtb);
            //    SQLite3.Finalize(stmtb);
            //}
            //CommitTransaction();
            //NWEBenchmark.Finish(tTag);

            //tTag = "NWDBasis insert " + sNumber.ToString() + " Data update";
            NWEBenchmark.Start(tTag);
            BeginTransaction();
            foreach (NWDBenchmarkExample tData in tTest)
            {
                //tData.InternalKey = NWDToolbox.RandomString(24);
                //tData.InternalDescription = NWDToolbox.RandomString(64);
                //tData.InternalKey = "test";
                //tData.InternalDescription = "test";
                //tData.DD = 6;
                string tQuery = tData.QueryInsert();
                Sqlite3DatabaseHandle stmtb = SQLite3.Prepare2(Handle, tQuery);
                SQLite3.Step(stmtb);
                SQLite3.Finalize(stmtb);
            }
            CommitTransaction();
            NWEBenchmark.Finish(tTag);

            tTag = "NWDBasis insert " + sNumber.ToString() + " Data update";
            NWEBenchmark.Start(tTag);
            BeginTransaction();
            foreach (NWDBenchmarkExample tData in tTest)
            {
                //tData.InternalKey = NWDToolbox.RandomString(24);
                //tData.InternalDescription = NWDToolbox.RandomString(64);
                tData.InternalKey = "test";
                tData.InternalDescription = "test";
                tData.DD = 6;
                string tQuery = tData.QueryInsert();
                Sqlite3DatabaseHandle stmtb = SQLite3.Prepare2(Handle, tQuery);
                SQLite3.Step(stmtb);
                SQLite3.Finalize(stmtb);
            }
            CommitTransaction();
            NWEBenchmark.Finish(tTag);

            tTag = "NWDBasis insert " + sNumber.ToString() + " Data reload";
            NWEBenchmark.Start(tTag);
            List<PropertyInfo> tProplist = new List<PropertyInfo>();
            List<string> tColumnList = new List<string>();
            foreach (PropertyInfo tProp in typeof(NWDBenchmarkExample).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (tProp.Name != "ID" && string.IsNullOrEmpty(tProp.Name) == false)
                {
                    tProplist.Add(tProp);
                    tColumnList.Add(tProp.Name);
                }
            }
            Sqlite3DatabaseHandle stmtc = SQLite3.Prepare2(Handle, "SELECT `" + string.Join("`, `", tColumnList) + "` FROM `NWDBenchmarkExample`");
            int c = 0;
            while (SQLite3.Step(stmtc) == SQLite3.Result.Row)
            {
                NWDBenchmarkExample tD = new NWDBenchmarkExample();
                for (int i = 0; i < tProplist.Count; i++)
                {
                    PropertyInfo tProp = tProplist[i];
                    if (tProp.GetType() == typeof(int))
                    {
                        tProp.SetValue(tD, SQLite3.ColumnInt(stmtc, i));
                    }
                    else if (tProp.GetType() == typeof(string))
                    {
                        tProp.SetValue(tD, SQLite3.ColumnString(stmtc, i));
                    }
                }
                c++;
            }
            SQLite3.Finalize(stmtc);
            NWEBenchmark.Finish(tTag, true, c.ToString() + " rows");

            //tTag = "NWDBasis insert " + sNumber.ToString() + " Data 5";
            //NWEBenchmark.Start(tTag);
            //foreach (NWDBenchmarkExample tD in tToDelete)
            //{
            //    tD.DeleteData(NWDWritingMode.QueuedMainThread);
            //}
            //NWDDataManager.SharedInstance().DataQueueMainExecute();
            //NWEBenchmark.Finish(tTag);

            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        public string SQLInsert()
        {
            NWDBasisHelper tHelper = BasisHelper();
            List<string> tKeys = new List<string>();
            List<string> tValues = new List<string>();
            foreach (PropertyInfo tProp in tHelper.ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (tProp.Name != "ID")
                {
                    tKeys.Add(tProp.Name);
                    tValues.Add(tProp.GetValue(this).ToString());
                }
            }
            string rReturn = "INSERT OR REPLACE INTO `" + tHelper.ClassNamePHP + "` (`" + string.Join("`, `", tKeys) + "`) VALUES (\"" + string.Join("\", \"", tValues) + "\");";
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBenchmarkExample
    {
        //-------------------------------------------------------------------------------------------------------------
        private string _Reference;
        public string Reference
        {
            get
            {
                if (string.IsNullOrEmpty(_Reference))
                {
                    _Reference = NWDToolbox.GenerateUniqueID();
                }
                return _Reference;
            }
            set
            {
                _Reference = value;
            }
        }
        public int CheckList { get; set; }
        public int WebModel { get; set; }
        public string InternalKey { get; set; }
        public string InternalDescription { get; set; }
        public string Preview { get; set; }
        public int AC { get; set; }
        public int DC { get; set; }
        public int DM { get; set; }
        public int DD { get; set; }
        public int XX { get; set; }
        public string Integrity { get; set; }
        public int DS { get; set; }
        public int DevSync { get; set; }
        public int PreprodSync { get; set; }
        public int ProdSync { get; set; }
        public int Tag { get; set; }
        public string ServerHash { get; set; }
        public string ServerLog { get; set; }
        public int InError { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        public NWDBenchmarkExample()
        {
            //Reference = NWDToolbox.GenerateUniqueID();
            Reference = "";
            InternalKey = "";
            InternalDescription = "";
            Preview = "";
            Integrity = "";
            ServerHash = "";
            ServerLog = "";
        }
        //-------------------------------------------------------------------------------------------------------------
        public string SQLInsert()
        {
            // very slow! (4x)
            List<string> tKeys = new List<string>();
            List<string> tValues = new List<string>();
            foreach (PropertyInfo tProp in typeof(NWDBenchmarkExample).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (tProp.Name != "ID" && string.IsNullOrEmpty(tProp.Name) == false)
                {
                    object tV = tProp.GetValue(this);
                    if (tV != null)
                    {
                        tKeys.Add(tProp.Name);
                        tValues.Add(tV.ToString());
                    }
                }
            }
            string rReturn = "INSERT OR REPLACE INTO `NWDBenchmarkExample` (`" + string.Join("`, `", tKeys) + "`) VALUES (\"" + string.Join("\", \"", tValues) + "\");";
            //NWDDebug.Log(rReturn);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string QueryInsert()
        {
            // very fast! (1x)
            return "INSERT OR REPLACE INTO `NWDBenchmarkExample` (`Reference`, `CheckList`, `WebModel`, `InternalKey`, `InternalDescription`, `Preview`, `AC`, `DC`, `DM`, `DD`, `XX`, `Integrity`, `DS`, `DevSync`, `PreprodSync`, `ProdSync`, `Tag`, `ServerHash`, `ServerLog`, `InError`) VALUES " +
            " (" +
            "\"" + Reference.ToString() + "\", " +
            "\"" + CheckList.ToString() + "\", " +
            "\"" + WebModel.ToString() + "\", " +
            "\"" + InternalKey.ToString() + "\", " +
            "\"" + InternalDescription.ToString() + "\", " +
            "\"" + Preview.ToString() + "\", " +
            "\"" + AC.ToString() + "\", " +
            "\"" + DC.ToString() + "\", " +
            "\"" + DM.ToString() + "\", " +
            "\"" + DD.ToString() + "\", " +
            "\"" + XX.ToString() + "\", " +
            "\"" + Integrity.ToString() + "\", " +
            "\"" + DS.ToString() + "\", " +
            "\"" + DevSync.ToString() + "\", " +
            "\"" + PreprodSync.ToString() + "\", " +
            "\"" + ProdSync.ToString() + "\", " +
            "\"" + Tag.ToString() + "\", " +
            "\"" + ServerHash.ToString() + "\", " +
            "\"" + ServerLog.ToString() + "\", " +
            "\"" + InError.ToString() + "\"" +
            ")";
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDExample : NWDBasis
    {
        public string QueryInsert()
        {
            return "INSERT OR REPLACE INTO `NWDExample` (`Reference`, `CheckList`, `WebModel`, `InternalKey`, `InternalDescription`, `Preview`, `AC`, `DC`, `DM`, `DD`, `XX`, `Integrity`, `DS`, `DevSync`, `PreprodSync`, `ProdSync`, `Tag`, `ServerHash`, `ServerLog`, `InError`) VALUES " +
            " (" +
            "\"" + Reference.ToString() + "\", " +
            "\"" + CheckList.ToString() + "\", " +
            "\"" + WebModel.ToString() + "\", " +
            "\"" + InternalKey.ToString() + "\", " +
            "\"" + InternalDescription.ToString() + "\", " +
            "\"" + Preview.ToString() + "\", " +
            "\"" + AC.ToString() + "\", " +
            "\"" + DC.ToString() + "\", " +
            "\"" + DM.ToString() + "\", " +
            "\"" + DD.ToString() + "\", " +
            "\"" + XX.ToString() + "\", " +
            "\"" + Integrity.ToString() + "\", " +
            "\"" + DS.ToString() + "\", " +
            "\"" + DevSync.ToString() + "\", " +
            "\"" + PreprodSync.ToString() + "\", " +
            "\"" + ProdSync.ToString() + "\", " +
            "\"" + Tag.ToString() + "\", " +
            "\"" + ServerHash.ToString() + "\", " +
            "\"" + ServerLog.ToString() + "\", " +
            "\"" + InError.ToString() + "\"" +
            ")";
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================