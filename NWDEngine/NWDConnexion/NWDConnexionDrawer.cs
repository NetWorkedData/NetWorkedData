//=====================================================================================================================
//
// ideMobi copyright 2018 
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
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDConnectionDrawer draw the control field in the inspector.
    /// </summary>
	[CustomPropertyDrawer (typeof(NWDConnectionBasis), true)]
	public class NWDConnectionDrawer: PropertyDrawer
	{
		//-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Gets the height of the property.
        /// </summary>
        /// <returns>The property height.</returns>
        /// <param name="property">Property.</param>
        /// <param name="label">Label.</param>
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			//Debug.Log ("NWDConnectionDrawer GetPropertyHeight");
			NWDConnectionAttribut tReferenceConnection = new NWDConnectionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnectionAttribut), true).Length > 0)
			{
				tReferenceConnection = (NWDConnectionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnectionAttribut), true)[0];
			}
			float tHeight = 0.0f;
			//Debug.Log ("Type of property " + property.type);
			Type tType = Type.GetType ("NetWorkedData."+property.type);
			Type tTypeParent = tType.BaseType;
			//Debug.Log ("tTypeParent " + tTypeParent.Name);
			Type tTypeDefintion = null;
			if (tTypeParent.IsGenericType) {
				tTypeDefintion = tTypeParent.GetGenericArguments ()[0];
				//Debug.Log ("tTypeDefintion " + tTypeDefintion.Name);
				string tTargetReference = property.FindPropertyRelative ("Reference").stringValue;
				var tMethodInfo = tTypeDefintion.GetMethod ("ReferenceConnectionHeightSerializedString", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null)
				{
					string tHeightString = tMethodInfo.Invoke (null, new object[]{property, tReferenceConnection.ShowInspector}) as string;
					float.TryParse (tHeightString, out tHeight);
				}
			}
			return tHeight;
		}
		//-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Ons the GUI.
        /// </summary>
        /// <param name="position">Position.</param>
        /// <param name="property">Property.</param>
        /// <param name="label">Label.</param>
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDConnectionAttribut tReferenceConnection = new NWDConnectionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnectionAttribut), true).Length > 0)
			{
				tReferenceConnection = (NWDConnectionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnectionAttribut), true)[0];
			}
			//Debug.Log ("Type of property " + property.type);
			Type tType = Type.GetType ("NetWorkedData."+property.type);
			Type tTypeParent = tType.BaseType;
			//Debug.Log ("tTypeParent " + tTypeParent.Name);
			Type tTypeDefintion = null;
			if (tTypeParent.IsGenericType) {
				tTypeDefintion = tTypeParent.GetGenericArguments ()[0];
				//Debug.Log ("tTypeDefintion " + tTypeDefintion.Name);
				string tTargetReference = property.FindPropertyRelative ("Reference").stringValue;
				//bool tConnection = true;
				//if (tTargetReference != null && tTargetReference != "")
				//{
				//	if (NWDBasis<K>.InstanceByReference(Value) == null)
				//	{
				//		tConnection = false;
				//	}
				//}
				//EditorGUI.BeginDisabledGroup(!tConnection);
				var tMethodInfo = tTypeDefintion.GetMethod ("ReferenceConnectionFieldSerialized", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null)
				{
					tMethodInfo.Invoke (null, new object[]{position, property.displayName, property, string.Empty, tReferenceConnection.ShowInspector, tReferenceConnection.Editable, tReferenceConnection.EditButton, tReferenceConnection.NewButton});
				}
			//EditorGUI.EndDisabledGroup();
			//if (tConnection == false)
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
}
//=====================================================================================================================
#endif