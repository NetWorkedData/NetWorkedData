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
                rReturn = string.Copy(tObject.ReferenceValue());
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
            BasisHelper().EditorTableDatas.Sort((x, y) => string.Compare(x.DatasMenu(), y.DatasMenu(), StringComparison.Ordinal));
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void IntegritySelection()
        {
            foreach (K tObject in BasisHelper().EditorTableDatas)
            {
                if (tObject.TestIntegrity() == false || tObject.XX > 0)
                {
                    if (BasisHelper().EditorTableDatasSelected.ContainsKey(tObject))
                    {
                        BasisHelper().EditorTableDatasSelected[tObject] = false;
                    }
                }
            }
        }
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
                BasisHelper().EditorTableDatasSelected[tObject]= true;
            }
            IntegritySelection();
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
            IntegritySelection();
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
            IntegritySelection();
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
            IntegritySelection();
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


            BasisHelper().EditorTableDatas = new List<NWDTypeClass>();
            BasisHelper().EditorTableDatasSelected = new Dictionary<NWDTypeClass, bool>();

            foreach (K tObject in BasisHelper().Datas)
            {
                bool tOccurence = true;

                if (tObject.TestIntegrity() == false && BasisHelper().m_ShowIntegrityError == false)
                {
                    tOccurence = false;
                }
                if (tObject.IsEnable() == true && BasisHelper().m_ShowEnable == false)
                {
                    tOccurence = false;
                }
                if (tObject.IsEnable() == false && BasisHelper().m_ShowDisable == false)
                {
                    tOccurence = false;
                }
                if (tObject.XX > 0 && BasisHelper().m_ShowTrashed == false)
                {
                    tOccurence = false;
                }

                if (BasisHelper().ClassType!= typeof(NWDAccount))
                    {
                    if (string.IsNullOrEmpty(BasisHelper().m_SearchAccount) == false)
                    {
                        if (BasisHelper().m_SearchAccount == "-=-") // empty
                        {
                            if (tObject.VisibleByAccountByEqual(string.Empty) == false)
                            {
                                tOccurence = false;
                            }
                        }
                        else if (BasisHelper().m_SearchAccount == "-+-") // not empty
                        {
                            if (tObject.VisibleByAccountByEqual(string.Empty) == true)
                            {
                                tOccurence = false;
                            }
                        }
                        else
                        {
                            if (tObject.VisibleByAccount(BasisHelper().m_SearchAccount) == false)
                            {
                                tOccurence = false;
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(BasisHelper().m_SearchAccount) == false)
                    {
                        if (BasisHelper().m_SearchAccount == "-=-") // empty
                        {
                        }
                        else if (BasisHelper().m_SearchAccount == "-+-") // not empty
                        {
                        }
                        else if (tObject.Reference != BasisHelper().m_SearchAccount)
                            {
                                tOccurence = false;
                        }
                    }
                }

                if (string.IsNullOrEmpty(BasisHelper().m_SearchGameSave) == false)
                {
                    if (BasisHelper().m_SearchGameSave == "-=-")
                    {
                        if (tObject.VisibleByGameSave(string.Empty) == false)
                        {
                            tOccurence = false;
                        }
                    }
                    else if (BasisHelper().m_SearchGameSave == "-+-")
                    {
                        if (tObject.VisibleByGameSave(string.Empty) == true)
                        {
                            tOccurence = false;
                        }
                    }
                    else
                    {
                        if (tObject.VisibleByGameSave(BasisHelper().m_SearchGameSave) == false)
                        {
                            tOccurence = false;
                        }
                    }
                }

                if (string.IsNullOrEmpty(BasisHelper().m_SearchReference) == false)
                {
                    if (tObject.Reference.Contains(BasisHelper().m_SearchReference) == false)
                    {
                        tOccurence = false;
                    }
                }
                if (string.IsNullOrEmpty(BasisHelper().m_SearchInternalName) == false)
                {
                    if (tObject.InternalKey.Contains(BasisHelper().m_SearchInternalName) == false)
                    {
                        tOccurence = false;
                    }
                }
                if (string.IsNullOrEmpty(BasisHelper().m_SearchInternalDescription) == false)
                {
                    if (tObject.InternalDescription.Contains(BasisHelper().m_SearchInternalDescription) == false)
                    {
                        tOccurence = false;
                    }
                }
                if (BasisHelper().m_SearchTag != NWDBasisTag.NoTag)
                {
                    if (tObject.Tag != BasisHelper().m_SearchTag /*&& tObject.Tag != NWDBasisTag.NoTag*/)
                    {
                        tOccurence = false;
                    }
                }
                if (tOccurence == true)
                {
                    if (BasisHelper().EditorTableDatas.Contains(tObject) == false)
                    {
                        BasisHelper().EditorTableDatas.Add(tObject);
                    }
                    if (BasisHelper().EditorTableDatasSelected.ContainsKey(tObject) == false)
                    {
                        BasisHelper().EditorTableDatasSelected.Add(tObject, false);
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
            }
            else if (tPagesExpected < tToogleToListPageLimit)
            {
                //m_PageSelected = GUILayout.Toolbar (m_PageSelected, tListOfPagesName, GUILayout.ExpandWidth (true));
                t_PageSelected = GUILayout.Toolbar(BasisHelper().m_PageSelected, tListOfPagesName, GUILayout.Width(tPagesExpected * tTabWidth));
            }
            else
            {
                t_PageSelected = EditorGUILayout.Popup(BasisHelper().m_PageSelected, tListOfPagesName, EditorStyles.popup, GUILayout.Width(tPopupWidth));
            }
            if (BasisHelper().m_PageSelected != t_PageSelected)
            {
                NWDDataManager.SharedInstance().DataQueueExecute();
            }
            BasisHelper().m_PageSelected = t_PageSelected;
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool mRowActions = true;
        public static bool mTableActions = true;
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
            if (BasisHelper().WebModelChanged == true)
            {
                string tTEXTWARNING = "<b><color=red>" + NWDConstants.K_APP_BASIS_WARNING_MODEL + "</color></b>";
                GUIContent tCC = new GUIContent(tTEXTWARNING);
                GUIStyle tWarningBoxStyle = new GUIStyle(EditorStyles.boldLabel);
                tWarningBoxStyle.normal.background = new Texture2D(1, 1);
                tWarningBoxStyle.normal.background.SetPixel(0, 0, Color.yellow);
                tWarningBoxStyle.normal.background.Apply();
                tWarningBoxStyle.alignment = TextAnchor.MiddleCenter;
                tWarningBoxStyle.richText = true;
                GUILayout.Label(tCC, tWarningBoxStyle);
            }
            if (BasisHelper().WebModelDegraded == true)
            {
                string tTEXTWARNING = "<b><color=red>" + NWDConstants.K_APP_BASIS_WARNING_MODEL_DEGRADED + "</color></b>";
                GUIContent tCC = new GUIContent(tTEXTWARNING);
                GUIStyle tWarningBoxStyle = new GUIStyle(EditorStyles.boldLabel);
                tWarningBoxStyle.normal.background = new Texture2D(1, 1);
                tWarningBoxStyle.normal.background.SetPixel(0, 0, Color.yellow);
                tWarningBoxStyle.normal.background.Apply();
                tWarningBoxStyle.alignment = TextAnchor.MiddleCenter;
                tWarningBoxStyle.richText = true;
                GUILayout.Label(tCC, tWarningBoxStyle);
            }
            //EditorGUILayout.BeginScrollView (Vector2.zero, EditorStyles.inspectorFullWidthMargins, GUILayout.ExpandWidth (false), GUILayout.ExpandHeight (false));
            // ===========================================
            GUILayout.BeginHorizontal();
            float tSearchWidth = 200.0F;
            float tOldLabelWidth = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 80.0f;
            // -------------------------------------------
            GUILayout.BeginVertical(GUILayout.Width(tSearchWidth));

            GUILayout.Label(NWDConstants.K_APP_TABLE_SEARCH_ZONE, EditorStyles.boldLabel);
            // |||||||||||||||||||||||||||||||||||||||||||
            //m_SearchReference = EditorGUILayout.TextField(NWDConstants.K_APP_TABLE_SEARCH_REFERENCE, m_SearchReference, GUILayout.Width(300));
            //NWDBasisHelper tDatas = Datas();

            BasisHelper().m_SearchReference = EditorGUILayout.TextField(NWDConstants.K_APP_TABLE_SEARCH_REFERENCE, BasisHelper().m_SearchReference, GUILayout.Width(tSearchWidth));


            BasisHelper().m_SearchInternalName = EditorGUILayout.TextField(NWDConstants.K_APP_TABLE_SEARCH_NAME, BasisHelper().m_SearchInternalName, GUILayout.Width(tSearchWidth));
            BasisHelper().m_SearchInternalDescription = EditorGUILayout.TextField(NWDConstants.K_APP_TABLE_SEARCH_DESCRIPTION, BasisHelper().m_SearchInternalDescription, GUILayout.Width(tSearchWidth));

            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUILayout.Width(tSearchWidth));
            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.Label(NWDConstants.K_APP_TABLE_SEARCH_ZONE, EditorStyles.boldLabel);
            // SELECT ACCOUNT 
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
            int tNewIndexAccount = EditorGUILayout.Popup(new GUIContent(NWDConstants.K_APP_TABLE_SEARCH_ACCOUNT), tIndexAccount, tContentFuturList.ToArray(),
                                                                        GUILayout.Width(tSearchWidth));
            if (tNewIndexAccount >= 0 && tNewIndexAccount < tReferenceList.Count())
            {
                BasisHelper().m_SearchAccount = tReferenceList[tNewIndexAccount];
            }
            else
            {
                BasisHelper().m_SearchAccount = string.Empty;
            }
            EditorGUI.EndDisabledGroup();

            // SELECT GameSave 
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
            int tNewIndexSave = EditorGUILayout.Popup(new GUIContent(NWDConstants.K_APP_TABLE_SEARCH_GAMESAVE), tIndexSave, tContentFuturSaveList.ToArray(),
                                                                        GUILayout.Width(tSearchWidth));
            if (tNewIndexSave >= 0 && tNewIndexSave < tReferenceSaveList.Count())
            {
                BasisHelper().m_SearchGameSave = tReferenceSaveList[tNewIndexSave];
            }
            else
            {
                BasisHelper().m_SearchGameSave = string.Empty;
            }
            EditorGUI.EndDisabledGroup();



            List<int> tTagIntList = new List<int>();
            List<string> tTagStringList = new List<string>();
            foreach (KeyValuePair<int, string> tTag in NWDAppConfiguration.SharedInstance().TagList)
            {
                tTagIntList.Add(tTag.Key);
                tTagStringList.Add(tTag.Value);
            }

            BasisHelper().m_SearchTag = (NWDBasisTag)EditorGUILayout.IntPopup(NWDConstants.K_APP_TABLE_SEARCH_TAG,
                                                                (int)BasisHelper().m_SearchTag, tTagStringList.ToArray(),
                                                                tTagIntList.ToArray(),
                                                                        GUILayout.Width(tSearchWidth));


            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.EndVertical();
            EditorGUIUtility.labelWidth = tOldLabelWidth;
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
                BasisHelper().m_SearchReference = string.Empty;
                BasisHelper().m_SearchInternalName = string.Empty;
                BasisHelper().m_SearchInternalDescription = string.Empty;
                BasisHelper().m_SearchTag = NWDBasisTag.NoTag;
                BasisHelper().m_SearchAccount = string.Empty;
                BasisHelper().m_SearchGameSave = string.Empty;
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
                Debug.Log(NWDConstants.K_APP_TABLE_SEARCH_RELOAD + "Action");
                string tReference = GetReferenceOfDataInEdition();
                GUI.FocusControl(null);
                SetObjectInEdition(null);
                BasisHelper().m_SearchInternalName = string.Empty;
                BasisHelper().m_SearchInternalDescription = string.Empty;
                //ReloadAllObjects ();
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
            bool t_ShowEnable = EditorGUILayout.ToggleLeft(NWDConstants.K_APP_TABLE_SHOW_ENABLE_DATAS, BasisHelper().m_ShowEnable, GUILayout.Width(120));
            if (BasisHelper().m_ShowEnable != t_ShowEnable)
            {
                BasisHelper().m_ShowEnable = t_ShowEnable;
                FilterTableEditor();
            }
            bool t_ShowDisable = EditorGUILayout.ToggleLeft(NWDConstants.K_APP_TABLE_SHOW_DISABLE_DATAS, BasisHelper().m_ShowDisable, GUILayout.Width(120));
            if (BasisHelper().m_ShowDisable != t_ShowDisable)
            {
                BasisHelper().m_ShowDisable = t_ShowDisable;
                FilterTableEditor();
            }
            EditorGUI.BeginDisabledGroup(!BasisHelper().m_ShowDisable);
            bool t_ShowTrashed = EditorGUILayout.ToggleLeft(NWDConstants.K_APP_TABLE_SHOW_TRASHED_DATAS, BasisHelper().m_ShowTrashed, GUILayout.Width(120));
            if (BasisHelper().m_ShowTrashed != t_ShowTrashed)
            {
                BasisHelper().m_ShowTrashed = t_ShowTrashed;
                FilterTableEditor();
            }
            EditorGUI.EndDisabledGroup();
            bool t_ShowIntegrityError = EditorGUILayout.ToggleLeft(NWDConstants.K_APP_TABLE_SHOW_INTEGRITY_ERROR_DATAS, BasisHelper().m_ShowIntegrityError, GUILayout.Width(120));
            if (BasisHelper().m_ShowIntegrityError != t_ShowIntegrityError)
            {
                BasisHelper().m_ShowIntegrityError = t_ShowIntegrityError;
                FilterTableEditor();
            }

            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(120));
            GUILayout.Label(NWDConstants.K_APP_TABLE_PAGINATION, tCenterLabel);
            //          GUILayout.BeginHorizontal ();
            int t_ItemPerPageSelection = EditorGUILayout.Popup(BasisHelper().m_ItemPerPageSelection, BasisHelper().m_ItemPerPageOptions, EditorStyles.popup);
            if (t_ItemPerPageSelection != BasisHelper().m_ItemPerPageSelection)
            {
                BasisHelper().m_PageSelected = 0;
            }
            BasisHelper().m_ItemPerPageSelection = t_ItemPerPageSelection;
            int tRealReference = BasisHelper().Datas.Count;
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

            int tResultReference = BasisHelper().EditorTableDatas.Count;
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


            //GUILayout.Label(NWDConstants.K_APP_TABLE_TOOLS_ZONE, tCenterLabel);
            //if (GUILayout.Button(NWDConstants.K_APP_TABLE_SHOW_TOOLS, EditorStyles.miniButton))
            //{
            //    NWDBasisClassInspector tBasisInspector = ScriptableObject.CreateInstance<NWDBasisClassInspector>();
            //    tBasisInspector.mTypeInEdition = ClassType();
            //    Selection.activeObject = tBasisInspector;
            //}
            // |||||||||||||||||||||||||||||||||||||||||||
            GUILayout.EndVertical();






            GUILayout.FlexibleSpace();

            GUILayout.BeginVertical(GUILayout.Width(120));
            // |||||||||||||||||||||||||||||||||||||||||||

            Texture2D tTextureOfClass = BasisHelper().TextureOfClass();
            if (tTextureOfClass != null)
            {
                GUILayout.Label(tTextureOfClass, tRightLabel, GUILayout.MaxHeight(64.0F));
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

                BasisHelper().m_ScrollPositionList = EditorGUILayout.BeginScrollView(BasisHelper().m_ScrollPositionList, EditorStyles.inspectorFullWidthMargins, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
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
                        if (BasisHelper().DatasByReference.ContainsKey(tSelected.Reference))
                        {
                            if (tSelected.XX == 0 && tSelected.TestIntegrity())
                            {
                                //int tIndex = Datas().ObjectsByReferenceList.IndexOf(tSelected.Reference);
                                BasisHelper().EditorTableDatasSelected[tSelected] = !BasisHelper().EditorTableDatasSelected[tSelected];
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
                                SetObjectInEdition(tNextSelected);
                                float tNumberPage = (tIndexSelected + 1) / BasisHelper().m_ItemPerPage;
                                int tPageExpected = (int)Math.Floor(tNumberPage);
                                BasisHelper().m_PageSelected = tPageExpected;
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
                                float tNumberPage = (tIndexSelected - 1) / BasisHelper().m_ItemPerPage;
                                int tPageExpected = (int)Math.Floor(tNumberPage);
                                BasisHelper().m_PageSelected = tPageExpected;
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

                float tNumberOfPage = BasisHelper().EditorTableDatas.Count / BasisHelper().m_ItemPerPage;
                int tPagesExpected = (int)Math.Floor(tNumberOfPage);

                // TODO: add instruction in tab view
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
                    if (BasisHelper().m_PageSelected > 0)
                    {
                        BasisHelper().m_PageSelected--;
                        // TODO : reselect first object
                        K tNextSelected = BasisHelper().EditorTableDatas.ElementAt(BasisHelper().m_ItemPerPage * BasisHelper().m_PageSelected)as K;
                        SetObjectInEdition(tNextSelected);
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

                for (int i = 0; i < BasisHelper().m_ItemPerPage; i++)
                {
                    int tItemIndexInPage = BasisHelper().m_ItemPerPage * BasisHelper().m_PageSelected + i;
                    if (tItemIndexInPage < BasisHelper().EditorTableDatas.Count)
                    {
                        K tObject = BasisHelper().EditorTableDatas.ElementAt(tItemIndexInPage) as K;
                        tObject.DrawRowInEditor(tMousePosition, sEditorWindow, tSelectAndClick);
                    }
                }


                // ===========================================
                EditorGUILayout.EndScrollView();

            }
            // -------------------------------------------

            int tSelectionCount = 0;
            bool tDeleteSelection = false; //prevent GUIlayout error
            bool tLocalizeLocalTable = false; //prevent GUIlayout error
            bool tTrashSelection = false;  //prevent GUIlayout error
            bool tResetTable = false;  //prevent GUIlayout error
            bool tCreateAllPHPForOnlyThisClassDEV = false; //prevent GUIlayout error
            bool tCreateAllPHPForOnlyThisClass = false; //prevent GUIlayout error
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
            // -------------------------------------------
            mRowActions = EditorGUILayout.Foldout(mRowActions, "Rows Actions");
            if (mRowActions == true )
            {
                //GUILayout.Label ("Management", EditorStyles.boldLabel);
                int tActualItems = BasisHelper().EditorTableDatas.Count;
                foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
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
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
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
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
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
                        }
                        tNextObjectSelected = tNextObject;
                    }
                    if (tNewData != 1)
                    {
                        tNextObjectSelected = null;
                    }
                    SetObjectInEdition(tNextObjectSelected);
                    //ReorderListOfManagementByName ();
                    BasisHelper().m_PageSelected = BasisHelper().m_MaxPage * 3;
                }

                if (GUILayout.Button(NWDConstants.K_APP_TABLE_UPDATE, EditorStyles.miniButton))
                {
                    foreach (KeyValuePair<NWDTypeClass, bool> tKeyValue in BasisHelper().EditorTableDatasSelected)
                    {
                        if (tKeyValue.Value == true)
                        {
                            K tObject = tKeyValue.Key as K;
                            tObject.UpdateData();
                        }
                    }
                }

                //GUILayout.Space(EditorStyles.miniButton.fixedHeight + NWDConstants.kFieldMarge);
                EditorGUI.EndDisabledGroup();

                if (GUILayout.Button("Export localization data", EditorStyles.miniButton))
                {
                    tLocalizeLocalTable = true;
                }


                // |||||||||||||||||||||||||||||||||||||||||||
                GUILayout.EndVertical();

                GUILayout.BeginVertical(GUILayout.Width(120));


                EditorGUI.BeginDisabledGroup(tSelectionCount == 0);
                NWDConstants.GUIRedButtonBegin();
                // DELETE SELECTION
                GUILayout.Label(NWDConstants.K_APP_TABLE_DELETE_WARNING, tCenterLabel);
                if (GUILayout.Button(NWDConstants.K_APP_TABLE_DELETE_BUTTON, EditorStyles.miniButton))
                {
                    tDeleteSelection = true;
                }
                // TRASH SELECTION
                if (GUILayout.Button(NWDConstants.K_APP_TABLE_TRASH_ZONE, EditorStyles.miniButton))
                {
                    tTrashSelection = true;
                }
                EditorGUI.EndDisabledGroup();
                NWDConstants.GUIRedButtonEnd();

                //GUILayout.Space(10.0F);

                // RESET TABLE
                //GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
                //GUILayout.Label(NWDConstants.K_APP_TABLE_RESET_WARNING, tCenterLabel);
                //if (GUILayout.Button(NWDConstants.K_APP_TABLE_RESET_ZONE, EditorStyles.miniButton))
                //{
                //    tResetTable = true;
                //}
                //if (GUILayout.Button(NWDConstants.K_APP_TABLE_SHOW_TOOLS, EditorStyles.miniButton))
                //{
                //    NWDBasisClassInspector tBasisInspector = ScriptableObject.CreateInstance<NWDBasisClassInspector>();
                //    tBasisInspector.mTypeInEdition = ClassType();
                //    Selection.activeObject = tBasisInspector;
                //}
                //if (GUILayout.Button(NWDConstants.K_APP_TABLE_PHP_DEV_TOOLS, EditorStyles.miniButton))
                //{
                //    tCreateAllPHPForOnlyThisClassDEV = true;
                //}
                //if (GUILayout.Button(NWDConstants.K_APP_TABLE_PHP_TOOLS, EditorStyles.miniButton))
                //{
                //    tCreateAllPHPForOnlyThisClass = true;
                //}

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
                GUILayout.BeginVertical(GUILayout.Width(120));
                // |||||||||||||||||||||||||||||||||||||||||||
                GUILayout.Space(120);
                // |||||||||||||||||||||||||||||||||||||||||||
                GUILayout.EndHorizontal();




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


                //GUILayout.BeginHorizontal(GUILayout.Width(120));
                //EditorGUI.BeginDisabledGroup(true);
                //if (GUILayout.Button("Protect in dev", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                //{
                //    ProtectInDev();
                //}
                //if (GUILayout.Button("Prepare to preprod", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                //{
                //    PrepareToPreprodPublish();
                //}
                //if (GUILayout.Button("Prepare to publish", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                //{
                //    PrepareToProdPublish();
                //}
                //EditorGUI.EndDisabledGroup();
                //GUILayout.EndHorizontal();

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

                    SynchronizationFromWebServiceForce(NWDAppConfiguration.SharedInstance().DevEnvironment);
                }
                if (GUILayout.Button("Force Sync table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                {

                    SynchronizationFromWebServiceForce(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                }
                EditorGUI.BeginDisabledGroup(tDisableProd);
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
                if (GUILayout.Button("Force Pull table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                {
                    tPullProdForce = true;
                }
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();


                //tOldColor = GUI.backgroundColor;
                //GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
                //// FORCE SYNCHRO And Clean
                //GUILayout.BeginHorizontal();
                //if (GUILayout.Button("Clean table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                //{
                //    if (Application.isPlaying == true && AccountDependent() == false)
                //    {
                //        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                //    }
                //    SynchronizationFromWebServiceClean(NWDAppConfiguration.SharedInstance().DevEnvironment);
                //}
                //if (GUILayout.Button("Clean table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                //{
                //    if (Application.isPlaying == true && AccountDependent() == false)
                //    {
                //        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                //    }
                //    SynchronizationFromWebServiceClean(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                //}
                //EditorGUI.BeginDisabledGroup(tDisableProd);

                //if (GUILayout.Button("Clean table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                //{
                //    if (Application.isPlaying == true && AccountDependent() == false)
                //    {
                //        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                //    }
                //    tSyncCleanProd = true;
                //}
                //EditorGUI.EndDisabledGroup();
                //GUILayout.EndHorizontal();


                //GUILayout.BeginHorizontal();
                //if (GUILayout.Button("Special", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                //{
                //    if (Application.isPlaying == true && AccountDependent() == false)
                //    {
                //        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                //    }
                //    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().DevEnvironment);
                //}
                //if (GUILayout.Button("Special", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                //{
                //    if (Application.isPlaying == true && AccountDependent() == false)
                //    {
                //        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                //    }
                //    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().PreprodEnvironment);
                //}
                //EditorGUI.BeginDisabledGroup(tDisableProd);

                //if (GUILayout.Button("Special", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                //{
                //    if (Application.isPlaying == true && AccountDependent() == false)
                //    {
                //        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                //    }
                //    tSyncSpecialProd = true;
                //}
                //EditorGUI.EndDisabledGroup();
                //GUILayout.EndHorizontal();

                //if (GUILayout.Button("Clean this local table", EditorStyles.miniButton))
                //{
                //    tCleanLocalTable = true;
                //}
                //if (GUILayout.Button("Not my Account Purge local table", EditorStyles.miniButton))
                //{
                //    tCleanLocalTableWithAccount = true;
                //}
                //GUI.backgroundColor = tOldColor;

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
                    if (BasisHelper().m_SearchTag != NWDBasisTag.NoTag)
                    {
                        tNewObject.Tag = BasisHelper().m_SearchTag;
                        tNewObject.UpdateData();
                    }
                    BasisHelper().m_PageSelected = BasisHelper().m_MaxPage * 3;
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


            }
            mTableActions = EditorGUILayout.Foldout(mTableActions, "Table Actions");
            if (mTableActions == true)
            {
































































                GUILayout.BeginHorizontal();
                // -------------------------------------------
                GUILayout.BeginVertical(GUILayout.Width(120));
                // |||||||||||||||||||||||||||||||||||||||||||
                GUILayout.Label(NWDConstants.K_APP_TABLE_RESET_WARNING, tCenterLabel);
                if (GUILayout.Button(NWDConstants.K_APP_TABLE_SHOW_TOOLS, EditorStyles.miniButton))
                {
                    NWDBasisClassInspector tBasisInspector = ScriptableObject.CreateInstance<NWDBasisClassInspector>();
                    tBasisInspector.mTypeInEdition = ClassType();
                    Selection.activeObject = tBasisInspector;
                }
                // |||||||||||||||||||||||||||||||||||||||||||
                GUILayout.EndVertical();
                NWDConstants.GUIRedButtonBegin();
                GUILayout.BeginVertical(GUILayout.Width(120));
                // |||||||||||||||||||||||||||||||||||||||||||
                GUILayout.Label(NWDConstants.K_APP_TABLE_RESET_WARNING, tCenterLabel);
                if (GUILayout.Button(NWDConstants.K_APP_TABLE_RESET_ZONE, EditorStyles.miniButton))
                {
                    tResetTable = true;
                }
                // |||||||||||||||||||||||||||||||||||||||||||
                GUILayout.EndVertical();
                GUILayout.BeginVertical(GUILayout.Width(120));
                // |||||||||||||||||||||||||||||||||||||||||||
                // RESET TABLE
                GUILayout.Label(NWDConstants.K_APP_WS_RESET_WARNING+" "+ NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000"), tCenterLabel);
                if (GUILayout.Button(NWDConstants.K_APP_WS_PHP_DEV_TOOLS.Replace("XXXX", NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000")), EditorStyles.miniButton))
                {
                    tCreateAllPHPForOnlyThisClassDEV = true;
                }
                if (GUILayout.Button(NWDConstants.K_APP_WS_PHP_TOOLS.Replace("XXXX", NWDAppConfiguration.SharedInstance().WebBuild.ToString("0000")), EditorStyles.miniButton))
                {
                    tCreateAllPHPForOnlyThisClass = true;
                }
                // |||||||||||||||||||||||||||||||||||||||||||
                GUILayout.EndVertical();
                NWDConstants.GUIRedButtonEnd();
                GUILayout.BeginVertical(GUILayout.Width(120));
                // |||||||||||||||||||||||||||||||||||||||||||
                GUILayout.Space(120);
                // |||||||||||||||||||||||||||||||||||||||||||
                GUILayout.EndHorizontal();

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
                NWDConstants.GUIRedButtonBegin();
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
                if (GUILayout.Button("Clean table", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    tSyncCleanProd = true;
                }
                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Special", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().DevEnvironment, NWDOperationSpecial.Special);
                }
                if (GUILayout.Button("Special", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().PreprodEnvironment, NWDOperationSpecial.Special);
                }
                if (GUILayout.Button("Special", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().ProdEnvironment, NWDOperationSpecial.Special);
                }
                GUILayout.EndHorizontal();





                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Upgrade", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().DevEnvironment, NWDOperationSpecial.Upgrade);
                }
                if (GUILayout.Button("Upgrade", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().PreprodEnvironment, NWDOperationSpecial.Upgrade);
                }
                if (GUILayout.Button("Upgrade", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().ProdEnvironment, NWDOperationSpecial.Upgrade);
                }
                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Optimize", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().DevEnvironment, NWDOperationSpecial.Optimize);
                }
                if (GUILayout.Button("Optimize", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().PreprodEnvironment, NWDOperationSpecial.Optimize);
                }
                if (GUILayout.Button("Optimize", EditorStyles.miniButton, GUILayout.Width(twPPD)))
                {
                    if (Application.isPlaying == true && AccountDependent() == false)
                    {
                        EditorUtility.DisplayDialog(NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_TITLE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_MESSAGE, NWDConstants.K_EDITOR_PLAYER_MODE_SYNC_ALERT_OK);
                    }
                    SynchronizationFromWebServiceSpecial(NWDAppConfiguration.SharedInstance().ProdEnvironment, NWDOperationSpecial.Optimize);
                }
                GUILayout.EndHorizontal();

                if (GUILayout.Button("Clean this local table", EditorStyles.miniButton))
                {
                    tCleanLocalTable = true;
                }
                if (GUILayout.Button("Not my Account Purge local table", EditorStyles.miniButton))
                {
                    tCleanLocalTableWithAccount = true;
                }
                NWDConstants.GUIRedButtonEnd();

                // |||||||||||||||||||||||||||||||||||||||||||
                GUILayout.EndVertical();


                GUILayout.FlexibleSpace();


                // -------------------------------------------
                GUILayout.EndHorizontal();










































            }

            GUILayout.Space(10.0f);


            //			GUILayout.Label ("Edit card", EditorStyles.boldLabel);
            //			m_ScrollPositionCard = EditorGUILayout.BeginScrollView (m_ScrollPositionCard, GUILayout.ExpandWidth (true), GUILayout.ExpandHeight (true));
            //			// ===========================================
            //			// ===========================================
            //			EditorGUILayout.EndScrollView ();

            // Do operation which need and alert and prevent GUIlayout error

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
                    }
                    SetObjectInEdition(null);
                    NWDDataManager.SharedInstance().RepaintWindowsInManager(ClassType());
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
                    //LoadFromDatabase();
                    //RepaintTableEditor();
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
                    CleanTable();
                }
            }
            if (tCleanLocalTableWithAccount == true)
            {
                if (EditorUtility.DisplayDialog(NWDConstants.K_PURGE_ALERT_TITLE,
                            NWDConstants.K_PURGE_ALERT_MESSAGE,
                            NWDConstants.K_PURGE_ALERT_OK,
                            NWDConstants.K_PURGE_ALERT_CANCEL))
                {
                    PurgeTable();
                }
            }
            if (tLocalizeLocalTable == true)
            {
                ExportLocalization();
            }

            if (tCreateAllPHPForOnlyThisClass == true)
            {
                //CreateAllPHPForOnlyThisClass();
                NWDAppConfiguration.SharedInstance().DevEnvironment.CreatePHP(new List<Type> { typeof(K) }, false, false);
                NWDAppConfiguration.SharedInstance().PreprodEnvironment.CreatePHP(new List<Type> { typeof(K) }, false, false);
                NWDAppConfiguration.SharedInstance().ProdEnvironment.CreatePHP(new List<Type> { typeof(K) }, false, false);
            }
            if (tCreateAllPHPForOnlyThisClassDEV == true)
            {
                //CreateDevPHPForOnlyThisClass();
                NWDAppConfiguration.SharedInstance().DevEnvironment.CreatePHP(new List<Type> { typeof(K) }, false, false);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_PrepareToPreprodPublish)]
        //public static void PrepareToPreprodPublish()
        //{
        //    foreach (K tOb in Datas().EditorTableDatas)
        //        {
        //        if (tOb.PreprodSync <= tOb.DevSync && tOb.PreprodSync>=0)
        //            {
        //            tOb.UpdateData();
        //            }
        //        }
        //        RepaintTableEditor();
        //}
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_PrepareToProdPublish)]
        //public static void PrepareToProdPublish()
        //{
        //    foreach (NWDTypeClass tData in Datas().EditorTableDatas)
        //    {
        //        K tObject = tData as K;
        //        if (tObject.PreprodSync == 0)
        //        {
        //            tObject.ProdSync = 0;
        //            tObject.UpdateData();
        //        }
        //        else if (tObject.PreprodSync > 0)
        //        {
        //            tObject.ProdSync = 1;
        //            tObject.UpdateData();
        //        }
        //        else if (tObject.PreprodSync < 0)
        //        {
        //            tObject.ProdSync = -1;
        //            tObject.UpdateData();
        //        }
        //    }
        //    RepaintTableEditor();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void ProtectInDev()
        //{
        //    foreach (NWDTypeClass tData in Datas().EditorTableDatas)
        //    {
        //        K tObject = tData as K;
        //        if (tObject.PreprodSync == 0)
        //        {
        //            tObject.ProdSync = 0;
        //            tObject.UpdateData();
        //        }
        //        else if (tObject.PreprodSync > 0)
        //        {
        //            tObject.ProdSync = 1;
        //            tObject.UpdateData();
        //        }
        //        else if (tObject.PreprodSync < 0)
        //        {
        //            tObject.ProdSync = -1;
        //            tObject.UpdateData();
        //        }
        //    }
        //    RepaintTableEditor();
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif