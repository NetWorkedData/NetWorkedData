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

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	[SerializeField]
	public class NWDLocalizablePrefabType : NWDLocalizableType
	{
		// TODO : Finish to access to object directly ... as GameObjectDataType 
		//-------------------------------------------------------------------------------------------------------------
		public NWDLocalizablePrefabType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDLocalizablePrefabType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
			kSplitDico = new Dictionary<string,string> ();
			DicoPopulate ();
		}
		//-------------------------------------------------------------------------------------------------------------
		[NonSerializedAttribute]
		private Dictionary<string,string> kSplitDico;
		//-------------------------------------------------------------------------------------------------------------
		private void DicoPopulate()
		{
			if (Value != null && Value != "") {
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string tValueArrayLine in tValueArray) {
					string[] tLineValue = tValueArrayLine.Split (new string[]{ NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
					if (tLineValue.Length == 2) {
						string tLangague = tLineValue [0];
						string tText = tLineValue [1];
						if (kSplitDico.ContainsKey (tLangague) == false) {
							kSplitDico.Add (tLangague, tText);
						}
					} else if (tLineValue.Length == 1) {
						string tLangague = tLineValue [0];
						string tText = "";
						if (kSplitDico.ContainsKey (tLangague) == false) {
							kSplitDico.Add (tLangague, tText);
						}
					}
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		private string SplitDico (string sKey)
		{
			string rReturn = "";
			if (kSplitDico == null) { 
				kSplitDico = new Dictionary<string,string> ();
				DicoPopulate ();
			}
			if (kSplitDico.ContainsKey(sKey))
			{
				rReturn = kSplitDico[sKey];
			} else if (kSplitDico.ContainsKey (NWDDataLocalizationManager.kBaseDev)) {
				rReturn = kSplitDico [NWDDataLocalizationManager.kBaseDev];
			} else {
				rReturn = "";
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public string GetLocalString ()
		{
			return SplitDico (NWDDataManager.SharedInstance.PlayerLanguage);
		}
		//-------------------------------------------------------------------------------------------------------------
		public string GetBaseString ()
		{
			return NWDToolbox.TextUnprotect(SplitDico (NWDDataLocalizationManager.kBaseDev));
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetLanguageString(string sLanguage)
        {
            return NWDToolbox.TextUnprotect(SplitDico(sLanguage));
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
			tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight (new GUIContent ("A"), 100);

			float rReturn = (NWDConstants.kPrefabSize + NWDConstants.kFieldMarge + tPopupdStyle.fixedHeight) * tRow;
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, string sTooltips = "")
		{
            NWDLocalizablePrefabType tTemporary = new NWDLocalizablePrefabType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

			float tWidth = sPosition.width;
			float tHeight = sPosition.height;
			float tX = sPosition.position.x;
            float tY = sPosition.position.y;
            float tLangWidth = EditorGUIUtility.labelWidth + NWDConstants.kLangWidth;

			GUIStyle tPopupdStyle = new GUIStyle (EditorStyles.popup);
			tPopupdStyle.fixedHeight = tPopupdStyle.CalcHeight (new GUIContent ("A"), tWidth);

			List<string> tLocalizationList = new List<string> ();
			tLocalizationList.Add ("-");

			string tLanguage = NWDAppConfiguration.SharedInstance().DataLocalizationManager.LanguagesString;
			string[] tLanguageArray = tLanguage.Split (new string[]{ ";" }, StringSplitOptions.RemoveEmptyEntries);

			tLocalizationList.AddRange (tLanguageArray);
			tLocalizationList.Sort ();

			List<string> tValueList = new List<string> ();
			List<string> tValueNextList = new List<string> ();

			Dictionary<string,string> tResult = new Dictionary<string,string> ();

			if (Value != null && Value != "") {
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				tValueList = new List<string> (tValueArray);
			}

			for (int i = 0; i < tValueList.Count; i++) {
				string tLine = tValueList.ElementAt (i);
				string[] tLineValue = tLine.Split (new string[]{ NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
				if (tLineValue.Length > 0) {
					//Debug.Log ("i remove " + tLineValue [0]);
					tLocalizationList.Remove (tLineValue [0]);
				}
			}
			string[] tLangageArray = tLocalizationList.ToArray ();
			//Debug.Log (" tLangageArray =  " + string.Join(".",tLangageArray));
			tValueList.Add ("");
			for (int i = 0; i < tValueList.Count; i++) {
				string tFieldName = sEntitled;
				if (i > 0) {
					tFieldName = "   ";
                    tContent = new GUIContent("   ");
				}
				string tLangague = "";
				string tText = "";
				string tLine = tValueList.ElementAt (i);
				string[] tLineValue = tLine.Split (new string[]{ NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
				if (tLineValue.Length == 2) {
					tLangague = tLineValue [0];
					tText = tLineValue [1];
				}
				if (tLineValue.Length == 1) {
					tLangague = tLineValue [0];
				}
				List<string> tValueFuturList = new List<string> ();
				tValueFuturList.AddRange (tLangageArray);
				tValueFuturList.Add (tLangague);
				tValueFuturList.Sort ();
				string[] tLangageFuturArray = tValueFuturList.ToArray ();
                List<GUIContent> tContentFuturList = new List<GUIContent>();
                foreach (string tS in tLangageFuturArray)
                {
                    tContentFuturList.Add(new GUIContent(tS));
                }
				//Debug.Log (" tLangageFuturArray =  " + string.Join(":",tLangageFuturArray));

				int tIndex = tValueFuturList.IndexOf (tLangague);
                //tIndex = EditorGUI.Popup (new Rect (tX, tY, tLangWidth, tPopupdStyle.fixedHeight), tFieldName, tIndex, tLangageFuturArray, tPopupdStyle);
                tIndex = EditorGUI.Popup(new Rect(tX, tY, tLangWidth, tPopupdStyle.fixedHeight), tContent, tIndex, tContentFuturList.ToArray(), tPopupdStyle);
				if (tIndex < 0 || tIndex >= tValueFuturList.Count) {
					tIndex = 0;
				}
				tLangague = tValueFuturList [tIndex];
				GUIStyle tObjectFieldStyle = new GUIStyle (EditorStyles.objectField);
				tObjectFieldStyle.fixedHeight = tObjectFieldStyle.CalcHeight (new GUIContent ("A"), tWidth);

				GameObject tObject = null;
				if (tText != null && tText != "") {
					tObject = AssetDatabase.LoadAssetAtPath (tText, typeof(GameObject)) as GameObject;
				}
				Texture2D tTexture2D = AssetPreview.GetAssetPreview (tObject);
				if (tTexture2D != null) {
					EditorGUI.DrawPreviewTexture (new Rect (tWidth - NWDConstants.kPrefabSize, tY + tObjectFieldStyle.fixedHeight, NWDConstants.kPrefabSize, NWDConstants.kPrefabSize), tTexture2D);
				}
				if (tLangague != "") {
					UnityEngine.Object pObj = EditorGUI.ObjectField (new Rect (tX + tLangWidth + NWDConstants.kFieldMarge, tY, tWidth - tLangWidth - NWDConstants.kFieldMarge, tPopupdStyle.fixedHeight), (UnityEngine.Object)tObject, typeof(GameObject), false);
					if (pObj != null) {
						if (PrefabUtility.GetPrefabType (pObj) == PrefabType.Prefab) {
							tText = AssetDatabase.GetAssetPath (PrefabUtility.GetPrefabObject (pObj));
						}
					} else {
						tText = "";
					}
				}
				//tText = EditorGUI.TextField (new Rect (tX + tLangWidth + NWDConstants.kFieldMarge, tY, tWidth - tLangWidth - NWDConstants.kFieldMarge, tPopupdStyle.fixedHeight), tText);
//				tY += tPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
				tY += NWDConstants.kPrefabSize + NWDConstants.kFieldMarge + tPopupdStyle.fixedHeight;
				if (tResult.ContainsKey (tLangague)) {
					tResult [tLangague] = tText;
				} else {
					tResult.Add (tLangague, tText);
				}
			}
			tResult.Remove ("-"); // remove default value
			tResult.Remove (""); // remove empty value
			foreach (KeyValuePair<string,string> tKeyValue in tResult) {
				tValueNextList.Add (tKeyValue.Key + NWDConstants.kFieldSeparatorB + tKeyValue.Value);
			}

			string[] tNextValueArray = tValueNextList.Distinct ().ToArray ();
			string tNextValue = string.Join (NWDConstants.kFieldSeparatorA, tNextValueArray);
			tNextValue = tNextValue.Trim (NWDConstants.kFieldSeparatorA.ToCharArray () [0]);
			if (tNextValue == NWDConstants.kFieldSeparatorB) {
				tNextValue = "";
			}
			tTemporary.Value = tNextValue;
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================