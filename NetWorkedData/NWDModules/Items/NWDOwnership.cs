using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("OWS")]
	[NWDClassDescriptionAttribute ("Ownership descriptions Class")]
	[NWDClassMenuNameAttribute ("Ownership")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDOwnership :NWDBasis <NWDOwnership>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		[Indexed ("AccountIndex", 0)]
		public NWDReferenceType<NWDAccount> AccountReference { get; set; }
		public NWDReferenceType<NWDItem> ItemReference { get; set; }
		[NWDIntSliderAttribute(0,250)]
		public int Quantity { get; set; }
		public string ValueA { get; set; }
		public string ValueB { get; set; }
		public string ValueC { get; set; }
		public string ValueD { get; set; }
		public string ValueE { get; set; }
		public string ValueF { get; set; }
		public string ValueG { get; set; }
		public string ValueH { get; set; }
		public string ValueI { get; set; }
		public string ValueJ { get; set; }
		public string Special { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDOwnership()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================