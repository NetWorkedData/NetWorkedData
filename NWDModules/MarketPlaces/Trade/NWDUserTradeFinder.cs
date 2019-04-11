//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using SQLite.Attribute;
using System;
using System.Collections.Generic;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigramme("UTF")]
    [NWDClassDescription("User Trade Finder descriptions Class")]
    [NWDClassMenuName("User Trade Finder")]
    public partial class NWDUserTradeFinder : NWDBasis<NWDUserTradeFinder>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Trade Detail", true, true, true)]
        [Indexed("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDReferenceType<NWDGameSave> GameSave
        {
            get; set;
        }
        public NWDReferenceType<NWDTradePlace> TradePlace
        {
            get; set;
        }
        //[NWDAlias("ForRelationshipOnly")]
        public bool ForRelationshipOnly
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Filters", true, true, true)]
        public NWDReferencesListType<NWDItem> FilterItems
        {
            get; set;
        }
        public NWDReferencesListType<NWDWorld> FilterWorlds
        {
            get; set;
        }
        public NWDReferencesListType<NWDCategory> FilterCategories
        {
            get; set;
        }
        public NWDReferencesListType<NWDFamily> FilterFamilies
        {
            get; set;
        }
        public NWDReferencesListType<NWDKeyword> FilterKeywords
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Results", true, true, true)]
        //[NWDAlias("TradeRequestsList")]
        public NWDReferencesListType<NWDUserTradeRequest> TradeRequestsList
        {
            get; set;
        }
        //[NWDGroupEnd]
        //-------------------------------------------------------------------------------------------------------------
        public delegate void tradeFinderBlock(bool error, NWDOperationResult result);
        public tradeFinderBlock tradeFinderBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================