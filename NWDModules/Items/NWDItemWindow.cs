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
    [NWDTypeWindowParamAttribute("Items",
                                 "Items",
                                 "NWDItemWindow",
        new Type[] {
            typeof(NWDItem),
            typeof(NWDItemRarity),
            typeof(NWDItemProperty),
            typeof(NWDItemGroup),
            typeof(NWDUserOwnership),
            typeof(NWDItemSlot),
            typeof(NWDUserItemSlot),
			/* Add NWDBasis here*/
		}
    )]
    public class NWDItemWindow : NWDBasisWindow<NWDItemWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Items", false, 530)]
        //-------------------------------------------------------------------------------------------------------------
        public static void MenuMethod()
        {
            EditorWindow tWindow = EditorWindow.GetWindow(typeof(NWDItemWindow));
            tWindow.Show();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif