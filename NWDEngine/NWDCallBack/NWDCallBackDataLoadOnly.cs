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
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_ENGINE_LAUNCH, delegate (NWENotification sNotification)
            {
                EngineLaunch(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_EDITOR_READY, delegate (NWENotification sNotification)
            {
                DBEditorConnected(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_REQUEST, delegate (NWENotification sNotification)
            {
                DBAccountPinCodeRequest(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_SUCCESS, delegate (NWENotification sNotification)
            {
                DBAccountPinCodeSuccess(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_FAIL, delegate (NWENotification sNotification)
            {
                DBAccountPinCodeFail(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_STOP, delegate (NWENotification sNotification)
            {
                DBAccountPinCodeStop(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_NEEDED, delegate (NWENotification sNotification)
            {
                DBAccountPinCodeNeeded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_READY, delegate (NWENotification sNotification)
            {
                DBAccountConnected(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_START_LOADING, delegate (NWENotification sNotification)
            {
                DataEditorStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_PARTIAL_LOADED, delegate (NWENotification sNotification)
            {
                float tPurcent = NWDDataManager.SharedInstance().PurcentEditorLoaded();
                DataEditorPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_LOADED, delegate (NWENotification sNotification)
            {
                DataEditorLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_START_LOADING, delegate (NWENotification sNotification)
            {
                DataAccountStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_PARTIAL_LOADED, delegate (NWENotification sNotification)
            {
                float tPurcent = NWDDataManager.SharedInstance().PurcentAccountLoaded();
                DataAccountPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_LOADED, delegate (NWENotification sNotification)
            {
                DataAccountLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_START_LOADING, delegate (NWENotification sNotification)
            {
                DataStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_PARTIAL_LOADED, delegate (NWENotification sNotification)
            {
                float tPurcent = NWDDataManager.SharedInstance().PurcentLoaded();
                DataPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_LOADED, delegate (NWENotification sNotification)
            {
                DataLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_INDEXATION_START, delegate (NWENotification sNotification)
            {
                DataIndexationStart(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_INDEXATION_FINISH, delegate (NWENotification sNotification)
            {
                DataIndexationFinish(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_INDEXATION_STEP, delegate (NWENotification sNotification)
            {
                float tPurcent = NWDDataManager.SharedInstance().PurcentIndexed();
                DataIndexationStep(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_ENGINE_READY, delegate (NWENotification sNotification)
            {
                EngineReady(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        void RemoveObserver()
        {
            //Debug.Log("Remove OBSERVER OF NWDCallBackDataLoadOnly");
            // get NWENotificationManager shared instance from the NWDGameDataManager Singleton
            NWENotificationManager tNotifManager = NWENotificationManager.SharedInstance();

            // remove this from NWENotificationManager
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ENGINE_LAUNCH);

            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_EDITOR_READY);

            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_REQUEST);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_SUCCESS);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_FAIL);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_STOP);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_NEEDED);

            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_READY);

            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_START_LOADING);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_PARTIAL_LOADED);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_LOADED);

            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_START_LOADING);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_PARTIAL_LOADED);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_LOADED);

            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_START_LOADING);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_PARTIAL_LOADED);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_LOADED);

            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_INDEXATION_START);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_INDEXATION_STEP);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_INDEXATION_FINISH);

            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_ENGINE_READY);
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
        //=============================================================================================================
        // VIRTUAL METHOD 
        //-------------------------------------------------------------------------------------------------------------
        public virtual void EngineLaunch(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBEditorConnected(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataEditorStartLoading(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataEditorPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataEditorLoaded(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountPinCodeRequest(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountPinCodeSuccess(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountPinCodeFail(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountPinCodeStop(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountPinCodeNeeded(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountConnected(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataAccountStartLoading(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataAccountPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataAccountLoaded(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataStartLoading(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataLoaded(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataIndexationStart(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataIndexationStep(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataIndexationFinish(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void EngineReady(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================