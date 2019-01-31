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
    public enum NWDOperationSpecial
    {
        None = 0,
        Special = 1,
        Upgrade = 2,
        Optimize = 3,
        Clean = 4,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDOperationWebSynchronisation : NWDOperationWebUnity
	{
		//-------------------------------------------------------------------------------------------------------------
		public List<Type> TypeList;
		public bool ForceSync = false;
		//public bool FlushTrash = false;
        public NWDOperationSpecial Special = NWDOperationSpecial.None;
        //-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebSynchronisation AddOperation (string sName,
		                                                           BTBOperationBlock sSuccessBlock = null, 
		                                                           BTBOperationBlock sFailBlock = null, 
		                                                           BTBOperationBlock sCancelBlock = null,
		                                                           BTBOperationBlock sProgressBlock = null, 
		                                                           NWDAppEnvironment sEnvironment = null,
			                                                       List<Type> sTypeList = null,
                                                                   bool sForceSync = false,
                                                                   bool sPriority = false,
                                                                   NWDOperationSpecial sSpecial = NWDOperationSpecial.None)
		{
			NWDOperationWebSynchronisation rReturn = Create (sName, sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock, sEnvironment, sTypeList, sForceSync, sSpecial);
			NWDDataManager.SharedInstance().WebOperationQueue.AddOperation (rReturn, sPriority);

			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		static public NWDOperationWebSynchronisation Create (string sName,
		                                                     BTBOperationBlock sSuccessBlock = null, 
		                                                     BTBOperationBlock sFailBlock = null,
		                                                     BTBOperationBlock sCancelBlock = null,
		                                                     BTBOperationBlock sProgressBlock = null,
			                                                 NWDAppEnvironment sEnvironment = null,
                                                             List<Type> sTypeList = null,
                                                             bool sForceSync = false,
                                                             NWDOperationSpecial sSpecial = NWDOperationSpecial.None)
		{
			NWDOperationWebSynchronisation rReturn = null;
			if (sName == null) {
				sName = "UnNamed Web Operation Synchronisation";
			}

			if (sEnvironment == null) {
				sEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment ();
			}

            GameObject tGameObjectToSpawn = new GameObject(sName);
            #if UNITY_EDITOR
            tGameObjectToSpawn.hideFlags = HideFlags.HideAndDontSave;
            #else
            tGameObjectToSpawn.transform.SetParent(NWDGameDataManager.UnitySingleton().transform);
            #endif
            rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebSynchronisation> ();
            rReturn.GameObjectToSpawn = tGameObjectToSpawn;
			rReturn.Environment = sEnvironment;
			rReturn.QueueName = sEnvironment.Environment;
			rReturn.TypeList = sTypeList;
			rReturn.ForceSync = sForceSync;
            rReturn.Special = sSpecial;
            rReturn.SecureData = sEnvironment.AllwaysSecureData;
            foreach (Type tType in sTypeList)
            {
                if (tType.GetCustomAttributes(typeof(NWDForceSecureDataAttribute), true).Length >0){
                    rReturn.SecureData = true;
                    break;
                }
            }
            rReturn.InitBlock (sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock);

            return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override string ServerFile ()
		{
			return NWD.K_WS_FILE;
        }
		//-------------------------------------------------------------------------------------------------------------
		public override void DataUploadPrepare ()
		{
            Dictionary<string, object> tData = NWDDataManager.SharedInstance().SynchronizationPushClassesDatas (ResultInfos, Environment, ForceSync, TypeList, Special);
			tData.Add ("action", "sync");
			Data = tData;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override void DataDownloadedCompute (NWDOperationResult sData)
		{
            NWDDataManager.SharedInstance().SynchronizationPullClassesDatas (ResultInfos, Environment, sData, TypeList);
		}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================