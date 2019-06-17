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
    [NWDTypeWindowParamAttribute("InApp",
                                 "NWDInAppPurchaseWindow window description",
        new Type[] {
            typeof(NWDShop),
            typeof(NWDRack),
            typeof(NWDPack),
            typeof(NWDItemPack),
            typeof(NWDItem),
            typeof(NWDInAppPack),
            typeof(NWDUserTransaction),
        })]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDInAppPurchaseWindow : NWDBasisWindow<NWDInAppPurchaseWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Marketplaces/In App Purchase", false, 570)]
        public static void MenuMethod()
        {
            EditorWindow tWindow = GetWindow(typeof(NWDInAppPurchaseWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif