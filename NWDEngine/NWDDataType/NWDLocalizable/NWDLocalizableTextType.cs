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
	//-------------------------------------------------------------------------------------------------------------
	public class NWDLocalizableTextType : NWDLocalizableType
	{
		//-------------------------------------------------------------------------------------------------------------
		const int kCONST_NUMBER_OF_LINE = 4;
		//-------------------------------------------------------------------------------------------------------------
		public NWDLocalizableTextType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDLocalizableTextType (string sValue = "")
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
				rReturn = "no value for key";
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void AddBaseString (string sValue)
		{
			AddValue (NWDDataLocalizationManager.kBaseDev, sValue);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void AddLocalString (string sValue)
		{
			AddValue (NWDDataManager.SharedInstance().PlayerLanguage, sValue);
		}
		//-------------------------------------------------------------------------------------------------------------
		public string GetLocalString ()
		{
			return NWDToolbox.TextUnprotect(SplitDico (NWDDataManager.SharedInstance().PlayerLanguage));
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
			int tRow = 0;
			if (Value != null && Value != "") {
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				tRow += tValueArray.Count ();
			}

            float rReturn = (NWDConstants.kTextFieldStyle.fixedHeight * kCONST_NUMBER_OF_LINE + NWDConstants.kFieldMarge) * tRow + NWDConstants.kPopupdStyle.fixedHeight;
//			return (tPopupdStyle.CalcHeight (new GUIContent ("A"), 100.0f) + NWDConstants.kFieldMarge) * tRow * kCONST_NUMBER_OF_LINE - NWDConstants.kFieldMarge;
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, string sTooltips = "")
		{
            NWDLocalizableTextType tTemporary = new NWDLocalizableTextType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

			float tWidth = sPosition.width;
			float tHeight = sPosition.height;
			float tX = sPosition.position.x;
            float tY = sPosition.position.y;
            float tLangWidth = EditorGUIUtility.labelWidth + NWDConstants.kLangWidth;

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
				//string tFieldName = sEntitled;
				if (i > 0) {
                    //tFieldName = "   ";
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
                tIndex = EditorGUI.Popup(new Rect(tX, tY, tLangWidth, NWDConstants.kPopupdStyle.fixedHeight), tContent, tIndex, tContentFuturList.ToArray(), NWDConstants.kPopupdStyle);
				if (tIndex < 0 || tIndex >= tValueFuturList.Count) {
					tIndex = 0;
				}
				tLangague = tValueFuturList [tIndex];
				if (tLangague != "") {
					//tText = EditorGUI.TextField (new Rect (tX + tLangWidth + NWDConstants.kFieldMarge, tY, tWidth - tLangWidth - NWDConstants.kFieldMarge, tPopupdStyle.fixedHeight), tText);
                    //tText = EditorGUI.TextArea (new Rect (tX + tLangWidth + NWDConstants.kFieldMarge, tY, tWidth - tLangWidth - NWDConstants.kFieldMarge, NWDConstants.kTextFieldStyle.fixedHeight * kCONST_NUMBER_OF_LINE), NWDToolbox.TextUnprotect (tText), NWDConstants.kTextAreaStyle);
                    tText = EditorGUI.TextArea(new Rect(tX + tLangWidth + NWDConstants.kFieldMarge, tY, tWidth - tLangWidth - NWDConstants.kFieldMarge, NWDConstants.kTextFieldStyle.fixedHeight * kCONST_NUMBER_OF_LINE), NWDToolbox.TextUnprotect(tText), NWDConstants.kTextAreaStyle);
					tText = NWDToolbox.TextProtect (tText);
				}
                tY += NWDConstants.kTextFieldStyle.fixedHeight * kCONST_NUMBER_OF_LINE + NWDConstants.kFieldMarge;
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