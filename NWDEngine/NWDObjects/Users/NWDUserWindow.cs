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
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDTypeWindowParamAttribute("Users",
                                 "Users",
                                 "NWDUserWindow", // NWDUserWindow
        new Type[] {
            typeof(NWDUserInfos),
            typeof(NWDUserNickname),
            typeof(NWDUserAvatar),
            typeof(NWDUserPreference),
            typeof(NWDUserNetWorking),
            typeof(NWDUserOwnership),
            typeof(NWDUserInterMessage),
            typeof(NWDMessage),
		/* Add NWDBasis here*/
		}
                                )]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDUserWindow : NWDBasisWindow<NWDUserWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Users", false, 301)]
        //-------------------------------------------------------------------------------------------------------------
        public static void MenuMethod()
        {
            EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDUserWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif