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
    [Serializable]
    public class NWDRecipientGroupConnection : NWDConnection<NWDRecipientGroup>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("RCG")]
    [NWDClassDescriptionAttribute("Recipient group descriptions Class")]
    [NWDClassMenuNameAttribute("Recipient Group")]
    public partial class NWDRecipientGroup : NWDBasis<NWDRecipientGroup>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStartAttribute("Description Item", true, true, true)] // ok
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        
        [NWDGroupStartAttribute("Usage", true, true, true)] // ok
        public bool CraftOnlyMax
        {
            get; set;
        }
        public bool CraftUnUsedElements
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        
        [NWDGroupStartAttribute("FX (Special Effects)", true, true, true)]
        [NWDHeaderAttribute("Active Recipient")]
        public NWDPrefabType ActiveParticles
        {
            get; set;
        }
        public NWDPrefabType ActiveSound
        {
            get; set;
        }

        [NWDHeaderAttribute("Add Item")]
        public NWDPrefabType AddParticles
        {
            get; set;
        }
        public NWDPrefabType AddSound
        {
            get; set;
        }

        [NWDHeaderAttribute("Disactive Recipient")]
        public NWDPrefabType DisactiveParticles
        {
            get; set;
        }
        public NWDPrefabType DisactiveSound
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDItem> ItemFailedResult
        {
            get; set;
        }
        [NWDGroupEndAttribute]
        
        [NWDGroupStartAttribute("Item(s) use as recipient", true, true, true)] // ok
        public NWDReferencesListType<NWDItem> ItemList
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================