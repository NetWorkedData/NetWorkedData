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
	//-------------------------------------------------------------------------------------------------------------
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDTextureType : NWDUnityType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDTextureType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDTextureType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public Texture2D ToTexture()
		{
			Texture2D rTexture = null;
			if (Value != null && Value != "") {
                #if UNITY_EDITOR
                rTexture = AssetDatabase.LoadAssetAtPath (Value, typeof(Texture2D)) as Texture2D;
                #else
				rTexture = Resources.Load (Value, typeof(Texture2D)) as Texture2D;
                #endif
            }
            return rTexture;
		}
        //-------------------------------------------------------------------------------------------------------------
        public Sprite ToSprite()
        {
            Texture2D tSprite = ToTexture();
            Sprite rSprite = null;
            if (tSprite != null)
            {
                rSprite = Sprite.Create(tSprite, new Rect(0, 0, tSprite.width, tSprite.height), new Vector2(0.5f, 0.5f));
            }
            return rSprite;
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
			NWDTextureType tTemporary = new NWDTextureType ();
			float tWidth = sPosition.width;
			float tHeight = sPosition.height;
			float tX = sPosition.position.x;
			float tY = sPosition.position.y;
			GUIStyle tObjectFieldStyle = new GUIStyle (EditorStyles.objectField);
			tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight (new GUIContent ("A"), tWidth);
			Texture2D tObject = null;
			if (Value != null && Value != "") {
				tObject = AssetDatabase.LoadAssetAtPath (Value, typeof(Texture2D)) as Texture2D;
			}
			UnityEngine.Object pObj = EditorGUI.ObjectField (new Rect (tX, tY, tWidth, tObjectFieldStyle.fixedHeight), sEntitled, tObject, typeof(Texture2D), false);
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