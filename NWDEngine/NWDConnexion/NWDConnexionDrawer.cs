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
	// CUSTOM PROPERTY DRAWER METHODS
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#if UNITY_EDITOR
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[CustomPropertyDrawer (typeof(NWDConnexionBasis), true)]
	public class NWDConnexionDrawer: PropertyDrawer
	{
		//-------------------------------------------------------------------------------------------------------------
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			Debug.Log ("NWDConnexionDrawer GetPropertyHeight");
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true)[0];
			}
			float tHeight = 0.0f;
			Debug.Log ("Type of property " + property.type);
			Type tType = Type.GetType ("NetWorkedData."+property.type);
			Type tTypeParent = tType.BaseType;
			Debug.Log ("tTypeParent " + tTypeParent.Name);
			Type tTypeDefintion = null;
			if (tTypeParent.IsGenericType) {
				tTypeDefintion = tTypeParent.GetGenericArguments ()[0];
				Debug.Log ("tTypeDefintion " + tTypeDefintion.Name);

				string tTargetReference = property.FindPropertyRelative ("Reference").stringValue;
				var tMethodInfo = tTypeDefintion.GetMethod ("ReferenceConnexionHeightSerializedString", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null)
				{
					string tHeightString = tMethodInfo.Invoke (null, new object[]{property, tReferenceConnexion.ShowInspector}) as string;
					float.TryParse (tHeightString, out tHeight);
				}
			}
			return tHeight;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true)[0];
			}
			Debug.Log ("Type of property " + property.type);
			Type tType = Type.GetType ("NetWorkedData."+property.type);
			Type tTypeParent = tType.BaseType;
			Debug.Log ("tTypeParent " + tTypeParent.Name);
			Type tTypeDefintion = null;


			if (tTypeParent.IsGenericType) {
				tTypeDefintion = tTypeParent.GetGenericArguments ()[0];
				Debug.Log ("tTypeDefintion " + tTypeDefintion.Name);
				string tTargetReference = property.FindPropertyRelative ("Reference").stringValue;


				//bool tConnexion = true;
				//if (tTargetReference != null && tTargetReference != "")
				//{
				//	if (NWDBasis<K>.InstanceByReference(Value) == null)
				//	{
				//		tConnexion = false;
				//	}
				//}
				//EditorGUI.BeginDisabledGroup(!tConnexion);


				var tMethodInfo = tTypeDefintion.GetMethod ("ReferenceConnexionFieldSerialized", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null)
				{
					tMethodInfo.Invoke (null, new object[]{position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton});
				}

			//EditorGUI.EndDisabledGroup();

			//if (tConnexion == false)
			//{
			//	tTemporary.Value = Value;

			//	GUI.Label(new Rect(tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_ERROR, tLabelStyle);
			//	tY = tY + NWDConstants.kFieldMarge + tLabelStyle.fixedHeight;
			//	//              GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelAssetStyle.fixedHeight), Value.Replace (NWDAssetType.kAssetDelimiter, ""),tLabelAssetStyle);
			//	//              tY = tY + NWDConstants.kFieldMarge + tLabelAssetStyle.fixedHeight;
			//	Color tOldColor = GUI.backgroundColor;
			//	GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
			//	if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth, tY, 60.0F, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_CLEAN, tMiniButtonStyle))
			//	{
			//		tTemporary.Value = "";
			//	}
			//	GUI.backgroundColor = tOldColor;
			//	tY = tY + NWDConstants.kFieldMarge + tMiniButtonStyle.fixedHeight;
			//}





			}
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
#endif
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================

#endif