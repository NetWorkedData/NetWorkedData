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
using UnityEngine;
using System.IO;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDEditorFooter : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public static void NewFooter()
        {
            NWDEditorFooter tFooter = EditorWindow.CreateWindow<NWDEditorFooter>();
            tFooter.ShowUtility();
            tFooter.Focus();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnEnable()
        {
            minSize = new Vector2(20, 32);
            maxSize = new Vector2(float.MaxValue, 16);
            TitleInit(NWDAppConfiguration.SharedInstance().SelectedEnvironment().AppName, typeof(NWDEditorFooter));
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            NWDGUI.LoadStyles();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(
                "   Project : <b>" + NWDAppConfiguration.SharedInstance().DevEnvironment.AppName +
                "</b>  Environment : <b> " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().Environment +
                "</b>  Webservice version : <b>" + NWDAppConfiguration.SharedInstance().WebBuild.ToString() +
                "</b>  Version Bundle : <b>" + PlayerSettings.bundleVersion +
                "</b>  SQLite : <b>" + NWDDataManager.SharedInstance().GetVersion() +
                "</b>",
                NWDGUI.kFooterLabelStyle);
            EditorGUILayout.EndHorizontal();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
