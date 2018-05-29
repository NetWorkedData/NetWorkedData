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
            if (tResultSplitDico.ContainsKey(NWDDataLocalizationManager.kBaseDev) == false)
            {
                tResultSplitDico.Add(NWDDataLocalizationManager.kBaseDev, "");
            }

            // order list by language array
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
        [NonSerializedAttribute]
        protected Dictionary<string, string> kSplitDico;
        //-------------------------------------------------------------------------------------------------------------
        protected void DicoPopulate()
        {
            if (Value != null && Value != "")
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tValueArrayLine in tValueArray)
                {
                    string[] tLineValue = tValueArrayLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        string tLangague = tLineValue[0];
                        string tText = tLineValue[1];
                        if (kSplitDico.ContainsKey(tLangague) == false)
                        {
                            kSplitDico.Add(tLangague, tText);
                        }
                    }
                    else if (tLineValue.Length == 1)
                    {
                        string tLangague = tLineValue[0];
                        string tText = "";
                        if (kSplitDico.ContainsKey(tLangague) == false)
                        {
                            kSplitDico.Add(tLangague, tText);
                        }
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        protected string SplitDico(string sKey)
        {
            string rReturn = "";
            if (kSplitDico == null)
            {
                kSplitDico = new Dictionary<string, string>();
                DicoPopulate();
            }
            if (kSplitDico.ContainsKey(sKey))
            {
                rReturn = kSplitDico[sKey];
            }
            else if (kSplitDico.ContainsKey(NWDDataLocalizationManager.kBaseDev))
            {
                rReturn = kSplitDico[NWDDataLocalizationManager.kBaseDev];
            }
            else
            {
                rReturn = "no value for key";
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = NWDDataLocalizationManager.kBaseDev+NWDConstants.kFieldSeparatorB;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void BaseVerif()
        {
            if (kSplitDico == null)
            {
                kSplitDico = new Dictionary<string, string>();
                DicoPopulate();
            }
            if (kSplitDico.ContainsKey(NWDDataLocalizationManager.kBaseDev) == false)
            {
                AddBaseString("");
            }
        }

        //-------------------------------------------------------------------------------------------------------------
        public void AddBaseString(string sValue)
        {
            AddValue(NWDDataLocalizationManager.kBaseDev, sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddLocalString(string sValue)
        {
            AddValue(NWDDataManager.SharedInstance().PlayerLanguage, sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetLocalString()
        {
            //Debug.Log("GetLocalString() for language = "+NWDDataManager.SharedInstance().PlayerLanguage);
            return NWDToolbox.TextUnprotect(SplitDico(NWDDataManager.SharedInstance().PlayerLanguage));
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetBaseString()
        {
            return NWDToolbox.TextUnprotect(SplitDico(NWDDataLocalizationManager.kBaseDev));
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetLanguageString(string sLanguage)
        {
            return NWDToolbox.TextUnprotect(SplitDico(sLanguage));
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
                tResult.Add(sKey, NWDToolbox.TextProtect(sValue));
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
	}
}
//=====================================================================================================================