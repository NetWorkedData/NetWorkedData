//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using SQLite4Unity3d;
using BasicToolBox;
using SQLite.Attribute;
#if UNITY_EDITOR
using UnityEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("GBP")]
    [NWDClassDescriptionAttribute("User Guild Subcription descriptions Class")]
    [NWDClassMenuNameAttribute("User Guild Subcription")]
    [NWDForceSecureDataAttribute]
    public partial class NWDUserGuildSubcription : NWDBasis<NWDUserGuildSubcription>
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
       // [NWDAlias("GuildPlace")]
        public NWDReferenceType<NWDGuildPlace> GuildPlace
        {
            get; set;
        }
       // [NWDAlias("GuildRequest")]
        public NWDReferenceType<NWDUserGuild> GuildRequest
        {
            get; set;
        }
        [NWDInspectorGroupEnd]
       // [NWDAlias("GuildStatus")]
        public NWDGuildStatus GuildStatus
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================