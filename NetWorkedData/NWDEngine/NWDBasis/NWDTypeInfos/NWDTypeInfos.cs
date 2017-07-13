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
		//-------------------------------------------------------------------------------------------------------------
//		public string SaltOk = "";
//		public string SaltA = "";
//		public string SaltB = "";
//		public string PrefBaseKey = "";
		//-------------------------------------------------------------------------------------------------------------
		public NWDTypeInfos ()
		{
		}
		//-------------------------------------------------------------------------------------------------------------
		public static Dictionary<Type,NWDTypeInfos> TypesDictionary = new Dictionary<Type,NWDTypeInfos> ();
		//-------------------------------------------------------------------------------------------------------------
		public static void Declare (Type sType, bool sServerSynchronize, string sTrigrammeName, string sDescription, string sMenuName)
		{
			if (sType.IsSubclassOf (typeof(NWDTypeClass))) {
				NWDTypeInfos tTypeInfos = null;
				if (TypesDictionary.ContainsKey (sType)) {
					tTypeInfos = TypesDictionary [sType];
				} else {
					tTypeInfos = new NWDTypeInfos ();
					TypesDictionary.Add (sType, tTypeInfos);
				}
				tTypeInfos.ClassName = sType.Name;
				tTypeInfos.TrigrammeName = sTrigrammeName;
				tTypeInfos.MenuName = sMenuName;
				tTypeInfos.ClassDescription = sDescription;
				tTypeInfos.ClassType = sType;
				tTypeInfos.ServerSynchronize = sServerSynchronize;
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
		public static string GetInfos (Type sType)
		{
			string rReturn = "";
			NWDTypeInfos tTypeInfos = FindTypeInfos (sType);
			if (tTypeInfos == null) {
				rReturn = "unknow";
			} else {
				rReturn = tTypeInfos.Infos ();
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public string Infos ()
		{
			return "ClassName = '" + ClassName + "' " +
			"TrigrammeName = '" + TrigrammeName + "' " +
			"ServerSynchronize = '" + ServerSynchronize + "' " +
			"ClassDescription = '" + ClassDescription + "' " +
			"MenuName = '" + MenuName + "' " +
			"";
		}
		//-------------------------------------------------------------------------------------------------------------
		public static void AllInfos ()
		{
			foreach (KeyValuePair<Type,NWDTypeInfos> tTypeTypeInfos in TypesDictionary) {
				Type tType = tTypeTypeInfos.Key;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================