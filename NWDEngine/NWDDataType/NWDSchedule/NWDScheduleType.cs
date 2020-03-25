//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:28:42
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================



using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

using UnityEngine;



//using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //TODO: FINISH THIS CLASS NWDScheduleType
    [SerializeField]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDScheduleType : NWEDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public static string kDaysSchedulePrefix = "N";
        public static string kDaysOfWeekSchedulePrefix = "D";
        public static string kMonthsSchedulePrefix = "M";
        public static string kHoursSchedulePrefix = "H";
        public static string kMinutesSchedulePrefix = "i";
        public static string kSecondsSchedulePrefix = "s";
        //-------------------------------------------------------------------------------------------------------------
        public static string kMinutesUnit = "M";
        public static string kHoursUnit = "H";
        public static string kNowSuccess = "Now √";
        public static string kNowFailed = "Now x";
        public static string kNowGameTime = "GameTime";
        public static string kNowGameSuccess = "Now GameTime √";
        public static string kNowGameFailed = "Now GameTime x";
        //-------------------------------------------------------------------------------------------------------------
        public NWDScheduleType()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDScheduleType(string sValue = NWEConstants.K_EMPTY_STRING)
        {
            if (sValue == null)
            {
                Value = string.Empty;
            }
            else
            {
                Value = sValue;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string StringResultOfDate(DateTime sDateTime)
        {
            string rReturn = string.Empty;
            DayOfWeek tDayOfWeek = sDateTime.DayOfWeek;
            switch (tDayOfWeek)
            {
                case DayOfWeek.Monday:
                    rReturn += kDaysOfWeekSchedulePrefix + "0";
                    break;
                case DayOfWeek.Tuesday:
                    rReturn += kDaysOfWeekSchedulePrefix + "1";
                    break;
                case DayOfWeek.Wednesday:
                    rReturn += kDaysOfWeekSchedulePrefix + "2";
                    break;
                case DayOfWeek.Thursday:
                    rReturn += kDaysOfWeekSchedulePrefix + "3";
                    break;
                case DayOfWeek.Friday:
                    rReturn += kDaysOfWeekSchedulePrefix + "4";
                    break;
                case DayOfWeek.Saturday:
                    rReturn += kDaysOfWeekSchedulePrefix + "5";
                    break;
                case DayOfWeek.Sunday:
                    rReturn += kDaysOfWeekSchedulePrefix + "6";
                    break;
            }
            int tMonth = sDateTime.Month - 1;
            rReturn += kMonthsSchedulePrefix + tMonth.ToString("00");

            int tDays = sDateTime.Day - 1;
            rReturn += kDaysSchedulePrefix + tDays.ToString("00");

            int tHour = sDateTime.Hour;
            rReturn += kHoursSchedulePrefix + tHour.ToString("00");

            int tMinute = sDateTime.Minute;
            rReturn += kMinutesSchedulePrefix + tMinute.ToString("00");

            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //[Obsolete("ResultForNow is deprecated, please use AvailableNow instead.")]
        //public bool ResultForNow () 
        //{
        //return ResultForDate (DateTime.Now);
        //}
        //-------------------------------------------------------------------------------------------------------------
        public bool AvailableNow()
        {
            return ResultForDate(DateTime.Now);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool AvailableNowInGameTime()
        {
            DateTime tDateTimeInGame = NWDAppEnvironment.SelectedEnvironment().DateTimeInGameTime();
            return ResultForDate(tDateTimeInGame);
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool ResultForDate(DateTime sDateTime)
        {
            return false;
        }
        public DateTime NextDateTime()
        {
            DateTime tReturn = DateTime.Now;
            // TODO Find next date time!
            /// 00-( ... ca va etre coton :-/ 
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            GUIStyle tPopupdStyle = new GUIStyle(EditorStyles.popup);
            float tHeight = tPopupdStyle.CalcHeight(new GUIContent(NWEConstants.K_A), 100.0f);
            return tHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDScheduleType tTemporary = new NWDScheduleType();
            //GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================