//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDAppLocalizationWindow show NWDBasisWindow for localization NWDBasis Class.
    /// </summary>
    [NWDTypeWindowParamAttribute("Localization",
                                 "Localize your meassage, error, UI by Localization reference. (Use Autolocalized script)",
                                 "NWDLocalizationWindow",
        new Type[] {
            typeof(NWDLocalization),
            typeof(NWDError),
            typeof(NWDMessage),
            /* Add NWDBasis here*/
        }
    )]
    public class NWDLocalizationWindow : NWDBasisWindow<NWDLocalizationWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "App/Localization, Error and Message", false, 201)]
        public static void MenuMethod()
        {
            EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDLocalizationWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Game/Localization, Error and Message", false, 223)]
        public static void MenuMethodBis()
        {
            EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDLocalizationWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
