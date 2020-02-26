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
    public class NWDColumnInfo
    {
        public string Name
        {
            get; set;
        }
        public int notnull
        {
            get; set;
        }
        public override string ToString()
        {
            return this.Name;
        }
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
                    //rReturn += " primary key not null";
                }
                if (sPropertyInfo.Name == NWDToolbox.PropertyName(() => NWDBasisHelper.FictiveData<NWDExample>().ID))
                {
                    rReturn += " primary key autoincrement not null default 0 ";
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string CreateTableSQLite()
        {
            StringBuilder tQuery = new StringBuilder();
            List<PropertyInfo> tActualsList = new List<PropertyInfo>(ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance));
            List<PropertyInfo> tMigratePropertyList = new List<PropertyInfo>(ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance));

            SQLiteConnection Connector = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            if (kAccountDependent == false)
            {
                Connector = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
            }
            Sqlite3DatabaseHandle stmt = SQLite3.Prepare2(Connector.Handle, "PRAGMA table_info(`" + ClassNamePHP + "`)");
            bool tNeedUpdate = false;
            bool tNeedCreate = false;
            bool tNeedMigrate = false;
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
                        tNeedMigrate = true;
                        //Debug.Log("tAuto " + tAuto + " ====> tActual " + tActual);
                    }
                }
            }
            if (tError == true)
            {
                //Debug.Log("Table " + ClassNamePHP + "error");
                tNeedCreate = true;
            }
            else
            {
                if (tActualsList.Count > 0)
                {
                    tNeedUpdate = true;
                    foreach (PropertyInfo tPropertyInfo in tActualsList)
                    {
                        //Debug.Log("tAuto " + "`" + tPropertyInfo.Name + "` " + PropertyInfoToSQLiteType(tPropertyInfo, true) + " <==== not present!!!");
                    }
                }
            }


            if (tNeedMigrate == true)
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
            else if (tNeedUpdate == true)
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
            else if (tNeedCreate == true)
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
                tQuery.Append(");");
                Debug.Log(tQuery.ToString());
            }
            else
            {
                //Debug.Log("Table `" + ClassNamePHP + "` is ok!");
            }
            return tQuery.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void CreateTable()
        {
            NWDDataManager.SharedInstance().CreateTable(ClassType, kAccountDependent);
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
        public void UpdateDataTable()
        {
            NWDDataManager.SharedInstance().MigrateTable(ClassType, kAccountDependent);
            //List<object> tObjectsListToDelete = new List<object>();
            foreach (NWDTypeClass tObject in Datas)
            {
                tObject.UpdateData();
            }
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
        static IntPtr NegativePointer = new IntPtr(-1);
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
        //private object ReadCol(Sqlite3Statement stmt, int index, SQLite3.ColType type, Type clrType)
        //{
        //    if (type == SQLite3.ColType.Null)
        //        return null;

        //    if (clrType == typeof(string))
        //        return SQLite3.ColumnString(stmt, index);

        //    if (clrType == typeof(int))
        //        return SQLite3.ColumnInt(stmt, index);

        //    if (clrType == typeof(bool))
        //        return SQLite3.ColumnInt(stmt, index) == 1;

        //    if (clrType == typeof(double))
        //        return SQLite3.ColumnDouble(stmt, index);

        //    if (clrType == typeof(float))
        //        return (float)SQLite3.ColumnDouble(stmt, index);

        //    if (clrType == typeof(TimeSpan))
        //        return new TimeSpan(SQLite3.ColumnInt64(stmt, index));

        //    if (clrType == typeof(DateTimeOffset))
        //    {
        //        return new DateTimeOffset(SQLite3.ColumnInt64(stmt, index), TimeSpan.Zero);
        //    }

        //    if (clrType.IsEnum)
        //    {
        //        return SQLite3.ColumnInt(stmt, index);
        //    }

        //    if (clrType == typeof(long))
        //        return SQLite3.ColumnInt64(stmt, index);

        //    if (clrType == typeof(uint))
        //        return (uint)SQLite3.ColumnInt64(stmt, index);

        //    if (clrType == typeof(decimal))
        //        return (decimal)SQLite3.ColumnDouble(stmt, index);

        //    if (clrType == typeof(byte))
        //        return (byte)SQLite3.ColumnInt(stmt, index);

        //    if (clrType == typeof(ushort))
        //        return (ushort)SQLite3.ColumnInt(stmt, index);

        //    if (clrType == typeof(short))
        //        return (short)SQLite3.ColumnInt(stmt, index);

        //    if (clrType == typeof(sbyte))
        //        return (sbyte)SQLite3.ColumnInt(stmt, index);

        //    if (clrType == typeof(byte[]))
        //        return SQLite3.ColumnByteArray(stmt, index);

        //    if (clrType == typeof(Guid))
        //    {
        //        string text = SQLite3.ColumnString(stmt, index);
        //        return new Guid(text);
        //    }
        //    //------------------------------------------
        //    //---------- ADD IDEMOBI START -------------
        //    if (clrType.IsSubclassOf(typeof(NWEDataType)))
        //    {
        //        NWEDataType tObject = Activator.CreateInstance(clrType) as NWEDataType;
        //        tObject.SetValue(SQLite3.ColumnString(stmt, index));
        //        return tObject;
        //    }
        //    if (clrType.IsSubclassOf(typeof(NWEDataTypeInt)))
        //    {
        //        NWEDataTypeInt tObject = Activator.CreateInstance(clrType) as NWEDataTypeInt;
        //        tObject.SetLong(SQLite3.ColumnInt64(stmt, index));
        //        return tObject;
        //    }
        //    if (clrType.IsSubclassOf(typeof(NWEDataTypeFloat)))
        //    {
        //        NWEDataTypeFloat tObject = Activator.CreateInstance(clrType) as NWEDataTypeFloat;
        //        tObject.SetDouble(SQLite3.ColumnDouble(stmt, index));
        //        return tObject;
        //    }
        //    if (clrType.IsSubclassOf(typeof(NWEDataTypeEnum)))
        //    {
        //        NWEDataTypeEnum tObject = Activator.CreateInstance(clrType) as NWEDataTypeEnum;
        //        tObject.SetLong(SQLite3.ColumnInt64(stmt, index));
        //        return tObject;
        //    }
        //    if (clrType.IsSubclassOf(typeof(NWEDataTypeMask)))
        //    {
        //        NWEDataTypeMask tObject = Activator.CreateInstance(clrType) as NWEDataTypeMask;
        //        tObject.SetLong(SQLite3.ColumnInt64(stmt, index));
        //        return tObject;
        //    }
        //    //---------- ADD IDEMOBI FINISH ------------
        //    //------------------------------------------
        //    throw new NotSupportedException("Don't know how to read " + clrType);
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void LoadFromDatabase()
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
            ResetDatas();
            SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
            if (kAccountDependent)
            {
                tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
            }
            if (tSQLiteConnection != null)
            {
                if (tSQLiteConnection.IsValid())
                {
                    //int c = 0;

                    //NWEBenchmark.Start("NewY_method");
                    //Sqlite3DatabaseHandle stmty = SQLite3.Prepare2(tSQLiteConnection.Handle, "SELECT * FROM `" + ClassNamePHP + "`;");

                    //while (SQLite3.Step(stmty) == SQLite3.Result.Row)
                    //{
                    //    var tD = Activator.CreateInstance(ClassType, new object[] { false });
                    //}
                    //SQLite3.Finalize(stmty);
                    //NWEBenchmark.Finish("NewY_method");

                    //ResetDatas();

                    //NWEBenchmark.Start("New_method");
                    //List<NWDTypeClass> tSelect = tSQLiteConnection.Query<K>("SELECT * FROM " + ClassNamePHP);
                    List<PropertyInfo> tProplist = new List<PropertyInfo>();
                    List<Type> tPropTypelist = new List<Type>();
                    List<string> tColumnList = new List<string>();
                    foreach (PropertyInfo tProp in ClassType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
                    {
                        //if (tProp.Name != "ID" && string.IsNullOrEmpty(tProp.Name) == false)
                        {
                            tProplist.Add(tProp);
                            tPropTypelist.Add(tProp.PropertyType);
                            tColumnList.Add(tProp.Name);
                        }
                    }
                    Sqlite3DatabaseHandle stmtc = SQLite3.Prepare2(tSQLiteConnection.Handle, "SELECT `" + string.Join("`, `", tColumnList) + "` FROM `" + ClassNamePHP + "`;");

                    while (SQLite3.Step(stmtc) == SQLite3.Result.Row)
                    {
                        var tD = Activator.CreateInstance(ClassType, new object[] { false });
                        for (int i = 0; i < tProplist.Count; i++)
                        {
                            PropertyInfo tProp = tProplist[i];
                            Type tTypeOfThis = tPropTypelist[i];
                            ReadCol(tTypeOfThis, tProp, stmtc, i, tD);
                            //var t = ReadCol(stmtc, i, SQLite3.ColumnType(stmtc, i), tTypeOfThis);
                        }
                        ((NWDTypeClass)tD).LoadedFromDatabase();
                        tCount++;
                    }
                    SQLite3.Finalize(stmtc);
                    //NWEBenchmark.Finish("New_method", true , " for " + ClassNamePHP);

                    //ResetDatas();

                    //NWEBenchmark.Start("Old_method");
                    ////List<NWDTypeClass> tSelect = tSQLiteConnection.Query<K>("SELECT * FROM " + ClassNamePHP);
                    //List<object> tSelect = tSQLiteConnection.Query(new TableMapping(ClassType), "SELECT * FROM " + ClassNamePHP);
                    //// Prepare the datas
                    //if (tSelect != null)
                    //{
                    //    foreach (object tItem in tSelect)
                    //    {
                    //        tCount++;
                    //        ((NWDTypeClass)tItem).LoadedFromDatabase();
                    //    }
                    //}
                    //NWEBenchmark.Finish("Old_method");
                }
            }
            //Debug.Log("NWDBasis LoadFromDatabase() tEnumerable tCount :" + tCount.ToString());
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
            //Debug.Log("UnloadDataByReference(" + sReference + ")");
            if (DatasByReference.ContainsKey(sReference))
            {
                NWDTypeClass tData = DatasByReference[sReference];
                tData.Desindex(); // call override method
                RemoveData(tData);
                //tData.Delete();
            }
#if UNITY_EDITOR
            FilterTableEditor();
            RepaintTableEditor();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetTable()
        {
            NWDDataManager.SharedInstance().ResetTable(ClassType, kAccountDependent);
            // reload empty datas
            LoadFromDatabase();
#if UNITY_EDITOR
            // refresh the tables windows
            RepaintTableEditor();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public void EmptyTable()
        {
            NWDDataManager.SharedInstance().EmptyTable(ClassType, kAccountDependent);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DropTable()
        {
            NWDDataManager.SharedInstance().DropTable(ClassType, kAccountDependent);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReInitializeTable()
        {
            NWDDataManager.SharedInstance().ReInitializeTable(ClassType, kAccountDependent);
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================