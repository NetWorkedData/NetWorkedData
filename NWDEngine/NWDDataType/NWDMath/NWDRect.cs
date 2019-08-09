//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:28:4
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
            Value = NWDToolbox.RectZero();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public NWDRect(string sValue = BTBConstants.K_EMPTY_STRING)
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
            float tHeight = tStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0f);
            return tHeight*3;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = BTBConstants.K_EMPTY_STRING, object sAdditionnal = null)
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