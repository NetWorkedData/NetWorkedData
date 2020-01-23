//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:17
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using UnityEditor;
using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDTypeWindowParamAttribute(
        "InApp",
        "NWDInAppPurchaseWindow window description",
        new Type[] {
            typeof(NWDInAppPack),
#if NWD_MODULE_MARKETPLACES
            //typeof(NWDShop),
            //typeof(NWDRack),
#endif
            typeof(NWDPack),
            typeof(NWDItemPack),
            typeof(NWDItem),
        })]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDInAppPurchaseWindow : NWDBasisWindow<NWDInAppPurchaseWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "In App", false, 535)]
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