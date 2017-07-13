using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("KWD")]
	[NWDClassDescriptionAttribute ("Keywords descriptions Class")]
	[NWDClassMenuNameAttribute ("Keywords")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDKeyword :NWDBasis<NWDKeyword>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDLocalizableStringType Name { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDKeyword()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================