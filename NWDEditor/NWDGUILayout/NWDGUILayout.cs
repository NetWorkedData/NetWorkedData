//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:53
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
            GUILayout.Space(NWDGUI.kFieldMarge);
            GUILayout.Label(string.Empty, NWDGUI.kSeparatorStyle);
            GUILayout.Space(NWDGUI.kFieldMarge);
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
            //GUILayout.Space(NWDGUI.kFieldMarge);
            Line();
            GUILayout.Label(sTitle, NWDGUI.kSectionStyle);
            EditorGUI.indentLevel = 1;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Section(GUIContent sTitle)
        {
            EditorGUI.indentLevel = 0;
            //GUILayout.Space(NWDGUI.kFieldMarge);
            Line();
            GUILayout.Label(sTitle, NWDGUI.kSectionStyle);
            EditorGUI.indentLevel = 1;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SubSection(string sTitle)
        {
            EditorGUI.indentLevel = 0;
            GUILayout.Space(NWDGUI.kFieldMarge);
            Line();
            GUILayout.Label(sTitle, NWDGUI.kSubSectionStyle);
            EditorGUI.indentLevel = 1;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SubSection(GUIContent sTitle)
        {
            EditorGUI.indentLevel = 0;
            GUILayout.Space(NWDGUI.kFieldMarge);
            Line();
            GUILayout.Label(sTitle, NWDGUI.kSubSectionStyle);
            EditorGUI.indentLevel = 1;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Informations(string sTitle, bool sIndent = true)
        {
            //LittleSpace();
            if (sIndent == true)
            {
                EditorGUILayout.HelpBox(sTitle, MessageType.None);
            }
            else
            {
                GUILayout.Label(sTitle, EditorStyles.helpBox);
            }
            //LittleSpace();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Informations(GUIContent sTitle, bool sIndent = true)
        {
            //LittleSpace();
            if (sIndent == true)
            {
                EditorGUILayout.HelpBox(sTitle.text, MessageType.None);
            }
            else
            {
                GUILayout.Label(sTitle, EditorStyles.helpBox);
            }
            //LittleSpace();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void HelpBox(string sTitle)
        {
            //LittleSpace();
            EditorGUILayout.HelpBox(sTitle, MessageType.Info);
            //LittleSpace();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void WarningBox(string sTitle)
        {
            //LittleSpace();
            EditorGUILayout.HelpBox(sTitle, MessageType.Warning);
            //LittleSpace();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ErrorBox(string sTitle)
        {
            //LittleSpace();
            EditorGUILayout.HelpBox(sTitle, MessageType.Error);
            //LittleSpace();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void LittleSpace()
        {
            GUILayout.Space(NWDGUI.kFieldMarge);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void BigSpace()
        {
            GUILayout.Space(NWDGUI.kFieldMarge*2);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
