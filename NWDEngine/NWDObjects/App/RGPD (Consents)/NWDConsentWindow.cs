// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:34:3
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
    [NWDTypeWindowParamAttribute("RGPD",
        "Consent Managment",
        "NWDConsentWindow", // NWDConsentWindow_ICON
        new Type[] {
        typeof(NWDConsent),
        typeof(NWDAccountConsent),
		/* Add NWDBasis here*/
		}
    )]
    public class NWDConsentWindow : NWDBasisWindow<NWDConsentWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "App/RGPD Consent", false, 250)]
        public static void MenuMethod()
        {
            EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDConsentWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif