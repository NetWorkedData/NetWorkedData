using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("CBK")]
	[NWDClassDescriptionAttribute ("Cook Recipes descriptions Class")]
	[NWDClassMenuNameAttribute ("Cook Recipes")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDCookRecipe :NWDBasis <NWDCookRecipe>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDLocalizableStringType Name { get; set; }

		public NWDLocalizableStringType SubName { get; set; }

		public NWDLocalizableStringType Description { get; set; }


		[NWDGroupStartAttribute("Classification", false , true ,true)]
		public NWDReferencesListType<NWDWorld> Worlds { get; set; }
		public NWDReferencesListType<NWDCategory> Categories { get; set; }
		public NWDReferencesListType<NWDFamily> Families { get; set; }
		public NWDReferencesListType<NWDKeyword>  Keywords { get; set; }
		[NWDGroupEndAttribute]

		[NWDHeaderAttribute("RECIPE CAN BE DISCOVER YOURSELF")]

		public bool DiscoverItYourSelf { get; set;}

		[NWDHeaderAttribute("ITEMS REQUIRED")]
		public NWDReferencesQuantityType<NWDItem> ItemsRequired{ get; set; }

		[NWDHeaderAttribute("RECIPE")]

		public bool OrderIsImportant { get; set; }
	
		public NWDReferenceType<NWDItem> ItemOne { get; set; }
		[NWDEnumAttribute(new int[]{0,1,2,3},new string[]{"0","1","2","3"})]
		public int DelayOne { get; set; }
		public NWDReferenceType<NWDItem> ItemTwo { get; set; }
		[NWDEnumAttribute(new int[]{0,1,2,3},new string[]{"0","1","2","3"})]
		public int DelayTwo { get; set; }
		public NWDReferenceType<NWDItem> ItemThree { get; set; }
		[NWDEnumAttribute(new int[]{0,1,2,3},new string[]{"0","1","2","3"})]
		public int DelayThree { get; set; }
		public NWDReferenceType<NWDItem> ItemFour { get; set; }
		[NWDEnumAttribute(new int[]{0,1,2,3},new string[]{"0","1","2","3"})]
		public int DelayFour { get; set; }
		public NWDReferenceType<NWDItem> ItemFive { get; set; }
		[NWDEnumAttribute(new int[]{0,1,2,3},new string[]{"0","1","2","3"})]
		public int DelayFive { get; set; }

		[NWDHeaderAttribute("RESULT")]
		public NWDReferencesQuantityType<NWDItem> Result { get; set; }

		public string ParticuleEffetGameObject { get; set; }

		[NWDHeaderAttribute("STAMP")]
		[NWDNotEditableAttribute]
		public string StampSearch { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDCookRecipe()
		{
			//Init your instance here
			DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
