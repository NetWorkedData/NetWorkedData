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
    /// <summary>
    /// the App configuration manager window. It's editor window to parameter the NWD App in the project.
    /// </summary>
    public class NWDAppConfigurationManager : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
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
        public static NWDAppConfigurationManager SharedInstance()
        {
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDAppConfigurationManager)) as NWDAppConfigurationManager;
            }
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
        public static NWDAppConfigurationManager SharedInstanceFocus()
        {
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            // Init Title and icon 
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDConstants.K_APP_CONFIGURATION_TITLE;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets("NWDAppConfigurationManager t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals("NWDAppConfigurationManager"))
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
            // Draw warning if salt for class is false
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.kAlertSaltShortError, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
            }
            // begin scroll view
            NWDGUILayout.Title("App configurations");
            NWDGUILayout.Informations("BECAREfull!");
            NWDGUILayout.Line();
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            // start interface
            NWDGUILayout.Section("Account");
            NWDGUILayout.SubSection("Account Anonymous");
            NWDAppConfiguration.SharedInstance().AnonymousPlayerIsLocal = EditorGUILayout.ToggleLeft("Anonymous account is stored on system device!", NWDAppConfiguration.SharedInstance().AnonymousPlayerIsLocal);

            NWDGUILayout.Section("WebServices");
            NWDGUILayout.SubSection("Webservices config for all environements");
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
            NWDAppConfiguration.SharedInstance().WebFolder = EditorGUILayout.TextField("WebService Folder", NWDAppConfiguration.SharedInstance().WebFolder);
            int tIndexWS = tWSListUsable.IndexOf(NWDAppConfiguration.SharedInstance().WebBuild);
            tIndexWS = EditorGUILayout.Popup("WebService active", tIndexWS, tWSListUsableString.ToArray());
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
            NWDAppConfiguration.SharedInstance().SurProtected = EditorGUILayout.Toggle("Sur Protected Database", NWDAppConfiguration.SharedInstance().SurProtected);
            if (NWDAppConfiguration.SharedInstance().SurProtected == true)
            {
                NWDAppConfiguration.SharedInstance().PreloadDatas = true;
            }
            EditorGUI.BeginDisabledGroup(!NWDAppConfiguration.SharedInstance().SurProtected);
            NWDAppConfiguration.SharedInstance().ProtectionTentativeMax = EditorGUILayout.IntField("Sur Protected Tentative", NWDAppConfiguration.SharedInstance().ProtectionTentativeMax);
            EditorGUI.EndDisabledGroup();
            EditorGUI.BeginDisabledGroup(NWDAppConfiguration.SharedInstance().SurProtected);
            NWDAppConfiguration.SharedInstance().PreloadDatas = EditorGUILayout.Toggle("Preload Datas", NWDAppConfiguration.SharedInstance().PreloadDatas);
            EditorGUI.EndDisabledGroup();
            NWDAppConfiguration.SharedInstance().RowDataIntegrity = EditorGUILayout.Toggle("Active Row Integrity", NWDAppConfiguration.SharedInstance().RowDataIntegrity);
            // Database editor informations
            string tDatabasePathEditor = NWDDataManager.SharedInstance().DatabasePathEditor + "/" + NWDDataManager.SharedInstance().DatabaseNameEditor;
            string tDatabasePathAccount = "/" + NWDDataManager.SharedInstance().DatabaseNameAccount;
            NWDGUILayout.SubSection("Databases Editor config for all environements");
            //GUILayout.Label(" TODO : explain", EditorStyles.helpBox);
            EditorGUILayout.LabelField("Editor path ", tDatabasePathEditor);
            EditorGUILayout.LabelField("EditorPass", NWDAppConfiguration.SharedInstance().EditorPass);
            EditorGUILayout.LabelField("EditorPassA", NWDAppConfiguration.SharedInstance().EditorPassA);
            EditorGUILayout.LabelField("EditorPassAB", NWDAppConfiguration.SharedInstance().EditorPassB);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Editor Pass Result", NWDAppConfiguration.SharedInstance().GetEditorPass());
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button("Editor Database File"))
            {
                EditorUtility.RevealInFinder(tDatabasePathEditor);
                Debug.LogWarning("DatabasePathEditor = " + tDatabasePathEditor);
                Debug.LogWarning("Editor Pass Result = " + NWDAppConfiguration.SharedInstance().GetEditorPass());
            }
            // Database account informations
            NWDGUILayout.SubSection("Databases Accountconfig for all environements (by device)");
            //GUILayout.Label(" TODO : explain", EditorStyles.helpBox);
            EditorGUILayout.LabelField("Account path ", tDatabasePathAccount);
            EditorGUILayout.LabelField("AccountHashSalt", NWDAppConfiguration.SharedInstance().AccountHashSalt);
            EditorGUILayout.LabelField("AccountHashSaltA", NWDAppConfiguration.SharedInstance().AccountHashSaltA);
            EditorGUILayout.LabelField("AccountHashSaltB", NWDAppConfiguration.SharedInstance().AccountHashSaltB);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.TextField("Account Pass Result", NWDAppConfiguration.SharedInstance().GetAccountPass());
            EditorGUI.EndDisabledGroup();
            if (GUILayout.Button("Account Database File"))
            {
                EditorUtility.RevealInFinder(tDatabasePathAccount);
                Debug.LogWarning("DatabasePathAccount = " + tDatabasePathAccount);
                Debug.LogWarning("Account Pass Result = " + NWDAppConfiguration.SharedInstance().GetAccountPass());
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
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
