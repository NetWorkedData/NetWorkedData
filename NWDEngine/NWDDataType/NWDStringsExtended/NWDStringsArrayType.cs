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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;
using UnityEngine;
//=====================================================================================================================
#if UNITY_EDITOR
using UnityEditor;
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDStringsArrayType used to put a reference with float in value. Use properties with name, like 'ItemArray', 'SpotArray', 'BonusArray' , etc.
    /// </summary>
    [SerializeField]
    public class NWDStringsArrayType : NWEDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDStringsArrayType()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDStringsArrayType(string[] sStrings)
        {
            Value = string.Empty;
            this.SetReferences(sStrings);
        }
        //-------------------------------------------------------------------------------------------------------------
        //public bool ContainsReference(string sString)
        //{
        //    if (sString == null)
        //    {
        //        return false;
        //    }
        //    return Value.Contains(NWDToolbox.TextProtect(sString));
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void SetReferences(string[] sReferences)
        {
            List<string> tList = new List<string>();
            foreach (string tReference in sReferences)
            {
                tList.Add(NWDToolbox.TextProtect(tReference));
            }
            string[] tNextValueArray = tList.ToArray();
            Value = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<string> GetStringList()
        {
            List<string> tList = new List<string>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                tList = new List<string>(tValueArray);
            }
            return tList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string[] GetStringArray()
        {
            return GetStringList().ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddReferences(string[] sReferences)
        {
            List<string> tList = new List<string>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                tList = new List<string>(tValueArray);
            }
            foreach (string tReference in sReferences)
            {
                tList.Add(NWDToolbox.TextProtect(tReference));
            }
            string[] tNextValueArray = tList.ToArray();
            Value = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveReferences(string[] sReferences)
        {
            List<string> tList = new List<string>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                tList = new List<string>(tValueArray);
            }
            foreach (string tReference in sReferences)
            {
                tList.Remove(NWDToolbox.TextProtect(tReference));
            }
            string[] tNextValueArray = tList.ToArray();
            Value = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string[] GetReferences()
        {
            List<string> rList = new List<string>();
            string[] tExplode = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string tReference in tExplode)
            {
                rList.Add(NWDToolbox.TextUnprotect(tReference));
            }
            return rList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            int tRow = 1;
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                tRow += tValueArray.Count();
            }
            float tHeight = (NWDGUI.kFieldMarge + NWDGUI.kTextFieldStyle.fixedHeight) * tRow - NWDGUI.kFieldMarge;
            return tHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDStringsArrayType tTemporary = new NWDStringsArrayType();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            tTemporary.Value = Value;
            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;
            List<string> tValueList = new List<string>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                tValueList = new List<string>(tValueArray);
            }
            bool tUp = false;
            bool tDown = false;
            int tIndexToMove = -1;
            tValueList.Add(string.Empty);
            for (int i = 0; i < tValueList.Count; i++)
            {
                if (i > 0)
                {
                    tContent = new GUIContent("   ");
                }
                string tV = tValueList.ElementAt(i);
                tV = EditorGUI.TextField(new Rect(tX, tY, tWidth, NWDGUI.kTextFieldStyle.fixedHeight), tContent, tV);
                if (string.IsNullOrEmpty(tV) == false)
                {
                    if (i > 0)
                    {
                        if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth - (NWDGUI.kUpDownWidth + NWDGUI.kFieldMarge), tY - 1, NWDGUI.kUpDownWidth, NWDGUI.kTextFieldStyle.fixedHeight), NWDGUI.kUpContentIcon, NWDGUI.kIconButtonStyle))
                        {
                            tUp = true;
                            tIndexToMove = i;
                        }
                        if (i < tValueList.Count - 2)
                        {
                            if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth - (NWDGUI.kUpDownWidth + NWDGUI.kFieldMarge) * 2, tY - 1, NWDGUI.kUpDownWidth, NWDGUI.kTextFieldStyle.fixedHeight), NWDGUI.kDownContentIcon, NWDGUI.kIconButtonStyle))
                            {
                                tDown = true;
                                tIndexToMove = i;
                            }
                        }
                    }
                    tValueList[i] = tV;
                }
                else
                {
                    tValueList[i] = string.Empty;
                }
                tY = tY + NWDGUI.kFieldMarge + NWDGUI.kTextFieldStyle.fixedHeight;
            }
            if (tDown == true)
            {
                int tNewIndex = tIndexToMove + 1;
                string tP = tValueList[tIndexToMove];
                tValueList.RemoveAt(tIndexToMove);
                if (tNewIndex >= tValueList.Count)
                {
                    tNewIndex = tValueList.Count - 1;
                }
                if (tNewIndex < 0)
                {
                    tNewIndex = 0;
                }
                tValueList.Insert(tNewIndex, tP);
            }
            if (tUp == true)
            {
                int tNewIndex = tIndexToMove - 1;
                string tP = tValueList[tIndexToMove];
                tValueList.RemoveAt(tIndexToMove);
                if (tNewIndex < 0)
                {
                    tNewIndex = 0;
                }
                tValueList.Insert(tNewIndex, tP);
            }

            List<string> tValuesVerifList = new List<string>();
            foreach (string tText in tValueList)
            {
                if (string.IsNullOrEmpty(tText) == false)
                {
                    tValuesVerifList.Add(tText);
                }
            }
            tValuesVerifList.Remove(NWDConstants.kFieldSeparatorA);
            string[] tNextValueArray = tValuesVerifList.ToArray();
            string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
            tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
            tTemporary.Value = tNextValue;
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        //public string ChangeReferenceForAnother(string sOldReference, string sNewReference)
        //{
        //    string rReturn = "NO";
        //    if (Value != null)
        //    {
        //        if (Value.Contains(sOldReference))
        //        {
        //            //Debug.Log("I CHANGE " + sOldReference + " FOR " + sNewReference + "");
        //            Value = Value.Replace(sOldReference, sNewReference);
        //            rReturn = "YES";
        //        }
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
