// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:28:35
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
	/// NWD day schedule type. Can determine wich days of week must return a true result or false result
	/// </summary>
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDDaysOfWeekScheduleType : NWDScheduleType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDDaysOfWeekScheduleType ()
		{
			Value = string.Empty;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDDaysOfWeekScheduleType (string sValue = BTBConstants.K_EMPTY_STRING)
		{
			if (sValue == null) {
				Value = string.Empty;
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public override bool ResultForDate (DateTime sDateTime)
		{
			bool rReturn = true;
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
			GUIStyle tLabelStyle = new GUIStyle (EditorStyles.boldLabel);
			float tHeightTitle = tLabelStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			int tCount = NWDDateTimeType.kDayNames.Length;
			return tHeight * tCount + tHeightTitle;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPos, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING, object sAdditionnal = null)
		{
            NWDDaysOfWeekScheduleType tTemporary = new NWDDaysOfWeekScheduleType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            EditorGUI.LabelField (new Rect (sPos.x, sPos.y, sPos.width, sPos.height), tContent);
			GUIStyle tToggleStyle = new GUIStyle (EditorStyles.toggle);
			float tHeight = tToggleStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			GUIStyle tLabelStyle = new GUIStyle (EditorStyles.boldLabel);
			float tHeightTitle = tLabelStyle.CalcHeight (new GUIContent ("A"), 100.0f);

			float tHeightAdd = 0;

			GUI.Label (new Rect (sPos.x+EditorGUIUtility.labelWidth, sPos.y, sPos.width, sPos.height), "Days of week selection", tLabelStyle);
			tHeightAdd += tHeightTitle;

//			bool [] tValue = new bool[NWDDateTimeType.kDayNames.Length];
			for (int i=0; i<NWDDateTimeType.kDayNames.Length;i++)
			{
				bool tValueI = GUI.Toggle (new Rect (sPos.x+EditorGUIUtility.labelWidth, sPos.y + tHeightAdd +tHeight * i, sPos.width, sPos.height),
					!Value.Contains (kDaysOfWeekSchedulePrefix+i.ToString()),
					NWDDateTimeType.kDayNames[i]);
				if (tValueI==false)
				{
					tTemporary.Value += kDaysOfWeekSchedulePrefix+i.ToString();
				}
			}

            if (base.AvailableNow() == false)
            {
                EditorGUI.LabelField(new Rect(sPos.x + 15, sPos.y + tHeight, sPos.width, sPos.height), kNowFailed);
            }
            else
            {
                EditorGUI.LabelField(new Rect(sPos.x + 15, sPos.y + tHeight, sPos.width, sPos.height), kNowSuccess);
            }

            DateTime tDateTimeInGame = NWDAppEnvironment.SelectedEnvironment().DateTimeInGameTime();
            EditorGUI.LabelField(new Rect(sPos.x + 15, sPos.y + tHeight * 3, sPos.width, sPos.height), kNowGameTime + " (" + NWDAppEnvironment.SelectedEnvironment().SpeedOfGameTime + "x)");
            EditorGUI.LabelField(new Rect(sPos.x + 15, sPos.y + tHeight * 4, sPos.width, sPos.height), tDateTimeInGame.ToString("yyyy-MMM-dd"));
            EditorGUI.LabelField(new Rect(sPos.x + 15, sPos.y + tHeight * 5, sPos.width, sPos.height), tDateTimeInGame.ToString("ddd HH:mm:ss"));
            if (base.AvailableNowInGameTime() == false)
            {
                EditorGUI.LabelField(new Rect(sPos.x + 15, sPos.y + tHeight * 6, sPos.width, sPos.height), kNowGameFailed);
            }
            else
            {
                EditorGUI.LabelField(new Rect(sPos.x + 15, sPos.y + tHeight * 6, sPos.width, sPos.height), kNowGameSuccess);
            }

            //EditorGUI.TextField(new Rect(sPos.x, sPos.y + tHeight * 9, sPos.width, sPos.height), tTemporary.Value);
            //EditorGUI.TextField(new Rect(sPos.x, sPos.y + tHeight * 10, sPos.width, sPos.height), StringResultOfDate(tDateTimeInGame));
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================