//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:28:29
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
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
    public class NWDRange
    {
        //-------------------------------------------------------------------------------------------------------------
        public float Min;
        public float Max;
        //public float Average;
        //-------------------------------------------------------------------------------------------------------------
        public NWDRange()
        {
            Min = 0.0F;
            Max = 0.0F;
            //Average = 0.0F;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDRange(string sString)
        {
            string[] tValueArray = sString.Split(new string[] { NWDConstants.kFieldSeparatorC }, StringSplitOptions.RemoveEmptyEntries);
            if (tValueArray.Length > 0)
            {
                Min = NWDToolbox.FloatFromString(tValueArray[0]);
                //float.TryParse(tValueArray[0], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out Min);
            }
            if (tValueArray.Length > 1)
            {
                Max = NWDToolbox.FloatFromString(tValueArray[1]);
                //float.TryParse(tValueArray[1], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out Max);
            }
            //if (tValueArray.Length > 2)
            //{
            //    float.TryParse(tValueArray[2], out Average);
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDRange(float sValue)
        {
            AddValue(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        new public string ToString()
        {
            return NWDToolbox.FloatToString(Min) + NWDConstants.kFieldSeparatorC + NWDToolbox.FloatToString(Max);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddValue(float sValue)
        {
            Min += sValue;
            Max = Max + 1.0F;
            //ReEvaluate();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveValue(float sValue)
        {
            if (Max > 0)
            {
                Min += sValue;
                Max = Max - 1.0F;
                //ReEvaluate();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetValue()
        {
            Min = 0.0F;
            Max = 0.0F;
            //Average = 0.0F;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public void ReEvaluate()
        //{
        //    if (Max <= 0)
        //    {
        //        Min = 0.0F;
        //        Average = 0.0F;
        //    }
        //    else
        //    {
        //        Average = Min / Max;
        //    }
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDReferencesRangeType used to put a reference with float in value. Use properties with name, like 'ItemRange', 'SpotRange', 'BonusRange' , etc.
    /// </summary>
    [SerializeField]
    public class NWDReferencesRangeType<K> : NWDReferenceMultiple where K : NWDBasis, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesRangeType()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ContainsData(K sData)
        {
            return Value.Contains(sData.Reference);
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
            SetReferenceAndRange(GetReferenceAndRange());
        }
        //-------------------------------------------------------------------------------------------------------------
        //public bool ContainedIn(NWDReferencesRangeType<K> sReferencesProportion, bool sExceptIfIsEmpty = true)
        //{
        //    bool rReturn = true;
        //    if (sExceptIfIsEmpty && Value == "")
        //    {
        //        return false;
        //    }
        //    // I compare all elemnt
        //    Dictionary<string, NWDRange> tThis = GetReferenceAndProportion();
        //    Dictionary<string, NWDRange> tOther = sReferencesProportion.GetReferenceAndProportion();

        //    foreach (KeyValuePair<string, NWDRange> tKeyValue in tThis)
        //    {
        //        if (tOther.ContainsKey(tKeyValue.Key) == false)
        //        {
        //            rReturn = false;
        //            break;
        //        }
        //        else
        //        {
        //            if (tKeyValue.Value > tOther[tKeyValue.Key])
        //            {
        //                return false;
        //            }
        //        }
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public bool ContainsReferencesProportion(NWDReferencesRangeType<K> sReferencesProportion)
        //{
        //    bool rReturn = true;
        //    // I compare all elemnt
        //    Dictionary<string, float> tThis = GetReferenceAndProportion();
        //    Dictionary<string, float> tOther = sReferencesProportion.GetReferenceAndProportion();

        //    foreach (KeyValuePair<string, float> tKeyValue in tOther)
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
        //-------------------------------------------------------------------------------------------------------------
        //public bool RemoveReferencesQuantity(NWDReferencesRangeType<K> sReferencesProportion, bool sCanBeNegative = true, bool sRemoveEmpty = true)
        //{
        //    //TODO : add comment to explain the used of a boolean
        //    bool rReturn = ContainsReferencesProportion(sReferencesProportion);
        //    if (rReturn == true)
        //    {
        //        Dictionary<string, float> tThis = GetReferenceAndProportion();
        //        Dictionary<string, float> tOther = sReferencesProportion.GetReferenceAndProportion();
        //        foreach (KeyValuePair<string, float> tKeyValue in tOther)
        //        {
        //            //TODO : check negative value
        //            //TODO : use RemoveObjectQuantity
        //            tThis[tKeyValue.Key] = tThis[tKeyValue.Key] - tKeyValue.Value;
        //        }
        //        SetReferenceAndQuantity(tThis);
        //    }
        //    return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
        //public void RemoveObjectQuantity(NWDBasis sObject, float sQuantity, bool sCanBeNegative = true, bool sRemoveEmpty = true)
        //{
        //    Dictionary<string, float> tThis = GetReferenceAndProportion();
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
        //-------------------------------------------------------------------------------------------------------------
        //public void AddReferencesQuantity(NWDReferencesRangeType<K> sReferencesProportion)
        //{
        //    // I compare all element
        //    Dictionary<string, float> tThis = GetReferenceAndProportion();
        //    Dictionary<string, float> tOther = sReferencesProportion.GetReferenceAndProportion();
        //    foreach (KeyValuePair<string, float> tKeyValue in tOther)
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
        //-------------------------------------------------------------------------------------------------------------
        public void AddDataAndValue(K sData, float sValue)
        {
            // I compare all element
            Dictionary<string, NWDRange> tThis = GetReferenceAndRange();
            if (tThis.ContainsKey(sData.Reference) == false)
            {
                tThis.Add(sData.Reference, new NWDRange(sValue));
            }
            else
            {
                tThis[sData.Reference].AddValue(sValue);
            }
            SetReferenceAndRange(tThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveData(K sObject)
        {
            // I compare all element
            Dictionary<string, NWDRange> tThis = GetReferenceAndRange();
            if (tThis.ContainsKey(sObject.Reference) == true)
            {
                tThis.Remove(sObject.Reference);
            }
            SetReferenceAndRange(tThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] GetReachabableDatas()
        {
            List<K> tList = new List<K>();
            string[] tArray = GetReferences();
            foreach (string tRef in tArray)
            {
                K tObject = NWDBasisHelper.GetReachableDataByReference<K>(tRef) as K;
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
                K tObject = NWDBasisHelper.GetRawDataByReference<K>(tRef) as K;
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
        public void SetReferenceAndRange(Dictionary<string, NWDRange> sDico)
        {
            List<string> tValueList = new List<string>();
            foreach (KeyValuePair<string, NWDRange> tKeyValue in sDico)
            {
                tValueList.Add(tKeyValue.Key + NWDConstants.kFieldSeparatorB + tKeyValue.Value.ToString());
            }
            string[] tNextValueArray = tValueList.Distinct().ToArray();
            string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
            tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
            Value = tNextValue;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, NWDRange> GetReferenceAndRange()
        {
            Dictionary<string, NWDRange> tValueDico = new Dictionary<string, NWDRange>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        NWDRange tQ = new NWDRange(tLineValue[1]);
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
        public Dictionary<K, NWDRange> GetReachableDatasAndRange()
        {
            Dictionary<K, NWDRange> tValueDico = new Dictionary<K, NWDRange>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        NWDRange tQ = new NWDRange(tLineValue[1]);
                        K tObject = NWDBasisHelper.GetReachableDataByReference<K>(tLineValue[0]) as K;
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
        public Dictionary<K, NWDRange> GetRawDatasAndRange()
        {
            Dictionary<K, NWDRange> tValueDico = new Dictionary<K, NWDRange>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        NWDRange tQ = new NWDRange(tLineValue[1]);
                        K tObject = NWDBasisHelper.GetRawDataByReference<K>(tLineValue[0]) as K;
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
        public string Description()
        {
            string rDescription = string.Empty;
            Dictionary<string, NWDRange> tDescDico = GetReferenceAndRange();
            foreach (KeyValuePair<string, NWDRange> tKeyValue in tDescDico)
            {
                K tObject = NWDBasisHelper.GetCorporateDataByReference<K>(tKeyValue.Key);
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
        //[NWDAliasMethod(NWDConstants.M_EditorGetObjects)]
        public override object[] GetEditorDatas()
        {
            List<K> rReturn = new List<K>();
            foreach (string tReference in GetReferences())
            {
                K tObj = NWDBasisHelper.GetRawDataByReference<K>(tReference) as K;
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
        public void EditorAddNewData()
        {
            K tNewObject = NWDBasisHelper.NewData<K>();
            this.AddDataAndValue(tNewObject, 0.0F);
            NWDBasisHelper.BasisHelper<K>().SetObjectInEdition(tNewObject, false, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<string> ReferenceInError(List<string> sReferencesList)
        {
            List<string> rReturn = new List<string>();
            foreach (string tReference in sReferencesList)
            {
                if (NWDBasisHelper.GetRawDataByReference<K>(tReference) == null)
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
            float tHeight = (NWDGUI.kFieldMarge + NWDGUI.kDataSelectorFieldStyle.fixedHeight + NWDGUI.kFieldMarge + NWDGUI.kTextFieldStyle.fixedHeight) * tRow - NWDGUI.kFieldMarge * 2 - NWDGUI.kTextFieldStyle.fixedHeight;
            return tHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = BTBConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDReferencesRangeType<K> tTemporary = new NWDReferencesRangeType<K>();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            tTemporary.Value = Value;
            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;
            float tIntWidth = NWDGUI.kIntWidth;
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

                //int tIndex = 0;
                NWDRange tQ = new NWDRange(string.Empty);
                string tV = string.Empty;
                string tLine = tValueList.ElementAt(i);
                string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                if (tLineValue.Length == 2)
                {
                    tV = tLineValue[0];
                    tQ = new NWDRange(tLineValue[1]);
                }

                tV = NWDDatasSelector.Field(NWDBasisHelper.FindTypeInfos(typeof(K)), new Rect(tX, tY, tWidth, NWDGUI.kDataSelectorFieldStyle.fixedHeight), tContent, tV, sDisabled);

                if (string.IsNullOrEmpty(tV) == false)
                {
                    float tYb = tY + NWDGUI.kDataSelectorFieldStyle.fixedHeight + NWDGUI.kFieldMarge;
                    float tXb = sPosition.x + EditorGUIUtility.labelWidth;
                    float tTiersWidth = Mathf.Ceil((sPosition.width - EditorGUIUtility.labelWidth + NWDGUI.kFieldMarge) / 2.0F);
                    float tTiersWidthB = tTiersWidth - NWDGUI.kFieldMarge;

                    int tIndentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;
                    EditorGUI.LabelField(new Rect(tX, tYb, EditorGUIUtility.labelWidth - NWDGUI.kFieldMarge, NWDGUI.kLabelRightStyle.fixedHeight), "Range", NWDGUI.kLabelRightStyle);
                    tQ.Min = EditorGUI.FloatField(new Rect(tXb, tYb, tTiersWidthB, NWDGUI.kTextFieldStyle.fixedHeight), tQ.Min);
                    tQ.Max = EditorGUI.FloatField(new Rect(tXb + tTiersWidth, tYb, tTiersWidthB, NWDGUI.kTextFieldStyle.fixedHeight), tQ.Max);

                    EditorGUI.indentLevel = tIndentLevel;
                    if (i > 0)
                    {
                        if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth - (NWDGUI.kUpDownWidth + NWDGUI.kFieldMarge), tY+ NWDGUI.kDatasSelectorYOffset, NWDGUI.kIconButtonStyle.fixedHeight, NWDGUI.kIconButtonStyle.fixedHeight - 2), NWDGUI.kUpContentIcon, NWDGUI.kIconButtonStyle))
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
                    if (!tValueListReferenceAllReady.Contains(tV))
                    {
                        tValueList[i] = tV + NWDConstants.kFieldSeparatorB + tQ.ToString();
                        tValueListReferenceAllReady.Add(tV);
                    }
                }
                else
                {
                    tValueList[i] = string.Empty;
                }
                tY = tY + NWDGUI.kFieldMarge + NWDGUI.kDataSelectorFieldStyle.fixedHeight;
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
        public override void CreatePloters(NWDNodeCard sNodalCard, float tHeight)
        {
            int tCounter = 0;
            foreach (string tRef in GetSortedReferences())
            {
                sNodalCard.PloterList.Add(new NWDNodePloter(sNodalCard, tRef,
                    new Vector2(0,
                    tHeight
                   +  (NWDGUI.kFieldMarge * tCounter) + (NWDGUI.kDataSelectorFieldStyle.fixedHeight + NWDGUI.kFieldMarge + NWDGUI.kTextFieldStyle.fixedHeight) * (tCounter) + NWDGUI.kDataSelectorFieldStyle.fixedHeight * 0.5F
                    )));
                tCounter++;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void CreatePlotersInvisible(NWDNodeCard sNodalCard, float tHeight)
        {
            foreach (string tRef in GetSortedReferences())
            {
                sNodalCard.PloterList.Add(new NWDNodePloter(sNodalCard, tRef, new Vector2(0,
                    tHeight
                    + NWDGUI.kFieldMarge + NWDGUI.kBoldFoldoutStyle.fixedHeight * (0.5f)
                    )));
            }
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