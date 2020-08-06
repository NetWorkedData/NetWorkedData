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
    public class NWDRGPDMacro : MDEDataTypeBoolGeneric<NWDRGPDMacro>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of enum controller
        public static string Title = SetTitle("NWD RGPD");
        //-------------------------------------------------------------------------------------------------------------
        // declare one value
        public static NWDRGPDMacro Macro = SetValue("NWD_RGPD");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // You can create custom enum of macro
    // Just follow this example class
    public class NWDCraftbookMacro : MDEDataTypeBoolGeneric<NWDCraftbookMacro>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of enum controller
        public static string Title = SetTitle("NWD Craftbook");
        //-------------------------------------------------------------------------------------------------------------
        // declare one value
        public static NWDCraftbookMacro Macro = SetValue("NWD_CRAFTBOOK");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // You can create custom enum of macro
    // Just follow this example class
    public class NWDInterMessageMacro : MDEDataTypeBoolGeneric<NWDInterMessageMacro>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of enum controller
        public static string Title = SetTitle("NWD InterMessage");
        //-------------------------------------------------------------------------------------------------------------
        // declare one value
        public static NWDInterMessageMacro Macro = SetValue("NWD_INTERMESSAGE");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif