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
using System.Reflection;
using UnityEngine;
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
        //public static long BenchmarkError = 0;
        private static float FrameRate = -1;
        private static Stopwatch Watch = new Stopwatch();
        //private static float LastStep = 0;
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
        //-------------------------------------------------------------------------------------------------------------
        static NWDBenchmark()
        {
            Watch.Start();
            if (Application.targetFrameRate == -1)
            {
#if (UNITY_EDITOR)
                FrameRate = 60.0F;
#elif (UNITY_ANDROID || UNITY_IOS)
                    FrameRate = 30.0F;
#else
                    FrameRate = 60.0F;
#endif
            }
            else
            {
                FrameRate = (float)Application.targetFrameRate;
            }
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
                string tLog = "benchmark : " + GetIndentation() + "•<b>" + sKey + "</b>\t" + " start now!";
                UnityEngine.Debug.Log(tLog);
            }

            //long tStop = Watch.ElapsedMilliseconds - tStart;
            //BenchmarkError += tStop;
            //UnityEngine.Debug.Log("NWEBenchmark STOPWATCH Start() : " + (tStop / 1000.0F).ToString("F3") + " s");
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
                string tMaxColor = "green";
                if (rDelta >= kWarningDefault)
                {
                    tMaxColor = "orange";
                }
                if (rDelta >= kMaxDefault)
                {
                    tMaxColor = "red";
                }
                UnityEngine.Debug.Log("benchmark : " + GetIndentation() + "| <b><color=" + tMaxColor + ">" + rDelta.ToString("F3") + "</color></b>" + "");

                string tLog = "benchmark : " + GetIndentation() + "•<b>" + sKey + "</b>\t" + " execute in <color=" + tMaxColor + ">" +
                 rDelta.ToString("F3") + " seconds </color> spent " + rFrameSpend.ToString("F1") + "F/" + FrameRate + "Fps. " + sMoreInfos;
                UnityEngine.Debug.Log(tLog);
                StartCount--;
                cStartDico.Remove(sKey);
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
                string tMaxColor = "green";
                if (rDelta >= kWarningDefault)
                {
                    tMaxColor = "orange";
                }
                if (rDelta >= kMaxDefault)
                {
                    tMaxColor = "red";
                }
                string tLog = "benchmark : " + GetIndentation() + "|    <b>" + sKey + "</b>\t" + " step <color=" + tMaxColor + ">" +
                rDelta.ToString("F3") + " seconds </color> spent " + rFrameSpend.ToString("F1") + "F/" + FrameRate + "Fps. (Delta Absolute = " + rDeltaAbsolute.ToString("F3") + ") " + sMoreInfos;
                UnityEngine.Debug.Log(tLog);
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================