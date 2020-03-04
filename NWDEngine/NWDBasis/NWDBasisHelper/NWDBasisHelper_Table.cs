//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:25:6
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;
using System.Text;
using SQLite4Unity3d;
using UnityEngine;

using Sqlite3DatabaseHandle = System.IntPtr;
using Sqlite3Statement = System.IntPtr;

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
        public NWDSQLiteTableState TableSQLiteState()
        {
            NWDSQLiteTableState rReturn = NWDSQLiteTableState.Error;
            StringBuilder tQuery = new StringBuilder();
            List<PropertyInfo> tActualsList = new List<PropertyInfo>(ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            List<PropertyInfo> tMigratePropertyList = new List<PropertyInfo>(ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance));

            Sqlite3DatabaseHandle tConnectorHandle = NWDDataManager.SharedInstance().SQLiteAccountHandle;
            //SQLiteConnection Connector = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            if (kAccountDependent == false)
            {
                //Connector = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }
            Sqlite3DatabaseHandle stmt = SQLite3.Prepare2(tConnectorHandle, "PRAGMA table_info(`" + ClassNamePHP + "`)");
     
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
            List<PropertyInfo> tActualsList = new List<PropertyInfo>(ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            List<PropertyInfo> tMigratePropertyList = new List<PropertyInfo>(ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance));

            Sqlite3DatabaseHandle tConnectorHandle = NWDDataManager.SharedInstance().SQLiteAccountHandle;
            if (kAccountDependent == false)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }


            if (sState == NWDSQLiteTableState.Migrate)
            {
                Debug.Log("tNeedMigrate");
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
                Debug.Log(tQuery.ToString());
            }
            else if (sState ==  NWDSQLiteTableState.Update)
            {
                Debug.Log("tNeedUpdate");
                foreach (PropertyInfo tPropertyInfo in tActualsList)
                {
                    if (tPropertyInfo != null)
                    {
                        tQuery.AppendLine("ALTER TABLE `" + ClassNamePHP + "` ADD COLUMN`" + tPropertyInfo.Name + "` " + PropertyInfoToSQLiteType(tPropertyInfo) + ";");
                    }
                }
                Debug.Log(tQuery.ToString());
            }
            else if (sState ==  NWDSQLiteTableState.Create)
            {
                //Debug.Log("tNeedCreate");
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
                tQuery.AppendLine("CREATE UNIQUE INDEX `" + ClassNamePHP + "_Index` ON `" + ClassNamePHP + "` (`"+ NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "`);");
                tQuery.AppendLine("CREATE INDEX `" + ClassNamePHP + "_Bundle` ON `" + ClassNamePHP + "` (`"+ NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Bundle) + "`);");
                Debug.Log(tQuery.ToString());
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
                tQuery.Append("CREATE UNIQUE INDEX `" + ClassNamePHP + "_Index` ON `" + ClassNamePHP + "` (`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Reference) + "`);");
            }
            return tQuery.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CreateIndexBundleSQLite(NWDSQLiteTableState sState)
        {
            StringBuilder tQuery = new StringBuilder();
            if (sState == NWDSQLiteTableState.Create)
            {
                tQuery.AppendLine("CREATE INDEX `" + ClassNamePHP + "_Bundle` ON `" + ClassNamePHP + "` (`" + NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().Bundle) + "`);");
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
        private void ReadCol(Type tTypeOfThis, PropertyInfo tProp, Sqlite3DatabaseHandle stmtc, int i, object tD)
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
        private void ReadCol_B(Type tTypeOfThis, PropertyInfo tProp, Sqlite3DatabaseHandle stmtc, int i, object tD)
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
                NWEDataType tV = Activator.CreateInstance(tTypeOfThis) as NWEDataType;
                tV.Value = SQLite3.ColumnString(stmtc, i);
                tProp.SetValue(tD, tV);
                return;
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeInt)))
            {
                NWEDataTypeInt tV = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeInt;
                tV.Value = SQLite3.ColumnInt64(stmtc, i);
                tProp.SetValue(tD, tV);
                return;
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeEnum)))
            {
                NWEDataTypeEnum tV = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeEnum;
                tV.Value = SQLite3.ColumnInt64(stmtc, i);
                tProp.SetValue(tD, tV);
                return;
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeMask)))
            {
                NWEDataTypeMask tV = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeMask;
                tV.Value = SQLite3.ColumnInt64(stmtc, i);
                tProp.SetValue(tD, tV);
                return;
            }
            else if (tTypeOfThis.IsSubclassOf(typeof(NWEDataTypeFloat)))
            {
                NWEDataTypeFloat tV = Activator.CreateInstance(tTypeOfThis) as NWEDataTypeFloat;
                tV.Value = SQLite3.ColumnDouble(stmtc, i);
                tProp.SetValue(tD, tV);
                return;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
<<<<<<< HEAD
        public void LoadFromDatabase()
=======
        public virtual void LoadFromDatabase()
>>>>>>> optimisation
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
            Sqlite3DatabaseHandle tConnectorHandle = NWDDataManager.SharedInstance().SQLiteAccountHandle;
            if (kAccountDependent == false)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }
<<<<<<< HEAD
                    List<PropertyInfo> tProplist = new List<PropertyInfo>();
                    List<Type> tPropTypelist = new List<Type>();
                    List<string> tColumnList = new List<string>();
                    foreach (PropertyInfo tProp in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                            tProplist.Add(tProp);
                            tPropTypelist.Add(tProp.PropertyType);
                            tColumnList.Add(tProp.Name);
                    }
                    Sqlite3DatabaseHandle stmtc = SQLite3.Prepare2(tConnectorHandle, "SELECT `" + string.Join("`, `", tColumnList) + "` FROM `" + ClassNamePHP + "`;");
=======
            if (tSQLiteConnection != null)
            {
                if (tSQLiteConnection.IsValid())
                {
                    List<PropertyInfo> tProplistA = new List<PropertyInfo>();
                    List<Type> tPropTypelistA = new List<Type>();
                    List<string> tColumnListA = new List<string>();
                    List<PropertyInfo> tPropTypelistToCreate = new List<PropertyInfo>();
                    foreach (PropertyInfo tProp in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        //if (tProp.Name != "ID" && string.IsNullOrEmpty(tProp.Name) == false)
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
                    }

                    PropertyInfo[] tProplist = tProplistA.ToArray();
                    Type[] tPropTypelist = tPropTypelistA.ToArray();
                    string[] tColumnList = tColumnListA.ToArray();
                    PropertyInfo[] tPropTypeArrayToCreate = tPropTypelistToCreate.ToArray();

                    // FOR TEST {
                    //NWEBenchmark.Start("eee");
                    //Sqlite3DatabaseHandle stmtcT = SQLite3.Prepare2(tSQLiteConnection.Handle, "SELECT `" + string.Join("`, `", tColumnList) + "` FROM `" + ClassNamePHP + "`;");
                    //while (SQLite3.Step(stmtcT) == SQLite3.Result.Row)
                    //{
                    //    var tD = CreateInstance_Bypass(false, true);
                    //}
                    //NWEBenchmark.Finish("eee");
                    //NWEBenchmark.Start("ffff");
                    //Sqlite3DatabaseHandle stmtcA = SQLite3.Prepare2(tSQLiteConnection.Handle, "SELECT `" + string.Join("`, `", tColumnList) + "` FROM `" + ClassNamePHP + "`;");
                    //while (SQLite3.Step(stmtcA) == SQLite3.Result.Row)
                    //{
                    //    var tD = Activator.CreateInstance(ClassType, new object[] { false });
                    //}
                    //NWEBenchmark.Finish("ffff");
                    // FOR TEST }

                    Sqlite3DatabaseHandle stmtc = SQLite3.Prepare2(tSQLiteConnection.Handle, "SELECT `" + string.Join("`, `", tColumnList) + "` FROM `" + ClassNamePHP + "`;");
>>>>>>> optimisation

                    while (SQLite3.Step(stmtc) == SQLite3.Result.Row)
                    {
                        var tD = CreateInstance_Bypass(false, true, tPropTypeArrayToCreate);
                        for (int tI = 0; tI < tProplist.Length; tI++)
                        {
<<<<<<< HEAD
                            PropertyInfo tProp = tProplist[i];
                            Type tTypeOfThis = tPropTypelist[i];
                            ReadCol(tTypeOfThis, tProp, stmtc, i, tD);
=======
                            ReadCol(tPropTypelist[tI], tProplist[tI], stmtc, tI, tD);
>>>>>>> optimisation
                        }
                        ((NWDTypeClass)tD).LoadedFromDatabase();
                        tCount++;
                    }
<<<<<<< HEAD
                    SQLite3.Finalize(stmtc);
=======

                    //while (SQLite3.Step(stmtc) == SQLite3.Result.Row)
                    //{
                    //    var tD = Activator.CreateInstance(ClassType, new object[] { false });
                    //    for (int tI = 0; tI < tProplist.Length; tI++)
                    //    {
                    //        ReadCol_B(tPropTypelist[tI], tProplist[tI], stmtc, tI, tD);
                    //    }
                    //    ((NWDTypeClass)tD).LoadedFromDatabase();
                    //    tCount++;
                    //}
                    SQLite3.Finalize(stmtc);
                }
            }
>>>>>>> optimisation
            DatasLoaded = true;
            ClassDatasAreLoaded();
#if UNITY_EDITOR
            FilterTableEditor();
            RepaintTableEditor();
#endif
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Finish(true, " " + ClassNamePHP + " " + tCount + " row loaded!");
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
            Sqlite3DatabaseHandle tConnectorHandle = NWDDataManager.SharedInstance().SQLiteAccountHandle;
            if (kAccountDependent == false)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }
            Sqlite3DatabaseHandle stmt = SQLite3.Prepare2(tConnectorHandle, "BEGIN TRANSACTION");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "DROP INDEX `" + ClassNamePHP + "_Index`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "DROP INDEX `" + ClassNamePHP + "_Bundle`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle,"DROP TABLE IF EXISTS `" +ClassNamePHP + "`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            // create table and indexes
            stmt = SQLite3.Prepare2(tConnectorHandle, CreateTableSQLite( NWDSQLiteTableState.Create));
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
            stmt = SQLite3.Prepare2(tConnectorHandle, "COMMIT");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
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
            Sqlite3DatabaseHandle tConnectorHandle = NWDDataManager.SharedInstance().SQLiteAccountHandle;
            if (kAccountDependent == false)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }
            Sqlite3DatabaseHandle stmt = SQLite3.Prepare2(tConnectorHandle, "BEGIN TRANSACTION");
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
            Sqlite3DatabaseHandle tConnectorHandle = NWDDataManager.SharedInstance().SQLiteAccountHandle;
            if (kAccountDependent == false)
            {
                tConnectorHandle = NWDDataManager.SharedInstance().SQLiteEditorHandle;
            }
            Sqlite3DatabaseHandle stmt = SQLite3.Prepare2(tConnectorHandle, "BEGIN TRANSACTION");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "DROP INDEX `" + ClassNamePHP + "_Index`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "DROP INDEX `" + ClassNamePHP + "_Bundle`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "DROP TABLE IF EXISTS `" + ClassNamePHP + "`;");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
            stmt = SQLite3.Prepare2(tConnectorHandle, "COMMIT");
            SQLite3.Step(stmt);
            SQLite3.Finalize(stmt);
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