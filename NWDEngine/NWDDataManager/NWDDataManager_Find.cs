﻿//=====================================================================================================================
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
            NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_START_LOADING);
            NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_EDITOR_START_LOADING);
            ClassEditorDataLoaded = 0;
            while (ClassEditorDataLoaded < ClassEditorExpected)
            {
                ReloadAllObjectsByClassEditor(ClassEditorDataLoaded);
                ClassEditorDataLoaded++;
                ClassDataLoaded = ClassEditorDataLoaded + ClassAccountDataLoaded;
                NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_EDITOR_PARTIAL_LOADED);
                NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_PARTIAL_LOADED);
                yield return null;
            }
            DataEditorLoaded = true;
            PlayerLanguageLoad();
            LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
            EditorRefresh();
            NWDLauncher.SetState(NWDStatut.DataEditorLoaded);
            NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_EDITOR_LOADED);
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
            NWEBenchmark.Start();
            if (DataEditorConnected == true)
            {
                //NWEBenchmark.Start("LoadData");
                DataEditorLoaded = false;
                NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_START_LOADING);
                NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_EDITOR_START_LOADING);
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
                    if (NWDAppConfiguration.SharedInstance().PreloadDatas == false)
                    {
                        NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_EDITOR_PARTIAL_LOADED);
                        NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_PARTIAL_LOADED);
                    }
                    //tBenchmark += NWEBenchmark.Finish("LoadData " + tType.Name);
                }
                NWDDataManager.SharedInstance().DataEditorLoaded = true;
                //NWEBenchmark.Finish("LoadData", true, "with total = " + tBenchmark.ToString("F5") + "s in total");
                PlayerLanguageLoad();
                LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
                EditorRefresh();
                NWDLauncher.SetState(NWDStatut.DataEditorLoaded);
                NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_EDITOR_LOADED);
            }
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public IEnumerator AsyncReloadAllObjectsAccount()
        {
            DataAccountLoaded = false;
            while (DataAccountConnected == false)
            {
                yield return null;
            }
            NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_ACCOUNT_START_LOADING);
            ClassAccountExpected = mTypeAccountDependantList.Count();
            ClassAccountDataLoaded = 0;
            while (ClassAccountDataLoaded < ClassAccountExpected)
            {
                ReloadAllObjectsByClassAccount(ClassAccountDataLoaded);
                ClassAccountDataLoaded++;
                ClassDataLoaded = ClassEditorDataLoaded + ClassAccountDataLoaded;
                NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_ACCOUNT_PARTIAL_LOADED);
                NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_PARTIAL_LOADED);
                yield return null;
            }
            DataAccountLoaded = true;
            Debug.Log("NWDDataManager AsyncReloadAllObjects() post notification Account is loaded and All Datas is loaded");
            PlayerLanguageLoad();
            LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
            EditorRefresh();
            NWDLauncher.SetState(NWDStatut.DataAccountLoaded);
            NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_ACCOUNT_LOADED);

            //NWDLauncher.SetState(NWDStatut.DataLoaded);
            NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_LOADED);
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
            NWEBenchmark.Start();
            if (DataAccountConnected == true)
            {
                DataAccountLoaded = false;
                NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_ACCOUNT_START_LOADING);
                ClassAccountExpected = mTypeAccountDependantList.Count();
                ClassAccountDataLoaded = 0;
                foreach (Type tType in mTypeAccountDependantList)
                {
                    NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                    tHelper.LoadFromDatabase();
                    //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_LoadFromDatabase);
                    ClassAccountDataLoaded++;
                    ClassDataLoaded = ClassEditorDataLoaded + ClassAccountDataLoaded;
                    if (NWDAppConfiguration.SharedInstance().PreloadDatas == false)
                    {
                        NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_ACCOUNT_PARTIAL_LOADED);
                        NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_PARTIAL_LOADED);
                    }
                }
                DataAccountLoaded = true;
                NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_ACCOUNT_LOADED);
                NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATA_LOADED);
                PlayerLanguageLoad();
                LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
                EditorRefresh();
            }
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public IEnumerator AsyncIndexAllObjects()
        {
            DatasIndexed = false;
            while (DataAccountConnected == false)
            {
                yield return null;
            }
            NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_INDEXATION_START);
            ClassIndexation = 0;
            while (ClassIndexation < ClassExpected)
            {
                IndexAllObjectsByClass(ClassIndexation);
                ClassIndexation++;
                NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_INDEXATION_STEP);
                yield return null;
            }
            DatasIndexed = true;
            NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_INDEXATION_FINISH);
            //PlayerLanguageLoad();
            //LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
            EditorRefresh();
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IndexAllObjectsByClass(int sCounter)
        {
            bool rReturn = false;
            if (sCounter >= 0 && sCounter < mTypeList.Count)
            {
                Type tType = mTypeList[sCounter];
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.IndexAll();
                //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_IndexAll);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int IndexationCounterOp = 0;
        //-------------------------------------------------------------------------------------------------------------
        public void IndexAllObjects()
        {
            NWEBenchmark.Start();
            int tRow = 0;
            int tMethod = 0;
            DatasIndexed = false;
            NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_INDEXATION_START);
            foreach (Type tType in mTypeList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tMethod += tHelper.IndexInsertMethodList.Count;
                tRow += tHelper.Datas.Count;
                tHelper.IndexAll();
                //NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_IndexAll);
                NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_INDEXATION_STEP);
            }
            DatasIndexed = true;
            NWENotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_INDEXATION_FINISH);
            NWEBenchmark.Finish(true, "row indexed : " + tRow + " rows. Use " + IndexationCounterOp + " operation(s). Use " + tMethod + " method(s).");
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