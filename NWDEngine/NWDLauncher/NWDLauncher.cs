//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:5
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using SQLite4Unity3d;
using System.Collections;
using System.IO;
using System.Reflection;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDStatut
    {
        Error = -9,

        None = -1,

        // launch engine NetWorkedData
        EngineLaunching = 10,

        ClassDeclareStart = 11,
        ClassDeclareFinish = 12,
        ClassRestaureStart = 13,
        ClassRestaureFinish = 14,

        EngineLaunched = 19,
        // engine NetWorkedData ready

        // connect database editor
        DataEditorConnecting = 20,
        DataEditorConnected = 21,
        DataEditorTableUpdated = 22,
        // waiting to load data async or sync order
        // then Notify K_DB_EDITOR_START_ASYNC_LOADING to call 
        // DataEditorStartLoading
        // DataEditorPartialLoaded
        // DataEditorLoaded
        DataEditorLoading = 28,
        DataEditorLoaded = 29,


        DataEditorIndexationStart = 50,
        DataEditorIndexationFinish = 52,

        DataAccountConnecting = 30,
        DataAccountCodePinCreate = 31,
        DataAccountCodePinRequest = 32,
        DataAccountCodePinFail = 33,
        DataAccountCodePinStop = 34,
        DataAccountCodePinSuccess = 35,
        DataAccountConnected = 36,

        DataAccountTableUpdated = 37,
        // waiting to load data async or sync order
        // then Notify K_DB_ACCOUNT_START_ASYNC_LOADING to call 
        // DataAccountStartLoading
        // DataAccountPartialLoaded
        // DataAccountLoaded
        DataAccountLoading = 38,
        DataAccountLoaded = 39,

        // waiting to load data async or sync order
        // then Notify K_DB_INDEXATION_START_ASYNC_LOADING to call 
        // DataIndexationStart
        // DataIndexationStep
        // DataIndexationFinish
        DataIndexationStart = 40,
        DataIndexationFinish = 42,

        NetWorkedDataReady = 99,

    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static partial class NWDLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        static private NWDStatut State = NWDStatut.None;
        static private int StepSum;
        static private int StepIndex;
        static private bool Launched = false;
        static bool Preload = true;
        //-------------------------------------------------------------------------------------------------------------
        static public void StepIndcrement()
        {
            StepIndex++;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDStatut GetState()
        {
            return State;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public bool GetPreload()
        {
            return Preload;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public bool EditorByPass;
        //-------------------------------------------------------------------------------------------------------------
        public const string K_PINCODE_KEY = "K_PINCODE_KEY_jkghvjh";
        //-------------------------------------------------------------------------------------------------------------
        static public void Launch()
        {
            NWEBenchmark.Start();
            if (Launched == false)
            {
                Launched = true;
                //NWDToolbox.EditorAndPlaying("NWDLauncher Launch()");
                EditorByPass = false;
#if UNITY_EDITOR
                EditorApplication.quitting += Quit;
                if (EditorApplication.isPlayingOrWillChangePlaymode == false)
                {
                    EditorByPass = true;
                    if (EditorPrefs.HasKey(K_PINCODE_KEY))
                    {
                        string tPincode = EditorPrefs.GetString(K_PINCODE_KEY);
                    }
                }
#endif
                if (EditorByPass == true)
                {
                    Preload = true;
                    Launch_Editor();
                }
                else
                {
                    Preload = NWDAppConfiguration.SharedInstance().PreloadDatas;
                    if (Preload == true)
                    {
                        Launch_Runtime_Sync();
                    }
                    else
                    {
                        //Launch_Runtime_Async(); // waiting order from NWDGameDataManager.ShareInstance()
                    }
                }
            }
            //LaunchNext();
            NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
        static void Quit()
        {
            // close connection?
            //TODO: close connection ?

            // delete editor key
#if UNITY_EDITOR
            //Debug.Log("Quitting the Editor");
            if (EditorPrefs.HasKey(K_PINCODE_KEY))
            {
                EditorPrefs.DeleteKey(K_PINCODE_KEY);
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void LaunchNext()
        {
            /*
            //Debug.Log("LaunchNext() Preload " + Preload.ToString() + " and state = " + State.ToString());
            //NWDToolbox.EditorAndPlaying("NWDLauncher LaunchNext()");
            switch (State)
            {
                case NWDStatut.Error:
                    {
                        // TODO error;
                    }
                    break;
                case NWDStatut.None:
                    {
                        EngineLaunch();
                    }
                    break;
                case NWDStatut.EngineLaunching:
                    {
                        // engine in progress ... do nothing
                    }
                    break;
                case NWDStatut.EngineLaunched:
                    {
                        ConnectToDatabaseEditor();
                    }
                    break;
                case NWDStatut.DataEditorConnecting:
                    {
                        // engine in progress ... do nothing
                    }
                    break;
                case NWDStatut.DataEditorConnected:
                    {
                        DatabaseEditorTable();
                    }
                    break;
                case NWDStatut.DataEditorTableUpdated:
                    {
                        DatabaseEditorLoadData();
                    }
                    break;
                case NWDStatut.DataEditorLoading:
                    {
                    }
                    break;
                case NWDStatut.DataEditorLoaded:
                    {
                        ConnectToDatabaseAccount();
                    }
                    break;
                case NWDStatut.DataAccountConnecting:
                    {
                    }
                    break;
                case NWDStatut.DataAccountCodePinCreate:
                    {
                    }
                    break;
                case NWDStatut.DataAccountCodePinRequest:
                    {
                        ConnectToDatabaseAccount();
                    }
                    break;
                case NWDStatut.DataAccountCodePinFail:
                    {
                    }
                    break;
                case NWDStatut.DataAccountCodePinStop:
                    {
                    }
                    break;
                case NWDStatut.DataAccountCodePinSuccess:
                    {
                    }
                    break;
                case NWDStatut.DataAccountConnected:
                    {
                        DatabaseAccountTable();
                    }
                    break;
                case NWDStatut.DataAccountTableUpdated:
                    {
                        DatabaseAccountLoadData();
                    }
                    break;
                case NWDStatut.DataAccountLoading:
                    {
                    }
                    break;
                case NWDStatut.DataAccountLoaded:
                    {
                        DatabaseIndexationStart();
                    }
                    break;
                case NWDStatut.DataIndexationStart:
                    {
                    }
                    break;
                case NWDStatut.DataIndexationFinish:
                    {
                        Ready();
                    }
                    break;
                case NWDStatut.NetWorkedDataReady:
                    {

                    }
                    break;
            }
            */
        }
        //-------------------------------------------------------------------------------------------------------------
        //static public void LaunchPause()
        //{
        //    switch (State)
        //    {
        //        case NWDStatut.None:
        //            {
        //            }
        //            break;
        //        case NWDStatut.EngineLaunching:
        //            {
        //                // engine in progress ... do nothing
        //            }
        //            break;
        //        case NWDStatut.EngineLaunched:
        //            {
        //            }
        //            break;
        //        case NWDStatut.DataEditorConnecting:
        //            {
        //            }
        //            break;
        //        case NWDStatut.DataEditorConnected:
        //            {
        //            }
        //            break;
        //        case NWDStatut.DataEditorTableUpdated:
        //            {
        //            }
        //            break;
        //        case NWDStatut.DataEditorLoading:
        //            {
        //                State = NWDStatut.DataEditorTableUpdated;
        //            }
        //            break;
        //        case NWDStatut.DataEditorLoaded:
        //            {
        //            }
        //            break;
        //        case NWDStatut.DataAccountConnecting:
        //            {
        //                State = NWDStatut.DataEditorLoaded;
        //            }
        //            break;
        //        case NWDStatut.DataAccountCodePinCreate:
        //            {
        //                State = NWDStatut.DataEditorLoaded;
        //            }
        //            break;
        //        case NWDStatut.DataAccountCodePinRequest:
        //            {
        //                State = NWDStatut.DataEditorLoaded;
        //            }
        //            break;
        //        case NWDStatut.DataAccountCodePinFail:
        //            {
        //                State = NWDStatut.DataEditorLoaded;
        //            }
        //            break;
        //        case NWDStatut.DataAccountCodePinStop:
        //            {
        //                State = NWDStatut.DataEditorLoaded;
        //            }
        //            break;
        //        case NWDStatut.DataAccountCodePinSuccess:
        //            {
        //                State = NWDStatut.DataEditorLoaded;
        //            }
        //            break;
        //        case NWDStatut.DataAccountConnected:
        //            {
        //                State = NWDStatut.DataEditorLoaded;
        //            }
        //            break;
        //        case NWDStatut.DataAccountTableUpdated:
        //            {
        //            }
        //            break;
        //        case NWDStatut.DataAccountLoading:
        //            {
        //                State = NWDStatut.DataAccountTableUpdated;
        //            }
        //            break;
        //        case NWDStatut.DataAccountLoaded:
        //            {
        //            }
        //            break;
        //        case NWDStatut.DataIndexationStart:
        //            {
        //            }
        //            break;
        //        case NWDStatut.DataIndexationFinish:
        //            {
        //            }
        //            break;
        //        case NWDStatut.NetWorkedDataReady:
        //            {
        //            }
        //            break;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
        //static public void LaunchResume()
        //{
        //    LaunchNext();
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================