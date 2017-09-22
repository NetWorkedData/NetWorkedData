//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
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
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDBasis <K> : NWDTypeClass where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		public void AddNetWorkedDataToObject (GameObject sGameObject)
		{
			SetNetWorkedDataObject (sGameObject, this);
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void SetNetWorkedDataObject (GameObject sGameObject, NWDBasis <K> sObject)
		{
			NWDMonoBehaviour tNWDMonoBehaviour = NWDMonoBehaviour.SetNetWorkedDataObject (sGameObject, sObject);
			tNWDMonoBehaviour.Type = sObject.GetType ().ToString();
			tNWDMonoBehaviour.Reference = sObject.Reference;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDBasis <K> GetNetWorkedDataObject (GameObject sGameObject)
		{
			object rReturn = NWDMonoBehaviour.GetNetWorkedDataObject (sGameObject);
			if (rReturn.GetType () != typeof(K)) {
				rReturn = null;
			}
			return rReturn as NWDBasis <K>;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================