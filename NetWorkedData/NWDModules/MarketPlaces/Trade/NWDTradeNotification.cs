using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("TRN")]
	[NWDClassDescriptionAttribute ("Trade Notification descriptions Class")]
	[NWDClassMenuNameAttribute ("Trade Notification")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDTradeNotification :NWDBasis <NWDTradeNotification>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		[Indexed ("AccountIndex", 0)]
		public NWDReferenceType<NWDAccount> AccountReference { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDTradeNotification()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================