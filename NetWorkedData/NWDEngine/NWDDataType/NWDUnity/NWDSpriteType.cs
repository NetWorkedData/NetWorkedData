﻿using System;
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
	//-------------------------------------------------------------------------------------------------------------
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDSpriteType : NWDUnityType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDSpriteType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDSpriteType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public Sprite ToSprite ()
		{
			Sprite tObject = null;
			if (Value != null && Value != "") {
				#if UNITY_EDITOR
				tObject = AssetDatabase.LoadAssetAtPath (Value, typeof(Sprite)) as Sprite;
				#else
				tObject = Resources.Load (Value, typeof(Sprite)) as Sprite;
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
			NWDSpriteType tTemporary = new NWDSpriteType ();
			float tWidth = sPosition.width;
			float tHeight = sPosition.height;
			float tX = sPosition.position.x;
			float tY = sPosition.position.y;
			GUIStyle tObjectFieldStyle = new GUIStyle (EditorStyles.objectField);
			tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight (new GUIContent ("A"), tWidth);
			Sprite tObject = null;
			if (Value != null && Value != "") {
				tObject = AssetDatabase.LoadAssetAtPath (Value, typeof(Sprite)) as Sprite;
			}
			UnityEngine.Object pObj = EditorGUI.ObjectField (new Rect (tX, tY, tWidth, tObjectFieldStyle.fixedHeight), sEntitled, tObject, typeof(Sprite), false);
			if (pObj != null) {
				tTemporary.Value = AssetDatabase.GetAssetPath (pObj);
			} else {
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