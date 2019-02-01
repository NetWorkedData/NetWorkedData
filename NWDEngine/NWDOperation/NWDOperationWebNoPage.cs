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
    public class NWDOperationWebNoPage : NWDOperationWebUnity
	{
		//-------------------------------------------------------------------------------------------------------------
        public List<Type> TypeList = new List<Type>();
        public string Action;
		public bool ForceSync = false;
		//-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebNoPage AddOperation (string sName,
		                                                           BTBOperationBlock sSuccessBlock = null, 
		                                                           BTBOperationBlock sFailBlock = null, 
		                                                           BTBOperationBlock sCancelBlock = null,
                                                                BTBOperationBlock sProgressBlock = null,
                                                                NWDAppEnvironment sEnvironment = null,
                                                                bool sForceSync = false, 
                                                                bool sPriority = false)
        {
            Debug.Log("NWDOperationWebNoPage AddOperation()");
            NWDOperationWebNoPage rReturn = NWDOperationWebNoPage.Create (sName, sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock, sEnvironment, sForceSync);
			NWDDataManager.SharedInstance().WebOperationQueue.AddOperation (rReturn, sPriority);
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebNoPage Create (string sName,
		                                                     BTBOperationBlock sSuccessBlock = null, 
		                                                     BTBOperationBlock sFailBlock = null,
		                                                     BTBOperationBlock sCancelBlock = null,
                                                          BTBOperationBlock sProgressBlock = null,
			NWDAppEnvironment sEnvironment = null,bool sForceSync = false)
        {
            //Debug.Log("NWDOperationWebNoPage Create()");
            NWDOperationWebNoPage rReturn = null;
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
            rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebNoPage> ();
			rReturn.GameObjectToSpawn = tGameObjectToSpawn;
			rReturn.Environment = sEnvironment;
			rReturn.QueueName = sEnvironment.Environment;
			rReturn.ForceSync = sForceSync;
            rReturn.SecureData = sEnvironment.AllwaysSecureData;
            rReturn.InitBlock (sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock);
            return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override string ServerFile ()
        {
            //Debug.Log("NWDOperationWebNoPage ServerFile()");
			return NWD.K_NO_PAGE_PHP;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void DataUploadPrepare ()
        {
            //Debug.Log("NWDOperationWebNoPage DataUploadPrepare()");
            Dictionary<string, object> tData = NWDDataManager.SharedInstance().SynchronizationPushClassesDatas (ResultInfos, Environment, ForceSync, TypeList, NWDOperationSpecial.None);
            tData.Add ("action", Action);
			Data = tData;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override void DataDownloadedCompute (NWDOperationResult sData)
		{
            //Debug.Log("NWDOperationWebNoPage DataDownloadedCompute()");
            // I put null for pull typeList to analyze all NWDClass
            NWDDataManager.SharedInstance().SynchronizationPullClassesDatas (ResultInfos, Environment, sData, null, NWDOperationSpecial.None);
		}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================