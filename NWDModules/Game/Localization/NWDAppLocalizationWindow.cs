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
    [NWDTypeWindowParamAttribute("App Localization",
        "Project Edition, You can add, change, remove the item of your game here. " +
        "Everythings can be item : money, gold, dress. The item can be win, buy in the pack, etc.",
        "NWDIcons_02",
        new Type[] {
            typeof(NWDLocalization),
            typeof(NWDError),
            typeof(NWDMessage),
            /* Add NWDBasis here*/
        }
    )]
    public class NWDAppLocalizationWindow : NWDBasisWindow<NWDAppWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Game/Localization, Error and Message", false, 300)]
        //-------------------------------------------------------------------------------------------------------------
        public static void MenuMethod()
        {
            EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDAppLocalizationWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
