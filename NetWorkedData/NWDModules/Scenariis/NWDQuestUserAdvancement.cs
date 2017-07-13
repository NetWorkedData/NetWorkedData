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

namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("QUA")]
	[NWDClassDescriptionAttribute ("Quest User Advancement descriptions Class")]
	[NWDClassMenuNameAttribute ("Quest User Advancement")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDQuestUserAdvancement :NWDBasis <NWDQuestUserAdvancement>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public  NWDReferenceType<NWDAccount> AccountReference { get; set; }
		public  NWDReferenceType<NWDQuest> QuestReference { get; set; }
		public  NWDReferenceType<NWDDialog> DialogReference { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDQuestUserAdvancement()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================