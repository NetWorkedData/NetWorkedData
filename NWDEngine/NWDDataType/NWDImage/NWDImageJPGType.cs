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
	public class NWDImageJPGType : NWDAssetType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDImageJPGType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDImageJPGType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		private bool TryParseBase64 (string sEncodedText)
		{
			try {
				//byte[] textAsBytes = Convert.FromBase64String (sEncodedText);
                Convert.FromBase64String(sEncodedText);
				return true;
			} catch (Exception) {
				return false;   
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public Texture2D ToTexture ()
		{
			if (Value != null && Value != "") {
				try {
					byte[] tDecodedBytes = Convert.FromBase64String (Value);
					if (tDecodedBytes != null) {
						Texture2D tTexture = new Texture2D (2, 2);
						tTexture.LoadImage (tDecodedBytes);
						return tTexture;
					} else {
						return null;   
					}
				} catch (Exception) {
					return null;   
				}
			} else {
				return null;   
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetTexture (Texture2D sTexture)
		{
			if (sTexture != null) {
				byte[] tByteOfPicture = sTexture.EncodeToJPG ();
				if (tByteOfPicture != null) {
					Value = Convert.ToBase64String (tByteOfPicture);
				}
			} else {
				Value = "";
			}
		}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Convert to sprite.
        /// </summary>
        /// <returns>The sprite.</returns>
        public Sprite ToSprite()
        {
            Texture2D tTexture = ToTexture();
            if (tTexture != null)
            {
                return Sprite.Create(tTexture, new Rect(0, 0, tTexture.width, tTexture.height), new Vector2(0.5f, 0.5f));
            }

            return null;
        }
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
			int tAdd = 0;
			if (Value != "") {
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
            NWDImageJPGType tTemporary = new NWDImageJPGType ();
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
			//bool tNoError = true;

			Texture2D tTexture = tTemporary.ToTexture ();
			if (Value != null && Value != "" && tTexture == null) {
                EditorGUI.LabelField (new Rect (tX, tY, tWidth, tObjectFieldStyle.fixedHeight), tContent);
				Color tOldColor = GUI.backgroundColor;
				GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
				if (GUI.Button (new Rect (tX + EditorGUIUtility.labelWidth, tY, 60.0F, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_CLEAN, tMiniButtonStyle)) {
					tTemporary.Value = "";
				}
				GUI.backgroundColor = tOldColor;
			} else if (Value != null && Value != "") {
						
				if (tTexture != null) {
					EditorGUI.DrawPreviewTexture (new Rect (tX + EditorGUIUtility.labelWidth, tY + NWDConstants.kFieldMarge + tObjectFieldStyle.fixedHeight, NWDConstants.kPrefabSize, NWDConstants.kPrefabSize)
						, tTexture);
				}
				EditorGUI.LabelField (new Rect (tX, tY, tWidth, tObjectFieldStyle.fixedHeight), sEntitled);
				Color tOldColor = GUI.backgroundColor;
				GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
				if (GUI.Button (new Rect (tX + EditorGUIUtility.labelWidth, tY, 60.0F, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_CLEAN, tMiniButtonStyle)) {
					tTemporary.Value = "";
				}
				GUI.backgroundColor = tOldColor;
			} else {
				UnityEngine.Object tObjSprite = EditorGUI.ObjectField (new Rect (tX, tY, tWidth, tObjectFieldStyle.fixedHeight), sEntitled, tObject, typeof(Texture2D), false);
				tY = tY + NWDConstants.kFieldMarge + tObjectFieldStyle.fixedHeight;
				if (tObjSprite != null) {
					Texture2D tNewTexture = AssetPreview.GetAssetPreview (tObjSprite);
					tTemporary.SetTexture (tNewTexture);
				} else {
					//tTemporary.Value = "";
				}
			}
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================