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
		//-------------------------------------------------------------------------------------------------------------
		public NWDScheduleType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDScheduleType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public virtual bool ResultNow () 
		{
			return ResultForDate (DateTime.Now);
		}
		//-------------------------------------------------------------------------------------------------------------
		public virtual bool ResultForDate (DateTime sDateTime)
		{
			return false;
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
			GUIStyle tPopupdStyle = new GUIStyle (EditorStyles.popup);
			float tHeight = tPopupdStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			return tHeight;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, string sTooltips = "")
		{
            NWDScheduleType tTemporary = new NWDScheduleType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================