//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("LVS")]
	[NWDClassDescriptionAttribute ("Level's Score descriptions Class")]
	[NWDClassMenuNameAttribute ("Level's Score")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDLevelScore :NWDBasis <NWDLevelScore>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		[Indexed ("AccountIndex", 0)]
		public NWDReferenceType<NWDAccount> AccountReference { get; set; }
		public NWDReferenceType<NWDLevel> LevelReference { get; set; }

		public int NumberOfPlay { get; set; }
		public int NumberOfFinish { get; set; }
		public int NumberOfCancel { get; set; }
		public int NumberOfFailed { get; set; }

		public float BestScore { get; set; }
		public float MiddleScore { get; set; }

		public int NumberOfStars { get; set; }

		//[NWDDateTime]
		public int DateLastGame { get; set; }
		//[NWDDateTime]
		public int DateLastSuccess { get; set; }
		//[NWDDateTime]
		public int DateLastFailed { get; set; }

		public int Ranking { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDLevelScore()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================