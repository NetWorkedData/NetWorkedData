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
    public partial class NWDUserGuild : NWDBasis<NWDUserGuild>
    {
        //-------------------------------------------------------------------------------------------------------------
        public delegate void GuildRequestBlock(bool result, NWDTradeStatus status, NWDOperationResult infos);
        public GuildRequestBlock GuildRequestBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserGuild()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserGuild(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            //ForRelationshipOnly = false;
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDGuildPlace), typeof(NWDUserGuild), typeof(NWDUserGuildSubcription) };
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================