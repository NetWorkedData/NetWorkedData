// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:9
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
	[NWDClassServerSynchronizeAttribute(true)]
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
    //    [NWDAlias("ForRelationshipOnly")]
        public bool ForRelationshipOnly
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Results", true, true, true)]
       // [NWDAlias("GuildRequestsList")]
        public NWDReferencesListType<NWDUserGuild> GuildRequestsList
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================