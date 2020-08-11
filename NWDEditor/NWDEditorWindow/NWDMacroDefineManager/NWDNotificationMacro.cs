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
    public class NWDNotificationMacro : MDEDataTypeEnumGeneric<NWDNotificationMacro>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of enum controller
        public static string Title = SetTitle("NWD CRUD Notification");
        public static string Group = SetGroup(MDEConstants.GroupEngine);
        public static int Order = SetOrder(11);
        //-------------------------------------------------------------------------------------------------------------
        // declare all values
        public static NWDNotificationMacro None = AddNone("without CRUD notification");
        // string will be convert in UNIX format automatically
        public static NWDNotificationMacro Sample = Add(1, "NWD_CRUD_NOTIFICATION", "with CRUD notification");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif