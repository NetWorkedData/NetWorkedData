// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:44
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//=====================================================================================================================
//namespace NetWorkedData
//{
//    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//    [InitializeOnLoad]
//    public class NWDEditorDataManager : Editor
//    {
//        //-------------------------------------------------------------------------------------------------------------
//        static NWDEditorDataManager()
//        {
//            EditorApplication.update += Update;
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        // Update is called once per frame
//        static void Update()
//        {
//            EditorApplication.update -= Update;
//            if (Application.isPlaying == false)
//            {
//                if (NWDTypeLauncher.DataLoaded == false)
//                {
//                    //Debug.Log("NWD => Load data for editor");
//                    NWDDataManager tShareInstance = NWDDataManager.SharedInstance();
//                    tShareInstance.ReloadAllObjects();
//                    tShareInstance.RestaureObjectInEdition();
//                }
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//    }
//    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//}
//=====================================================================================================================
#endif