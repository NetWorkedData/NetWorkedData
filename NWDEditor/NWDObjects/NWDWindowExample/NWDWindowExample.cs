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
#endif
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.MacroDefine
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /* MACRO_SCRIPT
    // You can create custom enum of macro
    // Just follow this example class
    public class NWD_EXAMPLE_MACRO_Macro : MDEDataTypeBoolGeneric<NWD_EXAMPLE_MACRO_Macro>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of bool controller
        public static string Title = SetTitle("NWD_EXAMPLE_MACRO");
        public static string Group = SetGroup(MDEConstants.GroupOptions);
        public static int Order = SetOrder(0);
        //-------------------------------------------------------------------------------------------------------------
        // declare one value
        public static NWD_EXAMPLE_MACRO_Macro MacroBool = SetValue("NWD_EXAMPLE_MACRO");
        //-------------------------------------------------------------------------------------------------------------
    }
    MACRO_SCRIPT */
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //MACRO_DEFINE #if NWD_EXAMPLE_MACRO
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDTypeWindowParamAttribute("NWDWindowExample_Name",
        "NWDWindowExample_Description",
        new Type[] {
        typeof(NWDExample),/* Add NWDBasis here*/
})]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDWindowExample : NWDBasisWindow<NWDWindowExample>
    {
        //-------------------------------------------------------------------------------------------------------------
        //[MenuItem (NWDEditorMenu.K_NETWORKEDDATA + "NWDWindowExample_Name" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, NWDEditorMenu.K_CUSTOMS_MANAGEMENT_INDEX + 1)]
        public static void MenuMethod()
        {
            ShowWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //MACRO_DEFINE #endif //NWD_EXAMPLE_MACRO
}
//=====================================================================================================================
#endif
