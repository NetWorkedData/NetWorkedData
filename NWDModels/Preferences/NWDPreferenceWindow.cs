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
using System;
#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDTypeWindowParamAttribute(
        "Preferences",
        "Preferences",
        new Type[] {
            typeof(NWDPreferenceKey),
            typeof(NWDAccountPreference),
            typeof(NWDUserPreference),
		}
    )]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDPreferenceWindow : NWDBasisWindow<NWDPreferenceWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_PREFERENCES_MENU = "Preferences" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT;
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_PREFERENCES_MENU, false, NWDEditorMenu.K_ENGINE_MANAGEMENT_INDEX + 2)]
        public static void MenuMethod()
        {
            ShowWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_PREFERENCES_MENU + "/Preferences Key", false, NWDEditorMenu.K_ENGINE_MANAGEMENT_INDEX + 3)]
        public static void MenuMethodPreferenceKey()
        {
            ShowWindow(typeof(NWDPreferenceKey));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_PREFERENCES_MENU + "/Account Preferences", false, NWDEditorMenu.K_ENGINE_MANAGEMENT_INDEX + 3)]
        public static void MenuMethodAccountPreferences()
        {
            ShowWindow(typeof(NWDAccountPreference));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_PREFERENCES_MENU + "/User Preferences", false, NWDEditorMenu.K_ENGINE_MANAGEMENT_INDEX + 3)]
        public static void MenuMethodUserPreferences()
        {
            ShowWindow(typeof(NWDUserPreference));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif