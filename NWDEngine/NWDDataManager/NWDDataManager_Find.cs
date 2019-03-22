//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDDataManager
    {
        //------------------------------------------------------------------------------------------------------------- 
#if UNITY_EDITOR
        public void EditorRefresh()
        {
            foreach (Type tType in mTypeList)
            {
                RepaintWindowsInManager(tType);
            }
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_EDITOR_REFRESH);
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public IEnumerator AsyncReloadAllObjectsEditor()
        {
            //Debug.Log("NWDDataManager AsyncReloadAllObjects()");
            NWDTypeLauncher.DataEditorLoaded = false;
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_EDITOR_START_LOADING);
            NWDTypeLauncher.ClassesEditorExpected = mTypeNotAccountDependantList.Count();
            NWDTypeLauncher.ClassesEditorDataLoaded = 0;
            while (NWDTypeLauncher.ClassesEditorDataLoaded < NWDTypeLauncher.ClassesEditorExpected)
            {
                NWDDataManager.SharedInstance().ReloadAllObjectsByClassEditor(NWDTypeLauncher.ClassesEditorDataLoaded);
                NWDTypeLauncher.ClassesEditorDataLoaded++;
                BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_EDITOR_PARTIAL_LOADED);
                yield return null;
            }
            NWDTypeLauncher.DataEditorLoaded = true;
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_EDITOR_LOADED);
            //PlayerLanguageLoad();
            LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
#if UNITY_EDITOR
            EditorRefresh();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ReloadAllObjectsByClassEditor(int sCounter)
        {
            bool rReturn = false;
            if (sCounter >= 0 && sCounter < mTypeNotAccountDependantList.Count)
            {
                Type tType = mTypeNotAccountDependantList[sCounter];
                NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_LoadFromDatabase);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public IEnumerator AsyncReloadAllObjectsAccount()
        {
            //Debug.Log("NWDDataManager AsyncReloadAllObjects()");
            while (NWDTypeLauncher.DataEditorLoaded == false)
            {
                yield return null;
            }
            NWDTypeLauncher.DatabaseAccountLauncher();
            while (NWDTypeLauncher.DataAccountConnected == false)
            {
                yield return null;
            }
            NWDTypeLauncher.DataAccountLoaded = false;
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_ACCOUNT_START_LOADING);
            NWDTypeLauncher.ClassesAccountExpected = mTypeAccountDependantList.Count();
            NWDTypeLauncher.ClassesAccountDataLoaded = 0;
            while (NWDTypeLauncher.ClassesAccountDataLoaded < NWDTypeLauncher.ClassesAccountExpected)
            {
                NWDDataManager.SharedInstance().ReloadAllObjectsByClassAccount(NWDTypeLauncher.ClassesAccountDataLoaded);
                NWDTypeLauncher.ClassesAccountDataLoaded++;
                BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_ACCOUNT_PARTIAL_LOADED);
                yield return null;
            }
            NWDTypeLauncher.DataAccountLoaded = true;
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_ACCOUNT_LOADED);
            //PlayerLanguageLoad();
            LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
#if UNITY_EDITOR
            EditorRefresh();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ReloadAllObjectsByClassAccount(int sCounter)
        {
            bool rReturn = false;
            if (sCounter >= 0 && sCounter < mTypeAccountDependantList.Count)
            {
                Type tType = mTypeAccountDependantList[sCounter];
                NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_LoadFromDatabase);
            }
            return rReturn;
        }


        //-------------------------------------------------------------------------------------------------------------
        public void ReloadAllObjectsAccount()
        {
            BTBBenchmark.Start();
            if (NWDTypeLauncher.DataAccountConnected == true)
            {
                long tStartMemory = System.GC.GetTotalMemory(true);
                NWDTypeLauncher.DataAccountLoaded = false;
                BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_ACCOUNT_START_LOADING);
                NWDTypeLauncher.ClassesAccountExpected = mTypeAccountDependantList.Count();
                NWDTypeLauncher.ClassesAccountDataLoaded = 0;
                foreach (Type tType in mTypeAccountDependantList)
                {
                    NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_LoadFromDatabase);
                    NWDTypeLauncher.ClassesAccountDataLoaded++;
                    BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_ACCOUNT_PARTIAL_LOADED);
                }
                NWDTypeLauncher.DataAccountLoaded = true;
                BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_ACCOUNT_LOADED);
                //PlayerLanguageLoad();
                LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
#if UNITY_EDITOR
                EditorRefresh();
#endif
                long tFinishMemory = System.GC.GetTotalMemory(true);
                long tDataMemory = (tFinishMemory - tStartMemory) / 1024 / 1024;
                NWDDebug.Log("#### ReloadAllObjectsAccount engine memory = " + tDataMemory.ToString() + "Mo");
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReloadAllObjectsEditor()
        {
            BTBBenchmark.Start();
            if (NWDTypeLauncher.DataEditorConnected == true)
            {
                long tStartMemory = System.GC.GetTotalMemory(true);
                NWDTypeLauncher.DataEditorLoaded = false;
                BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_EDITOR_START_LOADING);
                NWDTypeLauncher.ClassesEditorExpected = mTypeNotAccountDependantList.Count();
                NWDTypeLauncher.ClassesEditorDataLoaded = 0;
                foreach (Type tType in mTypeNotAccountDependantList)
                {
                    NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_LoadFromDatabase);
                    NWDTypeLauncher.ClassesEditorDataLoaded++;
                    BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_EDITOR_PARTIAL_LOADED);
                }
                NWDTypeLauncher.DataEditorLoaded = true;
                BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_EDITOR_LOADED);
                //PlayerLanguageLoad();
                LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
#if UNITY_EDITOR
                EditorRefresh();
#endif
                long tFinishMemory = System.GC.GetTotalMemory(true);
                long tDataMemory = (tFinishMemory - tStartMemory) / 1024 / 1024;
                NWDDebug.Log("#### ReloadAllObjectsEditor engine memory = " + tDataMemory.ToString() + "Mo");
            }
            BTBBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReloadAllObjects()
        {
            ReloadAllObjectsEditor();
            ReloadAllObjectsAccount();
        }
        //        //-------------------------------------------------------------------------------------------------------------
        //        public void ReloadAllObjects ()
        //        {
        //            BTBBenchmark.Start();
        //            long tStartMemory = System.GC.GetTotalMemory(true);
        //            NWDTypeLauncher.DataLoaded = false;
        //            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_START_LOADING);
        //            NWDTypeLauncher.ClassesExpected = mTypeList.Count();
        //            NWDTypeLauncher.ClassesDataLoaded = 0;
        //			foreach( Type tType in mTypeList)
        //			{
        //                NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_LoadFromDatabase);
        //                NWDTypeLauncher.ClassesDataLoaded++;
        //                BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_PARTIAL_LOADED);
        //            }
        //            NWDTypeLauncher.DataLoaded = true;
        //            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_LOADED);
        //            //PlayerLanguageLoad();
        //            LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
        //#if UNITY_EDITOR
        //            EditorRefresh();
        //#endif
        //    BTBBenchmark.Finish();
        //    long tFinishMemory = System.GC.GetTotalMemory(true);
        //    long tDataMemory = (tFinishMemory - tStartMemory) / 1024 / 1024;
        //    NWDDebug.Log("#### ReloadAllObjects engine memory = " + tDataMemory.ToString() + "Mo");
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void RestaureObjectInEdition()
        {
#if UNITY_EDITOR
            foreach (Type tType in mTypeList)
            {
                NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_RestaureObjectInEdition);
            }
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_EDITOR_REFRESH);
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================