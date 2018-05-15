//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDPreferencesConnection : NWDConnection <NWDPreferences> {}
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("PRF")]
	[NWDClassDescriptionAttribute ("User Preferences descriptions Class")]
	[NWDClassMenuNameAttribute ("User Preferences")]
	//-------------------------------------------------------------------------------------------------------------
//	[NWDTypeClassInPackageAttribute]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDPreferences : NWDBasis <NWDPreferences>
	{
		//-------------------------------------------------------------------------------------------------------------
		//#warning YOU MUST FOLLOW THIS INSTRUCTIONS
		//-------------------------------------------------------------------------------------------------------------
		// YOU MUST GENERATE PHP FOR THIS CLASS AFTER FIELD THIS CLASS WITH YOUR PROPERTIES
		// YOU MUST GENERATE WEBSITE AND UPLOAD THE FOLDER ON YOUR SERVER
		// YOU MUST UPDATE TABLE ON THE SERVER WITH THE MENU FOR DEV, FOR PREPROD AND FOR PROD
		//-------------------------------------------------------------------------------------------------------------
		#region Properties
		//-------------------------------------------------------------------------------------------------------------
		// Your properties
		/// <summary>
		/// Get or set the account reference.
		/// </summary>
		/// <value>The account reference.</value>
		[Indexed ("AccountIndex", 0)]
		public NWDReferenceType<NWDAccount> AccountReference { get; set; }
		/// <summary>
		/// Get or set the value.
		/// </summary>
		/// <value>The value.</value>
		public NWDMultiType Value { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Constructors
		//-------------------------------------------------------------------------------------------------------------
		public NWDPreferences()
        {
            //Debug.Log("NWDPreferences Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDPreferences(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDPreferences Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Class methods
		//-------------------------------------------------------------------------------------------------------------
		public static NWDPreferences GetPreferenceByInternalKeyOrCreate (string sInternalKey, string sDefaultValue, string sInternalDescription = "")
		{
			//Debug.Log ("GetPreferenceByInternalKeyOrCreate");
			NWDPreferences rObject = GetObjectByInternalKey (sInternalKey) as NWDPreferences;
			if (rObject == null) {
				//Debug.Log ("New object");
				rObject = NWDBasis<NWDPreferences>.NewObject ();
				RemoveObjectInListOfEdition (rObject);
				rObject.InternalKey = sInternalKey;
				NWDReferenceType<NWDAccount> tAccountReference = new NWDReferenceType<NWDAccount>();
				tAccountReference.SetReference (NWDAppConfiguration.SharedInstance().SelectedEnvironment ().PlayerAccountReference);
				rObject.AccountReference = tAccountReference;
                NWDMultiType tValue = new NWDMultiType (sDefaultValue);
				rObject.Value = tValue;
				rObject.InternalDescription = sInternalDescription;
				rObject.UpdateMe ();
				AddObjectInListOfEdition (rObject);
			}
			return rObject;
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
			NWDPreferences tObject = GetPreferenceByInternalKeyOrCreate (sKey, sDefault);
			return tObject.Value.ToString();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Set the string for internal key.
		/// </summary>
		/// <param name="sKey">S key.</param>
		/// <param name="sValue">S value.</param>
		public static void SetString (string sKey, string sValue)
		{
			NWDPreferences tObject = GetPreferenceByInternalKeyOrCreate (sKey, sValue);
			tObject.Value.SetString (sValue);
			tObject.SaveModifications ();
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
			NWDPreferences tObject = GetPreferenceByInternalKeyOrCreate (sKey, sDefault.ToString ());
			return tObject.Value.ToInt();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Set the int for internal key.
		/// </summary>
		/// <param name="sKey">S key.</param>
		/// <param name="sValue">S value.</param>
		public static void SetInt (string sKey, int sValue)
		{
			NWDPreferences tObject = GetPreferenceByInternalKeyOrCreate (sKey, sValue.ToString ());
			tObject.Value.SetInt (sValue);
			tObject.SaveModifications ();
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
			NWDPreferences tObject = GetPreferenceByInternalKeyOrCreate (sKey, sDefault.ToString ());
			return tObject.Value.ToBool();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Set the bool value for internal key.
		/// </summary>
		/// <param name="sKey">S key.</param>
		/// <param name="sValue">If set to <c>true</c> s value.</param>
		public static void SetBool (string sKey, bool sValue)
		{
			NWDPreferences tObject = GetPreferenceByInternalKeyOrCreate (sKey, sValue.ToString ());
			tObject.Value.SetBool (sValue);
			tObject.SaveModifications ();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get the float value for internal key.
		/// </summary>
		/// <returns>The float.</returns>
		/// <param name="sKey">key.</param>
		/// <param name="sDefault">default value.</param>
		public static float GetFloat (string sKey, float sDefault = 0.0F)
		{
			NWDPreferences tObject = GetPreferenceByInternalKeyOrCreate (sKey, sDefault.ToString ());
			return tObject.Value.ToFloat();
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Set the float value for internal key.
		/// </summary>
		/// <param name="sKey">S key.</param>
		/// <param name="sValue">S value.</param>
		public static void SetFloat (string sKey, float sValue)
		{
			NWDPreferences tObject = GetPreferenceByInternalKeyOrCreate (sKey, sValue.ToString ());
			tObject.Value.SetFloat (sValue);
			tObject.SaveModifications ();
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
		//-------------------------------------------------------------------------------------------------------------
		public void MyInstanceMethod ()
		{
			// do something with this object
		}
		//-------------------------------------------------------------------------------------------------------------
		#region NetWorkedData addons methods
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonInsertMe ()
		{
			// do something when object will be inserted
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonUpdateMe ()
		{
			// do something when object will be updated
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonUpdatedMe ()
		{
			// do something when object finish to be updated
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonDuplicateMe ()
		{
			// do something when object will be dupplicate
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonEnableMe ()
		{
			// do something when object will be enabled
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonDisableMe ()
		{
			// do something when object will be disabled
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonTrashMe ()
		{
			// do something when object will be put in trash
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void AddonUnTrashMe ()
		{
			// do something when object will be remove from trash
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		//Addons for Edition
		//-------------------------------------------------------------------------------------------------------------
		public override bool AddonEdited( bool sNeedBeUpdate)
		{
			if (sNeedBeUpdate == true) 
			{
				// do something
			}
			return sNeedBeUpdate;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override float AddonEditor (Rect sInRect)
		{
			// Draw the interface addon for editor
			float tYadd = 0.0f;
			return tYadd;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override float AddonEditorHeight ()
		{
			// Height calculate for the interface addon for editor
			float tYadd = 0.0f;
			return tYadd;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
	}
	//-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================
