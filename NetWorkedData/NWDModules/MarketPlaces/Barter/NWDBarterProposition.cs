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
	[NWDClassTrigrammeAttribute ("BPR")]
	[NWDClassDescriptionAttribute ("Barter Proposition descriptions Class")]
	[NWDClassMenuNameAttribute ("Barter Proposition")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDBarterProposition :NWDBasis <NWDBarterProposition>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDReferenceType<NWDBarterRequest> BarterReference { get; set; }

		[Indexed ("AccountIndex", 0)]
		public NWDReferenceType<NWDAccount> AccountReference { get; set; }

		public NWDReferencesQuantityType<NWDItem> ItemsProposed { get; set; }

		public bool Accepted { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDBarterProposition ()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
			Accepted = false;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================