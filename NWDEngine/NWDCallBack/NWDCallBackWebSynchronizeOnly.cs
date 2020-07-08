//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#endif
//=====================================================================================================================

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using UnityEngine.Networking;

//=====================================================================================================================
namespace NetWorkedData
{

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCallBackWebSynchronizeOnly : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Installs the observer in the NWENotification manager
        /// </summary>
        void InstallObserver()
        {
            // get NWENotificationManager shared instance from the NWDGameDataManager Singleton
            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();
            // change from web data
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATAS_WEB_UPDATE, delegate (NWENotification sNotification)
            {
                NotificationDatasWebUpdate(sNotification);
            });

            // error
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ERROR, delegate (NWENotification sNotification)
            {
                NotificationError(sNotification);
            });

            // player/user change
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_CHANGE, delegate (NWENotification sNotification)
            {
                NotificationAccountChanged(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_SESSION_EXPIRED, delegate (NWENotification sNotification)
            {
                NotificationAccountSessionExpired(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_BANNED, delegate (NWENotification sNotification)
            {
                NotificationAccountBanned(sNotification);
            });

            // Network statut
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORK_OFFLINE, delegate (NWENotification sNotification)
            {
                NotificationNetworkOffLine(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORK_ONLINE, delegate (NWENotification sNotification)
            {
                NotificationNetworkOnLine(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORK_UNKNOW, delegate (NWENotification sNotification)
            {
                NotificationNetworkUnknow(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORK_CHECK, delegate (NWENotification sNotification)
            {
                NotificationNetworkCheck(sNotification);
            });
            // Operation Web
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_ERROR, delegate (NWENotification sNotification)
            {
                NotificationWebOperationError(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_START, delegate (NWENotification sNotification)
            {
                NotificationWebOperationUploadStart(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_IN_PROGRESS, delegate (NWENotification sNotification)
            {
                NWDOperationWebUnity tSender = sNotification.Sender as NWDOperationWebUnity;
                NotificationWebOperationUploadInProgress(sNotification,tSender.Request.uploadProgress);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IN_PROGRESS, delegate (NWENotification sNotification)
            {
                NWDOperationWebUnity tSender = sNotification.Sender as NWDOperationWebUnity;
                NotificationWebOperationDownloadInProgress(sNotification, tSender.Request.downloadProgress);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE, delegate (NWENotification sNotification)
            {
                NotificationWebOperationDownloadIsDone(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_FAILED, delegate (NWENotification sNotification)
            {
                NotificationWebOperationDownloadFailed(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_ERROR, delegate (NWENotification sNotification)
            {
                NotificationWebOperationDownloadError(sNotification);
            });
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED, delegate (NWENotification sNotification)
            {
                NotificationWebOperationDownloadSuccessed(sNotification);
            });

            // generic
            tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NOTIFICATION_KEY, delegate (NWENotification sNotification)
            {
                NotificationGeneric(sNotification);
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        void RemoveObserver()
        {
            // get NWENotificationManager shared instance from the NWDGameDataManager Singleton
            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();

			// remove this from NWENotificationManager
			tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_WEB_UPDATE);
			tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ERROR);
			tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORK_OFFLINE);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORK_ONLINE);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORK_UNKNOW);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORK_CHECK);
			tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NOTIFICATION_KEY);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_ERROR);
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
        public virtual void NotificationEngineLaunch(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasStartLoading(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasLoaded(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationLanguageChanged(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDataLocalUpdate(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDataLocalInsert(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDataLocalDelete(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasWebUpdate(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationError(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationAccountChanged(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationAccountSessionExpired(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationAccountBanned(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationNetworkOffLine(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationNetworkOnLine(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationNetworkUnknow(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationNetworkCheck(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationError(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationUploadStart(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationUploadInProgress(NWENotification sNotification, float sPurcent)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadInProgress(NWENotification sNotification, float sPurcent)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadIsDone(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadFailed(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadError(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadSuccessed(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationGeneric(NWENotification sNotification)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================