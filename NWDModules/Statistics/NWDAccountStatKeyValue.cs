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
#if ENABLE_CLOUD_SERVICES_ANALYTICS
using UnityEngine.Analytics;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("STC")]
    [NWDClassDescriptionAttribute("StatKeyValue")]
    [NWDClassMenuNameAttribute("StatKeyValue")]
    public partial class NWDAccountStatKeyValue : NWDBasis<NWDAccountStatKeyValue>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Connection")]
        public NWDReferenceType<NWDAccount> Account {get; set;}
        public NWDReferenceType<NWDStatKey> StatKey
        {
            get; set;
        }
        [NWDGroupEnd()]
        [NWDGroupSeparator()]
        [NWDGroupStart("Values")]
        public double Total
        {
            get; set;
        }
        public double Counter
        {
            get; set;
        }
        public double Average
        {
            get; set;
        }
        public double AverageWithParent
        {
            get; set;
        }
        public double Last
        {
            get; set;
        }
        public double Max
        {
            get; set;
        }
        public double Min
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
//        public NWDAccountStatKeyValue()
//        {
//            //Debug.Log("NWDUserStatKeyValue Constructor");
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public NWDAccountStatKeyValue(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
//        {
//            //Debug.Log("NWDUserStatKeyValue Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
//        {
            
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        /// <summary>
//        /// Exampel of implement for class method.
//        /// </summary>
//        public static void MyClassMethod()
//        {
//            // do something with this class
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public double AddEnterForParent(float sValue, NWDStatKey sStatKey)
//        {

//            if (sStatKey.UnityStat == true)
//            {
//#if ENABLE_CLOUD_SERVICES_ANALYTICS
//            //    AnalyticsEvent.Custom(sStatKey.InternalKey, new Dictionary<string, object>
//            //{
//            //        {"nwd_account_ref", Account.GetReference()},
//            //        {"nwd_value", sValue},
//            //        /*
//            //    {"nwd_total", Total},
//            //    {"nwd_counter", Counter},
//            //    {"nwd_average", Average},
//            //    {"nwd_average_parent", AverageWithParent},
//            //    {"nwd_last", Last},
//            //    {"nwd_max", Max},
//            //    {"nwd_min", Min},
//            //    */
//            //});
//#endif
        //    }
        //    if (Max < sValue)
        //    {
        //        Max = sValue;
        //    }
        //    if (Min > sValue)
        //    {
        //        Min = sValue;
        //    }
        //    Last = sValue;
        //    Counter += 1.0F;
        //    Total += sValue;
        //    if (Counter != 0)
        //    {
        //        Average = Total / Counter;
        //    }
        //    else
        //    {
        //        Average = 0.0F;
        //    }
        //    UpdateData(true, kWritingMode);
        //    return Counter;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void Reevaluate()
        //{
        //    NWDStatKey tStatKey = StatKey.GetObject();
        //    if (tStatKey.Parent != null)
        //    {
        //        NWDStatKey tStatKeyParent = tStatKey.Parent.GetObject();
        //        if (tStatKeyParent != null)
        //        {
        //            //I need transfert data to parent and use data for recalulate result
        //            double tCounter = tStatKeyParent.Counter();
        //            if (tCounter != 0)
        //            {
        //                AverageWithParent = Total / tCounter;
        //            }
        //            else
        //            {
        //                AverageWithParent = 0.0F;
        //            }
        //        }
        //    }
        //    UpdateData(true, kWritingMode);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void AddEnter(float sValue)
        //{
        //    //Debug.Log("NWDStatKeyValue AddEnter(" + sValue.ToString() + ")");
        //    NWDStatKey tStatKey = StatKey.GetObject();
        //    AddEnterForParent(sValue, tStatKey);
        //    if (tStatKey.Parent != null)
        //    {
        //        NWDStatKey tStatKeyParent = tStatKey.Parent.GetObject();
        //        if (tStatKeyParent != null)
        //        {
        //            //I need transfert data to parent and use data for recalulate result
        //            double tCounter = tStatKeyParent.AddEnterForParent(sValue);
        //            if (tCounter != 0)
        //            {
        //                AverageWithParent = Total / tCounter;
        //            }
        //            else
        //            {
        //                AverageWithParent = 0.0F;
        //            }
        //        }
        //    }
        //    foreach (NWDStatKey tStatKeyChild in tStatKey.Dependent.GetObjects())
        //    {
        //        tStatKeyChild.AddEnterForParent(sValue);
        //    }
        //    UpdateData(true, kWritingMode);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public string TotalStylized()
        //{
        //    NWDStatKey tStaKey = StatKey.GetObject();
        //    return tStaKey.ReturnWithFormat(Total);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public string AverageStylized()
        //{
        //    NWDStatKey tStaKey = StatKey.GetObject();
        //    return tStaKey.ReturnWithFormat(Average);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public string LastStylized()
        //{
        //    NWDStatKey tStaKey = StatKey.GetObject();
        //    return tStaKey.ReturnWithFormat(Last);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public string MinStylized()
        //{
        //    NWDStatKey tStaKey = StatKey.GetObject();
        //    return tStaKey.ReturnWithFormat(Min);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public string MaxStylized()
        //{
        //    NWDStatKey tStaKey = StatKey.GetObject();
        //    return tStaKey.ReturnWithFormat(Max);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public string TotalDescription ()
        //{
        //    NWDStatKey tStaKey = StatKey.GetObject();
        //    string tTotalFormatted = tStaKey.ReturnWithFormat(Total);
        //    string rReturn = string.Empty;
        //    if (Total == 0.0f)
        //    {
        //        rReturn = tStaKey.NoTotalFormat.GetLocalString().Replace("#x#",tTotalFormatted);
        //    }
        //    else if (Total == 1.0f)
        //    {
        //        rReturn = tStaKey.SingleTotalFormat.GetLocalString().Replace("#x#", tTotalFormatted);
        //    }
        //    else
        //    {
        //        rReturn = tStaKey.PluralTotalFormat.GetLocalString().Replace("#x#", tTotalFormatted);
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public string AverageDescription()
        //{
        //    NWDStatKey tStaKey = StatKey.GetObject();
        //    string tAverageFormatted = tStaKey.ReturnWithFormat(Average);
        //    if (tStaKey.ShowAverageWithParentAsPurcent == true)
        //    {
        //        tAverageFormatted = (Average * 100.0F).ToString("F2") + " %";
        //    }
        //    string rReturn = string.Empty;
        //    if (Average == 0.0f)
        //    {
        //        rReturn = tStaKey.NoAverageFormat.GetLocalString().Replace("#x#", tAverageFormatted);
        //    }
        //    else if (Average == 1.0f)
        //    {
        //        rReturn = tStaKey.SingleAverageFormat.GetLocalString().Replace("#x#", tAverageFormatted);
        //    }
        //    else
        //    {
        //        rReturn = tStaKey.PluralAverageFormat.GetLocalString().Replace("#x#", tAverageFormatted);
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public string AverageWithParentDescription()
        //{
        //    NWDStatKey tStaKey = StatKey.GetObject();
        //    string tAverageWithParentFormatted = tStaKey.ReturnWithFormat(AverageWithParent);
        //    if (tStaKey.ShowAverageWithParentAsPurcent == true)
        //    {
        //        tAverageWithParentFormatted = (AverageWithParent * 100.0F).ToString("F2") + " %";
        //    }
        //    string rReturn = string.Empty;
        //    if (AverageWithParent == 0.0f)
        //    {
        //        rReturn = tStaKey.NoAverageWithParentFormat.GetLocalString().Replace("#x#", tAverageWithParentFormatted);
        //    }
        //    else if (AverageWithParent == 1.0f)
        //    {
        //        rReturn = tStaKey.SingleAverageWithParentFormat.GetLocalString().Replace("#x#", tAverageWithParentFormatted);
        //    }
        //    else
        //    {
        //        rReturn = tStaKey.PluralAverageWithParentFormat.GetLocalString().Replace("#x#", tAverageWithParentFormatted);
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public string LastDescription()
        //{
        //    NWDStatKey tStaKey = StatKey.GetObject();
        //    string tLastFormatted = tStaKey.ReturnWithFormat(Last);
        //    string rReturn = string.Empty;
        //    if (Last == 0.0f)
        //    {
        //        rReturn = tStaKey.NoLastFormat.GetLocalString().Replace("#x#", tLastFormatted);
        //    }
        //    else if (Last == 1.0f)
        //    {
        //        rReturn = tStaKey.SingleLastFormat.GetLocalString().Replace("#x#", tLastFormatted);
        //    }
        //    else
        //    {
        //        rReturn = tStaKey.PluralLastFormat.GetLocalString().Replace("#x#", tLastFormatted);
        //    }
        //    return rReturn;
        //}

        ////-------------------------------------------------------------------------------------------------------------
        //public string MinDescription()
        //{
        //    NWDStatKey tStaKey = StatKey.GetObject();
        //    string tMinFormatted = tStaKey.ReturnWithFormat(Min);
        //    string rReturn = string.Empty;
        //    if (Min == 0.0f)
        //    {
        //        rReturn = tStaKey.NoMinFormat.GetLocalString().Replace("#x#", tMinFormatted);
        //    }
        //    else if (Min == 1.0f)
        //    {
        //        rReturn = tStaKey.SingleMinFormat.GetLocalString().Replace("#x#", tMinFormatted);
        //    }
        //    else
        //    {
        //        rReturn = tStaKey.PluralMinFormat.GetLocalString().Replace("#x#", tMinFormatted);
        //    }
        //    return rReturn;
        //}

        ////-------------------------------------------------------------------------------------------------------------
        //public string MaxDescription()
        //{
        //    NWDStatKey tStaKey = StatKey.GetObject();
        //    string tMaxFormatted = tStaKey.ReturnWithFormat(Max);
        //    string rReturn = string.Empty;
        //    if (Max == 0.0f)
        //    {
        //        rReturn = tStaKey.NoMaxFormat.GetLocalString().Replace("#x#", tMaxFormatted);
        //    }
        //    else if (Max == 1.0f)
        //    {
        //        rReturn = tStaKey.SingleMaxFormat.GetLocalString().Replace("#x#", tMaxFormatted);
        //    }
        //    else
        //    {
        //        rReturn = tStaKey.PluralMaxFormat.GetLocalString().Replace("#x#", tMaxFormatted);
        //    }
        //    return rReturn;
        //}

        ////-------------------------------------------------------------------------------------------------------------
        //public string CounterDescription()
        //{
        //    NWDStatKey tStaKey = StatKey.GetObject();
        //    string tCounterFormatted = ((int)Counter).ToString();
        //    string rReturn = string.Empty;
        //    if (Counter == 0.0f)
        //    {
        //        rReturn = tStaKey.NoCounterFormat.GetLocalString().Replace("#x#", tCounterFormatted);
        //    }
        //    else if (Counter == 1.0f)
        //    {
        //        rReturn = tStaKey.SingleCounterFormat.GetLocalString().Replace("#x#", tCounterFormatted);
        //    }
        //    else
        //    {
        //        rReturn = tStaKey.PluralCounterFormat.GetLocalString().Replace("#x#", tCounterFormatted);
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public static List<Type> OverrideClasseInThisSync()
        //{
        //    return new List<Type> { typeof(NWDAccountStatKeyValue), typeof(NWDStatKey) };
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================