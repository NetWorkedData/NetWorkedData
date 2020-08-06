//=====================================================================================================================
//Copyright NetWorkedDatas ideMobi 2020
//Created by Jean-François CONTART 
//=====================================================================================================================
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
#define NWD_LOG
#define NWD_BENCHMARK
#endif
#endif
//=====================================================================================================================
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
#if UNITY_EDITOR
//=====================================================================================================================
namespace NetWorkedData.MacroDefine
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // You can create custom enum of macro
    // Just follow this example class
    public class NWDRowIntegrity : MDEDataTypeEnumGeneric<NWDRowIntegrity>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of enum controller
        public static string Title = SetTitle("Row integrity");
        //-------------------------------------------------------------------------------------------------------------
        // declare all values
        public static NWDRowIntegrity None = AddNone("with Integrity");
        // string will be convert in UNIX format automatically
        public static NWDRowIntegrity Sample = Add(2, "NWD_INTEGRITY_NONE", "without Integrity");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif