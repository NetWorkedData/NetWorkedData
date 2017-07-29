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
	[NWDClassTrigrammeAttribute ("IAP")]
	[NWDClassDescriptionAttribute ("In App Purchase descriptions Class")]
	[NWDClassMenuNameAttribute ("In App Purchase")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDInAppPack :NWDBasis <NWDInAppPack>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDLocalizableStringType Name { get; set; }

		public NWDLocalizableStringType SubName { get; set; }

		public NWDLocalizableStringType Description { get; set; }

		public string AppleID { get; set; }
		public string GoogleID { get; set; }
		public string UnityID { get; set; }
		public string SteamID { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDInAppPack()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================