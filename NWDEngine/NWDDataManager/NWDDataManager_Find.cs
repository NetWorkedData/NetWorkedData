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
                RepaintWindowsInManager(tType); // Test
            }
            //NWDDataInspector.ShareInstance().Repaint();
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
                var tMethodInfo = tType.GetMethod("LoadTableEditor", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    tMethodInfo.Invoke(null, null);
                    rReturn = true;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
		public void ReloadAllObjects ()
        {
            //double tStartTimestamp = BTBDateHelper.ConvertToTimestamp(DateTime.Now);
            NWDTypeLauncher.DataLoaded = false;
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_START_LOADING);
            NWDTypeLauncher.ClassesExpected = mTypeList.Count();
            NWDTypeLauncher.ClassesDataLoaded = 0;
			foreach( Type tType in mTypeList)
			{
				var tMethodInfo = tType.GetMethod("LoadTableEditor", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) 
				{
					tMethodInfo.Invoke(null, null);
				}
                //double tTimeStamp = BTBDateHelper.ConvertToTimestamp(DateTime.Now);
                //Debug.Log("ReloadAllObjects () => tOperationsNeeded = " + NWDTypeLauncher.ClassesDataLoaded.ToString() + "/" + NWDTypeLauncher.ClassesExpected.ToString() + " (reload datas for '"+tType.Name+"') at " + tTimeStamp.ToString());
                NWDTypeLauncher.ClassesDataLoaded++;
                BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_PARTIAL_LOADED);
            }
            NWDTypeLauncher.DataLoaded = true;
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_DATAS_LOADED);
            //PlayerLanguageLoad();
#if UNITY_EDITOR
            EditorRefresh();
            #endif

            //double tFinishTimestamp = BTBDateHelper.ConvertToTimestamp(DateTime.Now);
            //double tDelta = tFinishTimestamp - tStartTimestamp;
            //Debug.Log("NWD => NetWorkeData load all datas in " + tDelta.ToString() + " seconds");
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RestaureObjectInEdition()
        {
            #if UNITY_EDITOR
            foreach (Type tType in mTypeList)
            {
                var tMethodInfo = tType.GetMethod("RestaureObjectInEdition", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                if (tMethodInfo != null)
                {
                    tMethodInfo.Invoke(null, null);
                }
            }
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_EDITOR_REFRESH);
            #endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================