//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:16
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using UnityEngine;
using System.IO;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDEditorFooter : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public static void NewFooter()
        {
            NWDEditorFooter tFooter = EditorWindow.CreateWindow<NWDEditorFooter>();
            tFooter.ShowUtility();
            tFooter.Focus();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnEnable()
        {
            minSize = new Vector2(20, 32);
            maxSize = new Vector2(float.MaxValue, 16);
            TitleInit(NWDAppConfiguration.SharedInstance().SelectedEnvironment().AppName, typeof(NWDEditorFooter));
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            NWDGUI.LoadStyles();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(
                "   Project : <b>" + NWDAppConfiguration.SharedInstance().DevEnvironment.AppName +
                "</b>  Environment : <b> " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().Environment +
                "</b>  Webservice version : <b>" + NWDAppConfiguration.SharedInstance().WebBuild.ToString() +
                "</b>  Version Bundle : <b>" + PlayerSettings.bundleVersion +
                "</b>  SQLite : <b>" + NWDDataManager.SharedInstance().GetVersion() +
                "</b>",
                NWDGUI.kFooterLabelStyle);
            EditorGUILayout.EndHorizontal();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
