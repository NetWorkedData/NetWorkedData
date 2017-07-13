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
	//TODO: FINISH THIS CLASS NWDCalendarType
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDCalendarType : BTBDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDCalendarType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDCalendarType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public Time ToTime ()
		{
			return new Time ();
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
			NWDCalendarType tTemporary = new NWDCalendarType ();
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================