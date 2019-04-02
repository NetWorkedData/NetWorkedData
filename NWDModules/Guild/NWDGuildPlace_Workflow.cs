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

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDGuildPlace : NWDBasis<NWDGuildPlace>
    {
        //-------------------------------------------------------------------------------------------------------------
        public const int K_Guild_PROPOSITIONS_PER_REQUEST_MIN = 1;
        public const int K_Guild_PROPOSITIONS_PER_REQUEST_MAX = 10;

        public const int K_Guild_PROPOSITIONS_PER_USER_MIN = 1;
        public const int K_Guild_PROPOSITIONS_PER_USER_MAX = 10;

        public const int K_Guild_REQUEST_MIN = 1;
        public const int K_Guild_REQUEST_MAX = 200;
        //-------------------------------------------------------------------------------------------------------------
        public NWDGuildPlace()
        {
            //Debug.Log("NWDGuildPlace Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDGuildPlace(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDGuildPlace Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            //RefreshDelay = 60 * 3;
            //CancelDelay = 60 * 5;
            //MaxRequestPerUser = K_Guild_REQUEST_MAX;
            //MaxPropositionsPerUser = K_Guild_PROPOSITIONS_PER_USER_MAX;
            //MaxPropositionsPerRequest = K_Guild_PROPOSITIONS_PER_REQUEST_MAX;
            //WaitingLifeTime = 60 * 60 * 1;
            //RequestLifeTime = 60 * 60 * 24;
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> {typeof(NWDGuildPlace), typeof(NWDUserGuild), typeof(NWDUserGuildSubcription) };
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================