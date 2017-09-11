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
using System.Reflection;
using System.IO;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	[SerializeField]
	public class NWDAssetType : NWDUnityType
	{
		//-------------------------------------------------------------------------------------------------------------
		public static string kAssetDelimiter = "**";
		// TODO: must protect asset path by a symbol start and symbol end!
		//-------------------------------------------------------------------------------------------------------------
		public bool ChangeAssetPath (string sOldPath, string sNewPath) {
			//BTBDebug.Log ("BTBDataType ChangeAssetPath " + sOldPath + " to " + sNewPath + " in Value = " + Value);
			bool rChange = false;
			if (Value.Contains (sOldPath)) {
				Value = Value.Replace (kAssetDelimiter+sOldPath+kAssetDelimiter, kAssetDelimiter+sNewPath+kAssetDelimiter);
				rChange = true;
				//BTBDebug.Log ("BTBDataType ChangeAssetPath YES I DID", BTBDebugResult.Success);
			}
			return rChange;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================