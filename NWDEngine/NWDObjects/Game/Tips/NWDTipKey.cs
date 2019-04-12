// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:41:47
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
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
//=====================================================================================================================
namespace NetWorkedData
{ //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("TPK")]
    [NWDClassDescriptionAttribute("Tips And Tricks descriptions Class")]
    [NWDClassMenuNameAttribute("Tips And Tricks")]
    public partial class NWDTipKey : NWDBasis<NWDTipKey>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Classification", false, true, true)]
        public NWDReferencesListType<NWDWorld> WorldList
        {
            get; set;
        }
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
        [NWDInspectorGroupStart("Tips and Tricks", false, true, true)]
        [NWDCertified]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        [NWDCertified]
        public NWDLocalizableStringType SubTitle
        {
            get; set;
        }
        [NWDCertified]
        public NWDLocalizableTextType Message
        {
            get; set;
        }
        [NWDCertified]
        public NWDSpriteType Icon
        {
            get; set;
        }
        [NWDInspectorHeader("Items required to be visible")]
        [NWDCertified]
        public NWDReferencesConditionalType<NWDItem> ItemConditional
        {
            get; set;
        }
        [NWDInspectorHeader("Weighting")]
        [NWDIntSlider(1, 9)]
        [NWDCertified]
        public int Weighting
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================