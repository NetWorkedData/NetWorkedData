//=====================================================================================================================
//
//  ideMobi 2020©
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



//using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //TODO: FINISH THIS CLASS NWDVector4
    [SerializeField]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDVector4 : NWEDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDVector4()
        {
            Value = NWDToolbox.Vector4Zero();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public NWDVector4(string sValue = NWEConstants.K_EMPTY_STRING)
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
        public NWDVector4(Vector4 sVector)
        {
            Value = NWDToolbox.Vector4ToString(sVector);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = NWDToolbox.Vector4Zero();
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
        public void SetVector(Vector4 sVector)
        {
            //Value = sVector.x.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
            //sVector.y.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
            //sVector.z.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
            //sVector.w.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry);
            Value = NWDToolbox.Vector4ToString(sVector);
        }
        //-------------------------------------------------------------------------------------------------------------
        public Vector4 GetVector()
        {
            //string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            //float tX = 0.0F;
            //float tY = 0.0F;
            //float tZ = 0.0F;
            //float tW = 0.0F;
            //if (tFloats.Count() == 4)
            //{
            //    float.TryParse(tFloats[0], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tX);
            //    float.TryParse(tFloats[1], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tY);
            //    float.TryParse(tFloats[2], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tZ);
            //    float.TryParse(tFloats[3], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tW);
            //}
            //Vector4 rReturn = new Vector4(tX, tY, tZ, tW);
            //return rReturn;
            return NWDToolbox.Vector4FromString(Value);
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            GUIStyle tStyle = new GUIStyle(EditorStyles.textField);
            float tHeight = tStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100.0f);
            return tHeight*2;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDVector4 tTemporary = new NWDVector4();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            Vector4 tVector = GetVector();
            Vector4 tNexVector = EditorGUI.Vector4Field(new Rect(sPosition.x, sPosition.y, sPosition.width, NWDGUI.kLabelStyle.fixedHeight),
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