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
        private GUIContent IconAndTitle;
        //-------------------------------------------------------------------------------------------------------------
        private void OnEnable()
        {
            minSize = new Vector2(10, 16);
            maxSize = new Vector2(float.MaxValue, 16);

            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDAppConfiguration.SharedInstance().SelectedEnvironment().AppName;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets(typeof(NWDEditorFooter).Name + " t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals(typeof(NWDEditorFooter).Name))
                        {
                            IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        }
                    }
                }
                titleContent = IconAndTitle;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnGUI()
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
