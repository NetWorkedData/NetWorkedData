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
	// TODO : DOC
	//-------------------------------------------------------------------------------------------------------------
	public class NWDMultiType : BTBDataType
	{
		//-------------------------------------------------------------------------------------------------------------
		public NWDMultiType ()
		{
			Value = string.Empty;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWDMultiType (string sValue = BTBConstants.K_EMPTY_STRING)
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
            Value = string.Empty;
        }
		//-------------------------------------------------------------------------------------------------------------
		public override string ToString()
		{
			return Value;
		}
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
			if (Value.ToUpper () == "FALSE" || Value.ToUpper () == "NO" || Value == "0") {
				rReturn = false;
			} else if (Value.ToUpper () == "TRUE" || Value.ToUpper () == "YES" || Value == "1") {
				rReturn = true;
			}
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetBool(bool sValue)
		{
			Value = sValue.ToString();
		}
		//-------------------------------------------------------------------------------------------------------------
		public Color ToColor(Color sDefault)
		{
			Color rReturn = new Color (sDefault.r, sDefault.g, sDefault.b, sDefault.a);
			ColorUtility.TryParseHtmlString (BTBConstants.K_HASHTAG + Value, out rReturn);
			return rReturn;
		}
		//-------------------------------------------------------------------------------------------------------------
		public void SetColor (Color sColor)
		{
			Value = ColorUtility.ToHtmlStringRGBA (sColor);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetVector2Int(Vector2Int sVector)
        {
            Value = sVector.x + NWDConstants.kFieldSeparatorA +
                    sVector.y;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Vector2Int GetVector2Int()
        {
            string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            int tX = 0;
            int tY = 0;
            if (tFloats.Count() == 2)
            {
                int.TryParse(tFloats[0], out tX);
                int.TryParse(tFloats[1], out tY);
            }
            Vector2Int rReturn = new Vector2Int(tX, tY);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetVector2(Vector2 sVector)
        {
            Value = sVector.x + NWDConstants.kFieldSeparatorA +
                    sVector.y;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Vector2 GetVector2()
        {
            string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            float tX = 0.0F;
            float tY = 0.0F;
            if (tFloats.Count() == 2)
            {
                float.TryParse(tFloats[0], out tX);
                float.TryParse(tFloats[1], out tY);
            }
            Vector2 rReturn = new Vector2(tX, tY);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetVector3Int(Vector3Int sVector)
        {
            Value = sVector.x + NWDConstants.kFieldSeparatorA +
                    sVector.y + NWDConstants.kFieldSeparatorA +
                    sVector.z;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Vector3Int GetVector3Int()
        {
            string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            int tX = 0;
            int tY = 0;
            int tZ = 0;
            if (tFloats.Count() == 3)
            {
                int.TryParse(tFloats[0], out tX);
                int.TryParse(tFloats[1], out tY);
                int.TryParse(tFloats[2], out tZ);
            }
            Vector3Int rReturn = new Vector3Int(tX, tY, tZ);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetVector3(Vector3 sVector)
        {
            Value = sVector.x + NWDConstants.kFieldSeparatorA +
                    sVector.y + NWDConstants.kFieldSeparatorA +
                    sVector.z;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Vector3 GetVector3()
        {
            string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            float tX = 0.0F;
            float tY = 0.0F;
            float tZ = 0.0F;
            if (tFloats.Count() == 3)
            {
                float.TryParse(tFloats[0], out tX);
                float.TryParse(tFloats[1], out tY);
                float.TryParse(tFloats[2], out tZ);
            }
            Vector3 rReturn = new Vector3(tX, tY, tZ);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetVector4(Vector4 sVector)
        {
            Value = sVector.x + NWDConstants.kFieldSeparatorA +
                    sVector.y + NWDConstants.kFieldSeparatorA +
                    sVector.z + NWDConstants.kFieldSeparatorA +
                           sVector.w;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Vector4 GetVector4()
        {
            string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            float tX = 0.0F;
            float tY = 0.0F;
            float tZ = 0.0F;
            float tW = 0.0F;
            if (tFloats.Count() == 4)
            {
                float.TryParse(tFloats[0], out tX);
                float.TryParse(tFloats[1], out tY);
                float.TryParse(tFloats[2], out tZ);
                float.TryParse(tFloats[3], out tW);
            }
            Vector4 rReturn = new Vector4(tX, tY, tZ, tW);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetRect(Rect sRect)
        {
            Value = sRect.x + NWDConstants.kFieldSeparatorA +
                    sRect.y + NWDConstants.kFieldSeparatorA +
                         sRect.height + NWDConstants.kFieldSeparatorA +
                         sRect.width;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Rect GetRect()
        {
            string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            float tX = 0.0F;
            float tY = 0.0F;
            float tHeight = 0.0F;
            float tWidth = 0.0F;
            if (tFloats.Count() == 4)
            {
                float.TryParse(tFloats[0], out tX);
                float.TryParse(tFloats[1], out tY);
                float.TryParse(tFloats[2], out tHeight);
                float.TryParse(tFloats[3], out tWidth);
            }
            Rect rReturn = new Rect(tX, tY, tHeight, tWidth);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetRectInt(RectInt sRectInt)
        {
            Value = sRectInt.x + NWDConstants.kFieldSeparatorA +
                    sRectInt.y + NWDConstants.kFieldSeparatorA +
                         sRectInt.height + NWDConstants.kFieldSeparatorA +
                         sRectInt.width;
        }
        //-------------------------------------------------------------------------------------------------------------
        public RectInt GetRectInt()
        {
            string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            int tX = 0;
            int tY = 0;
            int tHeight = 0;
            int tWidth = 0;
            if (tFloats.Count() == 4)
            {
                int.TryParse(tFloats[0], out tX);
                int.TryParse(tFloats[1], out tY);
                int.TryParse(tFloats[2], out tHeight);
                int.TryParse(tFloats[3], out tWidth);
            }
            RectInt rReturn = new RectInt(tX, tY, tHeight, tWidth);
            return rReturn;
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
			GUIStyle tTextFieldStyle = new GUIStyle (EditorStyles.textField);
			float tHeight = tTextFieldStyle.CalcHeight (new GUIContent ("A"), 100.0f);
			return tHeight;
		}
		//-------------------------------------------------------------------------------------------------------------
        public override object ControlField (Rect sPosition, string sEntitled, string sTooltips = "")
		{
            NWDMultiType tTemporary = new NWDMultiType ();
            GUIContent tContent = new GUIContent(sEntitled+ "*", sTooltips);
            string tNextValue = EditorGUI.TextField (sPosition , tContent, Value);
			tTemporary.Value = tNextValue;
			return tTemporary;
		}
		//-------------------------------------------------------------------------------------------------------------
		#endif
		//-------------------------------------------------------------------------------------------------------------
	}
}
//=====================================================================================================================