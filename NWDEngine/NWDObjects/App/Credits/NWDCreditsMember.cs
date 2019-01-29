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
    [NWDClassTrigrammeAttribute("CDM")]
    [NWDClassDescriptionAttribute("Credits Member")]
    [NWDClassMenuNameAttribute("Credits Member")]
    public partial class NWDCreditsMember : NWDBasis<NWDCreditsMember>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Member Informations")]
        public NWDLocalizableStringType Lastname
        {
            get; set;
        }
        public NWDLocalizableStringType Firstname
        {
            get; set;
        }
        public NWDLocalizableStringType Nickname
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Member post")]
        public NWDLocalizableStringType Office
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Member Actor")]
        public NWDReferencesListType<NWDCharacter> CharacterList
        {
            get; set;
        }
        public NWDLocalizableStringType Role
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================