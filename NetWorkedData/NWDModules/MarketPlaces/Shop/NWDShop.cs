using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("SHP")]
	[NWDClassDescriptionAttribute ("Shop descriptions Class")]
	[NWDClassMenuNameAttribute ("Shop")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDShop :NWDBasis <NWDShop>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDLocalizableStringType Name { get; set; }
		public NWDLocalizableStringType SubName { get; set; }
		public NWDLocalizableStringType Description { get; set; }
		public NWDSpriteType icon { get; set;}

//		[NWDGroupStartAttribute("Classification",true, true, true)]
//		public NWDReferencesListType<NWDWorld> Worlds { get; set; }
//		public NWDReferencesListType<NWDCategory> Categories { get; set; }
//		public NWDReferencesListType<NWDFamily> Families { get; set; }
//		public NWDReferencesListType<NWDKeyword>  Keywords { get; set; }
//		[NWDGroupEndAttribute]
//
//		[NWDGroupStartAttribute("Items Authorization",true, true, true)]
//		public NWDReferencesListType<NWDWorld> FilterWorlds { get; set; }
//		public NWDReferencesListType<NWDCategory> FilterCategories { get; set; }
//		public NWDReferencesListType<NWDFamily> FilterFamilies { get; set; }
//		public NWDReferencesListType<NWDKeyword>  FilterKeywords { get; set; }
//		[NWDGroupEndAttribute]
//
		[NWDGroupStartAttribute("Opening",true, true, true)]
		public int OpenDateTime { get; set; }
		public int CloseDateTime { get; set; }
		public int Calendar { get; set; }
		[NWDGroupEndAttribute]

		[NWDGroupStartAttribute("Racks",true, true, true)]
		public int RequestLifeTime { get; set; }
		public NWDReferencesListType<NWDRack> DailyRack { get; set; }
		public NWDReferencesListType<NWDRack> WeeklyRack { get; set; }
		public NWDReferencesListType<NWDRack> MonthlyRack { get; set; }
		//[NWDGroupEndAttribute]
		//-------------------------------------------------------------------------------------------------------------
		public NWDShop()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================