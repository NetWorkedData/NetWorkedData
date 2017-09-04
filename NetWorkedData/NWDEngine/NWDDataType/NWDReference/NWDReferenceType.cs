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
			if (sReference == null) 
			{
				sReference =  "";
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
		public K GetObject ()
		{
			return NWDBasis <K>.GetObjectByReference (Value) as K;
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
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
//			Debug.Log ("Je suis dans l'invocation de hauteur");
			GUIStyle tPopupdStyle = new GUIStyle (EditorStyles.popup);
			 float tHeight = tPopupdStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			return tHeight;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override object ControlField (Rect sPosition, string sEntitled)
		{
			NWDReferenceType<K> tTemporary = new NWDReferenceType<K> ();
			tTemporary.Value = Value;
			Type sFromType = typeof(K);
			float tWidth = sPosition.width;
			float tHeight = sPosition.height;
			float tX = sPosition.position.x;
			float tY = sPosition.position.y;

			float tEditWidth = NWDConstants.kEditWidth;

			GUIStyle tPopupdStyle = new GUIStyle (EditorStyles.popup);
			tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight (new GUIContent ("A"), tWidth);
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
			if (tIndex >= 0) {
				if (GUI.Button (new Rect (tX + tWidth - tEditWidth, tY, tEditWidth, tPopupdStyle.fixedHeight), "!")) {
					NWDBasis<K>.SetObjectInEdition (NWDBasis<K>.InstanceByReference (tReferenceList.ElementAt (rIndex)),false);
				}
			}
			if (rIndex != tIndex) 
			{
				string tNextValue = tReferenceList.ElementAt (rIndex);
				tNextValue = tNextValue.Trim (NWDConstants.kFieldSeparatorA.ToCharArray () [0]);
				tTemporary.Value = tNextValue;
			}
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
		public bool ChangeReferenceForAnother(string sOldReference, string sNewReference)
		{
			bool rReturn = false;
			if (Value != null) {
				if (Value.Contains (sOldReference)) {
					Debug.Log ("I CHANGE "+sOldReference+" FOR "+sNewReference+"");
					Value = Value.Replace (sOldReference, sNewReference);
					rReturn = true;
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================