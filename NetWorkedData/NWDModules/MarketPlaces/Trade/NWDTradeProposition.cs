using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("TRP")]
	[NWDClassDescriptionAttribute ("Trade Proposition descriptions Class")]
	[NWDClassMenuNameAttribute ("Trade Proposition")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDTradeProposition :NWDBasis <NWDTradeProposition>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDReferenceType<NWDTradeRequest> TradeReference { get; set; }
		public NWDReferenceType<NWDAccount> AccountReference { get; set; }

		public NWDReferencesQuantityType<NWDItem> ItemsProposed { get; set; }

		public bool Accepted { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDTradeProposition ()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
			Accepted = false;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================