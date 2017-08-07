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
	[NWDClassTrigrammeAttribute ("OWS")]
	[NWDClassDescriptionAttribute ("Ownership descriptions Class")]
	[NWDClassMenuNameAttribute ("Ownership")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDOwnership :NWDBasis <NWDOwnership>
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
		[Indexed ("AccountIndex", 0)]
		public NWDReferenceType<NWDAccount> AccountReference { get; set; }
		public NWDReferenceType<NWDItem> ItemReference { get; set; }
		//[NWDIntSliderAttribute(0,250)]
		public int Quantity { get; set; }
		public string ValueA { get; set; }
		public string ValueB { get; set; }
		public string ValueC { get; set; }
		public string ValueD { get; set; }
		public string ValueE { get; set; }
		public string ValueF { get; set; }
		public string ValueG { get; set; }
		public string ValueH { get; set; }
		public string ValueI { get; set; }
		public string ValueJ { get; set; }
		public string Special { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Constructors
		//-------------------------------------------------------------------------------------------------------------
		public NWDOwnership()
		{
			//Init your instance here
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Class methods
		//-------------------------------------------------------------------------------------------------------------
		public static void MyClassMethod ()
		{
			// do something with this class
		}
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Instance methods
		//-------------------------------------------------------------------------------------------------------------

		//-------------------------------------------------------------------------------------------------------------
		// OWNERSHIP AND ITEM FOR PLAYER
		//-------------------------------------------------------------------------------------------------------------
		public NWDOwnership OwnershipForItem (NWDItem sItem)
		{
			NWDOwnership rOwnershipToUse = null;
			foreach (NWDOwnership tOwnership in NWDOwnership.GetAllObjects()) {
				if (tOwnership.ItemReference.GetObject () == sItem) {
					rOwnershipToUse = tOwnership;
					break;
				}
			}
			if (rOwnershipToUse == null) {
				rOwnershipToUse = NWDOwnership.NewObject ();
				rOwnershipToUse.ItemReference.SetObject (sItem);
				rOwnershipToUse.Quantity = 0;
				rOwnershipToUse.SaveModifications ();
			}
			return rOwnershipToUse;
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool OwnershipForItemExists (NWDItem sItem)
		{
			NWDOwnership rOwnershipToUse = null;
			foreach (NWDOwnership tOwnership in NWDOwnership.GetAllObjects()) {
				if (tOwnership.ItemReference.GetObject () == sItem) {
					rOwnershipToUse = tOwnership;
					break;
				}
			}
			return rOwnershipToUse != null;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOwnership AddItemToOwnership (NWDItem sItem, int sQuantity)
		{
			NWDOwnership rOwnershipToUse = OwnershipForItem (sItem);
			rOwnershipToUse.Quantity += sQuantity;
			rOwnershipToUse.SaveModifications ();
			return rOwnershipToUse;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOwnership RemoveItemToOwnership (NWDItem sItem, int sQuantity)
		{
			NWDOwnership rOwnershipToUse = OwnershipForItem (sItem);
			rOwnershipToUse.Quantity -= sQuantity;
			rOwnershipToUse.SaveModifications ();
			return rOwnershipToUse;
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
	#region Connexion NWDOwnership with Unity MonoBehavior
	//-------------------------------------------------------------------------------------------------------------
	/// <summary>
	/// NWDOwnership connexion.
	/// In your MonoBehaviour Script connect object with :
	/// <code>
	///	[NWDConnexionAttribut(true,true, true, true)]
	/// public NWDOwnershipConnexion MyNWDOwnershipObject;
	/// </code>
	/// </summary>
	//-------------------------------------------------------------------------------------------------------------
	// CONNEXION STRUCTURE METHODS
	//-------------------------------------------------------------------------------------------------------------
	[Serializable]
	public class NWDOwnershipConnexion
	{
		//-------------------------------------------------------------------------------------------------------------
		[SerializeField]
		public string Reference;
		//-------------------------------------------------------------------------------------------------------------
		public NWDOwnership GetObject ()
		{
			return NWDOwnership.GetObjectByReference (Reference);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetObject (NWDOwnership sObject)
		{
			Reference = sObject.Reference;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDOwnership NewObject ()
		{
			NWDOwnership tObject = NWDOwnership.NewObject ();
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
	[CustomPropertyDrawer (typeof(NWDOwnershipConnexion))]
	public class NWDOwnershipConnexionDrawer : PropertyDrawer
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
			return NWDOwnership.ReferenceConnexionHeightSerialized(property, tReferenceConnexion.ShowInspector);
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
			NWDOwnership.ReferenceConnexionFieldSerialized (position, property.displayName, property, "", tReferenceConnexion.ShowInspector, tReferenceConnexion.Editable, tReferenceConnexion.EditButton, tReferenceConnexion.NewButton);
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