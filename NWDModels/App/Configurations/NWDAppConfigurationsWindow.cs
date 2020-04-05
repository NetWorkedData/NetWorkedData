//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:37
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
        "App Configuration",
        "Project Edition, You can add, change, remove the item of your game here. " +
        "Everythings can be item : money, gold, dress. The item can be win, buy in the pack, etc.",
        new Type[] {
            typeof(NWDVersion),
            typeof(NWDPreferenceKey),
            typeof(NWDParameter),
            typeof(NWDError),
            typeof(NWDAssetBundle),
		}
    )]
    public class NWDAppWindow : NWDBasisWindow<NWDAppWindow>
    {
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "App/Version", false, 200)]
        public static void MenuMethod()
        {
            ShowWindow();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "App/Preference Key", false, 201)]
        public static void MenuMethodPreferenceKey()
        {
            ShowWindow(typeof(NWDPreferenceKey));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "App/Parameter", false, 202)]
        public static void MenuMethodParameter()
        {
            ShowWindow(typeof(NWDParameter));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "App/Error", false, 203)]
        public static void MenuMethodError()
        {
            ShowWindow(typeof(NWDError));
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "App/Asset Bundle", false, 220)]
        public static void MenuMethodAssetBundle()
        {
            ShowWindow(typeof(NWDAssetBundle));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
