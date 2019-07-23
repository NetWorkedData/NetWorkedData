//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:17
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
            //BTBBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDAppEnvironmentConfigurationManager)) as NWDAppEnvironmentConfigurationManager;
            }
            //BTBBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentConfigurationManager SharedInstanceFocus()
        {
            //BTBBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //BTBBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Refresh()
        {
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDAppEnvironmentConfigurationManager));
            foreach (NWDAppEnvironmentConfigurationManager tWindow in tWindows)
            {
                tWindow.Repaint();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static bool IsSharedInstance()
        //{
        //    if (kSharedInstance != null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            //BTBBenchmark.Start();
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDConstants.K_ENVIRONMENTS_CONFIGURATION_TITLE;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets(typeof(NWDAppEnvironmentConfigurationManager).Name +" t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals(typeof(NWDAppEnvironmentConfigurationManager).Name))
                        {
                            IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        }
                    }
                }
                titleContent = IconAndTitle;
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnGUI()
        {
            //BTBBenchmark.Start();
            NWDGUI.LoadStyles();
            NWDGUILayout.Title("Configuration environments");
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_ALERT_SALT_SHORT_ERROR, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
            }
            // Draw warning if salt for class is false
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_ALERT_SALT_SHORT_ERROR, MessageType.Error);
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
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
