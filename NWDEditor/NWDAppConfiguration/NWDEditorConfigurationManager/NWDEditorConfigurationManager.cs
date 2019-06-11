// =====================================================================================================================
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
// =====================================================================================================================
#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDEditorConfigurationManager : EditorWindow
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
        public static NWDEditorConfigurationManager SharedInstance()
        {
            //BTBBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDEditorConfigurationManager)) as NWDEditorConfigurationManager;
            }
            //BTBBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDEditorConfigurationManager SharedInstanceFocus()
        {
            //BTBBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //BTBBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Refresh()
        {
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDEditorConfigurationManager));
            foreach (NWDEditorConfigurationManager tWindow in tWindows)
            {
                tWindow.Repaint();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static bool IsSharedInstance()
        //{
        //    if (kSharedInstance != null)
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        return false;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
            //BTBBenchmark.Start();
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDConstants.K_EDITOR_CONFIGURATION_TITLE;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets("NWDEditorConfigurationManager t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals("NWDEditorConfigurationManager"))
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
        public void OnGUI()
        {
            //BTBBenchmark.Start();
            NWDGUI.LoadStyles();
            NWDGUILayout.Title("Editor preferences");
            //NWDGUILayout.Informations("Some informations!");
            NWDAppConfiguration.SharedInstance().EditorTableCommun = EditorGUILayout.Toggle("Table Pref commun", NWDAppConfiguration.SharedInstance().EditorTableCommun);
            NWDGUILayout.Line();
            float tMinWidht = 270.0F;
            int tColum = 1;
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.K_ALERT_SALT_SHORT_ERROR, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
            }
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
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
            GUILayout.EndScrollView();

            NWDGUILayout.Line();
            GUILayout.Space(NWDGUI.kFieldMarge);
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
