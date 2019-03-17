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
    [NWDClassTrigrammeAttribute("CAT")]
    [NWDClassDescriptionAttribute("This class is used to reccord the category available in the game")]
    [NWDClassMenuNameAttribute("Category")]
    public partial class NWDCategory : NWDBasis<NWDCategory>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStartAttribute("Description", true, true, true)]
        [NWDTooltips("The name of this Category")]
        public NWDLocalizableStringType Name
        {
            get; set;
        }
        [NWDTooltips("The description item. Usable to be ownershipped")]
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDInspectorGroupEndAttribute]

        [NWDInspectorGroupStartAttribute("Arrangement", true, true, true)]
        public NWDReferencesListType<NWDCategory> ParentCategoryList
        {
            get; set;
        }
        [NWDNotEditable]
        public NWDReferencesListType<NWDCategory> ChildrenCategoryList
        {
            get; set;
        }
        [NWDNotEditable]
        public NWDReferencesListType<NWDCategory> CascadeCategoryList
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
