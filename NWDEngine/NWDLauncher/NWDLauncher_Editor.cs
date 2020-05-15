//=====================================================================================================================
//
//  ideMobi 2020©
//
//  All rights reserved by ideMobi
//
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
            //if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
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

            NotifyDataEditorReady();

            // need account pincode
            ConnectAccountStandard();
            // create table account
            CreateTableAccountStandard();
            // load account data account
            LoadDataAccountStandard(NWDBundle.ALL);
            // index all data
            IndexAccountStandard();

            NotifyDataAccountReady();

            // Special NWDAppConfiguration loaded()
            NWDAppConfiguration.SharedInstance().Loaded();
            // Ready!
            Ready();

            NotifyNetWorkedDataReady();

            //if (ActiveBenchmark)
            {
                //TimeFinish = NWEBenchmark.SinceStartup();
                TimeFinish = Time.realtimeSinceStartup;
                TimeNWDFinish = NWEBenchmark.Finish();
                //LauncherBenchmarkToMarkdown();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void EngineStandard()
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
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
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void DeclareStandard()
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
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
                NWEBenchmark.Finish(true, " classes delared : " + tClassDeclare);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void RestaureStandard()
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            State = NWDStatut.ClassRestaureStart;
            //NWDTypeLauncher.AllTypes = AllNetWorkedDataTypes.ToArray();
            //NWEBenchmark.Start();
            NWDAppConfiguration.SharedInstance().RestaureTypesConfigurations();
            //NWEBenchmark.Finish();
            NWDDataManager.SharedInstance().ClassInEditorDatabaseRumberExpected = NWDDataManager.SharedInstance().ClassInEditorDatabaseList.Count();
            NWDDataManager.SharedInstance().ClassInDeviceDatabaseNumberExpected = NWDDataManager.SharedInstance().ClassInDeviceDatabaseList.Count();
            NWDDataManager.SharedInstance().ClassNumberExpected = NWDDataManager.SharedInstance().ClassInEditorDatabaseRumberExpected + NWDDataManager.SharedInstance().ClassInDeviceDatabaseNumberExpected;
            State = NWDStatut.ClassRestaureFinish;
            if (ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void AddIndexMethod()
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            State = NWDStatut.IndexMethodStart;
            Type[] tAllTypes = Assembly.GetExecutingAssembly().GetTypes();
            Type[] tAllHelperDTypes = (from Type type in tAllTypes where type.IsSubclassOf(typeof(NWDIndexer)) select type).ToArray();
            foreach (Type tType in tAllHelperDTypes)
            {
                if (tType != typeof(NWDIndexer))
                {
                    tType.GetMethod("Install").Invoke(null, null);
                }
            }
            State = NWDStatut.IndexMethodFinish;
            if (ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void ConnectEditorStandard()
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
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
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void CreateTableEditorStandard()
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            State = NWDStatut.DataEditorTableCreateStart;
            NWDDataManager.SharedInstance().CreateAllTablesLocalEditor();
            State = NWDStatut.DataEditorTableCreateFinish;
            if (ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void LoadDataEditorStandard(NWDBundle sBundle)
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            State = NWDStatut.DataEditorLoadStart;
            NWDDataManager.SharedInstance().ReloadAllObjectsEditor(sBundle);
            State = NWDStatut.DataEditorLoadFinish;
            if (ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void IndexEditorStandard()
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            State = NWDStatut.DataEditorIndexStart;
            State = NWDStatut.DataEditorIndexFinish;
            State = NWDStatut.EditorReady;
            if (ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void ConnectAccountStandard()
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
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
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void CreateTableAccountStandard()
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            State = NWDStatut.DataAccountTableCreateStart;
            NWDDataManager.SharedInstance().CreateAllTablesLocalAccount();
            State = NWDStatut.DataAccountTableCreateFinish;
            if (ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void LoadDataAccountStandard(NWDBundle sBundle)
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            State = NWDStatut.DataAccountLoadStart;
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount(sBundle);
            State = NWDStatut.DataAccountLoadFinish;
            if (ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void IndexAccountStandard()
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            State = NWDStatut.DataAccountIndexStart;
            NWDDataManager.SharedInstance().IndexAllObjects();
            State = NWDStatut.DataAccountIndexFinish;
            State = NWDStatut.AccountReady;
            if (ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Ready()
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            State = NWDStatut.NetWorkedDataReady;
            if (ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
#if UNITY_EDITOR
            NWDEditorConfigurationManager.Refresh();
            NWDAppConfigurationManager.Refresh();
            NWDAppEnvironmentConfigurationManager.Refresh();
            NWDModelManager.Refresh();
            NWDAppEnvironmentSync.Refresh();
            NWDAppEnvironmentChooser.Refresh();
            NWDAppConfiguration.SharedInstance().ServerEnvironmentCheck();
#endif
            //Debug.Log("!!!! BenchmarkError = " + (NWEBenchmark.BenchmarkError / 1000.0F).ToString("F3") + " s !!!!");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================