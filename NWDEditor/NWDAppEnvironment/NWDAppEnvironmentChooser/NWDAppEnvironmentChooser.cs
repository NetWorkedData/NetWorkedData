﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:16
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
    public class NWDAppEnvironmentChooser : NWDEditorWindow
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
            //NWEBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDAppEnvironmentChooser)) as NWDAppEnvironmentChooser;
            }
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAppEnvironmentChooser SharedInstanceFocus()
        {
            //NWEBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Refresh()
        {
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDAppEnvironmentChooser));
            foreach (NWDAppEnvironmentChooser tWindow in tWindows)
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
        public void OnEnable()
        {
            //NWEBenchmark.Start();
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDConstants.K_APP_CHOOSER_ENVIRONMENT_TITLE;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets(typeof(NWDAppEnvironmentChooser).Name + " t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals(typeof(NWDAppEnvironmentChooser).Name))
                        {
                            IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        }
                    }
                }
                titleContent = IconAndTitle;
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            //NWEBenchmark.Start();
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
            string[] tTabList = {
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
                NWDDataInspector.Refresh();
            }
            // Show version selected
            EditorGUILayout.LabelField("Webservice version", NWDAppConfiguration.SharedInstance().WebBuild.ToString());
            EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_VERSION_BUNDLE, PlayerSettings.bundleVersion, EditorStyles.label);
            //SystemInfo.deviceUniqueIdentifier
            EditorGUILayout.LabelField("Device ID", SystemInfo.deviceUniqueIdentifier, EditorStyles.label);
            EditorGUILayout.LabelField("Secret Key used", NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevice(), EditorStyles.label);
            EditorGUILayout.LabelField("Secret Key editor", NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDeviceEditor(), EditorStyles.label);
            EditorGUILayout.LabelField("Secret Key player", NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevicePlayer(), EditorStyles.label);
            EditorGUILayout.LabelField("Account used", NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference);
            NWDAccount tAccount = null;
            string tAccountReference = NWDAccount.CurrentReference();
            if (NWDBasisHelper.BasisHelper<NWDAccount>().DatasByReference.ContainsKey(tAccountReference))
            {
                tAccount = NWDBasisHelper.BasisHelper<NWDAccount>().DatasByReference[tAccountReference] as NWDAccount;
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
                if (NWDBasisHelper.GetCorporateFirstData<NWDAccountInfos>(NWDAccount.CurrentReference(), null) != null)
                {
                    NWDAccountInfos tAccountInfos = NWDBasisHelper.GetCorporateFirstData<NWDAccountInfos>(NWDAccount.CurrentReference(), null);
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
                if (NWDGameSave.SelectCurrentDataForAccount(tAccount.Reference) != null)
                {
                    NWDGameSave tGameSave = NWDGameSave.SelectCurrentDataForAccount(tAccount.Reference);
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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif