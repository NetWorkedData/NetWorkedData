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

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;
using System.IO;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public static void ReOrderAllLocalizations ()
		{
			string tLanguage = NWDAppConfiguration.SharedInstance.DataLocalizationManager.LanguagesString;
			string[] tLanguageArray = tLanguage.Split (new string[]{ ";" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (NWDBasis<K> tObject in NWDBasis<K>.ObjectsList) {
				tObject.ReOrderLocalizationsValues (tLanguageArray);
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void ReOrderLocalizationsValues (string[] sLanguageArray)
		{
			if (TestIntegrity () == true) {
				bool tUpdate = false;
//				string tRows = "";
				Type tType = ClassType ();
				List<string> tPropertiesList = PropertiesOrderArray ();
				foreach (string tPropertieName in tPropertiesList) {
					PropertyInfo tProp = tType.GetProperty (tPropertieName);
					Type tTypeOfThis = tProp.PropertyType;

					//TO-DO : (FUTUR ADDS) Insert new NWDxxxxType Localizable
					if (tTypeOfThis.IsSubclassOf (typeof(NWDLocalizableType))) {
						NWDLocalizableType tValueStruct = (NWDLocalizableType)tProp.GetValue (this, null);
						if (tValueStruct.ReOrder (sLanguageArray)) {
							tUpdate = true;
						}
					}
//					if (tTypeOfThis == typeof(NWDLocalizableStringType)) {
//						NWDLocalizableStringType tValueStruct = (NWDLocalizableStringType)tProp.GetValue (this, null);
//						NWDLocalizableStringType tValueStructNext = tValueStruct.ReOrder (sLanguageArray);
//						if (tValueStructNext.Value != tValueStruct.Value) {
//							tUpdate = true;
//						}
//					}
//
//					if (tTypeOfThis == typeof(NWDLocalizableTextType)) {
//						NWDLocalizableTextType tValueStruct = (NWDLocalizableTextType)tProp.GetValue (this, null);
//						NWDLocalizableTextType tValueStructNext = tValueStruct.ReOrder (sLanguageArray);
//						if (tValueStructNext.Value != tValueStruct.Value) {
//							tProp.SetValue (this, tValueStructNext, null);
//							tUpdate = true;
//						}
//					}
//
//					if (tTypeOfThis == typeof(NWDLocalizablePrefabType)) {
//						NWDLocalizablePrefabType tValueStruct = (NWDLocalizablePrefabType)tProp.GetValue (this, null);
//						NWDLocalizablePrefabType tValueStructNext = tValueStruct.ReOrder (sLanguageArray);
//						if (tValueStructNext.Value != tValueStruct.Value) {
//							tProp.SetValue (this, tValueStructNext, null);
//							tUpdate = true;
//						}
//					}

				}
				if (tUpdate == true) {
					UpdateMe ();
				}
			}
		}

		// export CSV

		//-------------------------------------------------------------------------------------------------------------
		public static string ExportAllLocalization ()
		{
			string tRows = "";
			string tLanguage = NWDAppConfiguration.SharedInstance.DataLocalizationManager.LanguagesString;
			string[] tLanguageArray = tLanguage.Split (new string[]{ ";" }, StringSplitOptions.RemoveEmptyEntries);
			foreach (NWDBasis<K> tObject in NWDBasis<K>.ObjectsList) {
				tRows += tObject.ExportLocalization (tLanguageArray);
			}
			return tRows;
		}

		//-------------------------------------------------------------------------------------------------------------
		public string ExportLocalization (string[] sLanguageArray)
		{
			string tRows = "";
			Type tType = ClassType ();
			List<string> tPropertiesList = PropertiesOrderArray ();
			foreach (string tPropertieName in tPropertiesList) {
				PropertyInfo tProp = tType.GetProperty (tPropertieName);
				Type tTypeOfThis = tProp.PropertyType;
				//TO-DO : (FUTUR ADDS) Insert new NWDxxxxType Localizable 
				if (tTypeOfThis.IsSubclassOf (typeof(NWDLocalizableType))) {
					NWDLocalizableType tValueObject = (NWDLocalizableType)tProp.GetValue (this, null);
					string tValue = tValueObject.Value;
					Dictionary<string,string> tResultSplitDico = new Dictionary<string,string> ();

					if (tValue != null && tValue != "") {
						string[] tValueArray = tValue.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
						foreach (string tValueArrayLine in tValueArray) {
							string[] tLineValue = tValueArrayLine.Split (new string[]{ NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
							if (tLineValue.Length == 2) {
								string tLangague = tLineValue [0];
								string tText = tLineValue [1];
								if (tResultSplitDico.ContainsKey (tLangague) == false) {
									tResultSplitDico.Add (tLangague, tText);
								}
							}
						}
					}

					tRows += "\"" + ClassTrigramme () + "\";\"" + Reference + "\";\"" + InternalKey + "\";\"" + InternalDescription + "\";\"" + tPropertieName + "\";";
					foreach (string tLang in sLanguageArray) {
						if (tResultSplitDico.ContainsKey (tLang) == true) {
							string tValueToWrite = tResultSplitDico [tLang];
							tValueToWrite = tValueToWrite.Replace ("\"", "&quot;");
							tValueToWrite = tValueToWrite.Replace (";", "&#59");
							tValueToWrite = tValueToWrite.Replace ("\r\n", "\n");
							tValueToWrite = tValueToWrite.Replace ("\n\r", "\n");
							tValueToWrite = tValueToWrite.Replace ("\r", "\n");
							tValueToWrite = tValueToWrite.Replace ("\n", "&#00");

							tRows += "\"" + tValueToWrite + "\"";
						}
						tRows += ";";
					}
					tRows += "\n";
				}
//
//				if (tTypeOfThis == typeof(NWDLocalizableTextType)) {
//					NWDLocalizableTextType tValueStruct = (NWDLocalizableTextType)tProp.GetValue (this, null);
//					string tValue = tValueStruct.Value;
//					Dictionary<string,string> tResultSplitDico = new Dictionary<string,string> ();
//
//					if (tValue != null && tValue != "") {
//						string[] tValueArray = tValue.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
//						foreach (string tValueArrayLine in tValueArray) {
//							string[] tLineValue = tValueArrayLine.Split (new string[]{ NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
//							if (tLineValue.Length == 2) {
//								string tLangague = tLineValue [0];
//								string tText = tLineValue [1];
//								if (tResultSplitDico.ContainsKey (tLangague) == false) {
//									tResultSplitDico.Add (tLangague, tText);
//								}
//							}
//						}
//					}
//
//					tRows += "\"" + ClassTrigramme () + "\";\"" + Reference + "\";\"" + InternalKey + "\";\"" + InternalDescription + "\";\"" + tPropertieName + "\";";
//					foreach (string tLang in sLanguageArray) {
//						if (tResultSplitDico.ContainsKey (tLang) == true) {
//							string tValueToWrite = tResultSplitDico [tLang];
//							tValueToWrite = tValueToWrite.Replace ("\"", "&quot;");
//							tValueToWrite = tValueToWrite.Replace (";", "&#59");
//							tValueToWrite = tValueToWrite.Replace ("\r\n", "\n");
//							tValueToWrite = tValueToWrite.Replace ("\n\r", "\n");
//							tValueToWrite = tValueToWrite.Replace ("\r", "\n");
//							tValueToWrite = tValueToWrite.Replace ("\n", "&#00");
//
//							tRows += "\"" + tValueToWrite + "\"";
//						}
//						tRows += ";";
//					}
//					tRows += "\n";
//				}
			}
			return tRows;
		}
			
		// import CSV

		//-------------------------------------------------------------------------------------------------------------
		public static void ImportAllLocalizations (string[] sLanguageArray, string[] sCSVFileArray)
		{
			//Debug.Log ("ImportAllLocalizations");
			//string tHeaders = "\"Type\";\"Reference\";\"InternalKey\";\"InternalDescription\";\"PropertyName\";\"" + 
			int tI = 0;
			int tCount = sCSVFileArray.Length;
			string tKeys = sCSVFileArray [0];
			//Debug.Log ("ImportAllLocalizations tKeys = " + tKeys);
			string[] tKeysArray = tKeys.Split (new string[]{ ";" }, StringSplitOptions.RemoveEmptyEntries);

			//Debug.Log ("ImportAllLocalizations tCount = " + tCount);
			for (tI = 1; tI < tCount; tI++) {
				NWDBasis<K>.ImportLocalization (sLanguageArray, tKeysArray, sCSVFileArray [tI]);
			}
		}

		//-------------------------------------------------------------------------------------------------------------
		public static void ImportLocalization (string[] sLanguageArray, string[] sKeysArray, string sCSVrow)
		{

			//Debug.Log ("sCSVrow = " + sCSVrow);
			//string tHeaders = "\"Type\";\"Reference\";\"InternalKey\";\"InternalDescription\";\"PropertyName\";\"" + 
			string[] tValuesArray = sCSVrow.Split (new string[]{ ";" }, StringSplitOptions.RemoveEmptyEntries);
			Dictionary<string,string> tDico = new Dictionary<string,string> ();
			int i = 0;
			for (i = 0; i < tValuesArray.Length; i++) {
				string tKey = sKeysArray [i].Trim ('"');
				string tValue = tValuesArray [i].Trim ('"');
				//Debug.Log ("dico : " + tKey + " =  " + tValue);
				tDico.Add (tKey, tValue);
			}
			//if (tDico.ContainsKey ("Reference") && tDico.ContainsKey ("PropertyName") && tDico.ContainsKey ("Type")) 
			{
				if (tDico ["Type"] == ClassTrigramme ()) {
					//Debug.Log ("tDico [\"Reference\"] = " + tDico ["Reference"]);
					NWDBasis<K> tObject = NWDBasis<K>.InstanceByReference (tDico ["Reference"]);
					if (tObject != null) {
						//Debug.Log ("tObject reference " + tObject.Reference + " found ");
						if (tObject.TestIntegrity () == true) {
							List<string> tValueNextList = new List<string> ();
							foreach (string tLang in sLanguageArray) {
								if (tDico.ContainsKey (tLang)) {
									string tLangValue = tDico [tLang];
									tLangValue = tLangValue.Replace ("&#59", ";");
									tLangValue = tLangValue.Replace ("&#00", "\n");
									tLangValue = tLangValue.Replace ("&quot;", "\"");
									tValueNextList.Add (tLang + NWDConstants.kFieldSeparatorB + tLangValue);
								}
							}
							string[] tNextValueArray = tValueNextList.Distinct ().ToArray ();
							string tNextValue = string.Join (NWDConstants.kFieldSeparatorA, tNextValueArray);
							tNextValue = tNextValue.Trim (NWDConstants.kFieldSeparatorA.ToCharArray () [0]);
							if (tNextValue == NWDConstants.kFieldSeparatorB) {
								tNextValue = "";
							}
							string tPropertyName = tDico ["PropertyName"];

							PropertyInfo tInfo = tObject.GetType ().GetProperty (tPropertyName);

							if (tInfo.PropertyType.IsSubclassOf (typeof(NWDLocalizableType))) {
								NWDLocalizableType tPropertyValueOld = (NWDLocalizableType)tInfo.GetValue (tObject, null);
								if (tPropertyValueOld.Value != tNextValue) {
									tPropertyValueOld.Value = tNextValue;
									NWDDataManager.SharedInstance.AddObjectToUpdateQueue (tObject);
								}
							}
//								
//							//TODO : (FUTUR ADDS) Insert new NWDxxxxType Localizable
//							if (tInfo.PropertyType == typeof(NWDLocalizableStringType)) {
//								NWDLocalizableStringType tPropertyValue = new NWDLocalizableStringType ();
//								tPropertyValue.Value = tNextValue;
//
//								NWDLocalizableStringType tPropertyValueOld = (NWDLocalizableStringType)tInfo.GetValue (tObject, null);
//								if (tPropertyValueOld.Value != tPropertyValue.Value) {
//									tInfo.SetValue (tObject, tPropertyValue, null);
//									NWDDataManager.SharedInstance.AddObjectToUpdateQueue (tObject);
//								}
//							}
//							if (tInfo.PropertyType == typeof(NWDLocalizableTextType)) {
//								NWDLocalizableTextType tPropertyValue = new NWDLocalizableTextType ();
//								tPropertyValue.Value = tNextValue;
//
//								NWDLocalizableTextType tPropertyValueOld = (NWDLocalizableTextType)tInfo.GetValue (tObject, null);
//								if (tPropertyValueOld.Value != tPropertyValue.Value) {
//									tInfo.SetValue (tObject, tPropertyValue, null);
//									NWDDataManager.SharedInstance.AddObjectToUpdateQueue (tObject);
//								}
//							}

						}
					}
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================