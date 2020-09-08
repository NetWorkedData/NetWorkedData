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
    public class NWDEditorFooterContent : NWDEditorWindowContent
    {
        //-------------------------------------------------------------------------------------------------------------
        private static NWDEditorFooterContent _kSharedInstanceContent;
        //-------------------------------------------------------------------------------------------------------------
        public GUIContent InfosContent = new GUIContent();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Returns the <see cref="_kSharedInstanceContent"/> or instance one
        /// </summary>
        /// <returns></returns>
        public static NWDEditorFooterContent SharedInstance()
        {
            NWDBenchmark.Start();
            if (_kSharedInstanceContent == null)
            {
                _kSharedInstanceContent = new NWDEditorFooterContent();
            }
            NWDBenchmark.Finish();
            return _kSharedInstanceContent;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnEnable(NWDEditorWindow sEditorWindow)
        {
            NWDBenchmark.Start();
            Texture2D[] tIcons = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Unknown);
            Texture2D tIcon = null;
            if (tIcons.Length > 0)
            {
                tIcon = tIcons[0];
            }
            InfosContent = new GUIContent(
                " Project : <b>" + NWDAppConfiguration.SharedInstance().DevEnvironment.AppName +
                "</b>  Environment : <b> " + NWDAppConfiguration.SharedInstance().SelectedEnvironment().Environment +
                "</b>  WebService version : <b>" + NWDAppConfiguration.SharedInstance().WebBuild.ToString() +
                "</b>  Version : <b>" + PlayerSettings.bundleVersion +
                "</b>  SQLite : <b>" + NWDDataManager.SharedInstance().GetVersion() +
                "</b>", tIcon);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnDisable(NWDEditorWindow sEditorWindow)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        ///  On GUI drawing.
        /// </summary>
        public override void OnPreventGUI(Rect sRect)
        {
            NWDBenchmark.Start();
            EditorGUILayout.BeginHorizontal();
            GUILayout.Label(InfosContent, NWDGUI.kFooterLabelStyle);
            EditorGUILayout.EndHorizontal();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDEditorFooter : NWDEditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        public override string TutorialLink(string sLink = "")
        {
            return NWDConstants.K_NWDURL + "footer/";
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void NewFooter()
        {
            NWDBenchmark.Start();
            NWDEditorFooter tFooter = EditorWindow.CreateWindow<NWDEditorFooter>();
            tFooter.ShowUtility();
            tFooter.Focus();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void OnEnable()
        {
            NWDBenchmark.Start();
            NormalizeWidth = 300;
            NormalizeHeight = 20;
            MinWidth = 100;
            MinHeight = NormalizeHeight;
            MaxWidth = 2048;
            MaxHeight = NormalizeHeight;
            minSize = new Vector2(MinWidth, MinHeight);
            maxSize = new Vector2(MaxWidth, MaxHeight);
            TitleInit(NWDAppConfiguration.SharedInstance().SelectedEnvironment().AppName, typeof(NWDEditorFooter));
            NWDEditorFooterContent.SharedInstance().OnEnable(this);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void OnPreventGUI()
        {
            NWDBenchmark.Start();
            NWDGUI.LoadStyles();
            EditorGUI.DrawRect(new Rect (0,0,position.width, position.height), NWDGUI.kRowColorSelected);
            NWDEditorFooterContent.SharedInstance().OnPreventGUI(position);
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
