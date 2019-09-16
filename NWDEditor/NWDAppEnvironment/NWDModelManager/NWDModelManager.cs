//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:20:16
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using UnityEngine;
using System.IO;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDModelManager : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The icon and title.
        /// </summary>
        GUIContent IconAndTitle;
        Vector2 ScrollPosition;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        public static NWDModelManager kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDModelManager SharedInstance()
        {
            //BTBBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDModelManager)) as NWDModelManager;
            }
            //BTBBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDModelManager SharedInstanceFocus()
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
            var tWindows = Resources.FindObjectsOfTypeAll(typeof(NWDModelManager));
            foreach (NWDModelManager tWindow in tWindows)
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
        public void OnEnable()
        {
            //BTBBenchmark.Start();
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDConstants.K_APP_CHOOSER_ENVIRONMENT_TITLE;
                if (IconAndTitle.image == null)
                {
                    string[] sGUIDs = AssetDatabase.FindAssets(typeof(NWDModelManager).Name + " t:texture");
                    foreach (string tGUID in sGUIDs)
                    {
                        string tPathString = AssetDatabase.GUIDToAssetPath(tGUID);
                        string tPathFilename = Path.GetFileNameWithoutExtension(tPathString);
                        if (tPathFilename.Equals(typeof(NWDModelManager).Name))
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
            NWDGUILayout.Title("Model Manager");
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList)
            {
                GUILayout.BeginVertical(/*EditorStyles.helpBox*/);
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                NWDGUILayout.SubSection(tHelper.ClassNamePHP);

                GUILayout.BeginHorizontal();
                Texture2D tTextureOfClass = tHelper.TextureOfClass();
                if (tTextureOfClass != null)
                {
                    GUILayout.Label(tTextureOfClass, NWDGUI.KTableSearchClassIcon, GUILayout.Width(48.0F), GUILayout.Height(48.0F));
                }
                GUILayout.BeginVertical();
                GUILayout.Label(tHelper.ClassMenuName, EditorStyles.boldLabel);
                GUILayout.Label("Webservice last version generated for this Class  is " + tHelper.LastWebBuild.ToString() + " ( App use web service " + NWDAppConfiguration.SharedInstance().WebBuild.ToString() + ")");
                GUILayout.Label(tHelper.ClassDescription);
                if (tHelper.WebModelChanged == true)
                {
                    NWDGUILayout.WarningBox(NWDConstants.K_APP_BASIS_WARNING_MODEL);
                }
                if (tHelper.WebModelDegraded == true)
                {
                    NWDGUILayout.WarningBox(NWDConstants.K_APP_BASIS_WARNING_MODEL_DEGRADED);
                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();
                GUILayout.EndVertical();
            }

            GUILayout.EndScrollView();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
