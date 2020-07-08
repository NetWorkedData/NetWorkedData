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
#endif
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
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
    public class NWDPasswordType : NWEDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDPasswordType()
        {
            //Value = NWDToolbox.RandomStringCypher(24);
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDPasswordType(string sValue = "")
        {
            if (string.IsNullOrEmpty(sValue))
            {
                //Value = "CHanGeYourPassWorDNow";
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
        public override void BaseVerif()
        {
            // Need to check with a new dictionary each time
            if (string.IsNullOrEmpty(Value))
            {
                Default();
            }
        }
        ////-------------------------------------------------------------------------------------------------------------
        //public int ToInt()
        //{
        //    int rVersionInteger = 0;
        //    int.TryParse(ToString().Replace(".", string.Empty), out rVersionInteger);
        //    return rVersionInteger;
        //}
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            return NWDGUI.kPopupStyle.fixedHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDPasswordType tTemporary = new NWDPasswordType();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            float tX = sPosition.x + EditorGUIUtility.labelWidth;
            float tTiersWidth = Mathf.Ceil((sPosition.width - EditorGUIUtility.labelWidth + NWDGUI.kFieldMarge) / 3.0F);
            float tTiersWidthB = tTiersWidth - NWDGUI.kFieldMarge;
            EditorGUI.LabelField(new Rect(sPosition.x, sPosition.y, sPosition.width, NWDGUI.kLabelStyle.fixedHeight), tContent);
            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            tTemporary.Value = EditorGUI.TextField(new Rect(tX, sPosition.y, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight), Value);
            if (GUI.Button(new Rect(tX + tTiersWidth * 1, sPosition.y, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight),"test"))
            {
                NWEPassAnalyseWindow.SharedInstance().AnalyzePassword(Value);
            }
            if (GUI.Button(new Rect(tX + tTiersWidth * 2, sPosition.y, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight),"rand"))
            {
                tTemporary.Value = NWDToolbox.RandomStringCypher(24);
            }
            EditorGUI.indentLevel = tIndentLevel;
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================