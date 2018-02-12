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
    public class NWDOperationWebUserNickname : NWDOperationWebUnity
	{
		//-------------------------------------------------------------------------------------------------------------
        public List<Type> TypeList = new List<Type>();
        public NWDUserNickname UserNicknameReference;
        public string Nickname;
        public string Action;
        public bool ForceSync = false;
		//-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebUserNickname AddOperation (string sName,
		                                                           BTBOperationBlock sSuccessBlock = null, 
		                                                           BTBOperationBlock sFailBlock = null, 
		                                                           BTBOperationBlock sCancelBlock = null,
		                                                           BTBOperationBlock sProgressBlock = null, 
		                                                           NWDAppEnvironment sEnvironment = null,
                                                                bool sForceSync = false, 
                                                                bool sPriority = false)
        {
            Debug.Log("NWDOperationWebUserInfos AddOperation()");
            NWDOperationWebUserNickname rReturn = NWDOperationWebUserNickname.Create (sName, sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock, sEnvironment, sForceSync);
			NWDDataManager.SharedInstance.WebOperationQueue.AddOperation (rReturn, sPriority);
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebUserNickname Create (string sName,
		                                                     BTBOperationBlock sSuccessBlock = null, 
		                                                     BTBOperationBlock sFailBlock = null,
		                                                     BTBOperationBlock sCancelBlock = null,
		                                                     BTBOperationBlock sProgressBlock = null,
			NWDAppEnvironment sEnvironment = null,bool sForceSync = false)
        {
            Debug.Log("NWDOperationWebUserNickname Create()");
            NWDOperationWebUserNickname rReturn = null;
			if (sName == null) {
				sName = "UnNamed Web Operation Synchronisation";
			}
			if (sEnvironment == null) {
				sEnvironment = NWDAppConfiguration.SharedInstance.SelectedEnvironment ();
			}

			// IF BTBOperationUnity
			GameObject tGameObjectToSpawn = new GameObject (sName);
            rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebUserNickname> ();
			rReturn.GameObjectToSpawn = tGameObjectToSpawn;

			rReturn.Environment = sEnvironment;
			rReturn.QueueName = sEnvironment.Environment;
            rReturn.TypeList.Add(NWDUserNickname.ClassType());
			rReturn.ForceSync = sForceSync;
			rReturn.InitBlock (sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock);

			#if UNITY_EDITOR
			#else
			DontDestroyOnLoad (tGameObjectToSpawn);
			#endif

			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override string ServerFile ()
        {
            Debug.Log("NWDOperationWebUserNickname ServerFile()");
            return "usernickname.php";
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void DataUploadPrepare ()
        {
            Debug.Log("NWDOperationWebUserNickname DataUploadPrepare()");
            Dictionary<string, object> tData = NWDDataManager.SharedInstance.SynchronizationPushClassesDatas (Environment, ForceSync, TypeList, false);
            tData.Add ("action", Action);
            tData.Add("nickname", Nickname);
            if (UserNicknameReference != null)
            {
                tData.Add("reference", UserNicknameReference.Reference);
            }
			Data = tData;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override void DataDownloadedCompute (NWDOperationResult sData)
		{
            Debug.Log("NWDOperationWebUserNickname DataDownloadedCompute()");
            NWDDataManager.SharedInstance.SynchronizationPullClassesDatas (Environment, sData, TypeList);
		}
        //-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================