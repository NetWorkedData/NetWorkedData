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
	public class NWDPrefabType : NWDAssetType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDPrefabType ()
		{
			Value = string.Empty;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDPrefabType (string sValue = BTBConstants.K_EMPTY_STRING)
		{
			if (sValue == null) {
				Value = string.Empty;
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public GameObject ToPrefab ()
		{
			GameObject tObject = null;
            if (Value != null && Value != string.Empty)
            {
                string tPath = Value.Replace(NWDAssetType.kAssetDelimiter, string.Empty);

#if UNITY_EDITOR
                tObject = AssetDatabase.LoadAssetAtPath(tPath, typeof(GameObject)) as GameObject;
#else
                tPath = BTBPathResources.PathAbsoluteToPathDB(tPath);
                tObject = Resources.Load (tPath, typeof(GameObject)) as GameObject;
                #endif
                //Debug.LogWarning("tObject at path " + tPath);
            }
			return tObject;
		}
		//-------------------------------------------------------------------------------------------------------------
		public GameObject ToGameObject (GameObject sParent = null)
		{
			GameObject rReturn = null;
			GameObject tPrefab = ToPrefab ();
			if (tPrefab != null) {
				rReturn = UnityEngine.Object.Instantiate (ToPrefab ());
				if (sParent != null) {
					rReturn.transform.SetParent (sParent.transform);
				}
			}
			return rReturn;
		}
        //-------------------------------------------------------------------------------------------------------------
        public bool IsEmpty()
        {
            return (Value == string.Empty);
        }
        //-------------------------------------------------------------------------------------------------------------
        #if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsInError()
        {
            bool rReturn = false;
            if (string.IsNullOrEmpty(Value) == false)
            {
                if (Value.Contains("Resources") == false)
                {
                    rReturn = true;
                }
                else
                {
                    string tPath = Value.Replace(NWDAssetType.kAssetDelimiter, string.Empty);
                    GameObject tObject = AssetDatabase.LoadAssetAtPath(tPath, typeof(GameObject)) as GameObject;
                    if (tObject == null)
                    {
                        rReturn = true;
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight ()
		{
			int tAdd = 0;
			if (Value != string.Empty) {
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
        public override object ControlField (Rect sPosition, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING)
		{
            NWDPrefabType tTemporary = new NWDPrefabType ();
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

			GameObject tObject = null;

			bool tRessource = true;

			if (Value != null && Value != string.Empty) {
				string tPath = Value.Replace (NWDAssetType.kAssetDelimiter, string.Empty);
				tObject = AssetDatabase.LoadAssetAtPath (tPath, typeof(GameObject)) as GameObject;
				if (tObject == null) {
					tRessource = false;
				} else {
					Texture2D tTexture2D = AssetPreview.GetAssetPreview (tObject);
					if (tTexture2D != null) {
						EditorGUI.DrawPreviewTexture (new Rect (tX + EditorGUIUtility.labelWidth, tY + NWDConstants.kFieldMarge + tObjectFieldStyle.fixedHeight, NWDConstants.kPrefabSize, NWDConstants.kPrefabSize)
							, tTexture2D);
					}
                }
                if (Value.Contains("Resources") == false)
                {
                    EditorGUI.LabelField(new Rect(tX, tY + tLabelAssetStyle.fixedHeight, tWidth, tLabelAssetStyle.fixedHeight), NWDConstants.K_NOT_IN_RESOURCES_FOLDER, tLabelStyle);
                }
			}

			EditorGUI.BeginDisabledGroup (!tRessource);
            UnityEngine.Object pObj = EditorGUI.ObjectField (new Rect (tX, tY, tWidth, tObjectFieldStyle.fixedHeight), tContent, (UnityEngine.Object)tObject, typeof(GameObject), false);
			tY = tY + NWDConstants.kFieldMarge + tObjectFieldStyle.fixedHeight;
			if (pObj != null) {
                //if (PrefabUtility.GetPrefabType (pObj) == PrefabType.Prefab) 
                if (PrefabUtility.GetPrefabInstanceStatus(pObj) == PrefabInstanceStatus.Connected)
                {
					tTemporary.Value = NWDAssetType.kAssetDelimiter + AssetDatabase.GetAssetPath (PrefabUtility.GetPrefabInstanceHandle(pObj)) + NWDAssetType.kAssetDelimiter;
				}
			} else {
				tTemporary.Value = string.Empty;
			}
			EditorGUI.EndDisabledGroup ();
			if (tRessource == true) {
			} else {
				tTemporary.Value = Value;

				GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_ASSET_MUST_BE_DOWNLOAD, tLabelStyle);
				tY = tY + NWDConstants.kFieldMarge + tLabelStyle.fixedHeight;
				GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelAssetStyle.fixedHeight), Value.Replace (NWDAssetType.kAssetDelimiter, ""), tLabelAssetStyle);
				tY = tY + NWDConstants.kFieldMarge + tLabelAssetStyle.fixedHeight;
				Color tOldColor = GUI.backgroundColor;
				GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
				if (GUI.Button (new Rect (tX + EditorGUIUtility.labelWidth, tY, 60.0F, tMiniButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_CLEAN, tMiniButtonStyle)) {
					tTemporary.Value = string.Empty;
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