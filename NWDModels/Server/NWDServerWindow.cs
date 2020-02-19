﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:27
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
    /// <summary>
    /// 
    /// </summary>
    [NWDTypeWindowParamAttribute(
        "Servers",
        "Servers",
        new Type[] {
            typeof(NWDServerDomain),
            typeof(NWDServerServices),
            typeof(NWDServerDatas),
            typeof(NWDBasisPreferences),
            //typeof(NWDUserNetWorking),
            //typeof(NWDRequestToken),
            typeof(NWDIPBan),
        }
    )]
    public class NWDServerWindow : NWDBasisWindow<NWDServerWindow>
    {

        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Cluster Configuration/Cluster sizer", false, 60)]
        public static void ClusterSizerMenuMethod()
        {
            NWDClusterSizer.SharedInstanceFocus();
        }
        //-------------------------------------------------------------------------------------------------------------
        [MenuItem(NWDConstants.K_MENU_BASE + "Cluster Configuration/Servers Domain (DNS)", false, 80)]
        public static void MenuMethod()
        {
            ShowWindow(typeof(NWDServerDomain));
        }
        //-------------------------------------------------------------------------------------------------------------        
        [MenuItem(NWDConstants.K_MENU_BASE + "Cluster Configuration/Servers Services (WS)", false, 81)]
        public static void MenuMethodServices()
        {
            ShowWindow(typeof(NWDServerServices));
        }
        //-------------------------------------------------------------------------------------------------------------        
        [MenuItem(NWDConstants.K_MENU_BASE + "Cluster Configuration/Servers Datas (MySQL)", false, 81)]
        public static void MenuMethodDatas()
        {
            ShowWindow(typeof(NWDServerDatas));
        }
        //-------------------------------------------------------------------------------------------------------------        
        //[MenuItem(NWDConstants.K_MENU_BASE + "Cluster Configuration/Basis Preference", false, 100)]
        //public static void MenuMethodBasicPreference()
        //{
        //    ShowWindow(typeof(NWDBasisPreferences));
        //}
        //-------------------------------------------------------------------------------------------------------------        
        //[MenuItem(NWDConstants.K_MENU_BASE + "Cluster Configuration/NetWorking", false, 101)]
        //public static void MenuMethodNetWorking()
        //{
        //    ShowWindow(typeof(NWDUserNetWorking));
        //}
        //-------------------------------------------------------------------------------------------------------------        
        [MenuItem(NWDConstants.K_MENU_BASE + "Cluster Configuration/Token", false, 182)]
        public static void MenuMethodToken()
        {
            ShowWindow(typeof(NWDRequestToken));
        }
        //-------------------------------------------------------------------------------------------------------------        
        [MenuItem(NWDConstants.K_MENU_BASE + "Cluster Configuration/IP BAN", false, 183)]
        public static void MenuMethodIPBAN()
        {
            ShowWindow(typeof(NWDIPBan));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif