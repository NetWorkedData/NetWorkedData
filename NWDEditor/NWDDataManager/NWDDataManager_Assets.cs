//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
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
            //NWDBenchmark.Start();
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
            //NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif