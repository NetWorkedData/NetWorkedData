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
using System.Linq.Expressions;

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
				if (tObject.IsReacheableByAccount())
				{
					rReturn.Add(tObject);
				}
			}
			return rReturn.ToArray();
		}
		//-------------------------------------------------------------------------------------------------------------
		public static K GetObjectByReference(string sReference)
		{
			K rReturn = null;
			int tIndex = ObjectsByReferenceList.IndexOf(sReference);
			if (tIndex >= 0) {
				K tObject = ObjectsList.ElementAt (tIndex) as K;
				if (tObject.IsReacheableByAccount ()) {
					rReturn = tObject;
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static K[] GetObjectsByReferences(string[] sReferences)
		{
			List<K> rReturn = new List<K>();
			foreach (string tReference in sReferences) {
				K tObject = GetObjectByReference(tReference);
				if (tObject!=null)
				{
					rReturn.Add(tObject);
				}
			}
			return rReturn.ToArray();
		}
		//-------------------------------------------------------------------------------------------------------------
		public static K GetObjectByInternalKey(string sInternalKey)
		{
			K rReturn = null;
			int tIndex = ObjectsByKeyList.IndexOf(sInternalKey);
			if (tIndex >= 0) {
				K tObject = ObjectsList.ElementAt (tIndex) as K;
				if (tObject.IsReacheableByAccount ()) {
					rReturn = tObject;
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		//TODO : GetAllObjectByInternalKey(string sInternalKey)
		//-------------------------------------------------------------------------------------------------------------
		public static K GetObjectByInternalKeyOrCreate(string sInternalKey, string sInternalDescription = "")
		{
			K rReturn = null;
			int tIndex = ObjectsByKeyList.IndexOf(sInternalKey);
			if (tIndex >= 0) {
				K tObject = ObjectsList.ElementAt (tIndex) as K;
				if (tObject.IsReacheableByAccount ()) {
					rReturn = tObject;
				}
			}
			if (rReturn == null) {
				rReturn = NWDBasis<K>.NewInstance () as K;
				RemoveObjectInListOfEdition (rReturn);
				rReturn.InternalKey = sInternalKey;
				rReturn.InternalDescription = sInternalDescription;
				// TODO : add Internal reference of account dependent
				rReturn.UpdateMe ();
				AddObjectInListOfEdition (rReturn);
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static K[] GetObjectsByInternalKeys(string[] sInternalKeys)
		{
			List<K> rReturn = new List<K>();
			foreach (string tInternalKey in sInternalKeys) {
				K tObject = GetObjectByInternalKey(tInternalKey);
				if (tObject!=null)
				{
					rReturn.Add(tObject);
				}
			}
			return rReturn.ToArray();
		}
		//-------------------------------------------------------------------------------------------------------------
		//TODO : GetAllObjectByInternalKeys(string[] sInternalKey)
		//-------------------------------------------------------------------------------------------------------------
		public static K[] GetObjectsByInternalKeysOrCreate(string[] sInternalKeys, string sInternalDescription = "")
		{
			List<K> rReturn = new List<K>();
			foreach (string tInternalKey in sInternalKeys) {
				K tObject = GetObjectByInternalKeyOrCreate(tInternalKey, sInternalDescription);
				if (tObject!=null)
				{
					rReturn.Add(tObject);
				}
			}
			return rReturn.ToArray();
		}
		//-------------------------------------------------------------------------------------------------------------
		// TODO: must be tested
		public static K[] Where (Expression<Func<K, bool>> predExpr)
		{
			IEnumerable<K> tEnumerable = NWDDataManager.SharedInstance.SQLiteConnection.Table<K> ().Where (predExpr);
			List<K> tAllReferences = new List<K> ();
			foreach (K tItem in tEnumerable) {
				int tIndex = ObjectsByReferenceList.IndexOf(tItem.Reference);
				K tObject = ObjectsList.ElementAt (tIndex) as K;
				if (tObject.IsReacheableByAccount ()) {
					tAllReferences.Add (tObject);
				}
			}
			return tAllReferences.ToArray();
		}
		//-------------------------------------------------------------------------------------------------------------
		public static K[] SelectOrderedList(K[] sArray, Comparison<K> sComparison) {
			List<K> tList = new List<K> ();
			tList.AddRange (sArray);
			tList.Sort (sComparison);
			return tList.ToArray();
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion

		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================