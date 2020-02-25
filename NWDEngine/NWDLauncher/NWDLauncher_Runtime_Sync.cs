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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static partial class NWDLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        private static void Launch_Runtime_Sync()
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            StepSum = 12 +
                NWDAppConfiguration.SharedInstance().LauncherClassEditorStep + // load editor class
                NWDAppConfiguration.SharedInstance().LauncherClassAccountStep + // load account class
                NWDAppConfiguration.SharedInstance().LauncherClassEditorStep + // load editor class
                NWDAppConfiguration.SharedInstance().LauncherClassAccountStep + // load account class
                0;
            StepIndex = 0;
            // lauch engine
            Engine_Runtime_Sync();
            // declare models
            Declare_Editor();
            // restaure models' param
            Restaure_Editor();

            NotifyEngineReady();

            // connect editor
            Connect_Editor_Editor();
            // create table editor
            CreateTable_Editor_Editor();
            // load editor data
            LoadData_Editor_Editor();
            // index all data editor
            Index_Editor_Editor();

            NotifyDataEditorReady();

            // connect account
            Connect_Account_Editor();
            // create table account
            CreateTable_Account_Editor();
            // load account data account
            LoadData_Account_Editor();
            // index all data
            Index_Account_Editor();

            NotifyDataAccountReady();

            // Special NWDAppConfiguration loaded()
            NWDAppConfiguration.SharedInstance().Loaded();
            // Ready!
            Ready_Editor();

            NotifyNetWorkedDataReady();

            if (ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Engine_Runtime_Sync()
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
                if (tType != typeof(NWDBasis) && tType.IsGenericType == false)
                {
                    bool tEditorOnly = false;
                    if (tType != typeof(NWDAccount))
                    {
                        if (tType.GetCustomAttributes(typeof(NWDClassUnityEditorOnlyAttribute), true).Length > 0)
                        {
                            tEditorOnly = true;
                            NWEBenchmark.LogWarning("exclude " + tType.Name);
                        }
                    }
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