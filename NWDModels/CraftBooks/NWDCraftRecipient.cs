//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:47:46
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
    [Serializable]
    public class NWDCraftRecipientConnection : NWDConnection<NWDCraftRecipient>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("CFR")]
    [NWDClassDescriptionAttribute("Craft Recipient descriptions Class")]
    [NWDClassMenuNameAttribute("Craft Recipient")]
    public partial class NWDCraftRecipient : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Description Item", true, true, true)] // ok
        public NWDReferenceType<NWDItem> ItemDescription
        {
            get; set;
        }
        [NWDInspectorGroupEnd]
        
        [NWDInspectorGroupStart("Usage", true, true, true)] // ok
        public bool CraftOnlyMax
        {
            get; set;
        }
        public bool CraftUnUsedElements
        {
            get; set;
        }
        [NWDInspectorGroupEnd]
        
        [NWDInspectorGroupStart("FX (Special Effects)", true, true, true)]
        [NWDInspectorHeader("Active Recipient")]
        public NWDPrefabType ActiveParticles
        {
            get; set;
        }
        public NWDPrefabType ActiveSound
        {
            get; set;
        }

        [NWDInspectorHeader("Add Item")]
        public NWDPrefabType AddParticles
        {
            get; set;
        }
        public NWDPrefabType AddSound
        {
            get; set;
        }

        [NWDInspectorHeader("Disactive Recipient")]
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
        [NWDInspectorGroupEnd]
        
        [NWDInspectorGroupStart("Item(s) use as recipient", true, true, true)] // ok
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