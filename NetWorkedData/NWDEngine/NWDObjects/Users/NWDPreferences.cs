using System;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("PRF")]
	[NWDClassDescriptionAttribute ("User Preferences descriptions Class")]
	[NWDClassMenuNameAttribute ("User Preferences")]
	//-------------------------------------------------------------------------------------------------------------
	[NWDPackageClassAttribute]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDPreferences : NWDBasis <NWDPreferences>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		[Indexed ("AccountIndex", 0)]
		/// <summary>
		/// Get or set the account reference.
		/// </summary>
		/// <value>The account reference.</value>
		public NWDReferenceType<NWDAccount> AccountReference { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get or set the value.
		/// </summary>
		/// <value>The value.</value>
		public NWDMultiType Value { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDPreferences ()
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
		public static string GetString (string sKey, string sDefault = "")
		{
			NWDPreferences tObject = NWDBasis<NWDPreferences>.GetObjectByInternalKey (sKey) as NWDPreferences;
			string rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.Value.ToString();
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Set the string for internal key.
		/// </summary>
		/// <param name="sKey">S key.</param>
		/// <param name="sValue">S value.</param>
		public static void SetString (string sKey, string sValue)
		{
			NWDPreferences tObject = NWDBasis<NWDPreferences>.GetObjectByInternalKey (sKey) as NWDPreferences;
			if (tObject == null) {
				tObject = NWDBasis<NWDPreferences>.NewInstance() as NWDPreferences;
				NWDReferenceType<NWDAccount> tAccountValue = new NWDReferenceType<NWDAccount> ();
				tAccountValue.SetReference (NWDAppConfiguration.SharedInstance.SelectedEnvironment ().PlayerAccountReference);
				tObject.AccountReference = tAccountValue;
			}
			tObject.Value.SetString (sValue);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get the int value for internal key.
		/// </summary>
		/// <returns>The int.</returns>
		/// <param name="sKey">key.</param>
		/// <param name="sDefault">default value.</param>
		public static int GetInt (string sKey, int sDefault = 0)
		{
			NWDPreferences tObject = NWDBasis<NWDPreferences>.GetObjectByInternalKey (sKey) as NWDPreferences;
			int rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.Value.ToInt();
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Set the int for internal key.
		/// </summary>
		/// <param name="sKey">S key.</param>
		/// <param name="sValue">S value.</param>
		public static void SetInt (string sKey, int sValue)
		{
			NWDPreferences tObject = NWDBasis<NWDPreferences>.GetObjectByInternalKey (sKey) as NWDPreferences;
			if (tObject == null) {
				tObject = NWDBasis<NWDPreferences>.NewInstance() as NWDPreferences;
				NWDReferenceType<NWDAccount> tAccountValue = new NWDReferenceType<NWDAccount> ();
				tAccountValue.SetReference (NWDAppConfiguration.SharedInstance.SelectedEnvironment ().PlayerAccountReference);
				tObject.AccountReference = tAccountValue;
			}
			tObject.Value.SetInt (sValue);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get the bool value for internal key.
		/// </summary>
		/// <returns><c>true</c>, if bool was gotten, <c>false</c> otherwise.</returns>
		/// <param name="sKey">key.</param>
		/// <param name="sDefault">If set to <c>true</c> default value.</param>
		public static bool GetBool (string sKey, bool sDefault = false)
		{
			NWDPreferences tObject = NWDBasis<NWDPreferences>.GetObjectByInternalKey (sKey) as NWDPreferences;
			bool rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.Value.ToBool();
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Set the bool value for internal key.
		/// </summary>
		/// <param name="sKey">S key.</param>
		/// <param name="sValue">If set to <c>true</c> s value.</param>
		public static void SetBool (string sKey, bool sValue)
		{
			NWDPreferences tObject = NWDBasis<NWDPreferences>.GetObjectByInternalKey (sKey) as NWDPreferences;
			if (tObject == null) {
				tObject = NWDBasis<NWDPreferences>.NewInstance() as NWDPreferences;
				NWDReferenceType<NWDAccount> tAccountValue = new NWDReferenceType<NWDAccount> ();
				tAccountValue.SetReference (NWDAppConfiguration.SharedInstance.SelectedEnvironment ().PlayerAccountReference);
				tObject.AccountReference = tAccountValue;
			}
			tObject.Value.SetBool (sValue);
		}
		//------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get the float value for internal key.
		/// </summary>
		/// <returns>The float.</returns>
		/// <param name="sKey">key.</param>
		/// <param name="sDefault">default value.</param>
		public static float GetFloat (string sKey, float sDefault = 0.0F)
		{
			NWDPreferences tObject = NWDBasis<NWDPreferences>.GetObjectByInternalKey (sKey) as NWDPreferences;
			float rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.Value.ToFloat();
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Set the float value for internal key.
		/// </summary>
		/// <param name="sKey">S key.</param>
		/// <param name="sValue">S value.</param>
		public static void SetFloat (string sKey, float sValue)
		{
			NWDPreferences tObject = NWDBasis<NWDPreferences>.GetObjectByInternalKey (sKey) as NWDPreferences;
			if (tObject == null) {
				tObject = NWDBasis<NWDPreferences>.NewInstance() as NWDPreferences;
				NWDReferenceType<NWDAccount> tAccountValue = new NWDReferenceType<NWDAccount> ();
				tAccountValue.SetReference (NWDAppConfiguration.SharedInstance.SelectedEnvironment ().PlayerAccountReference);
				tObject.AccountReference = tAccountValue;
			}
			tObject.Value.SetFloat (sValue);
		}
		//--------------------------------------------------------------------------------------------------------------

	}
}
//=====================================================================================================================
