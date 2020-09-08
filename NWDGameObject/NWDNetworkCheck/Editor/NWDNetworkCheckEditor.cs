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
using System.Collections;

using UnityEngine;
using UnityEngine.SceneManagement;

using NetWorkedData;
using System.IO;

using UnityEditor;
using UnityEditorInternal;
using UnityEditor.SceneManagement;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [CustomEditor(typeof(NWDNetworkCheck))]
    public class NWDNetworkCheckEditor : Editor
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void OnInspectorGUI()
        {
            //bool tNextSceneEdition = false;
            // Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
            this.serializedObject.Update();
            // Script Editor
            NWDNetworkCheck tScript = (NWDNetworkCheck)target;
            if (GUILayout.Button("Test network now"))
            {
                tScript.TestNetwork();
            }
            DrawDefaultInspector();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
