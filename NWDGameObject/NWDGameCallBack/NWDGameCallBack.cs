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
    public partial class NWDGameCallBack : MonoBehaviour
    {
        [Header("Track NetWorkedData Engine")]
        public bool TrackEngineLaunch = true;
        public NWDCallBackEvent EngineLaunchEvent;
        [Header("Track NetWorkedData Data load")]
        public bool TrackDatasStartLoading = true;
        public NWDCallBackEvent DatasStartLoadingEvent;
        public bool TrackDatasPartialLoaded = true;
        public NWDCallBackEvent DatasPartialLoadedEvent;
        public bool TrackDatasLoaded = true;
        public NWDCallBackEvent DatasLoadedEvent;
        [Header("Track NetWorkedData Language change")]
        public bool TrackLanguageChanged = true;
        public NWDCallBackEvent LanguageChangedEvent;
        [Header("Track NetWorkedData Account")]
        public bool TrackAccountBanned = true;
        public NWDCallBackEvent AccountBannedEvent;
        public bool TrackAccountChanged = true;
        public NWDCallBackEvent AccountChangedEvent;
        public bool TrackAccountSessionExpired = true;
        public NWDCallBackEvent AccountSessionExpiredEvent;
        [Header("Track NetWorkedData Local change")]
        public bool TrackDataLocalDelete = true;
        public NWDCallBackEvent DataLocalDeleteEvent;
        public bool TrackDataLocalInsert = true;
        public NWDCallBackEvent DataLocalInsertEvent;
        public bool TrackDataLocalUpdate = true;
        public NWDCallBackEvent DataLocalUpdateEvent;
        [Header("Track NetWorkedData Web change")]
        public bool TrackDatasWebUpdate = true;
        public NWDCallBackEvent DatasWebUpdateEvent;
        public bool TrackWebOperationError = true;
        public NWDCallBackEvent WebOperationErrorEvent;
        public bool TrackWebOperationUploadStart = true;
        public NWDCallBackEvent WebOperationUploadStartEvent;
        public bool TrackWebOperationUploadInProgress = true;
        public NWDCallBackEvent WebOperationUploadInProgressEvent;
        public bool TrackWebOperationDownloadError = true;
        public NWDCallBackEvent WebOperationDownloadErrorEvent;
        public bool TrackWebOperationDownloadFailed = true;
        public NWDCallBackEvent WebOperationDownloadFailedEvent;
        public bool TrackWebOperationDownloadInProgress = true;
        public NWDCallBackEvent WebOperationDownloadInProgressEvent;
        public bool TrackWebOperationDownloadIsDone = true;
        public NWDCallBackEvent WebOperationDownloadIsDoneEvent;
        public bool TrackWebOperationDownloadSuccessed = true;
        public NWDCallBackEvent WebOperationDownloadSuccessedEvent;
        [Header("Track NetWorkedData Error")]
        public bool TrackError = true;
        public NWDCallBackEvent ErrorEvent;
        [Header("Track NetWorkedData Network change")]
        public bool TrackNetworkOffLine = true;
        public NWDCallBackEvent NetworkOffLineEvent;
        public bool TrackNetworkOnLine = true;
        public NWDCallBackEvent NetworkOnLineEvent;
        public bool TrackNetworkUnknow = true;
        public NWDCallBackEvent NetworkUnknowEvent;
        public bool TrackNetworkCheck = true;
        public NWDCallBackEvent NetworkCheckEvent;
        [Header("Track NetWorkedData Generic")]
        public bool TrackGeneric = true;
        public NWDCallBackEvent GenericEvent;


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
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_ENGINE_LAUNCH, delegate (BTBNotification sNotification)
                {
                    NotificationEngineLaunch(sNotification);
                });
            }

            // load datas
            if (TrackDatasStartLoading == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATAS_START_LOADING, delegate (BTBNotification sNotification)
                {
                    NotificationDatasStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
                });
            }
            if (TrackDatasPartialLoaded == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATAS_PARTIAL_LOADED, delegate (BTBNotification sNotification)
                {
                    float tPurcent = (float)NWDTypeLauncher.ClassesDataLoaded / (float)NWDTypeLauncher.ClassesExpected;
                    NotificationDatasPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
                });
            }
            if (TrackDatasLoaded == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATAS_LOADED, delegate (BTBNotification sNotification)
                {
                    NotificationDatasLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
                });
            }
            // change language

            if (TrackLanguageChanged == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_LANGUAGE_CHANGED, delegate (BTBNotification sNotification)
                {
                    NotificationLanguageChanged(sNotification);
                });
            }

            // change data

            if (TrackDataLocalInsert == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATA_LOCAL_INSERT, delegate (BTBNotification sNotification)
                {
                    NotificationDataLocalInsert(sNotification);
                });
            }
            if (TrackDataLocalUpdate == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATA_LOCAL_UPDATE, delegate (BTBNotification sNotification)
                {
                    NotificationDataLocalUpdate(sNotification);
                });
            }

            if (TrackDataLocalDelete == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATA_LOCAL_DELETE, delegate (BTBNotification sNotification)
                {
                    NotificationDataLocalDelete(sNotification);
                });
            }

            // change from web data

            if (TrackDatasWebUpdate == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_DATAS_WEB_UPDATE, delegate (BTBNotification sNotification)
                {
                    NotificationDatasWebUpdate(sNotification);
                });
            }
            // error

            if (TrackError == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_ERROR, delegate (BTBNotification sNotification)
                {
                    NotificationError(sNotification);
                });
            }
            // player/user change

            if (TrackAccountChanged == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_ACCOUNT_CHANGE, delegate (BTBNotification sNotification)
                {
                    NotificationAccountChanged(sNotification);
                });
            }
            if (TrackAccountSessionExpired == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_ACCOUNT_SESSION_EXPIRED, delegate (BTBNotification sNotification)
                {
                    NotificationAccountSessionExpired(sNotification);
                });
            }

            if (TrackAccountBanned == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_ACCOUNT_BANNED, delegate (BTBNotification sNotification)
                {
                    NotificationAccountBanned(sNotification);
                });
            }

            // Network statut
            if (TrackNetworkOffLine == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_NETWORK_OFFLINE, delegate (BTBNotification sNotification)
                {
                    NotificationNetworkOffLine(sNotification);
                });
            }
            if (TrackNetworkOnLine == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_NETWORK_ONLINE, delegate (BTBNotification sNotification)
                {
                    NotificationNetworkOnLine(sNotification);
                });
            }
            if (TrackNetworkUnknow == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_NETWORK_UNKNOW, delegate (BTBNotification sNotification)
                {
                    NotificationNetworkUnknow(sNotification);
                });
            }
            if (TrackNetworkCheck == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_NETWORK_CHECK, delegate (BTBNotification sNotification)
                {
                    NotificationNetworkCheck(sNotification);
                });
            }
            if (TrackWebOperationError == true)
            {

                // Operation Web
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_OPERATION_WEB_ERROR, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationError(sNotification);
                });
            }
            if (TrackWebOperationUploadStart == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_START, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationUploadStart(sNotification);
                });
            }
            if (TrackWebOperationUploadInProgress == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_IN_PROGRESS, delegate (BTBNotification sNotification)
                {
                    NWDOperationWebUnity tSender = sNotification.Sender as NWDOperationWebUnity;
                    NotificationWebOperationUploadInProgress(sNotification, tSender.Request.uploadProgress);
                });
            }
            if (TrackWebOperationDownloadInProgress == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IN_PROGRESS, delegate (BTBNotification sNotification)
                {
                    NWDOperationWebUnity tSender = sNotification.Sender as NWDOperationWebUnity;
                    NotificationWebOperationDownloadInProgress(sNotification, tSender.Request.downloadProgress);
                });
            }
            if (TrackWebOperationDownloadIsDone == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationDownloadIsDone(sNotification);
                });
            }
            if (TrackWebOperationDownloadFailed == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_FAILED, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationDownloadFailed(sNotification);
                });
            }
            if (TrackWebOperationDownloadError == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_ERROR, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationDownloadError(sNotification);
                });
            }
            if (TrackWebOperationDownloadSuccessed == true)
            {
                tNotificationManager.AddObserver(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationDownloadSuccessed(sNotification);
                });
            }

            // generic
            if (TrackGeneric == true)
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
            if(EngineLaunchEvent!=null)
            {
                EngineLaunchEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasStartLoading(BTBNotification sNotification, bool sPreloadDatas)
        {
            if ( DatasStartLoadingEvent!= null)
            {
                DatasStartLoadingEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasPartialLoaded(BTBNotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            if ( DatasPartialLoadedEvent!= null)
            {
                DatasPartialLoadedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasLoaded(BTBNotification sNotification, bool sPreloadDatas)
        {
            if ( DatasLoadedEvent!= null)
            {
                DatasLoadedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationLanguageChanged(BTBNotification sNotification)
        {
            if ( LanguageChangedEvent!= null)
            {
                LanguageChangedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDataLocalUpdate(BTBNotification sNotification)
        {
            if ( DataLocalUpdateEvent!= null)
            {
                DataLocalUpdateEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDataLocalInsert(BTBNotification sNotification)
        {
            if ( DataLocalInsertEvent!= null)
            {
                DataLocalInsertEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDataLocalDelete(BTBNotification sNotification)
        {
            if ( DataLocalDeleteEvent!= null)
            {
                DataLocalDeleteEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationDatasWebUpdate(BTBNotification sNotification)
        {
            if ( DatasWebUpdateEvent!= null)
            {
                DatasWebUpdateEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationError(BTBNotification sNotification)
        {
            if ( ErrorEvent!= null)
            {
                ErrorEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationAccountChanged(BTBNotification sNotification)
        {
            if ( AccountChangedEvent!= null)
            {
                AccountChangedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationAccountSessionExpired(BTBNotification sNotification)
        {
            if ( AccountSessionExpiredEvent!= null)
            {
                AccountSessionExpiredEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationAccountBanned(BTBNotification sNotification)
        {
            if ( AccountBannedEvent!= null)
            {
                AccountBannedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationNetworkOffLine(BTBNotification sNotification)
        {
            if ( NetworkOffLineEvent!= null)
            {
                NetworkOffLineEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationNetworkOnLine(BTBNotification sNotification)
        {
            if ( NetworkOnLineEvent!= null)
            {
                NetworkOnLineEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationNetworkUnknow(BTBNotification sNotification)
        {
            if ( NetworkUnknowEvent!= null)
            {
                NetworkUnknowEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationNetworkCheck(BTBNotification sNotification)
        {
            if (NetworkCheckEvent != null)
            {
                NetworkCheckEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationError(BTBNotification sNotification)
        {
            if ( WebOperationErrorEvent!= null)
            {
                WebOperationErrorEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationUploadStart(BTBNotification sNotification)
        {
            if (WebOperationUploadStartEvent != null)
            {
                WebOperationUploadStartEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationUploadInProgress(BTBNotification sNotification, float sPurcent)
        {
            if ( WebOperationUploadInProgressEvent!= null)
            {
                WebOperationUploadInProgressEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadInProgress(BTBNotification sNotification, float sPurcent)
        {
            if ( WebOperationDownloadInProgressEvent!= null)
            {
                WebOperationDownloadInProgressEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadIsDone(BTBNotification sNotification)
        {
            if ( WebOperationDownloadIsDoneEvent!= null)
            {
                WebOperationDownloadIsDoneEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadFailed(BTBNotification sNotification)
        {
            if ( WebOperationDownloadFailedEvent!= null)
            {
                WebOperationDownloadFailedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadError(BTBNotification sNotification)
        {
            if ( WebOperationDownloadErrorEvent!= null)
            {
                WebOperationDownloadErrorEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadSuccessed(BTBNotification sNotification)
        {
            if ( WebOperationDownloadSuccessedEvent!= null)
            {
                WebOperationDownloadSuccessedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationGeneric(BTBNotification sNotification)
        {
            if ( GenericEvent!=null)
            {
                GenericEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================