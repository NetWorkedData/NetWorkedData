//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:28:9
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
    //TODO: FINISH THIS CLASS NWDVector2
    [SerializeField]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDVector2 : BTBDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDVector2()
        {
            Value = NWDToolbox.Vector2Zero();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public NWDVector2(string sValue = BTBConstants.K_EMPTY_STRING)
        //{
        //    if (sValue == null)
        //    {
        //        Value = string.Empty;
        //    }
        //    else
        //    {
        //        Value = sValue;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        public NWDVector2(Vector2 sVector)
        {
            Value = NWDToolbox.Vector2ToString(sVector);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = NWDToolbox.Vector2Zero();
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
        public void SetVector(Vector2 sVector)
        {
            //Value = sVector.x.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
                    //sVector.y.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry);
            Value = NWDToolbox.Vector2ToString(sVector);
        }
        //-------------------------------------------------------------------------------------------------------------
        public Vector2 GetVector()
        {
            //string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            //float tX = 0.0F;
            //float tY = 0.0F;
            //if (tFloats.Count() == 2)
            //{
            //    float.TryParse(tFloats[0], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tX);
            //    float.TryParse(tFloats[1], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tY);
            //}
            //Vector2 rReturn = new Vector2(tX, tY);
            //return rReturn;
            return NWDToolbox.Vector2FromString(Value);
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            GUIStyle tStyle = new GUIStyle(EditorStyles.textField);
            float tHeight = tStyle.CalcHeight(new GUIContent(BTBConstants.K_A), 100.0f);
            return tHeight*2;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPos, string sEntitled, bool sDisabled, string sTooltips = BTBConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDVector2 tTemporary = new NWDVector2();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            Vector2 tVector = GetVector();
            Vector2 tNexVector = EditorGUI.Vector2Field(new Rect(sPos.x, sPos.y, sPos.width, NWDGUI.kLabelStyle.fixedHeight),
                                   tContent,tVector);

            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.indentLevel = tIndentLevel;

            tTemporary.SetVector(tNexVector);
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================