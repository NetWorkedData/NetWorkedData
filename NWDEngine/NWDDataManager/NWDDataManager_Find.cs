//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:27:8
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;
//using BasicToolBox;

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
            foreach (Type tType in mTypeList)
            {
                RepaintWindowsInManager(tType);
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RestaureObjectInEdition()
        {
#if UNITY_EDITOR
            foreach (Type tType in mTypeList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.RestaureObjectInEdition();
                //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_RestaureObjectInEdition);
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public IEnumerator AsyncReloadAllObjectsEditor(NWDBasisBundle sBundle)
        {
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            DataEditorLoaded = false;
            while (DataEditorConnected == false)
            {
                yield return null;
            }
            ClassEditorDataLoaded = 0;
            while (ClassEditorDataLoaded < ClassEditorExpected)
            {
                ReloadAllObjectsByClassEditor(ClassEditorDataLoaded, sBundle);
                ClassEditorDataLoaded++;
                ClassDataLoaded = ClassEditorDataLoaded + ClassAccountDataLoaded;
                NWDLauncher.NotifyStep();
                if (NWDLauncher.YieldValid())
                {
                    yield return null;
                }
            }
            DataEditorLoaded = true;
            PlayerLanguageLoad();
            //LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
            EditorRefresh();
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ReloadAllObjectsByClassEditor(int sCounter, NWDBasisBundle sBundle)
        {
            bool rReturn = false;
            if (sCounter >= 0 && sCounter < mTypeNotAccountDependantList.Count)
            {
                Type tType = mTypeNotAccountDependantList[sCounter];
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.LoadFromDatabaseByBundle(sBundle);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReloadAllObjectsEditor(NWDBasisBundle sBundle)
        {
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            if (DataEditorConnected == true)
            {
                //NWEBenchmark.Start("LoadData");
                DataEditorLoaded = false;
                ClassEditorExpected = mTypeNotAccountDependantList.Count();
                ClassEditorDataLoaded = 0;
                //double tBenchmark = 0.0F;
                foreach (Type tType in mTypeNotAccountDependantList)
                {
                    //NWEBenchmark.Start("LoadData " + tType.Name);
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.LoadFromDatabaseByBundle(sBundle);
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_LoadFromDatabase);
                    ClassEditorDataLoaded++;
                    ClassDataLoaded = ClassEditorDataLoaded + ClassAccountDataLoaded;
                    //tBenchmark += NWEBenchmark.Finish("LoadData " + tType.Name);
                }
                NWDDataManager.SharedInstance().DataEditorLoaded = true;
                //NWEBenchmark.Finish("LoadData", true, "with total = " + tBenchmark.ToString("F5") + "s in total");
                PlayerLanguageLoad();
                //LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
                EditorRefresh();
                //NWDLauncher.SetState(NWDStatut.DataEditorLoaded);
            }
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public IEnumerator AsyncReloadAllObjectsAccount(NWDBasisBundle sBundle)
        {
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            DataAccountLoaded = false;
            while (DataAccountConnected == false)
            {
                yield return null;
            }
            ClassAccountExpected = mTypeAccountDependantList.Count();
            ClassAccountDataLoaded = 0;
            while (ClassAccountDataLoaded < ClassAccountExpected)
            {
                ReloadAllObjectsByClassAccount(ClassAccountDataLoaded, sBundle);
                ClassAccountDataLoaded++;
                ClassDataLoaded = ClassEditorDataLoaded + ClassAccountDataLoaded;
                NWDLauncher.NotifyStep();
                if (NWDLauncher.YieldValid())
                {
                    yield return null;
                }
            }
            DataAccountLoaded = true;
            //Debug.Log("NWDDataManager AsyncReloadAllObjects() post notification Account is loaded and All Datas is loaded");
            PlayerLanguageLoad();
            LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
            EditorRefresh();
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ReloadAllObjectsByClassAccount(int sCounter, NWDBasisBundle sBundle)
        {
            bool rReturn = false;
            if (sCounter >= 0 && sCounter < mTypeAccountDependantList.Count)
            {
                Type tType = mTypeAccountDependantList[sCounter];
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.LoadFromDatabaseByBundle(sBundle);
                //tHelper.LoadFromDatabase();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReloadAllObjectsAccount(NWDBasisBundle sBundle)
        {
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            if (DataAccountConnected == true)
            {
                DataAccountLoaded = false;
                ClassAccountExpected = mTypeAccountDependantList.Count();
                ClassAccountDataLoaded = 0;
                foreach (Type tType in mTypeAccountDependantList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.LoadFromDatabaseByBundle(sBundle);
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_LoadFromDatabase);
                    ClassAccountDataLoaded++;
                    ClassDataLoaded = ClassEditorDataLoaded + ClassAccountDataLoaded;
                }
                DataAccountLoaded = true;
                PlayerLanguageLoad();
                LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
                EditorRefresh();
            }
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public IEnumerator AsyncIndexAllObjects()
        {
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            int tRow = 0;
            int tMethod = 0;
            DatasIndexed = false;
            ClassIndexation = 0;
            while (ClassIndexation < ClassExpected)
            {
                NWDBasisHelper tHelper = IndexInMemoryAllObjectsByClass(ClassIndexation);
                ClassIndexation++;
                NWDLauncher.NotifyStep();
                if (NWDLauncher.YieldValid())
                {
                    yield return null;
                }
                if (tHelper != null)
                {
                    tMethod += tHelper.IndexInMemoryMethodList.Count;
                    tRow += tHelper.Datas.Count;
                }
            }
            DatasIndexed = true;
            //PlayerLanguageLoad();
            //LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
            EditorRefresh();
            NWDLauncher.RowInformations = "Rows indexed : " + tRow + " rows. Used " + IndexationCounterOp + " operation(s) and " + tMethod + " method(s).";
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Log(NWDLauncher.RowInformations);
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisHelper IndexInMemoryAllObjectsByClass(int sCounter)
        {
            NWDBasisHelper tHelper = null;
            if (sCounter >= 0 && sCounter < mTypeList.Count)
            {
                if (NWDLauncher.ActiveBenchmark)
                {
                    NWEBenchmark.Start();
                }
                Type tType = mTypeList[sCounter];
                tHelper = NWDBasisHelper.FindTypeInfos(tType);
                int tRow = tHelper.IndexInMemoryAllObjects();
                if (NWDLauncher.ActiveBenchmark)
                {
                    NWEBenchmark.Finish(true, " " + tHelper.ClassNamePHP + " " + tRow + " rows indexed");
                }
            }
            return tHelper;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int IndexationCounterOp = 0;
        //-------------------------------------------------------------------------------------------------------------
        public void IndexAllObjects()
        {
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Start();
            }
            int tRow = 0;
            int tMethod = 0;
            DatasIndexed = false;
            foreach (Type tType in mTypeList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tMethod += tHelper.IndexInMemoryMethodList.Count;
                tRow += tHelper.Datas.Count;
                tHelper.IndexInMemoryAllObjects();
            }
            DatasIndexed = true;
            EditorRefresh();
            NWDLauncher.RowInformations = "Rows indexed : " + tRow + " rows. Used " + IndexationCounterOp + " operation(s) and " + tMethod + " method(s).";
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Log(NWDLauncher.RowInformations);
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReloadAllObjects(NWDBasisBundle sBundle)
        {
            ReloadAllObjectsEditor(sBundle);
            ReloadAllObjectsAccount(sBundle);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================