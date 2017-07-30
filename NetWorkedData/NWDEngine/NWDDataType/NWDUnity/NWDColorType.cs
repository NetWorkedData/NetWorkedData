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
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDColorType : BTBDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDColorType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDColorType (string sValue = "00000000")
		{
			if (sValue == null) {
				Value = "00000000";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
		public Color GetColor ()
		{
			Color tColor = new Color ();
			ColorUtility.TryParseHtmlString ("#" + Value, out tColor);
			return tColor;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetColor (Color sColor)
		{
			Value = ColorUtility.ToHtmlStringRGBA (sColor);
		}
		//-------------------------------------------------------------------------------------------------------------
		#if UNITY_EDITOR
		//-------------------------------------------------------------------------------------------------------------
		public override float ControlFieldHeight ()
		{
			GUIStyle tColorFieldStyle = new GUIStyle (EditorStyles.colorField);
			return tColorFieldStyle.CalcHeight (new GUIContent ("A"), 100);
		}
		//-------------------------------------------------------------------------------------------------------------
		public override object ControlField (Rect sPosition, string sEntitled)
		{
			NWDColorType tTemporary = new NWDColorType ();
			tTemporary.SetColor (EditorGUI.ColorField (sPosition, sEntitled, GetColor ()));
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================