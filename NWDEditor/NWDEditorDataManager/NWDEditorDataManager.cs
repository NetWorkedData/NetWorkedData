using System.Collections;
using System.Collections.Generic;
using UnityEditor;
#if UNITY_EDITOR
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [InitializeOnLoad]
    public class NWDEditorDataManager : Editor
    {
        //-------------------------------------------------------------------------------------------------------------
        static NWDEditorDataManager()
        {
            EditorApplication.update += Update;
        }
        //-------------------------------------------------------------------------------------------------------------
        // Update is called once per frame
        static void Update()
        {
            EditorApplication.update -= Update;
            if (Application.isPlaying == false)
            {
                if (NWDTypeLauncher.DataLoaded == false)
                {
                    Debug.Log("NWD => Load data for editor");
                    NWDDataManager tShareInstance = NWDDataManager.SharedInstance();
                    tShareInstance.ReloadAllObjects();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif