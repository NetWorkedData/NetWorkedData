using System;

using UnityEngine;

using SQLite4Unity3d;

using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("PCK")]
	[NWDClassDescriptionAttribute ("Pack descriptions Class")]
	[NWDClassMenuNameAttribute ("Pack")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDPack :NWDBasis <NWDPack>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		// for example : pack of forest hunter 
		// referenceList : pack of 5 arrows; longbow
		[NWDHeaderAttribute("Informations")]
		public NWDLocalizableStringType Name { get; set; }
		public NWDLocalizableStringType SubName { get; set; }
		public NWDLocalizableStringType Description { get; set; }
		[NWDHeaderAttribute("Images")]
		public NWDTextureType NormalTexture { get; set; }
		public NWDTextureType SelectedTexture { get; set; }

		[NWDGroupStartAttribute("Classification",true, true, true)]
		public NWDReferencesListType<NWDWorld> Worlds { get; set; }
		public NWDReferencesListType<NWDCategory> Categories { get; set; }
		public NWDReferencesListType<NWDFamily> Families { get; set; }
		public NWDReferencesListType<NWDKeyword>  Keywords { get; set; }
		[NWDGroupEndAttribute]

		[NWDHeaderAttribute("Item Pack in this Pack")]
		public NWDReferencesQuantityType<NWDItemPack> ItemPackReference { get; set; }

		[NWDHeaderAttribute("Pay with these items")]
		// the itemPack reference to pay this  or if this null,
		public NWDReferencesQuantityType<NWDItem> ItemsToPay { get; set; }
		[NWDHeaderAttribute("Or pay with in app purchase Key")]
		// … the inAppPack to paid this pack with in app purchase
		public NWDReferenceType<NWDInAppPack> InAppPurchasePack { get; set; }
//
//		[NWDHeaderAttribute("Shop management")]
//		public int QuantityInShop { get; set; }
//		public int StartSellAtDate { get; set; }
//		public int DeliveryDelay { get; set; }
//		public int ReapprovisioningDelay { get; set; }
//		public int FinishSellAtDate { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDPack()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDItem[] GetAllItemsInPack ()
		{
			List<NWDItem> tlist = new List<NWDItem> ();
			foreach (NWDItemPack tItemPack in ItemPackReference.GetObjects ()) {
				tlist.AddRange (tItemPack.Items.GetObjects ());
			}
			return tlist.ToArray ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool UserHasEnoughMoney ()
		{
			bool rReturn = false;
			// check si on a assez d'items pour acheter
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================