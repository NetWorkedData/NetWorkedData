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
        private GUIContent IconAndTitle;
        protected bool ProSkinActive;
        private string EditorTitle;
        private Type EditorType;
        private bool TitleIsInit = false;
        //-------------------------------------------------------------------------------------------------------------
        public void SkinChange()
        {
            if (EditorGUIUtility.isProSkin != ProSkinActive || TitleIsInit == false)
            {
                TitleEnable();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TitleInit(string tTitle, Type tType)
        {
            EditorTitle = tTitle;
            EditorType = tType;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TitleEnable()
        {
            //NWEBenchmark.Start();
            if (string.IsNullOrEmpty(EditorTitle) == false && EditorType != null)
            {
                TitleIsInit = true;
                ProSkinActive = EditorGUIUtility.isProSkin;
                if (IconAndTitle == null)
                {
                    IconAndTitle = new GUIContent();
                }
                IconAndTitle.text = EditorTitle;

                // find texture by default
                string tIconName = EditorGUIUtility.isProSkin ? typeof(NWDEditorWindow).Name + "_pro" : typeof(NWDEditorWindow).Name;
                //NWEBenchmark.Step(true, tIconName);
                string[] sGUIDs = AssetDatabase.FindAssets(tIconName + " t:texture");
                foreach (string tGUID in sGUIDs)
                {
                    string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                    string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                    if (tPathFilename.Equals(tIconName))
                    {
                        IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        break;
                    }
                }

                // find texture by in parameters
                tIconName = EditorGUIUtility.isProSkin ? EditorType.Name + "_pro" : EditorType.Name;
                //NWEBenchmark.Step(true, tIconName);
                sGUIDs = AssetDatabase.FindAssets(tIconName + " t:texture");
                foreach (string tGUID in sGUIDs)
                {
                    string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                    string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                    if (tPathFilename.Equals(tIconName))
                    {
                        IconAndTitle.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                    }
                }
                titleContent = IconAndTitle;

                IconAndTitleCompile = new GUIContent();
                string tIconNameCompile = EditorGUIUtility.isProSkin ? typeof(NWDEditorWindow).Name + "Compile_pro" : typeof(NWDEditorWindow).Name + "Compile";
                string[] sGUIDCompiles = AssetDatabase.FindAssets(tIconNameCompile + " t:texture");
                foreach (string tGUID in sGUIDCompiles)
                {
                    string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                    string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                    if (tPathFilename.Equals(tIconNameCompile))
                    {
                        IconAndTitleCompile.image = AssetDatabase.LoadAssetAtPath(tPathString, typeof(Texture2D)) as Texture2D;
                        break;
                    }
                }
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateCSharpFile()
        {
            if (NWDAppConfiguration.SharedInstance().ShowCompile == true)
            {
                foreach (NWDEditorWindow tWindow in AllWindowsList)
                {
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
            SkinChange();
            NWDGUI.LoadStyles();
            if (Recompile == false || EditorApplication.isCompiling == false)
            {
                OnPreventGUI();
            }
            else
            {
                GUILayout.FlexibleSpace();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label(IconAndTitleCompile, NWDGUI.kIconCenterStyle);
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("NeWeeDy!");
                GUILayout.FlexibleSpace();
                GUILayout.EndHorizontal();
                GUILayout.Space(10.0F);
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
