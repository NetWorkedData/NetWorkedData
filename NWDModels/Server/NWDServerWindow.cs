//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
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
        "Clusters",
        "Clusters",
        new Type[] {
            typeof(NWDCluster),
            typeof(NWDServer),
            typeof(NWDServerDomain),
            typeof(NWDServerServices),
            typeof(NWDServerDatas),
            typeof(NWDServerOther),
            typeof(NWDBasisPreferences),
            typeof(NWDRequestToken),
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