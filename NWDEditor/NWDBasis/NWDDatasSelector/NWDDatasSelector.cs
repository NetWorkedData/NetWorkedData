// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:48
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
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
        List<NWDTypeClass> ResultList = new List<NWDTypeClass>();
        string ActualSelection;
        static public string Field(NWDBasisHelper sHelper, Rect sRect, GUIContent sContent, string sReference, float sInsertion = 0)
        {
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
            Rect tEntitlement = new Rect(sRect.position.x, sRect.position.y, EditorGUIUtility.labelWidth, NWDGUI.kDatasSelectorRowStyle.fixedHeight);
            Rect tField = new Rect(sRect.position.x + EditorGUIUtility.labelWidth, sRect.position.y, sRect.width - EditorGUIUtility.labelWidth - NWDGUI.kEditWidth - NWDGUI.kFieldMarge - sInsertion, NWDGUI.kDatasSelectorRowStyle.fixedHeight);
            Rect tEditRect = new Rect(sRect.position.x + sRect.width - NWDGUI.kEditWidth, sRect.position.y + NWDGUI.kDatasSelectorYOffset, NWDGUI.kEditWidth, NWDGUI.kMiniButtonStyle.fixedHeight);

            tEntitlement = EditorGUI.IndentedRect(tEntitlement);
            GUI.Label(tEntitlement, sContent, NWDGUI.kPropertyEntitlementStyle);

            GUIContent sDataLabel = sHelper.New_GetGuiContent(sReference);
            NWDTypeClass tData = sHelper.New_GetDataByReference(sReference);
            if (string.IsNullOrEmpty(sReference) == false && sHelper.New_GetDataByReference(sReference) == null)
            {
                GUI.Label(tField, sDataLabel, NWDGUI.kDatasSelectorRowErrorStyle);

                NWDGUI.BeginRedArea();
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
                    else if (tData.TestIntegrity() == false)
                    {
                        NWDGUI.BeginColorArea(NWDGUI.kRowColorWarning);
                    }
                    else
                    {
                        NWDGUI.EndColorArea();
                    }
                }
                if (GUI.Button(tField, sDataLabel, NWDGUI.kDatasSelectorRowStyle))
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

                    if (GUI.Button(tEditRect, NWDGUI.kEditContentIcon, NWDGUI.kEditButtonStyle))
                    {
                        sHelper.New_SetObjectInEdition(sHelper.New_GetDataByReference(sReference), false);
                    }
                }
                else
                {
                    if (GUI.Button(tEditRect, NWDGUI.kNewContentIcon, NWDGUI.kEditButtonStyle))
                    {
                        NWDTypeClass tNewObject = sHelper.New_NewData();
                        if (ControllerResult.ContainsKey(tID))
                        {
                            ControllerResult.Remove(tID);
                        }
                        ControllerResult.Add(tID, tNewObject.Reference);


                        //NWDBasis<K>.SetObjectInEdition(tNewObject, false, true);

                        //NWDDataManager.SharedInstance().RepaintWindowsInManager(tNewObject.GetType());
                    }
                }
            }
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
            rReturn.Show(sHelper ,- 2, sInitialInternalResearch, sInitialDescriptionResearch, sTag, sSelectedBlock, sSelection);
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
            Initialization();
            if (Datas == null)
            {
                Datas = sHelper.Datas;
            }
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
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnGUI()
        {
            NWDGUI.LoadStyles();
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
                NWDGUI.kSelectorTileStyle.fixedHeight = kZoom * 100;
                NWDGUI.kSelectorTileStyle.fixedWidth = kZoom * 100;
                NWDGUI.kSelectorTileDarkStyle.fixedHeight = kZoom * 100;
                NWDGUI.kSelectorTileDarkStyle.fixedWidth = kZoom * 100;
            }

            NWDGUILayout.Separator();

            SelectorWindow.ScrollPosition = GUILayout.BeginScrollView(SelectorWindow.ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (kZoom <= 1)
            {
                if (GUILayout.Button(new GUIContent("none"), NWDGUI.kSelectorRowStyle))
                {
                    if (SelectedBlock != null)
                    {
                        SelectedBlock(SelectorWindow.ID, true, true, null);
                    }
                    SelectorWindow.Close();
                }
                foreach (NWDTypeClass tItem in ResultList)
                {
                    GUIContent Content = new GUIContent(tItem.InternalKey, tItem.PreviewTexture2D(), tItem.InternalDescription);
                    GUIStyle tSytle = NWDGUI.kSelectorRowStyle;
                    if (ActualSelection == tItem.Reference)
                    {
                        tSytle = NWDGUI.kSelectorRowDarkStyle;
                        if (SelectorWindow.ScrollInit == false && Event.current.type == EventType.Repaint)
                        {
                            Rect tLastRect = GUILayoutUtility.GetLastRect();
                            tSelectionVector = new Vector2(tLastRect.x, tLastRect.y);
                            //Debug.Log("tSelectionVector init at " + tSelectionVector.ToString());
                        }
                        NWDGUI.BeginColorArea(NWDGUI.kSelectorRowSelected);
                        if (GUILayout.Button(Content, tSytle))
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
                        if (tItem.IsTrashed())
                        {
                            NWDGUI.BeginColorArea(NWDGUI.kRowColorTrash);
                        }
                        else if (tItem.IsEnable() ==false)
                        {
                            NWDGUI.BeginColorArea(NWDGUI.kRowColorDisactive);
                        }
                        else if (tItem.TestIntegrity() ==false)
                        {
                            NWDGUI.BeginColorArea(NWDGUI.kRowColorWarning);
                        }
                        else
                        {
                            NWDGUI.EndColorArea();
                        }
                        if (GUILayout.Button(Content, tSytle))
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
            }
            else
            {
                float tWidth = (SelectorWindow.position.width - NWDGUI.kScrollbar);
                int tColumn = (int)Math.Floor(tWidth / NWDGUI.kSelectorTileStyle.fixedWidth);
                GUILayout.BeginHorizontal(GUILayout.Width(tWidth));
                if (GUILayout.Button(new GUIContent("none"), NWDGUI.kSelectorTileStyle))
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
                    if (tIternation >= tColumn)
                    {
                        tIternation = 0;
                        GUILayout.EndHorizontal();
                        GUILayout.BeginHorizontal(GUILayout.Width(tWidth));
                    }
                    GUIContent Content = new GUIContent(tItem.InternalKey, tItem.PreviewTexture2D(), tItem.InternalDescription);
                    GUIStyle tSytle = NWDGUI.kSelectorTileStyle;
                    if (ActualSelection == tItem.Reference)
                    {
                        tSytle = NWDGUI.kSelectorTileDarkStyle;
                        if (SelectorWindow.ScrollInit == false && Event.current.type == EventType.Repaint)
                        {
                            Rect tLastRect = GUILayoutUtility.GetLastRect();
                            tSelectionVector = new Vector2(tLastRect.x, tLastRect.y);
                            //Debug.Log("tSelectionVector init at " + tSelectionVector.ToString());
                        }
                        NWDGUI.BeginColorArea(NWDGUI.kSelectorTileSelected);
                        if (GUILayout.Button(Content, tSytle))
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
