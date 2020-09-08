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
    //TODO: FINISH THIS CLASS NWDRectInt
    [SerializeField]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDRectInt : NWEDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDRectInt()
        {
            Value = NWDToolbox.RectIntZero();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public NWDRectInt(string sValue = NWEConstants.K_EMPTY_STRING)
        //{
        //    if (sValue == null)
        //    {
        //        Value = NWDToolbox.RectIntZero();
        //    }
        //    else
        //    {
        //        Value = sValue;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        public NWDRectInt(RectInt sRectInt)
        {
            Value = NWDToolbox.RectIntToString(sRectInt);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = NWDToolbox.RectIntZero();
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
        public void SetRectInt(RectInt sRectInt)
        {
            //Value = sRectInt.x + NWDConstants.kFieldSeparatorA +
            //sRectInt.y + NWDConstants.kFieldSeparatorA +
            //sRectInt.height + NWDConstants.kFieldSeparatorA +
            //sRectInt.width;
            Value = NWDToolbox.RectIntToString(sRectInt);
        }
        //-------------------------------------------------------------------------------------------------------------
        public RectInt GetRectInt()
        {
            //string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            //int tX = 0;
            //int tY = 0;
            //int tHeight = 0;
            //int tWidth = 0;
            //if (tFloats.Count() == 4)
            //{
            //    int.TryParse(tFloats[0], out tX);
            //    int.TryParse(tFloats[1], out tY);
            //    int.TryParse(tFloats[2], out tHeight);
            //    int.TryParse(tFloats[3], out tWidth);
            //}
            //RectInt rReturn = new RectInt(tX, tY, tHeight, tWidth);
            //return rReturn;
            return NWDToolbox.RectIntFromString(Value);
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
        public override object ControlField(Rect sPos, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDRectInt tTemporary = new NWDRectInt();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            RectInt tRectInt = GetRectInt();
            RectInt tNexrect = EditorGUI.RectIntField(new Rect(sPos.x, sPos.y, sPos.width, NWDGUI.kLabelStyle.fixedHeight),
                                                      tContent,tRectInt);

            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.indentLevel = tIndentLevel;

            tTemporary.SetRectInt(tNexrect);
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
