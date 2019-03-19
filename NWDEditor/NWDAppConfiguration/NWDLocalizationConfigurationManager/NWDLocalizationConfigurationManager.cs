//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
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
    public class NWDLocalizationConfigurationManager : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        private static NWDLocalizationConfigurationManager kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The icon and title.
        /// </summary>
        GUIContent IconAndTitle;
        /// <summary>
        /// The scroll position.
        /// </summary>
        static Vector2 ScrollPosition;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDLocalizationConfigurationManager SharedInstance()
        {
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDLocalizationConfigurationManager)) as NWDLocalizationConfigurationManager;
            }
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDLocalizationConfigurationManager SharedInstanceFocus()
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
                IconAndTitle.text = NWDConstants.K_LOCALIZATION_CONFIGURATION_TITLE;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets("NWDLocalizationConfigurationManager t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals("NWDLocalizationConfigurationManager"))
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
            NWDGUILayout.Title(NWDConstants.K_APP_CONFIGURATION_LANGUAGE_AREA);
            NWDGUILayout.Informations("Some informations");
            NWDGUILayout.Line();
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDConstants.kInspectorFullWidthMargins, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            NWDGUILayout.Section(NWDConstants.K_APP_CONFIGURATION_LANGUAGE_AREA);
            Dictionary<string, string> tLanguageDico = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguageDico;
            NWDGUILayout.Informations("Select the languages of multi-localizable field in editor.");
            GUILayout.BeginHorizontal();
            List<string> tResult = new List<string>();
            float tToggleWidth = 140.0f;
            int tColunm = 0;
            float tWidthUsed = EditorGUIUtility.currentViewWidth;
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
            GUILayout.Label("Experimental : localizate the bundle IOS and MacOS app name.", EditorStyles.helpBox);
            foreach (string tLang in tResult)
            {
                if (NWDAppConfiguration.SharedInstance().BundleName.ContainsKey(tLang) == false)
                {
                    NWDAppConfiguration.SharedInstance().BundleName.Add(tLang, string.Empty);
                }
                NWDAppConfiguration.SharedInstance().BundleName[tLang] = EditorGUILayout.TextField(tLang, NWDAppConfiguration.SharedInstance().BundleName[tLang]);
            }
            NWDGUILayout.Section(NWDConstants.K_APP_CONFIGURATION_DEV_LOCALALIZATION_AREA);
            GUILayout.Label("Select the default language of the app.", EditorStyles.helpBox);
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
            GUILayout.Label("Reorder all localizations for all datas (to see the same order in all datas).", EditorStyles.helpBox);
            if (GUILayout.Button("Reorder all localizations"))
            {
                NWDAppConfiguration.SharedInstance().DataLocalizationManager.ReOrderAllLocalizations();
            }

            GUILayout.Label("Export all localizations in CSV's file to send to translate.", EditorStyles.helpBox);
            if (GUILayout.Button("Export localizations in CSV's file"))
            {
                NWDAppConfiguration.SharedInstance().DataLocalizationManager.ExportToCSV();
            }

            GUILayout.Label("Import all localizations translated from CSV's file.", EditorStyles.helpBox);
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
