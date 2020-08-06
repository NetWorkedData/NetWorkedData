//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
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
        const string kColorGreen_Pro = "2EDD66FF";
        const string kColorOrange_Pro = "FF9842FF";
        const string kColorRed_Pro = "FF7070FF";
        const string kColorBlue_Pro = "7092FFFF";

        const string kColorGreen = "007626FF";
        const string kColorOrange = "B45200FF";
        const string kColorRed = "890000FF";
        const string kColorBlue = "002089FF";
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
        bool BenchmarkShowStart;
        float BenchmarkLimit;
        bool Clipboard;
        string UserName;
        int PanelWidth;
        bool ShowCompile;
        Color Green;
        Color Red;
        Color Orange;
        Color Blue;
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
            //NWDBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDProjectConfigurationManager)) as NWDProjectConfigurationManager;
            }
            //NWDBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the SharedInstance of Editor Configuration Manager Window and focus on.
        /// </summary>
        /// <returns></returns>
        public static NWDProjectConfigurationManager SharedInstanceFocus()
        {
            //NWDBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //NWDBenchmark.Finish();
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
            //NWDBenchmark.Start();
            // set title
            TitleInit(NWDConstants.K_PROJECT_CONFIGURATION_TITLE, typeof(NWDProjectConfigurationManager));
            // get values
            Load();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Load()
        {
            //NWDBenchmark.Start();
            // get values
            BenchmarkShowStart = NWDProjectPrefs.GetBool(NWDConstants.K_EDITOR_BENCHMARK_SHOW_START);
            BenchmarkLimit = NWDProjectPrefs.GetFloat(NWDConstants.K_EDITOR_BENCHMARK_LIMIT);
            Clipboard = NWDProjectPrefs.GetBool(NWDConstants.K_EDITOR_CLIPBOARD_LAST_LOG);
            UserName = NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_USER_BUILDER, "(user)");
            PanelWidth = NWDProjectPrefs.GetInt(NWDConstants.K_EDITOR_PANEL_WIDTH, 320);
            ShowCompile = NWDProjectPrefs.GetBool(NWDConstants.K_EDITOR_SHOW_COMPILE);
            EditorBuildEnvironment = GetEditoBuildEnvironment();
            EditorBuildRename = GetEditoBuildRename();
            EditorBuildDatabaseUpdate = GetEditorBuildDatabaseUpdate();

            if (EditorGUIUtility.isProSkin)
            {
                Green = NWDToolbox.ColorFromString(NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_BENCHMARK_GREEN_PRO, kColorGreen_Pro));
                Orange = NWDToolbox.ColorFromString(NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_BENCHMARK_ORANGE_PRO, kColorOrange_Pro));
                Red = NWDToolbox.ColorFromString(NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_BENCHMARK_RED_PRO, kColorRed_Pro));
                Blue = NWDToolbox.ColorFromString(NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_BENCHMARK_BLUE_PRO, kColorBlue_Pro));
            }
            else
            {
                Green = NWDToolbox.ColorFromString(NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_BENCHMARK_GREEN, kColorGreen));
                Orange = NWDToolbox.ColorFromString(NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_BENCHMARK_ORANGE, kColorOrange));
                Red = NWDToolbox.ColorFromString(NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_BENCHMARK_RED, kColorRed));
                Blue = NWDToolbox.ColorFromString(NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_BENCHMARK_BLUE, kColorBlue));
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Save()
        {
            //NWDBenchmark.Start();
            // set values
            NWDProjectPrefs.SetBool(NWDConstants.K_EDITOR_BENCHMARK_SHOW_START, BenchmarkShowStart);
            NWDProjectPrefs.SetFloat(NWDConstants.K_EDITOR_BENCHMARK_LIMIT, BenchmarkLimit);
            NWDProjectPrefs.SetBool(NWDConstants.K_EDITOR_CLIPBOARD_LAST_LOG, Clipboard);
            NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_USER_BUILDER, UserName);
            NWDProjectPrefs.SetInt(NWDConstants.K_EDITOR_PANEL_WIDTH, PanelWidth);
            NWDProjectPrefs.SetBool(NWDConstants.K_EDITOR_SHOW_COMPILE, ShowCompile);

            SetEditorBuildEnvironment(EditorBuildEnvironment);
            SetEditorBuildRename(EditorBuildRename);
            SetEditorBuildDatabaseUpdate(EditorBuildDatabaseUpdate);

            if (EditorGUIUtility.isProSkin)
            {
                NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_BENCHMARK_GREEN_PRO, NWDToolbox.ColorToString(Green));
                NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_BENCHMARK_ORANGE_PRO, NWDToolbox.ColorToString(Orange));
                NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_BENCHMARK_RED_PRO, NWDToolbox.ColorToString(Red));
                NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_BENCHMARK_BLUE_PRO, NWDToolbox.ColorToString(Blue));
            }
            else
            {
                NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_BENCHMARK_GREEN, NWDToolbox.ColorToString(Green));
                NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_BENCHMARK_ORANGE, NWDToolbox.ColorToString(Orange));
                NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_BENCHMARK_RED, NWDToolbox.ColorToString(Red));
                NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_BENCHMARK_BLUE, NWDToolbox.ColorToString(Blue));
            }

            NWDBenchmark.PrefReload();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI()
        {
            NWDBenchmark.Start();
            NWDGUI.LoadStyles();

            NWDGUILayout.Title("Project preferences");

            // start scroll
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            EditorGUI.BeginChangeCheck();

            //User preferences
            NWDGUILayout.Section("User preferences");
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

            // build Debug section
            NWDGUILayout.Section("Debug and Benchmark in Editor");
            NWDGUILayout.Informations("Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)");
            Clipboard = EditorGUILayout.ToggleLeft("Copy NWDDebug.Log in clipoard", Clipboard);
            BenchmarkShowStart = EditorGUILayout.ToggleLeft("Benchmark show start", BenchmarkShowStart);
            BenchmarkLimit = EditorGUILayout.Slider("Benchmark min show", BenchmarkLimit, 0F, 1.5F);

            if (EditorGUI.EndChangeCheck() == true)
            {
                Save();
            }

            EditorGUI.BeginChangeCheck();
            Green = EditorGUILayout.ColorField("Green highlight", Green);
            Orange = EditorGUILayout.ColorField("Orange highlight", Orange);
            Red = EditorGUILayout.ColorField("Red highlight", Red);
            Blue = EditorGUILayout.ColorField("Blue highlight", Blue);
            if (GUILayout.Button("reset color"))
            {
                if (EditorGUIUtility.isProSkin)
                {
                    NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_BENCHMARK_GREEN_PRO, kColorGreen_Pro);
                    Green = NWDToolbox.ColorFromString(kColorGreen_Pro);
                    NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_BENCHMARK_ORANGE_PRO, kColorOrange_Pro);
                    Orange = NWDToolbox.ColorFromString(kColorOrange_Pro);
                    NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_BENCHMARK_RED_PRO, kColorRed_Pro);
                    Red = NWDToolbox.ColorFromString(kColorRed_Pro);
                    NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_BENCHMARK_BLUE_PRO, kColorBlue_Pro);
                    Blue = NWDToolbox.ColorFromString(kColorBlue_Pro);
                }
                else
                {
                    NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_BENCHMARK_GREEN, kColorGreen);
                    Green = NWDToolbox.ColorFromString(kColorGreen);
                    NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_BENCHMARK_ORANGE, kColorOrange);
                    Orange = NWDToolbox.ColorFromString(kColorOrange);
                    NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_BENCHMARK_RED, kColorRed);
                    Red = NWDToolbox.ColorFromString(kColorRed);
                    NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_BENCHMARK_BLUE, kColorBlue);
                    Blue = NWDToolbox.ColorFromString(kColorBlue);
                }
            }

            if (EditorGUI.EndChangeCheck() == true)
            {
                Debug.Log("" +
                    "TEST <color=#" + NWDToolbox.ColorToString(Green) + ">green " + NWDToolbox.ColorToString(Green) + " </color> " +
                    "TEST <color=#" + NWDToolbox.ColorToString(Orange) + ">orange " + NWDToolbox.ColorToString(Orange) + " </color> " +
                    "TEST <color=#" + NWDToolbox.ColorToString(Red) + ">red " + NWDToolbox.ColorToString(Red) + " </color>" +
                    "TEST <color=#" + NWDToolbox.ColorToString(Blue) + ">blue " + NWDToolbox.ColorToString(Blue) + " </color>" +
                    "");
                Save();
            }
            // end scroll
            GUILayout.EndScrollView();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
