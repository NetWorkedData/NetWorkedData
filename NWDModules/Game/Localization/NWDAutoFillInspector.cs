//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [CustomEditor(typeof(NWDAutoFill))]
    public class NWDAutoFillInspector : Editor
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void OnInspectorGUI()
        {
            NWDAutoFill tTarget = (NWDAutoFill)target;
            DrawDefaultInspector();
            //if (GUILayout.Button("AutoFill copy"))
            {
                NWDLocalization tLocalization = tTarget.LocalizationReference.GetObject();
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