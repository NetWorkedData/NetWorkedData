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
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData.MacroDefine
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // You can create custom enum of macro
    // Just follow this example class
    public class NWDRGPDMacro : MDEDataTypeBoolGeneric<NWDRGPDMacro>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of bool controller
        public static string Title = SetTitle("NWD RGPD");
        public static string Group = SetGroup(MDEConstants.GroupOptions);
        public static int Order = SetOrder(0);
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
        // the title of bool controller
        public static string Title = SetTitle("NWD Craftbook");
        public static string Group = SetGroup(MDEConstants.GroupModule);
        public static int Order = SetOrder(0);
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
        // the title of bool controller
        public static string Title = SetTitle("NWD InterMessage");
        public static string Group = SetGroup(MDEConstants.GroupModule);
        public static int Order = SetOrder(0);
        //-------------------------------------------------------------------------------------------------------------
        // declare one value
        public static NWDInterMessageMacro MacroBool = SetValue("NWD_INTERMESSAGE");
        //-------------------------------------------------------------------------------------------------------------
    }
    ////+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //// You can create custom enum of macro
    //// Just follow this example class
    //public class NWDIdentityMacro : MDEDataTypeEnumGeneric<NWDIdentityMacro>
    //{
    //    //-------------------------------------------------------------------------------------------------------------
    //    // the title of enum controller
    //    public static string Title = SetTitle("NWD Identity");
    //    //-------------------------------------------------------------------------------------------------------------
    //    // declare all values
    //    public static NWDIdentityMacro None = AddNone("no identity"); // It's optional
    //    // string will be convert in UNIX format automatically
    //    public static NWDIdentityMacro AccountIdentity = Add(1, "NWD_ACCOUNT_IDENTITY", "global (account)");
    //    public static NWDIdentityMacro UserIdentity = Add(2, "NWD_USER_IDENTITY", "by GameSave (user)");
    //    public static NWDIdentityMacro BothIdentity = Add(3, "NWD_BOTH_IDENTITY", "both", "NWD_USER_IDENTITY,NWD_ACCOUNT_IDENTITY");
    //    //-------------------------------------------------------------------------------------------------------------
    //}
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // You can create custom bool of macro
    // Just follow this example class
    public class NWDUserIdentityMacro : MDEDataTypeBoolGeneric<NWDUserIdentityMacro>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of bool controller
        public static string Title = SetTitle("NWD User Identity");
        public static string Group = SetGroup(MDEConstants.GroupOptions);
        public static int Order = SetOrder(3);
        //-------------------------------------------------------------------------------------------------------------
        // declare one value
        public static NWDUserIdentityMacro MacroBool = SetValue("NWD_USER_IDENTITY");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // You can create custom bool of macro
    // Just follow this example class
    public class NWDAccountIdentityMacro : MDEDataTypeBoolGeneric<NWDAccountIdentityMacro>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of bool controller
        public static string Title = SetTitle("NWD Account Identity");
        public static string Group = SetGroup(MDEConstants.GroupOptions);
        public static int Order = SetOrder(2);
        //-------------------------------------------------------------------------------------------------------------
        // declare one value
        public static NWDAccountIdentityMacro MacroBool = SetValue("NWD_ACCOUNT_IDENTITY");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // You can create custom bool of macro
    // Just follow this example class
    public class NWDClassificationMacro : MDEDataTypeBoolGeneric<NWDClassificationMacro>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of bool controller
        public static string Title = SetTitle("NWD Classification");
        public static string Group = SetGroup(MDEConstants.GroupModule);
        public static int Order = SetOrder(0);
        //-------------------------------------------------------------------------------------------------------------
        // declare one value
        public static NWDClassificationMacro MacroBool = SetValue("NWD_CLASSIFICATION");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // You can create custom bool of macro
    // Just follow this example class
    public class NWDItemPackMacro : MDEDataTypeBoolGeneric<NWDItemPackMacro>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of bool controller
        public static string Title = SetTitle("NWD ItemPack");
        public static string Group = SetGroup(MDEConstants.GroupModule);
        public static int Order = SetOrder(0);
        //-------------------------------------------------------------------------------------------------------------
        // declare one value
        public static NWDItemPackMacro MacroBool = SetValue("NWD_ITEMPACK");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // You can create custom bool of macro
    // Just follow this example class
    public class NWDUserNetWorkingMacro : MDEDataTypeBoolGeneric<NWDUserNetWorkingMacro>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of bool controller
        public static string Title = SetTitle("NWD User NetWorking");
        public static string Group = SetGroup(MDEConstants.GroupModule);
        public static int Order = SetOrder(0);
        //-------------------------------------------------------------------------------------------------------------
        // declare one value
        public static NWDUserNetWorkingMacro MacroBool = SetValue("NWD_USER_NETWORKING");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    // You can create custom bool of macro
    // Just follow this example class
    public class NWDQuestMacro : MDEDataTypeBoolGeneric<NWDQuestMacro>
    {
        //-------------------------------------------------------------------------------------------------------------
        // the title of bool controller
        public static string Title = SetTitle("NWD Quest");
        public static string Group = SetGroup(MDEConstants.GroupQuest);
        public static int Order = SetOrder(0);
        //-------------------------------------------------------------------------------------------------------------
        // declare one value
        public static NWDQuestMacro MacroBool = SetValue("NWD_QUEST");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif