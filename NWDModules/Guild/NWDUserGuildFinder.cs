//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;
using SQLite.Attribute;
using System;
using System.Collections.Generic;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[NWDClassServerSynchronize(true)]
    [NWDClassTrigramme("GBF")]
    [NWDClassDescription("User Guild Finder descriptions Class")]
    [NWDClassMenuName("User Guild Finder")]
    public partial class NWDUserGuildFinder : NWDBasis<NWDUserGuildFinder>
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
        public NWDReferenceType<NWDGuildPlace> GuildPlace
        {
            get; set;
        }
        [NWDAlias("ForRelationshipOnly")]
        public bool ForRelationshipOnly
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Results", true, true, true)]
        [NWDAlias("GuildRequestsList")]
        public NWDReferencesListType<NWDUserGuild> GuildRequestsList
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================