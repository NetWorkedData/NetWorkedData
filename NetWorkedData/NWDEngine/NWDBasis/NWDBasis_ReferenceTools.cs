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
using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial  class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		public string UUIDTransformForReference (string sUUID)
		{
			string tUUID = "-" + sUUID;
			tUUID = tUUID.Replace ("ACC", "");
			tUUID = tUUID.Replace ("S", "");
			//			tUUID = tUUID.Replace ("T", ""); // Je ne remplace pas le T de l'accompte ... ainsi je verrai les References crée sur un compte temporaire non vérifié
			tUUID = tUUID.Replace ("-", "");
			return tUUID;
		}

		public string NewReferenceFromUUID (string sUUID)
		{
			string rReturn = "";
			bool tValid = false;
			if (sUUID != null && sUUID != "") 
			{
				sUUID = UUIDTransformForReference (sUUID) + "-";
			}
			int tTime =  NWDToolbox.Timestamp ()-1492711200; // je compte depuis le 20 avril 2017 à 20h00
			while (tValid == false) {
				rReturn = ClassTrigramme () + "-" + sUUID  + tTime.ToString () + "-" + UnityEngine.Random.Range (100000, 999999).ToString ();
				tValid = TestReference (rReturn);
			}
			return rReturn;
		}

		public void UpdateReference (string sOldUser, string sNewUser)
		{
			string tReference = Reference.Replace (UUIDTransformForReference(sOldUser),UUIDTransformForReference(sNewUser));
			bool tValid = TestReference (tReference);
			while (tValid == false) {
				tReference = tReference + UnityEngine.Random.Range (100, 999);
				tValid = TestReference (tReference);
			}
		}


		public bool TestReference (string sReference)
		{
			bool rValid = false;
			IEnumerable<K> tEnumerable = NWDDataManager.SharedInstance.SQLiteConnection.Table<K> ().Where (x => x.Reference == sReference);
			int tCount = tEnumerable.Cast<K> ().Count<K> ();
			if (tCount == 0) {
				rValid = true;
			}
			return rValid;
		}
		public virtual string NewReference ()
		{
//			string tUUID = NWDDataManager.SharedInstance.PlayerAccountUUID;
			if (IsAccountDependent () == true) {
				return NewReferenceFromUUID (NWDAppConfiguration.SharedInstance.SelectedEnvironment ().PlayerAccountReference);
			} else {
				return NewReferenceFromUUID ("");
			}
		}

		public void RegenerateNewReference ()
		{
			string tOldReference = Reference;
			string tNewReference = NewReference ();
			foreach (Type tType in NWDDataManager.SharedInstance.mTypeList)
			{
				var tMethodInfo = tType.GetMethod ("ChangeReferenceForAnotherInAllObjects", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
					if (tMethodInfo != null) {
					tMethodInfo.Invoke (null, new object[]{tOldReference,tNewReference,GetType ()});
					}
			}
			RemoveObjectInListOfEdition (this);
			Reference = tNewReference;
			UpdateMe (true);
			AddObjectInListOfEdition (this);
		}

		public static void ChangeReferenceForAnotherInAllObjects(string sOldReference, string sNewReference, Type sType)
		{
			//BTBDebug.LogVerbose ("I WILL CHANGE "+sOldReference+" FOR "+sNewReference+" in objects of class " + ClassName ());
			foreach (NWDBasis<K> tObject in NWDBasis<K>.ObjectsList)
			{
				tObject.ChangeReferenceForAnother( sOldReference, sNewReference, sType);
			}
		}

		public void ChangeReferenceForAnother(string sOldReference, string sNewReference, Type sType)
		{
			bool rModify = false;
			Type tType = ClassType ();
			foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
			{
				Type tTypeOfThis = tProp.PropertyType;
				if (tTypeOfThis != null) 
				{
					if (tTypeOfThis.IsGenericType)
					{
						if (
							tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferenceHashType<>) ||
							tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferenceType<>) ||
//							tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDConnexionType<>) ||
							tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferencesListType<>) ||
							tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferencesQuantityType<>)
						)
						{

//							BTBDebug.LogVerbose ("I WILL CHANGE "+sOldReference+" FOR "+sNewReference+" in Property " + tProp.Name);
							var tMethodInfo = tTypeOfThis.GetMethod ("ChangeReferenceForAnother", BindingFlags.Public | BindingFlags.Instance);
							if (tMethodInfo != null) {
								var tNext = tProp.GetValue (this, null);
								if (tNext == null) {
									tNext = Activator.CreateInstance (tTypeOfThis);
								}
//								BTBDebug.LogVerbose ("tNext preview = " + tNext);
								string tChanged = tMethodInfo.Invoke (tNext, new object[]{sOldReference,sNewReference, sType}) as string;
								if (tChanged == "YES") 
								{
//									BTBDebug.LogVerbose ("tNext changed = " + tNext);
										tProp.SetValue (this, tNext, null);
										rModify = true;
								}
							}
						}
					}
				}
			}
			if (rModify == true)
			{
//				BTBDebug.LogVerbose ("I WAS UPDATED");
				UpdateMe();
			}
		}
	}
}
//=====================================================================================================================