//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        private static string GetReferenceOfDataInEdition()
        {
            string rReturn = null;
            NWDTypeClass tObject = NWDDataInspector.ObjectInEdition() as NWDTypeClass;
            if (tObject != null)
            {
                rReturn = string.Copy(tObject.Reference);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void RestaureDataInEditionByReference(string sReference)
        {
            K tObject = null;
            if (sReference != null)
            {
                if (BasisHelper().DatasByReference.ContainsKey(sReference))
                {
                    tObject = BasisHelper().DatasByReference[sReference] as K;
                }
                if (tObject != null)
                {
                    if (BasisHelper().EditorTableDatas.Contains(tObject))
                    {
                        BasisHelper().New_SetObjectInEdition(tObject);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static void IntegritySelection()
        //{
        //    foreach (K tObject in BasisHelper().EditorTableDatas)
        //    {
        //        if (tObject.TestIntegrity() == false || tObject.XX > 0)
        //        {
        //            if (BasisHelper().EditorTableDatasSelected.ContainsKey(tObject))
        //            {
        //                BasisHelper().EditorTableDatasSelected[tObject] = false;
        //            }
        //        }
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static void SelectAllObjectInTableList()
        {
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                BasisHelper().EditorTableDatasSelected[tObject] = true;
            }
            //IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DeselectAllObjectInTableList()
        {
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                BasisHelper().EditorTableDatasSelected[tObject] = false;
            }
            //IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void InverseSelectionOfAllObjectInTableList()
        {
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                BasisHelper().EditorTableDatasSelected[tObject] = !BasisHelper().EditorTableDatasSelected[tObject];
            }
            //IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SelectAllObjectEnableInTableList()
        {
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                K tObjectK = tObject as K;
                BasisHelper().EditorTableDatasSelected[tObjectK] = tObjectK.IsEnable();
            }
            //IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SelectAllObjectDisableInTableList()
        {
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                K tObjectK = tObject as K;
                BasisHelper().EditorTableDatasSelected[tObjectK] = !tObjectK.IsEnable();
            }
            //IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static void FilterTableEditor()
        //{
        //    BasisHelper().EditorTableDatas = new List<NWDTypeClass>();
        //    BasisHelper().EditorTableDatasSelected = new Dictionary<NWDTypeClass, bool>();
        //    foreach (K tObject in BasisHelper().Datas)
        //    {
        //        bool tOccurence = true;

        //        if (tObject.TestIntegrity() == false && BasisHelper().m_ShowIntegrityError == false)
        //        {
        //            tOccurence = false;
        //        }
        //        if (tObject.IsEnable() == true && BasisHelper().m_ShowEnable == false)
        //        {
        //            tOccurence = false;
        //        }
        //        if (tObject.IsEnable() == false && BasisHelper().m_ShowDisable == false)
        //        {
        //            tOccurence = false;
        //        }
        //        if (tObject.XX > 0 && BasisHelper().m_ShowTrashed == false)
        //        {
        //            tOccurence = false;
        //        }

        //        if (BasisHelper().ClassType != typeof(NWDAccount))
        //        {
        //            if (string.IsNullOrEmpty(BasisHelper().m_SearchAccount) == false)
        //            {
        //                if (BasisHelper().m_SearchAccount == "-=-") // empty
        //                {
        //                    if (tObject.VisibleByAccountByEqual(string.Empty) == false)
        //                    {
        //                        tOccurence = false;
        //                    }
        //                }
        //                else if (BasisHelper().m_SearchAccount == "-+-") // not empty
        //                {
        //                    if (tObject.VisibleByAccountByEqual(string.Empty) == true)
        //                    {
        //                        tOccurence = false;
        //                    }
        //                }
        //                else
        //                {
        //                    if (tObject.VisibleByAccount(BasisHelper().m_SearchAccount) == false)
        //                    {
        //                        tOccurence = false;
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (string.IsNullOrEmpty(BasisHelper().m_SearchAccount) == false)
        //            {
        //                if (BasisHelper().m_SearchAccount == "-=-") // empty
        //                {
        //                }
        //                else if (BasisHelper().m_SearchAccount == "-+-") // not empty
        //                {
        //                }
        //                else if (tObject.Reference != BasisHelper().m_SearchAccount)
        //                {
        //                    tOccurence = false;
        //                }
        //            }
        //        }

        //        if (string.IsNullOrEmpty(BasisHelper().m_SearchGameSave) == false)
        //        {
        //            if (BasisHelper().m_SearchGameSave == "-=-")
        //            {
        //                if (tObject.VisibleByGameSave(string.Empty) == false)
        //                {
        //                    tOccurence = false;
        //                }
        //            }
        //            else if (BasisHelper().m_SearchGameSave == "-+-")
        //            {
        //                if (tObject.VisibleByGameSave(string.Empty) == true)
        //                {
        //                    tOccurence = false;
        //                }
        //            }
        //            else
        //            {
        //                if (tObject.VisibleByGameSave(BasisHelper().m_SearchGameSave) == false)
        //                {
        //                    tOccurence = false;
        //                }
        //            }
        //        }
        //        if (string.IsNullOrEmpty(BasisHelper().m_SearchReference) == false)
        //        {
        //            if (tObject.Reference.Contains(BasisHelper().m_SearchReference) == false)
        //            {
        //                tOccurence = false;
        //            }
        //        }
        //        if (string.IsNullOrEmpty(BasisHelper().m_SearchInternalName) == false)
        //        {
        //            if (tObject.InternalKey.ToLower().Contains(BasisHelper().m_SearchInternalName.ToLower()) == false)
        //            {
        //                tOccurence = false;
        //            }
        //        }
        //        if (string.IsNullOrEmpty(BasisHelper().m_SearchInternalDescription) == false)
        //        {
        //            if (tObject.InternalDescription.ToLower().Contains(BasisHelper().m_SearchInternalDescription.ToLower()) == false)
        //            {
        //                tOccurence = false;
        //            }
        //        }
        //        if (BasisHelper().m_SearchTag != NWDBasisTag.NoTag)
        //        {
        //            if (tObject.Tag != BasisHelper().m_SearchTag /*&& tObject.Tag != NWDBasisTag.NoTag*/)
        //            {
        //                tOccurence = false;
        //            }
        //        }
        //        if (BasisHelper().m_SearchCheckList != NWDBasisCheckList.Nothing)
        //        {
        //            if (tObject.CheckList != null)
        //            {
        //                if (tObject.CheckList.ContainsMask(BasisHelper().m_SearchCheckList) == false)
        //                {
        //                    tOccurence = false;
        //                }
        //            }
        //        }
        //        if (tOccurence == true)
        //        {
        //            if (BasisHelper().EditorTableDatas.Contains(tObject) == false)
        //            {
        //                BasisHelper().EditorTableDatas.Add(tObject);
        //            }
        //            if (BasisHelper().EditorTableDatasSelected.ContainsKey(tObject) == false)
        //            {
        //                BasisHelper().EditorTableDatasSelected.Add(tObject, false);
        //            }
        //        }
        //    }
        //    BasisHelper().SortEditorTableDatas();
        //}

        //-------------------------------------------------------------------------------------------------------------
        //public static void RepaintTableEditor()
        //{
        //    NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void RepaintInspectorEditor()
        //{
        //    NWDDataInspector.ActiveRepaint();
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static float DrawPagesTab(Rect sRect)
        {
            float rReturn = sRect.height;
            float tWidth = sRect.width;
            float tTabWidth = 35.0f;
            //float tPopupWidth = 60.0f;
            int tToogleToListPageLimit = (int)Math.Floor(tWidth / tTabWidth);
            //GUILayout.Space(NWDConstants.KTablePageMarge);
            Rect tRect = new Rect(sRect.x + NWDGUI.kFieldMarge, sRect.y, sRect.width - NWDGUI.kFieldMarge * 2, EditorStyles.toolbar.fixedHeight);
            BasisHelper().m_ItemPerPage = int.Parse(BasisHelper().m_ItemPerPageOptions[BasisHelper().m_ItemPerPageSelection]);
            float tNumberOfPage = BasisHelper().EditorTableDatas.Count / BasisHelper().m_ItemPerPage;
            int tPagesExpected = (int)Math.Floor(tNumberOfPage);
            if (tPagesExpected != 0)
            {
                if (BasisHelper().EditorTableDatas.Count % (tPagesExpected * BasisHelper().m_ItemPerPage) != 0)
                {
                    tPagesExpected++;
                }
            }
            if (BasisHelper().m_PageSelected > tPagesExpected - 1)
            {
                BasisHelper().m_PageSelected = tPagesExpected - 1;
            }
            BasisHelper().m_MaxPage = tPagesExpected + 1;
            string[] tListOfPagesName = new string[tPagesExpected];
            for (int p = 0; p < tPagesExpected; p++)
            {
                int tP = p + 1;
                tListOfPagesName[p] = string.Empty + tP.ToString();
            }
            int t_PageSelected = BasisHelper().m_PageSelected;
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
                //m_PageSelected = GUILayout.Toolbar (m_PageSelected, tListOfPagesName, GUILayout.ExpandWidth (true));
                t_PageSelected = GUI.Toolbar(tRect, BasisHelper().m_PageSelected, tListOfPagesName);
            }
            else
            {
                //rReturn = EditorStyles.popup.fixedHeight + NWDGUI.kFieldMarge;
                rReturn = NWDGUI.KTableToolbar.fixedHeight + NWDGUI.kFieldMarge;
                tRect.height = EditorStyles.popup.fixedHeight;
                tRect.y += 2;
                t_PageSelected = EditorGUI.Popup(tRect, BasisHelper().m_PageSelected, tListOfPagesName, EditorStyles.popup);
            }
            if (BasisHelper().m_PageSelected != t_PageSelected)
            {
                NWDDataManager.SharedInstance().DataQueueExecute();
            }
            BasisHelper().m_PageSelected = t_PageSelected;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ChangeScroolPositionToSelection()
        {
            NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
            foreach (EditorWindow tWindow in NWDDataManager.SharedInstance().EditorWindowsInManager(ClassType()))
            {
                Rect tWindowRect = new Rect(tWindow.position.x, tWindow.position.y, tWindow.position.width, tWindow.position.height);
                if (tWindowRect.width < NWDGUI.KTableMinWidth)
                {
                    tWindowRect.width = NWDGUI.KTableMinWidth;
                }
                Rect tRect = new Rect(0, 0, tWindowRect.width, 0);
                // offset the tab bar 
                tRect.y += 50;                 //float tScrollHeight = tWindowRect.height - tRect.y - tPagesBarHeight - tRectForBottom.height - NWDGUI.kTableHeaderHeight - NWDGUI.kFieldMarge;
                float tScrollHeight = tWindowRect.height - tRect.y - NWDGUI.kTableHeaderHeight - NWDGUI.kFieldMarge;                 Rect tScrollHeader = new Rect(0, tRect.y, tWindowRect.width, NWDGUI.kTableHeaderHeight);                 Rect tScrollRect = new Rect(0, tRect.y + NWDGUI.kTableHeaderHeight, tWindowRect.width, tScrollHeight);
                ChangeScroolPositionToSelection(tScrollRect);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ChangeScroolPositionToSelection(Rect sScrollRect)
        {
            int tIndexSelected = BasisHelper().EditorTableDatas.IndexOf((NWDTypeClass)NWDDataInspector.ObjectInEdition());
            float tNumberPage = tIndexSelected / BasisHelper().m_ItemPerPage;
            int tPageExpected = (int)Math.Floor(tNumberPage);
            BasisHelper().m_PageSelected = tPageExpected;
            Vector2 tMouseScrollTop = new Vector2(0, (tIndexSelected - tPageExpected * BasisHelper().m_ItemPerPage) * NWDGUI.kTableRowHeight * BasisHelper().RowZoom);
            Vector2 tMouseScrollBottom = new Vector2(tMouseScrollTop.x, tMouseScrollTop.y + NWDGUI.kTableRowHeight * BasisHelper().RowZoom);
            Rect tAreaVisible = new Rect(BasisHelper().m_ScrollPositionList.x, BasisHelper().m_ScrollPositionList.y, sScrollRect.width, sScrollRect.height);
            if (tAreaVisible.Contains(tMouseScrollTop) == false && tAreaVisible.Contains(tMouseScrollBottom) == false)
            {
                BasisHelper().m_ScrollPositionList.y = tMouseScrollTop.y;
                tAreaVisible = new Rect(BasisHelper().m_ScrollPositionList.x, BasisHelper().m_ScrollPositionList.y, sScrollRect.width, sScrollRect.height);
            }
            int tCountSecurity = BasisHelper().m_ItemPerPage;
            while ((tAreaVisible.Contains(tMouseScrollTop) == false || tAreaVisible.Contains(tMouseScrollBottom) == false) && tCountSecurity > 0)
            {
                //Debug.Log("Analyze next ");
                tCountSecurity--;
                if (tAreaVisible.Contains(tMouseScrollBottom) == false)
                {
                    BasisHelper().m_ScrollPositionList.y += NWDGUI.kTableRowHeight * BasisHelper().RowZoom;
                }
                else if (tAreaVisible.Contains(tMouseScrollTop) == false)
                {
                    BasisHelper().m_ScrollPositionList.y -= NWDGUI.kTableRowHeight * BasisHelper().RowZoom;
                }
                tAreaVisible = new Rect(BasisHelper().m_ScrollPositionList.x, BasisHelper().m_ScrollPositionList.y, sScrollRect.width, sScrollRect.height);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect DrawTableEditorTop(Rect sRect)
        {
            EditorGUIUtility.labelWidth = NWDGUI.KTableSearchLabelWidth;
            Rect rRect = new Rect(sRect.x, sRect.y, sRect.width, 0);
            if (BasisHelper().SearchActions() == true)
            {
                Rect tRect = new Rect(sRect.x + NWDGUI.kFieldMarge, sRect.y, sRect.width, 0);
                tRect.width = NWDGUI.KTableSearchFieldWidth;
                tRect.height = NWDGUI.KTableSearchToggle.fixedHeight;
                // draw Search zone
                GUI.Label(tRect, NWDConstants.K_APP_TABLE_SEARCH_ZONE, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw Reference
                BasisHelper().m_SearchReference = EditorGUI.TextField(tRect, NWDConstants.K_APP_TABLE_SEARCH_REFERENCE,
                     BasisHelper().m_SearchReference, NWDGUI.KTableSearchTextfield);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw internal key
                BasisHelper().m_SearchInternalName = EditorGUI.TextField(tRect, NWDConstants.K_APP_TABLE_SEARCH_NAME,
                    BasisHelper().m_SearchInternalName, NWDGUI.KTableSearchTextfield);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw Internal description
                BasisHelper().m_SearchInternalDescription = EditorGUI.TextField(tRect, NWDConstants.K_APP_TABLE_SEARCH_DESCRIPTION,
                    BasisHelper().m_SearchInternalDescription, NWDGUI.KTableSearchTextfield);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;

                // Change column
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = sRect.y;

                GUI.Label(tRect, NWDConstants.K_APP_TABLE_FILTER_ZONE, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw accounts popup
                EditorGUI.BeginDisabledGroup(!AccountDependent());
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
                foreach (KeyValuePair<string, string> tKeyValue in NWDAccount.BasisHelper().EditorDatasMenu.OrderBy(i => i.Value))
                {
                    tReferenceList.Add(tKeyValue.Key);
                    tInternalNameList.Add(tKeyValue.Value);
                }
                List<GUIContent> tContentFuturList = new List<GUIContent>();
                foreach (string tS in tInternalNameList.ToArray())
                {
                    tContentFuturList.Add(new GUIContent(tS));
                }
                int tIndexAccount = tReferenceList.IndexOf(BasisHelper().m_SearchAccount);
                int tNewIndexAccount = EditorGUI.Popup(tRect, new GUIContent(NWDConstants.K_APP_TABLE_SEARCH_ACCOUNT), tIndexAccount, tContentFuturList.ToArray(),
                                                                           NWDGUI.KTableSearchEnum);
                if (tNewIndexAccount >= 0 && tNewIndexAccount < tReferenceList.Count())
                {
                    BasisHelper().m_SearchAccount = tReferenceList[tNewIndexAccount];
                }
                else
                {
                    BasisHelper().m_SearchAccount = string.Empty;
                }
                EditorGUI.EndDisabledGroup();
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw GameSave popup
                EditorGUI.BeginDisabledGroup(!GameSaveDependent());
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
                foreach (KeyValuePair<string, string> tKeyValue in NWDGameSave.BasisHelper().EditorDatasMenu.OrderBy(i => i.Value))
                {
                    tReferenceSaveList.Add(tKeyValue.Key);
                    tInternalNameSaveList.Add(tKeyValue.Value);
                }
                List<GUIContent> tContentFuturSaveList = new List<GUIContent>();
                foreach (string tS in tInternalNameSaveList.ToArray())
                {
                    tContentFuturSaveList.Add(new GUIContent(tS));
                }
                int tIndexSave = tReferenceSaveList.IndexOf(BasisHelper().m_SearchGameSave);
                int tNewIndexSave = EditorGUI.Popup(tRect, new GUIContent(NWDConstants.K_APP_TABLE_SEARCH_GAMESAVE), tIndexSave, tContentFuturSaveList.ToArray(),
                                                                           NWDGUI.KTableSearchEnum);
                if (tNewIndexSave >= 0 && tNewIndexSave < tReferenceSaveList.Count())
                {
                    BasisHelper().m_SearchGameSave = tReferenceSaveList[tNewIndexSave];
                }
                else
                {
                    BasisHelper().m_SearchGameSave = string.Empty;
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
                BasisHelper().m_SearchTag = (NWDBasisTag)EditorGUI.IntPopup(tRect, NWDConstants.K_APP_TABLE_SEARCH_TAG,
                                                                    (int)BasisHelper().m_SearchTag, tTagStringList.ToArray(),
                                                                    tTagIntList.ToArray(),
                                                                    NWDGUI.KTableSearchEnum);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;


                EditorGUI.BeginDisabledGroup(AccountDependent());
                BasisHelper().m_SearchCheckList = (NetWorkedData.NWDBasisCheckList)BasisHelper().m_SearchCheckList.ControlField(tRect, NWDConstants.K_APP_TABLE_SEARCH_CHECKLIST);
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
                    BasisHelper().New_SetObjectInEdition(null);
                    BasisHelper().New_FilterTableEditor();
                    RestaureDataInEditionByReference(tReference);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw Remove filter
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_SEARCH_REMOVE_FILTER, NWDGUI.KTableSearchButton))
                {

                    string tReference = GetReferenceOfDataInEdition();
                    GUI.FocusControl(null);
                    BasisHelper().New_SetObjectInEdition(null);
                    //m_SearchReference = "";
                    BasisHelper().m_SearchReference = string.Empty;
                    BasisHelper().m_SearchInternalName = string.Empty;
                    BasisHelper().m_SearchInternalDescription = string.Empty;
                    BasisHelper().m_SearchTag = NWDBasisTag.NoTag;
                    BasisHelper().m_SearchAccount = string.Empty;
                    BasisHelper().m_SearchGameSave = string.Empty;
                    BasisHelper().m_SearchCheckList.Value = 0;
                    BasisHelper().New_FilterTableEditor();
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
                    int tRealReference = BasisHelper().Datas.Count;
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
                    int tResultReference = BasisHelper().EditorTableDatas.Count;
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
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
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




                // draw reload all datas
                //if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_SEARCH_RELOAD, NWDConstants.KTableSearchButton))
                //{
                //    //Debug.Log(NWDConstants.K_APP_TABLE_SEARCH_RELOAD + "Action");
                //    string tReference = GetReferenceOfDataInEdition();
                //    GUI.FocusControl(null);
                //    SetObjectInEdition(null);
                //    BasisHelper().m_SearchInternalName = string.Empty;
                //    BasisHelper().m_SearchInternalDescription = string.Empty;
                //    //ReloadAllObjects ();
                //    //LoadTableEditor ();
                //    LoadFromDatabase();
                //    RestaureDataInEditionByReference(tReference);
                //}
                //tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw before last column
                tRect.x = sRect.width - NWDGUI.KTableSearchFieldWidth - NWDGUI.KTableSearchToggle.fixedHeight * 3 - NWDGUI.kFieldMarge * 4;
                tRect.y = sRect.y;
                tRect.width = NWDGUI.KTableSearchFieldWidth;
                // Draw title
                GUI.Label(tRect, BasisHelper().ClassNamePHP, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // Draw texture of this class
                tRect.height = NWDGUI.KTableSearchToggle.fixedHeight * 3 + NWDGUI.kFieldMarge * 2;
                GUI.Label(tRect, BasisHelper().ClassDescription, NWDGUI.KTableSearchDescription);
                tRect.height = NWDGUI.KTableSearchToggle.fixedHeight;
                // draw last column
                tRect.x = sRect.width - NWDGUI.KTableSearchToggle.fixedHeight * 3 - NWDGUI.kFieldMarge * 3;
                tRect.y = sRect.y;
                tRect.width = NWDGUI.KTableSearchToggle.fixedHeight * 3 + NWDGUI.kFieldMarge * 2;
                // Draw title
                GUI.Label(tRect, BasisHelper().ClassTrigramme, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // Draw texture of this class
                tRect.height = NWDGUI.KTableSearchToggle.fixedHeight * 3 + NWDGUI.kFieldMarge * 2;
                Texture2D tTextureOfClass = BasisHelper().TextureOfClass();
                if (tTextureOfClass != null)
                {
                    GUI.Label(tRect, tTextureOfClass, NWDGUI.KTableSearchClassIcon);
                }
                tRect.height = NWDGUI.KTableSearchToggle.fixedHeight;


                rRect.height = NWDGUI.KTableSearchToggle.fixedHeight * 5 + NWDGUI.kFieldMarge * 5;
                //EditorGUI.DrawRect(rRect, Color.red);
            }
            return rRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Rect DrawTableEditorBottom(Rect sRect)
        {
            Rect rRect = new Rect(sRect.x, sRect.y, sRect.width, 0);

            Rect tRectActionLeft = Rect.zero;
            Rect tRectActionRight = Rect.zero;
            Rect tRectTableLeft = Rect.zero;
            Rect tRectTableRight = Rect.zero;

            float tHeightAction = 0;
            float tHeightTable = 0;

            if (BasisHelper().RowActions() == true)
            {
                tHeightAction = NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge + (NWDGUI.KTableSearchTextfield.fixedHeight + NWDGUI.kFieldMarge) * 6;
            }
            if (BasisHelper().TableActions() == true)
            {
                tHeightTable = NWDGUI.kBoldLabelStyle.fixedHeight + NWDGUI.kFieldMarge + (NWDGUI.KTableSearchTextfield.fixedHeight + NWDGUI.kFieldMarge) * 5;
            }
            rRect.height = tHeightAction + tHeightTable;
            rRect.y = sRect.height - rRect.height;

            if (BasisHelper().RowActions() == true)
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

            if (BasisHelper().TableActions() == true)
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
            bool tLocalizeLocalTable = false; //prevent GUIlayout error
            bool tTrashSelection = false;  //prevent GUIlayout error
            bool tUntrashSelection = false;  //prevent GUIlayout error
            bool tReintegrateSelection = false;  //prevent GUIlayout error
            bool tResetTable = false;  //prevent GUIlayout error
            bool tCreateAllPHPForOnlyThisClassDEV = false; //prevent GUIlayout error
            bool tCreateAllPHPForOnlyThisClass = false; //prevent GUIlayout error
            bool tReintegrateOnlyThisClass = false; //prevent GUIlayout error
            bool tDeleteOldModelOnlyThisClass = false; //prevent GUIlayout error
            bool tSyncProd = false; //prevent GUIlayout error
            bool tSyncForceProd = false; //prevent GUIlayout error
            bool tPullProd = false; //prevent GUIlayout error
            bool tPullProdForce = false; //prevent GUIlayout error
            bool tSyncCleanProd = false; //prevent GUIlayout error
            bool tSyncSpecialProd = false; //prevent GUIlayout error
            bool tCleanLocalTable = false; //prevent GUIlayout error
            bool tCleanLocalTableWithAccount = false; //prevent GUIlayout error
            bool tDisableProd = false;
            if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains(ClassType()))
            {
                tDisableProd = true;
            }
            if (AccountDependent() == true)
            {
                tDisableProd = true;
            }
            // TODO Extract in Basishelper()
            int tSelectionCount = 0;
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
            {
                if (tKeyValue.Value == true)
                {
                    tSelectionCount++;
                }
            }
            // ===========================================
            Rect tRect = new Rect(0, 0, NWDGUI.KTableSearchWidth, NWDGUI.KTableSearchTextfield.fixedHeight);
            if (BasisHelper().RowActions() == true)
            {

                tRect.x = tRectActionLeft.x;
                tRect.y = tRectActionLeft.y;
                int tActualItems = BasisHelper().EditorTableDatas.Count;
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

                GUI.Label(tRect, NWDConstants.K_APP_TABLE_ACTIONS, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_REACTIVE, NWDGUI.KTableSearchButton))
                {
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
                    {
                        if (tKeyValue.Value == true)
                        {
                            K tObject = tKeyValue.Key as K;
                            tObject.EnableData();
                            tObject.RowAnalyze();
                        }
                    }
                    NWDDataInspector.ActiveRepaint();
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_DISACTIVE, NWDGUI.KTableSearchButton))
                {
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
                    {
                        if (tKeyValue.Value == true)
                        {
                            K tObject = tKeyValue.Key as K;
                            tObject.DisableData();
                            tObject.RowAnalyze();
                        }
                    }
                    NWDDataInspector.ActiveRepaint();
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_DUPPLICATE, NWDGUI.KTableSearchButton))
                {
                    NWDBasis<K> tNextObjectSelected = null;
                    int tNewData = 0;
                    List<K> tListToUse = new List<K>();
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
                    {
                        if (tKeyValue.Value == true)
                        {
                            K tObject = tKeyValue.Key as K;
                            tListToUse.Add(tObject);
                        }
                    }
                    foreach (K tObject in tListToUse)
                    {
                        tNewData++;
                        K tNextObject = tObject.DuplicateData();
                        if (BasisHelper().m_SearchTag != NWDBasisTag.NoTag)
                        {
                            tNextObject.Tag = BasisHelper().m_SearchTag;
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
                    BasisHelper().New_SetObjectInEdition(tNextObjectSelected);
                    //ReorderListOfManagementByName ();
                    BasisHelper().m_PageSelected = BasisHelper().m_MaxPage * 3;
                    NWDDataInspector.ActiveRepaint();
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_UPDATE, NWDGUI.KTableSearchButton))
                {
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
                    {
                        if (tKeyValue.Value == true)
                        {
                            K tObject = tKeyValue.Key as K;
                            tObject.UpdateData();
                            tObject.RowAnalyze();
                        }
                    }
                    NWDDataInspector.ActiveRepaint();
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                //EditorGUI.EndDisabledGroup();
                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_EXPORT_TRANSLATION, NWDGUI.KTableSearchButton))
                {
                    tLocalizeLocalTable = true;
                }


                // Change Colmun
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = tRectActionLeft.y;

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

                if (false)
                {
                    if (GUI.Button(tRect, "UnLoad", NWDGUI.KTableSearchButton))
                    {
                        List<NWDTypeClass> tListToUnload = new List<NWDTypeClass>();
                        foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
                        {
                            if (tKeyValue.Value == true)
                            {
                                tListToUnload.Add(tKeyValue.Key);
                            }
                        }
                        foreach (NWDTypeClass tObject in tListToUnload)
                        {
                            UnloadDataByReference(tObject.Reference);
                        }
                    }
                    tRect.y += tRect.height + NWDGUI.kFieldMarge;
                }
                EditorGUI.EndDisabledGroup();
                NWDGUI.EndRedArea();

                tRect = new Rect(tRectActionRight.x, tRectActionRight.y, NWDGUI.KTableSearchWidth, tRectActionRight.height);

                GUIContent tDevContent = new GUIContent(NWDConstants.K_DEVELOPMENT_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().DevEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
                GUI.Label(tRect, tDevContent, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Sync table", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
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
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    PullFromWebService(NWDAppConfiguration.SharedInstance().DevEnvironment);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Force Pull table", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    PullFromWebServiceForce(NWDAppConfiguration.SharedInstance().DevEnvironment);
                }


                // Change Colmun
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = tRectActionRight.y;



                GUIContent tPreprodContent = new GUIContent(NWDConstants.K_PREPRODUCTION_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().PreprodEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
                GUI.Label(tRect, tPreprodContent, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Sync table", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
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
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    PullFromWebService(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Force Pull table", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    PullFromWebServiceForce(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                }
                // Change Colmun
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = tRectActionRight.y;

                GUIContent tProdContent = new GUIContent(NWDConstants.K_PRODUCTION_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().ProdEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
                GUI.Label(tRect, tProdContent, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                EditorGUI.BeginDisabledGroup(tDisableProd);
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
            if (BasisHelper().TableActions() == true)
            {
                // Start Colmun
                tRect.x = tRectTableLeft.x;
                tRect.y = tRectTableLeft.y;
                GUI.Label(tRect, NWDConstants.K_APP_TABLE_TOOLS, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;


                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_SEARCH_RELOAD, NWDGUI.KTableSearchButton))
                {
                    //Debug.Log(NWDConstants.K_APP_TABLE_SEARCH_RELOAD + "Action");
                    string tReference = GetReferenceOfDataInEdition();
                    GUI.FocusControl(null);
                    BasisHelper().New_SetObjectInEdition(null);
                    BasisHelper().m_SearchInternalName = string.Empty;
                    BasisHelper().m_SearchInternalDescription = string.Empty;
                    //ReloadAllObjects ();
                    //LoadTableEditor ();
                    BasisHelper().New_LoadFromDatabase();
                    RestaureDataInEditionByReference(tReference);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;

                if (GUI.Button(tRect, NWDConstants.K_APP_TABLE_SHOW_TOOLS, NWDGUI.KTableSearchButton))
                {
                    NWDBasisClassInspector tBasisInspector = ScriptableObject.CreateInstance<NWDBasisClassInspector>();
                    tBasisInspector.mTypeInEdition = ClassType();
                    Selection.activeObject = tBasisInspector;
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                //NWDGUI.BeginRedButton()
                //if (GUI.Button(tRect, NWDConstants.K_APP_BASIS_CLASS_INTEGRITY_REEVALUE, NWDConstants.KTableSearchButton)
                //
                //    GUI.FocusControl(null)
                //    RecalculateAllIntegrities()
                //
                //NWDGUI.EndRedButton()
                //tRect.y += tRect.height + NWDGUI.kFieldMarge
                // reset icon of clas
                if (GUI.Button(tRect, "Select script", NWDGUI.KTableSearchButton))
                {
                    BasisHelper().SelectScript();
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // reset icon of class
                NWDGUI.BeginRedArea();
                if (GUI.Button(tRect, "Reset Icon", NWDGUI.KTableSearchButton))
                {
                    BasisHelper().ResetIconByDefaultIcon();
                }
                NWDGUI.EndRedArea();
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
                // draw Clean table
                if (GUI.Button(tRect, "Clean local table", NWDGUI.KTableSearchButton))
                {
                    tCleanLocalTable = true;
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw Purge table
                EditorGUI.BeginDisabledGroup(!GameSaveDependent());
                if (GUI.Button(tRect, "Purge accounts", NWDGUI.KTableSearchButton))
                {
                    tCleanLocalTableWithAccount = true;
                }
                EditorGUI.EndDisabledGroup();
                tRect.y += tRect.height + NWDGUI.kFieldMarge;

                // reintegrate all objects
                NWDGUI.BeginRedArea();
                if (GUI.Button(tRect, NWDConstants.K_APP_BASIS_CLASS_INTEGRITY_REEVALUE, NWDGUI.KTableSearchButton))
                {
                    GUI.FocusControl(null);
                    RecalculateAllIntegrities();
                }
                NWDGUI.EndRedArea();
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
                // draw Replace WS in all by sftp
                if (GUI.Button(tRect, NWDConstants.K_APP_WS_PHP_TOOLS.Replace("XXXX", NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000")), NWDGUI.KTableSearchButton))
                {
                    tCreateAllPHPForOnlyThisClass = true;
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw reintegrate the model
                if (GUI.Button(tRect, NWDConstants.K_APP_WS_MODEL_TOOLS, NWDGUI.KTableSearchButton))
                {
                    tReintegrateOnlyThisClass = true;
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                // draw delete old model
                if (GUI.Button(tRect, NWDConstants.K_APP_WS_DELETE_OLD_MODEL_TOOLS, NWDGUI.KTableSearchButton))
                {
                    tDeleteOldModelOnlyThisClass = true;
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                NWDGUI.EndRedArea();
                // Change Colmun
                tRect.x = tRectTableRight.x;
                tRect.y = tRectTableRight.y;
                GUIContent tDevContent = new GUIContent(NWDConstants.K_DEVELOPMENT_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().DevEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
                GUI.Label(tRect, tDevContent, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                NWDGUI.BeginRedArea();
                if (GUI.Button(tRect, "Clean table", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceClean(NWDAppConfiguration.SharedInstance().DevEnvironment);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Special", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().DevEnvironment, NWDOperationSpecial.Special);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Upgrade", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().DevEnvironment, NWDOperationSpecial.Upgrade);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Optimize", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().DevEnvironment, NWDOperationSpecial.Optimize);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                NWDGUI.EndRedArea();
                // Change Colmun
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = tRectTableRight.y;
                GUIContent tPreprodContent = new GUIContent(NWDConstants.K_PREPRODUCTION_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().PreprodEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
                GUI.Label(tRect, tPreprodContent, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                NWDGUI.BeginRedArea();
                if (GUI.Button(tRect, "Clean table", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceClean(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Special", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().PreprodEnvironment, NWDOperationSpecial.Special);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Upgrade", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().PreprodEnvironment, NWDOperationSpecial.Upgrade);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Optimize", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().PreprodEnvironment, NWDOperationSpecial.Optimize);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                NWDGUI.EndRedArea();
                // Change Colmun
                tRect.x += tRect.width + NWDGUI.kFieldMarge;
                tRect.y = tRectTableRight.y;
                GUIContent tProdContent = new GUIContent(NWDConstants.K_PRODUCTION_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().ProdEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
                GUI.Label(tRect, tProdContent, NWDGUI.KTableSearchTitle);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                NWDGUI.BeginRedArea();
                if (GUI.Button(tRect, "Clean table", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    tSyncCleanProd = true;
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Special", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().ProdEnvironment, NWDOperationSpecial.Special);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Upgrade", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().ProdEnvironment, NWDOperationSpecial.Upgrade);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                if (GUI.Button(tRect, "Optimize", NWDGUI.KTableSearchButton))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().ProdEnvironment, NWDOperationSpecial.Optimize);
                }
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                NWDGUI.EndRedArea();

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
                    List<object> tListToDelete = new List<object>();
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
                    {
                        if (tKeyValue.Value == true)
                        {
                            NWDBasis<K> tObject = tKeyValue.Key as NWDBasis<K>;
                            tListToDelete.Add((object)tObject);
                        }
                    }
                    foreach (object tObject in tListToDelete)
                    {
                        NWDBasis<K> tObjectToDelete = (NWDBasis<K>)tObject;
                        //RemoveObjectInListOfEdition(tObjectToDelete);
                        tObjectToDelete.DeleteData();
                        //tObjectToDelete.RowAnalyze();
                    }
                    BasisHelper().New_SetObjectInEdition(null);
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
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

                    List<object> tListToTrash = new List<object>();
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
                    {
                        if (tKeyValue.Value == true)
                        {
                            NWDBasis<K> tObject = tKeyValue.Key as NWDBasis<K>;
                            tListToTrash.Add((object)tObject);
                        }
                    }
                    foreach (object tObject in tListToTrash)
                    {
                        NWDBasis<K> tObjectToTrash = (NWDBasis<K>)tObject;
                        tObjectToTrash.TrashData();
                        tObjectToTrash.RowAnalyze();
                    }
                    BasisHelper().New_SetObjectInEdition(null);
                    //                  sEditorWindow.Repaint ();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
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

                    List<object> tListToUntrash = new List<object>();
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
                    {
                        if (tKeyValue.Value == true)
                        {
                            NWDBasis<K> tObject = tKeyValue.Key as NWDBasis<K>;
                            tListToUntrash.Add((object)tObject);
                        }
                    }
                    foreach (object tObject in tListToUntrash)
                    {
                        NWDBasis<K> tObjectToUntrash = (NWDBasis<K>)tObject;
                        tObjectToUntrash.UnTrashData();
                        tObjectToUntrash.RowAnalyze();
                    }
                    BasisHelper().New_SetObjectInEdition(null);
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
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
                    List<object> tListToReintegrate = new List<object>();
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
                    {
                        if (tKeyValue.Value == true)
                        {
                            NWDBasis<K> tObject = tKeyValue.Key as NWDBasis<K>;
                            tListToReintegrate.Add((object)tObject);
                        }
                    }
                    foreach (object tObject in tListToReintegrate)
                    {
                        NWDBasis<K> tObjectToReintegrate = (NWDBasis<K>)tObject;
                        //tObjectToReintegrate.UpdateIntegrity();
                        tObjectToReintegrate.UpdateData();
                        tObjectToReintegrate.RowAnalyze();
                    }
                    BasisHelper().New_SetObjectInEdition(null);
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
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
                    NWDBasis<K>.BasisHelper().New_ResetTable();
                }
            }
            if (tPullProd == true)
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                }
                PullFromWebService(NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }
            if (tPullProdForce == true)
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                }
                PullFromWebServiceForce(NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }

            if (tSyncProd == true)
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                }
                SynchronizationFromWebService(NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }

            if (tSyncForceProd == true)
            {

                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                }
                SynchronizationFromWebServiceForce(NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }
            if (tSyncCleanProd == true)
            {

                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                }
                SynchronizationFromWebServiceClean(NWDAppConfiguration.SharedInstance().ProdEnvironment);
            }
            if (tSyncSpecialProd == true)
            {

                if (Application.isPlaying == true && AccountDependent() == false)
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
                    BasisHelper().New_CleanTable();
                }
            }
            if (tCleanLocalTableWithAccount == true)
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_PURGE_ALERT_TITLE,
                            NWDConstants.K_PURGE_ALERT_MESSAGE,
                            NWDConstants.K_PURGE_ALERT_OK,
                            NWDConstants.K_PURGE_ALERT_CANCEL))
                {
                    BasisHelper().New_PurgeTable();
                }
            }
            if (tLocalizeLocalTable == true)
            {
                BasisHelper().New_ExportLocalization();
            }
            if (tCreateAllPHPForOnlyThisClass == true)
            {
                NWDAppConfiguration.SharedInstance().DevEnvironment.CreatePHP(new List<Type> { typeof(K) }, false, false);
                NWDAppConfiguration.SharedInstance().PreprodEnvironment.CreatePHP(new List<Type> { typeof(K) }, false, false);
                NWDAppConfiguration.SharedInstance().ProdEnvironment.CreatePHP(new List<Type> { typeof(K) }, false, false);
            }
            if (tReintegrateOnlyThisClass == true)
            {
                ForceOrders(NWDAppConfiguration.SharedInstance().WebBuild);
                NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            }
            if (tDeleteOldModelOnlyThisClass == true)
            {
                DeleteOldsModels();
                NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            }
            if (tCreateAllPHPForOnlyThisClassDEV == true)
            {
                NWDAppConfiguration.SharedInstance().DevEnvironment.CreatePHP(new List<Type> { typeof(K) }, false, false);
            }

            //EditorGUI.DrawRect(rRect, Color.yellow);
            //rRect.height += NWDGUI.KTableSearchTextfield.fixedHeight + NWDGUI.kFieldMarge * 3;
            //Debug.Log("rRect height : " +rRect.height);
            //rRect.height = 0;
            return rRect;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draws the table editor.
        /// </summary>
        public static void DrawTableEditor(EditorWindow sEditorWindow)
        {
            //BTBBenchmark.Start();
            bool rLoaded = DatabaseIsLoaded();

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
            //GUI.DrawTexture(tRectToogle, NWDToolbox.TextureFromColor(Color.red));
            BasisHelper().SetSearchActions(GUI.Toggle(tRectToogle, BasisHelper().SearchActions(), "Search"));
            tRectToogle.x += tWidthTiers;
            BasisHelper().SetRowActions(GUI.Toggle(tRectToogle, BasisHelper().RowActions(), "Actions"));
            tRectToogle.x += tWidthTiers;
            BasisHelper().SetTableActions(GUI.Toggle(tRectToogle, BasisHelper().TableActions(), "Table"));

            tRect.y += NWDGUI.kToggleStyle.fixedHeight + NWDGUI.kFieldMarge;

            tRect.y += NWDGUI.Line(tRect).height;

            // if necessary recalcul row informations
            BasisHelper().RowAnalyze();
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
                //tRect.y += NWDGUI.kFieldMarge;
                Rect tRectInfos = new Rect(tRect.x + NWDGUI.kFieldMarge, tRect.y, tRect.width - NWDGUI.kFieldMarge * 2, tRect.height);
                EditorGUI.HelpBox(tRectInfos, NWDConstants.K_ALERT_SALT_SHORT_ERROR, MessageType.Error);
                tRect.y += tRect.height + NWDGUI.kFieldMarge;
                tRect.height = NWDGUI.KTableSearchButton.fixedHeight;
                Rect tRectButton = new Rect(tRect.x + NWDGUI.kFieldMarge, tRect.y, tRect.width - NWDGUI.kFieldMarge * 2, tRect.height);
                if (GUI.Button(tRectButton, NWDConstants.K_APP_CLASS_SALT_REGENERATE, NWDGUI.KTableSearchButton))
                {
                    NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                    GUIUtility.ExitGUI();
                }
                tRect.y += NWDGUI.KTableSearchButton.fixedHeight + NWDGUI.kFieldMarge;
            }
            // Alert warning model infos
            if (BasisHelper().WebModelChanged == true)
            {
                if (tMargeNeed == false)
                {
                    tRect.y += NWDGUI.kFieldMarge;
                }
                tMargeNeed = true;
                //string tTEXTWARNING = "<b><color=red>" + NWDConstants.K_APP_BASIS_WARNING_MODEL + "</color></b>";
                //GUIContent tCC = new GUIContent(tTEXTWARNING);
                //GUIStyle tWarningBoxStyle = new GUIStyle(EditorStyles.boldLabel);
                //tWarningBoxStyle.normal.background = new Texture2D(1, 1);
                //tWarningBoxStyle.normal.background.SetPixel(0, 0, Color.yellow);
                //tWarningBoxStyle.normal.background.Apply();
                //tWarningBoxStyle.alignment = TextAnchor.MiddleCenter;
                //tWarningBoxStyle.richText = true;
                //tRect.height = tWarningBoxStyle.CalcHeight(tCC, tRect.width);
                //tRect.y += NWDGUI.kFieldMarge;
                //GUI.Label(tRect, tCC, tWarningBoxStyle);
                //tRect.y += tRect.height + NWDGUI.kFieldMarge;

                tRect.y += NWDGUI.WarningBox(NWDGUI.MargeLeftRight(tRect), NWDConstants.K_APP_BASIS_WARNING_MODEL).height + NWDGUI.kFieldMarge;

            }
            // alert degadraded model infos
            if (BasisHelper().WebModelDegraded == true)
            {
                if (tMargeNeed == false)
                {
                    tRect.y += NWDGUI.kFieldMarge;
                }
                tMargeNeed = true;
                //string tTEXTWARNING = "<b><color=red>" + NWDConstants.K_APP_BASIS_WARNING_MODEL_DEGRADED + "</color></b>";
                //GUIContent tCC = new GUIContent(tTEXTWARNING);
                //GUIStyle tWarningBoxStyle = new GUIStyle(EditorStyles.boldLabel);
                //tWarningBoxStyle.normal.background = new Texture2D(1, 1);
                //tWarningBoxStyle.normal.background.SetPixel(0, 0, Color.yellow);
                //tWarningBoxStyle.normal.background.Apply();
                //tWarningBoxStyle.alignment = TextAnchor.MiddleCenter;
                //tWarningBoxStyle.richText = true;
                //tRect.height = tWarningBoxStyle.CalcHeight(tCC, tRect.width);
                //tRect.y += NWDGUI.kFieldMarge;
                //GUI.Label(tRect, tCC, tWarningBoxStyle);
                //tRect.y += tRect.height + NWDGUI.kFieldMarge;
                tRect.y += NWDGUI.WarningBox(NWDGUI.MargeLeftRight(tRect), NWDConstants.K_APP_BASIS_WARNING_MODEL_DEGRADED).height + NWDGUI.kFieldMarge;

            }
            if (BasisHelper().SearchActions() == true)
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
            if (BasisHelper().Datas.Count > BasisHelper().m_ItemPerPage)
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
                if (NWDLauncher.GetState() == NWDStatut.DataAccountCodePinStop)
                {
                    tDialogRect.height -= NWDGUI.kFieldMarge;
                    Rect tDialogRectBox = NWDGUI.UnMargeAll(tDialogRect);
                    GUI.Label(tDialogRectBox, "", EditorStyles.helpBox);
                    tDialogRect.height += NWDGUI.kTextFieldStyle.fixedHeight;
                    tDialogRect.y += NWDGUI.WarningBox(tDialogRect, NWDLauncher.GetState().ToString()).height + NWDGUI.kFieldMarge;
                    tDialogRect.height = NWDGUI.kTextFieldStyle.fixedHeight;
                    tDialogRect.y += NWDGUI.ErrorBox(tDialogRect, "TOO MUCH TENTATIVE").height + NWDGUI.kFieldMarge;
                }
                else if (NWDLauncher.GetState() == NWDStatut.DataAccountCodePinRequest || NWDLauncher.GetState() == NWDStatut.DataAccountCodePinFail)
                {
                    tDialogRect.height -= NWDGUI.kFieldMarge;
                    Rect tDialogRectBox = NWDGUI.UnMargeAll(tDialogRect);
                    GUI.Label(tDialogRectBox, "", EditorStyles.helpBox);
                    tDialogRect.height += NWDGUI.kTextFieldStyle.fixedHeight;
                    tDialogRect.y += NWDGUI.WarningBox(tDialogRect, NWDLauncher.GetState().ToString()).height + NWDGUI.kFieldMarge;

                    tDialogRect.height = NWDGUI.kTextFieldStyle.fixedHeight;
                    NWDLauncher.CodePinValue = EditorGUI.PasswordField(tDialogRect, "CodePin", NWDLauncher.CodePinValue, NWDGUI.kTextFieldStyle);
                    tDialogRect.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;

                    tDialogRect.height = NWDGUI.kMiniButtonStyle.fixedHeight;
                    if (GUI.Button(tDialogRect, "Valid", NWDGUI.kMiniButtonStyle))
                    {
                        //Debug.LogWarning("Try this code");
                        NWDLauncher.DatabaseAccountConnection(NWDLauncher.CodePinValue);
                    }
                    tDialogRect.y += (NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge);
                    tDialogRect.y += NWDGUI.Separator(tDialogRect).height;
                    NWDGUI.BeginRedArea();
                    if (GUI.Button(tDialogRect, "DELETE ACCOUNT BASE", NWDGUI.kMiniButtonStyle))
                    {
                        Debug.LogWarning("Delete account base?!");
                        if (EditorUtility.DisplayDialog("DELETE ACCOUNT DATABASE", "YOU WILL DELETE ACCOUNT DATABASE! ARE YOU SURE?", "DELETE!", "CANCEL"))
                        {
                            NWDDataManager.SharedInstance().DeleteDatabaseAccount();
                            NWDLauncher.Launch();
                        }
                        GUI.FocusControl(null);
                        GUIUtility.ExitGUI();
                    }
                    tDialogRect.y += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                    NWDGUI.EndRedArea();

                }
                else if (NWDLauncher.GetState() == NWDStatut.DataAccountCodePinCreate)
                {
                    tDialogRect.height += NWDGUI.kFieldMarge;
                    Rect tDialogRectBox = NWDGUI.UnMargeAll(tDialogRect);
                    GUI.Label(tDialogRectBox, "", EditorStyles.helpBox);
                    tDialogRect.height += NWDGUI.kTextFieldStyle.fixedHeight;
                    tDialogRect.y += NWDGUI.WarningBox(tDialogRect, NWDLauncher.GetState().ToString()).height + NWDGUI.kFieldMarge;

                    tDialogRect.height = NWDGUI.kTextFieldStyle.fixedHeight;
                    NWDLauncher.CodePinValue = EditorGUI.PasswordField(tDialogRect, "CodePin", NWDLauncher.CodePinValue, NWDGUI.kTextFieldStyle);
                    tDialogRect.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                    NWDLauncher.CodePinValueConfirm = EditorGUI.PasswordField(tDialogRect, "CodePin confirm", NWDLauncher.CodePinValueConfirm, NWDGUI.kTextFieldStyle);
                    tDialogRect.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                    NWDGUI.BeginRedArea();
                    bool tValid = true;
                    if (string.IsNullOrEmpty(NWDLauncher.CodePinValue))
                    {
                        NWDLauncher.CodePinValue = string.Empty;
                    }
                    if (string.IsNullOrEmpty(NWDLauncher.CodePinValueConfirm))
                    {
                        NWDLauncher.CodePinValueConfirm = string.Empty;
                    }
                    if (NWDLauncher.CodePinValue != NWDLauncher.CodePinValueConfirm)
                    {
                        tValid = false;
                    }
                    if (NWDLauncher.CodePinValue.Length < NWDAppConfiguration.SharedInstance().PinCodeLenghtMin || NWDLauncher.CodePinValue.Length > NWDAppConfiguration.SharedInstance().PinCodeLenghtMax)
                    {
                        tValid = false;
                    }
                    if (string.IsNullOrEmpty(NWDLauncher.CodePinValue))
                    {
                        tValid = false;
                    }
                    if (NWDLauncher.CodePinValue.Length < 4)
                    {
                        tValid = false;
                    }
                    if (tValid == true)
                    {
                        EditorGUI.LabelField(tDialogRect, "valid CodePin");
                    }
                    else
                    {
                        EditorGUI.LabelField(tDialogRect, "invalid CodePin");
                    }
                    tDialogRect.y += NWDGUI.kTextFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                    EditorGUI.BeginDisabledGroup(!tValid);
                    if (GUI.Button(tDialogRect, "CREATE ACCOUNT BASE", NWDGUI.kMiniButtonStyle))
                    {
                        Debug.LogWarning("Create account base?!");
                        GUI.FocusControl(null);
                        NWDLauncher.DatabaseAccountConnection(NWDLauncher.CodePinValue);
                    }
                    tDialogRect.y += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
                    EditorGUI.EndDisabledGroup();
                    NWDGUI.EndRedArea();
                }
                else if (NWDLauncher.GetState() == NWDStatut.DataEditorLoaded)
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

                float tRowHeight = NWDGUI.kTableRowHeight * BasisHelper().RowZoom;
                // ===========================================
                // Get index rows for this page
                int tIndexPageStart = BasisHelper().m_ItemPerPage * BasisHelper().m_PageSelected;
                int tIndexPageStop = tIndexPageStart + BasisHelper().m_ItemPerPage;
                // Limit to max
                if (tIndexPageStop > BasisHelper().EditorTableDatas.Count)
                {
                    tIndexPageStop = BasisHelper().EditorTableDatas.Count;
                }
                int tIndexRowInPage = tIndexPageStop - tIndexPageStart + 1;
                // create rects for scrollview
                // TODO get put this line!
                //NWDGUI.kRowHeaderHeight = NWDConstants.KTableSearchToggle.fixedHeight;

                //if (tPagesBarHeight > 0)
                //{
                //    tScrollHeight += NWDGUI.kFieldMarge;
                //}
                // draw headers

                Rect tScrollContentRect = new Rect(0, 0, tWindowRect.width - NWDGUI.kScrollbar, tIndexRowInPage * tRowHeight);


                Rect tScrollHeader = new Rect(tScrollRect.x, tScrollRect.y - NWDGUI.kTableHeaderHeight, tWindowRect.width, NWDGUI.kTableHeaderHeight);
                Rect tScrollHeaderBottom = new Rect(tScrollRect.x, tScrollRect.y + tScrollRect.height, tWindowRect.width, NWDGUI.kTableHeaderHeight);

                DrawHeaderInEditor(tScrollHeader, tScrollRect, BasisHelper().RowZoom);
                DrawHeaderBottomInEditor(tScrollHeaderBottom, tScrollRect);
                // Get only visible rows
                int tIndexMax = Mathf.Min(Mathf.FloorToInt(tScrollRect.height / tRowHeight), BasisHelper().m_ItemPerPage);
                int tIndexStart = Mathf.FloorToInt(BasisHelper().m_ScrollPositionList.y / tRowHeight);
                int tIndexStop = tIndexStart + tIndexMax;
                if (tIndexStop - tIndexStart < BasisHelper().m_ItemPerPage - 1)
                {
                    tIndexStop++;
                }
                BasisHelper().m_ScrollPositionList = GUI.BeginScrollView(
                    tScrollRect,
                    BasisHelper().m_ScrollPositionList,
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
                    NWDBasis<K> tSelected = NWDDataInspector.ObjectInEdition() as NWDBasis<K>;
                    if (tSelected != null)
                    {
                        if (BasisHelper().DatasByReference.ContainsKey(tSelected.Reference))
                        {
                            //if (tSelected.XX == 0 && tSelected.TestIntegrity())
                            {
                                //int tIndex = Datas().ObjectsByReferenceList.IndexOf(tSelected.Reference);
                                if (BasisHelper().EditorTableDatasSelected.ContainsKey(tSelected))
                                {
                                    BasisHelper().EditorTableDatasSelected[tSelected] = !BasisHelper().EditorTableDatasSelected[tSelected];
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
                    NWDBasis<K> tSelected = NWDDataInspector.ObjectInEdition() as NWDBasis<K>;
                    if (tSelected != null)
                    {
                        if (BasisHelper().EditorTableDatas.Contains(tSelected))
                        {
                            int tIndexSelected = BasisHelper().EditorTableDatas.IndexOf(tSelected);
                            if (tIndexSelected < BasisHelper().EditorTableDatas.Count - 1)
                            {
                                K tNextSelected = BasisHelper().EditorTableDatas.ElementAt(tIndexSelected + 1) as K;
                                BasisHelper().New_SetObjectInEdition(tNextSelected);
                                //float tNumberPage = (tIndexSelected + 1) / BasisHelper().m_ItemPerPage;
                                //int tPageExpected = (int)Math.Floor(tNumberPage);
                                //BasisHelper().m_PageSelected = tPageExpected;
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
                    NWDBasis<K> tSelected = NWDDataInspector.ObjectInEdition() as NWDBasis<K>;
                    if (tSelected != null)
                    {
                        if (BasisHelper().EditorTableDatas.Contains(tSelected))
                        {
                            int tIndexSelected = BasisHelper().EditorTableDatas.IndexOf(tSelected);
                            if (tIndexSelected > 0)
                            {
                                K tNextSelected = BasisHelper().EditorTableDatas.ElementAt(tIndexSelected - 1) as K;
                                BasisHelper().New_SetObjectInEdition(tNextSelected);
                                //float tNumberPage = (tIndexSelected - 1) / BasisHelper().m_ItemPerPage;
                                //int tPageExpected = (int)Math.Floor(tNumberPage);
                                //BasisHelper().m_PageSelected = tPageExpected;
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
                float tNumberOfPage = BasisHelper().EditorTableDatas.Count / BasisHelper().m_ItemPerPage;
                int tPagesExpected = (int)Math.Floor(tNumberOfPage);
                // TODO: add instruction in tab view
                // KEY RIGHT ARROW
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.RightArrow)
                {
                    //Debug.LogVerbose ("KeyDown RightArrow", DebugResult.Success);
                    if (BasisHelper().m_PageSelected < tPagesExpected)
                    {
                        BasisHelper().m_PageSelected++;
                        // TODO : reselect first object
                        int tIndexSel = BasisHelper().m_ItemPerPage * BasisHelper().m_PageSelected;
                        if (tIndexSel < BasisHelper().EditorTableDatas.Count)
                        {
                            K tNextSelected = BasisHelper().EditorTableDatas.ElementAt(tIndexSel) as K;
                            BasisHelper().New_SetObjectInEdition(tNextSelected);
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
                    if (BasisHelper().m_PageSelected > 0)
                    {
                        BasisHelper().m_PageSelected--;
                        // TODO : reselect first object
                        K tNextSelected = BasisHelper().EditorTableDatas.ElementAt(BasisHelper().m_ItemPerPage * BasisHelper().m_PageSelected) as K;
                        BasisHelper().New_SetObjectInEdition(tNextSelected);
                        ChangeScroolPositionToSelection(tScrollRect);
                        Event.current.Use();
                        sEditorWindow.Focus();
                    }
                    else
                    {
                    }
                }
                if (BasisHelper().m_PageSelected < 0)
                {
                    BasisHelper().m_PageSelected = 0;
                }
                if (BasisHelper().m_PageSelected > tPagesExpected)
                {
                    BasisHelper().m_PageSelected = tPagesExpected;
                }

                Rect tRectRow = new Rect(sEditorWindow.position.x, sEditorWindow.position.y,
                     sEditorWindow.position.width, sEditorWindow.position.height);

                if (tRectRow.width < NWDGUI.KTableMinWidth)
                {
                    tRectRow.width = NWDGUI.KTableMinWidth;
                }
                for (int i = tIndexStart; i < tIndexStop; i++)
                {
                    int tItemIndexInPage = BasisHelper().m_ItemPerPage * BasisHelper().m_PageSelected + i;
                    if (tItemIndexInPage >= 0 && tItemIndexInPage < BasisHelper().EditorTableDatas.Count)
                    {
                        K tObject = BasisHelper().EditorTableDatas.ElementAt(tItemIndexInPage) as K;
                        tObject.DrawRowInEditor(tMousePosition, tRectRow, tSelectAndClick, i, BasisHelper().RowZoom);
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


                /*
                GUILayout.BeginHorizontal(NWDConstants.KTableAreaColorDark);
                // draw number of page enum
                t_ItemPerPageSelection = EditorGUILayout.Popup(BasisHelper().m_ItemPerPageSelection, BasisHelper().m_ItemPerPageOptions, NWDConstants.KTableSearchEnum, GUILayout.Width(NWDConstants.KTableSearchWidth));
                if (t_ItemPerPageSelection != BasisHelper().m_ItemPerPageSelection)
                {
                    BasisHelper().m_PageSelected = 0;
                }
                BasisHelper().m_ItemPerPageSelection = t_ItemPerPageSelection;
                // draw toogle enable
                bool t_ShowEnableLine = EditorGUILayout.ToggleLeft(NWDConstants.K_APP_TABLE_SHOW_ENABLE_DATAS, BasisHelper().m_ShowEnable,
                 GUILayout.Height(NWDConstants.KTableSearchTextfield.fixedHeight), GUILayout.Width(NWDConstants.KTableSearchWidth));
                if (BasisHelper().m_ShowEnable != t_ShowEnableLine)
                {
                    BasisHelper().m_ShowEnable = t_ShowEnableLine;
                    FilterTableEditor();
                }
                // draw toogle disable
                bool t_ShowDisableLine = EditorGUILayout.ToggleLeft(NWDConstants.K_APP_TABLE_SHOW_DISABLE_DATAS, BasisHelper().m_ShowDisable,
                 GUILayout.Height(NWDConstants.KTableSearchTextfield.fixedHeight), GUILayout.Width(NWDConstants.KTableSearchWidth));
                if (BasisHelper().m_ShowDisable != t_ShowDisableLine)
                {
                    BasisHelper().m_ShowDisable = t_ShowDisableLine;
                    FilterTableEditor();
                }
                // draw toogle trashed
                EditorGUI.BeginDisabledGroup(!BasisHelper().m_ShowDisable);
                bool t_ShowTrashedLine = EditorGUILayout.ToggleLeft(NWDConstants.K_APP_TABLE_SHOW_TRASHED_DATAS, BasisHelper().m_ShowTrashed,
                 GUILayout.Height(NWDConstants.KTableSearchTextfield.fixedHeight), GUILayout.Width(NWDConstants.KTableSearchWidth));
                if (BasisHelper().m_ShowTrashed != t_ShowTrashedLine)
                {
                    BasisHelper().m_ShowTrashed = t_ShowTrashedLine;
                    FilterTableEditor();
                }
                // draw toogle corrupted
                EditorGUI.EndDisabledGroup();
                bool t_ShowIntegrityErrorLine = EditorGUILayout.ToggleLeft(NWDConstants.K_APP_TABLE_SHOW_INTEGRITY_ERROR_DATAS, BasisHelper().m_ShowIntegrityError,
                 GUILayout.Height(NWDConstants.KTableSearchTextfield.fixedHeight), GUILayout.Width(NWDConstants.KTableSearchWidth));
                if (BasisHelper().m_ShowIntegrityError != t_ShowIntegrityErrorLine)
                {
                    BasisHelper().m_ShowIntegrityError = t_ShowIntegrityErrorLine;
                    FilterTableEditor();
                }
                bool tShowMoreInfos = false;
                if (tShowMoreInfos)
                {
                    // draw objects in database
                    tRealReference = BasisHelper().Datas.Count;
                    if (tRealReference == 0)
                    {
                        GUILayout.Label(NWDConstants.K_APP_TABLE_NO_OBJECT, NWDConstants.KTableSearchLabel, GUILayout.Width(NWDConstants.KTableSearchWidth));
                    }
                    else if (tRealReference == 1)
                    {
                        GUILayout.Label(NWDConstants.K_APP_TABLE_ONE_OBJECT, NWDConstants.KTableSearchLabel, GUILayout.Width(NWDConstants.KTableSearchWidth));
                    }
                    else
                    {
                        GUILayout.Label(tRealReference + NWDConstants.K_APP_TABLE_X_OBJECTS, NWDConstants.KTableSearchLabel, GUILayout.Width(NWDConstants.KTableSearchWidth));
                    }
                    // draw objects in results
                    tResultReference = BasisHelper().EditorTableDatas.Count;
                    if (tResultReference == 0)
                    {
                        GUILayout.Label(NWDConstants.K_APP_TABLE_NO_OBJECT_FILTERED, NWDConstants.KTableSearchLabel, GUILayout.Width(NWDConstants.KTableSearchWidth));
                    }
                    else if (tResultReference == 1)
                    {
                        GUILayout.Label(NWDConstants.K_APP_TABLE_ONE_OBJECT_FILTERED, NWDConstants.KTableSearchLabel, GUILayout.Width(NWDConstants.KTableSearchWidth));
                    }
                    else
                    {
                        GUILayout.Label(tResultReference + NWDConstants.K_APP_TABLE_X_OBJECTS_FILTERED, NWDConstants.KTableSearchLabel, GUILayout.Width(NWDConstants.KTableSearchWidth));
                    }
                    // draw selection
                    if (tSelectionCount == 0)
                    {
                        GUILayout.Label(NWDConstants.K_APP_TABLE_NO_SELECTED_OBJECT, NWDConstants.KTableSearchLabel, GUILayout.Width(NWDConstants.KTableSearchWidth));
                    }
                    else if (tSelectionCount == 1)
                    {
                        GUILayout.Label(NWDConstants.K_APP_TABLE_ONE_SELECTED_OBJECT, NWDConstants.KTableSearchLabel, GUILayout.Width(NWDConstants.KTableSearchWidth));
                    }
                    else
                    {
                        GUILayout.Label(tSelectionCount + NWDConstants.K_APP_TABLE_XX_SELECTED_OBJECT, NWDConstants.KTableSearchLabel, GUILayout.Width(NWDConstants.KTableSearchWidth));
                    }
                }
                GUILayout.FlexibleSpace();
                if (GUILayout.Button(NWDConstants.K_APP_TABLE_ADD_ROW, NWDConstants.KTableSearchButton, GUILayout.Width(NWDConstants.KTableSearchWidth)))
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
                GUILayout.EndHorizontal();
                */
                // ===========================================

                EditorGUI.EndDisabledGroup();
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif