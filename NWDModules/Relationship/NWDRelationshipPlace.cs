//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDRelationshipStatus
    {
        None = 0,
        GenerateCode = 1,
        WaitingFriend = 2, // no sent
        InsertCode = 10,
        WaitingValidation = 11, // no sent
        ProposeFriend = 20, // no sent
        RefuseFriend = 21,
        AcceptFriend = 22,
        Valid = 30, // no sent
        Sync = 40,
        Delete = 80,
        CodeInvalid =87, // no sent
        AllreadyFriend = 88, // no sent
        Expired = 89, // no sent


        Force = 99, // admin only 
        ForceNone = 999, // admin only 
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("RSP")]
    [NWDClassDescriptionAttribute("RelationshipPlaces Class")]
    [NWDClassMenuNameAttribute("RelationshipPlace")]
    public partial class NWDRelationshipPlace : NWDBasis<NWDRelationshipPlace>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Description", true, true, true)]
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDGroupEnd]
       
        [NWDGroupStart("Classes Shared in relationship", true, true, true)]
        [NWDTooltips("Classes Shared")]
        [NWDAlias("ClassesSharedToStartRelation")]
        public NWDAccountClassesListType ClassesSharedToStartRelation
        {
            get; set;
        }
        [NWDAlias("ClassesShared")]
        public NWDAccountClassesListType ClassesShared
        {
            get; set;
        }
        [NWDIntSlider(K_CODE_LENGHT_MIN, K_CODE_LENGHT_MAX)]
        [NWDAlias("CodeLenght")]
        public int CodeLenght
        {
            get; set;
        }
        [NWDIntSlider(K_EXPIRE_TIME_MIN, K_EXPIRE_TIME_MAX)]
        [NWDAlias("ExpireTime")]
        public int ExpireTime
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================