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
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using UnityEngine;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //TODO: FINISH THIS CLASS NWDRect
    [SerializeField]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDRect : NWEDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDRect()
        {
            Value = NWDToolbox.RectZero();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public NWDRect(string sValue = NWEConstants.K_EMPTY_STRING)
        //{
        //    if (sValue == null)
        //    {
        //        Value = NWDToolbox.RectZero();
        //    }
        //    else
        //    {
        //        Value = sValue;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        public NWDRect(Rect sRect)
        {
            Value = NWDToolbox.RectToString(sRect);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = NWDToolbox.RectZero();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void BaseVerif()
        {
            // Need to check with a new dictionary each time
            if (string.IsNullOrEmpty(Value))
            {
                Default();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetRect(Rect sRect)
        {
            Value = NWDToolbox.RectToString(sRect);
        }
        //-------------------------------------------------------------------------------------------------------------
        public Rect GetRect()
        {
            //string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            //float tX = 0.0F;
            //float tY = 0.0F;
            //float tHeight = 0.0F;
            //float tWidth = 0.0F;
            //if (tFloats.Count() == 4)
            //{
            //    float.TryParse(tFloats[0], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tX);
            //    float.TryParse(tFloats[1], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tY);
            //    float.TryParse(tFloats[2], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tHeight);
            //    float.TryParse(tFloats[3], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tWidth);
            //}
            //Rect rReturn = new Rect(tX, tY, tHeight, tWidth);
            return NWDToolbox.RectFromString(Value);
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            GUIStyle tStyle = new GUIStyle(EditorStyles.textField);
            float tHeight = tStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100.0f);
            return tHeight*3;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDRect tTemporary = new NWDRect();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            Rect tRect = GetRect();
            Rect tNexrect = EditorGUI.RectField(new Rect(sPosition.x, sPosition.y, sPosition.width, NWDGUI.kLabelStyle.fixedHeight),
                                                      tContent,tRect);

            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.indentLevel = tIndentLevel;

            tTemporary.SetRect(tNexrect);
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
