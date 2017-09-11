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
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	#if UNITY_EDITOR
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public class NWDBasisObjectInspector : ScriptableObject
	{
		public object mObjectInEdition;
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[CustomEditor (typeof(NWDBasisObjectInspector))]
	public class NWDBasisEditor : Editor
	{
		public override void OnInspectorGUI ()
		{
			NWDBasisObjectInspector tTarget = (NWDBasisObjectInspector)target;
			if (tTarget.mObjectInEdition == null)
			{
			} 
			else 
			{
				Type tType = tTarget.mObjectInEdition.GetType ();
				var tMethodInfo = tType.GetMethod ("DrawObjectEditor", BindingFlags.Public | BindingFlags.Instance);
				if (tMethodInfo != null) 
				{
					tMethodInfo.Invoke (tTarget.mObjectInEdition, new object[]{Rect.zero,false});
				}
			}
		}
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	#endif
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================