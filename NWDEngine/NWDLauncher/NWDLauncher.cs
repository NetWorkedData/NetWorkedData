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
#define NWD_LOG
#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using System;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
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
#if UNITY_EDITOR
        public static NWDCompileTypeBypass kByPass = NWDCompileTypeBypass.NoneBypass;
#endif
        //-------------------------------------------------------------------------------------------------------------
        static public NWDCompileType CompileAs()
        {
            NWDCompileType rReturn = NWDCompileType.Runtime;
            if (Application.isEditor == true)
            {
                rReturn = NWDCompileType.Editor;
                if (Application.isPlaying == true)
                {
                    rReturn = NWDCompileType.PlayMode;
                }
            }
#if UNITY_EDITOR
            switch (kByPass)
            {
                case NWDCompileTypeBypass.Editor:
                    {
                        rReturn = NWDCompileType.Editor;
                    }
                    break;
                case NWDCompileTypeBypass.PlayMode:
                    {
                        rReturn = NWDCompileType.PlayMode;
                    }
                    break;
                case NWDCompileTypeBypass.Runtime:
                    {
                        rReturn = NWDCompileType.Runtime;
                    }
                    break;
                case NWDCompileTypeBypass.NoneBypass:
                    {
                        // return true value
                    }
                    break;
            }
#endif
            return rReturn;
        }
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
        //static public bool ActiveBenchmark = false;
        static public string RowInformations = string.Empty;
        static public bool CopyDatabase = false;
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
#if NWD_LAUNCHER_BENCHMARK || NWD_LAUNCHER_RESULT_BENCHMARK
            //UnityEngine.Debug.Log("LauncherBenchmarkToMarkdown()");
            Dictionary<string, string> tRepport = new Dictionary<string, string>();
            List<string> tRepportLayout = new List<string>();
            // Engine part
            tRepport.Add("ENGINE","NWD "+NWDEngineVersion.Version); tRepportLayout.Add("---");
            tRepport.Add("DATE", DateTime.Now.ToString("yyyy-MM-dd")); tRepportLayout.Add("---");
            tRepport.Add("TIME", DateTime.Now.ToString("HH:mm:ss")); tRepportLayout.Add("---");
            // build by, on, ...
            tRepport.Add("BUILDER", NWDAppConfiguration.SharedInstance().BuilderUser); tRepportLayout.Add("---");
            tRepport.Add("COMPILE ON", NWDAppConfiguration.SharedInstance().CompileOn); tRepportLayout.Add("---");
            tRepport.Add("COMPILE FOR", Application.platform.ToString()); tRepportLayout.Add("---");
            tRepport.Add("OS VERSION", SystemInfo.operatingSystem); tRepportLayout.Add("---");
            tRepport.Add("DEVICE", SystemInfo.deviceModel); tRepportLayout.Add("---");
            tRepport.Add("DUID", SystemInfo.deviceUniqueIdentifier); tRepportLayout.Add("---");
            // Unity3D version
            tRepport.Add("UNITY3D", Application.unityVersion); tRepportLayout.Add("---");
            //tRepport.Add("LAUNCH UNITY3D", TimeStart.ToString("F3") + "s"); tRepportLayout.Add("---");
            // Engine configuration
            if (GetPreload() == false)
            {
                tRepport.Add("PRELOAD DATAS", "No preload data (" + NWDAppConfiguration.SharedInstance().LauncherFaster.ToString() + ")"); tRepportLayout.Add("---");
            }
            else
            {
                tRepport.Add("PRELOAD DATAS", "Preload datas"); tRepportLayout.Add("---");
            }

#if NWD_LAUNCHER_BENCHMARK
                tRepport.Add("BENCHMARK STEP", "Launcher benchmarked"); tRepportLayout.Add("---");
#else
                tRepport.Add("BENCHMARK STEP", "Launcher no benchmark"); tRepportLayout.Add("---");
#endif
            tRepport.Add("SQL Version", NWDDataManager.SharedInstance().GetVersion()); tRepportLayout.Add("---");
            if (NWDDataManager.SharedInstance().IsSecure() == true)
            {
                tRepport.Add("SQL Secure", "SQL secure by SQLCipher"); tRepportLayout.Add("---");
            }
            else
            {
                tRepport.Add("SQL Secure", "SQL no secure"); tRepportLayout.Add("---");
            }
            if (CopyDatabase == true)
            {
                tRepport.Add("COPY DATABASE", "Database copied"); tRepportLayout.Add("---");
            }
            else
            {
                tRepport.Add("COPY DATABASE", "Database already copied"); tRepportLayout.Add("---");
            }
            tRepport.Add("LAUNCH NWD", TimeNWDFinish.ToString("F3") + "s"); tRepportLayout.Add("---");
            tRepport.Add("LAUNCH FINAL", TimeFinish.ToString("F3") + "s"); tRepportLayout.Add("---");
            // rows generate in memory and operations
            tRepport.Add("ROWS INFORMATIONS", RowInformations); tRepportLayout.Add("---");
            // free to remplace by informations
            tRepport.Add("INFOS", "(infos)"); tRepportLayout.Add("---");
            // free to remplace by conclusion of this benchmark
            tRepport.Add("CONCLUSION", ""); tRepportLayout.Add("---");
            // if you active benchmark you can have result in Markdown table
#if NWD_LAUNCHER_BENCHMARK
                NWDDebug.Log("benchmark : REPPORT | " + string.Join(" | ", tRepport.Keys) + " |");
                NWDDebug.Log("benchmark : REPPORT | " + string.Join(" | ", tRepportLayout) + " |");
#endif
            NWDDebug.ForceLog("benchmark : REPPORT | " + string.Join(" | ", tRepport.Values) + " |");
#endif
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
            RowInformations = string.Empty;
            CopyDatabase = false;
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
                EditorUtility.DisplayDialog("NetWorkedData Alert", "Your launcher is not started", "OK");
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
