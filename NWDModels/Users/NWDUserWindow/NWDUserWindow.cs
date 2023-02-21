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
        "Users",
        "Users",
        new Type[] {
            typeof(NWDUserInfos),
            typeof(NWDUserPreference),
            typeof(NWDUserOwnership),
#if NWD_USER_IDENTITY
            typeof(NWDUserNickname),
            typeof(NWDUserAvatar),
#endif
#if NWD_INTERMESSAGE
            typeof(NWDUserInterMessage),
#endif
#if NWD_MODULE_MARKETPLACES
            typeof(NWDUserTransaction),
#endif
		}
    )]
    public partial class NWDUserWindow : NWDBasisWindow<NWDUserWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        const string K_PREFERENCES_MENU = NWDEditorMenu.K_NETWORKEDDATA + "User" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT;
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_PREFERENCES_MENU + "/Preferences", false, NWDEditorMenu.K_PLAYER_MANAGEMENT_INDEX + 50)]
        public static void MenuMethodPreference()
        {
            ShowWindow(typeof(NWDUserPreference));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_PREFERENCES_MENU + "/Game Save", false, NWDEditorMenu.K_PLAYER_MANAGEMENT_INDEX + 50)]
        public static void MenuMethodGameSave()
        {
            ShowWindow(typeof(NWDGameSave));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(K_PREFERENCES_MENU + "/Ownership", false, NWDEditorMenu.K_PLAYER_MANAGEMENT_INDEX + 50)]
        public static void MenuMethodOwnership()
        {
            ShowWindow(typeof(NWDUserOwnership));
        }
        //-------------------------------------------------------------------------------------------------------------
#if NWD_MODULE_MARKETPLACES
        [MenuItem(K_PREFERENCES_MENU +"/Transaction", false, NWDEditorMenu.K_PLAYER_MANAGEMENT_INDEX + 50)]
        public static void MenuMethodTransaction()
        {
            //ShowWindow(typeof(NWDUserTransaction));
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
