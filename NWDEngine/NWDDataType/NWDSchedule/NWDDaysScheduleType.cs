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
	public class NWDDaysScheduleType : NWDScheduleType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDDaysScheduleType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDDaysScheduleType (string sValue = "")
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
				int tDays = sDateTime.Day-1;
				rReturn = !Value.Contains (kDaysSchedulePrefix + tDays.ToString("00"));
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
			int tCountD = 11;
			return tHeight * (tCountD) + tHeightTitle;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override object ControlField (Rect sPos, string sEntitled)
		{
			NWDDaysScheduleType tTemporary = new NWDDaysScheduleType ();

			GUI.Label (new Rect (sPos.x, sPos.y, sPos.width, sPos.height), sEntitled);
			GUIStyle tToggleStyle = new GUIStyle (EditorStyles.toggle);
			float tHeight = tToggleStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			GUIStyle tLabelStyle = new GUIStyle (EditorStyles.boldLabel);
			float tHeightTitle = tLabelStyle.CalcHeight (new GUIContent ("A"), 100.0f);

			float tTiersWidth = Mathf.Ceil( (sPos.width - EditorGUIUtility.labelWidth) / 3.0F);
			float tHeightAdd = 0;

			GUI.Label (new Rect (sPos.x+EditorGUIUtility.labelWidth, sPos.y, sPos.width, sPos.height), "Days of month selection", tLabelStyle);
			tHeightAdd += tHeightTitle;

//			GUI.Label (new Rect (sPos.x+EditorGUIUtility.labelWidth, sPos.y+tHeightAdd, sPos.width, sPos.height), "Dates of day", tLabelStyle);
//			tHeightAdd += tHeightTitle;
			for (int i=0; i<NWDDateTimeType.kDays.Length;i++)
			{
				int c = i % 11;
				int l = (i - c) / 11;
				bool tValueTest = GUI.Toggle (new Rect (sPos.x+EditorGUIUtility.labelWidth + l*tTiersWidth, sPos.y +tHeightAdd+ tHeight * c, tTiersWidth, sPos.height),
					!Value.Contains (kDaysSchedulePrefix+i.ToString("00")),
					NWDDateTimeType.kDays[i]);
				if (tValueTest==false)
				{
					tTemporary.Value += kDaysSchedulePrefix+i.ToString("00");
				}
			}

			if (ResultNow () == false) {
				GUI.Label (new Rect (sPos.x, sPos.y + tHeight, sPos.width, sPos.height), kNowFailed);
			} else {
				GUI.Label (new Rect (sPos.x, sPos.y + tHeight, sPos.width, sPos.height), kNowSuccess);
			}
			//GUI.Label (new Rect (sPos.x, sPos.y + tHeight *2, sPos.width, sPos.height), Value);
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================