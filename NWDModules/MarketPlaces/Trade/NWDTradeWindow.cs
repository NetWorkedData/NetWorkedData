//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

using UnityEngine;

using SQLite4Unity3d;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDTypeWindowParamAttribute("Trade",
        "Trade window description",
        new Type[] {
            typeof(NWDTradePlace),
            typeof(NWDUserTradeRequest),
            typeof(NWDUserTradeProposition),
            typeof(NWDUserTradeFinder),
			/* Add NWDBasis here*/
		}
    )]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDTradeWindow : NWDBasisWindow<NWDTradeWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Marketplaces/Trade" + NWDConstants.K_MENU_BASIS_WINDOWS_MANAGEMENT, false, 551)]
        public static void MenuMethod()
        {
            EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDTradeWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif