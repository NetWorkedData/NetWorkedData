//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEngine;
using System.IO;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAppEnvironmentConfigurationManager : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        private static NWDAppEnvironmentConfigurationManager kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        GUIContent IconAndTitle;
        Vector2 ScrollPosition;
        static int TabSelected = 0;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentConfigurationManager SharedInstance()
        {
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDAppEnvironmentConfigurationManager)) as NWDAppEnvironmentConfigurationManager;
            }
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentConfigurationManager SharedInstanceFocus()
        {
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsSharedInstance()
        {
            if (kSharedInstance != null)
            {
                return true;
            }
            else
            {
                return false;
            }
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
        public void OnGUI()
        {
            NWDConstants.LoadStyles();
            NWDGUILayout.Title("Configuration environments");
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.kAlertSaltShortError, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
            }
            // Draw warning if salt for class is false
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.kAlertSaltShortError, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
            }
            // List environment
            string[] tTabList = new string[3] {
                NWDConstants.K_APP_CONFIGURATION_DEV,
                NWDConstants.K_APP_CONFIGURATION_PREPROD,
                NWDConstants.K_APP_CONFIGURATION_PROD
            };
            // Draw interface for environment chooser
            //NWDGUILayout.LittleSpace();
            int tTabSelect = GUILayout.Toolbar(TabSelected, tTabList);
            if (tTabSelect != TabSelected)
            {
                GUI.FocusControl(null);
                ScrollPosition = Vector2.zero;
                TabSelected = tTabSelect;
            }
            NWDGUILayout.LittleSpace();
            NWDGUILayout.Line();
            // Draw interface for environment selected inn scrollview
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
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
            NWDGUILayout.Line();
            NWDGUILayout.LittleSpace();
            NWDGUI.BeginRedArea();
            if (GUILayout.Button(NWDConstants.K_APP_CONFIGURATION_SAVE_BUTTON))
            {
                NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            }
            NWDGUI.EndRedArea();
            NWDGUILayout.BigSpace();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
