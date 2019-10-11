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
using System.Collections.Generic;
using System.Linq;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDModelManager : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The icon and title.
        /// </summary>
        GUIContent IconAndTitle;
        Vector2 ScrollPosition;
        public List<Type> TypeList = new List<Type>();
        public List<Type> TypeErrorList = new List<Type>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Shared Instance.
        /// </summary>
        public static NWDModelManager kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDModelManager SharedInstance()
        {
            //NWEBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDModelManager)) as NWDModelManager;
            }
            //NWEBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDModelManager SharedInstanceFocus()
        {
            //NWEBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //NWEBenchmark.Finish();
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
            //NWEBenchmark.Start();
            if (IconAndTitle == null)
            {
                IconAndTitle = new GUIContent();
                IconAndTitle.text = NWDConstants.K_APP_MODEL_MANAGER_TITLE;
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
            TypeList.Clear();
            TypeErrorList.Clear();
            foreach (Type tType in NWDDataManager.SharedInstance().mTypeLoadedList.OrderBy(A => NWDBasisHelper.FindTypeInfos(A).ClassNamePHP))
            {
                TypeList.Add(tType);
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                if (tHelper.WebModelChanged == true || tHelper.WebModelDegraded == true || tHelper.SaltValid == false)
                {
                    TypeErrorList.Add(tType);
                }
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void DrawType(Type sType)
        {
            GUILayout.BeginVertical(/*EditorStyles.helpBox*/);
            NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(sType);
            NWDGUILayout.SubSection(tHelper.ClassNamePHP);
            tHelper.DrawTypeInformations();
            GUILayout.EndVertical();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            //NWEBenchmark.Start();
            NWDGUI.LoadStyles();
            NWDGUILayout.Title("Model Manager");
            ScrollPosition = GUILayout.BeginScrollView(ScrollPosition, NWDGUI.kScrollviewFullWidth, GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(true));
            if (TypeErrorList.Count > 0)
            {
                NWDGUILayout.Section("Models in error");
                foreach (Type tType in TypeErrorList)
                {
                    DrawType(tType);
                }
            }
            NWDGUILayout.Section("Models list");
            foreach (Type tType in TypeList)
            {
                DrawType(tType);
            }
            GUILayout.EndScrollView();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
