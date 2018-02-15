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
	public class NWDReferenceType<K>: BTBDataType where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDReferenceType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool ContainsReference (string sReference)
		{
			if (sReference == null) {
				return false;
			}
			return Value.Contains (sReference);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetReference (string sReference)
		{
			if (sReference == null) {
				sReference = "";
			}
			Value = sReference;
		}
		//-------------------------------------------------------------------------------------------------------------
		public string GetReference ()
		{
			if (Value == null) {
				return "";
			}
			return Value;
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool ContainsObject (K sObject)
		{
			if (sObject == null) {
				return false;
			}
			return Value.Contains (sObject.Reference);
		}
		//-------------------------------------------------------------------------------------------------------------
        public K GetObject (string sAccountReference = null)
		{
            return NWDBasis <K>.GetObjectByReference (Value,sAccountReference) as K;
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] GetObjects(string sAccountReference = null)
        {
            return new K[] { NWDBasis<K>.GetObjectByReference(Value,sAccountReference) as K };
        }
		//-------------------------------------------------------------------------------------------------------------
		public void SetObject (K sObject)
		{
			if (sObject != null) {
				Value = sObject.Reference;
			} else {
				Value = "";
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public List<string> ReferenceInError( List<string> sReferencesList) {
			List<string> rReturn = new List<string> ();
			foreach (string tReference in sReferencesList) {
				if (NWDBasis<K>.InstanceByReference (tReference) == null) {
					rReturn.Add (tReference);
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
        #if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public K[] EditorGetObjects()
        {
            return new K[] { NWDBasis<K>.InstanceByReference(Value) as K };
        }
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
//			Debug.Log ("Je suis dans l'invocation de hauteur");

			int tConnexion = 0;
			if (Value != null && Value != "") {
				if (NWDBasis<K>.InstanceByReference (Value) == null) {
					tConnexion = 1;
				}
			}
			float tWidth = 100.0F;
			GUIStyle tPopupdStyle = new GUIStyle (EditorStyles.popup);
			tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight (new GUIContent ("A"), tWidth);
			GUIStyle tLabelStyle = new GUIStyle (EditorStyles.label);
			tLabelStyle.fixedHeight = tLabelStyle.CalcHeight (new GUIContent ("A"), tWidth);
			tLabelStyle.normal.textColor = Color.red;
			GUIStyle tLabelAssetStyle = new GUIStyle (EditorStyles.label);
			tLabelAssetStyle.fontSize = 9;
			tLabelAssetStyle.fixedHeight = tLabelAssetStyle.CalcHeight (new GUIContent ("A"), tWidth);
			tLabelAssetStyle.normal.textColor = Color.gray;
			GUIStyle tMiniButtonStyle = new GUIStyle (EditorStyles.miniButton);
			tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight (new GUIContent ("A"), tWidth);
			float tHeight = tPopupdStyle.fixedHeight + tConnexion * (tLabelStyle.fixedHeight + NWDConstants.kFieldMarge +
				//tLabelAssetStyle.fixedHeight+NWDConstants.kFieldMarge+
			                tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge);

			// test if error in reference and add button height
			if (Value != null && Value != "") {
				if (ReferenceInError (new List<string> (Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries))).Count > 0) {
					tHeight = tHeight + tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
				}
			}
			return tHeight;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override object ControlField (Rect sPosition, string sEntitled)
		{
            NWDConstants.LoadImages();
            NWDConstants.LoadStyles();
			NWDReferenceType<K> tTemporary = new NWDReferenceType<K> ();
			tTemporary.Value = Value;

			Type sFromType = typeof(K);
			float tWidth = sPosition.width;
			float tHeight = sPosition.height;
			float tX = sPosition.position.x;
			float tY = sPosition.position.y;

			float tEditWidth = NWDConstants.kEditWidth;


			bool tConnexion = true;
			if (Value != null && Value != "") {
				if (NWDBasis<K>.InstanceByReference (Value) == null) {
					tConnexion = false;
				}
			}

			GUIStyle tPopupdStyle = new GUIStyle (EditorStyles.popup);
			tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight (new GUIContent ("A"), tWidth);
			GUIStyle tLabelStyle = new GUIStyle (EditorStyles.label);
			tLabelStyle.fixedHeight = tLabelStyle.CalcHeight (new GUIContent ("A"), tWidth);
			tLabelStyle.normal.textColor = Color.red;
			GUIStyle tLabelAssetStyle = new GUIStyle (EditorStyles.label);
			tLabelAssetStyle.fontSize = 9;
			tLabelAssetStyle.fixedHeight = tLabelAssetStyle.CalcHeight (new GUIContent ("A"), tWidth);
			tLabelAssetStyle.normal.textColor = Color.red;
			GUIStyle tMiniButtonStyle = new GUIStyle (EditorStyles.miniButton);
			tMiniButtonStyle.fixedHeight = tMiniButtonStyle.CalcHeight (new GUIContent ("A"), tWidth);

			EditorGUI.BeginDisabledGroup (!tConnexion);
			List<string> tReferenceList = new List<string> ();
			List<string> tInternalNameList = new List<string> ();
			tReferenceList.Add (NWDConstants.kFieldSeparatorA);
			tInternalNameList.Add (" ");
			var tReferenceListInfo = sFromType.GetField ("ObjectsByReferenceList", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
			if (tReferenceListInfo != null) {
				tReferenceList.AddRange (tReferenceListInfo.GetValue (null) as List<string>);
			}

			var tInternalNameListInfo = sFromType.GetField ("ObjectsInEditorTableKeyList", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
			if (tInternalNameListInfo != null) {
				tInternalNameList.AddRange (tInternalNameListInfo.GetValue (null) as List<string>);
			}

			int tIndex = tReferenceList.IndexOf (Value);
			int rIndex = EditorGUI.Popup (new Rect (tX, tY, tWidth - NWDConstants.kFieldMarge - tEditWidth, tPopupdStyle.fixedHeight), sEntitled, tIndex, tInternalNameList.ToArray (), tPopupdStyle);

			if (tConnexion == false) {
				GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth + NWDConstants.kFieldMarge, tY + 1, tWidth - EditorGUIUtility.labelWidth - NWDConstants.kFieldMarge * 4 - tEditWidth, tLabelAssetStyle.fixedHeight), "? <" + Value + ">", tLabelAssetStyle);

			}	
			
			if (tIndex >= 0) {
                //if (GUI.Button(new Rect(tX + tWidth - tEditWidth, tY, tEditWidth, tPopupdStyle.fixedHeight), "!"))
                GUIContent tDeleteContent = new GUIContent(NWDConstants.kImageTabReduce, "edit");
                if (GUI.Button(new Rect(tX + tWidth - tEditWidth, tY, tEditWidth, tPopupdStyle.fixedHeight), tDeleteContent, NWDConstants.StyleMiniButton))
                {
					NWDBasis<K>.SetObjectInEdition (NWDBasis<K>.InstanceByReference (tReferenceList.ElementAt (rIndex)), false);
				}
			}

			if (rIndex != tIndex) {
				string tNextValue = tReferenceList.ElementAt (rIndex);
				tNextValue = tNextValue.Trim (NWDConstants.kFieldSeparatorA.ToCharArray () [0]);
				tTemporary.Value = tNextValue;
			}

			tY = tY + NWDConstants.kFieldMarge + tPopupdStyle.fixedHeight;
			EditorGUI.EndDisabledGroup ();

			if (tConnexion == false) {
				tTemporary.Value = Value;

				GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_ERROR, tLabelStyle);
				tY = tY + NWDConstants.kFieldMarge + tLabelStyle.fixedHeight;
//				GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelAssetStyle.fixedHeight), Value.Replace (NWDAssetType.kAssetDelimiter, ""),tLabelAssetStyle);
//				tY = tY + NWDConstants.kFieldMarge + tLabelAssetStyle.fixedHeight;
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
		public string ChangeReferenceForAnother (string sOldReference, string sNewReference)
		{
			string rReturn = "NO";
			if (Value != null) {
				if (Value.Contains (sOldReference)) {
					Debug.Log ("I CHANGE " + sOldReference + " FOR " + sNewReference + "");
					Value = Value.Replace (sOldReference, sNewReference);
					rReturn = "YES";
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================