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
#endif
//=====================================================================================================================
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using BasicToolBox;
using UnityEditor;
using UnityEditor.Build;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [InitializeOnLoad]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDPlayerPreProcess : EditorWindow
    {
        //-------------------------------------------------------------------------------------------------------------
        static NWDPlayerPreProcess()
        {
            EditorApplication.playModeStateChanged -= PlayModeStateChangedCallback; // remove old callback from compile for this class
            EditorApplication.playModeStateChanged += PlayModeStateChangedCallback;
#if UNITY_2018
            EditorApplication.quitting -= Quit;
            EditorApplication.quitting += Quit;
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Quit()
        {
            //NWEBenchmark.Start();
            //Force all datas to be write in database
            NWDDataManager.SharedInstance().DataQueueExecute();
            //Debug.Log("Play Mode State must recompile NWDParameter.cs file!");
            NWDEditorWindow.GenerateCSharpFile();
            //NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            // NWDVersion.UpdateVersionBundle();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void PlayModeStateChangedCallback(PlayModeStateChange sState)
        {
            //NWEBenchmark.Start();
            //Debug.Log("Play Mode State Changed!");
            if (sState == PlayModeStateChange.ExitingEditMode)
            {
                // prevent error not exist (delete by dev)
                NWDErrorHelper tErrorHelper = NWDBasisHelper.BasisHelper<NWDError>() as NWDErrorHelper;
                tErrorHelper.GenerateBasisError();
                //Force all datas to be write in database
                NWDDataManager.SharedInstance().DataQueueExecute();
                // must check the accounts for test
                //Debug.Log("Play Mode State must recompile NWDParameter.cs file!");
                NWDEditorWindow.GenerateCSharpFile();
                //NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            }
            // update bundle before playing to test with the good version 
            NWDVersion.UpdateVersionBundle();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif