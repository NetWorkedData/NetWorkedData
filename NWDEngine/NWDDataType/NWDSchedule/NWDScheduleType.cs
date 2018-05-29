//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

using UnityEngine;

using SQLite4Unity3d;

using BasicToolBox;

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
    public class NWDScheduleType : BTBDataType
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
            Value = "";
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDScheduleType(string sValue = "")
        {
            if (sValue == null)
            {
                Value = "";
            }
            else
            {
                Value = sValue;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = "";
        }
        //-------------------------------------------------------------------------------------------------------------
        public string StringResultOfDate(DateTime sDateTime)
        {
            string rReturn = "";
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
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            GUIStyle tPopupdStyle = new GUIStyle(EditorStyles.popup);
            float tHeight = tPopupdStyle.CalcHeight(new GUIContent("A"), 100.0f);
            return tHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, string sTooltips = "")
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