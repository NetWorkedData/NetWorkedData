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
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Linq;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static partial class NWDLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        private static void LaunchRuntimeSync()
        {
            NWDDebug.Log("Runtime Sync log is active");
            NWDLauncherBenchmark.Watch.Start();
            //if (ActiveBenchmark)
            {
                NWDBenchmark.Start();
            }
            NWDBundle tBasisBundle = NWDBundle.None;
            if (NWDAppConfiguration.SharedInstance().BundleDatas == false)
            {
                tBasisBundle = NWDBundle.ALL;
            }
            StepSum = 13 +
                NWDAppConfiguration.SharedInstance().LauncherClassEditorStep + // load editor class
                NWDAppConfiguration.SharedInstance().LauncherClassAccountStep + // load account class
                NWDAppConfiguration.SharedInstance().LauncherClassEditorStep + // load editor class
                NWDAppConfiguration.SharedInstance().LauncherClassAccountStep + // load account class
                0;
            StepIndex = 0;
            // lauch engine
            EngineRuntimeSync();
            // declare models
            DeclareStandard();
            // restaure models' param
            RestaureStandard();


            NWDLauncherBenchmark.WatchEngineLaunch = NWDLauncherBenchmark.Watch.ElapsedMilliseconds;
            NotifyEngineReady();

            // connect editor
            AddIndexMethod();
            ConnectEditorStandard();
            // create table editor
            CreateTableEditorStandard();
            // load editor data
            LoadDataEditorStandard(tBasisBundle);
            // index all data editor
            IndexEditorStandard();

            NWDLauncherBenchmark.WatchEditorLaunch = NWDLauncherBenchmark.Watch.ElapsedMilliseconds;
            NotifyDataEditorReady();

            // connect account
            ConnectAccountStandard();
            // create table account
            CreateTableAccountStandard();
            // load account data account
            LoadDataAccountStandard(tBasisBundle);
            // index all data
            IndexAccountStandard();

            NWDLauncherBenchmark.WatchAccountLaunch = NWDLauncherBenchmark.Watch.ElapsedMilliseconds;
            NotifyDataAccountReady();

            // Special NWDAppConfiguration loaded()
            NWDAppConfiguration.SharedInstance().Loaded();
            // Ready!
            Ready();

            NWDLauncherBenchmark.WatchFinalLaunch = NWDLauncherBenchmark.Watch.ElapsedMilliseconds;
            NotifyNetWorkedDataReady();

            //if (ActiveBenchmark)
            {
                //TimeFinish = NWDBenchmark.SinceStartup();
                TimeFinish = Time.realtimeSinceStartup;
                NWDBenchmark.Finish();
                TimeNWDFinish = NWDLauncherBenchmark.Watch.ElapsedMilliseconds / 1000.0F;
                //LauncherBenchmarkToMarkdown();
            }
            NWDLauncherBenchmark.Watch.Stop();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void EngineRuntimeSync()
        {
            if (ActiveBenchmark)
            {
                NWDBenchmark.Start();
            }
            State = NWDStatut.EngineStart;
            Thread.CurrentThread.CurrentCulture = NWDConstants.FormatCountry;
            AllNetWorkedDataTypes.Clear();
            BasisToHelperList.Clear();
            List<Type> tTypeList = new List<Type>();
            Type[] tAllTypes = Assembly.GetExecutingAssembly().GetTypes();
            Type[] tAllNWDTypes = (from Type type in tAllTypes where type.IsSubclassOf(typeof(NWDTypeClass)) select type).ToArray();
            Type[] tAllHelperDTypes = (from Type type in tAllTypes where type.IsSubclassOf(typeof(NWDBasisHelper)) select type).ToArray();
            foreach (Type tType in tAllNWDTypes)
            {
                if (tType != typeof(NWDBasis) &&
                    tType != typeof(NWDBasisBundled) &&
                    tType != typeof(NWDBasisUnsynchronize) &&
                    tType != typeof(NWDBasisAccountUnsynchronize) &&
                    tType != typeof(NWDBasisAccountDependent) &&
                    tType != typeof(NWDBasisAccountPublish) &&
                    tType != typeof(NWDBasisAccountShared) &&
                    tType != typeof(NWDBasisGameSavePublish) &&
                    tType != typeof(NWDBasisGameSaveShared) &&
                    tType != typeof(NWDBasisAccountRestricted) &&
                    tType != typeof(NWDBasisGameSaveDependent) &&
                    tType.IsGenericType == false)
                {
                    bool tEditorOnly = false;
                    if (tType.IsSubclassOf(typeof(NWDBasisAccountRestricted)))
                    {
                        tEditorOnly = true;
                        NWDBenchmark.LogWarning("exclude " + tType.Name);
                    }
                    //if (tType != typeof(NWDAccount))
                    //{
                    //    if (tType.GetCustomAttributes(typeof(NWDClassUnityEditorOnlyAttribute), true).Length > 0)
                    //    {
                    //        tEditorOnly = true;
                    //        NWDBenchmark.LogWarning("exclude " + tType.Name);
                    //    }
                    //}
                    if (tEditorOnly == false)
                    {
                        if (AllNetWorkedDataTypes.Contains(tType) == false)
                        {
                            AllNetWorkedDataTypes.Add(tType);
                            foreach (Type tPossibleHelper in tAllHelperDTypes)
                            {
                                if (tPossibleHelper.ContainsGenericParameters == false)
                                {
                                    if (tPossibleHelper.BaseType.GenericTypeArguments.Contains(tType))
                                    {
                                        if (BasisToHelperList.ContainsKey(tType) == false)
                                        {
                                            BasisToHelperList.Add(tType, tPossibleHelper);
                                        }
                                        break;
                                    }
                                }
                            }
                            if (BasisToHelperList.ContainsKey(tType) == false)
                            {
                                BasisToHelperList.Add(tType, typeof(NWDBasisHelper));
                            }
                        }
                    }
                }
            }
            StepSum = StepSum + AllNetWorkedDataTypes.Count * 3;
            State = NWDStatut.EngineFinish;
            if (ActiveBenchmark)
            {
                NWDBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
