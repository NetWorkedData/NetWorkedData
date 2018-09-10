//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
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
                                 "NWDInAppPurchaseWindow",
        new Type[] {
            typeof(NWDShop),
            typeof(NWDRack),
            typeof(NWDPack),
            typeof(NWDItemPack),
            typeof(NWDItem),
            typeof(NWDInAppPack),
            typeof(NWDTransaction),
			/* Add NWDBasis here*/
		}
                                 )]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDInAppPurchaseWindow : NWDBasisWindow<NWDInAppPurchaseWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Marketplaces/In App Purchase", false, 570)]
        //-------------------------------------------------------------------------------------------------------------
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