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
using SQLite4Unity3d;
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
        private void LoadAllDatas()
        {
            foreach (Type tType in mTypeLoadedList)
            {
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tType);
                if (tHelper.kAssetDependent == true)
                {
                    if (tHelper.DatasAreLoaded() == false)
                    {
                        tHelper.LoadFromDatabase();
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ChangeAssetPath (string sOldPath, string sNewPath)
        {
            //NWEBenchmark.Start();
            string tProgressBarTitle = "NetWorkedData is looking for asset(s) in datas";
			float tCountClass = mTypeList.Count + 2;
			float tOperation = 1;
            EditorUtility.DisplayProgressBar(tProgressBarTitle, "Load all datas", tOperation++ / tCountClass);
            LoadAllDatas();
			EditorUtility.DisplayProgressBar(tProgressBarTitle, "Prepare", tOperation++/tCountClass);
			foreach( Type tType in mTypeList)
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