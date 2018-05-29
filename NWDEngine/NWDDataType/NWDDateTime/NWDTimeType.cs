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
	//TODO: FINISH THIS CLASS NWDTimeType
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDTimeType : BTBDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDTimeType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDTimeType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = "";
        }
		//-------------------------------------------------------------------------------------------------------------
		public void SetDateTime (DateTime sDatetime)
		{
			Value = 1970+NWDConstants.kFieldSeparatorA+
				1+NWDConstants.kFieldSeparatorA+
				1+NWDConstants.kFieldSeparatorA+
				sDatetime.Hour+NWDConstants.kFieldSeparatorA+
				sDatetime.Minute+NWDConstants.kFieldSeparatorA+
				sDatetime.Second;
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

			DateTime rReturn = new DateTime(tYear, tMonth,tDay,tHour,tMinute,tSecond);
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
        public override object ControlField (Rect sPos, string sEntitled, string sTooltips = "")
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

			DateTime tDateTime = new DateTime(tYear, tMonth,tDay,tHour,tMinute,tSecond);

			float tTiersWidth = Mathf.Ceil( (sPos.width - EditorGUIUtility.labelWidth + NWDConstants.kFieldMarge) / 3.0F);
			float tTiersWidthB = tTiersWidth - NWDConstants.kFieldMarge;
//			float tTiersWidthC = tTiersWidth - NWDConstants.kFieldMarge*3;
			float tHeightAdd = 0;

//			float tWidthYear = tTiersWidthB + 10;
//			float tWidthMonth = tTiersWidthB -5;
//			float tWidthDay = tTiersWidthB -5;
            EditorGUI.LabelField (new Rect (sPos.x, sPos.y, sPos.width, NWDConstants.kLabelStyle.fixedHeight), tContent);

            // remove EditorGUI.indentLevel to draw next controller without indent 
            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            GUI.Label (new Rect (tX , sPos.y+tHeightAdd,tTiersWidthB*2+NWDConstants.kFieldMarge-2, NWDConstants.kSeparatorStyle.fixedHeight), ":",NWDConstants.kSeparatorStyle);
            GUI.Label (new Rect (tX + tTiersWidthB + NWDConstants.kFieldMarge, sPos.y+tHeightAdd, tTiersWidthB*2+NWDConstants.kFieldMarge-2, NWDConstants.kSeparatorStyle.fixedHeight), ":", NWDConstants.kSeparatorStyle);

            tHour = EditorGUI.Popup (new Rect (tX, sPos.y + tHeightAdd, tTiersWidthB, NWDConstants.kPopupdStyle.fixedHeight),
				tDateTime.Hour, NWDDateTimeType.kHours);
            tMinute = EditorGUI.Popup (new Rect (tX +tTiersWidth, sPos.y + tHeightAdd, tTiersWidthB, NWDConstants.kPopupdStyle.fixedHeight),
				tDateTime.Minute, NWDDateTimeType.kMinutes);
            tSecond = EditorGUI.Popup (new Rect (tX +tTiersWidth*2, sPos.y + tHeightAdd, tTiersWidthB,NWDConstants.kPopupdStyle.fixedHeight),
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