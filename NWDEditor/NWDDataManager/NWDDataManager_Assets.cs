//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using SQLite4Unity3d;
using UnityEngine;
using BasicToolBox;
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
			EditorUtility.DisplayProgressBar(tProgressBarTitle, "Prepare", tOperation/tCountClass);
			foreach( Type tType in mTypeList)
			{
                EditorUtility.DisplayProgressBar(tProgressBarTitle, "Change asset path in "+tType.Name+" objects", tOperation/tCountClass);
				tOperation++;
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.New_ChangeAssetPath(sOldPath,sNewPath);
    //            MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicStaticFlattenHierarchy(tType, NWDConstants.M_ChangeAssetPath);
				//if (tMethodInfo != null) 
				//{
				//	tMethodInfo.Invoke(null, new object[] {sOldPath, sNewPath});
				//}
			}
            DataQueueExecute ();
			EditorUtility.DisplayProgressBar(tProgressBarTitle, "Finish", 1.0F);
			EditorUtility.ClearProgressBar();
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif