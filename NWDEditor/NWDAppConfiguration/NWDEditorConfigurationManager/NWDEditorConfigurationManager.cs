//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:11
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
    public class NWDEditorConfigurationManager : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        private static NWDEditorConfigurationManager kSharedInstance;
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
        public static NWDEditorConfigurationManager SharedInstance()
        {
            //NWEBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDEditorConfigurationManager)) as NWDEditorConfigurationManager;
            }
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Show the SharedInstance of Editor Configuration Manager Window and focus on.
        /// </summary>
        /// <returns></returns>
        public static NWDEditorConfigurationManager SharedInstanceFocus()
        {
            //NWEBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Repaint all Editor Configuration Manager Windows.
        /// </summary>
        public static void Refresh()
        {
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDEditorConfigurationManager));
            foreach (NWDEditorConfigurationManager tWindow in tWindows)
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
        /// <summary>
        /// On enable action.
        /// </summary>
        public void OnEnable()
        {
            //NWEBenchmark.Start();
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDConstants.K_EDITOR_CONFIGURATION_TITLE;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets(typeof(NWDEditorConfigurationManager).Name+" t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals(typeof(NWDEditorConfigurationManager).Name))
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
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI()
        {
            //NWEBenchmark.Start();
            NWDGUI.LoadStyles();

            NWDGUILayout.Title("Editor preferences");

            // start scroll
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            //General preferences
            NWDGUILayout.Section("User preferences");
           EditorPrefs.SetString("NWD_USER_BUILDER", EditorGUILayout.TextField("User builder name", EditorPrefs.GetString("NWD_USER_BUILDER","(user)")));

            //General preferences
            NWDGUILayout.Section("General preferences");
            NWDAppConfiguration.SharedInstance().EditorTableCommun = EditorGUILayout.Toggle("Table Pref commun", NWDAppConfiguration.SharedInstance().EditorTableCommun);
            NWDAppConfiguration.SharedInstance().ShowCompile = EditorGUILayout.Toggle("Show re-compile ", NWDAppConfiguration.SharedInstance().ShowCompile);
            NWDAppConfiguration.SharedInstance().TintColor = EditorGUILayout.ColorField("Tint color ", NWDAppConfiguration.SharedInstance().TintColor);
            if (GUILayout.Button("Reset Tint color"))
            {
                NWDAppConfiguration.SharedInstance().ResetTintColor();
            }
            NWDGUILayout.Line();
            float tMinWidht = 270.0F;
            int tColum = 1;
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_ALERT_SALT_SHORT_ERROR, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDEditorWindow.GenerateCSharpFile();
                    //NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
            }

            // build preference section
            NWDGUILayout.Section("Editor build preferences");
            //define environment build
            NWDEditorBuildEnvironment tNWDEditorBuildEnvironment = NWDBuildPreProcess.GetEditoBuildEnvironment();
            tNWDEditorBuildEnvironment = (NWDEditorBuildEnvironment)EditorGUILayout.EnumPopup("Build Environment", tNWDEditorBuildEnvironment);
            NWDBuildPreProcess.SetEditorBuildEnvironment(tNWDEditorBuildEnvironment);
            // define rename option
            NWDEditorBuildRename tNWDEditorBuildRename = NWDBuildPreProcess.GetEditoBuildRename();
            tNWDEditorBuildRename = (NWDEditorBuildRename)EditorGUILayout.EnumPopup("Build Rename", tNWDEditorBuildRename);
            NWDBuildPreProcess.SetEditorBuildRename(tNWDEditorBuildRename);
            // define update database
            NWDEditorBuildDatabaseUpdate tNWDEditorBuildDatabaseUpdate = NWDBuildPreProcess.GetEditorBuildDatabaseUpdate();
            tNWDEditorBuildDatabaseUpdate = (NWDEditorBuildDatabaseUpdate)EditorGUILayout.EnumPopup("Build Database Update", tNWDEditorBuildDatabaseUpdate);
            NWDBuildPreProcess.SetEditorBuildDatabaseUpdate(tNWDEditorBuildDatabaseUpdate);

            // Data tag
            NWDGUILayout.Section("Datas Tags");
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


            // end scroll
            GUILayout.EndScrollView();

            NWDGUILayout.Line();
            GUILayout.Space(NWDGUI.kFieldMarge);
            NWDGUI.BeginRedArea();
            if (GUILayout.Button(NWDConstants.K_APP_CONFIGURATION_SAVE_BUTTON))
            {
                NWDEditorWindow.GenerateCSharpFile();
                //NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
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
