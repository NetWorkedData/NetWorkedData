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
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
    {
        //------------------------------------------------------------------------------------------------------------- 
        public void EditorRefresh()
        {
#if UNITY_EDITOR
            foreach (Type tType in ClassTypeList)
            {
                RepaintWindowsInManager(tType);
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RestaureObjectInEdition()
        {
#if UNITY_EDITOR
            foreach (Type tType in ClassTypeList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.RestaureObjectInEdition();
                //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_RestaureObjectInEdition);
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public IEnumerator AsyncReloadAllObjectsInEditorDatabase(NWDBundle sBundle)
        {

            NWDBenchmarkLauncher.Start();
            EditorDatabaseLoaded = false;
            while (EditorDatabaseConnected == false)
            {
                yield return null;
            }
            ClassInEditorDatabaseNumberLoaded = 0;
            while (ClassInEditorDatabaseNumberLoaded < ClassInEditorDatabaseRumberExpected)
            {
                ReloadClassInEditorDatabase(ClassInEditorDatabaseNumberLoaded, sBundle);
                ClassInEditorDatabaseNumberLoaded++;
                ClassNumberLoaded = ClassInEditorDatabaseNumberLoaded + ClassInDeviceDatabaseNumberLoaded;
                NWDLauncher.NotifyStep();
                if (NWDLauncher.YieldValid())
                {
                    yield return null;
                }
            }
            EditorDatabaseLoaded = true;
            //AccountLanguageLoad();
            //LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
            EditorRefresh();
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ReloadClassInEditorDatabase(int sCounter, NWDBundle sBundle)
        {
            bool rReturn = false;
            if (sCounter >= 0 && sCounter < ClassInEditorDatabaseList.Count)
            {
                Type tType = ClassInEditorDatabaseList[sCounter];
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.LoadFromDatabaseByBundle(sBundle, true);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReloadAllObjectsInEditorDatabase(NWDBundle sBundle)
        {
            NWDBenchmarkLauncher.Start();
            if (EditorDatabaseConnected == true)
            {
                //NWDBenchmark.Start("LoadData");
                EditorDatabaseLoaded = false;
                ClassInEditorDatabaseRumberExpected = ClassInEditorDatabaseList.Count;
                ClassInEditorDatabaseNumberLoaded = 0;
                //double tBenchmark = 0.0F;
                foreach (Type tType in ClassInEditorDatabaseList)
                {
                    //NWDBenchmark.Start("LoadData " + tType.Name);
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.LoadFromDatabaseByBundle(sBundle, true);
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_LoadFromDatabase);
                    ClassInEditorDatabaseNumberLoaded++;
                    ClassNumberLoaded = ClassInEditorDatabaseNumberLoaded + ClassInDeviceDatabaseNumberLoaded;
                    //tBenchmark += NWDBenchmark.Finish("LoadData " + tType.Name);
                }
                NWDDataManager.SharedInstance().EditorDatabaseLoaded = true;
                //NWDBenchmark.Finish("LoadData", true, "with total = " + tBenchmark.ToString("F5") + "s in total");
                AccountLanguageLoad();
                //LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
                EditorRefresh();
                //NWDLauncher.SetState(NWDStatut.DataEditorLoaded);
            }
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public IEnumerator AsyncReloadAllObjectsInDeviceDatabase(NWDBundle sBundle)
        {
            NWDBenchmarkLauncher.Start();
            DeviceDatabaseLoaded = false;
            while (DeviceDatabaseConnected == false)
            {
                yield return null;
            }
            ClassInDeviceDatabaseNumberExpected = ClassInDeviceDatabaseList.Count;
            ClassInDeviceDatabaseNumberLoaded = 0;
            while (ClassInDeviceDatabaseNumberLoaded < ClassInDeviceDatabaseNumberExpected)
            {
                ReloadClassInDeviceDatabase(ClassInDeviceDatabaseNumberLoaded, sBundle);
                ClassInDeviceDatabaseNumberLoaded++;
                ClassNumberLoaded = ClassInEditorDatabaseNumberLoaded + ClassInDeviceDatabaseNumberLoaded;
                NWDLauncher.NotifyStep();
                if (NWDLauncher.YieldValid())
                {
                    yield return null;
                }
            }
            DeviceDatabaseLoaded = true;
            //Debug.Log("NWDDataManager AsyncReloadAllObjects() post notification Account is loaded and All Datas is loaded");
            LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
            AccountLanguageLoad();
            EditorRefresh();
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ReloadClassInDeviceDatabase(int sCounter, NWDBundle sBundle)
        {
            bool rReturn = false;
            if (sCounter >= 0 && sCounter < ClassInDeviceDatabaseList.Count)
            {
                Type tType = ClassInDeviceDatabaseList[sCounter];
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.LoadFromDatabaseByBundle(sBundle, true);
                //tHelper.LoadFromDatabase();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReloadAllObjectsInDeviceDatabase(NWDBundle sBundle)
        {
            NWDBenchmarkLauncher.Start();
            if (DeviceDatabaseConnected == true)
            {
                DeviceDatabaseLoaded = false;
                ClassInDeviceDatabaseNumberExpected = ClassInDeviceDatabaseList.Count;
                ClassInDeviceDatabaseNumberLoaded = 0;
                foreach (Type tType in ClassInDeviceDatabaseList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.LoadFromDatabaseByBundle(sBundle, true);
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_LoadFromDatabase);
                    ClassInDeviceDatabaseNumberLoaded++;
                    ClassNumberLoaded = ClassInEditorDatabaseNumberLoaded + ClassInDeviceDatabaseNumberLoaded;
                }
                DeviceDatabaseLoaded = true;
                LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
                AccountLanguageLoad();
                EditorRefresh();
            }
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public IEnumerator AsyncIndexAllObjects()
        {

            NWDBenchmarkLauncher.Start();
            RowsCounterOp = 0;
            MethodCounterOp = 0;
            DatasIndexed = false;
            ClassNumberIndexation = 0;
            while (ClassNumberIndexation < ClassNumberExpected)
            {
                NWDBasisHelper tHelper = IndexInMemoryAllObjectsByClass(ClassNumberIndexation);
                ClassNumberIndexation++;
                NWDLauncher.NotifyStep();
                if (NWDLauncher.YieldValid())
                {
                    yield return null;
                }
                if (tHelper != null)
                {
                    //MethodCounterOp += tHelper.IndexInMemoryMethodList.Count;
                    MethodCounterOp += tHelper.IndexerInMemoryList.Count;
                    RowsCounterOp += tHelper.Datas.Count;
                }
            }
            DatasIndexed = true;
            //PlayerLanguageLoad();
            //LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
            EditorRefresh();
            NWDLauncher.RowInformations = "Rows indexed : " + RowsCounterOp + " rows. Used " + IndexationCounterOp + " operation(s) and " + MethodCounterOp + " method(s).";
            NWDBenchmarkLauncher.Log(NWDLauncher.RowInformations);
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisHelper IndexInMemoryAllObjectsByClass(int sCounter)
        {
            NWDBasisHelper tHelper = null;
            if (sCounter >= 0 && sCounter < ClassTypeList.Count)
            {
                NWDBenchmarkLauncher.Start();
                Type tType = ClassTypeList[sCounter];
                tHelper = NWDBasisHelper.FindTypeInfos(tType);
                int tRow = tHelper.IndexInMemoryAllObjects();
                NWDBenchmarkLauncher.Finish(true, " " + tHelper.ClassNamePHP + " " + tRow + " rows indexed");
            }
            return tHelper;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int IndexationCounterOp = 0;
        public int RowsCounterOp = 0;
        public int MethodCounterOp = 0;
        //-------------------------------------------------------------------------------------------------------------
        public void IndexAllObjects()
        {
            NWDBenchmarkLauncher.Start();
            RowsCounterOp = 0;
            MethodCounterOp = 0;
            DatasIndexed = false;
            foreach (Type tType in ClassTypeList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                //MethodCounterOp += tHelper.IndexInMemoryMethodList.Count;
                MethodCounterOp += tHelper.IndexerInMemoryList.Count;
                RowsCounterOp += tHelper.Datas.Count;
                tHelper.IndexInMemoryAllObjects();
            }
            DatasIndexed = true;
            EditorRefresh();
            NWDLauncher.RowInformations = "Rows indexed : " + RowsCounterOp + " rows. Used " + IndexationCounterOp + " operation(s) and " + MethodCounterOp + " method(s).";
            NWDBenchmarkLauncher.Log(NWDLauncher.RowInformations);
            NWDBenchmarkLauncher.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReloadAllObjects(NWDBundle sBundle)
        {
            ReloadAllObjectsInEditorDatabase(sBundle);
            ReloadAllObjectsInDeviceDatabase(sBundle);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
