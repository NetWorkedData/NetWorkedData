//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:26:37
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
using UnityEngine.Events;

//using BasicToolBox;
using UnityEngine.Networking;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCallBackDataLoadOnly : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        void InstallObserver()
        {
            //Debug.Log("NWDCallBackDataLoadOnly InstallObserver()");
            NWENotificationManager tNotifManager = NWENotificationManager.SharedInstance();
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_LAUNCHER_ENGINE_READY, delegate (NWENotification sNotification)
            {
                LauncherEngineReady(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });

            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_LAUNCHER_EDITOR_READY, delegate (NWENotification sNotification)
            {
                LauncherEditorReady(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });

            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_LAUNCHER_ACCOUNT_READY, delegate (NWENotification sNotification)
            {
                LauncherAccountReady(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });

            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_LAUNCHER_STEP, delegate (NWENotification sNotification)
            {
                LauncherStep(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, NWDLauncher.GetPurcent());
            });

            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_NETWORKEDDATA_READY, delegate (NWENotification sNotification)
            {
                LauncherNetWorkdeDataReady(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        void RemoveObserver()
        {
            // get NWENotificationManager shared instance from the NWDGameDataManager Singleton
            NWENotificationManager tNotifManager = NWENotificationManager.SharedInstance();
            // remove this from NWENotificationManager
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_LAUNCHER_STEP);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_LAUNCHER_ENGINE_READY);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_LAUNCHER_EDITOR_READY);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_LAUNCHER_ACCOUNT_READY);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_NETWORKEDDATA_READY);
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
        //-------------------------------------------------------------------------------------------------------------
        public void PinCodeInsert(string sPinCode, string sPinCodeConfirm)
        {
            //NWDLauncher.CodePinValue = sPinCode;
            //NWDLauncher.CodePinValueConfirm = sPinCodeConfirm;
            //NWDLauncher.DatabaseAccountConnection(sPinCode);
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void LauncherEngineReady(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override LauncherEngineReady(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void LauncherEditorReady(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override LauncherEditorReady(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void LauncherAccountReady(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override LauncherAccountReady(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void LauncherStep(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            // create your method by override
            throw new Exception("override LauncherStep(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void LauncherNetWorkdeDataReady(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override LauncherNetWorkdeDataReady(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================