//=====================================================================================================================
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
//=====================================================================================================================

#if UNITY_EDITOR
using System;
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDTypeWindowParamAttribute(
            "RGPD",
            "Consent Managment",
            "NWDConsentWindow",
        new Type[] {
            typeof(NWDConsent),
            typeof(NWDAccountConsent),
		}
    )]
    public class NWDConsentWindow : NWDBasisWindow<NWDConsentWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "App/RGPD", false, 250)]
        public static void MenuMethod()
        {
            ShowWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif