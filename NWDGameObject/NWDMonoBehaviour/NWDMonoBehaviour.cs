//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:45:56
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

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