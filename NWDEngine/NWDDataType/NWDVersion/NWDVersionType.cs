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
    public class NWDVersionType : BTBDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDVersionType()
        {
            Value = "0.00.00";
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDVersionType(string sValue = "0.00.00")
        {
            if (string.IsNullOrEmpty(sValue))
            {
                Value = "0.00.00";
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
            NWDVersionType tTemporary = new NWDVersionType();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            int tMajorIndex = 0;
            int tMinorIndex = 0;
            int tBuildIndex = 0;
            float tX = sPosition.x + EditorGUIUtility.labelWidth;
            float tTiersWidth = Mathf.Ceil((sPosition.width - EditorGUIUtility.labelWidth + NWDGUI.kFieldMarge) / 3.0F);
            float tTiersWidthB = tTiersWidth - NWDGUI.kFieldMarge;
            float tHeightAdd = 0;
            if (Value != null)
            {
                string[] tValues = Value.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (tValues.Length > 0)
                {
                    tMajorIndex = Array.IndexOf(NWDConstants.K_VERSION_MAJOR_ARRAY, tValues[0]);
                }
                if (tValues.Length > 1)
                {
                    tMinorIndex = Array.IndexOf(NWDConstants.K_VERSION_MINOR_ARRAY, tValues[1]);
                }
                if (tValues.Length > 2)
                {
                    tBuildIndex = Array.IndexOf(NWDConstants.K_VERSION_BUILD_ARRAY, tValues[2]);
                }
            }
            EditorGUI.LabelField(new Rect(sPosition.x, sPosition.y, sPosition.width, NWDGUI.kLabelStyle.fixedHeight), tContent);
            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            tMajorIndex = EditorGUI.Popup(new Rect(tX, sPosition.y + tHeightAdd, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight),
                                          tMajorIndex, NWDConstants.K_VERSION_MAJOR_ARRAY);
            tMinorIndex = EditorGUI.Popup(new Rect(tX + tTiersWidth, sPosition.y + tHeightAdd, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight),
                                          tMinorIndex, NWDConstants.K_VERSION_MINOR_ARRAY);
            tBuildIndex = EditorGUI.Popup(new Rect(tX + tTiersWidth * 2, sPosition.y + tHeightAdd, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight),
                                          tBuildIndex, NWDConstants.K_VERSION_BUILD_ARRAY);
            EditorGUI.indentLevel = tIndentLevel;
            tTemporary.Value = NWDConstants.K_VERSION_MAJOR_ARRAY[tMajorIndex] + "." + NWDConstants.K_VERSION_MINOR_ARRAY[tMinorIndex] + "." + NWDConstants.K_VERSION_BUILD_ARRAY[tBuildIndex];
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================