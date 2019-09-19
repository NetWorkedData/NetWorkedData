//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:29
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;
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
            //BTBBenchmark.Start();
            //Force all datas to be write in database
            NWDDataManager.SharedInstance().DataQueueExecute();
            //Debug.Log("Play Mode State must recompile NWDParameter.cs file!");
            NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            // NWDVersion.UpdateVersionBundle();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void PlayModeStateChangedCallback(PlayModeStateChange sState)
        {
            //BTBBenchmark.Start();
            //Debug.Log("Play Mode State Changed!");
            if (sState == PlayModeStateChange.ExitingEditMode)
            {
                //Force all datas to be write in database
                NWDDataManager.SharedInstance().DataQueueExecute();
                // must check the accounts for test
                //Debug.Log("Play Mode State must recompile NWDParameter.cs file!");
                NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            }
            // update bundle before playing to test with the good version 
            NWDVersion.UpdateVersionBundle();
            //BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif