//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:27:27
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
            //Value = string.Empty;
            Value = (long)NWEDateHelper.ConvertToTimestamp(DateTime.UtcNow);
        }
		//-------------------------------------------------------------------------------------------------------------
		public void SetDateTime (DateTime sDatetime)
		{
            sDatetime = sDatetime.ToUniversalTime();

            Value = (long)NWEDateHelper.ConvertToTimestamp(sDatetime);

    //        Value = sDatetime.Year+NWDConstants.kFieldSeparatorA+
				//sDatetime.Month+NWDConstants.kFieldSeparatorA+
				//sDatetime.Day+NWDConstants.kFieldSeparatorA+
				//sDatetime.Hour+NWDConstants.kFieldSeparatorA+
				//sDatetime.Minute+NWDConstants.kFieldSeparatorA+
				//sDatetime.Second;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetTimeStamp(double sTimestamp)
        {
            SetDateTime(NWEDateHelper.ConvertFromTimestamp(sTimestamp));
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetCurrentDateTime()
        {
            SetDateTime(DateTime.Now);
        }
        //-------------------------------------------------------------------------------------------------------------
        public DateTime ToDateTime ()
		{
            //string[] tDateComponent=Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            //int tYear = 1970;
            //int tMonth = 1;
            //int tDay = 1;
            //int tHour = 0;
            //int tMinute = 0;
            //int tSecond = 0;
            //if (tDateComponent.Count() == 6) {
            //	int.TryParse(tDateComponent [0], out tYear);
            //	int.TryParse(tDateComponent [1], out tMonth);
            //	int.TryParse(tDateComponent [2],out tDay);
            //	int.TryParse(tDateComponent [3], out tHour);
            //	int.TryParse(tDateComponent [4], out tMinute);
            //	int.TryParse(tDateComponent [5], out tSecond);
            //}
            //// test result of parsing 
            //if (tYear < 1 || tYear > 3000) {
            //	tYear = 1970;
            //}
            //if (tMonth < 1 || tMonth > 12) {
            //	tMonth = 1;
            //}
            //if (tDay < 1) {
            //	tDay = 1;
            //}
            //int tDaysTest = DateTime.DaysInMonth (tYear, tMonth);
            //if (tDay > tDaysTest) {
            //	tDay = tDaysTest;
            //}
            //if (tHour < 0 || tHour > 23 ) {
            //	tHour = 0;
            //}
            //if (tMinute < 0 || tMinute > 59 ) {
            //	tMinute = 0;
            //}
            //if (tSecond < 0 || tSecond > 59 ) {
            //	tSecond = 0;
            //}
            //DateTime rReturn = new DateTime(tYear, tMonth,tDay,tHour,tMinute,tSecond, DateTimeKind.Utc);
            //return rReturn;

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
            //Debug.Log("Value Receipt= " + Value);
            NWDDateTimeUtcType tTemporary = new NWDDateTimeUtcType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            float tHeight = NWDGUI.kPopupStyle.fixedHeight;
            //string[] tDateComponent=Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
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
			//if (tDateComponent.Count() == 6) {
			//	int.TryParse(tDateComponent [0], out tYear);
			//	int.TryParse(tDateComponent [1], out tMonth);
			//	int.TryParse(tDateComponent [2],out tDay);
			//	int.TryParse(tDateComponent [3], out tHour);
			//	int.TryParse(tDateComponent [4], out tMinute);
			//	int.TryParse(tDateComponent [5], out tSecond);
			//}
			// test result of parsing 
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
//			float tTiersWidthC = tTiersWidth - NWDGUI.kFieldMarge*3;
			float tHeightAdd = 0;

			float tWidthYear = tTiersWidthB + 10;
			float tWidthMonth = tTiersWidthB -5;
			float tWidthDay = tTiersWidthB -5;
            EditorGUI.LabelField (new Rect (sPos.x, sPos.y, sPos.width, NWDGUI.kLabelStyle.fixedHeight), tContent);

            // remove EditorGUI.indentLevel to draw next controller without indent 
            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUI.LabelField(new Rect(tX, sPos.y + tHeightAdd, 30, NWDGUI.kPopupStyle.fixedHeight),"UTC");

            tYear = NWDDateTimeType.kYearStart() + EditorGUI.Popup (new Rect (tX+30, sPos.y + tHeightAdd, tWidthYear-30, NWDGUI.kPopupStyle.fixedHeight),
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


            //Debug.Log("Value = fields : " + tYear + NWDConstants.kFieldSeparatorA +
            //tMonth + NWDConstants.kFieldSeparatorA +
            //tDay + NWDConstants.kFieldSeparatorA +
            //tHour + NWDConstants.kFieldSeparatorA +
            //tMinute + NWDConstants.kFieldSeparatorA +
            //tSecond);

            tTemporary.Value = (long)NWEDateHelper.ConvertToTimestamp(tDateTimeFinal);

            //Debug.Log("tTemporary.Value = " + tTemporary.Value);
            //        tTemporary.Value = tYear+NWDConstants.kFieldSeparatorA+
            //tMonth+NWDConstants.kFieldSeparatorA+
            //tDay+NWDConstants.kFieldSeparatorA+
            //tHour+NWDConstants.kFieldSeparatorA+
            //tMinute+NWDConstants.kFieldSeparatorA+
            //tSecond;

            //GUI.Label (new Rect (sPos.x, sPos.y+tHeightAdd, sPos.width, sPos.height), Value);

            // move EditorGUI.indentLevel to draw next controller with indent 
            EditorGUI.indentLevel = tIndentLevel;

			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================