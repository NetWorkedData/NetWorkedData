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
using UnityEngine.Networking;

//=====================================================================================================================
namespace NetWorkedData
{

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDCallBackDataLoadOnly : MonoBehaviour
    {
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Installs the observer in the BTBNotification manager
        /// </summary>
        void InstallObserver()
        {
            Debug.Log("INSTALL OBSERVER OF NWDCallBackDataLoadOnly");
            // get BTBNotificationManager shared instance from the NWDGameDataManager Singleton
            BTBNotificationManager tNotifManager = BTBNotificationManager.SharedInstance();

            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_ENGINE_LAUNCH, delegate (BTBNotification sNotification)
            {
                EngineLaunch(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_EDITOR_READY, delegate (BTBNotification sNotification)
            {
                DBEditorConnected(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });


            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_REQUEST, delegate (BTBNotification sNotification)
            {
                DBAccountPinCodeRequest(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_SUCCESS, delegate (BTBNotification sNotification)
            {
                DBAccountPinCodeSuccess(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_FAIL, delegate (BTBNotification sNotification)
            {
                DBAccountPinCodeFail(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_STOP, delegate (BTBNotification sNotification)
            {
                DBAccountPinCodeStop(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_PINCODE_NEEDED, delegate (BTBNotification sNotification)
            {
                DBAccountPinCodeNeeded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DB_ACCOUNT_READY, delegate (BTBNotification sNotification)
            {
                DBAccountConnected(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });

            // load datas
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_START_LOADING, delegate (BTBNotification sNotification)
            {
                DataEditorStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_PARTIAL_LOADED, delegate (BTBNotification sNotification)
            {
                float tPurcent = NWDDataManager.SharedInstance().PurcentEditorLoaded();
                DataEditorPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_EDITOR_LOADED, delegate (BTBNotification sNotification)
            {
                DataEditorLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            // load datas
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_START_LOADING, delegate (BTBNotification sNotification)
            {
                DataAccountStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_PARTIAL_LOADED, delegate (BTBNotification sNotification)
            {
                float tPurcent = NWDDataManager.SharedInstance().PurcentAccountLoaded();
                DataAccountPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_ACCOUNT_LOADED, delegate (BTBNotification sNotification)
            {
                DataAccountLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            // load datas
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_START_LOADING, delegate (BTBNotification sNotification)
            {
                DataStartLoading(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_PARTIAL_LOADED, delegate (BTBNotification sNotification)
            {
                float tPurcent = NWDDataManager.SharedInstance().PurcentLoaded();
                DataPartialLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas, tPurcent);
            });
            tNotifManager.AddObserverForAll(this, NWDNotificationConstants.K_DATA_LOADED, delegate (BTBNotification sNotification)
            {
                DataLoaded(sNotification, NWDAppConfiguration.SharedInstance().PreloadDatas);
            });
        }
        //-------------------------------------------------------------------------------------------------------------
        void RemoveObserver()
        {
            // get BTBNotificationManager shared instance from the NWDGameDataManager Singleton
            BTBNotificationManager tNotifManager = BTBNotificationManager.SharedInstance();

            // remove this from BTBNotificationManager
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
        public virtual void EngineLaunch(BTBNotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }






        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBEditorConnected(BTBNotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataEditorStartLoading(BTBNotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataEditorPartialLoaded(BTBNotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataEditorLoaded(BTBNotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }





        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountPinCodeRequest(BTBNotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountPinCodeSuccess(BTBNotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountPinCodeFail(BTBNotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountPinCodeStop(BTBNotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountPinCodeNeeded(BTBNotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DBAccountConnected(BTBNotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataAccountStartLoading(BTBNotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataAccountPartialLoaded(BTBNotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataAccountLoaded(BTBNotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }





        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataStartLoading(BTBNotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataPartialLoaded(BTBNotification sNotification, bool sPreloadDatas, float sPurcent)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void DataLoaded(BTBNotification sNotification, bool sPreloadDatas)
        {
            // create your method by override
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================