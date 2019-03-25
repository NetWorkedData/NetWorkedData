//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

//using BTBMiniJSON;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;

using BasicToolBox;

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
                                                                   BTBOperationBlock sSuccessBlock = null,
                                                                   BTBOperationBlock sFailBlock = null,
                                                                   BTBOperationBlock sCancelBlock = null,
                                                                BTBOperationBlock sProgressBlock = null,
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
                                                          BTBOperationBlock sSuccessBlock = null,
                                                          BTBOperationBlock sFailBlock = null,
                                                          BTBOperationBlock sCancelBlock = null,
                                                          BTBOperationBlock sProgressBlock = null,
            NWDAppEnvironment sEnvironment = null, bool sForceSync = false)
        {
            //Debug.Log("NWDOperationWebMaintenance Create()");
            NWDOperationWebMaintenance rReturn = null;
            if (NWDTypeLauncher.DataLoaded() == true)
            {
                if (sName == null)
                {
                    sName = "UnNamed Web Operation Synchronisation";
                }
                if (sEnvironment == null)
                {
                    sEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
                }
                GameObject tGameObjectToSpawn = new GameObject(sName);
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
                rReturn.SecureData = sEnvironment.AllwaysSecureData;
                rReturn.InitBlock(sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock);
            }
            else
            {
                //BTBOperation tOperation = new BTBOperation();
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
        public override string ServerBase()
        {
            string tFolderWebService = NWDAppConfiguration.SharedInstance().WebServiceFolder();
            string rURL = Environment.ServerHTTPS.TrimEnd('/') + "/" + tFolderWebService + "/" + ServerFile();
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