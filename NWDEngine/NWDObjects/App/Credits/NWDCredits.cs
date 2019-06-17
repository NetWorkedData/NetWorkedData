//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:53
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

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
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("CRD")]
    [NWDClassDescriptionAttribute("Credits")]
    [NWDClassMenuNameAttribute("Credits")]
    public partial class NWDCredits : NWDBasis<NWDCredits>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Your credits, your company")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        public NWDReferenceType<NWDCreditsCompany> Company
        {
            get; set;
        }
        [NWDInspectorGroupEnd()]
        
        [NWDInspectorGroupStart("Your stuff")]
        public NWDReferencesListType<NWDCreditsStuff> StuffList
        {
            get; set;
        }
        [NWDInspectorGroupEnd()]
        
        [NWDInspectorGroupStart("Associated companies")]
        public NWDReferencesListType<NWDCreditsCompany> CompanyList
        {
            get; set;
        }
        [NWDInspectorGroupEnd()]
        
        [NWDInspectorGroupStart("Legal Informations")]
        public NWDLocalizableTextType LegalFooter
        {
            get; set;
        }
        public NWDLocalizableTextType Copyright
        {
            get; set;
        }
        [NWDInspectorGroupEnd()]
        
        [NWDInspectorGroupStart("Options")]
        public float ScrollSpeed
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================