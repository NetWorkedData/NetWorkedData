using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using SQLite4Unity3d;

using UnityEngine;
using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDDataManager
	{
		public void ChangeAssetPath (string sOldPath, string sNewPath) {
			BTBDebug.Log ("ChangeAssetPath " + sOldPath + " to " + sNewPath);
			string tProgressBarTitle = "NetWorkedData Create all C# files";
			float tCountClass = mTypeList.Count + 1;
			float tOperation = 1;
			EditorUtility.DisplayProgressBar(tProgressBarTitle, "Create prepare", tOperation/tCountClass);
			foreach( Type tType in mTypeList)
			{
				EditorUtility.DisplayProgressBar(tProgressBarTitle, "ChangeAssetPath in "+tType.Name+" objects", tOperation/tCountClass);
				tOperation++;
				var tMethodInfo = tType.GetMethod("ChangeAssetPath", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) 
				{
					tMethodInfo.Invoke(null, new object[] {sOldPath, sNewPath});
				}
			}
			UpdateQueueExecute ();
			EditorUtility.DisplayProgressBar(tProgressBarTitle, "Finish", 1.0F);
			EditorUtility.ClearProgressBar();
		}
	}
}
//=====================================================================================================================
#endif