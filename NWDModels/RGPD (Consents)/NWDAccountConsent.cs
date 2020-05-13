//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:33:57
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================


//using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("ACS")]
    [NWDClassDescriptionAttribute("Account Consent for RGPD")]
    [NWDClassMenuNameAttribute("Account Consent")]
    public partial class NWDAccountConsent : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
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
        public NWESwitchState Authorization
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================