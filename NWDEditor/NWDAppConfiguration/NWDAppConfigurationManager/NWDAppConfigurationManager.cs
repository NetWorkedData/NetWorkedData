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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// the App configuration manager window. It's editor window to parameter the NWD App in the project.
    /// </summary>
    public class NWDAppConfigurationManager : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The shared instance.
        /// </summary>
        private static NWDAppConfigurationManager kSharedInstance;
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
        /// <summary>
        /// Returns the SharedInstance or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDAppConfigurationManager SharedInstance()
        {
            //BTBBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDAppConfigurationManager)) as NWDAppConfigurationManager;
            }
            //BTBBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Repaint all app configuration manager windows.
        /// </summary>
        public static void Refresh()
        {
            //BTBBenchmark.Start();
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDAppConfigurationManager));
            foreach (NWDAppConfigurationManager tWindow in tWindows)
            {
                tWindow.Repaint();
            }
            //BTBBenchmark.Finish();
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
            //BTBBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //BTBBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// On enable action.
        /// </summary>
        public void OnEnable()
        {
            //BTBBenchmark.Start();
            // Init Title and icon 
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDConstants.K_APP_CONFIGURATION_TITLE;
                if (IconAndTitle.image == null)
                {
                    
                    string[] sGUIDs = AssetDatabase.FindAssets(typeof(NWDAppConfigurationManager).Name+" t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals(typeof(NWDAppConfigurationManager).Name))
                        {
                            IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        }
                    }
                }
                titleContent = IconAndTitle;
            }
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public void OnGUI()
        {
            //BTBBenchmark.Start();
            NWDGUI.LoadStyles();
            // Draw warning if salt for class is false
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_ALERT_SALT_SHORT_ERROR, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
            }
            // begin scroll view
            NWDGUILayout.Title("App configurations");
            NWDGUILayout.Informations("The settings below modify the compiled binary. Be careful when making changes.!");
            NWDGUILayout.Line();
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            // start interface
            NWDGUILayout.Section("Account");
            NWDGUILayout.SubSection("Account Anonymous");
            //NWDAppConfiguration.SharedInstance().AnonymousPlayerIsLocal = EditorGUILayout.ToggleLeft("Anonymous account is stored on system device!", NWDAppConfiguration.SharedInstance().AnonymousPlayerIsLocal);
            NWDAppConfiguration.SharedInstance().AnonymousDeviceConnected = EditorGUILayout.ToggleLeft("Anonymous account connected from system device!", NWDAppConfiguration.SharedInstance().AnonymousDeviceConnected);

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
                NWDBasisHelper tDatasToTest = NWDBasisHelper.FindTypeInfos(typeof(NWDParameter));
                if (tDatasToTest.WebModelSQLOrder.ContainsKey(tWS.Key) == false)
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
            NWDAppConfiguration.SharedInstance().SurProtected = EditorGUILayout.Toggle("Overprotected Database", NWDAppConfiguration.SharedInstance().SurProtected);
            if (NWDAppConfiguration.SharedInstance().SurProtected == true)
            {
                NWDAppConfiguration.SharedInstance().PreloadDatas = false;
            }
            EditorGUI.BeginDisabledGroup(!NWDAppConfiguration.SharedInstance().SurProtected);
            NWDAppConfiguration.SharedInstance().PinCodeLenghtMin = EditorGUILayout.IntField("Min length", NWDAppConfiguration.SharedInstance().PinCodeLenghtMin);
            NWDAppConfiguration.SharedInstance().PinCodeLenghtMax = EditorGUILayout.IntField("Max length", NWDAppConfiguration.SharedInstance().PinCodeLenghtMax);

            NWDAppConfiguration.SharedInstance().ProtectionTentativeMax = EditorGUILayout.IntField("Maximum attempt", NWDAppConfiguration.SharedInstance().ProtectionTentativeMax);
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(NWDAppConfiguration.SharedInstance().SurProtected);
            NWDAppConfiguration.SharedInstance().PreloadDatas = EditorGUILayout.Toggle("Preload Datas", NWDAppConfiguration.SharedInstance().PreloadDatas);
            EditorGUI.EndDisabledGroup();
            NWDAppConfiguration.SharedInstance().RowDataIntegrity = EditorGUILayout.Toggle("Active Row Integrity", NWDAppConfiguration.SharedInstance().RowDataIntegrity);
            // Database editor informations
            string tDatabasePathEditor = NWDDataManager.SharedInstance().DatabasePathEditor + "/" + NWDDataManager.SharedInstance().DatabaseNameEditor;
            NWDGUILayout.SubSection("Databases editor config for all environments");
            EditorGUILayout.LabelField("Editor path ", tDatabasePathEditor);
            EditorGUILayout.LabelField("Editor pass", NWDAppConfiguration.SharedInstance().EditorPass);
            EditorGUILayout.LabelField("Editor pass A", NWDAppConfiguration.SharedInstance().EditorPassA);
            EditorGUILayout.LabelField("Editor pass B", NWDAppConfiguration.SharedInstance().EditorPassB);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Editor pass result", NWDAppConfiguration.SharedInstance().GetEditorPass());
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button("Editor Database File"))
            {
                EditorUtility.RevealInFinder("Assets/" + tDatabasePathEditor);
                Debug.LogWarning("DatabasePathEditor = Assets/" + tDatabasePathEditor);
                Debug.LogWarning("Editor pass result = " + NWDAppConfiguration.SharedInstance().GetEditorPass());
            }
            // Database account informations
            NWDGUILayout.SubSection("Databases accounts config for all environments (by device)");
            EditorGUILayout.LabelField("Account path ", NWDDataManager.SharedInstance().PathDatabaseAccount());
            EditorGUILayout.LabelField("Account hash salt", NWDAppConfiguration.SharedInstance().AccountHashSalt);
            EditorGUILayout.LabelField("Account hash salt A", NWDAppConfiguration.SharedInstance().AccountHashSaltA);
            EditorGUILayout.LabelField("Account hash salt B", NWDAppConfiguration.SharedInstance().AccountHashSaltB);
            EditorGUI.BeginDisabledGroup(true);
            if (EditorPrefs.HasKey(NWDLauncher.K_PINCODE_KEY))
            {
                EditorGUILayout.TextField("Account hash result", NWDAppConfiguration.SharedInstance().GetAccountPass(EditorPrefs.GetString(NWDLauncher.K_PINCODE_KEY)));
            }
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button("Account Database File"))
            {
                EditorUtility.RevealInFinder(NWDDataManager.SharedInstance().PathDatabaseAccount());
                Debug.LogWarning("DatabasePathAccount = " + NWDDataManager.SharedInstance().PathDatabaseAccount());
                if (EditorPrefs.HasKey(NWDLauncher.K_PINCODE_KEY))
                {
                    Debug.LogWarning("Account pass result = " + NWDAppConfiguration.SharedInstance().GetAccountPass(EditorPrefs.GetString(NWDLauncher.K_PINCODE_KEY)));
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
                NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            }
            NWDGUI.EndRedArea();
            NWDGUILayout.BigSpace();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
