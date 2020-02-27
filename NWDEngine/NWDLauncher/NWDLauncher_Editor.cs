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
        private static void Launch_Editor()
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
#if UNITY_EDITOR
            EditorUtility.ClearProgressBar();
#endif
            StepSum = 12 +
                NWDAppConfiguration.SharedInstance().LauncherClassEditorStep + // load editor class
                NWDAppConfiguration.SharedInstance().LauncherClassAccountStep + // load account class
                NWDAppConfiguration.SharedInstance().LauncherClassEditorStep + // load editor class
                NWDAppConfiguration.SharedInstance().LauncherClassAccountStep + // load account class
                0;
            StepIndex = 0;
            // lauch engine
            Engine_Editor();
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

            // need account pincode
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
                double tNWDFinish = NWEBenchmark.Finish();
                LauncherBenchmarkToMarkdown(tNWDFinish);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Engine_Editor()
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
        private static void Declare_Editor()
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            State = NWDStatut.ClassDeclareStart;
            foreach (Type tType in AllNetWorkedDataTypes)
            {
                if (tType != typeof(NWDBasis))
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.Declare(tType, BasisToHelperList[tType]);
                }
            }
            State = NWDStatut.ClassDeclareFinish;
            if (ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Restaure_Editor()
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
            NWDDataManager.SharedInstance().ClassEditorExpected = NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Count();
            NWDDataManager.SharedInstance().ClassAccountExpected = NWDDataManager.SharedInstance().mTypeAccountDependantList.Count();
            NWDDataManager.SharedInstance().ClassExpected = NWDDataManager.SharedInstance().ClassEditorExpected + NWDDataManager.SharedInstance().ClassAccountExpected;
            State = NWDStatut.ClassRestaureFinish;
            if (ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Connect_Editor_Editor()
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
        private static void CreateTable_Editor_Editor()
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
        private static void LoadData_Editor_Editor()
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            State = NWDStatut.DataEditorLoadStart;
            NWDDataManager.SharedInstance().ReloadAllObjectsEditor();
            State = NWDStatut.DataEditorLoadFinish;
            if (ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Index_Editor_Editor()
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
        private static void Connect_Account_Editor()
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
        private static void CreateTable_Account_Editor()
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
        private static void LoadData_Account_Editor()
        {
            if (ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            State = NWDStatut.DataAccountLoadStart;
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            State = NWDStatut.DataAccountLoadFinish;
            if (ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Index_Account_Editor()
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
        private static void Ready_Editor()
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
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================