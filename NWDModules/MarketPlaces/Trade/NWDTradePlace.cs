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
    public enum NWDTradeStatus
    {
        None = 0,
        Submit = 1,
        Waiting = 2,
        Deal = 3,
        Accepted = 4,
        Expired = 6,
        Cancel = 8,
        Cancelled = 9,

        NoDeal = 20,

        Refresh = 40,

        Force = 99, // admin only 
        ForceNone = 999, // admin only 
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
	[Serializable]
    public class NWDTradePlaceConnection : NWDConnection<NWDTradePlace>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("TRW")]
    [NWDClassDescriptionAttribute("Trade Place descriptions Class")]
    [NWDClassMenuNameAttribute("Trade Place")]
    public partial class NWDTradePlace : NWDBasis<NWDTradePlace>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Description", true, true, true)]
        public NWDReferenceType<NWDItem> DescriptionItem
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Items Authorization", true, true, true)]
        public NWDReferencesListType<NWDWorld> FilterWorlds { get; set; }
        public NWDReferencesListType<NWDCategory> FilterCategories
        {
            get; set;
        }
        public NWDReferencesListType<NWDFamily> FilterFamilies
        {
            get; set;
        }
        public NWDReferencesListType<NWDKeyword> FilterKeywords
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        //[NWDGroupSeparator]

        [NWDInspectorGroupStart("Trade Detail", true, true, true)]
        [NWDAlias("ForRelationshipOnly")]
        public bool ForRelationshipOnly
        {
            get; set;
        }
        public NWDReferencesListType<NWDItem> Moneys
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDItem> RequestFixCost
        {
            get; set;
        }
        public NWDReferencesQuantityType<NWDItem> RequestPerItemCost
        {
            get; set;
        }
        public int MaxRequestPerUser
        {
            get; set;
        }
        [NWDIntSlider(10, 300)]
        public int RefreshDelay
        {
            get; set;
        }
        [NWDTooltips("Minimum time before cancel")]
        public int CancelDelay
        {
            get; set;
        }
        public int RequestLifeTime
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTradePlace()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDTradePlace(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
            RefreshDelay = 60 * 3;
            CancelDelay = 60 * 5;
            RequestLifeTime = 60 * 60 * 24;
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDUserOwnership), typeof(NWDTradePlace), typeof(NWDUserTradeRequest), typeof(NWDUserTradeProposition), typeof(NWDUserTradeFinder) };
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