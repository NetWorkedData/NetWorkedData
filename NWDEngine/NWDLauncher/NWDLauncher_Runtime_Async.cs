//=====================================================================================================================
//
//  ideMobi 2020©
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using System.Collections;
using System.IO;
using System.Threading;
using System.Reflection;
using System.Linq;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDGameDataManager : NWDCallBackDataLoadOnly
    {
        //-------------------------------------------------------------------------------------------------------------
        private void Launch_Runtime_Async()
        {
            if (NWDAppConfiguration.SharedInstance().PreloadDatas == false)
            {
                if (NWDLauncher.GetState() != NWDStatut.NetWorkedDataReady)
                {
                    // Load async the engine!
                    Debug.Log("########## <color=blue>Load async the engine</color>!");
                    //NWDLauncher.Launch_Runtime_Async();
                    StartCoroutine(NWDLauncher.Launch_Runtime_Async());
                }
                else
                {
                    Debug.Log("########## <color=blue>Load async the engine ALL READY READY!</color>!");
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static partial class NWDLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        public static IEnumerator Launch_Runtime_Async()
        {
            NWEBenchmark.Start();
            Debug.Log("########## <color=blue>Launch_Runtime_AsyncAsync</color>!");

            NWENotificationManager.SharedInstance().PostNotification(null, NWDNotificationConstants.K_ENGINE_LAUNCH);
            // lauch engine
            Engine_Editor();
            while (State != NWDStatut.EngineLaunched)
            {
                yield return null;
            }
            // declare models
            Declare_Editor();
            while (State != NWDStatut.ClassDeclareFinish)
            {
                yield return null;
            }
            // restaure models' param
            Restaure_Editor();
            while (State != NWDStatut.ClassRestaureFinish)
            {
                yield return null;
            }
            // connect editor
            Connect_Editor_Editor();
            while (State != NWDStatut.DataEditorConnected)
            {
                yield return null;
            }
            // create table editor
            CreateTable_Editor_Editor();
            while (State != NWDStatut.DataEditorTableUpdated)
            {
                yield return null;
            }
            // load editor data
            LoadData_Editor_Editor();
            while (State != NWDStatut.DataEditorLoaded)
            {
                yield return null;
            }
            // index all data editor
            Index_Editor_Editor();
            while (State != NWDStatut.DataEditorIndexationFinish)
            {
                yield return null;
            }
            // need account pincode
            Connect_Account_Editor();
            while (State != NWDStatut.DataAccountConnected)
            {
                yield return null;
            }
            // create table account
            CreateTable_Account_Editor();
            while (State != NWDStatut.DataAccountTableUpdated)
            {
                yield return null;
            }
            // load account data account
            LoadData_Account_Editor();
            while (State != NWDStatut.DataAccountLoaded)
            {
                yield return null;
            }
            // index all data
            Index_Account_Editor();
            while (State != NWDStatut.DataIndexationFinish)
            {
                yield return null;
            }
            // Ready!
            Ready_Editor();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================