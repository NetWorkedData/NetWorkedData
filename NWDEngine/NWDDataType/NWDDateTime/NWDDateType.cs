//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:27:29
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
	//TODO: FINISH THIS CLASS NWDDateType
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDDateType : NWEDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDDateType ()
		{
			Value = string.Empty;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDDateType (string sValue = NWEConstants.K_EMPTY_STRING)
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
            SetDateTime(NWEDateHelper.ConvertFromTimestamp(0));
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
        public void SetDateTime(DateTime sDatetime)
        {
            sDatetime = sDatetime.ToLocalTime();
            Value = sDatetime.Year+NWDConstants.kFieldSeparatorA+
				    sDatetime.Month+NWDConstants.kFieldSeparatorA+
				    sDatetime.Day+NWDConstants.kFieldSeparatorA;
		}
        //-------------------------------------------------------------------------------------------------------------
        public void SetCurrentDate()
        {
            SetDateTime(DateTime.Now);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetTimestamp(double sTimestamp)
        {
            SetDateTime(NWEDateHelper.ConvertFromTimestamp(sTimestamp));
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

			DateTime rReturn = new DateTime(tYear, tMonth,tDay,tHour,tMinute,tSecond, DateTimeKind.Local);
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
            return NWDGUI.kPopupStyle.fixedHeight;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPos, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
		{
            NWDDateType tTemporary = new NWDDateType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
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

			DateTime tDateTime = new DateTime(tYear, tMonth,tDay,tHour,tMinute,tSecond, DateTimeKind.Local);

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

			tTemporary.Value = tYear+NWDConstants.kFieldSeparatorA+
				tMonth+NWDConstants.kFieldSeparatorA+
				tDay;

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