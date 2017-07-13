using System;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("CAT")]
	[NWDClassDescriptionAttribute ("Categories descriptions Class")]
	[NWDClassMenuNameAttribute ("Categories")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDCategory :NWDBasis <NWDCategory>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDLocalizableStringType Name { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDCategory()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================