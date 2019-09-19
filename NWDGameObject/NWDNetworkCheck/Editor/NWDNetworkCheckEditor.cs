//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:45:58
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
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