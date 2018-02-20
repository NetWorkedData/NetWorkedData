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
	//-------------------------------------------------------------------------------------------------------------
    //TODO: Change for an editable Tag system

	public enum NWDBasisTag {
		NoTag = -1,
		TagZero = 0,
		TagOne = 1,
		TagTwo = 2,

		MultipleData = 98,

		InError = 99,

		GGDontDoThat = 997,

		WhatTheFuck = 998,
		MustBeDestroyed = 999,
	}
	//-------------------------------------------------------------------------------------------------------------
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
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public static K NewObjectWithReference(string sReference)
		{
			K rReturn = NWDBasis <K>.NewInstanceWithReference (sReference) as K;
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
        public static K[] GetAllObjects(string sAccountReference = null)
		{
			List<K> rReturn = new List<K>();
			foreach (K tObject in NWDBasis<K>.ObjectsList)
			{
                if (tObject.IsReacheableByAccount(sAccountReference))
				{
					rReturn.Add(tObject);
				}
			}
			return rReturn.ToArray();
		}
		//-------------------------------------------------------------------------------------------------------------
        public static K GetObjectByReference(string sReference,string sAccountReference = null)
		{
			K rReturn = null;
			int tIndex = ObjectsByReferenceList.IndexOf(sReference);
			if (tIndex >= 0) {
				K tObject = ObjectsList.ElementAt (tIndex) as K;
                if (tObject.IsReacheableByAccount (sAccountReference)) {
					rReturn = tObject;
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
        public static K[] GetObjectsByReferences(string[] sReferences, string sAccountReference = null)
		{
			List<K> rReturn = new List<K>();
			foreach (string tReference in sReferences) {
                K tObject = GetObjectByReference(tReference, sAccountReference);
				if (tObject!=null)
				{
					rReturn.Add(tObject);
				}
			}
			return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K[] GetAllObjectsByInternalKey(string sInternalKey, string sAccountReference = null)
        {
            List<K> rReturn = new List<K>();
            int[] tIndexes = ObjectsByKeyList.Select((b, i) => b == sInternalKey ? i : -1).Where(i => i != -1).ToArray();
            foreach (int tIndex in tIndexes)
            {
                K tObject = ObjectsList.ElementAt(tIndex) as K;
                if (tObject.IsReacheableByAccount(sAccountReference))
                {
                    rReturn.Add(tObject);
                }
            }
            return rReturn.ToArray();
        }
		//-------------------------------------------------------------------------------------------------------------
        public static K GetObjectByInternalKey(string sInternalKey,string sAccountReference = null)
		{
			//K rReturn = null;
			//int tIndex = ObjectsByKeyList.IndexOf(sInternalKey);
			//if (tIndex >= 0) {
			//	K tObject = ObjectsList.ElementAt (tIndex) as K;
   //             if (tObject.IsReacheableByAccount (sAccountReference)) {
			//		rReturn = tObject;
			//	}
			//}
            K[] rReturnArray = GetAllObjectsByInternalKey(sInternalKey, sAccountReference);
            K rReturn = null;
            if (rReturnArray.Length > 0)
            {
                rReturn = rReturnArray[0];
            }
            if (rReturnArray.Length > 1)
            {
                Debug.LogWarning("The InternalKey " + sInternalKey + " for " + ClassNamePHP() + " is not unique!");
            }
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
        public static K GetObjectByInternalKeyOrCreate(string sInternalKey, string sInternalDescription = "",string sAccountReference = null)
		{
            K[] rReturnArray = GetAllObjectsByInternalKey(sInternalKey, sAccountReference);
            K rReturn = null;
            if (rReturnArray.Length > 0)
            {
                rReturn = rReturnArray[0];
            }
            if (rReturnArray.Length > 1)
            {
                Debug.LogWarning("The InternalKey "+sInternalKey+" for "+ClassNamePHP()+" is not unique!");
            }
            if (rReturn == null)
            {
                rReturn = NWDBasis<K>.NewObject();
                rReturn.InternalKey = sInternalKey;
                rReturn.InternalDescription = sInternalDescription;
                rReturn.UpdateMe ();
                AddObjectInListOfEdition(rReturn);
            }
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
        public static K[] GetObjectsByInternalKeys(string[] sInternalKeys,string sAccountReference = null)
		{
			List<K> rReturn = new List<K>();
			foreach (string tInternalKey in sInternalKeys) {
                K tObject = GetObjectByInternalKey(tInternalKey,sAccountReference);
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
        public static K[] GetObjectsByInternalKeysOrCreate(string[] sInternalKeys, string sInternalDescription = "",string sAccountReference = null)
		{
			List<K> rReturn = new List<K>();
			foreach (string tInternalKey in sInternalKeys) {
                K tObject = GetObjectByInternalKeyOrCreate(tInternalKey, sInternalDescription,sAccountReference);
				if (tObject!=null)
				{
					rReturn.Add(tObject);
				}
			}
			return rReturn.ToArray();
		}
		//-------------------------------------------------------------------------------------------------------------
		// TODO: must be tested
        public static K[] Where (Expression<Func<K, bool>> predExpr,string sAccountReference = null)
		{
			SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance.SQLiteConnectionEditor;
			if (AccountDependent ())
			{
				tSQLiteConnection = NWDDataManager.SharedInstance.SQLiteConnectionAccount;
			}
			IEnumerable<K> tEnumerable = tSQLiteConnection.Table<K> ().Where (predExpr);
			List<K> tAllReferences = new List<K> ();
			foreach (K tItem in tEnumerable) {
				int tIndex = ObjectsByReferenceList.IndexOf(tItem.Reference);
				K tObject = ObjectsList.ElementAt (tIndex) as K;
                if (tObject.IsReacheableByAccount (sAccountReference)) {
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
        #region Instance Methods
        //-------------------------------------------------------------------------------------------------------------
        public bool IsEnable()
        {
            return AC;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsTrashed()
        {
            if (XX > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
		//-------------------------------------------------------------------------------------------------------------
		public NWDBasisTag GetTag()
		{
			return (NWDBasisTag)Tag;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void setTag(NWDBasisTag sTag)
		{
			Tag = (int)sTag;
			UpdateMeIfModified ();
		}
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================