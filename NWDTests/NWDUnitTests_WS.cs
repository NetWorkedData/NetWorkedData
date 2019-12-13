//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:24:53
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using NetWorkedData;
using UnityEngine;

using SQLite4Unity3d;
//using BasicToolBox;


#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUnitTests
    {
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_INCLUDE_TESTS
        //-------------------------------------------------------------------------------------------------------------
        static bool Installed = false;
        static private NWDUnitTests kSharedInstance;
        //-------------------------------------------------------------------------------------------------------------
        static Dictionary<string, bool> kSyncFinished = new Dictionary<string, bool>();
        static Dictionary<string, bool> kSyncError = new Dictionary<string, bool>();
        static bool LogMode = false;
        static bool LogInFileMode = false;
        //-------------------------------------------------------------------------------------------------------------
        static bool OverrideLogMode = true;
        static bool OverrideLogInFileMode = true;
        //-------------------------------------------------------------------------------------------------------------
        public static NWEOperationBlock kSuccess;
        public static NWEOperationBlock kFailBlock;
        public static NWEOperationBlock kCancelBlock;
        public static NWEOperationBlock kProgressBlock;
        //-------------------------------------------------------------------------------------------------------------
        static NWDUnitTests()
        {
            Install();
            kSharedInstance = new NWDUnitTests();
        }
        //-------------------------------------------------------------------------------------------------------------
        ~NWDUnitTests()
        {
            Uninstall();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Uninstall()
        {
            if (Installed == true)
            {
                Debug.Log("NWDUnitTests Uninstall");
                NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
                tEnvironment.LogMode = LogMode;
                tEnvironment.LogInFileMode = LogInFileMode;
                Installed = false;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void Install()
        {
            if (Installed == false)
            {
                Debug.Log("NWDUnitTests Install");
                Installed = true;

                NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
                LogMode = tEnvironment.LogMode;
                LogInFileMode = tEnvironment.LogInFileMode;

                tEnvironment.LogMode = OverrideLogMode;
                tEnvironment.LogInFileMode = OverrideLogInFileMode;

                kSuccess = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
                {
                    Debug.Log("NWDSyncTest Success");
                    kSyncFinished[bOperation.name] = true;
                };
                kFailBlock = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
                {
                    NWDOperationResult tResult = (NWDOperationResult)bResult;

                    Debug.LogWarning("TNWDSyncTest FailBlock error :" + tResult.errorCode);
                    kSyncFinished[bOperation.name] = true;
                    kSyncError[bOperation.name] = true;
                };
                kCancelBlock = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
                {
                    Debug.Log("TNWDSyncTest CancelBlock");
                    kSyncFinished[bOperation.name] = true;
                };
                kProgressBlock = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
                {
                    Debug.Log("TNWDSyncTest ProgressBlock " + bProgress.ToString() + "");
                };
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool NewWebService(string tOperationName)
        {
            bool rReturn = false;
            if (kSyncFinished.ContainsKey(tOperationName) == false)
            {
                rReturn = true;
                kSyncFinished.Add(tOperationName, false);
                kSyncError.Add(tOperationName, false);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool WebServiceIsFinished(string tOperationName)
        {
            return kSyncFinished[tOperationName];
        }
        //-------------------------------------------------------------------------------------------------------------
        public static bool WebServiceIsRunning(string tOperationName)
        {
            return !kSyncFinished[tOperationName];
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================