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
        [NWDGroupStart("Your credits, your company")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        public NWDReferenceType<NWDCreditsCompany> Company
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Your stuff")]
        public NWDReferencesListType<NWDCreditsStuff> StuffList
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Associated companies")]
        public NWDReferencesListType<NWDCreditsCompany> CompanyList
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Legal Informations")]
        public NWDLocalizableTextType LegalFooter
        {
            get; set;
        }
        public NWDLocalizableTextType Copyright
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Options")]
        public float ScrollSpeed
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================