//=====================================================================================================================
//
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
    [NWDClassTrigrammeAttribute("GBR")]
    [NWDClassDescriptionAttribute("User Guild descriptions Class")]
    [NWDClassMenuNameAttribute("User Guild")]
    [NWDForceSecureDataAttribute]
    public partial class NWDUserGuild : NWDBasis<NWDUserGuild>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Guild Detail", true, true, true)]
        [Indexed("AccountIndex", 0)]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDReferenceType<NWDGameSave> GameSave
        {
            get; set;
        }
        [NWDAlias("GuildPlace")]
        public NWDReferenceType<NWDGuildPlace> GuildPlace
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Description", true, true, true)]
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        public NWDLocalizableTextType Name
        {
            get; set;
        }
        public NWDLocalizableLongTextType Description
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDIntSlider(NWDGuildPlace.K_Guild_REQUEST_MIN, NWDGuildPlace.K_Guild_REQUEST_MAX)]
        [NWDAlias("MaxMembers")]
        public int MaxMembers
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDAlias("SubscriptionCounter")]
        public int SubscriptionCounter
        {
            get; set;
        }
        [NWDNotEditable]
        [NWDAlias("Subscriptions")]
        public NWDReferencesListType<NWDUserGuildSubcription> Subscriptions
        {
            get; set;
        }
        [NWDAlias("SubscriptionsWaiting")]
        public NWDReferencesListType<NWDUserGuildSubcription> SubscriptionsWaiting
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================