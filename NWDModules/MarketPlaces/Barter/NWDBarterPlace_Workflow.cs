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
    public partial class NWDBarterPlace : NWDBasis<NWDBarterPlace>
    {
        //-------------------------------------------------------------------------------------------------------------
        public const int K_BARTER_PROPOSITIONS_PER_REQUEST_MIN = 1;
        public const int K_BARTER_PROPOSITIONS_PER_REQUEST_MAX = 10;

        public const int K_BARTER_PROPOSITIONS_PER_USER_MIN = 1;
        public const int K_BARTER_PROPOSITIONS_PER_USER_MAX = 10;

        public const int K_BARTER_REQUEST_MIN = 1;
        public const int K_BARTER_REQUEST_MAX = 5;
        //-------------------------------------------------------------------------------------------------------------
        public NWDBarterPlace()
        {
            //Debug.Log("NWDBarterPlace Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDBarterPlace(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDBarterPlace Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            RefreshDelay = 60 * 3;
            CancelDelay = 60 * 5;
            MaxRequestPerUser = 3;
            MaxPropositionsPerUser = 20;
            MaxPropositionsPerRequest = 10;
            WaitingLifeTime = 60 * 60 * 1;
            RequestLifeTime = 60 * 60 * 24;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================