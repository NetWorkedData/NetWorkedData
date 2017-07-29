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
		public override object ControlField (Rect sPosition, string sEntitled)
		{
			NWDScheduleType tTemporary = new NWDScheduleType ();
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		/// <summary>
		/// Return if the asset path is used in this DataType.
		/// </summary>
		/// <returns><c>true</c>, if asset path was changed, <c>false</c> otherwise.</returns>
		/// <param name="sOldPath">old path.</param>
		/// <param name="sNewPath">new path.</param>
		public override bool ChangeAssetPath (string sOldPath, string sNewPath) {
			return false;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================