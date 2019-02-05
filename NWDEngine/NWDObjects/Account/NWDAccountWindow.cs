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
    [NWDTypeWindowParamAttribute("Account",
                                 "Account",
        new Type[] {
        typeof(NWDAccount),
        typeof(NWDAccountInfos),
        typeof(NWDAccountNickname),
        typeof(NWDAccountAvatar),
        typeof(NWDAccountPreference),
        typeof(NWDAccountConsent),
        typeof(NWDRelationship),
		}
    )]
    public class NWDAccountWindow : NWDBasisWindow<NWDAccountWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Accounts", false, 300)]
        public static void MenuMethod()
        {
            EditorWindow tWindow = GetWindow(typeof(NWDAccountWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif