//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;
//=====================================================================================================================
#if UNITY_EDITOR
using Sqlite = NetWorkedData.Logged.SQLite3; // Have a logged interface for SQLite (Editor only !)
#else
using Sqlite = NetWorkedData.SQLite3;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDSQLiteTableState : int
    {
        Error,
        Update,
        Create,
        Migrate,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        private string PropertyInfoToSQLiteType(PropertyInfo sPropertyInfo, bool sJustType = false)
        {
            NWDBenchmark.QuickStart();
            string rReturn = "varchar";
            Type tTypeOfThis = sPropertyInfo.PropertyType;
            if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
            {
                rReturn = "varchar";
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)))
            {
                rReturn = "int";
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)))
            {
                rReturn = "float";
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)))
            {
                rReturn = "int";
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
            {
                rReturn = "int";
            }
            else if (tTypeOfThis == typeof(int) ||
                tTypeOfThis == typeof(Int16) ||
                tTypeOfThis == typeof(Int32) ||
                tTypeOfThis.IsEnum
                )
            {
                rReturn = "integer";
            }
            else if (tTypeOfThis == typeof(long) ||
                tTypeOfThis == typeof(Int64)
                )
            {
                rReturn = "bigint";
            }
            else if (tTypeOfThis == typeof(float) ||
                tTypeOfThis == typeof(double) ||
                tTypeOfThis == typeof(Double))
            {
                rReturn = "float";
            }
            else if (tTypeOfThis == typeof(bool))
            {
                rReturn = "integer";
            }
            else if (tTypeOfThis == typeof(string) || tTypeOfThis == typeof(String))
            {
                rReturn = "varchar";
            }
            else
            {
                rReturn = "blob";
            }
            if (sJustType == false)
            {
                if (sPropertyInfo.Name == NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference))
                {
                    rReturn += " primary key not null";
                }
                if (sPropertyInfo.Name == NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ID))
                {
                    //rReturn += " primary key autoincrement not null default 0 ";
                }
            }
            NWDBenchmark.QuickFinish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        List<PropertyInfo> tActualsList = new List<PropertyInfo>();
        List<PropertyInfo> tMigratePropertyList = new List<PropertyInfo>();
        List<string> tTransfertList = new List<string>();
        //-------------------------------------------------------------------------------------------------------------
        public NWDSQLiteTableState TableSQLiteState()
        {
            NWDSQLiteTableState rReturn = NWDSQLiteTableState.Error;
            StringBuilder tQuery = new StringBuilder();
            tActualsList = new List<PropertyInfo>(ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            tMigratePropertyList = new List<PropertyInfo>(ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            tTransfertList.Clear();
            IntPtr tConnectorHandle = NWDDataManager.SharedInstance().SQLiteDeviceHandle;
            //SQLiteConnection Connector = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            //if (kAccountDependent == false)
            //if (TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
            if (TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseEditor)
            {
                //Connector = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }
            IntPtr stmt = Sqlite.Prepare2(tConnectorHandle, "PRAGMA table_info(`" + ClassNamePHP + "`)");

            bool tError = true;
            while (Sqlite.Step(stmt) == SQLite3.Result.Row)
            {
                tError = false;
                //Debug.Log("???");
                //Debug.Log(Sqlite.ColumnString(stmt,1) + " " + Sqlite.ColumnString(stmt, 2));
                string tPropName = Sqlite.ColumnString(stmt, 1);
                PropertyInfo tPropertyInfo = ClassType.GetProperty(tPropName);
                if (tPropertyInfo != null)
                {
                    tActualsList.Remove(tPropertyInfo);
                    tMigratePropertyList.Add(tPropertyInfo);
                    string tActual = "`" + Sqlite.ColumnString(stmt, 1) + "` " + Sqlite.ColumnString(stmt, 2);
                    string tAuto = "`" + tPropertyInfo.Name + "` " + PropertyInfoToSQLiteType(tPropertyInfo, true);
                    if (tAuto != tActual)
                    {
                        rReturn = NWDSQLiteTableState.Migrate;
                    }
                    else
                    {
                        tTransfertList.Add(tPropName);
                    }
                }
                else
                {
                    string tPropNameUpper = tPropName.ToUpper();
                    foreach (PropertyInfo tPropertyInfoUPPER in PropertiesArray)
                    {
                        if (tPropNameUpper == tPropertyInfoUPPER.Name.ToUpper())
                        {
                            rReturn = NWDSQLiteTableState.Migrate;
                        }
                    }
                }
            }
            if (tError == true)
            {
                rReturn = NWDSQLiteTableState.Create;
            }
            else
            {
                if (tActualsList.Count > 0)
                {
                    rReturn = NWDSQLiteTableState.Update;
                }
            }
            Sqlite.Finalize(stmt);
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        public string[] CreateTableSQLite(NWDSQLiteTableState sState)
        {

            List<string> tQuery = new List<string>();
            IntPtr tConnectorHandle = NWDDataManager.SharedInstance().SQLiteDeviceHandle;
            //if (kAccountDependent == false)
            //if (TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
            if (TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseEditor)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }
            if (sState == NWDSQLiteTableState.Migrate)
            {
                StringBuilder tQueryBuilder = new StringBuilder();
                List<string> PropertiesSQL = new List<string>();
                foreach (PropertyInfo tPropertyInfo in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (tPropertyInfo != null)
                    {
                        PropertiesSQL.Add("`" + tPropertyInfo.Name + "` " + PropertyInfoToSQLiteType(tPropertyInfo));
                    }
                }
                tQueryBuilder.Append("CREATE TABLE IF NOT EXISTS `" + ClassNamePHP + "_new` (");
                tQueryBuilder.Append(string.Join(",", PropertiesSQL.ToArray()));
                tQueryBuilder.Append(");");
                tQuery.Add(tQueryBuilder.ToString());

                tQueryBuilder = new StringBuilder();
                List<string> PropertiesName = new List<string>();
                foreach (string tName in tTransfertList)
                {
                    if (string.IsNullOrEmpty(tName) == false)
                    {
                        PropertiesName.Add("`" + tName + "` ");
                    }
                }
                if (PropertiesName.Count > 0)
                {
                    tQueryBuilder.Append("INSERT INTO `" + ClassNamePHP + "_new` (");
                    tQueryBuilder.Append(string.Join(",", PropertiesName.ToArray()));
                    tQueryBuilder.Append(") SELECT ");
                    tQueryBuilder.Append(string.Join(",", PropertiesName.ToArray()));
                    tQueryBuilder.Append("FROM `" + ClassNamePHP + "`;");
                    tQuery.Add(tQueryBuilder.ToString());
                }

                tQueryBuilder = new StringBuilder();
                tQueryBuilder.Append("DROP TABLE `" + ClassNamePHP + "`;");
                tQuery.Add(tQueryBuilder.ToString());

                tQueryBuilder = new StringBuilder();
                tQueryBuilder.Append("ALTER TABLE `" + ClassNamePHP + "_new` RENAME TO `" + ClassNamePHP + "`;");
                tQuery.Add(tQueryBuilder.ToString());
            }
            else if (sState == NWDSQLiteTableState.Update)
            {
                foreach (PropertyInfo tPropertyInfo in tActualsList)
                {
                    if (tPropertyInfo != null)
                    {
                        tQuery.Add("ALTER TABLE `" + ClassNamePHP + "` ADD COLUMN`" + tPropertyInfo.Name + "` " + PropertyInfoToSQLiteType(tPropertyInfo) + ";");
                    }
                }
            }
            else if (sState == NWDSQLiteTableState.Create)
            {
                StringBuilder tQueryBuilder = new StringBuilder();
                tQueryBuilder.Append("CREATE TABLE IF NOT EXISTS `" + ClassNamePHP + "` (");
                List<string> PropertiesSQL = new List<string>();
                foreach (PropertyInfo tPropertyInfo in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (tPropertyInfo != null)
                    {
                        PropertiesSQL.Add("`" + tPropertyInfo.Name + "` " + PropertyInfoToSQLiteType(tPropertyInfo));
                    }
                }
                tQueryBuilder.Append(string.Join(",", PropertiesSQL.ToArray()));
                tQueryBuilder.AppendLine(");");
                tQuery.Add(tQueryBuilder.ToString());
            }
            return tQuery.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CreateIndexSQLite(NWDSQLiteTableState sState)
        {
            StringBuilder tQuery = new StringBuilder();
            if (sState == NWDSQLiteTableState.Create || sState == NWDSQLiteTableState.Migrate)
            {
                tQuery.Append("CREATE UNIQUE INDEX IF NOT EXISTS `" + ClassNamePHP + "_Index` ON `" + ClassNamePHP + "` (`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "`);");
            }
            return tQuery.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CreateIndexBundleSQLite(NWDSQLiteTableState sState)
        {
            StringBuilder tQuery = new StringBuilder();
            if (ClassType.IsSubclassOf(typeof(NWDBasisBundled)))
            {
                if (sState == NWDSQLiteTableState.Create || sState == NWDSQLiteTableState.Migrate)
                {
                    tQuery.AppendLine("CREATE INDEX IF NOT EXISTS `" + ClassNamePHP + "_Bundle` ON `" + ClassNamePHP + "` (`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDBasisBundled>().Bundle) + "`);");
                }
            }
            return tQuery.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CreateIndexModifiySQLite(NWDSQLiteTableState sState)
        {
            StringBuilder tQuery = new StringBuilder();
            if (sState == NWDSQLiteTableState.Create || sState == NWDSQLiteTableState.Migrate)
            {
                tQuery.AppendLine("CREATE INDEX IF NOT EXISTS `" + ClassNamePHP + "_Modified` ON `" + ClassNamePHP + "` (`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DM) + "`);");
            }
            return tQuery.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CleanTable()
        {
            //NWDDebug.Log("New_CleanTable()");
            List<NWDTypeClass> tObjectsListToDelete = new List<NWDTypeClass>();
            foreach (NWDTypeClass tObject in Datas)
            {
                if (tObject.XX > 0)
                {
                    tObjectsListToDelete.Add(tObject);
                }
            }
            foreach (NWDTypeClass tObject in tObjectsListToDelete)
            {
#if UNITY_EDITOR
                if (IsObjectInEdition(tObject))
                {
                    SetObjectInEdition(null);
                }
#endif
                tObject.DeleteData();
            }

#if UNITY_EDITOR
            RepaintTableEditor();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PurgeTable()
        {
            List<object> tObjectsListToDelete = new List<object>();
            // clean object not mine!
            foreach (NWDTypeClass tObject in Datas)
            {
                if (tObject.IsReacheableBy(null, null) == false)
                {
                    tObjectsListToDelete.Add(tObject);
                }
            }
            foreach (NWDTypeClass tObject in tObjectsListToDelete)
            {
#if UNITY_EDITOR
                if (IsObjectInEdition(tObject))
                {
                    SetObjectInEdition(null);
                }
#endif
                tObject.DeleteData();
            }

#if UNITY_EDITOR
            RepaintTableEditor();
#endif
        }
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public void FilterTableEditor()
        {
            EditorTableDatas = new List<NWDTypeClass>();
            EditorTableDatasSelected = new Dictionary<NWDTypeClass, bool>();
            foreach (NWDTypeClass tObject in Datas)
            {
                bool tOccurence = true;

                if (tObject.TestIntegrityResult == false && m_ShowIntegrityError == false)
                {
                    tOccurence = false;
                }
                if (tObject.IsEnable() == true && m_ShowEnable == false)
                {
                    tOccurence = false;
                }
                if (tObject.IsEnable() == false && m_ShowDisable == false)
                {
                    tOccurence = false;
                }
                if (tObject.XX > 0 && m_ShowTrashed == false)
                {
                    tOccurence = false;
                }

                if (ClassType != typeof(NWDAccount))
                {
                    if (string.IsNullOrEmpty(m_SearchAccount) == false)
                    {
                        if (m_SearchAccount == "-=-") // empty
                        {
                            if (tObject.IsReacheableBy(null, string.Empty) == false)
                            {
                                tOccurence = false;
                            }
                        }
                        else if (m_SearchAccount == "-+-") // not empty
                        {
                            if (tObject.IsReacheableBy(null, string.Empty) == true)
                            {
                                tOccurence = false;
                            }
                        }
                        else
                        {
                            if (tObject.IsReacheableBy(null, m_SearchAccount) == false)
                            {
                                tOccurence = false;
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(m_SearchAccount) == false)
                    {
                        if (m_SearchAccount == "-=-") // empty
                        {
                        }
                        else if (m_SearchAccount == "-+-") // not empty
                        {
                        }
                        else if (tObject.Reference != m_SearchAccount)
                        {
                            tOccurence = false;
                        }
                    }
                }

                if (string.IsNullOrEmpty(m_SearchGameSave) == false)
                {
                    if (m_SearchGameSave == "-=-")
                    {
                        if (tObject.IsReacheableBy(string.Empty) == false)
                        {
                            tOccurence = false;
                        }
                    }
                    else if (m_SearchGameSave == "-+-")
                    {
                        if (tObject.IsReacheableBy(string.Empty) == true)
                        {
                            tOccurence = false;
                        }
                    }
                    else
                    {
                        if (tObject.IsReacheableBy(m_SearchGameSave) == false)
                        {
                            tOccurence = false;
                        }
                    }
                }
                if (string.IsNullOrEmpty(m_SearchReference) == false)
                {
                    if (tObject.Reference.Contains(m_SearchReference) == false)
                    {
                        tOccurence = false;
                    }
                }
                if (string.IsNullOrEmpty(m_SearchInternalName) == false)
                {
                    if (tObject.InternalKey.ToLower().Contains(m_SearchInternalName.ToLower()) == false)
                    {
                        tOccurence = false;
                    }
                }
                if (string.IsNullOrEmpty(m_SearchInternalDescription) == false)
                {
                    if (tObject.InternalDescription.ToLower().Contains(m_SearchInternalDescription.ToLower()) == false)
                    {
                        tOccurence = false;
                    }
                }
                if (m_SearchTag != NWDBasisTag.NoTag)
                {
                    if (tObject.Tag != m_SearchTag /*&& tObject.Tag != NWDBasisTag.NoTag*/)
                    {
                        tOccurence = false;
                    }
                }
                if (m_SearchCheckList != NWDBasisCheckList.Nothing)
                {
                    if (tObject.CheckList != null)
                    {
                        if (tObject.CheckList.ContainsMask(m_SearchCheckList) == false)
                        {
                            tOccurence = false;
                        }
                    }
                }
                if (tOccurence == true)
                {
                    if (EditorTableDatas.Contains(tObject) == false)
                    {
                        EditorTableDatas.Add(tObject);
                    }
                    if (EditorTableDatasSelected.ContainsKey(tObject) == false)
                    {
                        EditorTableDatasSelected.Add(tObject, false);
                    }
                }
            }
            SortEditorTableDatas();
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        private void ReadCol(Type tTypeOfThis, PropertyInfo tProp, IntPtr stmtc, int i, object tD)
        {
            NWDBenchmark.QuickStart();
            if (tTypeOfThis == typeof(int) ||
                                tTypeOfThis == typeof(Int16) ||
                                tTypeOfThis == typeof(Int32) ||
                                tTypeOfThis.IsEnum
                                )
            {
                tProp.SetValue(tD, Sqlite.ColumnInt(stmtc, i));
                return;
            }
            else if (tTypeOfThis == typeof(long) ||
                tTypeOfThis == typeof(Int64)
                )
            {
                tProp.SetValue(tD, Sqlite.ColumnInt64(stmtc, i));
                return;
            }
            else if (tTypeOfThis == typeof(double) ||
                tTypeOfThis == typeof(Double))
            {
                tProp.SetValue(tD, Sqlite.ColumnDouble(stmtc, i));
                return;
            }
            else if (tTypeOfThis == typeof(float))
            {
                tProp.SetValue(tD, (float)Sqlite.ColumnDouble(stmtc, i));
                return;
            }
            else if (tTypeOfThis == typeof(bool))
            {
                tProp.SetValue(tD, Sqlite.ColumnInt(stmtc, i) == 1);
                return;
            }
            else if (tTypeOfThis == typeof(string) || tTypeOfThis == typeof(String))
            {
                tProp.SetValue(tD, Sqlite.ColumnString(stmtc, i));
                return;
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
            {
                NWEDataType tV = tProp.GetValue(tD) as NWEDataType;
                if (tV == null)
                {
                    string tVV = Sqlite.ColumnString(stmtc, i);
                    //if (string.IsNullOrEmpty(tVV) && NWDAppConfiguration.SharedInstance().NeverNullDataType == false)
                    if (string.IsNullOrEmpty(tVV))
                    {
#if NWD_NEVER_NULL_DATATYPE
                        tV = Activator.CreateInstance(tTypeOfThis) as NWEDataType;
                        tV.Value = tVV;
                        tProp.SetValue(tD, tV);
#endif
                    }
                    else
                    {
                        tV = Activator.CreateInstance(tTypeOfThis) as NWEDataType;
                        tV.Value = tVV;
                        tProp.SetValue(tD, tV);
                    }
                }
                else
                {
                    tV.Value = Sqlite.ColumnString(stmtc, i);
                }
                return;
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)))
            {
                NWEDataTypeInt tV = tProp.GetValue(tD) as NWEDataTypeInt;
                if (tV == null)
                {
                    tV = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeInt;
                    tProp.SetValue(tD, tV);
                }
                tV.Value = Sqlite.ColumnInt64(stmtc, i);
                return;
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)))
            {
                NWEDataTypeEnum tV = tProp.GetValue(tD) as NWEDataTypeEnum;
                if (tV == null)
                {
                    tV = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeEnum;
                    tProp.SetValue(tD, tV);
                }
                tV.Value = Sqlite.ColumnInt64(stmtc, i);
                return;
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
            {
                NWEDataTypeMask tV = tProp.GetValue(tD) as NWEDataTypeMask;
                if (tV == null)
                {
                    tV = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeMask;
                    tProp.SetValue(tD, tV);
                }
                tV.Value = Sqlite.ColumnInt64(stmtc, i);
                return;
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)))
            {
                NWEDataTypeFloat tV = tProp.GetValue(tD) as NWEDataTypeFloat;
                if (tV == null)
                {
                    tV = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeFloat;
                    tProp.SetValue(tD, tV);
                }
                tV.Value = Sqlite.ColumnInt64(stmtc, i);
                return;
            }
            NWDBenchmark.QuickFinish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void LoadFromDatabaseByBundle(NWDBundle sBundle, bool sOverrideMemory)
        {
            NWDBenchmark.QuickStart();
            if (sBundle != NWDBundle.ALL && ClassType.IsSubclassOf(typeof(NWDBasisBundled)))
            {
                LoadFromDatabase("WHERE `Bundle` = \"" + sBundle.ToLong() + "\"", sOverrideMemory);
            }
            else
            {
                LoadFromDatabase(string.Empty, sOverrideMemory);
            }
            NWDBenchmark.QuickFinish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void VerifLoadFromDatabaseForEditor()
        {
            NWDBenchmark.QuickStart();
            if (IsLoaded() == false)
            {
                LoadFromDatabaseByBundle(NWDBundle.ALL, true);
            }
            NWDBenchmark.QuickFinish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void LoadFromDatabaseByReference(string sReference, bool sOverrideMemory)
        {
            NWDBenchmark.QuickStart();
            LoadFromDatabase("WHERE `Reference` = \"" + sReference + "\"", sOverrideMemory);
            NWDBenchmark.QuickFinish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void LoadFromDatabaseByReferences(string[] sReferences, bool sOverrideMemory)
        {
            NWDBenchmark.QuickStart();
            LoadFromDatabase("WHERE `Reference` IN \"" + string.Join("", sReferences) + "\"", sOverrideMemory);
            NWDBenchmark.QuickFinish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void LoadFromDatabase(string sWhere, bool sOverrideMemory)
        {
            NWDDebug.Log("LoadFromDatabase" + ClassNamePHP);
            NWDBenchmark.QuickStart();
            NWDBenchmark.QuickStart("?");
            NWDBenchmark.QuickStart("Data Loading " + ClassNamePHP);
            // if no bundle class 
            // else do with bundle 
            NWDBenchmarkLauncher.Start();
            int tCount = 0;
#if UNITY_EDITOR
            NWDDataManager.SharedInstance().DataQueueExecute();
            RowAnalyzed = false;
#endif
            //ResetDatas();
            IntPtr tConnectorHandle = NWDDataManager.SharedInstance().SQLiteDeviceHandle;
            //if (kAccountDependent == false)
            //if (TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
            if (TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseEditor)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }

            List<PropertyInfo> tProplistA = new List<PropertyInfo>();
            List<Type> tPropTypelistA = new List<Type>();
            List<string> tColumnListA = new List<string>();
            //List<PropertyInfo> tPropTypelistToCreate = new List<PropertyInfo>();

            foreach (PropertyInfo tProp in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {

                tProplistA.Add(tProp);
                tPropTypelistA.Add(tProp.PropertyType);
                tColumnListA.Add(tProp.Name);

                //if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataType)))
                //{
                //    tPropTypelistToCreate.Add(tProp);
                //}
                //else if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataTypeInt)))
                //{
                //    tPropTypelistToCreate.Add(tProp);
                //}
                //else if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataTypeEnum)))
                //{
                //    tPropTypelistToCreate.Add(tProp);
                //}
                //else if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataTypeMask)))
                //{
                //    tPropTypelistToCreate.Add(tProp);
                //}
                //else if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataTypeFloat)))
                //{
                //    tPropTypelistToCreate.Add(tProp);
                //}
            }

            PropertyInfo[] tProplist = tProplistA.ToArray();
            Type[] tPropTypelist = tPropTypelistA.ToArray();
            string[] tColumnList = tColumnListA.ToArray();
            int tReferenceIndex = Array.IndexOf(tColumnList, "Reference");
            //NWDBenchmarkLauncher.Step();
            string tSQL = "SELECT `" + string.Join("`, `", tColumnList) + "` FROM `" + ClassNamePHP + "` " + sWhere + ";";
            try
            {
                IntPtr stmtc = Sqlite.Prepare2(tConnectorHandle, tSQL);
                while (Sqlite.Step(stmtc) == SQLite3.Result.Row)
                {
                    NWDBenchmark.QuickStart("Data Loading row " + ClassNamePHP);
                    string tReferenceFromDataBase = Sqlite.ColumnString(stmtc, tReferenceIndex);
                    if (DatasByReference.ContainsKey(tReferenceFromDataBase) == false)
                    {
                        // create new one object
                        //var tD = CreateInstance_Bypass(false, true, tPropTypeArrayToCreate);
                        var tD = CreateInstance_Bypass(false, true, null);
                        for (int tI = 0; tI < tProplist.Length; tI++)
                        {
                            ReadCol(tPropTypelist[tI], tProplist[tI], stmtc, tI, tD);
                        }
                        tD.LoadedFromDatabase();
                    }
                    else
                    {
                        if (sOverrideMemory == true)
                        {
                            // restaure data value!
                            var tD = DatasByReference[tReferenceFromDataBase];
                            for (int tI = 0; tI < tProplist.Length; tI++)
                            {
                                ReadCol(tPropTypelist[tI], tProplist[tI], stmtc, tI, tD);
                            }
                            tD.LoadedFromDatabase();
                        }
                    }
                    tCount++;
                    NWDBenchmark.QuickFinish("Data Loading row " + ClassNamePHP);
                }
                Sqlite.Finalize(stmtc);
            }
            catch
            {
                Debug.LogWarning("LoadFromDatabase IN ERROR from " + ClassNamePHP + "  with " + tSQL + "");
                Debug.LogWarning("error message : " + SQLite3.GetErrmsg(tConnectorHandle));
                if (tConnectorHandle == NWDDataManager.SharedInstance().SQLiteDeviceHandle)
                {
                    Debug.LogWarning("tried on account database from " + ClassNamePHP + " !");
                }
                else if (tConnectorHandle == NWDDataManager.SharedInstance().SQLiteEditorHandle)
                {
                    Debug.LogWarning("tried on editor database from " + ClassNamePHP + " !");
                }
                else
                {
                    Debug.LogWarning("tried on unknow database from " + ClassNamePHP + " !");
                }
            }
            DatasLoaded = true;
            //if (NWDLauncher.ActiveBenchmark)
            //{
            //    NWDBenchmark.Step(true, " " + ClassNamePHP + " " + tCount + " row loaded! Select ... " + sWhere);
            //}
            ClassDatasAreLoaded();
#if UNITY_EDITOR
            FilterTableEditor();
            RepaintTableEditor();
#endif
            NWDBenchmarkLauncher.Finish(true, " " + ClassNamePHP + " " + tCount + " row loaded! DatasByReference count = " + DatasByReference.Count);
            NWDBenchmark.QuickFinish("Data Loading " + ClassNamePHP);
            NWDBenchmark.QuickFinish("?");
            NWDBenchmark.QuickFinish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void UnloadDataByReference(string sReference)
        {
            if (DatasByReference.ContainsKey(sReference))
            {
                NWDTypeClass tData = DatasByReference[sReference];
                tData.DeindexInMemory(); // call override method
                RemoveData(tData);
            }
#if UNITY_EDITOR
            FilterTableEditor();
            RepaintTableEditor();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetTable()
        {
            // reset datas
            ResetDatas();
            // and reset the last sync to 0
            // but the bug persit with the datas pull because BuildTimestamp is defini
            // you must use an pullforce after that!
            SynchronizationSetNewTimestamp(NWDAppConfiguration.SharedInstance().SelectedEnvironment(), -1);
#if UNITY_EDITOR
            // in editor mode reste to 0 all environment
            SynchronizationSetNewTimestamp(NWDAppConfiguration.SharedInstance().DevEnvironment, -1);
            SynchronizationSetNewTimestamp(NWDAppConfiguration.SharedInstance().PreprodEnvironment, -1);
            SynchronizationSetNewTimestamp(NWDAppConfiguration.SharedInstance().ProdEnvironment, -1);

            NWDAppConfiguration.SharedInstance().DevEnvironment.BuildTimestamp = 0;
            NWDAppConfiguration.SharedInstance().PreprodEnvironment.BuildTimestamp = 0;
            NWDAppConfiguration.SharedInstance().ProdEnvironment.BuildTimestamp = 0;
#endif

            // delete indexes and table
            IntPtr tConnectorHandle = NWDDataManager.SharedInstance().SQLiteDeviceHandle;
            //if (kAccountDependent == false)
            //if (TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
            if (TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseEditor)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }
            //IntPtr stmt = Sqlite.Prepare2(tConnectorHandle, "BEGIN TRANSACTION");
            //Sqlite.Step(stmt);
            //Sqlite.Finalize(stmt);
            IntPtr stmt = Sqlite.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS `" + ClassNamePHP + "_Index`;");
            Sqlite.Step(stmt);
            Sqlite.Finalize(stmt);
            stmt = Sqlite.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS `" + ClassNamePHP + "_Bundle`;");
            Sqlite.Step(stmt);
            Sqlite.Finalize(stmt);
            stmt = Sqlite.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS`" + ClassNamePHP + "_Modified`;");
            Sqlite.Step(stmt);
            Sqlite.Finalize(stmt);
            stmt = Sqlite.Prepare2(tConnectorHandle, "DROP TABLE IF EXISTS `" + ClassNamePHP + "`;");
            Sqlite.Step(stmt);
            Sqlite.Finalize(stmt);
            //stmt = Sqlite.Prepare2(tConnectorHandle, "COMMIT");
            //Sqlite.Step(stmt);
            //Sqlite.Finalize(stmt);

            // create table and indexes
            foreach (string tQuery in CreateTableSQLite(NWDSQLiteTableState.Create))
            {
                stmt = Sqlite.Prepare2(tConnectorHandle, tQuery);
                Sqlite.Step(stmt);
                Sqlite.Finalize(stmt);
            }
            string tIndexA = CreateIndexSQLite(NWDSQLiteTableState.Create);
            if (string.IsNullOrEmpty(tIndexA) == false)
            {
                stmt = Sqlite.Prepare2(tConnectorHandle, tIndexA);
                Sqlite.Step(stmt);
                Sqlite.Finalize(stmt);
            }
            string tIndexB = CreateIndexBundleSQLite(NWDSQLiteTableState.Create);
            if (string.IsNullOrEmpty(tIndexB) == false)
            {
                stmt = Sqlite.Prepare2(tConnectorHandle, tIndexB);
                Sqlite.Step(stmt);
                Sqlite.Finalize(stmt);
            }
            string tIndexC = CreateIndexModifiySQLite(NWDSQLiteTableState.Create);
            if (string.IsNullOrEmpty(tIndexC) == false)
            {
                stmt = Sqlite.Prepare2(tConnectorHandle, tIndexC);
                Sqlite.Step(stmt);
                Sqlite.Finalize(stmt);
            }
            // reload empty datas
            LoadFromDatabase(string.Empty, true);
#if UNITY_EDITOR
            // refresh the tables windows
            RepaintTableEditor();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void FlushTable()
        {
            // reset datas
            ResetDatas();
            // delete all datas on table
            IntPtr tConnectorHandle = NWDDataManager.SharedInstance().SQLiteDeviceHandle;
            //if (kAccountDependent == false)
            //if (TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
            if (TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseEditor)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }
            IntPtr stmt = Sqlite.Prepare2(tConnectorHandle, "BEGIN TRANSACTION");
            Sqlite.Step(stmt);
            Sqlite.Finalize(stmt);
            stmt = Sqlite.Prepare2(tConnectorHandle, "DELETE FROM `" + ClassNamePHP + "`;"); // it's the TRUNCATE optimizer syntax for SQLite
            Sqlite.Step(stmt);
            Sqlite.Finalize(stmt);
            stmt = Sqlite.Prepare2(tConnectorHandle, "COMMIT");
            Sqlite.Step(stmt);
            Sqlite.Finalize(stmt);
            stmt = Sqlite.Prepare2(tConnectorHandle, "VACUUM;");
            Sqlite.Step(stmt);
            Sqlite.Finalize(stmt);
            // but the data are loaded ! anyway! just database is empty...
            DatasLoaded = true;
#if UNITY_EDITOR
            // refresh the tables windows
            RepaintTableEditor();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DropTable()
        {
            // reset datas
            ResetDatas();
            // delete indexes and table
            IntPtr tConnectorHandle = NWDDataManager.SharedInstance().SQLiteDeviceHandle;
            //if (kAccountDependent == false)
            //if (TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
            if (TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseEditor)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }
            //IntPtr stmt = Sqlite.Prepare2(tConnectorHandle, "BEGIN TRANSACTION");
            //Sqlite.Step(stmt);
            //Sqlite.Finalize(stmt);
            IntPtr stmt = Sqlite.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS `" + ClassNamePHP + "_Index`;");
            //Debug.Log("DROP INDEX IF EXISTS `" + ClassNamePHP + "_Index`;");
            Sqlite.Step(stmt);
            Sqlite.Finalize(stmt);
            stmt = Sqlite.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS `" + ClassNamePHP + "_Bundle`;");
            //Debug.Log("DROP INDEX IF EXISTS `" + ClassNamePHP + "_Bundle`;");
            Sqlite.Step(stmt);
            Sqlite.Finalize(stmt);
            stmt = Sqlite.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS `" + ClassNamePHP + "_Modified`;");
            //Debug.Log("DROP INDEX IF EXISTS `" + ClassNamePHP + "_Modified`;");
            Sqlite.Step(stmt);
            Sqlite.Finalize(stmt);
            stmt = Sqlite.Prepare2(tConnectorHandle, "DROP TABLE IF EXISTS `" + ClassNamePHP + "`;");
            Sqlite.Step(stmt);
            Sqlite.Finalize(stmt);
            //stmt = Sqlite.Prepare2(tConnectorHandle, "COMMIT");
            //Sqlite.Step(stmt);
            //Sqlite.Finalize(stmt);
#if UNITY_EDITOR
            // refresh the tables windows
            RepaintTableEditor();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RecreateAllIndexForTable()
        {
            // reset datas
            ResetDatas();
            // delete indexes and table
            IntPtr tConnectorHandle = NWDDataManager.SharedInstance().SQLiteDeviceHandle;
            //if (kAccountDependent == false)
            //if (TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
            if (TemplateHelper.GetDeviceDatabase() == NWDTemplateDeviceDatabase.ReccordableInDeviceDatabaseEditor)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }
            IntPtr stmt = Sqlite.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS `" + ClassNamePHP + "_Index`;");
            //Debug.Log("DROP INDEX IF EXISTS `" + ClassNamePHP + "_Index`;");
            Sqlite.Step(stmt);
            Sqlite.Finalize(stmt);
            stmt = Sqlite.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS `" + ClassNamePHP + "_Bundle`;");
            //Debug.Log("DROP INDEX IF EXISTS `" + ClassNamePHP + "_Bundle`;");
            Sqlite.Step(stmt);
            Sqlite.Finalize(stmt);
            stmt = Sqlite.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS `" + ClassNamePHP + "_Modified`;");
            //Debug.Log("DROP INDEX IF EXISTS `" + ClassNamePHP + "_Modified`;");
            Sqlite.Step(stmt);
            Sqlite.Finalize(stmt);

            string tIndexA = CreateIndexSQLite(NWDSQLiteTableState.Create);
            if (string.IsNullOrEmpty(tIndexA) == false)
            {
                stmt = Sqlite.Prepare2(tConnectorHandle, tIndexA);
                Sqlite.Step(stmt);
                Sqlite.Finalize(stmt);
            }
            string tIndexB = CreateIndexBundleSQLite(NWDSQLiteTableState.Create);
            if (string.IsNullOrEmpty(tIndexB) == false)
            {
                stmt = Sqlite.Prepare2(tConnectorHandle, tIndexB);
                Sqlite.Step(stmt);
                Sqlite.Finalize(stmt);
            }
            string tIndexC = CreateIndexModifiySQLite(NWDSQLiteTableState.Create);
            if (string.IsNullOrEmpty(tIndexC) == false)
            {
                stmt = Sqlite.Prepare2(tConnectorHandle, tIndexC);
                Sqlite.Step(stmt);
                Sqlite.Finalize(stmt);
            }
#if UNITY_EDITOR
            // refresh the tables windows
            RepaintTableEditor();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
