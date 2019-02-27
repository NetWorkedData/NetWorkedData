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
    /// <summary>
    /// NWDReferencesAmountType used to put a reference with float in value. Use properties with name, like 'ItemAmount', 'SpotAmount', 'BonusAmount' , etc.
    /// </summary>
    [SerializeField]
    public class NWDReferencesAmountType<K> : NWDReferenceMultiple where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesAmountType()
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
        public void DistinctReference()
        {
            SetReferenceAndAmount(GetReferenceAndAmount());
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ContainedIn(NWDReferencesAmountType<K> sReferencesAmount, bool sExceptIfIsEmpty = true)
        {
            bool rReturn = true;
            if (sExceptIfIsEmpty && Value == string.Empty)
            {
                return false;
            }
            // I compare all elemnt
            Dictionary<string, float> tThis = GetReferenceAndAmount();
            Dictionary<string, float> tOther = sReferencesAmount.GetReferenceAndAmount();

            foreach (KeyValuePair<string, float> tKeyValue in tThis)
            {
                if (tOther.ContainsKey(tKeyValue.Key) == false)
                {
                    rReturn = false;
                    break;
                }
                else
                {
                    if (tKeyValue.Value > tOther[tKeyValue.Key])
                    {
                        return false;
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ContainsReferencesAmount(NWDReferencesAmountType<K> sReferencesAmount)
        {
            bool rReturn = true;
            // I compare all elemnt
            Dictionary<string, float> tThis = GetReferenceAndAmount();
            Dictionary<string, float> tOther = sReferencesAmount.GetReferenceAndAmount();

            foreach (KeyValuePair<string, float> tKeyValue in tOther)
            {
                if (tThis.ContainsKey(tKeyValue.Key) == false)
                {
                    rReturn = false;
                    break;
                }
                else
                {
                    if (tKeyValue.Value > tThis[tKeyValue.Key])
                    {
                        return false;
                    }
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool RemoveReferencesAmount(NWDReferencesAmountType<K> sReferencesAmount, bool sCanBeNegative = true, bool sRemoveEmpty = true)
        {
            //TODO : add comment to explain the used of a boolean
            bool rReturn = ContainsReferencesAmount(sReferencesAmount);
            if (rReturn == true)
            {
                Dictionary<string, float> tThis = GetReferenceAndAmount();
                Dictionary<string, float> tOther = sReferencesAmount.GetReferenceAndAmount();
                foreach (KeyValuePair<string, float> tKeyValue in tOther)
                {
                    //TODO : check negative value
                    //TODO : use RemoveObjectQuantity
                    tThis[tKeyValue.Key] = tThis[tKeyValue.Key] - tKeyValue.Value;
                }
                SetReferenceAndAmount(tThis);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveObjectAmount(NWDBasis<K> sObject, float sAmount, bool sCanBeNegative = true, bool sRemoveEmpty = true)
        {
            Dictionary<string, float> tThis = GetReferenceAndAmount();
            if (tThis.ContainsKey(sObject.Reference) == false)
            {
                tThis.Add(sObject.Reference, -sAmount);
            }
            else
            {
                tThis[sObject.Reference] = tThis[sObject.Reference] - sAmount;
            }

            if (sCanBeNegative == false && tThis[sObject.Reference] < 0)
            {
                tThis[sObject.Reference] = 0;
            }

            if (sRemoveEmpty == true && tThis[sObject.Reference] == 0)
            {
                tThis.Remove(sObject.Reference);
            }

            SetReferenceAndAmount(tThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddReferencesAmount(NWDReferencesAmountType<K> sReferencesAmount)
        {
            // I compare all element
            Dictionary<string, float> tThis = GetReferenceAndAmount();
            Dictionary<string, float> tOther = sReferencesAmount.GetReferenceAndAmount();
            foreach (KeyValuePair<string, float> tKeyValue in tOther)
            {
                if (tThis.ContainsKey(tKeyValue.Key) == false)
                {
                    tThis.Add(tKeyValue.Key, tKeyValue.Value);
                }
                else
                {
                    tThis[tKeyValue.Key] = tThis[tKeyValue.Key] + tKeyValue.Value;
                }
            }
            SetReferenceAndAmount(tThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddObjectAmount(NWDBasis<K> sObject, float sAmount)
        {
            // I compare all element
            Dictionary<string, float> tThis = GetReferenceAndAmount();
            if (tThis.ContainsKey(sObject.Reference) == false)
            {
                tThis.Add(sObject.Reference, sAmount);
            }
            else
            {
                tThis[sObject.Reference] = tThis[sObject.Reference] + sAmount;
            }
            SetReferenceAndAmount(tThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveObject(NWDBasis<K> sObject)
        {
            // I compare all element
            Dictionary<string, float> tThis = GetReferenceAndAmount();
            if (tThis.ContainsKey(sObject.Reference) == true)
            {
                tThis.Remove(sObject.Reference);
            }
            SetReferenceAndAmount(tThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] GetObjects(string sAccountReference = null)
        {
            List<K> tList = new List<K>();
            string[] tArray = GetReferences();
            foreach (string tRef in tArray)
            {
                K tObject = NWDBasis<K>.FindDataByReference(tRef, sAccountReference) as K;
                if (tObject != null)
                {
                    tList.Add(tObject);
                }
            }
            return tList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] GetObjectsAbsolute()
        {
            List<K> tList = new List<K>();
            string[] tArray = GetReferences();
            foreach (string tRef in tArray)
            {
                K tObject = NWDBasis<K>.GetDataByReference(tRef) as K;
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
        public void SetReferenceAndAmount(Dictionary<string, float> sDico)
        {
            List<string> tValueList = new List<string>();
            foreach (KeyValuePair<string, float> tKeyValue in sDico)
            {
                tValueList.Add(tKeyValue.Key + NWDConstants.kFieldSeparatorB + NWDToolbox.FloatToString(tKeyValue.Value));
            }
            string[] tNextValueArray = tValueList.Distinct().ToArray();
            string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
            tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
            Value = tNextValue;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, float> GetReferenceAndAmount()
        {
            Dictionary<string, float> tValueDico = new Dictionary<string, float>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        float tQ = NWDToolbox.FloatFromString(tLineValue[1]);
                        //float.TryParse(tLineValue[1], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tQ);
                        if (tValueDico.ContainsKey(tLineValue[0]) == false)
                        {
                            tValueDico.Add(tLineValue[0], tQ);
                        }
                    }
                }
            }
            return tValueDico;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<K, float> GetObjectAndAmount(string sAccountReference = null)
        {
            Dictionary<K, float> tValueDico = new Dictionary<K, float>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        float tQ = NWDToolbox.FloatFromString(tLineValue[1]);
                        //float tQ = 0;
                        //float.TryParse(tLineValue[1], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tQ);
                        K tObject = NWDBasis<K>.FindDataByReference(tLineValue[0], sAccountReference) as K;
                        if (tObject != null)
                        {
                            if (tValueDico.ContainsKey(tObject) == false)
                            {
                                tValueDico.Add(tObject, tQ);
                            }
                        }
                    }
                }
            }
            return tValueDico;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<K, float> GetObjectAndAmountAbsolute()
        {
            Dictionary<K, float> tValueDico = new Dictionary<K, float>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        float tQ = NWDToolbox.FloatFromString(tLineValue[1]);
                        //float tQ = 0;
                        //float.TryParse(tLineValue[1], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tQ);
                        K tObject = NWDBasis<K>.GetDataByReference(tLineValue[0]) as K;
                        if (tObject != null)
                        {
                            if (tValueDico.ContainsKey(tObject) == false)
                            {
                                tValueDico.Add(tObject, tQ);
                            }
                        }
                    }
                }
            }
            return tValueDico;
        }
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
                        float tQ = NWDToolbox.FloatFromString(tLineValue[1]);
                        //float tQ = 0;
                        //float.TryParse(tLineValue[1], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out tQ);
                        K tObject = NWDBasis<K>.FindDataByReference(tLineValue[0]) as K;
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
        public string Description()
        {
            string rDescription = string.Empty;
            Dictionary<string, float> tDescDico = GetReferenceAndAmount();
            foreach (KeyValuePair<string, float> tKeyValue in tDescDico)
            {
                K tObject = NWDBasis<K>.FindDataByReference(tKeyValue.Key);
                if (tObject == null)
                {
                    rDescription = tKeyValue.Key + " (in error) : " + tKeyValue.Value;
                }
                else
                {
                    rDescription = tObject.InternalKey + " : " + tKeyValue.Value;
                }
            }
            return rDescription;
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
        [NWDAliasMethod(NWDConstants.M_EditorGetObjects)]
        public K[] EditorGetObjects()
        {
            List<K> rReturn = new List<K>();
            foreach (string tReference in GetReferences())
            {
                K tObj = NWDBasis<K>.GetDataByReference(tReference);
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
        public void EditorAddNewObject()
        {
            K tNewObject = NWDBasis<K>.NewData();
            this.AddObjectAmount(tNewObject,1);
            NWDBasis<K>.SetObjectInEdition(tNewObject, false, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<string> ReferenceInError(List<string> sReferencesList)
        {
            List<string> rReturn = new List<string>();
            foreach (string tReference in sReferencesList)
            {
                if (NWDBasis<K>.GetDataByReference(tReference) == null)
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
            float tHeight = (NWDConstants.kFieldMarge + NWDConstants.kDatasSelectorRowStyle.fixedHeight) * tRow - NWDConstants.kFieldMarge;

            return tHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING)
        {
            NWDReferencesAmountType<K> tTemporary = new NWDReferencesAmountType<K>();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            tTemporary.Value = Value;
            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;
            float tIntWidth = NWDConstants.kIntWidth;
            List<string> tValueList = new List<string>();
            List<string> tValueListReferenceAllReady = new List<string>();
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
                float tQ = 1;
                string tV = string.Empty;
                string tLine = tValueList.ElementAt(i);
                string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                if (tLineValue.Length == 2)
                {
                    tQ = NWDToolbox.FloatFromString(tLineValue[1]);
                    tV = tLineValue[0];
                }
                tV = NWDDatasSelector<K>.Field(new Rect(tX, tY, tWidth, NWDConstants.kDatasSelectorRowStyle.fixedHeight), tContent, tV, tIntWidth + NWDConstants.kFieldMarge*2);
                if (string.IsNullOrEmpty(tV) == false)
                {
                    int tIndentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;
                    tQ = EditorGUI.FloatField(new Rect(tX + tWidth - tIntWidth - NWDConstants.kEditWidth - NWDConstants.kFieldMarge * 2, tY, tIntWidth + NWDConstants.kFieldMarge, NWDConstants.kTextFieldStyle.fixedHeight), tQ);
                    
                    EditorGUI.indentLevel = tIndentLevel;

                    if (i > 0)
                    {
                        if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth - (NWDConstants.kUpDownWidth + NWDConstants.kFieldMarge) * 2, tY, NWDConstants.kUpDownWidth, NWDConstants.kPopupButtonStyle.fixedHeight), NWDConstants.tUpContent, NWDConstants.kPopupButtonStyle))
                        {
                            tUp = true;
                            tIndexToMove = i;
                        }
                    }
                    if (i < tValueList.Count - 2)
                    {
                        if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth - (NWDConstants.kUpDownWidth + NWDConstants.kFieldMarge), tY, NWDConstants.kUpDownWidth, NWDConstants.kPopupButtonStyle.fixedHeight), NWDConstants.tDownContent, NWDConstants.kPopupButtonStyle))
                        {
                            tDown = true;
                            tIndexToMove = i;
                        }
                    }
                    if (!tValueListReferenceAllReady.Contains(tV))
                    {
                        tValueList[i] = tV + NWDConstants.kFieldSeparatorB + NWDToolbox.FloatToString(tQ);
                        tValueListReferenceAllReady.Add(tV);
                    }
                }
                else
                {
                    tValueList[i] = string.Empty;
                }

                tY = tY + NWDConstants.kFieldMarge + NWDConstants.kDatasSelectorRowStyle.fixedHeight;
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
            tTemporary.DistinctReference();
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