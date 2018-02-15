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
        private List<NWDNodeCard> AllCards = new List<NWDNodeCard>();
        //-------------------------------------------------------------------------------------------------------------
        public void SetData(NWDTypeClass sObject)
        {
            OriginalData = new NWDNodeCard();
            OriginalData.Line = 0;
            OriginalData.Column = 0;
            OriginalData.Position = new Vector2(0, 0);
            OriginalData.Data = sObject;
            Analyze();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Analyze() {
            AllCards = new List<NWDNodeCard>();
            AllCards.Add(OriginalData);
            // TODO analyze object and add the connexion!
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Draw()
        {
            DrawCard();
            DrawLine();
            DrawPlot();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void DrawLine()
        {
            foreach (NWDNodeCard tCard in AllCards)
            {
                tCard.DrawLine();
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
        public void DrawPlot()
        {
            foreach (NWDNodeCard tCard in AllCards)
            {
                tCard.DrawPlot();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
