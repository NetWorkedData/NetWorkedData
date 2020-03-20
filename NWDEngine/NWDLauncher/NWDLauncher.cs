//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:5
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;

using System.Collections;
using System.IO;
using System.Reflection;
using System.Diagnostics;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDStatut
    {
        Error = -9,

        None = 0,

        EngineStart = 1,
        EngineFinish = 9,

        ClassDeclareStart = 11,
        ClassDeclareStep = 12,
        ClassDeclareFinish = 19,

        ClassRestaureStart = 21,
        ClassRestaureFinish = 29,

        EngineReady = 30,

        DataEditorConnectionStart = 31,
        DataEditorConnectionError = 32,
        DataEditorConnectionFinish = 33,

        DataEditorTableCreateStart = 34,
        DataEditorTableCreateStep = 35,
        DataEditorTableCreateFinish = 36,

        DataEditorLoadStart = 37,
        DataEditorLoadStep = 38,
        DataEditorLoadFinish = 39,

        DataEditorIndexStart = 40,
        DataEditorIndexStep = 41,
        DataEditorIndexFinish = 42,

        EditorReady = 50,

        DataAccountConnectionStart = 51,

        DataAccountCodePinCreate = 52,
        DataAccountCodePinRequest = 53,
        DataAccountCodePinFail = 54,
        DataAccountCodePinStop = 55,
        DataAccountCodePinSuccess = 56,

        DataAccountConnectionError = 62,
        DataAccountConnectionFinish = 63,

        DataAccountTableCreateStart = 64,
        DataAccountTableCreateStep = 65,
        DataAccountTableCreateFinish = 66,

        DataAccountLoadStart = 67,
        DataAccountLoadStep = 68,
        DataAccountLoadFinish = 69,

        DataAccountIndexStart = 70,
        DataAccountIndexStep = 71,
        DataAccountIndexFinish = 72,

        AccountReady = 80,

        NetWorkedDataReady = 99,

    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDNotificationConstants
    {
        //-------------------------------------------------------------------------------------------------------------
        public const string K_LAUNCHER_STEP = "K_LAUNCHER_STEP";
        public const string K_LAUNCHER_ENGINE_READY = "K_LAUNCHER_ENGINE_READY";
        public const string K_LAUNCHER_EDITOR_READY = "K_LAUNCHER_EDITOR_READY";
        public const string K_LAUNCHER_ACCOUNT_READY = "K_LAUNCHER_ACCOUNT_READY";
        public const string K_NETWORKEDDATA_READY = "K_NETWORKEDDATA_READY";
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static partial class NWDLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        public static List<Type> AllNetWorkedDataTypes = new List<Type>();
        //-------------------------------------------------------------------------------------------------------------
        static private Dictionary<Type, Type> BasisToHelperList = new Dictionary<Type, Type>();
        //-------------------------------------------------------------------------------------------------------------
        static private NWDStatut State = NWDStatut.None;
        static private int StepSum = 0;
        static private int StepIndex = 0;
        static private bool Launched = false;
        static bool Preload = true;
        static public bool ActiveBenchmark = false;
        static public string RowInformations = string.Empty;
        static public bool CopyDatabase = false;
        //-------------------------------------------------------------------------------------------------------------
        static private bool EditorByPass = false;
        //-------------------------------------------------------------------------------------------------------------
        static public double TimeStart = 0;
        static public double TimeFinish = 0;
        static public double TimeNWDFinish = 0;
        //-------------------------------------------------------------------------------------------------------------
        static public float GetPurcent()
        {
            return (float)StepIndex / (float)StepSum;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void NotifyStep(bool sYeld = false)
        {
            StepIndex++;
            if (sYeld || YieldValid())
            {
                NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_LAUNCHER_STEP);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool YieldValid()
        {
            return (StepIndex % NWDAppConfiguration.SharedInstance().LauncherFaster == 0);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void NotifyEngineReady()
        {
            NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_LAUNCHER_ENGINE_READY);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void NotifyDataEditorReady()
        {
            NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_LAUNCHER_EDITOR_READY);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void NotifyDataAccountReady()
        {
            NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_LAUNCHER_ACCOUNT_READY);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void NotifyNetWorkedDataReady()
        {
            NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_NETWORKEDDATA_READY);
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDStatut GetState()
        {
            return State;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public bool GetPreload()
        {
            return Preload;
        }
        //-------------------------------------------------------------------------------------------------------------
        static void LauncherBenchmarkToMarkdown()
        {
            Dictionary<string, string> tRepport = new Dictionary<string, string>();
            List<string> tRepportLayout = new List<string>();
            tRepport.Add("DATE", DateTime.Now.ToString("yyyy-MM-dd")); tRepportLayout.Add("---");
            tRepport.Add("TIME", DateTime.Now.ToString("HH:mm:ss")); tRepportLayout.Add("---");
            tRepport.Add("BUILDER", NWDAppConfiguration.SharedInstance().BuilderUser); tRepportLayout.Add("---");

            tRepport.Add("COMPILE ON", NWDAppConfiguration.SharedInstance().CompileOn); tRepportLayout.Add("---");
            tRepport.Add("COMPILE FOR", Application.platform.ToString()); tRepportLayout.Add("---");
            tRepport.Add("OS VERSION", SystemInfo.operatingSystem); tRepportLayout.Add("---");
            //tRepport.Add("ARCH", System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE", EnvironmentVariableTarget.Machine)); tRepportLayout.Add("---");
            //tRepport.Add("OS VERSION", SystemInfo.operatingSystemFamily.ToString()); tRepportLayout.Add("---");
            tRepport.Add("COMPILE WITH", Application.unityVersion); tRepportLayout.Add("---");
            //tRepport.Add("DEVICE", SystemInfo.deviceName); tRepportLayout.Add("---");
            tRepport.Add("DEVICE", SystemInfo.deviceModel); tRepportLayout.Add("---");
            if (GetPreload() == false)
            {
                tRepport.Add("PRELOAD DATAS", GetPreload().ToString() + " (" + NWDAppConfiguration.SharedInstance().LauncherFaster.ToString() + ")"); tRepportLayout.Add("---");
            }
            else
            {
                tRepport.Add("PRELOAD DATAS", GetPreload().ToString()); tRepportLayout.Add("---");
            }
            tRepport.Add("BENCHMARK STEP", ActiveBenchmark.ToString()); tRepportLayout.Add("---");

            tRepport.Add("INFOS", "(infos)"); tRepportLayout.Add("---");
            tRepport.Add("LAUNCH UNITY", TimeStart.ToString("F3") + "s"); tRepportLayout.Add("---");
            tRepport.Add("SQL Secure", NWDDataManager.SharedInstance().IsSecure().ToString()); tRepportLayout.Add("---");
            //tRepport.Add("SQL Version", SQLite3.LibVersionNumber().ToString()); tRepportLayout.Add("---");
            tRepport.Add("SQL Version", NWDDataManager.SharedInstance().GetVersion()); tRepportLayout.Add("---");
            tRepport.Add("LAUNCH NWD", TimeNWDFinish.ToString("F3") + "s"); tRepportLayout.Add("---");
            tRepport.Add("COPY DATABASE", CopyDatabase.ToString()); tRepportLayout.Add("---");
            tRepport.Add("LAUNCH FINAL", TimeFinish.ToString("F3") + "s"); tRepportLayout.Add("---");

            tRepport.Add("ROWS INFORMATIONS", RowInformations); tRepportLayout.Add("---");

            tRepport.Add("SIGNIN", "(infos)"); tRepportLayout.Add("---");

            tRepport.Add("CONCLUSION", ""); tRepportLayout.Add("---");

            if (ActiveBenchmark)
            {
                UnityEngine.Debug.Log("benchmark : REPPORT | " + string.Join(" | ", tRepport.Keys) + " |");
                UnityEngine.Debug.Log("benchmark : REPPORT | " + string.Join(" | ", tRepportLayout) + " |");
            }
            UnityEngine.Debug.Log("benchmark : REPPORT | " + string.Join(" | ", tRepport.Values) + " |");
        }
        //-------------------------------------------------------------------------------------------------------------
        [RuntimeInitializeOnLoadMethod]
        static public void Launch()
        {
            if (Launched == false)
            {
                TimeStart = Time.realtimeSinceStartup;
                ActiveBenchmark = NWDAppConfiguration.SharedInstance().LauncherBenchmark;
                StepSum = 0;
                StepIndex = 0;
                NWEBenchmark.Start("Launch");
                //Stopwatch tSW = new Stopwatch();
                //tSW.Start();
                Launched = true;
                //NWDToolbox.EditorAndPlaying("NWDLauncher Launch()");
                EditorByPass = false;
#if UNITY_EDITOR
                if (ActiveBenchmark)
                {
                    NWEBenchmark.Log("Pass in editor macro-block");
                }
#endif
                if (Application.isEditor == true)
                {
                    if (ActiveBenchmark)
                    {
                        NWEBenchmark.Log("Launch in editor");
                    }
                    EditorByPass = true;
                    if (Application.isPlaying == true)
                    {
                        if (ActiveBenchmark)
                        {
                            NWEBenchmark.Log("Launch as playmode");
                        }
                        EditorByPass = true;
                    }
                }
                if (EditorByPass == true)
                {
                    Preload = true;
                    LaunchStandard();
                }
                else
                {
                    Preload = NWDAppConfiguration.SharedInstance().PreloadDatas;
                    if (Preload == true)
                    {
                        if (ActiveBenchmark)
                        {
                            NWEBenchmark.Log("Launch in runtime preload (sync)");
                        }
                        LaunchRuntimeSync();
                    }
                    else
                    {
                        if (ActiveBenchmark)
                        {
                            NWEBenchmark.Log("Launch in runtime by NWDGameDataManager.ShareInstance (async)");
                        }
                        //Launch_Runtime_Async(); // waiting order from NWDGameDataManager.ShareInstance()
                    }
                }
                //tSW.Stop();
                //UnityEngine.Debug.Log("STOPWATCH : " + (tSW.ElapsedMilliseconds / 1000.0F).ToString("F3") + " s");
                NWEBenchmark.Finish("Launch");
                if (Preload == true)
                {
                    if (ActiveBenchmark)
                    {
                        LauncherBenchmarkToMarkdown();
                        NWBBenchmarkResult.CurrentData().BenchmarkNow();
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void ResetLauncher()
        {
            AllNetWorkedDataTypes.Clear();
            BasisToHelperList.Clear();
            State = NWDStatut.None;
            StepSum = 0;
            StepIndex = 0;
            Launched = false;
            Preload = true;
            ActiveBenchmark = false;
            RowInformations = string.Empty;
            CopyDatabase = false;
            EditorByPass = false;
            TimeStart = 0;
            TimeFinish = 0;
            TimeNWDFinish = 0;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return if the launcher is ready or not and some error.
        /// use to prevent empty type list launched error in unity Editor
        /// </summary>
        /// <returns></returns>
        static public bool LauncherIsReady()
        {
            bool rReturn = true;
            if (Launched == false)
            {
                // WTF ... your launcher is not started! 
#if UNITY_EDITOR
                EditorUtility.DisplayDialog("NetWorkedData Alert", "Your launcher is not ready", "OK");
#endif
                //rReturn = false; //? no prefere launch the static method
                Launch();
            }
            else
            {
                if (GetState() != NWDStatut.NetWorkedDataReady)
                {
                    // WTF ... your launcher is not Ready! 
#if UNITY_EDITOR
                    EditorUtility.DisplayDialog("NetWorkedData Alert", "Your launcher is not ready", "OK");
#endif
                    rReturn = false;
                }
                else
                {
                    if (AllNetWorkedDataTypes.Count == 0)
                    {
                        // WTF ... your launcher is empty! 
#if UNITY_EDITOR
                        EditorUtility.DisplayDialog("NetWorkedData Alert", "Your launcher return a empty list of type! It's not possible.", "OK");
#endif
                    rReturn = false;
                    }
                }
            }
            return rReturn;
        }

        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================