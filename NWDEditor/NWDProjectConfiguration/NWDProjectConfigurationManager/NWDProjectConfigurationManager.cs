//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDProjectConfigurationManager : NWDEditorWindow
    {

        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        private static NWDProjectConfigurationManager kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The scroll position.
        /// </summary>
        static Vector2 ScrollPosition;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the SharedInstance or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDProjectConfigurationManager SharedInstance()
        {
            //NWEBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDProjectConfigurationManager)) as NWDProjectConfigurationManager;
            }
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the SharedInstance of Editor Configuration Manager Window and focus on.
        /// </summary>
        /// <returns></returns>
        public static NWDProjectConfigurationManager SharedInstanceFocus()
        {
            //NWEBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDEditorBuildEnvironment GetEditoBuildEnvironment()
        {
            return (NWDEditorBuildEnvironment)NWDProjectPrefs.GetInt(NWDConstants.K_EDITOR_BUILD_ENVIRONMENT);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetEditorBuildEnvironment(NWDEditorBuildEnvironment sValue)
        {
            NWDProjectPrefs.SetInt(NWDConstants.K_EDITOR_BUILD_ENVIRONMENT, (int)sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDEditorBuildRename GetEditoBuildRename()
        {
            return (NWDEditorBuildRename)NWDProjectPrefs.GetInt(NWDConstants.K_EDITOR_BUILD_RENAME);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetEditorBuildRename(NWDEditorBuildRename sValue)
        {
            NWDProjectPrefs.SetInt(NWDConstants.K_EDITOR_BUILD_RENAME, (int)sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDEditorBuildDatabaseUpdate GetEditorBuildDatabaseUpdate()
        {
            return (NWDEditorBuildDatabaseUpdate)NWDProjectPrefs.GetInt(NWDConstants.K_EDITOR_BUILD_DATABASE_UPDATE);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetEditorBuildDatabaseUpdate(NWDEditorBuildDatabaseUpdate sValue)
        {
            NWDProjectPrefs.SetInt(NWDConstants.K_EDITOR_BUILD_DATABASE_UPDATE, (int)sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Repaint all Editor Configuration Manager Windows.
        /// </summary>
        public static void Refresh()
        {
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDProjectConfigurationManager));
            foreach (NWDProjectConfigurationManager tWindow in tWindows)
            {
                tWindow.Repaint();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool IsSharedInstanced()
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
        /// <summary>
        /// On enable action.
        /// </summary>
        public void OnEnable()
        {
            //NWEBenchmark.Start();
            TitleInit(NWDConstants.K_PROJECT_CONFIGURATION_TITLE, typeof(NWDProjectConfigurationManager));
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI()
        {
            //NWEBenchmark.Start();

            NWDGUI.LoadStyles();

            NWDGUILayout.Title("Project preferences");

            // start scroll
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            EditorGUI.BeginChangeCheck();

            //General preferences
            NWDGUILayout.Section("User preferences");
            bool tEDITOR_CLIPBOARD_LAST_LOG  = EditorGUILayout.ToggleLeft("Copy DebugLog In Clipoard", NWDProjectPrefs.GetBool(NWDConstants.K_EDITOR_CLIPBOARD_LAST_LOG));
            string tEDITOR_USER_BUILDER = EditorGUILayout.TextField("User builder name", NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_USER_BUILDER, "(user)"));
            int tEDITOR_PANEL_WIDTH = EditorGUILayout.IntSlider("Panel data width", NWDProjectPrefs.GetInt(NWDConstants.K_EDITOR_PANEL_WIDTH, 320), 300, 400);

            //General preferences
            NWDGUILayout.Section("General preferences");
           // NWDAppConfiguration.SharedInstance().EditorTableCommun = EditorGUILayout.Toggle("Table Pref commun", NWDAppConfiguration.SharedInstance().EditorTableCommun);

            bool tEDITOR_SHOW_COMPILE = EditorGUILayout.Toggle("Show re-compile ", NWDProjectPrefs.GetBool(NWDConstants.K_EDITOR_SHOW_COMPILE));
            //NWDAppConfiguration.SharedInstance().TintColor = EditorGUILayout.ColorField("Tint color ", NWDAppConfiguration.SharedInstance().TintColor);
            //if (GUILayout.Button("Reset Tint color"))
            //{
            //    NWDAppConfiguration.SharedInstance().ResetTintColor();
            //}
            NWDGUILayout.Line();
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_ALERT_SALT_SHORT_ERROR, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDEditorWindow.GenerateCSharpFile();
                    //NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
            }

            // build preference section
            NWDGUILayout.Section("Project build preferences");
            //define environment build
            NWDEditorBuildEnvironment tNWDEditorBuildEnvironment = GetEditoBuildEnvironment();
            tNWDEditorBuildEnvironment = (NWDEditorBuildEnvironment)EditorGUILayout.EnumPopup("Build Environment", tNWDEditorBuildEnvironment);
            // define rename option
            NWDEditorBuildRename tNWDEditorBuildRename = GetEditoBuildRename();
            tNWDEditorBuildRename = (NWDEditorBuildRename)EditorGUILayout.EnumPopup("Build Rename", tNWDEditorBuildRename);
            // define update database
            NWDEditorBuildDatabaseUpdate tNWDEditorBuildDatabaseUpdate = GetEditorBuildDatabaseUpdate();
            tNWDEditorBuildDatabaseUpdate = (NWDEditorBuildDatabaseUpdate)EditorGUILayout.EnumPopup("Copy database in build", tNWDEditorBuildDatabaseUpdate);


            if (EditorGUI.EndChangeCheck())
            {
                NWDProjectPrefs.SetBool(NWDConstants.K_EDITOR_CLIPBOARD_LAST_LOG, tEDITOR_CLIPBOARD_LAST_LOG);
                NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_USER_BUILDER, tEDITOR_USER_BUILDER);
                NWDProjectPrefs.SetInt(NWDConstants.K_EDITOR_PANEL_WIDTH, tEDITOR_PANEL_WIDTH);
                NWDProjectPrefs.SetBool(NWDConstants.K_EDITOR_SHOW_COMPILE, tEDITOR_SHOW_COMPILE);
                SetEditorBuildEnvironment(tNWDEditorBuildEnvironment);
                SetEditorBuildRename(tNWDEditorBuildRename);
                SetEditorBuildDatabaseUpdate(tNWDEditorBuildDatabaseUpdate);
            }

                // end scroll
                GUILayout.EndScrollView();

            //NWDGUILayout.Line();
            //GUILayout.Space(NWDGUI.kFieldMarge);
            //NWDGUI.BeginRedArea();
            //if (GUILayout.Button(NWDConstants.K_APP_CONFIGURATION_SAVE_BUTTON))
            //{
            //    NWDEditorWindow.GenerateCSharpFile();
            //    //NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            //}
            //NWDGUI.EndRedArea();
            NWDGUILayout.BigSpace();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
