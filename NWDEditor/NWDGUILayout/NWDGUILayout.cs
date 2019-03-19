//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDGUILayout
    {
        //-------------------------------------------------------------------------------------------------------------
        public static void Line()
        {
            GUILayout.Label(string.Empty, NWDGUI.kLineStyle);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Separator()
        {
            GUILayout.Label(string.Empty, NWDGUI.kSeparatorStyle);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Title(string sTitle)
        {
            EditorGUI.indentLevel=0;
            Line();
            GUILayout.Label(sTitle, NWDGUI.kTitleStyle);
            Line();
            EditorGUI.indentLevel=1;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Title(GUIContent sTitle)
        {
            EditorGUI.indentLevel = 0;
            Line();
            GUILayout.Label(sTitle, NWDGUI.kTitleStyle);
            Line();
            EditorGUI.indentLevel = 1;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Section(string sTitle)
        {
            EditorGUI.indentLevel = 0;
            //GUILayout.Space(NWDConstants.kFieldMarge);
            Line();
            GUILayout.Label(sTitle, NWDGUI.kSectionStyle);
            EditorGUI.indentLevel = 1;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Section(GUIContent sTitle)
        {
            EditorGUI.indentLevel = 0;
            //GUILayout.Space(NWDConstants.kFieldMarge);
            Line();
            GUILayout.Label(sTitle, NWDGUI.kSectionStyle);
            EditorGUI.indentLevel = 1;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SubTitle(string sTitle)
        {
            EditorGUI.indentLevel = 0;
            GUILayout.Space(NWDConstants.kFieldMarge);
            Line();
            GUILayout.Label(sTitle, NWDGUI.kSubSectionStyle);
            EditorGUI.indentLevel = 1;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SubTitle(GUIContent sTitle)
        {
            EditorGUI.indentLevel = 0;
            GUILayout.Space(NWDConstants.kFieldMarge);
            Line();
            GUILayout.Label(sTitle, NWDGUI.kSubSectionStyle);
            EditorGUI.indentLevel = 1;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Informations(string sTitle)
        {
            EditorGUILayout.HelpBox(sTitle, MessageType.None);
            //GUILayout.Label(sTitle, EditorStyles.helpBox);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Informations(GUIContent sTitle)
        {
            EditorGUILayout.HelpBox(sTitle.text, MessageType.None);
            //GUILayout.Label(sTitle, EditorStyles.helpBox);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void HelpBox(string sTitle)
        {
            EditorGUILayout.HelpBox(sTitle, MessageType.Info);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void WarningBox(string sTitle)
        {
            EditorGUILayout.HelpBox(sTitle, MessageType.Warning);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ErrorBox(string sTitle)
        {
            EditorGUILayout.HelpBox(sTitle, MessageType.Error);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void LittleSpace()
        {
            GUILayout.Space(NWDConstants.kFieldMarge);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void BigSpace()
        {
            GUILayout.Space(NWDConstants.kFieldMarge*2);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
