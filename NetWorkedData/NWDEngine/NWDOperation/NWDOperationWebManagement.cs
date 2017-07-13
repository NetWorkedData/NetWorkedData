using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

using MiniJSON;

#if UNITY_EDITOR
using UnityEditor;

using SQLite4Unity3d;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
	public class NWDOperationWebManagement : NWDOperationWebUnity
	{
		static public NWDOperationWebManagement AddOperation (string sName,
		                                                      BTBOperationBlock sSuccessBlock = null, 
		                                                      BTBOperationBlock sFailBlock = null, 
		                                                      BTBOperationBlock sCancelBlock = null,
		                                                      BTBOperationBlock sProgressBlock = null, 
		                                                      NWDAppEnvironment sEnvironment = null, bool sPriority = false)
		{
			NWDOperationWebManagement rReturn = NWDOperationWebManagement.Create (sName, sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock, sEnvironment);
			NWDDataManager.SharedInstance.WebOperationQueue.AddOperation (rReturn, sPriority);
			return rReturn;
		}

		static public NWDOperationWebManagement Create (string sName,
		                                                BTBOperationBlock sSuccessBlock = null, 
		                                                BTBOperationBlock sFailBlock = null,
		                                                BTBOperationBlock sCancelBlock = null,
		                                                BTBOperationBlock sProgressBlock = null,
		                                                NWDAppEnvironment sEnvironment = null)
		{
			NWDOperationWebManagement rReturn = null;
			if (sName == null) {
				sName = "UnNamed Web Operation Management";
			}
			if (sEnvironment == null) {
				sEnvironment = NWDAppConfiguration.SharedInstance.SelectedEnvironment ();
			}

			// IF BTBOperationUnity
			GameObject tGameObjectToSpawn = new GameObject (sName);
			rReturn = tGameObjectToSpawn.AddComponent<NWDOperationWebManagement> ();
			rReturn.GameObjectToSpawn = tGameObjectToSpawn;
			rReturn.Environment = sEnvironment;
			rReturn.QueueName = sEnvironment.Environment;
			rReturn.InitBlock (sSuccessBlock, sFailBlock, sCancelBlock, sProgressBlock);

			#if UNITY_EDITOR
			#else
			DontDestroyOnLoad (tGameObjectToSpawn);
			#endif
			// ELSE IF BTBOperationWWW
//			rReturn = new BTBOperationSynchronisation();
			// END
			return rReturn;
		}

		public override string ServerFile ()
		{
			return "management.php";
		}

		public override void DataUploadPrepare ()
		{
			Data.Add ("management", "create");
		}

		public override void DataDownloadedCompute (Dictionary<string, object> sData)
		{
		}

	}
}
//=====================================================================================================================
#endif