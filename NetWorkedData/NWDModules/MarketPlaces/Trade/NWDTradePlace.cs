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
	[NWDClassTrigrammeAttribute ("TRW")]
	[NWDClassDescriptionAttribute ("Trade Place descriptions Class")]
	[NWDClassMenuNameAttribute ("Trade Place")]
	//-------------------------------------------------------------------------------------------------------------
	public partial class NWDTradePlace :NWDBasis <NWDTradePlace>
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
		public NWDReferencesListType<NWDKeyword>  FilterKeywords { get; set; }
		[NWDGroupEndAttribute]

		[NWDGroupStartAttribute("Money Authorization",true, true, true)]
		public NWDReferencesListType<NWDWorld> FilterMoneyWorlds { get; set; }
		public NWDReferencesListType<NWDCategory> FilterMoneyCategories { get; set; }
		public NWDReferencesListType<NWDFamily> FilterMoneyFamilies { get; set; }
		public NWDReferencesListType<NWDKeyword>  FilterMoneyKeywords { get; set; }
		[NWDGroupEndAttribute]


		public int OpenDateTime { get; set; }
		public int CloseDateTime { get; set; }
		public int Calendar { get; set; }

		[NWDEnumAttribute(new int[]{0,1,2}, new string[]{"marketplace", "BarterPlace"})]
		public int TypeOfTrade { get; set; }
		public NWDReferencesQuantityType<NWDItem> RequestCreationItemsCost { get; set; } // not resell if cancel
		public NWDReferencesQuantityType<NWDItem> TransactionFixItemsCost { get; set; } 
		public NWDReferencesQuantityType<NWDItem> TransactionNumberOfItemsCost { get; set; }
		public float NumberStep { get; set; }

		public int RequestLifeTime { get; set; }
		//-------------------------------------------------------------------------------------------------------------
		public NWDTradePlace()
		{
			//Init your instance here
			//DiscoverItYourSelf = true;
		}
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================