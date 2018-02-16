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
        private NWDNodeCard OriginalData;
        public List<NWDNodeCard> AllCards = new List<NWDNodeCard>();
        private int PropertyMax = 0;

        private float Width = 200.0F;
        public float Height = 100.0F;
        public float Margin = 100.0F;
        public float HeightLabel = 16.0F;
        public float HeightProperty = 18.0F;

        private Dictionary<int, int> LineListMax = new Dictionary<int, int>();

        private int ColumnMax = 0;
        private int LineMax = 0;

        private float InformationsHeight = 50.0F;

        public bool ForceOrthoCards = false;

        public Dictionary<string, bool> AnalyzeTheseClasses = new Dictionary<string, bool>();

        public Dictionary<string, bool> ShowTheseClasses = new Dictionary<string, bool>();

        //-------------------------------------------------------------------------------------------------------------
        public NWDNodeDocument()
        {
            // TODO AnalyzeTheseTypes compelet by default from Class analyze
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SavePreferences()
        {
            // TODO 
        }
        //-------------------------------------------------------------------------------------------------------------
        public void LoadPreferences()
        {
            // TODO 
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawPreferences()
        {
            // TODO on gui
            bool tChanged = false;

            // to show
            int tCounter = 0;
            Dictionary<string, bool> tShowTheseClassesCopy = new Dictionary<string, bool>(ShowTheseClasses);
            foreach (KeyValuePair<string,bool> tKeyValue in tShowTheseClassesCopy)
            {
                // TODO on gui
                tCounter++;
                bool tNew = EditorGUI.ToggleLeft(new Rect(0, HeightProperty * tCounter, 200, HeightProperty), tKeyValue.Key, tKeyValue.Value);
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
                // TODO on gui
                tCounter++;
                bool tNew= EditorGUI.ToggleLeft(new Rect(250, HeightProperty * tCounter, 200, HeightProperty), tKeyValue.Key, tKeyValue.Value);
                if (AnalyzeTheseClasses[tKeyValue.Key] != tNew)
                {
                    tChanged = true;
                }
                AnalyzeTheseClasses[tKeyValue.Key] = tNew;
            }
            if (tChanged == true)
            {
                SavePreferences();
                ReAnalyze();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public Rect Dimension()
        {
            return new Rect(0, 0, (GetColumnMax() + 1) * (Width + Margin) + Margin, (GetLineMax() + 1) * (Height + Margin) + Margin);
        }
        //-------------------------------------------------------------------------------------------------------------
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
        public void SetInformationsHeight(float sInformationsHeight)
        {
            InformationsHeight = Mathf.Max(InformationsHeight, sInformationsHeight);
        }
        //-------------------------------------------------------------------------------------------------------------
        public float GetInformationsHeight()
        {
            return InformationsHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetWidth(float sWidth)
        {
            Width = Mathf.Max(Width, sWidth);
        }
        //-------------------------------------------------------------------------------------------------------------
        public float GetWidth()
        {
            return Width;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ColumnMaxCount(int sCount)
        {
            ColumnMax = Math.Max(ColumnMax, sCount);
        }
        //-------------------------------------------------------------------------------------------------------------
        public int GetColumnMax()
        {
            return ColumnMax;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void LineMaxCount(int sCount)
        {
            LineMax = Math.Max(LineMax, sCount);
        }
        //-------------------------------------------------------------------------------------------------------------
        public int GetLineMax()
        {
            return LineMax;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PropertyCount(int sCount)
        {
            PropertyMax = Math.Max(PropertyMax, sCount);
        }
        //-------------------------------------------------------------------------------------------------------------
        public int GetPropertyMax()
        {
            return PropertyMax;
        }
        //-------------------------------------------------------------------------------------------------------------
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
        public void SetData(NWDTypeClass sObject, bool tReset = true)
        {
            if (tReset == true)
            {
                ShowTheseClasses = new Dictionary<string, bool>();
                AnalyzeTheseClasses = new Dictionary<string, bool>();
                foreach (Type tType in NWDDataManager.SharedInstance.mTypeList.ToArray())
                {
                    ShowTheseClasses.Add(tType.Name, true);
                    AnalyzeTheseClasses.Add(tType.Name, true);
                }
                // TODO load preference 
                LoadPreferences();
            }

            AllCards = new List<NWDNodeCard>();
            PropertyMax = 0;

            Width = 200.0F;
            Height = 100.0F;
            Margin = 100.0F;
            HeightLabel = 16.0F;
            HeightProperty = 18.0F;

            LineListMax = new Dictionary<int, int>();

            ColumnMax = 0;
            LineMax = 0;

            InformationsHeight = 50.0F;

            OriginalData = new NWDNodeCard();
            OriginalData.Line = 0;
            OriginalData.Column = 0;
            OriginalData.Position = new Vector2(0, 0);
            OriginalData.Data = sObject;
            Analyze();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReAnalyze()
        {
            SetData(OriginalData.Data, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Analyze()
        {
            BTBConsole.Clear();
            Debug.Log("NWDNodeDocument Analyze()");
            AllCards = new List<NWDNodeCard>();
            if (OriginalData != null)
            {
                OriginalData.Analyze(this);
            }
            Debug.Log(AllCards.Count + " Cards found");
            ReEvaluateLayout();

        }
        //-------------------------------------------------------------------------------------------------------------
        public void Draw(Rect sViewRect)
        {
            DrawCanvas(sViewRect);
            DrawCard();
            DrawBackgroundPlot();
            DrawForwardPlot();
            DrawPreferences();
        }
        //-------------------------------------------------------------------------------------------------------------
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

            Handles.color = new Color(0.4F,0.4F,0.4F,0.3F);
            for (int i = -tFraction; i <= (tColumnMax +2 )*tFraction; i++)
            {
                Handles.DrawLine(new Vector2(Margin / 2.0F + i * tWidMarDec, 0), new Vector2(Margin / 2.0F + i * tWidMarDec, tLineMax * tHeiMar));
            }
            for (int i = -tFraction; i <= (tLineMax+ 2)*tFraction; i++)
            {
                Handles.DrawLine(new Vector2(0, Margin / 2.0F + i * tHeiMarDec), new Vector2(tColumnMax * tWidMar, Margin / 2.0F + i * tHeiMarDec));
            }

            Handles.color = new Color(0.5F, 0.5F, 0.5F, 0.7F);
            for (int i = 0; i <= tColumnMax + 2; i++)
            {
                Handles.DrawLine(new Vector2(Margin / 2.0F + i * tWidMar, 0), new Vector2(Margin / 2.0F + i * tWidMar, tLineMax * tHeiMar));
            }
            for (int i = 0; i <= tLineMax + 2; i++)
            {
                Handles.DrawLine(new Vector2(0, Margin / 2.0F + i * tHeiMar), new Vector2(tColumnMax * tWidMar, Margin / 2.0F + i * tHeiMar));
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawBackgroundPlot()
        {
            foreach (NWDNodeCard tCard in AllCards)
            {
                tCard.DrawBackgroundPlot();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawCard()
        {
            foreach (NWDNodeCard tCard in AllCards)
            {
                tCard.DrawCard();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
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
