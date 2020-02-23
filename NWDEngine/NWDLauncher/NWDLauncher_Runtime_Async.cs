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
    public partial class NWDGameDataManager : NWDCallBackDataLoadOnly
    {
        //-------------------------------------------------------------------------------------------------------------
        private void Launch_Runtime_Async()
        {
            if (NWDAppConfiguration.SharedInstance().PreloadDatas == false)
            {
                if (NWDLauncher.GetState() != NWDStatut.NetWorkedDataReady)
                {
                    // Load async the engine!
                    //Debug.Log("########## <color=blue>Load async the engine</color>!");
                    StartCoroutine(NWDLauncher.Launch_Runtime_Async());
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
        public static IEnumerator Launch_Runtime_Async()
        {
            NWEBenchmark.Start();
            IEnumerator tWaitTime = null;
            StepSum = 1024;
            StepIndex = 0;
            // lauch engine
            tWaitTime = Engine_Runtime_Async();
            NotifyStep();
            yield return tWaitTime;
            // declare models
            tWaitTime = Declare_Runtime_Async();
            NotifyStep();
            yield return tWaitTime;
            // restaure models' param
            tWaitTime = null;
            Restaure_Editor();
            NotifyStep();
            yield return tWaitTime;

            StepSum = 12 +
                NWDDataManager.SharedInstance().ClassEditorExpected + // load editor class
                NWDDataManager.SharedInstance().ClassAccountExpected + // load account class
                NWDDataManager.SharedInstance().ClassEditorExpected + // index editor class
                NWDDataManager.SharedInstance().ClassAccountExpected + // index account class
                0;

            NotifyEngineReady();

            // connect editor
            tWaitTime = null;
            Connect_Editor_Editor();
            NotifyStep();
            yield return tWaitTime;
            // create table editor
            tWaitTime = null;
            CreateTable_Editor_Editor();
            NotifyStep();
            yield return tWaitTime;
            // load editor data
            tWaitTime = NWDDataManager.SharedInstance().AsyncReloadAllObjectsEditor();
            NotifyStep();
            yield return tWaitTime;
            // index all data editor
            //tWaitTime = NWDDataManager.SharedInstance().AsyncIndexAllObjects();
            //yield return tWaitTime;

            NotifyDataEditorReady();

            // need account pincode
            tWaitTime = null;
            Connect_Account_Editor();
            NotifyStep();
            yield return tWaitTime;
            // create table account
            tWaitTime = null;
            CreateTable_Account_Editor();
            NotifyStep();
            yield return tWaitTime;

            NotifyDataAccountReady();
            NotifyStep();

            // load account data account
            tWaitTime = NWDDataManager.SharedInstance().AsyncReloadAllObjectsAccount();
            yield return tWaitTime;
            // index all data
            tWaitTime = NWDDataManager.SharedInstance().AsyncIndexAllObjects();
            yield return tWaitTime;
            // Special NWDAppConfiguration loaded()
            tWaitTime = null;
            NWDAppConfiguration.SharedInstance().Loaded();
            NotifyStep();
            yield return tWaitTime;
            // Ready!
            tWaitTime = null;
            Ready_Editor();
            NotifyStep();
            yield return tWaitTime;

            NotifyNetWorkedDataReady();

            NWEBenchmark.Log(" NWDDataManager.SharedInstance().ClassEditorExpected = " + NWDDataManager.SharedInstance().ClassEditorExpected);
            NWEBenchmark.Log(" NWDDataManager.SharedInstance().ClassAccountExpected = " + NWDDataManager.SharedInstance().ClassAccountExpected);
            NWEBenchmark.Log(" StepSum = " + StepSum + " and StepIndex =" + StepIndex);

            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static IEnumerator Engine_Runtime_Async()
        {
            NWEBenchmark.Start();
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
                if (tType != typeof(NWDBasis))
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
                        AllNetWorkedDataTypes.Add(tType);
                        foreach (Type tPossibleHelper in tAllHelperDTypes)
                        {
                            if (tPossibleHelper.ContainsGenericParameters == false)
                            {
                                if (tPossibleHelper.BaseType.GenericTypeArguments.Contains(tType))
                                {
                                    BasisToHelperList.Add(tType, tPossibleHelper);
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
                yield return null;
            }
            StepSum = StepSum + AllNetWorkedDataTypes.Count * 3;
            State = NWDStatut.EngineFinish;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static IEnumerator Declare_Runtime_Async()
        {
            NWEBenchmark.Start();
            State = NWDStatut.ClassDeclareStart;
            foreach (Type tType in AllNetWorkedDataTypes)
            {
                if (tType != typeof(NWDBasis))
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.Declare(tType, BasisToHelperList[tType]);
                    State = NWDStatut.ClassDeclareStep;
                }
                yield return null;
            }
            State = NWDStatut.ClassDeclareFinish;
            NotifyStep();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================