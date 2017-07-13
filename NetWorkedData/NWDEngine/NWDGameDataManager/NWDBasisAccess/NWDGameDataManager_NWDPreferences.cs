using UnityEngine;

using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
	public partial class NWDGameDataManager : MonoBehaviour
	{
		//-------------------------------------------------------------------------------------------------------------
		// PREFERENCES FOR PLAYER
		//-------------------------------------------------------------------------------------------------------------
		public NWDPreferences PreferenceWithInternalKey (string sInternalKey)
		{
			NWDPreferences rPreference = NWDPreferences.GetObjectWithInternalKey (sInternalKey);
			if (rPreference == null) {
				rPreference = NWDPreferences.NewObject ();
				rPreference.InternalKey = sInternalKey;
				rPreference.SaveModifications ();
			}
			return rPreference;
		}
		//-------------------------------------------------------------------------------------------------------------
		public float GetFloatPreferenceWithInternalKey (string sInternalKey, float sDefaultValue)
		{
			NWDPreferences rPreference = NWDPreferences.GetObjectWithInternalKey (sInternalKey);
			if (rPreference == null) {
				rPreference = NWDPreferences.NewObject ();
				rPreference.InternalKey = sInternalKey;
				rPreference.Value = sDefaultValue.ToString ();
				rPreference.SaveModifications ();
			}
			float tReturn = 0.0f;
			float.TryParse (rPreference.Value, out tReturn);
			return tReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetFloatPreferenceWithInternalKey (string sInternalKey, float sFloatValue)
		{
			NWDPreferences rPreference = NWDPreferences.GetObjectWithInternalKey (sInternalKey);
			if (rPreference == null) {
				rPreference = NWDPreferences.NewObject ();
				rPreference.InternalKey = sInternalKey;
				rPreference.SaveModifications ();
			}
			rPreference.Value = sFloatValue.ToString ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public int GetIntPreferenceWithInternalKey (string sInternalKey, int sDefaultValue)
		{
			NWDPreferences rPreference = NWDPreferences.GetObjectWithInternalKey (sInternalKey);
			if (rPreference == null) {
				rPreference = NWDPreferences.NewObject ();
				rPreference.InternalKey = sInternalKey;
				rPreference.Value = sDefaultValue.ToString ();
				rPreference.SaveModifications ();
			}
			int tReturn = 0;
			int.TryParse (rPreference.Value, out tReturn);
			return tReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetIntPreferenceWithInternalKey (string sInternalKey, int sIntValue)
		{
			NWDPreferences rPreference = NWDPreferences.GetObjectWithInternalKey (sInternalKey);
			if (rPreference == null) {
				rPreference = NWDPreferences.NewObject ();
				rPreference.InternalKey = sInternalKey;
				rPreference.SaveModifications ();
			}
			rPreference.Value = sIntValue.ToString ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool GetBoolPreferenceWithInternalKey (string sInternalKey, bool sDefaultValue)
		{
			NWDPreferences rPreference = NWDPreferences.GetObjectWithInternalKey (sInternalKey);
			if (rPreference == null) {
				rPreference = NWDPreferences.NewObject ();
				rPreference.InternalKey = sInternalKey;
				rPreference.Value = sDefaultValue.ToString ();
				rPreference.SaveModifications ();
			}
			bool tReturn = false;
			bool.TryParse (rPreference.Value, out tReturn);
			return tReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetBoolPreferenceWithInternalKey (string sInternalKey, bool sBoolValue)
		{
			NWDPreferences rPreference = NWDPreferences.GetObjectWithInternalKey (sInternalKey);
			if (rPreference == null) {
				rPreference = NWDPreferences.NewObject ();
				rPreference.InternalKey = sInternalKey;
				rPreference.SaveModifications ();
			}
			rPreference.Value = sBoolValue.ToString ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public string GetStringPreferenceWithInternalKey (string sInternalKey, string sDefaultValue)
		{
			NWDPreferences rPreference = NWDPreferences.GetObjectWithInternalKey (sInternalKey);
			if (rPreference == null) {
				rPreference = NWDPreferences.NewObject ();
				rPreference.InternalKey = sInternalKey;
				rPreference.Value = sDefaultValue;
				rPreference.SaveModifications ();
			}
			return rPreference.Value;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetDataTypePreferenceWithInternalKey (string sInternalKey, BTBDataType sDataValue)
		{
			NWDPreferences rPreference = NWDPreferences.GetObjectWithInternalKey (sInternalKey);
			if (rPreference == null) {
				rPreference = NWDPreferences.NewObject ();
				rPreference.InternalKey = sInternalKey;
				rPreference.SaveModifications ();
			}
			rPreference.Value = sDataValue.ToString ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public BTBDataType GetDatatypePreferenceWithInternalKey (string sInternalKey, BTBDataType sDefaultValue)
		{
			NWDPreferences rPreference = NWDPreferences.GetObjectWithInternalKey (sInternalKey);
			if (rPreference == null) {
				rPreference = NWDPreferences.NewObject ();
				rPreference.InternalKey = sInternalKey;
				rPreference.Value = sDefaultValue.ToString ();
				rPreference.SaveModifications ();
			}
			return new BTBDataType (rPreference.Value);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetStringPreferenceWithInternalKey (string sInternalKey, string sStringValue)
		{
			NWDPreferences rPreference = NWDPreferences.GetObjectWithInternalKey (sInternalKey);
			if (rPreference == null) {
				rPreference = NWDPreferences.NewObject ();
				rPreference.InternalKey = sInternalKey;
				rPreference.SaveModifications ();
			}
			rPreference.Value = sStringValue;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
