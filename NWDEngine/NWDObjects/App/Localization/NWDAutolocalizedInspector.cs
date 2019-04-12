// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:33:40
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
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
    [CustomEditor(typeof(NWDAutolocalized))]
    public class NWDAutolocalizedInspector : Editor
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void OnInspectorGUI()
        {
            NWDAutolocalized tTarget = (NWDAutolocalized)target;
            DrawDefaultInspector();
            //if (GUILayout.Button("AutoLocalize copy"))
            {
                NWDLocalization tLocalization = tTarget.LocalizationReference.GetObject();
                if (tLocalization != null)
                {
                    tTarget.LocalizeEditor();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif