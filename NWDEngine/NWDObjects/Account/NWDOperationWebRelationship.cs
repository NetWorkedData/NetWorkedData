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
 //   public class NWDOperationWebRelationship : NWDOperationWebUnity
	//{
		////-------------------------------------------------------------------------------------------------------------
  //      public List<Type> TypeList = new List<Type>();
  //      public string Action;
  //      public string Classes;
  //      public NWDRelationship Relationship;
  //      public string PinCode;
  //      public string Nickname;
  //      public string NicknameID;
		//public bool ForceSync = false;
  //      public int PinSize = 6;
  //      public int PinDelay = 60;
		////public bool FlushTrash = false;
  //      public bool Bilateral = false;
		////-------------------------------------------------------------------------------------------------------------
  //      static public NWDOperationWebRelationship AddOperation (string sName,
		//                                                           BTBOperationBlock sSuccessBlock = null, 
		//                                                           BTBOperationBlock sFailBlock = null, 
		//                                                           BTBOperationBlock sCancelBlock = null,
  //                                                              BTBOperationBlock sProgressBlock = null, 
  //                                               List<Type> sAdditionalTypes = null,
  //                                                              NWDAppEnvironment sEnvironment = null,
  //                                                              bool sForceSync = false, 
  //                                                              bool sPriority = false)
  //      {
  //          //Debug.Log("NWDOperationWebRelationship AddOperation()");
  //          NWDOperationWebRelationship rReturn = NWDOperationWebRelationship.Create (sName, sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock, sAdditionalTypes, sEnvironment, sForceSync);
		//	NWDDataManager.SharedInstance().WebOperationQueue.AddOperation (rReturn, sPriority);
		//	return rReturn;
		//}
		////-------------------------------------------------------------------------------------------------------------
  //      static public NWDOperationWebRelationship Create (string sName,
		//                                                     BTBOperationBlock sSuccessBlock = null, 
		//                                                     BTBOperationBlock sFailBlock = null,
		//                                                     BTBOperationBlock sCancelBlock = null,
  //                                                        BTBOperationBlock sProgressBlock = null,
  //                                               List<Type> sAdditionalTypes = null,
		//	NWDAppEnvironment sEnvironment = null,bool sForceSync = false)
  //      {
  //          //Debug.Log("NWDOperationWebRelationship Create()");
  //          NWDOperationWebRelationship rReturn = null;
		//	if (sName == null) {
		//		sName = "UnNamed Web Operation Synchronisation";
		//	}
		//	if (sEnvironment == null) {
		//		sEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment ();
		//	}

		//	// IF BTBOperationUnity
		//	GameObject tGameObjectToSpawn = new GameObject (sName);
  //          rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebRelationship> ();
		//	rReturn.GameObjectToSpawn = tGameObjectToSpawn;

		//	rReturn.Environment = sEnvironment;
		//	rReturn.QueueName = sEnvironment.Environment;
  //          // add class to sync.
  //          rReturn.TypeList.Add(NWDRelationship.ClassType());
  //          if (sAdditionalTypes != null)
  //          {
  //              rReturn.TypeList.AddRange(sAdditionalTypes);
  //          }
		//	rReturn.ForceSync = sForceSync;
		//	rReturn.InitBlock (sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock);

		//	#if UNITY_EDITOR
		//	#else
		//	//DontDestroyOnLoad (tGameObjectToSpawn);
		//	#endif

		//	return rReturn;
		//}
		////-------------------------------------------------------------------------------------------------------------
		//public override string ServerFile ()
  //      {
  //          //Debug.Log("NWDOperationWebRelationship ServerFile()");
		//	return "relationship.php";
		//}
		////-------------------------------------------------------------------------------------------------------------
		//public override void DataUploadPrepare ()
  //      {
  //          //Debug.Log("NWDOperationWebRelationship DataUploadPrepare()");
  //          Dictionary<string, object> tData = NWDDataManager.SharedInstance().SynchronizationPushClassesDatas (ResultInfos, Environment, ForceSync, TypeList, NWDOperationSpecial.None);
  //          tData.Add ("action", Action);
  //          if (Relationship != null)
  //          {
  //              tData.Add("reference", Relationship.Reference);
  //          }
  //          tData.Add("pincode", PinCode);
  //          tData.Add("nickname", Nickname);
  //          tData.Add("nicknameid", NicknameID);
  //          tData.Add("pinsize",PinSize);
  //          tData.Add("pindelay", PinDelay);
  //          tData.Add("classes", Classes);
  //          tData.Add("bilateral", Bilateral.ToString());
		//	Data = tData;
		//}
		////-------------------------------------------------------------------------------------------------------------
  //      public override void DataDownloadedCompute (NWDOperationResult sData)
		//{
  //          //Debug.Log("NWDOperationWebRelationship DataDownloadedCompute()");
  //          // I put null for pull typeList to analyze all NWDClass
  //          NWDDataManager.SharedInstance().SynchronizationPullClassesDatas (ResultInfos, Environment, sData, null, NWDOperationSpecial.None);
		//}
    //    //-------------------------------------------------------------------------------------------------------------
    //}
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================