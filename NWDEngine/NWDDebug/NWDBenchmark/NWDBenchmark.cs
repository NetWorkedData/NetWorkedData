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
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.Reflection;
using UnityEngine;
using System.Linq;


#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDBenchmark
    {
        //-------------------------------------------------------------------------------------------------------------
        const string MACRO = "NWD_BENCHMARK";
        //-------------------------------------------------------------------------------------------------------------
        private static Dictionary<string, long> cStartDico = new Dictionary<string, long>(new StringIndexKeyComparer());
        private static Dictionary<string, long> cStepDico = new Dictionary<string, long>(new StringIndexKeyComparer());
        private static Dictionary<string, List<double>> kMethodResult = new Dictionary<string, List<double>>();
        //public static long BenchmarkError = 0;
        private static float FrameRate = -1;
        private static Stopwatch Watch = new Stopwatch();
        private static Dictionary<string, long> cQuickStartDico = new Dictionary<string, long>(new StringIndexKeyComparer());
        //-------------------------------------------------------------------------------------------------------------
#if (UNITY_EDITOR)
        public static float kWarningDefault = 0.0033f;
        public static float kMaxDefault = 0.010f;
#elif (UNITY_ANDROID || UNITY_IOS)
        public static float kWarningDefault = 0.10f;
        public static float kMaxDefault = 0.030f;
#else
        public static float kWarningDefault = 0.05f;
        public static float kMaxDefault = 0.015f;
#endif
        private static int StartCount = 0;
        private static float BenchmarkLimit = 0.0F;
        private static bool BenchmarkShowStart = true;
        private static string Green = "#007626FF";
        private static string Orange = "#B45200FF";
        private static string Red = "#890000FF";
        private static string Blue = "#002089FF";
        //-------------------------------------------------------------------------------------------------------------
        static NWDBenchmark()
        {
            PrefReload();
            Watch.Start();
#if (UNITY_EDITOR)
            FrameRate = 60.0F;
#elif (UNITY_ANDROID || UNITY_IOS)
                    FrameRate = 30.0F;
#else
                    FrameRate = 60.0F;
#endif

        }
        //-------------------------------------------------------------------------------------------------------------
        static public void PrefReload()
        {
#if (UNITY_EDITOR)
            BenchmarkLimit = NWDProjectPrefs.GetFloat(NWDConstants.K_EDITOR_BENCHMARK_LIMIT, 0.0F);
            BenchmarkShowStart = NWDProjectPrefs.GetBool(NWDConstants.K_EDITOR_BENCHMARK_SHOW_START);
            FrameRate = 60.0F;
            if (EditorGUIUtility.isProSkin)
            {
                Green = "#" + NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_BENCHMARK_GREEN_PRO);
                Orange = "#" + NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_BENCHMARK_ORANGE_PRO);
                Red = "#" + NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_BENCHMARK_RED_PRO);
                Blue = "#" + NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_BENCHMARK_BLUE_PRO);
            }
            else
            {
                Green = "#" + NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_BENCHMARK_GREEN);
                Orange = "#" + NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_BENCHMARK_ORANGE);
                Red = "#" + NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_BENCHMARK_RED);
                Blue = "#" + NWDProjectPrefs.GetString(NWDConstants.K_EDITOR_BENCHMARK_BLUE);
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        [Conditional(MACRO)]
        public static void ResetAll()
        {
            Watch.Restart();
            cStartDico.Clear();
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static string GetKeyWihRandom()
        //{
        //    return GetKey() + " " + NWDToolbox.RandomStringUnix(12);
        //}
        //-------------------------------------------------------------------------------------------------------------
        private static string GetKey()
        {
            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(2);
            MethodBase tM = sf.GetMethod();
            string tDot = ".";
            if (tM.IsStatic == true) { tDot = ">"; }
            string tMethod = tM.DeclaringType.Name + tDot + tM.Name;
            return tMethod;
        }
        //-------------------------------------------------------------------------------------------------------------
        [Conditional(MACRO)]
        public static void Start()
        {
            Start(GetKey());
        }
        //-------------------------------------------------------------------------------------------------------------
        private static string GetIndentation()
        {
            string rReturn = "";
            for (int i = 0; i < StartCount; i++)
            {
                if (i == 0)
                {
                    rReturn = rReturn + "\t";
                }
                else
                {
                    rReturn = rReturn + "|\t";
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        [Conditional(MACRO)]
        public static void Start(string sKey)
        {
            //long tStart = Watch.ElapsedMilliseconds;

            if (cStartDico.ContainsKey(sKey) == true)
            {
                //string tLog = "benchmark : " + GetIndentation() + "<b>" + sKey + "</b>\t" + " all ready started!";
                //UnityEngine.Debug.Log(tLog);
            }
            else
            {
                StartCount++;
                cStartDico.Add(sKey, Watch.ElapsedMilliseconds);
                if (cStepDico.ContainsKey(sKey) == true)
                {
                    cStepDico[sKey] = Watch.ElapsedMilliseconds;
                }
                else
                {
                    cStepDico.Add(sKey, Watch.ElapsedMilliseconds);
                }
                if (BenchmarkShowStart == true)
                {
                    string tLog = "benchmark : " + GetIndentation() + "•<b>" + sKey + "</b>\t" + " start now!";
                    UnityEngine.Debug.Log(tLog);
                }
            }

            //long tStop = Watch.ElapsedMilliseconds - tStart;
            //BenchmarkError += tStop;
            //UnityEngine.Debug.Log("NWDBenchmark STOPWATCH Start() : " + (tStop / 1000.0F).ToString("F3") + " s");
        }
        //-------------------------------------------------------------------------------------------------------------
        [Conditional(MACRO)]
        public static void Log(string sInfos = "")
        {
            UnityEngine.Debug.Log("benchmark : " + GetIndentation() + "|\t• " + " Log : " + sInfos);
        }
        //-------------------------------------------------------------------------------------------------------------
        //public static double SinceStartup(string sInfos = "")
        //{
        //    double tTime = Time.realtimeSinceStartup;
        //    UnityEngine.Debug.Log("benchmark : Realtime Since Startup : " + tTime.ToString("F3") + " seconds " + sInfos);
        //    return tTime;
        //}
        //-------------------------------------------------------------------------------------------------------------
        [Conditional(MACRO)]
        public static void LogWarning(string sInfos = "")
        {
            UnityEngine.Debug.LogWarning("benchmark : " + GetIndentation() + "|\t !!! " + " Log : " + sInfos);
        }
        //-------------------------------------------------------------------------------------------------------------
        [Conditional(MACRO)]
        public static void Finish(bool sWithDebug = true, string sMoreInfos = "")
        {
            Finish(GetKey(), sWithDebug, sMoreInfos);
        }
        //-------------------------------------------------------------------------------------------------------------
        [Conditional(MACRO)]
        public static void Step(bool sWithDebug = true, string sMoreInfos = "")
        {
            Step(GetKey(), sWithDebug, sMoreInfos);
        }
        //-------------------------------------------------------------------------------------------------------------
        [Conditional(MACRO)]
        public static void Finish(string sKey, bool sWithDebug = true, string sMoreInfos = "")
        {
            //long tStart = Watch.ElapsedMilliseconds;
            if (cStepDico.ContainsKey(sKey) == true)
            {
                cStepDico[sKey] = Watch.ElapsedMilliseconds;
            }
            else
            {
                cStepDico.Add(sKey, Watch.ElapsedMilliseconds);
            }
            double rDelta = 0;
            double rFrameSpend = 0;
            if (cStartDico.ContainsKey(sKey) == true)
            {
                rDelta = (Watch.ElapsedMilliseconds - cStartDico[sKey]) / 1000.0F;
                rFrameSpend = FrameRate * rDelta;
                string tMaxColor = Green;
                if (rDelta >= kWarningDefault)
                {
                    tMaxColor = Orange;
                }
                if (rDelta >= kMaxDefault)
                {
                    tMaxColor = Red;
                }
                if (rDelta > BenchmarkLimit)
                {
                    if (BenchmarkShowStart == true)
                    {
                        UnityEngine.Debug.Log("benchmark : " + GetIndentation() + "| <b><color=" + tMaxColor + ">" + rDelta.ToString("F3") + "</color></b>" + "");
                    }
                    string tLog = "benchmark : " + GetIndentation() + "•<b>" + sKey + "</b>\t" + " execute in <color=" + tMaxColor + ">" +
                 rDelta.ToString("F3") + " seconds </color> spent " + rFrameSpend.ToString("F1") + "F/" + FrameRate + "Fps. " + sMoreInfos;
                    UnityEngine.Debug.Log(tLog);
                }
                StartCount--;
                cStartDico.Remove(sKey);

                if (kMethodResult.ContainsKey(sKey) == false)
                {
                    kMethodResult.Add(sKey, new List<double>());
                }
                kMethodResult[sKey].Add(rDelta);
            }
            else
            {
                UnityEngine.Debug.Log("benchmark : error '" + GetIndentation() + sKey + "' has no start value. " + sMoreInfos);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [Conditional(MACRO)]
        public static void Step(string sKey, bool sWithDebug = true, string sMoreInfos = "")
        {
            double rDeltaAbsolute = 0;
            double rDelta = 0;
            double rFrameSpend = 0;
            long LastStep = 0;
            if (cStartDico.ContainsKey(sKey) == true)
            {
                rDeltaAbsolute = (Watch.ElapsedMilliseconds - cStartDico[sKey]) / 1000.0F;
                if (cStartDico.ContainsKey(sKey) == true)
                {
                    LastStep = cStepDico[sKey];
                }
                rDelta = (Watch.ElapsedMilliseconds - LastStep) / 1000.0F;
                rFrameSpend = FrameRate * rDelta;
                string tMaxColor = Green;
                if (rDelta >= kWarningDefault)
                {
                    tMaxColor = Orange;
                }
                if (rDelta >= kMaxDefault)
                {
                    tMaxColor = Red;
                }
                if (rDelta > BenchmarkLimit)
                {
                    string tLog = "benchmark : " + GetIndentation() + "|    <b>" + sKey + "</b>\t" + " step <color=" + tMaxColor + ">" +
                    rDelta.ToString("F3") + " seconds </color> spent " + rFrameSpend.ToString("F1") + "F/" + FrameRate + "Fps. (Delta Absolute = " + rDeltaAbsolute.ToString("F3") + ") " + sMoreInfos;
                    UnityEngine.Debug.Log(tLog);
                }
            }
            else
            {
                UnityEngine.Debug.Log("benchmark : error '" + GetIndentation() + sKey + "' has no start value. " + sMoreInfos);
            }
            if (cStepDico.ContainsKey(sKey) == true)
            {
                cStepDico[sKey] = Watch.ElapsedMilliseconds;
            }
            else
            {
                cStepDico.Add(sKey, Watch.ElapsedMilliseconds);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [Conditional(MACRO)]
        public static void QuickStart(string sKey = null)
        {
            string tKey = string.Empty;
            if (sKey != null)
            {
                tKey = GetKey();
                tKey = tKey + " <color=" + Blue + ">" + sKey + "</color>";
            }
            else
            {
                tKey = GetKey();
            }
            if (cQuickStartDico.ContainsKey(tKey) == true)
            {
                //string tLog = "benchmark : " + GetIndentation() + "<b>" + tKey + "</b>\t" + " all ready started!";
                //UnityEngine.Debug.Log(tLog);
            }
            else
            {
                cQuickStartDico.Add(tKey, Watch.ElapsedMilliseconds);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [Conditional(MACRO)]
        public static void QuickFinish(string sKey = null)
        {
            string tKey = string.Empty;
            if (sKey != null)
            {
                tKey = GetKey();
                tKey = tKey + " <color=" + Blue + ">" + sKey + "</color>";
            }
            else
            {
                tKey = GetKey();
            }
            if (cQuickStartDico.ContainsKey(tKey) == true)
            {
                double rDelta = (Watch.ElapsedMilliseconds - cQuickStartDico[tKey]) / 1000.0F;
                cQuickStartDico.Remove(tKey);
                if (kMethodResult.ContainsKey(tKey) == false)
                {
                    kMethodResult.Add(tKey, new List<double>());
                }
                kMethodResult[tKey].Add(rDelta);
            }
            else
            {
                UnityEngine.Debug.Log("benchmark : error '" + tKey + "' has no QuickStart value.");
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [Conditional(MACRO)]
        public static void ResetAllResults()
        {
            kMethodResult = new Dictionary<string, List<double>>();
        }
        //-------------------------------------------------------------------------------------------------------------
        [Conditional(MACRO)]
        public static void AllResults()
        {
            foreach (KeyValuePair<string, List<double>> tResult in kMethodResult)
            {
                List<double> tResultList = tResult.Value.OrderByDescending(d => d).ToList();

                double rRawDelta = Enumerable.Average(tResultList);
                double rRawSum = Enumerable.Sum(tResultList);
                double rRawMax = Enumerable.Max(tResultList);
                double rRawMin = Enumerable.Min(tResultList);
                double rDelta = rRawDelta;
                double rMax = rRawMax;
                double rMin = rRawMin;
                bool AA = false;
                int tD = 0;
                if (tResultList.Count > 20)
                {
                    AA = true;
                    int tCount = tResultList.Count;
                    int tNN = (int)Math.Floor(tCount * 0.95);
                    tD = tCount - tNN;
                    for (int i = 0; i < tD; i++)
                    {
                        //tResultList.RemoveAt(tResultList.Count-1);
                        tResultList.RemoveAt(0);
                    }
                    rMax = Enumerable.Max(tResultList);
                    rMin = Enumerable.Min(tResultList);
                }
                rDelta = Enumerable.Average(tResultList);
                string tMaxColor = Green;
                if (rDelta >= kWarningDefault)
                {
                    tMaxColor = Orange;
                }
                if (rDelta >= kMaxDefault)
                {
                    tMaxColor = Red;
                }


                string tSumColor = Green;
                if (rRawSum >= kWarningDefault)
                {
                    tSumColor = Orange;
                }
                if (rRawSum >= kMaxDefault)
                {
                    tSumColor = Red;
                }

                UnityEngine.Debug.Log("benchmark Result " +
                    "'<b>" + tResult.Key + "</b>' has " + tResultList.Count + " value" + (tResult.Value.Count > 1 ? "s" : "") +
                    " and average is <color=" + tMaxColor + ">" + rDelta.ToString("F6") + "</color> seconds" +
                    (tResult.Value.Count > 1 ? " (min " + rMin.ToString("F6") + " max " + rMax.ToString("F6") + ")" : "") +
                    (AA == true ? " at 95%  with " + tResult.Value.Count + " raw datas average <color=" + tMaxColor + ">" + rRawDelta.ToString("F6") + "</color> seconds (min " + rRawMin.ToString("F6") + " max " + rRawMax.ToString("F6") + ") " : " ") +
                    (tResult.Value.Count > 1 ? " sum is <color=" + tSumColor + ">" + rRawSum.ToString("F6") + "</color> seconds" : "") +

                    ""
                    );

            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================