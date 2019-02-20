//=====================================================================================================================
//NWDAccountRelationship
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using BasicToolBox;
using SQLite.Attribute;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("ARS")]
    [NWDClassDescriptionAttribute("Account Relationship descriptions Class")]
    [NWDClassMenuNameAttribute("Account Relationship")]
    [NWDForceSecureDataAttribute]
    public partial class NWDAccountRelationship : NWDBasis<NWDAccountRelationship>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("User Relationship Detail", true, true, true)]
        [Indexed("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDReferenceType<NWDRelationshipPlace> RelationPlace
        {
            get; set;
        }

        public NWDReferenceFreeType<NWDAccountRelationship> FriendUserRelationShip
        {
            get; set;
        }
        public NWDReferenceFreeType<NWDAccount> FriendAccount
        {
            get; set;
        }
        [NWDNotEditable]
        public NWDDateTimeUtcType FriendLastSynchronization
        {
            get; set;
        }

        [NWDGroupEnd]
       
        [NWDGroupStart("Relationship informations", true, true, true)]
        [NWDAlias("RelationshipStatus")]
        public NWDRelationshipStatus RelationshipStatus
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDAlias("RelationshipHash")]
        public string RelationshipHash
        {
            get; set;
        }
        [NWDAlias("RelationshipCode")]
        public string RelationshipCode
        {
            get; set;
        }
        [NWDAlias("LimitDayTime")]
        public NWDDateTimeUtcType LimitDayTime
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================