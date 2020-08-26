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
using UnityEngine;
using System.IO;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAppEnvironmentConfigurationManagerContent : NWDEditorWindowContent
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Scroll position in window
        /// </summary>
        private static Vector2 _kScrollPosition;
        /// <summary>
        /// The tab of environment selected.
        /// </summary>
        private static int _kTabSelected = 0;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance for deamon class.
        /// </summary>
        private static NWDAppEnvironmentConfigurationManagerContent _kSharedInstanceContent;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstanceContent"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDAppEnvironmentConfigurationManagerContent SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstanceContent == null)
            {
                _kSharedInstanceContent = new NWDAppEnvironmentConfigurationManagerContent();
            }
            NWDBenchmark.Finish();
            return _kSharedInstanceContent;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI(Rect sRect)
        {
            base.OnPreventGUI(sRect);
            NWDBenchmark.Start();
            NWDGUILayout.Title("Configuration environments");
            NWDGUILayout.Section("Environments");
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_ALERT_SALT_SHORT_ERROR, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDEditorWindow.GenerateCSharpFile();
                    //NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
            }
            // Draw warning if salt for class is false
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_ALERT_SALT_SHORT_ERROR, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDEditorWindow.GenerateCSharpFile();
                    //NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
            }
            // List environment
            string[] tTabList = new string[3] {
                NWDConstants.K_APP_CONFIGURATION_DEV,
                NWDConstants.K_APP_CONFIGURATION_PREPROD,
                NWDConstants.K_APP_CONFIGURATION_PROD
            };
            // Draw interface for environment chooser
            int tTabSelect = GUILayout.Toolbar(_kTabSelected, tTabList);
            if (tTabSelect != _kTabSelected)
            {
                GUI.FocusControl(null);
                _kScrollPosition = Vector2.zero;
                _kTabSelected = tTabSelect;
            }
            NWDGUILayout.LittleSpace();
            NWDGUILayout.Line();
            // Draw interface for environment selected inn scrollview
            _kScrollPosition = GUILayout.BeginScrollView(_kScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            switch (tTabSelect)
            {
                case 0:
                    {
                        NWDAppConfiguration.SharedInstance().DevEnvironment.DrawInEditor();
                    }
                    break;
                case 1:
                    {
                        NWDAppConfiguration.SharedInstance().PreprodEnvironment.DrawInEditor();
                    }
                    break;
                case 2:
                    {
                        NWDAppConfiguration.SharedInstance().ProdEnvironment.DrawInEditor();
                    }
                    break;
            }
            GUILayout.EndScrollView();
            NWDGUILayout.Line();
            NWDGUILayout.LittleSpace();
            NWDGUI.BeginRedArea();
            if (GUILayout.Button(NWDConstants.K_APP_CONFIGURATION_SAVE_BUTTON))
            {
                NWDEditorWindow.GenerateCSharpFile();
                //NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            }
            NWDGUI.EndRedArea();
            NWDGUILayout.BigSpace();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAppEnvironmentConfigurationManager : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        private static NWDAppEnvironmentConfigurationManager _kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentConfigurationManager SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstance == null)
            {
                _kSharedInstance = EditorWindow.GetWindow(typeof(NWDAppEnvironmentConfigurationManager), ShowAsUtility()) as NWDAppEnvironmentConfigurationManager;
            }
            NWDBenchmark.Finish();
            return _kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentConfigurationManager SharedInstanceFocus()
        {
            NWDBenchmark.Start();
            //SharedInstance().ShowUtility();
            SharedInstance().ShowMe();
            SharedInstance().Focus();
            NWDBenchmark.Finish();
            return _kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Refresh()
        {
            NWDBenchmark.Start();
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDAppEnvironmentConfigurationManager));
            foreach (NWDAppEnvironmentConfigurationManager tWindow in tWindows)
            {
                tWindow.Repaint();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            NWDBenchmark.Start();
            TitleInit(NWDConstants.K_ENVIRONMENTS_CONFIGURATION_TITLE, typeof(NWDAppEnvironmentConfigurationManager));
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            NWDBenchmark.Start();
            NWDGUI.LoadStyles();
            NWDAppEnvironmentConfigurationManagerContent.SharedInstance().OnPreventGUI(position);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
