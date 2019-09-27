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
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_EDITOR_START_ASYNC_LOADING, delegate (NWENotification sNotification)
            {
                DBEditorStartAsyncLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
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
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_START_ASYNC_LOADING, delegate (NWENotification sNotification)
            {
                DBAccountStartAsyncLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
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
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_INDEXATION_START_ASYNC, delegate (NWENotification sNotification)
            {
                DataIndexationStartAsync(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
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
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_EDITOR_START_ASYNC_LOADING);

            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_REQUEST);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_SUCCESS);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_FAIL);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_STOP);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_NEEDED);

            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_READY);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_START_ASYNC_LOADING);

            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_START_LOADING);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_PARTIAL_LOADED);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_LOADED);

            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_START_LOADING);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_PARTIAL_LOADED);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_LOADED);

            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_START_LOADING);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_PARTIAL_LOADED);
            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_DATA_LOADED);

            tNotifManager.RemoveObserverForAll(this, NWDNotificationConstants.K_INDEXATION_START_ASYNC);
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
        //-------------------------------------------------------------------------------------------------------------
        public void PinCodeInsert(string sPinCode, string sPinCodeConfirm)
        {
            NWDLauncher.CodePinValue = sPinCode;
            NWDLauncher.CodePinValueConfirm = sPinCodeConfirm;
            NWDLauncher.DatabaseAccountConnection(sPinCode);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected void LaunchNext()
        {
            if (NWDLauncher.GetPreload() == false)
            {
                switch (NWDLauncher.GetState())
                {
                    case NWDStatut.DataEditorTableUpdated:
                        {
                            StartCoroutine(NWDLauncher.DatabaseEditorLoadDataAsync());
                        }
                        break;
                    case NWDStatut.DataAccountTableUpdated:
                        {
                            StartCoroutine(NWDLauncher.DatabaseAccountLoadDataAsync());
                        }
                        break;
                    case NWDStatut.DataAccountLoaded:
                        {
                            StartCoroutine(NWDLauncher.DatabaseIndexationAsync());
                        }
                        break;
                }
            }
        }
        //=============================================================================================================
        // VIRTUAL METHOD 
        //-------------------------------------------------------------------------------------------------------------
        public virtual void EngineLaunch(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override EngineLaunch(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBEditorConnected(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DBEditorConnected(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBEditorStartAsyncLoading(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DBEditorStartAsyncLoading(NWENotification sNotification, bool sPreloadDatas) and call LaunchNext()");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataEditorStartLoading(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DataEditorStartLoading(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataEditorPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            // create your method by override
            throw new Exception("override DataEditorPartialLoaded(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataEditorLoaded(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DataEditorLoaded(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountPinCodeRequest(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DBAccountPinCodeRequest(NWENotification sNotification, bool sPreloadDatas) : get  code and call PinCodeInsert(string sCode, string sCodeConfirm);");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountPinCodeSuccess(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DBAccountPinCodeSuccess(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountPinCodeFail(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DBAccountPinCodeFail(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountPinCodeStop(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DBAccountPinCodeStop(NWENotification sNotification, bool sPreloadDatas), assume to delete the database or not and quit app...");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountPinCodeNeeded(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DBAccountPinCodeNeeded(NWENotification sNotification, bool sPreloadDatas) : create new code and call PinCodeInsert(string sCode, string sCodeConfirm);");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountConnected(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DBAccountConnected(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountStartAsyncLoading(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DBAccountStartAsyncLoading(NWENotification sNotification, bool sPreloadDatas) and call LaunchNext()");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataAccountStartLoading(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DataAccountStartLoading(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataAccountPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            // create your method by override
            throw new Exception("override DataAccountPartialLoaded(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataAccountLoaded(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DataAccountLoaded(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataStartLoading(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DataStartLoading(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            // create your method by override
            throw new Exception("override DataPartialLoaded(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataLoaded(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DataLoaded(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataIndexationStartAsync(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DataIndexationStartAsync(NWENotification sNotification, bool sPreloadDatas) call LaunchNext()");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataIndexationStart(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DataIndexationStart(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataIndexationStep(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            // create your method by override
            throw new Exception("override DataIndexationStep(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataIndexationFinish(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override DataIndexationFinish(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void EngineReady(NWENotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
            throw new Exception("override EngineReady(NWENotification sNotification, bool sPreloadDatas)");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================