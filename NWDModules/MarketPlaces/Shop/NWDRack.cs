//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("RCK")]
    [NWDClassDescriptionAttribute("Rack descriptions Class")]
    [NWDClassMenuNameAttribute("Rack")]
    public partial class NWDRack : NWDBasis<NWDRack>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Description Item", true, true, true)]
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Packs in this Rack", true, true, true)]
        public NWDReferencesQuantityType<NWDPack> PackQuantity
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Classification", true, true, true)]
        public NWDReferencesListType<NWDWorld> Worlds
        {
            get; set;
        }
        public NWDReferencesListType<NWDCategory> Categories
        {
            get; set;
        }
        public NWDReferencesListType<NWDFamily> Families
        {
            get; set;
        }
        public NWDReferencesListType<NWDKeyword> Keywords
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================