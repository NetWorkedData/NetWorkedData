//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Debug and Benchmark only for this file!
#if UNITY_EDITOR
//#define NET_WORKED_DATA_DEBUG
//#define NET_WORKED_DATA_BENCHMARK
#elif DEBUG
//#define NET_WORKED_DATA_DEBUG
//#define NET_WORKED_DATA_BENCHMARK
#else
//#define NET_WORKED_DATA_DEBUG
//#define NET_WORKED_DATA_BENCHMARK
#endif
//=====================================================================================================================

using System;
using System.Diagnostics;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// This class is used to benchmark the performance of NetWorkedData engine 
    /// </summary>
    public static class NWDEngineBenchmark
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// The Watch instance
        /// </summary>
        public static Stopwatch Watch = new Stopwatch();
        /// <summary>
        ///  The reference for launching engine
        /// </summary>
        public static long WatchEngineLaunch;
        /// <summary>
        /// The reference for launching and loading editor data
        /// </summary>
        public static long WatchEditorLaunch;
        /// <summary>
        /// The reference for launching and loading account data
        /// </summary>
        public static long WatchAccountLaunch;
        /// <summary>
        /// The reference for launching and loading all data
        /// </summary>
        public static long WatchFinalLaunch;
        //-------------------------------------------------------------------------------------------------------------
        static string GetWatchEngineLaunch()
        {
            if (WatchEngineLaunch > 0)
            {
                return (WatchEngineLaunch / 1000.0F).ToString("F3") + " seconds";
            }
            else
            {
                return "??? seconds";
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static string GetWatchEditorLaunch()
        {
            if (WatchEditorLaunch > 0)
            {
                return ((WatchEditorLaunch - WatchEngineLaunch) / 1000.0F).ToString("F3") + " seconds";
            }
            else
            {
                return "??? seconds";
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static string GetWatchAccountLaunch()
        {
            if (WatchAccountLaunch > 0)
            {
                return ((WatchAccountLaunch - WatchEditorLaunch) / 1000.0F).ToString("F3") + " seconds";
            }
            else
            {
                return "??? seconds";
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static string GetWatchFinalLaunch()
        {
            if (WatchFinalLaunch > 0)
            {
                return ((WatchFinalLaunch - WatchAccountLaunch) / 1000.0F).ToString("F3") + " seconds";
            }
            else
            {
                return "??? seconds";
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static string GetWatchTotalLaunch()
        {
            if (WatchFinalLaunch > 0)
            {
                return ((WatchFinalLaunch) / 1000.0F).ToString("F3") + " seconds";
            }
            else
            {
                return "??? seconds";
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetWatch()
        {
            long RowEditor = 0;
            foreach (Type tType in NWDDataManager.SharedInstance().ClassInEditorDatabaseList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                RowEditor += tHelper.Datas.Count;
            }
            long RowDevice = 0;
            foreach (Type tType in NWDDataManager.SharedInstance().ClassInDeviceDatabaseList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                RowDevice += tHelper.Datas.Count;
            }

            string rReturn = "Engine:" + GetWatchEngineLaunch() +
                " Editor datas:" + GetWatchEditorLaunch() + "(" + RowEditor + " rows)" +
                " Account datas:" + GetWatchAccountLaunch() + "(" + RowDevice + " rows) " +
                " Final:" + GetWatchFinalLaunch() +
                " Total:" + GetWatchTotalLaunch() +
                "";
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================