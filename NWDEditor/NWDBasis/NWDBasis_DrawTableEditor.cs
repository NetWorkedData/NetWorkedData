//=====================================================================================================================
//
// ideMobi copyright 2018 
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
using SQLite4Unity3d;
using System.IO;
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
                rReturn = string.Copy(tObject.ReferenceUsedValue());
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void RestaureDataInEditionByReference(string sReference)
        {
            K tObject = null;
            if (sReference != null)
            {
                if (Datas().DatasByReference.ContainsKey(sReference))
                {
                    tObject = Datas().DatasByReference[sReference] as K;
                }
                if (tObject != null)
                {
                    if (Datas().NEW_EditorTableDatas.Contains(tObject))
                    {
                        SetObjectInEdition(tObject);
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SortEditorTableDatas()
        {
            //BTBBenchmark.Start();
            //Debug.Log("NWDBasis<K> SortEditorTableDatas()");
            Datas().NEW_EditorTableDatas.Sort((x, y) => string.Compare(x.DatasMenu(), y.DatasMenu(), StringComparison.Ordinal));
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void IntegritySelection()
        {
            foreach (K tObject in Datas().NEW_EditorTableDatas)
            {
                if (tObject.TestIntegrity() == false || tObject.XX > 0)
                {
                    if (Datas().NEW_EditorTableDatasSelected.ContainsKey(tObject))
                    {
                        Datas().NEW_EditorTableDatasSelected[tObject] = false;
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SelectAllObjectInTableList()
        {
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in Datas().NEW_EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                Datas().NEW_EditorTableDatasSelected[tObject]= true;
            }
            IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void DeselectAllObjectInTableList()
        {
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in Datas().NEW_EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                Datas().NEW_EditorTableDatasSelected[tObject] = false;
            }
            IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void InverseSelectionOfAllObjectInTableList()
        {
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in Datas().NEW_EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                Datas().NEW_EditorTableDatasSelected[tObject] = !Datas().NEW_EditorTableDatasSelected[tObject];
            }
            IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SelectAllObjectEnableInTableList()
        {
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in Datas().NEW_EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                K tObjectK = tObject as K;
                Datas().NEW_EditorTableDatasSelected[tObjectK] = tObjectK.IsEnable();
            }
            IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SelectAllObjectDisableInTableList()
        {
            List<NWDTypeClass> tListToUse = new List<NWDTypeClass>();
            foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in Datas().NEW_EditorTableDatasSelected)
            {
                tListToUse.Add(tKeyValue.Key);
            }
            foreach (NWDTypeClass tObject in tListToUse)
            {
                K tObjectK = tObject as K;
                Datas().NEW_EditorTableDatasSelected[tObjectK] = !tObjectK.IsEnable();
            }
            IntegritySelection();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static IEnumerable<K> SelectForEditionObjects(string sReference, string sInternalKey, string sInternalDescription, NWDBasisTag sTag)
        //{
        //    SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionEditor;
        //    if (AccountDependent())
        //    {
        //        tSQLiteConnection = NWDDataManager.SharedInstance().SQLiteConnectionAccount;
        //    }
        //    if (string.IsNullOrEmpty(sReference) && string.IsNullOrEmpty(sInternalKey) && string.IsNullOrEmpty(sInternalDescription) && (int)sTag < 0)
        //    {
        //        return tSQLiteConnection.Table<K>().OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //    }
        //    else
        //    {
        //        if (!string.IsNullOrEmpty(sReference))
        //        {
        //            //Debug.Log("sReference = " + sReference);
        //            if (!string.IsNullOrEmpty(sInternalKey))
        //            {
        //                if (!string.IsNullOrEmpty(sInternalDescription))
        //                {
        //                    if ((int)sTag >= 0)
        //                    {
        //                        return tSQLiteConnection.Table<K>().Where(x =>
        //                                                                  x.Reference.Contains(sReference)
        //                                                                  && x.InternalKey.Contains(sInternalKey)
        //                                                                  && x.InternalDescription.Contains(sInternalDescription)
        //                                                                  && x.Tag.Equals((int)sTag)
        //                                                                 ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //                    }
        //                    else
        //                    {
        //                        return tSQLiteConnection.Table<K>().Where(x =>
        //                                                                  x.Reference.Contains(sReference)
        //                                                                  && x.InternalKey.Contains(sInternalKey)
        //                                                                  && x.InternalDescription.Contains(sInternalDescription)
        //                                                                 ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //                    }
        //                }
        //                else
        //                {
        //                    if ((int)sTag >= 0)
        //                    {
        //                        return tSQLiteConnection.Table<K>().Where(x =>
        //                                                                  x.Reference.Contains(sReference)
        //                                                                  && x.InternalKey.Contains(sInternalKey)
        //                                                                  && x.Tag.Equals((int)sTag)
        //                                                                 ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //                    }
        //                    else
        //                    {
        //                        return tSQLiteConnection.Table<K>().Where(x => x.Reference.Contains(sReference)
        //                                                                  && x.InternalKey.Contains(sInternalKey)
        //                                                                 ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (!string.IsNullOrEmpty(sInternalDescription))
        //                {
        //                    if ((int)sTag >= 0)
        //                    {
        //                        return tSQLiteConnection.Table<K>().Where(x => x.Reference.Contains(sReference)
        //                                                                  && x.InternalDescription.Contains(sInternalDescription)
        //                                                                  && x.Tag.Equals((int)sTag)
        //                                                                 ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //                    }
        //                    else
        //                    {
        //                        return tSQLiteConnection.Table<K>().Where(x => x.Reference.Contains(sReference)
        //                                                                  && x.InternalDescription.Contains(sInternalDescription)
        //                                                                 ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //                    }
        //                }
        //                else
        //                {
        //                    if ((int)sTag >= 0)
        //                    {
        //                        return tSQLiteConnection.Table<K>().Where(x => x.Reference.Contains(sReference)
        //                                                                  && x.Tag.Equals((int)sTag)
        //                                                                 ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //                    }
        //                    else
        //                    {
        //                        return tSQLiteConnection.Table<K>().Where(x => x.Reference.Contains(sReference)
        //                                                                 ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            if (!string.IsNullOrEmpty(sInternalKey))
        //            {
        //                if (!string.IsNullOrEmpty(sInternalDescription))
        //                {
        //                    if ((int)sTag >= 0)
        //                    {
        //                        return tSQLiteConnection.Table<K>().Where(x =>
        //                                                                   x.InternalKey.Contains(sInternalKey)
        //                                                                  && x.InternalDescription.Contains(sInternalDescription)
        //                                                                  && x.Tag.Equals((int)sTag)
        //                                                                 ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //                    }
        //                    else
        //                    {
        //                        return tSQLiteConnection.Table<K>().Where(x =>
        //                                                                   x.InternalKey.Contains(sInternalKey)
        //                                                                  && x.InternalDescription.Contains(sInternalDescription)
        //                                                                 ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //                    }
        //                }
        //                else
        //                {
        //                    if ((int)sTag >= 0)
        //                    {
        //                        return tSQLiteConnection.Table<K>().Where(x =>
        //                                                                   x.InternalKey.Contains(sInternalKey)
        //                                                                  && x.Tag.Equals((int)sTag)
        //                                                                 ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //                    }
        //                    else
        //                    {
        //                        return tSQLiteConnection.Table<K>().Where(x => x.InternalKey.Contains(sInternalKey)
        //                                                                 ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                if (!string.IsNullOrEmpty(sInternalDescription))
        //                {
        //                    if ((int)sTag >= 0)
        //                    {
        //                        return tSQLiteConnection.Table<K>().Where(x => x.InternalDescription.Contains(sInternalDescription)
        //                                                                  && x.Tag.Equals((int)sTag)
        //                                                                 ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //                    }
        //                    else
        //                    {
        //                        return tSQLiteConnection.Table<K>().Where(x => x.InternalDescription.Contains(sInternalDescription)
        //                                                                 ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //                    }
        //                }
        //                else
        //                {
        //                    if ((int)sTag >= 0)
        //                    {
        //                        return tSQLiteConnection.Table<K>().Where(x => x.Tag.Equals((int)sTag)
        //                                                                 ).OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //                    }
        //                    else
        //                    {
        //                        return tSQLiteConnection.Table<K>().OrderBy(x => x.InternalKey); // REMOVE THIS using of SQLITE
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    //return null;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public static void FilterTableEditor()
        {
            //BTBBenchmark.Start();
            //Debug.Log("FilterTableEditor()");


            Datas().NEW_EditorTableDatas = new List<NWDTypeClass>();
            Datas().NEW_EditorTableDatasSelected = new Dictionary<NWDTypeClass, bool>();

            foreach (K tObject in Datas().Datas)
            {
                bool tOccurence = true;

                if (tObject.TestIntegrity() == false && Datas().m_ShowIntegrityError == false)
                {
                    tOccurence = false;
                }
                if (tObject.IsEnable() == true && Datas().m_ShowEnable == false)
                {
                    tOccurence = false;
                }
                if (tObject.IsEnable() == false && Datas().m_ShowDisable == false)
                {
                    tOccurence = false;
                }
                if (tObject.XX > 0 && Datas().m_ShowTrashed == false)
                {
                    tOccurence = false;
                }

                if (string.IsNullOrEmpty(Datas().m_SearchAccount) == false)
                {
                    if(tObject.VisibleByAccount(Datas().m_SearchAccount) == false)
                    {
                        tOccurence = false;
                    }
                }

                if (string.IsNullOrEmpty(Datas().m_SearchGameSave) == false)
                {
                    if (tObject.VisibleByGameSave(Datas().m_SearchGameSave) == false)
                    {
                        tOccurence = false;
                    }
                }

                if (string.IsNullOrEmpty(Datas().m_SearchReference) == false)
                {
                    if (tObject.Reference.Contains(Datas().m_SearchReference) == false)
                    {
                        tOccurence = false;
                    }
                }
                if (string.IsNullOrEmpty(Datas().m_SearchInternalName) == false)
                {
                    if (tObject.InternalKey.Contains(Datas().m_SearchInternalName) == false)
                    {
                        tOccurence = false;
                    }
                }
                if (string.IsNullOrEmpty(Datas().m_SearchInternalDescription) == false)
                {
                    if (tObject.InternalDescription.Contains(Datas().m_SearchInternalDescription) == false)
                    {
                        tOccurence = false;
                    }
                }
                if (Datas().m_SearchTag != NWDBasisTag.NoTag)
                {
                    if (tObject.Tag != Datas().m_SearchTag /*&& tObject.Tag != NWDBasisTag.NoTag*/)
                    {
                        tOccurence = false;
                    }
                }
                if (tOccurence == true)
                {
                    if (Datas().NEW_EditorTableDatas.Contains(tObject) == false)
                    {
                        Datas().NEW_EditorTableDatas.Add(tObject);
                    }
                    if (Datas().NEW_EditorTableDatasSelected.ContainsKey(tObject) == false)
                    {
                        Datas().NEW_EditorTableDatasSelected.Add(tObject, false);
                    }
                }
            }



            SortEditorTableDatas();


            //Datas().NEW_EditorTableDatas = Datas().NEW_EditorTableDatas.OrderBy(x as K => x.InternalKey);
            /*
            //Debug.Log("FilterTableEditor()");
            //          Debug.Log ("m_SearchInternalName = " + m_SearchInternalName);
            // change filter, remove selection
            Datas().DatasInEditorSelectionList.ForEach(delegate (bool tSelection)
            {
                tSelection = false;
            });
            // change results
            Datas().DatasInEditorReferenceList = new List<string>();
            //IEnumerable tEnumerable = SelectForEditionObjects(m_SearchReference, m_SearchInternalName, m_SearchInternalDescription, m_SearchTag);

            IEnumerable tEnumerable = SelectForEditionObjects(Datas().m_SearchReference, Datas().m_SearchInternalName, Datas().m_SearchInternalDescription, Datas().m_SearchTag);
            if (tEnumerable != null)
            {
                foreach (NWDBasis<K> tItem in tEnumerable)
                {
                    bool tAdd = true;
                    if (tItem.TestIntegrity() == false && Datas().m_ShowIntegrityError == false)
                    {
                        tAdd = false;
                    }
                    if (tItem.AC == true && Datas().m_ShowEnable == false)
                    {
                        tAdd = false;
                    }
                    if (tItem.AC == false && Datas().m_ShowDisable == false)
                    {
                        tAdd = false;
                    }
                    if (tItem.XX > 0 && Datas().m_ShowTrashed == false)
                    {
                        tAdd = false;
                    }
                    if (tAdd == true)
                    {
                        Datas().DatasInEditorReferenceList.Add(tItem.Reference);
                    }
                }
            }
            foreach (NWDBasis<K> tObject in Datas().ObjectsList)
            {
                if (Datas().DatasInEditorReferenceList.Contains(tObject.Reference))
                {
                    // I keep the actual selection 
                }
                else
                {
                    int tIndex = Datas().ObjectsByReferenceList.IndexOf(tObject.Reference);
                    Datas().DatasInEditorSelectionList[tIndex] = false;
                }
            }

            */
            //BTBBenchmark.Finish();
        }
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
            float tNumberOfPage = Datas().NEW_EditorTableDatas.Count / Datas().m_ItemPerPage;
            int tPagesExpected = (int)Math.Floor(tNumberOfPage);
            if (tPagesExpected != 0)
            {
                if (Datas().NEW_EditorTableDatas.Count % (tPagesExpected * Datas().m_ItemPerPage) != 0)
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
                NWDDataManager.SharedInstance().DataQueueExecute();
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
            NWDDatas tDatas = Datas();

            Datas().m_SearchReference = EditorGUILayout.TextField(NWDConstants.K_APP_TABLE_SEARCH_REFERENCE, Datas().m_SearchReference, GUILayout.Width(300));


            Datas().m_SearchInternalName = EditorGUILayout.TextField(NWDConstants.K_APP_TABLE_SEARCH_NAME, Datas().m_SearchInternalName, GUILayout.Width(300));
            Datas().m_SearchInternalDescription = EditorGUILayout.TextField(NWDConstants.K_APP_TABLE_SEARCH_DESCRIPTION, Datas().m_SearchInternalDescription, GUILayout.Width(300));

            // SELECT ACCOUNT 
            EditorGUI.BeginDisabledGroup(!AccountDependent());
            List<string> tReferenceList = new List<string>();
            List<string> tInternalNameList = new List<string>();
            tReferenceList.Add("");
            tInternalNameList.Add(NWDConstants.kFieldNone);

            foreach (KeyValuePair<string, string> tKeyValue in NWDAccount.Datas().NEW_EditorDatasMenu.OrderBy(i => i.Value))
            {
                tReferenceList.Add(tKeyValue.Key);
                tInternalNameList.Add(tKeyValue.Value);
            }
            List<GUIContent> tContentFuturList = new List<GUIContent>();
            foreach (string tS in tInternalNameList.ToArray())
            {
                tContentFuturList.Add(new GUIContent(tS));
            }
            int tIndexAccount = tReferenceList.IndexOf(Datas().m_SearchAccount);
            int tNewIndexAccount = EditorGUILayout.Popup(new GUIContent(NWDConstants.K_APP_TABLE_SEARCH_ACCOUNT), tIndexAccount, tContentFuturList.ToArray());
            if (tNewIndexAccount >= 0 && tNewIndexAccount < tReferenceList.Count())
            {
                Datas().m_SearchAccount = tReferenceList[tNewIndexAccount];
            }
            else
            {
                Datas().m_SearchAccount = "";
            }
            EditorGUI.EndDisabledGroup();

            // SELECT GameSave 
            EditorGUI.BeginDisabledGroup(!GameSaveDependent());
            List<string> tReferenceSaveList = new List<string>();
            List<string> tInternalNameSaveList = new List<string>();
            tReferenceSaveList.Add("");
            tInternalNameSaveList.Add(NWDConstants.kFieldNone);

            foreach (KeyValuePair<string, string> tKeyValue in NWDGameSave.Datas().NEW_EditorDatasMenu.OrderBy(i => i.Value))
            {
                tReferenceSaveList.Add(tKeyValue.Key);
                tInternalNameSaveList.Add(tKeyValue.Value);
            }
            List<GUIContent> tContentFuturSaveList = new List<GUIContent>();
            foreach (string tS in tInternalNameSaveList.ToArray())
            {
                tContentFuturSaveList.Add(new GUIContent(tS));
            }
            int tIndexSave = tReferenceSaveList.IndexOf(Datas().m_SearchGameSave);
            int tNewIndexSave = EditorGUILayout.Popup(new GUIContent(NWDConstants.K_APP_TABLE_SEARCH_GAMESAVE), tIndexSave, tContentFuturSaveList.ToArray());
            if (tNewIndexSave >= 0 && tNewIndexSave < tReferenceSaveList.Count())
            {
                Datas().m_SearchGameSave = tReferenceSaveList[tNewIndexSave];
            }
            else
            {
                Datas().m_SearchGameSave = "";
            }
            EditorGUI.EndDisabledGroup();



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
                string tReference = GetReferenceOfDataInEdition();
                GUI.FocusControl(null);
                SetObjectInEdition(null);
                FilterTableEditor();
                RestaureDataInEditionByReference(tReference);
            }
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_SEARCH_REMOVE_FILTER, EditorStyles.miniButton, GUILayout.Width(120)))
            {

                string tReference = GetReferenceOfDataInEdition();
                GUI.FocusControl(null);
                SetObjectInEdition(null);
                //m_SearchReference = "";
                Datas().m_SearchReference = "";
                Datas().m_SearchInternalName = "";
                Datas().m_SearchInternalDescription = "";
                Datas().m_SearchTag = NWDBasisTag.NoTag;
                Datas().m_SearchAccount = "";
                Datas().m_SearchGameSave = "";
                FilterTableEditor();
                RestaureDataInEditionByReference(tReference);
            }
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_SEARCH_SORT, EditorStyles.miniButton, GUILayout.Width(120)))
            {

                string tReference = GetReferenceOfDataInEdition();
                //GUI.FocusControl(null);
                //SetObjectInEdition(null);
                SortEditorTableDatas();
                RestaureDataInEditionByReference(tReference);
            }
            if (GUILayout.Button(NWDConstants.K_APP_TABLE_SEARCH_RELOAD, EditorStyles.miniButton, GUILayout.Width(120)))
            {

                string tReference = GetReferenceOfDataInEdition();
                GUI.FocusControl(null);
                SetObjectInEdition(null);
                Datas().m_SearchInternalName = "";
                Datas().m_SearchInternalDescription = "";
                //				ReloadAllObjects ();
                				//LoadTableEditor ();
                LoadFromDatabase();
                SortEditorTableDatas();
                RestaureDataInEditionByReference(tReference);
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
            EditorGUI.BeginDisabledGroup(!Datas().m_ShowDisable);
            bool t_ShowTrashed = EditorGUILayout.ToggleLeft(NWDConstants.K_APP_TABLE_SHOW_TRASHED_DATAS, Datas().m_ShowTrashed, GUILayout.Width(120));
            if (Datas().m_ShowTrashed != t_ShowTrashed)
            {
                Datas().m_ShowTrashed = t_ShowTrashed;
                FilterTableEditor();
            }
            EditorGUI.EndDisabledGroup();
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
            int tRealReference = Datas().Datas.Count;
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

            int tResultReference = Datas().NEW_EditorTableDatas.Count;
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
                        if (Datas().DatasByReference.ContainsKey(tSelected.Reference))
                        {
                            if (tSelected.XX == 0 && tSelected.TestIntegrity())
                            {
                                //int tIndex = Datas().ObjectsByReferenceList.IndexOf(tSelected.Reference);
                                Datas().NEW_EditorTableDatasSelected[tSelected] = !Datas().NEW_EditorTableDatasSelected[tSelected];
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
                        if (Datas().NEW_EditorTableDatas.Contains(tSelected))
                        {
                            int tIndexSelected = Datas().NEW_EditorTableDatas.IndexOf(tSelected);
                            if (tIndexSelected < Datas().NEW_EditorTableDatas.Count - 1)
                            {
                                K tNextSelected = Datas().NEW_EditorTableDatas.ElementAt(tIndexSelected + 1) as K;
                                SetObjectInEdition(tNextSelected);
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
                        if (Datas().NEW_EditorTableDatas.Contains(tSelected))
                        {
                            int tIndexSelected = Datas().NEW_EditorTableDatas.IndexOf(tSelected);
                            if (tIndexSelected > 0)
                            {
                                K tNextSelected = Datas().NEW_EditorTableDatas.ElementAt(tIndexSelected - 1) as K;
                                float tNumberPage = (tIndexSelected - 1) / Datas().m_ItemPerPage;
                                int tPageExpected = (int)Math.Floor(tNumberPage);
                                Datas().m_PageSelected = tPageExpected;
                                SetObjectInEdition(tNextSelected);
                                Event.current.Use();
                                sEditorWindow.Focus();
                            }
                        }
                        else
                        {
                        }
                    }
                }

                float tNumberOfPage = Datas().NEW_EditorTableDatas.Count / Datas().m_ItemPerPage;
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
                        if (tIndexSel < Datas().NEW_EditorTableDatas.Count)
                        {
                            K tNextSelected = Datas().NEW_EditorTableDatas.ElementAt(tIndexSel) as K;
                            SetObjectInEdition(tNextSelected);
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
                        K tNextSelected = Datas().NEW_EditorTableDatas.ElementAt(Datas().m_ItemPerPage * Datas().m_PageSelected)as K;
                        SetObjectInEdition(tNextSelected);
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
                    if (tItemIndexInPage < Datas().NEW_EditorTableDatas.Count)
                    {
                        K tObject = Datas().NEW_EditorTableDatas.ElementAt(tItemIndexInPage) as K;
                        tObject.DrawRowInEditor(tMousePosition, sEditorWindow, tSelectAndClick);
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
            int tActualItems = Datas().NEW_EditorTableDatas.Count;
            foreach (KeyValuePair<NWDTypeClass,bool> tKeyValue in Datas().NEW_EditorTableDatasSelected)
            {
                if (tKeyValue.Value == true)
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
                foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in Datas().NEW_EditorTableDatasSelected)
                {
                    if (tKeyValue.Value == true)
                    {
                        K tObject = tKeyValue.Key as K;
                        tObject.EnableData();
                    }
                }
            }

            if (GUILayout.Button(NWDConstants.K_APP_TABLE_DISACTIVE, EditorStyles.miniButton))
            {
                foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in Datas().NEW_EditorTableDatasSelected)
                {
                    if (tKeyValue.Value == true)
                    {
                        K tObject = tKeyValue.Key as K;
                        tObject.DisableData();
                    }
                }
            }

            if (GUILayout.Button(NWDConstants.K_APP_TABLE_DUPPLICATE, EditorStyles.miniButton))
            {
                NWDBasis<K> tNextObjectSelected = null;
                int tNewData = 0;
                List<K> tListToUse = new List<K>();
                foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in Datas().NEW_EditorTableDatasSelected)
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
                    tNextObjectSelected = tNextObject;
                }
                if (tNewData != 1)
                {
                    tNextObjectSelected = null;
                }
                SetObjectInEdition(tNextObjectSelected);
                //ReorderListOfManagementByName ();
                Datas().m_PageSelected = Datas().m_MaxPage * 3;
            }

            if (GUILayout.Button(NWDConstants.K_APP_TABLE_UPDATE, EditorStyles.miniButton))
            {
                foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in Datas().NEW_EditorTableDatasSelected)
                {
                    if (tKeyValue.Value == true)
                    {
                        K tObject = tKeyValue.Key as K;
                        tObject.UpdateData();
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
            if (GUILayout.Button("Protect in dev", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                ProtectInDev();
            }
            if (GUILayout.Button("Prepare to preprod", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                PrepareToPreprodPublish();
            }
            if (GUILayout.Button("Prepare to publish", EditorStyles.miniButton, GUILayout.Width(twPPD)))
            {
                PrepareToProdPublish();
            }
            EditorGUI.EndDisabledGroup();
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
                K tNewObject = NWDBasis<K>.NewData();
                Datas().m_PageSelected = Datas().m_MaxPage * 3;
                SetObjectInEdition(tNewObject);
                //				sEditorWindow.Repaint ();
                NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
            }

            /*
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
            */
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
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in Datas().NEW_EditorTableDatasSelected)
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
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in Datas().NEW_EditorTableDatasSelected)
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
                    //LoadTableEditor();
                    LoadFromDatabase();
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
            foreach (K tOb in Datas().NEW_EditorTableDatas)
                {
                if (tOb.PreprodSync <= tOb.DevSync && tOb.PreprodSync>=0)
                    {
                    tOb.UpdateData();
                    }
                }
                RepaintTableEditor();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void PrepareToProdPublish()
        {
            foreach (NWDTypeClass tData in Datas().NEW_EditorTableDatas)
            {
                K tObject = tData as K;
                if (tObject.PreprodSync == 0)
                {
                    tObject.ProdSync = 0;
                    tObject.UpdateData();
                }
                else if (tObject.PreprodSync > 0)
                {
                    tObject.ProdSync = 1;
                    tObject.UpdateData();
                }
                else if (tObject.PreprodSync < 0)
                {
                    tObject.ProdSync = -1;
                    tObject.UpdateData();
                }
            }
            RepaintTableEditor();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ProtectInDev()
        {
            foreach (NWDTypeClass tData in Datas().NEW_EditorTableDatas)
            {
                K tObject = tData as K;
                if (tObject.PreprodSync == 0)
                {
                    tObject.ProdSync = 0;
                    tObject.UpdateData();
                }
                else if (tObject.PreprodSync > 0)
                {
                    tObject.ProdSync = 1;
                    tObject.UpdateData();
                }
                else if (tObject.PreprodSync < 0)
                {
                    tObject.ProdSync = -1;
                    tObject.UpdateData();
                }
            }
            RepaintTableEditor();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif