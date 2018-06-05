//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using BasicToolBox;
using UnityEngine.Networking;

//=====================================================================================================================
namespace NetWorkedData
{
    //-------------------------------------------------------------------------------------------------------------
    //	TODO : finish implementation  : add notifications key and callback method : Must be test
    //-------------------------------------------------------------------------------------------------------------
    /// <summary>
    /// NWD game call back.
    /// Use in game object to connect the other gameobject to action in the NetWorkedData package 
    /// Each scene can be connect independently
    /// </summary>
    public partial class NWDCallBackDataLoadOnly : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Installs the observer in the BTBNotification manager
        /// </summary>
        void InstallObserver()
        {
            // get BTBNotificationManager shared instance from the NWDGameDataManager Singleton
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();
            // load datas
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATAS_START_LOADING, delegate (BTBNotification sNotification)
            {
                NotificationDatasStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATAS_PARTIAL_LOADED, delegate (BTBNotification sNotification)
            {
                float tPurcent = (float)NWDTypeLauncher.ClassesDataLoaded / (float)NWDTypeLauncher.ClassesExpected;
                NotificationDatasPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATAS_LOADED, delegate (BTBNotification sNotification)
            {
                NotificationDatasLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        void RemoveObserver()
        {
            // get BTBNotificationManager shared instance from the NWDGameDataManager Singleton
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();

            // remove this from BTBNotificationManager
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_START_LOADING);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_PARTIAL_LOADED);
			tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_LOADED);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected void OnEnable()
        {
            InstallObserver();
        }
        //-------------------------------------------------------------------------------------------------------------
        protected void OnDisable()
        {
            RemoveObserver();
        }
		//=============================================================================================================
        // VIRTUAL METHOD 
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasStartLoading(BTBNotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasPartialLoaded(BTBNotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasLoaded(BTBNotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================