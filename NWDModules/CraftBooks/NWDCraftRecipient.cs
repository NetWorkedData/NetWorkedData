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
        [NWDInspectorGroupStartAttribute("Description Item", true, true, true)] // ok
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDInspectorGroupEndAttribute]
        
        [NWDInspectorGroupStartAttribute("Usage", true, true, true)] // ok
        public bool CraftOnlyMax
        {
            get; set;
        }
        public bool CraftUnUsedElements
        {
            get; set;
        }
        [NWDInspectorGroupEndAttribute]
        
        [NWDInspectorGroupStartAttribute("FX (Special Effects)", true, true, true)]
        [NWDInspectorHeaderAttribute("Active Recipient")]
        public NWDPrefabType ActiveParticles
        {
            get; set;
        }
        public NWDPrefabType ActiveSound
        {
            get; set;
        }

        [NWDInspectorHeaderAttribute("Add Item")]
        public NWDPrefabType AddParticles
        {
            get; set;
        }
        public NWDPrefabType AddSound
        {
            get; set;
        }

        [NWDInspectorHeaderAttribute("Disactive Recipient")]
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
        [NWDInspectorGroupEndAttribute]
        
        [NWDInspectorGroupStartAttribute("Item(s) use as recipient", true, true, true)] // ok
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