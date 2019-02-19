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
	/// <summary>
	/// NWD date time range type. This type can reccord two date time (year:month:day:hour:minute:second) 
	/// and determine if another date time is between this range.
	/// </summary>
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDDateRangeType : BTBDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NetWorkedData.NWDDateRangeType"/> class.
		/// </summary>
		public NWDDateRangeType ()
		{
			Value = string.Empty;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NetWorkedData.NWDDateRangeType"/> class.
		/// </summary>
		/// <param name="sValue">S value.</param>
		public NWDDateRangeType (string sValue = BTBConstants.K_EMPTY_STRING)
		{
			if (sValue == null) {
				Value = string.Empty;
			} else {
				Value = sValue;
			}
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void BaseVerif()
        {
            // Need to check with a new dictionary each time
            if (string.IsNullOrEmpty(Value))
            {
                Default();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Results if "now" is in range.
        /// </summary>
        /// <returns><c>true</c>, if now was resulted, <c>false</c> otherwise.</returns>
        public bool AvailableNow ()
		{
            return AvailableDate (DateTime.Now);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool AvailableNowInGameTime()
        {
            return AvailableDate(NWDAppEnvironment.SelectedEnvironment().DateTimeInGameTime());
        }
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Results if date time is in range.
		/// </summary>
		/// <returns><c>true</c>, if for date was resulted, <c>false</c> otherwise.</returns>
		/// <param name="sDateTime">S date time.</param>
        public bool AvailableDate (DateTime sDateTime)
		{
			int tDate = NWDToolbox.Timestamp (sDateTime);
			DateTime[] tRangeDates = ToDateTime ();
			int tDateStart = NWDToolbox.Timestamp (tRangeDates [0]);
			int tDateEnd = NWDToolbox.Timestamp (tRangeDates [1]);
			bool rReturn = false;
			if (tDate >= tDateStart && tDate <= tDateEnd) {
				rReturn = true;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Sets the date time for the start and end of range.
		/// </summary>
		/// <param name="sStartDatetime">S start datetime.</param>
		/// <param name="sEndDatetime">S end datetime.</param>
		public void SetDateTime (DateTime sStartDatetime, DateTime sEndDatetime)
		{
			Value = sStartDatetime.Year + NWDConstants.kFieldSeparatorA +
			sStartDatetime.Month + NWDConstants.kFieldSeparatorA +
			sStartDatetime.Day + NWDConstants.kFieldSeparatorA +
			sStartDatetime.Hour + NWDConstants.kFieldSeparatorA +
			sStartDatetime.Minute + NWDConstants.kFieldSeparatorA +
			sStartDatetime.Second +
			NWDConstants.kFieldSeparatorB +
			sEndDatetime.Year + NWDConstants.kFieldSeparatorA +
			sEndDatetime.Month + NWDConstants.kFieldSeparatorA +
			sEndDatetime.Day + NWDConstants.kFieldSeparatorA +
			sEndDatetime.Hour + NWDConstants.kFieldSeparatorA +
			sEndDatetime.Minute + NWDConstants.kFieldSeparatorA +
			sEndDatetime.Second;
		}
		//-------------------------------------------------------------------------------------------------------------
		public DateTime StartDateTime ()
		{
			return ToDateTime () [0];
		}
		//-------------------------------------------------------------------------------------------------------------
		public DateTime EndDateTime ()
		{
			return ToDateTime () [1];
		}
		//-------------------------------------------------------------------------------------------------------------
		public DateTime[] ToDateTime ()
		{
			DateTime[] rReturn = new DateTime[2];
			rReturn [0] = new DateTime (1970, 1, 1, 0, 0, 0);
			rReturn [1] = new DateTime (1970, 1, 1, 0, 0, 0);
			string[] tDates = Value.Split (new string[]{ NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
			int tIndex = 0;
			foreach (string tDate in tDates) {
				string[] tDateComponent = tDate.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
				int tYear = 1970;
				int tMonth = 1;
				int tDay = 1;
				int tHour = 0;
				int tMinute = 0;
				int tSecond = 0;
				if (tDateComponent.Count () == 6) {
					int.TryParse (tDateComponent [0], out tYear);
					int.TryParse (tDateComponent [1], out tMonth);
					int.TryParse (tDateComponent [2], out tDay);
					int.TryParse (tDateComponent [3], out tHour);
					int.TryParse (tDateComponent [4], out tMinute);
					int.TryParse (tDateComponent [5], out tSecond);
				}
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
				if (tHour < 0 || tHour > 23) {
					tHour = 0;
				}
				if (tMinute < 0 || tMinute > 59) {
					tMinute = 0;
				}
				if (tSecond < 0 || tSecond > 59) {
					tSecond = 0;
				}

				rReturn [tIndex++] = new DateTime (tYear, tMonth, tDay, tHour, tMinute, tSecond);
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The height of the field. Add a simular method like ControlFieldHeight in your code
		/// </summary>
		/// <returns>The field height.</returns>
		public override float ControlFieldHeight ()
		{
			//GUIStyle tPopupStyle = new GUIStyle (EditorStyles.popup);
			//float tHeight = tPopupStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			//GUIStyle tLabelStyle = new GUIStyle (EditorStyles.boldLabel);
			//float tHeightTitle = tLabelStyle.CalcHeight (new GUIContent ("A"), 100.0f);
            return NWDConstants.kPopupdStyle.fixedHeight * 2 + NWDConstants.kFieldMarge * 3 + NWDConstants.kLabelStyle.fixedHeight * 2;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The field to edit value in editor.
		/// </summary>
		/// <returns>The field.</returns>
		/// <param name="sPosition">S position.</param>
		/// <param name="sEntitled">S entitled.</param>
		/// <param name="sPos">S position.</param>
        public override object ControlField (Rect sPos, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING)
		{
            NWDDateRangeType tTemporary = new NWDDateRangeType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
			//GUIStyle tPopupStyle = new GUIStyle (EditorStyles.popup);

			//GUIStyle tSeparatorStyle = new GUIStyle (EditorStyles.label);
			//tSeparatorStyle.alignment = TextAnchor.MiddleCenter;
            float tHeight = NWDConstants.kPopupdStyle.fixedHeight;

			//GUIStyle tLabelStyle = new GUIStyle (EditorStyles.boldLabel);
            float tHeightTitle = NWDConstants.kLabelStyle.fixedHeight;

//			string[] tDateComponent = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);

			DateTime[] tDateTimes = ToDateTime ();
			DateTime tStartDateTimes = tDateTimes [0];
			DateTime tEndDateTimes = tDateTimes [1];

			float tX = sPos.x + EditorGUIUtility.labelWidth;

			float tTiersWidth = Mathf.Ceil ((sPos.width - EditorGUIUtility.labelWidth + NWDConstants.kFieldMarge) / 3.0F);
			float tTiersWidthB = tTiersWidth - NWDConstants.kFieldMarge;
//			float tTiersWidthC = tTiersWidth - NWDConstants.kFieldMarge * 3;
			float tHeightAdd = 0;

			float tWidthYear = tTiersWidthB + 10;
			float tWidthMonth = tTiersWidthB - 5;
			float tWidthDay = tTiersWidthB - 5;
            EditorGUI.LabelField (new Rect (sPos.x, sPos.y, sPos.width, NWDConstants.kLabelStyle.fixedHeight), tContent);

            // remove EditorGUI.indentLevel to draw next controller without indent 
            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            GUI.Label (new Rect (sPos.x + EditorGUIUtility.labelWidth, sPos.y, sPos.width, NWDConstants.kLabelStyle.fixedHeight), "Start", NWDConstants.kLabelStyle);
			tHeightAdd += tHeightTitle;


            int tYearStart = NWDDateTimeType.kYearStart() + EditorGUI.Popup (new Rect (tX, sPos.y + tHeightAdd, tWidthYear, sPos.height),
                                                                           tStartDateTimes.Year - NWDDateTimeType.kYearStart(), NWDDateTimeType.kYears);

			int tMonthStart = 1 + EditorGUI.Popup (new Rect (tX + tWidthYear + NWDConstants.kFieldMarge, sPos.y + tHeightAdd, tWidthMonth, sPos.height),
				                  tStartDateTimes.Month - 1, NWDDateTimeType.kMonths);

			int tDayNumberStart = DateTime.DaysInMonth (tYearStart, tMonthStart);

			int tDayStart = 1;
			if (tDayNumberStart == 31) {
                tDayStart = 1 + EditorGUI.Popup (new Rect (tX + tWidthYear + tWidthMonth + NWDConstants.kFieldMarge * 2, sPos.y + tHeightAdd, tWidthDay, NWDConstants.kPopupdStyle.fixedHeight),
					tStartDateTimes.Day - 1, NWDDateTimeType.kDays);
			} else if (tDayNumberStart == 30) {
                tDayStart = 1 + EditorGUI.Popup (new Rect (tX + tWidthYear + tWidthMonth + NWDConstants.kFieldMarge * 2, sPos.y + tHeightAdd, tWidthDay, NWDConstants.kPopupdStyle.fixedHeight),
					tStartDateTimes.Day - 1, NWDDateTimeType.kDaysB);
			} else if (tDayNumberStart == 28) {
                tDayStart = 1 + EditorGUI.Popup (new Rect (tX + tWidthYear + tWidthMonth + NWDConstants.kFieldMarge * 2, sPos.y + tHeightAdd, tWidthDay, NWDConstants.kPopupdStyle.fixedHeight),
					tStartDateTimes.Day - 1, NWDDateTimeType.kDaysC);
			} else if (tDayNumberStart == 29) {
                tDayStart = 1 + EditorGUI.Popup (new Rect (tX + tWidthYear + tWidthMonth + NWDConstants.kFieldMarge * 2, sPos.y + tHeightAdd, tWidthDay, NWDConstants.kPopupdStyle.fixedHeight),
					tStartDateTimes.Day - 1, NWDDateTimeType.kDaysD);
			}

			tHeightAdd += tHeight + NWDConstants.kFieldMarge;

			//GUI.Label (new Rect (tX, sPos.y + tHeightAdd, tTiersWidthB * 2 + NWDConstants.kFieldMarge - 2, sPos.height), ":", tSeparatorStyle);
			//GUI.Label (new Rect (tX + tTiersWidthB + NWDConstants.kFieldMarge, sPos.y + tHeightAdd, tTiersWidthB * 2 + NWDConstants.kFieldMarge - 2, sPos.height), ":", tSeparatorStyle);

			//int tHourStart = EditorGUI.Popup (new Rect (tX, sPos.y + tHeightAdd, tTiersWidthB, sPos.height),
			//	                 tStartDateTimes.Hour, NWDDateTimeType.kHours);
			//int tMinuteStart = EditorGUI.Popup (new Rect (tX + tTiersWidth, sPos.y + tHeightAdd, tTiersWidthB, sPos.height),
			//	                   tStartDateTimes.Minute, NWDDateTimeType.kMinutes);
			//int tSecondStart = EditorGUI.Popup (new Rect (tX + tTiersWidth * 2, sPos.y + tHeightAdd, tTiersWidthB, sPos.height),
			//	                   tStartDateTimes.Second, NWDDateTimeType.kSeconds);

			//tHeightAdd += tHeight + NWDConstants.kFieldMarge;


            int tHourStart = 0;
            int tMinuteStart = 0;
            int tSecondStart = 0;

            GUI.Label (new Rect (sPos.x + EditorGUIUtility.labelWidth, sPos.y + tHeightAdd, sPos.width, NWDConstants.kLabelStyle.fixedHeight), "End", NWDConstants.kLabelStyle);
			tHeightAdd += tHeightTitle;

            int tYearNext = NWDDateTimeType.kYearStart() + EditorGUI.Popup (new Rect (tX, sPos.y + tHeightAdd, tWidthYear, sPos.height),
                                                                          tEndDateTimes.Year - NWDDateTimeType.kYearStart(), NWDDateTimeType.kYears);

			int tMonthNext = 1 + EditorGUI.Popup (new Rect (tX + tWidthYear + NWDConstants.kFieldMarge, sPos.y + tHeightAdd, tWidthMonth, sPos.height),
				                 tEndDateTimes.Month - 1, NWDDateTimeType.kMonths);

			int tDayNumberNext = DateTime.DaysInMonth (tYearNext, tMonthNext);

			int tDayNext = 1;
			if (tDayNumberNext == 31) {
                tDayNext = 1 + EditorGUI.Popup (new Rect (tX + tWidthYear + tWidthMonth + NWDConstants.kFieldMarge * 2, sPos.y + tHeightAdd, tWidthDay, NWDConstants.kPopupdStyle.fixedHeight),
					tEndDateTimes.Day - 1, NWDDateTimeType.kDays);
			} else if (tDayNumberNext == 30) {
                tDayNext = 1 + EditorGUI.Popup (new Rect (tX + tWidthYear + tWidthMonth + NWDConstants.kFieldMarge * 2, sPos.y + tHeightAdd, tWidthDay, NWDConstants.kPopupdStyle.fixedHeight),
					tEndDateTimes.Day - 1, NWDDateTimeType.kDaysB);
			} else if (tDayNumberNext == 28) {
                tDayNext = 1 + EditorGUI.Popup (new Rect (tX + tWidthYear + tWidthMonth + NWDConstants.kFieldMarge * 2, sPos.y + tHeightAdd, tWidthDay, NWDConstants.kPopupdStyle.fixedHeight),
					tEndDateTimes.Day - 1, NWDDateTimeType.kDaysC);
			} else if (tDayNumberNext == 29) {
                tDayNext = 1 + EditorGUI.Popup (new Rect (tX + tWidthYear + tWidthMonth + NWDConstants.kFieldMarge * 2, sPos.y + tHeightAdd, tWidthDay, NWDConstants.kPopupdStyle.fixedHeight),
					tEndDateTimes.Day - 1, NWDDateTimeType.kDaysD);
			}

			tHeightAdd += tHeight + NWDConstants.kFieldMarge;

			//GUI.Label (new Rect (tX, sPos.y + tHeightAdd, tTiersWidthB * 2 + NWDConstants.kFieldMarge - 2, sPos.height), ":", tSeparatorStyle);
			//GUI.Label (new Rect (tX + tTiersWidthB + NWDConstants.kFieldMarge, sPos.y + tHeightAdd, tTiersWidthB * 2 + NWDConstants.kFieldMarge - 2, sPos.height), ":", tSeparatorStyle);

			//int tHourNext = EditorGUI.Popup (new Rect (tX, sPos.y + tHeightAdd, tTiersWidthB, sPos.height),
			//	                tEndDateTimes.Hour, NWDDateTimeType.kHours);
			//int tMinuteNext = EditorGUI.Popup (new Rect (tX + tTiersWidth, sPos.y + tHeightAdd, tTiersWidthB, sPos.height),
			//	                  tEndDateTimes.Minute, NWDDateTimeType.kMinutes);
			//int tSecondNext = EditorGUI.Popup (new Rect (tX + tTiersWidth * 2, sPos.y + tHeightAdd, tTiersWidthB, sPos.height),
				                  //tEndDateTimes.Second, NWDDateTimeType.kSeconds);

            //tHeightAdd += tHeight + NWDConstants.kFieldMarge;

            // move EditorGUI.indentLevel to draw next controller with indent 
            EditorGUI.indentLevel = tIndentLevel;

            int tHourNext = 0;
            int tMinuteNext = 0;
            int tSecondNext = 0;

            if (AvailableNow () == false) {
                EditorGUI.LabelField (new Rect (sPos.x + 15, sPos.y + tHeight, sPos.width, NWDConstants.kLabelStyle.fixedHeight), NWDScheduleType.kNowFailed);
			} else {
                EditorGUI.LabelField (new Rect (sPos.x + 15, sPos.y + tHeight, sPos.width, NWDConstants.kLabelStyle.fixedHeight), NWDScheduleType.kNowSuccess);
			}



            DateTime tDateTimeInGame = NWDAppEnvironment.SelectedEnvironment().DateTimeInGameTime();
            EditorGUI.LabelField(new Rect(sPos.x + 15, sPos.y + tHeight * 2, sPos.width, sPos.height), NWDScheduleType.kNowGameTime + " (" + NWDAppEnvironment.SelectedEnvironment().SpeedOfGameTime + "x)");
            EditorGUI.LabelField(new Rect(sPos.x + 15, sPos.y + tHeight * 3, sPos.width, sPos.height), tDateTimeInGame.ToString("yyyy-MMM-dd"));
            //GUI.Label(new Rect(sPos.x + 15, sPos.y + tHeight * 5, sPos.width, sPos.height), tDateTimeInGame.ToString("ddd HH:mm:ss"));
            if (AvailableNowInGameTime() == false)
            {
                EditorGUI.LabelField(new Rect(sPos.x + 15, sPos.y + tHeight * 4, sPos.width, sPos.height), NWDScheduleType.kNowGameFailed);
            }
            else
            {
                EditorGUI.LabelField(new Rect(sPos.x + 15, sPos.y + tHeight * 4, sPos.width, sPos.height), NWDScheduleType.kNowGameSuccess);
            }



			tTemporary.Value = tYearStart + NWDConstants.kFieldSeparatorA +
			tMonthStart + NWDConstants.kFieldSeparatorA +
			tDayStart + NWDConstants.kFieldSeparatorA +
			tHourStart + NWDConstants.kFieldSeparatorA +
			tMinuteStart + NWDConstants.kFieldSeparatorA +
			tSecondStart +
			NWDConstants.kFieldSeparatorB +
			tYearNext + NWDConstants.kFieldSeparatorA +
			tMonthNext + NWDConstants.kFieldSeparatorA +
			tDayNext + NWDConstants.kFieldSeparatorA +
			tHourNext + NWDConstants.kFieldSeparatorA +
			tMinuteNext + NWDConstants.kFieldSeparatorA +
			tSecondNext;

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