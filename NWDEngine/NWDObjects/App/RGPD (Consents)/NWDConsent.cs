// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:34:0
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
using BasicToolBox;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDConsent class. This class is used to reccord the consent available in the game. 
    /// </summary>
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("CST")]
    [NWDClassDescriptionAttribute("NWDConsent class. This class is used to reccord the consent available in the game for RGPD")]
    [NWDClassMenuNameAttribute("Consent")]
    public partial class NWDConsent : NWDBasis<NWDConsent>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Consent description")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        public NWDLocalizableTextType Description
        {
            get; set;
        }
        public string LawReferences
        {
            get; set;
        }
        [NWDInspectorGroupEnd()]
        
        [NWDInspectorGroupStart("Consent version")]
        public string KeyOfConsent
        {
            get; set;
        }
        public NWDVersionType Version
        {
            get; set;
        }
        [NWDInspectorGroupEnd()]
        
        [NWDInspectorGroupStart("Consent default state proposition")]
        public BTBSwitchState DefaultState
        {
            get; set;
        }
        [NWDTooltips("Expected state to continue the game. If 'Unknow' any value is ok")]
        public BTBSwitchState ExpectedState
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================