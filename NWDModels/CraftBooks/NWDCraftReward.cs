//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:47:50
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("CFW")]
    [NWDClassDescriptionAttribute("Craft Reward")]
    [NWDClassMenuNameAttribute("Craft Reward")]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDCraftReward : NWDBasis
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