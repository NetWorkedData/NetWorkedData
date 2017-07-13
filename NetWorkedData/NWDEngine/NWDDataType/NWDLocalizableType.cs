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
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================