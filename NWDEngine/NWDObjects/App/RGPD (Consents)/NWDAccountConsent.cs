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
        [NWDInspectorGroupStart("Account")]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        [NWDInspectorGroupEnd()]
        
        [NWDInspectorGroupStart("Consent")]
        public NWDReferenceType<NWDConsent> Consent
        {
            get; set;
        }
        public NWDVersionType Version
        {
            get; set;
        }
        [NWDInspectorGroupEnd()]
        
        [NWDInspectorGroupStart("User's choise")]
        public BTBSwitchState Authorization
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================