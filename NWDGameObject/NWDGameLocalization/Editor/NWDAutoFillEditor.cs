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
using UnityEditor;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [CustomEditor(typeof(NWDAutoFill))]
    public class NWDAutoFillEditor : Editor
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void OnInspectorGUI()
        {
            NWDAutoFill tTarget = (NWDAutoFill)target;
            DrawDefaultInspector();
            //if (GUILayout.Button("AutoFill copy"))
            {
                NWDLocalization tLocalization = tTarget.LocalizationReference.GetReachableData();
                if (tLocalization != null)
                {
                    tTarget.LocalizeEditor();
                }
            }
            if (GUILayout.Button("AutoFill anim restart"))
            {
                tTarget.StartFilling();
            }
            EditorGUI.BeginDisabledGroup(tTarget.IsRunning() == false);
            if (GUILayout.Button("Fast Fill"))
            {
                tTarget.FastSpeed();
            }
            if (GUILayout.Button("Normal Fill"))
            {
                tTarget.NormalSpeed();
            }
            EditorGUI.EndDisabledGroup();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
