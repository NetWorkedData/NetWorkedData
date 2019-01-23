//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //TODO: FINISH THIS CLASS NWDRect
    [SerializeField]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDRect : BTBDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDRect()
        {
            Value = 0.0F + NWDConstants.kFieldSeparatorA + 0.0F+ NWDConstants.kFieldSeparatorA + 0.0F+ NWDConstants.kFieldSeparatorA + 0.0F;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDRect(string sValue = BTBConstants.K_EMPTY_STRING)
        {
            if (sValue == null)
            {
                Value = string.Empty;
            }
            else
            {
                Value = sValue;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetRect(Rect sRect)
        {
            Value = sRect.x.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
                    sRect.y.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
                         sRect.height.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
                         sRect.width.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry);
        }
        //-------------------------------------------------------------------------------------------------------------
        public Rect GetRect()
        {
            string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            float tX = 0.0F;
            float tY = 0.0F;
            float tHeight = 0.0F;
            float tWidth = 0.0F;
            if (tFloats.Count() == 4)
            {
                float.TryParse(tFloats[0], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tX);
                float.TryParse(tFloats[1], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tY);
                float.TryParse(tFloats[2], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tHeight);
                float.TryParse(tFloats[3], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tWidth);
            }
            Rect rReturn = new Rect(tX, tY, tHeight, tWidth);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            GUIStyle tStyle = new GUIStyle(EditorStyles.textField);
            float tHeight = tStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0f);
            return tHeight*3;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPos, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING)
        {
            NWDRect tTemporary = new NWDRect();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            Rect tRect = GetRect();
            Rect tNexrect = EditorGUI.RectField(new Rect(sPos.x, sPos.y, sPos.width, NWDConstants.kLabelStyle.fixedHeight),
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