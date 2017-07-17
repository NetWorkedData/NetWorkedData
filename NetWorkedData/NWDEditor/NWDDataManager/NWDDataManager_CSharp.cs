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
			string tProgressBarTitle = "NetWorkedData Create all C# files";
			float tCountClass = mTypeList.Count + 1;
			float tOperation = 1;
			EditorUtility.DisplayProgressBar(tProgressBarTitle, "Create NWDAppConfiguration", tOperation/tCountClass);
			tOperation++;
			NWDAppConfiguration.SharedInstance.GenerateCSharpFile (NWDAppConfiguration.SharedInstance.SelectedEnvironment ());
			foreach( Type tType in mTypeList)
			{
				EditorUtility.DisplayProgressBar(tProgressBarTitle, "Create "+tType.Name+" files", tOperation/tCountClass);
				tOperation++;
				var tMethodInfo = tType.GetMethod("CreateCSharp", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) 
				{
					tMethodInfo.Invoke(null, null);
				}
			}
			EditorUtility.DisplayProgressBar(tProgressBarTitle, "Finish", 1.0F);
			EditorUtility.ClearProgressBar();
		}
		#endif
	}
}