// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:40
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

#if UNITY_EDITOR
using UnityEditor;
using SQLite4Unity3d;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    public class NWDOperationWebManagement : NWDOperationWebUnity
    {
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebManagement AddOperation(string sName,
                                                              BTBOperationBlock sSuccessBlock = null,
                                                              BTBOperationBlock sFailBlock = null,
                                                              BTBOperationBlock sCancelBlock = null,
                                                              BTBOperationBlock sProgressBlock = null,
                                                              NWDAppEnvironment sEnvironment = null, bool sPriority = false)
        {
            NWDOperationWebManagement rReturn = NWDOperationWebManagement.Create(sName, sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock, sEnvironment);
            if (rReturn != null)
            {
                NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(rReturn, sPriority);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebManagement Create(string sName,
                                                        BTBOperationBlock sSuccessBlock = null,
                                                        BTBOperationBlock sFailBlock = null,
                                                        BTBOperationBlock sCancelBlock = null,
                                                        BTBOperationBlock sProgressBlock = null,
                                                        NWDAppEnvironment sEnvironment = null)
        {
            NWDOperationWebManagement rReturn = null;
            if (NWDDataManager.SharedInstance().DataLoaded() == true)
            {
                if (sName == null)
                {
                    sName = "UnNamed Web Operation Management";
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
                rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebManagement>();
                rReturn.GameObjectToSpawn = tGameObjectToSpawn;
                rReturn.Environment = sEnvironment;
                rReturn.QueueName = sEnvironment.Environment;
                rReturn.SecureData = true;
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
            return NWD.K_MANAGEMENT_FILE;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool CanRestart()
        {
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataUploadPrepare()
        {
            Data.Add("management", "create");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DataDownloadedCompute(NWDOperationResult sData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================
#endif