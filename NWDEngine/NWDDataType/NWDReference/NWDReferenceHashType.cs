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
	public class NWDReferenceHashType<K>: BTBDataType where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDReferenceHashType ()
		{
			Value = "";
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
			return NWDBasis<K>.GetObjectByReference (Value);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetObject (K sObject)
		{
			Value = sObject.Reference;
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
			GUIStyle tPopupdStyle = new GUIStyle (EditorStyles.popup);
			float tHeight = tPopupdStyle.CalcHeight (new GUIContent ("A"), 100.0f);

			// test if error in reference and add button height
			if (Value != null && Value != "") 
			{
//				if (ReferenceInError (new List<string> (Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries))).Count > 0) {
//					tHeight = tHeight + tMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
//				}
			}

			return tHeight;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, string sTooltips = "")
		{
            NWDReferenceHashType<K> tTemporary = new NWDReferenceHashType<K> ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
			tTemporary.Value = Value;
			Type sFromType = typeof(K);
			float tWidth = sPosition.width;
			float tHeight = sPosition.height;
			float tX = sPosition.position.x;
			float tY = sPosition.position.y;
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

            List<GUIContent> tContentFuturList = new List<GUIContent>();
            foreach (string tS in tInternalNameList.ToArray())
            {
                tContentFuturList.Add(new GUIContent(tS));
            }
			int tIndex = tReferenceList.IndexOf (Value);
            int rIndex = EditorGUI.Popup (new Rect (tX, tY, tWidth, tPopupdStyle.fixedHeight), tContent, tIndex, tContentFuturList.ToArray (), tPopupdStyle);
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