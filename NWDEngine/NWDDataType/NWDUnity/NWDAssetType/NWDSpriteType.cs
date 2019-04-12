// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:28:54
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
	public class NWDSpriteType : NWDAssetType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDSpriteType ()
		{
			Value = string.Empty;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDSpriteType (string sValue = BTBConstants.K_EMPTY_STRING)
		{
			if (sValue == null) {
				Value = string.Empty;
			} else {
				Value = sValue;
			}
        }
        //-------------------------------------------------------------------------------------------------------------
        public Sprite ToSpriteAsync(Sprite sInterim, NWDOperationSpriteDelegate sDelegate)
        {
            string tPath = Value.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
            tPath = BTBPathResources.PathAbsoluteToPathDB(tPath);
            NWDOperationSprite tOperation = NWDOperationSprite.AddOperation(tPath, sInterim, false, sDelegate);
            return tOperation.Interim;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Sprite ToSprite ()
		{
            Sprite rSprite = null;
			if (!string.IsNullOrEmpty(Value))
            {
				string tPath = Value.Replace(kAssetDelimiter, string.Empty);
				#if UNITY_EDITOR
				rSprite = AssetDatabase.LoadAssetAtPath(tPath, typeof(Sprite)) as Sprite;
                #else
                tPath = BTBPathResources.PathAbsoluteToPathDB(tPath);
                rSprite = Resources.Load (tPath, typeof(Sprite)) as Sprite;
                #endif
                if (rSprite == null)
                {
                    //Debug.LogWarning("rSprite is null at path " + tPath);
                }
			}
			return rSprite;
		}
		//-------------------------------------------------------------------------------------------------------------
        #if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override bool ErrorAnalyze()
        {
            bool rReturn = false;
            if (string.IsNullOrEmpty(Value) == false)
            {
                if (Value.Contains(NWD.K_Resources) == false)
                {
                    rReturn = true;
                }
                else
                {
                    string tPath = Value.Replace(kAssetDelimiter, string.Empty);
                    Sprite tObject = AssetDatabase.LoadAssetAtPath(tPath, typeof(Sprite)) as Sprite;
                    if (tObject == null)
                    {
                        rReturn = true;
                    }
                }
            }
            InError = rReturn;
            return rReturn;
        }
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
			int tAdd = 0;
			if (Value != string.Empty) 
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

			return tObjectFieldStyle.fixedHeight + tAdd * (NWDGUI.kPrefabSize + NWDGUI.kFieldMarge);
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING)
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

			if (Value != null && Value != string.Empty) {
				string tPath = Value.Replace (NWDAssetType.kAssetDelimiter, string.Empty);
				tObject = AssetDatabase.LoadAssetAtPath (tPath, typeof(Sprite)) as Sprite;
				if (tObject == null) {
					tRessource = false;
				} else {
					//Texture2D tTexture2D = AssetPreview.GetAssetPreview (tObject);
					//if (tTexture2D != null) {
					//	EditorGUI.DrawPreviewTexture (new Rect (tX + EditorGUIUtility.labelWidth, tY+NWDGUI.kFieldMarge+tObjectFieldStyle.fixedHeight, NWDGUI.kPrefabSize, NWDGUI.kPrefabSize)
					//		, tTexture2D);
					//}
                    GUI.DrawTexture(new Rect(tX + EditorGUIUtility.labelWidth, tY + NWDGUI.kFieldMarge + tObjectFieldStyle.fixedHeight, NWDGUI.kPrefabSize, NWDGUI.kPrefabSize),
                     tObject.texture, ScaleMode.ScaleToFit, true);
                }
                if (Value.Contains(NWD.K_Resources) == false)
                {
                    EditorGUI.LabelField(new Rect(tX, tY + tLabelAssetStyle.fixedHeight, tWidth, tLabelAssetStyle.fixedHeight), "NOT IN \"Resources\"", tLabelStyle);
                }
			}
			EditorGUI.BeginDisabledGroup (!tRessource);
            UnityEngine.Object pObj = EditorGUI.ObjectField (new Rect (tX, tY, tWidth, tObjectFieldStyle.fixedHeight), tContent, tObject, typeof(Sprite), false);
			tY = tY + NWDGUI.kFieldMarge + tObjectFieldStyle.fixedHeight;
			if (pObj != null) {
				tTemporary.Value = NWDAssetType.kAssetDelimiter+AssetDatabase.GetAssetPath (pObj)+NWDAssetType.kAssetDelimiter;
			} else {
				tTemporary.Value = string.Empty;
			}
			EditorGUI.EndDisabledGroup ();
			if (tRessource == true) {
			} else {
				tTemporary.Value = Value;

				GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ASSET_MUST_BE_DOWNLOAD, tLabelStyle);
				tY = tY + NWDGUI.kFieldMarge + tLabelStyle.fixedHeight;
				GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelAssetStyle.fixedHeight), Value.Replace (NWDAssetType.kAssetDelimiter, ""),tLabelAssetStyle);
				tY = tY + NWDGUI.kFieldMarge + tLabelAssetStyle.fixedHeight;
                NWDGUI.BeginRedArea();
                if (GUI.Button (new Rect (tX + EditorGUIUtility.labelWidth, tY, 60.0F, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_CLEAN, tMiniButtonStyle)) {
					tTemporary.Value = string.Empty;
                }
                NWDGUI.EndRedArea();
                tY = tY + NWDGUI.kFieldMarge + tMiniButtonStyle.fixedHeight;
			}
			return tTemporary;
		}
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================