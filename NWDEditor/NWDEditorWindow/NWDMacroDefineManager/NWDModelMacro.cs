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
        public static NWDRGPDMacro MacroBool = SetValue("NWD_RGPD");
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
        public static NWDCraftbookMacro MacroBool = SetValue("NWD_CRAFTBOOK");
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
        public static NWDInterMessageMacro MacroBool = SetValue("NWD_INTERMESSAGE");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // You can create custom enum of macro
    // Just follow this example class
    public class NWDIdentityMacro : MDEDataTypeEnumGeneric<NWDIdentityMacro>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of enum controller
        public static string Title = SetTitle("NWD Identity");
        //-------------------------------------------------------------------------------------------------------------
        // declare all values
        public static NWDIdentityMacro None = AddNone("no identity"); // It's optional
        // string will be convert in UNIX format automatically
        public static NWDIdentityMacro AccountIdentity = Add(1, "NWD_ACCOUNT_IDENTITY", "Global (account)");
        public static NWDIdentityMacro UserIdentity = Add(2, "NWD_USER_IDENTITY", "by GameSave (user)");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif