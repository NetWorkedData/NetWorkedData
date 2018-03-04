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
	public class NWDHoursScheduleType : NWDScheduleType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDHoursScheduleType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDHoursScheduleType (string sValue = "")
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
			int tHour = sDateTime.Hour;
			rReturn = !Value.Contains (kHoursSchedulePrefix + tHour.ToString("00"));
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
			return tHeight * 8 + tHeightTitle;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, string sTooltips = "")
		{
            NWDHoursScheduleType tTemporary = new NWDHoursScheduleType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);

            EditorGUI.LabelField (new Rect (sPosition.x, sPosition.y, sPosition.width, sPosition.height), tContent);
			GUIStyle tToggleStyle = new GUIStyle (EditorStyles.toggle);
			float tHeight = tToggleStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			GUIStyle tLabelStyle = new GUIStyle (EditorStyles.boldLabel);
			float tHeightTitle = tLabelStyle.CalcHeight (new GUIContent ("A"), 100.0f);

			float tTiersWidth = Mathf.Ceil( (sPosition.width - EditorGUIUtility.labelWidth) / 3.0F);



			float tHeightAdd = 0;

			GUI.Label (new Rect (sPosition.x+EditorGUIUtility.labelWidth, sPosition.y, sPosition.width, sPosition.height), "Hours selection", tLabelStyle);
			tHeightAdd += tHeightTitle;


			for (int i=0; i<NWDDateTimeType.kHours.Length;i++)
			{
				int c = i % 8;
				int l = (i - c) / 8;
				bool tValueTest = GUI.Toggle (new Rect (sPosition.x+EditorGUIUtility.labelWidth + l*tTiersWidth, sPosition.y + tHeightAdd+ tHeight * c, tTiersWidth, sPosition.height),
					!Value.Contains (kHoursSchedulePrefix+i.ToString("00")),
					NWDDateTimeType.kHours[i]+"H");
				if (tValueTest==false)
				{
					tTemporary.Value += kHoursSchedulePrefix+i.ToString("00");
				}
			}


            if (base.AvailableNow() == false)
            {
                EditorGUI.LabelField(new Rect(sPosition.x + 15, sPosition.y + tHeight, sPosition.width, sPosition.height), kNowFailed);
            }
            else
            {
                EditorGUI.LabelField(new Rect(sPosition.x + 15, sPosition.y + tHeight, sPosition.width, sPosition.height), kNowSuccess);
            }

            DateTime tDateTimeInGame = NWDAppEnvironment.SelectedEnvironment().DateTimeInGameTime();
            EditorGUI.LabelField(new Rect(sPosition.x + 15, sPosition.y + tHeight * 3, sPosition.width, sPosition.height), kNowGameTime + " (" + NWDAppEnvironment.SelectedEnvironment().SpeedOfGameTime + "x)");
            EditorGUI.LabelField(new Rect(sPosition.x + 15, sPosition.y + tHeight * 4, sPosition.width, sPosition.height), tDateTimeInGame.ToString("yyyy-MMM-dd"));
            EditorGUI.LabelField(new Rect(sPosition.x + 15, sPosition.y + tHeight * 5, sPosition.width, sPosition.height), tDateTimeInGame.ToString("ddd HH:mm:ss"));
            if (base.AvailableNowInGameTime() == false)
            {
                EditorGUI.LabelField(new Rect(sPosition.x + 15, sPosition.y + tHeight * 6, sPosition.width, sPosition.height), kNowGameFailed);
            }
            else
            {
                EditorGUI.LabelField(new Rect(sPosition.x + 15, sPosition.y + tHeight * 6, sPosition.width, sPosition.height), kNowGameSuccess);
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