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

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

using SQLite4Unity3d;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		public static void ChangeAssetPath (string sOldPath, string sNewPath) {
			//BTBDebug.Log (ClassName () +" ChangeAssetPath " + sOldPath + " to " + sNewPath);
			foreach (NWDBasis<K> tObject in NWDBasis<K>.ObjectsList) {
				tObject.ChangeAssetPathMe(sOldPath, sNewPath);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		// TODO: create an ovveride only for editor
		public virtual void ChangeAssetPathMe (string sOldPath, string sNewPath)
		{
			//BTBDebug.Log (ClassName () +" ChangeAssetPathMe " + sOldPath + " to " + sNewPath);
			if (TestIntegrity () == true) {
				// TODO: Change Preview Type by NWDGameObjectType
				bool tUpdate = false;
				if (Preview != null) {
					if (Preview.Contains (sOldPath)) {
						Preview = Preview.Replace (sOldPath, sNewPath);
						tUpdate = true;
						//BTBDebug.Log ("Preview ChangeAssetPath YES I DID", BTBDebugResult.Success);
					}
				}
				Type tType = ClassType ();
				foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
					Type tTypeOfThis = tProp.PropertyType;
					if (tTypeOfThis.IsSubclassOf (typeof(NWDUnityType))) {
						NWDUnityType tValueStruct = (NWDUnityType)tProp.GetValue (this, null);
						if (tValueStruct != null) {
							if (tValueStruct.ChangeAssetPath (sOldPath, sNewPath)) {
								tUpdate = true;
							}
						}
					}
				}
				if (tUpdate == true) {
					UpdateMeLater ();
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
#endif