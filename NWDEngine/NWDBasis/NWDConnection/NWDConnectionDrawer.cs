//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================

using System;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDConnectionDrawer draw the control field in the inspector.
    /// </summary>
	[CustomPropertyDrawer (typeof(NWDBasisConnection), true)]
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
			Type tType = Type.GetType ("NetWorkedData."+property.type);
			Type tTypeParent = tType.BaseType;
			if (tTypeParent.IsGenericType) {
                Type tTypeDefintion = tTypeParent.GetGenericArguments ()[0];
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tTypeDefintion);
                tHeight = tHelper.ReferenceConnectionHeightSerialized(property, tReferenceConnection.ShowInspector);
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
			if (tTypeParent.IsGenericType) 
            {
                Type tTypeDefintion = tTypeParent.GetGenericArguments ()[0];
                NWDBasisHelper tHelper = NWDBasisHelper.FindTypeInfos(tTypeDefintion);
                tHelper.ReferenceConnectionFieldSerialized(position, property.displayName, property, string.Empty, tReferenceConnection.ShowInspector);
            }
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
