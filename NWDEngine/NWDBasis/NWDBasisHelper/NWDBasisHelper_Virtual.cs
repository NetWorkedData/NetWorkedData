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
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisHelper
    {
        //-------------------------------------------------------------------------------------------------------------
        public virtual void  ClassInitialization()
        {
             //Debug.Log("ClassInitialization() base method (" + GetType().FullName + ")");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void ClassDatasAreLoaded()
        {
            //Debug.Log("ClassDatasAreLoaded() base method (" + GetType().FullName + ")");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual List<Type>  OverrideClasseInThisSync()
        {
            //Debug.Log("New_OverrideClasseInThisSync() base method (" + GetType().FullName + ")");
            return new List<Type>() { ClassType };
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================