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
        public IEnumerator AsyncReloadAllObjectsEditor()
        {
            DataEditorLoaded = false;
            while (DataEditorConnected == false)
            {
                yield return null;
            }
            ClassEditorDataLoaded = 0;
            while (ClassEditorDataLoaded < ClassEditorExpected)
            {
                ReloadAllObjectsByClassEditor(ClassEditorDataLoaded);
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
            LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
            EditorRefresh();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ReloadAllObjectsByClassEditor(int sCounter)
        {
            bool rReturn = false;
            if (sCounter >= 0 && sCounter < mTypeNotAccountDependantList.Count)
            {
                Type tType = mTypeNotAccountDependantList[sCounter];

                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.LoadFromDatabase();

                //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_LoadFromDatabase);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReloadAllObjectsEditor()
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
                    tHelper.LoadFromDatabase();
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_LoadFromDatabase);
                    ClassEditorDataLoaded++;
                    ClassDataLoaded = ClassEditorDataLoaded + ClassAccountDataLoaded;
                    //tBenchmark += NWEBenchmark.Finish("LoadData " + tType.Name);
                }
                NWDDataManager.SharedInstance().DataEditorLoaded = true;
                //NWEBenchmark.Finish("LoadData", true, "with total = " + tBenchmark.ToString("F5") + "s in total");
                PlayerLanguageLoad();
                LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
                EditorRefresh();
                //NWDLauncher.SetState(NWDStatut.DataEditorLoaded);
            }
            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public IEnumerator AsyncReloadAllObjectsAccount()
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
                ReloadAllObjectsByClassAccount(ClassAccountDataLoaded);
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
        public bool ReloadAllObjectsByClassAccount(int sCounter)
        {
            bool rReturn = false;
            if (sCounter >= 0 && sCounter < mTypeAccountDependantList.Count)
            {

                Type tType = mTypeAccountDependantList[sCounter];
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.LoadFromDatabase();
                //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_LoadFromDatabase);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReloadAllObjectsAccount()
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
                    tHelper.LoadFromDatabase();
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
                NWDBasisHelper tHelper = IndexAllObjectsByClass(ClassIndexation);
                ClassIndexation++;
                NWDLauncher.NotifyStep();
                if (NWDLauncher.YieldValid())
                {
                    yield return null;
                }
                if (tHelper != null)
                {
                    tMethod += tHelper.IndexInsertMethodList.Count;
                    tRow += tHelper.Datas.Count;
                }
            }
            DatasIndexed = true;
            //PlayerLanguageLoad();
            //LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
            EditorRefresh();

            NWEBenchmark.Log("row indexed : " + tRow + " rows. Use " + IndexationCounterOp + " operation(s). Use " + tMethod + " method(s).");

            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Finish();

            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisHelper IndexAllObjectsByClass(int sCounter)
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
                tHelper.IndexAll();
                if (NWDLauncher.ActiveBenchmark)
                {
                    NWEBenchmark.Finish(true, " " + tHelper.ClassNamePHP);
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
                tMethod += tHelper.IndexInsertMethodList.Count;
                tRow += tHelper.Datas.Count;
                tHelper.IndexAll();
            }
            DatasIndexed = true;

            EditorRefresh();

            NWEBenchmark.Log("row indexed : " + tRow + " rows. Use " + IndexationCounterOp + " operation(s). Use " + tMethod + " method(s).");

            if (NWDLauncher.ActiveBenchmark)
            {
                NWEBenchmark.Finish();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReloadAllObjects()
        {
            ReloadAllObjectsEditor();
            ReloadAllObjectsAccount();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================