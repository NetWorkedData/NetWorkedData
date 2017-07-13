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
	public partial  class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		public static string ClassID ()
		{
			string tReturn = typeof(K).Name;
			return tReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, string> kMenuNameType = new Dictionary<string, string> ();
		//-------------------------------------------------------------------------------------------------------------
		public static string MenuName ()
		{
			string rReturn = null;
			//Debug.Log ("ClassID () to find key" + ClassID ());
			if (kMenuNameType.ContainsKey (ClassID ())) {
				//Debug.Log ("rReturn  find key");
				rReturn = kMenuNameType [ClassID ()];
			}
			if (rReturn == null) {
				rReturn = "";
			}
			//Debug.Log ("rReturn " + rReturn);
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void SetMenuName (string sMenuName)
		{
			if (sMenuName != null) {
				kMenuNameType [ClassID ()] = sMenuName;
			}
			;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, Type> kClassType = new Dictionary<string, Type> ();
		//-------------------------------------------------------------------------------------------------------------
		public static Type ClassType ()
		{
			Type rReturn = null;
			if (kClassType.ContainsKey (ClassID ())) {
				rReturn = kClassType [ClassID ()];
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void SetClassType (Type sType)
		{
			if (sType != null) {
				kClassType [ClassID ()] = sType;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, string> kClassName = new Dictionary<string, string> ();
		//-------------------------------------------------------------------------------------------------------------
		public static string[] AllClassName ()
		{
			return kClassName.Values.ToArray<string> ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public static string ClassName ()
		{
			string rReturn = "";
			if (kClassName.ContainsKey(ClassID ()))
			{
				rReturn = kClassName [ClassID ()];
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void SetClassName (string sClassName)
		{
			if (sClassName != null) {
				kClassName [ClassID ()] = sClassName;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, string> kTableName = new Dictionary<string, string> ();
		//-------------------------------------------------------------------------------------------------------------
		public static string[] AllTableName ()
		{
			return kTableName.Values.ToArray<string> ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public static string TableName ()
		{
			string rReturn = "";
			if (kTableName.ContainsKey(ClassID ()))
			{
				rReturn = kTableName [ClassID ()];
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void SetTableName (string sTableName)
		{
			if (sTableName != null) {
				kTableName [ClassID ()] = sTableName;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, string> kClassTrigramme = new Dictionary<string, string> ();
		//-------------------------------------------------------------------------------------------------------------
		public static string ClassTrigramme ()
		{
			string rReturn = "";
			if (kClassTrigramme.ContainsKey(ClassID ()))
			{
				rReturn = kClassTrigramme [ClassID ()];
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void SetClassTrigramme (string sClassTrigramme)
		{
			if (sClassTrigramme != null) {
				kClassTrigramme [ClassID ()] = sClassTrigramme;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, string> kClassDescription = new Dictionary<string, string> ();
		//-------------------------------------------------------------------------------------------------------------
		public static string ClassDescription ()
		{
			string rReturn = "";
			if (kClassDescription.ContainsKey(ClassID ()))
			{
				rReturn = kClassDescription [ClassID ()];
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void SetClassDescription (string sClassDescription)
		{
			if (sClassDescription != null) {
				kClassDescription [ClassID ()] = sClassDescription;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, string> kPrefBaseKey = new Dictionary<string, string> ();
		//-------------------------------------------------------------------------------------------------------------
		public static string PrefBaseKey ()
		{
			string rReturn = "";
			if (kPrefBaseKey.ContainsKey(ClassID ()))
			{
				rReturn = kPrefBaseKey [ClassID ()];
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void SetPrefBaseKey (string skPrefBaseKey)
		{
			if (skPrefBaseKey != null) {
				kPrefBaseKey [ClassID ()] = skPrefBaseKey;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, string> kPrefSalt = new Dictionary<string, string> ();
		//-------------------------------------------------------------------------------------------------------------
		public static string PrefSalt ()
		{
			string rReturn = "";
			if (kPrefSalt.ContainsKey(ClassID ()))
			{
				rReturn = kPrefSalt [ClassID ()];
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void SetPrefSalt (string sPrefSalt)
		{
			if (sPrefSalt != null) {
				kPrefSalt [ClassID ()] = sPrefSalt;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public static bool TestSaltValid()
		{
			bool rReturn = false;
			if (PrefSalt () == "ok") {
				rReturn = true;
			} else {
				//Debug.Log ("!!! error in salt memorize : " + ClassNamePHP ());
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, string> kPrefSaltA = new Dictionary<string, string> ();
		//-------------------------------------------------------------------------------------------------------------
		public static string PrefSaltA ()
		{
			string rReturn = "";
			if (kPrefSaltA.ContainsKey(ClassID ()))
			{
				rReturn = kPrefSaltA [ClassID ()];
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void SetPrefSaltA (string sPrefSaltA)
		{
			if (sPrefSaltA != null) {
				kPrefSaltA [ClassID ()] = sPrefSaltA;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, string> kPrefSaltB = new Dictionary<string, string> ();
		//-------------------------------------------------------------------------------------------------------------
		public static string PrefSaltB ()
		{
			string rReturn = "";
			if (kPrefSaltB.ContainsKey(ClassID ()))
			{
				rReturn = kPrefSaltB [ClassID ()];
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void SetPrefSaltB (string sPrefSaltB)
		{
			if (sPrefSaltB != null) {
				kPrefSaltB [ClassID ()] = sPrefSaltB;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<string, NWDBasis <K>> kObjectInEdition = new Dictionary<string, NWDBasis <K>> ();
		//-------------------------------------------------------------------------------------------------------------
		public static NWDBasis <K> ObjectInEdition ()
		{
			NWDBasis <K> rReturn = null;
			if (kObjectInEdition.ContainsKey (ClassID ())) {
				rReturn = kObjectInEdition [ClassID ()];
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void redefineClassToUse (Type sType, string sClassTrigramme, string sMenuName, string sDescription = "")
		{
			string tTableName = sType.Name;
			string tClassName = sType.AssemblyQualifiedName;
			SetClassType (sType);
			SetClassName (tClassName);
			SetTableName (tTableName);
			SetClassTrigramme (sClassTrigramme);
			SetPrefBaseKey (tTableName + "_");
			SetMenuName (sMenuName);
			SetClassDescription (sDescription);
			PrefLoad ();
            
#if UNITY_EDITOR
            CreateTable();
            LoadTableEditor();
            FilterTableEditor();
#else
            ConnectToDatabase();
            LoadTableEditor ();
#endif

            PrefSave();
		}
		//-------------------------------------------------------------------------------------------------------------
		static public string kPrefSaltValidKey = "SaltValid";
		static public string kPrefSaltAKey = "SaltA";
		static public string kPrefSaltBKey = "SaltB";
		//-------------------------------------------------------------------------------------------------------------
		protected static void PrefSave ()
		{
			//Debug.Log ("PrefSave");
			// reccord data to user's preferences
			NWDAppConfiguration.SharedInstance.SetSalt(PrefBaseKey (),kPrefSaltAKey, PrefSaltA ());
			NWDAppConfiguration.SharedInstance.SetSalt(PrefBaseKey (),kPrefSaltBKey, PrefSaltB ());
			NWDAppConfiguration.SharedInstance.SetSaltValid(PrefBaseKey (),kPrefSaltValidKey, "ok");
#if UNITY_EDITOR
			//NWDAppConfiguration.SharedInstance.SaveNewCSharpFile ();
#endif
		}

		public static void PrefLoad ()
		{
			//Debug.Log ("PrefLoad");
			// load data from user's preferences
			SetPrefSaltA (NWDAppConfiguration.SharedInstance.GetSalt(PrefBaseKey (),kPrefSaltAKey, kPrefSaltValidKey));
			SetPrefSaltB (NWDAppConfiguration.SharedInstance.GetSalt(PrefBaseKey (),kPrefSaltBKey, kPrefSaltValidKey));
			SetPrefSalt (NWDAppConfiguration.SharedInstance.GetSaltValid(PrefBaseKey (),kPrefSaltValidKey));
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================