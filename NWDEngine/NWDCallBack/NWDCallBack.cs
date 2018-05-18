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
    public partial class NWDCallBack : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        [Header("Track NetWorkedData Engine")]
        public bool Track_ENGINE_LAUNCH = true;
        [Header("Track NetWorkedData Data load")]
        public bool Track_DATAS_LOADED = true;
        public bool Track_DATAS_PARTIAL_LOADED = true;
        public bool Track_DATAS_START_LOADING = true;
        [Header("Track NetWorkedData Language change")]
        public bool Track_LANGUAGE_CHANGED = true;
        [Header("Track NetWorkedData Account")]
        public bool Track_ACCOUNT_BANNED = true;
        public bool Track_ACCOUNT_CHANGE = true;
        public bool Track_ACCOUNT_SESSION_EXPIRED = true;
        [Header("Track NetWorkedData Local change")]
        public bool Track_DATA_LOCAL_DELETE = true;
        public bool Track_DATA_LOCAL_INSERT = true;
        public bool Track_DATA_LOCAL_UPDATE = true;
        [Header("Track NetWorkedData Web change")]
        public bool Track_DATAS_WEB_UPDATE = true;
        public bool Track_OPERATION_WEB_ERROR = true;
        public bool Track_WEB_OPERATION_DOWNLOAD_ERROR = true;
        public bool Track_WEB_OPERATION_DOWNLOAD_FAILED = true;
        public bool Track_WEB_OPERATION_DOWNLOAD_IN_PROGRESS = true;
        public bool Track_WEB_OPERATION_DOWNLOAD_IS_DONE = true;
        public bool Track_WEB_OPERATION_DOWNLOAD_SUCCESSED = true;
        public bool Track_WEB_OPERATION_UPLOAD_IN_PROGRESS = true;
        [Header("Track NetWorkedData Error")]
        public bool Track_ERROR = true;
        [Header("Track NetWorkedData Network change")]
        public bool Track_NETWORK_OFFLINE = true;
        public bool Track_NETWORK_ONLINE = true;
        public bool Track_NETWORK_UNKNOW = true;
        [Header("Track NetWorkedData Generic")]
        public bool Track_NOTIFICATION_KEY = true;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Installs the observer in the BTBNotification manager
        /// </summary>
        void InstallObserver()
        {
            // get BTBNotificationManager shared instance from the NWDGameDataManager Singleton
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();

            // Launch engine
            if (Track_ENGINE_LAUNCH == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_ENGINE_LAUNCH, delegate (BTBNotification sNotification)
                {
                    NotificationEngineLaunch(sNotification);
                });
            }

            // load datas
            if (Track_DATAS_START_LOADING == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATAS_START_LOADING, delegate (BTBNotification sNotification)
                {
                    NotificationDatasStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
                });
            }
            if (Track_DATAS_PARTIAL_LOADED == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATAS_PARTIAL_LOADED, delegate (BTBNotification sNotification)
                {
                    float tPurcent = (float)NWDTypeLauncher.ClassesDataLoaded / (float)NWDTypeLauncher.ClassesExpected;
                    NotificationDatasPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
                });
            }
            if (Track_DATAS_LOADED == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATAS_LOADED, delegate (BTBNotification sNotification)
                {
                    NotificationDatasLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
                });
            }
            // change language

            if (Track_LANGUAGE_CHANGED == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_LANGUAGE_CHANGED, delegate (BTBNotification sNotification)
                {
                    NotificationLanguageChanged(sNotification);
                });
            }

            // change data

            if (Track_DATA_LOCAL_INSERT == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATA_LOCAL_INSERT, delegate (BTBNotification sNotification)
                {
                    NotificationDataLocalInsert(sNotification);
                });
            }
            if (Track_DATA_LOCAL_UPDATE == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATA_LOCAL_UPDATE, delegate (BTBNotification sNotification)
                {
                    NotificationDataLocalUpdate(sNotification);
                });
            }

            if (Track_DATA_LOCAL_DELETE == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATA_LOCAL_DELETE, delegate (BTBNotification sNotification)
                {
                    NotificationDataLocalDelete(sNotification);
                });
            }

            // change from web data

            if (Track_DATAS_WEB_UPDATE == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATAS_WEB_UPDATE, delegate (BTBNotification sNotification)
                {
                    NotificationDatasWebUpdate(sNotification);
                });
            }
            // error

            if (Track_ERROR == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_ERROR, delegate (BTBNotification sNotification)
                {
                    NotificationError(sNotification);
                });
            }
            // player/user change

            if (Track_ACCOUNT_CHANGE == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_ACCOUNT_CHANGE, delegate (BTBNotification sNotification)
                {
                    NotificationAccountChanged(sNotification);
                });
            }
            if (Track_ACCOUNT_SESSION_EXPIRED == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_ACCOUNT_SESSION_EXPIRED, delegate (BTBNotification sNotification)
                {
                    NotificationAccountSessionExpired(sNotification);
                });
            }

            if (Track_ACCOUNT_BANNED == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_ACCOUNT_BANNED, delegate (BTBNotification sNotification)
                {
                    NotificationAccountBanned(sNotification);
                });
            }

            // Network statut
            if (Track_NETWORK_OFFLINE == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_NETWORK_OFFLINE, delegate (BTBNotification sNotification)
                {
                    NotificationNetworkOffLine(sNotification);
                });
            }
            if (Track_NETWORK_ONLINE == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_NETWORK_ONLINE, delegate (BTBNotification sNotification)
                {
                    NotificationNetworkOnLine(sNotification);
                });
            }
            if (Track_NETWORK_UNKNOW == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_NETWORK_UNKNOW, delegate (BTBNotification sNotification)
                {
                    NotificationNetworkUnknow(sNotification);
                });
            }
            if (Track_OPERATION_WEB_ERROR == true)
            {

                // Operation Web
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_OPERATION_WEB_ERROR, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationError(sNotification);
                });
            }
            if (Track_WEB_OPERATION_UPLOAD_IN_PROGRESS == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_IN_PROGRESS, delegate (BTBNotification sNotification)
                {
                    NWDOperationWebUnity tSender = sNotification.Sender as NWDOperationWebUnity;
                    NotificationWebOperationUploadInProgress(sNotification, tSender.Request.uploadProgress);
                });
            }
            if (Track_WEB_OPERATION_DOWNLOAD_IN_PROGRESS == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IN_PROGRESS, delegate (BTBNotification sNotification)
                {
                    NWDOperationWebUnity tSender = sNotification.Sender as NWDOperationWebUnity;
                    NotificationWebOperationDownloadInProgress(sNotification, tSender.Request.downloadProgress);
                });
            }
            if (Track_WEB_OPERATION_DOWNLOAD_IS_DONE == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationDownloadIsDone(sNotification);
                });
            }
            if (Track_WEB_OPERATION_DOWNLOAD_FAILED == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_FAILED, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationDownloadFailed(sNotification);
                });
            }
            if (Track_WEB_OPERATION_DOWNLOAD_ERROR == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_ERROR, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationDownloadError(sNotification);
                });
            }
            if (Track_WEB_OPERATION_DOWNLOAD_SUCCESSED == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationDownloadSuccessed(sNotification);
                });
            }

            // generic
            if (Track_NOTIFICATION_KEY == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_NOTIFICATION_KEY, delegate (BTBNotification sNotification)
                {
                    NotificationGeneric(sNotification);
                });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void RemoveObserver()
        {
            // get BTBNotificationManager shared instance from the NWDGameDataManager Singleton
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();

            // remove this from BTBNotificationManager
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_BANNED);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_CHANGE);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_SESSION_EXPIRED);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_LOADED);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_PARTIAL_LOADED);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_START_LOADING);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_WEB_UPDATE);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_LOCAL_DELETE);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_LOCAL_INSERT);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_LOCAL_UPDATE);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ENGINE_LAUNCH);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ERROR);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_LANGUAGE_CHANGED);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORK_OFFLINE);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORK_ONLINE);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORK_UNKNOW);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NOTIFICATION_KEY);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_OPERATION_WEB_ERROR);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_ERROR);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_FAILED);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IN_PROGRESS);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_IN_PROGRESS);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void TrackReset()
        {
            RemoveObserver();
            InstallObserver();
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
        public virtual void NotificationEngineLaunch(BTBNotification sNotification)
        {
            // create your method by override
        }
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
        public virtual void NotificationLanguageChanged(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDataLocalUpdate(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDataLocalInsert(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDataLocalDelete(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasWebUpdate(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationError(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationAccountChanged(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationAccountSessionExpired(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationAccountBanned(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationNetworkOffLine(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationNetworkOnLine(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationNetworkUnknow(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationError(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationUploadInProgress(BTBNotification sNotification, float sPurcent)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadInProgress(BTBNotification sNotification, float sPurcent)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadIsDone(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadFailed(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadError(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadSuccessed(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationGeneric(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================