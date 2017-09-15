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
	public class NWDReferencesListType<K>: BTBDataType where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDReferencesListType ()
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
		public void SetReferences (string[] sReferences)
		{
			List<string> tList = new List<string> ();
			foreach (string tReference in sReferences) {
				tList.Add (tReference);
			}
			string[] tNextValueArray = tList.Distinct ().ToArray ();
			Value = string.Join (NWDConstants.kFieldSeparatorA, tNextValueArray);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void AddReferences (string[] sReferences)
		{
			List<string> tList = new List<string> ();
			if (Value != null && Value != "") 
			{
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				tList = new List<string> (tValueArray);
			}
			foreach (string tReference in sReferences) {
				tList.Add (tReference);
			}
			string[] tNextValueArray = tList.Distinct ().ToArray ();
			Value = string.Join (NWDConstants.kFieldSeparatorA, tNextValueArray);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void RemoveReferences (string[] sReferences)
		{
			List<string> tList = new List<string> ();
			if (Value != null && Value != "") 
			{
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				tList = new List<string> (tValueArray);
			}
			foreach (string tReference in sReferences) {
				tList.Remove (tReference);
			}
			string[] tNextValueArray = tList.Distinct ().ToArray ();
			Value = string.Join (NWDConstants.kFieldSeparatorA, tNextValueArray);
		}
		//-------------------------------------------------------------------------------------------------------------
		public string[] GetReferences ()
		{
			return Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
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
		public K[] GetObjects ()
		{
			List<K> tList = new List<K>();
			string [] tArray = GetReferences ();
			foreach (string tRef in tArray) {
				K tObject = NWDBasis<K>.GetObjectByReference (tRef) as K;
				if (tObject != null) {
					tList.Add (tObject);
				}
			}
			return tList.ToArray ();
		}
        //-------------------------------------------------------------------------------------------------------------
        public List<K> GetObjectsList()
        {
            List<K> tList = new List<K>();
            string[] tArray = GetReferences();
            foreach (string tRef in tArray) {
                K tObject = NWDBasis<K>.GetObjectByReference(tRef) as K;
                if (tObject != null) {
                    tList.Add(tObject);
                }
            }
            return tList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetObjects (K[] sObjects)
		{
			List<string> tList = new List<string>();
			foreach (K tObject in sObjects) {
				tList.Add (tObject.Reference);
			}
			SetReferences (tList.ToArray ());
		}
		//-------------------------------------------------------------------------------------------------------------
		public void AddObject (K sObject)
		{
			if (sObject != null) {
				AddReferences (new string[]{sObject.Reference});
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void AddObjects (K[] sObjects)
		{
			List<string> tList = new List<string>();
			foreach (K tObject in sObjects) {
				tList.Add (tObject.Reference);
			}
			AddReferences (tList.ToArray ());
		}
		//-------------------------------------------------------------------------------------------------------------
		public void RemoveObjects (K[] sObjects)
		{
			List<string> tList = new List<string>();
			foreach (K tObject in sObjects) {
				tList.Add (tObject.Reference);
			}
			RemoveReferences (tList.ToArray ());
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
			int tRow = 1;
			if (Value != null && Value != "") 
			{
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				tRow += tValueArray.Count ();
			}
			GUIStyle tPopupdStyle = new GUIStyle (EditorStyles.popup);
			float tHeight = (tPopupdStyle.CalcHeight (new GUIContent ("A"), 100.0f) + NWDConstants.kFieldMarge) * tRow - NWDConstants.kFieldMarge;
			return tHeight;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override object ControlField (Rect sPosition, string sEntitled)
		{
			NWDReferencesListType<K> tTemporary = new NWDReferencesListType<K> ();
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
			if (tReferenceListInfo != null) 
			{
				tReferenceList.AddRange (tReferenceListInfo.GetValue (null) as List<string>);
			}
			var tInternalNameListInfo = sFromType.GetField ("ObjectsInEditorTableKeyList", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
			if (tInternalNameListInfo != null) 
			{
				tInternalNameList.AddRange (tInternalNameListInfo.GetValue (null) as List<string>);
			}
			List<string> tValueList = new List<string> ();
			if (Value != null && Value != "") 
			{
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				tValueList = new List<string> (tValueArray);
			}
			tValueList.Add ("");
			for (int i = 0; i < tValueList.Count; i++) 
			{
				string tFieldName = sEntitled;
				if (i > 0) 
				{
					tFieldName = "   ";
				}
				string tV = tValueList.ElementAt (i);
				int tIndex = tReferenceList.IndexOf (tV);
				tIndex = EditorGUI.Popup (new Rect (tX, tY, tWidth - NWDConstants.kFieldMarge - tEditWidth, tPopupdStyle.fixedHeight), tFieldName, tIndex, tInternalNameList.ToArray (), tPopupdStyle);
				if (tIndex >= 0) {
					if (GUI.Button (new Rect (tX + tWidth - tEditWidth, tY, tEditWidth, tPopupdStyle.fixedHeight), "!")) {
						NWDBasis<K>.SetObjectInEdition (NWDBasis<K>.InstanceByReference (tReferenceList.ElementAt (tIndex)),false);
					}
				}
				tY += tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
				if (tIndex >= 0 && tIndex < tReferenceList.Count) 
				{
					tValueList [i] = tReferenceList.ElementAt (tIndex);
				} 
				else 
				{
					tValueList [i] = "";
				}
			}
			string[] tNextValueArray = tValueList.Distinct ().ToArray ();
			string tNextValue = string.Join (NWDConstants.kFieldSeparatorA, tNextValueArray);
			tNextValue = tNextValue.Trim (NWDConstants.kFieldSeparatorA.ToCharArray () [0]);
			tTemporary.Value = tNextValue;
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
		public string ChangeReferenceForAnother(string sOldReference, string sNewReference)
		{
			string rReturn = "NO";
			if (Value != null) {
				if (Value.Contains (sOldReference)) {
					Debug.Log ("I CHANGE "+sOldReference+" FOR "+sNewReference+"");
					Value = Value.Replace (sOldReference, sNewReference);
					rReturn = "YES";
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================