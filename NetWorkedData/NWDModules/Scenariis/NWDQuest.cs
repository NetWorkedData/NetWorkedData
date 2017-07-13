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
	[NWDClassTrigrammeAttribute ("QST")]
	[NWDClassDescriptionAttribute ("Quest descriptions Class")]
	[NWDClassMenuNameAttribute ("Quest")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDQuest :NWDBasis <NWDQuest>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		[NWDGroupStartAttribute("Start Dialog",true, true, true)]
		public NWDReferenceType<NWDDialog> DialogReference { get; set; }
		public bool Finish { get; set; }
		public bool Succeess { get; set; }
		public bool Fail { get; set; }
		public bool Unique { get; set; }
		public bool Permanent { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDQuest()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================