//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:38
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
using SQLite4Unity3d;
//using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDOperationWebBlank : NWDOperationWebUnity
    {
        //-------------------------------------------------------------------------------------------------------------
        public List<Type> TypeList = new List<Type>();
        public string Action;
        public bool ForceSync = false;
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebBlank AddOperation(string sName,
                                                                   NWEOperationBlock sSuccessBlock = null,
                                                                   NWEOperationBlock sFailBlock = null,
                                                                   NWEOperationBlock sCancelBlock = null,
                                                                NWEOperationBlock sProgressBlock = null,
                                                                NWDAppEnvironment sEnvironment = null,
                                                                bool sForceSync = false,
                                                                bool sPriority = false)
        {
            //Debug.Log("NWDOperationWebBlank AddOperation()");
            NWDOperationWebBlank rReturn = NWDOperationWebBlank.Create(sName, sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock, sEnvironment, sForceSync);
            if (rReturn != null)
            {
                NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(rReturn, sPriority);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebBlank Create(string sName,
                                                             NWEOperationBlock sSuccessBlock = null,
                                                             NWEOperationBlock sFailBlock = null,
                                                             NWEOperationBlock sCancelBlock = null,
                                                          NWEOperationBlock sProgressBlock = null,
            NWDAppEnvironment sEnvironment = null, bool sForceSync = false)
        {
            //Debug.Log("NWDOperationWebBlank Create()");
            NWDOperationWebBlank rReturn = null;
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
                GameObject tGameObjectToSpawn = new GameObject(NWDToolbox.RandomStringUnix(16)+sName);
#if UNITY_EDITOR
                tGameObjectToSpawn.hideFlags = HideFlags.HideAndDontSave;
#else
            tGameObjectToSpawn.transform.SetParent(NWDGameDataManager.UnitySingleton().transform);
#endif
                rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebBlank>();
                rReturn.GameObjectToSpawn = tGameObjectToSpawn;
                rReturn.Environment = sEnvironment;
                rReturn.QueueName = sEnvironment.Environment;
                rReturn.ForceSync = sForceSync;
                rReturn.SecureData = sEnvironment.AllwaysSecureData;
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
            //Debug.Log("NWDOperationWebBlank ServerFile()");
            return NWD.K_BLANK_PHP;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool CanRestart()
        {
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataUploadPrepare()
        {
            //Debug.Log("NWDOperationWebBlank DataUploadPrepare()");
            //         Dictionary<string, object> tData = NWDDataManager.SharedInstance().SynchronizationPushClassesDatas (ResultInfos, Environment, ForceSync, TypeList, NWDOperationSpecial.None);
            //         tData.Add ("action", Action);
            //Data = tData;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataDownloadedCompute(NWDOperationResult sData)
        {
            //Debug.Log("NWDOperationWebBlank DataDownloadedCompute()");
            // I put null for pull typeList to analyze all NWDClass
            //NWDDataManager.SharedInstance().SynchronizationPullClassesDatas (ResultInfos, Environment, sData, null, NWDOperationSpecial.None);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================