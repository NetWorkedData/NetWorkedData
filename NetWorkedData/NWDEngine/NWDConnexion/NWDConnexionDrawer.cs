//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

using SQLite4Unity3d;

using BasicToolBox;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;

//=====================================================================================================================
namespace NetWorkedData
{
	[CustomPropertyDrawer (typeof(NWDConnexionBasis), true)]
//	[CustomPropertyDrawer (typeof(NWDConnexion<NWDAccount>), true)]
	public partial class NWDConnexionDrawer : PropertyDrawer
	{
		//-------------------------------------------------------------------------------------------------------------
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			float tHeight = 80.0f;
			Debug.Log ("I PASS HERREEEEEEEEEEE HEIGHTTTTT");
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true)[0];
			}
			Type tType = Type.GetType ("NetWorkedData."+property.type);
			Type tTypeParent = tType.BaseType;
			Type tTypeDefintion = null;
			if (tTypeParent.IsGenericType) {
				tTypeDefintion = tTypeParent.GetGenericArguments ()[0];
			}
			Type tClassType  = tTypeParent.BaseType;
			if (tClassType == typeof(NWDConnexion<>) && tTypeDefintion!=null)
			{
				string tTargetReference = property.FindPropertyRelative ("Reference").stringValue;
				var tMethodInfo = tTypeDefintion.GetMethod ("ReferenceConnexionHeight", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
			if (tMethodInfo != null)
			{
				string tHeightString = tMethodInfo.Invoke (null, new object[]
					{
						tTargetReference,
						tReferenceConnexion.ShowInspector
					}) as string;
				float.TryParse (tHeightString, out tHeight);
			}
			}
			return tHeight;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			Debug.Log ("I PASS HERREEEEEEEEEEE");
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true)[0];
			}
			Type tType = Type.GetType ("NetWorkedData."+property.type);
			Type tTypeParent = tType.BaseType;
			Type tTypeDefintion = null;
			if (tTypeParent.IsGenericType) {
				tTypeDefintion = tTypeParent.GetGenericArguments ()[0];
			}
			Type tClassType  = tTypeParent.BaseType;
			if (tClassType == typeof(NWDConnexion<>) && tTypeDefintion != null) {
				string tTargetReference = property.FindPropertyRelative ("Reference").stringValue;
				var tMethodInfo = tTypeDefintion.GetMethod ("ReferenceConnexionField", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) {
					string tNewValue = tMethodInfo.Invoke (null,
						                  new object[] {position,
							property.displayName,
							tTargetReference,
							"",
							tReferenceConnexion.ShowInspector,
							tReferenceConnexion.Editable,
							tReferenceConnexion.EditButton,
							tReferenceConnexion.NewButton
						}) as string;
					property.FindPropertyRelative ("Reference").stringValue = tNewValue;
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================

#endif