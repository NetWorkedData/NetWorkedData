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
	[NWDClassTrigrammeAttribute ("ITP")]
	[NWDClassDescriptionAttribute ("Item Pack descriptions Class")]
	[NWDClassMenuNameAttribute ("Item Pack")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDItemPack :NWDBasis <NWDItemPack>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		// for example : pack of 5 arrows 
		// reference : arrow 
		// quantity : 5

		[NWDHeaderAttribute("Informations")]
		public NWDLocalizableStringType Name { get; set; }
		public NWDLocalizableStringType SubName { get; set; }
		public NWDLocalizableStringType Description { get; set; }

		[NWDHeaderAttribute("Images")]
		public NWDTextureType NormalTexture { get; set; }
		public NWDTextureType SelectedTexture { get; set; }

		[NWDGroupStartAttribute("Classification",true, true, true)]
		public NWDReferencesListType<NWDWorld> Worlds { get; set; }
		public NWDReferencesListType<NWDCategory> Categories { get; set; }
		public NWDReferencesListType<NWDFamily> Families { get; set; }
		public NWDReferencesListType<NWDKeyword>  Keywords { get; set; }
		[NWDGroupEndAttribute]

        [NWDHeaderAttribute("Items in this Item Pack")]
        public NWDReferencesQuantityType<NWDItem> Items { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDItemPack()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================