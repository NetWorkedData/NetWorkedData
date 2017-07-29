//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Reflection;
using System;

//=====================================================================================================================
namespace NetWorkedData
{
	#if UNITY_EDITOR
	public class NWDBasisClassInspector : ScriptableObject
	{
		//public string mTest;
		public Type mTypeInEdition;
//		public NWDBasisWindow mWindowInEdition;
	}

	[CustomEditor (typeof(NWDBasisClassInspector))]
	public class NWDBasisClassEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			NWDBasisClassInspector tTarget = (NWDBasisClassInspector)target;
			//Debug.Log ("OnInspectorGUI " + tTarget.mTest);
			if (tTarget.mTypeInEdition == null) {
				//GUILayout.Label ("No Table to draw! Select one table");
			} else {
				//GUILayout.Label ("DRAW TYPE EDITOR " + tTarget.mTypeInEdition.Name);
				var tMethodInfo = tTarget.mTypeInEdition.GetMethod ("DrawTypeInInspector", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) {
					tMethodInfo.Invoke (null, null);
				}

			}
		}
	}
	#endif
}
//=====================================================================================================================