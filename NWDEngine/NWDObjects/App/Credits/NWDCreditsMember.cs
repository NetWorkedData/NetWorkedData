// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:30:0
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
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("CRM")]
    [NWDClassDescriptionAttribute("Credits Member")]
    [NWDClassMenuNameAttribute("Credits Member")]
    public partial class NWDCreditsMember : NWDBasis<NWDCreditsMember>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Member Informations")]
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
        [NWDInspectorGroupEnd()]
        
        [NWDInspectorGroupStart("Member post")]
        public NWDLocalizableStringType Office
        {
            get; set;
        }
        [NWDInspectorGroupEnd()]
        
        [NWDInspectorGroupStart("Member Actor")]
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