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
using System;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAppEnvironmentChooserContent : NWDEditorWindowContent
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
        private static NWDAppEnvironmentChooserContent _kSharedInstanceContent;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstanceContent"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDAppEnvironmentChooserContent SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstanceContent == null)
            {
                _kSharedInstanceContent = new NWDAppEnvironmentChooserContent();
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
            NWDGUILayout.Title("Environment chooser");
            // Section Compile bypass
            NWDGUILayout.Section("Compile bypass");
            EditorGUILayout.LabelField("Compile used", NWDLauncher.CompileAs().ToString());
            NWDCompileTypeBypass tByPass = (NWDCompileTypeBypass)EditorGUILayout.EnumPopup("Bypass mode", NWDLauncher.kByPass);
            if (tByPass != NWDLauncher.kByPass)
            {
                NWDLauncher.kByPass = tByPass;
                NWDProjectConfigurationManager.Refresh();
                NWDAppConfigurationManager.Refresh();
                NWDAppEnvironmentConfigurationManager.Refresh();
                NWDModelManager.Refresh();
                NWDAppEnvironmentSync.Refresh();
            }

            // Section Environments
            NWDGUILayout.Section("Environments");
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
                            NWDAccountInfos.ResetCurrentData();
                        }
                        break;
                    case 1:
                        {
                            NWDAppConfiguration.SharedInstance().DevEnvironment.Selected = false;
                            NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected = true;
                            NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected = false;
                            NWDAccountInfos.ResetCurrentData();
                        }
                        break;
                    case 2:
                        {
                            NWDAppConfiguration.SharedInstance().DevEnvironment.Selected = false;
                            NWDAppConfiguration.SharedInstance().PreprodEnvironment.Selected = false;
                            NWDAppConfiguration.SharedInstance().ProdEnvironment.Selected = true;
                            NWDAccountInfos.ResetCurrentData();
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
            _kScrollPosition = GUILayout.BeginScrollView(_kScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            //NWDBenchmark.Step();

            // Section DNS bypass
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
            if (tIndex < 0)
            {
                if (tServerDNSList.Count > 1)
                {
                    NWDAccountInfos.CurrentData().Server.SetData(tServerDNSList[0]);
                }
                else
                {
                    NWDAccountInfos.CurrentData().Server.SetData(null);
                }
                NWDAccountInfos.CurrentData().SaveData();
            }
            //NWDBenchmark.Step();

            bool tDNSSelectable = NWDAccountInfos.CurrentData() == null;
            EditorGUI.BeginDisabledGroup(tDNSSelectable);
            int tNewIndex = EditorGUILayout.Popup("DNS", tIndex, tServerList.ToArray());
            if (tNewIndex != tIndex)
            {
                if (tNewIndex < 0)
                {
                    tNewIndex = 0;
                }
                Debug.Log("DNS Changed for " + tServerDNSList[tNewIndex].ServerDNS + " !");
                NWDAccountInfos.CurrentData().Server.SetData(tServerDNSList[tNewIndex]);
                NWDAccountInfos.CurrentData().SaveData();
            }
            EditorGUI.EndDisabledGroup();

            //NWDBenchmark.Step();

            // Section WebServices informations
            NWDGUILayout.Section("Webservices informations");

            // Show version selected
            EditorGUILayout.LabelField("Webservice version", NWDAppConfiguration.SharedInstance().WebBuild.ToString());
            EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_VERSION_BUNDLE, PlayerSettings.bundleVersion, EditorStyles.label);

            // Section Devices informations
            NWDGUILayout.Section("Devices informations");
            EditorGUILayout.LabelField("Device ID", SystemInfo.deviceUniqueIdentifier, EditorStyles.label);
            EditorGUILayout.LabelField("Secret key used", NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevice(), EditorStyles.label);
            EditorGUILayout.LabelField("Secret key editor", NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDeviceEditor(), EditorStyles.label);
            EditorGUILayout.LabelField("Secret key player", NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevicePlayer(), EditorStyles.label);
            EditorGUILayout.LabelField("Account used", NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetAccountReference());
            if (NWDAccountInfos.CurrentData() != null)
            {
                EditorGUILayout.LabelField("Account infos used", NWDAccountInfos.CurrentData().Reference);
            }
            else
            {
                EditorGUILayout.LabelField("Account infos used", "ERROR NO ACCOUNT INFOS");
            }
            if (NWDAppConfiguration.SharedInstance().SelectedEnvironment().GetAccountReference().Contains(NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE) == false)
            {
                if (GUILayout.Button("Reset session"))
                {
                    NWDAppEnvironmentSyncContent.SharedInstance().Reset(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
            }
            NWDAccount tAccount = null;
            string tAccountReference = NWDAccount.CurrentReference();
            if (NWDBasisHelper.BasisHelper<NWDAccount>().DatasByReference.ContainsKey(tAccountReference))
            {
                tAccount = NWDBasisHelper.BasisHelper<NWDAccount>().DatasByReference[tAccountReference] as NWDAccount;
            }

            NWDGUILayout.SubSection("Account");
            EditorGUILayout.LabelField(NWDConstants.K_ENVIRONMENT_CHOOSER_ACCOOUNT_REFERENCE, tAccountReference);
            if (tAccount != null)
            {
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
            }
            else
            {
            }

            NWDGUILayout.SubSection("Account informations");
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

            NWDGUILayout.SubSection("Gamsesave informations");
            string tGameSaveReference = "?";
            if (NWDGameSave.SelectCurrentDataForAccount(tAccountReference) != null)
            {
                NWDGameSave tGameSave = NWDGameSave.SelectCurrentDataForAccount(tAccountReference);
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
            GUILayout.EndScrollView();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAppEnvironmentChooser : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string TutorialLink(string sLink = "")
        {
            return NWDConstants.K_NWDURL + "app-environment-chooser/";
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance for deamon class.
        /// </summary>
        private static NWDAppEnvironmentChooser _kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstance"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDAppEnvironmentChooser SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstance == null)
            {
                _kSharedInstance = EditorWindow.GetWindow(typeof(NWDAppEnvironmentChooser), ShowAsWindow()) as NWDAppEnvironmentChooser;
            }
            NWDBenchmark.Finish();
            return _kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the <see cref="_kSharedInstance"/> of <see cref="NWDAppEnvironmentChooser"/> and focus on.
        /// </summary>
        /// <returns></returns>
        public static void SharedInstanceFocus()
        {
            NWDBenchmark.Start();
            //SharedInstance().ShowUtility();
            SharedInstance().ShowMe();
            SharedInstance().Focus();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Repaint all <see cref="NWDAppEnvironmentChooser"/>.
        /// </summary>
        public static void Refresh()
        {
            NWDBenchmark.Start();
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDAppEnvironmentChooser));
            foreach (NWDAppEnvironmentChooser tWindow in tWindows)
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
            NormalizeHeight = 780;
            TitleInit(NWDConstants.K_APP_CHOOSER_ENVIRONMENT_TITLE, typeof(NWDAppEnvironmentChooser));
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
            NWDAppEnvironmentChooserContent.SharedInstance().OnPreventGUI(position);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
