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

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDPrefabType : NWDUnityType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDPrefabType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDPrefabType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public GameObject ToGameObject ()
		{
			GameObject tObject = null;
			if (Value != null && Value != "") {
				#if UNITY_EDITOR
				tObject = AssetDatabase.LoadAssetAtPath (Value, typeof(GameObject)) as GameObject;
				#else
				tObject = Resources.Load (Value, typeof(GameObject)) as GameObject;
				#endif
			}
			return tObject;
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
			int tAdd = 0;
			if (Value != "") 
			{
				tAdd = 1;
			}
			GUIStyle tObjectFieldStyle = new GUIStyle (EditorStyles.objectField);
			float tHeight = tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight (new GUIContent ("A"), 100.0f) + tAdd * NWDConstants.kPrefabSize;
			return tHeight;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override object ControlField (Rect sPosition, string sEntitled)
		{
			NWDPrefabType tTemporary = new NWDPrefabType ();
			tTemporary.Value = Value;

			float tWidth = sPosition.width;
			float tHeight = sPosition.height;
			float tX = sPosition.position.x;
			float tY = sPosition.position.y;

			GUIStyle tObjectFieldStyle = new GUIStyle (EditorStyles.objectField);
			tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight (new GUIContent ("A"), tWidth);

			GameObject tObject = null;
			if (Value != null && Value != "") {
				tObject = AssetDatabase.LoadAssetAtPath (Value, typeof(GameObject)) as GameObject;
			}
			//Editor tGameObjectEditor = Editor.CreateEditor (tObject);

			Texture2D tTexture2D = AssetPreview.GetAssetPreview (tObject);
			if (tTexture2D != null) {
				EditorGUI.DrawPreviewTexture (new Rect (tWidth - NWDConstants.kPrefabSize, tY + tObjectFieldStyle.fixedHeight, NWDConstants.kPrefabSize, NWDConstants.kPrefabSize), tTexture2D);
			}
			UnityEngine.Object pObj = EditorGUI.ObjectField (new Rect (tX, tY, tWidth, tObjectFieldStyle.fixedHeight), sEntitled, (UnityEngine.Object)tObject, typeof(GameObject), false);
			if (pObj != null) 
			{
				if (PrefabUtility.GetPrefabType (pObj) == PrefabType.Prefab) 
				{
					tTemporary.Value = AssetDatabase.GetAssetPath (PrefabUtility.GetPrefabObject (pObj));
				}
			} 
			else 
			{
				tTemporary.Value = "";
			}
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================