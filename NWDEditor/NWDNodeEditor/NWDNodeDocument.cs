//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:58
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using System;
using System.Reflection;
using System.IO;

using UnityEditor;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDClasseAnalyseEnum : int
    {
        None,
        Show,
        Analyze,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDNodeDocument
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The original data.
        /// </summary>
        private NWDNodeCard OriginalData;
        /// <summary>
        /// All cards. Used to prevent dupplicated card
        /// </summary>
        public List<NWDNodeCard> AllCards = new List<NWDNodeCard>();
        public List<NWDNodeCard> AllCardsAnalyzed = new List<NWDNodeCard>();
        public Dictionary<int, List<NWDNodeCard>> MatrixCards = new Dictionary<int, List<NWDNodeCard>>();
        //public Dictionary<string, NWDNodeLiner> LinerCards = new Dictionary<string, NWDNodeLiner>();
        /// <summary>
        /// The properties counter max found.
        /// </summary>
        private int PropertyMax = 0;
        /// <summary>
        /// The height of the marge.
        /// </summary>
        public float CardMarge = 100.0F;
        /// <summary>
        /// The width of the marge.
        /// </summary>
        /// <summary>
        /// The width of card (default). auto reevaluate.
        /// </summary>
        //private float Width = 300.0F;
        ///// <summary>
        ///// The height of card (default). auto reevaluate.
        ///// </summary>
        //public float Height = 100.0F;
        ///// <summary>
        ///// The margin cettween two card area.
        ///// </summary>
        ////public float Margin = 100.0F;
        ///// <summary>
        ///// The height of label.
        ///// </summary>
        //public float HeightLabel = 16.0F;
        ///// <summary>
        ///// The height of property
        ///// </summary>
        //public float HeightProperty = 20.0F;
        /// <summary>
        /// The list of max line by columns.
        /// </summary>
        private Dictionary<int, int> LineListMax = new Dictionary<int, int>();
        /// <summary>
        /// The column max.
        /// </summary>
        private int ColumnMax = 0;
        /// <summary>
        /// The line max.
        /// </summary>
        private int LineMax = 0;
        /// <summary>
        /// The height of the informations area.
        /// </summary>
        private float InformationsHeight = 50.0F;
        /// <summary>
        /// The force ortho cards. Cards have allways the same size
        /// </summary>
        //public bool ForceOrthoCards = false;
        /// <summary>
        /// The type list.
        /// </summary>
        public List<Type> TypeList;
        /// <summary>
        /// The classes analyzed.
        /// </summary>
        //public Dictionary<string, bool> AnalyzeTheseClasses = new Dictionary<string, bool>();
        /// <summary>
        /// The classes show.
        /// </summary>
        //public Dictionary<string, bool> ShowTheseClasses = new Dictionary<string, bool>();

        public float DocumentMarge = 220.0F;
        public float DocumentPrefHeight = 0;
        private float DocumentWidth = 0;
        private float DocumentHeight = 0;

        public List<string> ClassesUsed = new List<string>();
        public Dictionary<string, NWDClasseAnalyseEnum> AnalyzeStyleClasses = new Dictionary<string, NWDClasseAnalyseEnum>();
        /// <summary>
        /// The regroup properties in document.
        /// </summary>
        //public bool ReGroupProperties = false;
        //public bool UsedOnlyProperties = false;

        public string Language = NWDDataLocalizationManager.kBaseDev;

        public bool FixeMargePreference = true;

        public bool DrawInformationsArea = true;
        public bool DrawPropertiesArea = true;
        public bool DrawActionArea = false;
        public bool DrawAddOnArea = false;
        public NWDNodeEditor EditorWindow;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the <see cref="T:NetWorkedData.NWDNodeDocument"/> class.
        /// </summary>
        public NWDNodeDocument()
        {
            // TODO AnalyzeTheseTypes compelet by default from Class analyze
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Saves the preferences.
        /// </summary>
        public void SavePreferences()
        {
            //BTBBenchmark.Start();
            Dictionary<string, NWDClasseAnalyseEnum> tAnalyzeStyleClassesCopy = new Dictionary<string, NWDClasseAnalyseEnum>(AnalyzeStyleClasses);
            foreach (KeyValuePair<string, NWDClasseAnalyseEnum> tKeyValue in tAnalyzeStyleClassesCopy)
            {
                EditorPrefs.SetInt("NWDEditorNodal_" + tKeyValue.Key, (int)AnalyzeStyleClasses[tKeyValue.Key]);
            }
            EditorPrefs.SetString("NWDNodeEditorLanguage", Language);
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Loads the preferences.
        /// </summary>
        public void LoadPreferences()
        {
            //BTBBenchmark.Start();
            Dictionary<string, NWDClasseAnalyseEnum> tAnalyzeStyleClassesCopy = new Dictionary<string, NWDClasseAnalyseEnum>(AnalyzeStyleClasses);
            foreach (KeyValuePair<string, NWDClasseAnalyseEnum> tKeyValue in tAnalyzeStyleClassesCopy)
            {
                AnalyzeStyleClasses[tKeyValue.Key] = (NWDClasseAnalyseEnum)EditorPrefs.GetInt("NWDEditorNodal_" + tKeyValue.Key, (int)NWDClasseAnalyseEnum.Analyze);
            }
            Language = EditorPrefs.GetString("NWDNodeEditorLanguage");
            //BTBBenchmark.Finish();
            FixeMargePreference = EditorPrefs.GetBool("FixeMargePreference");
            DrawInformationsArea = EditorPrefs.GetBool("TopCard");
            DrawPropertiesArea = EditorPrefs.GetBool("MiddleCard");
            DrawActionArea = EditorPrefs.GetBool("BottomCard");
            DrawAddOnArea = EditorPrefs.GetBool("AddonnEditorCard");
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draws the preferences.
        /// </summary>
        public void DrawPreferences()
        {
            //BTBBenchmark.Start();
            bool tChanged = false;
            float tX = 0;
            float tY = 0;
            float tWHalf = (DocumentMarge - NWDGUI.kFieldMarge * 3) / 2.0f;
            float tW = (DocumentMarge - NWDGUI.kFieldMarge * 2);
            Rect tTotal = new Rect(tX, tY, DocumentMarge, DocumentHeight);
            // root object zone
            Rect tTitleRect = NWDGUI.Title(new Rect(tX, tY, DocumentMarge, NWDGUI.kTitleStyle.fixedHeight), "Root object");
            tY += tTitleRect.height + NWDGUI.kFieldMarge;
            if (GUI.Button(new Rect(tX + NWDGUI.kFieldMarge, tY, tW, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_EDITOR_NODE_SHOW_SELECTED_OBJECT, NWDGUI.kMiniButtonStyle))
            {
                NWDNodeEditor.SetObjectInNodeWindow((NWDTypeClass)NWDDataInspector.ShareInstance().mObjectInEdition);
                ReAnalyze();
            }
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            bool tFixeMargePreference = GUI.Toggle(new Rect(tX + NWDGUI.kFieldMarge, tY, tW, NWDGUI.kToggleStyle.fixedHeight), FixeMargePreference, "Fixe Pref marge");
            if (tFixeMargePreference != FixeMargePreference)
            {
                FixeMargePreference = tFixeMargePreference;
                EditorPrefs.SetBool("FixeMargePreference", FixeMargePreference);
                ReEvaluateLayout();
            }
            tY += NWDGUI.kToggleStyle.fixedHeight + NWDGUI.kFieldMarge;
            float tCardMarge = GUI.HorizontalSlider(new Rect(tX + NWDGUI.kFieldMarge, tY, tW, NWDGUI.kIntFieldStyle.fixedHeight), CardMarge, 50, 200);
            if (tCardMarge != CardMarge)
            {
                CardMarge = tCardMarge;
                ReEvaluateLayout();
            }
            tY += NWDGUI.kIntFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
            tTitleRect = NWDGUI.Title(new Rect(tX, tY, DocumentMarge, NWDGUI.kTitleStyle.fixedHeight),
                      "Analyze object");
            tY += tTitleRect.height + NWDGUI.kFieldMarge;
            bool tTopCard = GUI.Toggle(new Rect(tX + NWDGUI.kFieldMarge, tY, tW, NWDGUI.kToggleStyle.fixedHeight), DrawInformationsArea, "View informations");
            if (tTopCard != DrawInformationsArea)
            {
                DrawInformationsArea = tTopCard;
                EditorPrefs.SetBool("TopCard", DrawInformationsArea);
                ReEvaluateLayout();
            }
            tY += NWDGUI.kToggleStyle.fixedHeight + NWDGUI.kFieldMarge;

            bool tMiddleCard = GUI.Toggle(new Rect(tX + NWDGUI.kFieldMarge, tY, tW, NWDGUI.kToggleStyle.fixedHeight), DrawPropertiesArea, "View properties");
            if (tMiddleCard != DrawPropertiesArea)
            {
                DrawPropertiesArea = tMiddleCard;
                EditorPrefs.SetBool("MiddleCard", DrawPropertiesArea);
                ReEvaluateLayout();
            }
            tY += NWDGUI.kToggleStyle.fixedHeight + NWDGUI.kFieldMarge;

            bool tAddonnEditorCard = GUI.Toggle(new Rect(tX + NWDGUI.kFieldMarge, tY, tW, NWDGUI.kToggleStyle.fixedHeight), DrawAddOnArea, "View add-on");
            if (tAddonnEditorCard != DrawAddOnArea)
            {
                DrawAddOnArea = tAddonnEditorCard;
                EditorPrefs.SetBool("AddonnEditorCard", DrawAddOnArea);
                ReEvaluateLayout();
            }
            tY += NWDGUI.kToggleStyle.fixedHeight + NWDGUI.kFieldMarge;

            bool tBottomCard = GUI.Toggle(new Rect(tX + NWDGUI.kFieldMarge, tY, tW, NWDGUI.kToggleStyle.fixedHeight), DrawActionArea, "View action");
            if (tBottomCard != DrawActionArea)
            {
                DrawActionArea = tBottomCard;
                EditorPrefs.SetBool("BottomCard", DrawActionArea);
                ReEvaluateLayout();
            }
            tY += NWDGUI.kToggleStyle.fixedHeight + NWDGUI.kFieldMarge;
            // Localization Zone
            tTitleRect = NWDGUI.Title(new Rect(tX, tY, DocumentMarge, NWDGUI.kTitleStyle.fixedHeight),
                      "Localization test");
            tY += tTitleRect.height + NWDGUI.kFieldMarge;
            string tLanguage = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString;
            string[] tLanguageArray = tLanguage.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> tLocalizationList = new List<string>(tLanguageArray);
            int tIndexActual = tLocalizationList.IndexOf(Language);

            int tIndexNext = EditorGUI.Popup(new Rect(tX + NWDGUI.kFieldMarge, tY, tW, NWDGUI.kPopupStyle.fixedHeight), NWDConstants.K_EDITOR_NODE_CHOOSE_LANGUAGE, tIndexActual, tLocalizationList.ToArray());
            if (tIndexNext < 0 || tIndexNext >= tLocalizationList.Count)
            {
                tIndexNext = 0;
            }
            if (tIndexActual != tIndexNext)
            {
                Language = tLocalizationList[tIndexNext];
                EditorPrefs.SetString("NWDNodeEditorLanguage", Language);
            }
            tY += NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kFieldMarge;
            float tXA = NWDGUI.kFieldMarge;
            float tWidthB = NWDGUI.kEditIconSide;
            float tWidthA = DocumentMarge - NWDGUI.kEditIconSide - NWDGUI.kFieldMarge;
            float tXB = tWidthA + NWDGUI.kFieldMarge;
            tTitleRect = NWDGUI.Title(new Rect(tX, tY, DocumentMarge, NWDGUI.kTitleStyle.fixedHeight),
                      "Classses");
            tY += tTitleRect.height + NWDGUI.kFieldMarge;
            if (GUI.Button(new Rect(tX + NWDGUI.kFieldMarge, tY, tWHalf, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_EDITOR_NODE_SHOW_ALL, NWDGUI.kMiniButtonStyle))
            {
                Dictionary<string, NWDClasseAnalyseEnum> tAnalyzeStyleClassesCopyCopy = new Dictionary<string, NWDClasseAnalyseEnum>(AnalyzeStyleClasses);
                foreach (KeyValuePair<string, NWDClasseAnalyseEnum> tKeyValue in tAnalyzeStyleClassesCopyCopy)
                {
                    AnalyzeStyleClasses[tKeyValue.Key] = NWDClasseAnalyseEnum.Show;
                    EditorPrefs.SetInt("NWDEditorNodal_" + tKeyValue.Key, (int)AnalyzeStyleClasses[tKeyValue.Key]);
                }
                ReAnalyze();
            }
            if (GUI.Button(new Rect(tX + tWHalf + NWDGUI.kFieldMarge * 2, tY, tWHalf, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_EDITOR_NODE_MASK_ALL, NWDGUI.kMiniButtonStyle))
            {
                Dictionary<string, NWDClasseAnalyseEnum> tAnalyzeStyleClassesCopyCopy = new Dictionary<string, NWDClasseAnalyseEnum>(AnalyzeStyleClasses);
                foreach (KeyValuePair<string, NWDClasseAnalyseEnum> tKeyValue in tAnalyzeStyleClassesCopyCopy)
                {

                    AnalyzeStyleClasses[tKeyValue.Key] = NWDClasseAnalyseEnum.None;
                    EditorPrefs.SetInt("NWDEditorNodal_" + tKeyValue.Key, (int)AnalyzeStyleClasses[tKeyValue.Key]);
                }
                ReAnalyze();
            }
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            if (GUI.Button(new Rect(tX + NWDGUI.kFieldMarge, tY, tWHalf, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_EDITOR_NODE_ANALYZE_ALL, NWDGUI.kMiniButtonStyle))
            {
                Dictionary<string, NWDClasseAnalyseEnum> tAnalyzeStyleClassesCopyCopy = new Dictionary<string, NWDClasseAnalyseEnum>(AnalyzeStyleClasses);
                foreach (KeyValuePair<string, NWDClasseAnalyseEnum> tKeyValue in tAnalyzeStyleClassesCopyCopy)
                {
                    AnalyzeStyleClasses[tKeyValue.Key] = NWDClasseAnalyseEnum.Analyze;
                    EditorPrefs.SetInt("NWDEditorNodal_" + tKeyValue.Key, (int)AnalyzeStyleClasses[tKeyValue.Key]);
                }
                ReAnalyze();
            }
            tY += NWDGUI.kMiniButtonStyle.fixedHeight + NWDGUI.kFieldMarge;
            //if (tNextReduce == true)
            {
                // to show

                int tCounter = 0;
                Dictionary<string, NWDClasseAnalyseEnum> tAnalyzeStyleClasses = new Dictionary<string, NWDClasseAnalyseEnum>(AnalyzeStyleClasses);
                foreach (KeyValuePair<string, NWDClasseAnalyseEnum> tKeyValue in tAnalyzeStyleClasses)
                {

                    if (ClassesUsed.Contains(tKeyValue.Key))
                    {
                        //Debug.Log("tWidthA loop = " + tWidthA.ToString());
                        NWDClasseAnalyseEnum tNew = (NWDClasseAnalyseEnum)EditorGUI.EnumPopup(new Rect(tX + tXA, tY, DocumentMarge - NWDGUI.kFieldMarge * 2, 20), tKeyValue.Key, tKeyValue.Value);
                        if (AnalyzeStyleClasses[tKeyValue.Key] != tNew)
                        {
                            tChanged = true;
                        }
                        AnalyzeStyleClasses[tKeyValue.Key] = tNew;
                        tCounter++;
                        tY += 20;
                    }
                }
                if (tChanged == true)
                {
                    SavePreferences();
                    ReAnalyze();
                }
            }

            DocumentPrefHeight = tY;
            DocumentHeight = Math.Max(DocumentPrefHeight, DocumentHeight);
            if (FixeMargePreference == false)
            {
                if(EditorWindow!=null)
                {
                Handles.color = NWDGUI.kNodeCanvasMargeBlack;
                Handles.DrawLine(new Vector2(tX + DocumentMarge, 0), new Vector2(tX + DocumentMarge, Math.Max(EditorWindow.position.height, DocumentHeight)));
                }
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public float DrawAnalyzer(Rect sRect, NWDNodeCard sNodalCard, string sClassName)
        {
            EditorGUI.BeginDisabledGroup(NWDDataInspector.ShareInstance().mObjectInEdition == sNodalCard.DataObject);
            if (GUI.Button(new Rect(sRect.x, sRect.y, NWDGUI.kEditWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDGUI.kEditContentIcon, NWDGUI.kMiniButtonStyle))
            {
                NWDDataInspector.ShareInstance().Data(sNodalCard.DataObject);
            }
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(NWDNodeEditor.SharedInstance().Document.OriginalData == sNodalCard);
            if (GUI.Button(new Rect(sRect.x + NWDGUI.kFieldMarge + NWDGUI.kEditWidth, sRect.y, NWDGUI.kEditWidth, NWDGUI.kMiniButtonStyle.fixedHeight), NWDConstants.K_BUTTON_ROOT_NODAL, NWDGUI.kMiniButtonStyle))
            {
                NWDNodeEditor.SetObjectInNodeWindow(sNodalCard.DataObject);
            }
            EditorGUI.EndDisabledGroup();
            NWDClasseAnalyseEnum tNew = (NWDClasseAnalyseEnum)EditorGUI.EnumPopup(new Rect(sRect.x + sRect.width - NWDGUI.kEditWidth * 2, sRect.y, NWDGUI.kEditWidth * 2, NWDGUI.kMiniButtonStyle.fixedHeight), AnalyzeStyleClasses[sClassName]);
            if (AnalyzeStyleClasses[sClassName] != tNew)
            {
                AnalyzeStyleClasses[sClassName] = tNew;
                SavePreferences();
                ReAnalyze();
            }
            return NWDGUI.kPopupStyle.fixedHeight + NWDGUI.kFieldMarge;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Dimension this instance.
        /// </summary>
        /// <returns>The dimension.</returns>
        public Rect Dimension()
        {
            return new Rect(0, 0, DocumentWidth, DocumentHeight);
        }
        //-------------------------------------------------------------------------------------------------------------
        public Rect DimensionB()
        {
            return new Rect(0, 0, DocumentMarge, DocumentHeight);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Reevaluate layout.
        /// </summary>
        public void ReEvaluateLayout()
        {
            //BTBBenchmark.Start();
            DocumentWidth = DocumentMarge;
            foreach (KeyValuePair<int, List<NWDNodeCard>> tColumnList in MatrixCards)
            {
                DocumentWidth += CardMarge + NWDGUI.kNodeCardWidth;
                float tY = CardMarge;
                foreach (NWDNodeCard tCard in tColumnList.Value)
                {
                    tY += CardMarge + tCard.ReEvaluateLayout(tY);
                }
                DocumentHeight = Math.Max(DocumentHeight, tY);
            }
            DocumentWidth += CardMarge;
            DocumentHeight = Math.Max(DocumentHeight, DocumentPrefHeight);

            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the height of the informations area.
        /// </summary>
        /// <param name="sInformationsHeight">S informations height.</param>
        public void SetInformationsHeight(float sInformationsHeight)
        {
            InformationsHeight = Mathf.Max(InformationsHeight, sInformationsHeight);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the height of the informations.
        /// </summary>
        /// <returns>The informations height.</returns>
        public float GetInformationsHeight()
        {
            return InformationsHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Columns the max count.
        /// </summary>
        /// <param name="sCount">S count.</param>
        public void ColumnMaxCount(int sCount)
        {
            ColumnMax = Math.Max(ColumnMax, sCount);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the column max.
        /// </summary>
        /// <returns>The column max.</returns>
        public int GetColumnMax()
        {
            return ColumnMax;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Lines the max count.
        /// </summary>
        /// <param name="sCount">S count.</param>
        public void LineMaxCount(int sCount)
        {
            LineMax = Math.Max(LineMax, sCount);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the line max.
        /// </summary>
        /// <returns>The line max.</returns>
        public int GetLineMax()
        {
            return LineMax;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Properties the count.
        /// </summary>
        /// <param name="sCount">S count.</param>
        public void PropertyCount(int sCount)
        {
            PropertyMax = Math.Max(PropertyMax, sCount);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the property max.
        /// </summary>
        /// <returns>The property max.</returns>
        public int GetPropertyMax()
        {
            return PropertyMax;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the next line.
        /// </summary>
        /// <returns>The next line.</returns>
        /// <param name="sCard">S card.</param>
        public int GetNextLine(NWDNodeCard sCard)
        {
            int rResult = 0;
            if (LineListMax.ContainsKey(sCard.Column))
            {
                LineListMax[sCard.Column]++;
                rResult = LineListMax[sCard.Column];
            }
            else
            {
                LineListMax.Add(sCard.Column, rResult);
            }
            LineMaxCount(rResult);
            return rResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Loads the classes.
        /// </summary>
        public void LoadClasses()
        {
            //BTBBenchmark.Start();
            AnalyzeStyleClasses = new Dictionary<string, NWDClasseAnalyseEnum>();
            TypeList = NWDDataManager.SharedInstance().mTypeList;
            TypeList.Sort((tA, tB) => string.Compare(tA.Name, tB.Name, StringComparison.Ordinal));
            foreach (Type tType in TypeList)
            {
                AnalyzeStyleClasses.Add(tType.Name, NWDClasseAnalyseEnum.Analyze);
            }
            AnalyzeStyleClasses = new Dictionary<string, NWDClasseAnalyseEnum>();
            TypeList.Sort((tA, tB) => string.Compare(tA.Name, tB.Name, StringComparison.Ordinal));
            foreach (Type tType in TypeList)
            {
                AnalyzeStyleClasses.Add(tType.Name, NWDClasseAnalyseEnum.Analyze);
            }
            LoadPreferences();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="sObject">S object.</param>
        /// <param name="tReset">If set to <c>true</c> t reset.</param>
        public void SetData(NWDTypeClass sObject, bool tReset = true)
        {
            //BTBBenchmark.Start();
            if (NWDBasisHelper.FindTypeInfos(sObject.GetType()).DatabaseIsLoaded())
            {
                if (tReset == true)
                {
                    LoadClasses();
                }
                ClassesUsed.Clear();
                AllCards = new List<NWDNodeCard>();
                AllCardsAnalyzed = new List<NWDNodeCard>();
                MatrixCards = new Dictionary<int, List<NWDNodeCard>>();
                PropertyMax = 0;
                LineListMax = new Dictionary<int, int>();
                ColumnMax = 0;
                LineMax = 0;
                InformationsHeight = 50.0F;
                if (sObject != null)
                {
                    OriginalData = new NWDNodeCard();
                    OriginalData.Line = 0;
                    OriginalData.Column = 0;
                    OriginalData.Position = new Vector2(0, 0);
                    OriginalData.SetData(sObject);
                    AllCards.Add(OriginalData);
                    MatrixCards.Add(0, new List<NWDNodeCard>());
                    MatrixCards[0].Add(OriginalData);
                    OriginalData.Analyze(this);
                }
                ReEvaluateLayout();
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Res the analyze.
        /// </summary>
        public void ReAnalyze()
        {
            //BTBBenchmark.Start();
            if (OriginalData != null)
            {
                SetData(OriginalData.DataObject, false);
            }
            ReEvaluateLayout();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Re the analyze.
        /// </summary>
        public void ReAnalyzeIfNecessary(object sObjectModified)
        {
            //BTBBenchmark.Start();
            if (OriginalData != null)
            {
                bool tNeedBeReAnalyze = false;
                foreach (NWDNodeCard tCard in AllCards)
                {
                    if (sObjectModified == tCard.DataObject)
                    {
                        tNeedBeReAnalyze = true;
                        break;
                    }
                }
                if (tNeedBeReAnalyze == true)
                {
                    SetData(OriginalData.DataObject, false);
                }
            }
            ReEvaluateLayout();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draw the specified sViewRect.
        /// </summary>
        /// <returns>The draw.</returns>
        /// <param name="sViewRect">S view rect.</param>
        public void Draw(Rect sViewRect, Rect sVisibleRect)
        {
            //BTBBenchmark.Start();
            DrawCanvas(sViewRect);
            DrawBackgroundLine();
            DrawCard(sVisibleRect);
            DrawForwardPlot();
            if (FixeMargePreference == false)
            {
                DrawPreferences();
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draws the canvas.
        /// </summary>
        /// <param name="sViewRect">S view rect.</param>
        public void DrawCanvas(Rect sViewRect)
        {
            //BTBBenchmark.Start();
            Rect sDocumentRect = Dimension();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draws the background plot.
        /// </summary>
        public void DrawBackgroundLine()
        {
            //BTBBenchmark.Start();
            foreach (NWDNodeCard tCard in AllCards)
            {
                tCard.DrawConnection(AllCards);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draws the card.
        /// </summary>
        public void DrawCard(Rect sVisibleRect)
        {
            //BTBBenchmark.Start();
            foreach (NWDNodeCard tCard in AllCards)
            {
                tCard.DrawCard(sVisibleRect);
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draws the forward plot.
        /// </summary>
        public void DrawForwardPlot()
        {
            //BTBBenchmark.Start();
            foreach (NWDNodeCard tCard in AllCards)
            {
                tCard.DrawForwardPlot();
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
