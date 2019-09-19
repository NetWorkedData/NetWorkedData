//=====================================================================================================================
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
//=====================================================================================================================



using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

//using BasicToolBox;

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
        public override void NotificationEngineLaunch(NWENotification sNotification)
        {
            if(EngineLaunchEvent!=null)
            {
                EngineLaunchEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasEditorStartLoading(NWENotification sNotification, bool sPreloadDatas)
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
        public override void NotificationDatasEditorPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
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
        public override void NotificationDatasEditorLoaded(NWENotification sNotification, bool sPreloadDatas)
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
        public override void NotificationDatasAccountStartLoading(NWENotification sNotification, bool sPreloadDatas)
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
        public override void NotificationDatasAccountPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
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
        public override void NotificationDatasAccountLoaded(NWENotification sNotification, bool sPreloadDatas)
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
        public override void NotificationLanguageChanged(NWENotification sNotification)
        {
            if ( LanguageChangedEvent!= null)
            {
                LanguageChangedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDataLocalUpdate(NWENotification sNotification)
        {
            if ( DataLocalUpdateEvent!= null)
            {
                DataLocalUpdateEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDataLocalInsert(NWENotification sNotification)
        {
            if ( DataLocalInsertEvent!= null)
            {
                DataLocalInsertEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDataLocalDelete(NWENotification sNotification)
        {
            if ( DataLocalDeleteEvent!= null)
            {
                DataLocalDeleteEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationDatasWebUpdate(NWENotification sNotification)
        {
            if ( DatasWebUpdateEvent!= null)
            {
                DatasWebUpdateEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationError(NWENotification sNotification)
        {
            if ( ErrorEvent!= null)
            {
                ErrorEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationAccountChanged(NWENotification sNotification)
        {
            if ( AccountChangedEvent!= null)
            {
                AccountChangedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationAccountSessionExpired(NWENotification sNotification)
        {
            if ( AccountSessionExpiredEvent!= null)
            {
                AccountSessionExpiredEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationAccountBanned(NWENotification sNotification)
        {
            if ( AccountBannedEvent!= null)
            {
                AccountBannedEvent.Invoke(sNotification);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void NotificationNetworkOffLine(NWENotification sNotification)
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
        public override void NotificationNetworkOnLine(NWENotification sNotification)
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
        public override void NotificationNetworkUnknow(NWENotification sNotification)
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
        public override void NotificationNetworkCheck(NWENotification sNotification)
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
        public override void NotificationWebOperationError(NWENotification sNotification)
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
        public override void NotificationWebOperationUploadStart(NWENotification sNotification)
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
        public override void NotificationWebOperationUploadInProgress(NWENotification sNotification, float sPurcent)
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
        public override void NotificationWebOperationDownloadInProgress(NWENotification sNotification, float sPurcent)
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
        public override void NotificationWebOperationDownloadIsDone(NWENotification sNotification)
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
        public override void NotificationWebOperationDownloadFailed(NWENotification sNotification)
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
        public override void NotificationWebOperationDownloadError(NWENotification sNotification)
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
        public override void NotificationWebOperationDownloadSuccessed(NWENotification sNotification)
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
        public override void NotificationGeneric(NWENotification sNotification)
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