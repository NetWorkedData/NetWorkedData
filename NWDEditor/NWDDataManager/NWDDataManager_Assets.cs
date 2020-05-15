//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:40
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using UnityEngine;
//using BasicToolBox;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDDataManager
    {
        //-------------------------------------------------------------------------------------------------------------
        public void ChangeAssetPath (string sOldPath, string sNewPath)
        {
            //NWEBenchmark.Start();
            //NWDDebug.Log("sOldPath = " + sOldPath + " to sNewPath " + sNewPath);
            string tProgressBarTitle = "NetWorkedData is looking for asset(s) in datas";
			float tCountClass = ClassTypeList.Count + 2;
			float tOperation = 1;
            EditorUtility.DisplayProgressBar(tProgressBarTitle, "Load all datas", tOperation++ / tCountClass);
            foreach (Type tType in ClassTypeLoadedList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                if (tHelper.kAssetDependent == true)
                {
                    if (tHelper.IsLoaded() == false)
                    {
                        tHelper.LoadFromDatabase(string.Empty, false);
                    }
                }
            }
            EditorUtility.DisplayProgressBar(tProgressBarTitle, "Prepare", tOperation++/tCountClass);
			foreach( Type tType in ClassTypeList)
			{
                EditorUtility.DisplayProgressBar(tProgressBarTitle, "Change asset path in "+tType.Name+" objects", tOperation++/tCountClass);
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                tHelper.ChangeAssetPath(sOldPath,sNewPath);
			}
            DataQueueExecute ();
			EditorUtility.DisplayProgressBar(tProgressBarTitle, "Finish", 1.0F);
			EditorUtility.ClearProgressBar();
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif