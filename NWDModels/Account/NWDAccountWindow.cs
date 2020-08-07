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
        }
    )]
    public class NWDAccountWindow : NWDBasisWindow<NWDAccountWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Account/All", false, 300)]
        public static void MenuMethod()
        {
            ShowWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Account/Information", false, 301)]
        public static void MenuMethodInformation()
        {
            ShowWindow(typeof(NWDAccountInfos));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Account/Sign", false, 302)]
        public static void MenuMethodSign()
        {
            ShowWindow(typeof(NWDAccountSign));
        }
        //-------------------------------------------------------------------------------------------------------------
#if NWD_ACCOUNT_IDENTITY
        [MenuItem(NWDConstants.K_MENU_BASE + "Account/Nickname", false, 303)]
        public static void MenuMethodNickname()
        {
            ShowWindow(typeof(NWDAccountNickname));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Account/Avatar", false, 304)]
        public static void MenuMethodAvatar()
        {
            ShowWindow(typeof(NWDAccountAvatar));
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Account/Preference", false, 305)]
        public static void MenuMethodPreference()
        {
            ShowWindow(typeof(NWDAccountPreference));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif