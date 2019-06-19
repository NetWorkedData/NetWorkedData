//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:0
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

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
    public class NWDIPType : BTBDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDIPType()
        {
            Value = "0.0.0.0";
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDIPType(string sValue = "0.0.0.0")
        {
            if (string.IsNullOrEmpty(sValue))
            {
                Value = "0.0.0.0";
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
        public override void BaseVerif()
        {
            // Need to check with a new dictionary each time
            if (string.IsNullOrEmpty(Value))
            {
                Default();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public int ToInt()
        {
            int rVersionInteger = 0;
            int.TryParse(ToString().Replace(".", string.Empty), out rVersionInteger);
            return rVersionInteger;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            return NWDGUI.kPopupStyle.fixedHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDIPType tTemporary = new NWDIPType();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            string tPlageA = "0";
            string tPlageB = "0";
            string tPlageC = "0";
            string tPlageD = "0";
            float tX = sPosition.x + EditorGUIUtility.labelWidth;
            float tTiersWidth = Mathf.Ceil((sPosition.width - EditorGUIUtility.labelWidth + NWDGUI.kFieldMarge) / 4.0F);
            float tTiersWidthB = tTiersWidth - NWDGUI.kFieldMarge;
            float tHeightAdd = 0;
            if (Value != null)
            {
                string[] tValues = Value.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (tValues.Length > 0)
                {
                    tPlageA = tValues[0];
                }
                if (tValues.Length > 1)
                {
                    tPlageB = tValues[1];
                }
                if (tValues.Length > 2)
                {
                    tPlageC = tValues[2];
                }
                if (tValues.Length > 3)
                {
                    tPlageD = tValues[3];
                }
            }
            EditorGUI.LabelField(new Rect(sPosition.x, sPosition.y, sPosition.width, NWDGUI.kLabelStyle.fixedHeight), tContent);
            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            tPlageA = EditorGUI.TextField(new Rect(tX, sPosition.y + tHeightAdd, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight),
                                          tPlageA);
            tPlageB = EditorGUI.TextField(new Rect(tX + tTiersWidth * 1, sPosition.y + tHeightAdd, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight),
                                          tPlageB);
            tPlageC = EditorGUI.TextField(new Rect(tX + tTiersWidth * 2, sPosition.y + tHeightAdd, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight),
                                          tPlageC);
            tPlageD = EditorGUI.TextField(new Rect(tX + tTiersWidth * 3, sPosition.y + tHeightAdd, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight),
                                          tPlageD);
            if (string.IsNullOrEmpty(tPlageA))
            {
                tPlageA = "0";
            }
            else if (tPlageA == "?")
            {
            }
            else
            {
                int tA_int;
                int.TryParse(tPlageA, out tA_int);
                if (tA_int < 0)
                {
                    tA_int = 0;
                }
                if (tA_int > 255)
                {
                    tA_int = 255;
                }
                tPlageA = tA_int.ToString();
            }
            if (string.IsNullOrEmpty(tPlageB))
            {
                tPlageB = "0";
            }
            else if (tPlageB == "?")
            {
            }
            else
            {
                int tA_int;
                int.TryParse(tPlageB, out tA_int);
                if (tA_int < 0)
                {
                    tA_int = 0;
                }
                if (tA_int > 255)
                {
                    tA_int = 255;
                }
                tPlageB = tA_int.ToString();
            }
            if (string.IsNullOrEmpty(tPlageC))
            {
                tPlageC = "0";
            }
            else if (tPlageC == "?")
            {
            }
            else
            {
                int tA_int;
                int.TryParse(tPlageC, out tA_int);
                if (tA_int < 0)
                {
                    tA_int = 0;
                }
                if (tA_int > 255)
                {
                    tA_int = 255;
                }
                tPlageC = tA_int.ToString();
            }
            if (string.IsNullOrEmpty(tPlageD))
            {
                tPlageD = "0";
            }
            else if (tPlageD == "?")
            {
            }
            else
            {
                int tA_int;
                int.TryParse(tPlageD, out tA_int);
                if (tA_int < 0)
                {
                    tA_int = 0;
                }
                if (tA_int > 255)
                {
                    tA_int = 255;
                }
                tPlageD = tA_int.ToString();
            }
            EditorGUI.indentLevel = tIndentLevel;
            tTemporary.Value = tPlageA + "." + tPlageB + "." + tPlageC + "." + tPlageD;
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================