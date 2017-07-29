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
	[NWDClassTrigrammeAttribute ("ITX")]
	[NWDClassDescriptionAttribute ("Item Extension descriptions Class")]
	[NWDClassMenuNameAttribute ("Item Extension")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDItemExtension :NWDBasis <NWDItemExtension>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDReferenceType<NWDItem> ItemReference  { get; set; }
		public string NewValueA  { get; set; }
		public string NewValueB  { get; set; }
		public string NewValueC  { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDItemExtension()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================