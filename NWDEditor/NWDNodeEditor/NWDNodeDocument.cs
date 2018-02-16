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

        private Dictionary<int,int> LineListMax = new Dictionary<int, int>();

        private int ColumnMax = 0;
        private int LineMax = 0;

        private float InformationsHeight;

        public bool ForceOrthoCards = false;

        //-------------------------------------------------------------------------------------------------------------
        public Rect Dimension()
        {
            return new Rect(0,0, (GetColumnMax()+1)*(Width+Margin)+Margin, (GetLineMax() + 1) * (Height + Margin) + Margin);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReEvaluateLayout()
        {
            Height = NWDConstants.kFieldMarge + (HeightLabel + NWDConstants.kFieldMarge) * 3 + InformationsHeight + NWDConstants.kFieldMarge + (HeightProperty + NWDConstants.kFieldMarge)  * PropertyMax;
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
        public void SetData(NWDTypeClass sObject)
        {
            PropertyMax = 0;
       ColumnMax = 0;
       LineMax = 0;
            LineListMax = new Dictionary<int, int>();

            OriginalData = new NWDNodeCard();
            OriginalData.Line = 0;
            OriginalData.Column = 0;
            OriginalData.Position = new Vector2(0, 0);
            OriginalData.Data = sObject;
            Analyze();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Analyze() {
            BTBConsole.Clear();
            Debug.Log("NWDNodeDocument Analyze()");
            AllCards = new List<NWDNodeCard>();
            if (OriginalData != null)
            {
                OriginalData.Analyze(this);
            }
            Debug.Log(AllCards.Count+" Cards found");
            ReEvaluateLayout();

        }
        //-------------------------------------------------------------------------------------------------------------
        public void Draw()
        {
            DrawCard();
            DrawBackgroundPlot();
            DrawForwardPlot();
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
