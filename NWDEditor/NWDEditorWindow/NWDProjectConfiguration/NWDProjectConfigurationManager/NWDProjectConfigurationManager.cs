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
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// Class use to configure the project
    /// </summary>
    public class NWDProjectConfigurationManagerContent : NWDEditorWindowContent
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// benchmark color pro skin for fast result
        /// </summary>
        const string kColorGreen_Pro = "2EDD66FF";
        /// <summary>
        /// benchmark color pro skin for normal result
        /// </summary>
        const string kColorOrange_Pro = "FF9842FF";
        /// <summary>
        /// benchmark color pro skin for slow result
        /// </summary>
        const string kColorRed_Pro = "FF7070FF";
        /// <summary>
        /// benchmark color pro skin for quick analyze
        /// </summary>
        const string kColorBlue_Pro = "7092FFFF";
        /// <summary>
        /// benchmark color standard skin for fast result
        /// </summary>
        const string kColorGreen = "007626FF";
        /// <summary>
        /// benchmark color standard skin for normal result
        /// </summary>
        const string kColorOrange = "B45200FF";
        /// <summary>
        /// benchmark color standard skin for slow result
        /// </summary>
        const string kColorRed = "890000FF";
        /// <summary>
        /// benchmark color standard skin for quick analyze
        /// </summary>
        const string kColorBlue = "002089FF";
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Scroll position in window
        /// </summary>
        private static Vector2 _kScrollPosition;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show start benchmark footprint in console
        /// </summary>
        bool BenchmarkShowStart;
        /// <summary>
        /// Limit the min result's time to print console
        /// </summary>
        float BenchmarkLimit;
        /// <summary>
        /// Copy the log in clipboard when use <see cref="NWDDebug.Log(string, Object)"/> 
        /// </summary>
        bool Clipboard;
        /// <summary>
        /// The username for this desktop
        /// </summary>
        public string UserName = string.Empty;
        /// <summary>
        /// Draw when recompile 
        /// </summary>
        bool ShowCompile;
        /// <summary>
        /// Color for fast result
        /// </summary>
        Color Green;
        /// <summary>
        /// Color for normal result
        /// </summary>
        Color Red;
        /// <summary>
        /// Color for slow result
        /// </summary>
        Color Orange;
        /// <summary>
        /// Color for quick analyze
        /// </summary>
        Color Blue;
        /// <summary>
        /// Window style for new <see cref="NWDEditorWindow"/>
        /// </summary>
        NWDWindowStyle EditorWindowStyle;
        /// <summary>
        /// Bypass build evironment dialog with predefine choice or not
        /// </summary>
        NWDEditorBuildEnvironment EditorBuildEnvironment;
        /// <summary>
        /// Bypass build rename dialog with predefine choice or not
        /// </summary>
        NWDEditorBuildRename EditorBuildRename;
        /// <summary>
        /// Bypass build database dialog with predefine choice or not
        /// </summary>
        NWDEditorBuildDatabaseUpdate EditorBuildDatabaseUpdate;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance for deamon class.
        /// </summary>
        private static NWDProjectConfigurationManagerContent _kSharedInstanceContent;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstanceContent"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDProjectConfigurationManagerContent SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstanceContent == null)
            {
                _kSharedInstanceContent = new NWDProjectConfigurationManagerContent();
                _kSharedInstanceContent.Load();
            }
            NWDBenchmark.Finish();
            return _kSharedInstanceContent;
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
        public static NWDWindowStyle GetWindowStyle()
        {
            return (NWDWindowStyle)NWDProjectPrefs.GetInt(NWDConstants.K_EDITOR_WINDOW_STYLE);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetWindowStyle(NWDWindowStyle sValue)
        {
            NWDProjectPrefs.SetInt(NWDConstants.K_EDITOR_WINDOW_STYLE, (int)sValue);
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
        public void Load()
        {
            NWDBenchmark.Start();
            // get values
            BenchmarkShowStart = NWDProjectPrefs.GetBool(NWDConstants.K_EDITOR_BENCHMARK_SHOW_START);
            BenchmarkLimit = NWDProjectPrefs.GetFloat(NWDConstants.K_EDITOR_BENCHMARK_LIMIT);
            Clipboard = NWDProjectPrefs.GetBool(NWDConstants.K_EDITOR_CLIPBOARD_LAST_LOG);
            UserName = NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_USER_BUILDER, "(user?)");
            ShowCompile = NWDProjectPrefs.GetBool(NWDConstants.K_EDITOR_SHOW_COMPILE);
            EditorBuildEnvironment = GetEditoBuildEnvironment();
            EditorBuildRename = GetEditoBuildRename();
            EditorBuildDatabaseUpdate = GetEditorBuildDatabaseUpdate();

            EditorWindowStyle = GetWindowStyle();

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
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Save()
        {
            NWDBenchmark.Start();
            // set values
            NWDProjectPrefs.SetBool(NWDConstants.K_EDITOR_BENCHMARK_SHOW_START, BenchmarkShowStart);
            NWDProjectPrefs.SetFloat(NWDConstants.K_EDITOR_BENCHMARK_LIMIT, BenchmarkLimit);
            NWDProjectPrefs.SetBool(NWDConstants.K_EDITOR_CLIPBOARD_LAST_LOG, Clipboard);
            NWDProjectPrefs.SetString(NWDConstants.K_EDITOR_USER_BUILDER, UserName);
            NWDProjectPrefs.SetBool(NWDConstants.K_EDITOR_SHOW_COMPILE, ShowCompile);

            SetEditorBuildEnvironment(EditorBuildEnvironment);
            SetEditorBuildRename(EditorBuildRename);
            SetEditorBuildDatabaseUpdate(EditorBuildDatabaseUpdate);

            SetWindowStyle(EditorWindowStyle);

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
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI(Rect sRect)
        {
            NWDBenchmark.Start();
            // start
            base.OnPreventGUI(sRect);
            NWDGUILayout.Title("Project's configurations");
            // start scroll
            _kScrollPosition = GUILayout.BeginScrollView(_kScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            EditorGUI.BeginChangeCheck();
            //User preferences
            NWDGUILayout.Section("User's configurations");
            UserName = EditorGUILayout.TextField("Username", UserName);
            EditorGUILayout.LabelField("System footprint", SystemInfo.deviceUniqueIdentifier);
            //General preferences
            NWDGUILayout.SubSection("General configurations");
            EditorWindowStyle = (NWDWindowStyle)EditorGUILayout.EnumPopup("Window style", EditorWindowStyle);
            ShowCompile = EditorGUILayout.Toggle("Show re-compile", ShowCompile);
            // build preference section
            NWDGUILayout.SubSection("Build configurations");
            //define environment build
            EditorBuildEnvironment = (NWDEditorBuildEnvironment)EditorGUILayout.EnumPopup("Build environment", EditorBuildEnvironment);
            // define rename option
            EditorBuildRename = (NWDEditorBuildRename)EditorGUILayout.EnumPopup("Build rename", EditorBuildRename);
            // define update database
            EditorBuildDatabaseUpdate = (NWDEditorBuildDatabaseUpdate)EditorGUILayout.EnumPopup("Copy database in build", EditorBuildDatabaseUpdate);
            // Debug and benchmark
            NWDGUILayout.SubSection("Debug and benchmark");
            NWDGUILayout.Informations("Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)");
            NWDGUILayout.SubSection("Console");
            Clipboard = EditorGUILayout.ToggleLeft("Copy NWDDebug.Log in clipboard", Clipboard);
            NWDGUILayout.SubSection("Benchmark");
            BenchmarkShowStart = EditorGUILayout.ToggleLeft("Benchmark show start", BenchmarkShowStart);
            BenchmarkLimit = EditorGUILayout.Slider("Benchmark limit", BenchmarkLimit, 0F, 1.5F);
            if (EditorGUI.EndChangeCheck() == true)
            {
                Save();
            }
            // Change color
            EditorGUI.BeginChangeCheck();
            Green = EditorGUILayout.ColorField("Green highlight", Green);
            Orange = EditorGUILayout.ColorField("Orange highlight", Orange);
            Red = EditorGUILayout.ColorField("Red highlight", Red);
            Blue = EditorGUILayout.ColorField("Blue highlight", Blue);
            if (GUILayout.Button("Reset color"))
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
            // Shared configurations
            NWDGUILayout.Section("Shared configurations");
            NWDGUILayout.SubSection("Webhook URL for Slack");
            NWDAppConfiguration.SharedInstance().SlackWebhookURLNotification = EditorGUILayout.TextField("Webhook URL Notification", NWDAppConfiguration.SharedInstance().SlackWebhookURLNotification);
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(NWDAppConfiguration.SharedInstance().SlackWebhookURLNotification));
            if (GUILayout.Button("Test Webhook Notification"))
            {
                NWDOperationWebhook.NewMessage("Test Webhook Notification integration success!", WebHookType.Notification);
            }

            NWDAppConfiguration.SharedInstance().SlackWebhookURLSync = EditorGUILayout.TextField("Webhook URL Sync", NWDAppConfiguration.SharedInstance().SlackWebhookURLSync);
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(NWDAppConfiguration.SharedInstance().SlackWebhookURLSync));
            if (GUILayout.Button("Test Webhook Sync"))
            {
                NWDOperationWebhook.NewMessage("Test Webhook Sync integration success!", WebHookType.Sync);
            }
            NWDAppConfiguration.SharedInstance().SlackWebhookURLUpgrade = EditorGUILayout.TextField("Webhook URL Upgrade", NWDAppConfiguration.SharedInstance().SlackWebhookURLUpgrade);
            EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(NWDAppConfiguration.SharedInstance().SlackWebhookURLUpgrade));
            if (GUILayout.Button("Test Webhook Upgrade"))
            {
                NWDOperationWebhook.NewMessage("Test Webhook Upgrade integration success!", WebHookType.Ugrade);
            }
            // end scroll
            GUILayout.EndScrollView();
            // finish with reccord red button
            NWDGUILayout.Line();
            NWDGUILayout.LittleSpace();
            NWDGUI.BeginRedArea();
            if (GUILayout.Button(NWDConstants.K_APP_CONFIGURATION_SAVE_BUTTON))
            {
                NWDEditorWindow.GenerateCSharpFile();
            }
            NWDGUI.EndRedArea();
            NWDGUILayout.BigSpace();
            // end
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDProjectConfigurationManager : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string TutorialLink(string sLink = "")
        {
            return NWDConstants.K_NWDURL + "project-configuration-manager/";
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        private static NWDProjectConfigurationManager kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the SharedInstance or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDProjectConfigurationManager SharedInstance()
        {
            NWDBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDProjectConfigurationManager), ShowAsWindow()) as NWDProjectConfigurationManager;
            }
            NWDBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the SharedInstance of Editor Configuration Manager Window and focus on.
        /// </summary>
        /// <returns></returns>
        public static NWDProjectConfigurationManager SharedInstanceFocus()
        {
            NWDBenchmark.Start();
            SharedInstance().ShowMe();
            SharedInstance().Focus();
            NWDBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Repaint all Editor Configuration Manager Windows.
        /// </summary>
        public static void Refresh()
        {
            NWDBenchmark.Start();
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDProjectConfigurationManager));
            foreach (NWDProjectConfigurationManager tWindow in tWindows)
            {
                tWindow.Repaint();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// On enable action.
        /// </summary>
        public void OnEnable()
        {
            NWDBenchmark.Start();
            // set ideal size
            NormalizeWidth = 380;
            NormalizeHeight = 750;
            // set title
            TitleInit(NWDConstants.K_PROJECT_CONFIGURATION_TITLE, typeof(NWDProjectConfigurationManager));
            // get values
            NWDProjectConfigurationManagerContent.SharedInstance().Load();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI()
        {
            NWDBenchmark.Start();
            NWDGUI.LoadStyles();
            NWDProjectConfigurationManagerContent.SharedInstance().OnPreventGUI(position);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
