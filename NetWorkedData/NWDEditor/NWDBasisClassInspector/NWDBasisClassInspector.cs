//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	#if UNITY_EDITOR
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDBasisClassInspector : ScriptableObject
	{
		//-------------------------------------------------------------------------------------------------------------
		public Type mTypeInEdition;
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[CustomEditor (typeof(NWDBasisClassInspector))]
	public class NWDBasisClassEditor : Editor
	{
		//-------------------------------------------------------------------------------------------------------------
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
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	#endif
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================