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
        const string NWDLauncher_Editor = "NWDLauncher_Editor";
        //-------------------------------------------------------------------------------------------------------------
        private static void Launch_Editor()
        {
#if UNITY_EDITOR
            NWEBenchmark.Start(NWDLauncher_Editor);
            EditorUtility.ClearProgressBar();
            // lauch engine
            Engine_Editor();
            // declare models
            Declare_Editor();
            // restaure models' param
            Restaure_Editor();
            // connect editor
            Connect_Editor_Editor();
            // load editor data
            LoadData_Editor_Editor();
            // index all data editor
            Index_Editor_Editor();
            // need account pincode
            PinCode_Account_Editor(string.Empty);
            // next in the PinCode_Account_Editor() method
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        private static void Engine_Editor()
        {
            NWEBenchmark.Start();

            State = NWDStatut.EngineLaunching;
            Thread.CurrentThread.CurrentCulture = NWDConstants.FormatCountry;
            AllNetWorkedDataTypes.Clear();

            List<Type> tTypeList = new List<Type>();

            List<Type> BasisTypeList = new List<Type>();
            Type[] tAllTypes = Assembly.GetExecutingAssembly().GetTypes();
            // sort and filter by NWDBasis (NWDTypeClass subclass)
            Type[] tAllNWDTypes = (from Type type in tAllTypes where type.IsSubclassOf(typeof(NWDTypeClass)) select type).ToArray();
            Type[] tAllHelperDTypes = (from Type type in tAllTypes where type.IsSubclassOf(typeof(NWDBasisHelper)) select type).ToArray();
            foreach (Type tType in tAllNWDTypes)
            {
                if (tType != typeof(NWDBasis))
                {
                    BasisTypeList.Add(tType);
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
            State = NWDStatut.ClassDeclareFinish;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Restaure_Editor()
        {
            NWEBenchmark.Start();
            State = NWDStatut.ClassRestaureStart;
            NWDTypeLauncher.AllTypes = AllNetWorkedDataTypes.ToArray();
            NWDAppConfiguration.SharedInstance().RestaureTypesConfigurations();
            NWDDataManager.SharedInstance().ClassEditorExpected = NWDDataManager.SharedInstance().mTypeNotAccountDependantList.Count();
            NWDDataManager.SharedInstance().ClassAccountExpected = NWDDataManager.SharedInstance().mTypeAccountDependantList.Count();
            NWDDataManager.SharedInstance().ClassExpected = NWDDataManager.SharedInstance().ClassEditorExpected + NWDDataManager.SharedInstance().ClassAccountExpected;
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
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void LoadData_Editor_Editor()
        {
            NWEBenchmark.Start();
            State = NWDStatut.DataEditorLoading;
            NWDDataManager.SharedInstance().ReloadAllObjectsEditor();
            State = NWDStatut.DataEditorLoaded;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Index_Editor_Editor()
        {
            NWEBenchmark.Start();
            State = NWDStatut.DataEditorIndexationStart;
            NWDDataManager.SharedInstance().IndexAllObjects();
            State = NWDStatut.DataEditorIndexationFinish;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void PinCode_Account_Editor(string sPinCode)
        {
            NWEBenchmark.Start();
            // connect account
            Connect_Account_Editor();
            // load account data account
            LoadData_Account_Editor();
            // index all data
            Index_Account_Editor();
            NWEBenchmark.Finish();
            // Ready!
            Ready_Editor();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Connect_Account_Editor()
        {
            NWEBenchmark.Start();
            State = NWDStatut.DataAccountConnecting;
            ConnectToDatabaseAccount();
            State = NWDStatut.DataAccountConnected;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void LoadData_Account_Editor()
        {
            NWEBenchmark.Start();
            State = NWDStatut.DataAccountLoading;
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            State = NWDStatut.DataAccountLoaded;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Index_Account_Editor()
        {
            NWEBenchmark.Start();
            State = NWDStatut.DataIndexationStart;
            NWDDataManager.SharedInstance().IndexAllObjects();
            State = NWDStatut.DataIndexationFinish;
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        private static void Ready_Editor()
        {
            NWEBenchmark.Start();
            State = NWDStatut.NetWorkedDataReady;
            NWEBenchmark.Finish();
            NWEBenchmark.Finish(NWDLauncher_Editor);
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================