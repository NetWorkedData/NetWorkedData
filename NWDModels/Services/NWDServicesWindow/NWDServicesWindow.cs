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
    
    // You can create custom enum of macro
    // Just follow this example class
    public class NWD_SERVICES_Macro : MDEDataTypeBoolGeneric<NWD_SERVICES_Macro>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of bool controller
        public static string Title = SetTitle("NWD_SERVICES");
        public static string Group = SetGroup(MDEConstants.GroupOptions);
        public static int Order = SetOrder(0);
        //-------------------------------------------------------------------------------------------------------------
        // declare one value
        public static NWD_SERVICES_Macro MacroBool = SetValue("NWD_SERVICES");
        //-------------------------------------------------------------------------------------------------------------
    }
    
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    #if NWD_SERVICES
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDTypeWindowParamAttribute("Services",
        "Services Management",
        new Type[] {
        typeof(NWDServiceKey),
        typeof(NWDAccountService),
		/* Add NWDBasis here*/
})]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDServicesWindow : NWDBasisWindow<NWDServicesWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem (NWDEditorMenu.K_NETWORKEDDATA + "Services" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, NWDEditorMenu.K_MODULES_MANAGEMENT_INDEX + 1)]
        public static void MenuMethod()
        {
            ShowWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    #endif //NWD_SERVICES
}
//=====================================================================================================================
#endif
