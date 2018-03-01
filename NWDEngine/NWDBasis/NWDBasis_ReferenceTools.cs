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
    public partial class NWDBasis<K> : NWDTypeClass where K : NWDBasis<K>, new()
	{

		//-------------------------------------------------------------------------------------------------------------

		#region Class Methods

		//-------------------------------------------------------------------------------------------------------------


		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Changes the reference for another in all objects.
		/// </summary>
		/// <param name="sOldReference">S old reference.</param>
		/// <param name="sNewReference">S new reference.</param>
		/// <param name="sType">S type.</param>
		public static void ChangeReferenceForAnotherInAllObjects (string sOldReference, string sNewReference)
		{
			//Debug.LogVerbose ("I WILL CHANGE "+sOldReference+" FOR "+sNewReference+" in objects of class " + ClassName ());
			foreach (NWDBasis<K> tObject in NWDBasis<K>.ObjectsList) {
				tObject.ChangeReferenceForAnother (sOldReference, sNewReference);
			}
		}


		//-------------------------------------------------------------------------------------------------------------
		public static void TryToChangeUserForAllObjects (string sOldUser, string sNewUser)
		{
			foreach (NWDBasis<K> tObject in NWDBasis<K> .ObjectsList)
			{
				tObject.ChangeUser(sOldUser, sNewUser);     
			}
		}
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Instance Methods

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// UUID transform for reference.
		/// </summary>
		/// <returns>The transform for reference.</returns>
		/// <param name="sUUID">UUID.</param>
		public string UUIDTransformForReference (string sUUID)
		{
			string tUUID = "-" + sUUID;
			tUUID = tUUID.Replace ("ACC", "");
			tUUID = tUUID.Replace ("S", "");
			//			tUUID = tUUID.Replace ("T", ""); // Je ne remplace pas le T de l'accompte ... ainsi je verrai les References crée sur un compte temporaire non vérifié
			tUUID = tUUID.Replace ("-", "");
			return tUUID;
		}

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// New reference from UUID.
		/// </summary>
		/// <returns>The reference from UUID.</returns>
		/// <param name="sUUID">UUID.</param>
		public string NewReferenceFromUUID (string sUUID)
		{
			string rReturn = "";
			bool tValid = false;
			if (sUUID != null && sUUID != "") {
				sUUID = UUIDTransformForReference (sUUID) + "-";
			}
			int tTime = NWDToolbox.Timestamp () - 1492711200; // je compte depuis le 20 avril 2017 à 20h00
			while (tValid == false) {
				rReturn = ClassTrigramme () + "-" + sUUID + tTime.ToString () + "-" + UnityEngine.Random.Range (100000, 999999).ToString ();
				tValid = TestReference (rReturn);
			}
			return rReturn;
		}

		//-------------------------------------------------------------------------------------------------------------
//		/// <summary>
//		/// Updates the reference without change repercsusion security.
//		/// </summary>
//		/// <param name="sOldUser">S old user.</param>
//		/// <param name="sNewUser">S new user.</param>
//		public void UpdateReference (string sOldUser, string sNewUser)
//		{
//			string tReference = Reference.Replace (UUIDTransformForReference (sOldUser), UUIDTransformForReference (sNewUser));
//			bool tValid = TestReference (tReference);
//			while (tValid == false) {
//				tReference = tReference + UnityEngine.Random.Range (100, 999);
//				tValid = TestReference (tReference);
//			}
//		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Test the reference allready exists.
		/// </summary>
		/// <returns><c>true</c>, if reference was tested, <c>false</c> otherwise.</returns>
		/// <param name="sReference">reference.</param>
		public bool TestReference (string sReference)
		{

			SQLiteConnection tSQLiteConnection = NWDDataManager.SharedInstance.SQLiteConnectionEditor;
			if (AccountDependent ())
			{
				tSQLiteConnection = NWDDataManager.SharedInstance.SQLiteConnectionAccount;
			}

			bool rValid = false;
			IEnumerable<K> tEnumerable = tSQLiteConnection.Table<K> ().Where (x => x.Reference == sReference);
			int tCount = tEnumerable.Cast<K> ().Count<K> ();
			if (tCount == 0) {
				rValid = true;
			}
			return rValid;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// New reference. If account dependent this UUID of Player Account is integrate in Reference generation
		/// </summary>
		/// <returns>The reference.</returns>
		public string NewReference ()
		{
			if (AccountDependent () == true) {
				return NewReferenceFromUUID (NWDAppConfiguration.SharedInstance().SelectedEnvironment ().PlayerAccountReference);
			} else {
				return NewReferenceFromUUID ("");
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Regenerates the reference.
		/// </summary>
		public void RegenerateNewReference ()
		{
			#if UNITY_EDITOR
			string tOldReference = Reference;
			string tNewReference = NewReference ();
			foreach (Type tType in NWDDataManager.SharedInstance.mTypeList) {
				var tMethodInfo = tType.GetMethod ("ChangeReferenceForAnotherInAllObjects", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
				if (tMethodInfo != null) {
					tMethodInfo.Invoke (null, new object[]{ tOldReference, tNewReference});
				}
			}
			Reference = tNewReference;
			UpdateMe (true);
			UpdateObjectInListOfEdition (this);
			NWDDataManager.SharedInstance.RepaintWindowsInManager (this.GetType ());
			#endif
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Changes the reference for another.
		/// </summary>
		/// <param name="sOldReference">old reference.</param>
		/// <param name="sNewReference">new reference.</param>
		/// <param name="sType">type.</param>
		public void ChangeReferenceForAnother (string sOldReference, string sNewReference)
		{
			bool rModify = false;
			Type tType = ClassType ();
			foreach (var tProp in tType.GetProperties(BindingFlags.Public | BindingFlags.Instance)) {
				Type tTypeOfThis = tProp.PropertyType;
				if (tTypeOfThis != null) {
					if (tTypeOfThis.IsGenericType) {
						if (
							tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferenceHashType<>) ||
							tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferenceType<>) ||
//							tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDConnectionType<>) ||
							tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferencesListType<>) ||
							tTypeOfThis.GetGenericTypeDefinition () == typeof(NWDReferencesQuantityType<>)) {

//							Debug.LogVerbose ("I WILL CHANGE "+sOldReference+" FOR "+sNewReference+" in Property " + tProp.Name);
							var tMethodInfo = tTypeOfThis.GetMethod ("ChangeReferenceForAnother", BindingFlags.Public | BindingFlags.Instance);
							if (tMethodInfo != null) {
								var tNext = tProp.GetValue (this, null);
								if (tNext == null) {
									tNext = Activator.CreateInstance (tTypeOfThis);
								}
//								Debug.LogVerbose ("tNext preview = " + tNext);
								string tChanged = tMethodInfo.Invoke (tNext, new object[]{ sOldReference, sNewReference}) as string;
								if (tChanged == "YES") {
//									Debug.LogVerbose ("tNext changed = " + tNext);
									tProp.SetValue (this, tNext, null);
									rModify = true;
								}
							}
						}
					}
				}
			}
			if (rModify == true) {
//				Debug.LogVerbose ("I WAS UPDATED");
				UpdateMe ();
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void ChangeUser(string sOldUser, string sNewUser)
		{
			if (AccountDependent () == true)
            {
				foreach (PropertyInfo tProp in PropertiesAccountConnect())
                {
					NWDReferenceType<NWDAccount> tObject = tProp.GetValue (this, null) as NWDReferenceType<NWDAccount>;
                    if (tObject != null)
                    {
                        tObject.ChangeReferenceForAnother(sOldUser, sNewUser);
                    }
				}
				UpdateMeIfModified ();
			}
		}
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================