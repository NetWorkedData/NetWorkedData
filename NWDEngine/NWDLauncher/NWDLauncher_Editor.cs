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

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static partial class NWDLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        private static void LaunchStandard()
        {
            NWDDebug.Log("Editor log is active");
            NWDLauncherBenchmark.Watch.Start();
            if (ActiveBenchmark)
            {
                NWDBenchmark.Start();
            }
            NWDDataManager.SharedInstance().ClassInDeviceDatabaseList.Clear();
            NWDDataManager.SharedInstance().ClassInEditorDatabaseList.Clear();

#if UNITY_EDITOR
            EditorUtility.ClearProgressBar();
#endif
            StepSum = 13 +
                NWDAppConfiguration.SharedInstance().LauncherClassEditorStep + // load editor class
                NWDAppConfiguration.SharedInstance().LauncherClassAccountStep + // load account class
                NWDAppConfiguration.SharedInstance().LauncherClassEditorStep + // load editor class
                NWDAppConfiguration.SharedInstance().LauncherClassAccountStep + // load account class
                0;
            StepIndex = 0;
            // lauch engine
            EngineStandard();
            // declare models
            DeclareStandard();
            // restaure models' param
            RestaureStandard();

            NWDLauncherBenchmark.WatchEngineLaunch = NWDLauncherBenchmark.Watch.ElapsedMilliseconds;

            NotifyEngineReady();
            AddIndexMethod();


            // connect editor
            ConnectEditorStandard();
            // create table editor
            CreateTableEditorStandard();
            // load editor data
            LoadDataEditorStandard(NWDBundle.ALL);
            // index all data editor
            IndexEditorStandard();

            NWDLauncherBenchmark.WatchEditorLaunch = NWDLauncherBenchmark.Watch.ElapsedMilliseconds;

            NotifyDataEditorReady();

            // need account pincode
            ConnectAccountStandard();
            // create table account
            CreateTableAccountStandard();
            // load account data account
            LoadDataAccountStandard(NWDBundle.ALL);
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

            if (ActiveBenchmark)
            {
                //TimeFinish = NWDBenchmark.SinceStartup();
                TimeFinish = Time.realtimeSinceStartup;
                NWDBenchmark.Finish();
                TimeNWDFinish = NWDLauncherBenchmark.Watch.ElapsedMilliseconds / 1000.0F;
                //LauncherBenchmarkToMarkdown();
            }
            NWDLauncherBenchmark.Watch.Stop();
            NWDBenchmark.AllResults();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void EngineStandard()
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
            StepSum = StepSum + AllNetWorkedDataTypes.Count * 3;
            State = NWDStatut.EngineFinish;
            if (ActiveBenchmark)
            {
                NWDBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void DeclareStandard()
        {
            if (ActiveBenchmark)
            {
                NWDBenchmark.Start();
            }
            State = NWDStatut.ClassDeclareStart;
            int tClassDeclare = 0;
            foreach (Type tType in AllNetWorkedDataTypes)
            {
                if (tType != typeof(NWDBasis))
                {
                    tClassDeclare++;
                    NWDBasisHelper tHelper = NWDBasisHelper.Declare(tType, BasisToHelperList[tType]);
                }
            }
            State = NWDStatut.ClassDeclareFinish;
            if (ActiveBenchmark)
            {
                NWDBenchmark.Finish(true, " classes delared : " + tClassDeclare);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void RestaureStandard()
        {
            if (ActiveBenchmark)
            {
                NWDBenchmark.Start();
            }
            State = NWDStatut.ClassRestaureStart;
            if (ActiveBenchmark)
            {
                NWDBenchmark.Step();
            }
            NWDAppConfiguration.SharedInstance().RestaureTypesConfigurations();
            if (ActiveBenchmark)
            {
                NWDBenchmark.Step();
            }
            NWDDataManager.SharedInstance().ClassInEditorDatabaseRumberExpected = NWDDataManager.SharedInstance().ClassInEditorDatabaseList.Count;
            if (ActiveBenchmark)
            {
                NWDBenchmark.Step();
            }
            NWDDataManager.SharedInstance().ClassInDeviceDatabaseNumberExpected = NWDDataManager.SharedInstance().ClassInDeviceDatabaseList.Count;
            if (ActiveBenchmark)
            {
                NWDBenchmark.Step();
            }
            NWDDataManager.SharedInstance().ClassNumberExpected = NWDDataManager.SharedInstance().ClassInEditorDatabaseRumberExpected + NWDDataManager.SharedInstance().ClassInDeviceDatabaseNumberExpected;
            if (ActiveBenchmark)
            {
                NWDBenchmark.Step();
            }
            State = NWDStatut.ClassRestaureFinish;
            if (ActiveBenchmark)
            {
                NWDBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void ConnectEditorStandard()
        {
            if (ActiveBenchmark)
            {
                NWDBenchmark.Start();
            }
            State = NWDStatut.DataEditorConnectionStart;
            if (NWDDataManager.SharedInstance().ConnectToDatabaseEditor())
            {
                State = NWDStatut.DataEditorConnectionFinish;
            }
            else
            {
                State = NWDStatut.DataEditorConnectionError;
            }
            if (ActiveBenchmark)
            {
                NWDBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void CreateTableEditorStandard()
        {
            if (ActiveBenchmark)
            {
                NWDBenchmark.Start();
            }
            State = NWDStatut.DataEditorTableCreateStart;
            NWDDataManager.SharedInstance().CreateAllTablesLocalEditor();
            State = NWDStatut.DataEditorTableCreateFinish;
            if (ActiveBenchmark)
            {
                NWDBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void LoadDataEditorStandard(NWDBundle sBundle)
        {
            if (ActiveBenchmark)
            {
                NWDBenchmark.Start();
            }
            State = NWDStatut.DataEditorLoadStart;
            NWDDataManager.SharedInstance().ReloadAllObjectsInEditorDatabase(sBundle);
            State = NWDStatut.DataEditorLoadFinish;
            if (ActiveBenchmark)
            {
                NWDBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void IndexEditorStandard()
        {
            if (ActiveBenchmark)
            {
                NWDBenchmark.Start();
            }
            State = NWDStatut.DataEditorIndexStart;
            State = NWDStatut.DataEditorIndexFinish;
            State = NWDStatut.EditorReady;
            if (ActiveBenchmark)
            {
                NWDBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void ConnectAccountStandard()
        {
            if (ActiveBenchmark)
            {
                NWDBenchmark.Start();
            }
            State = NWDStatut.DataAccountConnectionStart;
            if (NWDDataManager.SharedInstance().ConnectToDatabaseAccount(string.Empty))
            {
                State = NWDStatut.DataAccountConnectionFinish;
            }
            else
            {
                State = NWDStatut.DataAccountConnectionError;
            }
            if (ActiveBenchmark)
            {
                NWDBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void CreateTableAccountStandard()
        {
            if (ActiveBenchmark)
            {
                NWDBenchmark.Start();
            }
            State = NWDStatut.DataAccountTableCreateStart;
            NWDDataManager.SharedInstance().CreateAllTablesLocalAccount();
            State = NWDStatut.DataAccountTableCreateFinish;
            if (ActiveBenchmark)
            {
                NWDBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void LoadDataAccountStandard(NWDBundle sBundle)
        {
            if (ActiveBenchmark)
            {
                NWDBenchmark.Start();
            }
            State = NWDStatut.DataAccountLoadStart;
            NWDDataManager.SharedInstance().ReloadAllObjectsInDeviceDatabase(sBundle);
            State = NWDStatut.DataAccountLoadFinish;
            if (ActiveBenchmark)
            {
                NWDBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void IndexAccountStandard()
        {
            if (ActiveBenchmark)
            {
                NWDBenchmark.Start();
            }
            State = NWDStatut.DataAccountIndexStart;
            NWDDataManager.SharedInstance().IndexAllObjects();
            State = NWDStatut.DataAccountIndexFinish;
            State = NWDStatut.AccountReady;
            if (ActiveBenchmark)
            {
                NWDBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Ready()
        {
            if (ActiveBenchmark)
            {
                NWDBenchmark.Start();
            }
            State = NWDStatut.NetWorkedDataReady;
            if (ActiveBenchmark)
            {
                NWDBenchmark.Finish();
            }
#if UNITY_EDITOR
            NWDProjectConfigurationManager.Refresh();
            NWDAppConfigurationManager.Refresh();
            NWDAppEnvironmentConfigurationManager.Refresh();
            NWDModelManager.Refresh();
            NWDAppEnvironmentSync.Refresh();
            NWDAppEnvironmentChooser.Refresh();
            NWDAppConfiguration.SharedInstance().ServerEnvironmentCheck();
#endif
            //Debug.Log("!!!! BenchmarkError = " + (NWDBenchmark.BenchmarkError / 1000.0F).ToString("F3") + " s !!!!");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================