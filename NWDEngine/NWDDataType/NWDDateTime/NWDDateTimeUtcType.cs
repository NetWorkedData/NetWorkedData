//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using UnityEngine;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
	//TODO: FINISH THIS CLASS NWDUTCDateTimeType
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDDateTimeUtcType : NWEDataTypeInt
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDDateTimeUtcType ()
		{
            Value = (long)NWEDateHelper.ConvertToTimestamp(DateTime.UtcNow);

        }
		//-------------------------------------------------------------------------------------------------------------
		public NWDDateTimeUtcType (long sValue = 0)
		{
				Value = sValue;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = (long)NWEDateHelper.ConvertToTimestamp(DateTime.UtcNow);
        }
		//-------------------------------------------------------------------------------------------------------------
		public void SetDateTime (DateTime sDatetime)
		{
            sDatetime = sDatetime.ToUniversalTime();
            Value = (long)NWEDateHelper.ConvertToTimestamp(sDatetime);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetTimeStamp(double sTimestamp)
        {
            SetDateTime(NWEDateHelper.ConvertFromTimestamp(sTimestamp));
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetCurrentDateTime()
        {
            SetDateTime(DateTime.UtcNow);
        }
        //-------------------------------------------------------------------------------------------------------------
        public DateTime ToDateTime ()
		{
            return NWEDateHelper.ConvertFromTimestamp(Value);
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
            return NWDGUI.kPopupStyle.fixedHeight*2 + NWDGUI.kFieldMarge;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPos, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING)
        {
            NWDDateTimeUtcType tTemporary = new NWDDateTimeUtcType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            float tHeight = NWDGUI.kPopupStyle.fixedHeight;
            if (Value < 0) 
            {
                Value = 0;
            }
            DateTime tValueDateTime = NWEDateHelper.ConvertFromTimestamp(Value);
            int tYear = tValueDateTime.Year;
			int tMonth = tValueDateTime.Month;
			int tDay = tValueDateTime.Day;
			int tHour = tValueDateTime.Hour;
			int tMinute = tValueDateTime.Minute;
			int tSecond = tValueDateTime.Second;
			if (tYear < 1 || tYear > 3000) {
				tYear = 1970;
			}
			if (tMonth < 1 || tMonth > 12) {
				tMonth = 1;
			}
			if (tDay < 1) {
				tDay = 1;
			}
			int tDaysTest = DateTime.DaysInMonth (tYear, tMonth);
			if (tDay > tDaysTest) {
				tDay = tDaysTest;
			}
			if (tHour < 0 || tHour > 23 ) {
				tHour = 0;
			}
			if (tMinute < 0 || tMinute > 59 ) {
				tMinute = 0;
			}
			if (tSecond < 0 || tSecond > 59 ) {
				tSecond = 0;
			}
			float tX = sPos.x + EditorGUIUtility.labelWidth;
			DateTime tDateTime = new DateTime(tYear, tMonth,tDay,tHour,tMinute,tSecond, DateTimeKind.Utc);
			float tTiersWidth = Mathf.Ceil( (sPos.width - EditorGUIUtility.labelWidth + NWDGUI.kFieldMarge) / 3.0F);
			float tTiersWidthB = tTiersWidth - NWDGUI.kFieldMarge;
			float tHeightAdd = 0;
			float tWidthYear = tTiersWidthB + 10;
			float tWidthMonth = tTiersWidthB -5;
			float tWidthDay = tTiersWidthB -5;
            EditorGUI.LabelField (new Rect (sPos.x , sPos.y, sPos.width, NWDGUI.kLabelStyle.fixedHeight), tContent);
            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.LabelField(new Rect(sPos.x + 30,  sPos.y + tHeightAdd + NWDGUI.kLabelStyle.fixedHeight + NWDGUI.kFieldMarge, 30, NWDGUI.kPopupStyle.fixedHeight),"UTC");
            tYear = NWDDateTimeType.kYearStart() + EditorGUI.Popup (new Rect (tX, sPos.y + tHeightAdd, tWidthYear, NWDGUI.kPopupStyle.fixedHeight),
                                                                 tDateTime.Year - NWDDateTimeType.kYearStart(), NWDDateTimeType.kYears);
            tMonth = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +NWDGUI.kFieldMarge, sPos.y + tHeightAdd, tWidthMonth, NWDGUI.kPopupStyle.fixedHeight),
				tDateTime.Month-1, NWDDateTimeType.kMonths);
			int tDayNumber = DateTime.DaysInMonth(tYear,tMonth);
			if (tDayNumber == 31 )
			{
                tDay = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +tWidthMonth+NWDGUI.kFieldMarge*2, sPos.y + tHeightAdd, tWidthDay, NWDGUI.kPopupStyle.fixedHeight),
					tDateTime.Day-1, NWDDateTimeType.kDays);
			}
			else if (tDayNumber == 30)
			{
                tDay = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +tWidthMonth+NWDGUI.kFieldMarge*2, sPos.y + tHeightAdd, tWidthDay, NWDGUI.kPopupStyle.fixedHeight),
					tDateTime.Day-1, NWDDateTimeType.kDaysB);
			}
			else if (tDayNumber == 28)
			{
                tDay = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +tWidthMonth+NWDGUI.kFieldMarge*2, sPos.y + tHeightAdd, tWidthDay, NWDGUI.kPopupStyle.fixedHeight),
					tDateTime.Day-1, NWDDateTimeType.kDaysC);
			}
			else if (tDayNumber == 29)
			{
                tDay = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +tWidthMonth+NWDGUI.kFieldMarge*2, sPos.y + tHeightAdd, tWidthDay, NWDGUI.kPopupStyle.fixedHeight),
					tDateTime.Day-1, NWDDateTimeType.kDaysD);
			}
			tHeightAdd += tHeight + NWDGUI.kFieldMarge;
            GUI.Label (new Rect (tX , sPos.y+tHeightAdd,tTiersWidthB*2+NWDGUI.kFieldMarge-2, NWDGUI.kLabelStyle.fixedHeight), ":",NWDGUI.kLabelStyle);
            GUI.Label (new Rect (tX + tTiersWidthB + NWDGUI.kFieldMarge, sPos.y+tHeightAdd, tTiersWidthB*2+NWDGUI.kFieldMarge-2, NWDGUI.kLabelStyle.fixedHeight), ":",NWDGUI.kLabelStyle);
            tHour = EditorGUI.Popup (new Rect (tX, sPos.y + tHeightAdd, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight),
				tDateTime.Hour, NWDDateTimeType.kHours);
            tMinute = EditorGUI.Popup (new Rect (tX +tTiersWidth, sPos.y + tHeightAdd, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight),
				tDateTime.Minute, NWDDateTimeType.kMinutes);
            tSecond = EditorGUI.Popup (new Rect (tX +tTiersWidth*2, sPos.y + tHeightAdd, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight),
				tDateTime.Second, NWDDateTimeType.kSeconds);
            DateTime tDateTimeFinal = new DateTime(tYear, tMonth, tDay, tHour, tMinute, tSecond, DateTimeKind.Utc);
            tTemporary.Value = (long)NWEDateHelper.ConvertToTimestamp(tDateTimeFinal);
            EditorGUI.indentLevel = tIndentLevel;
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================
