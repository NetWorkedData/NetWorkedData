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
        public IEnumerator AsyncReloadAllObjects()
        {
            //Debug.Log("NWDDataManager AsyncReloadAllObjects()");
            NWDTypeLauncher.DataLoaded = false;
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_START_LOADING);
            NWDTypeLauncher.ClassesExpected = mTypeList.Count();
            NWDTypeLauncher.ClassesDataLoaded = 0;
            while (NWDTypeLauncher.ClassesDataLoaded < NWDTypeLauncher.ClassesExpected)
            {
                NWDDataManager.SharedInstance().ReloadAllObjectsByClass(NWDTypeLauncher.ClassesDataLoaded);
                NWDTypeLauncher.ClassesDataLoaded++;
                BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_PARTIAL_LOADED);
                yield return null;
            }
            NWDTypeLauncher.DataLoaded = true;
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_LOADED);
            //PlayerLanguageLoad();
            LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
#if UNITY_EDITOR
            EditorRefresh();
            #endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ReloadAllObjectsByClass(int sCounter)
        {
            bool rReturn = false;
            if (sCounter>=0 && sCounter < mTypeList.Count)
            {
                Type tType = mTypeList[sCounter];
                NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_LoadFromDatabase);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
		public void ReloadAllObjects ()
        {
            BTBBenchmark.Start();
            long tStartMemory = System.GC.GetTotalMemory(true);
            NWDTypeLauncher.DataLoaded = false;
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_START_LOADING);
            NWDTypeLauncher.ClassesExpected = mTypeList.Count();
            NWDTypeLauncher.ClassesDataLoaded = 0;
			foreach( Type tType in mTypeList)
			{
                NWDAliasMethod.InvokeClassMethod(tType, NWDConstants.M_LoadFromDatabase);
                NWDTypeLauncher.ClassesDataLoaded++;
                BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_PARTIAL_LOADED);
            }
            NWDTypeLauncher.DataLoaded = true;
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_LOADED);
            //PlayerLanguageLoad();
            LoadPreferences(NWDAppEnvironment.SelectedEnvironment());
#if UNITY_EDITOR
            EditorRefresh();
#endif
            BTBBenchmark.Finish();
            long tFinishMemory = System.GC.GetTotalMemory(true);
            long tDataMemory = (tFinishMemory - tStartMemory) / 1024 / 1024;
            NWDDebug.Log("#### ReloadAllObjects engine memory = " + tDataMemory.ToString() + "Mo");
        }
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