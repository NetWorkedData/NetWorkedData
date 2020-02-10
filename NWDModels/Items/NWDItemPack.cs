//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:49:19
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
    public partial class NWDItemPack : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Description Item", true, true, true)]
        public NWDReferenceType<NWDItem> ItemDescription
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Items in this Item Pack", true, true, true)]
        public NWDReferencesQuantityType<NWDItem> Items
        {
            get; set;
        }
        //[NWDInspectorGroupEnd]

        //[NWDInspectorGroupStart("Classification", true, true, true)]
#if NWD_MODULE_GAME
        public NWDReferencesListType<NWDWorld> Worlds
        {
            get; set;
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================