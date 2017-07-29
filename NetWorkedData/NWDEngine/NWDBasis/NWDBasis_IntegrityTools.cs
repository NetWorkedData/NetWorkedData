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

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	/// <summary>
	/// NWD basis. Integrity tools 
	/// </summary>
	public partial  class NWDBasis <K> where K : NWDBasis <K>, new()
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Updates the integrity. Set the integrity value of object's data in the field Integrity.
		/// </summary>
		public void UpdateIntegrity ()
		{
			Integrity = IntegrityValue ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Tests the integrity.
		/// </summary>
		/// <returns><c>true</c>, if integrity is validated, <c>false</c> if integrity is not validate.</returns>
		public bool TestIntegrity ()
		{
			bool rReturn = false;
			// test integrity
			if (Integrity == IntegrityValue ()) {
				rReturn = true;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Integrity value for this object's data.
		/// </summary>
		/// <returns>The value.</returns>
		public string IntegrityValue ()
		{
			// use salts to interfere with data assembly in the hash sum computing
			return HashSum (PrefSaltA () + DataAssembly () + PrefSaltB ());
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Determines hash sum of specified strToEncrypt.
		/// </summary>
		/// <returns>Integrity value</returns>
		/// <param name="strToEncrypt">String to encrypt.</param>
		public static string HashSum (string strToEncrypt)
		{
			// force to utf-8
			System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding ();
			byte[] bytes = ue.GetBytes (strToEncrypt);
			// encrypt bytes
			System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider ();
			byte[] hashBytes = md5.ComputeHash (bytes);
			// Convert the encrypted bytes back to a string (base 16)
			string hashString = "";
			for (int i = 0; i < hashBytes.Length; i++) 
			{
				hashString += System.Convert.ToString (hashBytes [i], 16).PadLeft (2, '0');
			}
			// remove the DataAssemblySeparator from hash (prevent error in networking transimission)
			hashString = hashString.Replace (NWDConstants.kStandardSeparator, "");
			// return value
			return hashString.PadLeft (24, '0');
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Recalculates all integrities for all objects in ObjectsList.
		/// </summary>
		public static void RecalculateAllIntegrities ()
		{
			//loop
			foreach (NWDBasis<K> tObject in ObjectsList) 
			{
				// update integrity value
				tObject.UpdateIntegrity ();
				// force to write object in database
				tObject.UpdateMe ();
			}
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================