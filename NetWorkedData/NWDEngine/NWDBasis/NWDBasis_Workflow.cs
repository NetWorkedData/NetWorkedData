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

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial  class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------

		#region Class Methods

		//-------------------------------------------------------------------------------------------------------------
		public static K NewObject()
		{
			K rReturn = NWDBasis <K>.NewInstance () as K;
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static K[] GetAllObjects()
		{
			List<K> rReturn = new List<K>();
			foreach (K tObject in NWDBasis<K>.ObjectsList)
			{
				if (tObject.IsVisibleForAccount())
				{
					rReturn.Add(tObject);
				}
			}
			return rReturn.ToArray();
		}
		//-------------------------------------------------------------------------------------------------------------
		public static K GetObjectWithReference(string sReference)
		{
			K rReturn = null;
			foreach (K tObject in GetAllObjects()) {
				if (tObject.Reference == sReference) {
					rReturn = tObject;
					break;
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static K[] GetObjectsWithReferences(string[] sReferences)
		{
			List<K> rReturn = new List<K>();
			foreach (K tObject in GetAllObjects()) {
				if (sReferences.Contains(tObject.Reference)) {
					rReturn.Add(tObject);
				}
			}
			return rReturn.ToArray();
		}
		//-------------------------------------------------------------------------------------------------------------
		public static K GetObjectWithInternalKey(string sInternalKey)
		{
			K rReturn = null;
			foreach (K tObject in GetAllObjects()) {
				if (tObject.InternalKey == sInternalKey) {
					rReturn = tObject;
					break;
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static K[] GetObjectsWithInternalKeys(string[] sInternalKeys)
		{
			List<K> rReturn = new List<K>();
			foreach (K tObject in GetAllObjects()) {
				if (sInternalKeys.Contains(tObject.InternalKey)) {
					rReturn.Add(tObject);
				}
			}
			return rReturn.ToArray();
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion

		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================