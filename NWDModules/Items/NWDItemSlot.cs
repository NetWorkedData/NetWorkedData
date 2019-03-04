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
        public int Number
        {
            get; set;
        }
        public NWDReferenceType<NWDItemGroup> ItemGroup
        {
            get; set;
        }
        public NWDReferenceType<NWDItem> ItemNone
        {
            get; set;
        }
        public bool RemoveFromOwnership
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================