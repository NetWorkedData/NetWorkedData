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
    public class NWDNeverNullDataType : MDEDataTypeEnumGeneric<NWDNeverNullDataType>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of enum controller
        public static string Title = SetTitle("Never Null Data Type");
        //-------------------------------------------------------------------------------------------------------------
        // declare all values
        public static NWDNeverNullDataType None = AddNone("can be Null");
        // string will be convert in UNIX format automatically
        public static NWDNeverNullDataType Sample = Add(2, "NWD_NEVER_NULL_DATATYPE", "never Null");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif