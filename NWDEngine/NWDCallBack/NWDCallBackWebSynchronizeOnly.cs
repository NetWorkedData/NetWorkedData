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
    public partial class NWDCallBackWebSynchronizeOnly : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Installs the observer in the BTBNotification manager
        /// </summary>
        void InstallObserver()
        {
            // get BTBNotificationManager shared instance from the NWDGameDataManager Singleton
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();
            // change from web data
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATAS_WEB_UPDATE, delegate (BTBNotification sNotification)
            {
                NotificationDatasWebUpdate(sNotification);
            });

            // error
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ERROR, delegate (BTBNotification sNotification)
            {
                NotificationError(sNotification);
            });

            // player/user change
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_CHANGE, delegate (BTBNotification sNotification)
            {
                NotificationAccountChanged(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_SESSION_EXPIRED, delegate (BTBNotification sNotification)
            {
                NotificationAccountSessionExpired(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_BANNED, delegate (BTBNotification sNotification)
            {
                NotificationAccountBanned(sNotification);
            });

            // Network statut
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORK_OFFLINE, delegate (BTBNotification sNotification)
            {
                NotificationNetworkOffLine(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORK_ONLINE, delegate (BTBNotification sNotification)
            {
                NotificationNetworkOnLine(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORK_UNKNOW, delegate (BTBNotification sNotification)
            {
                NotificationNetworkUnknow(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORK_CHECK, delegate (BTBNotification sNotification)
            {
                NotificationNetworkCheck(sNotification);
            });
            // Operation Web
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_OPERATION_WEB_ERROR, delegate (BTBNotification sNotification)
            {
                NotificationWebOperationError(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_START, delegate (BTBNotification sNotification)
            {
                NotificationWebOperationUploadStart(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_IN_PROGRESS, delegate (BTBNotification sNotification)
            {
                NWDOperationWebUnity tSender = sNotification.Sender as NWDOperationWebUnity;
                NotificationWebOperationUploadInProgress(sNotification,tSender.Request.uploadProgress);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IN_PROGRESS, delegate (BTBNotification sNotification)
            {
                NWDOperationWebUnity tSender = sNotification.Sender as NWDOperationWebUnity;
                NotificationWebOperationDownloadInProgress(sNotification, tSender.Request.downloadProgress);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE, delegate (BTBNotification sNotification)
            {
                NotificationWebOperationDownloadIsDone(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_FAILED, delegate (BTBNotification sNotification)
            {
                NotificationWebOperationDownloadFailed(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_ERROR, delegate (BTBNotification sNotification)
            {
                NotificationWebOperationDownloadError(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED, delegate (BTBNotification sNotification)
            {
                NotificationWebOperationDownloadSuccessed(sNotification);
            });

            // generic
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NOTIFICATION_KEY, delegate (BTBNotification sNotification)
            {
                NotificationGeneric(sNotification);
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        void RemoveObserver()
        {
            // get BTBNotificationManager shared instance from the NWDGameDataManager Singleton
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();

			// remove this from BTBNotificationManager
			tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_WEB_UPDATE);
			tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ERROR);
			tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORK_OFFLINE);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORK_ONLINE);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORK_UNKNOW);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORK_CHECK);
			tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NOTIFICATION_KEY);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_OPERATION_WEB_ERROR);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_START);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_IN_PROGRESS);
			tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_ERROR);
			tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_FAILED);
			tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IN_PROGRESS);
			tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED);
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
        public virtual void NotificationNetworkCheck(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationError(BTBNotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationUploadStart(BTBNotification sNotification)
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