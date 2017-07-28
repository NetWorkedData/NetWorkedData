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
			int tCountB = NWDDateType.kMonths.Length;
			return tHeight * tCountB;
		}
		//-------------------------------------------------------------------------------------------------------------
		public override object ControlField (Rect sPos, string sEntitled)
		{
			NWDMonthsScheduleType tTemporary = new NWDMonthsScheduleType ();

			GUI.Label (new Rect (sPos.x, sPos.y, sPos.width, sPos.height), sEntitled);
			GUIStyle tToggleStyle = new GUIStyle (EditorStyles.toggle);
			float tHeight = tToggleStyle.CalcHeight (new GUIContent ("A"), 100.0f);

			for (int i=0; i<NWDDateType.kMonths.Length;i++)
			{
				bool tValueTest = GUI.Toggle (new Rect (sPos.x+EditorGUIUtility.labelWidth, sPos.y + tHeight * i, sPos.width, sPos.height),
					!Value.Contains (kMonthsSchedulePrefix+i.ToString("00")),
					NWDDateType.kMonths[i]);
				if (tValueTest==false)
				{
					tTemporary.Value += kMonthsSchedulePrefix+i.ToString("00");
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