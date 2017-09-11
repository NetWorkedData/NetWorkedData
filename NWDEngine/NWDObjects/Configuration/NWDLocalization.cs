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
	[NWDClassTrigrammeAttribute ("LCL")]
	[NWDClassDescriptionAttribute ("Localization of game descriptions Class")]
	[NWDClassMenuNameAttribute ("Localization")]
	//-------------------------------------------------------------------------------------------------------------
//	[NWDTypeClassInPackageAttribute]
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWD game configuration.
	/// </summary>
	public partial class NWDLocalization : NWDBasis <NWDLocalization>
	{
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
		/// Gets or sets the value string.
		/// </summary>
		/// <value>The value string.</value>
		public NWDLocalizableTextType TextValue { get; set; }

		/// <summary>
		/// Gets or sets the annexe value.
		/// </summary>
		/// <value>The annexe value.</value>
		public NWDMultiType AnnexeValue { get; set; }
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Constructors

		//-------------------------------------------------------------------------------------------------------------
		public NWDLocalization ()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

		#region Class methods

		//-------------------------------------------------------------------------------------------------------------
		public static NWDLocalization CreateLocalizationTextValue (string sKey, string sDefault)
		{
			NWDLocalization rReturn = NWDBasis<NWDLocalization>.NewObject ();
			rReturn.InternalKey = sKey;
			if (sDefault != "") {
				rReturn.TextValue.AddBaseString (sKey);
				//rReturn.TextValue.AddLocalString (sKey);
			} else {
				rReturn.TextValue.AddBaseString (sDefault);
				//rReturn.TextValue.AddLocalString (sDefault);
			}
			rReturn.SaveModifications ();
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDLocalization CreateLocalizationAnnexe (string sKey, string sDefault)
		{
			NWDLocalization rReturn = NWDBasis<NWDLocalization>.NewObject ();
			rReturn.InternalKey = sKey;
			//rReturn.TextValue.AddBaseString (sKey);
			rReturn.AnnexeValue = new NWDMultiType (sDefault);
			rReturn.SaveModifications ();
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get the local string for internal key.
		/// </summary>
		/// <returns>The local string.</returns>
		/// <param name="sKey">key.</param>
		/// <param name="sDefault">default value.</param>
		public static string GetLocalText (string sKey, string sDefault = "")
		{
			NWDLocalization tObject = NWDBasis<NWDLocalization>.GetObjectByInternalKey (sKey) as NWDLocalization;
			string rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.TextValue.GetLocalString ();
			} else {
				CreateLocalizationTextValue (sKey, sDefault);
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDMultiType GetAnnexeValue (string sKey, string sDefault = "")
		{
			NWDLocalization tObject = NWDBasis<NWDLocalization>.GetObjectByInternalKey (sKey) as NWDLocalization;
			NWDMultiType rReturn = new NWDMultiType ();
			if (tObject != null) {
				rReturn = tObject.AnnexeValue;
			} else {
				CreateLocalizationAnnexe (sKey,sDefault);
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static string GetAnnexeString (string sKey, string sDefault = "")
		{
			NWDLocalization tObject = NWDBasis<NWDLocalization>.GetObjectByInternalKey (sKey) as NWDLocalization;
			string rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.AnnexeValue.ToString ();
			} else {
				CreateLocalizationAnnexe (sKey,sDefault);
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static bool GetAnnexeBool (string sKey, bool sDefault = false)
		{
			NWDLocalization tObject = NWDBasis<NWDLocalization>.GetObjectByInternalKey (sKey) as NWDLocalization;
			bool rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.AnnexeValue.ToBool ();
			} else {
				CreateLocalizationAnnexe (sKey,sDefault.ToString ());
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static float GetAnnexeFloat (string sKey, float sDefault = 0.0f)
		{
			NWDLocalization tObject = NWDBasis<NWDLocalization>.GetObjectByInternalKey (sKey) as NWDLocalization;
			float rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.AnnexeValue.ToFloat ();
			} else {
				CreateLocalizationAnnexe (sKey,sDefault.ToString ());
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static float GetAnnexeInt (string sKey, int sDefault = 0)
		{
			NWDLocalization tObject = NWDBasis<NWDLocalization>.GetObjectByInternalKey (sKey) as NWDLocalization;
			int rReturn = sDefault;
			if (tObject != null) {
				rReturn = tObject.AnnexeValue.ToInt ();
			} else {
				CreateLocalizationAnnexe (sKey,sDefault.ToString ());
			}
			return rReturn;
		}
        //-------------------------------------------------------------------------------------------------------------
        public static void AutoLocalize(UnityEngine.UI.Text sText, string sDefault = "")
        {
            if (sText.gameObject != null)
            {
                if (sDefault.Equals(""))
                {
                    sDefault = sText.text;
                }

                NWDLocalization tObject = NWDBasis<NWDLocalization>.GetObjectByInternalKey(sText.text) as NWDLocalization;
                if (tObject != null)
                {
                    sText.text = tObject.TextValue.GetLocalString();
                }
                else
                {
                    CreateLocalizationTextValue(sText.text, sDefault);
                }
            }
            else
            {
                EditorUtility.DisplayDialog("AutoLocalize", "Text component is null", "OK");
            }
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
		public override bool AddonEdited (bool sNeedBeUpdate)
		{
			if (sNeedBeUpdate == true) {
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
	#region Connexion NWDLocalization with Unity MonoBehavior
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWDLocalization connexion.
	/// In your MonoBehaviour Script connect object with :
	/// <code>
	///	[NWDConnexionAttribut(true,true, true, true)]
	/// public NWDLocalizationConnexion MyNWDLocalizationObject;
	/// </code>
	/// </summary>
	//-------------------------------------------------------------------------------------------------------------
	// CONNEXION STRUCTURE METHODS
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDLocalizationConnexion
	{
		//-------------------------------------------------------------------------------------------------------------
		[SerializeField]
		public string Reference;
		//-------------------------------------------------------------------------------------------------------------
		public NWDLocalization GetObject ()
		{
			return NWDLocalization.GetObjectByReference (Reference);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetObject (NWDLocalization sObject)
		{
			if (sObject != null) {
				Reference = sObject.Reference;
			} else {
				Reference = "";
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDLocalization NewObject ()
		{
			NWDLocalization tObject = NWDLocalization.NewObject ();
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
	[CustomPropertyDrawer (typeof(NWDLocalizationConnexion))]
	public class NWDLocalizationConnexionDrawer : PropertyDrawer
	{
		//-------------------------------------------------------------------------------------------------------------
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0) {
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true) [0];
			}
			return NWDLocalization.ReferenceConnexionHeightSerialized (property, tReferenceConnexion.ShowInspector);
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0) {
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true) [0];
			}
			NWDLocalization.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
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