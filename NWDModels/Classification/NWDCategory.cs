﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:34:17
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
    public partial class NWDCharacter : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupOrder("Classification", 9)]
        public NWDReferencesListType<NWDCategory> Categories { get; set; }
        [NWDInspectorGroupOrder("Classification", 9)]
        public NWDReferencesListType<NWDFamily> Families { get; set; }
        [NWDInspectorGroupOrder("Classification", 9)]
        public NWDReferencesListType<NWDKeyword> Keywords { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDItemPack : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupOrder("Classification", 9)]
        public NWDReferencesListType<NWDCategory> Categories { get; set; }
        [NWDInspectorGroupOrder("Classification", 9)]
        public NWDReferencesListType<NWDFamily> Families { get; set; }
        [NWDInspectorGroupOrder("Classification", 9)]
        public NWDReferencesListType<NWDKeyword> Keywords { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDPack : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupOrder("Classification", 9)]
        public NWDReferencesListType<NWDCategory> Categories { get; set; }
        [NWDInspectorGroupOrder("Classification", 9)]
        public NWDReferencesListType<NWDFamily> Families { get; set; }
        [NWDInspectorGroupOrder("Classification", 9)]
        public NWDReferencesListType<NWDKeyword> Keywords { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDParameter : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupOrder("Informations",9)]
        public NWDReferencesListType<NWDCategory> CategoryList
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("CAT")]
    [NWDClassDescriptionAttribute("This class is used to reccord the category available in the game")]
    [NWDClassMenuNameAttribute("Category")]
    public partial class NWDCategory : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Description", true, true, true)]
        [NWDTooltips("The name of this Category")]
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

        [NWDInspectorGroupStart("Arrangement", true, true, true)]
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
