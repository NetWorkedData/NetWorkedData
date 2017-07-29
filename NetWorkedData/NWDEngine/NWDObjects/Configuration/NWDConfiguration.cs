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
	[NWDClassTrigrammeAttribute ("CNF")]
	[NWDClassDescriptionAttribute ("Configurations of game descriptions Class")]
	[NWDClassMenuNameAttribute ("Configurations")]
	//-------------------------------------------------------------------------------------------------------------
	[NWDPackageClassAttribute]
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWD game configuration.
	/// </summary>
	public partial class NWDConfiguration : NWDBasis <NWDConfiguration>
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Gets or sets the value string.
		/// </summary>
		/// <value>The value string.</value>
		public NWDLocalizableStringType ValueString { get; set; }
		/// <summary>
		/// Gets or sets the value int.
		/// </summary>
		/// <value>The value int.</value>
		public int ValueInt { get; set; }
		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="NetWorkedData.NWDConfiguration"/> value bool.
		/// </summary>
		/// <value><c>true</c> if value bool; otherwise, <c>false</c>.</value>
		public bool ValueBool { get; set; }
		/// <summary>
		/// Gets or sets the value float.
		/// </summary>
		/// <value>The value float.</value>
		public float ValueFloat { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDConfiguration ()
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
		public static string GetLocalString (string sKey, string sDefault="")
		{
			NWDConfiguration tObject = NWDBasis<NWDConfiguration>.GetObjectByInternalKey (sKey) as NWDConfiguration;
			string rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.ValueString.GetLocalString ();
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get the int value for internal key.
		/// </summary>
		/// <returns>The int.</returns>
		/// <param name="sKey">key.</param>
		/// <param name="sDefault">default value.</param>
		public static int GetInt (string sKey, int sDefault=0)
		{
			NWDConfiguration tObject = NWDBasis<NWDConfiguration>.GetObjectByInternalKey (sKey) as NWDConfiguration;
			int rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.ValueInt;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get the bool value for internal key.
		/// </summary>
		/// <returns><c>true</c>, if bool was gotten, <c>false</c> otherwise.</returns>
		/// <param name="sKey">key.</param>
		/// <param name="sDefault">If set to <c>true</c> default value.</param>
		public static bool GetBool (string sKey, bool sDefault=false)
		{
			NWDConfiguration tObject = NWDBasis<NWDConfiguration>.GetObjectByInternalKey (sKey) as NWDConfiguration;
			bool rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.ValueBool;
			}
			return rReturn;
		}
		//------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get the float value for internal key.
		/// </summary>
		/// <returns>The float.</returns>
		/// <param name="sKey">key.</param>
		/// <param name="sDefault">default value.</param>
		public static float GetFloat (string sKey, float sDefault=0.0F)
		{
			NWDConfiguration tObject = NWDBasis<NWDConfiguration>.GetObjectByInternalKey (sKey) as NWDConfiguration;
			float rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.ValueFloat;
			}
			return rReturn;
		}
		//--------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================