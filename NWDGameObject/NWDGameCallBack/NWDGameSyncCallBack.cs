////=====================================================================================================================
////
////  ideMobi 2019©
////
////  Date		2019-4-12 18:45:46
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
//    public partial class NWDGameSyncCallBack : MonoBehaviour
//    {
//        [Header("Process")]
//        public bool TrackWebOperationUploadStart = true;
//        public UnityEvent WebOperationUploadStartEvent;
//        public bool TrackWebOperationUploadInProgress = true;
//        public NWDCallBackEventFloat WebOperationUploadInProgressEvent;
//        public bool TrackWebOperationDownloadInProgress = true;
//        public NWDCallBackEventFloat WebOperationDownloadInProgressEvent;
//        public bool TrackWebOperationDownloadIsDone = true;
//        public UnityEvent WebOperationDownloadIsDoneEvent;
//        public bool TrackWebOperationDownloadSuccessed = true;
//        public UnityEvent WebOperationDownloadSuccessedEvent;
//        [Header("Error area")]
//        public bool TrackWebOperationDownloadFailed = true;
//        public UnityEvent WebOperationDownloadFailedEvent;
//        public bool TrackWebOperationDownloadError = true;
//        public UnityEvent WebOperationDownloadErrorEvent;
//        public bool TrackWebOperationError = true;
//        public UnityEvent WebOperationErrorEvent;


//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Installs the observer in the NWENotification manager
//        /// </summary>
//        void InstallObserver()
//        {
//            // get NWENotificationManager shared instance from the NWDGameDataManager Singleton
//            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();
//            // Operation Web
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
//            if (TrackWebOperationDownloadSuccessed == true)
//            {
//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED, delegate (NWENotification sNotification)
//                {
//                    NotificationWebOperationDownloadSuccessed(sNotification);
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
//            if (TrackWebOperationError == true)
//            {

//                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_ERROR, delegate (NWENotification sNotification)
//                {
//                    NotificationWebOperationError(sNotification);
//                });
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        void RemoveObserver()
//        {
//            // get NWENotificationManager shared instance from the NWDGameDataManager Singleton
//            NWENotificationManager tNotificationManager = NWENotificationManager.SharedInstance();

//            // remove this from NWENotificationManager
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_START);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_IN_PROGRESS);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IN_PROGRESS);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED);

//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_FAILED);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_ERROR);
//            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_ERROR);
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
//        public virtual void NotificationWebOperationError(NWENotification sNotification)
//        {
//            if (WebOperationErrorEvent != null)
//            {
//                WebOperationErrorEvent.Invoke();
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationWebOperationUploadStart(NWENotification sNotification)
//        {
//            if (WebOperationUploadStartEvent != null)
//            {
//                WebOperationUploadStartEvent.Invoke();
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationWebOperationUploadInProgress(NWENotification sNotification, float sPurcent)
//        {
//            //Debug.Log("NotificationWebOperation   UploadInProgress sPurcent = " + sPurcent.ToString());
//            if (WebOperationUploadInProgressEvent != null)
//            {
//                WebOperationUploadInProgressEvent.Invoke(sPurcent);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationWebOperationDownloadInProgress(NWENotification sNotification, float sPurcent)
//        {
//            //Debug.Log("NotificationWebOperation   DownloadInProgress sPurcent = " + sPurcent.ToString());
//            if (WebOperationDownloadInProgressEvent != null)
//            {
//                WebOperationDownloadInProgressEvent.Invoke(sPurcent);
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationWebOperationDownloadIsDone(NWENotification sNotification)
//        {
//            if (WebOperationDownloadIsDoneEvent != null)
//            {
//                WebOperationDownloadIsDoneEvent.Invoke();
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationWebOperationDownloadFailed(NWENotification sNotification)
//        {
//            if (WebOperationDownloadFailedEvent != null)
//            {
//                WebOperationDownloadFailedEvent.Invoke();
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationWebOperationDownloadError(NWENotification sNotification)
//        {
//            if (WebOperationDownloadErrorEvent != null)
//            {
//                WebOperationDownloadErrorEvent.Invoke();
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public virtual void NotificationWebOperationDownloadSuccessed(NWENotification sNotification)
//        {
//            if (WebOperationDownloadSuccessedEvent != null)
//            {
//                WebOperationDownloadSuccessedEvent.Invoke();
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//    }
//    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
//}
////=====================================================================================================================