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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
	// Example of monobehaviour component
	// This class example can be use to simple connect gameobject with NWDItem Data
	// You can use this class to connect prefab, gameobject , etc.
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDMonoBehaviour : MonoBehaviour
	{
		//-------------------------------------------------------------------------------------------------------------
		public object NetWorkedData;
		public string Type;
		public string Reference;
		//-------------------------------------------------------------------------------------------------------------
		public static NWDMonoBehaviour SetNetWorkedDataObject (GameObject sGameObject, object sObject)
		{
			NWDMonoBehaviour tMonoBehavior = sGameObject.GetComponent<NWDMonoBehaviour> () as NWDMonoBehaviour;
			if (tMonoBehavior == null) {
				tMonoBehavior = sGameObject.AddComponent<NWDMonoBehaviour> ();
			}
			return tMonoBehavior;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static object GetNetWorkedDataObject (GameObject sGameObject)
		{
			object rReturn = null;
			NWDMonoBehaviour tMonoBehavior = sGameObject.GetComponent<NWDMonoBehaviour> () as NWDMonoBehaviour;
			if (tMonoBehavior != null) {
				rReturn = tMonoBehavior.NetWorkedData;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
