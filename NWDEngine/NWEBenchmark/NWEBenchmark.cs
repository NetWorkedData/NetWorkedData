using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWEBenchmark
    {
        //-------------------------------------------------------------------------------------------------------------
        public static Dictionary<string, DateTime> cStartDico = new Dictionary<string, DateTime>();
        public static Dictionary<string, int> cCounterDico = new Dictionary<string, int>();
        public static Dictionary<string, float> cMaxDico = new Dictionary<string, float>();
        public static Dictionary<string, float> cMaxGranDico = new Dictionary<string, float>();
        public static Dictionary<string, string> cTagDico = new Dictionary<string, string>();
        //-------------------------------------------------------------------------------------------------------------
        public static void ResetAll()
        {
            cStartDico = new Dictionary<string, DateTime>();
            cCounterDico = new Dictionary<string, int>();
            cTagDico = new Dictionary<string, string>();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetKeyWihRandom()
        {
            return GetKey() + " " + NWDToolbox.RandomStringUnix(12);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static string GetKey()
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
        protected static UnityEngine.Object GetObject()
        {
            //StackTrace st = new StackTrace();
            //StackFrame sf = st.GetFrame(2);
            UnityEngine.Object sObject = null;
            return sObject;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Start()
        {
            Start(GetKey());
        }
        //-------------------------------------------------------------------------------------------------------------
        static float kMaxDefault = 0.010f;
        static float kMaxPerOperationDefault = 0.001f;
        static int StartCount = 0;
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
            if (cStartDico.ContainsKey(sKey) == true)
            {
                //cStartDico[sKey] = DateTime.Now;
                cCounterDico[sKey] = 0;
                cTagDico[sKey] = string.Empty;
                cMaxDico[sKey] = kMaxDefault;
                cMaxGranDico[sKey] = kMaxPerOperationDefault;

                string tLog = "benchmark : " + GetIndentation() + "<b>" + sKey + "</b>\t" + " all ready started!";
#if UNITY_EDITOR
#else
                tLog = tLog.Replace("  ", " ").Replace("<b>", "").Replace("</b>", "");
#endif
                UnityEngine.Debug.Log(tLog);
            }
            else
            {
                StartCount++;
                cStartDico.Add(sKey, DateTime.Now);
                cCounterDico.Add(sKey, 0);
                cTagDico.Add(sKey, string.Empty);
                cMaxDico.Add(sKey, kMaxDefault);
                cMaxGranDico.Add(sKey, kMaxPerOperationDefault);

                string tLog = "benchmark : " + GetIndentation() + "<b>" + sKey + "</b>\t" + " start now!";
#if UNITY_EDITOR
#else
                tLog = tLog.Replace("  ", " ").Replace("<b>", "").Replace("</b>", "");
#endif
                UnityEngine.Debug.Log(tLog);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Tag(string sTag)
        {
            Tag(GetKey(), sTag);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Tag(string sKey, string sTag)
        {
            if (cStartDico.ContainsKey(sKey) == true)
            {
                cTagDico[sKey] = sTag;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Max(float sMax)
        {
            Max(GetKey(), sMax);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Max(string sKey, float sMax)
        {
            if (cStartDico.ContainsKey(sKey) == true)
            {
                cMaxDico[sKey] = sMax;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void MaxPerOperation(float sMax)
        {
            MaxPerOperation(GetKey(), sMax);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void MaxPerOperation(string sKey, float sMax)
        {
            if (cStartDico.ContainsKey(sKey) == true)
            {
                cMaxGranDico[sKey] = sMax;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Increment(int sVal = 1)
        {
            Increment(GetKey(), sVal);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Increment(string sKey, int sVal = 1)
        {
            if (cStartDico.ContainsKey(sKey) == true)
            {
                cCounterDico[sKey] = cCounterDico[sKey] + sVal;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Log(string sInfos = "")
        {
            UnityEngine.Debug.Log("benchmark : " + GetIndentation() + "|\t• " + " Log : " + sInfos);
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
        public static double Finish(string sKey, bool sWithDebug = true, string sMoreInfos = "")
        {
            double rDelta = 0;
            double rFrameSpend = 0;
            if (cStartDico.ContainsKey(sKey) == true)
            {
                double tStart = NWEDateHelper.ConvertToTimestamp(cStartDico[sKey]);
                int tCounter = cCounterDico[sKey];
                string tTag = cTagDico[sKey];
                if (string.IsNullOrEmpty(tTag) == false)
                {
                    tTag = " (tag : " + tTag + ")";
                }
                float tMax = cMaxDico[sKey];
                float tMaxGranule = cMaxGranDico[sKey];
                cStartDico.Remove(sKey);
                cCounterDico.Remove(sKey);
                cTagDico.Remove(sKey);
                cMaxDico.Remove(sKey);
                cMaxGranDico.Remove(sKey);
                double tFinish = NWEDateHelper.ConvertToTimestamp(DateTime.Now);
                rDelta = tFinish - tStart;
                rFrameSpend = 60 * rDelta;
                string tMaxColor = "green";
                if (rDelta >= tMax)
                {
                    tMaxColor = "red";
                }
                if (sWithDebug == true)
                {
                    if (tCounter == 1)
                    {
                        string tLog = "benchmark : " + GetIndentation() + "<b>" + sKey + "</b>\t" + tTag + " execute " + tCounter +
                          " operation in <color=" + tMaxColor + ">" +
                          rDelta.ToString("F3") + " seconds </color> spent " + rFrameSpend.ToString("F1") + "F/60Fps. " + sMoreInfos;
#if UNITY_EDITOR
#else
                        tLog = tLog.Replace("</color>", "").Replace("<color=" + tMaxColor + ">", "").Replace("  ", " ").Replace("<b>", "").Replace("</b>", "");
#endif
                        UnityEngine.Debug.Log(tLog);
                    }
                    else if (tCounter > 1)
                    {
                        double tGranule = rDelta / tCounter;
                        string tMaxGranuleColor = "black";
                        if (tGranule >= tMaxGranule)
                        {
                            tMaxGranuleColor = "red";
                        }
                        string tLog = "benchmark : " + GetIndentation() + "<b>" + sKey + "</b>\t" + tTag + " execute " + tCounter +
                         " operations in <color=" + tMaxColor + ">" + rDelta.ToString("F3") +
                         " seconds </color>(<color=" + tMaxGranuleColor + ">" + tGranule.ToString("F5") +
                         " seconds per operation</color>) spent " + rFrameSpend.ToString("F1") + "F/60Fps. " + sMoreInfos;
#if UNITY_EDITOR
#else
                        tLog = tLog.Replace("</color>", "").Replace("<color=" + tMaxColor + ">", "").Replace("<color=" + tMaxGranuleColor + ">", "").Replace("  ", " ").Replace("<b>", "").Replace("</b>", "");
#endif
                        UnityEngine.Debug.Log(tLog);
                    }
                    else
                    {
                        string tLog = "benchmark : " + GetIndentation() + "<b>" + sKey + "</b>\t" + tTag + " execute in <color=" + tMaxColor + ">" +
                         rDelta.ToString("F3") + " seconds </color> spent " + rFrameSpend.ToString("F1") + "F/60Fps. " + sMoreInfos;
#if UNITY_EDITOR
#else
                        tLog = tLog.Replace("</color>", "").Replace("<color=" + tMaxColor + ">", "").Replace("  ", " ").Replace("<b>", "").Replace("</b>", "");
#endif
                        UnityEngine.Debug.Log(tLog);
                    }
                }
                StartCount--;
            }
            else
            {
                if (sWithDebug == true)
                {
                    UnityEngine.Debug.Log("benchmark : error '" + GetIndentation() + sKey + "' has no start value. " + sMoreInfos);
                }
            }
            return rDelta;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================