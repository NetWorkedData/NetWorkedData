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

using SQLite4Unity3d;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDDataManager
	{
		/*
		public void CreateCShapAllClass () {
			//Debug.Log ("CreateCShapAllClass");
			string tProgressBarTitle = "NetWorkedData Create all C# files";
			float tCountClass = mTypeList.Count + 1;
			float tOperation = 1;
			EditorUtility.DisplayProgressBar(tProgressBarTitle, "Create NWDAppConfiguration", tOperation/tCountClass);
			tOperation++;
			NWDAppConfiguration.SharedInstance().GenerateCSharpFile (NWDAppConfiguration.SharedInstance().SelectedEnvironment ());
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
		*/
	}
}
//=====================================================================================================================
#endif