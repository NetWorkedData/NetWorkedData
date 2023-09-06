//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using UnityEngine;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDLocalizableType : NWEDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public bool ReOrder (string[] sLanguageArray)
		{
			bool rReturn = false;
			Dictionary<string,string> tResultSplitDico = new Dictionary<string,string> ();
			if (Value != null && Value != string.Empty) 
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
						string tText = string.Empty;
						if (tResultSplitDico.ContainsKey (tLangague) == false) {
							tResultSplitDico.Add (tLangague, tText);
						}
					}
				}
			}
            if (tResultSplitDico.ContainsKey(NWDDataLocalizationManager.kBaseDev) == false)
            {
                tResultSplitDico.Add(NWDDataLocalizationManager.kBaseDev, string.Empty);
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
				tNextValue = string.Empty;
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
        public Dictionary<string, string> kSplitDico;
        //-------------------------------------------------------------------------------------------------------------
        protected void DicoPopulate()
        {
            if (Value != null && Value != string.Empty)
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
                        string tText = string.Empty;
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
            string rReturn = string.Empty;
            if (kSplitDico == null)
            {
                kSplitDico = new Dictionary<string, string>();
                DicoPopulate();
            }
            if (kSplitDico.ContainsKey(sKey))
            {
                rReturn = kSplitDico[sKey];
            }
            else if (kSplitDico.ContainsKey(NWDAppConfiguration.SharedInstance().ProjetcLanguage))
            {
                rReturn = kSplitDico[NWDAppConfiguration.SharedInstance().ProjetcLanguage];
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
        public override void BaseVerif()
        {
            // Need to check with a new dictionary each time
            kSplitDico = new Dictionary<string, string>();
            DicoPopulate();

            if (kSplitDico.ContainsKey(NWDDataLocalizationManager.kBaseDev) == false)
            {
                Debug.LogWarning("no Base in localizable string");
                AddBaseString(string.Empty);
#if UNITY_EDITOR
                    // mark object to save! ?
#endif
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
			if (Value != null && Value != string.Empty) {
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
			sDictionary.Remove (NWEConstants.K_MINUS); // remove default value
			sDictionary.Remove (string.Empty); // remove empty value
			List<string> tValueNextList = new List<string> ();
			foreach (KeyValuePair<string,string> tKeyValue in sDictionary) {
				tValueNextList.Add (tKeyValue.Key + NWDConstants.kFieldSeparatorB + tKeyValue.Value);
			}
			string[] tNextValueArray = tValueNextList.Distinct ().ToArray ();
			string tNextValue = string.Join (NWDConstants.kFieldSeparatorA, tNextValueArray);
			tNextValue = tNextValue.Trim (NWDConstants.kFieldSeparatorA.ToCharArray () [0]);
			if (tNextValue == NWDConstants.kFieldSeparatorB) {
				tNextValue = string.Empty;
			}
			Value = tNextValue;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void AddValue (string sKey, string sValue)
		{
			Dictionary<string,string> tResult = GetDictionary ();
			if (sValue != null) {
                if (tResult.ContainsKey(sKey) == false)
                {
                    tResult.Add(sKey, NWDToolbox.TextProtect(sValue));
                }
                else
                {
                    tResult[sKey] = NWDToolbox.TextProtect(sValue);
                }
            } else {
				if (tResult.ContainsKey (sKey) == true) {
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
			Value = string.Empty;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
