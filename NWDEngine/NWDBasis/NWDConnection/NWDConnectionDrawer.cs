// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:25:42
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
                // DID : Change to remove invoke!
                //            MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicStaticFlattenHierarchy(tTypeDefintion, NWDConstants.M_ReferenceConnectionHeightSerializedString);
                //            //var tMethodInfo = tTypeDefintion.GetMethod ("ReferenceConnectionHeightSerializedString", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                //if (tMethodInfo != null)
                //{
                //	string tHeightString = tMethodInfo.Invoke (null, new object[]{property, tReferenceConnection.ShowInspector}) as string;
                //	float.TryParse (tHeightString, out tHeight);
                //}

                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tTypeDefintion);

                tHeight = tHelper.New_ReferenceConnectionHeightSerialized(property, tReferenceConnection.ShowInspector);
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
			Type tType = Type.GetType ("NetWorkedData."+property.type);
			Type tTypeParent = tType.BaseType;
			Type tTypeDefintion = null;
			if (tTypeParent.IsGenericType) 
            {
				tTypeDefintion = tTypeParent.GetGenericArguments ()[0];
				string tTargetReference = property.FindPropertyRelative ("Reference").stringValue;
    //            MethodInfo tMethodInfo = NWDAliasMethod.GetMethodPublicStaticFlattenHierarchy(tTypeDefintion, NWDConstants.M_ReferenceConnectionFieldSerialized);
    //            if (tMethodInfo != null)
				//{
				//	tMethodInfo.Invoke (null, new object[]{position, property.displayName, property, string.Empty, tReferenceConnection.ShowInspector});
				//}

                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tTypeDefintion);
                tHelper.New_ReferenceConnectionFieldSerialized(position, property.displayName, property, string.Empty, tReferenceConnection.ShowInspector);
            }
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif