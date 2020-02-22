//=====================================================================================================================
//
//  ideMobi 2020©
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDSplashscreenController : MonoBehaviour //: NWDCallBackDataLoadOnly
    {
        //-------------------------------------------------------------------------------------------------------------
        public bool Log = false;
        public bool AutoClickNextButton = true;
        public Button NextButton;
        public Button SyncButton;
        //-------------------------------------------------------------------------------------------------------------
        //private void Start()
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange> Start </color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //    if (NWDLauncher.GetState() != NWDStatut.NetWorkedDataReady)
        //    {
        //        if (NextButton != null)
        //        {
        //            NextButton.gameObject.SetActive(false);
        //        }
        //        if (SyncButton != null)
        //        {
        //            SyncButton.gameObject.SetActive(false);
        //        }
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //protected void OnApplicationPause(bool sPauseStatus)
        //{
        //    //NWDLauncher.OnApplicationPause(sPauseStatus);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void LauncherEngineReady(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange> EngineLaunch</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void LauncherEditorReady(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DBEditorConnected</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DBEditorStartAsyncLoading(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DBEditorStartAsyncLoading</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //    //LaunchNext();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DataEditorStartLoading(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DataEditorStartLoading</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DataEditorPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DataEditorPartialLoaded</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DataEditorLoaded(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DataEditorLoaded</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //    //LaunchNext();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DBAccountPinCodeRequest(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DBAccountPinCodeRequest</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DBAccountPinCodeSuccess(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DBAccountPinCodeSuccess</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DBAccountPinCodeFail(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DBAccountPinCodeFail</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DBAccountPinCodeStop(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DBAccountPinCodeStop</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DBAccountPinCodeNeeded(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DBAccountPinCodeNeeded</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void LauncherAccountReady(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DBAccountConnected</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DBAccountStartAsyncLoading(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DBAccountStartAsyncLoading</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //    //LaunchNext();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DataAccountStartLoading(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DataAccountStartLoading</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DataAccountPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DataAccountPartialLoaded</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DataAccountLoaded(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DataAccountLoaded</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //    //LaunchNext();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DataStartLoading(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DataStartLoading</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DataPartialLoaded(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DataPartialLoaded</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DataLoaded(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DataLoaded</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //    //LaunchNext();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DataIndexationStartAsync(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DataIndexationStartAsync</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //    //LaunchNext();
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DataIndexationStart(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DataIndexationStart</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void LauncherStep(NWENotification sNotification, bool sPreloadDatas, float sPurcent)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DataIndexationStep</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void DataIndexationFinish(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>DataIndexationFinish</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void LauncherNetWorkdeDataReady(NWENotification sNotification, bool sPreloadDatas)
        //{
        //    if (Log == true)
        //    {
        //        Debug.Log("<color=red>!!!!!</color><color=orange>EngineReady</color>" + " state : " + NWDLauncher.GetState().ToString());
        //    }
        //    if (NextButton != null)
        //    {
        //        NextButton.gameObject.SetActive(true);
        //        if (AutoClickNextButton == true)
        //        {
        //            NextButton.onClick.Invoke();
        //        }
        //    }
        //    if (SyncButton != null)
        //    {
        //        SyncButton.gameObject.SetActive(true);
        //    }
        //    NWDBasisHelper tHelper = NWDBasisHelper.BasisHelper<NWDVersion>();
        //    Debug.Log("FINAL : Application.version = " + Application.version);
        //    Debug.Log("FINAL : nombre de version = " + tHelper.Datas.Count);
        //    if (tHelper.Datas.Count > 0)
        //    {
        //        Debug.Log("FINAL : find 1 with = " + ((NWDVersion)tHelper.Datas[0]).Version.ToString());
        //    }
        //    if (NWDVersion.CurrentData() != null)
        //    {
        //        Debug.Log("FINAL : numero de version " + NWDVersion.CurrentData().Version.ToString());
        //    }
        //    else
        //    {
        //        Debug.Log("FINAL : numero de version CurrentData() ERROR");
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void SyncAll()
        //{
        //    NWDDataManager.SharedInstance().AddWebRequestAllSynchronization();
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
