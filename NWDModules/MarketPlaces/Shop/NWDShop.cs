//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:29
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum BuyPackResult : int { 
        Unknow,
        Enable,
        Disable,
        NotFound,
        NotEnoughCurrency,
        NotEnoughPackToBuy,
        EnoughPackToBuy,
        CanBuy,
        MissingPayCost,
        Failed
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute (true)]
	[NWDClassTrigrammeAttribute ("SHP")]
	[NWDClassDescriptionAttribute ("Shop descriptions Class")]
	[NWDClassMenuNameAttribute ("Shop")]
	public partial class NWDShop :NWDBasis <NWDShop>
	{
		//-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Description Item", true, true, true)]
        public NWDReferenceType<NWDItem> ItemDescription { get; set; }
        [NWDInspectorGroupEnd]

		[NWDInspectorGroupStart("Racks",true, true, true)]
		public NWDReferencesListType<NWDRack> DailyRack { get; set; }
		public NWDReferencesListType<NWDRack> WeeklyRack { get; set; }
		public NWDReferencesListType<NWDRack> MonthlyRack { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Special Racks", true, true, true)]
        public NWDDateTimeType SpecialDateStart { get; set; }
        public NWDDateTimeType SpecialDateEnd { get; set; }
        public NWDReferencesListType<NWDRack> SpecialRack { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Classification", true, true, true)]
        public NWDReferencesListType<NWDWorld> Worlds { get; set; }
        public NWDReferencesListType<NWDCategory> Categories { get; set; }
        public NWDReferencesListType<NWDFamily> Families { get; set; }
        public NWDReferencesListType<NWDKeyword> Keywords { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        public delegate void BuyPackBlock(BuyPackResult result, NWDUserTransaction transaction);
        public BuyPackBlock BuyPackBlockDelegate;
		//-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================