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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if NWD_CRAFTBOOK
#if UNITY_EDITOR
using System;
//=====================================================================================================================
using UnityEditor;
using NetWorkedData.NWDEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDTypeWindowParamAttribute(
        "Craft",
        "Craft edition",
        new Type[] {
            typeof(NWDCraftBook),
            typeof(NWDCraftReward),
            typeof(NWDCraftRecipient),
            typeof(NWDItemGroup),
            typeof(NWDItem),
        }
    )]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDCraftBookWindow : NWDBasisWindow<NWDCraftBookWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + "Craft" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 1003)]
        public static void MenuMethod()
        {
            ShowWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + "Craft" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT + "/Recipe CraftBook", false, 1003)]
        public static void MenuMethodRecipe()
        {
            ShowWindow(typeof(NWDCraftBook));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + "Craft" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT + "/Reward", false, 1003)]
        public static void MenuMethodReward()
        {
            ShowWindow(typeof(NWDCraftReward));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + "Craft" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT + "/Recipient", false, 1003)]
        public static void MenuMethodRecipient()
        {
            ShowWindow(typeof(NWDCraftRecipient));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
#endif
