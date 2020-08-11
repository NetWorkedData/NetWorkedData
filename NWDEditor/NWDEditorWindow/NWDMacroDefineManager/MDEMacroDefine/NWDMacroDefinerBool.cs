﻿//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;
using UnityEditor;
//=====================================================================================================================
namespace NetWorkedData.MacroDefine
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [Serializable]
    public class MDEDataTypeBoolGeneric<K> : NWDMacroBoolDefiner where K : MDEDataTypeBoolGeneric<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        static readonly Dictionary<long, K> kList = new Dictionary<long, K>();
        private static string kTitle = string.Empty;
        private static string kGroup = string.Empty;
        private static int kOrder = 0;
        //-------------------------------------------------------------------------------------------------------------
        public MDEDataTypeBoolGeneric()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override List<string> StringValuesArray()
        {
            List<string> rList = new List<string>();
            foreach (KeyValuePair<long, K> tKeyPair in kList)
            {
                string tName = string.Empty;
                if (tKeyPair.Value != null)
                {
                    tName = tKeyPair.Value.Name;
                }
                if (string.IsNullOrEmpty(tName) == false)
                {
                    rList.Add(tName);
                }
            }
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override List<string> StringValuesArrayAdd()
        {
            List<string> rList = new List<string>();
            foreach (KeyValuePair<long, K> tKeyPair in kList)
            {
                string tName = string.Empty;
                if (tKeyPair.Value != null)
                {
                    tName = tKeyPair.Value.Name;
                    if (tKeyPair.Value.Addon != null)
                    {
                        tName = tName + ";" + tKeyPair.Value.Addon;
                    }
                }
                if (string.IsNullOrEmpty(tName) == false)
                {
                    rList.Add(tName);
                }
            }
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = "")
        {
            NWDMacroDefiner tTemporary = new NWDMacroDefiner();
            tTemporary.Value = Value;
            List<long> kListIndex = new List<long>();
            List<K> kListK = new List<K>();
            int tSelection = -1;
            tSelection = kListIndex.IndexOf(Value);
            if (tSelection < 0)
            {
                tSelection = 0;
            }
            bool tSelected = false;
            if (tSelection == 1)
            {
                tSelected = true;
            }
            tSelected = EditorGUI.ToggleLeft(sPosition, sEntitled, tSelected);
            if (tSelected == false)
            {
                tSelection = 0;
            }
            else
            {
                tSelection = 1;
            }
            tTemporary.Value = kListIndex[tSelection];
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string SetTitle(string sTitle)
        {
            kTitle = sTitle;
            return kTitle;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string SetGroup(string sGroup)
        {
            kGroup = sGroup;
            return kGroup;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static int SetOrder(int sOrder)
        {
            kOrder = sOrder;
            return kOrder;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string GetTitle()
        {
            if (string.IsNullOrEmpty(kTitle))
            {
                kTitle = GetType().Name;
            }
            return kTitle;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string GetGroup()
        {
            return kGroup;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override int GetOrder()
        {
            return kOrder;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static K GetForValue(long sID)
        {
            return kList[sID];
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static K SetValue(string sName, string sAddList = null)
        {
            AddNone();
            return Add(1, sName, null, true, sAddList);
        }
        //-------------------------------------------------------------------------------------------------------------
        private static K AddNone()
        {
            return Add(0, MDEConstants.NONE, MDEConstants.NONE, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        protected static K Add(int sID, string sName, string sRepresentation, bool sOverridable, string sAddList = null)
        {
            sName = MDEMacroDefineEditor.UnixCleaner(sName);
            K rReturn;
            if (kList.ContainsKey(sID) == false)
            {
                rReturn = Activator.CreateInstance(typeof(K)) as K;
                rReturn.Value = sID;
                rReturn.Name = sName;
                rReturn.Representation = sRepresentation;
                rReturn.Overridable = sOverridable;
                if (string.IsNullOrEmpty(sAddList) == false)
                {
                    string[] sList = sAddList.Split(new char[] { ';', ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                    rReturn.Addon = string.Join(";", sList);
                }
                kList.Add(sID, rReturn);
            }
            else
            {
                rReturn = kList[sID];
                if (rReturn.Overridable == true)
                {
                    rReturn.Name = sName;
                    rReturn.Representation = sRepresentation;
                }
            }
            return rReturn as K;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override string ToString()
        {
            if (kList.ContainsKey(Value) == true)
            {
                K tParent = kList[Value];
                if (string.IsNullOrEmpty(tParent.Representation))
                {
                    if (string.IsNullOrEmpty(tParent.Name))
                    {
                        return tParent.Value.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        return tParent.Name;
                    }
                }
                else
                {
                    return tParent.Representation;
                }
            }
            else
            {
                return Value.ToString(CultureInfo.InvariantCulture);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [CustomPropertyDrawer(typeof(MDEDataTypeBoolDrawer<>))]
    public class MDEDataTypeBoolDrawer<K> : PropertyDrawer where K : MDEDataTypeBoolGeneric<K>, new()
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            K tTarget = fieldInfo.GetValue(property.serializedObject.targetObject) as K;
            EditorGUI.BeginChangeCheck();
            NWDMacroDefiner tResult = tTarget.ControlField(position, property.displayName, false, property.tooltip) as NWDMacroDefiner;
            if (EditorGUI.EndChangeCheck())
            {
                K tTargetFinal = MDEDataTypeBoolGeneric<K>.GetForValue(tResult.Value);
                fieldInfo.SetValue(property.serializedObject.targetObject, tTargetFinal);
                property.serializedObject.ApplyModifiedProperties();
                EditorUtility.SetDirty(property.serializedObject.targetObject);
            }
            EditorGUI.EndProperty();
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif