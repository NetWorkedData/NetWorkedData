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

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDDataManager
	{
		public Type TypeFromReference (string sReference)
		{
			Type rReturn = null;
			string tTrigram = sReference.Substring (0, 3);
			if (mTrigramTypeDictionary.ContainsKey (tTrigram)) {
				rReturn = mTrigramTypeDictionary [tTrigram];
			}
			return rReturn;
		}

		public object ObjectFromReference (string sReference)
		{
			object rReturn = null;
			Type tType = TypeFromReference (sReference);
			var tMethodInfo = tType.GetMethod ("GetObjectInObjectsByReferenceList", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
			if (tMethodInfo != null) {
				rReturn = tMethodInfo.Invoke (null, new object[]{ sReference }) as object;
			}
			return rReturn;
		}

		public void ReloadAllObjects ()
		{
			foreach( Type tType in mTypeList)
			{
				var tMethodInfo = tType.GetMethod("LoadTableEditor", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) 
				{
					tMethodInfo.Invoke(null, null);
				}
			}
		}
	}
}
//=====================================================================================================================