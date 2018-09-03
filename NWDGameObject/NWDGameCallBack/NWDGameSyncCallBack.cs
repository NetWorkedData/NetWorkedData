
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
    public partial class NWDGameSyncCallBack : MonoBehaviour
    {
        [Header("Process")]
        public bool TrackWebOperationUploadStart = true;
        public UnityEvent WebOperationUploadStartEvent;
        public bool TrackWebOperationUploadInProgress = true;
        public NWDCallBackEventFloat WebOperationUploadInProgressEvent;
        public bool TrackWebOperationDownloadInProgress = true;
        public NWDCallBackEventFloat WebOperationDownloadInProgressEvent;
        public bool TrackWebOperationDownloadIsDone = true;
        public UnityEvent WebOperationDownloadIsDoneEvent;
        public bool TrackWebOperationDownloadSuccessed = true;
        public UnityEvent WebOperationDownloadSuccessedEvent;
        [Header("Error area")]
        public bool TrackWebOperationDownloadFailed = true;
        public UnityEvent WebOperationDownloadFailedEvent;
        public bool TrackWebOperationDownloadError = true;
        public UnityEvent WebOperationDownloadErrorEvent;
        public bool TrackWebOperationError = true;
        public UnityEvent WebOperationErrorEvent;


        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Installs the observer in the BTBNotification manager
        /// </summary>
        void InstallObserver()
        {
            // get BTBNotificationManager shared instance from the NWDGameDataManager Singleton
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();
            // Operation Web
            if (TrackWebOperationUploadStart == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_START, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationUploadStart(sNotification);
                });
            }
            if (TrackWebOperationUploadInProgress == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_IN_PROGRESS, delegate (BTBNotification sNotification)
                {
                    NWDOperationWebUnity tSender = sNotification.Sender as NWDOperationWebUnity;
                    NotificationWebOperationUploadInProgress(sNotification, tSender.Request.uploadProgress);
                });
            }
            if (TrackWebOperationDownloadInProgress == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IN_PROGRESS, delegate (BTBNotification sNotification)
                {
                    NWDOperationWebUnity tSender = sNotification.Sender as NWDOperationWebUnity;
                    NotificationWebOperationDownloadInProgress(sNotification, tSender.Request.downloadProgress);
                });
            }
            if (TrackWebOperationDownloadIsDone == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationDownloadIsDone(sNotification);
                });
            }
            if (TrackWebOperationDownloadSuccessed == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationDownloadSuccessed(sNotification);
                });
            }



            if (TrackWebOperationDownloadFailed == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_FAILED, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationDownloadFailed(sNotification);
                });
            }
            if (TrackWebOperationDownloadError == true)
            {
                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_ERROR, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationDownloadError(sNotification);
                });
            }
            if (TrackWebOperationError == true)
            {

                tNotificationManager.AddObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_ERROR, delegate (BTBNotification sNotification)
                {
                    NotificationWebOperationError(sNotification);
                });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        void RemoveObserver()
        {
            // get BTBNotificationManager shared instance from the NWDGameDataManager Singleton
            BTBNotificationManager tNotificationManager = BTBNotificationManager.SharedInstance();

            // remove this from BTBNotificationManager
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_START);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_UPLOAD_IN_PROGRESS);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IN_PROGRESS);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_IS_DONE);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_SUCCESSED);

            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_FAILED);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_DOWNLOAD_ERROR);
            tNotificationManager.RemoveObserverForAll(this, NWDNotificationConstants.K_WEB_OPERATION_ERROR);
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
        public virtual void NotificationWebOperationError(BTBNotification sNotification)
        {
            if (WebOperationErrorEvent != null)
            {
                WebOperationErrorEvent.Invoke();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationUploadStart(BTBNotification sNotification)
        {
            if (WebOperationUploadStartEvent != null)
            {
                WebOperationUploadStartEvent.Invoke();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationUploadInProgress(BTBNotification sNotification, float sPurcent)
        {
            //Debug.Log("NotificationWebOperation   UploadInProgress sPurcent = " + sPurcent.ToString());
            if (WebOperationUploadInProgressEvent != null)
            {
                WebOperationUploadInProgressEvent.Invoke(sPurcent);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadInProgress(BTBNotification sNotification, float sPurcent)
        {
            //Debug.Log("NotificationWebOperation   DownloadInProgress sPurcent = " + sPurcent.ToString());
            if (WebOperationDownloadInProgressEvent != null)
            {
                WebOperationDownloadInProgressEvent.Invoke(sPurcent);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadIsDone(BTBNotification sNotification)
        {
            if (WebOperationDownloadIsDoneEvent != null)
            {
                WebOperationDownloadIsDoneEvent.Invoke();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadFailed(BTBNotification sNotification)
        {
            if (WebOperationDownloadFailedEvent != null)
            {
                WebOperationDownloadFailedEvent.Invoke();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadError(BTBNotification sNotification)
        {
            if (WebOperationDownloadErrorEvent != null)
            {
                WebOperationDownloadErrorEvent.Invoke();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NotificationWebOperationDownloadSuccessed(BTBNotification sNotification)
        {
            if (WebOperationDownloadSuccessedEvent != null)
            {
                WebOperationDownloadSuccessedEvent.Invoke();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================