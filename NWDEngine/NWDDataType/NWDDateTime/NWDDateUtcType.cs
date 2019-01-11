////=====================================================================================================================
////
//// ideMobi copyright 2017 
//// All rights reserved by ideMobi
////
////=====================================================================================================================

//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//using System.Reflection;
//using System.IO;

//using UnityEngine;

//using SQLite4Unity3d;

//using BasicToolBox;

//#if UNITY_EDITOR
//using UnityEditor;
//using UnityEditorInternal;
//#endif


////=====================================================================================================================
//namespace NetWorkedData
//{
//	//TODO: FINISH THIS CLASS NWDDateUtcType
//	[SerializeField]
//	//-------------------------------------------------------------------------------------------------------------
//	public class NWDDateUtcType : BTBDataTypeInt
//    {
//        //-------------------------------------------------------------------------------------------------------------
//        public NWDDateUtcType ()
//        {
//            //Value = string.Empty;
//            Value = BTBDateHelper.ConvertToTimestamp(DateTime.UtcNow);
//        }
//		//-------------------------------------------------------------------------------------------------------------
//		public NWDDateUtcType (string sValue = BTBConstants.K_EMPTY_STRING)
//        {
//            if (string.IsNullOrEmpty(sValue) == true)
//            {
//                //Value = string.Empty;
//                Value = BTBDateHelper.ConvertToTimestamp(DateTime.UtcNow);
//            }
//            else
//            {
//                Value = sValue;
//            }
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public override void Default()
//        {
//            //Value = string.Empty;
//            Value = BTBDateHelper.ConvertToTimestamp(DateTime.UtcNow).ToString();
//        }
//		//-------------------------------------------------------------------------------------------------------------
//		public void SetDateTime(DateTime sDatetime)
//        {
//            sDatetime = sDatetime.ToUniversalTime();

//            Value = BTBDateHelper.ConvertToTimestamp(sDatetime).ToString();
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public void SetTimeStamp(double sTimestamp)
//        {
//            SetDateTime(BTBDateHelper.ConvertFromTimestamp(sTimestamp));
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public void SetCurrentDate()
//        {
//            SetDateTime(DateTime.Now);
//        }
//        //-------------------------------------------------------------------------------------------------------------
//        public DateTime ToDateTime ()
//		{
//            //string[] tDateComponent=Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
//            //int tYear = 1970;
//            //int tMonth = 1;
//            //int tDay = 1;
//            //int tHour = 0;
//            //int tMinute = 0;
//            //int tSecond = 0;
//            //if (tDateComponent.Count() == 3) {
//            //	int.TryParse(tDateComponent [0], out tYear);
//            //	int.TryParse(tDateComponent [1], out tMonth);
//            //	int.TryParse(tDateComponent [2],out tDay);
//            //}
//            //// test result of parsing 
//            //if (tYear < 1 || tYear > 3000) {
//            //	tYear = 1970;
//            //}
//            //if (tMonth < 1 || tMonth > 12) {
//            //	tMonth = 1;
//            //}
//            //if (tDay < 1) {
//            //	tDay = 1;
//            //}
//            //int tDaysTest = DateTime.DaysInMonth (tYear, tMonth);
//            //if (tDay > tDaysTest) {
//            //	tDay = tDaysTest;
//            //}

//            //DateTime rReturn = new DateTime(tYear, tMonth,tDay,tHour,tMinute,tSecond, DateTimeKind.Utc);
//            //return rReturn;

//            int tTimeStamp = 0;
//            int.TryParse(Value, out tTimeStamp);
//            return BTBDateHelper.ConvertFromTimestamp(tTimeStamp);
//        }
//		//-------------------------------------------------------------------------------------------------------------
//		#if UNITY_EDITOR
//		//-------------------------------------------------------------------------------------------------------------
//		public override float ControlFieldHeight ()
//		{
//            return NWDConstants.kPopupdStyle.fixedHeight;
//		}
//		//-------------------------------------------------------------------------------------------------------------
//        public override object ControlField (Rect sPos, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING)
//		{
//            NWDDateUtcType tTemporary = new NWDDateUtcType ();
//            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
//            //string[] tDateComponent=Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
//            int tTimeStamp = 0;
//            int.TryParse(Value, out tTimeStamp);
//            DateTime tValueDateTime = BTBDateHelper.ConvertFromTimestamp(tTimeStamp);

//            int tYear = tValueDateTime.Year;
//            int tMonth = tValueDateTime.Month;
//            int tDay = tValueDateTime.Day;
//            int tHour = 0;
//            int tMinute = 0;
//            int tSecond = 0;
//            //if (tDateComponent.Count() == 6) {
//            //  int.TryParse(tDateComponent [0], out tYear);
//            //  int.TryParse(tDateComponent [1], out tMonth);
//            //  int.TryParse(tDateComponent [2],out tDay);
//            //  int.TryParse(tDateComponent [3], out tHour);
//            //  int.TryParse(tDateComponent [4], out tMinute);
//            //  int.TryParse(tDateComponent [5], out tSecond);
//            //}
//            // test result of parsing 
//            if (tYear < 1 || tYear > 3000) {
//				tYear = 1970;
//			}
//			if (tMonth < 1 || tMonth > 12) {
//				tMonth = 1;
//			}
//			if (tDay < 1) {
//				tDay = 1;
//			}
//			int tDaysTest = DateTime.DaysInMonth (tYear, tMonth);
//			if (tDay > tDaysTest) {
//				tDay = tDaysTest;
//			}

//			float tX = sPos.x + EditorGUIUtility.labelWidth;

//			DateTime tDateTime = new DateTime(tYear, tMonth,tDay,tHour,tMinute,tSecond, DateTimeKind.Utc);

//			float tTiersWidth = Mathf.Ceil( (sPos.width - EditorGUIUtility.labelWidth + NWDConstants.kFieldMarge) / 3.0F);
//			float tTiersWidthB = tTiersWidth - NWDConstants.kFieldMarge;
////			float tTiersWidthC = tTiersWidth - NWDConstants.kFieldMarge*3;
//			float tHeightAdd = 0;

//			float tWidthYear = tTiersWidthB + 10;
//			float tWidthMonth = tTiersWidthB -5;
//			float tWidthDay = tTiersWidthB -5;
//            EditorGUI.LabelField (new Rect (sPos.x, sPos.y, sPos.width, NWDConstants.kLabelStyle.fixedHeight), tContent);

//            // remove EditorGUI.indentLevel to draw next controller without indent 
//            int tIndentLevel = EditorGUI.indentLevel;
//            EditorGUI.indentLevel = 0;

//            tYear = NWDDateTimeType.kYearStart+ EditorGUI.Popup (new Rect (tX, sPos.y + tHeightAdd, tWidthYear, NWDConstants.kPopupdStyle.fixedHeight),
//                                                                 tDateTime.Year - NWDDateTimeType.kYearStart, NWDDateTimeType.kYears);

//            tMonth = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +NWDConstants.kFieldMarge, sPos.y + tHeightAdd, tWidthMonth, NWDConstants.kPopupdStyle.fixedHeight),
//				tDateTime.Month-1, NWDDateTimeType.kMonths);

//			int tDayNumber = DateTime.DaysInMonth(tYear,tMonth);

//			if (tDayNumber == 31 )
//			{
//                tDay = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +tWidthMonth+NWDConstants.kFieldMarge*2, sPos.y + tHeightAdd, tWidthDay, NWDConstants.kPopupdStyle.fixedHeight),
//					tDateTime.Day-1, NWDDateTimeType.kDays);
//			}
//			else if (tDayNumber == 30)
//			{
//                tDay = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +tWidthMonth+NWDConstants.kFieldMarge*2, sPos.y + tHeightAdd, tWidthDay, NWDConstants.kPopupdStyle.fixedHeight),
//					tDateTime.Day-1, NWDDateTimeType.kDaysB);
//			}
//			else if (tDayNumber == 28)
//			{
//                tDay = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +tWidthMonth+NWDConstants.kFieldMarge*2, sPos.y + tHeightAdd, tWidthDay, NWDConstants.kPopupdStyle.fixedHeight),
//					tDateTime.Day-1, NWDDateTimeType.kDaysC);
//			}
//			else if (tDayNumber == 29)
//			{
//                tDay = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +tWidthMonth+NWDConstants.kFieldMarge*2, sPos.y + tHeightAdd, tWidthDay, NWDConstants.kPopupdStyle.fixedHeight),
//					tDateTime.Day-1, NWDDateTimeType.kDaysD);
//			}

//            DateTime tDateTimeFinal = new DateTime(tYear, tMonth, tDay, tHour, tMinute, tSecond, DateTimeKind.Utc);


//            //Debug.Log("Value = fields : " + tYear + NWDConstants.kFieldSeparatorA +
//            //tMonth + NWDConstants.kFieldSeparatorA +
//            //tDay + NWDConstants.kFieldSeparatorA +
//            //tHour + NWDConstants.kFieldSeparatorA +
//            //tMinute + NWDConstants.kFieldSeparatorA +
//            //tSecond);

//            tTemporary.Value = BTBDateHelper.ConvertToTimestamp(tDateTimeFinal).ToString();

//            //Debug.Log("tTemporary.Value = " + tTemporary.Value);
//            //        tTemporary.Value = tYear+NWDConstants.kFieldSeparatorA+
//            //tMonth+NWDConstants.kFieldSeparatorA+
//            //tDay+NWDConstants.kFieldSeparatorA+
//            //tHour+NWDConstants.kFieldSeparatorA+
//            //tMinute+NWDConstants.kFieldSeparatorA+
//            //tSecond;

//            //GUI.Label (new Rect (sPos.x, sPos.y+tHeightAdd, sPos.width, sPos.height), Value);

//            // move EditorGUI.indentLevel to draw next controller with indent 
//            EditorGUI.indentLevel = tIndentLevel;

//            return tTemporary;
//		}
//		//-------------------------------------------------------------------------------------------------------------
//		#endif
//		//-------------------------------------------------------------------------------------------------------------
//	}
//}
////=====================================================================================================================