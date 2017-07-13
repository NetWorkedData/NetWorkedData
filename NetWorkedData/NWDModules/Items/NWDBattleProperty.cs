using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("BTP")]
	[NWDClassDescriptionAttribute ("Battle Properties descriptions Class")]
	[NWDClassMenuNameAttribute ("Battle Properties")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDBattleProperty :NWDBasis <NWDBattleProperty>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public bool isAttack { get; set; }
		public string AttackEffectParticule { get; set; }
		public string VictimEffectParticule { get; set; }
		public bool isDefense { get; set; }
		public string DefenseEffectParticule { get; set; }
		public string CounterEffectParticule { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDBattleProperty()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
