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
using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
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
        public void LoadFromDatabase()
        {
#if UNITY_EDITOR
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
                    //List<NWDTypeClass> tSelect = tSQLiteConnection.Query<K>("SELECT * FROM " + ClassNamePHP);
                    List<object> tSelect = tSQLiteConnection.Query(new TableMapping(ClassType), "SELECT * FROM " + ClassNamePHP);
                    int tCount = 0;
                    // Prepare the datas
                    if (tSelect != null)
                    {
                        foreach (object tItem in tSelect)
                        {
                            tCount++;
                            ((NWDTypeClass)tItem).LoadedFromDatabase();
                        }
                    }
                }
            }
            //Debug.Log("NWDBasis LoadFromDatabase() tEnumerable tCount :" + tCount.ToString());
            DatasLoaded = true;
            ClassDatasAreLoaded();
#if UNITY_EDITOR
            FilterTableEditor();
            RepaintTableEditor();
#endif

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