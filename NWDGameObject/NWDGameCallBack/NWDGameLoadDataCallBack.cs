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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWD game call back.
    /// Use in game object to connect the other gameobject to action in the NetWorkedData package 
    /// Each scene can be connect independently
    /// </summary>
    public partial class NWDGameLoadDataCallBack : MonoBehaviour
    {
        [Header("Track NetWorkedData Engine")]
        public bool TrackEngineLaunch = true;
        public UnityEvent EngineLaunchEvent;
        [Header("Track NetWorkedData Data load")]
        public bool TrackDatasStartLoading = true;
        public UnityEvent DatasStartLoadingEvent;
        public bool TrackDatasPartialLoaded = true;
        public NWDCallBackEventFloat DatasPartialLoadedEvent;
        public bool TrackDatasLoaded = true;
        public UnityEvent DatasLoadedEvent;

        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Installs the observer in the BTBNotification manager
        /// </summary>
        void InstallObserver()
        {
            // get BTBNotificationManager shared instance from the NWDGameDataManager Singleton
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();

            // Launch engine
            if (TrackEngineLaunch == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ENGINE_LAUNCH, delegate (BTBNotification sNotification)
                {
                    NotificationEngineLaunch(sNotification);
                });
            }

            // load datas
            if (TrackDatasStartLoading == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATAS_START_LOADING, delegate (BTBNotification sNotification)
                {
                    NotificationDatasStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
                });
            }
            if (TrackDatasPartialLoaded == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATAS_PARTIAL_LOADED, delegate (BTBNotification sNotification)
                {
                    float tPurcent = (float)NWDTypeLauncher.ClassesDataLoaded / (float)NWDTypeLauncher.ClassesExpected;
                    NotificationDatasPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
                });
            }
            if (TrackDatasLoaded == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATAS_LOADED, delegate (BTBNotification sNotification)
                {
                    NotificationDatasLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
                });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void RemoveObserver()
        {
            // get BTBNotificationManager shared instance from the NWDGameDataManager Singleton
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();

            // remove this from BTBNotificationManager
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ENGINE_LAUNCH);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_LOADED);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_PARTIAL_LOADED);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_START_LOADING);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TrackReset()
        {
            RemoveObserver();
            InstallObserver();
        }
        //-------------------------------------------------------------------------------------------------------------
        protected virtual void OnEnable()
        {
            InstallObserver();
        }
        //-------------------------------------------------------------------------------------------------------------
        protected virtual void OnDisable()
        {
            RemoveObserver();
        }

        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationEngineLaunch(BTBNotification sNotification)
        {
            if (EngineLaunchEvent != null)
            {
                EngineLaunchEvent.Invoke();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasStartLoading(BTBNotification sNotification, bool sPreloadDatas)
        {
            if (DatasStartLoadingEvent != null)
            {
                DatasStartLoadingEvent.Invoke();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasPartialLoaded(BTBNotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            if (DatasPartialLoadedEvent != null)
            {
                DatasPartialLoadedEvent.Invoke(sPurcent);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasLoaded(BTBNotification sNotification, bool sPreloadDatas)
        {
            if (DatasLoadedEvent != null)
            {
                DatasLoadedEvent.Invoke();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================





