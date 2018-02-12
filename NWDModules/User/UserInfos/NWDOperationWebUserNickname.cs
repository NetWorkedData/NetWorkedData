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
    public class NWDOperationWebUserInfos : NWDOperationWebUnity
	{
		//-------------------------------------------------------------------------------------------------------------
        public List<Type> TypeList = new List<Type>();
        public NWDUserNickname UserNicknameReference;
        public string Nickname;
        public string Action;
        public bool ForceSync = false;
		//-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebUserInfos AddOperation (string sName,
		                                                           BTBOperationBlock sSuccessBlock = null, 
		                                                           BTBOperationBlock sFailBlock = null, 
		                                                           BTBOperationBlock sCancelBlock = null,
		                                                           BTBOperationBlock sProgressBlock = null, 
		                                                           NWDAppEnvironment sEnvironment = null,
                                                                bool sForceSync = false, 
                                                                bool sPriority = false)
        {
            Debug.Log("NWDOperationWebUserInfos AddOperation()");
            NWDOperationWebUserInfos rReturn = NWDOperationWebUserInfos.Create (sName, sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock, sEnvironment, sForceSync);
			NWDDataManager.SharedInstance.WebOperationQueue.AddOperation (rReturn, sPriority);
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
        static public NWDOperationWebUserInfos Create (string sName,
		                                                     BTBOperationBlock sSuccessBlock = null, 
		                                                     BTBOperationBlock sFailBlock = null,
		                                                     BTBOperationBlock sCancelBlock = null,
		                                                     BTBOperationBlock sProgressBlock = null,
			NWDAppEnvironment sEnvironment = null,bool sForceSync = false)
        {
            Debug.Log("NWDOperationWebUserInfos Create()");
            NWDOperationWebUserInfos rReturn = null;
			if (sName == null) {
				sName = "UnNamed Web Operation Synchronisation";
			}
			if (sEnvironment == null) {
				sEnvironment = NWDAppConfiguration.SharedInstance.SelectedEnvironment ();
			}

			// IF BTBOperationUnity
			GameObject tGameObjectToSpawn = new GameObject (sName);
            rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebUserInfos> ();
			rReturn.GameObjectToSpawn = tGameObjectToSpawn;

			rReturn.Environment = sEnvironment;
			rReturn.QueueName = sEnvironment.Environment;
            rReturn.TypeList.Add(NWDRelationship.ClassType());
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
            Debug.Log("NWDOperationWebUserInfos ServerFile()");
            return "usernickname.php";
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void DataUploadPrepare ()
        {
            Debug.Log("NWDOperationWebUserInfos DataUploadPrepare()");
            Dictionary<string, object> tData = NWDDataManager.SharedInstance.SynchronizationPushClassesDatas (Environment, ForceSync, TypeList, false);
            tData.Add ("action", Action);
            tData.Add("nickname", Nickname);
            if (UserNicknameReference != null)
            {
                tData.Add("reference", UserNicknameReference.Reference);
            }
            //tData.Add("bilateral", Bilateral);
			Data = tData;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override void DataDownloadedCompute (NWDOperationResult sData)
		{
            Debug.Log("NWDOperationWebUserInfos DataDownloadedCompute()");
            NWDDataManager.SharedInstance.SynchronizationPullClassesDatas (Environment, sData, TypeList);
		}
        //-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================