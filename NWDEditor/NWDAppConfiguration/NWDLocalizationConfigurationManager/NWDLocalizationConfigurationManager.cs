//=====================================================================================================================
//
// ideMobi copyright 2017 
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
	public class NWDLocalizationConfigurationManager : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        GUIContent IconAndTitle;
        static Vector2 ScrollPosition;
        //-------------------------------------------------------------------------------------------------------------
        public NWDLocalizationConfigurationManager()
        {
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
        public static NWDLocalizationConfigurationManager SharedInstance()
        {
            NWDEditorMenu.DataLocalizationManager();
            return NWDEditorMenu.kNWDDataLocalizationManager;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnGUI()
        {
            NWDConstants.LoadStyles();


            // Draw interface for language chooser
            Dictionary<string, string> tLanguageDico = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguageDico;
            //GUILayout.Label(NWDConstants.K_APP_CONFIGURATION_LANGUAGE_AREA, EditorStyles.helpBox);

            GUILayout.Label(NWDConstants.K_APP_CONFIGURATION_LANGUAGE_AREA, NWDConstants.kLabelTitleStyle);

            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, EditorStyles.inspectorFullWidthMargins, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
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
            GUILayout.Label(NWDConstants.K_APP_CONFIGURATION_BUNDLENAMEE_AREA, NWDConstants.kLabelTitleStyle);

            foreach (string tLang in tResult)
            {
                if (NWDAppConfiguration.SharedInstance().BundleName.ContainsKey(tLang) == false)
                {
                    NWDAppConfiguration.SharedInstance().BundleName.Add(tLang, string.Empty);
                }
                NWDAppConfiguration.SharedInstance().BundleName[tLang] = EditorGUILayout.TextField(tLang, NWDAppConfiguration.SharedInstance().BundleName[tLang]);
            }

            GUILayout.Label(NWDConstants.K_APP_CONFIGURATION_DEV_LOCALALIZATION_AREA, NWDConstants.kLabelTitleStyle);
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

            GUILayout.Label("Special localizations operations", NWDConstants.kLabelTitleStyle);
            if (GUILayout.Button("Reoder all localizations"))
            {
                NWDAppConfiguration.SharedInstance().DataLocalizationManager.ReOrderAllLocalizations();
            }
            if (GUILayout.Button("Export localizations in CSV's file"))
            {
                NWDAppConfiguration.SharedInstance().DataLocalizationManager.ExportToCSV();
            }
            if (GUILayout.Button("Import localizations from CSV's file"))
            {
                NWDAppConfiguration.SharedInstance().DataLocalizationManager.ImportFromCSV();
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
