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
    public class NWDCraftRecipientConnection : NWDConnection<NWDCraftRecipient>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("CFR")]
    [NWDClassDescriptionAttribute("Craft Recipient descriptions Class")]
    [NWDClassMenuNameAttribute("Craft Recipient")]
    public partial class NWDCraftRecipient : NWDBasis<NWDCraftRecipient>
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
        //public NWDReferencesListType<NWDItem> ItemList
        //{
        //    get; set;
        //}
        public NWDReferenceType<NWDItemGroup> ItemGroup
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================