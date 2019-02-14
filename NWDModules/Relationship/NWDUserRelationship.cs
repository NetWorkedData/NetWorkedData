//=====================================================================================================================
//NWDUserRelationship
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
    [NWDClassTrigrammeAttribute("URS")]
    [NWDClassDescriptionAttribute("User Relationship descriptions Class")]
    [NWDClassMenuNameAttribute("User Relationship")]
    [NWDForceSecureDataAttribute]
    public partial class NWDUserRelationship : NWDBasis<NWDUserRelationship>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("User Relationship Detail", true, true, true)]
        [Indexed("AccountIndex", 0)]
        [NWDAlias("Account")]
        public NWDReferenceType<NWDAccount> Account  {  get; set;
        }
        [NWDAlias("GameSave")]
        public NWDReferenceType<NWDGameSave> GameSave  {  get; set;}
        [NWDAlias("RelationPlace")]
        public NWDReferenceType<NWDRelationshipPlace> RelationPlace  {  get; set;   }
        [NWDAlias("FriendUserRelationShip")]
        public NWDReferenceFreeType<NWDUserRelationship> FriendUserRelationShip  { get; set;
        }
        [NWDAlias("FriendAccount")]
        public NWDReferenceFreeType<NWDAccount> FriendAccount  {   get; set;
        }
        [NWDAlias("FriendGameSave")]
        public NWDReferenceFreeType<NWDGameSave> FriendGameSave  {   get; set; }
        [NWDNotEditable]
        [NWDAlias("FriendLastSynchronization")]
        public NWDDateTimeUtcType FriendLastSynchronization  {  get; set; }
        [NWDGroupEnd]
       
        [NWDGroupStart("Relationship informations", true, true, true)]
        [NWDAlias("RelationshipStatus")]
        public NWDRelationshipStatus RelationshipStatus { get; set; }
        [NWDNotEditable]
        [NWDAlias("RelationshipHash")]
        public string RelationshipHash { get; set;  }
        [NWDAlias("RelationshipCode")]
        public string RelationshipCode { get; set; }
        [NWDAlias("LimitDayTime")]
        public NWDDateTimeUtcType LimitDayTime {get; set;}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================