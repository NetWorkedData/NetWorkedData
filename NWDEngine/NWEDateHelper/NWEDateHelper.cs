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

using System;

//=====================================================================================================================
namespace NetWorkedData
{
	public class NWEDateHelper
	{
        //-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Convert a DateTime to an Unix Timestamp (since 1 january 1970)
        ///// </summary>
        ///// <param name="sDate">A DateTime</param>
        ///// <returns>A converted DateTime to Unix Timestamp.</returns>
        //public static double ConvertToUnixTimestamp(DateTime sDate)
        //{
        //    DateTime origin = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        //    TimeSpan diff = sDate.ToUniversalTime() - origin;
        //    return Math.Floor(diff.TotalSeconds);
        //}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Convert a DateTime to an Unix Timestamp (since 1 january 1970)
        /// </summary>
        /// <param name="sDate">A DateTime</param>
        /// <returns>A converted DateTime to Unix Timestamp.</returns>
        public static double ConvertToTimestamp(DateTime sDate)
        {
            return sDate.ToUniversalTime().Subtract(
                new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            ).TotalSeconds;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Converts from timestamp.
        /// </summary>
        /// <returns>The from timestamp.</returns>
        /// <param name="sTimeStamp">S time stamp.</param>
        public static DateTime ConvertFromTimestamp(double sTimeStamp)
        {
            DateTime rDateTime = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            rDateTime = rDateTime.AddSeconds(sTimeStamp);
            return rDateTime;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
