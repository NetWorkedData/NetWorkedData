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
	[NWDClassTrigrammeAttribute ("ITG")]
	[NWDClassDescriptionAttribute ("Item Group descriptions Class")]
	[NWDClassMenuNameAttribute ("Item Group")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDItemGroup :NWDBasis <NWDItemGroup>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		[NWDHeaderAttribute("Informations")]
		public NWDLocalizableStringType Name { get; set; }
		public NWDLocalizableStringType SubName { get; set; }
		public NWDLocalizableStringType Description { get; set; }
		[NWDFloatSliderAttribute(0.0F,1.0F)]
		public float Rarity { get; set; }

		[NWDHeaderAttribute("Images")]
		public NWDTextureType NormalTexture { get; set; }
		public NWDTextureType SelectedTexture { get; set; }

		[NWDHeaderAttribute("Color")]
		public NWDColorType ColorNormal { get; set; }
		public NWDColorType ColorSelected { get; set; }

		[NWDHeaderAttribute("Prefab")]
		public NWDPrefabType NormalPrefab { get; set; }
		public NWDPrefabType SelectedPrefab { get; set; }

		[NWDGroupStartAttribute("Classification",true, true, true)]
		public NWDReferencesListType<NWDWorld> Worlds { get; set; }
		public NWDReferencesListType<NWDCategory> Categories { get; set; }
		public NWDReferencesListType<NWDFamily> Families { get; set; }
		public NWDReferencesListType<NWDKeyword> Keywords { get; set; }

		//[NWDGroupEndAttribute]
		//-------------------------------------------------------------------------------------------------------------
		public NWDItemGroup()
		{
			//Init your instance here
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================