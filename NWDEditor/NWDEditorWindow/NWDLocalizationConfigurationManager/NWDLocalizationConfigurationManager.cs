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
    public class NWDLocalizationConfigurationManagerContent : NWDEditorWindowContent
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Scroll position in window
        /// </summary>
        private static Vector2 _kScrollPosition;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance for deamon class.
        /// </summary>
        private static NWDLocalizationConfigurationManagerContent _kSharedInstanceContent;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstanceContent"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDLocalizationConfigurationManagerContent SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstanceContent == null)
            {
                _kSharedInstanceContent = new NWDLocalizationConfigurationManagerContent();
            }
            NWDBenchmark.Finish();
            return _kSharedInstanceContent;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnEnable(NWDEditorWindow sEditorWindow)
        {
            NWDBenchmark.Start();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnDisable(NWDEditorWindow sEditorWindow)
        {
            NWDBenchmark.Start();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI(Rect sRect)
        {
            base.OnPreventGUI(sRect);
            NWDBenchmark.Start();
            float tWidthUsed = sRect.width;
            NWDGUILayout.Title(NWDConstants.K_APP_CONFIGURATION_LANGUAGE_AREA);
            //NWDGUILayout.Informations("Some informations");
            //NWDGUILayout.Line();
            _kScrollPosition = GUILayout.BeginScrollView(_kScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            NWDGUILayout.Section(NWDConstants.K_APP_CONFIGURATION_LANGUAGE_AREA);
            Dictionary<string, string> tLanguageDico = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguageDico;
            //NWDGUILayout.Label("Select the languages of multi-localizable field in editor.");
            GUILayout.BeginHorizontal();
            List<string> tResult = new List<string>();
            float tToggleWidth = 140.0f;
            int tColunm = 0;
            int tColunmMax = Mathf.CeilToInt(tWidthUsed / tToggleWidth) - 1;
            if (tColunmMax < 1)
            {
                tColunmMax = 1;
            }
            foreach (KeyValuePair<string, string> tKeyValue in tLanguageDico)
            {
                if (tColunmMax <= tColunm)
                {
                    tColunm = 0;
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
                tColunm++;
                bool tContains = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString.Contains(tKeyValue.Value);
                tContains = EditorGUILayout.ToggleLeft(tKeyValue.Key, tContains, GUILayout.Width(tToggleWidth));
                if (tContains == true)
                {
                    tResult.Add(tKeyValue.Value);
                }
            }
            GUILayout.EndHorizontal();
            if (tResult.Count == 0)
            {
                tResult.Add("en");
            }
            tResult.Sort();
            NWDGUILayout.Section(NWDConstants.K_APP_CONFIGURATION_BUNDLENAMEE_AREA);
            NWDGUILayout.Label("Experimental : localizate the bundle IOS and MacOS app name.");
            foreach (string tLang in tResult)
            {
                if (NWDAppConfiguration.SharedInstance().BundleName.ContainsKey(tLang) == false)
                {
                    NWDAppConfiguration.SharedInstance().BundleName.Add(tLang, string.Empty);
                }
                NWDAppConfiguration.SharedInstance().BundleName[tLang] = EditorGUILayout.TextField(tLang, NWDAppConfiguration.SharedInstance().BundleName[tLang]);
            }
            NWDGUILayout.Section(NWDConstants.K_APP_CONFIGURATION_DEV_LOCALALIZATION_AREA);
            NWDGUILayout.Label("Select the default language of the app.");
            int tIndex = tResult.IndexOf(NWDAppConfiguration.SharedInstance().ProjetcLanguage);
            if (tIndex < 0)
            {
                tIndex = 0;
            }
            int tSelect = EditorGUILayout.Popup(NWDConstants.K_APP_CONFIGURATION_DEV_LOCALALIZATION_CHOOSE, tIndex, tResult.ToArray());
            NWDAppConfiguration.SharedInstance().ProjetcLanguage = tResult[tSelect];
            string tNewLanguages = NWDDataLocalizationManager.kBaseDev + ";" + string.Join(";", tResult.ToArray());
            if (NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString != tNewLanguages)
            {
                NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString = tNewLanguages;
                NWDDataInspector.ActiveRepaint();
            }
            NWDGUILayout.Section("Special localizations operations");
            NWDGUILayout.Label("Reorder all localizations for all datas (to see the same order in all datas).");
            if (GUILayout.Button("Reorder all localizations"))
            {
                NWDAppConfiguration.SharedInstance().DataLocalizationManager.ReOrderAllLocalizations();
            }

            NWDGUILayout.Label("Export all localizations in CSV's file to send to translate.");
            if (GUILayout.Button("Export localizations in CSV's file"))
            {
                NWDAppConfiguration.SharedInstance().DataLocalizationManager.ExportToCSV();
            }

            NWDGUILayout.Label("Import all localizations translated from CSV's file.");
            if (GUILayout.Button("Import localizations from CSV's file"))
            {
                NWDAppConfiguration.SharedInstance().DataLocalizationManager.ImportFromCSV();
            }
            NWDGUILayout.BigSpace();
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
    public class NWDLocalizationConfigurationManager : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        private static NWDLocalizationConfigurationManager _kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstance"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDLocalizationConfigurationManager SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstance == null)
            {
                _kSharedInstance = EditorWindow.GetWindow(typeof(NWDLocalizationConfigurationManager)) as NWDLocalizationConfigurationManager;
            }
            NWDBenchmark.Finish();
            return _kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the <see cref="_kSharedInstance"/> of <see cref="NWDLocalizationConfigurationManager"/> and focus on.
        /// </summary>
        /// <returns></returns>
        public static void SharedInstanceFocus()
        {
            NWDBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Repaint all <see cref="NWDLocalizationConfigurationManager"/>.
        /// </summary>
        public static void Refresh()
        {
            NWDBenchmark.Start();
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDLocalizationConfigurationManager));
            foreach (NWDLocalizationConfigurationManager tWindow in tWindows)
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
            TitleInit(NWDConstants.K_LOCALIZATION_CONFIGURATION_TITLE, typeof(NWDLocalizationConfigurationManager));
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
            //NWDLocalizationConfigurationManagerContent.SharedInstance().OnPreventGUI(EditorGUIUtility.currentViewWidth);
            NWDLocalizationConfigurationManagerContent.SharedInstance().OnPreventGUI(position);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
