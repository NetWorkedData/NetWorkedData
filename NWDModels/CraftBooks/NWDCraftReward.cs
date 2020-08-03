//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#endif
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