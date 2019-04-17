// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:28:24
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

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
    public enum NWDConditional : int
    {
        EqualTo = 0,
        UpperThan = 1,
        UpperThanOrEqual = 2,
        LowerThan = 3,
        LowerThanOrEqual = 4,
        DifferentTo = 5,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDReferenceConditionalType used to put a reference with float in value. Use properties with name, like 'ItemConditional', 'SpotConditional', 'BonusConditional' , etc.
    /// </summary>
    [SerializeField]
    public class NWDReferenceConditionalType<K> : NWDReferenceMultiple where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Reference;
        public int Quantity;
        public NWDConditional Condition;
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferenceConditionalType(string sValue)
        {
            string[] tLineValue = sValue.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
            if (tLineValue.Length == 2)
            {
                Reference = tLineValue[0];
                Quantity = NWDToolbox.IntFromString(tLineValue[1]);
                //int.TryParse(tLineValue[1], System.Globalization.NumberStyles.Integer, NWDConstants.FormatCountry, out Quantity);
                Condition = NWDConditional.EqualTo;
            }
            if (tLineValue.Length == 3)
            {
                Reference = tLineValue[0];
                Quantity = NWDToolbox.IntFromString(tLineValue[1]);
                //int.TryParse(tLineValue[1], System.Globalization.NumberStyles.Integer, NWDConstants.FormatCountry, out Quantity);
                Condition = (NWDConditional)Enum.Parse(typeof(NWDConditional), tLineValue[2], true);
            }
            ReEvalute();
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferenceConditionalType(string sReference, int sQuantity, NWDConditional sCondition)
        {
            Reference = sReference;
            Quantity = sQuantity;
            Condition = sCondition;
            ReEvalute();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReEvalute()
        {
            Value = Reference + NWDConstants.kFieldSeparatorB + Quantity + NWDConstants.kFieldSeparatorB + Condition.ToString();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Description()
        {
            K tObject = NWDBasis<K>.FilterDataByReference(Reference);
            string rDescription = Reference;
            if (tObject == null)
            {
                rDescription = Reference + " (in error) ";
            }
            else if (tObject.InternalKey == null)
            {
                rDescription = tObject.InternalKey;
            }
            switch (Condition)
            {
                case NWDConditional.DifferentTo:
                    {
                        rDescription += " != " + Quantity + "\r\n";
                    }
                    break;
                case NWDConditional.EqualTo:
                    {
                        rDescription += " !== " + Quantity + "\r\n";
                    }
                    break;
                case NWDConditional.UpperThan:
                    {
                        rDescription += " > " + Quantity + "\r\n";
                    }
                    break;
                case NWDConditional.UpperThanOrEqual:
                    {
                        rDescription += " >= " + Quantity + "\r\n";
                    }
                    break;
                case NWDConditional.LowerThan:
                    {

                        rDescription += " < " + Quantity + "\r\n";
                    }
                    break;
                case NWDConditional.LowerThanOrEqual:
                    {
                        rDescription += " <= " + Quantity + "\r\n";
                    }
                    break;
                default:
                    break;
            }
            return rDescription;
        }
        //-------------------------------------------------------------------------------------------------------------
        public K GetObject()
        {
            return NWDBasis<K>.FilterDataByReference(Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool isValid(int sQuantity)
        {
            bool rReturn = true;
            switch (Condition)
            {
                case NWDConditional.DifferentTo:
                    {
                        if (sQuantity == Quantity)
                        {
                            rReturn = false;
                        }
                    }
                    break;
                case NWDConditional.EqualTo:
                    {
                        if (sQuantity != Quantity)
                        {
                            rReturn = false;
                        }
                    }
                    break;
                case NWDConditional.UpperThan:
                    {
                        if (sQuantity <= Quantity)
                        {
                            rReturn = false;
                        }
                    }
                    break;
                case NWDConditional.UpperThanOrEqual:
                    {
                        if (sQuantity < Quantity)
                        {
                            rReturn = false;
                        }
                    }
                    break;
                case NWDConditional.LowerThan:
                    {
                        if (sQuantity >= Quantity)
                        {
                            rReturn = false;
                        }
                    }
                    break;
                case NWDConditional.LowerThanOrEqual:
                    {
                        if (sQuantity > Quantity)
                        {
                            rReturn = false;
                        }
                    }
                    break;
                default:
                    break;
            }

            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
    public class NWDReferencesConditionalType<K> : NWDReferenceMultiple where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesConditionalType()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ContainsObject(K sObject)
        {
            return Value.Contains(sObject.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ContainsReference(string sReference)
        {
            return Value.Contains(sReference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsEmpty()
        {
            bool rReturn = true;
            if (Value != string.Empty)
            {
                rReturn = false;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsNotEmpty()
        {
            bool rReturn = false;
            if (Value != string.Empty)
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsValid(NWDReferencesQuantityType<K> sReferencesQuantity, bool sExceptIfIsEmpty = false)
        {
            bool rReturn = true;
            if (sExceptIfIsEmpty && Value == string.Empty)
            {
                return false;
            }
            Dictionary<string, int> tDico = sReferencesQuantity.GetReferenceAndQuantity();
            foreach (NWDReferenceConditionalType<K> tConditional in GetReferenceQuantityConditional())
            {
                int tQuantity = tDico[tConditional.Reference];
                if (tConditional.isValid(tQuantity) == false)
                {
                    rReturn = false;
                    break;
                }
            }
            return rReturn;
        }
        ////-------------------------------------------------------------------------------------------------------------
        //public bool ContainsReferencesQuantity(NWDReferencesConditionalType<K> sReferencesQuantity)
        //{
        //    bool rReturn = true;
        //    // I compare all elemnt
        //    Dictionary<string, int> tThis = GetReferenceAndQuantity();
        //    Dictionary<string, int> tOther = sReferencesQuantity.GetReferenceAndQuantity();

        //    foreach (KeyValuePair<string, int> tKeyValue in tOther)
        //    {
        //        if (tThis.ContainsKey(tKeyValue.Key) == false)
        //        {
        //            rReturn = false;
        //            break;
        //        }
        //        else
        //        {
        //            if (tKeyValue.Value > tThis[tKeyValue.Key])
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public bool RemoveReferencesQuantity(NWDReferencesConditionalType<K> sReferencesQuantity, bool sCanBeNegative = true, bool sRemoveEmpty = true)
        //{
        //    //TODO : add comment to explain the used of a boolean
        //    bool rReturn = ContainsReferencesQuantity(sReferencesQuantity);
        //    if (rReturn == true)
        //    {
        //        Dictionary<string, int> tThis = GetReferenceAndQuantity();
        //        Dictionary<string, int> tOther = sReferencesQuantity.GetReferenceAndQuantity();
        //        foreach (KeyValuePair<string, int> tKeyValue in tOther)
        //        {
        //            //TODO : check negative value
        //            //TODO : use RemoveObjectQuantity
        //            tThis[tKeyValue.Key] = tThis[tKeyValue.Key] - tKeyValue.Value;
        //        }
        //        SetReferenceAndQuantity(tThis);
        //    }
        //    return rReturn;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void RemoveObjectQuantity(NWDBasis<K> sObject, int sQuantity, bool sCanBeNegative = true, bool sRemoveEmpty = true)
        //{
        //    Dictionary<string, int> tThis = GetReferenceAndQuantity();
        //    if (tThis.ContainsKey(sObject.Reference) == false)
        //    {
        //        tThis.Add(sObject.Reference, -sQuantity);
        //    }
        //    else
        //    {
        //        tThis[sObject.Reference] = tThis[sObject.Reference] - sQuantity;
        //    }

        //    if (sCanBeNegative == false && tThis[sObject.Reference] < 0)
        //    {
        //        tThis[sObject.Reference] = 0;
        //    }

        //    if (sRemoveEmpty == true && tThis[sObject.Reference] == 0)
        //    {
        //        tThis.Remove(sObject.Reference);
        //    }

        //    SetReferenceAndQuantity(tThis);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void AddReferencesQuantity(NWDReferencesConditionalType<K> sReferencesQuantity)
        //{
        //    // I compare all element
        //    Dictionary<string, int> tThis = GetReferenceAndQuantity();
        //    Dictionary<string, int> tOther = sReferencesQuantity.GetReferenceAndQuantity();
        //    foreach (KeyValuePair<string, int> tKeyValue in tOther)
        //    {
        //        if (tThis.ContainsKey(tKeyValue.Key) == false)
        //        {
        //            tThis.Add(tKeyValue.Key, tKeyValue.Value);
        //        }
        //        else
        //        {
        //            tThis[tKeyValue.Key] = tThis[tKeyValue.Key] + tKeyValue.Value;
        //        }
        //    }
        //    SetReferenceAndQuantity(tThis);
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public void AddObjectQuantity(NWDBasis<K> sObject, int sQuantity)
        //{
        //    // I compare all element
        //    Dictionary<string, int> tThis = GetReferenceAndQuantity();
        //    if (tThis.ContainsKey(sObject.Reference) == false)
        //    {
        //        tThis.Add(sObject.Reference, sQuantity);
        //    }
        //    else
        //    {
        //        tThis[sObject.Reference] = tThis[sObject.Reference] + sQuantity;
        //    }
        //    SetReferenceAndQuantity(tThis);
        //}
        //-------------------------------------------------------------------------------------------------------------
        public K[] FindDats(string sAccountReference = null)
        {
            List<K> tList = new List<K>();
            string[] tArray = GetReferences();
            foreach (string tRef in tArray)
            {
                K tObject = NWDBasis<K>.FilterDataByReference(tRef, sAccountReference) as K;
                if (tObject != null)
                {
                    tList.Add(tObject);
                }
            }
            return tList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] GetRawDatas()
        {
            List<K> tList = new List<K>();
            string[] tArray = GetReferences();
            foreach (string tRef in tArray)
            {
                K tObject = NWDBasis<K>.RawDataByReference(tRef) as K;
                if (tObject != null)
                {
                    tList.Add(tObject);
                }
            }
            return tList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string[] GetReferences()
        {
            List<string> tValueList = new List<string>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        tValueList.Add(tLineValue[0]);
                    }
                }
            }
            return tValueList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public string[] GetSortedReferences()
        {
            string[] tResult = GetReferences();
            Array.Sort(tResult, StringComparer.InvariantCulture);
            return tResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetReferenceQuantityConditional(List<NWDReferenceConditionalType<K>> sList)
        {
            List<string> tValueList = new List<string>();
            foreach (NWDReferenceConditionalType<K> tKeyValue in sList)
            {
                tValueList.Add(tKeyValue.Value);
            }
            string[] tNextValueArray = tValueList.Distinct().ToArray();
            string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
            tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
            Value = tNextValue;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDReferenceConditionalType<K>> GetReferenceQuantityConditional()
        {
            List<NWDReferenceConditionalType<K>> rList = new List<NWDReferenceConditionalType<K>>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    rList.Add(new NWDReferenceConditionalType<K>(tLine));
                }
            }
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Description()
        {
            List<NWDReferenceConditionalType<K>> tList = GetReferenceQuantityConditional();
            string rDescription = string.Empty;
            foreach (NWDReferenceConditionalType<K> tKeyValue in tList)
            {
                rDescription += tKeyValue.Description();
            }
            return rDescription;
        }
        ////-------------------------------------------------------------------------------------------------------------
        //public Dictionary<K, int> GetObjectAndQuantity(string sAccountReference = null)
        //{
        //    Dictionary<K, int> tValueDico = new Dictionary<K, int>();
        //    if (Value != null && Value != "")
        //    {
        //        string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
        //        foreach (string tLine in tValueArray)
        //        {
        //            string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
        //            if (tLineValue.Length == 2)
        //            {
        //                int tQ = 0;
        //                int.TryParse(tLineValue[1], out tQ);
        //                K tObject = NWDBasis<K>.GetObjectByReference(tLineValue[0], sAccountReference) as K;
        //                if (tObject != null)
        //                {
        //                    tValueDico.Add(tObject, tQ);
        //                }
        //            }
        //        }
        //    }
        //    return tValueDico;
        //}
        ////-------------------------------------------------------------------------------------------------------------
        //public Dictionary<K, int> GetObjectAndQuantityAbsolute()
        //{
        //    Dictionary<K, int> tValueDico = new Dictionary<K, int>();
        //    if (Value != null && Value != "")
        //    {
        //        string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
        //        foreach (string tLine in tValueArray)
        //        {
        //            string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
        //            if (tLineValue.Length == 2)
        //            {
        //                int tQ = 0;
        //                int.TryParse(tLineValue[1], out tQ);
        //                K tObject = NWDBasis<K>.GetObjectAbsoluteByReference(tLineValue[0]) as K;
        //                if (tObject != null)
        //                {
        //                    tValueDico.Add(tObject, tQ);
        //                }
        //            }
        //        }
        //    }
        //    return tValueDico;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public List<K> ExploseInItemsList()
        {
            List<K> rList = new List<K>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        int tQ = NWDToolbox.IntFromString(tLineValue[1]);
                        //int tQ = 0;
                        //int.TryParse(tLineValue[1], System.Globalization.NumberStyles.Integer, NWDConstants.FormatCountry, out tQ);
                        K tObject = NWDBasis<K>.FilterDataByReference(tLineValue[0]) as K;
                        if (tObject != null)
                        {
                            for (int i = 0; i < tQ; i++)
                            {
                                rList.Add(tObject);
                            }
                        }
                    }
                }
            }
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override bool ErrorAnalyze()
        {
            bool rReturn = false;
            if (string.IsNullOrEmpty(Value) == false)
            {
                List<string> tValueListERROR = ReferenceInError(new List<string>(GetReferences()));
                if (tValueListERROR.Count > 0)
                {
                    rReturn = true;
                }
            }
            InError = rReturn;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_EditorGetObjects)]
        public override object[] EditorGetDatas()
        {
            List<K> rReturn = new List<K>();
            foreach (string tReference in GetReferences())
            {
                K tObj = NWDBasis<K>.RawDataByReference(tReference);
                //if (tObj != null)
                {
                    if (rReturn.Contains(tObj) == false)
                    {
                        rReturn.Add(tObj);
                    }
                }
            }
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<string> ReferenceInError(List<string> sReferencesList)
        {
            List<string> rReturn = new List<string>();
            foreach (string tReference in sReferencesList)
            {
                if (NWDBasis<K>.RawDataByReference(tReference) == null)
                {
                    rReturn.Add(tReference);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            int tRow = 1;
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                tRow += tValueArray.Count();
            }
            float tHeight = (NWDGUI.kFieldMarge + NWDGUI.kDataSelectorFieldStyle.fixedHeight) * tRow - NWDGUI.kFieldMarge;
            return tHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDReferencesConditionalType<K> tTemporary = new NWDReferencesConditionalType<K>();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            tTemporary.Value = Value;
            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;
            float tIntWidth = NWDGUI.kIntWidth;
            float tConWidth = NWDGUI.kConditionalWidth;
            float tEditWidth = NWDGUI.kEditWidth;
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
            string tNewReferenceQuantity = string.Empty;
            for (int i = 0; i < tValueList.Count; i++)
            {
                if (i > 0)
                {
                    tContent = new GUIContent("   ");
                }
                int tQ = 1;
                int tC = 0;
                string tV = string.Empty;
                string tLine = tValueList.ElementAt(i);
                string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                if (tLineValue.Length == 2)
                {
                    tV = tLineValue[0];
                    tQ = NWDToolbox.IntFromString(tLineValue[1]);
                }
                if (tLineValue.Length == 3)
                {
                    tV = tLineValue[0];
                    tQ = NWDToolbox.IntFromString(tLineValue[1]);
                    tC = NWDToolbox.IntFromString(tLineValue[2]);
                }

                tV = NWDDatasSelector.Field(NWDBasisHelper.FindTypeInfos(typeof(K)), new Rect(tX, tY, tWidth, NWDGUI.kDataSelectorFieldStyle.fixedHeight), tContent, tV, tIntWidth + tConWidth + NWDGUI.kFieldMarge*2);
                if (string.IsNullOrEmpty(tV) == false)
                {

                    tC = EditorGUI.Popup(new Rect(tX + tWidth - tIntWidth - tConWidth - tEditWidth - NWDGUI.kFieldMarge*2 , tY + NWDGUI.kDatasSelectorYOffset, tConWidth, NWDGUI.kTextFieldStyle.fixedHeight),
                                         tC, new string[] { "=", ">", ">=", "<", "=<", "!" });

                    int tIndentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;
                    tQ = EditorGUI.IntField(new Rect(tX + tWidth - tIntWidth - tEditWidth - NWDGUI.kFieldMarge, tY + NWDGUI.kDatasSelectorYOffset, tIntWidth, NWDGUI.kTextFieldStyle.fixedHeight), tQ);
                    EditorGUI.indentLevel = tIndentLevel;
                    if (i > 0)
                    {
                        if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth - (NWDGUI.kUpDownWidth + NWDGUI.kFieldMarge), tY + NWDGUI.kDatasSelectorYOffset, NWDGUI.kIconButtonStyle.fixedHeight, NWDGUI.kIconButtonStyle.fixedHeight - 2), NWDGUI.kUpContentIcon, NWDGUI.kIconButtonStyle))
                        {
                            tUp = true;
                            tIndexToMove = i;
                        }
                        if (i < tValueList.Count - 2)
                        {
                            if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth - (NWDGUI.kUpDownWidth + NWDGUI.kFieldMarge) * 2, tY + NWDGUI.kDatasSelectorYOffset, NWDGUI.kIconButtonStyle.fixedHeight, NWDGUI.kIconButtonStyle.fixedHeight - 2), NWDGUI.kDownContentIcon, NWDGUI.kIconButtonStyle))
                            {
                                tDown = true;
                                tIndexToMove = i;
                            }
                        }
                    }
                    tValueList[i] = tV + NWDConstants.kFieldSeparatorB + NWDToolbox.IntToString(tQ) + NWDConstants.kFieldSeparatorB + NWDToolbox.IntToString(tC);
                }
                else
                {
                    tValueList[i] = string.Empty;
                }
                tY = tY + NWDGUI.kFieldMarge + NWDGUI.kDataSelectorFieldStyle.fixedHeight;
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
            tValueList.Distinct();
            tValueList.Remove(NWDConstants.kFieldSeparatorA);
            string[] tNextValueArray = tValueList.ToArray();
            string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray) + tNewReferenceQuantity;
            tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
            tTemporary.Value = tNextValue;
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        public string ChangeReferenceForAnother(string sOldReference, string sNewReference)
        {
            string rReturn = "NO";
            if (Value != null)
            {
                if (Value.Contains(sOldReference))
                {
                    Value = Value.Replace(sOldReference, sNewReference);
                    rReturn = "YES";
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================