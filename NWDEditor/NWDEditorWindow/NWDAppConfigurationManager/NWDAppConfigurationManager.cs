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
using UnityEditor;
using System;
using System.IO;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDAppConfigurationManagerContent : NWDEditorWindowContent
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
        private static NWDAppConfigurationManagerContent _kSharedInstanceContent;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstanceContent"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDAppConfigurationManagerContent SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstanceContent == null)
            {
                _kSharedInstanceContent = new NWDAppConfigurationManagerContent();
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
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_ALERT_SALT_SHORT_ERROR, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDEditorWindow.GenerateCSharpFile();
                }
            }
            // Begin scroll view
            NWDGUILayout.Title("App's configurations");
            //NWDGUILayout.Informations("The settings below modify the compiled binary. Be careful when making changes!");
            //NWDGUILayout.Line();
            _kScrollPosition = GUILayout.BeginScrollView(_kScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            // Launcher
            NWDGUILayout.Section("Launcher");
            NWDGUILayout.SubSection("Launcher configuration");
            NWDGUILayout.Informations("Show result of launcher benchmark in log?");
            NWDAppConfiguration.SharedInstance().LauncherBenchmark = EditorGUILayout.Toggle("Show benchmark", NWDAppConfiguration.SharedInstance().LauncherBenchmark);
            NWDGUILayout.Informations("Coroutine render scene after a specific number of classes wereloaded. More the value is high, more the refresh is down but not fine. More the  value is low, more the refresh is high but the app take more time to finsh the launching because every render take time.");
            //NWDAppConfiguration.SharedInstance().LauncherFaster = EditorGUILayout.IntField("Launcher faster", NWDAppConfiguration.SharedInstance().LauncherFaster);
            NWDAppConfiguration.SharedInstance().LauncherFaster = EditorGUILayout.IntSlider("Launcher faster", NWDAppConfiguration.SharedInstance().LauncherFaster, 1, NWDLauncher.AllNetWorkedDataTypes.Count);
            if (NWDAppConfiguration.SharedInstance().LauncherFaster < 1)
            {
                NWDAppConfiguration.SharedInstance().LauncherFaster = 1;
            }
            // Data tag
            float tMinWidht = 270.0F;
            int tColum = 1;
            NWDGUILayout.Section("Datas tags");
            NWDGUILayout.Informations("Some informations about tags!");
            if (tColum > 1)
            {
                EditorGUILayout.BeginHorizontal();
            }
            EditorGUILayout.BeginVertical(GUILayout.MinWidth(tMinWidht));
            NWDAppConfiguration.SharedInstance().TagList[-1] = "No Tag";
            Dictionary<int, string> tTagList = new Dictionary<int, string>(NWDAppConfiguration.SharedInstance().TagList);
            for (int tI = -1; tI <= NWDAppConfiguration.SharedInstance().TagNumber; tI++)
            {
                if (NWDAppConfiguration.SharedInstance().TagList.ContainsKey(tI) == false)
                {
                    NWDAppConfiguration.SharedInstance().TagList.Add(tI, "tag " + tI.ToString());
                }
                EditorGUI.BeginDisabledGroup(tI < 0 || tI > NWDAppConfiguration.SharedInstance().TagNumberUser);
                string tV = EditorGUILayout.TextField("tag " + tI.ToString(), NWDAppConfiguration.SharedInstance().TagList[tI]);
                tTagList[tI] = tV.Replace("\"", "`");
                EditorGUI.EndDisabledGroup();
            }
            NWDAppConfiguration.SharedInstance().TagList = tTagList;
            EditorGUILayout.EndVertical();
            if (tColum > 1)
            {
                EditorGUILayout.EndHorizontal();
            }
            NWDGUILayout.Section("Cache on compile");
            NWDGUILayout.SubSection("Override max methods in cache");
            NWDAppConfiguration.SharedInstance().OverrideCacheMethodEverywhere = EditorGUILayout.Toggle("Override all", NWDAppConfiguration.SharedInstance().OverrideCacheMethodEverywhere);
            if (NWDAppConfiguration.SharedInstance().OverrideCacheMethodEverywhere == false)
            {
                NWDAppConfiguration.SharedInstance().OverrideCacheMethod = EditorGUILayout.Toggle("Override", NWDAppConfiguration.SharedInstance().OverrideCacheMethod);
                NWDAppConfiguration.SharedInstance().OverrideCacheMethodInPlayMode = EditorGUILayout.Toggle("Override in playmode", NWDAppConfiguration.SharedInstance().OverrideCacheMethodInPlayMode);
            }
            else
            {
                NWDAppConfiguration.SharedInstance().OverrideCacheMethod = true;
                NWDAppConfiguration.SharedInstance().OverrideCacheMethodInPlayMode = true;
            }
            //NWDGUILayout.Section("Slack with editor");
            //NWDGUILayout.SubSection("Webhook URL");
            //NWDAppConfiguration.SharedInstance().SlackWebhookURL = EditorGUILayout.TextField("Webhook URL", NWDAppConfiguration.SharedInstance().SlackWebhookURL);
            //EditorGUI.BeginDisabledGroup(string.IsNullOrEmpty(NWDAppConfiguration.SharedInstance().SlackWebhookURL));
            //if (GUILayout.Button("Test webhook"))
            //{
            //    NWDOperationWebhook.NewMessage("Test webhook integration success!");
            //}
            //EditorGUI.EndDisabledGroup();
            NWDGUILayout.Section("Account");
            NWDGUILayout.SubSection("Account anonymous");
            NWDAppConfiguration.SharedInstance().AnonymousDeviceConnected = EditorGUILayout.ToggleLeft("Anonymous account connected from system device!", NWDAppConfiguration.SharedInstance().AnonymousDeviceConnected);
            NWDAppConfiguration.SharedInstance().PurgeOldAccountDatabase = EditorGUILayout.ToggleLeft("Purge old account", NWDAppConfiguration.SharedInstance().PurgeOldAccountDatabase);
            // Web Services
            NWDGUILayout.Section("Webservices");
            NWDGUILayout.SubSection("Webservices config for all environments");
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
            NWDAppConfiguration.SharedInstance().WebFolder = EditorGUILayout.TextField("Webservice folder", NWDAppConfiguration.SharedInstance().WebFolder);
            NWDAppConfiguration.SharedInstance().TablePrefixe = EditorGUILayout.TextField("Table prefixe in webservice", NWDAppConfiguration.SharedInstance().TablePrefixe);
            int tIndexWS = tWSListUsable.IndexOf(NWDAppConfiguration.SharedInstance().WebBuild);
            tIndexWS = EditorGUILayout.Popup("Webservice active", tIndexWS, tWSListUsableString.ToArray());
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
                foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeList)
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
            NWDAppConfiguration.SharedInstance().AutoDeleteTrashDatas = EditorGUILayout.Toggle("Auto delete trashed datas", NWDAppConfiguration.SharedInstance().AutoDeleteTrashDatas);
            NWDAppConfiguration.SharedInstance().PreloadDatas = EditorGUILayout.Toggle("Preload datas", NWDAppConfiguration.SharedInstance().PreloadDatas);
            NWDAppConfiguration.SharedInstance().BundleDatas = EditorGUILayout.Toggle("Bundle datas", NWDAppConfiguration.SharedInstance().BundleDatas);
            string tDatabasePathEditor = /*NWD.K_StreamingAssets + "/" +*/ NWDDataManager.SharedInstance().DatabaseEditorName();
            NWDGUILayout.SubSection("Databases editor config for all environments");
            EditorGUILayout.LabelField("Editor path ", tDatabasePathEditor);
            EditorGUILayout.LabelField("Editor hash salt", NWDAppConfiguration.SharedInstance().EditorPass);
            EditorGUILayout.LabelField("Editor hash salt A", NWDAppConfiguration.SharedInstance().EditorPassA);
            EditorGUILayout.LabelField("Editor hash salt B", NWDAppConfiguration.SharedInstance().EditorPassB);
            if (NWDDataManager.SharedInstance().IsSecure())
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField("Editor password", NWDAppConfiguration.SharedInstance().GetEditorPass());
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Editor database show path and copy password to clipboard"))
                {
                    NWEClipboard.CopyToClipboard(NWDAppConfiguration.SharedInstance().GetEditorPass());
                    EditorUtility.RevealInFinder(/*NWD.K_Assets + "/" +*/ tDatabasePathEditor);
                    EditorUtility.OpenWithDefaultApp(/*NWD.K_Assets + "/" + */ tDatabasePathEditor);
                    Debug.LogWarning("Editor database path  = " /*+ NWD.K_Assets + "/"*/+ tDatabasePathEditor);
                    Debug.LogWarning("Editor password = " + NWDAppConfiguration.SharedInstance().GetEditorPass());
                }
            }
            else
            {
                if (GUILayout.Button("Editor database show path "))
                {
                    EditorUtility.RevealInFinder(/*NWD.K_Assets + "/" +*/ tDatabasePathEditor);
                    EditorUtility.OpenWithDefaultApp(/*NWD.K_Assets + "/" +*/ tDatabasePathEditor);
                    Debug.LogWarning("Editor database path = " /*+ NWD.K_Assets + "/" */+ tDatabasePathEditor);
                }
            }
            string tDatabasePathBuild = NWD.K_Assets + "/" + NWD.K_StreamingAssets + "/" + NWDDataManager.SharedInstance().DatabaseBuildName();
            NWDGUILayout.SubSection("Databases Build config for all environments");
            EditorGUILayout.LabelField("Build path ", tDatabasePathBuild);
            EditorGUILayout.LabelField("Build hash salt", NWDAppConfiguration.SharedInstance().EditorPass);
            EditorGUILayout.LabelField("Build hash salt A", NWDAppConfiguration.SharedInstance().EditorPassA);
            EditorGUILayout.LabelField("Build hash salt B", NWDAppConfiguration.SharedInstance().EditorPassB);
            if (NWDDataManager.SharedInstance().IsSecure())
            {
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField("Build password", NWDAppConfiguration.SharedInstance().GetEditorPass());
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Build database show path and copy password to clipboard"))
                {
                    NWEClipboard.CopyToClipboard(NWDAppConfiguration.SharedInstance().GetEditorPass());
                    EditorUtility.RevealInFinder( tDatabasePathBuild);
                    EditorUtility.OpenWithDefaultApp( tDatabasePathBuild);
                    Debug.LogWarning("Build database path  = " + tDatabasePathBuild);
                    Debug.LogWarning("Build password = " + NWDAppConfiguration.SharedInstance().GetEditorPass());
                }
            }
            else
            {
                if (GUILayout.Button("Build database show path "))
                {
                    EditorUtility.RevealInFinder( tDatabasePathBuild);
                    EditorUtility.OpenWithDefaultApp( tDatabasePathBuild);
                    Debug.LogWarning("Build database path = " + tDatabasePathBuild);
                }
            }
            if (GUILayout.Button("Copy Build database to Editor Database"))
            {
                if (File.Exists(tDatabasePathEditor))
                {
                    File.Delete(tDatabasePathEditor);
                }
                File.Copy(tDatabasePathBuild, tDatabasePathEditor);
            }
            //Database account informations
            NWDGUILayout.SubSection("Databases accounts config for all environments (by device)");
            EditorGUILayout.LabelField("Account path ", NWDDataManager.SharedInstance().PathDatabaseAccount());
            EditorGUILayout.LabelField("Account hash salt", NWDAppConfiguration.SharedInstance().AccountHashSalt);
            EditorGUILayout.LabelField("Account hash salt A", NWDAppConfiguration.SharedInstance().AccountHashSaltA);
            EditorGUILayout.LabelField("Account hash salt B", NWDAppConfiguration.SharedInstance().AccountHashSaltB);
            if (NWDDataManager.SharedInstance().IsSecure())
            {
                string tAccountPass = NWDAppConfiguration.SharedInstance().GetAccountPass(string.Empty);
                EditorGUI.BeginDisabledGroup(true);
                EditorGUILayout.TextField("Account password", tAccountPass);
                EditorGUI.EndDisabledGroup();
                if (GUILayout.Button("Account database show path and copy password to clipboard"))
                {
                    NWEClipboard.CopyToClipboard(tAccountPass);
                    EditorUtility.RevealInFinder(NWDDataManager.SharedInstance().PathDatabaseAccount());
                    EditorUtility.OpenWithDefaultApp(NWDDataManager.SharedInstance().PathDatabaseAccount());
                    Debug.LogWarning("Account database path = " + NWDDataManager.SharedInstance().PathDatabaseAccount());
                    Debug.LogWarning("Account password = " + tAccountPass);
                }
            }
            else
            {
                if (GUILayout.Button("Account database show path"))
                {
                    EditorUtility.RevealInFinder(NWDDataManager.SharedInstance().PathDatabaseAccount());
                    EditorUtility.OpenWithDefaultApp(NWDDataManager.SharedInstance().PathDatabaseAccount());
                    Debug.LogWarning("Account database path = " + NWDDataManager.SharedInstance().PathDatabaseAccount());
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
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// The <see cref="NWDAppConfigurationManager"/> is an editor window to parameter <see cref="NetWorkedData"/> in the application final compile.
    /// </summary>
    public class NWDAppConfigurationManager : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string TutorialLink(string sLink = "")
        {
            return NWDConstants.K_NWDURL + "app-configuration-manager/";
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The shared instance.
        /// </summary>
        private static NWDAppConfigurationManager _kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the <see cref="_kSharedInstance"/> of <see cref="NWDAppConfigurationManager"/> and focus on.
        /// </summary>
        /// <returns></returns>
        public static NWDAppConfigurationManager SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstance == null)
            {
                //_kSharedInstance = EditorWindow.GetWindow(typeof(NWDAppConfigurationManager)) as NWDAppConfigurationManager;
                _kSharedInstance = EditorWindow.GetWindow(typeof(NWDAppConfigurationManager), ShowAsWindow()) as NWDAppConfigurationManager;
            }
            NWDBenchmark.Finish();
            return _kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Repaint all <see cref="NWDAppConfigurationManager"/>.
        /// </summary>
        public static void Refresh()
        {
            NWDBenchmark.Start();
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDAppConfigurationManager));
            foreach (NWDAppConfigurationManager tWindow in tWindows)
            {
                tWindow.Repaint();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the <see cref="_kSharedInstance"/> of <see cref="NWDAppConfigurationManager"/> and focus on.
        /// </summary>
        /// <returns></returns>
        public static NWDAppConfigurationManager SharedInstanceFocus()
        {
            NWDBenchmark.Start();
            //SharedInstance().ShowUtility();
            SharedInstance().ShowMe();
            SharedInstance().Focus();
            NWDBenchmark.Finish();
            return _kSharedInstance;
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
            NormalizeHeight = 900;
            // set title
            TitleInit(NWDConstants.K_APP_CONFIGURATION_TITLE, typeof(NWDAppConfigurationManager));
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
            NWDAppConfigurationManagerContent.SharedInstance().OnPreventGUI(position);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
