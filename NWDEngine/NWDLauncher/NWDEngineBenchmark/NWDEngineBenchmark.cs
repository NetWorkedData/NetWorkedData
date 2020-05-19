//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System.Diagnostics;

//=====================================================================================================================
namespace NetWorkedData
{
   
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static class NWDEngineBenchmark
    {
        //-------------------------------------------------------------------------------------------------------------
        public static Stopwatch Watch = new Stopwatch();
        public static long WatchEngineLaunch;
        public static long WatchEditorLaunch;
        public static long WatchAccountLaunch;
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
            return "Engine:"+ GetWatchEngineLaunch() + " Editor datas:"+ GetWatchEditorLaunch() + " Account datas:" + GetWatchAccountLaunch() + " Final:" + GetWatchFinalLaunch() + " Total:" + GetWatchTotalLaunch();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================