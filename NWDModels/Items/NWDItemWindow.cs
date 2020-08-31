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
#if UNITY_EDITOR
using System;
//=====================================================================================================================
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDTypeWindowParamAttribute(
        "Items",
        "Items",
        new Type[] {
            typeof(NWDItem),
            typeof(NWDItemGroup),
            typeof(NWDUserOwnership),
#if NWD_ITEMPACK
            typeof(NWDPack),
            typeof(NWDItemPack),
#endif
        }
    )]
    public partial class NWDItemWindow : NWDBasisWindow<NWDItemWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + "Items" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 1001)]
        public static void MenuMethod()
        {
            ShowWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif