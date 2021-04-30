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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [InitializeOnLoad]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDPlayerPreProcess : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        static NWDPlayerPreProcess()
        {
            NWDBenchmark.Start();
            EditorApplication.playModeStateChanged -= PlayModeStateChangedCallback; // remove old callback from compile for this class
            EditorApplication.quitting -= Quit;
#if NWD_PLAYER_PRE_PROCESS
            EditorApplication.playModeStateChanged += PlayModeStateChangedCallback;
#if UNITY_2018_1_OR_NEWER
            EditorApplication.quitting += Quit;
#endif
#endif
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Quit()
        {
            NWDBenchmark.Start();
            //Force all datas to be write in database
            NWDDataManager.SharedInstance().DataQueueExecute();
            NWDEditorWindow.GenerateCSharpFile();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void PlayModeStateChangedCallback(PlayModeStateChange sState)
        {
            NWDBenchmark.Start();
            if (sState == PlayModeStateChange.ExitingEditMode)
            {
                // prevent error not exist (delete by dev)
                NWDErrorHelper tErrorHelper = NWDBasisHelper.BasisHelper<NWDError>() as NWDErrorHelper;
                tErrorHelper.GenerateBasisError();
                // force all datas to be write in database
                NWDDataManager.SharedInstance().DataQueueExecute();
                // must check the accounts for test
                NWDEditorWindow.GenerateCSharpFile();
            }
            // update bundle before playing to test with the good version 
            NWDVersion.UpdateVersionBundle();
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
