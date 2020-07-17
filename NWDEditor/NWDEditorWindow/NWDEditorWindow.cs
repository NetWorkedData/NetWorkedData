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
#endif
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
        private static List<NWDEditorWindow> AllWindowsList = new List<NWDEditorWindow>();
        //-------------------------------------------------------------------------------------------------------------
        private GUIContent IconAndTitleCompile;
        private GUIContent IconAndTitle;
        private bool ProSkinActive;
        private string EditorTitle;
        private Type EditorType;
        private bool TitleIsInit = false;
        private bool Recompile = false;
        //-------------------------------------------------------------------------------------------------------------
        public void SkinChange()
        {
            NWDBenchmark.Start();
            if (EditorGUIUtility.isProSkin != ProSkinActive || TitleIsInit == false)
            {
                NWDBenchmark.PrefReload();
                TitleEnable();
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TitleInit(string tTitle, Type tType)
        {
            NWDBenchmark.Start();
            EditorTitle = tTitle;
            EditorType = tType;
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TitleEnable()
        {
            NWDBenchmark.Start();
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
                //NWDBenchmark.Step(true, tIconName);
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
                //NWDBenchmark.Step(true, tIconName);
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
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void GenerateCSharpFile()
        {
            NWDBenchmark.Start();
            if (NWDProjectPrefs.GetBool(NWDConstants.K_EDITOR_SHOW_COMPILE, true) == true)
            {
                foreach (NWDEditorWindow tWindow in AllWindowsList)
                {
                    tWindow.Recompile = true;
                    tWindow.Repaint();
                }
            }
            NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        [DidReloadScripts]
        private static void OnScriptsReloaded()
        {
            NWDBenchmark.Start();
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
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDEditorWindow()
        {
            //NWDBenchmark.Start();
            if (AllWindowsList.Contains(this) == false)
            {
                AllWindowsList.Add(this);
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        ~NWDEditorWindow()
        {
            //NWDBenchmark.Start();
            if (AllWindowsList.Contains(this) == false)
            {
                AllWindowsList.Remove(this);
            }
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void OnGUI()
        {
            NWDBenchmark.Start();
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

                //GUILayout.BeginHorizontal();
                //GUILayout.FlexibleSpace();
                //GUILayout.Label("NeWeeDy!");
                //GUILayout.FlexibleSpace();
                //GUILayout.EndHorizontal();

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
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void OnPreventGUI()
        {
            throw new Exception("override OnPreventGUI() in place of OnGUI");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
