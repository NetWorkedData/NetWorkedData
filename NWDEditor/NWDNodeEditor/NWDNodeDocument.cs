using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

using UnityEngine;
using System;
using System.Reflection;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
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
        /// <summary>
        /// The properties counter max found.
        /// </summary>
        private int PropertyMax = 0;
        /// <summary>
        /// The height of the marge.
        /// </summary>
        public float MargeHeight = 300.0F;
        /// <summary>
        /// The width of the marge.
        /// </summary>
        public float MargeWidth = 220.0F;
        /// <summary>
        /// The width of card (default). auto reevaluate.
        /// </summary>
        private float Width = 200.0F;
        /// <summary>
        /// The height of card (default). auto reevaluate.
        /// </summary>
        public float Height = 100.0F;
        /// <summary>
        /// The margin cettween two card area.
        /// </summary>
        public float Margin = 100.0F;
        /// <summary>
        /// The height of label.
        /// </summary>
        public float HeightLabel = 16.0F;
        /// <summary>
        /// The height of property
        /// </summary>
        public float HeightProperty = 20.0F;
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
        public bool ForceOrthoCards = false;
        /// <summary>
        /// The type list.
        /// </summary>
        public List<Type> TypeList;
        /// <summary>
        /// The classes analyzed.
        /// </summary>
        public Dictionary<string, bool> AnalyzeTheseClasses = new Dictionary<string, bool>();
        /// <summary>
        /// The classes show.
        /// </summary>
        public Dictionary<string, bool> ShowTheseClasses = new Dictionary<string, bool>();
        /// <summary>
        /// The regroup properties in document.
        /// </summary>
        public bool ReGroupProperties = false;
        public bool UsedOnlyProperties = false;

        public string Language = NWDDataLocalizationManager.kBaseDev;
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
            Dictionary<string, bool> tClassesCopy = new Dictionary<string, bool>(ShowTheseClasses);
            foreach (KeyValuePair<string, bool> tKeyValue in tClassesCopy)
            {
                EditorPrefs.SetBool("NWDEditorShow_" + tKeyValue.Key, ShowTheseClasses[tKeyValue.Key]);
                EditorPrefs.SetBool("NWDEditorAnalyze_" + tKeyValue.Key, AnalyzeTheseClasses[tKeyValue.Key]);
            }
            EditorPrefs.SetBool("NWDEditorGroup", ReGroupProperties);
            EditorPrefs.SetBool("NWDEditorusedOnly", UsedOnlyProperties);
            EditorPrefs.SetString("NWDNodeEditorLanguage", Language);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Loads the preferences.
        /// </summary>
        public void LoadPreferences()
        {
            Dictionary<string, bool> tClassesCopy = new Dictionary<string, bool>(ShowTheseClasses);
            foreach (KeyValuePair<string, bool> tKeyValue in tClassesCopy)
            {
                ShowTheseClasses[tKeyValue.Key] = EditorPrefs.GetBool("NWDEditorShow_" + tKeyValue.Key, true);
                AnalyzeTheseClasses[tKeyValue.Key] = EditorPrefs.GetBool("NWDEditorAnalyze_" + tKeyValue.Key, true);
            }

            ReGroupProperties = EditorPrefs.GetBool("NWDEditorGroup");
            UsedOnlyProperties = EditorPrefs.GetBool("NWDEditorusedOnly");
            Language = EditorPrefs.GetString("NWDNodeEditorLanguage");
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draws the preferences.
        /// </summary>
        public void DrawPreferences()
        {
            bool tChanged = false;
            float tY = NWDConstants.kFieldMarge;
            float tWHalf = (MargeWidth - NWDConstants.kFieldMarge * 3) / 2.0f;
            float tW = (MargeWidth - NWDConstants.kFieldMarge * 2);

            // root object zone
            GUI.Label(new Rect(NWDConstants.kFieldMarge, tY, MargeWidth, HeightProperty),
                      "Root object", EditorStyles.boldLabel);
            tY += HeightProperty + NWDConstants.kFieldMarge;

            if (GUI.Button(new Rect(NWDConstants.kFieldMarge, tY, tW, NWDConstants.kEditWidth), NWDConstants.K_EDITOR_NODE_SHOW_SELECTED_OBJECT, EditorStyles.miniButton))
            {
                NWDNodeEditor.SetObjectInNodeWindow((NWDTypeClass)NWDDataInspector.ShareInstance().mObjectInEdition);
                ReAnalyze();
            }
            tY += NWDConstants.kEditWidth + NWDConstants.kFieldMarge;



            // Analyze object zone

            GUI.Label(new Rect(NWDConstants.kFieldMarge, tY, MargeWidth, HeightProperty),
                      "Analyze object", EditorStyles.boldLabel);
            tY += HeightProperty + NWDConstants.kFieldMarge;



            if (GUI.Button(new Rect(NWDConstants.kFieldMarge, tY, tWHalf, NWDConstants.kEditWidth), NWDConstants.K_EDITOR_NODE_SHOW_ALL, EditorStyles.miniButton))
            {
                Dictionary<string, bool> tClassesCopy = new Dictionary<string, bool>(ShowTheseClasses);
                foreach (KeyValuePair<string, bool> tKeyValue in tClassesCopy)
                {
                    ShowTheseClasses[tKeyValue.Key] = true;
                    EditorPrefs.SetBool("NWDEditorShow_" + tKeyValue.Key, ShowTheseClasses[tKeyValue.Key]);
                }
                ReAnalyze();
            }
            if (GUI.Button(new Rect(tWHalf + NWDConstants.kFieldMarge * 2, tY, tWHalf, NWDConstants.kEditWidth), NWDConstants.K_EDITOR_NODE_MASK_ALL, EditorStyles.miniButton))
            {
                Dictionary<string, bool> tClassesCopy = new Dictionary<string, bool>(ShowTheseClasses);
                foreach (KeyValuePair<string, bool> tKeyValue in tClassesCopy)
                {
                    ShowTheseClasses[tKeyValue.Key] = false;
                    EditorPrefs.SetBool("NWDEditorShow_" + tKeyValue.Key, ShowTheseClasses[tKeyValue.Key]);
                }
                ReAnalyze();
            }

            tY += NWDConstants.kEditWidth + NWDConstants.kFieldMarge;


            if (GUI.Button(new Rect(NWDConstants.kFieldMarge, tY, tWHalf, NWDConstants.kEditWidth), NWDConstants.K_EDITOR_NODE_ANALYZE_ALL, EditorStyles.miniButton))
            {
                Dictionary<string, bool> tClassesCopy = new Dictionary<string, bool>(AnalyzeTheseClasses);
                foreach (KeyValuePair<string, bool> tKeyValue in tClassesCopy)
                {
                    AnalyzeTheseClasses[tKeyValue.Key] = true;
                    EditorPrefs.SetBool("NWDEditorAnalyze_" + tKeyValue.Key, AnalyzeTheseClasses[tKeyValue.Key]);
                }
                ReAnalyze();
            }
            if (GUI.Button(new Rect(tWHalf + NWDConstants.kFieldMarge * 2, tY, tWHalf, NWDConstants.kEditWidth), NWDConstants.K_EDITOR_NODE_ANALYZE_NONE, EditorStyles.miniButton))
            {
                Dictionary<string, bool> tClassesCopy = new Dictionary<string, bool>(AnalyzeTheseClasses);
                foreach (KeyValuePair<string, bool> tKeyValue in tClassesCopy)
                {
                    AnalyzeTheseClasses[tKeyValue.Key] = false;
                    EditorPrefs.SetBool("NWDEditorAnalyze_" + tKeyValue.Key, AnalyzeTheseClasses[tKeyValue.Key]);
                }
                ReAnalyze();
            }
            tY += NWDConstants.kEditWidth + NWDConstants.kFieldMarge;

            bool tReGroupProperties = GUI.Toggle(new Rect(NWDConstants.kFieldMarge, tY, tW, NWDConstants.kEditWidth), ReGroupProperties, NWDConstants.K_EDITOR_NODE_GROUP_PROPERTIES);
            if (tReGroupProperties != ReGroupProperties)
            {
                ReGroupProperties = tReGroupProperties;
                EditorPrefs.SetBool("NWDEditorGroup", ReGroupProperties);
                ReAnalyze();
            }
            tY += NWDConstants.kEditWidth + NWDConstants.kFieldMarge;

            bool tUsedOnlyProperties = GUI.Toggle(new Rect(NWDConstants.kFieldMarge, tY, tW, NWDConstants.kEditWidth), UsedOnlyProperties, NWDConstants.K_EDITOR_NODE_ONLY_USED_PROPERTIES);
            if (tUsedOnlyProperties != UsedOnlyProperties)
            {
                UsedOnlyProperties = tUsedOnlyProperties;
                EditorPrefs.SetBool("NWDEditorusedOnly", UsedOnlyProperties);
                ReAnalyze();
            }
            tY += NWDConstants.kEditWidth + NWDConstants.kFieldMarge;



            // Localization Zone

            GUI.Label(new Rect(NWDConstants.kFieldMarge, tY, MargeWidth, HeightProperty),
                      "Localization test", EditorStyles.boldLabel);
            tY += HeightProperty + NWDConstants.kFieldMarge;

            string tLanguage = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString;
            string[] tLanguageArray = tLanguage.Split(new string[] { ";" }, StringSplitOptions.RemoveEmptyEntries);
            List<string> tLocalizationList = new List<string>(tLanguageArray);
            int tIndexActual = tLocalizationList.IndexOf(Language);

            int tIndexNext = EditorGUI.Popup(new Rect(NWDConstants.kFieldMarge, tY, tW, NWDConstants.kEditWidth), NWDConstants.K_EDITOR_NODE_CHOOSE_LANGUAGE, tIndexActual, tLocalizationList.ToArray());
            if (tIndexNext < 0 || tIndexNext >= tLocalizationList.Count)
            {
                tIndexNext = 0;
            }
            if (tIndexActual != tIndexNext)
            {
                Language = tLocalizationList[tIndexNext];
                EditorPrefs.SetString("NWDNodeEditorLanguage", Language);
            }

            tY += NWDConstants.kEditWidth + NWDConstants.kFieldMarge;










            float tXA = NWDConstants.kFieldMarge;
            float tWidthB = 20.0F;
            float tWidthA = MargeWidth - NWDConstants.kEditWidth - tWidthB - NWDConstants.kFieldMarge * 4;
            float tXB = tWidthA + NWDConstants.kFieldMarge * 2;
            //Debug.Log("MargeWidth = " + MargeWidth.ToString());
            //Debug.Log("NWDConstants.kFieldMarge = " + NWDConstants.kFieldMarge.ToString());
            //Debug.Log("tXA = " + tXA.ToString());
            //Debug.Log("tWidthA = " + tWidthA.ToString());
            //Debug.Log("tXB = " + tXB.ToString());
            //Debug.Log("tWidthB = " + tWidthB.ToString());

            GUI.Label(new Rect(NWDConstants.kFieldMarge, MargeHeight + NWDConstants.kFieldMarge, MargeWidth, HeightProperty), NWDConstants.K_EDITOR_NODE_LIST, EditorStyles.boldLabel);
            // to show
            int tCounter = 0;
            Dictionary<string, bool> tShowTheseClassesCopy = new Dictionary<string, bool>(ShowTheseClasses);
            foreach (KeyValuePair<string, bool> tKeyValue in tShowTheseClassesCopy)
            {
                tCounter++;
                //Debug.Log("tWidthA loop = " + tWidthA.ToString());
                bool tNew = EditorGUI.Toggle(new Rect(tXA, MargeHeight + NWDConstants.kFieldMarge + HeightProperty * tCounter, tWidthA, HeightProperty), tKeyValue.Key, tKeyValue.Value);
                if (ShowTheseClasses[tKeyValue.Key] != tNew)
                {
                    tChanged = true;
                }
                ShowTheseClasses[tKeyValue.Key] = tNew;
            }

            // to analyze...
            tCounter = 0;
            Dictionary<string, bool> tAnalyzeTheseClassesCopy = new Dictionary<string, bool>(AnalyzeTheseClasses);
            foreach (KeyValuePair<string, bool> tKeyValue in tAnalyzeTheseClassesCopy)
            {
                tCounter++;
                bool tNew = EditorGUI.ToggleLeft(new Rect(tXB, MargeHeight + NWDConstants.kFieldMarge + HeightProperty * tCounter, tWidthB, HeightProperty), string.Empty, tKeyValue.Value);
                if (AnalyzeTheseClasses[tKeyValue.Key] != tNew)
                {
                    tChanged = true;
                }
                AnalyzeTheseClasses[tKeyValue.Key] = tNew;
            }
            // to add new...
            tCounter = 0;
            Type tTypeToCreate = null;
            foreach (Type tType in TypeList)
            {
                tCounter++;
                GUIContent tNewContent = new GUIContent(NWDConstants.kImageNew, "New");
                if (GUI.Button(new Rect(MargeWidth - NWDConstants.kEditWidth - NWDConstants.kFieldMarge, MargeHeight + NWDConstants.kFieldMarge + HeightProperty * tCounter, NWDConstants.kEditWidth, NWDConstants.kEditWidth), tNewContent, NWDConstants.StyleMiniButton))
                {
                    tTypeToCreate = tType;
                }
            }
            // I must creat new object (prevent collection modified)
            if (tTypeToCreate != null)
            {
            // TODO : Change to remove invoke!
                //Debug.Log("try NewObject " + tTypeToCreate.Name);
                var tDataTypeNewObject = tTypeToCreate.GetMethod("NewObject", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tDataTypeNewObject != null)
                {
                    //Debug.Log("NewObject is Ok ");
                    object tObject = tDataTypeNewObject.Invoke(null, null);
                    NWDDataInspector.InspectNetWorkedData(tObject);
                    this.SetData(tObject as NWDTypeClass, true);
                    ReAnalyze();
                }
            }


            if (tChanged == true)
            {
                SavePreferences();
                ReAnalyze();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Dimension this instance.
        /// </summary>
        /// <returns>The dimension.</returns>
        public Rect Dimension()
        {
            float tHeight = Mathf.Max((GetLineMax() + 1) * (Height + Margin) + Margin, MargeHeight + (AnalyzeTheseClasses.Count + 3) * HeightProperty);
            return new Rect(0, 0, MargeWidth + (GetColumnMax() + 1) * (Width + Margin) + Margin, tHeight);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Reevaluate layout.
        /// </summary>
        public void ReEvaluateLayout()
        {
            Height = NWDConstants.kFieldMarge + (HeightLabel + NWDConstants.kFieldMarge) * 3 + InformationsHeight + NWDConstants.kFieldMarge + (HeightProperty + NWDConstants.kFieldMarge) * PropertyMax;
            foreach (NWDNodeCard tCard in AllCards)
            {
                tCard.ReEvaluateHeightWidth();
                // Force OrthoCard
                if (ForceOrthoCards == true)
                {
                    tCard.Height = Height;
                    tCard.Width = Width;
                }
                tCard.ReEvaluateLayout();
            }
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
        /// Sets the width.
        /// </summary>
        /// <param name="sWidth">S width.</param>
        public void SetWidth(float sWidth)
        {
            Width = Mathf.Max(Width, sWidth);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <returns>The width.</returns>
        public float GetWidth()
        {
            return Width;
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
            ShowTheseClasses = new Dictionary<string, bool>();
            AnalyzeTheseClasses = new Dictionary<string, bool>();
            TypeList = NWDDataManager.SharedInstance().mTypeList;
            TypeList.Sort((tA, tB) => string.Compare(tA.Name, tB.Name, StringComparison.Ordinal));
            foreach (Type tType in TypeList)
            {
                ShowTheseClasses.Add(tType.Name, true);
                AnalyzeTheseClasses.Add(tType.Name, true);
            }
            LoadPreferences();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Sets the data.
        /// </summary>
        /// <param name="sObject">S object.</param>
        /// <param name="tReset">If set to <c>true</c> t reset.</param>
        public void SetData(NWDTypeClass sObject, bool tReset = true)
        {
            if (tReset == true)
            {
                LoadClasses();
            }

            AllCards = new List<NWDNodeCard>();
            AllCardsAnalyzed = new List<NWDNodeCard>();
            PropertyMax = 0;

            Width = 200.0F;
            Height = 100.0F;
            //Margin = 100.0F;
            //HeightLabel = 16.0F;
            //HeightProperty = 20.0F;

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
                OriginalData.Data = sObject;
                AllCards.Add(OriginalData);
                OriginalData.Analyze(this);
            }
            //            Debug.Log(AllCards.Count + " Cards found");
            ReEvaluateLayout();
            //Analyze();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Res the analyze.
        /// </summary>
        public void ReAnalyze()
        {
            //Debug.Log("ReAnalyze()");
            if (OriginalData != null)
            {
                SetData(OriginalData.Data, false);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Re the analyze.
        /// </summary>
        public void ReAnalyzeIfNecessary(object sObjectModified)
        {
            if (OriginalData != null)
            {
                bool tNeedBeReAnalyze = false;
                foreach (NWDNodeCard tCard in AllCards)
                {
                    if (sObjectModified == tCard.Data)
                    {
                        tNeedBeReAnalyze = true;
                        break;
                    }
                }
                if (tNeedBeReAnalyze == true)
                {
                    SetData(OriginalData.Data, false);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Analyze this instance.
        /// </summary>
        //public void Analyze()
        //{
        //    //BTBConsole.Clear();
        //    Debug.Log("NWDNodeDocument Analyze()");
        //    AllCards = new List<NWDNodeCard>();
        //    AllCardsAnalyzed = new List<NWDNodeCard>();
        //    if (OriginalData != null)
        //    {
        //        OriginalData.Analyze(this);
        //    }
        //    Debug.Log(AllCards.Count + " Cards found");
        //    ReEvaluateLayout();

        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draw the specified sViewRect.
        /// </summary>
        /// <returns>The draw.</returns>
        /// <param name="sViewRect">S view rect.</param>
        public void Draw(Rect sViewRect)
        {
            DrawCanvas(sViewRect);
            DrawBackgroundLine();
            DrawCard();
            DrawForwardPlot();
            DrawPreferences();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draws the canvas.
        /// </summary>
        /// <param name="sViewRect">S view rect.</param>
        public void DrawCanvas(Rect sViewRect)
        {
            int tFraction = 20;
            Rect sDocumentRect = Dimension();
            float tW = Mathf.Max(sDocumentRect.width, sViewRect.width);
            float tH = Mathf.Max(sDocumentRect.height, sViewRect.height);

            float tColumnMax = 1 + tW / (Width + Margin);
            float tLineMax = 1 + tH / (Height + Margin);

            float tWidMar = Width + Margin;
            float tWidMarDec = tWidMar / tFraction;

            float tHeiMar = Height + Margin;
            float tHeiMarDec = tHeiMar / tFraction;

            Handles.color = new Color(0.3F, 0.3F, 0.3F, 1F);
            Handles.DrawLine(new Vector2(MargeWidth, 0), new Vector2(MargeWidth, tLineMax * tHeiMar));
            Handles.color = new Color(0.9F, 0.9F, 0.9F, 1F);
            Handles.DrawLine(new Vector2(MargeWidth - 1, 0), new Vector2(MargeWidth - 1, tLineMax * tHeiMar));

            Handles.color = new Color(0.4F, 0.4F, 0.4F, 0.3F);
            for (int i = -0; i <= (tColumnMax + 2) * tFraction; i++)
            {
                Handles.DrawLine(new Vector2(MargeWidth + Margin / 2.0F + i * tWidMarDec, 0), new Vector2(MargeWidth + Margin / 2.0F + i * tWidMarDec, tLineMax * tHeiMar));
            }
            for (int i = -0; i <= (tLineMax + 2) * tFraction; i++)
            {
                Handles.DrawLine(new Vector2(MargeWidth, Margin / 2.0F + i * tHeiMarDec), new Vector2(MargeWidth + tColumnMax * tWidMar, Margin / 2.0F + i * tHeiMarDec));
            }

            Handles.color = new Color(0.5F, 0.5F, 0.5F, 0.7F);
            for (int i = 0; i <= tColumnMax + 2; i++)
            {
                Handles.DrawLine(new Vector2(MargeWidth + Margin / 2.0F + i * tWidMar, 0), new Vector2(MargeWidth + Margin / 2.0F + i * tWidMar, tLineMax * tHeiMar));
            }
            for (int i = 0; i <= tLineMax + 2; i++)
            {
                Handles.DrawLine(new Vector2(MargeWidth, Margin / 2.0F + i * tHeiMar), new Vector2(MargeWidth + tColumnMax * tWidMar, Margin / 2.0F + i * tHeiMar));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draws the background plot.
        /// </summary>
        public void DrawBackgroundLine()
        {
            foreach (NWDNodeCard tCard in AllCards)
            {
                tCard.DrawBackgroundLine();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draws the card.
        /// </summary>
        public void DrawCard()
        {
            foreach (NWDNodeCard tCard in AllCards)
            {
                tCard.DrawCard();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Draws the forward plot.
        /// </summary>
        public void DrawForwardPlot()
        {
            foreach (NWDNodeCard tCard in AllCards)
            {
                tCard.DrawForwardPlot();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
