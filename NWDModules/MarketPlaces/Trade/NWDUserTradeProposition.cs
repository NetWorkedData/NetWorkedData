//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using SQLite.Attribute;
using UnityEngine;
using BasicToolBox;
using System.Collections.Generic;
using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigramme("UTP")]
    [NWDClassDescription("User Trade Proposition descriptions Class")]
    [NWDClassMenuName("User Trade Proposition")]
    [NWDForceSecureDataAttribute]
    public partial class NWDUserTradeProposition : NWDBasis<NWDUserTradeProposition>
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
        [NWDAlias("TradePlace")]
        public NWDReferenceType<NWDTradePlace> TradePlace
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Trade References", true, true, true)]
        [NWDAlias("TradeRequest")]
        public NWDReferenceType<NWDUserTradeRequest> TradeRequest
        {
            get; set;
        }
        [NWDAlias("ItemsProposed")]
        public NWDReferencesQuantityType<NWDItem> ItemsProposed
        {
            get; set;
        }
        [NWDAlias("ItemsAsked")]
        public NWDReferencesQuantityType<NWDItem> ItemsAsked
        {
            get; set;
        }
        [NWDAlias("TradeStatus")]
        public NWDTradeStatus TradeStatus
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDAlias("TradeRequestHash")]
        public string TradeRequestHash
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public delegate void tradeProposalBlock(bool error, NWDTradeStatus status, NWDOperationResult result);
        public tradeProposalBlock tradeProposalBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        private NWDMessage Message;
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================