//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDTypeClass
    {
        protected Texture2D ImageDisk = NWDConstants.kImageDiskUnknow;
        protected Texture2D ImageSync = NWDConstants.kImageSyncGeneralWaiting;
        public int AnalyzeSync = 0;
        protected Texture2D ImageDevSync = NWDConstants.kImageSyncRequired;
        public int AnalyzeDevSync = 0;
        protected Texture2D ImagePreprodSync = NWDConstants.kImageSyncRequired;
        public int AnalyzePreprodSync = 0;
        protected Texture2D ImageProdSync = NWDConstants.kImageSyncRequired;
        public int AnalyzeProdSync = 0;
        public int AnalyzePrefab = 0;
        protected bool TestIntegrityResult = true;
        protected bool TestWebserviceVersionIsValid = true;
        protected string StringRow = string.Empty;
        public string StateInfos = string.Empty;
        public int AnalyzeStateInfos = 0;
        public int AnalyzeModel = 0;
        public string ModelInfos = string.Empty;
        //protected string ChecklistInfos = string.Empty;
        protected Texture2D ImageChecklist = NWDConstants.kImageSyncWaiting;
        public int AnalyzeChecklist = 0;
        public int AnalyzeID = 0;
        public bool AnalyzeSelected = false;
        //GUIStyle tStyleBox = NWDConstants.KTableRowNormal;
        protected Color tBoxColor = Color.clear;
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        const int KSyncRequired = -1;
        const int KSyncWaiting = -2;
        const int KSyncSuccessed = 0;
        const int KSyncForbidden = 1;
        const int KSyncDanger = 2;
        const int KSyncForward = 3;

        const int KChecklistValid = 1;
        const int KChecklistWorkInProgress = 2;
        const int KChecklistWarning = 3;

        const int KAnalyzeStateEnable = 0;
        const int KAnalyzeStateDisable = 1;
        const int KAnalyzeStateTrashed = 2;
        const int KAnalyzeStateWarning = 3;
        const int KAnalyzeStateModelError = 8;
        const int KAnalyzeStateCorrupted = 9;

        //-------------------------------------------------------------------------------------------------------------
        public static void DrawHeaderInEditor(Rect sRect, Rect sScrollRect, float sZoom)
        {
            EditorGUI.DrawRect(sRect, NWDConstants.kTabHeaderColor);
            //EditorGUI.DrawRect(sRect, Color.yellow);
            //Rect ColorRect = new Rect(sRect.x + 2, sRect.y + 2, sRect.width - 4, sRect.height - 4);
            //EditorGUI.DrawRect(ColorRect, Color.red);
            sRect.x += NWDConstants.kFieldMarge;
            sRect.width = sRect.width - NWDConstants.kScrollbar;
            //EditorGUI.DrawRect(sRect, Color.blue);
            Rect tRect = new Rect(sRect.x, sRect.y + NWDConstants.kFieldMarge, NWDConstants.kSelectWidth, sRect.height - NWDConstants.kFieldMarge * 2);
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_SELECT, NWDConstants.KTableHeaderSelect))
            {
                //Debug.Log("sort by selected toggle");
                // Update select in real time
                foreach (NWDTypeClass tData in BasisHelper().Datas)
                {
                    tData.AnalyzeSelected = BasisHelper().EditorTableDatasSelected[tData];
                }
                // toogle sort
                if (BasisHelper().SortType == NWDBasisEditorDatasSortType.BySelectAscendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.BySelectDescendant;
                }
                else if (BasisHelper().SortType == NWDBasisEditorDatasSortType.BySelectDescendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.BySelectAscendant;
                }
                else
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.BySelectDescendant;
                }
                BasisHelper().SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            tRect.width = NWDConstants.kIDWidth;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_ID, NWDConstants.KTableHeaderId))
            {
                //Debug.Log("sort by ID toggle");
                if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByIDAscendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByIDDescendant;
                }
                else if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByIDDescendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByIDAscendant;
                }
                else
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByIDDescendant;
                }
                BasisHelper().SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            tRect.width = NWDConstants.kPrefabWidth * sZoom;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_PREFAB, NWDConstants.KTableHeaderPrefab))
            {
                //Debug.Log("sort by prefab toggle");
                if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByPrefabAscendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByPrefabDescendant;
                }
                else if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByPrefabDescendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByPrefabAscendant;
                }
                else
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByPrefabDescendant;
                }
                BasisHelper().SortEditorTableDatas();
            }
            tRect.x += tRect.width;
            tRect.width = sRect.width
                - NWDConstants.kFieldMarge
                - NWDConstants.kIDWidth
                - NWDConstants.kSelectWidth
                - NWDConstants.kPrefabWidth * sZoom
                - NWDConstants.KTableSearchWidth
                - NWDConstants.KTableReferenceWidth
                - NWDConstants.KTableRowWebModelWidth
                - NWDConstants.KTableIconWidth * 6;

            if (tRect.width < NWDConstants.KTableSearchWidth)
            {
                tRect.width = NWDConstants.KTableSearchWidth;
            }
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_DESCRIPTION, NWDConstants.KTableHeaderInformations))
            {
                //Debug.Log("sort by internal key toggle");
                if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByInternalKeyAscendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByInternalKeyDescendant;
                }
                else if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByInternalKeyDescendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByInternalKeyAscendant;
                }
                else
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByInternalKeyDescendant;
                }
                BasisHelper().SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            tRect.width = NWDConstants.KTableRowWebModelWidth;
            if (GUI.Button(tRect, "webservice", NWDConstants.KTableHeaderIcon))
            {
                //Debug.Log("sort by disk webservice");
                if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByModelAscendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByModelDescendant;
                }
                else if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByModelDescendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByModelAscendant;
                }
                else
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByModelDescendant;
                }
                BasisHelper().SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            tRect.width = NWDConstants.KTableIconWidth;
            if (GUI.Button(tRect, "Check", NWDConstants.KTableHeaderIcon))
            {
                //Debug.Log("sort by disk checklist");
                if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByChecklistAscendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByChecklistDescendant;
                }
                else if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByChecklistDescendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByChecklistAscendant;
                }
                else
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByChecklistDescendant;
                }
                BasisHelper().SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            tRect.width = NWDConstants.KTableIconWidth;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_DISK, NWDConstants.KTableHeaderIcon))
            {
                //Debug.Log("sort by disk toggle ???");
            }
            tRect.x += tRect.width;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_SYNCHRO, NWDConstants.KTableHeaderIcon))
            {
                //Debug.Log("sort by synchro toggle");
                if (BasisHelper().SortType == NWDBasisEditorDatasSortType.BySyncAscendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.BySyncDescendant;
                }
                else if (BasisHelper().SortType == NWDBasisEditorDatasSortType.BySyncDescendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.BySyncAscendant;
                }
                else
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.BySyncDescendant;
                }
                BasisHelper().SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_DEVSYNCHRO, NWDConstants.KTableHeaderIcon))
            {
                //Debug.Log("sort by dev sync toggle");
                if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByDevSyncAscendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByDevSyncDescendant;
                }
                else if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByDevSyncDescendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByDevSyncAscendant;
                }
                else
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByDevSyncDescendant;
                }
                BasisHelper().SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_PREPRODSYNCHRO, NWDConstants.KTableHeaderIcon))
            {
                //Debug.Log("sort by preprod sync toggle");
                if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByPreprodSyncAscendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByPreprodSyncDescendant;
                }
                else if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByPreprodSyncDescendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByPreprodSyncAscendant;
                }
                else
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByPreprodSyncDescendant;
                }
                BasisHelper().SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_PRODSYNCHRO, NWDConstants.KTableHeaderIcon))
            {
                //Debug.Log("sort by prod sync toggle");
                if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByProdSyncAscendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByProdSyncDescendant;
                }
                else if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByProdSyncDescendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByProdSyncAscendant;
                }
                else
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByProdSyncDescendant;
                }
                BasisHelper().SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            tRect.width = NWDConstants.KTableSearchWidth;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_STATUT, NWDConstants.KTableHeaderStatut))
            {
                //Debug.Log("sort by statut toggle");
                if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByStatutAscendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByStatutDescendant;
                }
                else if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByStatutDescendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByStatutAscendant;
                }
                else
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByStatutDescendant;
                }
                BasisHelper().SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
            tRect.x += tRect.width;
            tRect.width = NWDConstants.KTableReferenceWidth;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_HEADER_REFERENCE + " ", NWDConstants.KTableHeaderReference))
            {
                //Debug.Log("sort by reference toggle");
                if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByReferenceAscendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByReferenceDescendant;
                }
                else if (BasisHelper().SortType == NWDBasisEditorDatasSortType.ByReferenceDescendant)
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByReferenceAscendant;
                }
                else
                {
                    BasisHelper().SortType = NWDBasisEditorDatasSortType.ByReferenceDescendant;
                }
                BasisHelper().SortEditorTableDatas();
                ChangeScroolPositionToSelection(sScrollRect);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DrawHeaderBottomInEditor(Rect sRect, Rect sScrollRect)
        {
            EditorGUI.DrawRect(sRect, NWDConstants.kTabHeaderColor);
            sRect.width = sRect.width - NWDConstants.kScrollbar;
            Rect tRect = new Rect(sRect.x + NWDConstants.kFieldMarge,
                 sRect.y + NWDConstants.kFieldMarge,
                 NWDConstants.KTableSearchWidth,
                 NWDConstants.KTableSearchToggle.fixedHeight);
            // TODO MOVE THIS CAC
            int tSelectionCount = 0;
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
            {
                if (tKeyValue.Value == true)
                {
                    tSelectionCount++;
                }
            }
            int t_ItemPerPageSelection = EditorGUI.Popup(tRect, BasisHelper().m_ItemPerPageSelection, BasisHelper().m_ItemPerPageOptions, NWDConstants.KTableSearchEnum);
            if (t_ItemPerPageSelection != BasisHelper().m_ItemPerPageSelection)
            {
                BasisHelper().m_PageSelected = 0;
            }
            BasisHelper().m_ItemPerPageSelection = t_ItemPerPageSelection;
            tRect.x += NWDConstants.KTableSearchWidth + NWDConstants.kFieldMarge;
            float tRowZoom = EditorGUI.Slider(tRect, BasisHelper().RowZoom, 1.0F, 2.0F);
            if (System.Math.Abs(tRowZoom - BasisHelper().RowZoom) > 0.01F)
            {
                BasisHelper().RowZoom = tRowZoom;
                ChangeScroolPositionToSelection(sScrollRect);
                //GUIUtility.ExitGUI();
            }
            tRect.x += NWDConstants.KTableSearchWidth + NWDConstants.kFieldMarge;
            // draw toogle enable
            bool t_ShowEnableLine = EditorGUI.ToggleLeft(tRect, NWDConstants.K_APP_TABLE_SHOW_ENABLE_DATAS, BasisHelper().m_ShowEnable);
            if (BasisHelper().m_ShowEnable != t_ShowEnableLine)
            {
                BasisHelper().m_ShowEnable = t_ShowEnableLine;
                FilterTableEditor();
            }
            tRect.x += NWDConstants.KTableSearchWidth + NWDConstants.kFieldMarge;
            // draw toogle disable
            bool t_ShowDisableLine = EditorGUI.ToggleLeft(tRect, NWDConstants.K_APP_TABLE_SHOW_DISABLE_DATAS, BasisHelper().m_ShowDisable);
            if (BasisHelper().m_ShowDisable != t_ShowDisableLine)
            {
                BasisHelper().m_ShowDisable = t_ShowDisableLine;
                FilterTableEditor();
            }
            tRect.x += NWDConstants.KTableSearchWidth + NWDConstants.kFieldMarge;
            // draw toogle trashed
            EditorGUI.BeginDisabledGroup(!BasisHelper().m_ShowDisable);
            bool t_ShowTrashedLine = EditorGUI.ToggleLeft(tRect, NWDConstants.K_APP_TABLE_SHOW_TRASHED_DATAS, BasisHelper().m_ShowTrashed);
            if (BasisHelper().m_ShowTrashed != t_ShowTrashedLine)
            {
                BasisHelper().m_ShowTrashed = t_ShowTrashedLine;
                FilterTableEditor();
            }
            tRect.x += NWDConstants.KTableSearchWidth + NWDConstants.kFieldMarge;
            // draw toogle corrupted
            EditorGUI.EndDisabledGroup();
            bool t_ShowIntegrityErrorLine = EditorGUI.ToggleLeft(tRect, NWDConstants.K_APP_TABLE_SHOW_INTEGRITY_ERROR_DATAS, BasisHelper().m_ShowIntegrityError);
            if (BasisHelper().m_ShowIntegrityError != t_ShowIntegrityErrorLine)
            {
                BasisHelper().m_ShowIntegrityError = t_ShowIntegrityErrorLine;
                FilterTableEditor();
            }
            bool tShowMoreInfos = false;
            if (tShowMoreInfos)
            {
                // draw objects in database
                int tRealReference = BasisHelper().Datas.Count;
                if (tRealReference == 0)
                {
                    GUI.Label(tRect, NWDConstants.K_APP_TABLE_NO_OBJECT, NWDConstants.KTableSearchLabel);
                }
                else if (tRealReference == 1)
                {
                    GUI.Label(tRect, NWDConstants.K_APP_TABLE_ONE_OBJECT, NWDConstants.KTableSearchLabel);
                }
                else
                {
                    GUI.Label(tRect, tRealReference + NWDConstants.K_APP_TABLE_X_OBJECTS, NWDConstants.KTableSearchLabel);
                }
                tRect.x += NWDConstants.KTableSearchWidth + NWDConstants.kFieldMarge;
                // draw objects in results
                int tResultReference = BasisHelper().EditorTableDatas.Count;
                if (tResultReference == 0)
                {
                    GUI.Label(tRect, NWDConstants.K_APP_TABLE_NO_OBJECT_FILTERED, NWDConstants.KTableSearchLabel);
                }
                else if (tResultReference == 1)
                {
                    GUI.Label(tRect, NWDConstants.K_APP_TABLE_ONE_OBJECT_FILTERED, NWDConstants.KTableSearchLabel);
                }
                else
                {
                    GUI.Label(tRect, tResultReference + NWDConstants.K_APP_TABLE_X_OBJECTS_FILTERED, NWDConstants.KTableSearchLabel);
                }
                tRect.x += NWDConstants.KTableSearchWidth + NWDConstants.kFieldMarge;
                // draw selection
                if (tSelectionCount == 0)
                {
                    GUI.Label(tRect, NWDConstants.K_APP_TABLE_NO_SELECTED_OBJECT, NWDConstants.KTableSearchLabel);
                }
                else if (tSelectionCount == 1)
                {
                    GUI.Label(tRect, NWDConstants.K_APP_TABLE_ONE_SELECTED_OBJECT, NWDConstants.KTableSearchLabel);
                }
                else
                {
                    GUI.Label(tRect, tSelectionCount + NWDConstants.K_APP_TABLE_XX_SELECTED_OBJECT, NWDConstants.KTableSearchLabel);
                }
                tRect.x += NWDConstants.KTableSearchWidth + NWDConstants.kFieldMarge;
            }
            tRect.x = sRect.width - NWDConstants.KTableSearchWidth;
            if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_ADD_ROW, NWDConstants.KTableSearchButton))
            {
                K tNewObject = NWDBasis<K>.NewData();
                if (BasisHelper().m_SearchTag != NWDBasisTag.NoTag)
                {
                    tNewObject.Tag = BasisHelper().m_SearchTag;
                    tNewObject.UpdateData();
                }
                BasisHelper().m_PageSelected = BasisHelper().m_MaxPage * 3;
                SetObjectInEdition(tNewObject);
                NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RowAnalyze()
        {
            //Debug.Log("RowAnalyze");
            AnalyzeID = ID;
            AnalyzeModel = WebModel;
            TestIntegrityResult = TestIntegrity();
            TestWebserviceVersionIsValid = WebserviceVersionIsValid();
            AnalyzePrefab = 1;
            if (string.IsNullOrEmpty(Preview))
            {
                AnalyzePrefab = 0;
            }
            string tIsInError = string.Empty;
            //IsInErrorCheck();
            if (InError == true)
            {
                tIsInError = NWDConstants.K_WARNING;
            }
            StringRow = "<size=13><b>" + InternalKey + "</b></size>     <i>(" + InternalDescription + ")</i> ";
            ModelInfos = "[" + WebModel.ToString() + "/" + WebModelToUse() + "/" + NWDAppConfiguration.SharedInstance().WebBuild + "]";
            // verif if object is in error
            if (FromDatabase == true)
            {
                ImageDisk = NWDConstants.kImageDiskDatabase;
            }
            switch (WritingPending)
            {
                case NWDWritingPending.Unknow:
                    ImageDisk = NWDConstants.kImageDiskUnknow;
                    break;
                case NWDWritingPending.UpdateInMemory:
                    ImageDisk = NWDConstants.kImageDiskUpdate;
                    break;
                case NWDWritingPending.InsertInMemory:
                    ImageDisk = NWDConstants.kImageDiskInsert;
                    break;
                case NWDWritingPending.DeleteInMemory:
                    ImageDisk = NWDConstants.kImageDiskDelete;
                    break;
                case NWDWritingPending.InDatabase:
                    ImageDisk = NWDConstants.kImageDiskDatabase;
                    break;
            }
            bool tDisableProd = false;
            if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains(ClassType()))
            {
                tDisableProd = true;
            }
            if (AccountDependent() == true)
            {
                tDisableProd = true;
            }

            if (DevSync < 0 && PreprodSync < 0 && (ProdSync < 0 || tDisableProd == true))
            {
                ImageSync = NWDConstants.kImageSyncGeneralForbidden;
            }
            else
            {
                if (DS > 0)
                {
                    if (DevSync > 1 && PreprodSync < 1 && ProdSync < 1 && DS == DevSync)
                    {
                        ImageSync = NWDConstants.kImageSyncGeneralSuccessed;
                        AnalyzeSync = KSyncSuccessed;
                    }
                    else if (DevSync > 1 && PreprodSync > 1 && ProdSync < 1 && (DS == DevSync || DS == PreprodSync))
                    {
                        ImageSync = NWDConstants.kImageSyncGeneralSuccessed;
                        AnalyzeSync = KSyncSuccessed;
                    }
                    else if (DS < DM)
                    {
                        ImageSync = NWDConstants.kImageSyncGeneralForward;
                        AnalyzeSync = KSyncForward;
                    }
                    else
                    {
                        ImageSync = NWDConstants.kImageSyncGeneralWaiting;
                        AnalyzeSync = KSyncWaiting;
                    }
                }
            }
            if (DevSync == 0)
            {
                ImageDevSync = NWDConstants.kImageSyncRequired;
                AnalyzeDevSync = KSyncRequired;
            }
            else if (DevSync == 1)
            {
                ImageDevSync = NWDConstants.kImageSyncWaiting;
                AnalyzeDevSync = KSyncWaiting;
            }
            else if (DevSync > 1)
            {
                ImageDevSync = NWDConstants.kImageSyncSuccessed;
                AnalyzeDevSync = KSyncSuccessed;
            }
            else if (DevSync == -1)
            {
                ImageDevSync = NWDConstants.kImageSyncForbidden;
                AnalyzeDevSync = KSyncForbidden;
            }
            else if (DevSync < -1)
            {
                ImageDevSync = NWDConstants.kImageSyncDanger;
                AnalyzeDevSync = KSyncDanger;
            }

            if (PreprodSync == 0)
            {
                ImagePreprodSync = NWDConstants.kImageSyncRequired;
                AnalyzePreprodSync = KSyncRequired;
            }
            else if (PreprodSync == 1)
            {
                ImagePreprodSync = NWDConstants.kImageSyncWaiting;
                AnalyzePreprodSync = KSyncWaiting;
            }
            else if (PreprodSync > 1)
            {
                if (PreprodSync > DevSync)
                {
                    ImagePreprodSync = NWDConstants.kImageSyncSuccessed;
                    AnalyzePreprodSync = KSyncSuccessed;
                }
                else
                {
                    ImagePreprodSync = NWDConstants.kImageSyncForward;
                    AnalyzePreprodSync = KSyncForward;
                }
            }
            else if (PreprodSync == -1)
            {
                ImagePreprodSync = NWDConstants.kImageSyncForbidden;
                AnalyzePreprodSync = KSyncForbidden;
            }
            else if (PreprodSync < -1)
            {
                ImagePreprodSync = NWDConstants.kImageSyncDanger;
                AnalyzePreprodSync = KSyncDanger;
            }
            if (tDisableProd == true)
            {
                ImageProdSync = NWDConstants.kImageSyncForbidden;
                AnalyzeProdSync = KSyncForbidden;
            }
            else
            {
                if (ProdSync == 0)
                {
                    ImageProdSync = NWDConstants.kImageSyncRequired;
                    AnalyzeProdSync = KSyncRequired;
                }
                else if (ProdSync == 1)
                {
                    ImageProdSync = NWDConstants.kImageSyncWaiting;
                    AnalyzeProdSync = KSyncWaiting;
                }
                if (ProdSync > 1)
                {
                    if (ProdSync > DevSync && ProdSync > PreprodSync)
                    {
                        ImageProdSync = NWDConstants.kImageSyncSuccessed;
                        AnalyzeProdSync = KSyncSuccessed;
                    }
                    else
                    {
                        ImageProdSync = NWDConstants.kImageSyncForward;
                        AnalyzeProdSync = KSyncForward;
                    }
                }
                else if (ProdSync == -1)
                {
                    ImageProdSync = NWDConstants.kImageSyncForbidden;
                    AnalyzeProdSync = KSyncForbidden;
                }
                else if (ProdSync < -1)
                {
                    ImageProdSync = NWDConstants.kImageSyncDanger;
                    AnalyzeProdSync = KSyncDanger;
                }
            }
            if (BasisHelper().kAccountDependent == false)
            {
                if (CheckList == null)
                {
                    CheckList = new NWDBasisCheckList();
                }
                if (CheckList.Value != 0)
                {
                    //ChecklistInfos = "<color=orange>[WIP]</color> ";
                    ImageChecklist = NWDConstants.kImageCheckWorkInProgress;
                    AnalyzeChecklist = KChecklistWorkInProgress;
                }
                else
                {
                    //ChecklistInfos = "<color=green>[√]</color> ";
                    ImageChecklist = NWDConstants.kImageCheckValid;
                    AnalyzeChecklist = KChecklistValid;
                }
            }
            else
            {
                ImageChecklist = null;
            }
            StateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_OK;
            AnalyzeStateInfos = KAnalyzeStateEnable;
            tBoxColor = Color.clear;
            if (TestWebserviceVersionIsValid)
            {
                if (TestIntegrityResult == false)
                {
                    tBoxColor = NWDConstants.kRowColorError;
                    StateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_INTEGRITY_ERROR;
                    StringRow = "<color=#a52a2aff>" + StringRow + "</color>";
                    AnalyzeStateInfos = KAnalyzeStateCorrupted;
                    ImageChecklist = NWDConstants.kImageCheckWarning;
                }
                else if (XX > 0)
                {
                    tBoxColor = NWDConstants.kRowColorTrash;
                    StateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_TRASH;
                    StringRow = "<color=#444444ff>" + StringRow + "</color>";
                    AnalyzeStateInfos = KAnalyzeStateTrashed;
                    ImageChecklist = null;
                }
                else
                {
                    if (AC == false)
                    {
                        tBoxColor = NWDConstants.kRowColorDisactive;
                        StateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_DISACTIVE;
                        StringRow = "<color=#555555ff>" + StringRow + "</color>";
                        AnalyzeStateInfos = KAnalyzeStateDisable;
                        ImageChecklist = null;
                    }
                    else
                    {
                        StateInfos = "<color=red>" + tIsInError + "</color>";
                    }
                }
            }
            else
            {
                tBoxColor = NWDConstants.kRowColorWarning;
                StateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_WEBSERVICE_ERROR;
                StringRow = "<color=#cc6600ff>" + StringRow + "</color>";
                AnalyzeStateInfos = KAnalyzeStateModelError;
            }
            if (InError == true)
            {
                ImageChecklist = NWDConstants.kImageCheckWarning;
                AnalyzeChecklist = KChecklistWarning;
                AnalyzeStateInfos = KAnalyzeStateWarning;
                ImageChecklist = NWDConstants.kImageCheckWarning;
            }
            StringRow = StringRow.Replace("()", string.Empty);
            while (StringRow.Contains("  "))
            {
                StringRow = StringRow.Replace("  ", " ");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        //public Rect DrawRowInEditorLayout(Vector2 sMouseClickPosition, EditorWindow sEditorWindow, bool sSelectAndClick, int sRow)
        //{
        //    float tWidthUsed = sEditorWindow.position.width - 20;
        //    //GUILayout.Space(NWDConstants.kRowHeightSpace);
        //    GUILayout.BeginHorizontal(/*tStyleBox*/);
        //    // Toogle
        //    GUILayout.Label(string.Empty, NWDConstants.KTableHeaderSelect);
        //    //GUILayout.Label(string.Empty, GUILayout.Width(NWDConstants.kOriginWidth));
        //    Rect tRect = GUILayoutUtility.GetLastRect();
        //    // determine rect to select and draw
        //    Rect rRect = new Rect(tRect.x, tRect.y - 5, tWidthUsed, NWDConstants.kRowHeight);
        //    // draw color rect
        //    Rect rRectColored = new Rect(tRect.x - 5, tRect.y - 5, tWidthUsed + 1024, NWDConstants.kRowHeight + 5);
        //    EditorGUI.DrawRect(rRectColored, tBoxColor);
        //    if (IsObjectInEdition(this) == true)
        //    {
        //        EditorGUI.DrawRect(rRectColored, NWDConstants.kRowColorSelected);
        //    }
        //    // determine rect to analyze
        //    Rect rRectAnalyze = new Rect(tRect.x - 10, tRect.y - 5, tWidthUsed, NWDConstants.kRowHeight + 10);
        //    // check click in rect
        //    if (rRectAnalyze.Contains(sMouseClickPosition))
        //    {
        //        //NWDDataManager.SharedInstance().UpdateQueueExecute(); // update execute in another place!? in NWDDataManager.SharedInstance() destroyed?
        //        GUI.FocusControl(null);
        //        SetObjectInEdition(this);
        //        if (sSelectAndClick == true)
        //        {
        //            if (XX == 0 && TestIntegrity())
        //            {
        //                //Datas().DatasInEditorSelectionList[tIndex] = !Datas().DatasInEditorSelectionList [tIndex];
        //                BasisHelper().EditorTableDatasSelected[this] = !BasisHelper().EditorTableDatasSelected[this];
        //                Event.current.Use();
        //            }
        //        }
        //        sEditorWindow.Focus();
        //    }
        //    string tStringReference = "[" + Reference + "]";
        //    // WebserviceVersionCheckMe();
        //    BasisHelper().EditorTableDatasSelected[this] = EditorGUI.ToggleLeft(tRect, "", BasisHelper().EditorTableDatasSelected[this]);
        //    // Draw ID
        //    GUILayout.Label(ID.ToString(), NWDConstants.KTableHeaderIcon);
        //    // Draw prefab
        //    GUILayout.Label(string.Empty, NWDConstants.KTableHeaderPrefab);
        //    Rect tRectPreview = GUILayoutUtility.GetLastRect();
        //    // Draw Informations
        //    GUILayout.Label(tStringRow, NWDConstants.KTableHeaderInformations);
        //    // Draw Disk State
        //    GUILayout.Label(tImageDisk, NWDConstants.KTableHeaderIcon);
        //    // Draw Sync State
        //    GUILayout.Label(tImageSync, NWDConstants.KTableHeaderIcon);
        //    // Draw Dev Sync State
        //    GUILayout.Label(tImageDevSync, NWDConstants.KTableHeaderIcon);
        //    // Draw Preprod Sync State
        //    GUILayout.Label(tImagePreprodSync, NWDConstants.KTableHeaderIcon);
        //    // Draw Prod Sync State
        //    GUILayout.Label(tImageProdSync, NWDConstants.KTableHeaderIcon);
        //    // Draw State
        //    GUILayout.Label(sStateInfos, NWDConstants.KTableHeaderStatut);
        //    // Draw Reference
        //    GUILayout.Label(tStringReference, NWDConstants.KTableHeaderReference);
        //    // Draw prefab preview
        //    DrawPreviewTexture2D(new Rect(tRectPreview.x, tRectPreview.y - 3, NWDConstants.kPrefabWidth, NWDConstants.kPrefabWidth));
        //    // draw line to delimit the rect
        //    GUILayout.EndHorizontal();
        //    // finish line
        //    Rect tEndLine = GUILayoutUtility.GetLastRect();
        //    tEndLine.y += tEndLine.height;
        //    tEndLine.height = 1;
        //    NWDConstants.GUILine(tEndLine);
        //    return rRect;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public Rect DrawRowInEditor(Vector2 sMouseClickPosition, EditorWindow sEditorWindow, bool sSelectAndClick, int sRow, float sZoom)
        {
            Rect tRectRow = new Rect(0, NWDConstants.kRowHeight * sRow * sZoom, sEditorWindow.position.width, NWDConstants.kRowHeight * sZoom);
            Rect tRectRowLineWhite = new Rect(0, NWDConstants.kRowHeight * sRow * sZoom +1, sEditorWindow.position.width, 1);
            Rect tRectRowLineBLack = new Rect(0, NWDConstants.kRowHeight * (sRow + 1) * sZoom, sEditorWindow.position.width, 1);
            Rect tRect = new Rect(NWDConstants.kFieldMarge, NWDConstants.kRowHeight * sRow * sZoom, 0, NWDConstants.kRowHeight * sZoom);
            EditorGUI.DrawRect(tRectRow, tBoxColor);
            if (IsObjectInEdition(this) == true)
            {
                Rect tRectRowSelected = new Rect(tRectRow.x, tRectRow.y +2, tRectRow.width, tRectRow.height-2);
                EditorGUI.DrawRect(tRectRowSelected, NWDConstants.kRowColorSelected);
            }
            //GUI.Label(tRectRow, "TEST " + sRow.ToString());
            if (tRectRow.Contains(sMouseClickPosition))
            {
                GUI.FocusControl(null);
                SetObjectInEdition(this);
                if (sSelectAndClick == true)
                {
                    if (XX == 0 && TestIntegrity())
                    {
                        //Datas().DatasInEditorSelectionList[tIndex] = !Datas().DatasInEditorSelectionList [tIndex];
                        BasisHelper().EditorTableDatasSelected[this] = !BasisHelper().EditorTableDatasSelected[this];
                        Event.current.Use();
                    }
                }
                sEditorWindow.Focus();
            }
            string tStringReference = "[" + Reference + "]";
            // Draw toogle
            tRect.width = NWDConstants.kSelectWidth;
            float tToggleFix = (NWDConstants.kRowHeight * sZoom - NWDConstants.KTableRowSelect.fixedHeight) / 2.0F;
            Rect tRectToogle = new Rect(tRect.x, tRect.y + tToggleFix, tRect.width, NWDConstants.kRowHeight * sZoom);
            //EditorGUI.DrawRect(tRectToogle, Color.red);

            BasisHelper().EditorTableDatasSelected[this] = EditorGUI.ToggleLeft(tRectToogle, "", BasisHelper().EditorTableDatasSelected[this]);
            tRect.x += NWDConstants.kSelectWidth;
            // Draw ID
            tRect.width = NWDConstants.kIDWidth;
            GUI.Label(tRect, ID.ToString(), NWDConstants.KTableRowId);
            tRect.x += NWDConstants.kIDWidth;
            // Draw prefab
            tRect.width = NWDConstants.kPrefabWidth * sZoom;
            DrawPreviewTexture2D(new Rect(tRect.x + NWDConstants.kFieldMarge, tRect.y + NWDConstants.kFieldMarge, NWDConstants.kPrefabWidth * sZoom - +NWDConstants.kFieldMarge * 2, NWDConstants.kRowHeight * sZoom - +NWDConstants.kFieldMarge * 2));
            tRect.x += NWDConstants.kPrefabWidth * sZoom;
            // Draw Informations
            tRect.width = sEditorWindow.position.width
                - NWDConstants.kFieldMarge
                - NWDConstants.kScrollbar
                - NWDConstants.kSelectWidth
                - NWDConstants.kIDWidth
                - NWDConstants.kPrefabWidth * sZoom
                - NWDConstants.KTableIconWidth * 6
                - NWDConstants.KTableSearchWidth
                - NWDConstants.KTableRowWebModelWidth
                - NWDConstants.KTableReferenceWidth;
            if (tRect.width < NWDConstants.KTableSearchWidth)
            {
                tRect.width = NWDConstants.KTableSearchWidth;
            }
            GUI.Label(tRect, StringRow, NWDConstants.KTableRowInformations);
            tRect.x += tRect.width;
            // Draw Disk State
            tRect.width = NWDConstants.KTableRowWebModelWidth;
            GUI.Label(tRect, ModelInfos, NWDConstants.KTableRowStatut);
            tRect.x += NWDConstants.KTableRowWebModelWidth;
            // draw check
            tRect.width = NWDConstants.KTableIconWidth;
            GUI.Label(tRect, ImageChecklist, NWDConstants.KTableRowIcon);
            tRect.x += NWDConstants.KTableIconWidth;
            // draw disk
            tRect.width = NWDConstants.KTableIconWidth;
            GUI.Label(tRect, ImageDisk, NWDConstants.KTableRowIcon);
            tRect.x += NWDConstants.KTableIconWidth;
            // Draw Sync State
            GUI.Label(tRect, ImageSync, NWDConstants.KTableRowIcon);
            tRect.x += NWDConstants.KTableIconWidth;
            // Draw Dev Sync State
            GUI.Label(tRect, ImageDevSync, NWDConstants.KTableRowIcon);
            tRect.x += NWDConstants.KTableIconWidth;
            // Draw Preprod Sync State
            GUI.Label(tRect, ImagePreprodSync, NWDConstants.KTableRowIcon);
            tRect.x += NWDConstants.KTableIconWidth;
            // Draw Prod Sync State
            GUI.Label(tRect, ImageProdSync, NWDConstants.KTableRowIcon);
            tRect.x += NWDConstants.KTableIconWidth;
            // Draw State
            tRect.width = NWDConstants.KTableSearchWidth;
            GUI.Label(tRect, StateInfos, NWDConstants.KTableRowStatut);
            tRect.x += NWDConstants.KTableSearchWidth;
            // Draw Reference
            tRect.width = NWDConstants.KTableReferenceWidth;
            GUI.Label(tRect, tStringReference, NWDConstants.KTableRowReference);
            // finish line
            EditorGUI.DrawRect(tRectRowLineWhite, NWDConstants.kRowColorLineWhite);
            EditorGUI.DrawRect(tRectRowLineBLack, NWDConstants.kRowColorLine);
            return tRectRow;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif