//=====================================================================================================================
//
// ideMobi copyright 2019
// All rights reserved by ideMobi
//
// Read License-en or Licence-fr
//
//=====================================================================================================================
using System;
using System.Collections.Generic;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDItemSlot class. This class is use for (complete description here).
    /// </summary>
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("ITS")]
    [NWDClassDescriptionAttribute("Slot for item definition")]
    [NWDClassMenuNameAttribute("Item Slot")]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDItemSlot : NWDBasis<NWDItemSlot>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDIntSlider(K_NUMBER_MIN, K_NUMBER_MAX)]
        [NWDTooltips("The max number of item in this slot (fill by null or ItemNone)")]
        public int Number
        {
            get; set;
        }
        [NWDTooltips("The item group of item to used")]
        public NWDReferenceType<NWDItemGroup> ItemGroup
        {
            get; set;
        }
        [NWDTooltips("The item none to fill the array by default")]
        public NWDReferenceType<NWDItem> ItemNone
        {
            get; set;
        }
        [NWDTooltips("Items are remove and readd to the ownership when used/unused in slot")]
        public bool FromOwnership
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================