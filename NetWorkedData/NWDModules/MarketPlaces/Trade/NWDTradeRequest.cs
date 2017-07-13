using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("TRR")]
	[NWDClassDescriptionAttribute ("Trade Request descriptions Class")]
	[NWDClassMenuNameAttribute ("Trade Request")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDTradeRequest :NWDBasis <NWDTradeRequest>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDReferenceType<NWDTradePlace> MarketPlaceReference { get; set; }

		[Indexed ("AccountIndex", 0)]
		public NWDReferenceType<NWDAccount> AccountReference { get; set; }

		public NWDReferencesQuantityType<NWDItem> ItemsProposed { get; set; }

		[NWDHeaderAttribute("For marketplace only")]
		public NWDReferencesQuantityType<NWDItem> ItemsWanted { get; set; }

		public int LimitDateTime { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDTradeRequest()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================