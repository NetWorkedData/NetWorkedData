//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using UnityEngine;
using System.IO;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAppEnvironmentChooser : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The icon and title.
        /// </summary>
        GUIContent IconAndTitle;
        Vector2 ScrollPosition;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        public static NWDAppEnvironmentChooser kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentChooser SharedInstance()
        {
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDAppEnvironmentChooser)) as NWDAppEnvironmentChooser;
            }
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentChooser SharedInstanceFocus()
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
                IconAndTitle.text = NWDConstants.K_APP_CHOOSER_ENVIRONMENT_TITLE;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets("NWDAppEnvironmentChooser t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals("NWDAppEnvironmentChooser"))
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
            NWDGUI.LoadStyles();

            NWDGUILayout.Title("Environment chooser");

            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            this.minSize = new Vector2(300, 150);
            this.maxSize = new Vector2(300, 4096);
            int tTabSelected = -1;
            if (NWDAppConfiguration.SharedInstance().DevEnvironment.Selected == true)
            {
                tTabSelected = 0;
            }
            if (NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected == true)
            {
                tTabSelected = 1;
            }
            if (NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected == true)
            {
                tTabSelected = 2;
            }
            string[] tTabList = new string[3] {
                NWDConstants.K_APP_CONFIGURATION_DEV,
                NWDConstants.K_APP_CONFIGURATION_PREPROD,
                NWDConstants.K_APP_CONFIGURATION_PROD
            };
            int tTabSelect = GUILayout.Toolbar(tTabSelected, tTabList);
            if (tTabSelect != tTabSelected)
            {
                GUI.FocusControl(null);
                EditorPrefs.SetInt(NWDAppConfiguration.kEnvironmentSelectedKey, tTabSelect);
                switch (tTabSelect)
                {
                    case 0:
                        {
                            NWDAppConfiguration.SharedInstance().DevEnvironment.Selected = true;
                            NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected = false;
                            NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected = false;
                        }
                        break;
                    case 1:
                        {
                            NWDAppConfiguration.SharedInstance().DevEnvironment.Selected = false;
                            NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected = true;
                            NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected = false;
                        }
                        break;
                    case 2:
                        {
                            NWDAppConfiguration.SharedInstance().DevEnvironment.Selected = false;
                            NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected = false;
                            NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected = true;
                        }
                        break;
                }
                NWDVersion.UpdateVersionBundle();
                if (NWDAppEnvironmentSync.IsSharedInstance())
                {
                    NWDAppEnvironmentSync.SharedInstance().Repaint();
                }
            }
            // Show version selected
            EditorGUILayout.LabelField("Webservice version", NWDAppConfiguration.SharedInstance().WebBuild.ToString());
            EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_VERSION_BUNDLE, PlayerSettings.bundleVersion, EditorStyles.label);
            EditorGUILayout.LabelField("Account used", NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference);
            NWDAccount tAccount = null;
            string tAccountReference = NWDAccount.CurrentReference();
            if (NWDAccount.BasisHelper().DatasByReference.ContainsKey(tAccountReference))
            {
                tAccount = NWDAccount.BasisHelper().DatasByReference[tAccountReference] as NWDAccount;
            }
            if (tAccount != null)
            {
                NWDGUILayout.SubSection("Account");
                EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_REFERENCE, tAccount.Reference);
                EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_INTERNALKEY, tAccount.InternalKey);
                if (GUILayout.Button(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_SELECT))
                {
                    NWDDataInspector.InspectNetWorkedData(tAccount, true, true);
                }
                if (GUILayout.Button(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_FILTER))
                {
                    foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList)
                    {
                        NWDBasisHelper.FindTypeInfos(tType).m_SearchAccount = tAccount.Reference;
                        NWDDataManager.SharedInstance().RepaintWindowsInManager(tType);
                    }
                }
                NWDGUILayout.SubSection("Account informations");
                string tAccountInfosReference = "?";
                if (NWDAccountInfos.GetFirstData(NWDAccount.CurrentReference(), null) != null)
                {
                    NWDAccountInfos tAccountInfos = NWDAccountInfos.GetFirstData(NWDAccount.CurrentReference(), null);
                    tAccountInfosReference = tAccountInfos.Reference;
                    EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOUNTINFOS_REFERENCE, tAccountInfosReference);

                    if (GUILayout.Button(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOUNTINFOS_SELECT))
                    {
                        NWDDataInspector.InspectNetWorkedData(tAccountInfos, true, true);
                    }
                }
                else
                {
                    EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOUNTINFOS_REFERENCE, "ERROR NO ACCOUNT INFOS");
                }
                NWDGUILayout.SubSection("Gamse save informations");
                string tGameSaveReference = "?";
                if (NWDGameSave.CurrentByAccount(tAccount.Reference) != null)
                {
                    NWDGameSave tGameSave = NWDGameSave.CurrentByAccount(tAccount.Reference);
                    tGameSaveReference = tGameSave.Reference;
                    EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_GAMESAVE_REFERENCE, tGameSaveReference);
                    if (GUILayout.Button(NWDConstants.K_ENVIRONMENT_CHOOSER_GAMESAVE_FILTER))
                    {
                        foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList)
                        {
                            NWDBasisHelper.FindTypeInfos(tType).m_SearchGameSave = tGameSaveReference;
                            NWDDataManager.SharedInstance().RepaintWindowsInManager(tType);
                        }
                    }
                    if (GUILayout.Button(NWDConstants.K_ENVIRONMENT_CHOOSER_GAMESAVE_SELECT))
                    {
                        NWDDataInspector.InspectNetWorkedData(tGameSave, true, true);
                    }
                }
                else
                {
                    EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_GAMESAVE_REFERENCE, "ERROR NO GAMESAVE");
                }
                NWDGUILayout.BigSpace();
            }
            GUILayout.EndScrollView();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
