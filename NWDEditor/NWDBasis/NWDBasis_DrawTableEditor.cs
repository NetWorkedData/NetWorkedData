//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================


using System;

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;
using System.IO;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------

        /// <summary>
        /// The scroll position list.
        /// </summary>
        //public static Vector2 m_ScrollPositionList;

        ///// <summary>
        ///// The number of items per page.
        ///// </summary>
        //public static int m_ItemPerPage = 10;

        ///// <summary>
        ///// The number of items per page.
        ///// </summary>
        //public static int m_ItemPerPageSelection = 0;

        //public static string[] m_ItemPerPageOptions = new string[] {
        //    "15", "20", "30", "40", "50", "100", "200",
        //};

        ///// <summary>
        ///// The selected page.
        ///// </summary>
        //public static int m_PageSelected = 0;

        ///// <summary>
        ///// The selected page.
        ///// </summary>
        //public static int m_MaxPage = 0;

        ///// <summary>
        ///// The toogle to list page limit.
        ///// </summary>
        ////public static int m_ToogleToListPageLimit = 15;


        //public static bool m_ShowEnable = true;
        //public static bool m_ShowDisable = true;
        //public static bool m_ShowTrashed = true;
        //public static bool m_ShowIntegrityError = true;

        //-------------------------------------------------------------------------------------------------------------
        public static void RepaintTableEditor()
        {
            NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void RepaintInspectorEditor()
        {
            NWDDataInspector.ActiveRepaint();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DrawPagesTab()
        {
            float tWidth = EditorGUIUtility.currentViewWidth;
            //			float tWidth = EditorGUIUtility.fieldWidth;
            float tTabWidth = 35.0f;
            float tPopupWidth = 60.0f;
            //Debug.Log ("tWidth = " + tWidth);
            int tToogleToListPageLimit = (int)Math.Floor(tWidth / tTabWidth);
            //Debug.Log ("tToogleToListPageLimit = " + tToogleToListPageLimit);
            //			kObjectsInTableList
            GUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            Datas().m_ItemPerPage = int.Parse(Datas().m_ItemPerPageOptions[Datas().m_ItemPerPageSelection]);
            float tNumberOfPage = Datas().ObjectsInEditorTableList.Count / Datas().m_ItemPerPage;
            int tPagesExpected = (int)Math.Floor(tNumberOfPage);
            if (tPagesExpected != 0)
            {
                if (Datas().ObjectsInEditorTableList.Count % (tPagesExpected * Datas().m_ItemPerPage) != 0)
                {
                    tPagesExpected++;
                }
            }
            if (Datas().m_PageSelected > tPagesExpected - 1)
            {
                Datas().m_PageSelected = tPagesExpected - 1;
            }
            Datas().m_MaxPage = tPagesExpected + 1;
            string[] tListOfPagesName = new string[tPagesExpected];
            for (int p = 0; p < tPagesExpected; p++)
            {
                int tP = p + 1;
                tListOfPagesName[p] = "" + tP.ToString();
            }
            int t_PageSelected = Datas().m_PageSelected;
            if (tPagesExpected == 0 || tPagesExpected == 1)
            {
                // no choose
                t_PageSelected = 0;
            }
            else if (tPagesExpected < tToogleToListPageLimit)
            {
                //m_PageSelected = GUILayout.Toolbar (m_PageSelected, tListOfPagesName, GUILayout.ExpandWidth (true));
                t_PageSelected = GUILayout.Toolbar(Datas().m_PageSelected, tListOfPagesName, GUILayout.Width(tPagesExpected * tTabWidth));
            }
            else
            {
                t_PageSelected = EditorGUILayout.Popup(Datas().m_PageSelected, tListOfPagesName, EditorStyles.popup, GUILayout.Width(tPopupWidth));
            }
            if (Datas().m_PageSelected != t_PageSelected)
            {
                NWDDataManager.SharedInstance().UpdateQueueExecute();
            }
            Datas().m_PageSelected = t_PageSelected;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draws the table editor.
        /// </summary>
        public static void DrawTableEditor(EditorWindow sEditorWindow)
        {
            GUIStyle tRightLabel = new GUIStyle(EditorStyles.boldLabel);
            tRightLabel.alignment = TextAnchor.MiddleRight;

            GUIStyle tLeftLabel = new GUIStyle(EditorStyles.boldLabel);
            tLeftLabel.alignment = TextAnchor.MiddleLeft;

            GUIStyle tCenterLabel = new GUIStyle(EditorStyles.boldLabel);
            tCenterLabel.alignment = TextAnchor.MiddleCenter;

            //			if (TestSaltValid () == false) {
            //				EditorGUILayout.HelpBox (NWDConstants.kAlertSaltShortError, MessageType.Error);
            //			}
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.kAlertSaltShortError, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
            }

            //EditorGUILayout.BeginScrollView (Vector2.zero, EditorStyles.inspectorFullWidthMargins, GUILayout.ExpandWidth (false), GUILayout.ExpandHeight (false));
            // ===========================================
            GUILayout.BeginHorizontal();
            // -------------------------------------------
            GUILayout.BeginVertical(GUILayout.Width(300));
            GUILayout.Label(NWDConstants.K_APP_TABLE_SEARCH_ZONE, EditorStyles.boldLabel);
            // |||||||||||||||||||||||||||||||||||||||||||
            //m_SearchReference = EditorGUILayout.TextField(NWDConstants.K_APP_TABLE_SEARCH_REFERENCE, m_SearchReference, GUILayout.Width(300));
            Datas().m_SearchReference = EditorGUILayout.TextField(NWDConstants.K_APP_TABLE_SEARCH_REFERENCE, Datas().m_SearchReference, GUILayout.Width(300));


            Datas().m_SearchInternalName = EditorGUILayout.TextField(NWDConstants.K_APP_TABLE_SEARCH_NAME, Datas().m_SearchInternalName, GUILayout.Width(300));
            Datas().m_SearchInternalDescription = EditorGUILayout.TextField(NWDConstants.K_APP_TABLE_SEARCH_DESCRIPTION, Datas().m_SearchInternalDescription, GUILayout.Width(300));




            List<int> tTagIntList = new List<int>();
            List<string> tTagStringList = new List<string>();
            foreach (KeyValuePair<int, string> tTag in NWDAppConfiguration.SharedInstance().TagList)
            {
                tTagIntList.Add(tTag.Key);
                tTagStringList.Add(tTag.Value);
            }

            Datas().m_SearchTag = (NWDBasisTag)EditorGUILayout.IntPopup(NWDConstants.K_APP_TABLE_SEARCH_TAG,
                                                                (int)Datas().m_SearchTag, tTagStringList.ToArray(),
                                                                tTagIntList.ToArray(),
                                                                GUILayout.Width(300));


            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUILayout.Width(120));
            GUILayout.Label(" ", EditorStyles.boldLabel);
            // |||||||||||||||||||||||||||||||||||||||||||
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_SEARCH_FILTER, EditorStyles.miniButton, GUILayout.Width(120)))
            {
                GUI.FocusControl(null);
                SetObjectInEdition(null);
                FilterTableEditor();
            }
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_SEARCH_REMOVE_FILTER, EditorStyles.miniButton, GUILayout.Width(120)))
            {
                GUI.FocusControl(null);
                SetObjectInEdition(null);
                //m_SearchReference = "";
                Datas().m_SearchReference = "";
                Datas().m_SearchInternalName = "";
                Datas().m_SearchInternalDescription = "";
                Datas().m_SearchTag = NWDBasisTag.NoTag;
                FilterTableEditor();
            }
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_SEARCH_SORT, EditorStyles.miniButton, GUILayout.Width(120)))
            {
                GUI.FocusControl(null);
                SetObjectInEdition(null);
                ReorderListOfManagementByName();
            }
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_SEARCH_RELOAD, EditorStyles.miniButton, GUILayout.Width(120)))
            {
                GUI.FocusControl(null);
                SetObjectInEdition(null);
                Datas().m_SearchInternalName = "";
                Datas().m_SearchInternalDescription = "";
                //				ReloadAllObjects ();
                				LoadTableEditor ();

                ReorderListOfManagementByName();
            }
            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.EndVertical();


            GUILayout.BeginVertical(GUILayout.Width(120));
            GUILayout.Label(NWDConstants.K_APP_TABLE_FILTER_ZONE, EditorStyles.boldLabel);
            // |||||||||||||||||||||||||||||||||||||||||||
            bool t_ShowEnable = EditorGUILayout.ToggleLeft(NWDConstants.K_APP_TABLE_SHOW_ENABLE_DATAS, Datas().m_ShowEnable, GUILayout.Width(120));
            if (Datas().m_ShowEnable != t_ShowEnable)
            {
                Datas().m_ShowEnable = t_ShowEnable;
                FilterTableEditor();
            }
            bool t_ShowDisable = EditorGUILayout.ToggleLeft(NWDConstants.K_APP_TABLE_SHOW_DISABLE_DATAS, Datas().m_ShowDisable, GUILayout.Width(120));
            if (Datas().m_ShowDisable != t_ShowDisable)
            {
                Datas().m_ShowDisable = t_ShowDisable;
                FilterTableEditor();
            }
            bool t_ShowTrashed = EditorGUILayout.ToggleLeft(NWDConstants.K_APP_TABLE_SHOW_TRASHED_DATAS, Datas().m_ShowTrashed, GUILayout.Width(120));
            if (Datas().m_ShowTrashed != t_ShowTrashed)
            {
                Datas().m_ShowTrashed = t_ShowTrashed;
                FilterTableEditor();
            }
            bool t_ShowIntegrityError = EditorGUILayout.ToggleLeft(NWDConstants.K_APP_TABLE_SHOW_INTEGRITY_ERROR_DATAS, Datas().m_ShowIntegrityError, GUILayout.Width(120));
            if (Datas().m_ShowIntegrityError != t_ShowIntegrityError)
            {
                Datas().m_ShowIntegrityError = t_ShowIntegrityError;
                FilterTableEditor();
            }

            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(120));
            GUILayout.Label(NWDConstants.K_APP_TABLE_PAGINATION, tCenterLabel);
            //          GUILayout.BeginHorizontal ();
            int t_ItemPerPageSelection = EditorGUILayout.Popup(Datas().m_ItemPerPageSelection, Datas().m_ItemPerPageOptions, EditorStyles.popup);
            if (t_ItemPerPageSelection != Datas().m_ItemPerPageSelection)
            {
                Datas().m_PageSelected = 0;
            }
            Datas().m_ItemPerPageSelection = t_ItemPerPageSelection;
            int tRealReference = Datas().ObjectsByReferenceList.Count;
            if (tRealReference == 0)
            {
                GUILayout.Label(NWDConstants.K_APP_TABLE_NO_OBJECT);
            }
            else if (tRealReference == 1)
            {
                GUILayout.Label(NWDConstants.K_APP_TABLE_ONE_OBJECT);
            }
            else
            {
                GUILayout.Label(tRealReference + NWDConstants.K_APP_TABLE_X_OBJECTS);
            }

            int tResultReference = Datas().ObjectsInEditorTableList.Count;
            if (tResultReference == 0)
            {
                GUILayout.Label(NWDConstants.K_APP_TABLE_NO_OBJECT_FILTERED);
            }
            else if (tResultReference == 1)
            {
                GUILayout.Label(NWDConstants.K_APP_TABLE_ONE_OBJECT_FILTERED);
            }
            else
            {
                GUILayout.Label(tResultReference + NWDConstants.K_APP_TABLE_X_OBJECTS_FILTERED);
            }

            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.EndVertical();






            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical(GUILayout.Width(120));
            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.Label(NWDConstants.K_APP_TABLE_TOOLS_ZONE, tCenterLabel);
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_SHOW_TOOLS, EditorStyles.miniButton))
            {
                NWDBasisClassInspector tBasisInspector = ScriptableObject.CreateInstance<NWDBasisClassInspector>();
                tBasisInspector.mTypeInEdition = ClassType();
                Selection.activeObject = tBasisInspector;
            }
            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.EndVertical();

            // -------------------------------------------
            GUILayout.EndHorizontal();


            //EditorGUILayout.HelpBox(NWDConstants.K_APP_TABLE_SHORTCUT_ZONE_A + " " +
            //NWDConstants.K_APP_TABLE_SHORTCUT_ZONE_B + " " +
            //NWDConstants.K_APP_TABLE_SHORTCUT_ZONE_C, MessageType.Info);

            // ===========================================
            if (NWDTypeLauncher.DataLoaded == true)
            {
                DrawPagesTab();
            }



            /// DRAW SCROLLVIEW
            if (NWDTypeLauncher.DataLoaded == false)
            {

                GUILayout.FlexibleSpace();
                GUILayout.Label(NWDConstants.K_APP_TABLE_DATAS_ARE_LOADING_ZONE, tCenterLabel);
                GUILayout.FlexibleSpace();
            }
            else
            {

                DrawHeaderInEditor();

                Datas().m_ScrollPositionList = EditorGUILayout.BeginScrollView(Datas().m_ScrollPositionList, EditorStyles.inspectorFullWidthMargins, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
                // ===========================================

                //m_ItemList.Count

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
                    }
                }

                //
                // TODO: add instruction in tab view
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.S)
                {
                    NWDBasis<K> tSelected = NWDDataInspector.ObjectInEdition() as NWDBasis<K>;
                    if (tSelected != null)
                    {
                        if (Datas().ObjectsByReferenceList.Contains(tSelected.Reference))
                        {
                            if (tSelected.XX == 0 && tSelected.TestIntegrity())
                            {
                                int tIndex = Datas().ObjectsByReferenceList.IndexOf(tSelected.Reference);
                                Datas().ObjectsInEditorTableSelectionList[tIndex] = !Datas().ObjectsInEditorTableSelectionList[tIndex];
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
                        if (Datas().ObjectsInEditorTableList.Contains(tSelected.Reference))
                        {
                            int tIndexSelected = Datas().ObjectsInEditorTableList.IndexOf(tSelected.Reference);
                            if (tIndexSelected < Datas().ObjectsInEditorTableList.Count - 1)
                            {
                                string tNextReference = Datas().ObjectsInEditorTableList.ElementAt(tIndexSelected + 1);
                                int tNextObjectIndex = Datas().ObjectsByReferenceList.IndexOf(tNextReference);
                                SetObjectInEdition(Datas().ObjectsList.ElementAt(tNextObjectIndex));
                                float tNumberPage = (tIndexSelected + 1) / Datas().m_ItemPerPage;
                                int tPageExpected = (int)Math.Floor(tNumberPage);
                                Datas().m_PageSelected = tPageExpected;
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
                        if (Datas().ObjectsInEditorTableList.Contains(tSelected.Reference))
                        {
                            int tIndexSelected = Datas().ObjectsInEditorTableList.IndexOf(tSelected.Reference);
                            if (tIndexSelected > 0)
                            {
                                string tNextReference = Datas().ObjectsInEditorTableList.ElementAt(tIndexSelected - 1);
                                int tNextObjectIndex = Datas().ObjectsByReferenceList.IndexOf(tNextReference);
                                float tNumberPage = (tIndexSelected - 1) / Datas().m_ItemPerPage;
                                int tPageExpected = (int)Math.Floor(tNumberPage);
                                Datas().m_PageSelected = tPageExpected;
                                SetObjectInEdition(Datas().ObjectsList.ElementAt(tNextObjectIndex));
                                Event.current.Use();
                                sEditorWindow.Focus();
                            }
                        }
                        else
                        {
                        }
                    }
                }

                float tNumberOfPage = Datas().ObjectsInEditorTableList.Count / Datas().m_ItemPerPage;
                int tPagesExpected = (int)Math.Floor(tNumberOfPage);

                // TODO: add instruction in tab view
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.RightArrow)
                {
                    //Debug.LogVerbose ("KeyDown RightArrow", DebugResult.Success);
                    if (Datas().m_PageSelected < tPagesExpected)
                    {
                        Datas().m_PageSelected++;
                        // TODO : reselect first object
                        int tIndexSel = Datas().m_ItemPerPage * Datas().m_PageSelected;
                        if (tIndexSel < Datas().ObjectsInEditorTableList.Count)
                        {
                            string tNextReference = Datas().ObjectsInEditorTableList.ElementAt(tIndexSel);
                            int tNextObjectIndex = Datas().ObjectsByReferenceList.IndexOf(tNextReference);
                            SetObjectInEdition(Datas().ObjectsList.ElementAt(tNextObjectIndex));
                            Event.current.Use();
                            sEditorWindow.Focus();
                        }
                    }
                    else
                    {
                    }
                }


                // TODO: add instruction in tab view
                if (Event.current.type == EventType.KeyDown && Event.current.keyCode == KeyCode.LeftArrow)
                {
                    //Debug.LogVerbose ("KeyDown LeftArrow", DebugResult.Success);
                    if (Datas().m_PageSelected > 0)
                    {
                        Datas().m_PageSelected--;
                        // TODO : reselect first object
                        string tNextReference = Datas().ObjectsInEditorTableList.ElementAt(Datas().m_ItemPerPage * Datas().m_PageSelected);
                        int tNextObjectIndex = Datas().ObjectsByReferenceList.IndexOf(tNextReference);
                        SetObjectInEdition(Datas().ObjectsList.ElementAt(tNextObjectIndex));
                        Event.current.Use();
                        sEditorWindow.Focus();
                    }
                    else
                    {
                    }
                }


                if (Datas().m_PageSelected < 0)
                {
                    Datas().m_PageSelected = 0;
                }
                if (Datas().m_PageSelected > tPagesExpected)
                {
                    Datas().m_PageSelected = tPagesExpected;
                }

                for (int i = 0; i < Datas().m_ItemPerPage; i++)
                {
                    int tItemIndexInPage = Datas().m_ItemPerPage * Datas().m_PageSelected + i;
                    if (tItemIndexInPage < Datas().ObjectsInEditorTableList.Count)
                    {
                        string tReference = Datas().ObjectsInEditorTableList.ElementAt(tItemIndexInPage);
                        int tObjectIndex = Datas().ObjectsByReferenceList.IndexOf(tReference);
                        if (Datas().ObjectsList.Count > tObjectIndex && tObjectIndex >= 0)
                        {
                            NWDBasis<K> tObject = (NWDBasis<K>)Datas().ObjectsList.ElementAt(tObjectIndex);
                            if (tObject != null)
                            {
                                tObject.DrawRowInEditor(tMousePosition, sEditorWindow, tSelectAndClick);
                            }
                        }
                    }
                }


                // ===========================================
                EditorGUILayout.EndScrollView();

            }
            // -------------------------------------------

            GUILayout.Space(5.0f);

            Rect tRect = GUILayoutUtility.GetLastRect();
            EditorGUI.DrawRect(new Rect(tRect.x - 10.0f, tRect.y, 4096.0f, 1024.0f), new Color(0.0f, 0.0f, 0.0f, 0.10f));
            EditorGUI.DrawRect(new Rect(tRect.x - 10.0f, tRect.y, 4096.0f, 1.0f), new Color(0.0f, 0.0f, 0.0f, 0.30f));

            if (NWDTypeLauncher.DataLoaded == true)
            {
                DrawPagesTab();
            }

            GUILayout.Space(5.0f);

            //			GUILayout.Label ("Management", EditorStyles.boldLabel);

            int tSelectionCount = 0;
            int tActualItems = Datas().ObjectsInEditorTableSelectionList.Count;
            for (int tSelect = 0; tSelect < tActualItems; tSelect++)
            {
                if (Datas().ObjectsInEditorTableSelectionList[tSelect] == true)
                {
                    tSelectionCount++;
                }
            }

            GUILayout.BeginHorizontal();
            // -------------------------------------------
            GUILayout.BeginVertical(GUILayout.Width(120));
            // |||||||||||||||||||||||||||||||||||||||||||
            if (tSelectionCount == 0)
            {
                GUILayout.Label(NWDConstants.K_APP_TABLE_NO_SELECTED_OBJECT, tCenterLabel);
            }
            else if (tSelectionCount == 1)
            {
                GUILayout.Label(NWDConstants.K_APP_TABLE_ONE_SELECTED_OBJECT, tCenterLabel);
            }
            else
            {
                GUILayout.Label(tSelectionCount + NWDConstants.K_APP_TABLE_XX_SELECTED_OBJECT, tCenterLabel);
            }

            EditorGUI.BeginDisabledGroup(tSelectionCount == tActualItems);
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_SELECT_ALL, EditorStyles.miniButton))
            {
                //				for (int tSelect = 0; tSelect < tActualItems; tSelect++) {
                //					kObjectsSelectionList [tSelect] = true;
                //				}
                SelectAllObjectInTableList();
            }
            EditorGUI.EndDisabledGroup();

            EditorGUI.BeginDisabledGroup(tSelectionCount == 0);
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_DESELECT_ALL, EditorStyles.miniButton))
            {
                //				for (int tSelect = 0; tSelect < tActualItems; tSelect++) {
                //					kObjectsSelectionList [tSelect] = false;
                //				}
                DeselectAllObjectInTableList();
            }
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button(NWDConstants.K_APP_TABLE_INVERSE, EditorStyles.miniButton))
            {
                //				for (int tSelect = 0; tSelect < tActualItems; tSelect++) {
                //					kObjectsSelectionList [tSelect] = !kObjectsSelectionList [tSelect];
                //				}
                InverseSelectionOfAllObjectInTableList();
            }
            EditorGUI.EndDisabledGroup();

            if (GUILayout.Button(NWDConstants.K_APP_TABLE_SELECT_ENABLED, EditorStyles.miniButton))
            {
                SelectAllObjectEnableInTableList();
            }
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_SELECT_DISABLED, EditorStyles.miniButton))
            {
                SelectAllObjectDisableInTableList();
            }

            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.EndHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(120));
            // |||||||||||||||||||||||||||||||||||||||||||
            //			GUILayout.Label ("For all selected objects");

            EditorGUI.BeginDisabledGroup(tSelectionCount == 0);
            GUILayout.Label(NWDConstants.K_APP_TABLE_ACTIONS, tCenterLabel);

            if (GUILayout.Button(NWDConstants.K_APP_TABLE_REACTIVE, EditorStyles.miniButton))
            {
                for (int tSelect = 0; tSelect < tActualItems; tSelect++)
                {
                    if (Datas().ObjectsInEditorTableSelectionList[tSelect] == true)
                    {
                        NWDBasis<K> tObject = (NWDBasis<K>)Datas().ObjectsList.ElementAt(tSelect);
                        tObject.EnableMe();
                        //tObject.AddMeToUpdateQueue();
                    }
                }
            }

            if (GUILayout.Button(NWDConstants.K_APP_TABLE_DISACTIVE, EditorStyles.miniButton))
            {
                for (int tSelect = 0; tSelect < tActualItems; tSelect++)
                {
                    if (Datas().ObjectsInEditorTableSelectionList[tSelect] == true)
                    {
                        NWDBasis<K> tObject = (NWDBasis<K>)Datas().ObjectsList.ElementAt(tSelect);
                        tObject.DisableMe();
                        //tObject.AddMeToUpdateQueue();
                    }
                }
            }

            if (GUILayout.Button(NWDConstants.K_APP_TABLE_DUPPLICATE, EditorStyles.miniButton))
            {
                NWDBasis<K> tNextObjectSelected = null;
                int tNewObect = 0;
                for (int tSelect = 0; tSelect < tActualItems; tSelect++)
                {
                    if (Datas().ObjectsInEditorTableSelectionList[tSelect] == true)
                    {
                        tNewObect++;
                        NWDBasis<K> tObject = (NWDBasis<K>)Datas().ObjectsList.ElementAt(tSelect);
                        NWDBasis<K> tNextObject = tObject.DuplicateMe();
                        AddObjectInListOfEdition(tNextObject);
                        tNextObjectSelected = tNextObject;
                    }
                }
                if (tNewObect != 1)
                {
                    tNextObjectSelected = null;
                }
                SetObjectInEdition(tNextObjectSelected);
                //ReorderListOfManagementByName ();
                Datas().m_PageSelected = Datas().m_MaxPage * 3;
            }

            if (GUILayout.Button(NWDConstants.K_APP_TABLE_UPDATE, EditorStyles.miniButton))
            {
                for (int tSelect = 0; tSelect < tActualItems; tSelect++)
                {
                    if (Datas().ObjectsInEditorTableSelectionList[tSelect] == true)
                    {
                        NWDBasis<K> tObject = (NWDBasis<K>)Datas().ObjectsList.ElementAt(tSelect);
                        tObject.UpdateMe();
                    }
                }
            }


            EditorGUI.EndDisabledGroup();

            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(120));


            EditorGUI.BeginDisabledGroup(tSelectionCount == 0);

            Color tOldColor = GUI.backgroundColor;
            GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
            // DELETE SELECTION
            GUILayout.Label(NWDConstants.K_APP_TABLE_DELETE_WARNING, tCenterLabel);
            bool tDeleteSelection = false; //prevent GUIlayout error
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_DELETE_BUTTON, EditorStyles.miniButton))
            {
                tDeleteSelection = true;
            }
            // TRASH SELECTION
            bool tTrashSelection = false;  //prevent GUIlayout error
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_TRASH_ZONE, EditorStyles.miniButton))
            {
                tTrashSelection = true;
            }
            EditorGUI.EndDisabledGroup();
            GUI.backgroundColor = tOldColor;

            GUILayout.Space(10.0F);

            // RESET TABLE
            GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
            GUILayout.Label(NWDConstants.K_APP_TABLE_RESET_WARNING, tCenterLabel);
            bool tResetTable = false;  //prevent GUIlayout error
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_RESET_ZONE, EditorStyles.miniButton))
            {
                tResetTable = true;
            }

            GUI.backgroundColor = tOldColor;

            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.EndVertical();





            /*GUILayout.BeginVertical(GUILayout.Width(120));
            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.Label(NWDConstants.K_APP_TABLE_TOOLS_ZONE, tCenterLabel);
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_SHOW_TOOLS, EditorStyles.miniButton))
            {
                NWDBasisClassInspector tBasisInspector = ScriptableObject.CreateInstance<NWDBasisClassInspector>();
                tBasisInspector.mTypeInEdition = ClassType();
                Selection.activeObject = tBasisInspector;
            }
            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.Label(NWDConstants.K_APP_TABLE_PAGINATION, tCenterLabel);
            //			GUILayout.BeginHorizontal ();
            int t_ItemPerPageSelection = EditorGUILayout.Popup(m_ItemPerPageSelection, m_ItemPerPageOptions, EditorStyles.popup);
            if (t_ItemPerPageSelection != m_ItemPerPageSelection)
            {
                m_PageSelected = 0;
            }
            m_ItemPerPageSelection = t_ItemPerPageSelection;
            int tRealReference = ObjectsByReferenceList.Count;
            if (tRealReference == 0)
            {
                GUILayout.Label(NWDConstants.K_APP_TABLE_NO_OBJECT);
            }
            else if (tRealReference == 1)
            {
                GUILayout.Label(NWDConstants.K_APP_TABLE_ONE_OBJECT);
            }
            else
            {
                GUILayout.Label(tRealReference + NWDConstants.K_APP_TABLE_X_OBJECTS);
            }

            int tResultReference = ObjectsInEditorTableList.Count;
            if (tResultReference == 0)
            {
                GUILayout.Label(NWDConstants.K_APP_TABLE_NO_OBJECT_FILTERED);
            }
            else if (tResultReference == 1)
            {
                GUILayout.Label(NWDConstants.K_APP_TABLE_ONE_OBJECT_FILTERED);
            }
            else
            {
                GUILayout.Label(tResultReference + NWDConstants.K_APP_TABLE_X_OBJECTS_FILTERED);
            }

            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.EndVertical();
            */
            GUILayout.FlexibleSpace();



            bool tDisableProd = false;
            if (NWDDataManager.SharedInstance().mTypeUnSynchronizedList.Contains(ClassType()))
            {
                tDisableProd = true;
            }
            if (AccountDependent() == true)
            {
                tDisableProd = true;
            }

            GUILayout.BeginVertical(GUILayout.Width(120));

            // SYNCHRONIZATION
            // no big title
            // GUILayout.Label(NWDConstants.K_APP_BASIS_CLASS_SYNC, tCenterLabel);
            var tStyleBoldCenter = new GUIStyle(EditorStyles.boldLabel);
            tStyleBoldCenter.alignment = TextAnchor.MiddleCenter;


            float twPPD = 110.0F;

            // SYNCHRO ENVIRONMENT (TIMESTAMP as date in tooltips)
            GUILayout.BeginHorizontal();
            GUIContent tDevContent = new GUIContent(NWDConstants.K_DEVELOPMENT_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().DevEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
            GUILayout.Label(tDevContent, tStyleBoldCenter, GUILayout.Width(twPPD));
            GUIContent tPreprodContent = new GUIContent(NWDConstants.K_PREPRODUCTION_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().PreprodEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
            GUILayout.Label(tPreprodContent, tStyleBoldCenter, GUILayout.Width(twPPD));
            GUIContent tProdContent = new GUIContent(NWDConstants.K_PRODUCTION_NAME, NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().ProdEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
            GUILayout.Label(tProdContent, tStyleBoldCenter, GUILayout.Width(twPPD));
            GUILayout.EndHorizontal();


            // SYNCHRO TIMESTAMP
            // tooltips in title of section
            //GUILayout.BeginHorizontal();
            //GUILayout.Label(NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().DevEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
            //GUILayout.Label(NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().PreprodEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
            //GUILayout.Label(NWDToolbox.TimeStampToDateTime(SynchronizationGetLastTimestamp(NWDAppConfiguration.SharedInstance().ProdEnvironment)).ToString("yyyy/MM/dd HH:mm:ss"));
            //GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal(GUILayout.Width(120));

            EditorGUI.BeginDisabledGroup(true);
            if (GUILayout.Button("Prepare to publish", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
            }
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button("Prepare to publish", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                PrepareToPreprodPublish();
            }
            if (GUILayout.Button("Prepare to publish", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                
                PrepareToProdPublish();
            }

            GUILayout.EndHorizontal();

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Sync table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                SynchronizationFromWebService(NWDAppConfiguration.SharedInstance().DevEnvironment);
            }

            if (GUILayout.Button("Sync table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                SynchronizationFromWebService(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
            }
            EditorGUI.BeginDisabledGroup(tDisableProd);
            bool tSyncProd = false; //prevent GUIlayout error
            if (GUILayout.Button("Sync table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                tSyncProd = true;
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();


            // FORCE SYNCHRO
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Force Sync table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                SynchronizationFromWebServiceForce(NWDAppConfiguration.SharedInstance().DevEnvironment);
            }
            if (GUILayout.Button("Force Sync table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                SynchronizationFromWebServiceForce(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
            }
            EditorGUI.BeginDisabledGroup(tDisableProd);
            bool tSyncForceProd = false; //prevent GUIlayout error
            if (GUILayout.Button("Force Sync table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                tSyncForceProd = true;
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();



            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Pull table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                PullFromWebService(NWDAppConfiguration.SharedInstance().DevEnvironment);
            }

            if (GUILayout.Button("Pull table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                PullFromWebService(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
            }
            EditorGUI.BeginDisabledGroup(tDisableProd);
            bool tPullProd = false; //prevent GUIlayout error
            if (GUILayout.Button("Pull table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                tPullProd = true;
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Force Pull table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                PullFromWebServiceForce(NWDAppConfiguration.SharedInstance().DevEnvironment);
            }

            if (GUILayout.Button("Force Pull table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                PullFromWebServiceForce(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
            }
            EditorGUI.BeginDisabledGroup(tDisableProd);
            bool tPullProdForce = false; //prevent GUIlayout error
            if (GUILayout.Button("Force Pull table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                tPullProdForce = true;
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();


            tOldColor = GUI.backgroundColor;
            GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
            // FORCE SYNCHRO And Clean
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Clean table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                SynchronizationFromWebServiceClean(NWDAppConfiguration.SharedInstance().DevEnvironment);
            }
            if (GUILayout.Button("Clean table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                SynchronizationFromWebServiceClean(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
            }
            EditorGUI.BeginDisabledGroup(tDisableProd);

            bool tSyncCleanProd = false; //prevent GUIlayout error
            if (GUILayout.Button("Clean table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                }
                tSyncCleanProd = true;
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();

            bool tCleanLocalTable = false; //prevent GUIlayout error
            if (GUILayout.Button("Clean this local table", EditorStyles.miniButton))
            {
                tCleanLocalTable = true;
            }
            GUI.backgroundColor = tOldColor;

            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.EndVertical();


            //GUILayout.BeginVertical(GUILayout.Width(120));
            //if (GUILayout.Button("prepare publish to preprod", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            //{
            //    foreach (K tOb in ObjectsList)
            //    {
            //        if (tOb.PreprodSync <= tOb.DevSync)
            //        {
            //            tOb.PreprodSync = 0;
            //            tOb.UpdateMe();
            //        }
            //    }
            //    RepaintTableEditor();
            //}
            //if (GUILayout.Button("prepare publish to prod", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            //{
            //    foreach (K tOb in ObjectsList)
            //    {
            //        if (tOb.ProdSync <= tOb.DevSync || tOb.ProdSync < tOb.PreprodSync)
            //        {
            //            tOb.ProdSync = 0;
            //            tOb.UpdateMe();
            //        }
            //    }
            //    RepaintTableEditor();
            //}

            //GUILayout.EndVertical();




            GUILayout.FlexibleSpace();


            GUILayout.BeginVertical(GUILayout.Width(120));

            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.Label(NWDConstants.K_APP_TABLE_ADD_ZONE, tCenterLabel);
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_ADD_ROW, EditorStyles.miniButton))
            {
                // add card editor change
                //				NWDBasis<K> tNewObject = NWDBasis<K>.NewInstance ();
                //				AddObjectInListOfEdition (tNewObject);
                K tNewObject = NWDBasis<K>.NewObject();
                Datas().m_PageSelected = Datas().m_MaxPage * 3;
                SetObjectInEdition(tNewObject);
                //				sEditorWindow.Repaint ();
                NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
            }

             //ADD new object by the new instance directly (not NewObject() method)
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_ADD_ROW + "by new() ", EditorStyles.miniButton))
            {
                K tNewObject = new K();
                Datas().m_PageSelected = Datas().m_MaxPage * 3;
                SetObjectInEdition(tNewObject);
                NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
            }
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_ADD_ROW + "by NewData() ", EditorStyles.miniButton))
            {
                K tNewObject = NWDBasis<K>.NewData();
                Datas().m_PageSelected = Datas().m_MaxPage * 3;
                SetObjectInEdition(tNewObject);
                NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
            }
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_ADD_ROW + "by NewData(Pool) ", EditorStyles.miniButton))
            {
                K tNewObject = NWDBasis<K>.NewData(NWDWritingMode.PoolThread);
                Datas().m_PageSelected = Datas().m_MaxPage * 3;
                SetObjectInEdition(tNewObject);
                NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
            }
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_ADD_ROW + "by NewData(Queue) ", EditorStyles.miniButton))
            {
                K tNewObject = NWDBasis<K>.NewData(NWDWritingMode.QueuedMainThread);
                Datas().m_PageSelected = Datas().m_MaxPage * 3;
                SetObjectInEdition(tNewObject);
                NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
            }
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_ADD_ROW + "by NewData(Queue pool) ", EditorStyles.miniButton))
            {
                K tNewObject = NWDBasis<K>.NewData(NWDWritingMode.QueuedPoolThread);
                Datas().m_PageSelected = Datas().m_MaxPage * 3;
                SetObjectInEdition(tNewObject);
                NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
            }

            if (GUILayout.Button("ExecuteQueueMain", EditorStyles.miniButton))
            {
                NWDDataManager.SharedInstance().DataQueueMainExecute();
            }

            if (GUILayout.Button("ExecuteQueuePool", EditorStyles.miniButton))
            {
                NWDDataManager.SharedInstance().DataQueuePoolExecute();
            }

            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.EndVertical();
            // -------------------------------------------
            GUILayout.EndHorizontal();

            GUILayout.Space(10.0f);


            //			GUILayout.Label ("Edit card", EditorStyles.boldLabel);
            //			m_ScrollPositionCard = EditorGUILayout.BeginScrollView (m_ScrollPositionCard, GUILayout.ExpandWidth (true), GUILayout.ExpandHeight (true));
            //			// ===========================================
            //			// ===========================================
            //			EditorGUILayout.EndScrollView ();

            // Do operation which need and alert and prevent GUIlayout error

                if (tDeleteSelection == true)
            {
                string tDialog = "";
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
                    for (int tSelect = 0; tSelect < tActualItems; tSelect++)
                    {
                        if (Datas().ObjectsInEditorTableSelectionList[tSelect] == true)
                        {
                            NWDBasis<K> tObject = (NWDBasis<K>)Datas().ObjectsList.ElementAt(tSelect);
                            tListToDelete.Add((object)tObject);
                        }
                    }
                    foreach (object tObject in tListToDelete)
                    {
                        NWDBasis<K> tObjectToDelete = (NWDBasis<K>)tObject;
                        RemoveObjectInListOfEdition(tObjectToDelete);
                        tObjectToDelete.DeleteMe();
                    }
                    SetObjectInEdition(null);
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
                }
            }




            if (tTrashSelection == true)
            {
                string tDialog = "";
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
                    for (int tSelect = 0; tSelect < tActualItems; tSelect++)
                    {
                        if (Datas().ObjectsInEditorTableSelectionList[tSelect] == true)
                        {
                            NWDBasis<K> tObject = (NWDBasis<K>)Datas().ObjectsList.ElementAt(tSelect);
                            tListToTrash.Add((object)tObject);
                        }
                    }
                    foreach (object tObject in tListToTrash)
                    {
                        NWDBasis<K> tObjectToTrash = (NWDBasis<K>)tObject;
                        tObjectToTrash.TrashMe();
                    }
                    SetObjectInEdition(null);
                    //                  sEditorWindow.Repaint ();
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
                }
            }
            if (tResetTable == true)
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_APP_TABLE_RESET_ALERT,
                                                NWDConstants.K_APP_TABLE_RESET_TABLE,
                                                NWDConstants.K_APP_TABLE_RESET_YES,
                                                NWDConstants.K_APP_TABLE_RESET_NO))
                {

                    NWDBasis<K>.ResetTable();
                    //UpdateReferencesList ();
                    LoadTableEditor();
                    RepaintTableEditor();

                }
            }
            if (tPullProd == true)
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                }
                //if (EditorUtility.DisplayDialog(NWDConstants.K_SYNC_ALERT_TITLE,
                //        NWDConstants.K_SYNC_ALERT_MESSAGE,
                //        NWDConstants.K_SYNC_ALERT_OK,
                //        NWDConstants.K_SYNC_ALERT_CANCEL))
                //{
                PullFromWebService(NWDAppConfiguration.SharedInstance().ProdEnvironment);
                //}
            }
            if (tPullProdForce == true)
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                }
                //if (EditorUtility.DisplayDialog(NWDConstants.K_SYNC_ALERT_TITLE,
                //        NWDConstants.K_SYNC_ALERT_MESSAGE,
                //        NWDConstants.K_SYNC_ALERT_OK,
                //        NWDConstants.K_SYNC_ALERT_CANCEL))
                //{
                PullFromWebServiceForce(NWDAppConfiguration.SharedInstance().ProdEnvironment);
                //}
            }

            if (tSyncProd == true)
            {
                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                }
                //if (EditorUtility.DisplayDialog(NWDConstants.K_SYNC_ALERT_TITLE,
                //        NWDConstants.K_SYNC_ALERT_MESSAGE,
                //        NWDConstants.K_SYNC_ALERT_OK,
                //        NWDConstants.K_SYNC_ALERT_CANCEL))
                //{
                    SynchronizationFromWebService(NWDAppConfiguration.SharedInstance().ProdEnvironment);
                //}
            }

            if (tSyncForceProd == true)
            {
                //if (EditorUtility.DisplayDialog(NWDConstants.K_SYNC_ALERT_TITLE,
                //        NWDConstants.K_SYNC_ALERT_MESSAGE,
                //        NWDConstants.K_SYNC_ALERT_OK,
                //        NWDConstants.K_SYNC_ALERT_CANCEL))
                //{
                if (Application.isPlaying == true && AccountDependent()==false)
                {
                    EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                }
                    SynchronizationFromWebServiceForce(NWDAppConfiguration.SharedInstance().ProdEnvironment);
                //}
            }
            if (tSyncCleanProd == true)
            {

                if (Application.isPlaying == true && AccountDependent() == false)
                {
                    EditorUtility.DisplayDialog("ALERT NO SYNC VALID IN EDITOR", " ", "OK");
                }
                //if (EditorUtility.DisplayDialog(NWDConstants.K_SYNC_ALERT_TITLE,
                //        NWDConstants.K_SYNC_ALERT_MESSAGE,
                //        NWDConstants.K_SYNC_ALERT_OK,
                //        NWDConstants.K_SYNC_ALERT_CANCEL))
                //{
                    SynchronizationFromWebServiceClean(NWDAppConfiguration.SharedInstance().ProdEnvironment);
                //}
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
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void PrepareToPreprodPublish()
        {
            foreach (K tOb in Datas().ObjectsList)
                {
                if (tOb.PreprodSync <= tOb.DevSync && tOb.PreprodSync>=0)
                    {
                        tOb.UpdateMe();
                    }
                }
                RepaintTableEditor();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void PrepareToProdPublish()
        {
            foreach (K tOb in Datas().ObjectsList)
            {
                if ((tOb.ProdSync <= tOb.DevSync || tOb.ProdSync < tOb.PreprodSync) && tOb.ProdSync >= 0)
                {
                    tOb.UpdateMe();
                }
            }
            RepaintTableEditor();
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================