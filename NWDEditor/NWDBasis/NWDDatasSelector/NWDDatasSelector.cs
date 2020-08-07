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
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq.Expressions;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDDatasSelectorWindow : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public int ID = -1;
        public GUIContent IconAndTitle;
        public Vector2 ScrollPosition;
        public bool ScrollInit = false;
        public NWDDatasSelectorBasis SelectorBasis;
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            //NWDBenchmark.Start();
            // Init Title and icon 
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDConstants.K_DATA_SELECTOR_TITLE;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets("NWDBasisHelper t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals("NWDBasisHelper"))
                        {
                            IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        }
                    }
                }
                titleContent = IconAndTitle;
            }
            NWDGUI.LoadStylesReforce();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnLostFocus()
        {
            //NWDBenchmark.Start();
            if (SelectorBasis != null)
            {
                if (SelectorBasis.SelectedBlock != null)
                {
                    SelectorBasis.SelectedBlock(ID, false, false, null);
                }
            }
            Close();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            //NWDBenchmark.Start();
            if (SelectorBasis != null)
            {
                SelectorBasis.OnGUI();
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDDatasSelectorBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public delegate void NWDDatasSelectorBlock(int sID, bool sChange, bool sNone, NWDTypeClass sSelection);
        public NWDDatasSelectorBlock SelectedBlock;
        //-------------------------------------------------------------------------------------------------------------
        public NWDDatasSelectorWindow SelectorWindow;
        //-------------------------------------------------------------------------------------------------------------
        public float kZoom = 1.0F;
        public float kZoomRowMin = 1.0F;
        public float kZoomRowLimit = 4.0F;
        public float kZoomTileMax = 10.0F;
        public static Dictionary<int, string> ControllerResult = new Dictionary<int, string>();
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnGUI()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDDatasSelector : NWDDatasSelectorBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDTypeClass> Datas = null;
        List<int> TagIntList = new List<int>();
        List<string> TagStringList = new List<string>();
        //-------------------------------------------------------------------------------------------------------------
        public string InternalResearch = "";
        public string DescriptionResearch = "";
        public NWDBasisTag Tag;
        public bool EnableDatas = true;
        public bool DisableDatas = true;
        public bool TrashedDatas = true;
        public bool CorruptedDatas = true;
        List<NWDTypeClass> ResultList = new List<NWDTypeClass>();
        string ActualSelection; // by reference string
        GUIStyle RowSytle = null;
        NWDBasisHelper Helper;
        bool PrefReccord = false;
        bool NeedInitDesing = true;
        public string kPreferencePrefix;
        //-------------------------------------------------------------------------------------------------------------
        private string PreferenceKey<T>(Expression<Func<T>> sProperty)
        {
            return Helper.ClassPrefBaseKey + "_DataSelector_" + NWDToolbox.PropertyName(sProperty);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void LoadPreference()
        {
            if (Helper != null)
            {
                if (NWDProjectPrefs.GetBool(PreferenceKey(() => PrefReccord)) == true)
                {
                    InternalResearch = NWDProjectPrefs.GetString(PreferenceKey(() => InternalResearch));
                    DescriptionResearch = NWDProjectPrefs.GetString(PreferenceKey(() => DescriptionResearch));
                    EnableDatas = NWDProjectPrefs.GetBool(PreferenceKey(() => EnableDatas));
                    DisableDatas = NWDProjectPrefs.GetBool(PreferenceKey(() => DisableDatas));
                    TrashedDatas = NWDProjectPrefs.GetBool(PreferenceKey(() => TrashedDatas));
                    CorruptedDatas = NWDProjectPrefs.GetBool(PreferenceKey(() => CorruptedDatas));
                    Tag = (NWDBasisTag)NWDProjectPrefs.GetInt(PreferenceKey(() => Tag));
                    kZoom = NWDProjectPrefs.GetFloat(PreferenceKey(() => kZoom));
                }
                else
                {
                    Tag = NWDBasisTag.NoTag;
                    EnableDatas = true;
                    DisableDatas = true;
                    TrashedDatas = true;
                    CorruptedDatas = true;
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void SavePreference()
        {
            if (Helper != null)
            {
                NWDProjectPrefs.SetBool(PreferenceKey(() => PrefReccord), true);
                NWDProjectPrefs.SetString(PreferenceKey(() => InternalResearch), InternalResearch);
                NWDProjectPrefs.SetString(PreferenceKey(() => DescriptionResearch), DescriptionResearch);
                NWDProjectPrefs.SetBool(PreferenceKey(() => EnableDatas), EnableDatas);
                NWDProjectPrefs.SetBool(PreferenceKey(() => DisableDatas), DisableDatas);
                NWDProjectPrefs.SetBool(PreferenceKey(() => TrashedDatas), TrashedDatas);
                NWDProjectPrefs.SetBool(PreferenceKey(() => CorruptedDatas), CorruptedDatas);
                NWDProjectPrefs.SetInt(PreferenceKey(() => Tag), (int)Tag);
                NWDProjectPrefs.SetFloat(PreferenceKey(() => kZoom), kZoom);
            }
        }
        //-------------------------------------------------------------------------------------------------------------

        static public string Field(NWDBasisHelper sHelper, Rect sRect, GUIContent sContent, string sReference, bool sDisabled, float sInsertion = 0)
        {
            //NWDBenchmark.Start();
            sHelper.RowAnalyze();
            NWDGUI.LoadStyles();
            string tReference = sReference;
            int tID = GUIUtility.GetControlID(sContent, FocusType.Keyboard, sRect);
            //Debug.Log("Field with selection : " + sReference + " control id : " + tID.ToString());

            Event tEvent = Event.current;
            EventType tEventType = tEvent.type;
            if (tEventType != EventType.Repaint)
            {
                if (ControllerResult.ContainsKey(tID))
                {
                    tReference = ControllerResult[tID];
                    ControllerResult.Remove(tID);
                    GUI.changed = true;
                }
            }
            Rect tEntitlement = new Rect(sRect.position.x, sRect.position.y, EditorGUIUtility.labelWidth, NWDGUI.kDataSelectorFieldStyle.fixedHeight);
            Rect tField = new Rect(sRect.position.x + EditorGUIUtility.labelWidth, sRect.position.y, sRect.width - EditorGUIUtility.labelWidth - NWDGUI.kEditWidth - NWDGUI.kFieldMarge - sInsertion, NWDGUI.kDataSelectorFieldStyle.fixedHeight);
            Rect tEditRect = new Rect(sRect.position.x + sRect.width - NWDGUI.kEditWidth, sRect.position.y + NWDGUI.kDatasSelectorYOffset, NWDGUI.kEditWidth, NWDGUI.kMiniButtonStyle.fixedHeight);

            if (sHelper.HasPreview(sReference))
            {
                GUIContent sImageLabel = sHelper.GetGUIPreview(sReference);
                Rect tPreviewRect = new Rect(sRect.position.x + EditorGUIUtility.labelWidth, sRect.position.y, NWDGUI.kDataSelectorFieldStyle.fixedHeight, NWDGUI.kDataSelectorFieldStyle.fixedHeight);
                //GUI.Label(tPreviewRect, sImageLabel);
                GUI.Label(tPreviewRect, sImageLabel, NWDGUI.kDataSelectorFieldIconStyle);
                tField.x += NWDGUI.kFieldMarge + NWDGUI.kDataSelectorFieldStyle.fixedHeight;
                tField.width -= NWDGUI.kFieldMarge + NWDGUI.kDataSelectorFieldStyle.fixedHeight;
            }

            tEntitlement = EditorGUI.IndentedRect(tEntitlement);
            GUI.Label(tEntitlement, sContent, NWDGUI.kPropertyEntitlementStyle);

            GUIContent sDataLabel = sHelper.GetGUIContent(sReference);
            NWDTypeClass tData = sHelper.GetDataByReference(sReference);


            if (string.IsNullOrEmpty(sReference) == false && sHelper.GetDataByReference(sReference) == null)
            {
                NWDGUI.BeginRedArea();
                GUI.Label(tField, sDataLabel, NWDGUI.kDataSelectorFieldStyle);
                //EditorGUI.BeginDisabledGroup(true);
                if (GUI.Button(tEditRect, NWDGUI.kCleanContentIcon, NWDGUI.kEditButtonStyle))
                {
                    if (ControllerResult.ContainsKey(tID))
                    {
                        ControllerResult.Remove(tID);
                    }
                    ControllerResult.Add(tID, string.Empty);
                }
                //EditorGUI.EndDisabledGroup();
                NWDGUI.EndRedArea();
            }
            else
            {
                if (tData != null)
                {
                    if (tData.IsTrashed())
                    {
                        NWDGUI.BeginColorArea(NWDGUI.kRowColorTrash);
                    }
                    else if (tData.IsEnable() == false)
                    {
                        NWDGUI.BeginColorArea(NWDGUI.kRowColorDisactive);
                    }
                    else if (tData.IntegrityIsValid() == false)
                    {
                        NWDGUI.BeginColorArea(NWDGUI.kRowColorWarning);
                    }
                    else
                    {
                        NWDGUI.EndColorArea();
                    }
                }
                if (GUI.Button(tField, sDataLabel, NWDGUI.kDataSelectorFieldStyle))
                {
                    ShowNow(sHelper, tID, "", "", NWDBasisTag.NoTag, delegate (int sID, bool sChange, bool sNone, NWDTypeClass sSelection)
                    {
                        if (sChange == true)
                        {
                            string bResult = string.Empty;
                            if (sNone == true)
                            {
                                bResult = string.Empty;
                            }
                            else
                            {
                                if (sSelection != null)
                                {
                                    bResult = sSelection.Reference;
                                }
                            }
                            if (ControllerResult.ContainsKey(sID))
                            {
                                ControllerResult.Remove(sID);
                            }
                            ControllerResult.Add(sID, bResult);
                        }
                    }, sReference);
                    GUIUtility.ExitGUI();
                }
                NWDGUI.EndColorArea();
                if (string.IsNullOrEmpty(sReference) == false)
                {
                    EditorGUI.EndDisabledGroup();
                    if (GUI.Button(tEditRect, NWDGUI.kEditContentIcon, NWDGUI.kEditButtonStyle))
                    {
                        sHelper.SetObjectInEdition(sHelper.GetDataByReference(sReference), false);
                    }
                    EditorGUI.BeginDisabledGroup(sDisabled);
                }
                else
                {
                    if (GUI.Button(tEditRect, NWDGUI.kNewContentIcon, NWDGUI.kEditButtonStyle))
                    {
                        NWDTypeClass tNewObject = sHelper.NewData();
                        if (ControllerResult.ContainsKey(tID))
                        {
                            ControllerResult.Remove(tID);
                        }
                        ControllerResult.Add(tID, tNewObject.Reference);
                    }
                }
            }
            //NWDBenchmark.Finish();
            return tReference;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void ShowNow(NWDBasisHelper sHelper, int sID, string sInitialInternalResearch = "",
                                string sInitialDescriptionResearch = "",
                                NWDBasisTag sTag = NWDBasisTag.NoTag,
                                NWDDatasSelectorBlock sSelectedBlock = null,
                                string sSelection = "")
        {
            NWDDatasSelector rReturn = new NWDDatasSelector();
            rReturn.Show(sHelper, sID, sInitialInternalResearch, sInitialDescriptionResearch, sTag, sSelectedBlock, sSelection);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void ShowNow(NWDBasisHelper sHelper, string sInitialInternalResearch = "",
                                string sInitialDescriptionResearch = "",
                                NWDBasisTag sTag = NWDBasisTag.NoTag,
                                NWDDatasSelectorBlock sSelectedBlock = null,
                                string sSelection = "")
        {
            NWDDatasSelector rReturn = new NWDDatasSelector();
            rReturn.Show(sHelper, -2, sInitialInternalResearch, sInitialDescriptionResearch, sTag, sSelectedBlock, sSelection);
        }

        //-------------------------------------------------------------------------------------------------------------
        static public void SelectNow(NWDBasisHelper sHelper, List<NWDTypeClass> sDatas, string sInitialInternalResearch = "",
                                string sInitialDescriptionResearch = "",
                                NWDBasisTag sTag = NWDBasisTag.NoTag,
                                NWDDatasSelectorBlock sSelectedBlock = null,
                                string sSelection = "")
        {
            NWDDatasSelector rReturn = new NWDDatasSelector();
            rReturn.Datas = sDatas;
            rReturn.Show(sHelper, -2, sInitialInternalResearch, sInitialDescriptionResearch, sTag, sSelectedBlock, sSelection);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Show(NWDBasisHelper sHelper, string sInitialInternalResearch = "",
                            string sInitialDescriptionResearch = "",
                            NWDBasisTag sTag = NWDBasisTag.NoTag,
                            NWDDatasSelectorBlock sSelectedBlock = null,
                            string sSelection = "")
        {
            Show(sHelper, -2, sInitialInternalResearch, sInitialDescriptionResearch, sTag, sSelectedBlock, sSelection);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Show(NWDBasisHelper sHelper, int sID, string sInitialInternalResearch = "",
                        string sInitialDescriptionResearch = "",
                        NWDBasisTag sTag = NWDBasisTag.NoTag,
                        NWDDatasSelectorBlock sSelectedBlock = null,
                        string sSelection = "")
        {
            //NWDBenchmark.Start();
            Helper = sHelper;
            Helper.RowAnalyze();
            Initialization();
            if (Datas == null)
            {
                Datas = sHelper.Datas;
            }
            if (string.IsNullOrEmpty(sInitialInternalResearch) == false)
            {
                InternalResearch = sInitialInternalResearch;
            }
            if (string.IsNullOrEmpty(sInitialDescriptionResearch) == false)
            {
                DescriptionResearch = sInitialDescriptionResearch;
            }
            if (sTag != NWDBasisTag.NoTag)
            {
                Tag = sTag;
            }

            Filter();
            SelectedBlock = sSelectedBlock;
            if (sSelection != null)
            {
                ActualSelection = sSelection;
            }
            else
            {
                ActualSelection = "";
            }
            SelectorWindow = ScriptableObject.CreateInstance(typeof(NWDDatasSelectorWindow)) as NWDDatasSelectorWindow;
            SelectorWindow.SelectorBasis = this;
            SelectorWindow.ScrollInit = false;
            SelectorWindow.ID = sID;
            SelectorWindow.minSize = new Vector2(480, 400);
            SelectorWindow.IconAndTitle = new GUIContent();
            SelectorWindow.IconAndTitle.text = NWDConstants.K_DATA_SELECTOR_TITLE_FOR + Helper.ClassMenuName;
            if (SelectorWindow.IconAndTitle.image == null)
            {
                string[] sGUIDs = AssetDatabase.FindAssets("NWDBasisHelper t:texture");
                foreach (string tGUID in sGUIDs)
                {
                    string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                    string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                    if (tPathFilename.Equals("NWDBasisHelper"))
                    {
                        SelectorWindow.IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                    }
                }
            }
            SelectorWindow.titleContent = SelectorWindow.IconAndTitle;

            SelectorWindow.ShowUtility();
            SelectorWindow.Focus();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        void Initialization()
        {
            //NWDBenchmark.Start();
            LoadPreference();

            // Tag management
            foreach (KeyValuePair<int, string> tTag in NWDAppConfiguration.SharedInstance().TagList)
            {
                TagIntList.Add(tTag.Key);
                TagStringList.Add(tTag.Value);
            }
            if (TagIntList.Contains(-1) ==false)
            {
                TagIntList.Add(-1);
                TagStringList.Add("No Tag");
            }

            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        void Filter()
        {
            SavePreference();
            //NWDBenchmark.Start();
            ResultList = new List<NWDTypeClass>();
            string tIntLower = "";
            if (string.IsNullOrEmpty(InternalResearch) == false)
            {
                tIntLower = InternalResearch.ToLower();
            }
            string tDesLower = "";
            if (string.IsNullOrEmpty(DescriptionResearch) == false)
            {
                tDesLower = DescriptionResearch.ToLower();
            }
            foreach (NWDTypeClass tItem in Datas)
            {
                bool tInclude = true;
                //if (tItem.IsEnable() == EnableDatas)
                //{
                //    tInclude = true;
                //}
                //if (DisableDatas == true)
                //{
                //    if (tItem.IsEnable() == false)
                //    {
                //        if (TrashedDatas == false)
                //        {
                //            if (tItem.IsTrashed() == false)
                //            {
                //                tInclude = true;
                //            }
                //        }
                //        else
                //        {
                //            tInclude = true;
                //        }
                //    }
                //}
                //if (CorruptedDatas == false)
                //{
                //    tInclude = tItem.TestIntegrityResult;
                //}



                if (tItem.TestIntegrityResult == false && CorruptedDatas == false)
                {
                    tInclude = false;
                }
                if (tItem.IsEnable() == true && EnableDatas == false)
                {
                    tInclude = false;
                }
                if (tItem.IsEnable() == false && DisableDatas == false)
                {
                    tInclude = false;
                }
                if (tItem.XX > 0 && TrashedDatas == false)
                {
                    tInclude = false;
                }



                if (tItem.InternalKey.ToLower().Contains(tIntLower) == false)
                {
                    tInclude = false;
                }
                if (tInclude == true)
                {
                    if (tItem.InternalDescription.ToLower().Contains(tDesLower) == false)
                    {
                        tInclude = false;
                    }
                }
                if (tInclude == true)
                {
                    if (Tag != NWDBasisTag.NoTag)
                    {
                        if (tItem.Tag != Tag)
                        {
                            tInclude = false;
                        }
                    }
                }
                if (tInclude == true)
                {
                    ResultList.Add(tItem);
                }
            }
            ResultList.Sort((tA, tB) => string.Compare(tA.InternalKey, tB.InternalKey, StringComparison.Ordinal));
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DesignChange()
        {
            //NWDGUI.kSelectorTileDarkStyle.fixedHeight = kZoomHeightInt;
            //NWDGUI.kSelectorTileDarkStyle.fixedWidth = kZoomHeightInt;
            if (kZoom <= kZoomRowLimit)
            {
                //float tWidthRow = (SelectorWindow.position.width - NWDGUI.kScrollbar);
                //NWDGUI.kDatasSelectorRowStyle.fixedWidth = tWidthRow;
                NWDGUI.kDataSelectorRowStyle.fixedHeight = kZoom * NWDGUI.kDataSelectorFieldStyle.fixedHeight;
                RowSytle = NWDGUI.kDataSelectorRowStyle;
            }
            else
            {
                int kZoomHeightInt = Mathf.FloorToInt(kZoom * NWDGUI.kDataSelectorFieldStyle.fixedHeight);
                NWDGUI.kDataSelectorTileStyle.fixedHeight = kZoomHeightInt;
                NWDGUI.kDataSelectorTileStyle.fixedWidth = kZoomHeightInt;
                RowSytle = NWDGUI.kDataSelectorTileStyle;
            }

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnGUI()
        {
            NWDGUI.LoadStyles();
            if (NeedInitDesing == true)
            {
                DesignChange();
                NeedInitDesing = false;
            }


            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(NWDGUI.kIconWidth));
            GUILayout.Label(Helper.TextureOfClass(), GUILayout.Width(NWDGUI.kIconWidth), GUILayout.Height(NWDGUI.kIconWidth));
            GUILayout.EndVertical();
            GUILayout.BeginVertical();
            //NWDBenchmark.Start();
            //Vector2 tSelectionVector = SelectorWindow.ScrollPosition;
            //Debug.Log("OnGUI with selection : " + ActualSelection);
            string tNewInternalResearch = EditorGUILayout.TextField("Internal filter", InternalResearch);
            if (tNewInternalResearch != InternalResearch)
            {
                InternalResearch = tNewInternalResearch;
                Filter();
            }
            string tNewDescriptionResearch = EditorGUILayout.TextField("description filter", DescriptionResearch);
            if (tNewDescriptionResearch != DescriptionResearch)
            {
                DescriptionResearch = tNewDescriptionResearch;
                Filter();
            }
            NWDBasisTag tNewTag = (NWDBasisTag)EditorGUILayout.IntPopup(NWDConstants.K_APP_BASIS_INTERNAL_TAG,
                                                                       (int)Tag,
                                                                       TagStringList.ToArray(), TagIntList.ToArray());
            if (tNewTag != Tag)
            {
                Tag = tNewTag;
                Filter();
            }
            float tNextZoom = EditorGUILayout.Slider("Zoom", kZoom, kZoomRowMin, kZoomTileMax);
            if (Math.Abs(kZoom - tNextZoom) > 0.001F)
            {
                kZoom = tNextZoom;
                SavePreference();
                DesignChange();
            }
            GUILayout.BeginHorizontal();
            bool tEnableDatas = GUILayout.Toggle(EnableDatas, "Enable datas");
            if (EnableDatas != tEnableDatas)
            {
                EnableDatas = tEnableDatas;
                Filter();
            }
            bool tDisableDatas = GUILayout.Toggle(DisableDatas, "Disable datas");
            if (DisableDatas != tDisableDatas)
            {
                DisableDatas = tDisableDatas;
                Filter();
            }
            EditorGUI.BeginDisabledGroup(!DisableDatas);
            bool tTrashedDatas = GUILayout.Toggle(TrashedDatas, "Trashed datas");
            if (TrashedDatas != tTrashedDatas)
            {
                TrashedDatas = tTrashedDatas;
                Filter();
            }

            bool tCorruptedDatas = GUILayout.Toggle(CorruptedDatas, "Corrupted datas");
            if (CorruptedDatas != tCorruptedDatas)
            {
                CorruptedDatas = tCorruptedDatas;
                Filter();
            }
            EditorGUI.EndDisabledGroup();
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();
            GUILayout.Space(NWDGUI.kFieldMarge);
            NWDGUILayout.Line();
            bool tByTile = false;
            if (kZoom >= kZoomRowLimit)
            {
                tByTile = true;
            }
            //SelectorWindow.ScrollPosition = GUILayout.BeginScrollView(SelectorWindow.ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true));
            //SelectorWindow.ScrollPosition = GUILayout.BeginScrollView(SelectorWindow.ScrollPosition,false,true, GUILayout.ExpandWidth(false), GUILayout.ExpandHeight(true));
            SelectorWindow.ScrollPosition = GUILayout.BeginScrollView(SelectorWindow.ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            {
                float tWidth = (SelectorWindow.position.width - NWDGUI.kScrollbar - NWDGUI.kFieldMarge);
                int tColumn = 1;
                if (tByTile)
                {
                    tColumn = (int)Math.Floor(tWidth / NWDGUI.kDataSelectorTileStyle.fixedWidth);
                }
                if (tByTile)
                {
                    GUILayout.BeginHorizontal(GUILayout.Width(tWidth));
                }
                if (GUILayout.Button(new GUIContent(NWDConstants.kFieldNone), RowSytle))
                {
                    if (SelectedBlock != null)
                    {
                        SelectedBlock(SelectorWindow.ID, true, true, null);
                    }
                    SelectorWindow.Close();
                }
                int tIternation = 0;
                foreach (NWDTypeClass tItem in ResultList)
                {
                    tIternation++;
                    if (tItem != null)
                    {
                        if (tByTile)
                        {
                            if (tIternation >= tColumn)
                            {
                                tIternation = 0;
                                GUILayout.EndHorizontal();
                                GUILayout.BeginHorizontal(GUILayout.Width(tWidth));
                            }
                        }
                        GUIContent Content = tItem.Content;
                        if (Content == null)
                        {
                            //Content = new GUIContent(tItem.InternalKey, tItem.PreviewTexture2D(), tItem.InternalDescription);
                            Content = new GUIContent(tItem.InternalKey, tItem.PreviewTexture2D(), tItem.Reference + " : " + tItem.InternalDescription);
                        }
                        //GUIContent Content = tItem.Content;
                        if (ActualSelection == tItem.Reference)
                        {
                            NWDGUI.BeginColorArea(NWDGUI.kRowColorSelectedDark);
                            if (GUILayout.Button(Content, RowSytle))
                            {
                                if (SelectedBlock != null)
                                {
                                    SelectedBlock(SelectorWindow.ID, true, false, tItem);
                                }
                                SelectorWindow.Close();
                            }
                            NWDGUI.EndColorArea();
                        }
                        else
                        {
                            NWDGUI.BeginColorArea(tItem.DataSelectorBoxColor);
                            if (GUILayout.Button(Content, RowSytle))
                            {
                                if (SelectedBlock != null)
                                {
                                    SelectedBlock(SelectorWindow.ID, true, false, tItem);
                                }
                                SelectorWindow.Close();
                            }
                            NWDGUI.EndColorArea();
                        }
                    }
                    else
                    {
                        GUILayout.Label("ERROR", RowSytle);
                    }
                }
                if (tByTile)
                {
                    GUILayout.EndHorizontal();
                }
            }
            GUILayout.EndScrollView();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
