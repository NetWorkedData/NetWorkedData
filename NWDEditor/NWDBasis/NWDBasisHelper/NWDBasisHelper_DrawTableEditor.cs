//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:27
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        //TODO : RENAME!
        public void DrawInEditor(EditorWindow sEditorWindow, bool sAutoSelect = false)
        {
            //NWEBenchmark.Start();
            DrawTableEditor(sEditorWindow);
            if (sAutoSelect == true)
            {
                SelectedFirstObjectInTable(sEditorWindow);
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public string GetReferenceOfDataInEdition()
        {
            //NWEBenchmark.Start();
            string rReturn = null;
            NWDTypeClass tObject = NWDDataInspector.ObjectInEdition() as NWDTypeClass;
            if (tObject != null)
            {
                rReturn = string.Copy(tObject.Reference);
            }
            //NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void RestaureDataInEditionByReference(string sReference)
        {
            //NWEBenchmark.Start();
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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void SelectAllObjectInTableList()
        {
            //NWEBenchmark.Start();
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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void DeselectAllObjectInTableList()
        {
            //NWEBenchmark.Start();
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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void InverseSelectionOfAllObjectInTableList()
        {
            //NWEBenchmark.Start();
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                EditorTableDatasSelected[tObject] = !EditorTableDatasSelected[tObject];
            }
            //NWEBenchmark.Finish();
            //IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void SelectAllObjectEnableInTableList()
        {
            //NWEBenchmark.Start();
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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void SelectAllObjectDisableInTableList()
        {
            //NWEBenchmark.Start();
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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public float DrawPagesTab(Rect sRect)
        {
            //NWEBenchmark.Start();
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
            //NWEBenchmark.Finish();
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void ChangeScroolPositionToSelection()
        {
            //NWEBenchmark.Start();
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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void ChangeScroolPositionToSelection(Rect sScrollRect)
        {
            //NWEBenchmark.Start();
            int tIndexSelected = EditorTableDatas.IndexOf((NWDTypeClass)NWDDataInspector.ObjectInEdition());
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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void DrawHeaderInEditor(Rect sRect, Rect sScrollRect, float sZoom)
        {
            //NWEBenchmark.Start();
            EditorGUI.DrawRect(sRect, NWDGUI.kTableHeaderColor);
            sRect.x += NWDGUI.kFieldMarge;
            sRect.width = sRect.width - NWDGUI.kScrollbar;

            if (sRect.width < NWDGUI.KTableMinWidth)
            {
                sRect.width = NWDGUI.KTableMinWidth;
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
            //if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_ID, NWDGUI.KTableHeaderId))
            //{
            //    if (SortType == NWDBasisEditorDatasSortType.ByIDAscendant)
            //    {
            //        SortType = NWDBasisEditorDatasSortType.ByIDDescendant;
            //    }
            //    else if (SortType == NWDBasisEditorDatasSortType.ByIDDescendant)
            //    {
            //        SortType = NWDBasisEditorDatasSortType.ByIDAscendant;
            //    }
            //    else
            //    {
            //        SortType = NWDBasisEditorDatasSortType.ByIDDescendant;
            //    }
            //    SortEditorTableDatas();
            //    ChangeScroolPositionToSelection(sScrollRect);
            //}
            //tRect.x += tRect.width;
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
                //- NWDGUI.kTableIDWidth
                - NWDGUI.kTableSelectWidth
                - NWDGUI.kTablePrefabWidth * sZoom
                - NWDGUI.KTableSearchWidth
                - NWDGUI.KTableReferenceWidth
                - NWDGUI.KTableRowWebModelWidth
                - NWDGUI.kTableIconWidth * 6;

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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public void DrawHeaderBottomInEditor(Rect sRect, Rect sScrollRect)
        {
            //NWEBenchmark.Start();
            EditorGUI.DrawRect(sRect, NWDGUI.kTableHeaderColor);
            sRect.width = sRect.width - NWDGUI.kScrollbar;

            if (sRect.width < NWDGUI.KTableMinWidth)
            {
                sRect.width = NWDGUI.KTableMinWidth;
            }

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
                m_PageSelected = 0;
            }
            m_ItemPerPageSelection = t_ItemPerPageSelection;
            tRect.x += NWDGUI.KTableSearchWidth + NWDGUI.kFieldMarge;
            float tRowZoom = EditorGUI.Slider(tRect, RowZoom, 1.0F, 2.0F);
            if (System.Math.Abs(tRowZoom - RowZoom) > 0.01F)
            {
                RowZoom = tRowZoom;
                SaveEditorPrefererences();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += NWDGUI.KTableSearchWidth + NWDGUI.kFieldMarge;
            // draw toogle enable
            bool t_ShowEnableLine = EditorGUI.ToggleLeft(tRect, NWDConstants.K_APP_TABLE_SHOW_ENABLE_DATAS, m_ShowEnable);
            if (m_ShowEnable != t_ShowEnableLine)
            {
                m_ShowEnable = t_ShowEnableLine;
                FilterTableEditor();
            }
            tRect.x += NWDGUI.KTableSearchWidth + NWDGUI.kFieldMarge;
            // draw toogle disable
            bool t_ShowDisableLine = EditorGUI.ToggleLeft(tRect, NWDConstants.K_APP_TABLE_SHOW_DISABLE_DATAS, m_ShowDisable);
            if (m_ShowDisable != t_ShowDisableLine)
            {
                m_ShowDisable = t_ShowDisableLine;
                FilterTableEditor();
            }
            tRect.x += NWDGUI.KTableSearchWidth + NWDGUI.kFieldMarge;
            // draw toogle trashed
            EditorGUI.BeginDisabledGroup(!m_ShowDisable);
            bool t_ShowTrashedLine = EditorGUI.ToggleLeft(tRect, NWDConstants.K_APP_TABLE_SHOW_TRASHED_DATAS, m_ShowTrashed);
            if (m_ShowTrashed != t_ShowTrashedLine)
            {
                m_ShowTrashed = t_ShowTrashedLine;
                FilterTableEditor();
            }
            tRect.x += NWDGUI.KTableSearchWidth + NWDGUI.kFieldMarge;
            // draw toogle corrupted
            bool t_ShowIntegrityErrorLine = EditorGUI.ToggleLeft(tRect, NWDConstants.K_APP_TABLE_SHOW_INTEGRITY_ERROR_DATAS, m_ShowIntegrityError);
            if (m_ShowIntegrityError != t_ShowIntegrityErrorLine)
            {
                m_ShowIntegrityError = t_ShowIntegrityErrorLine;
                FilterTableEditor();
            }
            EditorGUI.EndDisabledGroup();
            bool tShowMoreInfos = false;
            if (tShowMoreInfos)
            {
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
                tRect.x += NWDGUI.KTableSearchWidth + NWDGUI.kFieldMarge;
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
                tRect.x += NWDGUI.KTableSearchWidth + NWDGUI.kFieldMarge;
                // draw selection
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
                tRect.x += NWDGUI.KTableSearchWidth + NWDGUI.kFieldMarge;
            }
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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public Rect DrawTableEditorTop(Rect sRect)
        {
            int tMaxForMenuOrTextField = 25;
            //NWEBenchmark.Start();
            EditorGUIUtility.labelWidth = NWDGUI.KTableSearchLabelWidth;
            Rect rRect = new Rect(sRect.x, sRect.y, sRect.width, 0);
            if (SearchActions == true)
            {
                Rect tRect = new Rect(sRect.x + NWDGUI.kFieldMarge, sRect.y, sRect.width, 0);
                tRect.width = NWDGUI.KTableSearchFieldWidth;
                tRect.height = NWDGUI.KTableSearchToggle.fixedHeight;
                // draw Search zone
                GUI.Label(tRect, NWDConstants.K_APP_TABLE_SEARCH_ZONE, NWDGUI.KTableSearchTitle);
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

                // Change column
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = sRect.y;

                GUI.Label(tRect, NWDConstants.K_APP_TABLE_FILTER_ZONE, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw accounts popup
                EditorGUI.BeginDisabledGroup(!kAccountDependent);

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
                    m_SearchAccount = EditorGUI.TextField(tRect, new GUIContent(NWDConstants.K_APP_TABLE_SEARCH_GAMESAVE), m_SearchAccount, NWDGUI.KTableSearchTextfield);
                }
                EditorGUI.EndDisabledGroup();
                // draw GameSave popup
                tRect.y += tRect.height + NWDGUI.kFieldMarge;

                EditorGUI.BeginDisabledGroup(!ClassGameSaveDependent);

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


                EditorGUI.BeginDisabledGroup(kAccountDependent);
                m_SearchCheckList = (NetWorkedData.NWDBasisCheckList)m_SearchCheckList.ControlField(tRect, NWDConstants.K_APP_TABLE_SEARCH_CHECKLIST, kAccountDependent);
                EditorGUI.EndDisabledGroup();
                tRect.y += tRect.height + NWDGUI.kFieldMarge;

                // Change column
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = sRect.y;
                tRect.width = NWDGUI.KTableSearchWidth;

                // draw title
                GUI.Label(tRect, "Actions", NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
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


                bool tShowMoreInfos = true;
                if (tShowMoreInfos)
                {
                    // Change column
                    tRect.x += tRect.width + NWDGUI.kFieldMarge;
                    tRect.y = sRect.y;
                    tRect.width = NWDGUI.KTableSearchWidth;

                    // draw title
                    GUI.Label(tRect, "Results", NWDGUI.KTableSearchTitle);
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

                    // draw selection
                    // TODO exit this method to count in basishelper;
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
                    tRect.y += tRect.height + NWDGUI.kFieldMarge;
                }
                // draw before last column
                tRect.x = sRect.width - NWDGUI.KTableSearchFieldWidth - NWDGUI.KTableSearchToggle.fixedHeight * 3 - NWDGUI.kFieldMarge * 4;
                tRect.y = sRect.y;
                tRect.width = NWDGUI.KTableSearchFieldWidth;
                // Draw title


                //GUI.Label(tRect, ClassNamePHP, NWDGUI.KTableSearchTitle);

                if (GUI.Button(tRect, ClassNamePHP, NWDGUI.KTableSearchTitle))
                {
                    //TODO edit script
                    //Debug.Log("ClassNamePHP to edit " + ClassNamePHP);
                    NWEScriptOpener.OpenScript(ClassType);

                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // Draw texture of this class
                tRect.height = NWDGUI.KTableSearchToggle.fixedHeight * 3 + NWDGUI.kFieldMarge * 2;
                GUI.Label(tRect, ClassDescription, NWDGUI.KTableSearchDescription);
                tRect.height = NWDGUI.KTableSearchToggle.fixedHeight;
                // draw last column
                tRect.x = sRect.width - NWDGUI.KTableSearchToggle.fixedHeight * 3 - NWDGUI.kFieldMarge * 3;
                tRect.y = sRect.y;
                tRect.width = NWDGUI.KTableSearchToggle.fixedHeight * 3 + NWDGUI.kFieldMarge * 2;
                // Draw title
                GUI.Label(tRect, ClassTrigramme, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // Draw texture of this class
                tRect.height = NWDGUI.KTableSearchToggle.fixedHeight * 3 + NWDGUI.kFieldMarge * 2;
                Texture2D tTextureOfClass = TextureOfClass();
                if (tTextureOfClass != null)
                {
                    GUI.Label(tRect, tTextureOfClass, NWDGUI.KTableSearchClassIcon);
                }
                tRect.height = NWDGUI.KTableSearchToggle.fixedHeight;
                rRect.height = NWDGUI.KTableSearchToggle.fixedHeight * 5 + NWDGUI.kFieldMarge * 5;
            }
            //NWEBenchmark.Finish();
            return rRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : put in private
        //TODO : RENAME!
        public Rect DrawTableEditorBottom(Rect sRect)
        {
            //NWEBenchmark.Start();
            Rect rRect = new Rect(sRect.x, sRect.y, sRect.width, 0);

            Rect tRectActionLeft = Rect.zero;
            Rect tRectActionRight = Rect.zero;
            Rect tRectTableLeft = Rect.zero;
            Rect tRectTableRight = Rect.zero;

            float tHeightAction = 0;
            float tHeightTable = 0;

            if (RowActions == true)
            {
                tHeightAction = NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge + (NWDGUI.KTableSearchTextfield.fixedHeight + NWDGUI.kFieldMarge) * 6;
            }
            if (TableActions == true)
            {
                tHeightTable = NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge + (NWDGUI.KTableSearchTextfield.fixedHeight + NWDGUI.kFieldMarge) * 6;
            }
            rRect.height = tHeightAction + tHeightTable;
            rRect.y = sRect.height - rRect.height;

            if (RowActions == true)
            {
                Rect tFoldoutRectAction = new Rect(sRect.x + NWDGUI.kFieldMarge, rRect.y, 0, 0);
                tFoldoutRectAction.height = NWDGUI.kBoldLabelStyle.fixedHeight;
                tFoldoutRectAction.width = sRect.width;
                GUI.Label(tFoldoutRectAction, "Rows Actions", NWDGUI.kBoldLabelStyle);
                tRectActionLeft = new Rect(sRect.x + NWDGUI.kFieldMarge, rRect.y + NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge, 0, 0);
                tRectActionLeft.height = NWDGUI.KTableSearchTextfield.fixedHeight;
                tRectActionLeft.width = NWDGUI.KTableSearchWidth;

                tRectActionRight = new Rect(sRect.x + sRect.width - (NWDGUI.kFieldMarge + NWDGUI.KTableSearchWidth) * 3, rRect.y + NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge, 0, 0);
                tRectActionRight.height = NWDGUI.KTableSearchTextfield.fixedHeight;
                tRectActionRight.width = NWDGUI.KTableSearchWidth;
            }

            if (TableActions == true)
            {
                Rect tFoldoutRectTable = new Rect(sRect.x + NWDGUI.kFieldMarge, rRect.y + tHeightAction, 0, 0);
                tFoldoutRectTable.height = NWDGUI.kBoldLabelStyle.fixedHeight;
                tFoldoutRectTable.width = sRect.width;
                GUI.Label(tFoldoutRectTable, "Table Actions", NWDGUI.kBoldLabelStyle);
                tRectTableLeft = new Rect(sRect.x + NWDGUI.kFieldMarge, rRect.y + NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge + tHeightAction, 0, 0);
                tRectTableLeft.height = NWDGUI.KTableSearchTextfield.fixedHeight;
                tRectTableLeft.width = NWDGUI.KTableSearchWidth;

                tRectTableRight = new Rect(sRect.x + sRect.width - (NWDGUI.kFieldMarge + NWDGUI.KTableSearchWidth) * 3, rRect.y + NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge + tHeightAction, 0, 0);
                tRectTableRight.height = NWDGUI.KTableSearchTextfield.fixedHeight;
                tRectTableRight.width = NWDGUI.KTableSearchWidth;
            }

            bool tDeleteSelection = false; //prevent GUIlayout error
            //bool tLocalizeLocalTable = false; //prevent GUIlayout error
            bool tTrashSelection = false;  //prevent GUIlayout error
            bool tUntrashSelection = false;  //prevent GUIlayout error
            bool tReintegrateSelection = false;  //prevent GUIlayout error
            bool tResetTable = false;  //prevent GUIlayout error
            bool tCreateAllPHPForOnlyThisClassDEV = false; //prevent GUIlayout error
            //bool tCreateAllPHPForOnlyThisClass = false; //prevent GUIlayout error
            //bool tReintegrateOnlyThisClass = false; //prevent GUIlayout error
            //bool tDeleteOldModelOnlyThisClass = false; //prevent GUIlayout error
            bool tSyncProd = false; //prevent GUIlayout error
            bool tSyncForceProd = false; //prevent GUIlayout error
            bool tPullProd = false; //prevent GUIlayout error
            bool tPullProdForce = false; //prevent GUIlayout error
            bool tSyncCleanProd = false; //prevent GUIlayout error
            bool tSyncSpecialProd = false; //prevent GUIlayout error
            bool tCleanLocalTable = false; //prevent GUIlayout error
            //bool tCleanLocalTableWithAccount = false; //prevent GUIlayout error
            bool tDisableProd = false;

            if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains(ClassType))
            {
                tDisableProd = true;
            }
            if (kAccountDependent == true)
            {
                tDisableProd = true;
            }
            int tSelectionCount = 0;
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
            {
                if (tKeyValue.Value == true)
                {
                    tSelectionCount++;
                }
            }
            // ===========================================
            Rect tRect = new Rect(0, 0, NWDGUI.KTableSearchWidth, NWDGUI.KTableSearchTextfield.fixedHeight);
            if (RowActions == true)
            {

                tRect.x = tRectActionLeft.x;
                tRect.y = tRectActionLeft.y;
                int tActualItems = EditorTableDatas.Count;
                if (tSelectionCount == 0)
                {
                    GUI.Label(tRect, NWDConstants.K_APP_TABLE_NO_SELECTED_OBJECT, NWDGUI.KTableSearchTitle);
                }
                else if (tSelectionCount == 1)
                {
                    GUI.Label(tRect, NWDConstants.K_APP_TABLE_ONE_SELECTED_OBJECT, NWDGUI.KTableSearchTitle);
                }
                else
                {
                    GUI.Label(tRect, tSelectionCount + NWDConstants.K_APP_TABLE_XX_SELECTED_OBJECT, NWDGUI.KTableSearchTitle);
                }
                // draw select all
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                EditorGUI.BeginDisabledGroup(tSelectionCount == tActualItems);
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_SELECT_ALL, NWDGUI.KTableSearchButton))
                {
                    SelectAllObjectInTableList();
                }
                EditorGUI.EndDisabledGroup();
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw deselect all
                EditorGUI.BeginDisabledGroup(tSelectionCount == 0);
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_DESELECT_ALL, NWDGUI.KTableSearchButton))
                {
                    DeselectAllObjectInTableList();
                }
                EditorGUI.EndDisabledGroup();
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw inverse
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_INVERSE, NWDGUI.KTableSearchButton))
                {
                    InverseSelectionOfAllObjectInTableList();
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw select all enable
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_SELECT_ENABLED, NWDGUI.KTableSearchButton))
                {
                    SelectAllObjectEnableInTableList();
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw select all disable
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_SELECT_DISABLED, NWDGUI.KTableSearchButton))
                {
                    SelectAllObjectDisableInTableList();
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // Change Colmun
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = tRectActionLeft.y;

                // draw row Actions 
                EditorGUI.BeginDisabledGroup(tSelectionCount == 0);
                GUI.Label(tRect, NWDConstants.K_APP_TABLE_ACTIONS, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_REACTIVE, NWDGUI.KTableSearchButton))
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
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_DISACTIVE, NWDGUI.KTableSearchButton))
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
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_DUPPLICATE, NWDGUI.KTableSearchButton))
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
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_UPDATE, NWDGUI.KTableSearchButton))
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
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                EditorGUI.EndDisabledGroup();
                //if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_EXPORT_TRANSLATION, NWDGUI.KTableSearchButton))
                //{
                //    tLocalizeLocalTable = true;
                //}
                // Change Colmun
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = tRectActionLeft.y;




                // draw row Datas
                GUI.Label(tRect, "Datas", NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;

                EditorGUI.BeginDisabledGroup(tSelectionCount == 0);
                if (GUI.Button(tRect, "Export data", NWDGUI.KTableSearchButton))
                {
                    ExportCSV(true);
                    GUIUtility.ExitGUI();
                }
                EditorGUI.EndDisabledGroup();
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Import data", NWDGUI.KTableSearchButton))
                {
                    ImportCSV();
                    GUIUtility.ExitGUI();
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = tRectActionLeft.y;



                // draw row translate
                GUI.Label(tRect, "Translates", NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // export import
                EditorGUI.BeginDisabledGroup(tSelectionCount == 0);
                if (GUI.Button(tRect, "Reorder localization", NWDGUI.KTableSearchButton))
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
                tRect.y += tRect.height + NWDGUI.kFieldMarge;

                if (GUI.Button(tRect, "Export localization", NWDGUI.KTableSearchButton))
                {
                    ExportLocalization(true);
                    GUIUtility.ExitGUI();
                }
                EditorGUI.EndDisabledGroup();
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Import localization", NWDGUI.KTableSearchButton))
                {
                    NWDAppConfiguration.SharedInstance().DataLocalizationManager.ImportFromCSV();
                    GUIUtility.ExitGUI();
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = tRectActionLeft.y;


                // draw row warning
                EditorGUI.BeginDisabledGroup(tSelectionCount == 0);
                NWDGUI.BeginRedArea();
                // DELETE SELECTION
                GUI.Label(tRect, NWDConstants.K_APP_TABLE_DELETE_WARNING, NWDGUI.KTableSearchTitle);

                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_DELETE_BUTTON, NWDGUI.KTableSearchButton))
                {
                    tDeleteSelection = true;
                }

                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // TRASH SELECTION
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_TRASH_ZONE, NWDGUI.KTableSearchButton))
                {
                    tTrashSelection = true;
                }

                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // UNTRASH SELECTION
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_UNTRASH_ZONE, NWDGUI.KTableSearchButton))
                {
                    tUntrashSelection = true;
                }

                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // REINTEGRATE SELECTION
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_REINTEGRATE_ZONE, NWDGUI.KTableSearchButton))
                {
                    tReintegrateSelection = true;
                }

                tRect.y += tRect.height + NWDGUI.kFieldMarge;

                //if (false)
                //{
                //    if (GUI.Button(tRect, "UnLoad", NWDGUI.KTableSearchButton))
                //    {
                //        List<NWDTypeClass> tListToUnload = new List<NWDTypeClass>();
                //        foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in EditorTableDatasSelected)
                //        {
                //            if (tKeyValue.Value == true)
                //            {
                //                tListToUnload.Add(tKeyValue.Key);
                //            }
                //        }
                //        foreach (NWDTypeClass tObject in tListToUnload)
                //        {
                //            New_UnloadDataByReference(tObject.Reference);
                //        }
                //    }
                //    tRect.y += tRect.height + NWDGUI.kFieldMarge;
                //}

                EditorGUI.EndDisabledGroup();
                NWDGUI.EndRedArea();

                tRect = new Rect(tRectActionRight.x, tRectActionRight.y, NWDGUI.KTableSearchWidth, tRectActionRight.height);

                EditorGUI.BeginDisabledGroup(WebModelChanged);

                GUIContent tDevContent = new GUIContent(NWDConstants.K_DEVELOPMENT_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().DevEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
                GUI.Label(tRect, tDevContent, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Sync table", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebService(NWDAppConfiguration.SharedInstance().DevEnvironment);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Force Sync table", NWDGUI.KTableSearchButton))
                {

                    SynchronizationFromWebServiceForce(NWDAppConfiguration.SharedInstance().DevEnvironment);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Pull table", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    PullFromWebService(NWDAppConfiguration.SharedInstance().DevEnvironment);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Force Pull table", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    PullFromWebServiceForce(NWDAppConfiguration.SharedInstance().DevEnvironment);
                }
                EditorGUI.EndDisabledGroup();
                // Change Colmun
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = tRectActionRight.y;

                GUIContent tPreprodContent = new GUIContent(NWDConstants.K_PREPRODUCTION_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().PreprodEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
                GUI.Label(tRect, tPreprodContent, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                EditorGUI.BeginDisabledGroup(WebModelChanged);
                if (GUI.Button(tRect, "Sync table", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebService(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Force Sync table", NWDGUI.KTableSearchButton))
                {

                    SynchronizationFromWebServiceForce(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Pull table", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    PullFromWebService(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Force Pull table", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    PullFromWebServiceForce(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                }
                EditorGUI.EndDisabledGroup();
                // Change Colmun
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = tRectActionRight.y;

                GUIContent tProdContent = new GUIContent(NWDConstants.K_PRODUCTION_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().ProdEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
                GUI.Label(tRect, tProdContent, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                EditorGUI.BeginDisabledGroup(tDisableProd || WebModelChanged);
                if (GUI.Button(tRect, "Sync table", NWDGUI.KTableSearchButton))
                {
                    tSyncProd = true;
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Force Sync table", NWDGUI.KTableSearchButton))
                {
                    tSyncForceProd = true;
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Pull table", NWDGUI.KTableSearchButton))
                {
                    tPullProd = true;
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Force Pull table", NWDGUI.KTableSearchButton))
                {
                    tPullProdForce = true;
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                EditorGUI.EndDisabledGroup();

            }
            if (TableActions == true)
            {
                // Start Colmun
                tRect.x = tRectTableLeft.x;
                tRect.y = tRectTableLeft.y;
                GUI.Label(tRect, NWDConstants.K_APP_TABLE_TOOLS, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;


                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_SEARCH_RELOAD, NWDGUI.KTableSearchButton))
                {
                    string tReference = GetReferenceOfDataInEdition();
                    GUI.FocusControl(null);
                    SetObjectInEdition(null);
                    m_SearchInternalName = string.Empty;
                    m_SearchInternalDescription = string.Empty;
                    LoadFromDatabase(string.Empty);
                    RestaureDataInEditionByReference(tReference);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;

                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_SHOW_TOOLS, NWDGUI.KTableSearchButton))
                {
                    NWDBasisClassInspector tBasisInspector = ScriptableObject.CreateInstance<NWDBasisClassInspector>();
                    tBasisInspector.mTypeInEdition = ClassType;
                    Selection.activeObject = tBasisInspector;
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // reset icon of clas
                if (GUI.Button(tRect, "Select script", NWDGUI.KTableSearchButton))
                {
                    SelectScript();
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // reset icon of class
                NWDGUI.BeginRedArea();
                //if (GUI.Button(tRect, "Reset Icon", NWDGUI.KTableSearchButton))
                //{
                //    ResetIconByDefaultIcon();
                //}
                //NWDGUI.EndRedArea();
                tRect.y += tRect.height + NWDGUI.kFieldMarge;

                // Change Colmun
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = tRectTableLeft.y;
                NWDGUI.BeginRedArea();
                GUI.Label(tRect, NWDConstants.K_APP_TABLE_RESET_WARNING, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw reset table
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_RESET_ZONE, NWDGUI.KTableSearchButton))
                {
                    tResetTable = true;
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw reset table
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_REINDEX_ZONE, NWDGUI.KTableSearchButton))
                {
                    IndexInMemoryAllObjects();
                    IndexInBaseAllObjects();
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw Clean table
                if (GUI.Button(tRect, "Clean local table", NWDGUI.KTableSearchButton))
                {
                    tCleanLocalTable = true;
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw Purge table
                //EditorGUI.BeginDisabledGroup(!ClassGameSaveDependent);
                //if (GUI.Button(tRect, "Purge accounts", NWDGUI.KTableSearchButton))
                //{
                //    tCleanLocalTableWithAccount = true;
                //}
                //EditorGUI.EndDisabledGroup();
                tRect.y += tRect.height + NWDGUI.kFieldMarge;

                // reintegrate all objects
                //NWDGUI.BeginRedArea();
                //if (GUI.Button(tRect, NWDConstants.K_APP_BASIS_CLASS_INTEGRITY_REEVALUE, NWDGUI.KTableSearchButton))
                //{
                //    GUI.FocusControl(null);
                //    RecalculateAllIntegrities();
                //}
                //NWDGUI.EndRedArea();
                tRect.y += tRect.height + NWDGUI.kFieldMarge;

                // Change Colmun
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = tRectTableLeft.y;
                // draw Replace WS in dev by sftp
                GUI.Label(tRect, NWDConstants.K_APP_WS_RESET_WARNING + " " + NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000"), NWDGUI.KTableSearchTitle);

                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, NWDConstants.K_APP_WS_PHP_DEV_TOOLS.Replace("XXXX", NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000")), NWDGUI.KTableSearchButton))
                {
                    tCreateAllPHPForOnlyThisClassDEV = true;
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                //// draw Replace WS in all by sftp
                //if (GUI.Button(tRect, NWDConstants.K_APP_WS_PHP_TOOLS.Replace("XXXX", NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000")), NWDGUI.KTableSearchButton))
                //{
                //    tCreateAllPHPForOnlyThisClass = true;
                //}
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw reintegrate the model
                //if (GUI.Button(tRect, NWDConstants.K_APP_WS_MODEL_TOOLS, NWDGUI.KTableSearchButton))
                //{
                //    tReintegrateOnlyThisClass = true;
                //}
                //tRect.y += tRect.height + NWDGUI.kFieldMarge;
                //// draw delete old model
                //if (GUI.Button(tRect, NWDConstants.K_APP_WS_DELETE_OLD_MODEL_TOOLS, NWDGUI.KTableSearchButton))
                //{
                //    tDeleteOldModelOnlyThisClass = true;
                //}
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                NWDGUI.EndRedArea();
                // Change Colmun
                tRect.x = tRectTableRight.x;
                tRect.y = tRectTableRight.y;
                GUIContent tDevContent = new GUIContent(NWDConstants.K_DEVELOPMENT_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().DevEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
                GUI.Label(tRect, tDevContent, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                EditorGUI.BeginDisabledGroup(WebModelChanged);
                NWDGUI.BeginRedArea();
                if (GUI.Button(tRect, "Clean table", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceClean(NWDAppConfiguration.SharedInstance().DevEnvironment);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Special", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().DevEnvironment, NWDOperationSpecial.Special);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Upgrade", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().DevEnvironment, NWDOperationSpecial.Upgrade);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Optimize", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().DevEnvironment, NWDOperationSpecial.Optimize);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Indexes", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().DevEnvironment, NWDOperationSpecial.Indexes);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                NWDGUI.EndRedArea();
                EditorGUI.EndDisabledGroup();
                // Change Colmun
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = tRectTableRight.y;
                GUIContent tPreprodContent = new GUIContent(NWDConstants.K_PREPRODUCTION_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().PreprodEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
                GUI.Label(tRect, tPreprodContent, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                EditorGUI.BeginDisabledGroup(WebModelChanged);
                NWDGUI.BeginRedArea();
                if (GUI.Button(tRect, "Clean table", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceClean(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Special", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().PreprodEnvironment, NWDOperationSpecial.Special);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Upgrade", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().PreprodEnvironment, NWDOperationSpecial.Upgrade);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Optimize", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().PreprodEnvironment, NWDOperationSpecial.Optimize);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Indexes", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().PreprodEnvironment, NWDOperationSpecial.Indexes);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                NWDGUI.EndRedArea();
                EditorGUI.EndDisabledGroup();
                // Change Colmun
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = tRectTableRight.y;
                GUIContent tProdContent = new GUIContent(NWDConstants.K_PRODUCTION_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().ProdEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
                GUI.Label(tRect, tProdContent, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                EditorGUI.BeginDisabledGroup(WebModelChanged);
                NWDGUI.BeginRedArea();
                if (GUI.Button(tRect, "Clean table", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    tSyncCleanProd = true;
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Special", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().ProdEnvironment, NWDOperationSpecial.Special);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Upgrade", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().ProdEnvironment, NWDOperationSpecial.Upgrade);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Optimize", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().ProdEnvironment, NWDOperationSpecial.Optimize);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Indexes", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && kAccountDependent == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().ProdEnvironment, NWDOperationSpecial.Indexes);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                NWDGUI.EndRedArea();
                EditorGUI.EndDisabledGroup();

            }
            // Page end!
            if (tDeleteSelection == true)
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
            }
            if (tTrashSelection == true)
            {
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
            }
            if (tUntrashSelection == true)
            {
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
            }
            if (tReintegrateSelection == true)
            {
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
            }
            if (tResetTable == true)
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_TABLE_RESET_ALERT,
                                                NWDConstants.K_APP_TABLE_RESET_TABLE,
                                                NWDConstants.K_APP_TABLE_RESET_YES,
                                                NWDConstants.K_APP_TABLE_RESET_NO))
                {
                    ResetTable();
                }
            }
            if (tPullProd == true)
            {
                if (Application.isPlaying == true && kAccountDependent == false)
                {
                    EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                }
                PullFromWebService(NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }
            if (tPullProdForce == true)
            {
                if (Application.isPlaying == true && kAccountDependent == false)
                {
                    EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                }
                PullFromWebServiceForce(NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }

            if (tSyncProd == true)
            {
                if (Application.isPlaying == true && kAccountDependent == false)
                {
                    EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                }
                SynchronizationFromWebService(NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }

            if (tSyncForceProd == true)
            {

                if (Application.isPlaying == true && kAccountDependent == false)
                {
                    EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                }
                SynchronizationFromWebServiceForce(NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }
            if (tSyncCleanProd == true)
            {

                if (Application.isPlaying == true && kAccountDependent == false)
                {
                    EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                }
                SynchronizationFromWebServiceClean(NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }
            if (tSyncSpecialProd == true)
            {

                if (Application.isPlaying == true && kAccountDependent == false)
                {
                    EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                }
                SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().ProdEnvironment, NWDOperationSpecial.Special);
            }
            if (tCleanLocalTable == true)
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_CLEAN_ALERT_TITLE,
                            NWDConstants.K_CLEAN_ALERT_MESSAGE,
                            NWDConstants.K_CLEAN_ALERT_OK,
                            NWDConstants.K_CLEAN_ALERT_CANCEL))
                {
                    CleanTable();
                }
            }
            //if (tCleanLocalTableWithAccount == true)
            //{
            //    if (EditorUtility.DisplayDialog(NWDConstants.K_PURGE_ALERT_TITLE,
            //                NWDConstants.K_PURGE_ALERT_MESSAGE,
            //                NWDConstants.K_PURGE_ALERT_OK,
            //                NWDConstants.K_PURGE_ALERT_CANCEL))
            //    {
            //        PurgeTable();
            //    }
            //}
            //if (tLocalizeLocalTable == true)
            //{
            //    ExportLocalization();
            //}
            //if (tCreateAllPHPForOnlyThisClass == true)
            //{
            //    ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
            //    NWDAppConfiguration.SharedInstance().DevEnvironment.CreatePHP(new List<Type> { ClassType }, false, false);
            //    NWDAppConfiguration.SharedInstance().PreprodEnvironment.CreatePHP(new List<Type> { ClassType }, false, false);
            //    NWDAppConfiguration.SharedInstance().ProdEnvironment.CreatePHP(new List<Type> { ClassType }, false, false);
            //    NWDEditorWindow.GenerateCSharpFile();
            //}
            //if (tReintegrateOnlyThisClass == true)
            //{
            //    ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
            //    NWDEditorWindow.GenerateCSharpFile();
            //    //NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            //}
            //if (tDeleteOldModelOnlyThisClass == true)
            //{
            //    DeleteOldsModels();
            //    ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
            //    NWDEditorWindow.GenerateCSharpFile();
            //    //NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            //}
            if (tCreateAllPHPForOnlyThisClassDEV == true)
            {
                ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
                NWDAppConfiguration.SharedInstance().DevEnvironment.CreatePHP(new List<Type> { ClassType }, false, false);
                NWDEditorWindow.GenerateCSharpFile();
            }
            //NWEBenchmark.Finish();
            return rRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        //TODO : RENAME!
        public void DrawTableEditor(EditorWindow sEditorWindow)
        {
            //NWEBenchmark.Start();
            bool rLoaded = AllDatabaseIsLoaded();

            EditorGUI.BeginDisabledGroup(!rLoaded);

            Rect tWindowRect = new Rect(sEditorWindow.position.x, sEditorWindow.position.y, sEditorWindow.position.width, sEditorWindow.position.height);
            if (tWindowRect.width < NWDGUI.KTableMinWidth)
            {
                tWindowRect.width = NWDGUI.KTableMinWidth;
            }
            Rect tRect = new Rect(0, 0, tWindowRect.width, 0);
            // offset the tab bar 
            tRect.y += NWDGUI.KTAB_BAR_HEIGHT + NWDGUI.kFieldMarge;

            float tWidthTiers = Mathf.Floor(sEditorWindow.position.width / 3.0F);
            Rect tRectToogle = new Rect(NWDGUI.kFieldMarge, tRect.y, tWidthTiers, NWDGUI.kToggleStyle.fixedHeight);

            bool tSearchActions = GUI.Toggle(tRectToogle, SearchActions, "Search");
            if (tSearchActions != SearchActions)
            {
                SearchActions = tSearchActions;
                SaveEditorPrefererences();
            }
            tRectToogle.x += tWidthTiers;
            bool tRowActions = GUI.Toggle(tRectToogle, RowActions, "Actions");
            if (tRowActions != RowActions)
            {
                RowActions = tRowActions;
                SaveEditorPrefererences();
            }
            tRectToogle.x += tWidthTiers;
            bool tTableActions = GUI.Toggle(tRectToogle, TableActions, "Table");
            if (tTableActions != TableActions)
            {
                TableActions = tTableActions;
                SaveEditorPrefererences();
            }
            tRect.y += NWDGUI.kToggleStyle.fixedHeight + NWDGUI.kFieldMarge;

            tRect.y += NWDGUI.Line(tRect).height;

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
            if (SearchActions == true)
            {
                if (tMargeNeed == false)
                {
                    tRect.y += NWDGUI.kFieldMarge;
                }
                tMargeNeed = true;
            }

            Rect tRectForTop = DrawTableEditorTop(tRect);
            tRect.y += tRectForTop.height;

            //tRect.y += NWDGUI.Line(tRect).height;

            Rect tRectForBottom = DrawTableEditorBottom(new Rect(0, 0, tWindowRect.width, tWindowRect.height));
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



            float tScrollRectHeight = tWindowRect.height - tRect.y - tPageSizeHeight * 2 - tRectForBottom.height - NWDGUI.kTableHeaderHeight * 2;

            Rect tScrollRect = new Rect(0, tRect.y + NWDGUI.kTableHeaderHeight + tPageSizeHeight, tWindowRect.width, tScrollRectHeight);
            if (tPageToolbar == true)
            {
                tScrollRect.height -= NWDGUI.kFieldMarge;
                //tScrollHeight -= NWDGUI.kFieldMarge;
            }


            // ===========================================
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
                //if (NWDLauncher.GetState() == NWDStatut.DataAccountCodePinStop)
                //{
                //    tDialogRect.height -= NWDGUI.kFieldMarge;
                //    Rect tDialogRectBox = NWDGUI.UnMargeAll(tDialogRect);
                //    GUI.Label(tDialogRectBox, "", EditorStyles.helpBox);
                //    tDialogRect.height += NWDGUI.kTextFieldStyle.fixedHeight;
                //    tDialogRect.y += NWDGUI.WarningBox(tDialogRect, NWDLauncher.GetState().ToString()).height + NWDGUI.kFieldMarge;
                //    tDialogRect.height = NWDGUI.kTextFieldStyle.fixedHeight;
                //    tDialogRect.y += NWDGUI.ErrorBox(tDialogRect, "TOO MUCH TENTATIVE").height + NWDGUI.kFieldMarge;
                //}
                //else if (NWDLauncher.GetState() == NWDStatut.DataAccountCodePinRequest || NWDLauncher.GetState() == NWDStatut.DataAccountCodePinFail)
                //{
                //    tDialogRect.height -= NWDGUI.kFieldMarge;
                //    Rect tDialogRectBox = NWDGUI.UnMargeAll(tDialogRect);
                //    GUI.Label(tDialogRectBox, "", EditorStyles.helpBox);
                //    tDialogRect.height += NWDGUI.kTextFieldStyle.fixedHeight;
                //    tDialogRect.y += NWDGUI.WarningBox(tDialogRect, NWDLauncher.GetState().ToString()).height + NWDGUI.kFieldMarge;

                //    //tDialogRect.height = NWDGUI.kTextFieldStyle.fixedHeight;
                //    //NWDLauncher.CodePinValue = EditorGUI.PasswordField(tDialogRect, "CodePin", NWDLauncher.CodePinValue, NWDGUI.kTextFieldStyle);
                //    //tDialogRect.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

                //    tDialogRect.height = NWDGUI.kMiniButtonStyle.fixedHeight;
                //    if (GUI.Button(tDialogRect, "Valid", NWDGUI.kMiniButtonStyle))
                //    {
                //        //Debug.LogWarning("Try this code");
                //        //NWDLauncher.DatabaseAccountConnection(NWDLauncher.CodePinValue);
                //    }
                //    tDialogRect.y += (NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge);
                //    tDialogRect.y += NWDGUI.Separator(tDialogRect).height;
                //    NWDGUI.BeginRedArea();
                //    if (GUI.Button(tDialogRect, "DELETE ACCOUNT BASE", NWDGUI.kMiniButtonStyle))
                //    {
                //        Debug.LogWarning("Delete account base?!");
                //        if (EditorUtility.DisplayDialog("DELETE ACCOUNT DATABASE", "YOU WILL DELETE ACCOUNT DATABASE! ARE YOU SURE?", "DELETE!", "CANCEL"))
                //        {
                //            //NWDDataManager.SharedInstance().DeleteDatabaseAccount();
                //            NWDLauncher.Launch();
                //        }
                //        GUI.FocusControl(null);
                //        GUIUtility.ExitGUI();
                //    }
                //    tDialogRect.y += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                //    NWDGUI.EndRedArea();

                //}
                //else if (NWDLauncher.GetState() == NWDStatut.DataAccountCodePinCreate)
                //{
                //    tDialogRect.height += NWDGUI.kFieldMarge;
                //    Rect tDialogRectBox = NWDGUI.UnMargeAll(tDialogRect);
                //    GUI.Label(tDialogRectBox, "", EditorStyles.helpBox);
                //    tDialogRect.height += NWDGUI.kTextFieldStyle.fixedHeight;
                //    tDialogRect.y += NWDGUI.WarningBox(tDialogRect, NWDLauncher.GetState().ToString()).height + NWDGUI.kFieldMarge;

                //    tDialogRect.height = NWDGUI.kTextFieldStyle.fixedHeight;
                //    NWDLauncher.CodePinValue = EditorGUI.PasswordField(tDialogRect, "CodePin", NWDLauncher.CodePinValue, NWDGUI.kTextFieldStyle);
                //    tDialogRect.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                //    NWDLauncher.CodePinValueConfirm = EditorGUI.PasswordField(tDialogRect, "CodePin confirm", NWDLauncher.CodePinValueConfirm, NWDGUI.kTextFieldStyle);
                //    tDialogRect.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                //    NWDGUI.BeginRedArea();
                //    bool tValid = true;
                //    if (string.IsNullOrEmpty(NWDLauncher.CodePinValue))
                //    {
                //        NWDLauncher.CodePinValue = string.Empty;
                //    }
                //    if (string.IsNullOrEmpty(NWDLauncher.CodePinValueConfirm))
                //    {
                //        NWDLauncher.CodePinValueConfirm = string.Empty;
                //    }
                //    if (NWDLauncher.CodePinValue != NWDLauncher.CodePinValueConfirm)
                //    {
                //        tValid = false;
                //    }
                //    //if (NWDLauncher.CodePinValue.Length < NWDAppConfiguration.SharedInstance().PinCodeLenghtMin || NWDLauncher.CodePinValue.Length > NWDAppConfiguration.SharedInstance().PinCodeLenghtMax)
                //    //{
                //    //    tValid = false;
                //    //}
                //    if (string.IsNullOrEmpty(NWDLauncher.CodePinValue))
                //    {
                //        tValid = false;
                //    }
                //    if (NWDLauncher.CodePinValue.Length < 4)
                //    {
                //        tValid = false;
                //    }
                //    if (tValid == true)
                //    {
                //        EditorGUI.LabelField(tDialogRect, "valid CodePin");
                //    }
                //    else
                //    {
                //        EditorGUI.LabelField(tDialogRect, "invalid CodePin");
                //    }
                //    tDialogRect.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                //    EditorGUI.BeginDisabledGroup(!tValid);
                //    if (GUI.Button(tDialogRect, "CREATE ACCOUNT BASE", NWDGUI.kMiniButtonStyle))
                //    {
                //        Debug.LogWarning("Create account base?!");
                //        GUI.FocusControl(null);
                //        //NWDLauncher.DatabaseAccountConnection(NWDLauncher.CodePinValue);
                //    }
                //    tDialogRect.y += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                //    EditorGUI.EndDisabledGroup();
                //    NWDGUI.EndRedArea();
                //}
                //else

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
                DrawPagesTab(tRect);
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


                Rect tScrollHeader = new Rect(tScrollRect.x, tScrollRect.y - NWDGUI.kTableHeaderHeight, tWindowRect.width, NWDGUI.kTableHeaderHeight);
                Rect tScrollHeaderBottom = new Rect(tScrollRect.x, tScrollRect.y + tScrollRect.height, tWindowRect.width, NWDGUI.kTableHeaderHeight);

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
                    tScrollRect,
                    m_ScrollPositionList,
                    tScrollContentRect,
                    false,
                    true
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
                    NWDTypeClass tSelected = NWDDataInspector.ObjectInEdition();
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
                    NWDTypeClass tSelected = NWDDataInspector.ObjectInEdition();
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
                    NWDTypeClass tSelected = NWDDataInspector.ObjectInEdition();
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

                Rect tRectRow = new Rect(sEditorWindow.position.x, sEditorWindow.position.y,
                     sEditorWindow.position.width, sEditorWindow.position.height);

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
                DrawPagesTab(tRect);
                //tRect.y += NWDGUI.Line(tRect).height;
                // ===========================================

                EditorGUI.EndDisabledGroup();
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
