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
using UnityEditor;
using System;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDTypeWindowParamAttribute(
        "Account",
        "Account",
        new Type[] {
            typeof(NWDAccount),
            typeof(NWDAccountInfos),
            typeof(NWDAccountSign),
#if NWD_ACCOUNT_IDENTITY
            typeof(NWDAccountNickname),
            typeof(NWDAccountAvatar),
#endif
            typeof(NWDAccountPreference),
            typeof(NWDGameSave),
#if NWD_DEVELOPER
            typeof(NWDBasisPreferences),
            typeof(NWDRequestToken),
#endif
        }
    )]
    public class NWDAccountWindow : NWDBasisWindow<NWDAccountWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_ACCOUNT_MANAGEMENT = "Account" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT;
        //-------------------------------------------------------------------------------------------------------------
        //[MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_ACCOUNT_MANAGEMENT, false, NWDEditorMenu.K_PLAYER_MANAGEMENT_INDEX+1)]
        public static void MenuMethod()
        {
            ShowWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_ACCOUNT_MANAGEMENT + "/Information", false, NWDEditorMenu.K_PLAYER_MANAGEMENT_INDEX + 2)]
        public static void MenuMethodInformation()
        {
            ShowWindow(typeof(NWDAccountInfos));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_ACCOUNT_MANAGEMENT + "/Preferences", false, NWDEditorMenu.K_PLAYER_MANAGEMENT_INDEX + 3)]
        public static void MenuMethodPreference()
        {
            ShowWindow(typeof(NWDAccountPreference));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_ACCOUNT_MANAGEMENT + "/Sign", false, NWDEditorMenu.K_PLAYER_MANAGEMENT_INDEX + 4)]
        public static void MenuMethodSign()
        {
            ShowWindow(typeof(NWDAccountSign));
        }
        //-------------------------------------------------------------------------------------------------------------
#if NWD_ACCOUNT_IDENTITY
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_ACCOUNT_MANAGEMENT + "/Avatar", false, NWDEditorMenu.K_PLAYER_MANAGEMENT_INDEX + 20)]
        public static void MenuMethodAvatar()
        {
            ShowWindow(typeof(NWDAccountAvatar));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_ACCOUNT_MANAGEMENT + "/Nickname", false, NWDEditorMenu.K_PLAYER_MANAGEMENT_INDEX + 21)]
        public static void MenuMethodNickname()
        {
            ShowWindow(typeof(NWDAccountNickname));
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
