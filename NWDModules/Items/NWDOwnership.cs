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
	/// <summary>
	/// NWD ownership. This class connect the item to the account. The item is decripted in NWDItem, but some informations
	/// specific to this ownership are available only here. For example : the quantity of this item in chest, the first 
	/// acquisition statut or some particular values (A, B, C, etc.).
	/// It's a generic class for traditionla game.
	/// </summary>
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
		/// <summary>
		/// Gets or sets the account reference.
		/// </summary>
		/// <value>The account reference.</value>
		/// 
		[NWDGroupStartAttribute("Ownership",true, true, true)] // ok
		public bool FirstAcquisition { get; set; }
		[Indexed ("AccountIndex", 0)]
		public NWDReferenceType<NWDAccount> Account { get; set; }
		public NWDReferenceType<NWDItem> Item { get; set; }
		//[NWDIntSliderAttribute(0,250)]
		public int Quantity { get; set; }
		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute ("Extensions", true, true, true)]
		public NWDReferencesQuantityType<NWDItem> ItemsContained{ get; set; }
		public NWDReferencesQuantityType<NWDItemProperties> ItemProperties { get; set; }
		[NWDGroupEndAttribute]
		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute ("Development addons", true, true, true)]
		public string JSON { get; set; }
		public string KeysValues { get; set; }
		//[NWDGroupEndAttribute]
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Constructors
		//-------------------------------------------------------------------------------------------------------------
		public NWDOwnership()
		{
			//Init your instance here
			FirstAcquisition = true;
		}
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------
        // OWNERSHIP AND ITEM FOR PLAYER
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Ownership for item's reference.
        /// </summary>
        /// <returns>The for item reference.</returns>
        /// <param name="sItemReference">S item reference.</param>
        public static NWDOwnership OwnershipForItemReference(string sItemReference)
        {
            NWDOwnership rOwnershipToUse = null;
            foreach (NWDOwnership tOwnership in NWDOwnership.GetAllObjects())
            {
                if (tOwnership.Item.GetReference() == sItemReference)
                {
                    rOwnershipToUse = tOwnership;
                    break;
                }
            }
            if (rOwnershipToUse == null)
            {
                rOwnershipToUse = NWDOwnership.NewObject();
                rOwnershipToUse.Item.SetReference(sItemReference);
                rOwnershipToUse.Quantity = 0;
                rOwnershipToUse.SaveModifications();
            }
            return rOwnershipToUse;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Ownership for item's reference exists.
        /// </summary>
        /// <returns><c>true</c>, if for item reference exists was ownershiped, <c>false</c> otherwise.</returns>
        /// <param name="sItemReference">S item reference.</param>
        public static bool OwnershipForItemReferenceExists(string sItemReference)
        {
            NWDOwnership rOwnershipToUse = null;
            foreach (NWDOwnership tOwnership in NWDOwnership.GetAllObjects())
            {
                if (tOwnership.Item.GetReference() == sItemReference)
                {
                    rOwnershipToUse = tOwnership;
                    break;
                }
            }
            return rOwnershipToUse != null;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Return the Ownership for item.
        /// </summary>
        /// <returns>The for item.</returns>
        /// <param name="sItem">S item.</param>
        public static NWDOwnership OwnershipForItem(NWDItem sItem)
        {
            NWDOwnership rOwnershipToUse = null;
            foreach (NWDOwnership tOwnership in NWDOwnership.GetAllObjects())
            {
                if (tOwnership.Item.GetObject() == sItem)
                {
                    rOwnershipToUse = tOwnership;
                    break;
                }
            }
            if (rOwnershipToUse == null)
            {
                rOwnershipToUse = NWDOwnership.NewObject();
                rOwnershipToUse.Item.SetObject(sItem);
                rOwnershipToUse.Quantity = 0;
                rOwnershipToUse.SaveModifications();
            }
            return rOwnershipToUse;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Ownership for item exists.
        /// </summary>
        /// <returns><c>true</c>, if for item exists was ownershiped, <c>false</c> otherwise.</returns>
        /// <param name="sItem">S item.</param>
        public static bool OwnershipForItemExists(NWDItem sItem)
        {
            NWDOwnership rOwnershipToUse = null;
            foreach (NWDOwnership tOwnership in NWDOwnership.GetAllObjects())
            {
                if (tOwnership.Item.GetObject() == sItem)
                {
                    rOwnershipToUse = tOwnership;
                    break;
                }
            }
            return rOwnershipToUse != null;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Add the item's quantity to ownership.
        /// </summary>
        /// <returns>The item to ownership.</returns>
        /// <param name="sItem">S item.</param>
        /// <param name="sQuantity">S quantity.</param>
        public static NWDOwnership AddItemToOwnership(NWDItem sItem, int sQuantity)
        {
            NWDOwnership rOwnershipToUse = OwnershipForItem(sItem);
            rOwnershipToUse.Quantity += sQuantity;
            rOwnershipToUse.SaveModifications();
            return rOwnershipToUse;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Remove the item's quantity to ownership.
        /// </summary>
        /// <returns>The item to ownership.</returns>
        /// <param name="sItem">S item.</param>
        /// <param name="sQuantity">S quantity.</param>
        public static NWDOwnership RemoveItemToOwnership(NWDItem sItem, int sQuantity)
        {
            NWDOwnership rOwnershipToUse = OwnershipForItem(sItem);
            rOwnershipToUse.Quantity -= sQuantity;
            rOwnershipToUse.SaveModifications();
            return rOwnershipToUse;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Add item's quantity to ownership.
        /// </summary>
        /// <param name="sItemsReferenceQuantity">items reference/quantity.</param>
        public static void AddItemToOwnership(NWDReferencesQuantityType<NWDItem> sItemsReferenceQuantity)
        {
            if (sItemsReferenceQuantity != null)
            {
                foreach (KeyValuePair<string, int> tItemQuantity in sItemsReferenceQuantity.GetReferenceAndQuantity())
                {
                    NWDOwnership rOwnershipToUse = OwnershipForItemReference(tItemQuantity.Key);
                    rOwnershipToUse.Quantity += tItemQuantity.Value;
                    rOwnershipToUse.SaveModifications();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Remove item's quantity to ownership.
        /// </summary>
        /// <param name="sItemsReferenceQuantity">items reference/quantity.</param>
        public static void RemoveItemToOwnership(NWDReferencesQuantityType<NWDItem> sItemsReferenceQuantity)
        {
            if (sItemsReferenceQuantity != null)
            {
                foreach (KeyValuePair<string, int> tItemQuantity in sItemsReferenceQuantity.GetReferenceAndQuantity())
                {
                    NWDOwnership rOwnershipToUse = OwnershipForItemReference(tItemQuantity.Key);
                    rOwnershipToUse.Quantity -= tItemQuantity.Value;
                    rOwnershipToUse.SaveModifications();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Contains items with quantities.
        /// </summary>
        /// <returns><c>true</c>, if items was contained with the enough quantity, <c>false</c> otherwise.</returns>
        /// <param name="sItemsReferenceQuantity">S items reference quantity.</param>
        public static bool ContainsItems(NWDReferencesQuantityType<NWDItem> sItemsReferenceQuantity)
        {
            bool rReturn = true;
            if (sItemsReferenceQuantity != null)
            {
                foreach (KeyValuePair<string, int> tItemQuantity in sItemsReferenceQuantity.GetReferenceAndQuantity())
                {
                    NWDOwnership rOwnershipToUse = OwnershipForItemReference(tItemQuantity.Key);
                    if (rOwnershipToUse.Quantity < tItemQuantity.Value)
                    {
                        rReturn = false;
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        public bool CheckOwnershipAndItemValidity()
        {
            bool rReturn = false;

            NWDItem tNWDItem = Item.GetObject();

            // Check if item is not null
            if (tNWDItem != null)
            {
                // Check if item is enable
                if (tNWDItem.IsEnable() && IsEnable())
                {
                    rReturn = true;
                }
            }

            return rReturn;
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
			if (sObject != null) {
				Reference = sObject.Reference;
			} else {
				Reference = "";
			}
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