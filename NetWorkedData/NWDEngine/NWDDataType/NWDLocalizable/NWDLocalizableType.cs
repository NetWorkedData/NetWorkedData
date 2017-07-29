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
	public class NWDLocalizableType : BTBDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public bool ReOrder (string[] sLanguageArray)
		{
			bool rReturn = false;
			Dictionary<string,string> tResultSplitDico = new Dictionary<string,string> ();
			if (Value != null && Value != "") 
			{
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string tValueArrayLine in tValueArray) {
					string[] tLineValue = tValueArrayLine.Split (new string[]{ NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
					if (tLineValue.Length == 2) {
						string tLangague = tLineValue [0];
						string tText = tLineValue [1];
						if (tResultSplitDico.ContainsKey (tLangague) == false) {
							tResultSplitDico.Add (tLangague, tText);
						}
					}
					else if (tLineValue.Length == 1) {
						string tLangague = tLineValue [0];
						string tText = "";
						if (tResultSplitDico.ContainsKey (tLangague) == false) {
							tResultSplitDico.Add (tLangague, tText);
						}
					}
				}
			}
			List<string> tValueNextList = new List<string> ();
			foreach (string tLang in sLanguageArray) {
				if (tResultSplitDico.ContainsKey (tLang) == true) {
					tValueNextList.Add (tLang + NWDConstants.kFieldSeparatorB + tResultSplitDico [tLang]);
				}
			}
			string[] tNextValueArray = tValueNextList.Distinct ().ToArray ();
			string tNextValue = string.Join (NWDConstants.kFieldSeparatorA, tNextValueArray);
			tNextValue = tNextValue.Trim (NWDConstants.kFieldSeparatorA.ToCharArray () [0]);
			if (tNextValue == NWDConstants.kFieldSeparatorB) {
				tNextValue = "";
			}
			if (Value != tNextValue) {
				Value = tNextValue;
				rReturn = true;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public Dictionary<string,string> GetDictionary ()
		{
			Dictionary<string,string> tResult = new Dictionary<string,string> ();
			if (Value != null && Value != "") {
				string[] tValueArray = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				foreach (string tValue in tValueArray) {
					string[] tLineValue = tValue.Split (new string[]{ NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
					if (tLineValue.Length == 2) {
						tResult.Add (tLineValue [0], tLineValue [1]);
					}
				}
			}
			return tResult;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetDictionary (Dictionary<string,string> sDictionary)
		{
			sDictionary.Remove ("-"); // remove default value
			sDictionary.Remove (""); // remove empty value
			List<string> tValueNextList = new List<string> ();
			foreach (KeyValuePair<string,string> tKeyValue in sDictionary) {
				tValueNextList.Add (tKeyValue.Key + NWDConstants.kFieldSeparatorB + tKeyValue.Value);
			}
			string[] tNextValueArray = tValueNextList.Distinct ().ToArray ();
			string tNextValue = string.Join (NWDConstants.kFieldSeparatorA, tNextValueArray);
			tNextValue = tNextValue.Trim (NWDConstants.kFieldSeparatorA.ToCharArray () [0]);
			if (tNextValue == NWDConstants.kFieldSeparatorB) {
				tNextValue = "";
			}
			Value = tNextValue;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void AddValue (string sKey, string sValue)
		{
			Dictionary<string,string> tResult = GetDictionary ();
			if (sValue != null) {
				tResult.Add (sKey, sValue);
			} else {
				if (tResult.ContainsKey (sKey)) {
					tResult.Remove (sKey);
				}
			}
			SetDictionary (tResult);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void RemoveValue (string sKey)
		{
			Dictionary<string,string> tDictionary = GetDictionary ();
			if (tDictionary.ContainsKey (sKey)) {
				tDictionary.Remove (sKey);
			}
			SetDictionary (tDictionary);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void RemoveAllValues ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================