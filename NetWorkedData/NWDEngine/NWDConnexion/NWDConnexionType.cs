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

using UnityEngine;

using SQLite4Unity3d;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	[Serializable]
	public class NWDConnexion 
	{
		[SerializeField]
		public string Reference;
		public Type PrivateType;
	}

	[Serializable]
	public class NWDConnexionType<T> : NWDConnexion
	{
	}

	public class NWDConnexionAttribut : PropertyAttribute
	{
		public bool ShowInspector = false;
		public bool Editable = false;
		public bool EditButton = true;
		public bool NewButton = true;

		public NWDConnexionAttribut ()
		{
		}

		public NWDConnexionAttribut (bool sShowInspector, bool sEditable = false, bool sEditButton = true, bool sNewButton = true)
		{
			ShowInspector = sShowInspector;
			Editable = sEditable;
			EditButton = sEditButton;
			NewButton = sNewButton;
		}
	}
	
	#if UNITY_EDITOR

	[CustomPropertyDrawer (typeof(NWDConnexion), true)]
	public partial class NWDConnexionDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			float tHeight = 80.0f;
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
			if (tClassType == typeof(NWDConnexion) && tTypeDefintion!=null)
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

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
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
			if (tClassType == typeof(NWDConnexion) && tTypeDefintion != null) {
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
	}
	#endif
}
//=====================================================================================================================