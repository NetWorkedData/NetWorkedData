// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date        2019-4-12 18:34:55
//  Author        Kortex (Jean-François CONTART) 
//  Email        jfcontart@idemobi.com
//  Project     NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("AREA")]
    [NWDClassDescriptionAttribute("This class is used to reccord the world/univers/island available in the game")]
    [NWDClassMenuNameAttribute("Area")]
    public partial class NWDArea : NWDBasis<NWDArea>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Informations", true, true, true)]
        [NWDTooltips("The name of this world")]
        public NWDLocalizableStringType Name
        {
            get; set;
        }
        [NWDTooltips("The description item. Usable to be ownershipped")]
        public NWDReferenceType<NWDItem> ItemDescription
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Classification", true, true, true)]
        public NWDReferencesListType<NWDCategory> CategoryList
        {
            get; set;
        }
        public NWDReferencesListType<NWDFamily> FamilyList
        {
            get; set;
        }
        public NWDReferencesListType<NWDKeyword> KeywordList
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Universe Arrangement", true, true, true)]
        public NWDReferencesListType<NWDArea> ParentAreaList
        {
            get; set;
        }
        [NWDNotEditable]
        public NWDReferencesListType<NWDArea> ChildAreaList
        {
            get; set;
        }
        [NWDNotEditable]
        public NWDReferencesListType<NWDArea> CascadeAreaList
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
