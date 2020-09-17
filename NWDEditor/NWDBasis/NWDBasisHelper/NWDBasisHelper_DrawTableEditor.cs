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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public EditorWindow CurrentWindow;
        //-------------------------------------------------------------------------------------------------------------
        //TODO : RENAME!
        public void DrawInEditor(EditorWindow sEditorWindow, Rect sRect, Rect sRectReal, bool sAutoSelect = false)
        {
            //NWDBenchmark.Start();
            DrawTableEditor(sEditorWindow, sRect, sRectReal);
            if (sAutoSelect == true)
            {
                SelectedFirstObjectInTable(sEditorWindow);
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public string GetReferenceOfDataInEdition()
        {
            //NWDBenchmark.Start();
            string rReturn = null;
            NWDTypeClass tObject = GetObjectInEdition() as NWDTypeClass;
            if (tObject != null)
            {
                rReturn = string.Copy(tObject.Reference);
            }
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void RestaureDataInEditionByReference(string sReference)
        {
            //NWDBenchmark.Start();
            NWDTypeClass tObject = null;
            if (sReference != null)
            {
                if (DatasByReference.ContainsKey(sReference))
                {
                    tObject = DatasByReference[sReference];
                }
                if (tObject != null)
                {
                    if (EditorTableDatas.Contains(tObject))
                    {
                        SetObjectInEdition(tObject);
                    }
                }
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void SelectAllObjectInTableList()
        {
            //NWDBenchmark.Start();
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                EditorTableDatasSelected[tObject] = true;
            }
            //IntegritySelection();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void DeselectAllObjectInTableList()
        {
            //NWDBenchmark.Start();
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                EditorTableDatasSelected[tObject] = false;
            }
            //IntegritySelection();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void InverseSelectionOfAllObjectInTableList()
        {
            //NWDBenchmark.Start();
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                EditorTableDatasSelected[tObject] = !EditorTableDatasSelected[tObject];
            }
            //NWDBenchmark.Finish();
            //IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void SelectAllObjectEnableInTableList()
        {
            //NWDBenchmark.Start();
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                EditorTableDatasSelected[tObject] = tObject.IsEnable();
            }
            //IntegritySelection();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void SelectAllObjectDisableInTableList()
        {
            //NWDBenchmark.Start();
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                EditorTableDatasSelected[tObject] = !tObject.IsEnable();
            }
            //IntegritySelection();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public float DrawPagesTab(Rect sRect)
        {
            //NWDBenchmark.Start();
            float rReturn = sRect.height;
            float tWidth = sRect.width;
            float tTabWidth = 35.0f;
            int tToogleToListPageLimit = (int)Math.Floor(tWidth / tTabWidth);
            Rect tRect = new Rect(sRect.x + NWDGUI.kFieldMarge, sRect.y, sRect.width - NWDGUI.kFieldMarge * 2, EditorStyles.toolbar.fixedHeight);
            m_ItemPerPage = int.Parse(m_ItemPerPageOptions[m_ItemPerPageSelection]);
            float tNumberOfPage = EditorTableDatas.Count / m_ItemPerPage;
            int tPagesExpected = (int)Math.Floor(tNumberOfPage);
            if (tPagesExpected != 0)
            {
                if (EditorTableDatas.Count % (tPagesExpected * m_ItemPerPage) != 0)
                {
                    tPagesExpected++;
                }
            }
            if (m_PageSelected > tPagesExpected - 1)
            {
                m_PageSelected = tPagesExpected - 1;
            }
            m_MaxPage = tPagesExpected + 1;
            string[] tListOfPagesName = new string[tPagesExpected];
            for (int p = 0; p < tPagesExpected; p++)
            {
                int tP = p + 1;
                tListOfPagesName[p] = string.Empty + tP.ToString();
            }
            int t_PageSelected = m_PageSelected;
            if (tPagesExpected == 0 || tPagesExpected == 1)
            {
                // no choose
                t_PageSelected = 0;
                rReturn = 0;
            }
            else if (tPagesExpected < tToogleToListPageLimit)
            {
                rReturn = NWDGUI.KTableToolbar.fixedHeight + NWDGUI.kFieldMarge;
                tRect.height = EditorStyles.toolbar.fixedHeight;
                t_PageSelected = GUI.Toolbar(tRect, m_PageSelected, tListOfPagesName);
            }
            else
            {
                rReturn = NWDGUI.KTableToolbar.fixedHeight + NWDGUI.kFieldMarge;
                tRect.height = EditorStyles.popup.fixedHeight;
                tRect.y += 2;
                t_PageSelected = EditorGUI.Popup(tRect, m_PageSelected, tListOfPagesName, EditorStyles.popup);
            }
            if (m_PageSelected != t_PageSelected)
            {
                NWDDataManager.SharedInstance().DataQueueExecute();
            }
            m_PageSelected = t_PageSelected;
            //NWDBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void ChangeScroolPositionToSelection()
        {
            //NWDBenchmark.Start();
            NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType);
            foreach (EditorWindow tWindow in NWDDataManager.SharedInstance().EditorWindowsInManager(ClassType))
            {
                Rect tWindowRect = new Rect(tWindow.position.x, tWindow.position.y, tWindow.position.width, tWindow.position.height);
                if (tWindowRect.width < NWDGUI.KTableMinWidth)
                {
                    tWindowRect.width = NWDGUI.KTableMinWidth;
                }
                Rect tRect = new Rect(0, 0, tWindowRect.width, 0);
                // offset the tab bar 
                tRect.y += 50;
                float tScrollHeight = tWindowRect.height - tRect.y - NWDGUI.kTableHeaderHeight - NWDGUI.kFieldMarge;
                Rect tScrollHeader = new Rect(0, tRect.y, tWindowRect.width, NWDGUI.kTableHeaderHeight);
                Rect tScrollRect = new Rect(0, tRect.y + NWDGUI.kTableHeaderHeight, tWindowRect.width, tScrollHeight);
                ChangeScroolPositionToSelection(tScrollRect);
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void ChangeScroolPositionToSelection(Rect sScrollRect)
        {
            //NWDBenchmark.Start();
            int tIndexSelected = EditorTableDatas.IndexOf(GetObjectInEdition());
            float tNumberPage = tIndexSelected / m_ItemPerPage;
            int tPageExpected = (int)Math.Floor(tNumberPage);
            m_PageSelected = tPageExpected;
            Vector2 tMouseScrollTop = new Vector2(0, (tIndexSelected - tPageExpected * m_ItemPerPage) * NWDGUI.kTableRowHeight * RowZoom);
            Vector2 tMouseScrollBottom = new Vector2(tMouseScrollTop.x, tMouseScrollTop.y + NWDGUI.kTableRowHeight * RowZoom);
            Rect tAreaVisible = new Rect(m_ScrollPositionList.x, m_ScrollPositionList.y, sScrollRect.width, sScrollRect.height);
            if (tAreaVisible.Contains(tMouseScrollTop) == false && tAreaVisible.Contains(tMouseScrollBottom) == false)
            {
                m_ScrollPositionList.y = tMouseScrollTop.y;
                tAreaVisible = new Rect(m_ScrollPositionList.x, m_ScrollPositionList.y, sScrollRect.width, sScrollRect.height);
            }
            int tCountSecurity = m_ItemPerPage;
            while ((tAreaVisible.Contains(tMouseScrollTop) == false || tAreaVisible.Contains(tMouseScrollBottom) == false) && tCountSecurity > 0)
            {
                tCountSecurity--;
                if (tAreaVisible.Contains(tMouseScrollBottom) == false)
                {
                    m_ScrollPositionList.y += NWDGUI.kTableRowHeight * RowZoom;
                }
                else if (tAreaVisible.Contains(tMouseScrollTop) == false)
                {
                    m_ScrollPositionList.y -= NWDGUI.kTableRowHeight * RowZoom;
                }
                tAreaVisible = new Rect(m_ScrollPositionList.x, m_ScrollPositionList.y, sScrollRect.width, sScrollRect.height);
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void DrawHeaderInEditor(Rect sRect, Rect sScrollRect, float sZoom)
        {
            //NWDBenchmark.Start();
            //GUI.BeginScrollView(
            //        sRect,
            //        Vector2.zero,
            //        sRect,
            //        false,
            //        false
            //        );
            EditorGUI.DrawRect(sRect, NWDGUI.kTableHeaderColor);
            sRect.x += NWDGUI.kFieldMarge;
            sRect.width = sRect.width - NWDGUI.kScrollbar;

            if (sRect.width < NWDGUI.KTableMinWidth - NWDGUI.kScrollbar)
            {
                sRect.width = NWDGUI.KTableMinWidth - NWDGUI.kScrollbar;
            }
            Rect tRect = new Rect(sRect.x, sRect.y + NWDGUI.kFieldMarge, NWDGUI.kTableSelectWidth, sRect.height - NWDGUI.kFieldMarge * 2);
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_SELECT, NWDGUI.KTableHeaderSelect))
            {
                // Update select in real time
                foreach (NWDTypeClass tData in Datas)
                {
                    tData.AnalyzeSelected = EditorTableDatasSelected[tData];
                }
                // toogle sort
                if (SortType == NWDBasisEditorDatasSortType.BySelectAscendant)
                {
                    SortType = NWDBasisEditorDatasSortType.BySelectDescendant;
                }
                else if (SortType == NWDBasisEditorDatasSortType.BySelectDescendant)
                {
                    SortType = NWDBasisEditorDatasSortType.BySelectAscendant;
                }
                else
                {
                    SortType = NWDBasisEditorDatasSortType.BySelectDescendant;
                }
                SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            tRect.width = NWDGUI.kTableIDWidth;
            tRect.width = NWDGUI.kTablePrefabWidth * sZoom;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_PREFAB, NWDGUI.KTableHeaderPrefab))
            {
                if (SortType == NWDBasisEditorDatasSortType.ByPrefabAscendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByPrefabDescendant;
                }
                else if (SortType == NWDBasisEditorDatasSortType.ByPrefabDescendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByPrefabAscendant;
                }
                else
                {
                    SortType = NWDBasisEditorDatasSortType.ByPrefabDescendant;
                }
                SortEditorTableDatas();
            }
            tRect.x += tRect.width;
            tRect.width = sRect.width
                - NWDGUI.kFieldMarge
                - NWDGUI.kTableSelectWidth
                - NWDGUI.kTablePrefabWidth * sZoom
                - NWDGUI.KTableSearchWidth
                - NWDGUI.KTableReferenceWidth
                - NWDGUI.KTableRowWebModelWidth * 2
                - NWDGUI.kTableIconWidth * 5;
            if (TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
            {
                tRect.width -= NWDGUI.kTableIconWidth;
            }
            if (tRect.width < NWDGUI.KTableSearchWidth)
            {
                tRect.width = NWDGUI.KTableSearchWidth;
            }
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_DESCRIPTION, NWDGUI.KTableHeaderInformations))
            {
                if (SortType == NWDBasisEditorDatasSortType.ByInternalKeyAscendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByInternalKeyDescendant;
                }
                else if (SortType == NWDBasisEditorDatasSortType.ByInternalKeyDescendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByInternalKeyAscendant;
                }
                else
                {
                    SortType = NWDBasisEditorDatasSortType.ByInternalKeyDescendant;
                }
                SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            tRect.width = NWDGUI.KTableRowWebModelWidth;
            if (GUI.Button(tRect, "Age", NWDGUI.KTableHeaderIcon))
            {
                if (SortType == NWDBasisEditorDatasSortType.ByAgeAscendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByAgeDescendant;
                }
                else if (SortType == NWDBasisEditorDatasSortType.ByAgeDescendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByAgeAscendant;
                }
                else
                {
                    SortType = NWDBasisEditorDatasSortType.ByAgeDescendant;
                }
                SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;

            tRect.width = NWDGUI.KTableRowWebModelWidth;
            if (GUI.Button(tRect, "webservice", NWDGUI.KTableHeaderIcon))
            {
                if (SortType == NWDBasisEditorDatasSortType.ByModelAscendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByModelDescendant;
                }
                else if (SortType == NWDBasisEditorDatasSortType.ByModelDescendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByModelAscendant;
                }
                else
                {
                    SortType = NWDBasisEditorDatasSortType.ByModelDescendant;
                }
                SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;

            if (TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
            {
                tRect.width = NWDGUI.kTableIconWidth;
                if (GUI.Button(tRect, "Check", NWDGUI.KTableHeaderIcon))
                {
                    if (SortType == NWDBasisEditorDatasSortType.ByChecklistAscendant)
                    {
                        SortType = NWDBasisEditorDatasSortType.ByChecklistDescendant;
                    }
                    else if (SortType == NWDBasisEditorDatasSortType.ByChecklistDescendant)
                    {
                        SortType = NWDBasisEditorDatasSortType.ByChecklistAscendant;
                    }
                    else
                    {
                        SortType = NWDBasisEditorDatasSortType.ByChecklistDescendant;
                    }
                    SortEditorTableDatas();
                    ChangeScroolPositionToSelection(sScrollRect);
                }
                tRect.x += tRect.width;
            }

            tRect.width = NWDGUI.kTableIconWidth;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_DISK, NWDGUI.KTableHeaderIcon))
            {
                //Debug.Log("sort by disk toggle ???");
            }
            tRect.x += tRect.width;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_SYNCHRO, NWDGUI.KTableHeaderIcon))
            {
                if (SortType == NWDBasisEditorDatasSortType.BySyncAscendant)
                {
                    SortType = NWDBasisEditorDatasSortType.BySyncDescendant;
                }
                else if (SortType == NWDBasisEditorDatasSortType.BySyncDescendant)
                {
                    SortType = NWDBasisEditorDatasSortType.BySyncAscendant;
                }
                else
                {
                    SortType = NWDBasisEditorDatasSortType.BySyncDescendant;
                }
                SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_DEVSYNCHRO, NWDGUI.KTableHeaderIcon))
            {
                if (SortType == NWDBasisEditorDatasSortType.ByDevSyncAscendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByDevSyncDescendant;
                }
                else if (SortType == NWDBasisEditorDatasSortType.ByDevSyncDescendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByDevSyncAscendant;
                }
                else
                {
                    SortType = NWDBasisEditorDatasSortType.ByDevSyncDescendant;
                }
                SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_PREPRODSYNCHRO, NWDGUI.KTableHeaderIcon))
            {
                if (SortType == NWDBasisEditorDatasSortType.ByPreprodSyncAscendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByPreprodSyncDescendant;
                }
                else if (SortType == NWDBasisEditorDatasSortType.ByPreprodSyncDescendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByPreprodSyncAscendant;
                }
                else
                {
                    SortType = NWDBasisEditorDatasSortType.ByPreprodSyncDescendant;
                }
                SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_PRODSYNCHRO, NWDGUI.KTableHeaderIcon))
            {
                if (SortType == NWDBasisEditorDatasSortType.ByProdSyncAscendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByProdSyncDescendant;
                }
                else if (SortType == NWDBasisEditorDatasSortType.ByProdSyncDescendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByProdSyncAscendant;
                }
                else
                {
                    SortType = NWDBasisEditorDatasSortType.ByProdSyncDescendant;
                }
                SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            tRect.width = NWDGUI.KTableSearchWidth;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_STATUT, NWDGUI.KTableHeaderStatut))
            {
                if (SortType == NWDBasisEditorDatasSortType.ByStatutAscendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByStatutDescendant;
                }
                else if (SortType == NWDBasisEditorDatasSortType.ByStatutDescendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByStatutAscendant;
                }
                else
                {
                    SortType = NWDBasisEditorDatasSortType.ByStatutDescendant;
                }
                SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            tRect.width = NWDGUI.KTableReferenceWidth;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_REFERENCE + " ", NWDGUI.KTableHeaderReference))
            {
                if (SortType == NWDBasisEditorDatasSortType.ByReferenceAscendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByReferenceDescendant;
                }
                else if (SortType == NWDBasisEditorDatasSortType.ByReferenceDescendant)
                {
                    SortType = NWDBasisEditorDatasSortType.ByReferenceAscendant;
                }
                else
                {
                    SortType = NWDBasisEditorDatasSortType.ByReferenceDescendant;
                }
                SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            //GUI.EndScrollView();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void DrawHeaderBottomInEditor(Rect sRect, Rect sScrollRect)
        {
            EditorGUI.DrawRect(sRect, NWDGUI.kTableHeaderColor);
            sRect.width = sRect.width - NWDGUI.kFieldMarge;
            Rect tRect = new Rect(sRect.x + NWDGUI.kFieldMarge,
                 sRect.y + NWDGUI.kFieldMarge,
                 NWDGUI.KTableSearchWidth,
                 NWDGUI.KTableSearchToggle.fixedHeight);
            // TODO MOVE THIS CAC
            int tSelectionCount = 0;
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
            {
                if (tKeyValue.Value == true)
                {
                    tSelectionCount++;
                }
            }
            int t_ItemPerPageSelection = EditorGUI.Popup(tRect, m_ItemPerPageSelection, m_ItemPerPageOptions, NWDGUI.KTableSearchEnum);
            if (t_ItemPerPageSelection != m_ItemPerPageSelection)
            {
                m_ItemPerPageSelection = t_ItemPerPageSelection;
                m_ItemPerPage = int.Parse(m_ItemPerPageOptions[m_ItemPerPageSelection]);
                NWDProjectPrefs.SetInt(ActionsPrefkey(() => m_ItemPerPageSelection), m_ItemPerPageSelection);
            }
            tRect.x += NWDGUI.KTableSearchWidth + NWDGUI.kFieldMarge;
            tRect.width = sRect.width - NWDGUI.KTableSearchWidth *2 - NWDGUI.kFieldMarge * 3;
            EditorGUI.BeginChangeCheck();
            float tRowZoom = GUI.HorizontalSlider(tRect, RowZoom, 1.0F, 2.0F);
            if (EditorGUI.EndChangeCheck() == true)
            {
                RowZoom = tRowZoom;
                SaveEditorPrefererences();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.width = NWDGUI.KTableSearchWidth;
            tRect.height = NWDGUI.KTableSearchButton.fixedHeight;
            tRect.x = sRect.width - NWDGUI.KTableSearchWidth;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_ADD_ROW, NWDGUI.KTableSearchButton))
            {
                NWDTypeClass tNewObject = NewData();
                if (m_SearchTag != NWDBasisTag.NoTag)
                {
                    tNewObject.Tag = m_SearchTag;
                    tNewObject.UpdateData();
                }
                m_PageSelected = m_MaxPage * 3;
                SetObjectInEdition(tNewObject);
                NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public Rect DrawTableEditorTop(Rect sRect, Rect sRectReal)
        {
            int tMaxForMenuOrTextField = 25;
            //NWDBenchmark.Start();
            float tOldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = NWDGUI.KTableSearchLabelWidth;
            Rect rRect = new Rect(sRect.x, sRect.y, sRect.width, 0);
            //init rect  
            Rect tRect = new Rect(sRect.x + NWDGUI.kFieldMarge, sRect.y, sRectReal.width, NWDGUI.KTableSearchToggle.fixedHeight);
            // draw Show
            tRect.width = NWDGUI.KTableSearchWidth;
            GUI.Label(tRect, "Show", NWDGUI.KTableSearchTitle);
            // draw column
            tRect.y += tRect.height + NWDGUI.kFieldMarge;
            // draw toogle enable
            bool t_ShowEnableLine = EditorGUI.ToggleLeft(tRect, NWDConstants.K_APP_TABLE_SHOW_ENABLE_DATAS, m_ShowEnable);
            if (m_ShowEnable != t_ShowEnableLine)
            {
                m_ShowEnable = t_ShowEnableLine;
                FilterTableEditor();
            }
            tRect.y += tRect.height + NWDGUI.kFieldMarge;
            // draw toogle disable
            bool t_ShowDisableLine = EditorGUI.ToggleLeft(tRect, NWDConstants.K_APP_TABLE_SHOW_DISABLE_DATAS, m_ShowDisable);
            if (m_ShowDisable != t_ShowDisableLine)
            {
                m_ShowDisable = t_ShowDisableLine;
                FilterTableEditor();
            }
            tRect.y += tRect.height + NWDGUI.kFieldMarge;
            // draw toogle trashed
            EditorGUI.BeginDisabledGroup(!m_ShowDisable);
            bool t_ShowTrashedLine = EditorGUI.ToggleLeft(tRect, NWDConstants.K_APP_TABLE_SHOW_TRASHED_DATAS, m_ShowTrashed);
            if (m_ShowTrashed != t_ShowTrashedLine)
            {
                m_ShowTrashed = t_ShowTrashedLine;
                FilterTableEditor();
            }
            tRect.y += tRect.height + NWDGUI.kFieldMarge;
            // draw toogle corrupted
            bool t_ShowIntegrityErrorLine = EditorGUI.ToggleLeft(tRect, NWDConstants.K_APP_TABLE_SHOW_INTEGRITY_ERROR_DATAS, m_ShowIntegrityError);
            if (m_ShowIntegrityError != t_ShowIntegrityErrorLine)
            {
                m_ShowIntegrityError = t_ShowIntegrityErrorLine;
                FilterTableEditor();
            }
            EditorGUI.EndDisabledGroup();
            // draw Filter
            tRect.y = sRect.y;
            tRect.x += NWDGUI.KTableSearchWidth + NWDGUI.kFieldMarge;
            tRect.width = sRectReal.width - NWDGUI.KTableSearchWidth * 2 - NWDGUI.kFieldMarge * 4;
            if (tRect.width > NWDGUI.KTableSearchFieldWidth * 2 + NWDGUI.kFieldMarge * 2)
            {
                GUI.Label(tRect, "Filters", NWDGUI.KTableSearchTitle);
                // divise area 
                tRect.width = Mathf.Floor((tRect.width - NWDGUI.KTableSearchWidth - NWDGUI.kFieldMarge * 2)/2.0f);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;

                // draw Reference
                m_SearchReference = EditorGUI.TextField(tRect, NWDConstants.K_APP_TABLE_SEARCH_REFERENCE,
                     m_SearchReference, NWDGUI.KTableSearchTextfield);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw internal key
                m_SearchInternalName = EditorGUI.TextField(tRect, NWDConstants.K_APP_TABLE_SEARCH_NAME,
                    m_SearchInternalName, NWDGUI.KTableSearchTextfield);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw Internal description
                m_SearchInternalDescription = EditorGUI.TextField(tRect, NWDConstants.K_APP_TABLE_SEARCH_DESCRIPTION,
                    m_SearchInternalDescription, NWDGUI.KTableSearchTextfield);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;

                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = sRect.y + tRect.height + NWDGUI.kFieldMarge;

                EditorGUI.BeginDisabledGroup(TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent);
                if (NWDBasisHelper.FindTypeInfos(typeof(NWDAccount)).Datas.Count < tMaxForMenuOrTextField)
                {
                    List<string> tReferenceList = new List<string>();
                    List<string> tInternalNameList = new List<string>();
                    tReferenceList.Add(string.Empty);
                    tInternalNameList.Add(NWDConstants.kFieldNone);
                    tReferenceList.Add("---");
                    tInternalNameList.Add(string.Empty);
                    tReferenceList.Add("-=-");
                    tInternalNameList.Add(NWDConstants.kFieldEmpty);
                    tReferenceList.Add("-+-");
                    tInternalNameList.Add(NWDConstants.kFieldNotEmpty);
                    foreach (KeyValuePair<string, string> tKeyValue in NWDBasisHelper.FindTypeInfos(typeof(NWDAccount)).EditorDatasMenu.OrderBy(i => i.Value))
                    {
                        tReferenceList.Add(tKeyValue.Key);
                        tInternalNameList.Add(tKeyValue.Value);
                    }
                    List<GUIContent> tContentFuturList = new List<GUIContent>();
                    foreach (string tS in tInternalNameList.ToArray())
                    {
                        tContentFuturList.Add(new GUIContent(tS));
                    }
                    int tIndexAccount = tReferenceList.IndexOf(m_SearchAccount);
                    int tNewIndexAccount = EditorGUI.Popup(tRect, new GUIContent(NWDConstants.K_APP_TABLE_SEARCH_ACCOUNT), tIndexAccount, tContentFuturList.ToArray(),
                                                                               NWDGUI.KTableSearchEnum);
                    if (tNewIndexAccount >= 0 && tNewIndexAccount < tReferenceList.Count())
                    {
                        m_SearchAccount = tReferenceList[tNewIndexAccount];
                    }
                    else
                    {
                        m_SearchAccount = string.Empty;
                    }

                }
                else
                {
                    m_SearchAccount = EditorGUI.TextField(tRect, new GUIContent(NWDConstants.K_APP_TABLE_SEARCH_ACCOUNT), m_SearchAccount, NWDGUI.KTableSearchTextfield);
                }
                EditorGUI.EndDisabledGroup();
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                EditorGUI.BeginDisabledGroup(TemplateHelper.GetGamesaveDependent() != NWDTemplateGameSaveDependent.NoGameSaveDependent);
                if (NWDBasisHelper.FindTypeInfos(typeof(NWDGameSave)).Datas.Count < tMaxForMenuOrTextField)
                {
                    List<string> tReferenceSaveList = new List<string>();
                    List<string> tInternalNameSaveList = new List<string>();
                    tReferenceSaveList.Add(string.Empty);
                    tInternalNameSaveList.Add(NWDConstants.kFieldNone);
                    tReferenceSaveList.Add("---");
                    tInternalNameSaveList.Add(string.Empty);
                    tReferenceSaveList.Add("-=-");
                    tInternalNameSaveList.Add(NWDConstants.kFieldEmpty);
                    tReferenceSaveList.Add("-+-");
                    tInternalNameSaveList.Add(NWDConstants.kFieldNotEmpty);
                    foreach (KeyValuePair<string, string> tKeyValue in NWDBasisHelper.FindTypeInfos(typeof(NWDGameSave)).EditorDatasMenu.OrderBy(i => i.Value))
                    {
                        tReferenceSaveList.Add(tKeyValue.Key);
                        tInternalNameSaveList.Add(tKeyValue.Value);
                    }
                    List<GUIContent> tContentFuturSaveList = new List<GUIContent>();
                    foreach (string tS in tInternalNameSaveList.ToArray())
                    {
                        tContentFuturSaveList.Add(new GUIContent(tS));
                    }
                    int tIndexSave = tReferenceSaveList.IndexOf(m_SearchGameSave);
                    int tNewIndexSave = EditorGUI.Popup(tRect, new GUIContent(NWDConstants.K_APP_TABLE_SEARCH_GAMESAVE), tIndexSave, tContentFuturSaveList.ToArray(),
                                                                               NWDGUI.KTableSearchEnum);
                    if (tNewIndexSave >= 0 && tNewIndexSave < tReferenceSaveList.Count())
                    {
                        m_SearchGameSave = tReferenceSaveList[tNewIndexSave];
                    }
                    else
                    {
                        m_SearchGameSave = string.Empty;
                    }
                }
                else
                {
                    m_SearchGameSave = EditorGUI.TextField(tRect, new GUIContent(NWDConstants.K_APP_TABLE_SEARCH_GAMESAVE), m_SearchGameSave, NWDGUI.KTableSearchTextfield);
                }
                EditorGUI.EndDisabledGroup();
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // Draw Internal Tag popup
                List<int> tTagIntList = new List<int>();
                List<string> tTagStringList = new List<string>();
                foreach (KeyValuePair<int, string> tTag in NWDAppConfiguration.SharedInstance().TagList)
                {
                    tTagIntList.Add(tTag.Key);
                    tTagStringList.Add(tTag.Value);
                }
                m_SearchTag = (NWDBasisTag)EditorGUI.IntPopup(tRect, NWDConstants.K_APP_TABLE_SEARCH_TAG,
                                                                    (int)m_SearchTag, tTagStringList.ToArray(),
                                                                    tTagIntList.ToArray(),
                                                                    NWDGUI.KTableSearchEnum);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // Draw Internal checklist popup
                EditorGUI.BeginDisabledGroup(TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent);
                m_SearchCheckList = (NetWorkedData.NWDBasisCheckList)m_SearchCheckList.ControlField(tRect, NWDConstants.K_APP_TABLE_SEARCH_CHECKLIST, TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent);
                EditorGUI.EndDisabledGroup();
                tRect.y += tRect.height + NWDGUI.kFieldMarge;


                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = sRect.y + tRect.height + NWDGUI.kFieldMarge;
                // restaure filter button to normal size
                tRect.width = NWDGUI.KTableSearchWidth;
                // draw button filter
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_SEARCH_FILTER, NWDGUI.KTableSearchButton))
                {
                    string tReference = GetReferenceOfDataInEdition();
                    GUI.FocusControl(null);
                    SetObjectInEdition(null);
                    FilterTableEditor();
                    RestaureDataInEditionByReference(tReference);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw Remove filter
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_SEARCH_REMOVE_FILTER, NWDGUI.KTableSearchButton))
                {

                    string tReference = GetReferenceOfDataInEdition();
                    GUI.FocusControl(null);
                    SetObjectInEdition(null);
                    //m_SearchReference = "";
                    m_SearchReference = string.Empty;
                    m_SearchInternalName = string.Empty;
                    m_SearchInternalDescription = string.Empty;
                    m_SearchTag = NWDBasisTag.NoTag;
                    m_SearchAccount = string.Empty;
                    m_SearchGameSave = string.Empty;
                    m_SearchCheckList.Value = 0;
                    FilterTableEditor();
                    RestaureDataInEditionByReference(tReference);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                //GUI.Label(tRect, "Filters", NWDGUI.KTableSearchTitle);
                //tRect.y += tRect.height + NWDGUI.kFieldMarge;

            }
            //else if (tRect.width > NWDGUI.KTableSearchFieldWidth * 3 + NWDGUI.kFieldMarge * 6)
            //{
            //    GUI.Label(tRect, "Filters ...", NWDGUI.KTableSearchTitle);

            //}
            else if (tRect.width > NWDGUI.KTableSearchWidth)//  small
            {
                GUI.Label(tRect, "...", NWDGUI.KTableSearchTitle);
            }
            else // too small
            {
            }
            // draw Results
            tRect.y = sRect.y;
            tRect.x = sRectReal.width - NWDGUI.KTableSearchWidth - NWDGUI.kFieldMarge;
            tRect.width = NWDGUI.KTableSearchWidth;
            GUI.Label(tRect, "Result", NWDGUI.KTableSearchTitle);
            // draw column
            tRect.y += tRect.height + NWDGUI.kFieldMarge;
            // draw objects in database
            int tRealReference = Datas.Count;
            if (tRealReference == 0)
            {
                GUI.Label(tRect, NWDConstants.K_APP_TABLE_NO_OBJECT, NWDGUI.KTableSearchLabel);
            }
            else if (tRealReference == 1)
            {
                GUI.Label(tRect, NWDConstants.K_APP_TABLE_ONE_OBJECT, NWDGUI.KTableSearchLabel);
            }
            else
            {
                GUI.Label(tRect, tRealReference + NWDConstants.K_APP_TABLE_X_OBJECTS, NWDGUI.KTableSearchLabel);
            }
            tRect.y += tRect.height + NWDGUI.kFieldMarge;
            // draw objects in results
            int tResultReference = EditorTableDatas.Count;
            if (tResultReference == 0)
            {
                GUI.Label(tRect, NWDConstants.K_APP_TABLE_NO_OBJECT_FILTERED, NWDGUI.KTableSearchLabel);
            }
            else if (tResultReference == 1)
            {
                GUI.Label(tRect, NWDConstants.K_APP_TABLE_ONE_OBJECT_FILTERED, NWDGUI.KTableSearchLabel);
            }
            else
            {
                GUI.Label(tRect, tResultReference + NWDConstants.K_APP_TABLE_X_OBJECTS_FILTERED, NWDGUI.KTableSearchLabel);
            }
            tRect.y += tRect.height + NWDGUI.kFieldMarge;
            int tSelectionCount = 0;
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
            {
                if (tKeyValue.Value == true)
                {
                    tSelectionCount++;
                }
            }
            if (tSelectionCount == 0)
            {
                GUI.Label(tRect, NWDConstants.K_APP_TABLE_NO_SELECTED_OBJECT, NWDGUI.KTableSearchLabel);
            }
            else if (tSelectionCount == 1)
            {
                GUI.Label(tRect, NWDConstants.K_APP_TABLE_ONE_SELECTED_OBJECT, NWDGUI.KTableSearchLabel);
            }
            else
            {
                GUI.Label(tRect, tSelectionCount + NWDConstants.K_APP_TABLE_XX_SELECTED_OBJECT, NWDGUI.KTableSearchLabel);
            }

            EditorGUIUtility.labelWidth = tOldLabelWidth;

            //NWDBenchmark.Finish();
            rRect.height = NWDGUI.KTableSearchToggle.fixedHeight * 5 + NWDGUI.kFieldMarge * 6;
            return rRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : RENAME!
        public void DrawTableEditor(EditorWindow sEditorWindow, Rect sRect, Rect sRectReal)
        {
            //NWDBenchmark.Start();
            float tOldLabelWidth = EditorGUIUtility.labelWidth;
            bool rLoaded = AllDatabaseIsLoaded();

            EditorGUI.BeginDisabledGroup(!rLoaded);

            Rect tWindowRectOriginal = new Rect(sRect.x, sRect.y, sRect.width, sRect.height);

            Rect tWindowRect = new Rect(sRect.x, sRect.y, sRect.width, sRect.height);

            if (tWindowRect.width < NWDGUI.KTableMinWidth)
            {
                tWindowRect.width = NWDGUI.KTableMinWidth;
            }
            Rect tRect = new Rect(0, 0, tWindowRect.width, 0);
            // offset the tab bar 
            tRect.y += NWDGUI.KTAB_BAR_HEIGHT + NWDGUI.kFieldMarge;

            float tWidthTiers = Mathf.Floor(sRect.width / 4.0F);
           

            float tW = NWDProjectPrefs.GetInt(NWDConstants.K_EDITOR_PANEL_WIDTH, 320);
            Rect tWindowRectInpsector = new Rect(tRect.width - tW, tRect.position.y, tW, sRect.height - tRect.position.y);
            
            // if necessary recalcul row informations
            RowAnalyze();
            bool tMargeNeed = false;
            // Alert Salts are false infos
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                tRect.y += tRect.height;
                if (tMargeNeed == false)
                {
                    tRect.y += NWDGUI.kFieldMarge;
                }
                tMargeNeed = true;
                tRect.height = NWDGUI.kTableRowHeight;
                Rect tRectInfos = new Rect(tRect.x + NWDGUI.kFieldMarge, tRect.y, tRect.width - NWDGUI.kFieldMarge * 2, tRect.height);
                EditorGUI.HelpBox(tRectInfos, NWDConstants.K_ALERT_SALT_SHORT_ERROR, MessageType.Error);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                tRect.height = NWDGUI.KTableSearchButton.fixedHeight;
                Rect tRectButton = new Rect(tRect.x + NWDGUI.kFieldMarge, tRect.y, tRect.width - NWDGUI.kFieldMarge * 2, tRect.height);
                if (GUI.Button(tRectButton, NWDConstants.K_APP_CLASS_SALT_REGENERATE, NWDGUI.KTableSearchButton))
                {
                    NWDEditorWindow.GenerateCSharpFile();
                    //NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                    GUIUtility.ExitGUI();
                }
                tRect.y += NWDGUI.KTableSearchButton.fixedHeight + NWDGUI.kFieldMarge;
            }
            // Alert warning prefixs infos
            if (TablePrefix != TablePrefixOld)
            {
                if (tMargeNeed == false)
                {
                    tRect.y += NWDGUI.kFieldMarge;
                }
                tMargeNeed = true;

                tRect.y += NWDGUI.WarningBox(NWDGUI.MargeLeftRight(tRect), NWDConstants.K_APP_BASIS_WARNING_PREFIXE).height + NWDGUI.kFieldMarge;
            }
            // Alert warning model infos
            if (WebModelChanged == true)
            {
                if (tMargeNeed == false)
                {
                    tRect.y += NWDGUI.kFieldMarge;
                }
                tMargeNeed = true;

                tRect.y += NWDGUI.WarningBox(NWDGUI.MargeLeftRight(tRect), NWDConstants.K_APP_BASIS_WARNING_MODEL).height + NWDGUI.kFieldMarge;
            }
            // alert degadraded model infos
            if (WebModelDegraded == true)
            {
                if (tMargeNeed == false)
                {
                    tRect.y += NWDGUI.kFieldMarge;
                }
                tMargeNeed = true;

                tRect.y += NWDGUI.WarningBox(NWDGUI.MargeLeftRight(tRect), NWDConstants.K_APP_BASIS_WARNING_MODEL_DEGRADED).height + NWDGUI.kFieldMarge;

                //if (GUI.Button(tRect, "See log", NWDGUI.KTableSearchButton))
                //{
                //    Debug.Log("log ...");
                //}
                //tRect.y += NWDGUI.KTableSearchButton.fixedHeight + NWDGUI.kFieldMarge;

            }
            //if (SearchActions == true)
            {
                if (tMargeNeed == false)
                {
                    tRect.y += NWDGUI.kFieldMarge;
                }
                tMargeNeed = true;
            }

            Rect tRectForTop = DrawTableEditorTop(tRect, sRectReal);
            tRect.y += tRectForTop.height;

            float tPageSizeHeight = 0;
            bool tPageToolbar = false;
            if (Datas.Count > m_ItemPerPage)
            {
                tPageToolbar = true;
                tPageSizeHeight = NWDGUI.KTableToolbar.fixedHeight + NWDGUI.kFieldMarge;

                if (tMargeNeed == false)
                {
                    tRect.y += NWDGUI.kFieldMarge;
                }
                tMargeNeed = true;
            }


            float tScrollRectHeight = tWindowRect.height - tRect.y - tPageSizeHeight * 2 - NWDGUI.kTableHeaderHeight + NWDGUI.kFieldMarge * 2;


            Rect tScrollRect = new Rect(0, tRect.y + NWDGUI.kTableHeaderHeight + tPageSizeHeight, tWindowRect.width, tScrollRectHeight);
            if (tPageToolbar == true)
            {
                tScrollRect.height -= NWDGUI.kFieldMarge;
                //tScrollHeight -= NWDGUI.kFieldMarge;
            }

            // ===========================================
            /// DRAW SCROLLVIEW
            /// 
            if (rLoaded == false)
            {
                //TODO : draw not loading
                float tWidthDilaog = NWDGUI.KTableSearchLabelWidth * 2 + NWDGUI.KTableSearchWidth;
                EditorGUIUtility.labelWidth = NWDGUI.KTableSearchLabelWidth * 2;
                //float tScrollHeight = tWindowRect.height - tRect.y - tRectForBottom.height - NWDGUI.kTableHeaderHeight - NWDGUI.kFieldMarge;
                Rect tDialogRect = new Rect(0, tRect.y + NWDGUI.kTableHeaderHeight, tWindowRect.width, NWDGUI.kLabelStyle.fixedHeight);

                tDialogRect.x = Mathf.Floor((tDialogRect.width - tWidthDilaog) / 2.0F);
                tDialogRect.width = tWidthDilaog;
                tDialogRect.height += (NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge) * 3 + NWDGUI.ErrorMinHeight;

                EditorGUI.EndDisabledGroup();
                
                if (NWDLauncher.GetState() == NWDStatut.EditorReady)
                {
                    tDialogRect.height += NWDGUI.kFieldMarge;
                    Rect tDialogRectBox = NWDGUI.UnMargeAll(tDialogRect);
                    GUI.Label(tDialogRectBox, "", EditorStyles.helpBox);
                    tDialogRect.height += NWDGUI.kTextFieldStyle.fixedHeight;
                    tDialogRect.y += NWDGUI.WarningBox(tDialogRect, NWDLauncher.GetState().ToString()).height + NWDGUI.kFieldMarge;

                    tDialogRect.height = NWDGUI.kTextFieldStyle.fixedHeight;

                    if (GUI.Button(tDialogRect, "Connect to database account", NWDGUI.kMiniButtonStyle))
                    {
                        GUI.FocusControl(null);
                        NWDLauncher.Launch();
                    }
                }
                else /*if (NWDLauncher.GetState() != NWDStatut.NetWorkedDataReady)*/
                {
                    tDialogRect.height += NWDGUI.kFieldMarge;
                    Rect tDialogRectBox = NWDGUI.UnMargeAll(tDialogRect);
                    GUI.Label(tDialogRectBox, "", EditorStyles.helpBox);
                    tDialogRect.height += NWDGUI.kTextFieldStyle.fixedHeight;
                    tDialogRect.y += NWDGUI.WarningBox(tDialogRect, NWDLauncher.GetState().ToString()).height + NWDGUI.kFieldMarge;
                }
                EditorGUIUtility.labelWidth = NWDGUI.KTableSearchLabelWidth;
                EditorGUI.BeginDisabledGroup(!rLoaded);
            }
            else
            {
                Rect tPagesTabRect = new Rect(tRect.x, tRect.y, sRect.width, tPageSizeHeight);
                DrawPagesTab(tPagesTabRect);
                tRect.y += tPageSizeHeight;

                float tRowHeight = NWDGUI.kTableRowHeight * RowZoom;
                // ===========================================
                // Get index rows for this page
                int tIndexPageStart = m_ItemPerPage * m_PageSelected;
                int tIndexPageStop = tIndexPageStart + m_ItemPerPage;
                // Limit to max
                if (tIndexPageStop > EditorTableDatas.Count)
                {
                    tIndexPageStop = EditorTableDatas.Count;
                }
                int tIndexRowInPage = tIndexPageStop - tIndexPageStart + 1;
                // create rects for scrollview

                Rect tScrollContentRect = new Rect(0, 0, tWindowRect.width - NWDGUI.kScrollbar, tIndexRowInPage * tRowHeight);


                Rect tScrollHeader = new Rect(tScrollRect.x, tScrollRect.y - NWDGUI.kTableHeaderHeight, tWindowRectOriginal.width, NWDGUI.kTableHeaderHeight);
                Rect tScrollHeaderBottom = new Rect(tScrollRect.x, tScrollRect.y + tScrollRect.height, tWindowRectOriginal.width, NWDGUI.kTableHeaderHeight);
                Rect tScrollRectDelimited = new Rect(tScrollRect.x, tScrollRect.y, sRect.width, tScrollRect.height);
                DrawHeaderInEditor(tScrollHeader, tScrollRect, RowZoom);
                DrawHeaderBottomInEditor(tScrollHeaderBottom, tScrollRect);
                // Get only visible rows
                int tIndexMax = Mathf.Min(Mathf.FloorToInt(tScrollRect.height / tRowHeight), m_ItemPerPage);
                int tIndexStart = Mathf.FloorToInt(m_ScrollPositionList.y / tRowHeight);
                int tIndexStop = tIndexStart + tIndexMax;
                if (tIndexStop - tIndexStart < m_ItemPerPage - 1)
                {
                    tIndexStop++;
                }
                m_ScrollPositionList = GUI.BeginScrollView(
                    /*tScrollRect*/ tScrollRectDelimited,
                    m_ScrollPositionList,
                    tScrollContentRect,
                    false,
                    true,
                    GUIStyle.none,
                    GUI.skin.verticalScrollbar
                    );
                tRect.y += tScrollRect.height + NWDGUI.kTableHeaderHeight * 2;
                // ===========================================
                // EVENT USE
                bool tSelectAndClick = false;
                Vector2 tMousePosition = new Vector2(-200, -200);
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    tMousePosition = Event.current.mousePosition;
                    if (Event.current.alt == true)
                    {
                        //Debug.Log("alt and select");
                        tSelectAndClick = true;
                        sEditorWindow.Focus();
                    }
                }
                // TODO: add instruction in tab view
                // KEY S
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.S)
                {
                    NWDTypeClass tSelected = GetObjectInEdition();
                    if (tSelected != null)
                    {
                        if (DatasByReference.ContainsKey(tSelected.Reference))
                        {
                            //if (tSelected.XX == 0 && tSelected.TestIntegrity())
                            {
                                //int tIndex = Datas().ObjectsByReferenceList.IndexOf(tSelected.Reference);
                                if (EditorTableDatasSelected.ContainsKey(tSelected))
                                {
                                    EditorTableDatasSelected[tSelected] = !EditorTableDatasSelected[tSelected];
                                }
                                Event.current.Use();
                            }
                        }
                    }
                }
                // TODO: add instruction in tab view
                // KEY DOWN ARROW
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.DownArrow)
                {
                    //Debug.LogVerbose ("KeyDown DownArrow", DebugResult.Success);
                    NWDTypeClass tSelected = GetObjectInEdition();
                    if (tSelected != null)
                    {
                        if (EditorTableDatas.Contains(tSelected))
                        {
                            int tIndexSelected = EditorTableDatas.IndexOf(tSelected);
                            if (tIndexSelected < EditorTableDatas.Count - 1)
                            {
                                NWDTypeClass tNextSelected = EditorTableDatas.ElementAt(tIndexSelected + 1);
                                SetObjectInEdition(tNextSelected);
                                //float tNumberPage = (tIndexSelected + 1) / m_ItemPerPage;
                                //int tPageExpected = (int)Math.Floor(tNumberPage);
                                //m_PageSelected = tPageExpected;
                                ChangeScroolPositionToSelection(tScrollRect);
                                Event.current.Use();
                                sEditorWindow.Focus();
                            }
                        }
                        else
                        {
                        }
                    }
                }
                // TODO: add instruction in tab view
                // KEY UP ARROW
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.UpArrow)
                {
                    //Debug.LogVerbose ("KeyDown UpArrow", DebugResult.Success);
                    NWDTypeClass tSelected = GetObjectInEdition();
                    if (tSelected != null)
                    {
                        if (EditorTableDatas.Contains(tSelected))
                        {
                            int tIndexSelected = EditorTableDatas.IndexOf(tSelected);
                            if (tIndexSelected > 0)
                            {
                                NWDTypeClass tNextSelected = EditorTableDatas.ElementAt(tIndexSelected - 1);
                                SetObjectInEdition(tNextSelected);
                                //float tNumberPage = (tIndexSelected - 1) / m_ItemPerPage;
                                //int tPageExpected = (int)Math.Floor(tNumberPage);
                                //m_PageSelected = tPageExpected;
                                ChangeScroolPositionToSelection(tScrollRect);
                                Event.current.Use();
                                sEditorWindow.Focus();
                            }
                        }
                        else
                        {
                        }
                    }
                }
                float tNumberOfPage = EditorTableDatas.Count / m_ItemPerPage;
                int tPagesExpected = (int)Math.Floor(tNumberOfPage);
                // TODO: add instruction in tab view
                // KEY RIGHT ARROW
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.RightArrow)
                {
                    //Debug.LogVerbose ("KeyDown RightArrow", DebugResult.Success);
                    if (m_PageSelected < tPagesExpected)
                    {
                        m_PageSelected++;
                        // TODO : reselect first object
                        int tIndexSel = m_ItemPerPage * m_PageSelected;
                        if (tIndexSel < EditorTableDatas.Count)
                        {
                            NWDTypeClass tNextSelected = EditorTableDatas.ElementAt(tIndexSel);
                            SetObjectInEdition(tNextSelected);
                            ChangeScroolPositionToSelection(tScrollRect);
                            Event.current.Use();
                            sEditorWindow.Focus();
                        }
                    }
                    else
                    {
                    }
                }
                // TODO: add instruction in tab view
                // KEY LEFT ARROW
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.LeftArrow)
                {
                    //Debug.LogVerbose ("KeyDown LeftArrow", DebugResult.Success);
                    if (m_PageSelected > 0)
                    {
                        m_PageSelected--;
                        // TODO : reselect first object
                        NWDTypeClass tNextSelected = EditorTableDatas.ElementAt(m_ItemPerPage * m_PageSelected);
                        SetObjectInEdition(tNextSelected);
                        ChangeScroolPositionToSelection(tScrollRect);
                        Event.current.Use();
                        sEditorWindow.Focus();
                    }
                    else
                    {
                    }
                }
                if (m_PageSelected < 0)
                {
                    m_PageSelected = 0;
                }
                if (m_PageSelected > tPagesExpected)
                {
                    m_PageSelected = tPagesExpected;
                }

                Rect tRectRow = new Rect(sRect.x, sRect.y, tRect.width, sRect.height);

                if (tRectRow.width < NWDGUI.KTableMinWidth)
                {
                    tRectRow.width = NWDGUI.KTableMinWidth;
                }
                for (int i = tIndexStart; i < tIndexStop; i++)
                {
                    int tItemIndexInPage = m_ItemPerPage * m_PageSelected + i;
                    if (tItemIndexInPage >= 0 && tItemIndexInPage < EditorTableDatas.Count)
                    {
                        NWDTypeClass tObject = EditorTableDatas.ElementAt(tItemIndexInPage);
                        tObject.DrawRowInEditor(tMousePosition, tRectRow, tSelectAndClick, i, RowZoom);
                    }
                }
                // ===========================================
                GUI.EndScrollView();
                //if (tPagesBarHeight > 0)
                //{
                //    tRect.y += NWDGUI.kFieldMarge;
                //    tRect.y += 
                //}
                tRect.y += NWDGUI.kFieldMarge;
                Rect tPagesTabRectBottom = new Rect(tRect.x, tRect.y, sRect.width, tPageSizeHeight);
                DrawPagesTab(tPagesTabRectBottom);
                //tRect.y += NWDGUI.Line(tRect).height;
                // ===========================================

                EditorGUI.EndDisabledGroup();
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisHelperPanel PanelActivate = NWDBasisHelperPanel.Data;
        //-------------------------------------------------------------------------------------------------------------
        public void ActivePanel(NWDBasisHelperPanel svalue)
        {
            if (svalue != PanelActivate)
            {
                PanelActivate = svalue;
                NWDProjectPrefs.SetInt(ActionsPrefkey(() => PanelActivate), (int)PanelActivate);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawPanelData(Rect sRect)
        {
            string tLastReferenceSelected = NWDProjectPrefs.GetString(LastSelectedObjectKey(), string.Empty);
            if (string.IsNullOrEmpty(tLastReferenceSelected) == false)
            {
                mObjectInEdition = GetDataByReference(tLastReferenceSelected);
            }
            //}

            if (mObjectInEdition != null)
            {
                mObjectInEdition.DrawEditor(sRect, true, null, CurrentWindow);
            }
            else
            {
                GUI.Label(sRect, NWDConstants.K_EDITOR_NO_DATA_SELECTED, NWDGUI.kInspectorNoData);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        Vector2 ScrollInfos;
        //-------------------------------------------------------------------------------------------------------------
        public void DrawPanelInfos(Rect sRect)
        {
            GUILayout.BeginArea(sRect);
            ScrollInfos = GUILayout.BeginScrollView(ScrollInfos,
                    false,
                    true,
                    GUIStyle.none,
                    GUI.skin.verticalScrollbar);

            if (GUILayout.Button(new GUIContent(ClassNamePHP, TextureOfClass()), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4)))
            {
                NWEScriptOpener.OpenScript(ClassType);
            }

            // draw section
            NWDGUILayout.Section("Informations");
            EditorGUILayout.LabelField("Trigramme", ClassTrigramme);
            EditorGUILayout.HelpBox(ClassDescription, MessageType.Info);

            // draw subsection
            NWDGUILayout.SubSection("Properties");
            EditorGUILayout.LabelField("Class", TemplateHelper.GetClassToString());
            EditorGUILayout.LabelField("Device", TemplateHelper.GetDeviceDatabase().ToString());
            EditorGUILayout.LabelField("Synchronize", TemplateHelper.GetSynchronizable().ToString());
            EditorGUILayout.LabelField("Bundle", TemplateHelper.GetBundlisable().ToString());
            EditorGUILayout.LabelField("Account", TemplateHelper.GetAccountDependent().ToString());
            EditorGUILayout.LabelField("GameSave", TemplateHelper.GetGamesaveDependent().ToString());

            // draw subsection
            NWDGUILayout.SubSection("Security");
            if (SaltValid == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_ALERT_SALT_SHORT_ERROR, MessageType.Warning);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDEditorWindow.GenerateCSharpFile();
                    GUIUtility.ExitGUI();
                }
            }
            EditorGUILayout.LabelField("Salt start", SaltStart);
            EditorGUILayout.LabelField("Salt end", SaltEnd);

            // draw section
            NWDGUILayout.Section("WebServices");
            EditorGUILayout.HelpBox("Webservice last version generated for this Class  is " + LastWebBuild.ToString() + " ( App use Webservice " + NWDAppConfiguration.SharedInstance().WebBuild.ToString() + ")", MessageType.Info);

            if (WebModelChanged == true)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_APP_BASIS_WARNING_MODEL + "\n" + ModelChangedGetChange(), MessageType.Warning);
                DrawWebServicesReplaces();
            }
            if (WebModelDegraded == true)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_APP_BASIS_WARNING_MODEL_DEGRADED + "\n" + ModelChangedGetChange(), MessageType.Warning);
                if (GUILayout.Button(NWDConstants.K_APP_WS_DELETE_OLD_MODEL_TOOLS))
                {
                    DeleteOldsModels();
                    NWDEditorWindow.GenerateCSharpFile();
                    GUIUtility.ExitGUI();
                }
            }
            NWDGUILayout.Separator();
            EditorGUILayout.LabelField("Model", LastWebBuild.ToString());
            EditorGUILayout.LabelField("App", NWDAppConfiguration.SharedInstance().WebBuild.ToString());
            NWDGUILayout.Separator();
            foreach (KeyValuePair<int, string> tModels in WebModelSQLOrder)
            {
                EditorGUILayout.LabelField("WebService " + tModels.Key.ToString(), PropertiesOrderArray(tModels.Key).Count() + " columns");
            }

            // draw section
            NWDGUILayout.SubSection("Table options");
            if (string.IsNullOrEmpty(TablePrefixOld) == false)
            {
                GUILayout.Label(new GUIContent("Prefixe Table in WS : " + TablePrefixOld));
            }
            GUILayout.BeginHorizontal();
            TablePrefix = EditorGUILayout.TextField(NWDConstants.K_APP_BASIS_CLASS_PREFIXE, TablePrefix);
            if (GUILayout.Button(NWDConstants.K_APP_BASIS_CLASS_RECCORD, EditorStyles.miniButton))
            {
                GUI.FocusControl(null);
                ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
                NWDEditorWindow.GenerateCSharpFile();
                GUIUtility.ExitGUI();
            }
            GUILayout.EndHorizontal();
            if (TablePrefix != TablePrefixOld)
            {
                EditorGUILayout.HelpBox("YOU CHANGE TABLE OPTION! YOU MUST REGENERATE CSHARP CONFIGURATION AND REGENERATE WEBSERVICES!", MessageType.Warning);

                //if (NWDGUILayout.WarningBoxButton(NWDConstants.K_APP_BASIS_WARNING_PREFIXE + " TablePrefix ='" + TablePrefix + "' but TablePrefixOld ='" + TablePrefixOld + "'", NWDConstants.K_APP_WS_PHP_TOOLS.Replace("XXXX", NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000"))))
                //{
                //    ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
                //    //NWDAppConfiguration.SharedInstance().DevEnvironment.CreatePHP(new List<Type> { ClassType }, false, false);
                //    //NWDAppConfiguration.SharedInstance().PreprodEnvironment.CreatePHP(new List<Type> { ClassType }, false, false);
                //    //NWDAppConfiguration.SharedInstance().ProdEnvironment.CreatePHP(new List<Type> { ClassType }, false, false);
                //    NWDEditorWindow.GenerateCSharpFile();
                //    GUIUtility.ExitGUI();
                //}
            }


            // draw section
            NWDGUILayout.Section("Regenerate files");

            if (GUILayout.Button("Reset Icon", EditorStyles.miniButton))
            {
                ResetIconByDefaultIcon();
            }

            if (GUILayout.Button("Generate File Empty Template", EditorStyles.miniButton))
            {
                GenerateFileEmptyTemplate();
            }

            if (GUILayout.Button("Generate File UnitTest", EditorStyles.miniButton))
            {
                GenerateFileUnitTest();
            }

            if (GUILayout.Button("Generate File Connection", EditorStyles.miniButton))
            {
                GenerateFileConnection();
            }

            if (GUILayout.Button("Generate File Workflow", EditorStyles.miniButton))
            {
                GenerateFileWorkflow();
            }

            if (GUILayout.Button("Generate File Override", EditorStyles.miniButton))
            {
                GenerateFileOverride();
            }

            if (GUILayout.Button("Generate File Extension", EditorStyles.miniButton))
            {
                GenerateFileExtension();
            }

            if (GUILayout.Button("Generate File Helper", EditorStyles.miniButton))
            {
                GenerateFileHelper();
            }

            if (GUILayout.Button("Generate File Editor", EditorStyles.miniButton))
            {
                GenerateFileEditor();
            }

            if (GUILayout.Button("Generate File Index", EditorStyles.miniButton))
            {
                GenerateFileIndex();
            }

            if (GUILayout.Button("Generate File PHP", EditorStyles.miniButton))
            {
                GenerateFilePHP();
            }
            if (GUILayout.Button("Generate File Icon", EditorStyles.miniButton))
            {
                GenerateFileIcon();
            }

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        //-------------------------------------------------------------------------------------------------------------
        Vector2 ScrollAction;
        //-------------------------------------------------------------------------------------------------------------
        public void DrawPanelAction(Rect sRect)
        {
            int tSelectionCount = 0;
            int tActualItems = EditorTableDatas.Count;
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
            {
                if (tKeyValue.Value == true)
                {
                    tSelectionCount++;
                }
            }

            GUILayout.BeginArea(sRect);
            ScrollAction = GUILayout.BeginScrollView(ScrollAction,
                    false,
                    true,
                    GUIStyle.none,
                    GUI.skin.verticalScrollbar);

            if (GUILayout.Button(new GUIContent(ClassNamePHP, TextureOfClass()), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4)))
            {
                NWEScriptOpener.OpenScript(ClassType);
            }

            // draw section
            NWDGUILayout.Section("Rows Actions");

            // draw subsection
            NWDGUILayout.SubSection("Selection");

            if (tSelectionCount == 0)
            {
                GUILayout.Label(NWDConstants.K_APP_TABLE_NO_SELECTED_OBJECT, NWDGUI.KTableSearchTitle);
            }
            else if (tSelectionCount == 1)
            {
                GUILayout.Label(NWDConstants.K_APP_TABLE_ONE_SELECTED_OBJECT, NWDGUI.KTableSearchTitle);
            }
            else
            {
                GUILayout.Label(tSelectionCount + NWDConstants.K_APP_TABLE_XX_SELECTED_OBJECT, NWDGUI.KTableSearchTitle);
            }
            // draw select all
            EditorGUI.BeginDisabledGroup(tSelectionCount == tActualItems);
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_SELECT_ALL, NWDGUI.KTableSearchButton))
            {
                SelectAllObjectInTableList();
            }
            EditorGUI.EndDisabledGroup();
            // draw deselect all
            EditorGUI.BeginDisabledGroup(tSelectionCount == 0);
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_DESELECT_ALL, NWDGUI.KTableSearchButton))
            {
                DeselectAllObjectInTableList();
            }
            EditorGUI.EndDisabledGroup();
            // draw inverse
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_INVERSE, NWDGUI.KTableSearchButton))
            {
                InverseSelectionOfAllObjectInTableList();
            }
            // draw select all enable
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_SELECT_ENABLED, NWDGUI.KTableSearchButton))
            {
                SelectAllObjectEnableInTableList();
            }
            // draw select all disable
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_SELECT_DISABLED, NWDGUI.KTableSearchButton))
            {
                SelectAllObjectDisableInTableList();
            }

            // draw subsection
            NWDGUILayout.SubSection("Action on selection");
            // draw row Actions 
            EditorGUI.BeginDisabledGroup(tSelectionCount == 0);
            //GUILayout.Label( NWDConstants.K_APP_TABLE_ACTIONS, NWDGUI.KTableSearchTitle);
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_REACTIVE, NWDGUI.KTableSearchButton))
            {
                foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
                {
                    if (tKeyValue.Value == true)
                    {
                        NWDTypeClass tObject = tKeyValue.Key;
                        tObject.EnableData();
                        tObject.RowAnalyze();
                    }
                }
                NWDDataInspector.ActiveRepaint();
            }
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_DISACTIVE, NWDGUI.KTableSearchButton))
            {
                foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
                {
                    if (tKeyValue.Value == true)
                    {
                        NWDTypeClass tObject = tKeyValue.Key;
                        tObject.DisableData();
                        tObject.RowAnalyze();
                    }
                }
                NWDDataInspector.ActiveRepaint();
            }
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_DUPPLICATE, NWDGUI.KTableSearchButton))
            {
                NWDTypeClass tNextObjectSelected = null;
                int tNewData = 0;
                List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
                foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
                {
                    if (tKeyValue.Value == true)
                    {
                        NWDTypeClass tObject = tKeyValue.Key;
                        tListToUse.Add(tObject);
                    }
                }
                foreach (NWDTypeClass tObject in tListToUse)
                {
                    tNewData++;
                    NWDTypeClass tNextObject = tObject.Base_DuplicateData();
                    if (m_SearchTag != NWDBasisTag.NoTag)
                    {
                        tNextObject.Tag = m_SearchTag;
                        tNextObject.UpdateData();
                        tObject.RowAnalyze();
                    }
                    tNextObjectSelected = tNextObject;
                    tNextObjectSelected.RowAnalyze();
                }
                if (tNewData != 1)
                {
                    tNextObjectSelected = null;
                }
                SetObjectInEdition(tNextObjectSelected);
                m_PageSelected = m_MaxPage * 3;
                NWDDataInspector.ActiveRepaint();
            }
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_UPDATE, NWDGUI.KTableSearchButton))
            {
                foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
                {
                    if (tKeyValue.Value == true)
                    {
                        NWDTypeClass tObject = tKeyValue.Key;
                        tObject.UpdateData();
                        tObject.RowAnalyze();
                    }
                }
                NWDDataInspector.ActiveRepaint();
            }

            NWDGUI.BeginRedArea();
            // DELETE SELECTION
            //GUILayout.Label(NWDConstants.K_APP_TABLE_DELETE_WARNING, NWDGUI.KTableSearchTitle);

            if (GUILayout.Button(NWDConstants.K_APP_TABLE_DELETE_BUTTON, NWDGUI.KTableSearchButton))
            {
                string tDialog = string.Empty;
                if (tSelectionCount == 0)
                {
                    tDialog = NWDConstants.K_APP_TABLE_DELETE_NO_OBJECT;
                }
                else if (tSelectionCount == 1)
                {
                    tDialog = NWDConstants.K_APP_TABLE_DELETE_ONE_OBJECT;
                }
                else
                {
                    tDialog = NWDConstants.K_APP_TABLE_DELETE_X_OBJECTS_A + tSelectionCount + NWDConstants.K_APP_TABLE_DELETE_X_OBJECTS_B;
                }
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_TABLE_DELETE_ALERT,
                        tDialog,
                        NWDConstants.K_APP_TABLE_DELETE_YES,
                        NWDConstants.K_APP_TABLE_DELETE_NO))
                {
                    List<NWDTypeClass> tListToDelete = new List<NWDTypeClass>();
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
                    {
                        if (tKeyValue.Value == true)
                        {
                            NWDTypeClass tObject = tKeyValue.Key;
                            tListToDelete.Add(tObject);
                        }
                    }
                    foreach (NWDTypeClass tObject in tListToDelete)
                    {
                        NWDTypeClass tObjectToDelete = tObject;
                        tObjectToDelete.DeleteData();
                    }
                    SetObjectInEdition(null);
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType);
                    NWDDataInspector.ActiveRepaint();
                }
                GUIUtility.ExitGUI();
            }

            // TRASH SELECTION
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_TRASH_ZONE, NWDGUI.KTableSearchButton))
            {
                //tTrashSelection = true;
                string tDialog = string.Empty;
                if (tSelectionCount == 0)
                {
                    tDialog = NWDConstants.K_APP_TABLE_TRASH_NO_OBJECT;
                }
                else if (tSelectionCount == 1)
                {
                    tDialog = NWDConstants.K_APP_TABLE_TRASH_ONE_OBJECT;
                }
                else
                {
                    tDialog = NWDConstants.K_APP_TABLE_TRASH_X_OBJECT_A + tSelectionCount + NWDConstants.K_APP_TABLE_TRASH_X_OBJECT_B;
                }
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_TABLE_TRASH_ALERT,
                        tDialog,
                        NWDConstants.K_APP_TABLE_TRASH_YES,
                        NWDConstants.K_APP_TABLE_TRASH_NO))
                {

                    List<NWDTypeClass> tListToTrash = new List<NWDTypeClass>();
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
                    {
                        if (tKeyValue.Value == true)
                        {
                            NWDTypeClass tObject = tKeyValue.Key;
                            tListToTrash.Add(tObject);
                        }
                    }
                    foreach (NWDTypeClass tObject in tListToTrash)
                    {
                        NWDTypeClass tObjectToTrash = tObject;
                        tObjectToTrash.TrashData();
                        tObjectToTrash.RowAnalyze();
                    }
                    SetObjectInEdition(null);
                    //                  sEditorWindow.Repaint ();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType);
                    NWDDataInspector.ActiveRepaint();
                }
                GUIUtility.ExitGUI();
            }

            // UNTRASH SELECTION
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_UNTRASH_ZONE, NWDGUI.KTableSearchButton))
            {
                //tUntrashSelection = true;
                string tDialog = string.Empty;
                if (tSelectionCount == 0)
                {
                    tDialog = NWDConstants.K_APP_TABLE_UNTRASH_NO_OBJECT;
                }
                else if (tSelectionCount == 1)
                {
                    tDialog = NWDConstants.K_APP_TABLE_UNTRASH_ONE_OBJECT;
                }
                else
                {
                    tDialog = NWDConstants.K_APP_TABLE_UNTRASH_X_OBJECT_A + tSelectionCount + NWDConstants.K_APP_TABLE_UNTRASH_X_OBJECT_B;
                }
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_TABLE_UNTRASH_ALERT,
                        tDialog,
                        NWDConstants.K_APP_TABLE_UNTRASH_YES,
                        NWDConstants.K_APP_TABLE_UNTRASH_NO))
                {

                    List<NWDTypeClass> tListToUntrash = new List<NWDTypeClass>();
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
                    {
                        if (tKeyValue.Value == true)
                        {
                            NWDTypeClass tObject = tKeyValue.Key;
                            tListToUntrash.Add(tObject);
                        }
                    }
                    foreach (NWDTypeClass tObject in tListToUntrash)
                    {
                        NWDTypeClass tObjectToUntrash = tObject;
                        tObjectToUntrash.UnTrashData();
                        tObjectToUntrash.RowAnalyze();
                    }
                    SetObjectInEdition(null);
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType);
                    NWDDataInspector.ActiveRepaint();
                }
                GUIUtility.ExitGUI();
            }

            // REINTEGRATE SELECTION
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_REINTEGRATE_ZONE, NWDGUI.KTableSearchButton))
            {
                //tReintegrateSelection = true;
                string tDialog = string.Empty;
                if (tSelectionCount == 0)
                {
                    tDialog = NWDConstants.K_APP_TABLE_REINTEGRATE_NO_OBJECT;
                }
                else if (tSelectionCount == 1)
                {
                    tDialog = NWDConstants.K_APP_TABLE_REINTEGRATE_ONE_OBJECT;
                }
                else
                {
                    tDialog = NWDConstants.K_APP_TABLE_REINTEGRATE_X_OBJECT_A + tSelectionCount + NWDConstants.K_APP_TABLE_REINTEGRATE_X_OBJECT_B;
                }
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_TABLE_REINTEGRATE_ALERT,
                        tDialog,
                        NWDConstants.K_APP_TABLE_REINTEGRATE_YES,
                        NWDConstants.K_APP_TABLE_REINTEGRATE_NO))
                {
                    List<NWDTypeClass> tListToReintegrate = new List<NWDTypeClass>();
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
                    {
                        if (tKeyValue.Value == true)
                        {
                            NWDTypeClass tObject = tKeyValue.Key;
                            tListToReintegrate.Add(tObject);
                        }
                    }
                    foreach (NWDTypeClass tObject in tListToReintegrate)
                    {
                        NWDTypeClass tObjectToReintegrate = tObject;
                        //tObjectToReintegrate.UpdateIntegrity();
                        tObjectToReintegrate.UpdateData();
                        tObjectToReintegrate.RowAnalyze();
                    }
                    SetObjectInEdition(null);
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType);
                    NWDDataInspector.ActiveRepaint();
                }
                GUIUtility.ExitGUI();
            }

            EditorGUI.EndDisabledGroup();
            NWDGUI.EndRedArea();

            // draw section
            NWDGUILayout.Section("Import/export ");

            // draw row Datas
            //GUILayout.Label( "Datas", NWDGUI.KTableSearchTitle);

            EditorGUI.BeginDisabledGroup(tSelectionCount == 0);
            if (GUILayout.Button("Export selection", NWDGUI.KTableSearchButton))
            {
                ExportCSV(true);
                GUIUtility.ExitGUI();
            }
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button("Export all", NWDGUI.KTableSearchButton))
            {
                ExportCSV(false);
                GUIUtility.ExitGUI();
            }
            if (GUILayout.Button("Import file", NWDGUI.KTableSearchButton))
            {
                ImportCSV();
                GUIUtility.ExitGUI();
            }


            // draw section
            NWDGUILayout.Section("Translates");
            // draw row translate
            //GUILayout.Label("Translates", NWDGUI.KTableSearchTitle);
            // export import
            // draw subsection
            NWDGUILayout.SubSection("Reorder localization");
            EditorGUI.BeginDisabledGroup(tSelectionCount == 0);
            if (GUILayout.Button("Reorder selection", NWDGUI.KTableSearchButton))
            {
                List<NWDTypeClass> tListToReorder = new List<NWDTypeClass>();
                foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
                {
                    if (tKeyValue.Value == true)
                    {
                        tListToReorder.Add(tKeyValue.Key);
                    }
                }
                ReOrderLocalizations(tListToReorder);
                NWDDataInspector.Refresh();
                GUIUtility.ExitGUI();
            }
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button("Reorder all ", NWDGUI.KTableSearchButton))
            {
                ReOrderLocalizations(Datas);
                NWDDataInspector.Refresh();
                GUIUtility.ExitGUI();
            }

            // draw subsection
            NWDGUILayout.SubSection("Export localization");
            EditorGUI.BeginDisabledGroup(tSelectionCount == 0);

            if (GUILayout.Button("Export selection", NWDGUI.KTableSearchButton))
            {
                ExportLocalization(true);
                GUIUtility.ExitGUI();
            }
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button("Export all", NWDGUI.KTableSearchButton))
            {
                ExportLocalization(false);
                GUIUtility.ExitGUI();
            }

            // draw subsection
            NWDGUILayout.SubSection("Import localization");
            if (GUILayout.Button("Import file", NWDGUI.KTableSearchButton))
            {
                NWDAppConfiguration.SharedInstance().DataLocalizationManager.ImportFromCSV();
                GUIUtility.ExitGUI();
            }

            // draw section
            NWDGUILayout.Section("Table Actions");

            if (GUILayout.Button(NWDConstants.K_APP_TABLE_SEARCH_RELOAD, NWDGUI.KTableSearchButton))
            {
                string tReference = GetReferenceOfDataInEdition();
                GUI.FocusControl(null);
                SetObjectInEdition(null);
                m_SearchInternalName = string.Empty;
                m_SearchInternalDescription = string.Empty;
                LoadFromDatabase(string.Empty, true);
                RestaureDataInEditionByReference(tReference);
            }

            NWDGUI.BeginRedArea();
            //GUILayout.Label(NWDConstants.K_APP_TABLE_RESET_WARNING, NWDGUI.KTableSearchTitle);
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_RESET_ZONE, NWDGUI.KTableSearchButton))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_TABLE_RESET_ALERT,
                                                NWDConstants.K_APP_TABLE_RESET_TABLE,
                                                NWDConstants.K_APP_TABLE_RESET_YES,
                                                NWDConstants.K_APP_TABLE_RESET_NO))
                {
                    ResetTable();
                }
                GUIUtility.ExitGUI();
            }
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_REINDEX_ZONE, NWDGUI.KTableSearchButton))
            {
                IndexInMemoryAllObjects();
                IndexInBaseAllObjects();
                GUIUtility.ExitGUI();
            }
            if (GUILayout.Button("Clean local table", NWDGUI.KTableSearchButton))
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_CLEAN_ALERT_TITLE,
                            NWDConstants.K_CLEAN_ALERT_MESSAGE,
                            NWDConstants.K_CLEAN_ALERT_OK,
                            NWDConstants.K_CLEAN_ALERT_CANCEL))
                {
                    CleanTable();
                }
                GUIUtility.ExitGUI();
            }

            // draw section
            NWDGUILayout.Section("WebServices");
            DrawWebServicesReplaces();

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        //-------------------------------------------------------------------------------------------------------------
        Vector2 ScrollSync;
        //-------------------------------------------------------------------------------------------------------------
        public void DrawPanelSync(Rect sRect)
        {


            int tSelectionCount = 0;
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
            {
                if (tKeyValue.Value == true)
                {
                    tSelectionCount++;
                }
            }
            bool tDisableProd = false;

            if (NWDDataManager.SharedInstance().ClassUnSynchronizeList.Contains(ClassType))
            {
                tDisableProd = true;
            }
            //if (kAccountDependent == true)
            if (TemplateHelper.GetAccountDependent() != NWDTemplateAccountDependent.NoAccountDependent)
            {
                tDisableProd = true;
            }


            GUILayout.BeginArea(sRect);
            ScrollSync = GUILayout.BeginScrollView(ScrollSync,
                    false,
                    true,
                    GUIStyle.none,
                    GUI.skin.verticalScrollbar);

            if (GUILayout.Button(new GUIContent(ClassNamePHP, TextureOfClass()), NWDGUI.KTableSearchClassIcon, GUILayout.Height(NWDGUI.kLabelStyle.fixedHeight * 4)))
            {
                NWEScriptOpener.OpenScript(ClassType);
            }

            // draw section
            NWDGUILayout.Section("Synchronize");

            EditorGUI.BeginDisabledGroup(WebModelChanged);

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();

            GUIContent tDevContent = new GUIContent(NWDConstants.K_DEVELOPMENT_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().DevEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
            GUILayout.Label(tDevContent, NWDGUI.KTableSearchTitle);

            //if (BasisType != NWDBasisType.UnsyncClass && BasisType != NWDBasisType.AccountUnsyncClass)
            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
            {
                if (NWDAppConfiguration.SharedInstance().DevServerIsActive())
                {
                    if (GUILayout.Button("Sync table", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        SynchronizationFromWebService(NWDAppConfiguration.SharedInstance().DevEnvironment);
                    }
                    if (GUILayout.Button("Force Sync table", NWDGUI.KTableSearchButton))
                    {

                        SynchronizationFromWebServiceForce(NWDAppConfiguration.SharedInstance().DevEnvironment);
                    }
                    if (GUILayout.Button("Pull table", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        PullFromWebService(NWDAppConfiguration.SharedInstance().DevEnvironment);
                    }
                    if (GUILayout.Button("Force Pull table", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        PullFromWebServiceForce(NWDAppConfiguration.SharedInstance().DevEnvironment);
                    }
                    EditorGUI.EndDisabledGroup();



                    //NWDGUI.BeginRedArea();
                    //EditorGUI.BeginDisabledGroup(tSelectionCount == 0 || WebModelChanged);
                    //if (GUILayout.Button(NWDConstants.K_APP_BASIS_PULL_FROM_SERVER, NWDGUI.KTableSearchButton))
                    //{
                    //    List<string> tReferencesList = new List<string>();
                    //    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
                    //    {
                    //        if (tKeyValue.Value == true)
                    //        {
                    //            tReferencesList.Add(tKeyValue.Key.Reference);
                    //        }
                    //    }
                    //    Dictionary<Type, List<string>> tTypeAndReferences = new Dictionary<Type, List<string>>();
                    //    tTypeAndReferences.Add(ClassType, tReferencesList);
                    //    PullFromWebServiceReferences(NWDAppConfiguration.SharedInstance().DevEnvironment, tTypeAndReferences);
                    //}
                    //EditorGUI.EndDisabledGroup();
                    //NWDGUI.EndRedArea();
                }
                else
                {
                    GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                }
            }
            else
            {
                GUILayout.Label("sync forbidden", NWDGUI.kNoConfigStyle);
            }

            GUILayout.EndVertical();
            // Change Colmun;
            GUILayout.BeginVertical();

            GUIContent tPreprodContent = new GUIContent(NWDConstants.K_PREPRODUCTION_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().PreprodEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
            GUILayout.Label(tPreprodContent, NWDGUI.KTableSearchTitle);

            //if (BasisType != NWDBasisType.UnsyncClass && BasisType != NWDBasisType.AccountUnsyncClass)
            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
            {
                if (NWDAppConfiguration.SharedInstance().PreprodServerIsActive())
                {
                    EditorGUI.BeginDisabledGroup(WebModelChanged);
                    if (GUILayout.Button("Sync table", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        SynchronizationFromWebService(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                    }
                    if (GUILayout.Button("Force Sync table", NWDGUI.KTableSearchButton))
                    {

                        SynchronizationFromWebServiceForce(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                    }
                    if (GUILayout.Button("Pull table", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        PullFromWebService(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                    }
                    if (GUILayout.Button("Force Pull table", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        PullFromWebServiceForce(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                    }
                    EditorGUI.EndDisabledGroup();

                    //NWDGUI.BeginRedArea();
                    //EditorGUI.BeginDisabledGroup(tSelectionCount == 0 || WebModelChanged);
                    //if (GUILayout.Button( NWDConstants.K_APP_BASIS_PULL_FROM_SERVER, NWDGUI.KTableSearchButton))
                    //{
                    //    List<string> tReferencesList = new List<string>();
                    //    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
                    //    {
                    //        if (tKeyValue.Value == true)
                    //        {
                    //            tReferencesList.Add(tKeyValue.Key.Reference);
                    //        }
                    //    }
                    //    Dictionary<Type, List<string>> tTypeAndReferences = new Dictionary<Type, List<string>>();
                    //    tTypeAndReferences.Add(ClassType, tReferencesList);
                    //    PullFromWebServiceReferences(NWDAppConfiguration.SharedInstance().PreprodEnvironment, tTypeAndReferences);
                    //}
                    //EditorGUI.EndDisabledGroup();
                    //NWDGUI.EndRedArea();
                }
                else
                {
                    GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                }
            }
            else
            {
                GUILayout.Label("sync forbidden", NWDGUI.kNoConfigStyle);
            }
            GUILayout.EndVertical();
            // Change Colmun
            GUILayout.BeginVertical();

            GUIContent tProdContent = new GUIContent(NWDConstants.K_PRODUCTION_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().ProdEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
            GUILayout.Label(tProdContent, NWDGUI.KTableSearchTitle);
            //if (BasisType == NWDBasisType.EditorClass)
            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
            {
                if (NWDAppConfiguration.SharedInstance().ProdServerIsActive())
                {
                    EditorGUI.BeginDisabledGroup(tDisableProd || WebModelChanged);
                    if (GUILayout.Button("Sync table", NWDGUI.KTableSearchButton))
                    {
                        //tSyncProd = true;
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                        }
                        SynchronizationFromWebService(NWDAppConfiguration.SharedInstance().ProdEnvironment);
                        GUIUtility.ExitGUI();
                    }
                    if (GUILayout.Button("Force Sync table", NWDGUI.KTableSearchButton))
                    {
                        //tSyncForceProd = true;
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                        }
                        SynchronizationFromWebServiceForce(NWDAppConfiguration.SharedInstance().ProdEnvironment);
                        GUIUtility.ExitGUI();
                    }
                    if (GUILayout.Button("Pull table", NWDGUI.KTableSearchButton))
                    {
                        //tPullProd = true;
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                        }
                        PullFromWebService(NWDAppConfiguration.SharedInstance().ProdEnvironment);
                        GUIUtility.ExitGUI();
                    }
                    if (GUILayout.Button("Force Pull table", NWDGUI.KTableSearchButton))
                    {
                        //tPullProdForce = true;
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                        }
                        PullFromWebServiceForce(NWDAppConfiguration.SharedInstance().ProdEnvironment);
                        GUIUtility.ExitGUI();
                    }
                    EditorGUI.EndDisabledGroup();

                    //NWDGUI.BeginRedArea();
                    //EditorGUI.BeginDisabledGroup(tSelectionCount == 0 || WebModelChanged);
                    //if (GUILayout.Button( NWDConstants.K_APP_BASIS_PULL_FROM_SERVER, NWDGUI.KTableSearchButton))
                    //{
                    //    List<string> tReferencesList = new List<string>();
                    //    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
                    //    {
                    //        if (tKeyValue.Value == true)
                    //        {
                    //            tReferencesList.Add(tKeyValue.Key.Reference);
                    //        }
                    //    }
                    //    Dictionary<Type, List<string>> tTypeAndReferences = new Dictionary<Type, List<string>>();
                    //    tTypeAndReferences.Add(ClassType, tReferencesList);
                    //    PullFromWebServiceReferences(NWDAppConfiguration.SharedInstance().ProdEnvironment, tTypeAndReferences);
                    //}
                    //EditorGUI.EndDisabledGroup();
                    //NWDGUI.EndRedArea();
                }
                else
                {
                    GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                }
            }
            else
            {
                GUILayout.Label("sync forbidden", NWDGUI.kNoConfigStyle);
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            EditorGUI.EndDisabledGroup();


            // draw section
            NWDGUILayout.Section("Operations");

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();


            GUILayout.Label(tDevContent, NWDGUI.KTableSearchTitle);

            //if (BasisType != NWDBasisType.UnsyncClass && BasisType != NWDBasisType.AccountUnsyncClass)
            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
            {
                if (NWDAppConfiguration.SharedInstance().DevServerIsActive())
                {
                    NWDGUI.BeginRedArea();
                    EditorGUI.BeginDisabledGroup(WebModelChanged);
                    if (GUILayout.Button("Clean table", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        //SynchronizationFromWebServiceClean(NWDAppConfiguration.SharedInstance().DevEnvironment);
                        SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().DevEnvironment, NWDOperationSpecial.Clean);
                    }
                    if (GUILayout.Button("Special", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().DevEnvironment, NWDOperationSpecial.Special);
                    }
                    if (GUILayout.Button("Upgrade", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().DevEnvironment, NWDOperationSpecial.Upgrade);
                    }
                    if (GUILayout.Button("Optimize", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().DevEnvironment, NWDOperationSpecial.Optimize);
                    }
                    if (GUILayout.Button("Indexes", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().DevEnvironment, NWDOperationSpecial.Indexes);
                    }
                    EditorGUI.EndDisabledGroup();
                    //if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerateDev))
                    //{
                    //    if (GUILayout.Button( NWDConstants.K_APP_WS_PHP_DEV_TOOLS.Replace("XXXX", NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000")), NWDGUI.KTableSearchButton))
                    //    {
                    //        ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
                    //        NWDAppConfiguration.SharedInstance().DevEnvironment.CreatePHP(new List<Type> { ClassType }, false, false);
                    //        NWDEditorWindow.GenerateCSharpFile();
                    //        GUIUtility.ExitGUI();
                    //    }
                    //}
                    //else
                    //{
                    //    if (GUILayout.Button( "Need credentials", NWDGUI.KTableSearchButton))
                    //    {
                    //        NWDProjectCredentialsManager.SharedInstanceFocus();
                    //    }
                    //}
                    NWDGUI.EndRedArea();
                }
                else
                {
                    GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                }
            }
            else
            {
                GUILayout.Label("sync forbidden", NWDGUI.kNoConfigStyle);
            }
            GUILayout.EndVertical();
            // Change Colmun
            GUILayout.BeginVertical();
            GUILayout.Label(tPreprodContent, NWDGUI.KTableSearchTitle);
            //if (BasisType != NWDBasisType.UnsyncClass && BasisType != NWDBasisType.AccountUnsyncClass)
            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
            {
                if (NWDAppConfiguration.SharedInstance().PreprodServerIsActive())
                {
                    NWDGUI.BeginRedArea();
                    EditorGUI.BeginDisabledGroup(WebModelChanged);
                    if (GUILayout.Button("Clean table", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        //SynchronizationFromWebServiceClean(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                        SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().PreprodEnvironment, NWDOperationSpecial.Clean);
                    }
                    if (GUILayout.Button("Special", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().PreprodEnvironment, NWDOperationSpecial.Special);
                    }
                    if (GUILayout.Button("Upgrade", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().PreprodEnvironment, NWDOperationSpecial.Upgrade);
                    }
                    if (GUILayout.Button("Optimize", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().PreprodEnvironment, NWDOperationSpecial.Optimize);
                    }
                    if (GUILayout.Button("Indexes", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().PreprodEnvironment, NWDOperationSpecial.Indexes);
                    }
                    EditorGUI.EndDisabledGroup();
                    //if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGeneratePreprod))
                    //{
                    //    if (GUILayout.Button( NWDConstants.K_APP_WS_PHP_PREPROD_TOOLS.Replace("XXXX", NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000")), NWDGUI.KTableSearchButton))
                    //    {
                    //        //tCreateAllPHPForOnlyThisClassPREPROD = true;
                    //        ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
                    //        NWDAppConfiguration.SharedInstance().PreprodEnvironment.CreatePHP(new List<Type> { ClassType }, false, false);
                    //        NWDEditorWindow.GenerateCSharpFile();
                    //        GUIUtility.ExitGUI();
                    //    }
                    //}
                    //else
                    //{
                    //    if (GUILayout.Button( "Need credentials", NWDGUI.KTableSearchButton))
                    //    {
                    //        NWDProjectCredentialsManager.SharedInstanceFocus();
                    //    }
                    //}
                    NWDGUI.EndRedArea();
                }
                else
                {
                    GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                }
            }
            else
            {
                GUILayout.Label("sync forbidden", NWDGUI.kNoConfigStyle);
            }
            GUILayout.EndVertical();
            // Change Colmun
            GUILayout.BeginVertical();
            GUILayout.Label(tProdContent, NWDGUI.KTableSearchTitle);
            //if (BasisType == NWDBasisType.EditorClass)
            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)

            {
                if (NWDAppConfiguration.SharedInstance().ProdServerIsActive())
                {
                    NWDGUI.BeginRedArea();
                    EditorGUI.BeginDisabledGroup(WebModelChanged);
                    if (GUILayout.Button("Clean table", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        //SynchronizationFromWebServiceClean(NWDAppConfiguration.SharedInstance().ProdEnvironment);
                        SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().ProdEnvironment, NWDOperationSpecial.Clean);
                    }
                    if (GUILayout.Button("Special", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().ProdEnvironment, NWDOperationSpecial.Special);
                    }
                    if (GUILayout.Button("Upgrade", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().ProdEnvironment, NWDOperationSpecial.Upgrade);
                    }
                    if (GUILayout.Button("Optimize", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().ProdEnvironment, NWDOperationSpecial.Optimize);
                    }
                    if (GUILayout.Button("Indexes", NWDGUI.KTableSearchButton))
                    {
                        if (Application.isPlaying == true && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)
                        {
                            EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                        }
                        SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().ProdEnvironment, NWDOperationSpecial.Indexes);
                    }
                    EditorGUI.EndDisabledGroup();
                    //if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerateProd))
                    //{
                    //    if (GUILayout.Button( NWDConstants.K_APP_WS_PHP_PROD_TOOLS.Replace("XXXX", NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000")), NWDGUI.KTableSearchButton))
                    //    {
                    //        //tCreateAllPHPForOnlyThisClassPROD = true;
                    //        ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
                    //        NWDAppConfiguration.SharedInstance().ProdEnvironment.CreatePHP(new List<Type> { ClassType }, false, false);
                    //        NWDEditorWindow.GenerateCSharpFile();
                    //        GUIUtility.ExitGUI();
                    //    }
                    //}
                    //else
                    //{
                    //    if (GUILayout.Button( "Need credentials", NWDGUI.KTableSearchButton))
                    //    {
                    //        NWDProjectCredentialsManager.SharedInstanceFocus();
                    //    }
                    //}
                    NWDGUI.EndRedArea();

                }
                else
                {
                    GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                }
            }
            else
            {
                GUILayout.Label("sync forbidden", NWDGUI.kNoConfigStyle);
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            // draw section
            NWDGUILayout.Section("WebServices");

            DrawWebServicesReplaces();

            GUILayout.EndScrollView();
            GUILayout.EndArea();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawWebServicesReplaces()
        {

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical();
            GUIContent tDevContent = new GUIContent(NWDConstants.K_DEVELOPMENT_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().DevEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
            GUIContent tPreprodContent = new GUIContent(NWDConstants.K_PREPRODUCTION_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().PreprodEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
            GUIContent tProdContent = new GUIContent(NWDConstants.K_PRODUCTION_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().ProdEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));

            GUILayout.Label(tDevContent, NWDGUI.KTableSearchTitle);

            //if (BasisType != NWDBasisType.UnsyncClass && BasisType != NWDBasisType.AccountUnsyncClass)
            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
            {
                if (NWDAppConfiguration.SharedInstance().DevServerIsActive())
                {
                    NWDGUI.BeginRedArea();
                    if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerateDev))
                    {
                        if (GUILayout.Button(NWDConstants.K_APP_WS_PHP_DEV_TOOLS.Replace("XXXX", NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000")), NWDGUI.KTableSearchButton))
                        {
                            ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
                            NWDAppConfiguration.SharedInstance().DevEnvironment.CreatePHP(new List<Type> { ClassType }, false, false);
                            NWDEditorWindow.GenerateCSharpFile();
                            GUIUtility.ExitGUI();
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Need credentials", NWDGUI.KTableSearchButton))
                        {
                            NWDProjectCredentialsManager.SharedInstanceFocus();
                        }
                    }
                    NWDGUI.EndRedArea();
                }
                else
                {
                    GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                }
            }
            else
            {
                GUILayout.Label("sync forbidden", NWDGUI.kNoConfigStyle);
            }
            GUILayout.EndVertical();
            // Change Colmun
            GUILayout.BeginVertical();
            GUILayout.Label(tPreprodContent, NWDGUI.KTableSearchTitle);
            //if (BasisType != NWDBasisType.UnsyncClass && BasisType != NWDBasisType.AccountUnsyncClass)
            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable)
            {
                if (NWDAppConfiguration.SharedInstance().PreprodServerIsActive())
                {
                    NWDGUI.BeginRedArea();
                    if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGeneratePreprod))
                    {
                        if (GUILayout.Button(NWDConstants.K_APP_WS_PHP_PREPROD_TOOLS.Replace("XXXX", NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000")), NWDGUI.KTableSearchButton))
                        {
                            //tCreateAllPHPForOnlyThisClassPREPROD = true;
                            ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
                            NWDAppConfiguration.SharedInstance().PreprodEnvironment.CreatePHP(new List<Type> { ClassType }, false, false);
                            NWDEditorWindow.GenerateCSharpFile();
                            GUIUtility.ExitGUI();
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Need credentials", NWDGUI.KTableSearchButton))
                        {
                            NWDProjectCredentialsManager.SharedInstanceFocus();
                        }
                    }
                    NWDGUI.EndRedArea();
                }
                else
                {
                    GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                }
            }
            else
            {
                GUILayout.Label("sync forbidden", NWDGUI.kNoConfigStyle);
            }
            GUILayout.EndVertical();
            // Change Colmun
            GUILayout.BeginVertical();
            GUILayout.Label(tProdContent, NWDGUI.KTableSearchTitle);
            //if (BasisType == NWDBasisType.EditorClass)
            if (TemplateHelper.GetSynchronizable() != NWDTemplateClusterDatabase.NoSynchronizable && TemplateHelper.GetAccountDependent() == NWDTemplateAccountDependent.NoAccountDependent)

            {
                if (NWDAppConfiguration.SharedInstance().ProdServerIsActive())
                {
                    NWDGUI.BeginRedArea();
                    if (NWDProjectCredentialsManager.IsValid(NWDCredentialsRequired.ForSFTPGenerateProd))
                    {
                        if (GUILayout.Button(NWDConstants.K_APP_WS_PHP_PROD_TOOLS.Replace("XXXX", NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000")), NWDGUI.KTableSearchButton))
                        {
                            //tCreateAllPHPForOnlyThisClassPROD = true;
                            ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
                            NWDAppConfiguration.SharedInstance().ProdEnvironment.CreatePHP(new List<Type> { ClassType }, false, false);
                            NWDEditorWindow.GenerateCSharpFile();
                            GUIUtility.ExitGUI();
                        }
                    }
                    else
                    {
                        if (GUILayout.Button("Need credentials", NWDGUI.KTableSearchButton))
                        {
                            NWDProjectCredentialsManager.SharedInstanceFocus();
                        }
                    }
                    NWDGUI.EndRedArea();

                }
                else
                {
                    GUILayout.Label("no config", NWDGUI.kNoConfigStyle);
                }
            }
            else
            {
                GUILayout.Label("sync forbidden", NWDGUI.kNoConfigStyle);
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDBasisHelperPanel
    {
        Infos,
        Actions,
        Sync,
        Data,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
