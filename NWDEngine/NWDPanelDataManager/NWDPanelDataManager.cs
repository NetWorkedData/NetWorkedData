// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:44
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
    public partial class NWDPanelDataManager : NWDGameCallBack
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDNetworkStatutRender NetworkStatutRender;
        public NWDNetworkWebStatutRender NetworkWebStatutRender;
        public NWDGaugeRender DatasLoadGauge;
        public NWDGaugeRender WebSyncGauge;
        public GameObject WebSyncSpinner;
        public GameObject AlertPanel;
        //-------------------------------------------------------------------------------------------------------------
        public void NetworkStatutTestAction()
        {
            NWDGameDataManager.UnitySingleton().TestNetWork();
        }
        //-------------------------------------------------------------------------------------------------------------
        private void Start()
        {
            if (DatasLoadGauge != null)
            {
                DatasLoadGauge.SetHidden(true);
            }
            if (WebSyncGauge != null)
            {
                WebSyncGauge.SetHidden(true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationEngineLaunch(BTBNotification sNotification)
        {
            if(EngineLaunchEvent!=null)
            {
                EngineLaunchEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasEditorStartLoading(BTBNotification sNotification, bool sPreloadDatas)
        {
            if (DatasEditorStartLoadingEvent != null)
            {
                DatasEditorStartLoadingEvent.Invoke(sNotification);
            }
            if (DatasLoadGauge != null)
            {
                DatasLoadGauge.SetHidden(false);
                DatasLoadGauge.SetHorizontalValue(0.0F);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasEditorPartialLoaded(BTBNotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            if (DatasEditorPartialLoadedEvent != null)
            {
                DatasEditorPartialLoadedEvent.Invoke(sNotification);
            }
            if (DatasLoadGauge != null)
            {
                DatasLoadGauge.SetHorizontalValue(sPurcent);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasEditorLoaded(BTBNotification sNotification, bool sPreloadDatas)
        {
            if (DatasEditorLoadedEvent != null)
            {
                DatasEditorLoadedEvent.Invoke(sNotification);
            }
            if (DatasLoadGauge != null)
            {
                DatasLoadGauge.SetHidden(true);
            }
        }


        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasAccountStartLoading(BTBNotification sNotification, bool sPreloadDatas)
        {
            if (DatasAccountStartLoadingEvent != null)
            {
                DatasAccountStartLoadingEvent.Invoke(sNotification);
            }
            if (DatasLoadGauge != null)
            {
                DatasLoadGauge.SetHidden(false);
                DatasLoadGauge.SetHorizontalValue(0.0F);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasAccountPartialLoaded(BTBNotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            if (DatasAccountPartialLoadedEvent != null)
            {
                DatasAccountPartialLoadedEvent.Invoke(sNotification);
            }
            if (DatasLoadGauge != null)
            {
                DatasLoadGauge.SetHorizontalValue(sPurcent);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasAccountLoaded(BTBNotification sNotification, bool sPreloadDatas)
        {
            if (DatasAccountLoadedEvent != null)
            {
                DatasAccountLoadedEvent.Invoke(sNotification);
            }
            if (DatasLoadGauge != null)
            {
                DatasLoadGauge.SetHidden(true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationLanguageChanged(BTBNotification sNotification)
        {
            if ( LanguageChangedEvent!= null)
            {
                LanguageChangedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDataLocalUpdate(BTBNotification sNotification)
        {
            if ( DataLocalUpdateEvent!= null)
            {
                DataLocalUpdateEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDataLocalInsert(BTBNotification sNotification)
        {
            if ( DataLocalInsertEvent!= null)
            {
                DataLocalInsertEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDataLocalDelete(BTBNotification sNotification)
        {
            if ( DataLocalDeleteEvent!= null)
            {
                DataLocalDeleteEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasWebUpdate(BTBNotification sNotification)
        {
            if ( DatasWebUpdateEvent!= null)
            {
                DatasWebUpdateEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationError(BTBNotification sNotification)
        {
            if ( ErrorEvent!= null)
            {
                ErrorEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationAccountChanged(BTBNotification sNotification)
        {
            if ( AccountChangedEvent!= null)
            {
                AccountChangedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationAccountSessionExpired(BTBNotification sNotification)
        {
            if ( AccountSessionExpiredEvent!= null)
            {
                AccountSessionExpiredEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationAccountBanned(BTBNotification sNotification)
        {
            if ( AccountBannedEvent!= null)
            {
                AccountBannedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationNetworkOffLine(BTBNotification sNotification)
        {
            //Debug.Log("NWDPanelDataManager NotificationNetworkOffLine()");
            if ( NetworkOffLineEvent!= null)
            {
                NetworkOffLineEvent.Invoke(sNotification);
            }
            if (NetworkStatutRender != null)
            {
                NetworkStatutRender.SetNetworkState(NWDNetworkState.OffLine);
            }
            if (NetworkWebStatutRender != null)
            {
                NetworkWebStatutRender.SetNetworkState(NWDNetworkState.OffLine);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationNetworkOnLine(BTBNotification sNotification)
        {
            //Debug.Log("NWDPanelDataManager NotificationNetworkOnLine()");
            if ( NetworkOnLineEvent!= null)
            {
                NetworkOnLineEvent.Invoke(sNotification);
            }
            if (NetworkStatutRender != null)
            {
                NetworkStatutRender.SetNetworkState(NWDNetworkState.OnLine);
            }
            if (NetworkWebStatutRender != null)
            {
                NetworkWebStatutRender.SetNetworkState(NWDNetworkState.OnLine);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationNetworkUnknow(BTBNotification sNotification)
        {
            //Debug.Log("NWDPanelDataManager NotificationNetworkUnknow()");
            if ( NetworkUnknowEvent!= null)
            {
                NetworkUnknowEvent.Invoke(sNotification);
            }
            if (NetworkStatutRender != null)
            {
                NetworkStatutRender.SetNetworkState(NWDNetworkState.Unknow);
            }
            if (NetworkWebStatutRender != null)
            {
                NetworkWebStatutRender.SetNetworkState(NWDNetworkState.Unknow);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationNetworkCheck(BTBNotification sNotification)
        {
            //Debug.Log("NWDPanelDataManager NotificationNetworkCheck()");
            if (NetworkCheckEvent != null)
            {
                NetworkCheckEvent.Invoke(sNotification);
            }
            if (NetworkStatutRender != null)
            {
                NetworkStatutRender.SetNetworkState(NWDNetworkState.Check);
            }
            if (NetworkWebStatutRender != null)
            {
                NetworkWebStatutRender.SetNetworkState(NWDNetworkState.Check);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationError(BTBNotification sNotification)
        {
            if ( WebOperationErrorEvent!= null)
            {
                WebOperationErrorEvent.Invoke(sNotification);
            }
            if (NetworkWebStatutRender != null)
            {
                //NetworkWebStatutRender.SetNetworkState(NWDNetworkState.Check);
                NetworkWebStatutRender.SyncError();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationUploadStart(BTBNotification sNotification)
        {
            if (WebOperationUploadStartEvent != null)
            {
                WebOperationUploadStartEvent.Invoke(sNotification);
            }
            if (WebSyncGauge != null)
            {
                WebSyncGauge.SetHidden(false);
                WebSyncGauge.SetHorizontalValue(0.0F);
            }
            if (NetworkWebStatutRender != null)
            {
                NetworkWebStatutRender.SyncStart();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationUploadInProgress(BTBNotification sNotification, float sPurcent)
        {
            if ( WebOperationUploadInProgressEvent!= null)
            {
                WebOperationUploadInProgressEvent.Invoke(sNotification);
            }
            if (WebSyncGauge != null)
            {
                WebSyncGauge.SetHorizontalValue(sPurcent/2.0F);
            }
            if (NetworkWebStatutRender != null)
            {
                NetworkWebStatutRender.SyncStep();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadInProgress(BTBNotification sNotification, float sPurcent)
        {
            if ( WebOperationDownloadInProgressEvent!= null)
            {
                WebOperationDownloadInProgressEvent.Invoke(sNotification);
            }
            if (WebSyncGauge != null)
            {
                WebSyncGauge.SetHorizontalValue(0.5F+sPurcent / 2.0F);
            }
            if (NetworkWebStatutRender != null)
            {
                NetworkWebStatutRender.SyncStep();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadIsDone(BTBNotification sNotification)
        {
            if ( WebOperationDownloadIsDoneEvent!= null)
            {
                WebOperationDownloadIsDoneEvent.Invoke(sNotification);
            }
            if (WebSyncGauge != null)
            {
                WebSyncGauge.SetHorizontalValue(1.0F);
                WebSyncGauge.SetHidden(true);
            }
            if (NetworkWebStatutRender != null)
            {
                //NetworkWebStatutRender.SetNetworkState(NWDNetworkState.Check);
                NetworkWebStatutRender.SyncSucess();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadFailed(BTBNotification sNotification)
        {
            if ( WebOperationDownloadFailedEvent!= null)
            {
                WebOperationDownloadFailedEvent.Invoke(sNotification);
            }
            if (WebSyncGauge != null)
            {
                WebSyncGauge.SetHorizontalValue(1.0F);
                WebSyncGauge.SetHidden(true);
            }
            if (NetworkWebStatutRender != null)
            {
                //NetworkWebStatutRender.SetNetworkState(NWDNetworkState.Check);
                NetworkWebStatutRender.SyncError();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadError(BTBNotification sNotification)
        {
            if ( WebOperationDownloadErrorEvent!= null)
            {
                WebOperationDownloadErrorEvent.Invoke(sNotification);
            }
            if (WebSyncGauge != null)
            {
                WebSyncGauge.SetHorizontalValue(1.0F);
                WebSyncGauge.SetHidden(true);
            }
            if (NetworkWebStatutRender != null)
            {
                //NetworkWebStatutRender.SetNetworkState(NWDNetworkState.Check);
                NetworkWebStatutRender.SyncError();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationWebOperationDownloadSuccessed(BTBNotification sNotification)
        {
            if ( WebOperationDownloadSuccessedEvent!= null)
            {
                WebOperationDownloadSuccessedEvent.Invoke(sNotification);
            }
            if (WebSyncGauge != null)
            {
                WebSyncGauge.SetHorizontalValue(1.0F);
                WebSyncGauge.SetHidden(true);
            }
            if (NetworkWebStatutRender != null)
            {
                //NetworkWebStatutRender.SetNetworkState(NWDNetworkState.Check);
                NetworkWebStatutRender.SyncSucess();
            }
        }
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