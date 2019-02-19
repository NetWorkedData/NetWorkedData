//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEditor;
using System.Text;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDDatasSelectorWindow : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        GUIContent IconAndTitle;
        public Vector2 ScrollPosition;
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
            //if (SelectorBasis != null)
            //{
            //    if (SelectorBasis.SelectedBlock != null)
            //    {
            //        SelectorBasis.SelectedBlock(null);
            //    }
            //}
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
        public delegate void NWDDatasSelectorBlock(NWDTypeClass sSelection);
        public NWDDatasSelectorBlock SelectedBlock;
        //-------------------------------------------------------------------------------------------------------------
        public NWDDatasSelectorWindow SelectorWindow;
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
        static float kZoom = 1.0F;
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisHelper Helper;
        List<int> TagIntList = new List<int>();
        List<string> TagStringList = new List<string>();
        //-------------------------------------------------------------------------------------------------------------
        public string InternalResearch = "";
        public string DescriptionResearch = "";
        public NWDBasisTag Tag;
        List<K> ResultList = new List<K>();
        //-------------------------------------------------------------------------------------------------------------
        static public void ShowNow(string sInitialInternalResearch = "",
                                string sInitialDescriptionResearch = "",
                                NWDBasisTag sTag = NWDBasisTag.NoTag,
                                NWDDatasSelectorBlock sSelectedBlock = null)
        {
            NWDDatasSelector<K> rReturn = new NWDDatasSelector<K>();
            rReturn.Show(sInitialInternalResearch, sInitialDescriptionResearch, sTag, sSelectedBlock);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Show(string sInitialInternalResearch = "",
                            string sInitialDescriptionResearch = "",
                            NWDBasisTag sTag = NWDBasisTag.NoTag,
                            NWDDatasSelectorBlock sSelectedBlock = null)
        {
            Initialization();
            Helper = NWDBasis<K>.BasisHelper();
            InternalResearch = sInitialInternalResearch;
            DescriptionResearch = sInitialDescriptionResearch;
            Tag = sTag;
            SelectedBlock = sSelectedBlock;
            Filter();
            SelectorWindow = ScriptableObject.CreateInstance(typeof(NWDDatasSelectorWindow)) as NWDDatasSelectorWindow;
            //SelectorWindow = EditorWindow.GetWindow(typeof(NWDDatasSelectorWindow)) as NWDDatasSelectorWindow;
            SelectorWindow.SelectorBasis = this;
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
            // TODO : create filter

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
            float tNextZoom = EditorGUILayout.Slider("Zoom",kZoom, 1.0F, 2.0F);
            if (Math.Abs(kZoom - tNextZoom) > 0.001F)
            {
                kZoom = tNextZoom;
                NWDConstants.kSelectorTileStyle.fixedHeight = kZoom*100;
                NWDConstants.kSelectorTileStyle.fixedWidth = kZoom*100;
            }

            NWDConstants.GUILayoutSeparator();

            SelectorWindow.ScrollPosition = GUILayout.BeginScrollView(SelectorWindow.ScrollPosition, NWDConstants.kInspectorFullWidthMargins, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (kZoom <= 1)
            {
                if (GUILayout.Button(new GUIContent("none"), NWDConstants.kSelectorRowStyle))
                {
                    if (SelectedBlock != null)
                    {
                        SelectedBlock(null);
                    }
                    SelectorWindow.Close();
                }
                foreach (K tItem in ResultList)
                {
                    GUIContent Content = new GUIContent(tItem.InternalKey, tItem.GetPreviewTexture2D(), tItem.InternalDescription);
                    if (GUILayout.Button(Content, NWDConstants.kSelectorRowStyle))
                    {
                        if (SelectedBlock != null)
                        {
                            SelectedBlock(tItem);
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
                        SelectedBlock(null);
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
                    if (GUILayout.Button(Content, NWDConstants.kSelectorTileStyle))
                    {
                        if (SelectedBlock != null)
                        {
                            SelectedBlock(tItem);
                        }
                        SelectorWindow.Close();
                    }
                }
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
