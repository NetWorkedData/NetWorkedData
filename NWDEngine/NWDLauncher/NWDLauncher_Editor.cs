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
            NWEBenchmark.Start();
#if UNITY_EDITOR
            EditorUtility.ClearProgressBar();
#endif
            StepSum = 12;
            StepIndex = 0;
            // lauch engine
            Engine_Editor();
            // declare models
            Declare_Editor();
            // restaure models' param
            Restaure_Editor();
            // connect editor
            Connect_Editor_Editor();
            // create table editor
            CreateTable_Editor_Editor();
            // load editor data
            LoadData_Editor_Editor();
            // index all data editor
            Index_Editor_Editor();
            // need account pincode
            Connect_Account_Editor();
            // create table account
            CreateTable_Account_Editor();
            // load account data account
            LoadData_Account_Editor();
            // index all data
            Index_Account_Editor();
            // Special NWDAppConfiguration loaded()
            NWDAppConfiguration.SharedInstance().Loaded();
            // Ready!
            Ready_Editor();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Engine_Editor()
        {
            NWEBenchmark.Start();
            State = NWDStatut.EngineLaunching;
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
            StepIndcrement();
            StepSum = StepSum + AllNetWorkedDataTypes.Count * 3;
            State = NWDStatut.EngineLaunched;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Declare_Editor()
        {
            NWEBenchmark.Start();
            State = NWDStatut.ClassDeclareStart;
            foreach (Type tType in AllNetWorkedDataTypes)
            {
                if (tType != typeof(NWDBasis))
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.Declare(tType, BasisToHelperList[tType]);
                }
            }
            StepIndcrement();
            State = NWDStatut.ClassDeclareFinish;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Restaure_Editor()
        {
            NWEBenchmark.Start();
            State = NWDStatut.ClassRestaureStart;
            //NWDTypeLauncher.AllTypes = AllNetWorkedDataTypes.ToArray();
            NWDAppConfiguration.SharedInstance().RestaureTypesConfigurations();
            NWDDataManager.SharedInstance().ClassEditorExpected = NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Count();
            NWDDataManager.SharedInstance().ClassAccountExpected = NWDDataManager.SharedInstance().mTypeAccountDependantList.Count();
            NWDDataManager.SharedInstance().ClassExpected = NWDDataManager.SharedInstance().ClassEditorExpected + NWDDataManager.SharedInstance().ClassAccountExpected;
            StepIndcrement();
            State = NWDStatut.ClassRestaureFinish;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Connect_Editor_Editor()
        {
            NWEBenchmark.Start();
            State = NWDStatut.DataEditorConnecting;
            if (NWDDataManager.SharedInstance().ConnectToDatabaseEditor())
            {
                State = NWDStatut.DataEditorConnected;
            }
            else
            {
                State = NWDStatut.Error;
            }
            StepIndcrement();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void CreateTable_Editor_Editor()
        {
            NWEBenchmark.Start();
            DatabaseEditorTable();
            StepIndcrement();
            State = NWDStatut.DataEditorTableUpdated;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void LoadData_Editor_Editor()
        {
            NWEBenchmark.Start();
            State = NWDStatut.DataEditorLoading;
            NWDDataManager.SharedInstance().ReloadAllObjectsEditor();
            StepIndcrement();
            State = NWDStatut.DataEditorLoaded;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Index_Editor_Editor()
        {
            NWEBenchmark.Start();
            State = NWDStatut.DataEditorIndexationStart;
            //NWDDataManager.SharedInstance().IndexAllObjects();
            StepIndcrement();
            State = NWDStatut.DataEditorIndexationFinish;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Connect_Account_Editor()
        {
            NWEBenchmark.Start();
            State = NWDStatut.DataAccountConnecting;
            //ConnectToDatabaseAccount();
            DatabaseAccountConnection(string.Empty);
            StepIndcrement();
            State = NWDStatut.DataAccountConnected;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void CreateTable_Account_Editor()
        {
            NWEBenchmark.Start();
            DatabaseAccountTable();
            State = NWDStatut.DataAccountTableUpdated;
            StepIndcrement();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void LoadData_Account_Editor()
        {
            NWEBenchmark.Start();
            State = NWDStatut.DataAccountLoading;
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            StepIndcrement();
            State = NWDStatut.DataAccountLoaded;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Index_Account_Editor()
        {
            NWEBenchmark.Start();
            State = NWDStatut.DataIndexationStart;
            NWDDataManager.SharedInstance().IndexAllObjects();
            StepIndcrement();
            State = NWDStatut.DataIndexationFinish;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Ready_Editor()
        {
            NWEBenchmark.Start();
            StepIndcrement();
            State = NWDStatut.NetWorkedDataReady;
            NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_ENGINE_READY);
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================