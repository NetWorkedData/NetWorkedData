// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:7
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================