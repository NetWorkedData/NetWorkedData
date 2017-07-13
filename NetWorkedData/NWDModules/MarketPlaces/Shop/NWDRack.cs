using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("RCK")]
	[NWDClassDescriptionAttribute ("Rack descriptions Class")]
	[NWDClassMenuNameAttribute ("Rack")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDRack :NWDBasis <NWDRack>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDLocalizableStringType Name { get; set; }
		public NWDLocalizableStringType SubName { get; set; }
		public NWDReferencesQuantityType<NWDPack> PackReference { get; set; }
		public bool Limited { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDRack()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
			Limited = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================