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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasis : NWDTypeClass
    {
        //-------------------------------------------------------------------------------------------------------------
        //public void AddNetWorkedDataToObject(GameObject sGameObject)
        //{
        //    SetNetWorkedDataObject(sGameObject, this);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static void SetNetWorkedDataObject(GameObject sGameObject, NWDBasis sObject)
        //{
        //    NWDMonoBehaviour tNWDMonoBehaviour = NWDMonoBehaviour.SetNetWorkedDataObject(sGameObject, sObject);
        //    tNWDMonoBehaviour.Type = sObject.GetType().ToString();
        //    tNWDMonoBehaviour.Reference = sObject.Reference;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public static NWDBasis GetNetWorkedDataObject(GameObject sGameObject)
        //{
        //    object rReturn = NWDMonoBehaviour.GetNetWorkedDataObject(sGameObject);
        //    if (rReturn.GetType() != typeof(K))
        //    {
        //        rReturn = null;
        //    }
        //    return rReturn as NWDBasis;
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================