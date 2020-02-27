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
using SQLite4Unity3d;
using System.Collections;
using System.IO;
using System.Reflection;

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
        public const string K_LAUNCHER_ACCOUNT_READY = "K_LAUNCHER_ACCOUN_READY";
        public const string K_NETWORKEDDATA_READY = "K_NETWORKEDDATA_READY";
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static partial class NWDLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        static private NWDStatut State = NWDStatut.None;
        static private int StepSum;
        static private int StepIndex;
        static private bool Launched = false;
        static bool Preload = true;
        static public bool ActiveBenchmark;
        //-------------------------------------------------------------------------------------------------------------
        static public float GetPurcent()
        {
            return (float)StepIndex / (float)StepSum;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void NotifyStep()
        {
            StepIndex++;
            NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_LAUNCHER_STEP);
            //NWEBenchmark.Log(" StepSum = " + StepSum + " and StepIndex =" + StepIndex);
        }
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
        static public bool EditorByPass;
        //-------------------------------------------------------------------------------------------------------------
        static double TimeStart;
        //-------------------------------------------------------------------------------------------------------------
        static void LauncherBenchmarkToMarkdown(double tNWDFinish)
        {
            double tTimeFinish = NWEBenchmark.SinceStartup();
            Dictionary<string, string> tRepport = new Dictionary<string, string>();
            List<string> tRepportLayout = new List<string>();
            tRepport.Add("DATE", DateTime.Now.ToString("yyyy-MM-dd")); tRepportLayout.Add("---");
            tRepport.Add("TIME", DateTime.Now.ToString("HH:mm:ss")); tRepportLayout.Add("---");
            tRepport.Add("USER", "(User)"); tRepportLayout.Add("---");

            tRepport.Add("COMPILE ON", NWDAppConfiguration.SharedInstance().CompileOn); tRepportLayout.Add("---");
            tRepport.Add("COMPILE FOR", Application.platform.ToString()); tRepportLayout.Add("---");
            tRepport.Add("COMPILE WITH", Application.unityVersion); tRepportLayout.Add("---");
            tRepport.Add("DEVICE", SystemInfo.deviceName); tRepportLayout.Add("---");
            tRepport.Add("PRELOAD DATAS", GetPreload().ToString()); tRepportLayout.Add("---");
            tRepport.Add("INFOS", "(infos)"); tRepportLayout.Add("---");

            tRepport.Add("LAUNCH UNITY", TimeStart.ToString("F3") + "s"); tRepportLayout.Add("---");
            tRepport.Add("LAUNCH NWD", tNWDFinish.ToString("F3") + "s"); tRepportLayout.Add("---");
            tRepport.Add("LAUNCH FINAL", tTimeFinish.ToString("F3") + "s"); tRepportLayout.Add("---");

            tRepport.Add("SIGNIN", "(infos)"); tRepportLayout.Add("---");

            Debug.Log("benchmark : !!!! REPPORT | " + string.Join(" | ", tRepport.Keys) + " |");
            Debug.Log("benchmark : !!!! REPPORT | " + string.Join(" | ", tRepportLayout) + " |");
            Debug.Log("benchmark : !!!! REPPORT | " + string.Join(" | ", tRepport.Values) + " |");
        }
        //-------------------------------------------------------------------------------------------------------------
        public const string K_PINCODE_KEY = "K_PINCODE_KEY_jkghvjh";
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
                NWEBenchmark.Start();
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
                    Launch_Editor();
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
                        Launch_Runtime_Sync();
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
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static void Quit()
        {
            // delete editor key
#if UNITY_EDITOR
            //Debug.Log("Quitting the Editor");
            if (EditorPrefs.HasKey(K_PINCODE_KEY))
            {
                EditorPrefs.DeleteKey(K_PINCODE_KEY);
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================