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
    [NWDTypeWindowParamAttribute("Credits",
        "Credits",
        "NWDIcons_02", // NWDCreditsWindow_ICON
        new Type[] {typeof(NWDCredits),
        typeof(NWDCreditsStuff),
        typeof(NWDCreditsCompany),
        typeof(NWDCreditsMember),
		/* Add NWDBasis here*/
		}
    )]
    public class NWDCreditsWindow : NWDBasisWindow<NWDCreditsWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Credits" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 2000)]
        //-------------------------------------------------------------------------------------------------------------
        public static void MenuMethod()
        {
            EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDCreditsWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif