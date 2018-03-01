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
    [Serializable]
    public class NWDCraftBookConnection : NWDConnection<NWDCraftBook>
    {
    }
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("CBK")]
	[NWDClassDescriptionAttribute ("Craft Book Recipes descriptions Class")]
	[NWDClassMenuNameAttribute ("Craft Book")]
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	public partial class NWDCraftBook :NWDBasis <NWDCraftBook>
	{
		//-------------------------------------------------------------------------------------------------------------
		#region Class Properties
		//-------------------------------------------------------------------------------------------------------------
		private static Dictionary<string,NWDCraftBook> HashByCraftDictionary = new Dictionary<string,NWDCraftBook> ();
		private static Dictionary<string,NWDCraftBook> ItemByCraftDictionary = new Dictionary<string,NWDCraftBook> ();
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
		#region Instance Properties
		//-------------------------------------------------------------------------------------------------------------
		[NWDGroupStartAttribute ("Description", true, true, true)] // ok
		public NWDReferenceType<NWDItem> ItemToDescribe { get; set; }
		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute ("Recipe attribut", true, true, true)] // ok
		public bool OrderIsImportant { get; set; }
		public NWDReferenceType<NWDRecipientGroup> RecipientGroup { get; set; }
		public NWDReferencesArrayType<NWDItemGroup> ItemGroupIngredient { get; set; }
		public NWDReferencesQuantityType<NWDItem> ItemResult { get; set; }
		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute ("FX (Special Effects)", true, true, true)]
		public NWDPrefabType SuccessParticles { get; set; }
		public NWDPrefabType SuccessSound { get; set; }
		public NWDPrefabType FailParticles { get; set; }
		public NWDPrefabType FailSound { get; set; }
		[NWDGroupEndAttribute]

		[NWDSeparatorAttribute]

		[NWDGroupStartAttribute ("Development addons", true, true, true)]
		[NWDNotEditableAttribute]
		public string RecipeHash { get; set; }
		//[NWDGroupEndAttribute]
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
        #region Constructors
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NetWorkedData.NWDCraftBook"/> class.
		/// </summary>
		public NWDCraftBook ()
        {
            Debug.Log("NWDCraftBook Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDCraftBook(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            Debug.Log("NWDCraftBook Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            OrderIsImportant = true;
        }
		//-------------------------------------------------------------------------------------------------------------
		#endregion
		//-------------------------------------------------------------------------------------------------------------
        #region Class methods
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Count the craft book in dictionary of hash.
		/// </summary>
		/// <returns>The craft book in hash dictionary.</returns>
		private static int CountCraftBookInHashDictionary ()
		{
			return HashByCraftDictionary.Count;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Add the craft book in dictionary of hash.
		/// </summary>
		/// <param name="sCraftBook">S craft book.</param>
		private static void AddCraftBookInHashDictionary (NWDCraftBook sCraftBook)
		{
            if (HashByCraftDictionary != null)
            {
                if (sCraftBook.RecipeHash != null)
                {
                    //HashByCraftDictionary
                    if (HashByCraftDictionary.ContainsValue(sCraftBook))
                    {
                        string tKey = HashByCraftDictionary.FirstOrDefault(x => x.Value == sCraftBook).Key;
                        HashByCraftDictionary.Remove(tKey);
                    }
                    if (HashByCraftDictionary.ContainsKey(sCraftBook.RecipeHash) == true)
                    {
                    }
                    else
                    {
                        HashByCraftDictionary.Add(sCraftBook.RecipeHash, sCraftBook);
                    }
                    // ItemByCraftDictionary
                    if (sCraftBook.ItemToDescribe.Value != "")
                    {
                        if (ItemByCraftDictionary.ContainsValue(sCraftBook))
                        {
                            string tKey = ItemByCraftDictionary.FirstOrDefault(x => x.Value == sCraftBook).Key;
                            ItemByCraftDictionary.Remove(tKey);
                        }
                        if (ItemByCraftDictionary.ContainsKey(sCraftBook.ItemToDescribe.Value) == true)
                        {
                        }
                        else
                        {
                            ItemByCraftDictionary.Add(sCraftBook.ItemToDescribe.Value, sCraftBook);
                        }
                    }
                }
            }
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Remove the craft book in dictionary of hash.
		/// </summary>
		/// <param name="sCraftBook">S craft book.</param>
		private static void RemoveCraftBookInHashDictionary (NWDCraftBook sCraftBook)
		{
			//HashByCraftDictionary
			if (HashByCraftDictionary.ContainsKey (sCraftBook.RecipeHash) == true) {
				HashByCraftDictionary.Remove (sCraftBook.RecipeHash);
			}
			//ItemByCraftDictionary
			if (sCraftBook.ItemToDescribe.Value != "") {
				if (ItemByCraftDictionary.ContainsKey (sCraftBook.ItemToDescribe.Value) == true) {
					ItemByCraftDictionary.Remove (sCraftBook.ItemToDescribe.Value);
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Update the craft book dictionary of hash.
		/// </summary>
		/// <param name="sCraftBook">S craft book.</param>
		private static void UpdateCraftBookInHashDictionary (NWDCraftBook sCraftBook)
		{
			if (HashByCraftDictionary.ContainsValue (sCraftBook)) {
				// TODO : remove old key/value ... but which key to used?
				string tKey = HashByCraftDictionary.FirstOrDefault (x => x.Value == sCraftBook).Key;
				HashByCraftDictionary.Remove (tKey);
			}
			if (HashByCraftDictionary.ContainsKey (sCraftBook.RecipeHash) == true) {
				// BIG ERROR Hash is not unique!
			} else {
				HashByCraftDictionary.Add (sCraftBook.RecipeHash, sCraftBook);
			}
			// ItemByCraftDictionary
			if (sCraftBook.ItemToDescribe.Value != "") {
				if (ItemByCraftDictionary.ContainsValue (sCraftBook)) {
					string tKey = ItemByCraftDictionary.FirstOrDefault (x => x.Value == sCraftBook).Key;
					ItemByCraftDictionary.Remove (tKey);
				} 
				if (ItemByCraftDictionary.ContainsKey (sCraftBook.ItemToDescribe.Value) == true) {
				} else {
					ItemByCraftDictionary.Add (sCraftBook.ItemToDescribe.Value, sCraftBook);
				}
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Find the craft book dictionary of hash by a specific hash.
		/// </summary>
		/// <returns>The craft book in hash dictionary.</returns>
		/// <param name="sHash">S hash.</param>
		private static NWDCraftBook FindCraftBookInHashDictionary (string sHash)
		{
			NWDCraftBook rReturn = null;
			if (HashByCraftDictionary.ContainsKey (sHash) == true) {
				rReturn = HashByCraftDictionary [sHash];
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get the craft book by item.
		/// </summary>
		/// <returns>The craft book by item.</returns>
		/// <param name="item">Item.</param>
		public static NWDCraftBook GetCraftBookByItem (NWDItem sItem)
		{
//			NWDCraftBook rReturn = null;
//			NWDCraftBook[] tRecipes = GetAllObjects ();
//			foreach (NWDCraftBook recipe in tRecipes) {
//				if (recipe.ItemToDescribe.ContainsReference (item.Reference)) {
//					rReturn = recipe;
//					break;
//				}
//			}
//			return rReturn;
//
			NWDCraftBook rReturn = null;
			if (ItemByCraftDictionary.ContainsKey (sItem.Reference) == true) {
				rReturn = ItemByCraftDictionary [sItem.Reference];
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get the first craft book collision (no recipient, just two ietm chock themselves).
		/// </summary>
		/// <returns>The first craft book collision.</returns>
		/// <param name="sItemA">S item a.</param>
		/// <param name="sItemB">S item b.</param>
		public static NWDCraftBook GetFirstCraftBookCollision (NWDItem sItemA, NWDItem sItemB)
		{
			Debug.Log ("GetFirstCraftBookCollision NWDCraftBook.GetAllObjects().Length) = " + NWDCraftBook.GetAllObjects ().Length);
			NWDCraftBook tCraftBook = null;
			NWDItemGroup[] tGroupA = sItemA.ItemGroupList.GetObjects ();
			NWDItemGroup[] tGroupB = sItemB.ItemGroupList.GetObjects ();
			if (tGroupA.Length > 0 && tGroupB.Length > 0) {
				foreach (NWDItemGroup tItemA in tGroupA) {
					foreach (NWDItemGroup tItemB in tGroupB) {
						NWDReferencesArrayType<NWDItemGroup> tItems = new NWDReferencesArrayType<NWDItemGroup> ();
						tItems.AddObject (tItemA);
						tItems.AddObject (tItemB);
						NWDCraftBook tCraftFound = NWDCraftBook.GetFirstCraftBookFor (tItems);
						if (tCraftFound != null) {
							if (tCraftFound.AC == true) {
								tCraftBook = tCraftFound;
							}
						}
						if (tCraftBook != null) {
							break;
						}
					}
					if (tCraftBook != null) {
						break;
					}
				}
			}
			// Add Craftbook to ownership
			if (tCraftBook != null) {
				// Add to ownership  :-)
				if (tCraftBook.ItemToDescribe.GetObject () != null) {
				}
				NWDOwnership.SetItemToOwnership (tCraftBook.ItemToDescribe.GetObject (), 1);
			}
			return tCraftBook;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get the first craft book without recipient between item's groups.
		/// </summary>
		/// <returns>The first craft book for.</returns>
		/// <param name="sItemGroupIngredient">S item group ingredient.</param>
		public static NWDCraftBook GetFirstCraftBookFor (NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient)
		{
			Debug.Log ("GetFirstCraftBookFor NWDCraftBook.GetAllObjects().Length) = " + NWDCraftBook.GetAllObjects ().Length);
			NWDCraftBook tCraftBook = null;
			string tRecipientValue = "";
			bool tOrdered = true;
			string tAssemblyA = tOrdered.ToString () + tRecipientValue + sItemGroupIngredient.ToString ();
			tOrdered = false;
			string tAssemblyB = tOrdered.ToString () + tRecipientValue + sItemGroupIngredient.ToStringSorted ();
			string tRecipeHashA = BTBSecurityTools.GenerateSha (tAssemblyA, BTBSecurityShaTypeEnum.Sha1);
			string tRecipeHashB = BTBSecurityTools.GenerateSha (tAssemblyB, BTBSecurityShaTypeEnum.Sha1);
			foreach (NWDCraftBook tCraft in NWDCraftBook.GetAllObjects()) {
				if (tCraft.RecipeHash == tRecipeHashA || tCraft.RecipeHash == tRecipeHashB) {
					tCraftBook = tCraft;
					break;
				}
			}
			// Add Craftbook to ownership
			if (tCraftBook != null) {
				// Add to ownership  :-)
				if (tCraftBook.ItemToDescribe.GetObject () != null) {
				}
				NWDOwnership.SetItemToOwnership (tCraftBook.ItemToDescribe.GetObject (), 1);
			}
			return tCraftBook;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Get the craft book with recipients and items inside.
		/// </summary>
		/// <returns>The craft book with recipients.</returns>
		/// <param name="sRecipientGroup">S recipient group.</param>
		/// <param name="sItemGroupIngredientList">S item group ingredient list.</param>
		public static NWDCraftBook[] GetCraftBookWithRecipients (NWDReferencesListType<NWDRecipientGroup> sRecipientGroup, List<NWDReferencesArrayType<NWDItemGroup>> sItemGroupIngredientList)
		{
			Debug.Log ("GetCraftBookWithRecipients NWDCraftBook.GetAllObjects().Length) = " + NWDCraftBook.GetAllObjects ().Length);
			NWDCraftBook tReturnPrimary = null;
			NWDRecipientGroup tReturnRecipient = null;
			NWDReferencesArrayType<NWDItemGroup> tItemsGroupUsed = new NWDReferencesArrayType<NWDItemGroup>();
			List<NWDCraftBook> tReturnList = new List<NWDCraftBook> ();
			// I get all recipients possibilities
			string[] tRecipientsArray = new string[]{ "" };
			if (sRecipientGroup != null) {
				if (sRecipientGroup.Value != "") {
					tRecipientsArray = sRecipientGroup.GetReferences ();
				}
			}
			if (sItemGroupIngredientList.Count > 0) {
				// I sort by Order
				sItemGroupIngredientList.Sort ((tA, tB) => tB.Value.Length.CompareTo (tA.Value.Length));
				// I get all items layout
				// I search the max lenght
				List<NWDReferencesArrayType<NWDItemGroup>> tItemGroupIngredientListMax = new List<NWDReferencesArrayType<NWDItemGroup>> ();
				int tLenghtMax = sItemGroupIngredientList [0].Value.Length;
				foreach (NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient in sItemGroupIngredientList) {
					if (tLenghtMax == sItemGroupIngredient.Value.Length) {
						tItemGroupIngredientListMax.Add (sItemGroupIngredient);
					}
				}
				foreach (string tRecipientValue in tRecipientsArray) {
					NWDRecipientGroup tRecipient = NWDRecipientGroup.GetObjectByReference (tRecipientValue);


					if (tRecipient != null) {
						Debug.Log ("GetCraftBookWithRecipients search for tRecipient = " + tRecipient.InternalKey);
						// I craft only max items composition ?
						if (tRecipient.CraftOnlyMax == true) {

							foreach (NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient in tItemGroupIngredientListMax) {

								bool tOrdered = true;
								string tAssemblyA = tOrdered.ToString () + tRecipientValue + sItemGroupIngredient.ToString ();
								tOrdered = false;
								string tAssemblyB = tOrdered.ToString () + tRecipientValue + sItemGroupIngredient.ToStringSorted ();
								string tRecipeHashA = BTBSecurityTools.GenerateSha (tAssemblyA, BTBSecurityShaTypeEnum.Sha1);
								string tRecipeHashB = BTBSecurityTools.GenerateSha (tAssemblyB, BTBSecurityShaTypeEnum.Sha1);

								Debug.Log ("GetCraftBookWithRecipients search for tRecipient " + tRecipient.InternalKey + " Craft !!!only max!!! with " + sItemGroupIngredient.ToString () + "  => hash " + tRecipeHashA + " or " + tRecipeHashB);

								foreach (NWDCraftBook tCraft in NWDCraftBook.GetAllObjects()) {
									if (tCraft.RecipeHash == tRecipeHashA || tCraft.RecipeHash == tRecipeHashB) {
										tReturnList.Add (tCraft);
										tReturnPrimary = tCraft;
										tItemsGroupUsed = sItemGroupIngredient;
										tReturnRecipient = tRecipient;
										break;
									}
								}
								if (tReturnPrimary != null) {
									break;
								}
							}
						} else {
							foreach (NWDReferencesArrayType<NWDItemGroup> sItemGroupIngredient in sItemGroupIngredientList) {
								bool tOrdered = true;
								string tAssemblyA = tOrdered.ToString () + tRecipientValue + sItemGroupIngredient.ToString ();
								tOrdered = false;
								string tAssemblyB = tOrdered.ToString () + tRecipientValue + sItemGroupIngredient.ToStringSorted ();
								string tRecipeHashA = BTBSecurityTools.GenerateSha (tAssemblyA, BTBSecurityShaTypeEnum.Sha1);
								string tRecipeHashB = BTBSecurityTools.GenerateSha (tAssemblyB, BTBSecurityShaTypeEnum.Sha1);

								Debug.Log ("GetCraftBookWithRecipients search for tRecipient " + tRecipient.InternalKey + " Craft with " + sItemGroupIngredient.ToString () + "  => hash " + tRecipeHashA + " or " + tRecipeHashB);

								foreach (NWDCraftBook tCraft in NWDCraftBook.GetAllObjects()) {
									if (tCraft.RecipeHash == tRecipeHashA || tCraft.RecipeHash == tRecipeHashB) {
										tReturnList.Add (tCraft);
										tReturnPrimary = tCraft;
										tItemsGroupUsed = sItemGroupIngredient;
										tReturnRecipient = tRecipient;
										break;
									}
								}
								if (tReturnPrimary != null) {
									break;
								}
							}
						}
					}
					if (tReturnPrimary != null) {
						break;
					}
				}

				// if I have a craftbook I change the Value of craft
				if (tReturnPrimary != null) {

					Debug.Log ("GetCraftBookWithRecipients CRAFT FOUND !!!!  " + tReturnPrimary.InternalKey);
					tRecipientsArray = new string[]{ tReturnRecipient.Reference };
					// Add Craftbook to ownership
					if (tReturnPrimary != null) {
						// Add to ownership  :-)
						if (tReturnPrimary.ItemToDescribe.GetObject () != null) {
						}
						NWDOwnership.SetItemToOwnership (tReturnPrimary.ItemToDescribe.GetObject (), 1);
					}
				}
				// I check all possibilities in rest of recipeint element by element (destructive mode?)
				foreach (string tRecipientValue in tRecipientsArray) {
					NWDRecipientGroup tRecipient = NWDRecipientGroup.GetObjectByReference (tRecipientValue);
					if (tRecipient.CraftUnUsedElements == true) {

						NWDReferencesArrayType<NWDItemGroup> tItemsGroupUnUsed = sItemGroupIngredientList [0];
						tItemsGroupUnUsed.RemoveReferencesArray (tItemsGroupUsed);
						foreach (string tItemReference in tItemsGroupUnUsed.GetReferences()) {
							Debug.Log ("GetCraftBookWithRecipients search modulo scrft for for tRecipient = " + tRecipient.InternalKey + " and item : " + tItemReference);
							bool tOrdered = true;
							string tAssemblyA = tOrdered.ToString () + tRecipient + tItemReference;
							tOrdered = false;
							string tAssemblyB = tOrdered.ToString () + tRecipient + tItemReference;
							string tRecipeHashA = BTBSecurityTools.GenerateSha (tAssemblyA, BTBSecurityShaTypeEnum.Sha1);
							string tRecipeHashB = BTBSecurityTools.GenerateSha (tAssemblyB, BTBSecurityShaTypeEnum.Sha1);
							foreach (NWDCraftBook tCraft in NWDCraftBook.GetAllObjects()) {
								if (tCraft.RecipeHash == tRecipeHashA || tCraft.RecipeHash == tRecipeHashB) {
									tReturnList.Add (tCraft);
									// Add Craftbook to ownership
									if (tCraft != null) {
										// Add to ownership  :-)
										if (tCraft.ItemToDescribe.GetObject () != null) {
										}
										NWDOwnership.SetItemToOwnership (tCraft.ItemToDescribe.GetObject (), 1);
									}
									break;
								}
							}
						}
					} else {
						// unused elements are destroyed
					}
				}
			}
			return tReturnList.ToArray ();
		}
		//-------------------------------------------------------------------------------------------------------------

		#endregion

		//-------------------------------------------------------------------------------------------------------------

        #region Instance methods

		//-------------------------------------------------------------------------------------------------------------
		public void GetItemsRequired ()
		{
			//ItemsOne
        }

        //-------------------------------------------------------------------------------------------------------------

        #endregion
		//-------------------------------------------------------------------------------------------------------------

		#region NetWorkedData addons methods

		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just after loaded from database.
		/// </summary>
		public override void AddonLoadedMe ()
		{
			// do something when object was loaded
			// TODO verif if method is call in good place in good timing
			NWDCraftBook.AddCraftBookInHashDictionary (this);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before unload from memory.
		/// </summary>
		public override void AddonUnloadMe ()
		{
			// do something when object will be unload
			// TODO verif if method is call in good place in good timing
			NWDCraftBook.RemoveCraftBookInHashDictionary (this);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before insert.
		/// </summary>
		public override void AddonInsertMe ()
		{
			// do something when object will be inserted
			NWDCraftBook.AddCraftBookInHashDictionary (this);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before update.
		/// </summary>
		public override void AddonUpdateMe ()
		{
			// do something when object will be updated
			//--------------
			#if UNITY_EDITOR
			//--------------
			// TODO recalculate all sign possibilities
			// I need test all possibilities .. I use an Hack : if ordered == false I sort by Name before

			if (RecipientGroup == null) {
				RecipientGroup = new NWDReferenceType<NWDRecipientGroup> ();
			}
			if (ItemGroupIngredient == null) {
				ItemGroupIngredient = new NWDReferencesArrayType<NWDItemGroup> ();
			}

			string tAssembly = "";
			if (OrderIsImportant == true) {
				tAssembly = OrderIsImportant.ToString () +
				RecipientGroup.ToString () +
				ItemGroupIngredient.ToString ();
			} else {
				tAssembly = OrderIsImportant.ToString () +
				RecipientGroup.ToString () +
				ItemGroupIngredient.ToStringSorted ();
			}
			RecipeHash = BTBSecurityTools.GenerateSha (tAssembly, BTBSecurityShaTypeEnum.Sha1);

			NWDDataManager.SharedInstance.RepaintWindowsInManager (this.GetType ());
			NWDDataInspector.ActiveRepaint ();
			//--------------
			#endif
			//--------------
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method when updated.
		/// </summary>
		public override void AddonUpdatedMe ()
		{
			// do something when object finish to be updated
			NWDCraftBook.UpdateCraftBookInHashDictionary (this);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method when updated me from Web.
		/// </summary>
        public override void AddonUpdatedMeFromWeb ()
		{
			// do something when object finish to be updated from CSV from WebService response
			// TODO verif if method is call in good place in good timing
			NWDCraftBook.UpdateCraftBookInHashDictionary (this);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before dupplicate.
		/// </summary>
		public override void AddonDuplicateMe ()
		{
			// do something when object will be dupplicate
			NWDCraftBook.AddCraftBookInHashDictionary (this);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before enable.
		/// </summary>
		public override void AddonEnableMe ()
		{
			// do something when object will be enabled
			NWDCraftBook.AddCraftBookInHashDictionary (this);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before disable.
		/// </summary>
		public override void AddonDisableMe ()
		{
			// do something when object will be disabled
			NWDCraftBook.RemoveCraftBookInHashDictionary (this);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before put in trash.
		/// </summary>
		public override void AddonTrashMe ()
		{
			// do something when object will be put in trash
			NWDCraftBook.RemoveCraftBookInHashDictionary (this);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addon method just before remove from trash.
		/// </summary>
		public override void AddonUnTrashMe ()
		{
			// do something when object will be remove from trash
			NWDCraftBook.AddCraftBookInHashDictionary (this);
        }
        //-------------------------------------------------------------------------------------------------------------

        #endregion
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		//Addons for Edition
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addons in edition state of object.
		/// </summary>
		/// <returns><c>true</c>, if object need to be update, <c>false</c> or not not to be update.</returns>
		/// <param name="sNeedBeUpdate">If set to <c>true</c> need be update in enter.</param>
		public override bool AddonEdited (bool sNeedBeUpdate)
		{
			if (sNeedBeUpdate == true) {
				// do something
			}
			return sNeedBeUpdate;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addons editor interface.
		/// </summary>
		/// <returns>The editor height addon.</returns>
		/// <param name="sInRect">S in rect.</param>
		public override float AddonEditor (Rect sInRect)
		{
			// Draw the interface addon for editor
			float tYadd = 0.0f;
			return tYadd;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Addons editor intreface expected height.
		/// </summary>
		/// <returns>The editor expected height.</returns>
		public override float AddonEditorHeight ()
		{
			// Height calculate for the interface addon for editor
			float tYadd = 0.0f;
			return tYadd;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif

		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================