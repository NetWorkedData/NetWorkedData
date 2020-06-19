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
        bool Clipboard;
        string UserName;
        int PanelWidth;
        bool ShowCompile;
        NWDEditorBuildEnvironment EditorBuildEnvironment;
        NWDEditorBuildRename EditorBuildRename;
        NWDEditorBuildDatabaseUpdate EditorBuildDatabaseUpdate;
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
            // set title
            TitleInit(NWDConstants.K_PROJECT_CONFIGURATION_TITLE, typeof(NWDProjectConfigurationManager));
            // get values
            Load();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Load()
        {
            //NWEBenchmark.Start();
            // get values
            Clipboard = NWDProjectPrefs.GetBool(NWDConstants.K_EDITOR_CLIPBOARD_LAST_LOG);
            UserName = NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_USER_BUILDER, "(user)");
            PanelWidth = NWDProjectPrefs.GetInt(NWDConstants.K_EDITOR_PANEL_WIDTH, 320);
            ShowCompile = NWDProjectPrefs.GetBool(NWDConstants.K_EDITOR_SHOW_COMPILE);
            EditorBuildEnvironment = GetEditoBuildEnvironment();
            EditorBuildRename = GetEditoBuildRename();
            EditorBuildDatabaseUpdate = GetEditorBuildDatabaseUpdate();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Save()
        {
            //NWEBenchmark.Start();
            // set values
            NWDProjectPrefs.SetBool(NWDConstants.K_EDITOR_CLIPBOARD_LAST_LOG, Clipboard);
            NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_USER_BUILDER, UserName);
            NWDProjectPrefs.SetInt(NWDConstants.K_EDITOR_PANEL_WIDTH, PanelWidth);
            NWDProjectPrefs.SetBool(NWDConstants.K_EDITOR_SHOW_COMPILE, ShowCompile);
            SetEditorBuildEnvironment(EditorBuildEnvironment);
            SetEditorBuildRename(EditorBuildRename);
            SetEditorBuildDatabaseUpdate(EditorBuildDatabaseUpdate);
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

            //User preferences
            NWDGUILayout.Section("User preferences");
            Clipboard = EditorGUILayout.ToggleLeft("Copy DebugLog In Clipoard", Clipboard);
            UserName = EditorGUILayout.TextField("User builder name", UserName);
            PanelWidth = EditorGUILayout.IntSlider("Panel data width", PanelWidth, 300, 400);

            //General preferences
            NWDGUILayout.Section("General preferences");
            ShowCompile = EditorGUILayout.Toggle("Show re-compile ", ShowCompile);
          
            // build preference section
            NWDGUILayout.Section("Project build preferences");
            //define environment build
            EditorBuildEnvironment = (NWDEditorBuildEnvironment)EditorGUILayout.EnumPopup("Build Environment", EditorBuildEnvironment);
            // define rename option
            EditorBuildRename = (NWDEditorBuildRename)EditorGUILayout.EnumPopup("Build Rename", EditorBuildRename);
            // define update database
            EditorBuildDatabaseUpdate = (NWDEditorBuildDatabaseUpdate)EditorGUILayout.EnumPopup("Copy database in build", EditorBuildDatabaseUpdate);

            if (EditorGUI.EndChangeCheck() == true)
            {
                Save();
            }

            // end scroll
            GUILayout.EndScrollView();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
