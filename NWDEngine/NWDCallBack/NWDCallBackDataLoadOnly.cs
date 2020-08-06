//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

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