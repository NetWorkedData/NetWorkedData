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
	//-------------------------------------------------------------------------------------------------------------
	//	TODO : finish implementation  : add notifications key and callback method : Must be test
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWD game call back.
	/// Use in game object to connect the other gameobject to action in the NetWorkedData package 
	/// Each scene can be connect independently
	/// </summary>
    public partial class NWDGameCallBack : NWDCallBack
	{
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent EngineLaunchEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationEngineLaunch(BTBNotification sNotification)
        {
            if(EngineLaunchEvent!=null)
            {
                EngineLaunchEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent DatasStartLoadingEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasStartLoading(BTBNotification sNotification, bool sPreloadDatas)
        {
            if ( DatasStartLoadingEvent!= null)
            {
                DatasStartLoadingEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent DatasPartialLoadedEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasPartialLoaded(BTBNotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            if ( DatasPartialLoadedEvent!= null)
            {
                DatasPartialLoadedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent DatasLoadedEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasLoaded(BTBNotification sNotification, bool sPreloadDatas)
        {
            if ( DatasLoadedEvent!= null)
            {
                DatasLoadedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent LanguageChangedEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationLanguageChanged(BTBNotification sNotification)
        {
            if ( LanguageChangedEvent!= null)
            {
                LanguageChangedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent DataLocalUpdateEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDataLocalUpdate(BTBNotification sNotification)
        {
            if ( DataLocalUpdateEvent!= null)
            {
                DataLocalUpdateEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent DataLocalInsertEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDataLocalInsert(BTBNotification sNotification)
        {
            if ( DataLocalInsertEvent!= null)
            {
                DataLocalInsertEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent DataLocalDeleteEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDataLocalDelete(BTBNotification sNotification)
        {
            if ( DataLocalDeleteEvent!= null)
            {
                DataLocalDeleteEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent DatasWebUpdateEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasWebUpdate(BTBNotification sNotification)
        {
            if ( DatasWebUpdateEvent!= null)
            {
                DatasWebUpdateEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent ErrorEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationError(BTBNotification sNotification)
        {
            if ( ErrorEvent!= null)
            {
                ErrorEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent AccountChangedEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationAccountChanged(BTBNotification sNotification)
        {
            if ( AccountChangedEvent!= null)
            {
                AccountChangedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent AccountSessionExpiredEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationAccountSessionExpired(BTBNotification sNotification)
        {
            if ( AccountSessionExpiredEvent!= null)
            {
                AccountSessionExpiredEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent AccountBannedEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationAccountBanned(BTBNotification sNotification)
        {
            if ( AccountBannedEvent!= null)
            {
                AccountBannedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent NetworkOffLineEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationNetworkOffLine(BTBNotification sNotification)
        {
            if ( NetworkOffLineEvent!= null)
            {
                NetworkOffLineEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent NetworkOnLineEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationNetworkOnLine(BTBNotification sNotification)
        {
            if ( NetworkOnLineEvent!= null)
            {
                NetworkOnLineEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent NetworkUnknowEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationNetworkUnknow(BTBNotification sNotification)
        {
            if ( NetworkUnknowEvent!= null)
            {
                NetworkUnknowEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent WebOperationErrorEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationError(BTBNotification sNotification)
        {
            if ( WebOperationErrorEvent!= null)
            {
                WebOperationErrorEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent WebOperationUploadInProgressEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationUploadInProgress(BTBNotification sNotification)
        {
            if ( WebOperationUploadInProgressEvent!= null)
            {
                WebOperationUploadInProgressEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent WebOperationDownloadInProgressEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadInProgress(BTBNotification sNotification)
        {
            if ( WebOperationDownloadInProgressEvent!= null)
            {
                WebOperationDownloadInProgressEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent WebOperationDownloadIsDoneEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadIsDone(BTBNotification sNotification)
        {
            if ( WebOperationDownloadIsDoneEvent!= null)
            {
                WebOperationDownloadIsDoneEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent WebOperationDownloadFailedEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadFailed(BTBNotification sNotification)
        {
            if ( WebOperationDownloadFailedEvent!= null)
            {
                WebOperationDownloadFailedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent WebOperationDownloadErrorEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadError(BTBNotification sNotification)
        {
            if ( WebOperationDownloadErrorEvent!= null)
            {
                WebOperationDownloadErrorEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent WebOperationDownloadSuccessedEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadSuccessed(BTBNotification sNotification)
        {
            if ( WebOperationDownloadSuccessedEvent!= null)
            {
                WebOperationDownloadSuccessedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent GenericEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationGeneric(BTBNotification sNotification)
        {
            if ( GenericEvent!=null)
            {
                GenericEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================