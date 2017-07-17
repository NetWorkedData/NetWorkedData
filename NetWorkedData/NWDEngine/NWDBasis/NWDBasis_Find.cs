using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;

using BasicToolBox;

namespace NetWorkedData
{
	public partial  class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		// This part can be automatically rewrite and optimized by generate CSharp File
		#region Generic Analyze


		public static bool AccountDependent ()
		{
			bool rAccountConnected = false;
			Type tType = ClassType ();
			foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
				Type tTypeOfThis = tProp.PropertyType;
				if (tTypeOfThis != null) {
					if (tTypeOfThis.IsGenericType) {
						if (tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferenceType<>)) {
							Type tSubType = tTypeOfThis.GetGenericArguments () [0];
							if (tSubType == typeof(NWDAccount)) {
								rAccountConnected = true;
							}
						}
					}
				}
			}
			return rAccountConnected;
		}


		public virtual bool IsAccountDependent ()
		{
			bool rAccountConnected = false;
			Type tType = GetType ();
			foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
				Type tTypeOfThis = tProp.PropertyType;
				if (tTypeOfThis != null) {
					if (tTypeOfThis.IsGenericType) {
						if (tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferenceType<>)) {
							Type tSubType = tTypeOfThis.GetGenericArguments () [0];
							if (tSubType == typeof(NWDAccount)) {
								rAccountConnected = true;
							}
						}
					}
				}
			}
			return rAccountConnected;
		}

		public virtual bool IsAccountConnected (string sAccountReference)
		{
			bool rAccountConnected = false;
			Type tType = GetType ();
			foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
				Type tTypeOfThis = tProp.PropertyType;
				if (tTypeOfThis != null) {
					if (tTypeOfThis.IsGenericType) {
						if (
							tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferenceType<>)
							|| tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferencesListType<>)
							|| tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferencesQuantityType<>)
						) {
							Type tSubType = tTypeOfThis.GetGenericArguments () [0];
							if (tSubType == typeof(NWDAccount)) {
								var tValue = tProp.GetValue (this, null);
								var tMethodInfo = tValue.GetType ().GetMethod ("ToString", BindingFlags.Public | BindingFlags.Instance);
								if (tMethodInfo != null) {
									string tValueToString = tMethodInfo.Invoke (tValue, null) as string;
									if (tValueToString.Contains (sAccountReference)) {
										rAccountConnected = true;
									}
								}
							}
						}
					}
				}
			}
			return rAccountConnected;
		}

		public virtual bool IsLockedObject () // return true during the player game
			{
			#if UNITY_EDITOR
			return false;
			#else
				bool rLockedObject = true;
				Type tType = GetType ();
				foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
				Type tTypeOfThis = tProp.PropertyType;
				if (tTypeOfThis != null) {
					if (tTypeOfThis.IsGenericType) {
						if (tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferenceType<>)) {
							Type tSubType = tTypeOfThis.GetGenericArguments () [0];
							if (tSubType == typeof(NWDAccount)) {
								rLockedObject = false;
							}
						}
					}
				}
			}
			return rLockedObject;
			#endif
		}
		#endregion

		#region Static Get Object
		public static NWDBasis<K> GetObjectByReference (string sReference)
		{
			NWDBasis<K> rObject = null;
			if (ObjectsByReferenceList.Contains (sReference)) {
				int tObjectIndex = ObjectsByReferenceList.IndexOf (sReference);
				rObject = (NWDBasis<K>)ObjectsList.ElementAt (tObjectIndex);
			}
			return rObject;
		}

		public static NWDBasis<K> GetObjectByReferenceOrCreate (string sReference, string sInternalKey="", string sInternalDescription = "")
		{
			NWDBasis<K> rObject = null;
			if (ObjectsByReferenceList.Contains (sReference)) {
				int tObjectIndex = ObjectsByReferenceList.IndexOf (sReference);
				rObject = (NWDBasis<K>)ObjectsList.ElementAt (tObjectIndex);
			}
			if (rObject == null) {
				rObject = NWDBasis<K>.NewInstance ();
				RemoveObjectInListOfEdition (rObject);
				rObject.Reference = sReference;
				rObject.InternalKey = sInternalKey;
				rObject.InternalDescription = sInternalDescription;
				rObject.UpdateMe ();
				AddObjectInListOfEdition (rObject);
			}
			return rObject;
		}


		public static NWDBasis<K> GetObjectByReferenceLimited (string sReference)
		{
			NWDBasis<K> rObject = null;
			if (ObjectsByReferenceList.Contains (sReference)) {
				int tObjectIndex = ObjectsByReferenceList.IndexOf (sReference);
				rObject = (NWDBasis<K>)ObjectsList.ElementAt (tObjectIndex);
			}
			return rObject;
		}

		public static NWDBasis<K> GetObjectInObjectsByReferenceList (string sReference)
		{
			NWDBasis<K> rObject = null;
			if (ObjectsByReferenceList.Contains (sReference)) {
				int tObjectIndex = ObjectsByReferenceList.IndexOf (sReference);
				rObject = (NWDBasis<K>)ObjectsList.ElementAt (tObjectIndex);
			}
			return rObject;
		}

		public static NWDBasis<K> GetObjectInObjectsByKeyList (string sKey)
		{
			NWDBasis<K> rObject = null;
			if (ObjectsByKeyList.Contains (sKey)) {
				int tObjectIndex = ObjectsByKeyList.IndexOf (sKey);
				rObject = (NWDBasis<K>)ObjectsList.ElementAt (tObjectIndex);
			}
			return rObject;
		}
		#endregion



		/*
		public static Dictionary<string, K> kDictionaryOfAllObjects = new Dictionary<string, K> ();
		public static Dictionary<string, K> kDictionaryOfAllActiveObjects = new Dictionary<string, K> ();
		public static IEnumerable<K> kEnumerableOfAllObjects;

		public static void ReloadAllObjects ()
		{
			kDictionaryOfAllObjects = new Dictionary<string, K> ();
			kDictionaryOfAllActiveObjects = new Dictionary<string, K> ();
			kEnumerableOfAllObjects = NWDDataManager.SharedInstance.SQLiteConnection.Table<K> ();
			if (kEnumerableOfAllObjects != null) 
			{
				foreach (K tItem in kEnumerableOfAllObjects) 
				{
					kDictionaryOfAllObjects.Add (tItem.Reference, tItem);
					if (tItem.AC == true) 
					{
						kDictionaryOfAllActiveObjects.Add (tItem.Reference, tItem);
					}
				}

			}
			NWDDataManager.SharedInstance.NotificationCenter.PostNotification (new IDENotification (NWDDataManager.kUpdateDatasNotificationsKey, null));
			FilterTableEditor ();
		}

		public IEnumerable<K> AllObjects ()
		{
			return kEnumerableOfAllObjects;
		}

		public List<K> ListAllObjects (bool sActiveOnly = false)
		{
			Dictionary<string, K> tDico = kDictionaryOfAllObjects;
			if (sActiveOnly == true)
			{
				tDico = kDictionaryOfAllActiveObjects;
			}
			List<K> rList = new List<K> ();
			foreach (KeyValuePair<string, K> tEntry in tDico) 
			{
				rList.Add(tEntry.Value);
			}
			return rList;
		}

		public static List<K> ListOfObjectsWithReferences(string[] sReferences, bool sActiveOnly = false)
		{
			Dictionary<string, K> tDico = kDictionaryOfAllObjects;
			if (sActiveOnly == true)
			{
				tDico = kDictionaryOfAllActiveObjects;
			}
			List<K> rList = new List<K> ();
			foreach (string tRef in sReferences)
			{
				if (tDico.ContainsKey (tRef)) 
				{
					rList.Add(tDico[tRef]);
				}
			}
			return rList;
		}

		public static K ObjectWithReference(string sReference, bool sActiveOnly = false)
		{
			Dictionary<string, K> tDico = kDictionaryOfAllObjects;
			if (sActiveOnly == true)
			{
				tDico = kDictionaryOfAllActiveObjects;
			}
			K rObject = null;
			if (tDico.ContainsKey (sReference)) 
				{
				rObject = tDico[sReference];
				}
			return rObject;
		}
*/
	}
}