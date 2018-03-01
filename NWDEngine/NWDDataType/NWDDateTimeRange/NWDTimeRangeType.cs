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
	/// NWD time range type. This type can reccord two times (hour:minute:second) and determine if another time is between this range.
	/// </summary>
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDTimeRangeType : BTBDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NetWorkedData.NWDTimeRangeType"/> class.
		/// </summary>
		public NWDTimeRangeType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Initializes a new instance of the <see cref="NetWorkedData.NWDTimeRangeType"/> class.
		/// </summary>
		/// <param name="sValue">S value.</param>
		public NWDTimeRangeType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Results if "now" is in range.
		/// </summary>
		/// <returns><c>true</c>, if now was resulted, <c>false</c> otherwise.</returns>
		public bool ResultNow ()
		{
			return ResultForDate (DateTime.Now);
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Results for date if the time of this date is in range (only the hour:minute:second).
		/// </summary>
		/// <returns><c>true</c>, if for date was resulted, <c>false</c> otherwise.</returns>
		/// <param name="sDateTime">S date time.</param>
		public bool ResultForDate (DateTime sDateTime)
		{
			DateTime tDateExtract = new DateTime (1970, 1, 1, sDateTime.Hour, sDateTime.Minute, sDateTime.Second);
			int tDate = NWDToolbox.Timestamp (tDateExtract);
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
		/// Set start and end time.
		/// </summary>
		/// <param name="sStartDatetime">S start datetime.</param>
		/// <param name="sEndDatetime">S end datetime.</param>
		public void SetDateTime (DateTime sStartDatetime, DateTime sEndDatetime)
		{
			Value = 
				sStartDatetime.Hour + NWDConstants.kFieldSeparatorA +
			sStartDatetime.Minute + NWDConstants.kFieldSeparatorA +
			sStartDatetime.Second +
			NWDConstants.kFieldSeparatorB +
			sEndDatetime.Hour + NWDConstants.kFieldSeparatorA +
			sEndDatetime.Minute + NWDConstants.kFieldSeparatorA +
			sEndDatetime.Second;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// return start time.
		/// </summary>
		/// <returns>The date time.</returns>
		public DateTime StartDateTime ()
		{
			return ToDateTime () [0];
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// return end time.
		/// </summary>
		/// <returns>The date time.</returns>
		public DateTime EndDateTime ()
		{
			return ToDateTime () [1];
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// return the datetime of range
		/// </summary>
		/// <returns>The date time.</returns>
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
				if (tDateComponent.Count () == 3) {
					int.TryParse (tDateComponent [0], out tHour);
					int.TryParse (tDateComponent [1], out tMinute);
					int.TryParse (tDateComponent [2], out tSecond);
				}
				// test result of parsing 
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
			GUIStyle tPopupStyle = new GUIStyle (EditorStyles.popup);
			float tHeight = tPopupStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			GUIStyle tLabelStyle = new GUIStyle (EditorStyles.boldLabel);
			float tHeightTitle = tLabelStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			return tHeight * 4 + NWDConstants.kFieldMarge * 3 + tHeightTitle * 2;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// The field to edit value in editor.
		/// </summary>
		/// <returns>The field.</returns>
		/// <param name="sPosition">S position.</param>
		/// <param name="sEntitled">S entitled.</param>
		/// <param name="sPos">S position.</param>
        public override object ControlField (Rect sPos, string sEntitled, string sTooltips = "")
		{
            NWDTimeRangeType tTemporary = new NWDTimeRangeType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
			GUIStyle tPopupStyle = new GUIStyle (EditorStyles.popup);

			GUIStyle tSeparatorStyle = new GUIStyle (EditorStyles.label);
			tSeparatorStyle.alignment = TextAnchor.MiddleCenter;
			float tHeight = tPopupStyle.CalcHeight (new GUIContent ("A"), 100.0f);

			GUIStyle tLabelStyle = new GUIStyle (EditorStyles.boldLabel);
			float tHeightTitle = tLabelStyle.CalcHeight (new GUIContent ("A"), 100.0f);

//			string[] tDateComponent = Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);

			DateTime[] tDateTimes = ToDateTime ();
			DateTime tStartDateTimes = tDateTimes [0];
			DateTime tEndDateTimes = tDateTimes [1];

			float tX = sPos.x + EditorGUIUtility.labelWidth;

			float tTiersWidth = Mathf.Ceil ((sPos.width - EditorGUIUtility.labelWidth + NWDConstants.kFieldMarge) / 3.0F);
			float tTiersWidthB = tTiersWidth - NWDConstants.kFieldMarge;
//			float tTiersWidthC = tTiersWidth - NWDConstants.kFieldMarge * 3;
			float tHeightAdd = 0;

//			float tWidthYear = tTiersWidthB + 10;
//			float tWidthMonth = tTiersWidthB - 5;
//			float tWidthDay = tTiersWidthB - 5;
			GUI.Label (new Rect (sPos.x, sPos.y, sPos.width, sPos.height), sEntitled);

			GUI.Label (new Rect (sPos.x + EditorGUIUtility.labelWidth, sPos.y, sPos.width, sPos.height), "Start", tLabelStyle);
			tHeightAdd += tHeightTitle;

			GUI.Label (new Rect (tX, sPos.y + tHeightAdd, tTiersWidthB * 2 + NWDConstants.kFieldMarge - 2, sPos.height), ":", tSeparatorStyle);
			GUI.Label (new Rect (tX + tTiersWidthB + NWDConstants.kFieldMarge, sPos.y + tHeightAdd, tTiersWidthB * 2 + NWDConstants.kFieldMarge - 2, sPos.height), ":", tSeparatorStyle);

			int tHourStart = EditorGUI.Popup (new Rect (tX, sPos.y + tHeightAdd, tTiersWidthB, sPos.height),
				                 tStartDateTimes.Hour, NWDDateTimeType.kHours);
			int tMinuteStart = EditorGUI.Popup (new Rect (tX + tTiersWidth, sPos.y + tHeightAdd, tTiersWidthB, sPos.height),
				                   tStartDateTimes.Minute, NWDDateTimeType.kMinutes);
			int tSecondStart = EditorGUI.Popup (new Rect (tX + tTiersWidth * 2, sPos.y + tHeightAdd, tTiersWidthB, sPos.height),
				                   tStartDateTimes.Second, NWDDateTimeType.kSeconds);

			tHeightAdd += tHeight + NWDConstants.kFieldMarge;

			GUI.Label (new Rect (sPos.x + EditorGUIUtility.labelWidth, sPos.y + tHeightAdd, sPos.width, sPos.height), "End", tLabelStyle);
			tHeightAdd += tHeightTitle;

			GUI.Label (new Rect (tX, sPos.y + tHeightAdd, tTiersWidthB * 2 + NWDConstants.kFieldMarge - 2, sPos.height), ":", tSeparatorStyle);
			GUI.Label (new Rect (tX + tTiersWidthB + NWDConstants.kFieldMarge, sPos.y + tHeightAdd, tTiersWidthB * 2 + NWDConstants.kFieldMarge - 2, sPos.height), ":", tSeparatorStyle);

			int tHourNext = EditorGUI.Popup (new Rect (tX, sPos.y + tHeightAdd, tTiersWidthB, sPos.height),
				                tEndDateTimes.Hour, NWDDateTimeType.kHours);
			int tMinuteNext = EditorGUI.Popup (new Rect (tX + tTiersWidth, sPos.y + tHeightAdd, tTiersWidthB, sPos.height),
				                  tEndDateTimes.Minute, NWDDateTimeType.kMinutes);
			int tSecondNext = EditorGUI.Popup (new Rect (tX + tTiersWidth * 2, sPos.y + tHeightAdd, tTiersWidthB, sPos.height),
				                  tEndDateTimes.Second, NWDDateTimeType.kSeconds);

			tHeightAdd += tHeight + NWDConstants.kFieldMarge;

			if (ResultNow () == false) {
				GUI.Label (new Rect (sPos.x, sPos.y + tHeight, sPos.width, sPos.height), NWDScheduleType.kNowFailed);
			} else {
				GUI.Label (new Rect (sPos.x, sPos.y + tHeight, sPos.width, sPos.height), NWDScheduleType.kNowSuccess);
			}

			tTemporary.Value = 
			tHourStart + NWDConstants.kFieldSeparatorA +
			tMinuteStart + NWDConstants.kFieldSeparatorA +
			tSecondStart +
			NWDConstants.kFieldSeparatorB +
			tHourNext + NWDConstants.kFieldSeparatorA +
			tMinuteNext + NWDConstants.kFieldSeparatorA +
			tSecondNext;

			//GUI.Label (new Rect (sPos.x, sPos.y+tHeightAdd, sPos.width, sPos.height), Value);

			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================