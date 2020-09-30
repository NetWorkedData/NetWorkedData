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
    public partial class NWDGameDataManager : NWDCallBackDataLoadOnly
    {
        //-------------------------------------------------------------------------------------------------------------
        private void LaunchRuntimeAsync()
        {
            NWDDebug.Log("Runtime Async log is active");
            if (NWDLauncher.GetPreload() == false)
            {
                if (NWDLauncher.GetState() != NWDStatut.NetWorkedDataReady)
                {
                    // Load async the engine!
                    //Debug.Log("########## <color=blue>Load async the engine</color>!");
                    StartCoroutine(NWDLauncher.LaunchRuntimeAsync());
                }
                else
                {
                    //Debug.Log("########## <color=blue>Load async the engine ALL READY READY!</color>!");
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static partial class NWDLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        public static IEnumerator LaunchRuntimeAsync()
        {
            NWDLauncherChronometer.Watch.Start();
            NWDBenchmarkLauncher.Start();
            NWDBundle tBasisBundle = NWDBundle.None;
            if (NWDAppConfiguration.SharedInstance().BundleDatas == false)
            {
                tBasisBundle = NWDBundle.ALL;
            }
            IEnumerator tWaitTime = null;
            StepSum = 12 +
                NWDAppConfiguration.SharedInstance().LauncherClassEditorStep + // load editor class
                NWDAppConfiguration.SharedInstance().LauncherClassAccountStep + // load account class
                NWDAppConfiguration.SharedInstance().LauncherClassEditorStep + // load editor class
                NWDAppConfiguration.SharedInstance().LauncherClassAccountStep + // load account class
                0;
            StepIndex = 0;
            NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_LAUNCHER_STEP); // to init the gauge to 0
            // lauch engine
            tWaitTime = EngineRuntimeAsync();
            NotifyStep(true);
            yield return tWaitTime;
            // declare models
            tWaitTime = DeclareRuntimeAsync();
            NotifyStep(true);
            yield return tWaitTime;
            // restaure models' param
            tWaitTime = null;
            RestaureStandard();
            NotifyStep(true);
            yield return tWaitTime;

            StepSum = 13 +
                NWDDataManager.SharedInstance().ClassInEditorDatabaseRumberExpected + // load editor class
                NWDDataManager.SharedInstance().ClassInDeviceDatabaseNumberExpected + // load account class
                NWDDataManager.SharedInstance().ClassInEditorDatabaseRumberExpected + // index editor class
                NWDDataManager.SharedInstance().ClassInDeviceDatabaseNumberExpected + // index account class
                0;

            NWDLauncherChronometer.WatchEngineLaunch = NWDLauncherChronometer.Watch.ElapsedMilliseconds;
            NotifyEngineReady();



            // connect editor
            tWaitTime = null;
            AddIndexMethod();
            NotifyStep(true);
            yield return tWaitTime;
            ConnectEditorStandard();
            NotifyStep(true);
            yield return tWaitTime;
            // create table editor
            tWaitTime = null;
            CreateTableEditorStandard();
            NotifyStep(true);
            yield return tWaitTime;
            // load editor data
            tWaitTime = NWDDataManager.SharedInstance().AsyncReloadAllObjectsInEditorDatabase(tBasisBundle);
            NotifyStep(true);
            yield return tWaitTime;
            // index all data editor
            //tWaitTime = NWDDataManager.SharedInstance().AsyncIndexAllObjects();
            //yield return tWaitTime;

            NWDLauncherChronometer.WatchEditorLaunch = NWDLauncherChronometer.Watch.ElapsedMilliseconds;
            NotifyDataEditorReady();

            // need account pincode
            tWaitTime = null;
            ConnectAccountStandard();
            NotifyStep(true);
            yield return tWaitTime;
            // create table account
            tWaitTime = null;
            CreateTableAccountStandard();
            NotifyStep(true);
            yield return tWaitTime;

            NWDLauncherChronometer.WatchAccountLaunch = NWDLauncherChronometer.Watch.ElapsedMilliseconds;
            NotifyDataAccountReady();
            NotifyStep(true);

            // load account data account
            tWaitTime = NWDDataManager.SharedInstance().AsyncReloadAllObjectsInDeviceDatabase(tBasisBundle);
            yield return tWaitTime;
            // index all data
            tWaitTime = NWDDataManager.SharedInstance().AsyncIndexAllObjects();
            yield return tWaitTime;
            // Special NWDAppConfiguration loaded()
            tWaitTime = null;
            NWDAppConfiguration.SharedInstance().Loaded();
            NotifyStep(true);
            yield return tWaitTime;
            // Ready!
            tWaitTime = null;
            Ready();
            NotifyStep(true);
            yield return tWaitTime;


            //NWDBenchmark.Log(" NWDDataManager.SharedInstance().ClassEditorExpected = " + NWDDataManager.SharedInstance().ClassEditorExpected);
            //NWDBenchmark.Log(" NWDDataManager.SharedInstance().ClassAccountExpected = " + NWDDataManager.SharedInstance().ClassAccountExpected);
            //NWDBenchmark.Log(" StepSum = " + StepSum + " and StepIndex =" + StepIndex);

                //TimeFinish = NWDBenchmark.SinceStartup();
                TimeFinish = Time.realtimeSinceStartup;
                TimeNWDFinish = NWDLauncherChronometer.Watch.ElapsedMilliseconds / 1000.0F;
                LauncherBenchmarkToMarkdown();
                if (NWBBenchmarkResult.CurrentData() != null)
                {
                    NWBBenchmarkResult.CurrentData().BenchmarkNow();
                }
            NWDBenchmarkLauncher.Finish();
            NWDLauncherChronometer.WatchFinalLaunch = NWDLauncherChronometer.Watch.ElapsedMilliseconds;
            NotifyNetWorkedDataReady();
            NWDLauncherChronometer.Watch.Stop();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static IEnumerator EngineRuntimeAsync()
        {

            NWDBenchmarkLauncher.Start();
            State = NWDStatut.EngineStart;
            Thread.CurrentThread.CurrentCulture = NWDConstants.FormatCountry;
            AllNetWorkedDataTypes.Clear();
            BasisToHelperList.Clear();
            //NWDBenchmark.Start("get_refelexion");
            List<Type> tTypeList = new List<Type>();
            Type[] tAllTypes = Assembly.GetExecutingAssembly().GetTypes();
            Type[] tAllNWDTypes = (from Type type in tAllTypes where type.IsSubclassOf(typeof(NWDTypeClass)) select type).ToArray();
            Type[] tAllHelperDTypes = (from Type type in tAllTypes where type.IsSubclassOf(typeof(NWDBasisHelper)) select type).ToArray();
            //NWDBenchmark.Finish("get_refelexion");
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
                if (YieldValid())
                {
                    yield return null;
                }
            }
            StepSum = StepSum + AllNetWorkedDataTypes.Count * 3;
            State = NWDStatut.EngineFinish;
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static IEnumerator DeclareRuntimeAsync()
        {
            NWDBenchmarkLauncher.Start();
            State = NWDStatut.ClassDeclareStart;
            foreach (Type tType in AllNetWorkedDataTypes)
            {
                if (tType != typeof(NWDBasis))
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.Declare(tType, BasisToHelperList[tType]);
                    State = NWDStatut.ClassDeclareStep;
                }
                if (YieldValid())
                {
                    yield return null;
                }
            }
            State = NWDStatut.ClassDeclareFinish;
            NotifyStep();
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
