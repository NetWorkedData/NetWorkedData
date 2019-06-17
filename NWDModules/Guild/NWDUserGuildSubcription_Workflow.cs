//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:48:20
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

//=====================================================================================================================
//
// ideMobi copyright 2019
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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserGuildSubcription : NWDBasis<NWDUserGuildSubcription>
    {
        //-------------------------------------------------------------------------------------------------------------
        public delegate void GuildProposalBlock(bool result, NWDTradeStatus status, NWDOperationResult infos);
        public GuildProposalBlock GuildProposalBlockDelegate;
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserGuildSubcription()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserGuildSubcription(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================