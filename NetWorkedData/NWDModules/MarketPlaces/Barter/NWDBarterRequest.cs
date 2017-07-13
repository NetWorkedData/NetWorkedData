using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("BRR")]
	[NWDClassDescriptionAttribute ("Barter Request descriptions Class")]
	[NWDClassMenuNameAttribute ("Barter Request")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDBarterRequest :NWDBasis <NWDBarterRequest>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDReferenceType<NWDBarterPlace> BarterReference { get; set; }

		[Indexed ("AccountIndex", 0)]
		public NWDReferenceType<NWDAccount> AccountReference { get; set; }

		public NWDReferencesQuantityType<NWDItem> ItemsProposed { get; set; }

		public int LimitDateTime { get; set; }

		public bool Accepted { get; set; }

		public NWDReferenceType<NWDTradeRequest> PropositionReference { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDBarterRequest()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================