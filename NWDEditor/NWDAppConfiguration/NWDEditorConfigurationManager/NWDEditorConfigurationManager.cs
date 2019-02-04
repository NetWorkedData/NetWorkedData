//=====================================================================================================================
//
// ideMobi copyright 2018 
// All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;
using System.Reflection;
using System.IO;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWD app environment manager window.
    /// </summary>
    public class NWDEditorConfigurationManager : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        GUIContent IconAndTitle;
        static Vector2 ScrollPosition;
        //-------------------------------------------------------------------------------------------------------------
        public NWDEditorConfigurationManager()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnEnable()
        {
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
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDEditorConfigurationManager SharedInstance()
        {
            NWDEditorMenu.EditorPreferenceShow();
            return NWDEditorMenu.kNWDEditorConfigurationManagerWindow;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnGUI()
        {
            NWDConstants.LoadStyles();
            float tMinWidht = 270.0F;
            float tScrollMarge = 20.0f;
            int tColum = 1;

            // Draw warning if salt for class is false
            if (NWDDataManager.SharedInstance().TestSaltMemorizationForAllClass() == false)
            {
                EditorGUILayout.HelpBox(NWDConstants.kAlertSaltShortError, MessageType.Error);
                if (GUILayout.Button(NWDConstants.K_APP_CLASS_SALT_REGENERATE))
                {
                    NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
                }
            }
            // Draw helpbox
            //EditorGUILayout.HelpBox (NWDConstants.K_APP_CONFIGURATION_HELPBOX, MessageType.None);
            // List environment
            //update the veriosn of Bundle
            //NWDVersion.UpdateVersionBundle ();
            // Draw interface for environment selected inn scrollview
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, EditorStyles.inspectorFullWidthMargins, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));

            GUILayout.Label("Datas Tags", NWDConstants.kLabelTitleStyle);
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
            GUILayout.Space(8.0f);
            if (GUILayout.Button(NWDConstants.K_APP_CONFIGURATION_SAVE_BUTTON))
            {
                NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            }
            GUILayout.Space(8.0f);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
