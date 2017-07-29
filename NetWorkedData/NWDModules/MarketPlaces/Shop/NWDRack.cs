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
	[NWDClassTrigrammeAttribute ("RCK")]
	[NWDClassDescriptionAttribute ("Rack descriptions Class")]
	[NWDClassMenuNameAttribute ("Rack")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDRack :NWDBasis <NWDRack>
	{
		//-------------------------------------------------------------------------------------------------------------
		[NWDHeaderAttribute("Informations")]
		public NWDLocalizableStringType Name { get; set; }
		public NWDLocalizableStringType SubName { get; set; }
		public NWDLocalizableStringType Description { get; set; }
		[NWDHeaderAttribute("Images")]
		public NWDTextureType NormalTexture { get; set; }
		public NWDTextureType SelectedTexture { get; set; }
		[NWDHeaderAttribute("Packs in this Rack")]
		public NWDReferencesQuantityType<NWDPack> PackReference { get; set; }
		[NWDHeaderAttribute("Rack limited by Pack's quantities")]
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