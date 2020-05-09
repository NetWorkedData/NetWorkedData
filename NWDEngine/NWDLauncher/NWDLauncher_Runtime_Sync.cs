﻿//=====================================================================================================================
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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static partial class NWDLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        private static void LaunchRuntimeSync()
        {
            //if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            NWDBundle tBasisBundle = NWDBundle.None;
            if (NWDAppConfiguration.SharedInstance().BundleDatas == false)
            {
                tBasisBundle = NWDBundle.ALL;
            }
            StepSum = 12 +
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

            NotifyEngineReady();

            // connect editor
            ConnectEditorStandard();
            // create table editor
            CreateTableEditorStandard();
            // load editor data
            LoadDataEditorStandard(tBasisBundle);
            // index all data editor
            IndexEditorStandard();

            NotifyDataEditorReady();

            // connect account
            ConnectAccountStandard();
            // create table account
            CreateTableAccountStandard();
            // load account data account
            LoadDataAccountStandard(tBasisBundle);
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
        private static void EngineRuntimeSync()
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
                    bool tEditorOnly = false;
                    if (tType.IsSubclassOf(typeof(NWDBasisAccountRestricted)))
                    {
                        tEditorOnly = true;
                        NWEBenchmark.LogWarning("exclude " + tType.Name);
                    }
                    //if (tType != typeof(NWDAccount))
                    //{
                    //    if (tType.GetCustomAttributes(typeof(NWDClassUnityEditorOnlyAttribute), true).Length > 0)
                    //    {
                    //        tEditorOnly = true;
                    //        NWEBenchmark.LogWarning("exclude " + tType.Name);
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
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================