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
    [NWDClassTrigrammeAttribute("ITP")]
    [NWDClassDescriptionAttribute("Item Pack descriptions Class")]
    [NWDClassMenuNameAttribute("Item Pack")]
    public partial class NWDItemPack : NWDBasis<NWDItemPack>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Description Item", true, true, true)]
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Items in this Item Pack", true, true, true)]
        public NWDReferencesQuantityType<NWDItem> Items
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