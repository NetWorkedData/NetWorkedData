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
	[NWDClassTrigrammeAttribute ("ERR")]
	[NWDClassDescriptionAttribute ("Error descriptions Class")]
	[NWDClassMenuNameAttribute ("Errors")]
	//-------------------------------------------------------------------------------------------------------------
	[NWDInternalKeyNotEditableAttribute]
	//-------------------------------------------------------------------------------------------------------------
//	[NWDTypeClassInPackageAttribute]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDError : NWDBasis <NWDError>
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
		//public bool DiscoverItYourSelf { get; set; }
		public string Domain { get; set; }

		public string Code { get; set; }

		public NWDLocalizableStringType LocalizedTitle { get; set; }

		public NWDLocalizableStringType LocalizedDescription { get; set; }

		[NWDEnumString (new string[] { "alert", "critical", "verbose" })]
		public string Type { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Constructors
		//-------------------------------------------------------------------------------------------------------------
		public NWDError ()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Class methods
		//-------------------------------------------------------------------------------------------------------------
		public static NWDError GetErrorWithCode (string sCode)
		{
			NWDError rReturn = null;
			foreach (NWDError tObject in NWDError.ObjectsList) {
				if (tObject.Code == sCode) {
					rReturn = tObject;
					break;
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public static NWDError GetErrorWithDomainAndCode (string sDomain , string sCode)
		{
			NWDError rReturn = null;
			foreach (NWDError tObject in NWDError.ObjectsList) {
				if (tObject.Code == sCode && tObject.Domain == sDomain) {
					rReturn = tObject;
					break;
				}
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public static NWDError CreateGenericError (string sDomain, string sCode, string sTitle, string sDescription, string sType = "verbose")
		{
			string tReference = "ERR-"+sDomain + "-" + sCode;
			// TODO: alert if reference is too long for ereg / or substring if too long
			NWDError tError = InstanceByReference (tReference) as NWDError;
			if (tError == null) {
				tError = NWDBasis<NWDError>.NewInstance () as NWDError;
				RemoveObjectInListOfEdition (tError);
				tError.Reference = tReference;
				//				tError.InternalKey = Domain + " : " + sCode;
				tError.InternalDescription = sDescription;
				// domain code
				tError.Domain = sDomain;
				tError.Code = sCode;
				// title
				NWDLocalizableStringType tTitle = new NWDLocalizableStringType ();
				tTitle.Value = "BASE:" + sTitle;
				tError.LocalizedTitle = tTitle;
				// description
				NWDLocalizableStringType tDescription = new NWDLocalizableStringType ();
				tDescription.Value = "BASE:" + sDescription;
				tError.LocalizedDescription = tDescription;
				// type of alert
				tError.Type = sType;
				// add-on edited
				tError.AddonEdited (true);
				// reccord
				tError.UpdateMe ();
				AddObjectInListOfEdition (tError);
			}
			return tError;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
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
			if (sNeedBeUpdate == true) {
				if (Domain == null || Domain == "") {
					Domain = "Unknow";
				}
				InternalKey = Domain + " : "+ Code;
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
	#region Connexion NWDError with Unity MonoBehavior
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWDError connexion.
	/// In your MonoBehaviour Script connect object with :
	/// <code>
	///	[NWDConnexionAttribut(true,true, true, true)]
	/// public NWDErrorConnexion MyNWDErrorObject;
	/// </code>
	/// </summary>
	//-------------------------------------------------------------------------------------------------------------
	// CONNEXION STRUCTURE METHODS
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDErrorConnexion
	{
		//-------------------------------------------------------------------------------------------------------------
		[SerializeField]
		public string Reference;
		//-------------------------------------------------------------------------------------------------------------
		public NWDError GetObject ()
		{
			return NWDError.GetObjectByReference (Reference);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetObject (NWDError sObject)
		{
			Reference = sObject.Reference;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDError NewObject ()
		{
			NWDError tObject = NWDError.NewObject ();
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
	[CustomPropertyDrawer (typeof(NWDErrorConnexion))]
	public class NWDErrorConnexionDrawer : PropertyDrawer
	{
		//-------------------------------------------------------------------------------------------------------------
		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true)[0];
			}
			return NWDError.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
		}
		//-------------------------------------------------------------------------------------------------------------
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			NWDConnexionAttribut tReferenceConnexion = new NWDConnexionAttribut ();
			if (fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true).Length > 0)
			{
				tReferenceConnexion = (NWDConnexionAttribut)fieldInfo.GetCustomAttributes (typeof(NWDConnexionAttribut), true)[0];
			}
			NWDError.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
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