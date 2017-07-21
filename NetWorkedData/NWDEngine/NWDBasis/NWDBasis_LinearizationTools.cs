using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;

using SQLite4Unity3d;

using UnityEngine;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	public partial  class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the reference value from CSV.
		/// </summary>
		/// <returns>The reference value from CSV.</returns>
		/// <param name="sDataArray">data array.</param>
		public static string GetReferenceValueFromCSV (string[] sDataArray)
		{
			return sDataArray [0];
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the DM value from CSV.
		/// </summary>
		/// <returns>The DM value from CSV.</returns>
		/// <param name="sDataArray">data array.</param>
		public static int GetDMValueFromCSV (string[] sDataArray)
		{
			int rReturn = 0;
			int.TryParse (sDataArray [1], out rReturn);
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets the integrity value from CSV.
		/// </summary>
		/// <returns>The integrity value from CSV.</returns>
		/// <param name="sDataArray">data array.</param>
		public static string GetIntegrityValueFromCSV (string[] sDataArray)
		{
			return sDataArray [sDataArray.Count () - 1];
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the integrity value from CSV.
		/// </summary>
		/// <returns><c>true</c>, if integrity value from CSV was tested, <c>false</c> otherwise.</returns>
		/// <param name="sDataArray">data array.</param>
		public static bool TestIntegrityValueFromCSV (string[] sDataArray)
		{
			bool rReturn = true;
			string tActualIntegrity = GetIntegrityValueFromCSV (sDataArray);
			string tAssembly = sDataArray [0] + sDataArray [1];
			int tMax = sDataArray.Count () - 1;
			for (int i = 6; i < tMax; i++) {
				tAssembly += sDataArray [i];
			}
			string tCalculateIntegrity = HashSum (PrefSaltA () + tAssembly + PrefSaltB ());
//			BTBDebug.LogVerbose ("tAssembly = " +tAssembly);
//			BTBDebug.LogVerbose ("tCalculateIntegrity = " + tCalculateIntegrity);
//			BTBDebug.LogVerbose ("tActualIntegrity = " + tActualIntegrity);
			if (tActualIntegrity != tCalculateIntegrity) {
				rReturn = false;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the with CSV.
		/// </summary>
		/// <param name="sDataArray">data array.</param>
		public void UpdateWithCSV (NWDAppEnvironment sEnvironment, string[] sDataArray)
		{
			// get key order assembly of cvs
			string[] tKey = CSVAssemblyOrderArray ();
			// get values 
			string[] tValue = sDataArray;
			// Short circuit the sync date
			// not replace the date from the other environment
			if (sEnvironment == NWDAppConfiguration.SharedInstance.DevEnvironment) {
				tValue [4] = PreprodSync.ToString ();
				tValue [5] = ProdSync.ToString ();
			} else if (sEnvironment == NWDAppConfiguration.SharedInstance.PreprodEnvironment) {
				tValue [3] = DevSync.ToString ();
				tValue [5] = ProdSync.ToString ();
			} else if (sEnvironment == NWDAppConfiguration.SharedInstance.ProdEnvironment) {
				tValue [3] = DevSync.ToString ();
				tValue [4] = PreprodSync.ToString ();
			}
			// process to insertion
			Type tType = ClassType ();
			for (int tI = 0; tI < tKey.Count (); tI++) {
				if (tValue.Count () > tI) {
					PropertyInfo tPropertyInfo = tType.GetProperty (tKey [tI], BindingFlags.Public | BindingFlags.Instance);
					Type tTypeOfThis = tPropertyInfo.PropertyType;
					string tValueString = tValue [tI] as string;

					//TO-DO : (FUTUR ADDS) Insert new NWDxxxxType
					// Do for NWD simple Structures

					if (tTypeOfThis.IsSubclassOf (typeof(BTBDataType))) {
						var tObject = Activator.CreateInstance (tTypeOfThis);
						var tMethodInfo = tObject.GetType ().GetMethod ("SetString", BindingFlags.Public | BindingFlags.Instance);
						if (tMethodInfo != null) {
							tMethodInfo.Invoke (tObject, new object[]{ tValueString });
							}
						tPropertyInfo.SetValue (this, tObject, null);
					}
//					if (tTypeOfThis == typeof(NWDColorType)) {
//						tPropertyInfo.SetValue (this, new NWDColorType (tValueString), null);
//					} else if (tTypeOfThis == typeof(NWDVersionType)) {
//						tPropertyInfo.SetValue (this, new NWDVersionType (tValueString), null);
//					} else if (tTypeOfThis == typeof(NWDPrefabType)) {
//						tPropertyInfo.SetValue (this, new NWDPrefabType (tValueString), null);
//					} else if (tTypeOfThis == typeof(NWDTextureType)) {
//						tPropertyInfo.SetValue (this, new NWDTextureType (tValueString), null);
//					} else if (tTypeOfThis == typeof(NWDLocalizableStringType)) {
//						tPropertyInfo.SetValue (this, new NWDLocalizableStringType (tValueString), null);
//					} else if (tTypeOfThis == typeof(NWDLocalizableTextType)) {
//						tPropertyInfo.SetValue (this, new NWDLocalizableTextType (tValueString), null);
//					} else if (tTypeOfThis == typeof(NWDLocalizablePrefabType)) {
//						tPropertyInfo.SetValue (this, new NWDLocalizablePrefabType (tValueString), null);
//					} else if (tTypeOfThis == typeof(NWDPrefabType)) {
//						tPropertyInfo.SetValue (this, new NWDPrefabType (tValueString), null);
//					} 
//					// Do for NWD Complexe Structures
//					else if (tTypeOfThis.IsGenericType == true) {
//						if (tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferenceType<>)
//						    || tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferenceHashType<>)
//						    || tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferencesListType<>)
//						    || tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferencesQuantityType<>)) {
//							var tObject = Activator.CreateInstance (tTypeOfThis);
//							var tMethodInfo = tObject.GetType ().GetMethod ("SetString", BindingFlags.Public | BindingFlags.Instance);
//							if (tMethodInfo != null) {
//								tMethodInfo.Invoke (tObject, new object[]{ tValueString });
//							}
//							tPropertyInfo.SetValue (this, tObject, null);
//						}
//
//					}
					// Do for Standard type
					else if (tTypeOfThis == typeof(String) || tTypeOfThis == typeof(string)) {
						tPropertyInfo.SetValue (this, tValueString, null);
					} else if (tTypeOfThis == typeof(bool)) {
						bool tValueInsert = false; 
						int tTemp = 0;
						int.TryParse (tValueString, out tTemp);
						if (tTemp > 0) {
							tValueInsert = true;
						}
						tPropertyInfo.SetValue (this, tValueInsert, null);
					} else if (tTypeOfThis == typeof(int) || tTypeOfThis == typeof(Int16) || tTypeOfThis == typeof(Int32) || tTypeOfThis == typeof(Int64)) {
						int tValueInsert = 0; 
						int.TryParse (tValueString, out tValueInsert);
						tPropertyInfo.SetValue (this, tValueInsert, null);
					} else if (tTypeOfThis == typeof(float) || tTypeOfThis == typeof(double) || tTypeOfThis == typeof(Single) || tTypeOfThis == typeof(Double) || tTypeOfThis == typeof(Decimal)) {
						float tValueInsert = 0; 
						float.TryParse (tValueString, out tValueInsert);
						tPropertyInfo.SetValue (this, tValueInsert, null);
					} else {

					}
				}
			}
			NWDDataManager.SharedInstance.UpdateObject (this);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Propertieses the order array.
		/// </summary>
		/// <returns>The order array.</returns>
		public static List<string> PropertiesOrderArray ()
		{
			List<string> rReturn = new List<string> ();
			Type tType = ClassType ();
			foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
				rReturn.Add (tProp.Name);
			}
			rReturn.Sort ();
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// CSV assembly order array.
		/// </summary>
		/// <returns>The assembly order array.</returns>
		public static string[] CSVAssemblyOrderArray ()
		{
			List<string> rReturn = PropertiesOrderArray ();
			rReturn.Remove ("Integrity");
			rReturn.Remove ("Reference");
			rReturn.Remove ("ID");
			rReturn.Remove ("DM");
			rReturn.Remove ("DS");
			rReturn.Remove ("DevSync");
			rReturn.Remove ("PreprodSync");
			rReturn.Remove ("ProdSync");
			// add the good order for this element
			rReturn.Insert (0, "Reference");
			rReturn.Insert (1, "DM");
			rReturn.Insert (2, "DS");
			rReturn.Insert (3, "DevSync");
			rReturn.Insert (4, "PreprodSync");
			rReturn.Insert (5, "ProdSync");
			rReturn.Add ("Integrity");
			return rReturn.ToArray<string> ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// SLQs the assembly order array.
		/// </summary>
		/// <returns>The assembly order array.</returns>
		public static string[] SLQAssemblyOrderArray ()
		{
			List<string> rReturn = PropertiesOrderArray ();
			rReturn.Remove ("Integrity");
			rReturn.Remove ("Reference");
			rReturn.Remove ("ID");
			rReturn.Remove ("DM");
			rReturn.Remove ("DS");
			rReturn.Remove ("DevSync");
			rReturn.Remove ("PreprodSync");
			rReturn.Remove ("ProdSync");
			// add the good order for this element
			rReturn.Insert (0, "DM");
			rReturn.Insert (1, "DS");
			rReturn.Insert (2, "DevSync");
			rReturn.Insert (3, "PreprodSync");
			rReturn.Insert (4, "ProdSync");
			rReturn.Add ("Integrity");
			return rReturn.ToArray<string> ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// SLQs the assembly order.
		/// </summary>
		/// <returns>The assembly order.</returns>
		public static string SLQAssemblyOrder ()
		{
			List<string> rReturn = PropertiesOrderArray ();
			rReturn.Remove ("Integrity");
			rReturn.Remove ("Reference");
			rReturn.Remove ("ID");
			rReturn.Remove ("DM");
			rReturn.Remove ("DS");
			rReturn.Remove ("DevSync");
			rReturn.Remove ("PreprodSync");
			rReturn.Remove ("ProdSync");
			// add the good order for this element
			rReturn.Insert (0, "Reference");
			rReturn.Insert (1, "DM");
			rReturn.Insert (2, "DS");
			rReturn.Insert (3, "DevSync");
			rReturn.Insert (4, "PreprodSync");
			rReturn.Insert (5, "ProdSync");
			rReturn.Add ("Integrity");
			return "`" + string.Join ("`, `", rReturn.ToArray ()) + "`";
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// SLQs the assembly order.
		/// </summary>
		/// <returns>The assembly order.</returns>
		public static List<string> SLQIntegrityOrder ()
		{
			List<string> rReturn = PropertiesOrderArray ();
			rReturn.Remove ("Integrity");
			rReturn.Remove ("Reference");
			rReturn.Remove ("ID");
			rReturn.Remove ("DM");
			rReturn.Remove ("DS");
			rReturn.Remove ("DevSync");
			rReturn.Remove ("PreprodSync");
			rReturn.Remove ("ProdSync");
			// add the good order for this element
			rReturn.Insert (0, "Reference");
			rReturn.Insert (1, "DM");
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Datas assembly for integrity calculate or cvs export.
		/// </summary>
		/// <returns>The assembly.</returns>
		/// <param name="sAsssemblyAsCVS">If set to <c>true</c> asssembly as CSV.</param>
		public string DataAssembly (bool sAsssemblyAsCSV = false)
		{
			string rReturn = "";
			Type tType = ClassType ();
			List<string> tPropertiesList = PropertiesOrderArray ();
			tPropertiesList.Remove ("Integrity"); // not include in integrity
			tPropertiesList.Remove ("Reference");
			tPropertiesList.Remove ("ID");
			tPropertiesList.Remove ("DM");
			tPropertiesList.Remove ("DS");// not include in integrity
			tPropertiesList.Remove ("DevSync");// not include in integrity
			tPropertiesList.Remove ("PreprodSync");// not include in integrity
			tPropertiesList.Remove ("ProdSync");// not include in integrity
			foreach (string tPropertieName in tPropertiesList) {
				PropertyInfo tProp = tType.GetProperty (tPropertieName);
				Type tTypeOfThis = tProp.PropertyType;
				string tValueString = "";

				//TO-DO : (FUTUR ADDS) Insert new NWDxxxxType
//				if (tTypeOfThis == typeof(NWDColorType)) {
//					NWDColorType tValueStruct = (NWDColorType)tProp.GetValue (this, null);
//					tValueString = tValueStruct.ToString ();
//				} else if (tTypeOfThis == typeof(NWDVersionType)) {
//					NWDVersionType tValueStruct = (NWDVersionType)tProp.GetValue (this, null);
//					tValueString = tValueStruct.ToString ();
//				} else if (tTypeOfThis == typeof(NWDPrefabType)) {
//					NWDPrefabType tValueStruct = (NWDPrefabType)tProp.GetValue (this, null);
//					tValueString = tValueStruct.ToString ();
//				} else if (tTypeOfThis == typeof(NWDTextureType)) {
//					NWDTextureType tValueStruct = (NWDTextureType)tProp.GetValue (this, null);
//					tValueString = tValueStruct.ToString ();
//				}
//				else if (tTypeOfThis == typeof(NWDLocalizableStringType)) {
//					NWDLocalizableStringType tValueStruct = (NWDLocalizableStringType)tProp.GetValue (this, null);
//					tValueString = tValueStruct.ToString ();
//				} else if (tTypeOfThis == typeof(NWDLocalizablePrefabType)) {
//					NWDLocalizablePrefabType tValueStruct = (NWDLocalizablePrefabType)tProp.GetValue (this, null);
//					tValueString = tValueStruct.ToString ();
//				} else if (tTypeOfThis == typeof(NWDLocalizableTextType)) {
//					NWDLocalizableTextType tValueStruct = (NWDLocalizableTextType)tProp.GetValue (this, null);
//					tValueString = tValueStruct.ToString ();
//				}
//
//				else if (tTypeOfThis.IsGenericType==true)
//				{
//				if (tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferenceType<>) 
//					||tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferenceHashType<>)
//					||tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferencesListType<>)
//					||tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferencesQuantityType<>)
//				) 
//				{
//					var tMethodInfo = tTypeOfThis.GetMethod ("ToString", BindingFlags.Public | BindingFlags.Instance);
//					if (tMethodInfo != null)
//					{
//						tValueString = tMethodInfo.Invoke (tProp.GetValue (this, null), null) as string;
//					}
//				}
//				}
//				else {
					object tValue = tProp.GetValue (this, null);
				if (tValue == null) {
					tValue = "";
				}
					tValueString = tValue.ToString ();
					if (tTypeOfThis == typeof(bool)) {
                    //BTBDebug.Log ("REFERENCE " + Reference + " AC + " + AC + " : " + tValueString);
                    if (tValueString == "False") {
							tValueString = "0";
						} else {
							tValueString = "1";
						}
					}
//				}
				if (sAsssemblyAsCSV == true) {
					rReturn += NWDToolbox.TextCSVProtect (tValueString) + NWDConstants.kStandardSeparator;
				} else {
					rReturn += NWDToolbox.TextCSVProtect (tValueString);
				}
			}
			if (sAsssemblyAsCSV == true) {
				rReturn = Reference + NWDConstants.kStandardSeparator +
					DM + NWDConstants.kStandardSeparator +
					DS + NWDConstants.kStandardSeparator + 
					DevSync + NWDConstants.kStandardSeparator +
					PreprodSync + NWDConstants.kStandardSeparator +
					ProdSync + NWDConstants.kStandardSeparator +
					rReturn + 
					Integrity;
			} else {
				rReturn = Reference +
					DM +
					//					DS +  // not include in integrity
					//					DevSync + // not include in integrity
					//					PreprodSync + // not include in integrity
					//					ProdSync + // not include in integrity
					rReturn;
			}
            //BTBDebug.Log ("DataAssembly = " + rReturn);
            return rReturn;
		}
	}
}
//=====================================================================================================================