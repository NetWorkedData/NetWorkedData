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
	[NWDClassTrigrammeAttribute ("TRS")]
	[NWDClassDescriptionAttribute ("Transaction descriptions Class")]
	[NWDClassMenuNameAttribute ("Transaction")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDTransaction :NWDBasis <NWDTransaction>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDReferenceType<NWDPack>  PackReference { get; set; }
		[Indexed ("AccountIndex", 0)]
		public NWDReferenceType<NWDAccount>  AccountReference { get; set; }
		public NWDReferenceType<NWDInAppPack>  InAppReference { get; set; }
		public string Platform { get; set; }
		public string InAppTransaction { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDTransaction()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================