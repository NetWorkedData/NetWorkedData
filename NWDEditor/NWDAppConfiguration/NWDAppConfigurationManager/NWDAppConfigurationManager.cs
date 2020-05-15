//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:9
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using System;
//using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// the App configuration manager window. It's editor window to parameter the NWD App in the project.
    /// </summary>
    public class NWDAppConfigurationManager : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The shared instance.
        /// </summary>
        private static NWDAppConfigurationManager kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The scroll position.
        /// </summary>
        static Vector2 ScrollPosition;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the SharedInstance or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDAppConfigurationManager SharedInstance()
        {
            //NWEBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDAppConfigurationManager)) as NWDAppConfigurationManager;
            }
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Repaint all app configuration manager windows.
        /// </summary>
        public static void Refresh()
        {
            //NWEBenchmark.Start();
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDAppConfigurationManager));
            foreach (NWDAppConfigurationManager tWindow in tWindows)
            {
                tWindow.Repaint();
            }
            //NWEBenchmark.Finish();
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
        /// Show the SharedInstance of app configuration manager window and focus on.
        /// </summary>
        /// <returns></returns>
        public static NWDAppConfigurationManager SharedInstanceFocus()
        {
            //NWEBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// On enable action.
        /// </summary>
        public void OnEnable()
        {
            //NWEBenchmark.Start();
            TitleInit(NWDConstants.K_APP_CONFIGURATION_TITLE, typeof(NWDAppConfigurationManager));
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI()
        {
            //NWEBenchmark.Start();
            NWDGUI.LoadStyles();
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
            // begin scroll view
            NWDGUILayout.Title("App configurations");
            NWDGUILayout.Informations("The settings below modify the compiled binary. Be careful when making changes.!");
            NWDGUILayout.Line();
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            // start interface

            NWDGUILayout.Section("Cache on compile");
            NWDGUILayout.SubSection("Override max methods in cache");
            NWDAppConfiguration.SharedInstance().OverrideCacheMethodEverywhere = EditorGUILayout.Toggle("Override All", NWDAppConfiguration.SharedInstance().OverrideCacheMethodEverywhere);
            if (NWDAppConfiguration.SharedInstance().OverrideCacheMethodEverywhere == false)
            {
                NWDAppConfiguration.SharedInstance().OverrideCacheMethod = EditorGUILayout.Toggle("Override", NWDAppConfiguration.SharedInstance().OverrideCacheMethod);
                NWDAppConfiguration.SharedInstance().OverrideCacheMethodInPlayMode = EditorGUILayout.Toggle("Override in PlayMode", NWDAppConfiguration.SharedInstance().OverrideCacheMethodInPlayMode);
            }
            else
            {
                NWDAppConfiguration.SharedInstance().OverrideCacheMethod = true;
                NWDAppConfiguration.SharedInstance().OverrideCacheMethodInPlayMode = true;
            }

            NWDGUILayout.Section("Slack with Editor");
            NWDGUILayout.SubSection("Webhook URL");
            NWDAppConfiguration.SharedInstance().SlackWebhookURL = EditorGUILayout.TextField("Webhook URL", NWDAppConfiguration.SharedInstance().SlackWebhookURL);


            NWDGUILayout.Section("Account");
            NWDGUILayout.SubSection("Account Anonymous");
            //NWDAppConfiguration.SharedInstance().AnonymousPlayerIsLocal = EditorGUILayout.ToggleLeft("Anonymous account is stored on system device!", NWDAppConfiguration.SharedInstance().AnonymousPlayerIsLocal);
            NWDAppConfiguration.SharedInstance().AnonymousDeviceConnected = EditorGUILayout.ToggleLeft("Anonymous account connected from system device!", NWDAppConfiguration.SharedInstance().AnonymousDeviceConnected);
            NWDAppConfiguration.SharedInstance().PurgeOldAccountDatabase = EditorGUILayout.ToggleLeft("Purge Old Account", NWDAppConfiguration.SharedInstance().PurgeOldAccountDatabase);

            NWDGUILayout.Section("Web Services");
            NWDGUILayout.SubSection("Web Services config for all environments");
            Dictionary<int, bool> tWSList = new Dictionary<int, bool>();
            tWSList.Add(0, true);
            foreach (KeyValuePair<int, bool> tWS in NWDAppConfiguration.SharedInstance().WSList)
            {
                if (tWSList.ContainsKey(tWS.Key) == false)
                {
                    tWSList.Add(tWS.Key, tWS.Value);
                }
            }
            List<int> tWSListUsable = new List<int>();
            List<string> tWSListUsableString = new List<string>();
            foreach (KeyValuePair<int, bool> tWS in tWSList)
            {
                if (tWS.Value == true)
                {
                    tWSListUsable.Add(tWS.Key);
                    tWSListUsableString.Add(NWDAppConfiguration.SharedInstance().WebFolder + "_" + tWS.Key.ToString("0000"));
                }
            }
            NWDAppConfiguration.SharedInstance().WebFolder = EditorGUILayout.TextField("Web Service Folder", NWDAppConfiguration.SharedInstance().WebFolder);
            NWDAppConfiguration.SharedInstance().TablePrefixe = EditorGUILayout.TextField("Table Prefixe in Web Service", NWDAppConfiguration.SharedInstance().TablePrefixe);
            int tIndexWS = tWSListUsable.IndexOf(NWDAppConfiguration.SharedInstance().WebBuild);
            tIndexWS = EditorGUILayout.Popup("Web Service active", tIndexWS, tWSListUsableString.ToArray());
            if (tIndexWS >= 0)
            {
                NWDAppConfiguration.SharedInstance().WebBuild = tWSListUsable[tIndexWS];
            }
            else
            {
                NWDAppConfiguration.SharedInstance().WebBuild = 0;
            }
            foreach (KeyValuePair<int, bool> tWS in tWSList)
            {
                EditorGUI.BeginDisabledGroup(tWS.Key == 0);
                bool tUnused = false;
                foreach (Type tType in NWDDataManager.SharedInstance().mTypeList)
                {
                    if (NWDBasisHelper.FindTypeInfos(tType).WebModelSQLOrder.ContainsKey(tWS.Key) == true)
                    {
                        tUnused = true;
                        break;
                    }
                }
                NWDBasisHelper tDatasToTest = NWDBasisHelper.FindTypeInfos(typeof(NWDParameter));

                if (tUnused == false)
                {
                    bool tV = EditorGUILayout.Toggle("(" + NWDAppConfiguration.SharedInstance().WebFolder + "_" + tWS.Key.ToString("0000") + " unused)", tWS.Value);
                    NWDAppConfiguration.SharedInstance().WSList[tWS.Key] = tV;
                }
                else
                {
                    bool tV = EditorGUILayout.Toggle(NWDAppConfiguration.SharedInstance().WebFolder + "_" + tWS.Key.ToString("0000") + " in config", tWS.Value);
                    NWDAppConfiguration.SharedInstance().WSList[tWS.Key] = tV;
                }
                EditorGUI.EndDisabledGroup();
            }
            // Database informations
            NWDGUILayout.Section("Databases");
            NWDGUILayout.SubSection("Databases parameters");

            //NWDAppConfiguration.SharedInstance().SurProtected = false;
            //NWDAppConfiguration.SharedInstance().SurProtected = EditorGUILayout.Toggle("Overprotected Database", NWDAppConfiguration.SharedInstance().SurProtected);
            //if (NWDAppConfiguration.SharedInstance().SurProtected == true)
            //{
            //    NWDAppConfiguration.SharedInstance().PreloadDatas = false;
            //}
            //EditorGUI.BeginDisabledGroup(!NWDAppConfiguration.SharedInstance().SurProtected);
            //NWDAppConfiguration.SharedInstance().PinCodeLenghtMin = EditorGUILayout.IntField("Min length", NWDAppConfiguration.SharedInstance().PinCodeLenghtMin);
            //NWDAppConfiguration.SharedInstance().PinCodeLenghtMax = EditorGUILayout.IntField("Max length", NWDAppConfiguration.SharedInstance().PinCodeLenghtMax);

            //NWDAppConfiguration.SharedInstance().ProtectionTentativeMax = EditorGUILayout.IntField("Maximum attempt", NWDAppConfiguration.SharedInstance().ProtectionTentativeMax);
            //EditorGUI.EndDisabledGroup();
            //EditorGUI.BeginDisabledGroup(NWDAppConfiguration.SharedInstance().SurProtected);

            NWDAppConfiguration.SharedInstance().AutoDeleteTrashDatas = EditorGUILayout.Toggle("Auto Delete Trash Datas", NWDAppConfiguration.SharedInstance().AutoDeleteTrashDatas);
            NWDAppConfiguration.SharedInstance().PreloadDatas = EditorGUILayout.Toggle("Preload Datas", NWDAppConfiguration.SharedInstance().PreloadDatas);
            NWDAppConfiguration.SharedInstance().BundleDatas = EditorGUILayout.Toggle("Bundle Datas", NWDAppConfiguration.SharedInstance().BundleDatas);
            NWDAppConfiguration.SharedInstance().LauncherBenchmark = EditorGUILayout.Toggle("Launcher Benchmark", NWDAppConfiguration.SharedInstance().LauncherBenchmark);
            NWDAppConfiguration.SharedInstance().LauncherFaster = EditorGUILayout.IntField("Launcher Faster", NWDAppConfiguration.SharedInstance().LauncherFaster);
            if (NWDAppConfiguration.SharedInstance().LauncherFaster < 1)
            {
                NWDAppConfiguration.SharedInstance().LauncherFaster = 1;
            }

            //EditorGUI.EndDisabledGroup();
            NWDAppConfiguration.SharedInstance().RowDataIntegrity = EditorGUILayout.Toggle("Active Row Integrity", NWDAppConfiguration.SharedInstance().RowDataIntegrity);
            // Database editor informations
            string tDatabasePathEditor = NWD.K_StreamingAssets + "/" + NWDDataManager.SharedInstance().DatabaseEditorName();
            NWDGUILayout.SubSection("Databases editor config for all environments");
            EditorGUILayout.LabelField("Editor path ", tDatabasePathEditor);
            EditorGUILayout.LabelField("Editor hash salt", NWDAppConfiguration.SharedInstance().EditorPass);
            EditorGUILayout.LabelField("Editor hash salt A", NWDAppConfiguration.SharedInstance().EditorPassA);
            EditorGUILayout.LabelField("Editor hash salt B", NWDAppConfiguration.SharedInstance().EditorPassB);
            if (NWDDataManager.SharedInstance().IsSecure())
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField("Editor pass result", NWDAppConfiguration.SharedInstance().GetEditorPass());
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Editor Database File and password copy to Clipboard"))
                {
                    NWEClipboard.CopyToClipboard(NWDAppConfiguration.SharedInstance().GetEditorPass());
                    EditorUtility.RevealInFinder(NWD.K_Assets+"/" + tDatabasePathEditor);
                    EditorUtility.OpenWithDefaultApp(NWD.K_Assets+"/" + tDatabasePathEditor);
                    Debug.LogWarning("DatabasePathEditor = "+NWD.K_Assets+"/" + tDatabasePathEditor);
                    Debug.LogWarning("Editor pass result = " + NWDAppConfiguration.SharedInstance().GetEditorPass());
                }
            }
            else
            {
                if (GUILayout.Button("Editor Database File "))
                {
                    EditorUtility.RevealInFinder(NWD.K_Assets+"/" + tDatabasePathEditor);
                    EditorUtility.OpenWithDefaultApp(NWD.K_Assets+"/" + tDatabasePathEditor);
                    Debug.LogWarning("DatabasePathEditor = "+ NWD.K_Assets + "/" + tDatabasePathEditor);
                }
            }
            // Database account informations
            NWDGUILayout.SubSection("Databases accounts config for all environments (by device)");
            EditorGUILayout.LabelField("Account path ", NWDDataManager.SharedInstance().PathDatabaseAccount());
            EditorGUILayout.LabelField("Account hash salt", NWDAppConfiguration.SharedInstance().AccountHashSalt);
            EditorGUILayout.LabelField("Account hash salt A", NWDAppConfiguration.SharedInstance().AccountHashSaltA);
            EditorGUILayout.LabelField("Account hash salt B", NWDAppConfiguration.SharedInstance().AccountHashSaltB);

            if (NWDDataManager.SharedInstance().IsSecure())
            {
                string tAccountPass = NWDAppConfiguration.SharedInstance().GetAccountPass(string.Empty);
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField("Account pass result", tAccountPass);
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Account Database File and password copy to Clipboard"))
                {
                    NWEClipboard.CopyToClipboard(tAccountPass);
                    EditorUtility.RevealInFinder(NWDDataManager.SharedInstance().PathDatabaseAccount());
                    EditorUtility.OpenWithDefaultApp(NWDDataManager.SharedInstance().PathDatabaseAccount());
                    Debug.LogWarning("DatabasePathAccount = " + NWDDataManager.SharedInstance().PathDatabaseAccount());
                    Debug.LogWarning("Account pass result = " + tAccountPass);
                }
            }
            else
            {
                if (GUILayout.Button("Account Database File"))
                {
                    EditorUtility.RevealInFinder(NWDDataManager.SharedInstance().PathDatabaseAccount());
                    EditorUtility.OpenWithDefaultApp(NWDDataManager.SharedInstance().PathDatabaseAccount());
                    Debug.LogWarning("DatabasePathAccount = " + NWDDataManager.SharedInstance().PathDatabaseAccount());

                }

            }
            NWDGUILayout.LittleSpace();
            // finish scroll view
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
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
