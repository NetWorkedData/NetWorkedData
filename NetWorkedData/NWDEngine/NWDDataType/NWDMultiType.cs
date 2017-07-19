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
	//TODO: FINISH THIS CLASS NWDDateType
	[SerializeField]
	//-------------------------------------------------------------------------------------------------------------
	public class NWDMultiType : BTBDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDMultiType ()
		{
			Value = "";
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDMultiType (string sValue = "")
		{
			if (sValue == null) {
				Value = "";
			} else {
				Value = sValue;
			}
		}
		//-------------------------------------------------------------------------------------------------------------
//		public string ToString()
//		{
//			return Value;
//		}
		//-------------------------------------------------------------------------------------------------------------
//		public void SetString(string sValue)
//		{
//			Value = sValue;
//		}
		//-------------------------------------------------------------------------------------------------------------
		public int ToInt(int sDefault=0)
		{
			int rReturn = sDefault;
			int.TryParse (Value, out rReturn);
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetInt(int sValue)
		{
			Value = sValue.ToString();
		}
		//-------------------------------------------------------------------------------------------------------------
		public float ToFloat(float sDefault=0.0F)
		{
			float rReturn = sDefault;
			float.TryParse (Value, out rReturn);
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetFloat(float sValue)
		{
			Value = sValue.ToString();
		}
		//-------------------------------------------------------------------------------------------------------------
		public bool ToBool(bool sDefault=false)
		{
			bool rReturn = sDefault;
			bool.TryParse (Value, out rReturn);
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetBool(bool sValue)
		{
			Value = sValue.ToString();
		}
		//-------------------------------------------------------------------------------------------------------------
		static Color kToColorDefault = Color.white;
		public Color ToColor(Color sDefault)
		{
			Color rReturn = new Color (sDefault.r, sDefault.g, sDefault.b, sDefault.a);
			ColorUtility.TryParseHtmlString ("#" + Value, out rReturn);
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetColor (Color sColor)
		{
			Value = ColorUtility.ToHtmlStringRGBA (sColor);
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetDateTime (DateTime sDatetime)
		{
			Value = sDatetime.ToString ();
		}
		//-------------------------------------------------------------------------------------------------------------
		public DateTime ToDateTime ()
		{
			DateTime rReturn = new DateTime (); 
			DateTime.TryParse (Value, out rReturn);
			return rReturn;
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
			NWDDateType tTemporary = new NWDDateType ();
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================