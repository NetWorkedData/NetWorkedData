//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("SPT")]
	[NWDClassDescriptionAttribute ("Spent descriptions Class")]
	[NWDClassMenuNameAttribute ("Spent")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDSpent :NWDBasis <NWDSpent>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDReferenceType<NWDShop> ShopReference { get; set; }
		public NWDReferenceType<NWDRack> RackReference { get; set; }
		public NWDReferenceType<NWDPack> PackReference { get; set; }
		public int PackQuanity { get; set; }
		[Indexed ("AccountIndex", 0)]
		public NWDReferenceType<NWDAccount> AccountReference { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDSpent()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================