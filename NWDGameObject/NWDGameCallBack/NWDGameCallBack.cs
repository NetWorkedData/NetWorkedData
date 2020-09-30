////=====================================================================================================================
////
////  ideMobi 2019©
////
////  Date		2019-4-12 18:45:43
////  Author		Kortex (Jean-François CONTART) 
////  Email		jfcontart@idemobi.com
////  Project 	NetWorkedData for Unity3D
////
////  All rights reserved by ideMobi
////
////=====================================================================================================================



//using System;
//using System.Collections.Generic;

//using UnityEngine;
//using UnityEngine.Events;

////using BasicToolBox;

////=====================================================================================================================
//namespace NetWorkedData
//{
//    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//    /// <summary>
//    /// NWD game call back.
//    /// Use in game object to connect the other gameobject to action in the NetWorkedData package 
//    /// Each scene can be connect independently
//    /// </summary>
//    public partial class NWDGameCallBack : MonoBehaviour
//    {
//        [Header("Track NetWorkedData Engine")]
//        public bool TrackEngineLaunch = true;
//        public NWDCallBackEvent EngineLaunchEvent;
//        [Header("Track NetWorkedData Data load")]
//        public bool TrackDatasEditorStartLoading = true;
//        public NWDCallBackEvent DatasEditorStartLoadingEvent;
//        public bool TrackDatasEditorPartialLoaded = true;
//        public NWDCallBackEvent DatasEditorPartialLoadedEvent;
//        public bool TrackDatasEditorLoaded = true;
//        public NWDCallBackEvent DatasEditorLoadedEvent;
//        public bool TrackDatasAccountStartLoading = true;
//        public NWDCallBackEvent DatasAccountStartLoadingEvent;
//        public bool TrackDatasAccountPartialLoaded = true;
//        public NWDCallBackEvent DatasAccountPartialLoadedEvent;
//        public bool TrackDatasAccountLoaded = true;
//        public NWDCallBackEvent DatasAccountLoadedEvent;
//        [Header("Track NetWorkedData Language change")]
//        public bool TrackLanguageChanged = true;
//        public NWDCallBackEvent LanguageChangedEvent;
//        [Header("Track NetWorkedData Account")]
//        public bool TrackAccountBanned = true;
//        public NWDCallBackEvent AccountBannedEvent;
//        public bool TrackAccountChanged = true;
//        public NWDCallBackEvent AccountChangedEvent;
//        public bool TrackAccountSessionExpired = true;
//        public NWDCallBackEvent AccountSessionExpiredEvent;
//        [Header("Track NetWorkedData Local change")]
//        public bool TrackDataLocalDelete = true;
//        public NWDCallBackEvent DataLocalDeleteEvent;
//        public bool TrackDataLocalInsert = true;
//        public NWDCallBackEvent DataLocalInsertEvent;
//        public bool TrackDataLocalUpdate = true;
//        public NWDCallBackEvent DataLocalUpdateEvent;
//        [Header("Track NetWorkedData Web change")]
//        public bool TrackDatasWebUpdate = true;
//        public NWDCallBackEvent DatasWebUpdateEvent;
//        public bool TrackWebOperationError = true;
//        public NWDCallBackEvent WebOperationErrorEvent;
//        public bool TrackWebOperationUploadStart = true;
//        public NWDCallBackEvent WebOperationUploadStartEvent;
//        public bool TrackWebOperationUploadInProgress = true;
//        public NWDCallBackEvent WebOperationUploadInProgressEvent;
//        public bool TrackWebOperationDownloadError = true;
//        public NWDCallBackEvent WebOperationDownloadErrorEvent;
//        public bool TrackWebOperationDownloadFailed = true;
//        public NWDCallBackEvent WebOperationDownloadFailedEvent;
//        public bool TrackWebOperationDownloadInProgress = true;
//        public NWDCallBackEvent WebOperationDownloadInProgressEvent;
//        public bool TrackWebOperationDownloadIsDone = true;
//        public NWDCallBackEvent WebOperationDownloadIsDoneEvent;
//        public bool TrackWebOperationDownloadSuccessed = true;
//        public NWDCallBackEvent WebOperationDownloadSuccessedEvent;
//        [Header("Track NetWorkedData Error")]
//        public bool TrackError = true;
//        public NWDCallBackEvent ErrorEvent;
//        [Header("Track NetWorkedData Network change")]
//        public bool TrackNetworkOffLine = true;
//        public NWDCallBackEvent NetworkOffLineEvent;
//        public bool TrackNetworkOnLine = true;
//        public NWDCallBackEvent NetworkOnLineEvent;
//        public bool TrackNetworkUnknow = true;
//        public NWDCallBackEvent NetworkUnknowEvent;
//        public bool TrackNetworkCheck = true;
//        public NWDCallBackEvent NetworkCheckEvent;
//        [Header("Track NetWorkedData Generic")]
//        public bool TrackGeneric = true;
//        public NWDCallBackEvent GenericEvent;


//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Installs the observer in the NWENotification manager
//        /// </summary>
//        void InstallObserver()
//        {
//            // get NWENotificationManager shared instance from the NWDGameDataManager Singleton
//            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();

//            // Launch engine
//            if (TrackEngineLaunch == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ENGINE_LAUNCH, delegate (NWENotification sNotification)
//                {
//                    NotificationEngineLaunch(sNotification);
//                });
//            }

//            // load datas
//            if (TrackDatasEditorStartLoading == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_START_LOADING, delegate (NWENotification sNotification)
//                {
//                    NotificationDatasEditorStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
//                });
//            }
//            if (TrackDatasEditorPartialLoaded == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_PARTIAL_LOADED, delegate (NWENotification sNotification)
//                {
//                    float tPurcent = NWDDataManager.SharedInstance().PurcentEditorLoaded();
//                    NotificationDatasEditorPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
//                });
//            }
//            if (TrackDatasEditorLoaded == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_LOADED, delegate (NWENotification sNotification)
//                {
//                    NotificationDatasEditorLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
//                });
//            }



//            if (TrackDatasAccountStartLoading == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_START_LOADING, delegate (NWENotification sNotification)
//                {
//                    NotificationDatasAccountStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
//                });
//            }
//            if (TrackDatasAccountPartialLoaded == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_PARTIAL_LOADED, delegate (NWENotification sNotification)
//                {
//                    float tPurcent = NWDDataManager.SharedInstance().PurcentAccountLoaded();
//                    NotificationDatasAccountPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
//                });
//            }
//            if (TrackDatasAccountLoaded == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_LOADED, delegate (NWENotification sNotification)
//                {
//                    NotificationDatasAccountLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
//                });
//            }
//            // change language

//            if (TrackLanguageChanged == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_LANGUAGE_CHANGED, delegate (NWENotification sNotification)
//                {
//                    NotificationLanguageChanged(sNotification);
//                });
//            }

//            // change data

//            if (TrackDataLocalInsert == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_LOCAL_INSERT, delegate (NWENotification sNotification)
//                {
//                    NotificationDataLocalInsert(sNotification);
//                });
//            }
//            if (TrackDataLocalUpdate == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_LOCAL_UPDATE, delegate (NWENotification sNotification)
//                {
//                    NotificationDataLocalUpdate(sNotification);
//                });
//            }

//            if (TrackDataLocalDelete == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_LOCAL_DELETE, delegate (NWENotification sNotification)
//                {
//                    NotificationDataLocalDelete(sNotification);
//                });
//            }

//            // change from web data

//            if (TrackDatasWebUpdate == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_DATAS_WEB_UPDATE, delegate (NWENotification sNotification)
//                {
//                    NotificationDatasWebUpdate(sNotification);
//                });
//            }
//            // error

//            if (TrackError == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ERROR, delegate (NWENotification sNotification)
//                {
//                    NotificationError(sNotification);
//                });
//            }
//            // player/user change

//            if (TrackAccountChanged == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_CHANGE, delegate (NWENotification sNotification)
//                {
//                    NotificationAccountChanged(sNotification);
//                });
//            }
//            if (TrackAccountSessionExpired == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_SESSION_EXPIRED, delegate (NWENotification sNotification)
//                {
//                    NotificationAccountSessionExpired(sNotification);
//                });
//            }

//            if (TrackAccountBanned == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_BANNED, delegate (NWENotification sNotification)
//                {
//                    NotificationAccountBanned(sNotification);
//                });
//            }

//            // Network statut
//            if (TrackNetworkOffLine == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORK_OFFLINE, delegate (NWENotification sNotification)
//                {
//                    NotificationNetworkOffLine(sNotification);
//                });
//            }
//            if (TrackNetworkOnLine == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORK_ONLINE, delegate (NWENotification sNotification)
//                {
//                    NotificationNetworkOnLine(sNotification);
//                });
//            }
//            if (TrackNetworkUnknow == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORK_UNKNOW, delegate (NWENotification sNotification)
//                {
//                    NotificationNetworkUnknow(sNotification);
//                });
//            }
//            if (TrackNetworkCheck == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORK_CHECK, delegate (NWENotification sNotification)
//                {
//                    NotificationNetworkCheck(sNotification);
//                });
//            }
//            if (TrackWebOperationError == true)
//            {

//                // Operation Web
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_ERROR, delegate (NWENotification sNotification)
//                {
//                    NotificationWebOperationError(sNotification);
//                });
//            }
//            if (TrackWebOperationUploadStart == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_START, delegate (NWENotification sNotification)
//                {
//                    NotificationWebOperationUploadStart(sNotification);
//                });
//            }
//            if (TrackWebOperationUploadInProgress == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_IN_PROGRESS, delegate (NWENotification sNotification)
//                {
//                    NWDOperationWebUnity tSender = sNotification.Sender as NWDOperationWebUnity;
//                    NotificationWebOperationUploadInProgress(sNotification, tSender.Request.uploadProgress);
//                });
//            }
//            if (TrackWebOperationDownloadInProgress == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IN_PROGRESS, delegate (NWENotification sNotification)
//                {
//                    NWDOperationWebUnity tSender = sNotification.Sender as NWDOperationWebUnity;
//                    NotificationWebOperationDownloadInProgress(sNotification, tSender.Request.downloadProgress);
//                });
//            }
//            if (TrackWebOperationDownloadIsDone == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE, delegate (NWENotification sNotification)
//                {
//                    NotificationWebOperationDownloadIsDone(sNotification);
//                });
//            }
//            if (TrackWebOperationDownloadFailed == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_FAILED, delegate (NWENotification sNotification)
//                {
//                    NotificationWebOperationDownloadFailed(sNotification);
//                });
//            }
//            if (TrackWebOperationDownloadError == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_ERROR, delegate (NWENotification sNotification)
//                {
//                    NotificationWebOperationDownloadError(sNotification);
//                });
//            }
//            if (TrackWebOperationDownloadSuccessed == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED, delegate (NWENotification sNotification)
//                {
//                    NotificationWebOperationDownloadSuccessed(sNotification);
//                });
//            }

//            // generic
//            if (TrackGeneric == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_NOTIFICATION_KEY, delegate (NWENotification sNotification)
//                {
//                    NotificationGeneric(sNotification);
//                });
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        void RemoveObserver()
//        {
//            // get NWENotificationManager shared instance from the NWDGameDataManager Singleton
//            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();

//            // remove this from NWENotificationManager
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_BANNED);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_CHANGE);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ACCOUNT_SESSION_EXPIRED);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_LOADED);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_PARTIAL_LOADED);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_START_LOADING);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_LOADED);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_PARTIAL_LOADED);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_START_LOADING);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATAS_WEB_UPDATE);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_LOCAL_DELETE);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_LOCAL_INSERT);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_LOCAL_UPDATE);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ENGINE_LAUNCH);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ERROR);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_LANGUAGE_CHANGED);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORK_OFFLINE);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORK_ONLINE);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORK_UNKNOW);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORK_CHECK);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NOTIFICATION_KEY);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_ERROR);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_START);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_IN_PROGRESS);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_ERROR);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_FAILED);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IN_PROGRESS);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED);
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public void TrackReset()
//        {
//            RemoveObserver();
//            InstallObserver();
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        protected virtual void OnEnable()
//        {
//            InstallObserver();
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        protected virtual void OnDisable()
//        {
//            RemoveObserver();
//        }

//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationEngineLaunch(NWENotification sNotification)
//        {
//            if (EngineLaunchEvent != null)
//            {
//                EngineLaunchEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationDatasEditorStartLoading(NWENotification sNotification, bool sPreloadDatas)
//        {
//            if (DatasEditorStartLoadingEvent != null)
//            {
//                DatasEditorStartLoadingEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationDatasEditorPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
//        {
//            if (DatasEditorPartialLoadedEvent != null)
//            {
//                DatasEditorPartialLoadedEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationDatasEditorLoaded(NWENotification sNotification, bool sPreloadDatas)
//        {
//            if (DatasEditorLoadedEvent != null)
//            {
//                DatasEditorLoadedEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationDatasAccountStartLoading(NWENotification sNotification, bool sPreloadDatas)
//        {
//            if (DatasAccountStartLoadingEvent != null)
//            {
//                DatasAccountStartLoadingEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationDatasAccountPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
//        {
//            if (DatasAccountPartialLoadedEvent != null)
//            {
//                DatasAccountPartialLoadedEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationDatasAccountLoaded(NWENotification sNotification, bool sPreloadDatas)
//        {
//            if (DatasAccountLoadedEvent != null)
//            {
//                DatasAccountLoadedEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationLanguageChanged(NWENotification sNotification)
//        {
//            if (LanguageChangedEvent != null)
//            {
//                LanguageChangedEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationDataLocalUpdate(NWENotification sNotification)
//        {
//            if (DataLocalUpdateEvent != null)
//            {
//                DataLocalUpdateEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationDataLocalInsert(NWENotification sNotification)
//        {
//            if (DataLocalInsertEvent != null)
//            {
//                DataLocalInsertEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationDataLocalDelete(NWENotification sNotification)
//        {
//            if (DataLocalDeleteEvent != null)
//            {
//                DataLocalDeleteEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationDatasWebUpdate(NWENotification sNotification)
//        {
//            if (DatasWebUpdateEvent != null)
//            {
//                DatasWebUpdateEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationError(NWENotification sNotification)
//        {
//            if (ErrorEvent != null)
//            {
//                ErrorEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationAccountChanged(NWENotification sNotification)
//        {
//            if (AccountChangedEvent != null)
//            {
//                AccountChangedEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationAccountSessionExpired(NWENotification sNotification)
//        {
//            if (AccountSessionExpiredEvent != null)
//            {
//                AccountSessionExpiredEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationAccountBanned(NWENotification sNotification)
//        {
//            if (AccountBannedEvent != null)
//            {
//                AccountBannedEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationNetworkOffLine(NWENotification sNotification)
//        {
//            if (NetworkOffLineEvent != null)
//            {
//                NetworkOffLineEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationNetworkOnLine(NWENotification sNotification)
//        {
//            if (NetworkOnLineEvent != null)
//            {
//                NetworkOnLineEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationNetworkUnknow(NWENotification sNotification)
//        {
//            if (NetworkUnknowEvent != null)
//            {
//                NetworkUnknowEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationNetworkCheck(NWENotification sNotification)
//        {
//            if (NetworkCheckEvent != null)
//            {
//                NetworkCheckEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationWebOperationError(NWENotification sNotification)
//        {
//            if (WebOperationErrorEvent != null)
//            {
//                WebOperationErrorEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationWebOperationUploadStart(NWENotification sNotification)
//        {
//            if (WebOperationUploadStartEvent != null)
//            {
//                WebOperationUploadStartEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationWebOperationUploadInProgress(NWENotification sNotification, float sPurcent)
//        {
//            if (WebOperationUploadInProgressEvent != null)
//            {
//                WebOperationUploadInProgressEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationWebOperationDownloadInProgress(NWENotification sNotification, float sPurcent)
//        {
//            if (WebOperationDownloadInProgressEvent != null)
//            {
//                WebOperationDownloadInProgressEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationWebOperationDownloadIsDone(NWENotification sNotification)
//        {
//            if (WebOperationDownloadIsDoneEvent != null)
//            {
//                WebOperationDownloadIsDoneEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationWebOperationDownloadFailed(NWENotification sNotification)
//        {
//            if (WebOperationDownloadFailedEvent != null)
//            {
//                WebOperationDownloadFailedEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationWebOperationDownloadError(NWENotification sNotification)
//        {
//            if (WebOperationDownloadErrorEvent != null)
//            {
//                WebOperationDownloadErrorEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationWebOperationDownloadSuccessed(NWENotification sNotification)
//        {
//            if (WebOperationDownloadSuccessedEvent != null)
//            {
//                WebOperationDownloadSuccessedEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationGeneric(NWENotification sNotification)
//        {
//            if (GenericEvent != null)
//            {
//                GenericEvent.Invoke(sNotification);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//    }
//    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//}
////=====================================================================================================================





