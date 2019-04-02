//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("CFW")]
    [NWDClassDescriptionAttribute("Craft Reward")]
    [NWDClassMenuNameAttribute("Craft Reward")]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDCraftReward : NWDBasis<NWDCraftReward>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Reward Batch and Quantity", true, true, true)]
        [NWDTooltips("Item conditional to win this reward")]
        public NWDReferencesConditionalType<NWDItem> ItemConditional
        {
            get; set;
        }
        [NWDTooltips("Item and quantity in the Bacth for random reward")]
        public NWDReferencesQuantityType<NWDItem> ItemBatch
        {
            get; set;
        }
        [NWDTooltips("Quantity to get randomly from Batch")]
        public int Quantity
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================