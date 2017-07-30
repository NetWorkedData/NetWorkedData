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
	//TODO: FINISH THIS CLASS NWDDateType
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDDateType : BTBDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDDateType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDDateType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetDateTime (DateTime sDatetime)
		{
			Value = sDatetime.Year+NWDConstants.kFieldSeparatorA+
				sDatetime.Month+NWDConstants.kFieldSeparatorA+
				sDatetime.Day+NWDConstants.kFieldSeparatorA+
				sDatetime.Hour+NWDConstants.kFieldSeparatorA+
				sDatetime.Minute+NWDConstants.kFieldSeparatorA+
				sDatetime.Second;
		}
		//-------------------------------------------------------------------------------------------------------------
		public DateTime ToDateTime ()
		{
			string[] tDateComponent=Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
			int tYear = 1970;
			int tMonth = 1;
			int tDay = 1;
			int tHour = 0;
			int tMinute = 0;
			int tSecond = 0;
			if (tDateComponent.Count() == 3) {
				int.TryParse(tDateComponent [0], out tYear);
				int.TryParse(tDateComponent [1], out tMonth);
				int.TryParse(tDateComponent [2],out tDay);
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

			DateTime rReturn = new DateTime(tYear, tMonth,tDay,tHour,tMinute,tSecond);
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
			GUIStyle tPopupStyle = new GUIStyle (EditorStyles.popup);
			float tHeight = tPopupStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			return tHeight;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override object ControlField (Rect sPos, string sEntitled)
		{
			NWDDateType tTemporary = new NWDDateType ();
//			GUIStyle tPopupStyle = new GUIStyle (EditorStyles.popup);

			GUIStyle tSeparatorStyle = new GUIStyle (EditorStyles.label);
			tSeparatorStyle.alignment = TextAnchor.MiddleCenter;
//			float tHeight = tPopupStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			string[] tDateComponent=Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
			int tYear = 1970;
			int tMonth = 1;
			int tDay = 1;
			int tHour = 0;
			int tMinute = 0;
			int tSecond = 0;
			if (tDateComponent.Count() == 3) {
				int.TryParse(tDateComponent [0], out tYear);
				int.TryParse(tDateComponent [1], out tMonth);
				int.TryParse(tDateComponent [2],out tDay);
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

			float tX = sPos.x + EditorGUIUtility.labelWidth;

			DateTime tDateTime = new DateTime(tYear, tMonth,tDay,tHour,tMinute,tSecond);

			float tTiersWidth = Mathf.Ceil( (sPos.width - EditorGUIUtility.labelWidth + NWDConstants.kFieldMarge) / 3.0F);
			float tTiersWidthB = tTiersWidth - NWDConstants.kFieldMarge;
//			float tTiersWidthC = tTiersWidth - NWDConstants.kFieldMarge*3;
			float tHeightAdd = 0;

			float tWidthYear = tTiersWidthB + 10;
			float tWidthMonth = tTiersWidthB -5;
			float tWidthDay = tTiersWidthB -5;
			GUI.Label (new Rect (sPos.x, sPos.y, sPos.width, sPos.height), sEntitled);

			tYear = 1900+ EditorGUI.Popup (new Rect (tX, sPos.y + tHeightAdd, tWidthYear, sPos.height),
				tDateTime.Year - 1900, NWDDateTimeType.kYears);

			tMonth = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +NWDConstants.kFieldMarge, sPos.y + tHeightAdd, tWidthMonth, sPos.height),
				tDateTime.Month-1, NWDDateTimeType.kMonths);

			int tDayNumber = DateTime.DaysInMonth(tYear,tMonth);

			if (tDayNumber == 31 )
			{
				tDay = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +tWidthMonth+NWDConstants.kFieldMarge*2, sPos.y + tHeightAdd, tWidthDay, sPos.height),
					tDateTime.Day-1, NWDDateTimeType.kDays);
			}
			else if (tDayNumber == 30)
			{
				tDay = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +tWidthMonth+NWDConstants.kFieldMarge*2, sPos.y + tHeightAdd, tWidthDay, sPos.height),
					tDateTime.Day-1, NWDDateTimeType.kDaysB);
			}
			else if (tDayNumber == 28)
			{
				tDay = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +tWidthMonth+NWDConstants.kFieldMarge*2, sPos.y + tHeightAdd, tWidthDay, sPos.height),
					tDateTime.Day-1, NWDDateTimeType.kDaysC);
			}
			else if (tDayNumber == 29)
			{
				tDay = 1+ EditorGUI.Popup (new Rect (tX+tWidthYear +tWidthMonth+NWDConstants.kFieldMarge*2, sPos.y + tHeightAdd, tWidthDay, sPos.height),
					tDateTime.Day-1, NWDDateTimeType.kDaysD);
			}

			tTemporary.Value = tYear+NWDConstants.kFieldSeparatorA+
				tMonth+NWDConstants.kFieldSeparatorA+
				tDay;

			//GUI.Label (new Rect (sPos.x, sPos.y+tHeightAdd, sPos.width, sPos.height), Value);

			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================