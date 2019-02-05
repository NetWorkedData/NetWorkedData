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
    //[Serializable]
    //public class NWDCraftBookAddConnection : NWDConnection<NWDCraftBookAdd>
    //{
    //}
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("CKA")]
    [NWDClassDescriptionAttribute("CraftBook Add")]
    [NWDClassMenuNameAttribute("CraftBook Add")]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDCraftBookAdd : NWDBasis<NWDCraftBookAdd>
    {
       //-------------------------------------------------------------------------------------------------------------
		public NWDReferencesConditionalType<NWDItem> ItemConditional {get; set;}
		public NWDReferencesQuantityType<NWDItem> ItemsRewards {get; set;}
		public int RewardsNumber {get; set;}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================