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
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
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
            NWDLauncherChronometer.Watch.Start();
            NWDBenchmarkLauncher.Start();
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

            NWDLauncherChronometer.WatchEngineLaunch = NWDLauncherChronometer.Watch.ElapsedMilliseconds;

            NotifyEngineReady();
            AddIndexMethod();

#if UNITY_EDITOR
            if (NWDAppConfiguration.SharedInstance().Installed == NWDAppInstallation.FirstInstallation)
            {
                NWDAppConfiguration.SharedInstance().Installed = NWDAppInstallation.FormInstallation;

                NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().DevEnvironment;
                NWDAppConfiguration.SharedInstance().FirstGenerateCSharpFile(tEnvironment);
                try
                {
                    AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
                }
                catch (IOException e)
                {
                    Debug.LogException(e);
                    throw;
                }
            }
            else
            {
#endif
                // connect editor
                ConnectEditorStandard();
                // create table editor
                CreateTableEditorStandard();
                // load editor data
                LoadDataEditorStandard(NWDBundle.ALL);
                // index all data editor
                IndexEditorStandard();

                NWDLauncherChronometer.WatchEditorLaunch = NWDLauncherChronometer.Watch.ElapsedMilliseconds;

                NotifyDataEditorReady();

                // need account pincode
                ConnectAccountStandard();
                // create table account
                CreateTableAccountStandard();
                // load account data account
                LoadDataAccountStandard(NWDBundle.ALL);
                // index all data
                IndexAccountStandard();

                NWDLauncherChronometer.WatchAccountLaunch = NWDLauncherChronometer.Watch.ElapsedMilliseconds;

                NotifyDataAccountReady();

                // Special NWDAppConfiguration loaded()
                NWDAppConfiguration.SharedInstance().Loaded();
                // Ready!
                Ready();

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
#if UNITY_EDITOR
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void EngineStandard()
        {
            NWDBenchmarkLauncher.Start();
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
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void DeclareStandard()
        {
            NWDBenchmarkLauncher.Start();
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
            NWDBenchmarkLauncher.Finish(true, " classes delared : " + tClassDeclare);
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void RestaureStandard()
        {
            NWDBenchmarkLauncher.Start();
            State = NWDStatut.ClassRestaureStart;
            NWDBenchmarkLauncher.Step();
            NWDAppConfiguration.SharedInstance().RestaureTypesConfigurations();
            NWDBenchmarkLauncher.Step();
            NWDDataManager.SharedInstance().ClassInEditorDatabaseRumberExpected = NWDDataManager.SharedInstance().ClassInEditorDatabaseList.Count;
            NWDBenchmarkLauncher.Step();
            NWDDataManager.SharedInstance().ClassInDeviceDatabaseNumberExpected = NWDDataManager.SharedInstance().ClassInDeviceDatabaseList.Count;
            NWDBenchmarkLauncher.Step();
            NWDDataManager.SharedInstance().ClassNumberExpected = NWDDataManager.SharedInstance().ClassInEditorDatabaseRumberExpected + NWDDataManager.SharedInstance().ClassInDeviceDatabaseNumberExpected;
            NWDBenchmarkLauncher.Step();
            State = NWDStatut.ClassRestaureFinish;
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void ConnectEditorStandard()
        {
            NWDBenchmarkLauncher.Start();
            State = NWDStatut.DataEditorConnectionStart;
            if (NWDDataManager.SharedInstance().ConnectToDatabaseEditor())
            {
                State = NWDStatut.DataEditorConnectionFinish;
            }
            else
            {
                State = NWDStatut.DataEditorConnectionError;
            }
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void CreateTableEditorStandard()
        {
            NWDBenchmarkLauncher.Start();
            State = NWDStatut.DataEditorTableCreateStart;
            NWDDataManager.SharedInstance().CreateAllTablesLocalEditor();
            State = NWDStatut.DataEditorTableCreateFinish;
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void LoadDataEditorStandard(NWDBundle sBundle)
        {
            NWDBenchmarkLauncher.Start();
            State = NWDStatut.DataEditorLoadStart;
            NWDDataManager.SharedInstance().ReloadAllObjectsInEditorDatabase(sBundle);
            State = NWDStatut.DataEditorLoadFinish;
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void IndexEditorStandard()
        {
            NWDBenchmarkLauncher.Start();
            State = NWDStatut.DataEditorIndexStart;
            State = NWDStatut.DataEditorIndexFinish;
            State = NWDStatut.EditorReady;
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void ConnectAccountStandard()
        {
            NWDBenchmarkLauncher.Start();
            State = NWDStatut.DataAccountConnectionStart;
            if (NWDDataManager.SharedInstance().ConnectToDatabaseAccount(string.Empty))
            {
                State = NWDStatut.DataAccountConnectionFinish;
            }
            else
            {
                State = NWDStatut.DataAccountConnectionError;
            }
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void CreateTableAccountStandard()
        {
            NWDBenchmarkLauncher.Start();
            State = NWDStatut.DataAccountTableCreateStart;
            NWDDataManager.SharedInstance().CreateAllTablesLocalAccount();
            State = NWDStatut.DataAccountTableCreateFinish;
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void LoadDataAccountStandard(NWDBundle sBundle)
        {
            NWDBenchmarkLauncher.Start();
            State = NWDStatut.DataAccountLoadStart;
            NWDDataManager.SharedInstance().ReloadAllObjectsInDeviceDatabase(sBundle);
            State = NWDStatut.DataAccountLoadFinish;
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void IndexAccountStandard()
        {
            NWDBenchmarkLauncher.Start();
            State = NWDStatut.DataAccountIndexStart;
            NWDDataManager.SharedInstance().IndexAllObjects();
            State = NWDStatut.DataAccountIndexFinish;
            State = NWDStatut.AccountReady;
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Ready()
        {
            NWDBenchmarkLauncher.Start();
            State = NWDStatut.NetWorkedDataReady;
            NWDBenchmarkLauncher.Finish();
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
