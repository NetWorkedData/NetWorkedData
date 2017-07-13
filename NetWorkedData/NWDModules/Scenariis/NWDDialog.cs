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
	[NWDClassTrigrammeAttribute ("DLG")]
	[NWDClassDescriptionAttribute ("Dialog descriptions Class")]
	[NWDClassMenuNameAttribute ("Dialog")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDDialog :NWDBasis <NWDDialog>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		[NWDGroupStartAttribute("Reply from preview dialog (optional)",true, true, true)]
		public string Reply { get; set; }
		public int ReplyType { get; set; } // default, cancel, ok ...
		public NWDReferencesQuantityType<NWDItem> ItemsNecessary { get; set; }
		public NWDReferencesListType<NWDQuest> QuestsNecessary { get; set; }
		[NWDGroupEndAttribute]

		[NWDGroupStartAttribute("Character Dialog",true, true, true)]
		public string CharacterReference { get; set; }
		public string CharacterEmotion { get; set; }

		public NWDPrefabType CharacterAnimation { get; set; }

		public string Dialog { get; set; }
		[NWDGroupEndAttribute]

		[NWDGroupStartAttribute("List of next replies",true, true, true)]
		public NWDReferencesListType<NWDDialog> ListOfAnswer { get; set; }


		public bool StepOfQuest { get; set; }
		public bool EndOfQuest { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDDialog()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================