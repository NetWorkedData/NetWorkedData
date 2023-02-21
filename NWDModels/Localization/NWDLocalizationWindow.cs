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
    /// <summary>
    /// NWDAppLocalizationWindow show NWDBasisWindow for localization NWDBasis Class.
    /// </summary>
    [NWDTypeWindowParamAttribute(
        "Localization",
        "Localize your meassage, error, UI by Localization reference. (Use Autolocalized script)",
        new Type[] {
            typeof(NWDLocalization),
            typeof(NWDMessage),
            typeof(NWDError),
        }
    )]
    public partial class NWDLocalizationWindow : NWDBasisWindow<NWDLocalizationWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_PREFERENCES_MENU = NWDEditorMenu.K_NETWORKEDDATA + "Localization" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT;
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_PREFERENCES_MENU + "/Localization", false, NWDEditorMenu.K_ENGINE_MANAGEMENT_INDEX + 3)]
        public static void MenuMethodLocalization()
        {
            ShowWindow(typeof(NWDLocalization));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_PREFERENCES_MENU + "/Message", false, NWDEditorMenu.K_ENGINE_MANAGEMENT_INDEX + 3)]
        public static void MenuMethodMessage()
        {
            ShowWindow(typeof(NWDMessage));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_PREFERENCES_MENU + "/Error", false, NWDEditorMenu.K_ENGINE_MANAGEMENT_INDEX + 3)]
        public static void MenuMethodError()
        {
            ShowWindow(typeof(NWDError));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
