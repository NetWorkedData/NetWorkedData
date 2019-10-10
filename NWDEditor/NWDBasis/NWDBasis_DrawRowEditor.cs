//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:21:2
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
//using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDTypeClass
    {
        protected Texture2D ImageDisk = NWDGUI.kImageDiskUnknow;
        protected Texture2D ImageSync = NWDGUI.kImageSyncGeneralWaiting;
        public int AnalyzeSync = 0;
        protected Texture2D ImageDevSync = NWDGUI.kImageSyncRequired;
        public int AnalyzeDevSync = 0;
        protected Texture2D ImagePreprodSync = NWDGUI.kImageSyncRequired;
        public int AnalyzePreprodSync = 0;
        protected Texture2D ImageProdSync = NWDGUI.kImageSyncRequired;
        public int AnalyzeProdSync = 0;
        public int AnalyzePrefab = 0;
        public bool TestIntegrityResult = true;
        protected bool TestWebserviceVersionIsValid = true;
        protected string StringRow = string.Empty;
        public string StateInfos = string.Empty;
        public int AnalyzeStateInfos = 0;
        public int AnalyzeModel = 0;
        public string ModelInfos = string.Empty;
        //protected string ChecklistInfos = string.Empty;
        protected Texture2D ImageChecklist = NWDGUI.kImageSyncWaiting;
        public int AnalyzeChecklist = 0;
        public int AnalyzeID = 0;
        public bool AnalyzeSelected = false;
        //GUIStyle tStyleBox = NWDConstants.KTableRowNormal;
        public Color tBoxColor = Color.clear;
#if UNITY_EDITOR
        public GUIContent Content;
        public Color DataSelectorBoxColor = Color.clear;
#endif
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis : NWDTypeClass
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
        public override void RowAnalyze()
        {
            //NWEBenchmark.Start();
            CanBeEdit = true; // change to false in draw editor
            AnalyzeID = ID;
            AnalyzeModel = WebModel;
            TestIntegrityResult = IntegrityIsValid();
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
                ImageDisk = NWDGUI.kImageDiskDatabase;
            }
            switch (WritingPending)
            {
                case NWDWritingPending.Unknow:
                    ImageDisk = NWDGUI.kImageDiskUnknow;
                    break;
                case NWDWritingPending.UpdateInMemory:
                    ImageDisk = NWDGUI.kImageDiskUpdate;
                    break;
                case NWDWritingPending.InsertInMemory:
                    ImageDisk = NWDGUI.kImageDiskInsert;
                    break;
                case NWDWritingPending.DeleteInMemory:
                    ImageDisk = NWDGUI.kImageDiskDelete;
                    break;
                case NWDWritingPending.InDatabase:
                    ImageDisk = NWDGUI.kImageDiskDatabase;
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
                ImageSync = NWDGUI.kImageSyncGeneralForbidden;
            }
            else
            {
                if (DS > 0)
                {
                    if (DevSync > 1 && PreprodSync < 1 && ProdSync < 1 && DS == DevSync)
                    {
                        ImageSync = NWDGUI.kImageSyncGeneralSuccessed;
                        AnalyzeSync = KSyncSuccessed;
                    }
                    else if (DevSync > 1 && PreprodSync > 1 && ProdSync < 1 && (DS == DevSync || DS == PreprodSync))
                    {
                        ImageSync = NWDGUI.kImageSyncGeneralSuccessed;
                        AnalyzeSync = KSyncSuccessed;
                    }
                    else if (DS < DM)
                    {
                        ImageSync = NWDGUI.kImageSyncGeneralForward;
                        AnalyzeSync = KSyncForward;
                    }
                    else
                    {
                        ImageSync = NWDGUI.kImageSyncGeneralWaiting;
                        AnalyzeSync = KSyncWaiting;
                    }
                }
            }
            if (DevSync == 0)
            {
                ImageDevSync = NWDGUI.kImageSyncRequired;
                AnalyzeDevSync = KSyncRequired;
            }
            else if (DevSync == 1)
            {
                ImageDevSync = NWDGUI.kImageSyncWaiting;
                AnalyzeDevSync = KSyncWaiting;
            }
            else if (DevSync > 1)
            {
                ImageDevSync = NWDGUI.kImageSyncSuccessed;
                AnalyzeDevSync = KSyncSuccessed;
            }
            else if (DevSync == -1)
            {
                ImageDevSync = NWDGUI.kImageSyncForbidden;
                AnalyzeDevSync = KSyncForbidden;
            }
            else if (DevSync < -1)
            {
                ImageDevSync = NWDGUI.kImageSyncDanger;
                AnalyzeDevSync = KSyncDanger;
            }

            if (PreprodSync == 0)
            {
                ImagePreprodSync = NWDGUI.kImageSyncRequired;
                AnalyzePreprodSync = KSyncRequired;
            }
            else if (PreprodSync == 1)
            {
                ImagePreprodSync = NWDGUI.kImageSyncWaiting;
                AnalyzePreprodSync = KSyncWaiting;
            }
            else if (PreprodSync > 1)
            {
                if (PreprodSync > DevSync)
                {
                    ImagePreprodSync = NWDGUI.kImageSyncSuccessed;
                    AnalyzePreprodSync = KSyncSuccessed;
                }
                else
                {
                    ImagePreprodSync = NWDGUI.kImageSyncForward;
                    AnalyzePreprodSync = KSyncForward;
                }
            }
            else if (PreprodSync == -1)
            {
                ImagePreprodSync = NWDGUI.kImageSyncForbidden;
                AnalyzePreprodSync = KSyncForbidden;
            }
            else if (PreprodSync < -1)
            {
                ImagePreprodSync = NWDGUI.kImageSyncDanger;
                AnalyzePreprodSync = KSyncDanger;
            }
            if (tDisableProd == true)
            {
                ImageProdSync = NWDGUI.kImageSyncForbidden;
                AnalyzeProdSync = KSyncForbidden;
            }
            else
            {
                if (ProdSync == 0)
                {
                    ImageProdSync = NWDGUI.kImageSyncRequired;
                    AnalyzeProdSync = KSyncRequired;
                }
                else if (ProdSync == 1)
                {
                    ImageProdSync = NWDGUI.kImageSyncWaiting;
                    AnalyzeProdSync = KSyncWaiting;
                }
                if (ProdSync > 1)
                {
                    if (ProdSync > DevSync && ProdSync > PreprodSync)
                    {
                        ImageProdSync = NWDGUI.kImageSyncSuccessed;
                        AnalyzeProdSync = KSyncSuccessed;
                    }
                    else
                    {
                        ImageProdSync = NWDGUI.kImageSyncForward;
                        AnalyzeProdSync = KSyncForward;
                    }
                }
                else if (ProdSync == -1)
                {
                    ImageProdSync = NWDGUI.kImageSyncForbidden;
                    AnalyzeProdSync = KSyncForbidden;
                }
                else if (ProdSync < -1)
                {
                    ImageProdSync = NWDGUI.kImageSyncDanger;
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
                    ImageChecklist = NWDGUI.kImageCheckWorkInProgress;
                    AnalyzeChecklist = KChecklistWorkInProgress;
                }
                else
                {
                    //ChecklistInfos = "<color=green>[√]</color> ";
                    ImageChecklist = NWDGUI.kImageCheckValid;
                    AnalyzeChecklist = KChecklistValid;
                }
            }
            else
            {
                ImageChecklist = null;
            }
            StateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_OK;
            AnalyzeStateInfos = KAnalyzeStateEnable;
            //tBoxColor = NWDGUI.DefaultColorArea();
            tBoxColor = Color.clear;
            DataSelectorBoxColor = NWDGUI.DefaultColorArea();
            if (TestWebserviceVersionIsValid)
            {
                if (TestIntegrityResult == false)
                {
                    tBoxColor = NWDGUI.kRowColorError;
                    DataSelectorBoxColor = NWDGUI.kRowColorError;
                    StateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_INTEGRITY_ERROR;
                    StringRow = "<color=#a52a2aff>" + StringRow + "</color>";
                    AnalyzeStateInfos = KAnalyzeStateCorrupted;
                    ImageChecklist = NWDGUI.kImageCheckWarning;
                }
                else if (XX > 0)
                {
                    tBoxColor = NWDGUI.kRowColorTrash;
                    DataSelectorBoxColor = NWDGUI.kRowColorTrash;
                    StateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_TRASH;
                    StringRow = "<color=#444444ff>" + StringRow + "</color>";
                    AnalyzeStateInfos = KAnalyzeStateTrashed;
                    ImageChecklist = null;
                }
                else
                {
                    if (AC == false)
                    {
                        tBoxColor = NWDGUI.kRowColorDisactive;
                        DataSelectorBoxColor = NWDGUI.kRowColorDisactive;
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
                tBoxColor = NWDGUI.kRowColorWarning;
                DataSelectorBoxColor = NWDGUI.kRowColorWarning;
                StateInfos = NWDConstants.K_APP_TABLE_ROW_OBJECT_WEBSERVICE_ERROR;
                StringRow = "<color=#cc6600ff>" + StringRow + "</color>";
                AnalyzeStateInfos = KAnalyzeStateModelError;
            }
            if (InError == true)
            {
                ImageChecklist = NWDGUI.kImageCheckWarning;
                AnalyzeChecklist = KChecklistWarning;
                AnalyzeStateInfos = KAnalyzeStateWarning;
                ImageChecklist = NWDGUI.kImageCheckWarning;
            }
            StringRow = StringRow.Replace("()", string.Empty);
            while (StringRow.Contains("  "))
            {
                StringRow = StringRow.Replace("  ", " ");
            }

            Content = new GUIContent(InternalKey, PreviewTexture2D(), InternalDescription);

            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override Rect DrawRowInEditor(Vector2 sMouseClickPosition, Rect sRectRow, bool sSelectAndClick, int sRow, float sZoom)
        {
            //NWEBenchmark.Start();
            Rect tRectRow = new Rect(0, NWDGUI.kTableRowHeight * sRow * sZoom, sRectRow.width, NWDGUI.kTableRowHeight * sZoom);
            Rect tRectRowLineWhite = new Rect(0, NWDGUI.kTableRowHeight * sRow * sZoom + 1, sRectRow.width, 1);
            Rect tRectRowLineBLack = new Rect(0, NWDGUI.kTableRowHeight * (sRow + 1) * sZoom, sRectRow.width, 1);
            Rect tRect = new Rect(NWDGUI.kFieldMarge, NWDGUI.kTableRowHeight * sRow * sZoom, 0, NWDGUI.kTableRowHeight * sZoom);
            EditorGUI.DrawRect(tRectRow, tBoxColor);
            if (BasisHelper().IsObjectInEdition(this) == true)
            {
                Rect tRectRowSelected = new Rect(tRectRow.x, tRectRow.y + 2, tRectRow.width, tRectRow.height - 2);
                EditorGUI.DrawRect(tRectRowSelected, NWDGUI.kRowColorSelected);
            }
            //GUI.Label(tRectRow, "TEST " + sRow.ToString());
            if (tRectRow.Contains(sMouseClickPosition))
            {
                GUI.FocusControl(null);
                BasisHelper().SetObjectInEdition(this);
                if (sSelectAndClick == true)
                {
                    if (XX == 0 && IntegrityIsValid())
                    {
                        //Datas().DatasInEditorSelectionList[tIndex] = !Datas().DatasInEditorSelectionList [tIndex];
                        BasisHelper().EditorTableDatasSelected[this] = !BasisHelper().EditorTableDatasSelected[this];
                        Event.current.Use();
                    }
                }
                //sEditorWindow.Focus();
            }
            string tStringReference = "[" + Reference + "]";
            // Draw toogle
            tRect.width = NWDGUI.kTableSelectWidth;
            float tToggleFix = (NWDGUI.kTableRowHeight * sZoom - NWDGUI.KTableRowSelect.fixedHeight) / 2.0F;
            Rect tRectToogle = new Rect(tRect.x, tRect.y + tToggleFix, tRect.width, NWDGUI.kTableRowHeight * sZoom);
            BasisHelper().EditorTableDatasSelected[this] = EditorGUI.ToggleLeft(tRectToogle, "", BasisHelper().EditorTableDatasSelected[this]);
            tRect.x += NWDGUI.kTableSelectWidth;
            // Draw ID
            tRect.width = NWDGUI.kTableIDWidth;
            GUI.Label(tRect, ID.ToString(), NWDGUI.KTableRowId);
            tRect.x += NWDGUI.kTableIDWidth;
            // Draw prefab
            tRect.width = NWDGUI.kTablePrefabWidth * sZoom;
            DrawPreviewTexture2D(new Rect(tRect.x + NWDGUI.kFieldMarge, tRect.y + NWDGUI.kFieldMarge, NWDGUI.kTablePrefabWidth * sZoom - +NWDGUI.kFieldMarge * 2, NWDGUI.kTableRowHeight * sZoom - +NWDGUI.kFieldMarge * 2));
            tRect.x += NWDGUI.kTablePrefabWidth * sZoom;
            // Draw Informations
            tRect.width = sRectRow.width
                - NWDGUI.kFieldMarge
                - NWDGUI.kScrollbar
                - NWDGUI.kTableSelectWidth
                - NWDGUI.kTableIDWidth
                - NWDGUI.kTablePrefabWidth * sZoom
                - NWDGUI.kTableIconWidth * 6
                - NWDGUI.KTableSearchWidth
                - NWDGUI.KTableRowWebModelWidth
                - NWDGUI.KTableReferenceWidth;
            if (tRect.width < NWDGUI.KTableSearchWidth)
            {
                tRect.width = NWDGUI.KTableSearchWidth;
            }
            GUI.Label(tRect, StringRow, NWDGUI.KTableRowInformations);
            tRect.x += tRect.width;
            // Draw Disk State
            tRect.width = NWDGUI.KTableRowWebModelWidth;
            GUI.Label(tRect, ModelInfos, NWDGUI.KTableRowStatut);
            tRect.x += NWDGUI.KTableRowWebModelWidth;
            // draw check
            tRect.width = NWDGUI.kTableIconWidth;
            GUI.Label(tRect, ImageChecklist, NWDGUI.KTableRowIcon);
            tRect.x += NWDGUI.kTableIconWidth;
            // draw disk
            tRect.width = NWDGUI.kTableIconWidth;
            GUI.Label(tRect, ImageDisk, NWDGUI.KTableRowIcon);
            tRect.x += NWDGUI.kTableIconWidth;
            // Draw Sync State
            GUI.Label(tRect, ImageSync, NWDGUI.KTableRowIcon);
            tRect.x += NWDGUI.kTableIconWidth;
            // Draw Dev Sync State
            GUI.Label(tRect, ImageDevSync, NWDGUI.KTableRowIcon);
            tRect.x += NWDGUI.kTableIconWidth;
            // Draw Preprod Sync State
            GUI.Label(tRect, ImagePreprodSync, NWDGUI.KTableRowIcon);
            tRect.x += NWDGUI.kTableIconWidth;
            // Draw Prod Sync State
            GUI.Label(tRect, ImageProdSync, NWDGUI.KTableRowIcon);
            tRect.x += NWDGUI.kTableIconWidth;
            // Draw State
            tRect.width = NWDGUI.KTableSearchWidth;
            GUI.Label(tRect, new GUIContent(StateInfos, ErrorLog), NWDGUI.KTableRowStatut);
            tRect.x += NWDGUI.KTableSearchWidth;
            // Draw Reference
            tRect.width = NWDGUI.KTableReferenceWidth;
            GUI.Label(tRect, tStringReference, NWDGUI.KTableRowReference);
            // finish line
            EditorGUI.DrawRect(tRectRowLineWhite, NWDGUI.kRowColorLineWhite);
            EditorGUI.DrawRect(tRectRowLineBLack, NWDGUI.kRowColorLineBlack);
            //NWEBenchmark.Finish();
            return tRectRow;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif