//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:36
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
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("ITS")]
    [NWDClassDescriptionAttribute("Slot for item definition")]
    [NWDClassMenuNameAttribute("Item Slot")]
    public partial class NWDItemSlot : NWDBasis
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