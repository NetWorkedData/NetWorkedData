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
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using System.IO;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDEditorWindow : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        static List<NWDEditorWindow> AllWindowsList = new List<NWDEditorWindow>();
        private GUIContent IconAndTitleCompile;
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateCSharpFile()
        {
            //Debug.Log("NWDEditorWindow GenerateCSharpFile()");
            if (NWDAppConfiguration.SharedInstance().ShowCompile == true)
            {
                foreach (NWDEditorWindow tWindow in AllWindowsList)
                {
                    //Debug.Log("NWDEditorWindow RecompilePrevent() tWindow = " + tWindow.titleContent.text);
                    tWindow.Recompile = true;
                    tWindow.Repaint();
                }
            }
            NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
        }
        //-------------------------------------------------------------------------------------------------------------
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            //Debug.Log("NWDEditorWindow OnScriptsReloaded()");
            if (AllWindowsList != null)
            {
                foreach (NWDEditorWindow tWindow in AllWindowsList)
                {
                    if (tWindow != null)
                    {
                        tWindow.Recompile = false;
                        tWindow.Repaint();
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        bool Recompile = false;
        //-------------------------------------------------------------------------------------------------------------
        public NWDEditorWindow()
        {
            //Debug.Log("NWDEditorWindow()");
            if (AllWindowsList.Contains(this) == false)
            {
                AllWindowsList.Add(this);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        ~NWDEditorWindow()
        {
            //Debug.Log("~NWDEditorWindow()");
            if (AllWindowsList.Contains(this) == false)
            {
                AllWindowsList.Remove(this);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnGUI()
        {
            //Debug.Log("NWDEditorWindow OnGUI()");
            NWDGUI.LoadStyles();
            if (Recompile == false || EditorApplication.isCompiling == false)
            {
                OnPreventGUI();
            }
            else
            {
                if (IconAndTitleCompile == null)
                {
                    IconAndTitleCompile = new GUIContent();
                    if (IconAndTitleCompile.image == null)
                    {
                        string[] sGUIDs = AssetDatabase.FindAssets(typeof(NWDEditorWindow).Name + " t:texture");
                        foreach (string tGUID in sGUIDs)
                        {
                            string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                            string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                            if (tPathFilename.Equals(typeof(NWDEditorWindow).Name))
                            {
                                IconAndTitleCompile.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                            }
                        }
                    }
                }

                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(IconAndTitleCompile, NWDGUI.kIconCenterStyle);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("...compile in progress...");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.FlexibleSpace();
            }
            // if (EditorGUIUtility.isProSkin == true)
            {
                float tLogoSize = NWDGUI.kTitleStyle.fixedHeight;
                GUI.Label(new Rect(position.width - tLogoSize, 0, tLogoSize, tLogoSize), NWDGUI.kNetWorkedDataLogoContent);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnPreventGUI()
        {
            //Debug.Log("NWDEditorWindow OnPreventGUI()");
            throw new Exception("override OnPreventGUI() in place of OnGUI");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
