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

		#region Class Methods

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
		/// Gets the DM value from CSV. DM for Date Modification.
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
			if (tActualIntegrity != tCalculateIntegrity) {
				rReturn = false;
			}
			return rReturn;
		}

		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, List<string>> kPropertiesOrderArray = new Dictionary<string, List<string>> ();
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Propertieses the order array.
		/// </summary>
		/// <returns>The order array.</returns>
		public static List<string> PropertiesOrderArray ()
		{
			if (kPropertiesOrderArray.ContainsKey (ClassID ()) == false) {
				List<string> rReturn = new List<string> ();
				Type tType = ClassType ();
				foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
					rReturn.Add (tProp.Name);
				}
				rReturn.Sort ();
				kPropertiesOrderArray [ClassID ()] = rReturn;
			} 
			return  kPropertiesOrderArray [ClassID ()];
		}
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, string[]> kCSVAssemblyOrderArray = new Dictionary<string, string[]> ();
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// CSV assembly order array.
		/// </summary>
		/// <returns>The assembly order array.</returns>
		public static string[] CSVAssemblyOrderArray ()
		{
			if (kCSVAssemblyOrderArray.ContainsKey (ClassID ()) == false) {
				List<string> rReturn = new List<string> ();
				rReturn.AddRange(PropertiesOrderArray ());
				rReturn.Remove ("Integrity");
				rReturn.Remove ("Reference");
				rReturn.Remove ("ID");
				rReturn.Remove ("DM");
                rReturn.Remove ("DS");
                rReturn.Remove ("ServerHash");
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
				kCSVAssemblyOrderArray [ClassID ()] = rReturn.ToArray<string> ();
			}
			return kCSVAssemblyOrderArray [ClassID ()];
		}
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, string[]> kSLQAssemblyOrderArray = new Dictionary<string, string[]> ();
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// SLQs the assembly order array.
		/// </summary>
		/// <returns>The assembly order array.</returns>
		public static string[] SLQAssemblyOrderArray ()
		{
			if (kSLQAssemblyOrderArray.ContainsKey (ClassID ()) == false) {
				List<string> rReturn = new List<string> ();
				rReturn.AddRange(PropertiesOrderArray ());
				rReturn.Remove ("Integrity");
				rReturn.Remove ("Reference");
				rReturn.Remove ("ID");
				rReturn.Remove ("DM");
                rReturn.Remove ("DS");
                rReturn.Remove ("ServerHash");
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
				kSLQAssemblyOrderArray [ClassID ()] = rReturn.ToArray<string> ();
			}
			return kSLQAssemblyOrderArray [ClassID ()];
		}
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, string> kSLQAssemblyOrder = new Dictionary<string, string> ();
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// SLQs the assembly order.
		/// </summary>
		/// <returns>The assembly order.</returns>
		public static string SLQAssemblyOrder ()
		{
			if (kSLQAssemblyOrder.ContainsKey (ClassID ()) == false) {
				List<string> rReturn = new List<string> ();
				rReturn.AddRange(PropertiesOrderArray ());
				rReturn.Remove ("Integrity");
				rReturn.Remove ("Reference");
				rReturn.Remove ("ID");
				rReturn.Remove ("DM");
                rReturn.Remove ("DS");
                rReturn.Remove ("ServerHash");
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
				kSLQAssemblyOrder [ClassID ()] = "`" + string.Join ("`, `", rReturn.ToArray ()) + "`";
			}
			return kSLQAssemblyOrder [ClassID ()];
		}

		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, List<string>> kSLQIntegrityOrder = new Dictionary<string, List<string>> ();
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// SLQs the assembly order.
		/// </summary>
		/// <returns>The assembly order.</returns>
		public static List<string> SLQIntegrityOrder ()
		{
			if (kSLQIntegrityOrder.ContainsKey (ClassID ()) == false) {
				List<string> rReturn = new List<string> ();
				rReturn.AddRange(PropertiesOrderArray ());
				rReturn.Remove ("Integrity");
				rReturn.Remove ("Reference");
				rReturn.Remove ("ID");
				rReturn.Remove ("DM");
                rReturn.Remove ("DS");
                rReturn.Remove ("ServerHash");
				rReturn.Remove ("DevSync");
				rReturn.Remove ("PreprodSync");
                rReturn.Remove ("ProdSync");
                rReturn.Sort((tA, tB) => tA.CompareTo(tB));
				// add the good order for this element
				rReturn.Insert (0, "Reference");
				rReturn.Insert (1, "DM");
				kSLQIntegrityOrder [ClassID ()] = rReturn;
			}
			return kSLQIntegrityOrder [ClassID ()];
		}

        //-------------------------------------------------------------------------------------------------------------
        public static Dictionary<string, List<string>> kSLQIntegrityServerOrder = new Dictionary<string, List<string>>();
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// SLQs the assembly order.
        /// </summary>
        /// <returns>The assembly order.</returns>
        public static List<string> SLQIntegrityServerOrder()
        {
            if (kSLQIntegrityServerOrder.ContainsKey(ClassID()) == false)
            {
                List<string> rReturn = new List<string>();
                rReturn.AddRange(PropertiesOrderArray());
                rReturn.Remove("Integrity");
                rReturn.Remove("Reference");
                rReturn.Remove("ID");
                rReturn.Remove("DS");
                rReturn.Remove("ServerHash");
                rReturn.Remove("DevSync");
                rReturn.Remove("PreprodSync");
                rReturn.Remove("ProdSync");

                // I remove this to be able to trash and untrash object without break server integrity (perhaps bad solution ?)
                rReturn.Remove("DM");
                rReturn.Remove("AC");
                rReturn.Remove("DC");
                rReturn.Remove("DD");
                // add the good order for this element
                rReturn.Sort((tA, tB) => tB.CompareTo(tA));
                // add the good order for this element
                rReturn.Insert(2, "Reference");
                // add another order for these element (perhaps bad solution ?)
                rReturn.Add("AC");
                //rReturn.Add("DD");
                rReturn.Add("DC");
                //rReturn.Add("DM");
                kSLQIntegrityServerOrder[ClassID()] = rReturn;
            }
            return kSLQIntegrityServerOrder[ClassID()];
        }
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, List<string>> kDataAssemblyPropertiesList = new Dictionary<string, List<string>> ();
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// SLQs the assembly order.
		/// </summary>
		/// <returns>The assembly order.</returns>
		public static List<string> DataAssemblyPropertiesList ()
		{
			if (kDataAssemblyPropertiesList.ContainsKey (ClassID ()) == false) {
				List<string> rReturn = new List<string> ();
				rReturn.AddRange(PropertiesOrderArray ());
				rReturn.Remove ("Integrity"); // not include in integrity
				rReturn.Remove ("Reference");
				rReturn.Remove ("ID");
				rReturn.Remove ("DM");
                rReturn.Remove ("DS");// not include in integrity
                rReturn.Remove ("ServerHash");// not include in integrity
				rReturn.Remove ("DevSync");// not include in integrity
				rReturn.Remove ("PreprodSync");// not include in integrity
				rReturn.Remove ("ProdSync");// not include in integrity
				kDataAssemblyPropertiesList [ClassID ()] = rReturn;
			}
			return kDataAssemblyPropertiesList [ClassID ()];
		}



		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// New instance from CSV.
		/// </summary>
		/// <returns>The instance from CS.</returns>
		/// <param name="sEnvironment">S environment.</param>
		/// <param name="sDataArray">S data array.</param>
		private static NWDBasis<K> NewInstanceFromCSV (NWDAppEnvironment sEnvironment, string[] sDataArray)
		{
			NWDBasis<K> rReturnObject = null;
			rReturnObject = (NWDBasis<K>)Activator.CreateInstance (ClassType ());
			rReturnObject.InstanceInit ();
			rReturnObject.UpdateWithCSV (sEnvironment, sDataArray);
			NWDDataManager.SharedInstance.InsertObject (rReturnObject,AccountDependent ());
			return rReturnObject;
		}
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Instance Methods

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the with CSV.
		/// </summary>
		/// <param name="sDataArray">data array.</param>
		public void UpdateWithCSV (NWDAppEnvironment sEnvironment, string[] sDataArray)
		{
            //Debug.Log("UpdateWithCSV ref " + Reference);
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

                  if (tTypeOfThis.IsEnum)
                    {
                        // sign = (NWDAppEnvironmentPlayerStatut)Enum.Parse(typeof(NWDAppEnvironmentPlayerStatut), data["sign"].ToString(), true);
                        int tValueInsert = 0;
                        int.TryParse(tValueString, out tValueInsert);
                        tPropertyInfo.SetValue(this, tValueInsert, null);
                    }
                    else if (tTypeOfThis.IsSubclassOf (typeof(BTBDataType))) {
//						var tObject = Activator.CreateInstance (tTypeOfThis);
//						var tMethodInfo = tObject.GetType ().GetMethod ("SetString", BindingFlags.Public | BindingFlags.Instance);
//						if (tMethodInfo != null) {
//							tMethodInfo.Invoke (tObject, new object[]{ tValueString });
//						}
//
						BTBDataType tObject = Activator.CreateInstance (tTypeOfThis) as BTBDataType;
						tObject.SetString (tValueString);

						tPropertyInfo.SetValue (this, tObject, null);
					}
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
			NWDDataManager.SharedInstance.UpdateObject (this,AccountDependent ());
			AddonUpdatedMeFromWeb ();
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
			List<string> tPropertiesList = DataAssemblyPropertiesList ();
			foreach (string tPropertieName in tPropertiesList) {
				PropertyInfo tProp = tType.GetProperty (tPropertieName);
				Type tTypeOfThis = tProp.PropertyType;

               // Debug.Log("this prop "+tProp.Name+" is type : " + tTypeOfThis.Name );

				string tValueString = "";

				object tValue = tProp.GetValue (this, null);
				if (tValue == null) {
					tValue = "";
				}
                tValueString = tValue.ToString ();
                if (tTypeOfThis.IsEnum)
                {
                    //Debug.Log("this prop  " + tTypeOfThis.Name + " is an enum");
                    int tInt = (int)tValue;
                    tValueString = tInt.ToString(); 
                }
				if (tTypeOfThis == typeof(bool)) {
					//Debug.Log ("REFERENCE " + Reference + " AC + " + AC + " : " + tValueString);
					if (tValueString == "False") {
						tValueString = "0";
					} else {
						tValueString = "1";
					}
				}
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
				rReturn;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================