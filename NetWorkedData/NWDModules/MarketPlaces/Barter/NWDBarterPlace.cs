using System;

using UnityEngine;

using SQLite4Unity3d;

//=====================================================================================================================
namespace NetWorkedData
{
	//-------------------------------------------------------------------------------------------------------------
	[NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("BRW")]
	[NWDClassDescriptionAttribute ("Barter Place descriptions Class")]
	[NWDClassMenuNameAttribute ("Barter Place")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDBarterPlace :NWDBasis <NWDBarterPlace>
	{
		//-------------------------------------------------------------------------------------------------------------
		//public bool DiscoverItYourSelf { get; set; }
		public NWDLocalizableStringType Name { get; set; }
		public NWDLocalizableStringType SubName { get; set; }
		public NWDLocalizableStringType Description { get; set; }

		[NWDGroupStartAttribute("Classification",true, true, true)]
		public NWDReferencesListType<NWDWorld> Worlds { get; set; }
		public NWDReferencesListType<NWDCategory> Categories { get; set; }
		public NWDReferencesListType<NWDFamily> Families { get; set; }
		public NWDReferencesListType<NWDKeyword>  Keywords { get; set; }
		[NWDGroupEndAttribute]

		[NWDGroupStartAttribute("Items Authorization",true, true, true)]
		public NWDReferencesListType<NWDWorld> FilterWorlds { get; set; }
		public NWDReferencesListType<NWDCategory> FilterCategories { get; set; }
		public NWDReferencesListType<NWDFamily> FilterFamilies { get; set; }
		public NWDReferencesListType<NWDKeyword> FilterKeywords { get; set; }
		[NWDGroupEndAttribute]

		public int OpenDateTime { get; set; }
		public int CloseDateTime { get; set; }
		public int WeeklyOpenDays { get; set; }

		public string Calendar { get; set; }

		public NWDReferencesQuantityType<NWDItem> RequestCreationItemsCost { get; set; } // not resell if cancel
		public NWDReferencesQuantityType<NWDItem> TransactionFixItemsCost { get; set; } 
		public NWDReferencesQuantityType<NWDItem> TransactionNumberOfItemsCost { get; set; }

		public float NumberStep { get; set; }

		public int RequestLifeTime { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDBarterPlace()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================