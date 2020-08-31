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
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDTypeWindowParamAttribute(
        "App Configuration",
        "Project Edition, You can add, change, remove the item of your game here. " +
        "Everythings can be item : money, gold, dress. The item can be win, buy in the pack, etc.",
        new Type[] {
            typeof(NWDVersion),
            typeof(NWDPreferenceKey),
            typeof(NWDParameter),
            typeof(NWDError),
            typeof(NWDAssetBundle),
		}
    )]
    public class NWDAppWindow : NWDBasisWindow<NWDAppWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_APP_MENU = "App" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT;
        //-------------------------------------------------------------------------------------------------------------
        //[MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_APP_MENU, false, NWDEditorMenu.K_ENGINE_MANAGEMENT_INDEX +1)]
        //public static void MenuMethod()
        //{
        //    ShowWindow();
        //}
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_APP_MENU + "/Version", false, NWDEditorMenu.K_ENGINE_MANAGEMENT_INDEX + 1)]
        public static void MenuMethodVersion()
        {
            ShowWindow(typeof(Version));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_APP_MENU + "/Preference Key", false, NWDEditorMenu.K_ENGINE_MANAGEMENT_INDEX + 1)]
        public static void MenuMethodPreferenceKey()
        {
            ShowWindow(typeof(NWDPreferenceKey));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_APP_MENU + "/Parameter", false, NWDEditorMenu.K_ENGINE_MANAGEMENT_INDEX + 1)]
        public static void MenuMethodParameter()
        {
            ShowWindow(typeof(NWDParameter));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_APP_MENU + "/Error", false, NWDEditorMenu.K_ENGINE_MANAGEMENT_INDEX + 1)]
        public static void MenuMethodError()
        {
            ShowWindow(typeof(NWDError));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_APP_MENU + "/Asset Bundle", false, NWDEditorMenu.K_ENGINE_MANAGEMENT_INDEX + 1)]
        public static void MenuMethodAssetBundle()
        {
            ShowWindow(typeof(NWDAssetBundle));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
