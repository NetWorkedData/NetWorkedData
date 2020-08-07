//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
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
        public static string Title = SetTitle("NWD Verbose");
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
        public static string Title = SetTitle("NWD Log");
        //-------------------------------------------------------------------------------------------------------------
        // declare all values
        public static NWDLog None = AddNone("no global Log");
        // string will be convert in UNIX format automatically
        public static NWDLog Sample = Add(2, "NWD_LOG", "with global Log");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // You can create custom enum of macro
    // Just follow this example class
    public class NWDBenchmark : MDEDataTypeEnumGeneric<NWDBenchmark>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of enum controller
        public static string Title = SetTitle("NWD Benchmark");
        //-------------------------------------------------------------------------------------------------------------
        // declare all values
        public static NWDBenchmark None = AddNone("no global Benchmark");
        // string will be convert in UNIX format automatically
        public static NWDBenchmark Sample = Add(2, "NWD_BENCHMARK", "with global Benchmark");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif