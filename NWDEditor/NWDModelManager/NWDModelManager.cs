//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
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
            //NWDBenchmark.Start();
            if (kSharedInstance == null)
            {
                kSharedInstance = EditorWindow.GetWindow(typeof(NWDModelManager)) as NWDModelManager;
            }
            //NWDBenchmark.Finish();
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDModelManager SharedInstanceFocus()
        {
            //NWDBenchmark.Start();
            SharedInstance().ShowUtility();
            SharedInstance().Focus();
            //NWDBenchmark.Finish();
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
            //NWDBenchmark.Start();
            Analyze();
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void Analyze()
        {
            //NWDBenchmark.Start();
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
            //NWDBenchmark.Finish();
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
            //NWDBenchmark.Start();
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
