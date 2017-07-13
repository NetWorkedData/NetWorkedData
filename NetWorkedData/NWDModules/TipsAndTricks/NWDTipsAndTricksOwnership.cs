using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("TTO")]
	[NWDClassDescriptionAttribute ("Tips And Tricks Ownership descriptions Class")]
	[NWDClassMenuNameAttribute ("Tips And Tricks Ownership")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDTipsAndTricksOwnership :NWDBasis <NWDTipsAndTricksOwnership>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		[Indexed ("AccountIndex", 0)]
		public NWDReferenceType<NWDAccount> AccountReference { get; set;}
		public NWDReferenceType<NWDTipsAndTricks> TipsAndTricksReference { get; set;}
		public bool IsActive { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDTipsAndTricksOwnership()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
			IsActive = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================