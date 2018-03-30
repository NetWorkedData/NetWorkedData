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
	public class NWDSpriteType : NWDAssetType
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
            Sprite rSprite = null;
            Debug.LogWarning("rSprite Value " + Value);
			if (Value != null && Value != "") {
				string tPath = Value.Replace (NWDAssetType.kAssetDelimiter, "");
				#if UNITY_EDITOR
				rSprite = AssetDatabase.LoadAssetAtPath (tPath, typeof(Sprite)) as Sprite;
                #else
                //tPath = tPath.Replace("Assets/Resources/", "");
                tPath = Path.GetFileNameWithoutExtension(tPath);
                rSprite = Resources.Load (tPath, typeof(Sprite)) as Sprite;
                #endif
                Debug.LogWarning("rSprite at path " + tPath);
                if (rSprite == null)
                {
                    Debug.LogWarning("rSprite is null at path " + tPath);
                }
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
			tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			GUIStyle tLabelStyle = new GUIStyle (EditorStyles.label);
			tLabelStyle.fixedHeight = tLabelStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			GUIStyle tMiniButtonStyle = new GUIStyle (EditorStyles.miniButton);
			tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			GUIStyle tLabelAssetStyle = new GUIStyle (EditorStyles.label);
			tLabelAssetStyle.fontSize = 12;
			tLabelAssetStyle.fixedHeight = tLabelAssetStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			tLabelAssetStyle.normal.textColor = Color.gray;

			return tObjectFieldStyle.fixedHeight + tAdd * (NWDConstants.kPrefabSize + NWDConstants.kFieldMarge);
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, string sTooltips = "")
		{
            NWDSpriteType tTemporary = new NWDSpriteType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

			tTemporary.Value = Value;

			float tWidth = sPosition.width;
			float tHeight = sPosition.height;
			float tX = sPosition.position.x;
			float tY = sPosition.position.y;

			GUIStyle tObjectFieldStyle = new GUIStyle (EditorStyles.objectField);
			tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight (new GUIContent ("A"), tWidth);
			GUIStyle tLabelStyle = new GUIStyle (EditorStyles.label);
			tLabelStyle.fixedHeight = tLabelStyle.CalcHeight (new GUIContent ("A"), tWidth);
			tLabelStyle.normal.textColor = Color.red;
			GUIStyle tLabelAssetStyle = new GUIStyle (EditorStyles.label);
			tLabelAssetStyle.fontSize = 12;
			tLabelAssetStyle.fixedHeight = tLabelAssetStyle.CalcHeight (new GUIContent ("A"), tWidth);
			tLabelAssetStyle.normal.textColor = Color.gray;
			GUIStyle tMiniButtonStyle = new GUIStyle (EditorStyles.miniButton);
			tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight (new GUIContent ("A"), tWidth);

			Sprite tObject = null;

			bool tRessource = true;

			if (Value != null && Value != "") {
				string tPath = Value.Replace (NWDAssetType.kAssetDelimiter, "");
				tObject = AssetDatabase.LoadAssetAtPath (tPath, typeof(Sprite)) as Sprite;
				if (tObject == null) {
					tRessource = false;
				} else {

					Texture2D tTexture2D = AssetPreview.GetAssetPreview (tObject);
					if (tTexture2D != null) {
						EditorGUI.DrawPreviewTexture (new Rect (tX + EditorGUIUtility.labelWidth, tY+NWDConstants.kFieldMarge+tObjectFieldStyle.fixedHeight, NWDConstants.kPrefabSize, NWDConstants.kPrefabSize)
							, tTexture2D);
					}
				}
                if (Value.Contains("Resources") == false)
                {
                    GUI.Label(new Rect(tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelAssetStyle.fixedHeight), "NOT IN Resurces FOLDER", tLabelAssetStyle);
                    tY = tY + NWDConstants.kFieldMarge + tLabelAssetStyle.fixedHeight;
                }
			}
			EditorGUI.BeginDisabledGroup (!tRessource);
            UnityEngine.Object pObj = EditorGUI.ObjectField (new Rect (tX, tY, tWidth, tObjectFieldStyle.fixedHeight), tContent, tObject, typeof(Sprite), false);
			tY = tY + NWDConstants.kFieldMarge + tObjectFieldStyle.fixedHeight;
			if (pObj != null) {
				tTemporary.Value = NWDAssetType.kAssetDelimiter+AssetDatabase.GetAssetPath (pObj)+NWDAssetType.kAssetDelimiter;
			} else {
				tTemporary.Value = "";
			}
			EditorGUI.EndDisabledGroup ();
			if (tRessource == true) {
			} else {
				tTemporary.Value = Value;

				GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ASSET_MUST_BE_DOWNLOAD, tLabelStyle);
				tY = tY + NWDConstants.kFieldMarge + tLabelStyle.fixedHeight;
				GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelAssetStyle.fixedHeight), Value.Replace (NWDAssetType.kAssetDelimiter, ""),tLabelAssetStyle);
				tY = tY + NWDConstants.kFieldMarge + tLabelAssetStyle.fixedHeight;
				Color tOldColor = GUI.backgroundColor;
				GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
				if (GUI.Button (new Rect (tX + EditorGUIUtility.labelWidth, tY, 60.0F, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_CLEAN, tMiniButtonStyle)) {
					tTemporary.Value = "";
				}
				GUI.backgroundColor = tOldColor;
				tY = tY + NWDConstants.kFieldMarge + tMiniButtonStyle.fixedHeight;
			}
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================