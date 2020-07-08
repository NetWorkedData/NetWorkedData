//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#endif
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
	//TODO: FINISH THIS CLASS NWDTimeType
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDTimeType : NWEDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDTimeType ()
		{
			Value = string.Empty;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDTimeType (string sValue = NWEConstants.K_EMPTY_STRING)
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
        public void SetDateTime (DateTime sDatetime)
        {
            sDatetime = sDatetime.ToLocalTime();
            Value = 1970+NWDConstants.kFieldSeparatorA+
				1+NWDConstants.kFieldSeparatorA+
				1+NWDConstants.kFieldSeparatorA+
				sDatetime.Hour+NWDConstants.kFieldSeparatorA+
				sDatetime.Minute+NWDConstants.kFieldSeparatorA+
				sDatetime.Second;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetCurrentTime()
        {
            SetDateTime(DateTime.Now);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetTimeStamp (double sTimestamp)
        {
            SetDateTime(NWEDateHelper.ConvertFromTimestamp(sTimestamp));
        }
        //-------------------------------------------------------------------------------------------------------------
        public DateTime ToDateTime ()
		{
			string[] tDateComponent=Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
			int tYear = DateTime.Now.Year;
			int tMonth = DateTime.Now.Month;
			int tDay = DateTime.Now.Day;
			int tHour = 0;
			int tMinute = 0;
			int tSecond = 0;
			if (tDateComponent.Count() == 3) {
				int.TryParse(tDateComponent [0], out tHour);
				int.TryParse(tDateComponent [1], out tMinute);
				int.TryParse(tDateComponent [2], out tSecond);
			}
			// test result of parsing 
			if (tHour < 0 || tHour > 23 ) {
				tHour = 0;
			}
			if (tMinute < 0 || tMinute > 59 ) {
				tMinute = 0;
			}
			if (tSecond < 0 || tSecond > 59 ) {
				tSecond = 0;
			}

			DateTime rReturn = new DateTime(tYear, tMonth,tDay,tHour,tMinute,tSecond, DateTimeKind.Local);
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
        public override object ControlField (Rect sPos, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
		{
            NWDTimeType tTemporary = new NWDTimeType ();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
//			GUIStyle tPopupStyle = new GUIStyle (EditorStyles.popup);

			//GUIStyle tSeparatorStyle = new GUIStyle (EditorStyles.label);
			//tSeparatorStyle.alignment = TextAnchor.MiddleCenter;
//			float tHeight = tPopupStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			string[] tDateComponent=Value.Split (new string[]{ NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
			int tYear = 1970;
			int tMonth = 1;
			int tDay = 1;
			int tHour = 0;
			int tMinute = 0;
			int tSecond = 0;
			if (tDateComponent.Count() == 3) {
				int.TryParse(tDateComponent [0], out tHour);
				int.TryParse(tDateComponent [1], out tMinute);
				int.TryParse(tDateComponent [2], out tSecond);
			}
			// test result of parsing 
			if (tHour < 0 || tHour > 23 ) {
				tHour = 0;
			}
			if (tMinute < 0 || tMinute > 59 ) {
				tMinute = 0;
			}
			if (tSecond < 0 || tSecond > 59 ) {
				tSecond = 0;
			}

			float tX = sPos.x + EditorGUIUtility.labelWidth;

			DateTime tDateTime = new DateTime(tYear, tMonth,tDay,tHour,tMinute,tSecond, DateTimeKind.Local);

			float tTiersWidth = Mathf.Ceil( (sPos.width - EditorGUIUtility.labelWidth + NWDGUI.kFieldMarge) / 3.0F);
			float tTiersWidthB = tTiersWidth - NWDGUI.kFieldMarge;
//			float tTiersWidthC = tTiersWidth - NWDGUI.kFieldMarge*3;
			float tHeightAdd = 0;

//			float tWidthYear = tTiersWidthB + 10;
//			float tWidthMonth = tTiersWidthB -5;
//			float tWidthDay = tTiersWidthB -5;
            EditorGUI.LabelField (new Rect (sPos.x, sPos.y, sPos.width, NWDGUI.kLabelStyle.fixedHeight), tContent);

            // remove EditorGUI.indentLevel to draw next controller without indent 
            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            GUI.Label (new Rect (tX , sPos.y+tHeightAdd,tTiersWidthB*2+NWDGUI.kFieldMarge-2, NWDGUI.kLabelStyle.fixedHeight), ":",NWDGUI.kLabelStyle);
            GUI.Label (new Rect (tX + tTiersWidthB + NWDGUI.kFieldMarge, sPos.y+tHeightAdd, tTiersWidthB*2+NWDGUI.kFieldMarge-2, NWDGUI.kLabelStyle.fixedHeight), ":", NWDGUI.kLabelStyle);

            tHour = EditorGUI.Popup (new Rect (tX, sPos.y + tHeightAdd, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight),
				tDateTime.Hour, NWDDateTimeType.kHours);
            tMinute = EditorGUI.Popup (new Rect (tX +tTiersWidth, sPos.y + tHeightAdd, tTiersWidthB, NWDGUI.kPopupStyle.fixedHeight),
				tDateTime.Minute, NWDDateTimeType.kMinutes);
            tSecond = EditorGUI.Popup (new Rect (tX +tTiersWidth*2, sPos.y + tHeightAdd, tTiersWidthB,NWDGUI.kPopupStyle.fixedHeight),
				tDateTime.Second, NWDDateTimeType.kSeconds);
			tTemporary.Value = tHour+NWDConstants.kFieldSeparatorA+
				tMinute+NWDConstants.kFieldSeparatorA+
				tSecond;

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