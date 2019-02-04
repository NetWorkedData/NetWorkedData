//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;
using System.Reflection;
using System.IO;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	/// <summary>
	/// NWD app environment manager window.
	/// </summary>
	public class NWDAppEnvironmentConfigurationManager : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        GUIContent IconAndTitle;
        static Vector2 ScrollPosition;
        static int TabSelected = 0;
        //-------------------------------------------------------------------------------------------------------------
        public NWDAppEnvironmentConfigurationManager ()
		{
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDConstants.K_ENVIRONMENTS_CONFIGURATION_TITLE;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets("NWDAppEnvironmentConfigurationManager t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals("NWDAppEnvironmentConfigurationManager"))
                        {
                            IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        }
                    }
                }
                titleContent = IconAndTitle;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentConfigurationManager SharedInstance()
        {
            NWDEditorMenu.AppEnvironmentManagerWindowShow();
            return NWDEditorMenu.kAppConfigurationManager;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnGUI()
        {
            NWDConstants.LoadStyles();
           
            float tMinWidht = 270.0F;
            float tScrollMarge = 20.0f;
            int tColum = 1;
            // Draw warning if salt for class is false
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.kAlertSaltShortError, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
            }
            // Draw helpbox
            //EditorGUILayout.HelpBox (NWDConstants.K_APP_CONFIGURATION_HELPBOX, MessageType.None);
            // List environment
            //update the veriosn of Bundle
            //NWDVersion.UpdateVersionBundle ();

            // Draw warning if salt for class is false
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.kAlertSaltShortError, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
            }
            // Draw helpbox
            //EditorGUILayout.HelpBox (NWDConstants.K_APP_CONFIGURATION_HELPBOX, MessageType.None);
            //GUILayout.Label(NWDConstants.K_APP_CONFIGURATION_ENVIRONMENT_AREA, NWDConstants.kLabelTitleStyle);
            // List environment
            string[] tTabList = new string[3] {
                NWDConstants.K_APP_CONFIGURATION_DEV,
                NWDConstants.K_APP_CONFIGURATION_PREPROD,
                NWDConstants.K_APP_CONFIGURATION_PROD
            };
            // Draw interface for environment chooser
            int tTabSelect = GUILayout.Toolbar(TabSelected, tTabList);
            if (tTabSelect != TabSelected)
            {
                GUI.FocusControl(null);
                ScrollPosition = Vector2.zero;
                TabSelected = tTabSelect;
            }
            //update the veriosn of Bundle
            //NWDVersion.UpdateVersionBundle ();
            // Draw interface for environment selected inn scrollview
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, EditorStyles.inspectorFullWidthMargins, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            switch (tTabSelect)
            {
                case 0:
                    {
                        NWDAppConfiguration.SharedInstance().DevEnvironment.DrawInEditor(this);
                    }
                    break;
                case 1:
                    {
                        NWDAppConfiguration.SharedInstance().PreprodEnvironment.DrawInEditor(this);
                    }
                    break;
                case 2:
                    {
                        NWDAppConfiguration.SharedInstance().ProdEnvironment.DrawInEditor(this);
                    }
                    break;
            }
            GUILayout.EndScrollView();
            GUILayout.Space(8.0f);
            if (GUILayout.Button(NWDConstants.K_APP_CONFIGURATION_SAVE_BUTTON))
            {
                NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            }
            GUILayout.Space(8.0f);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
