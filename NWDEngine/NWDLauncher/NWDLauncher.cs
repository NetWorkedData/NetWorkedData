//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDStatut : int
    {
        Error = -9,

        None = -1,

        EngineLaunching = 10,
        EngineLaunched = 19,

        DataEditorConnecting = 20,
        DataEditorConnected = 21,
        DataEditorTableUpdated = 22,
        DataEditorLoading = 23,
        DataEditorLoaded = 29,

        DataAccountConnecting = 30,
        DataAccountCodePinCreate = 31,
        DataAccountCodePinRequest = 32,
        DataAccountCodePinFail = 33,
        DataAccountCodePinStop = 34,
        DataAccountCodePinSuccess = 35,
        DataAccountConnected = 36,
        DataAccountTableUpdated = 37,
        DataAccountLoading = 38,
        DataAccountLoaded = 39,

        DataIndexationStart = 40,
        DataIndexationStep = 41,
        DataIndexationFinish = 42,

        NetWorkedDataReady = 99,

    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static partial class NWDLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        static private NWDStatut State = NWDStatut.None;
        static private bool Launched = false;
        //-------------------------------------------------------------------------------------------------------------
        static public NWDStatut GetState()
        {
            return State;
        }
        //-------------------------------------------------------------------------------------------------------------
        static private bool EditorByPass;
        //-------------------------------------------------------------------------------------------------------------
        public const string K_PINCODE_KEY = "K_PINCODE_KEY_jkghvjh";
        //-------------------------------------------------------------------------------------------------------------
        static public void Launch()
        {
            BTBBenchmark.Start("NetWorkedData");
            if (Launched == false)
            {
                Launched = true;

                //NWDToolbox.EditorAndPlaying("NWDLauncher Launch()");
                EditorByPass = false;
#if UNITY_EDITOR
                EditorApplication.quitting += Quit;
                if (EditorApplication.isPlayingOrWillChangePlaymode == false)
                {
                    //Debug.Log("EditorByPass = true");
                    EditorByPass = true;
                    if (EditorPrefs.HasKey(K_PINCODE_KEY))
                    {
                        string tPincode = EditorPrefs.GetString(K_PINCODE_KEY);
                        //Debug.Log("and tPincode found with value = '" + tPincode + "'");
                    }
                }
#endif
            }
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
        static void Quit()
        {
            // close connection?
            //TODO: close connection

            // delete editor key
#if UNITY_EDITOR
            Debug.Log("Quitting the Editor");
            if (EditorPrefs.HasKey(K_PINCODE_KEY))
            {
                EditorPrefs.DeleteKey(K_PINCODE_KEY);
            }
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        static private void LaunchNext()
        {
            //Debug.Log("LaunchNext() with state = "+ State.ToString());
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
                        // waiting ... do nothing
                    }
                    break;
                case NWDStatut.EngineLaunched:
                    {
                        ConnectToDatabaseEditor();
                    }
                    break;
                case NWDStatut.DataEditorConnecting:
                    {
                        // waiting ... do nothing
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
                        // waiting ... do nothing
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
                        // Reload all Index
                        DatabaseIndexation();
                    }
                    break;
                case NWDStatut.DataIndexationStart:
                    {
                    }
                    break;
                case NWDStatut.DataIndexationStep:
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
                        BTBBenchmark.Finish("NetWorkedData");
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void LaunchPause()
        {
            switch (State)
            {
                case NWDStatut.None:
                    {
                    }
                    break;
                case NWDStatut.EngineLaunching:
                    {
                    }
                    break;
                case NWDStatut.EngineLaunched:
                    {
                    }
                    break;
                case NWDStatut.DataEditorConnecting:
                    {
                    }
                    break;
                case NWDStatut.DataEditorConnected:
                    {
                    }
                    break;
                case NWDStatut.DataEditorTableUpdated:
                    {
                    }
                    break;
                case NWDStatut.DataEditorLoading:
                    {
                        State = NWDStatut.DataEditorTableUpdated;
                    }
                    break;
                case NWDStatut.DataEditorLoaded:
                    {
                    }
                    break;
                case NWDStatut.DataAccountConnecting:
                    {
                        State = NWDStatut.DataEditorLoaded;
                    }
                    break;
                case NWDStatut.DataAccountCodePinCreate:
                    {
                        State = NWDStatut.DataEditorLoaded;
                    }
                    break;
                case NWDStatut.DataAccountCodePinRequest:
                    {
                        State = NWDStatut.DataEditorLoaded;
                    }
                    break;
                case NWDStatut.DataAccountCodePinFail:
                    {
                        State = NWDStatut.DataEditorLoaded;
                    }
                    break;
                case NWDStatut.DataAccountCodePinStop:
                    {
                        State = NWDStatut.DataEditorLoaded;
                    }
                    break;
                case NWDStatut.DataAccountCodePinSuccess:
                    {
                        State = NWDStatut.DataEditorLoaded;
                    }
                    break;
                case NWDStatut.DataAccountConnected:
                    {
                        State = NWDStatut.DataEditorLoaded;
                    }
                    break;
                case NWDStatut.DataAccountTableUpdated:
                    {
                    }
                    break;
                case NWDStatut.DataAccountLoading:
                    {
                        State = NWDStatut.DataAccountTableUpdated;
                    }
                    break;
                case NWDStatut.DataAccountLoaded:
                    {
                    }
                    break;
                case NWDStatut.DataIndexationStart:
                    {
                    }
                    break;
                case NWDStatut.DataIndexationStep:
                    {
                    }
                    break;
                case NWDStatut.DataIndexationFinish:
                    {
                    }
                    break;
                case NWDStatut.NetWorkedDataReady:
                    {
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        static public void LaunchResume()
        {
            LaunchNext();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================