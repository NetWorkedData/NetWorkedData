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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
//=====================================================================================================================
namespace NetWorkedData.NWDEditor
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDBasisObjectInspector : ScriptableObject
	{
        //-------------------------------------------------------------------------------------------------------------
        //TODO rename
        public NWDTypeClass mObjectInEdition;
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [CustomEditor (typeof(NWDBasisObjectInspector))]
	public class NWDBasisEditor : Editor
    {
        //-------------------------------------------------------------------------------------------------------------
        //TODO rename
        static public Editor mGameObjectEditor;
        static public Type ObjectEditorLastType;
        //-------------------------------------------------------------------------------------------------------------
		public override void OnInspectorGUI ()
        {
            NWDBenchmark.Start();
            NWDBasisObjectInspector tTarget = (NWDBasisObjectInspector)target;
			if (tTarget.mObjectInEdition != null)
			{
                tTarget.mObjectInEdition.DrawEditor(Rect.zero, false, null, null);
            }
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif