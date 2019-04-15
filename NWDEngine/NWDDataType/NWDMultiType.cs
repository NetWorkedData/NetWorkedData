// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:2
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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

        AnColor = 9,

        AnVector2 = 20,
        AnVector2Int = 21,
        AnVector3 = 22,
        AnVector3Int = 23,
        AnVector4 = 24,

        //AnRect = 30,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
    public class NWDMultiType : BTBDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDMultiType()
        {
            Value = ((int)NWDMultiTypeEnum.AnString).ToString() + NWDConstants.kFieldSeparatorD + string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMultiType(string sValue = BTBConstants.K_EMPTY_STRING)
        {
            if (string.IsNullOrEmpty(sValue))
            {
                SetStringValue(string.Empty);
            }
            else
            {
                SetStringValue(sValue);
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
        public override void Default()
        {
            Value = ((int)NWDMultiTypeEnum.AnString).ToString() + NWDConstants.kFieldSeparatorD + string.Empty;
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
        public override string ToString()
        {
            return Value;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMultiTypeEnum GetTypeValue()
        {
            NWDMultiTypeEnum rReturn = NWDMultiTypeEnum.AnString;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);
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
            Value = ((int)NWDMultiTypeEnum.AnString).ToString() + NWDConstants.kFieldSeparatorD + NWDToolbox.TextProtect(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetStringValue(string sDefault = "")
        {
            string rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = 0;
                int.TryParse(tComponent[0], out tType);
                if (tType == (int)NWDMultiTypeEnum.AnString)
                {
                    rReturn = tComponent[1];
                }
            }
            return NWDToolbox.TextUnprotect(rReturn);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetIntValue(int sValue)
        {
            Value = ((int)NWDMultiTypeEnum.AnInt).ToString() + NWDConstants.kFieldSeparatorD + NWDToolbox.IntToString(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public int GetIntValue(int sDefault = 0)
        {
            int rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = NWDToolbox.IntFromString(tComponent[0]);
                if (tType == (int)NWDMultiTypeEnum.AnInt)
                {
                    rReturn = NWDToolbox.IntFromString(tComponent[1]);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetLongValue(long sValue)
        {
            Value = ((int)NWDMultiTypeEnum.AnLong).ToString() + NWDConstants.kFieldSeparatorD + NWDToolbox.LongToString(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public long GetLongValue(long sDefault = 0)
        {
            long rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = NWDToolbox.IntFromString(tComponent[0]);
                if (tType == (int)NWDMultiTypeEnum.AnLong)
                {
                    rReturn = NWDToolbox.LongFromString(tComponent[1]);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetFloatValue(float sValue)
        {
            Value = ((int)NWDMultiTypeEnum.AnFloat).ToString() + NWDConstants.kFieldSeparatorD + NWDToolbox.FloatToString(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public float GetFloatValue(float sDefault = 0.0F)
        {

            float rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = NWDToolbox.IntFromString(tComponent[0]);
                if (tType == (int)NWDMultiTypeEnum.AnFloat)
                {
                    rReturn = NWDToolbox.FloatFromString(tComponent[1]);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetDoubleValue(double sValue)
        {
            Value = ((int)NWDMultiTypeEnum.AnDouble).ToString() + NWDConstants.kFieldSeparatorD + NWDToolbox.DoubleToString(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public double GetDoubleValue(double sDefault = 0.0F)
        {
            double rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = NWDToolbox.IntFromString(tComponent[0]);
                if (tType == (int)NWDMultiTypeEnum.AnDouble)
                {
                    rReturn = NWDToolbox.DoubleFromString(tComponent[1]);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetBoolValue(bool sValue)
        {
            Value = ((int)NWDMultiTypeEnum.AnBool).ToString() + NWDConstants.kFieldSeparatorD + NWDToolbox.BoolToString(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool GetBoolValue(bool sDefault = false)
        {
            bool rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = NWDToolbox.IntFromString(tComponent[0]);
                if (tType == (int)NWDMultiTypeEnum.AnBool)
                {
                    rReturn = NWDToolbox.BoolFromString(tComponent[1]);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetColorValue(Color sValue)
        {
            Value = ((int)NWDMultiTypeEnum.AnColor).ToString() + NWDConstants.kFieldSeparatorD + NWDToolbox.ColorToString(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public Color GetColorValue(Color sDefault)
        {
            Color rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = NWDToolbox.IntFromString(tComponent[0]);
                if (tType == (int)NWDMultiTypeEnum.AnColor)
                {
                    rReturn = NWDToolbox.ColorFromString(tComponent[1]);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetVector2Value(Vector2 sValue)
        {
            Value = ((int)NWDMultiTypeEnum.AnVector2).ToString() + NWDConstants.kFieldSeparatorD + NWDToolbox.Vector2ToString(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public Vector2 GetVector2Value(Vector2 sDefault)
        {
            Vector2 rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = NWDToolbox.IntFromString(tComponent[0]);
                if (tType == (int)NWDMultiTypeEnum.AnVector2)
                {
                    rReturn = NWDToolbox.Vector2FromString(tComponent[1]);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetVector2IntValue(Vector2Int sValue)
        {
            Value = ((int)NWDMultiTypeEnum.AnVector2Int).ToString() + NWDConstants.kFieldSeparatorD + NWDToolbox.Vector2IntToString(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public Vector2Int GetVector2IntValue(Vector2Int sDefault)
        {
            Vector2Int rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = NWDToolbox.IntFromString(tComponent[0]);
                if (tType == (int)NWDMultiTypeEnum.AnVector2Int)
                {
                    rReturn = NWDToolbox.Vector2IntFromString(tComponent[1]);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetVector3Value(Vector3 sValue)
        {
            Value = ((int)NWDMultiTypeEnum.AnVector3).ToString() + NWDConstants.kFieldSeparatorD + NWDToolbox.Vector3ToString(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public Vector3 GetVector3Value(Vector3 sDefault)
        {
            Vector3 rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = NWDToolbox.IntFromString(tComponent[0]);
                if (tType == (int)NWDMultiTypeEnum.AnVector3)
                {
                    rReturn = NWDToolbox.Vector3FromString(tComponent[1]);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetVector3IntValue(Vector3Int sValue)
        {
            Value = ((int)NWDMultiTypeEnum.AnVector3Int).ToString() + NWDConstants.kFieldSeparatorD + NWDToolbox.Vector3IntToString(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public Vector3Int GetVector3IntValue(Vector3Int sDefault)
        {
            Vector3Int rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = NWDToolbox.IntFromString(tComponent[0]);
                if (tType == (int)NWDMultiTypeEnum.AnVector3Int)
                {
                    rReturn = NWDToolbox.Vector3IntFromString(tComponent[1]);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetVector4Value(Vector4 sValue)
        {
            Value = ((int)NWDMultiTypeEnum.AnVector4).ToString() + NWDConstants.kFieldSeparatorD + NWDToolbox.Vector4ToString(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public Vector4 GetVector4Value(Vector4 sDefault)
        {
            Vector4 rReturn = sDefault;
            string[] tComponent = Value.Split(new string[] { NWDConstants.kFieldSeparatorD }, StringSplitOptions.RemoveEmptyEntries);
            if (tComponent.Length == 2)
            {
                int tType = NWDToolbox.IntFromString(tComponent[0]);
                if (tType == (int)NWDMultiTypeEnum.AnVector4)
                {
                    rReturn = NWDToolbox.Vector4FromString(tComponent[1]);
                }
            }
            return rReturn;
        }
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
        public override object ControlField(Rect sPosition, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            // prepare next value
            NWDMultiType tTemporary = new NWDMultiType();
            // entitled content
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            // prepare positions values
            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;
            float tEnumWidth = EditorGUIUtility.labelWidth + NWDGUI.kEnumWidth;

            // field for value part 1
            NWDMultiTypeEnum tTypeOfMultiple = GetTypeValue();
            tTypeOfMultiple = (NWDMultiTypeEnum)EditorGUI.EnumPopup(new Rect(tX, tY, tEnumWidth, NWDGUI.kPopupStyle.fixedHeight), tContent, tTypeOfMultiple);

            // prevent indentation
            int tIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            // field for value part 2
            Rect tPosOfEnter = new Rect(tX + tEnumWidth + NWDGUI.kFieldMarge, tY, tWidth - tEnumWidth - NWDGUI.kFieldMarge, NWDGUI.kTextFieldStyle.fixedHeight);
            // use multi type value
            switch (tTypeOfMultiple)
            {
                case NWDMultiTypeEnum.AnString:
                    {
                        tTemporary.SetStringValue(EditorGUI.TextField(tPosOfEnter, GetStringValue("")));
                    }
                    break;
                case NWDMultiTypeEnum.AnBool:
                    {
                        bool tValue = GetBoolValue(false);
                        tTemporary.SetBoolValue(EditorGUI.ToggleLeft(tPosOfEnter, tValue.ToString(), tValue));
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
                case NWDMultiTypeEnum.AnColor:
                    {
                        tTemporary.SetColorValue(EditorGUI.ColorField(tPosOfEnter, GetColorValue(Color.white)));
                    }
                    break;
                case NWDMultiTypeEnum.AnVector2:
                    {
                        tTemporary.SetVector2Value(EditorGUI.Vector2Field(tPosOfEnter, "", GetVector2Value(Vector2.zero)));
                    }
                    break;
                case NWDMultiTypeEnum.AnVector2Int:
                    {
                        tTemporary.SetVector2IntValue(EditorGUI.Vector2IntField(tPosOfEnter, "", GetVector2IntValue(Vector2Int.zero)));
                    }
                    break;
                case NWDMultiTypeEnum.AnVector3:
                    {
                        tTemporary.SetVector3Value(EditorGUI.Vector3Field(tPosOfEnter, "", GetVector3Value(Vector3.zero)));
                    }
                    break;
                case NWDMultiTypeEnum.AnVector3Int:
                    {
                        tTemporary.SetVector3IntValue(EditorGUI.Vector3IntField(tPosOfEnter, "", GetVector3IntValue(Vector3Int.zero)));
                    }
                    break;
                case NWDMultiTypeEnum.AnVector4:
                    {
                        tTemporary.SetVector4Value(EditorGUI.Vector4Field(tPosOfEnter, "", GetVector4Value(Vector2.zero)));
                    }
                    break;
            }
            // restaure indentation
            EditorGUI.indentLevel = tIndentLevel;
            // return next value
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================