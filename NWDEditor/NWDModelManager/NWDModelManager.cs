//=====================================================================================================================
//
//  ideMobi 2020©
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
                tWindow.Analyze();
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
            Analyze();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Analyze()
        {
            //NWEBenchmark.Start();
            TitleInit(NWDConstants.K_APP_MODEL_MANAGER_TITLE, typeof(NWDModelManager));
            TypeList.Clear();
            TypeErrorList.Clear();
            foreach (Type tType in NWDDataManager.SharedInstance().ClassTypeLoadedList.OrderBy(A => NWDBasisHelper.FindTypeInfos(A).ClassNamePHP))
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
            NWDGUILayout.Title(NWDConstants.K_APP_MODEL_MANAGER_TITLE);
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
