//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:33:50
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
            typeof(NWDUserInterMessage),
            /* Add NWDBasis here*/
        }
    )]
    public class NWDLocalizationWindow : NWDBasisWindow<NWDLocalizationWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "App/Localization, Error and Message", false, 201)]
        public static void MenuMethod()
        {
            ShowWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Game/Localization, Error and Message", false, 223)]
        public static void MenuMethodBis()
        {
            ShowWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
