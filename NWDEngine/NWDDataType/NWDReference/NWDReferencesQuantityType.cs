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
	public class NWDReferencesQuantityType<K>: BTBDataType where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDReferencesQuantityType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool ContainsObject (K sObject)
		{
			return Value.Contains (sObject.Reference);
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool ContainsReference (string sReference)
		{
			return Value.Contains (sReference);
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool IsEmpty ()
		{
			bool rReturn = true;
			if (Value != "") {
				rReturn = false;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool IsNotEmpty ()
		{
			bool rReturn = false;
			if (Value != "") {
				rReturn = true;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool ContainsReferencesQuantity (NWDReferencesQuantityType<K> sReferencesQuantity)
		{
			bool rReturn = true;
			// I compare all elemnt
			Dictionary<string,int> tThis = GetReferenceAndQuantity ();
			Dictionary<string,int> tOther = sReferencesQuantity.GetReferenceAndQuantity ();

			foreach (KeyValuePair<string,int> tKeyValue in tOther) {
				if (tThis.ContainsKey (tKeyValue.Key) == false) {
					rReturn = false;
					break;
				} else {
					if (tKeyValue.Value > tThis [tKeyValue.Key]) {
						return false;
					}
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool RemoveReferencesQuantity (NWDReferencesQuantityType<K> sReferencesQuantity, bool sCanBeNegative = true, bool sRemoveEmpty = true)
		{
            //TODO : add comment to explain the used of a boolean
			bool rReturn = ContainsReferencesQuantity (sReferencesQuantity);
			if (rReturn == true) {
				Dictionary<string,int> tThis = GetReferenceAndQuantity ();
				Dictionary<string,int> tOther = sReferencesQuantity.GetReferenceAndQuantity ();
				foreach (KeyValuePair<string,int> tKeyValue in tOther) {
                    //TODO : check negative value
                    //TODO : use RemoveObjectQuantity
                    tThis[tKeyValue.Key] = tThis [tKeyValue.Key] - tKeyValue.Value;
				}
				SetReferenceAndQuantity (tThis);
			}
			return rReturn;
		}
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveObjectQuantity(NWDBasis<K> sObject, int sQuantity, bool sCanBeNegative = true, bool sRemoveEmpty = true)
        {
            Dictionary<string, int> tThis = GetReferenceAndQuantity();
            if (tThis.ContainsKey(sObject.Reference) == false)
            {
                tThis.Add(sObject.Reference, -sQuantity);
            }
            else
            {
                tThis[sObject.Reference] = tThis[sObject.Reference] - sQuantity;
            }

            if(sCanBeNegative == false && tThis[sObject.Reference] < 0)
            {
                tThis[sObject.Reference] = 0;
            }

            if (sRemoveEmpty == true && tThis[sObject.Reference] == 0)
            {
                tThis.Remove(sObject.Reference);
            }

            SetReferenceAndQuantity(tThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddReferencesQuantity (NWDReferencesQuantityType<K> sReferencesQuantity)
		{
			// I compare all element
			Dictionary<string,int> tThis = GetReferenceAndQuantity ();
			Dictionary<string,int> tOther = sReferencesQuantity.GetReferenceAndQuantity ();
			foreach (KeyValuePair<string,int> tKeyValue in tOther) {
				if (tThis.ContainsKey (tKeyValue.Key) == false) {
					tThis.Add (tKeyValue.Key, tKeyValue.Value);
				} else {
					tThis [tKeyValue.Key] = tThis [tKeyValue.Key] + tKeyValue.Value;
				}
			}
			SetReferenceAndQuantity (tThis);
		}
        //-------------------------------------------------------------------------------------------------------------
        public void AddObjectQuantity(NWDBasis<K> sObject, int sQuantity)
        {
            // I compare all element
            Dictionary<string, int> tThis = GetReferenceAndQuantity();
            if (tThis.ContainsKey(sObject.Reference) == false)
            {
                tThis.Add(sObject.Reference, sQuantity);
            }
            else
            {
                tThis[sObject.Reference] = tThis[sObject.Reference] + sQuantity;
            }
            SetReferenceAndQuantity(tThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] GetObjects ()
		{
			List<K> tList = new List<K> ();
			string[] tArray = GetReferences ();
			foreach (string tRef in tArray) {
				K tObject = NWDBasis<K>.GetObjectByReference (tRef) as K;
				if (tObject != null) {
					tList.Add (tObject);
				}
			}
			return tList.ToArray ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public string[] GetReferences ()
		{
			List<string> tValueList = new List<string> ();
			if (Value != null && Value != "") {
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string tLine in tValueArray) {
					string[] tLineValue = tLine.Split (new string[]{ NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
					if (tLineValue.Length == 2) {
						tValueList.Add (tLineValue [0]);
					}
				}
			}
			return tValueList.ToArray ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetReferenceAndQuantity (Dictionary<string,int>  sDico)
		{
			List<string> tValueList = new List<string> ();
			foreach (KeyValuePair<string,int> tKeyValue in sDico) {
				tValueList.Add (tKeyValue.Key + NWDConstants.kFieldSeparatorB + tKeyValue.Value.ToString ());
			}
			string[] tNextValueArray = tValueList.Distinct ().ToArray ();
			string tNextValue = string.Join (NWDConstants.kFieldSeparatorA, tNextValueArray);
			tNextValue = tNextValue.Trim (NWDConstants.kFieldSeparatorA.ToCharArray () [0]);
			Value = tNextValue;
		}
		//-------------------------------------------------------------------------------------------------------------
		public Dictionary<string,int> GetReferenceAndQuantity ()
		{
			Dictionary<string,int> tValueDico = new Dictionary<string,int> ();
			if (Value != null && Value != "") {
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string tLine in tValueArray) {
					string[] tLineValue = tLine.Split (new string[]{ NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
					if (tLineValue.Length == 2) {
						int tQ = 0;
						int.TryParse (tLineValue [1], out tQ);
						tValueDico.Add (tLineValue [0], tQ);
					}
				}
			}
			return tValueDico;
		}
		//-------------------------------------------------------------------------------------------------------------
		public Dictionary<K,int> GetObjectAndQuantity ()
		{
			Dictionary<K,int> tValueDico = new Dictionary<K,int> ();
			if (Value != null && Value != "") {
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string tLine in tValueArray) {
					string[] tLineValue = tLine.Split (new string[]{ NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
					if (tLineValue.Length == 2) {
						int tQ = 0;
						int.TryParse (tLineValue [1], out tQ);
						K tObject = NWDBasis<K>.GetObjectByReference (tLineValue [0]) as K;
						if (tObject != null) {
							tValueDico.Add (tObject, tQ);
						}
					}
				}
			}
			return tValueDico;
		}
		//-------------------------------------------------------------------------------------------------------------
		public List<K> ExploseInItemsList ()
		{
			List<K> rList = new List<K> ();
			if (Value != null && Value != "") {
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string tLine in tValueArray) {
					string[] tLineValue = tLine.Split (new string[]{ NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
					if (tLineValue.Length == 2) {
						int tQ = 0;
						int.TryParse (tLineValue [1], out tQ);
						K tObject = NWDBasis<K>.GetObjectByReference (tLineValue [0]) as K;
						if (tObject != null) {
							for (int i = 0; i < tQ; i++) {
								rList.Add (tObject);
							}
						}
					}
				}
			}
			return rList;
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
			int tRow = 1;
			if (Value != null && Value != "") {
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
			NWDReferencesQuantityType<K> tTemporary = new NWDReferencesQuantityType<K> ();
			tTemporary.Value = Value;
			Type sFromType = typeof(K);
			float tWidth = sPosition.width;
			float tHeight = sPosition.height;
			float tX = sPosition.position.x;
			float tY = sPosition.position.y;

			float tIntWidth = NWDConstants.kIntWidth;
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
			List<string> tValueList = new List<string> ();
			List<string> tValueListReferenceAllReady = new List<string> ();
			if (Value != null && Value != "") {
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				tValueList = new List<string> (tValueArray);
			}
			tValueList.Add ("");
			for (int i = 0; i < tValueList.Count; i++) {
				string tFieldName = sEntitled;
				if (i > 0) {
					tFieldName = "   ";
				}

				int tIndex = 0;
				int tQ = 1;
				string tV = "";
				string tLine = tValueList.ElementAt (i);
				string[] tLineValue = tLine.Split (new string[]{ NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
				if (tLineValue.Length == 2) {
					tV = tLineValue [0];
					tIndex = tReferenceList.IndexOf (tV);
					int.TryParse (tLineValue [1], out tQ);
				}

				tIndex = EditorGUI.Popup (new Rect (tX, tY, tWidth - tIntWidth - NWDConstants.kFieldMarge - NWDConstants.kFieldMarge - tEditWidth, tPopupdStyle.fixedHeight), tFieldName, tIndex, tInternalNameList.ToArray (), tPopupdStyle);
				if (tIndex > 0) {
					tQ = EditorGUI.IntField (new Rect (tX + tWidth - tIntWidth - NWDConstants.kFieldMarge - tEditWidth, tY, tIntWidth, tPopupdStyle.fixedHeight), tQ);
					if (GUI.Button (new Rect (tX + tWidth - tEditWidth, tY, tEditWidth, tPopupdStyle.fixedHeight), "!")) {
						NWDBasis<K>.SetObjectInEdition (NWDBasis<K>.InstanceByReference (tReferenceList.ElementAt (tIndex)), false);
					}
				}
				tY += tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;

				if (tIndex > 0 && tIndex < tReferenceList.Count) {
					//if (tQ != 0) {
					if (!tValueListReferenceAllReady.Contains (tReferenceList.ElementAt (tIndex))) {
						tValueList [i] = tReferenceList.ElementAt (tIndex) + NWDConstants.kFieldSeparatorB + tQ.ToString ();
						tValueListReferenceAllReady.Add (tReferenceList.ElementAt (tIndex));
					}
				} else {
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