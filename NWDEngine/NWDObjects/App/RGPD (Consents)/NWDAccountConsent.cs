//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("ACS")]
    [NWDClassDescriptionAttribute("Account Consent for RGPD")]
    [NWDClassMenuNameAttribute("Account Consent")]
    public partial class NWDAccountConsent : NWDBasis<NWDAccountConsent>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Account")]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Consent")]
        public NWDReferenceType<NWDConsent> Consent
        {
            get; set;
        }
        public NWDVersionType Version
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("User's choise")]
        public BTBSwitchState Authorization
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================