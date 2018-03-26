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
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	/// <summary>
	/// NWDExampleConnection can be use in MonBehaviour script to connect GameObject with NWDBasis<Data> in editor.
	/// Use like :
	/// public class MyScriptInGame : MonoBehaviour
	/// { 
	/// [NWDConnectionAttribut (true, true, true, true)] // optional
	/// public NWDExampleConnection MyNetWorkedData;
	/// }
	/// </summary>
	[Serializable]
	public class NWDOwnershipConnection : NWDConnection <NWDOwnership> {}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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
		public int Quantity { get; set; }
        public string Name { get; set; }
		[NWDGroupEndAttribute]

        [NWDGroupSeparatorAttribute]

		[NWDGroupStartAttribute ("Extensions", true, true, true)]
        public NWDReferencesArrayType<NWDOwnership> ItemsContained { get; set; }
		public NWDReferencesQuantityType<NWDItemProperties> ItemProperties { get; set; }
		[NWDGroupEndAttribute]
		
        [NWDGroupSeparatorAttribute]

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
            //Debug.Log("NWDOwnership Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDOwnership(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDOwnership Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------

        //-------------------------------------------------------------------------------------------------------------
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDOwnership), typeof(NWDItem)};
        }
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
            foreach (NWDOwnership tOwnership in GetAllObjects())
            {
                if (tOwnership.Item.GetReference() == sItemReference)
                {
                    rOwnershipToUse = tOwnership;
                    break;
                }
            }
            if (rOwnershipToUse == null)
            {
                rOwnershipToUse = NewObject();
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
        /// <param name="sItemReference">item reference.</param>
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
        /// Return the Ownership for selected item.
        /// </summary>
        /// <returns>The ownership.</returns>
        /// <param name="sItem">selected item.</param>
        public static NWDOwnership OwnershipForItem(NWDItem sItem)
        {
            NWDOwnership rOwnershipToUse = null;
            foreach (NWDOwnership tOwnership in GetAllObjects())
            {
                if (tOwnership.Item.GetObject() == sItem)
                {
                    rOwnershipToUse = tOwnership;
                    break;
                }
            }
            if (rOwnershipToUse == null)
            {
                rOwnershipToUse = NewObject();
				//--------------
				#if UNITY_EDITOR
				//--------------
				if (sItem.Name != null) {
					string tItemNameBase = sItem.Name.GetBaseString ();
					if (tItemNameBase != null) {
						rOwnershipToUse.InternalKey = tItemNameBase;
					}
				}
				rOwnershipToUse.InternalDescription = NWDPreferences.GetString("NickNameKey", "no nickname");
				//--------------
				#endif
				//--------------
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
		/// Add the item's quantity to ownership and set to sQuantity.
		/// </summary>
		/// <returns>The item to ownership.</returns>
		/// <param name="sItem">S item.</param>
		/// <param name="sQuantity">S quantity.</param>
		public static NWDOwnership SetItemToOwnership(NWDItem sItem, int sQuantity)
		{
			NWDOwnership rOwnershipToUse = OwnershipForItem(sItem);
			rOwnershipToUse.Quantity = sQuantity;
			rOwnershipToUse.SaveModifications();
			return rOwnershipToUse;
		}
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Add the item's quantity to ownership.
        /// </summary>
        /// <returns>The item to ownership.</returns>
        /// <param name="sItem">S item.</param>
        /// <param name="sQuantity">S quantity.</param>
        public static NWDOwnership AddItemToOwnership(NWDItem sItem, int sQuantity, bool sIsIncrement = true)
        {
            NWDOwnership rOwnership = OwnershipForItem(sItem);
            if (sIsIncrement)
            {
                rOwnership.Quantity += sQuantity;
            }
            else
            {
                rOwnership.Quantity = sQuantity;
            }
            rOwnership.SaveModifications();
            return rOwnership;
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
            NWDOwnership rOwnership = OwnershipForItem(sItem);
            rOwnership.Quantity -= sQuantity;
            rOwnership.SaveModifications();
            return rOwnership;
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
                if (sItemsReferenceQuantity.IsEmpty())
                {
                    // No test needed
                }
                else
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
            }
            return rReturn;
        }
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Contains item with quantitie.
		/// </summary>
		/// <returns><c>true</c>, if item was contained with the enough quantity, <c>false</c> otherwise.</returns>
		/// <param name="sItemsReferenceQuantity">S item reference quantity.</param>
		public static bool ContainsItem(NWDItem sItem, int sQuantity)
        {
			bool rReturn = true;
			if (sItem != null)
			{
                NWDOwnership rOwnershipToUse = OwnershipForItemReference(sItem.Reference);
				if (rOwnershipToUse.Quantity < sQuantity)
				{
					rReturn = false;
				}
			}
			return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // TODO : Verif this method
        public static bool ContainsItemGroups(NWDReferencesQuantityType<NWDItemGroup> sItemGroupsReferenceQuantity)
        {
            bool rReturn = true;
            if (sItemGroupsReferenceQuantity != null)
            {
                if (sItemGroupsReferenceQuantity.IsEmpty())
                {
                    // No test needed
                }
                else
                {
                    foreach (KeyValuePair<NWDItemGroup, int> tItemQuantity in sItemGroupsReferenceQuantity.GetObjectAndQuantity())
                    {
                        if (ContainsItemGroups(tItemQuantity.Key, tItemQuantity.Value) == false)
                        {
                            rReturn = false;
                            break;
                        }
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        // TODO : Verif this method
        public static bool ContainsItemGroups(NWDItemGroup sItemGroup, int sQuantity)
        {
            bool rReturn = true;
            if (sItemGroup != null)
            {
                rReturn = false;
                int tQ = 0;
                foreach (NWDItem tItem in sItemGroup.ItemList.GetObjects())
                {
                    NWDOwnership tOwnership = NWDOwnership.OwnershipForItem(tItem);
                    tQ = tQ + tOwnership.Quantity;
                    if (tQ >= sQuantity)
                    {
                        rReturn = true;
                        break;
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
        public override void Initialization()
        {
            FirstAcquisition = true;
        }
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