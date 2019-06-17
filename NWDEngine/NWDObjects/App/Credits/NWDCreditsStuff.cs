//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:30:3
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
    [NWDClassTrigrammeAttribute("CRS")]
    [NWDClassDescriptionAttribute("Credits Stuff")]
    [NWDClassMenuNameAttribute("Credits Stuff")]
    public partial class NWDCreditsStuff : NWDBasis<NWDCreditsStuff>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Stuff description")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        public NWDPrefabType Prefab
        {
            get; set;
        }
        [NWDInspectorGroupEnd()]
        
        [NWDInspectorGroupStart("Members")]
        public NWDPrefabType MemberPrefab
        {
            get; set;
        }
        public NWDReferencesListType<NWDCreditsMember> MemberList
        {
            get; set;
        }
        [NWDInspectorGroupEnd()]
        
        [NWDInspectorGroupStart("Show options")]
        public NWDColorType Tint
        {
            get; set;
        }
        public bool ShowPost
        {
            get; set;
        }
        public bool ShowCharacter
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================