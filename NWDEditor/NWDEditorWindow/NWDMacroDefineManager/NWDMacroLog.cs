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
    public class NWDVerbose : MDEDataTypeEnumGeneric<NWDVerbose>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of enum controller
        public static string Title = SetTitle("Verbose");
        //-------------------------------------------------------------------------------------------------------------
        // declare all values
        public static NWDVerbose None = AddNone("not Verbose");
        // string will be convert in UNIX format automatically
        public static NWDVerbose Sample = Add(2, "NWD_VERBOSE", "Verbose");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // You can create custom enum of macro
    // Just follow this example class
    public class NWDLog : MDEDataTypeEnumGeneric<NWDLog>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of enum controller
        public static string Title = SetTitle("Log");
        //-------------------------------------------------------------------------------------------------------------
        // declare all values
        public static NWDLog None = AddNone("no Log");
        // string will be convert in UNIX format automatically
        public static NWDLog Sample = Add(2, "NWD_LOG", "with Log");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // You can create custom enum of macro
    // Just follow this example class
    public class NWDBenchmark : MDEDataTypeEnumGeneric<NWDBenchmark>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of enum controller
        public static string Title = SetTitle("Benchmark");
        //-------------------------------------------------------------------------------------------------------------
        // declare all values
        public static NWDBenchmark None = AddNone("no Benchmark");
        // string will be convert in UNIX format automatically
        public static NWDBenchmark Sample = Add(2, "NWD_BENCHMARK", "with Benchmark");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif