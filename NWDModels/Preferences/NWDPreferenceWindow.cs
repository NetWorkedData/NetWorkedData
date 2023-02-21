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
        const string K_PREFERENCES_MENU = NWDEditorMenu.K_NETWORKEDDATA + "Preferences" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT;
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_PREFERENCES_MENU + "/Preferences Key", false, 803)]
        public static void MenuMethodPreferenceKey()
        {
            ShowWindow(typeof(NWDPreferenceKey));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_PREFERENCES_MENU + "/Account Preferences", false, 804)]
        public static void MenuMethodAccountPreferences()
        {
            ShowWindow(typeof(NWDAccountPreference));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_PREFERENCES_MENU + "/User Preferences", false, 805)]
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
