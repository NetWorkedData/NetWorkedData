using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.IO;

using SQLite4Unity3d;

namespace NetWorkedData
{
	public partial class NWDDataManager
	{
		#if UNITY_EDITOR
		public void CreateCShapAllClass () {
			//Debug.Log ("CreateCShapAllClass");
			NWDAppConfiguration.SharedInstance.GenerateCSharpFile (NWDAppConfiguration.SharedInstance.SelectedEnvironment ());
			foreach( Type tType in mTypeList)
			{
				var tMethodInfo = tType.GetMethod("CreateCSharp", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) 
				{
					tMethodInfo.Invoke(null, null);
				}
			}
		}
		#endif
	}
}