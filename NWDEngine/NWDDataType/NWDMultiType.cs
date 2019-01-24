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
            Value = ((int)NWDMultiTypeEnum.AnString).ToString() + NWDConstants.kFieldSeparatorD + string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDMultiType(string sValue = BTBConstants.K_EMPTY_STRING)
        {
            if (string.IsNullOrEmpty(sValue))
            {
                Value = ((int)NWDMultiTypeEnum.AnString).ToString() + NWDConstants.kFieldSeparatorD + string.Empty;
            }
            else
            {
                Value = ((int)NWDMultiTypeEnum.AnString).ToString() + NWDConstants.kFieldSeparatorD + sValue;
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
            Value = ((long)NWDMultiTypeEnum.AnLong).ToString() + NWDConstants.kFieldSeparatorD + NWDToolbox.TextProtect(sValue);
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
            Value = ((long)NWDMultiTypeEnum.AnLong).ToString() + NWDConstants.kFieldSeparatorD + NWDToolbox.LongToString(sValue);
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
            Value = ((int)NWDMultiTypeEnum.AnDouble).ToString() + NWDConstants.kFieldSeparatorD + NWDToolbox.FloatToString(sValue);
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