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
	[NWDClassTrigrammeAttribute ("USG")]
	[NWDClassDescriptionAttribute ("Usage descriptions Class")]
	[NWDClassMenuNameAttribute ("Usage")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDUsage :NWDBasis <NWDUsage>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		[Indexed ("AccountIndex", 0)]
		public NWDReferenceType<NWDAccount> AccountReference { get; set; }
		public NWDReferencesQuantityType<NWDItem> ItemsUsed { get; set; }
		public NWDReferencesQuantityType<NWDItem> ItemsGot { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDUsage()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================