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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

//using BasicToolBox;
using UnityEngine.Networking;

//=====================================================================================================================
namespace NetWorkedData
{

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCallBack : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        [Header("Track NetWorkedData Language change")]
        public bool TrackLanguageChanged = true;
        //public NWDCallBackEvent LanguageChangedEvent;
        [Header("Track NetWorkedData Database ")]
        public bool TrackDataLoaded = true;
        [Header("Track NetWorkedData Account")]
        public bool TrackAccountBanned = true;
        //public NWDCallBackEvent AccountBannedEvent;
        public bool TrackAccountChanged = true;
        //public NWDCallBackEvent AccountChangedEvent;
        public bool TrackAccountSessionExpired = true;
        //public NWDCallBackEvent AccountSessionExpiredEvent;
#if NWD_CRUD_NOTIFICATION
        [Header("Track NetWorkedData Local change")]
        public bool TrackDataLocalDelete = true;
        //public NWDCallBackEvent DataLocalDeleteEvent;
        public bool TrackDataLocalInsert = true;
        //public NWDCallBackEvent DataLocalInsertEvent;
        public bool TrackDataLocalUpdate = true;
#endif
        //public NWDCallBackEvent DataLocalUpdateEvent;
        [Header("Track NetWorkedData Web change")]
#if NWD_CRUD_NOTIFICATION
        public bool TrackDatasWebUpdate = true;
#endif
        //public NWDCallBackEvent DatasWebUpdateEvent;
        public bool TrackWebOperationError = true;
        //public NWDCallBackEvent WebOperationErrorEvent;
        public bool TrackWebOperationUploadStart = true;
        //public NWDCallBackEvent WebOperationUploadStartEvent;
        public bool TrackWebOperationUploadInProgress = true;
        //public NWDCallBackEvent WebOperationUploadInProgressEvent;
        public bool TrackWebOperationDownloadError = true;
        //public NWDCallBackEvent WebOperationDownloadErrorEvent;
        public bool TrackWebOperationDownloadFailed = true;
        //public NWDCallBackEvent WebOperationDownloadFailedEvent;
        public bool TrackWebOperationDownloadInProgress = true;
        //public NWDCallBackEvent WebOperationDownloadInProgressEvent;
        public bool TrackWebOperationDownloadIsDone = true;
        //public NWDCallBackEvent WebOperationDownloadIsDoneEvent;
        public bool TrackWebOperationDownloadSuccessed = true;
        //public NWDCallBackEvent WebOperationDownloadSuccessedEvent;
        [Header("Track NetWorkedData Error")]
        public bool TrackError = true;
        //public NWDCallBackEvent ErrorEvent;
        [Header("Track NetWorkedData Network change")]
        public bool TrackNetworkOffLine = true;
        //public NWDCallBackEvent NetworkOffLineEvent;
        public bool TrackNetworkOnLine = true;
        //public NWDCallBackEvent NetworkOnLineEvent;
        public bool TrackNetworkUnknow = true;
        //public NWDCallBackEvent NetworkUnknowEvent;
        public bool TrackNetworkCheck = true;
        //public NWDCallBackEvent NetworkCheckEvent;
        [Header("Track NetWorkedData Generic")]
        public bool TrackGeneric = true;
        //public NWDCallBackEvent GenericEvent;
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Installs the observer in the NWENotification manager
        /// </summary>
        void InstallObserver()
        {
            // get NWENotificationManager shared instance from the NWDGameDataManager Singleton
            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();
            // change language
            if (TrackLanguageChanged == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_LANGUAGE_CHANGED, delegate (NWENotification sNotification)
                {
                    NotificationLanguageChanged(sNotification);
                });
            }

            if (TrackDataLoaded == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORKEDDATA_READY, delegate (NWENotification sNotification)
                {
                    NotificationDatasLoaded(sNotification);
                });
            }
            // change data
#if NWD_CRUD_NOTIFICATION
            if (TrackDataLocalInsert == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_LOCAL_INSERT, delegate (NWENotification sNotification)
                {
                    NotificationDataLocalInsert(sNotification);
                });
            }
            if (TrackDataLocalUpdate == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_LOCAL_UPDATE, delegate (NWENotification sNotification)
                {
                    NotificationDataLocalUpdate(sNotification);
                });
            }

            if (TrackDataLocalDelete == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_LOCAL_DELETE, delegate (NWENotification sNotification)
                {
                    NotificationDataLocalDelete(sNotification);
                });
            }

            // change from web data

            if (TrackDatasWebUpdate == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATAS_WEB_UPDATE, delegate (NWENotification sNotification)
                {
                    NotificationDatasWebUpdate(sNotification);
                });
            }
#endif
            // error

            if (TrackError == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ERROR, delegate (NWENotification sNotification)
                {
                    NotificationError(sNotification);
                });
            }
            // player/user change

            if (TrackAccountChanged == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_CHANGE, delegate (NWENotification sNotification)
                {
                    NotificationAccountChanged(sNotification);
                });
            }
            if (TrackAccountSessionExpired == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_SESSION_EXPIRED, delegate (NWENotification sNotification)
                {
                    NotificationAccountSessionExpired(sNotification);
                });
            }

            if (TrackAccountBanned == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_BANNED, delegate (NWENotification sNotification)
                {
                    NotificationAccountBanned(sNotification);
                });
            }

            // Network statut
            if (TrackNetworkOffLine == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORK_OFFLINE, delegate (NWENotification sNotification)
                {
                    NotificationNetworkOffLine(sNotification);
                });
            }
            if (TrackNetworkOnLine == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORK_ONLINE, delegate (NWENotification sNotification)
                {
                    NotificationNetworkOnLine(sNotification);
                });
            }
            if (TrackNetworkUnknow == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORK_UNKNOW, delegate (NWENotification sNotification)
                {
                    NotificationNetworkUnknow(sNotification);
                });
            }
            if (TrackNetworkCheck == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORK_CHECK, delegate (NWENotification sNotification)
                {
                    NotificationNetworkCheck(sNotification);
                });
            }
            if (TrackWebOperationError == true)
            {

                // Operation Web
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_ERROR, delegate (NWENotification sNotification)
                {
                    NotificationWebOperationError(sNotification);
                });
            }
            if (TrackWebOperationUploadStart == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_START, delegate (NWENotification sNotification)
                {
                    NotificationWebOperationUploadStart(sNotification);
                });
            }
            if (TrackWebOperationUploadInProgress == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_IN_PROGRESS, delegate (NWENotification sNotification)
                {
                    NWDOperationWebUnity tSender = sNotification.Sender as NWDOperationWebUnity;
                    NotificationWebOperationUploadInProgress(sNotification, tSender.Request.uploadProgress);
                });
            }
            if (TrackWebOperationDownloadInProgress == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IN_PROGRESS, delegate (NWENotification sNotification)
                {
                    NWDOperationWebUnity tSender = sNotification.Sender as NWDOperationWebUnity;
                    NotificationWebOperationDownloadInProgress(sNotification, tSender.Request.downloadProgress);
                });
            }
            if (TrackWebOperationDownloadIsDone == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE, delegate (NWENotification sNotification)
                {
                    NotificationWebOperationDownloadIsDone(sNotification);
                });
            }
            if (TrackWebOperationDownloadFailed == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_FAILED, delegate (NWENotification sNotification)
                {
                    NotificationWebOperationDownloadFailed(sNotification);
                });
            }
            if (TrackWebOperationDownloadError == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_ERROR, delegate (NWENotification sNotification)
                {
                    NotificationWebOperationDownloadError(sNotification);
                });
            }
            if (TrackWebOperationDownloadSuccessed == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED, delegate (NWENotification sNotification)
                {
                    NotificationWebOperationDownloadSuccessed(sNotification);
                });
            }

            // generic
            if (TrackGeneric == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NOTIFICATION_KEY, delegate (NWENotification sNotification)
                {
                    NotificationGeneric(sNotification);
                });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void RemoveObserver()
        {
            // get NWENotificationManager shared instance from the NWDGameDataManager Singleton
            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();

            // remove this from NWENotificationManager
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORKEDDATA_READY);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_BANNED);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_CHANGE);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_SESSION_EXPIRED);
#if NWD_CRUD_NOTIFICATION
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_WEB_UPDATE);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_LOCAL_DELETE);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_LOCAL_INSERT);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_LOCAL_UPDATE);
#endif
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ERROR);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_LANGUAGE_CHANGED);
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
        //=============================================================================================================
        // VIRTUAL METHOD        
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasLoaded(NWENotification sNotification)
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
