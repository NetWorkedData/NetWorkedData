//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
#define NWD_LOG
#define NWD_BENCHMARK
#endif
//=====================================================================================================================
using UnityEngine;
using System.IO;
using System;
using System.Diagnostics;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDDebug is class to write Logs.
    /// </summary>
    public static class NWDDebug
    {
        //-------------------------------------------------------------------------------------------------------------
        const string MACRO = "NWD_LOG";
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return if logs are active or not 
        /// </summary>
        /// <returns></returns>
        public static bool IsActivated()
        {
            if (NWDAppEnvironment.SelectedEnvironment().LogMode != NWDEnvironmentLogMode.NoLog)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Log the specified sString.
        /// </summary>
        /// <param name="sString">S string.</param>
        /// <param name="sThis"></param>
        [Conditional(MACRO)]
        public static void Log(string sString, UnityEngine.Object sThis = null)
        {
            if (NWDAppEnvironment.SelectedEnvironment().LogMode != NWDEnvironmentLogMode.NoLog)
            {
                WriteConsole(sString, false, sThis);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Force log the specified sString.
        /// </summary>
        /// <param name="sString">S string.</param>
        /// <param name="sThis"></param>
        public static void ForceLog(string sString, UnityEngine.Object sThis = null)
        {
            WriteConsole(sString, false, sThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Log Warning the specified sString.
        /// </summary>
        /// <param name="sString">S string.</param>
        /// <param name="sThis"></param>
        //[Conditional("NWDDEBUG_LOGMODE")]
        public static void Warning(string sString, UnityEngine.Object sThis = null)
        {
            WriteConsole(sString, true, sThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Write in Console or in File
        /// </summary>
        /// <param name="sString"></param>
        /// <param name="sWarning"></param>
        /// <param name="sThis"></param>
        private static void WriteConsole(string sString, bool sWarning = false, UnityEngine.Object sThis = null)
        {
                if (sWarning == true)
                {
                    if (sThis != null)
                    {
                        UnityEngine.Debug.LogWarning(NWDAppEnvironment.SelectedEnvironment().Environment + ": " + sString, sThis);
                    }
                    else
                    {
                        UnityEngine.Debug.LogWarning(NWDAppEnvironment.SelectedEnvironment().Environment + ": " + sString);
                    }
                }
                else
                {
                    if (sThis != null)
                    {
                        UnityEngine.Debug.Log(NWDAppEnvironment.SelectedEnvironment().Environment + ": " + sString, sThis);
                    }
                    else
                    {
                        UnityEngine.Debug.Log(NWDAppEnvironment.SelectedEnvironment().Environment + ": " + sString);
                    }
                }
                if (NWDAppEnvironment.SelectedEnvironment().LogMode == NWDEnvironmentLogMode.LogInFile)
                {
                    string tFileDebug = "\r\n";
                    if (sWarning == true)
                    {
                        tFileDebug += "WARNING\r\n";
                    }
                    tFileDebug += NWDAppEnvironment.SelectedEnvironment().Environment + ": " + sString;
                    tFileDebug = tFileDebug.Replace(",\"", ",\r\n\"").Replace("{", "\r\n{\r\n").Replace("}", "\r\n}\r\n").Replace("\r\n}\r\n,\r\n", "\r\n},\r\n");
                    tFileDebug = NWDToolbox.CSharpFormat(tFileDebug);
                    string tPath = Application.persistentDataPath + "/WEBLOG-" + DateTime.Now.ToString("yyyy'-'MM'-'dd'_'HH'-'mm'-'ss") + ".txt";
                    File.AppendAllText(tPath, tFileDebug);
                }
#if UNITY_EDITOR
                if (NWDProjectPrefs.GetBool(NWDConstants.K_EDITOR_CLIPBOARD_LAST_LOG) == true)
                {
                    NWEClipboard.CopyToClipboard(sString);
                }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
