//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:40
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
        "Items",
        "Items",
        "NWDItemWindow",
        new Type[] {
            typeof(NWDItem),
            typeof(NWDItemRarity),
            typeof(NWDItemGroup),
            typeof(NWDItemSlot),
		}
    )]
    public class NWDItemWindow : NWDBasisWindow<NWDItemWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Item/All", false, 500)]
        public static void MenuMethod()
        {
            ShowWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Item/Rarity", false, 501)]
        public static void MenuMethodRarity()
        {
            ShowWindow(typeof(NWDItemRarity));
        }
        //-------------------------------------------------------------------------------------------------------------        
        [MenuItem(NWDConstants.K_MENU_BASE + "Item/Group", false, 502)]
        public static void MenuMethodGroup()
        {
            ShowWindow(typeof(NWDItemGroup));
        }
        //-------------------------------------------------------------------------------------------------------------        
        [MenuItem(NWDConstants.K_MENU_BASE + "Item/Slot", false, 503)]
        public static void MenuMethodSlot()
        {
            ShowWindow(typeof(NWDItemSlot));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif