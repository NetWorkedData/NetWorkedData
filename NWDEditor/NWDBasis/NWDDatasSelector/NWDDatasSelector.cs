//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDDatasSelectorWindow : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public int ID = -1;
        GUIContent IconAndTitle;
        public Vector2 ScrollPosition;
        public bool ScrollInit = false;
        public NWDDatasSelectorBasis SelectorBasis;
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
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
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnLostFocus()
        {
            if (SelectorBasis != null)
            {
                if (SelectorBasis.SelectedBlock != null)
                {
                    SelectorBasis.SelectedBlock(ID, false, false, null);
                }
            }
            Close();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnGUI()
        {
            if (SelectorBasis != null)
            {
                SelectorBasis.OnGUI();
            }
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
        public static float kZoom = 1.0F;
        public static Dictionary<int, string> ControllerResult = new Dictionary<int, string>();
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnGUI()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDDatasSelector<K> : NWDDatasSelectorBasis where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisHelper Helper;
        List<int> TagIntList = new List<int>();
        List<string> TagStringList = new List<string>();
        //-------------------------------------------------------------------------------------------------------------
        public string InternalResearch = "";
        public string DescriptionResearch = "";
        public NWDBasisTag Tag;
        List<K> ResultList = new List<K>();
        string ActualSelection;
        static public string Field(Rect sRect, GUIContent sContent, string sReference, float sInsertion = 0)
        {
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
            Rect tEntitlement = new Rect(sRect.position.x, sRect.position.y, EditorGUIUtility.labelWidth, NWDConstants.kDatasSelectorRowStyle.fixedHeight);
            Rect tField = new Rect(sRect.position.x + EditorGUIUtility.labelWidth, sRect.position.y, sRect.width - EditorGUIUtility.labelWidth - NWDConstants.kEditWidth - NWDConstants.kFieldMarge - sInsertion, NWDConstants.kDatasSelectorRowStyle.fixedHeight);
            Rect tEditRect = new Rect(sRect.position.x + sRect.width - NWDConstants.kEditWidth, sRect.position.y, NWDConstants.kEditWidth, NWDConstants.kMiniButtonStyle.fixedHeight);

            tEntitlement = EditorGUI.IndentedRect(tEntitlement);
            GUI.Label(tEntitlement, sContent, EditorStyles.label);

            GUIContent sDataLabel = NWDBasis<K>.GetGuiContent(sReference);
            if (string.IsNullOrEmpty(sReference) == false && NWDBasis<K>.GetDataByReference(sReference) == null)
            {
                GUI.Label(tField, sDataLabel, NWDConstants.kDatasSelectorRowErrorStyle);

                NWDConstants.GUIRedButtonBegin();
                if (GUI.Button(tEditRect, NWDConstants.tCleanContent, NWDConstants.kMiniButtonStyle))
                {
                    if (ControllerResult.ContainsKey(tID))
                    {
                        ControllerResult.Remove(tID);
                    }
                    ControllerResult.Add(tID, string.Empty);
                }
                NWDConstants.GUIRedButtonEnd();
            }
            else
            {
                if (GUI.Button(tField, sDataLabel, NWDConstants.kDatasSelectorRowStyle))
                {
                    ShowNow(tID, "", "", NWDBasisTag.NoTag, delegate (int sID, bool sChange, bool sNone, NWDTypeClass sSelection)
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
                                    bResult = sSelection.ReferenceValue();
                                }
                            }
                            if (ControllerResult.ContainsKey(sID))
                            {
                                ControllerResult.Remove(sID);
                            }
                            ControllerResult.Add(sID, bResult);
                        }
                    }, sReference);
                }
                if (string.IsNullOrEmpty(sReference) == false)
                {

                    if (GUI.Button(tEditRect, NWDConstants.tEditContent, NWDConstants.kMiniButtonStyle))
                    {
                        NWDBasis<K>.SetObjectInEdition(NWDBasis<K>.GetDataByReference(sReference), false);
                    }
                }
                else
                {
                    if (GUI.Button(tEditRect, NWDConstants.tNewContent, NWDConstants.kMiniButtonStyle))
                    {
                        NWDBasis<K> tNewObject = NWDBasis<K>.NewData();
                        if (ControllerResult.ContainsKey(tID))
                        {
                            ControllerResult.Remove(tID);
                        }
                        ControllerResult.Add(tID, tNewObject.Reference);
                        NWDBasis<K>.SetObjectInEdition(tNewObject, false, true);

                        //NWDDataManager.SharedInstance().RepaintWindowsInManager(tNewObject.GetType());
                    }
                }
            }
            return tReference;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void ShowNow(int sID, string sInitialInternalResearch = "",
                                string sInitialDescriptionResearch = "",
                                NWDBasisTag sTag = NWDBasisTag.NoTag,
                                NWDDatasSelectorBlock sSelectedBlock = null,
                                string sSelection = "")
        {
            NWDDatasSelector<K> rReturn = new NWDDatasSelector<K>();
            rReturn.Show(sID, sInitialInternalResearch, sInitialDescriptionResearch, sTag, sSelectedBlock, sSelection);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void ShowNow(string sInitialInternalResearch = "",
                                string sInitialDescriptionResearch = "",
                                NWDBasisTag sTag = NWDBasisTag.NoTag,
                                NWDDatasSelectorBlock sSelectedBlock = null,
                                string sSelection = "")
        {
            NWDDatasSelector<K> rReturn = new NWDDatasSelector<K>();
            rReturn.Show(-2, sInitialInternalResearch, sInitialDescriptionResearch, sTag, sSelectedBlock, sSelection);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Show(string sInitialInternalResearch = "",
                            string sInitialDescriptionResearch = "",
                            NWDBasisTag sTag = NWDBasisTag.NoTag,
                            NWDDatasSelectorBlock sSelectedBlock = null,
                            string sSelection = "")
        {
            Show(-2, sInitialInternalResearch, sInitialDescriptionResearch, sTag, sSelectedBlock, sSelection);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Show(int sID, string sInitialInternalResearch = "",
                        string sInitialDescriptionResearch = "",
                        NWDBasisTag sTag = NWDBasisTag.NoTag,
                        NWDDatasSelectorBlock sSelectedBlock = null,
                        string sSelection = "")
        {
            Initialization();
            Helper = NWDBasis<K>.BasisHelper();
            InternalResearch = sInitialInternalResearch;
            DescriptionResearch = sInitialDescriptionResearch;
            Tag = sTag;
            SelectedBlock = sSelectedBlock;
            if (sSelection != null)
            {
                ActualSelection = sSelection;
            }
            else
            {
                ActualSelection = "";
            }
            Filter();
            SelectorWindow = ScriptableObject.CreateInstance(typeof(NWDDatasSelectorWindow)) as NWDDatasSelectorWindow;
            //SelectorWindow = EditorWindow.GetWindow(typeof(NWDDatasSelectorWindow)) as NWDDatasSelectorWindow;
            SelectorWindow.SelectorBasis = this;
            SelectorWindow.ScrollInit = false;
            SelectorWindow.ID = sID;
            SelectorWindow.ShowUtility();
            SelectorWindow.Focus();
        }
        //-------------------------------------------------------------------------------------------------------------
        void Initialization()
        {
            // Tag management
            foreach (KeyValuePair<int, string> tTag in NWDAppConfiguration.SharedInstance().TagList)
            {
                TagIntList.Add(tTag.Key);
                TagStringList.Add(tTag.Value);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void Filter()
        {
            ResultList = new List<K>();
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
            foreach (K tItem in NWDBasis<K>.BasisHelper().Datas)
            {
                bool tInclude = true;
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
            ResultList.Sort((tA, tB) => string.Compare(tA.InternalKeyValue(), tB.InternalKeyValue(), StringComparison.Ordinal));
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnGUI()
        {
            NWDConstants.LoadStyles();
            Vector2 tSelectionVector = SelectorWindow.ScrollPosition;
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
            float tNextZoom = EditorGUILayout.Slider("Zoom", kZoom, 1.0F, 2.0F);
            if (Math.Abs(kZoom - tNextZoom) > 0.001F)
            {
                kZoom = tNextZoom;
                NWDConstants.kSelectorTileStyle.fixedHeight = kZoom * 100;
                NWDConstants.kSelectorTileStyle.fixedWidth = kZoom * 100;
            }

            NWDConstants.GUILayoutSeparator();

            SelectorWindow.ScrollPosition = GUILayout.BeginScrollView(SelectorWindow.ScrollPosition, NWDConstants.kInspectorFullWidthMargins, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (kZoom <= 1)
            {
                if (GUILayout.Button(new GUIContent("none"), NWDConstants.kSelectorRowStyle))
                {
                    if (SelectedBlock != null)
                    {
                        SelectedBlock(SelectorWindow.ID, true, true, null);
                    }
                    SelectorWindow.Close();
                }
                foreach (K tItem in ResultList)
                {
                    GUIContent Content = new GUIContent(tItem.InternalKey, tItem.GetPreviewTexture2D(), tItem.InternalDescription);
                    GUIStyle tSytle = NWDConstants.kSelectorRowStyle;
                    if (ActualSelection == tItem.ReferenceValue())
                    {
                        tSytle = NWDConstants.kSelectorRowDarkStyle;
                        if (SelectorWindow.ScrollInit == false && Event.current.type == EventType.Repaint)
                        {
                            Rect tLastRect = GUILayoutUtility.GetLastRect();
                            tSelectionVector = new Vector2(tLastRect.x, tLastRect.y);
                            //Debug.Log("tSelectionVector init at " + tSelectionVector.ToString());
                        }
                    }
                    if (GUILayout.Button(Content, tSytle))
                    {
                        if (SelectedBlock != null)
                        {
                            SelectedBlock(SelectorWindow.ID, true, false, tItem);
                        }
                        SelectorWindow.Close();
                    }
                }
            }
            else
            {
                float tWidth = (SelectorWindow.position.width - NWDConstants.BoxMarge);
                int tColumn = (int)Math.Floor(tWidth / NWDConstants.kSelectorTileStyle.fixedWidth);
                GUILayout.BeginHorizontal(GUILayout.Width(tWidth));
                if (GUILayout.Button(new GUIContent("none"), NWDConstants.kSelectorTileStyle))
                {
                    if (SelectedBlock != null)
                    {
                        SelectedBlock(SelectorWindow.ID, true, true, null);
                    }
                    SelectorWindow.Close();
                }
                int tIternation = 0;
                foreach (K tItem in ResultList)
                {
                    tIternation++;
                    if (tIternation >= tColumn)
                    {
                        tIternation = 0;
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(GUILayout.Width(tWidth));
                    }
                    GUIContent Content = new GUIContent(tItem.InternalKey, tItem.GetPreviewTexture2D(), tItem.InternalDescription);
                    GUIStyle tSytle = NWDConstants.kSelectorTileStyle;
                    if (ActualSelection == tItem.ReferenceValue())
                    {
                        tSytle = NWDConstants.kSelectorTileDarkStyle;
                        if (SelectorWindow.ScrollInit == false && Event.current.type == EventType.Repaint)
                        {
                            Rect tLastRect = GUILayoutUtility.GetLastRect();
                            tSelectionVector = new Vector2(tLastRect.x, tLastRect.y);
                            //Debug.Log("tSelectionVector init at " + tSelectionVector.ToString());
                        }
                    }
                    if (GUILayout.Button(Content, tSytle))
                    {
                        if (SelectedBlock != null)
                        {
                            SelectedBlock(SelectorWindow.ID, true, false, tItem);
                        }
                        SelectorWindow.Close();
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();

            if (SelectorWindow.ScrollInit == false && Event.current.type == EventType.Repaint)
            {
                SelectorWindow.ScrollInit = true;
                SelectorWindow.ScrollPosition = tSelectionVector;
                //Debug.Log("SelectorWindow.ScrollPosition init at " + SelectorWindow.ScrollPosition.ToString());
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
