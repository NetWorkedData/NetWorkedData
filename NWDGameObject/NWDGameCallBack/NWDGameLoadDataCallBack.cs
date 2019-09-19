//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:45:44
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

//using BasicToolBox;

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
        /// Installs the observer in the NWENotification manager
        /// </summary>
        void InstallObserver()
        {
            // get NWENotificationManager shared instance from the NWDGameDataManager Singleton
            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();

            // Launch engine
            if (TrackEngineLaunch == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ENGINE_LAUNCH, delegate (NWENotification sNotification)
                {
                    NotificationEngineLaunch(sNotification);
                });
            }

            // load datas
            if (TrackDatasEditorStartLoading == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_START_LOADING, delegate (NWENotification sNotification)
                {
                    NotificationDatasEditorStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
                });
            }
            if (TrackDatasEditorPartialLoaded == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_PARTIAL_LOADED, delegate (NWENotification sNotification)
                {
                    float tPurcent = NWDDataManager.SharedInstance().PurcentEditorLoaded();
                    NotificationDatasEditorPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
                });
            }
            if (TrackDatasEditorLoaded == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_LOADED, delegate (NWENotification sNotification)
                {
                    NotificationDatasEditorLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
                });
            }



            if (TrackDatasAccountStartLoading == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_START_LOADING, delegate (NWENotification sNotification)
                {
                    NotificationDatasAccountStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
                });
            }
            if (TrackDatasAccountPartialLoaded == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_PARTIAL_LOADED, delegate (NWENotification sNotification)
                {
                    float tPurcent = NWDDataManager.SharedInstance().PurcentAccountLoaded();
                    NotificationDatasAccountPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
                });
            }
            if (TrackDatasAccountLoaded == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_LOADED, delegate (NWENotification sNotification)
                {
                    NotificationDatasAccountLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
                });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void RemoveObserver()
        {
            // get NWENotificationManager shared instance from the NWDGameDataManager Singleton
            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();

            // remove this from NWENotificationManager
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
        public virtual void NotificationEngineLaunch(NWENotification sNotification)
        {
            if (EngineLaunchEvent != null)
            {
                EngineLaunchEvent.Invoke();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasEditorStartLoading(NWENotification sNotification, bool sPreloadDatas)
        {
            if (DatasEditorStartLoadingEvent != null)
            {
                DatasEditorStartLoadingEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasEditorPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            if (DatasEditorPartialLoadedEvent != null)
            {
                DatasEditorPartialLoadedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasEditorLoaded(NWENotification sNotification, bool sPreloadDatas)
        {
            if (DatasEditorLoadedEvent != null)
            {
                DatasEditorLoadedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasAccountStartLoading(NWENotification sNotification, bool sPreloadDatas)
        {
            if (DatasAccountStartLoadingEvent != null)
            {
                DatasAccountStartLoadingEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasAccountPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            if (DatasAccountPartialLoadedEvent != null)
            {
                DatasAccountPartialLoadedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasAccountLoaded(NWENotification sNotification, bool sPreloadDatas)
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





