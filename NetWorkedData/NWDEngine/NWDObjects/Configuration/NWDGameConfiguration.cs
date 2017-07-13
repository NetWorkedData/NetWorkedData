using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("GCF")]
	[NWDClassDescriptionAttribute ("Game's Configurations descriptions Class")]
	[NWDClassMenuNameAttribute ("Game's Configurations")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDGameConfiguration : NWDBasis <NWDGameConfiguration>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDLocalizableStringType ValueString { get; set; }
		public int ValueInt { get; set; }
		public bool ValueBool { get; set; }
		public float ValueFloat { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDGameConfiguration ()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================