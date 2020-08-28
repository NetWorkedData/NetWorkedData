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
//=====================================================================================================================
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
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
#if NWD_USER_NETWORKING
            typeof(NWDUserNetWorking),
#endif
            typeof(NWDBasisPreferences),
            typeof(NWDRequestToken),
            typeof(NWDIPBan),
        }
    )]
    public class NWDServerWindow : NWDBasisWindow<NWDServerWindow>
    {
//        //-------------------------------------------------------------------------------------------------------------
//        const string K_SERVER_MENU = "Cluster configuration";
//        //-------------------------------------------------------------------------------------------------------------
//        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_SERVER_MENU + "/Cluster sizer", false, 60)]
//        public static void ClusterSizerMenuMethod()
//        {
//            NWDClusterSizer.SharedInstanceFocus();
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_SERVER_MENU + "/Cluster", false, 60)]
//        public static void ClusterMenuMethod()
//        {
//            ShowWindow(typeof(NWDCluster));
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_SERVER_MENU + "/Server", false, 60)]
//        public static void ServerMenuMethod()
//        {
//            ShowWindow(typeof(NWDServer));
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_SERVER_MENU + "/Servers Domain", false, 80)]
//        public static void MenuMethod()
//        {
//            ShowWindow(typeof(NWDServerDomain));
//        }
//        //-------------------------------------------------------------------------------------------------------------        
//        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_SERVER_MENU + "/Servers Services", false, 81)]
//        public static void MenuMethodServices()
//        {
//            ShowWindow(typeof(NWDServerServices));
//        }
//        //-------------------------------------------------------------------------------------------------------------        
//        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_SERVER_MENU + "/Servers Datas", false, 81)]
//        public static void MenuMethodDatas()
//        {
//            ShowWindow(typeof(NWDServerDatas));
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        //[MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_SERVER_MENU + "/Basis Preference", false, 100)]
//        //public static void MenuMethodBasicPreference()
//        //{
//        //    ShowWindow(typeof(NWDBasisPreferences));
//        //}
//        //-------------------------------------------------------------------------------------------------------------
//#if NWD_USER_NETWORKING
//        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_SERVER_MENU + "/NetWorking", false, 101)]
//        public static void MenuMethodNetWorking()
//        {
//            ShowWindow(typeof(NWDUserNetWorking));
//        }
//#endif
//        //-------------------------------------------------------------------------------------------------------------
//        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_SERVER_MENU + "/Token", false, 182)]
//        public static void MenuMethodToken()
//        {
//            ShowWindow(typeof(NWDRequestToken));
//        }
//        //-------------------------------------------------------------------------------------------------------------        
//        [MenuItem(NWDEditorMenu.K_NETWORKEDDATA + K_SERVER_MENU + "/IP BAN", false, 183)]
//        public static void MenuMethodIPBAN()
//        {
//            ShowWindow(typeof(NWDIPBan));
//        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif