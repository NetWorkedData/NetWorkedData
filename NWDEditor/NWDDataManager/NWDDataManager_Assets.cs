//=====================================================================================================================
//
// ideMobi copyright 2018 
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
using BasicToolBox;
#if UNITY_EDITOR
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDDataManager
	{
		//-------------------------------------------------------------------------------------------------------------
		public void ChangeAssetPath (string sOldPath, string sNewPath) {
			//Debug.Log ("ChangeAssetPath " + sOldPath + " to " + sNewPath);
			string tProgressBarTitle = "NetWorkedData is looking for asset(s) in datas";
			float tCountClass = mTypeList.Count + 1;
			float tOperation = 1;
			EditorUtility.DisplayProgressBar(tProgressBarTitle, "P repare", tOperation/tCountClass);
			foreach( Type tType in mTypeList)
			{
				EditorUtility.DisplayProgressBar(tProgressBarTitle, "Change asset path in "+tType.Name+" objects", tOperation/tCountClass);
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
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif