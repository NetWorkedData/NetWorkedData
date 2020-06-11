//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================
#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;
//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDBasisObjectInspector : ScriptableObject
	{
		public NWDTypeClass mObjectInEdition;
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[CustomEditor (typeof(NWDBasisObjectInspector))]
	public class NWDBasisEditor : Editor
    {
        //-------------------------------------------------------------------------------------------------------------
        static public Editor mGameObjectEditor;
        public static Type ObjectEditorLastType;
        //-------------------------------------------------------------------------------------------------------------
		public override void OnInspectorGUI ()
        {
            //NWEBenchmark.Start();
            NWDBasisObjectInspector tTarget = (NWDBasisObjectInspector)target;
			if (tTarget.mObjectInEdition != null)
			{
                tTarget.mObjectInEdition.DrawEditor(Rect.zero, false, null);
            }
            //NWEBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif