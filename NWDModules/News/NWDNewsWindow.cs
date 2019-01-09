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
    /// <summary>
    /// NWDAppLocalizationWindow show NWDBasisWindow for localization NWDBasis Class.
    /// </summary>
    [NWDTypeWindowParamAttribute("News Message",
                                 "News Manager",
                                 "NWDNewsWindow",
        new Type[] {
            typeof(NWDNews),
            typeof(NWDUserNewsRead),
            /* Add NWDBasis here*/
        }
    )]
    public class NWDNewsWindow : NWDBasisWindow<NWDNewsWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "App/News Manager", false, 201)]
        public static void MenuMethod()
        {
            EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDNewsWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Game/News Manager", false, 223)]
        public static void MenuMethodBis()
        {
            EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDNewsWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
