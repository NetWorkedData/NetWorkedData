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
using System.Globalization;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDMultiTypeEnum : int
    {
        AnString = 0,
        AnBool = 1,
        AnInt = 2,
        AnLong = 3,
        AnFloat = 4,
        AnDouble = 5,

        //AnChar,
        //AnVector,
        //AnRect,
        //AnColor,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
    // TODO : DOC
    //-------------------------------------------------------------------------------------------------------------
    public class NWDMultiType : BTBDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDMultiType()
        {
            Value = ((int)NWDMultiTypeEnum.AnString).ToString() + NWDConstants.kFieldSeparatorA + string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMultiType(string sValue = BTBConstants.K_EMPTY_STRING)
        {
            if (sValue == null)
            {
                Value = ((int)NWDMultiTypeEnum.AnString).ToString() + NWDConstants.kFieldSeparatorA + string.Empty;
            }
            else
            {
                Value = ((int)NWDMultiTypeEnum.AnString).ToString() + NWDConstants.kFieldSeparatorA + sValue;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMultiType(int sValue)
        {
            SetIntValue(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMultiType(long sValue)
        {
            SetLongValue(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMultiType(float sValue)
        {
            SetFloatValue(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMultiType(double sValue)
        {
            SetDoubleValue(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMultiType(bool sValue)
        {
            SetBoolValue(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        //public NWDMultiType(Color sValue)
        //{
        //    SetColor(sValue);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDMultiType(Vector2Int sValue)
        //{
        //    SetVector2Int(sValue);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDMultiType(Vector2 sValue)
        //{
        //    SetVector2(sValue);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDMultiType(Vector3Int sValue)
        //{
        //    SetVector3Int(sValue);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDMultiType(Vector3 sValue)
        //{
        //    SetVector3(sValue);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDMultiType(Vector4 sValue)
        //{
        //    SetVector4(sValue);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDMultiType(RectInt sValue)
        //{
        //    SetRectInt(sValue);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDMultiType(Rect sValue)
        //{
        //    SetRect(sValue);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public NWDMultiType(DateTime sValue)
        //{
        //    SetDateTime(sValue);
        //}
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {

            Value = ((int)NWDMultiTypeEnum.AnString).ToString() + NWDConstants.kFieldSeparatorA + string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
            }
            return Value;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMultiTypeEnum GetTypeValue()
        {
            NWDMultiTypeEnum rReturn = NWDMultiTypeEnum.AnString;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = 0;
                int.TryParse(tComponent[0], out tType);
                rReturn = (NWDMultiTypeEnum)tType;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetStringValue(string sValue)
        {
            Value = ((long)NWDMultiTypeEnum.AnLong).ToString() + NWDConstants.kFieldSeparatorA + sValue;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetStringValue(string sDefault = "")
        {
            string rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = 0;
                int.TryParse(tComponent[0], out tType);
                if (tType == (int)NWDMultiTypeEnum.AnString)
                {
                    rReturn = tComponent[1];
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetIntValue(int sValue)
        {
            Value = ((int)NWDMultiTypeEnum.AnInt).ToString() + NWDConstants.kFieldSeparatorA + sValue.ToString(NWDConstants.FormatCountry);
        }
        //-------------------------------------------------------------------------------------------------------------
        public int GetIntValue(int sDefault = 0)
        {
            int rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = 0;
                int.TryParse(tComponent[0], out tType);
                if (tType == (int)NWDMultiTypeEnum.AnInt)
                {
                    int.TryParse(tComponent[0], out rReturn);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetLongValue(long sValue)
        {
            Value = ((long)NWDMultiTypeEnum.AnLong).ToString() + NWDConstants.kFieldSeparatorA + sValue.ToString(NWDConstants.FormatCountry);
        }
        //-------------------------------------------------------------------------------------------------------------
        public long GetLongValue(long sDefault = 0)
        {
            long rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = 0;
                int.TryParse(tComponent[0], out tType);
                if (tType == (int)NWDMultiTypeEnum.AnLong)
                {
                    long.TryParse(tComponent[0], out rReturn);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetFloatValue(float sValue)
        {
            Value = ((int)NWDMultiTypeEnum.AnDouble).ToString() + NWDConstants.kFieldSeparatorA + sValue.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry);
        }
        //-------------------------------------------------------------------------------------------------------------
        public float GetFloatValue(float sDefault = 0.0F)
        {

            float rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = 0;
                int.TryParse(tComponent[0], out tType);
                if (tType == (int)NWDMultiTypeEnum.AnLong)
                {
                    float.TryParse(tComponent[0], NumberStyles.Float, NWDConstants.FormatCountry, out rReturn);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetDoubleValue(double sValue)
        {
            Value = ((int)NWDMultiTypeEnum.AnDouble).ToString() + NWDConstants.kFieldSeparatorA + sValue.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry);
        }
        //-------------------------------------------------------------------------------------------------------------
        public double GetDoubleValue(double sDefault = 0.0F)
        {
            double rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = 0;
                int.TryParse(tComponent[0], out tType);
                if (tType == (int)NWDMultiTypeEnum.AnDouble)
                {
                    double.TryParse(tComponent[0], NumberStyles.Float, NWDConstants.FormatCountry, out rReturn);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetBoolValue(bool sValue)
        {
            Value = ((int)NWDMultiTypeEnum.AnBool).ToString() + NWDConstants.kFieldSeparatorA + sValue.ToString(NWDConstants.FormatCountry);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool GetBoolValue(bool sDefault = false)
        {
            bool rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = 0;
                int.TryParse(tComponent[0], out tType);
                if (tType == (int)NWDMultiTypeEnum.AnLong)
                {
                    bool.TryParse(tComponent[0], out rReturn);
                }
            }
            return rReturn;
        }
        ////-------------------------------------------------------------------------------------------------------------
        //public Color GetColor(Color sDefault)
        //{
        //	Color rReturn = new Color (sDefault.r, sDefault.g, sDefault.b, sDefault.a);
        //	ColorUtility.TryParseHtmlString (BTBConstants.K_HASHTAG + Value, out rReturn);
        //	return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void SetColor (Color sColor)
        //{
        //	Value = ColorUtility.ToHtmlStringRGBA (sColor);
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      public void SetVector2Int(Vector2Int sVector)
        //      {
        //          Value = sVector.x + NWDConstants.kFieldSeparatorA +
        //                  sVector.y;
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      public Vector2Int GetVector2Int()
        //      {
        //          string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
        //          int tX = 0;
        //          int tY = 0;
        //          if (tFloats.Count() == 2)
        //          {
        //              int.TryParse(tFloats[0], out tX);
        //              int.TryParse(tFloats[1], out tY);
        //          }
        //          Vector2Int rReturn = new Vector2Int(tX, tY);
        //          return rReturn;
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      public void SetVector2(Vector2 sVector)
        //      {
        //          Value = sVector.x.ToString(NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
        //                  sVector.y.ToString(NWDConstants.FormatCountry);
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      public Vector2 GetVector2()
        //      {
        //          string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
        //          float tX = 0.0F;
        //          float tY = 0.0F;
        //          if (tFloats.Count() == 2)
        //          {
        //              float.TryParse(tFloats[0], out tX);
        //              float.TryParse(tFloats[1], out tY);
        //          }
        //          Vector2 rReturn = new Vector2(tX, tY);
        //          return rReturn;
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      public void SetVector3Int(Vector3Int sVector)
        //      {
        //          Value = sVector.x + NWDConstants.kFieldSeparatorA +
        //                  sVector.y + NWDConstants.kFieldSeparatorA +
        //                  sVector.z;
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      public Vector3Int GetVector3Int()
        //      {
        //          string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
        //          int tX = 0;
        //          int tY = 0;
        //          int tZ = 0;
        //          if (tFloats.Count() == 3)
        //          {
        //              int.TryParse(tFloats[0], out tX);
        //              int.TryParse(tFloats[1], out tY);
        //              int.TryParse(tFloats[2], out tZ);
        //          }
        //          Vector3Int rReturn = new Vector3Int(tX, tY, tZ);
        //          return rReturn;
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      public void SetVector3(Vector3 sVector)
        //      {
        //          Value = sVector.x.ToString(NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
        //                  sVector.y.ToString(NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
        //                  sVector.z.ToString(NWDConstants.FormatCountry);
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      public Vector3 GetVector3()
        //      {
        //          string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
        //          float tX = 0.0F;
        //          float tY = 0.0F;
        //          float tZ = 0.0F;
        //          if (tFloats.Count() == 3)
        //          {
        //              float.TryParse(tFloats[0], out tX);
        //              float.TryParse(tFloats[1], out tY);
        //              float.TryParse(tFloats[2], out tZ);
        //          }
        //          Vector3 rReturn = new Vector3(tX, tY, tZ);
        //          return rReturn;
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      public void SetVector4(Vector4 sVector)
        //      {
        //          Value = sVector.x.ToString(NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
        //                  sVector.y.ToString(NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
        //                  sVector.z.ToString(NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
        //                         sVector.w.ToString(NWDConstants.FormatCountry);
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      public Vector4 GetVector4()
        //      {
        //          string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
        //          float tX = 0.0F;
        //          float tY = 0.0F;
        //          float tZ = 0.0F;
        //          float tW = 0.0F;
        //          if (tFloats.Count() == 4)
        //          {
        //              float.TryParse(tFloats[0], out tX);
        //              float.TryParse(tFloats[1], out tY);
        //              float.TryParse(tFloats[2], out tZ);
        //              float.TryParse(tFloats[3], out tW);
        //          }
        //          Vector4 rReturn = new Vector4(tX, tY, tZ, tW);
        //          return rReturn;
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      public void SetRect(Rect sRect)
        //      {
        //          Value = sRect.x.ToString(NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
        //                  sRect.y.ToString(NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
        //                       sRect.height.ToString(NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorA +
        //                       sRect.width.ToString(NWDConstants.FormatCountry);
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      public Rect GetRect()
        //      {
        //          string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
        //          float tX = 0.0F;
        //          float tY = 0.0F;
        //          float tHeight = 0.0F;
        //          float tWidth = 0.0F;
        //          if (tFloats.Count() == 4)
        //          {
        //              float.TryParse(tFloats[0], out tX);
        //              float.TryParse(tFloats[1], out tY);
        //              float.TryParse(tFloats[2], out tHeight);
        //              float.TryParse(tFloats[3], out tWidth);
        //          }
        //          Rect rReturn = new Rect(tX, tY, tHeight, tWidth);
        //          return rReturn;
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      public void SetRectInt(RectInt sRectInt)
        //      {
        //          Value = sRectInt.x + NWDConstants.kFieldSeparatorA +
        //                  sRectInt.y + NWDConstants.kFieldSeparatorA +
        //                       sRectInt.height + NWDConstants.kFieldSeparatorA +
        //                       sRectInt.width;
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      public RectInt GetRectInt()
        //      {
        //          string[] tFloats = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
        //          int tX = 0;
        //          int tY = 0;
        //          int tHeight = 0;
        //          int tWidth = 0;
        //          if (tFloats.Count() == 4)
        //          {
        //              int.TryParse(tFloats[0], out tX);
        //              int.TryParse(tFloats[1], out tY);
        //              int.TryParse(tFloats[2], out tHeight);
        //              int.TryParse(tFloats[3], out tWidth);
        //          }
        //          RectInt rReturn = new RectInt(tX, tY, tHeight, tWidth);
        //          return rReturn;
        //      }
        ////-------------------------------------------------------------------------------------------------------------
        //public void SetDateTime (DateTime sDatetime)
        //{
        //	Value = sDatetime.ToString ();
        //      }
        //      //-------------------------------------------------------------------------------------------------------------
        //      //public void SetDateTimeToUTC(DateTime sDatetime)
        //      //{
        //      //    DateTime tDateTime = sDatetime.ToUniversalTime();
        //      //    Value = sDatetime.ToString();
        //      //}
        //      //-------------------------------------------------------------------------------------------------------------
        //      public DateTime ToDateTime ()
        //{
        //DateTime rReturn = new DateTime (); 
        //DateTime.TryParse (Value, out rReturn);
        //return rReturn;
        //}

        //-------------------------------------------------------------------------------------------------------------
        //public DateTime ToDateTimeUTC()
        //{
        //    DateTime rReturn = new DateTime();
        //    DateTime.TryParse(Value, out rReturn);
        //    return rReturn.ToUniversalTime();
        //}
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            GUIStyle tTextFieldStyle = new GUIStyle(EditorStyles.textField);
            float tHeight = tTextFieldStyle.CalcHeight(new GUIContent("A"), 100.0f);
            return tHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPos, string sEntitled, string sTooltips = "")
        {
            NWDMultiType tTemporary = new NWDMultiType();
            GUIContent tContent = new GUIContent(sEntitled + "*", sTooltips);

            NWDMultiTypeEnum tTypeOfMultiple = GetTypeValue();

            float tX = sPos.x + EditorGUIUtility.labelWidth;
            float tEnumW = 60.0F;
            Rect tPosOfLabel = new Rect(sPos.x, sPos.y, EditorGUIUtility.labelWidth, NWDConstants.kLabelStyle.fixedHeight);
            Rect tPosOfEnum = new Rect(tX, sPos.y, tEnumW, NWDConstants.kPopupdStyle.fixedHeight);
            Rect tPosOfEnter = new Rect(tX + tEnumW + NWDConstants.kFieldMarge, sPos.y, sPos.width - NWDConstants.kFieldMarge - tEnumW - EditorGUIUtility.labelWidth, NWDConstants.kLabelStyle.fixedHeight);

            EditorGUI.LabelField(tPosOfLabel, tContent);
            tTypeOfMultiple = (NWDMultiTypeEnum)EditorGUI.EnumPopup(tPosOfEnum, tTypeOfMultiple);
            switch (tTypeOfMultiple)
            {
                case NWDMultiTypeEnum.AnString:
                    {
                        tTemporary.SetStringValue(EditorGUI.TextField(tPosOfEnter, GetStringValue("")));
                    }
                    break;
                case NWDMultiTypeEnum.AnBool:
                    {
                        tTemporary.SetBoolValue(EditorGUI.Toggle(tPosOfEnter, GetBoolValue(false)));
                    }
                    break;
                case NWDMultiTypeEnum.AnInt:
                    {
                        tTemporary.SetIntValue(EditorGUI.IntField(tPosOfEnter, GetIntValue(0)));
                    }
                    break;
                case NWDMultiTypeEnum.AnLong:
                    {
                        tTemporary.SetLongValue(EditorGUI.LongField(tPosOfEnter, GetLongValue(0)));
                    }
                    break;
                case NWDMultiTypeEnum.AnFloat:
                    {
                        tTemporary.SetFloatValue(EditorGUI.FloatField(tPosOfEnter, GetFloatValue(0.0F)));
                    }
                    break;
                case NWDMultiTypeEnum.AnDouble:
                    {
                        tTemporary.SetDoubleValue(EditorGUI.DoubleField(tPosOfEnter, GetDoubleValue(0.0F)));
                    }
                    break;
            }
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================