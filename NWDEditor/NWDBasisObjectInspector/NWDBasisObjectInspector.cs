// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:18
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================
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
			NWDBasisObjectInspector tTarget = (NWDBasisObjectInspector)target;
			if (tTarget.mObjectInEdition != null)
			{
    //            Type tType = tTarget.mObjectInEdition.GetType ();
    //            MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicInstance(tType, NWDConstants.M_DrawObjectEditor);
    //            if (tMethodInfo != null) 
				//{
				//	tMethodInfo.Invoke (tTarget.mObjectInEdition, new object[]{Rect.zero,false});
				//}
                tTarget.mObjectInEdition.New_DrawObjectEditor(Rect.zero, false);

        }
		}
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif