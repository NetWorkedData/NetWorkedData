//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
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
            EditorApplication.quitting -= PlayModeStateChangedCallback;
            EditorApplication.quitting += PlayModeStateChangedCallback;
#endif
		}
		//-------------------------------------------------------------------------------------------------------------
        public static void PlayModeStateChangedCallback (PlayModeStateChange sState)
        {
            //Debug.Log("Play Mode State Changed!");
            if (sState == PlayModeStateChange.ExitingEditMode)
            {
                //Force all datas to be write in database
                NWDDataManager.SharedInstance().UpdateQueueExecute();
                // must check the accounts for test
                //Debug.Log("Play Mode State must recompile NWDParameter.cs file!");
                NWDAppConfiguration.SharedInstance().GenerateCSharpFile(NWDAppConfiguration.SharedInstance().SelectedEnvironment());
            }
			NWDVersion.UpdateVersionBundle ();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif