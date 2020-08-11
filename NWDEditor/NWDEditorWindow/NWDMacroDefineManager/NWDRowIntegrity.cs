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
    public class NWDRowIntegrity : MDEDataTypeEnumGeneric<NWDRowIntegrity>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of enum controller
        public static string Title = SetTitle("NWD Row integrity");
        public static string Group = SetGroup(MDEConstants.GroupEngine);
        public static int Order = SetOrder(5);
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