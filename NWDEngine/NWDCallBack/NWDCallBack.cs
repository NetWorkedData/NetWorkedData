//=====================================================================================================================
//
// ideMobi copyright 2018 
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
    /// NWD call back.
    /// Use in game object to connect the other gameobject to notification from NetWorkedData Engine 
    /// Each scene can be connect independently
    /// </summary>
    public partial class NWDCallBack : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Installs the observer in the BTBNotification manager
        /// </summary>
        protected void InstallObserver()
        {
            // get BTBNotificationManager shared instance from the NWDGameDataManager Singleton
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();
            // Launch engine
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_ENGINE_LAUNCH, delegate (BTBNotification sNotification)
            {
                NotificationEngineLaunch(sNotification);
            });
            // load datas
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATAS_START_LOADING, delegate (BTBNotification sNotification)
            {
                NotificationDatasStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATAS_PARTIAL_LOADED, delegate (BTBNotification sNotification)
            {
                float tPurcent = (float)NWDTypeLauncher.ClassesDataLoaded / (float)NWDTypeLauncher.ClassesExpected;
                NotificationDatasPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
            });
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATAS_LOADED, delegate (BTBNotification sNotification)
            {
                NotificationDatasLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            // change language
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_LANGUAGE_CHANGED, delegate (BTBNotification sNotification)
            {
                NotificationLanguageChanged(sNotification);
            });
            // change data
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATA_LOCAL_INSERT, delegate (BTBNotification sNotification)
            {
                NotificationDataLocalInsert(sNotification);
            });
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATA_LOCAL_UPDATE, delegate (BTBNotification sNotification)
            {
                NotificationDataLocalUpdate(sNotification);
            });
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATA_LOCAL_DELETE, delegate (BTBNotification sNotification)
            {
                NotificationDataLocalDelete(sNotification);
            });
            // change from web data
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATAS_WEB_UPDATE, delegate (BTBNotification sNotification)
            {
                NotificationDatasWebUpdate(sNotification);
            });
            // error
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_ERROR, delegate (BTBNotification sNotification)
            {
                NotificationError(sNotification);
            });
            // player/user change
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_ACCOUNT_CHANGE, delegate (BTBNotification sNotification)
            {
                NotificationAccountChanged(sNotification);
            });
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_ACCOUNT_SESSION_EXPIRED, delegate (BTBNotification sNotification)
            {
                NotificationAccountSessionExpired(sNotification);
            });
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_ACCOUNT_BANNED, delegate (BTBNotification sNotification)
            {
                NotificationAccountBanned(sNotification);
            });
            // Network statut
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_NETWORK_OFFLINE, delegate (BTBNotification sNotification)
            {
                NotificationNetworkOffLine(sNotification);
            });
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_NETWORK_ONLINE, delegate (BTBNotification sNotification)
            {
                NotificationNetworkOnLine(sNotification);
            });
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_NETWORK_UNKNOW, delegate (BTBNotification sNotification)
            {
                NotificationNetworkUnknow(sNotification);
            });
            // Operation Web
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_OPERATION_WEB_ERROR, delegate (BTBNotification sNotification)
            {
                NotificationWebOperationError(sNotification);
            });
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_IN_PROGRESS, delegate (BTBNotification sNotification)
            {
                NWDOperationWebUnity tSender = sNotification.Sender as NWDOperationWebUnity;
                NotificationWebOperationUploadInProgress(sNotification, tSender.Request.uploadProgress);
            });
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IN_PROGRESS, delegate (BTBNotification sNotification)
            {
                NWDOperationWebUnity tSender = sNotification.Sender as NWDOperationWebUnity;
                NotificationWebOperationDownloadInProgress(sNotification, tSender.Request.downloadProgress);
            });
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE, delegate (BTBNotification sNotification)
            {
                NotificationWebOperationDownloadIsDone(sNotification);
            });
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_FAILED, delegate (BTBNotification sNotification)
            {
                NotificationWebOperationDownloadFailed(sNotification);
            });
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_ERROR, delegate (BTBNotification sNotification)
            {
                NotificationWebOperationDownloadError(sNotification);
            });
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED, delegate (BTBNotification sNotification)
            {
                NotificationWebOperationDownloadSuccessed(sNotification);
            });
            // generic
            tNotificationManager.AddObserver(this, NWDNotificationConstants.K_NOTIFICATION_KEY, delegate (BTBNotification sNotification)
            {
                NotificationGeneric(sNotification);
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        protected void RemoveObserver()
        {
            // get BTBNotificationManager shared instance from the NWDGameDataManager Singleton
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();
            // remove this from BTBNotificationManager
            tNotificationManager.RemoveObserverEveryWhere(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        //protected void Awake()
        //{
        //}
        //-------------------------------------------------------------------------------------------------------------
        protected void OnEnable()
        {
            // Install observer for ecah NWD Notification Key
            InstallObserver();
        }
        //-------------------------------------------------------------------------------------------------------------
        //protected void Start()
        //{

        //}
        //-------------------------------------------------------------------------------------------------------------
        protected void OnDisable()
        {
            // Remove observer()
            RemoveObserver();
        }
        //-------------------------------------------------------------------------------------------------------------
        //protected void OnDestroy()
        //{
        //}
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
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================