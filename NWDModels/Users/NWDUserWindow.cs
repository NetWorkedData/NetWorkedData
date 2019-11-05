//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:32
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
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
            typeof(NWDUserNickname),
            typeof(NWDUserAvatar),
            typeof(NWDUserPreference),
            typeof(NWDUserOwnership),
#if NWD_MODULE_MARKETPLACES
            //typeof(NWDUserTransaction),
#endif
            typeof(NWDUserInterMessage),
            typeof(NWDGameSave),
            typeof(NWDUserItemSlot),
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
        [MenuItem(NWDConstants.K_MENU_BASE + "User/Nickname", false, 301)]
        public static void MenuMethodNickname()
        {
            ShowWindow(typeof(NWDUserNickname));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "User/Avatar", false, 302)]
        public static void MenuMethodAvatar()
        {
            ShowWindow(typeof(NWDUserAvatar));
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
        [MenuItem(NWDConstants.K_MENU_BASE + "User/Inter Message", false, 306)]
        public static void MenuMethodInterMessage()
        {
            ShowWindow(typeof(NWDUserInterMessage));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "User/Game Save", false, 307)]
        public static void MenuMethodGameSave()
        {
            ShowWindow(typeof(NWDGameSave));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "User/Slot", false, 308)]
        public static void MenuMethodSlot()
        {
            ShowWindow(typeof(NWDUserItemSlot));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif