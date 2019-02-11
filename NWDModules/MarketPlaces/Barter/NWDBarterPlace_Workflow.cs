﻿//=====================================================================================================================
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
            MaxRequestPerUser = K_BARTER_REQUEST_MAX;
            MaxPropositionsPerUser = K_BARTER_PROPOSITIONS_PER_USER_MAX;
            MaxPropositionsPerRequest = K_BARTER_PROPOSITIONS_PER_REQUEST_MAX;
            WaitingLifeTime = 60 * 60 * 1;
            RequestLifeTime = 60 * 60 * 24;
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDUserOwnership), typeof(NWDBarterPlace), typeof(NWDUserBarterRequest), typeof(NWDUserBarterProposition) };
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetLifeTime()
        {
            // Return value
            string rLifeTime = "";

            // Set Trade Life Time
            DateTime tToday = DateTime.UtcNow;
            DateTime tLifeTime = tToday.AddSeconds(RequestLifeTime);
            TimeSpan tSpan = tLifeTime.Subtract(tToday);
            if (tSpan.TotalSeconds > 0)
            {
                if (tSpan.Days > 0)
                {
                    rLifeTime = tSpan.Days + " day(s)";
                }
                else
                {
                    if (tSpan.Hours > 0)
                    {
                        rLifeTime = tSpan.Hours + "h ";
                    }
                    if (tSpan.Minutes > 0)
                    {
                        rLifeTime  += tSpan.Minutes + "m ";
                    }
                    if (tSpan.Seconds > 0)
                    {
                        rLifeTime += tSpan.Seconds + "s";
                    }
                }
            }

            return rLifeTime;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================