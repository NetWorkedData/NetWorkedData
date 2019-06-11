// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:27
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
        typeof(NWDAccountSign),
        typeof(NWDAccountNickname),
        typeof(NWDAccountAvatar),
        typeof(NWDAccountPreference),
        typeof(NWDAccountConsent),
        typeof(NWDRequestToken),
        typeof(NWDBasisPreferences),
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