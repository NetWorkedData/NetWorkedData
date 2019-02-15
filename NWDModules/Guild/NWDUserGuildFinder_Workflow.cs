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
    public partial class NWDUserGuildFinder : NWDBasis<NWDUserGuildFinder>
    {
        //-------------------------------------------------------------------------------------------------------------
        public delegate void GuildFinderBlock(bool result, NWDOperationResult infos);
        public GuildFinderBlock GuildFinderBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserGuildFinder()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserGuildFinder(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDUserGuildFinder), typeof(NWDGuildPlace), typeof(NWDUserGuild), typeof(NWDUserGuildSubcription) };
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================