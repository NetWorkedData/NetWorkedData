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

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("STK")]
    [NWDClassDescriptionAttribute("Stat Key")]
    [NWDClassMenuNameAttribute("Stat Key")]
    public partial class NWDStatisticKey : NWDBasis<NWDStatisticKey>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Information")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        public NWDLocalizableTextType Description
        {
            get; set;
        }
        public NWDGameDomain Domain
        {
            get; set;
        }
        public NWDStatKeyFormat Format
        {
            get; set;
        }
        public bool UnityStat
        {
            get; set;
        }
        [NWDGroupEnd()]
        
        [NWDGroupStart("Use parent stats")]
        public NWDReferenceType<NWDStatisticKey> Parent
        {
            get; set;
        }
        public NWDReferencesListType<NWDStatisticKey> Dependent
        {
            get; set;
        }

        [NWDGroupEnd()]
        
        [NWDGroupStart("Counter format (#x# is the value)")]
        public float InitCounter
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Counter #x# time'")]
        public NWDLocalizableStringType NoCounterFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Counter #x# time'")]
        public NWDLocalizableStringType SingleCounterFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Counter #x# time'")]
        public NWDLocalizableStringType PluralCounterFormat
        {
            get; set;
        }
        [NWDGroupEnd()]
        
        [NWDGroupStart("Total format (#x# is the value)")]
        public float InitTotal
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Total #x# seconds per bit'")]
        public NWDLocalizableStringType NoTotalFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Total #x# seconds per bit'")]
        public NWDLocalizableStringType SingleTotalFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Total #x# seconds per bit'")]
        public NWDLocalizableStringType PluralTotalFormat
        {
            get; set;
        }
        [NWDGroupEnd()]
        
        [NWDGroupStart("Average format (#x# is the value)")]
        public float InitAverage
        {
            get; set;
        }
        public bool ShowAverageAsPurcent
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Average #x# seconds per bit'")]
        public NWDLocalizableStringType NoAverageFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Average #x# seconds per bit'")]
        public NWDLocalizableStringType SingleAverageFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Average #x# seconds per bit'")]
        public NWDLocalizableStringType PluralAverageFormat
        {
            get; set;
        }
        [NWDGroupEnd()]
        
        [NWDGroupStart("Average with parent format (#x# is the value)")]
        public float InitAverageWithParent
        {
            get; set;
        }
        public bool ShowAverageWithParentAsPurcent
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Average #x# seconds per bit'")]
        public NWDLocalizableStringType NoAverageWithParentFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Average #x# seconds per bit'")]
        public NWDLocalizableStringType SingleAverageWithParentFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Average #x# seconds per bit'")]
        public NWDLocalizableStringType PluralAverageWithParentFormat
        {
            get; set;
        }
        [NWDGroupEnd()]
        
        [NWDGroupStart("Last format (#x# is the value)")]
        public float InitLast
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Last #x# seconds per bit'")]
        public NWDLocalizableStringType NoLastFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Last #x# seconds per bit'")]
        public NWDLocalizableStringType SingleLastFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Last #x# seconds per bit'")]
        public NWDLocalizableStringType PluralLastFormat
        {
            get; set;
        }
        [NWDGroupEnd()]
        
        [NWDGroupStart("Max format (#x# is the value)")]
        public float InitMax
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Max #x# seconds per bit'")]
        public NWDLocalizableStringType NoMaxFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Max #x# seconds per bit'")]
        public NWDLocalizableStringType SingleMaxFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Max #x# seconds per bit'")]
        public NWDLocalizableStringType PluralMaxFormat
        {
            get; set;
        }
        [NWDGroupEnd()]
        
        [NWDGroupStart("Min format (#x# is the value)")]
        public float InitMin
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Min #x# seconds per bit'")]
        public NWDLocalizableStringType NoMinFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Min #x# seconds per bit'")]
        public NWDLocalizableStringType SingleMinFormat
        {
            get; set;
        }
        [NWDTooltips(" insert #x# in the format : exmaple 'Min #x# seconds per bit'")]
        public NWDLocalizableStringType PluralMinFormat
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public NWDStatKey()
        //{
        //    //Debug.Log("NWDStatKey Constructor");
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDStatKey(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        //{
        //    //Debug.Log("NWDStatKey Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        //{
        //    NoCounterFormat.AddBaseString("#x#");
        //    SingleCounterFormat.AddBaseString("#x#");
        //    PluralCounterFormat.AddBaseString("#x#");

        //    NoTotalFormat.AddBaseString("#x#");
        //    SingleTotalFormat.AddBaseString("#x#");
        //    PluralTotalFormat.AddBaseString("#x#");

        //    NoAverageFormat.AddBaseString("#x#");
        //    SingleAverageFormat.AddBaseString("#x#");
        //    PluralAverageFormat.AddBaseString("#x#");

        //    NoAverageWithParentFormat.AddBaseString("#x#");
        //    SingleAverageWithParentFormat.AddBaseString("#x#");
        //    PluralAverageWithParentFormat.AddBaseString("#x#");

        //    NoLastFormat.AddBaseString("#x#");
        //    SingleLastFormat.AddBaseString("#x#");
        //    PluralLastFormat.AddBaseString("#x#");

        //    NoMaxFormat.AddBaseString("#x#");
        //    SingleMaxFormat.AddBaseString("#x#");
        //    PluralMaxFormat.AddBaseString("#x#");

        //    NoMinFormat.AddBaseString("#x#");
        //    SingleMinFormat.AddBaseString("#x#");
        //    PluralMinFormat.AddBaseString("#x#");

        //    UnityStat = true;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void AddEnter(float sValue)
        //{
        //    switch (Domain)
        //    {
        //        case NWDStatKeyDomain.AccountStat:
        //            {
        //                NWDAccountStatKeyValue.UserStatForKey(Reference).AddEnter(sValue);
        //            }
        //            break;
        //        case NWDStatKeyDomain.GameSaveStat:
        //            {
        //                NWDUserStatKeyValue.UserStatForKey(Reference).AddEnter(sValue);
        //            }
        //            break;
        //    }
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public double AddEnterForParent(float sValue)
        //{
        //    double rReturn = 0.0F;
        //    switch (Domain)
        //    {
        //        case NWDStatKeyDomain.AccountStat:
        //            {
        //                rReturn = NWDAccountStatKeyValue.UserStatForKey(Reference).AddEnterForParent(sValue, this);
        //            }
        //            break;
        //        case NWDStatKeyDomain.GameSaveStat:
        //            {
        //                rReturn = NWDUserStatKeyValue.UserStatForKey(Reference).AddEnterForParent(sValue, this);
        //            }
        //            break;
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public double Counter()
        //{
        //    double rReturn = 0.0F;
        //    switch (Domain)
        //    {
        //        case NWDStatKeyDomain.AccountStat:
        //            {
        //                rReturn = NWDAccountStatKeyValue.UserStatForKey(Reference).Counter;
        //            }
        //            break;
        //        case NWDStatKeyDomain.GameSaveStat:
        //            {
        //                rReturn = NWDUserStatKeyValue.UserStatForKey(Reference).Counter;
        //            }
        //            break;
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        ///// <summary>
        ///// Exampel of implement for class method.
        ///// </summary>
        //public string ReturnWithFormat(double sValue)
        //{
        //    string rReturn = string.Empty;
        //    switch (Format)
        //    {
        //        case NWDStatKeyFormat.Integer:
        //            {
        //                rReturn = sValue.ToString("0");
        //            }
        //            break;
        //        case NWDStatKeyFormat.DecimalTwo:
        //            {
        //                rReturn = sValue.ToString("F2");
        //            }
        //            break;
        //        case NWDStatKeyFormat.DecimalThree:
        //            {
        //                rReturn = sValue.ToString("F3");
        //            }
        //            break;
        //        case NWDStatKeyFormat.DecimalFour:
        //            {
        //                rReturn = sValue.ToString("F4");
        //            }
        //            break;
        //        case NWDStatKeyFormat.Float:
        //            {
        //                rReturn = sValue.ToString("F7");
        //            }
        //            break;
        //        //case NWDStatKeyFormat.PurcentTwo:
        //        //{
        //        //    rReturn = sValue.ToString("P2");
        //        //}
        //        //break;
        //        case NWDStatKeyFormat.DaysHoursMinutesSeconds:
        //            {
        //                int tAllSeconds = (int)sValue;
        //                int tDays = tAllSeconds / (3600 * 24);
        //                int tHours = (tAllSeconds - tDays * 3600 * 24) / 3600;
        //                int tMinutes = (tAllSeconds - tDays * 3600 * 24 - tHours * 3600) / 60;
        //                int tSeconds = (tAllSeconds - tDays * 3600 * 24 - tHours * 3600 - tMinutes * 60);
        //                rReturn = tDays.ToString("D") + " Days " + tHours.ToString("D2") + ":" + tMinutes.ToString("D2") + ":" + tSeconds.ToString("D2");
        //            }
        //            break;
        //        case NWDStatKeyFormat.HoursMinutesSeconds:
        //            {
        //                int tAllSeconds = (int)sValue;
        //                int tHours = (tAllSeconds) / 3600;
        //                int tMinutes = (tAllSeconds - tHours * 3600) / 60;
        //                int tSeconds = (tAllSeconds - tHours * 3600 - tMinutes * 60);
        //                rReturn = tHours.ToString("D2") + ":" + tMinutes.ToString("D2") + ":" + tSeconds.ToString("D2");
        //            }
        //            break;
        //        case NWDStatKeyFormat.MinutesSeconds:
        //            {
        //                int tAllSeconds = (int)sValue;
        //                int tMinutes = (tAllSeconds) / 60;
        //                int tSeconds = (tAllSeconds - tMinutes * 60);
        //                rReturn = tMinutes.ToString("D2") + ":" + tSeconds.ToString("DD2");
        //            }
        //            break;
        //        case NWDStatKeyFormat.Seconds:
        //            {
        //                int tAllSeconds = (int)sValue;
        //                rReturn = tAllSeconds.ToString("D") + "s";
        //            }
        //            break;
        //        default:
        //            {
        //                rReturn = sValue.ToString();
        //            }
        //            break;
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================