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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDStatKeyFormat : int
    {
        None = 0,
        Integer = 1,
        DecimalTwo = 2,
        DecimalThree = 3,
        DecimalFour = 4,
        Float = 10,
        // %
        //PurcentTwo = 20,
        // for time show
        Seconds = 30,
        MinutesSeconds = 31,
        HoursMinutesSeconds = 32,
        DaysHoursMinutesSeconds = 33,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDStatisticKey : NWDBasis<NWDStatisticKey>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDStatisticKey()
        {
            //Debug.Log("NWDStatKey Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDStatisticKey(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDStatKey Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDUserStatistic), typeof(NWDAccountStatistic), typeof(NWDStatisticKey) };
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
            NoCounterFormat.AddBaseString("#x#");
            SingleCounterFormat.AddBaseString("#x#");
            PluralCounterFormat.AddBaseString("#x#");

            NoTotalFormat.AddBaseString("#x#");
            SingleTotalFormat.AddBaseString("#x#");
            PluralTotalFormat.AddBaseString("#x#");

            NoAverageFormat.AddBaseString("#x#");
            SingleAverageFormat.AddBaseString("#x#");
            PluralAverageFormat.AddBaseString("#x#");

            NoAverageWithParentFormat.AddBaseString("#x#");
            SingleAverageWithParentFormat.AddBaseString("#x#");
            PluralAverageWithParentFormat.AddBaseString("#x#");

            NoLastFormat.AddBaseString("#x#");
            SingleLastFormat.AddBaseString("#x#");
            PluralLastFormat.AddBaseString("#x#");

            NoMaxFormat.AddBaseString("#x#");
            SingleMaxFormat.AddBaseString("#x#");
            PluralMaxFormat.AddBaseString("#x#");

            NoMinFormat.AddBaseString("#x#");
            SingleMinFormat.AddBaseString("#x#");
            PluralMinFormat.AddBaseString("#x#");

            UnityStat = true;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddEnter(float sValue)
        {
            switch (Domain)
            {
                case NWDGameDomain.Account:
                    {
                        NWDAccountStatistic.UserStatForKey(Reference).AddEnter(sValue);
                    }
                    break;
                case NWDGameDomain.GameSave:
                    {
                        NWDUserStatistic.UserStatForKey(Reference).AddEnter(sValue);
                    }
                    break;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public double AddEnterForParent(float sValue)
        {
            double rReturn = 0.0F;
            switch (Domain)
            {
                case NWDGameDomain.Account:
                    {
                        rReturn = NWDAccountStatistic.UserStatForKey(Reference).AddEnterForParent(sValue, this);
                    }
                    break;
                case NWDGameDomain.GameSave:
                    {
                        rReturn = NWDUserStatistic.UserStatForKey(Reference).AddEnterForParent(sValue, this);
                    }
                    break;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public double Counter()
        {
            double rReturn = 0.0F;
            switch (Domain)
            {
                case NWDGameDomain.Account:
                    {
                        rReturn = NWDAccountStatistic.UserStatForKey(Reference).Counter;
                    }
                    break;
                case NWDGameDomain.GameSave:
                    {
                        rReturn = NWDUserStatistic.UserStatForKey(Reference).Counter;
                    }
                    break;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for class method.
        /// </summary>
        public string ReturnWithFormat(double sValue)
        {
            string rReturn = string.Empty;
            switch (Format)
            {
                case NWDStatKeyFormat.Integer:
                    {
                        rReturn = sValue.ToString("0");
                    }
                    break;
                case NWDStatKeyFormat.DecimalTwo:
                    {
                        rReturn = sValue.ToString("F2");
                    }
                    break;
                case NWDStatKeyFormat.DecimalThree:
                    {
                        rReturn = sValue.ToString("F3");
                    }
                    break;
                case NWDStatKeyFormat.DecimalFour:
                    {
                        rReturn = sValue.ToString("F4");
                    }
                    break;
                case NWDStatKeyFormat.Float:
                    {
                        rReturn = sValue.ToString("F7");
                    }
                    break;
                //case NWDStatKeyFormat.PurcentTwo:
                //{
                //    rReturn = sValue.ToString("P2");
                //}
                //break;
                case NWDStatKeyFormat.DaysHoursMinutesSeconds:
                    {
                        int tAllSeconds = (int)sValue;
                        int tDays = tAllSeconds / (3600 * 24);
                        int tHours = (tAllSeconds - tDays * 3600 * 24) / 3600;
                        int tMinutes = (tAllSeconds - tDays * 3600 * 24 - tHours * 3600) / 60;
                        int tSeconds = (tAllSeconds - tDays * 3600 * 24 - tHours * 3600 - tMinutes * 60);
                        rReturn = tDays.ToString("D") + " Days " + tHours.ToString("D2") + ":" + tMinutes.ToString("D2") + ":" + tSeconds.ToString("D2");
                    }
                    break;
                case NWDStatKeyFormat.HoursMinutesSeconds:
                    {
                        int tAllSeconds = (int)sValue;
                        int tHours = (tAllSeconds) / 3600;
                        int tMinutes = (tAllSeconds - tHours * 3600) / 60;
                        int tSeconds = (tAllSeconds - tHours * 3600 - tMinutes * 60);
                        rReturn = tHours.ToString("D2") + ":" + tMinutes.ToString("D2") + ":" + tSeconds.ToString("D2");
                    }
                    break;
                case NWDStatKeyFormat.MinutesSeconds:
                    {
                        int tAllSeconds = (int)sValue;
                        int tMinutes = (tAllSeconds) / 60;
                        int tSeconds = (tAllSeconds - tMinutes * 60);
                        rReturn = tMinutes.ToString("D2") + ":" + tSeconds.ToString("DD2");
                    }
                    break;
                case NWDStatKeyFormat.Seconds:
                    {
                        int tAllSeconds = (int)sValue;
                        rReturn = tAllSeconds.ToString("D") + "s";
                    }
                    break;
                default:
                    {
                        rReturn = sValue.ToString();
                    }
                    break;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================