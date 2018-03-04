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
	public class NWDMonthsScheduleType : NWDScheduleType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDMonthsScheduleType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDMonthsScheduleType (string sValue = "")
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
				int tMonth = sDateTime.Month-1;
				rReturn = !Value.Contains (kMonthsSchedulePrefix + tMonth.ToString("00"));
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
			int tCountB = NWDDateTimeType.kMonths.Length;
			return tHeight * tCountB + tHeightTitle;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPos, string sEntitled, string sTooltips = "")
		{
            NWDMonthsScheduleType tTemporary = new NWDMonthsScheduleType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            GUI.Label (new Rect (sPos.x, sPos.y, sPos.width, sPos.height), tContent);
			GUIStyle tToggleStyle = new GUIStyle (EditorStyles.toggle);

			GUIStyle tLabelStyle = new GUIStyle (EditorStyles.boldLabel);
			float tHeightTitle = tLabelStyle.CalcHeight (new GUIContent ("A"), 100.0f);

			float tHeightAdd = 0;

			GUI.Label (new Rect (sPos.x+EditorGUIUtility.labelWidth, sPos.y, sPos.width, sPos.height), "Month selection", tLabelStyle);
			tHeightAdd += tHeightTitle;


			float tHeight = tToggleStyle.CalcHeight (new GUIContent ("A"), 100.0f);

			for (int i=0; i<NWDDateTimeType.kMonths.Length;i++)
			{
				bool tValueTest = GUI.Toggle (new Rect (sPos.x+EditorGUIUtility.labelWidth, sPos.y + tHeightAdd+ tHeight * i, sPos.width, sPos.height),
					!Value.Contains (kMonthsSchedulePrefix+i.ToString("00")),
					NWDDateTimeType.kMonths[i]);
				if (tValueTest==false)
				{
					tTemporary.Value += kMonthsSchedulePrefix+i.ToString("00");
				}
			}


            if (base.AvailableNow() == false)
            {
                GUI.Label(new Rect(sPos.x + 15, sPos.y + tHeight, sPos.width, sPos.height), kNowFailed);
            }
            else
            {
                GUI.Label(new Rect(sPos.x + 15, sPos.y + tHeight, sPos.width, sPos.height), kNowSuccess);
            }

            DateTime tDateTimeInGame = NWDAppEnvironment.SelectedEnvironment().DateTimeInGameTime();
            GUI.Label(new Rect(sPos.x + 15, sPos.y + tHeight * 3, sPos.width, sPos.height), kNowGameTime + " (" + NWDAppEnvironment.SelectedEnvironment().SpeedOfGameTime + "x)");
            GUI.Label(new Rect(sPos.x + 15, sPos.y + tHeight * 4, sPos.width, sPos.height), tDateTimeInGame.ToString("yyyy-MMM-dd"));
            GUI.Label(new Rect(sPos.x + 15, sPos.y + tHeight * 5, sPos.width, sPos.height), tDateTimeInGame.ToString("ddd HH:mm:ss"));
            if (base.AvailableNowInGameTime() == false)
            {
                GUI.Label(new Rect(sPos.x + 15, sPos.y + tHeight * 6, sPos.width, sPos.height), kNowGameFailed);
            }
            else
            {
                GUI.Label(new Rect(sPos.x + 15, sPos.y + tHeight * 6, sPos.width, sPos.height), kNowGameSuccess);
            }

            //GUI.Label (new Rect (sPos.x, sPos.y + tHeight *9, sPos.width, sPos.height), Value);
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================