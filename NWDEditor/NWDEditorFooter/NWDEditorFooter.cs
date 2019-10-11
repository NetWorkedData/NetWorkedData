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
    public class NWDEditorFooter : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        private void OnEnable()
        {
            minSize = new Vector2(10, 16);
            maxSize = new Vector2(float.MaxValue, 16);
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnGUI()
        {
            titleContent = new GUIContent(NWDAppConfiguration.SharedInstance().SelectedEnvironment().AppName);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Project" , NWDAppConfiguration.SharedInstance().DevEnvironment.AppName);
            EditorGUILayout.LabelField("Environment", NWDAppConfiguration.SharedInstance().SelectedEnvironment().Environment);
            EditorGUILayout.LabelField("Webservice version", NWDAppConfiguration.SharedInstance().WebBuild.ToString());
            EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_VERSION_BUNDLE, PlayerSettings.bundleVersion, EditorStyles.label);
            EditorGUILayout.EndHorizontal();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
