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
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDTypeWindowParamAttribute(
        "Users",
        "Users",
        new Type[] {
            typeof(NWDUserInfos),
            //typeof(NWDUserNickname),
            //typeof(NWDUserAvatar),
            typeof(NWDUserPreference),
            typeof(NWDUserOwnership),
#if NWD_MODULE_MARKETPLACES
            //typeof(NWDUserTransaction),
#endif
            //typeof(NWDGameSave),
		}
    )]
    public partial class NWDUserWindow : NWDBasisWindow<NWDUserWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "User/Information", false, 300)]
        public static void MenuMethod()
        {
            ShowWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "User/Preference", false, 303)]
        public static void MenuMethodPreference()
        {
            ShowWindow(typeof(NWDUserPreference));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "User/Ownership", false, 304)]
        public static void MenuMethodOwnership()
        {
            ShowWindow(typeof(NWDUserOwnership));
        }
        //-------------------------------------------------------------------------------------------------------------
#if NWD_MODULE_MARKETPLACES
        [MenuItem(NWDConstants.K_MENU_BASE + "User/Transaction", false, 305)]
        public static void MenuMethodTransaction()
        {
            //ShowWindow(typeof(NWDUserTransaction));
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "User/Game Save", false, 307)]
        public static void MenuMethodGameSave()
        {
            ShowWindow(typeof(NWDGameSave));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif