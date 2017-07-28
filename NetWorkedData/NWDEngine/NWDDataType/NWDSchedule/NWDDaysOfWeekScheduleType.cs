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
	/// NWD day schedule type. Can determine wich days of week must return a true result or false result
	/// </summary>
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDDaysOfWeekScheduleType : NWDScheduleType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDDaysOfWeekScheduleType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDDaysOfWeekScheduleType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public override bool ResultForDate (DateTime sDateTime)
		{
			bool rReturn = false;
			DayOfWeek tDayOfWeek = sDateTime.DayOfWeek;
			switch (tDayOfWeek) {
			case DayOfWeek.Monday: 
				rReturn = !Value.Contains (kDaysOfWeekSchedulePrefix + "0");
				break;
			case DayOfWeek.Tuesday: 
				rReturn = !Value.Contains (kDaysOfWeekSchedulePrefix + "1");
				break;
			case DayOfWeek.Wednesday: 
				rReturn = !Value.Contains (kDaysOfWeekSchedulePrefix + "2");
				break;
			case DayOfWeek.Thursday: 
				rReturn = !Value.Contains (kDaysOfWeekSchedulePrefix + "3");
				break;
			case DayOfWeek.Friday: 
				rReturn = !Value.Contains (kDaysOfWeekSchedulePrefix + "4");
				break;
			case DayOfWeek.Saturday: 
				rReturn = !Value.Contains (kDaysOfWeekSchedulePrefix + "5");
				break;
			case DayOfWeek.Sunday: 
				rReturn = !Value.Contains (kDaysOfWeekSchedulePrefix + "6");
				break;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
			GUIStyle tToggleStyle = new GUIStyle (EditorStyles.toggle);
			float tHeight = tToggleStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			int tCount = NWDDateType.kDayNames.Length;
			return tHeight * tCount;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override object ControlField (Rect sPos, string sEntitled)
		{
			NWDDaysOfWeekScheduleType tTemporary = new NWDDaysOfWeekScheduleType ();

			GUI.Label (new Rect (sPos.x, sPos.y, sPos.width, sPos.height), sEntitled);
			GUIStyle tToggleStyle = new GUIStyle (EditorStyles.toggle);
			float tHeight = tToggleStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			bool [] tValue = new bool[NWDDateType.kDayNames.Length];
			for (int i=0; i<NWDDateType.kDayNames.Length;i++)
			{
				bool tValueI = GUI.Toggle (new Rect (sPos.x+EditorGUIUtility.labelWidth, sPos.y + tHeight * i, sPos.width, sPos.height),
					!Value.Contains (kDaysOfWeekSchedulePrefix+i.ToString()),
					NWDDateType.kDayNames[i]);
				if (tValueI==false)
				{
					tTemporary.Value += kDaysOfWeekSchedulePrefix+i.ToString();
				}
			}

			if (ResultNow () == false) {
				GUI.Label (new Rect (sPos.x, sPos.y + tHeight, sPos.width, sPos.height), kNowFailed);
			} else {
				GUI.Label (new Rect (sPos.x, sPos.y + tHeight, sPos.width, sPos.height), kNowSuccess);
			}
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override bool ChangeAssetPath (string sOldPath, string sNewPath)
		{
			return false;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================