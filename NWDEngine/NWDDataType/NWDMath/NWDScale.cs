//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:28:7
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



//using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //TODO: FINISH THIS CLASS NWDScale
    [SerializeField]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDScale : NWEDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDScale()
        {
            Value = NWDToolbox.Vector3One();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public NWDScale(string sValue = NWEConstants.K_EMPTY_STRING)
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
        public NWDScale(Vector3 sVector)
        {
            Value = NWDToolbox.Vector3ToString(sVector);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = NWDToolbox.Vector3One();
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
        public void SetVector(Vector3 sVector)
        {
            //Value = sVector.x.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
                    //sVector.y.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
                    //sVector.z.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry);
            Value = NWDToolbox.Vector3ToString(sVector);
        }
        //-------------------------------------------------------------------------------------------------------------
        public Vector3 GetVector()
        {
            return NWDToolbox.Vector3FromString(Value);
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
            NWDScale tTemporary = new NWDScale();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            Vector3 tVector = GetVector();
            Vector3 tNexVector = EditorGUI.Vector3Field(new Rect(sPosition.x, sPosition.y, sPosition.width, NWDGUI.kLabelStyle.fixedHeight),
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