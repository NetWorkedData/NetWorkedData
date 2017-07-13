using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("LVL")]
	[NWDClassDescriptionAttribute ("Level descriptions Class")]
	[NWDClassMenuNameAttribute ("Level")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDLevel :NWDBasis <NWDLevel>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public int LocalIdentifier { get; set; }

		public int Number { get; set; }

		public string JSON { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDLevel()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================