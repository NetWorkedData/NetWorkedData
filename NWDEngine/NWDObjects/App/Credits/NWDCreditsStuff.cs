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
        [NWDGroupStart("Stuff description")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        public NWDPrefabType Prefab
        {
            get; set;
        }
        [NWDGroupEnd()]
        
        [NWDGroupStart("Members")]
        public NWDPrefabType MemberPrefab
        {
            get; set;
        }
        public NWDReferencesListType<NWDCreditsMember> MemberList
        {
            get; set;
        }
        [NWDGroupEnd()]
        
        [NWDGroupStart("Show options")]
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