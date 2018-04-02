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

using UnityEngine;

using SQLite4Unity3d;


#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	public class NWDTypeInfos
	{
		//-------------------------------------------------------------------------------------------------------------
		public Type ClassType = null;
		public string ClassName = "";
		public bool ServerSynchronize;
		public string TrigrammeName = "";
		public string ClassDescription = "";
		public string MenuName = "";
		public string TableName = "";
        public GUIContent MenuNameContent = GUIContent.none;
		//-------------------------------------------------------------------------------------------------------------
		public string PrefBaseKey = "";

		static public string kPrefSaltValidKey = "SaltValid";
		static public string kPrefSaltAKey = "SaltA";
		static public string kPrefSaltBKey = "SaltB";

		public string SaltA = "";
		public string SaltB = "";
		public string SaltOk = "";




//		//-------------------------------------------------------------------------------------------------------------
//		protected void PrefSave ()
//		{
//			#if UNITY_EDITOR
//			//Debug.Log ("PrefSave");
//			// reccord data to user's preferences
//			NWDAppConfiguration.SharedInstance().SetSalt(PrefBaseKey,kPrefSaltAKey, SaltA);
//			NWDAppConfiguration.SharedInstance().SetSalt(PrefBaseKey,kPrefSaltBKey, SaltB);
//			NWDAppConfiguration.SharedInstance().SetSaltValid(PrefBaseKey,kPrefSaltValidKey, "ok");
//			//NWDAppConfiguration.SharedInstance().SaveNewCSharpFile ();
//			#endif
//		}
//		//-------------------------------------------------------------------------------------------------------------
//		public void PrefLoad ()
//		{
//			//Debug.Log ("PrefLoad");
//			// load data from user's preferences
//			SetPrefSaltA (NWDAppConfiguration.SharedInstance().GetSalt(PrefBaseKey,kPrefSaltAKey, kPrefSaltValidKey));
//			SetPrefSaltB (NWDAppConfiguration.SharedInstance().GetSalt(PrefBaseKey,kPrefSaltBKey, kPrefSaltValidKey));
//			SetPrefSalt (NWDAppConfiguration.SharedInstance().GetSaltValid(PrefBaseKey,kPrefSaltValidKey));
//		}
//




		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		//public Object kObjectInEdition;
		//public Object[] kObjectsArrayInEdition;
		//-------------------------------------------------------------------------------------------------------------
		public string m_SearchInternalName = "";
		public string m_SearchInternalDescription = "";
		public Vector2 m_ScrollPositionCard;
		public bool mSearchShowing = false;
		//-------------------------------------------------------------------------------------------------------------
		public List<string> ObjectsInEditorTableKeyList = new List<string> ();
		public List<string> ObjectsInEditorTableList = new List<string> ();
		public List<bool> ObjectsInEditorTableSelectionList = new List<bool> ();
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
		public NWDTypeInfos ()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<Type,NWDTypeInfos> TypesDictionary = new Dictionary<Type,NWDTypeInfos> ();
		//-------------------------------------------------------------------------------------------------------------
		public static void Declare (Type sType, bool sServerSynchronize, string sTrigrammeName, string sMenuName, string sDescription)
		{
			if (sType.IsSubclassOf (typeof(NWDTypeClass))) {
                // find infos object if exists or create 
				NWDTypeInfos tTypeInfos = null;
				if (TypesDictionary.ContainsKey (sType)) {
					tTypeInfos = TypesDictionary [sType];
				} else {
					tTypeInfos = new NWDTypeInfos ();
					TypesDictionary.Add (sType, tTypeInfos);
				}
                // insert basic infos
				tTypeInfos.ClassType = sType;
				tTypeInfos.TableName = sType.Name;
				tTypeInfos.ClassName = sType.AssemblyQualifiedName;
                // insert attributs infos
				tTypeInfos.TrigrammeName = sTrigrammeName;
				tTypeInfos.MenuName = sMenuName;
				tTypeInfos.ClassDescription = sDescription;
                tTypeInfos.ServerSynchronize = sServerSynchronize;
                // create GUI object
                tTypeInfos.MenuNameContent = new GUIContent(sMenuName, sDescription);
                // Prepare engine informlations
				tTypeInfos.PrefBaseKey = sType.Name + "_";
				tTypeInfos.PropertiesArrayPrepare ();
				tTypeInfos.PropertiesOrderArrayPrepare ();
				tTypeInfos.SLQAssemblyOrderArrayPrepare ();
				tTypeInfos.SLQAssemblyOrderPrepare ();
				tTypeInfos.SLQIntegrityOrderPrepare ();
				tTypeInfos.DataAssemblyPropertiesListPrepare ();
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDTypeInfos FindTypeInfos (Type sType)
		{
			NWDTypeInfos tTypeInfos = null;
			if (sType.IsSubclassOf (typeof(NWDTypeClass))) {
				if (TypesDictionary.ContainsKey (sType)) {
					tTypeInfos = TypesDictionary [sType];
				}
			}
			return tTypeInfos;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static string Informations (Type sType)
		{
			string rReturn = "";
			NWDTypeInfos tTypeInfos = FindTypeInfos (sType);
			if (tTypeInfos == null) {
				rReturn = "unknow";
			} else {
				rReturn = tTypeInfos.Informationss ();
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
        public string Informationss ()
		{
			return "ClassName = '" + ClassName + "' " +
			"TrigrammeName = '" + TrigrammeName + "' " +
			"ServerSynchronize = '" + ServerSynchronize + "' " +
			"ClassDescription = '" + ClassDescription + "' " +
			"MenuName = '" + MenuName + "' " +
			"";
		}
		//-------------------------------------------------------------------------------------------------------------
		//public static void AllInfos ()
		//{
		//	foreach (KeyValuePair<Type,NWDTypeInfos> tTypeTypeInfos in TypesDictionary) {
		//		Type tType = tTypeTypeInfos.Key;
		//	}
		//}
		//-------------------------------------------------------------------------------------------------------------
		public PropertyInfo[] PropertiesArray;
		//-------------------------------------------------------------------------------------------------------------
		public void PropertiesArrayPrepare ()
		{
			PropertiesArray = ClassType.GetProperties (BindingFlags.Public | BindingFlags.Instance);
		}
		//-------------------------------------------------------------------------------------------------------------
		public List<string> PropertiesOrderArray;
		//-------------------------------------------------------------------------------------------------------------
		public void PropertiesOrderArrayPrepare ()
		{
			List<string> rReturn = new List<string> ();
			foreach (var tProp in PropertiesArray) {
				rReturn.Add (tProp.Name);
			}
			rReturn.Sort ();
			PropertiesOrderArray = rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public string[] CSVAssemblyOrderArray;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// CSV assembly order array.
		/// </summary>
		/// <returns>The assembly order array.</returns>
		public void CSVAssemblyOrderArrayPrepare ()
		{
			List<string> rReturn = new List<string> ();
			rReturn.AddRange (PropertiesOrderArray);
			rReturn.Remove ("Integrity");
			rReturn.Remove ("Reference");
			rReturn.Remove ("ID");
			rReturn.Remove ("DM");
            rReturn.Remove ("DS");
            rReturn.Remove("ServerHash");
            rReturn.Remove("ServerLog");
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
			CSVAssemblyOrderArray = rReturn.ToArray<string> ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public  string[] SLQAssemblyOrderArray;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// SLQs the assembly order array.
		/// </summary>
		/// <returns>The assembly order array.</returns>
		public void SLQAssemblyOrderArrayPrepare ()
		{
			List<string> rReturn = new List<string> ();
			rReturn.AddRange (PropertiesOrderArray);
			rReturn.Remove ("Integrity");
			rReturn.Remove ("Reference");
			rReturn.Remove ("ID");
			rReturn.Remove ("DM");
            rReturn.Remove ("DS");
            rReturn.Remove("ServerHash");
            rReturn.Remove("ServerLog");
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
			SLQAssemblyOrderArray = rReturn.ToArray<string> ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public string SLQAssemblyOrder;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// SLQs the assembly order.
		/// </summary>
		/// <returns>The assembly order.</returns>
		public void SLQAssemblyOrderPrepare ()
		{
			List<string> rReturn = new List<string> ();
			rReturn.AddRange (PropertiesOrderArray);
			rReturn.Remove ("Integrity");
			rReturn.Remove ("Reference");
			rReturn.Remove ("ID");
			rReturn.Remove ("DM");
            rReturn.Remove ("DS");
            rReturn.Remove("ServerHash");
            rReturn.Remove("ServerLog");
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
			SLQAssemblyOrder = "`" + string.Join ("`, `", rReturn.ToArray ()) + "`";
		}
		//-------------------------------------------------------------------------------------------------------------
		public List<string> SLQIntegrityOrder;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// SLQs the assembly order.
		/// </summary>
		/// <returns>The assembly order.</returns>
		public void SLQIntegrityOrderPrepare ()
		{
			List<string> rReturn = new List<string> ();
			rReturn.AddRange (PropertiesOrderArray);
			rReturn.Remove ("Integrity");
			rReturn.Remove ("Reference");
			rReturn.Remove ("ID");
			rReturn.Remove ("DM");
            rReturn.Remove ("DS");
            rReturn.Remove("ServerHash");
            rReturn.Remove("ServerLog");
			rReturn.Remove ("DevSync");
			rReturn.Remove ("PreprodSync");
			rReturn.Remove ("ProdSync");
			// add the good order for this element
			rReturn.Insert (0, "Reference");
			rReturn.Insert (1, "DM");
			SLQIntegrityOrder = rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public List<string> DataAssemblyPropertiesList;
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// SLQs the assembly order.
		/// </summary>
		/// <returns>The assembly order.</returns>
		public void DataAssemblyPropertiesListPrepare ()
		{
			List<string> rReturn = new List<string> ();
			rReturn.AddRange (PropertiesOrderArray);
			rReturn.Remove ("Integrity"); // not include in integrity
			rReturn.Remove ("Reference");
			rReturn.Remove ("ID");
			rReturn.Remove ("DM");
            rReturn.Remove ("DS");// not include in integrity
            rReturn.Remove("ServerHash");// not include in integrity
            rReturn.Remove("ServerLog");// not include in integrity
			rReturn.Remove ("DevSync");// not include in integrity
			rReturn.Remove ("PreprodSync");// not include in integrity
			rReturn.Remove ("ProdSync");// not include in integrity
			DataAssemblyPropertiesList = rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool kAccountDependent;
		public PropertyInfo[] kAccountDependentProperties;
		public PropertyInfo[] kAccountConnectedProperties;
		public bool kLockedObject;
		public bool kAssetDependent;
		public PropertyInfo[] kAssetDependentProperties;
		//-------------------------------------------------------------------------------------------------------------

		//-------------------------------------------------------------------------------------------------------------

		//-------------------------------------------------------------------------------------------------------------

		//-------------------------------------------------------------------------------------------------------------
		public List<object> ObjectsList = new List<object> ();
		public List<string> ObjectsByReferenceList = new List<string> ();
		public List<string> ObjectsByKeyList = new List<string> (); // TODO Rename ObjectsByInternalKeyList
		//-------------------------------------------------------------------------------------------------------------

		//-------------------------------------------------------------------------------------------------------------

		//-------------------------------------------------------------------------------------------------------------

		public static string SynchronizeKeyData = "data";
		public static string SynchronizeKeyDataCount = "rowCount";
		public static string SynchronizeKeyTimestamp = "sync";
		public static string SynchronizeKeyLastTimestamp = "last";
		public static string SynchronizeKeyInWaitingTimestamp = "waiting";
		//-------------------------------------------------------------------------------------------------------------


		public static bool mSettingsShowing = false;
		//-------------------------------------------------------------------------------------------------------------
		public static bool mForceSynchronization = false;
		//-------------------------------------------------------------------------------------------------------------

		public static Vector2 m_ObjectEditorScrollPosition = Vector2.zero; // not obligation
		//-------------------------------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------------------------------
		//-------------------------------------------------------------------------------------------------------------


	}

}
//=====================================================================================================================