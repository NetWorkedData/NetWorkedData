//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("LCL")]
	[NWDClassDescriptionAttribute ("Localization of game descriptions Class")]
	[NWDClassMenuNameAttribute ("Localization")]
	//-------------------------------------------------------------------------------------------------------------
	[NWDPackageClassAttribute]
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWD game configuration.
	/// </summary>
	public partial class NWDLocalization : NWDBasis <NWDLocalization>
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the value string.
		/// </summary>
		/// <value>The value string.</value>
		public NWDLocalizableTextType TextValue { get; set; }
		/// <summary>
		/// Gets or sets the annexe value.
		/// </summary>
		/// <value>The annexe value.</value>
		public NWDMultiType AnnexeValue { get; set;}
		//-------------------------------------------------------------------------------------------------------------
		public NWDLocalization ()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get the local string for internal key.
		/// </summary>
		/// <returns>The local string.</returns>
		/// <param name="sKey">key.</param>
		/// <param name="sDefault">default value.</param>
		public static string GetLocalText (string sKey, string sDefault="")
		{
			NWDLocalization tObject = NWDBasis<NWDLocalization>.GetObjectByInternalKey (sKey) as NWDLocalization;
			string rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.TextValue.GetLocalString ();
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDMultiType GetAnnexeValue (string sKey)
		{
			NWDLocalization tObject = NWDBasis<NWDLocalization>.GetObjectByInternalKey (sKey) as NWDLocalization;
			NWDMultiType rReturn = new NWDMultiType();
			if (tObject != null) {
				rReturn = tObject.AnnexeValue;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static string GetAnnexeString (string sKey, string sDefault = "")
		{
			NWDLocalization tObject = NWDBasis<NWDLocalization>.GetObjectByInternalKey (sKey) as NWDLocalization;
			string rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.AnnexeValue.ToString();
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static bool GetAnnexeBool (string sKey, bool sDefault = false)
		{
			NWDLocalization tObject = NWDBasis<NWDLocalization>.GetObjectByInternalKey (sKey) as NWDLocalization;
			bool rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.AnnexeValue.ToBool();
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static float GetAnnexeFloat (string sKey, float sDefault = 0.0f)
		{
			NWDLocalization tObject = NWDBasis<NWDLocalization>.GetObjectByInternalKey (sKey) as NWDLocalization;
			float rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.AnnexeValue.ToFloat();
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static float GetAnnexeInt (string sKey, int sDefault = 0)
		{
			NWDLocalization tObject = NWDBasis<NWDLocalization>.GetObjectByInternalKey (sKey) as NWDLocalization;
			int rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.AnnexeValue.ToInt();
			}
			return rReturn;
		}
		//--------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================