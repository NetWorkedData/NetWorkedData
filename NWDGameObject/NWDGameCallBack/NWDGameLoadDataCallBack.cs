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
        public bool TrackDatasEditorStartLoading = true;
        public NWDCallBackEvent DatasEditorStartLoadingEvent;
        public bool TrackDatasEditorPartialLoaded = true;
        public NWDCallBackEvent DatasEditorPartialLoadedEvent;
        public bool TrackDatasEditorLoaded = true;
        public NWDCallBackEvent DatasEditorLoadedEvent;
        public bool TrackDatasAccountStartLoading = true;
        public NWDCallBackEvent DatasAccountStartLoadingEvent;
        public bool TrackDatasAccountPartialLoaded = true;
        public NWDCallBackEvent DatasAccountPartialLoadedEvent;
        public bool TrackDatasAccountLoaded = true;
        public NWDCallBackEvent DatasAccountLoadedEvent;

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
            if (TrackDatasEditorStartLoading == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_START_LOADING, delegate (BTBNotification sNotification)
                {
                    NotificationDatasEditorStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
                });
            }
            if (TrackDatasEditorPartialLoaded == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_PARTIAL_LOADED, delegate (BTBNotification sNotification)
                {
                    float tPurcent = NWDDataManager.SharedInstance().PurcentEditorLoaded();
                    NotificationDatasEditorPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
                });
            }
            if (TrackDatasEditorLoaded == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_LOADED, delegate (BTBNotification sNotification)
                {
                    NotificationDatasEditorLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
                });
            }



            if (TrackDatasAccountStartLoading == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_START_LOADING, delegate (BTBNotification sNotification)
                {
                    NotificationDatasAccountStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
                });
            }
            if (TrackDatasAccountPartialLoaded == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_PARTIAL_LOADED, delegate (BTBNotification sNotification)
                {
                    float tPurcent = NWDDataManager.SharedInstance().PurcentAccountLoaded();
                    NotificationDatasAccountPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
                });
            }
            if (TrackDatasAccountLoaded == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_LOADED, delegate (BTBNotification sNotification)
                {
                    NotificationDatasAccountLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
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
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_LOADED);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_PARTIAL_LOADED);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_START_LOADING);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_LOADED);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_PARTIAL_LOADED);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_START_LOADING);
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
        public virtual void NotificationDatasEditorStartLoading(BTBNotification sNotification, bool sPreloadDatas)
        {
            if (DatasEditorStartLoadingEvent != null)
            {
                DatasEditorStartLoadingEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasEditorPartialLoaded(BTBNotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            if (DatasEditorPartialLoadedEvent != null)
            {
                DatasEditorPartialLoadedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasEditorLoaded(BTBNotification sNotification, bool sPreloadDatas)
        {
            if (DatasEditorLoadedEvent != null)
            {
                DatasEditorLoadedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasAccountStartLoading(BTBNotification sNotification, bool sPreloadDatas)
        {
            if (DatasAccountStartLoadingEvent != null)
            {
                DatasAccountStartLoadingEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasAccountPartialLoaded(BTBNotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            if (DatasAccountPartialLoadedEvent != null)
            {
                DatasAccountPartialLoadedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasAccountLoaded(BTBNotification sNotification, bool sPreloadDatas)
        {
            if (DatasAccountLoadedEvent != null)
            {
                DatasAccountLoadedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================





