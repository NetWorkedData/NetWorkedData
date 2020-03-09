//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //public class NWDColumnInfo
    //{
    //    public string Name
    //    {
    //        get; set;
    //    }
    //    public int notnull
    //    {
    //        get; set;
    //    }
    //    public override string ToString()
    //    {
    //        return this.Name;
    //    }
    //}
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
        private string PropertyInfoToSQLiteType(PropertyInfo sPropertyInfo, bool sJustType = false)
        {
            //-------------------------------------------------------------------------------------------------------------
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
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        List<PropertyInfo> tActualsList = new List<PropertyInfo>();
        List<PropertyInfo> tMigratePropertyList = new List<PropertyInfo>();
        //-------------------------------------------------------------------------------------------------------------
        public NWDSQLiteTableState TableSQLiteState()
        {
            NWDSQLiteTableState rReturn = NWDSQLiteTableState.Error;
            StringBuilder tQuery = new StringBuilder();
            tActualsList = new List<PropertyInfo>(ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            tMigratePropertyList = new List<PropertyInfo>(ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            IntPtr tConnectorHandle = NWDDataManager.SharedInstance().SQLiteAccountHandle;
            //SQLiteConnection Connector = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            if (kAccountDependent == false)
            {
                //Connector = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }
            IntPtr stmt = SQLite3.Prepare2(tConnectorHandle, "PRAGMA table_info(`" + ClassNamePHP + "`)");

            bool tError = true;
            while (SQLite3.Step(stmt) == SQLite3.Result.Row)
            {
                tError = false;
                //Debug.Log("???");
                //Debug.Log(SQLite3.ColumnString(stmt,1) + " " + SQLite3.ColumnString(stmt, 2));
                string tPropName = SQLite3.ColumnString(stmt, 1);
                PropertyInfo tPropertyInfo = ClassType.GetProperty(tPropName);
                if (tPropertyInfo == null)
                {
                    //tNeedUpdate = true;
                    // Do nothing, old datas stay in table... migrate is too long
                    //Debug.Log("tAuto " + tPropName + " ====> UNKNOW");
                }
                else
                {
                    tActualsList.Remove(tPropertyInfo);
                    tMigratePropertyList.Add(tPropertyInfo);
                    string tActual = "`" + SQLite3.ColumnString(stmt, 1) + "` " + SQLite3.ColumnString(stmt, 2);
                    string tAuto = "`" + tPropertyInfo.Name + "` " + PropertyInfoToSQLiteType(tPropertyInfo, true);
                    if (tAuto != tActual)
                    {
                        rReturn = NWDSQLiteTableState.Migrate;
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
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
        public string CreateTableSQLite(NWDSQLiteTableState sState)
        {
            StringBuilder tQuery = new StringBuilder();

            IntPtr tConnectorHandle = NWDDataManager.SharedInstance().SQLiteAccountHandle;
            if (kAccountDependent == false)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }


            if (sState == NWDSQLiteTableState.Migrate)
            {
                tQuery.Append("CREATE TABLE IF NOT EXISTS `" + ClassNamePHP + "_new` (");
                List<string> PropertiesSQL = new List<string>();
                foreach (PropertyInfo tPropertyInfo in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (tPropertyInfo != null)
                    {
                        PropertiesSQL.Add("`" + tPropertyInfo.Name + "` " + PropertyInfoToSQLiteType(tPropertyInfo));
                    }
                }
                tQuery.Append(string.Join(",", PropertiesSQL.ToArray()));
                tQuery.Append(");");
                tQuery.Append("INSERT INTO `" + ClassNamePHP + "_new` (");
                List<string> tMigratePropertyListName = new List<string>();
                foreach (PropertyInfo tPropertyInfo in tMigratePropertyList)
                {
                    tMigratePropertyListName.Add("`" + tPropertyInfo.Name + "`");
                }
                tQuery.Append(string.Join(",", PropertiesSQL.ToArray()));
                tQuery.Append(") SELECT ");
                tQuery.Append(string.Join(",", PropertiesSQL.ToArray()));
                tQuery.Append("FROM `" + ClassNamePHP + "`;");
                tQuery.Append("DROP TABLE `" + ClassNamePHP + "`;");
                tQuery.Append("ALTER TABLE `" + ClassNamePHP + "_new` RENAME TO `" + ClassNamePHP + "`;");
                //Debug.Log(tQuery.ToString());
            }
            else if (sState == NWDSQLiteTableState.Update)
            {
                foreach (PropertyInfo tPropertyInfo in tActualsList)
                {
                    if (tPropertyInfo != null)
                    {
                        tQuery.AppendLine("ALTER TABLE `" + ClassNamePHP + "` ADD COLUMN`" + tPropertyInfo.Name + "` " + PropertyInfoToSQLiteType(tPropertyInfo) + ";");
                    }
                }
                //Debug.Log(tQuery.ToString());
            }
            else if (sState == NWDSQLiteTableState.Create)
            {
                tQuery.Append("CREATE TABLE IF NOT EXISTS `" + ClassNamePHP + "` (");
                List<string> PropertiesSQL = new List<string>();
                foreach (PropertyInfo tPropertyInfo in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    if (tPropertyInfo != null)
                    {
                        PropertiesSQL.Add("`" + tPropertyInfo.Name + "` " + PropertyInfoToSQLiteType(tPropertyInfo));
                    }
                }
                tQuery.Append(string.Join(",", PropertiesSQL.ToArray()));
                tQuery.AppendLine(");");
                //Debug.Log(tQuery.ToString());
            }
            else
            {
                //Debug.Log("Table `" + ClassNamePHP + "` is ok!");
            }
            return tQuery.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CreateIndexSQLite(NWDSQLiteTableState sState)
        {
            StringBuilder tQuery = new StringBuilder();
            if (sState == NWDSQLiteTableState.Create)
            {
                tQuery.Append("CREATE UNIQUE INDEX IF NOT EXISTS `" + ClassNamePHP + "_Index` ON `" + ClassNamePHP + "` (`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "`);");
            }
            return tQuery.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CreateIndexBundleSQLite(NWDSQLiteTableState sState)
        {
            StringBuilder tQuery = new StringBuilder();
            if (sState == NWDSQLiteTableState.Create)
            {
                tQuery.AppendLine("CREATE INDEX IF NOT EXISTS `" + ClassNamePHP + "_Bundle` ON `" + ClassNamePHP + "` (`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Bundle) + "`);");
            }
            return tQuery.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CreateIndexModifiySQLite(NWDSQLiteTableState sState)
        {
            StringBuilder tQuery = new StringBuilder();
            if (sState == NWDSQLiteTableState.Create)
            {
                tQuery.AppendLine("CREATE INDEX IF NOT EXISTS `" + ClassNamePHP + "_Modified` ON `" + ClassNamePHP + "` (`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().DM) + "`);");
            }
            return tQuery.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void CreateTable()
        //{
        //    NWDDataManager.SharedInstance().CreateTable(ClassType, kAccountDependent);
        //}
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
                if (tObject.IsReacheableByAccount() == false)
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
        //public void UpdateDataTable()
        //{
        //    NWDDataManager.SharedInstance().MigrateTable(ClassType, kAccountDependent);
        //    //List<object> tObjectsListToDelete = new List<object>();
        //    foreach (NWDTypeClass tObject in Datas)
        //    {
        //        tObject.UpdateData();
        //    }
        //}

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
                            if (tObject.VisibleByAccountByEqual(string.Empty) == false)
                            {
                                tOccurence = false;
                            }
                        }
                        else if (m_SearchAccount == "-+-") // not empty
                        {
                            if (tObject.VisibleByAccountByEqual(string.Empty) == true)
                            {
                                tOccurence = false;
                            }
                        }
                        else
                        {
                            if (tObject.VisibleByAccount(m_SearchAccount) == false)
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
                        if (tObject.VisibleByGameSave(string.Empty) == false)
                        {
                            tOccurence = false;
                        }
                    }
                    else if (m_SearchGameSave == "-+-")
                    {
                        if (tObject.VisibleByGameSave(string.Empty) == true)
                        {
                            tOccurence = false;
                        }
                    }
                    else
                    {
                        if (tObject.VisibleByGameSave(m_SearchGameSave) == false)
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
            if (tTypeOfThis == typeof(int) ||
                                tTypeOfThis == typeof(Int16) ||
                                tTypeOfThis == typeof(Int32) ||
                                tTypeOfThis.IsEnum
                                )
            {
                tProp.SetValue(tD, SQLite3.ColumnInt(stmtc, i));
                return;
            }
            else if (tTypeOfThis == typeof(long) ||
                tTypeOfThis == typeof(Int64)
                )
            {
                tProp.SetValue(tD, SQLite3.ColumnInt64(stmtc, i));
                return;
            }
            else if (tTypeOfThis == typeof(double) ||
                tTypeOfThis == typeof(Double))
            {
                tProp.SetValue(tD, SQLite3.ColumnDouble(stmtc, i));
                return;
            }
            else if (tTypeOfThis == typeof(float))
            {
                tProp.SetValue(tD, (float)SQLite3.ColumnDouble(stmtc, i));
                return;
            }
            else if (tTypeOfThis == typeof(bool))
            {
                tProp.SetValue(tD, SQLite3.ColumnInt(stmtc, i) == 1);
                return;
            }
            else if (tTypeOfThis == typeof(string) || tTypeOfThis == typeof(String))
            {
                tProp.SetValue(tD, SQLite3.ColumnString(stmtc, i));
                return;
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataType)))
            {
                //NWEDataType tV = Activator.CreateInstance(tTypeOfThis) as NWEDataType;
                //tV.Value = SQLite3.ColumnString(stmtc, i);
                //tProp.SetValue(tD, tV);

                NWEDataType tV = tProp.GetValue(tD) as NWEDataType;
                tV.Value = SQLite3.ColumnString(stmtc, i);
                return;
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)))
            {
                //NWEDataTypeInt tV = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeInt;
                //tV.Value = SQLite3.ColumnInt64(stmtc, i);
                //tProp.SetValue(tD, tV);

                NWEDataTypeInt tV = tProp.GetValue(tD) as NWEDataTypeInt;
                tV.Value = SQLite3.ColumnInt64(stmtc, i);
                return;
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)))
            {
                //NWEDataTypeEnum tV = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeEnum;
                //tV.Value = SQLite3.ColumnInt64(stmtc, i);
                //tProp.SetValue(tD, tV);

                NWEDataTypeEnum tV = tProp.GetValue(tD) as NWEDataTypeEnum;
                tV.Value = SQLite3.ColumnInt64(stmtc, i);
                return;
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
            {
                //NWEDataTypeMask tV = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeMask;
                //tV.Value = SQLite3.ColumnInt64(stmtc, i);
                //tProp.SetValue(tD, tV);

                NWEDataTypeMask tV = tProp.GetValue(tD) as NWEDataTypeMask;
                tV.Value = SQLite3.ColumnInt64(stmtc, i);
                return;
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)))
            {
                //NWEDataTypeFloat tV = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeFloat;
                //tV.Value = SQLite3.ColumnDouble(stmtc, i);
                //tProp.SetValue(tD, tV);

                NWEDataTypeFloat tV = tProp.GetValue(tD) as NWEDataTypeFloat;
                tV.Value = SQLite3.ColumnInt64(stmtc, i);
                return;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void LoadFromDatabaseByBundle(NWDBasisBundle sBundle)
        {
            if (sBundle == NWDBasisBundle.ALL)
            {
                LoadFromDatabase(string.Empty);
            }
            else
            {
                LoadFromDatabase("WHERE `Bundle` = \"" + sBundle.ToLong() + "\"");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void LoadFromDatabaseByReference(string sReference)
        {
            LoadFromDatabase("WHERE `Reference` = \"" + sReference + "\"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void LoadFromDatabaseByReferences(string[] sReferences)
        {
            LoadFromDatabase("WHERE `Reference` IN \"" + string.Join("", sReferences) + "\"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void LoadFromDatabase(string sWhere = "WHERE `Bundle` = \"0\"")
        {
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            int tCount = 0;
#if UNITY_EDITOR
            NWDDataManager.SharedInstance().DataQueueExecute();
            RowAnalyzed = false;
#endif
            //ResetDatas();
            IntPtr tConnectorHandle = NWDDataManager.SharedInstance().SQLiteAccountHandle;
            if (kAccountDependent == false)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }

            List<PropertyInfo> tProplistA = new List<PropertyInfo>();
            List<Type> tPropTypelistA = new List<Type>();
            List<string> tColumnListA = new List<string>();
            List<PropertyInfo> tPropTypelistToCreate = new List<PropertyInfo>();

            foreach (PropertyInfo tProp in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {

                tProplistA.Add(tProp);
                tPropTypelistA.Add(tProp.PropertyType);
                tColumnListA.Add(tProp.Name);

                if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataType)))
                {
                    tPropTypelistToCreate.Add(tProp);
                }
                else if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataTypeInt)))
                {
                    tPropTypelistToCreate.Add(tProp);
                }
                else if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataTypeEnum)))
                {
                    tPropTypelistToCreate.Add(tProp);
                }
                else if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataTypeMask)))
                {
                    tPropTypelistToCreate.Add(tProp);
                }
                else if (tProp.PropertyType.IsSubclassOf(typeof(NWEDataTypeFloat)))
                {
                    tPropTypelistToCreate.Add(tProp);
                }
            }

            PropertyInfo[] tProplist = tProplistA.ToArray();
            Type[] tPropTypelist = tPropTypelistA.ToArray();
            string[] tColumnList = tColumnListA.ToArray();
            int tReferenceIndex = Array.IndexOf(tColumnList, "Reference");
            PropertyInfo[] tPropTypeArrayToCreate = tPropTypelistToCreate.ToArray();

            IntPtr stmtc = SQLite3.Prepare2(tConnectorHandle, "SELECT `" + string.Join("`, `", tColumnList) + "` FROM `" + ClassNamePHP + "` " + sWhere + ";");

            while (SQLite3.Step(stmtc) == SQLite3.Result.Row)
            {
                string tReferenceFromDataBase = SQLite3.ColumnString(stmtc, tReferenceIndex);
                if (DatasByReference.ContainsKey(tReferenceFromDataBase) == false)
                {
                    var tD = CreateInstance_Bypass(false, true, tPropTypeArrayToCreate);
                    for (int tI = 0; tI < tProplist.Length; tI++)
                    {
                        ReadCol(tPropTypelist[tI], tProplist[tI], stmtc, tI, tD);
                    }
                    tD.LoadedFromDatabase();
                }
                tCount++;
            }
            SQLite3.Finalize(stmtc);
            DatasLoaded = true;
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Step(true, " " + ClassNamePHP + " " + tCount + " row loaded! Select ... " + sWhere);
            }
            ClassDatasAreLoaded();
#if UNITY_EDITOR
            FilterTableEditor();
            RepaintTableEditor();
#endif
            if (NWDLauncher.ActiveBenchmark)
            {
                //NWEBenchmark.Step(true, " " + ClassNamePHP + "Datas count = " + Datas.Count);
                //NWEBenchmark.Step(true, " " + ClassNamePHP + "DatasByReference count = " + DatasByReference.Count);
                //NWEBenchmark.Step(true, " " + ClassNamePHP + "DatasByInternalKey count = " + DatasByInternalKey.Count);
                NWEBenchmark.Finish(true, " " + ClassNamePHP + " " + tCount + " row loaded! DatasByReference count = "+ DatasByReference.Count);
            }
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
            // delete indexes and table
            IntPtr tConnectorHandle = NWDDataManager.SharedInstance().SQLiteAccountHandle;
            if (kAccountDependent == false)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }
            //IntPtr stmt = SQLite3.Prepare2(tConnectorHandle, "BEGIN TRANSACTION");
            //SQLite3.Step(stmt);
            //SQLite3.Finalize(stmt);
            IntPtr stmt = SQLite3.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS `" + ClassNamePHP + "_Index`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS `" + ClassNamePHP + "_Bundle`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS`" + ClassNamePHP + "_Modified`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "DROP TABLE IF EXISTS `" + ClassNamePHP + "`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            //stmt = SQLite3.Prepare2(tConnectorHandle, "COMMIT");
            //SQLite3.Step(stmt);
            //SQLite3.Finalize(stmt);

            // create table and indexes
            stmt = SQLite3.Prepare2(tConnectorHandle, CreateTableSQLite(NWDSQLiteTableState.Create));
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            string tIndexA = CreateIndexSQLite(NWDSQLiteTableState.Create);
            if (string.IsNullOrEmpty(tIndexA) == false)
            {
                stmt = SQLite3.Prepare2(tConnectorHandle, tIndexA);
                SQLite3.Step(stmt);
                SQLite3.Finalize(stmt);
            }
            string tIndexB = CreateIndexBundleSQLite(NWDSQLiteTableState.Create);
            if (string.IsNullOrEmpty(tIndexB) == false)
            {
                stmt = SQLite3.Prepare2(tConnectorHandle, tIndexB);
                SQLite3.Step(stmt);
                SQLite3.Finalize(stmt);
            }
            string tIndexC = CreateIndexModifiySQLite(NWDSQLiteTableState.Create);
            if (string.IsNullOrEmpty(tIndexC) == false)
            {
                stmt = SQLite3.Prepare2(tConnectorHandle, tIndexC);
                SQLite3.Step(stmt);
                SQLite3.Finalize(stmt);
            }
            // reload empty datas
            LoadFromDatabase();
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
            IntPtr tConnectorHandle = NWDDataManager.SharedInstance().SQLiteAccountHandle;
            if (kAccountDependent == false)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }
            IntPtr stmt = SQLite3.Prepare2(tConnectorHandle, "BEGIN TRANSACTION");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "DELETE FROM `" + ClassNamePHP + "`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "COMMIT");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "VACUUM;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
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
            IntPtr tConnectorHandle = NWDDataManager.SharedInstance().SQLiteAccountHandle;
            if (kAccountDependent == false)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }
            //IntPtr stmt = SQLite3.Prepare2(tConnectorHandle, "BEGIN TRANSACTION");
            //SQLite3.Step(stmt);
            //SQLite3.Finalize(stmt);
            IntPtr stmt = SQLite3.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS `" + ClassNamePHP + "_Index`;");
            //Debug.Log("DROP INDEX IF EXISTS `" + ClassNamePHP + "_Index`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS `" + ClassNamePHP + "_Bundle`;");
            //Debug.Log("DROP INDEX IF EXISTS `" + ClassNamePHP + "_Bundle`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS `" + ClassNamePHP + "_Modified`;");
            //Debug.Log("DROP INDEX IF EXISTS `" + ClassNamePHP + "_Modified`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "DROP TABLE IF EXISTS `" + ClassNamePHP + "`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            //stmt = SQLite3.Prepare2(tConnectorHandle, "COMMIT");
            //SQLite3.Step(stmt);
            //SQLite3.Finalize(stmt);
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
            IntPtr tConnectorHandle = NWDDataManager.SharedInstance().SQLiteAccountHandle;
            if (kAccountDependent == false)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }
            IntPtr stmt = SQLite3.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS `" + ClassNamePHP + "_Index`;");
            //Debug.Log("DROP INDEX IF EXISTS `" + ClassNamePHP + "_Index`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS `" + ClassNamePHP + "_Bundle`;");
            //Debug.Log("DROP INDEX IF EXISTS `" + ClassNamePHP + "_Bundle`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "DROP INDEX IF EXISTS `" + ClassNamePHP + "_Modified`;");
            //Debug.Log("DROP INDEX IF EXISTS `" + ClassNamePHP + "_Modified`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);

            string tIndexA = CreateIndexSQLite(NWDSQLiteTableState.Create);
            if (string.IsNullOrEmpty(tIndexA) == false)
            {
                stmt = SQLite3.Prepare2(tConnectorHandle, tIndexA);
                SQLite3.Step(stmt);
                SQLite3.Finalize(stmt);
            }
            string tIndexB = CreateIndexBundleSQLite(NWDSQLiteTableState.Create);
            if (string.IsNullOrEmpty(tIndexB) == false)
            {
                stmt = SQLite3.Prepare2(tConnectorHandle, tIndexB);
                SQLite3.Step(stmt);
                SQLite3.Finalize(stmt);
            }
            string tIndexC = CreateIndexModifiySQLite(NWDSQLiteTableState.Create);
            if (string.IsNullOrEmpty(tIndexC) == false)
            {
                stmt = SQLite3.Prepare2(tConnectorHandle, tIndexC);
                SQLite3.Step(stmt);
                SQLite3.Finalize(stmt);
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