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
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

//using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDOperationWebMaintenance : NWDOperationWebUnity
    {
        //-------------------------------------------------------------------------------------------------------------
        public List<Type> TypeList = new List<Type>();
        public string Action;
        public bool ForceSync = false;
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebMaintenance AddOperation(string sName,
                                                                   NWEOperationBlock sSuccessBlock = null,
                                                                   NWEOperationBlock sFailBlock = null,
                                                                   NWEOperationBlock sCancelBlock = null,
                                                                NWEOperationBlock sProgressBlock = null,
                                                                NWDAppEnvironment sEnvironment = null,
                                                                bool sForceSync = false,
                                                                bool sPriority = false)
        {
            Debug.Log("NWDOperationWebMaintenance AddOperation()");
            NWDOperationWebMaintenance rReturn = NWDOperationWebMaintenance.Create(sName, sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock, sEnvironment, sForceSync);
            if (rReturn != null)
            {
                NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(rReturn, sPriority);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebMaintenance Create(string sName,
                                                          NWEOperationBlock sSuccessBlock = null,
                                                          NWEOperationBlock sFailBlock = null,
                                                          NWEOperationBlock sCancelBlock = null,
                                                          NWEOperationBlock sProgressBlock = null,
            NWDAppEnvironment sEnvironment = null, bool sForceSync = false)
        {
            //Debug.Log("NWDOperationWebMaintenance Create()");
            NWDOperationWebMaintenance rReturn = null;
            if (NWDDataManager.SharedInstance().DataLoaded() == true)
            {
                if (sName == null)
                {
                    sName = "UnNamed Web Operation Synchronisation";
                }
                if (sEnvironment == null)
                {
                    sEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
                }
                GameObject tGameObjectToSpawn = new GameObject(NWDToolbox.RandomStringUnix(16) + sName);
#if UNITY_EDITOR
                tGameObjectToSpawn.hideFlags = HideFlags.HideAndDontSave;
#else
                tGameObjectToSpawn.transform.SetParent(NWDGameDataManager.UnitySingleton().transform);
#endif
                rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebMaintenance>();
                rReturn.GameObjectToSpawn = tGameObjectToSpawn;
                rReturn.Environment = sEnvironment;
                rReturn.QueueName = sEnvironment.Environment;
                rReturn.ForceSync = sForceSync;
                rReturn.SecureData = sEnvironment.AlwaysSecureData;
                rReturn.InitBlock(sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock);
            }
            else
            {
                //NWEOperation tOperation = new NWEOperation();
                //NWDOperationResult tResult = new NWDOperationResult();
                //tOperation.QueueName = NWDAppEnvironment.SelectedEnvironment().Environment;
                //sFailBlock(tOperation, 1.0F, tResult);
                sFailBlock(null, 1.0F, null);
                Debug.LogWarning("SYNC NEED TO OPEN ALL ACCOUNT TABLES AND LOADED ALL DATAS!");
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string ServerFile()
        {
            //Debug.Log("NWDOperationWebMaintenance ServerFile()");
            return NWD.K_MAINTENANCE_PHP;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool CanRestart()
        {
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string ServerBase()
        {
            string tFolderWebService = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            string rURL = Environment.GetServerHTTPS() + "/" + tFolderWebService + "/" + ServerFile();
            return rURL;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataUploadPrepare()
        {
            //Debug.Log("NWDOperationWebMaintenance DataUploadPrepare()");
            //         Dictionary<string, object> tData = NWDDataManager.SharedInstance().SynchronizationPushClassesDatas (ResultInfos, Environment, ForceSync, TypeList, NWDOperationSpecial.None);
            //         tData.Add ("action", Action);
            //Data = tData;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataDownloadedCompute(NWDOperationResult sData)
        {
            //Debug.Log("NWDOperationWebMaintenance DataDownloadedCompute()");
            // I put null for pull typeList to analyze all NWDClass
            //NWDDataManager.SharedInstance().SynchronizationPullClassesDatas (ResultInfos, Environment, sData, null, NWDOperationSpecial.None);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
