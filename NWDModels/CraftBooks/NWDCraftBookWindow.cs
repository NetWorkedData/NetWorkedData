//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:47:44
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
        "Craft",
        "Craft edition",
        new Type[] {
            typeof(NWDCraftBook),
            typeof(NWDCraftReward),
            typeof(NWDCraftRecipient),
            typeof(NWDItemGroup),
            typeof(NWDItem),
		}
    )]
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDCraftBookWindow : NWDBasisWindow<NWDCraftBookWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Craft/Recipe", false, 530)]
        public static void MenuMethod()
        {
            ShowWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Craft/Reward", false, 531)]
        public static void MenuMethodReward()
        {
            ShowWindow(typeof(NWDCraftReward));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Craft/Recipient", false, 532)]
        public static void MenuMethodRecipient()
        {
            ShowWindow(typeof(NWDCraftRecipient));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif