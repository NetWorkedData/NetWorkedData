//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
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
    public partial class NWDPanelDataManager : NWDCallBack
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDNetworkStatutRender NetworkStatutRender;
        public NWDGaugeRender DatasLoadGauge;
        public NWDGaugeRender WebSyncGauge;

        public GameObject WebSyncSpinner;
        public GameObject AlertPanel;

        //-------------------------------------------------------------------------------------------------------------
        public void ReloadDatasAction()
        {
            //NWDDataManager.SharedInstance().ReloadAllObjects();
            NWDGameDataManager.UnitySingleton().ReloadAllDatas();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void NetworkStatutTestAction()
        {
            NetworkStatutRender.SetNetworkState(NWDNetworkState.Check);
            NWDGameDataManager.UnitySingleton().TestNetWork();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Start()
        {
            if (DatasLoadGauge != null)
            {
                DatasLoadGauge.SetHidden(true);
            }
        }
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
            if (DatasLoadGauge != null)
            {
                DatasLoadGauge.SetHidden(false);
                DatasLoadGauge.SetHorizontalValue(0.0F);
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
            if (DatasLoadGauge != null)
            {
                DatasLoadGauge.SetHorizontalValue(sPurcent);
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
            if (DatasLoadGauge != null)
            {
                DatasLoadGauge.SetHidden(true);
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
            Debug.Log("NWDPanelDataManager NotificationNetworkOffLine()");
            if ( NetworkOffLineEvent!= null)
            {
                NetworkOffLineEvent.Invoke(sNotification);
            }
            if (NetworkStatutRender != null)
            {
                NetworkStatutRender.SetNetworkState(NWDNetworkState.OffLine);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent NetworkOnLineEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationNetworkOnLine(BTBNotification sNotification)
        {
            Debug.Log("NWDPanelDataManager NotificationNetworkOnLine()");
            if ( NetworkOnLineEvent!= null)
            {
                NetworkOnLineEvent.Invoke(sNotification);
            }
            if (NetworkStatutRender != null)
            {
                NetworkStatutRender.SetNetworkState(NWDNetworkState.OnLine);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent NetworkUnknowEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationNetworkUnknow(BTBNotification sNotification)
        {
            Debug.Log("NWDPanelDataManager NotificationNetworkUnknow()");
            if ( NetworkUnknowEvent!= null)
            {
                NetworkUnknowEvent.Invoke(sNotification);
            }
            if (NetworkStatutRender != null)
            {
                NetworkStatutRender.SetNetworkState(NWDNetworkState.Unknow);
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
        public override void NotificationWebOperationUploadInProgress(BTBNotification sNotification, float sPurcent)
        {
            if ( WebOperationUploadInProgressEvent!= null)
            {
                WebOperationUploadInProgressEvent.Invoke(sNotification);
            }

            if (WebSyncGauge != null)
            {
                WebSyncGauge.SetHidden(false);
                WebSyncGauge.SetVerticalValue(sPurcent/2.0F);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCallBackEvent WebOperationDownloadInProgressEvent;
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadInProgress(BTBNotification sNotification, float sPurcent)
        {
            if ( WebOperationDownloadInProgressEvent!= null)
            {
                WebOperationDownloadInProgressEvent.Invoke(sNotification);
            }
            if (WebSyncGauge != null)
            {
                WebSyncGauge.SetHidden(false);
                WebSyncGauge.SetVerticalValue(0.5F+sPurcent / 2.0F);
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
            if (WebSyncGauge != null)
            {
                WebSyncGauge.SetVerticalValue(1.0F);
                WebSyncGauge.SetHidden(true);
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
            if (WebSyncGauge != null)
            {
                WebSyncGauge.SetHidden(true);
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
            if (WebSyncGauge != null)
            {
                WebSyncGauge.SetHidden(true);
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
            if (WebSyncGauge != null)
            {
                WebSyncGauge.SetHidden(true);
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
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================