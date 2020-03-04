using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEBenchmark
    {
        //-------------------------------------------------------------------------------------------------------------
        private static Dictionary<string, long> cStartDico = new Dictionary<string, long>(new StringIndexKeyComparer());
        public static long BenchmarkError = 0;
        private static float FrameRate = -1;
        private static Stopwatch Watch = new Stopwatch();
        //-------------------------------------------------------------------------------------------------------------
#if (UNITY_EDITOR)
        public static float kMaxDefault = 0.010f;
#elif (UNITY_ANDROID || UNITY_IOS)
        public static float kMaxDefault = 0.030f;
#else
        public static float kMaxDefault = 0.015f;
#endif
        private static int StartCount = 0;
        //-------------------------------------------------------------------------------------------------------------
        static NWEBenchmark()
        {
            UnityEngine.Debug.Log("START NWEBenchmark CLASS");
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
        public static void ResetAll()
        {
            Watch.Restart();
            cStartDico.Clear();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetKeyWihRandom()
        {
            return GetKey() + " " + NWDToolbox.RandomStringUnix(12);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static string GetKey()
        {
            //long tStart = Watch.ElapsedMilliseconds;

            StackTrace st = new StackTrace();
            StackFrame sf = st.GetFrame(2);
            MethodBase tM = sf.GetMethod();
            string tDot = ".";
            if (tM.IsStatic == true) { tDot = ">"; }
            string tMethod = tM.DeclaringType.Name + tDot + tM.Name;

            //long tStop = Watch.ElapsedMilliseconds - tStart;
            //BenchmarkError += tStop;
            //UnityEngine.Debug.Log("NWEBenchmark STOPWATCH GetKey() : " + (tStop / 1000.0F).ToString("F3") + " s");

            return tMethod;
        }
        //-------------------------------------------------------------------------------------------------------------
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
                string tLog = "benchmark : " + GetIndentation() + "•<b>" + sKey + "</b>\t" + " start now!";
                UnityEngine.Debug.Log(tLog);
            }

            //long tStop = Watch.ElapsedMilliseconds - tStart;
            //BenchmarkError += tStop;
            //UnityEngine.Debug.Log("NWEBenchmark STOPWATCH Start() : " + (tStop / 1000.0F).ToString("F3") + " s");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Log(string sInfos = "")
        {
            UnityEngine.Debug.Log("benchmark : " + GetIndentation() + "|\t• " + " Log : " + sInfos);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static double SinceStartup(string sInfos = "")
        {
            double tTime = Time.realtimeSinceStartup;
            UnityEngine.Debug.Log("benchmark : Realtime Since Startup : " + tTime.ToString("F3") + " seconds " + sInfos);
            return tTime;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void LogWarning(string sInfos = "")
        {
            UnityEngine.Debug.LogWarning("benchmark : " + GetIndentation() + "|\t !!! " + " Log : " + sInfos);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static double Finish(bool sWithDebug = true, string sMoreInfos = "")
        {
            return Finish(GetKey(), sWithDebug, sMoreInfos);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static double Step(bool sWithDebug = true, string sMoreInfos = "")
        {
            return Step(GetKey(), sWithDebug, sMoreInfos);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static double Finish(string sKey, bool sWithDebug = true, string sMoreInfos = "")
        {
            //long tStart = Watch.ElapsedMilliseconds;

            double rDelta = 0;
            double rFrameSpend = 0;
            if (cStartDico.ContainsKey(sKey) == true)
            {
                rDelta = (Watch.ElapsedMilliseconds - cStartDico[sKey]) / 1000.0F;
                rFrameSpend = FrameRate * rDelta;
                string tMaxColor = "green";
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

            //long tStop = Watch.ElapsedMilliseconds - tStart;
            //BenchmarkError += tStop;
            //UnityEngine.Debug.Log("NWEBenchmark STOPWATCH Finish() : " + (tStop / 1000.0F).ToString("F3") + " s");

            return rDelta;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static double Step(string sKey, bool sWithDebug = true, string sMoreInfos = "")
        {
            //long tStart = Watch.ElapsedMilliseconds;

            double rDelta = 0;
            double rFrameSpend = 0;
            if (cStartDico.ContainsKey(sKey) == true)
            {
                rDelta = (Watch.ElapsedMilliseconds - cStartDico[sKey]) / 1000.0F;
                rFrameSpend = FrameRate * rDelta;
                string tMaxColor = "green";
                if (rDelta >= kMaxDefault)
                {
                    tMaxColor = "red";
                }
                string tLog = "benchmark : " + GetIndentation() + "<b>" + sKey + "</b>\t" + " step <color=" + tMaxColor + ">" +
                 rDelta.ToString("F3") + " seconds </color> spent " + rFrameSpend.ToString("F1") + "F/" + FrameRate + "Fps. " + sMoreInfos;
                UnityEngine.Debug.Log(tLog);
            }
            else
            {
                UnityEngine.Debug.Log("benchmark : error '" + GetIndentation() + sKey + "' has no start value. " + sMoreInfos);
            }

            //long tStop = Watch.ElapsedMilliseconds - tStart;
            //BenchmarkError += tStop;
            //UnityEngine.Debug.Log("NWEBenchmark STOPWATCH Step() : " + (tStop / 1000.0F).ToString("F3") + " s");

            return rDelta;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================