//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //-----------------------------------------------------------------------------------------------------------------
    [NWDTypeWindowParamAttribute("User",
        "User management … ",
        "NWDIcons_02",
        new Type[] {
            typeof(NWDAccount),
            typeof(NWDUserInfos),
            typeof(NWDUserNickname),
            typeof(NWDPreferences),
            typeof(NWDRelationship),
            typeof(NWDOwnership),
            typeof(NWDUsage),
            typeof(NWDUserStats),
            typeof(NWDUserConsolidatedStats),
            typeof(NWDInterUserMessage),
			/* Add NWDBasis here*/
		}
    )]
    //-----------------------------------------------------------------------------------------------------------------
    public class NWDUserWindow : NWDBasisWindow<NWDUserWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "User(s)" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 301)]
        //-------------------------------------------------------------------------------------------------------------
        public static void MenuMethod()
        {
            EditorWindow tWindow = GetWindow(typeof(NWDUserWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
#endif