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
			//Init your instance here
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Class methods
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
		//-------------------------------------------------------------------------------------------------------------
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
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Instance methods
		//-------------------------------------------------------------------------------------------------------------
		public void MyInstanceMethod ()
		{
			// do something with this object
		}
		//-------------------------------------------------------------------------------------------------------------
		#region override of NetWorkedData addons methods
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
	#region Connexion NWDPreferences with Unity MonoBehavior
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWDPreferences connexion.
	/// In your MonoBehaviour Script connect object with :
	/// <code>
	///	[NWDConnexionAttribut(true,true, true, true)]
	/// public NWDPreferencesConnexion MyNWDPreferencesObject;
	/// </code>
	/// </summary>
	//-------------------------------------------------------------------------------------------------------------
	// CONNEXION STRUCTURE METHODS
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDPreferencesConnexion
	{
		//-------------------------------------------------------------------------------------------------------------
		[SerializeField]
		public string Reference;
		//-------------------------------------------------------------------------------------------------------------
		public NWDPreferences GetObject ()
		{
			return NWDPreferences.GetObjectWithReference (Reference);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetObject (NWDPreferences sObject)
		{
			Reference = sObject.Reference;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDPreferences NewObject ()
		{
			NWDPreferences tObject = NWDPreferences.NewObject ();
			Reference = tObject.Reference;
			return tObject;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//-------------------------------------------------------------------------------------------------------------
	// CUSTOM PROPERTY DRAWER METHODS
	//-------------------------------------------------------------------------------------------------------------
	#if UNITY_EDITOR
	//-------------------------------------------------------------------------------------------------------------
	[CustomPropertyDrawer (typeof(NWDPreferencesConnexion))]
	public class NWDPreferencesConnexionDrawer : PropertyDrawer
	{
		//-------------------------------------------------------------------------------------------------------------
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			Debug.Log ("GetPropertyHeight");
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true)[0];
			}
			return NWDPreferences.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			Debug.Log ("OnGUI");
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true)[0];
			}
			NWDPreferences.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
		}
		//-------------------------------------------------------------------------------------------------------------
	}
	//-------------------------------------------------------------------------------------------------------------
	#endif
	//-------------------------------------------------------------------------------------------------------------
	#endregion
	//-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================
