//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAppEnvironmentChooser : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
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
        public static void SharedInstanceFocus()
        {
            //NWEBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //NWEBenchmark.Finish();
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
            TitleInit(NWDConstants.K_APP_CHOOSER_ENVIRONMENT_TITLE, typeof(NWDAppEnvironmentChooser));
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

            NWDGUILayout.Section("DNS bypass");
            EditorGUILayout.LabelField("DNS use", NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetServerDNS());
            List<NWDServerDomain> tServerDNSList = NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetServerDNSList();
            List<string> tServerList = new List<string>();
            foreach (NWDServerDomain tDomain in tServerDNSList)
            {
                tServerList.Add(tDomain.InternalKey);
            }
            //int tIndex = tServerDNSList.IndexOf(NWDAccountInfos.CurrentData().Server.GetReachableData());
            int tIndex = tServerList.IndexOf(NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetServerDNS());
            // If account not temporary DNS Can be bypassed
            bool tDNSSelectable = NWDAccountInfos.CurrentData() == null;
            EditorGUI.BeginDisabledGroup(tDNSSelectable);
            int tNewIndex = EditorGUILayout.Popup("DNS", tIndex, tServerList.ToArray());
            if (tNewIndex != tIndex)
            {
                //if (tNewIndex<0)
                //{
                //    tNewIndex = 0;
                //}
                //Debug.Log("DNS Changed!");
                //NWDAccountInfos.CurrentData().Server.SetData(tServerDNSList[tNewIndex]);
                //NWDAccountInfos.CurrentData().SaveData();
            }
            EditorGUI.EndDisabledGroup();

            NWDGUILayout.Section("Compile bypass");
            EditorGUILayout.LabelField("Compile use", NWDLauncher.CompileAs().ToString());
            NWDCompileTypeBypass tByPass = (NWDCompileTypeBypass)EditorGUILayout.EnumPopup("ByPass mode", NWDLauncher.kByPass);
            if (tByPass != NWDLauncher.kByPass)
            {
                NWDLauncher.kByPass = tByPass;
                NWDProjectConfigurationManager.Refresh();
                NWDAppConfigurationManager.Refresh();
                NWDAppEnvironmentConfigurationManager.Refresh();
                NWDModelManager.Refresh();
                NWDAppEnvironmentSync.Refresh();
            }

            NWDGUILayout.Section("Environment");
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
                NWDProjectPrefs.SetInt(NWDAppConfiguration.kEnvironmentSelectedKey, tTabSelect);
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
            if (NWDAccountInfos.CurrentData() != null)
            {
                EditorGUILayout.LabelField("Account Infos used", NWDAccountInfos.CurrentData().Reference);
            }
            else
            {
                EditorGUILayout.LabelField("Account Infos used", "ERROR NO ACCOUNT INFOS");
            }


            if (NWDAppConfiguration.SharedInstance().SelectedEnvironment().PlayerAccountReference.Contains(NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE) == false)
            {
                if (GUILayout.Button("Reset session"))
                {
                    NWDAppEnvironmentSync.SharedInstance().Reset(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
            }

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
                    foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeLoadedList)
                    {
                        NWDBasisHelper.FindTypeInfos(tType).m_SearchAccount = tAccount.Reference;
                        NWDDataManager.SharedInstance().RepaintWindowsInManager(tType);
                    }
                }
                NWDGUILayout.SubSection("Account informations");
                string tAccountInfosReference = "?";

                if (NWDAccountInfos.CurrentData() != null)
                {
                    EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOUNTINFOS_REFERENCE, NWDAccountInfos.CurrentData().Reference);
                    if (GUILayout.Button(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOUNTINFOS_SELECT))
                    {
                        NWDDataInspector.InspectNetWorkedData(NWDAccountInfos.CurrentData(), true, true);
                    }
                }
                else
                {
                    EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOUNTINFOS_REFERENCE, "ERROR NO ACCOUNT INFOS");
                }

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
                        foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeLoadedList)
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
