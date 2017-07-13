using System;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("PRF")]
	[NWDClassDescriptionAttribute ("User Preferences descriptions Class")]
	[NWDClassMenuNameAttribute ("User Preferences")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDPreferences :NWDBasis <NWDPreferences>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		[Indexed ("AccountIndex", 0)]
		public NWDReferenceType<NWDAccount> AccountReference { get; set; }
		public string Value { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDPreferences()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
