//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:28:27
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
    /// <summary>
    /// NWDReferencesQuantityType used to put a reference with float in value. Use properties with name, like 'ItemQuantity', 'SpotQuantity', 'BonusQuantity' , etc.
    /// </summary>
    [SerializeField]
    public class NWDReferencesQuantityType<K> : NWDReferenceMultiple where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesQuantityType()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ContainsData(K sObject)
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
        public bool ContainedIn(NWDReferencesQuantityType<K> sReferencesQuantity, bool sExceptIfIsEmpty = true)
        {
            bool rReturn = true;
            if (sExceptIfIsEmpty && Value == string.Empty)
            {
                return false;
            }
            // I compare all elemnt
            Dictionary<string, int> tThis = GetReferenceAndQuantity();
            Dictionary<string, int> tOther = sReferencesQuantity.GetReferenceAndQuantity();

            foreach (KeyValuePair<string, int> tKeyValue in tThis)
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
        public bool ContainsReferencesQuantity(NWDReferencesQuantityType<K> sReferencesQuantity)
        {
            bool rReturn = true;
            // I compare all elemnt
            Dictionary<string, int> tThis = GetReferenceAndQuantity();
            Dictionary<string, int> tOther = sReferencesQuantity.GetReferenceAndQuantity();

            foreach (KeyValuePair<string, int> tKeyValue in tOther)
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
        public bool RemoveReferencesQuantity(NWDReferencesQuantityType<K> sReferencesQuantity, bool sCanBeNegative = true, bool sRemoveEmpty = true)
        {
            //TODO : add comment to explain the used of a boolean
            bool rReturn = ContainsReferencesQuantity(sReferencesQuantity);
            if (rReturn == true)
            {
                Dictionary<string, int> tThis = GetReferenceAndQuantity();
                Dictionary<string, int> tOther = sReferencesQuantity.GetReferenceAndQuantity();
                foreach (KeyValuePair<string, int> tKeyValue in tOther)
                {
                    //TODO : check negative value
                    //TODO : use RemoveObjectQuantity
                    tThis[tKeyValue.Key] = tThis[tKeyValue.Key] - tKeyValue.Value;
                }
                SetReferenceAndQuantity(tThis);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveDataQuantity(NWDBasis<K> sObject, int sQuantity, bool sCanBeNegative = true, bool sRemoveEmpty = true)
        {
            Dictionary<string, int> tThis = GetReferenceAndQuantity();
            if (tThis.ContainsKey(sObject.Reference) == false)
            {
                tThis.Add(sObject.Reference, -sQuantity);
            }
            else
            {
                tThis[sObject.Reference] = tThis[sObject.Reference] - sQuantity;
            }

            if (sCanBeNegative == false && tThis[sObject.Reference] < 0)
            {
                tThis[sObject.Reference] = 0;
            }

            if (sRemoveEmpty == true && tThis[sObject.Reference] == 0)
            {
                tThis.Remove(sObject.Reference);
            }

            SetReferenceAndQuantity(tThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDReferencesQuantityType<K> operator +(NWDReferencesQuantityType<K> sLeft, NWDReferencesQuantityType<K> sRight)
        {
            NWDReferencesQuantityType<K> rReturn = new NWDReferencesQuantityType<K>();
            rReturn.AddReferencesQuantity(sLeft);
            rReturn.AddReferencesQuantity(sRight);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDReferencesQuantityType<K> operator -(NWDReferencesQuantityType<K> sLeft, NWDReferencesQuantityType<K> sRight)
        {
            NWDReferencesQuantityType<K> rReturn = new NWDReferencesQuantityType<K>();
            rReturn.AddReferencesQuantity(sLeft);
            rReturn.RemoveReferencesQuantity(sRight, true, false);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDReferencesQuantityType<K> operator *(NWDReferencesQuantityType<K> sLeft, int sCoefficient)
        {
            return sLeft.Multiply(sCoefficient);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDReferencesQuantityType<K> operator /(NWDReferencesQuantityType<K> sLeft, int sCoefficient)
        {
            return sLeft.Divise(sCoefficient);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesQuantityType<K> Multiply(int sCoefficient)
        {
            NWDReferencesQuantityType<K> rReturn = new NWDReferencesQuantityType<K>();
            Dictionary<string, int> tDico = GetReferenceAndQuantity();
            foreach (KeyValuePair<string, int> tRefQ in tDico)
            {
                rReturn.AddReferenceQuantity(tRefQ.Key, tRefQ.Value * sCoefficient);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesQuantityType<K> Divise(int sCoefficient)
        {
            NWDReferencesQuantityType<K> rReturn = new NWDReferencesQuantityType<K>();
            Dictionary<string, int> tDico = GetReferenceAndQuantity();
            foreach (KeyValuePair<string, int> tRefQ in tDico)
            {
                rReturn.AddReferenceQuantity(tRefQ.Key, tRefQ.Value / sCoefficient);
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddDatasList(List<K> sDatasList)
        {
            foreach (K tObject in sDatasList)
            {
                AddDataQuantity(tObject, 1);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddReferencesQuantity(NWDReferencesQuantityType<K> sReferencesQuantity)
        {
            // I compare all element
            Dictionary<string, int> tThis = GetReferenceAndQuantity();
            Dictionary<string, int> tOther = sReferencesQuantity.GetReferenceAndQuantity();
            foreach (KeyValuePair<string, int> tKeyValue in tOther)
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
            SetReferenceAndQuantity(tThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddDataQuantity(NWDBasis<K> sData, int sQuantity)
        {
            // I compare all element
            Dictionary<string, int> tThis = GetReferenceAndQuantity();
            if (tThis.ContainsKey(sData.Reference) == false)
            {
                tThis.Add(sData.Reference, sQuantity);
            }
            else
            {
                tThis[sData.Reference] = tThis[sData.Reference] + sQuantity;
            }
            SetReferenceAndQuantity(tThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddReferenceQuantity(string sReference, int sQuantity)
        {
            // I compare all element
            Dictionary<string, int> tThis = GetReferenceAndQuantity();
            if (tThis.ContainsKey(sReference) == false)
            {
                tThis.Add(sReference, sQuantity);
            }
            else
            {
                tThis[sReference] = tThis[sReference] + sQuantity;
            }
            SetReferenceAndQuantity(tThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveData(NWDBasis<K> sData)
        {
            // I compare all element
            Dictionary<string, int> tThis = GetReferenceAndQuantity();
            if (tThis.ContainsKey(sData.Reference) == true)
            {
                tThis.Remove(sData.Reference);
            }
            SetReferenceAndQuantity(tThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] GetReachableDatas()
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
        public void SetReferenceAndQuantity(Dictionary<string, int> sDico)
        {
            List<string> tValueList = new List<string>();
            foreach (KeyValuePair<string, int> tKeyValue in sDico)
            {
                tValueList.Add(tKeyValue.Key + NWDConstants.kFieldSeparatorB + NWDToolbox.IntToString(tKeyValue.Value));
            }
            string[] tNextValueArray = tValueList.Distinct().ToArray();
            string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
            tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
            Value = tNextValue;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, int> GetReferenceAndQuantity()
        {
            Dictionary<string, int> tValueDico = new Dictionary<string, int>();
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
                        tValueDico.Add(tLineValue[0], tQ);
                    }
                }
            }
            return tValueDico;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<K, int> GetReachableDatasAndQuantities()
        {
            Dictionary<K, int> tValueDico = new Dictionary<K, int>();
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
                        K tObject = NWDBasisHelper.GetReachableDataByReference<K>(tLineValue[0]) as K;
                        if (tObject != null)
                        {
                            tValueDico.Add(tObject, tQ);
                        }
                    }
                }
            }
            return tValueDico;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<K, int> GetRawDatasAndQuantities()
        {
            Dictionary<K, int> tValueDico = new Dictionary<K, int>();
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
                        K tObject = NWDBasisHelper.GetRawDataByReference<K>(tLineValue[0]) as K;
                        if (tObject != null)
                        {
                            tValueDico.Add(tObject, tQ);
                        }
                    }
                }
            }
            return tValueDico;
        }
        //-------------------------------------------------------------------------------------------------------------
        public K GetRawFirstDataByRandom()
        {
            List<K> tList = GetRawDatasList();

            if (tList.Count == 0)
            {
                return null;
            }

            int tRandom = UnityEngine.Random.Range(0, tList.Count);
            return tList[tRandom];
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<K> GetRawDatasByRandom(int sQuantity)
        {
            List<K> tList = GetRawDatasList();
            while (tList.Count > sQuantity)
            {
                int tRandom = UnityEngine.Random.Range(0, tList.Count);
                tList.RemoveAt(tRandom);
            }
            Debug.LogWarning("RETURN " + tList.Count.ToString() + "/" + sQuantity);
            return tList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<K> GetRawDatasList()
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
                        K tObject = NWDBasisHelper.GetRawDataByReference<K>(tLineValue[0]) as K;
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
            Dictionary<string, int> tDescDico = GetReferenceAndQuantity();
            foreach (KeyValuePair<string, int> tKeyValue in tDescDico)
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
        //public void EditorAddNewData()
        //{
        //    K tNewObject = NWDBasisHelper.NewData<K>();
        //    this.AddDataQuantity(tNewObject, 1);
        //    NWDBasisHelper.BasisHelper<K>().New_SetObjectInEdition(tNewObject, false, true);
        //}
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
        //public override float ControlFieldHeight()
        //{
        //    int tRow = 1;
        //    if (Value != null && Value != string.Empty)
        //    {
        //        string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
        //        tRow += tValueArray.Count();
        //    }
        //    float tHeight = (NWDGUI.kFieldMarge + NWDGUI.kDataSelectorFieldStyle.fixedHeight) * tRow - NWDGUI.kFieldMarge;
        //    return tHeight;
        //}
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = BTBConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDReferencesQuantityType<K> tTemporary = new NWDReferencesQuantityType<K>();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            tTemporary.Value = Value;
            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;

            float tIntWidth = NWDGUI.kIntWidth;
            float tEditWidth = NWDGUI.kEditWidth;
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
                int tQ = 1;
                string tV = string.Empty;
                string tLine = tValueList.ElementAt(i);
                string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                if (tLineValue.Length == 2)
                {
                    tV = tLineValue[0];
                    tQ = NWDToolbox.IntFromString(tLineValue[1]);
                }

                tV = NWDDatasSelector.Field(NWDBasisHelper.FindTypeInfos(typeof(K)), new Rect(tX, tY, tWidth, NWDGUI.kDataSelectorFieldStyle.fixedHeight), tContent, tV,sDisabled, tIntWidth + NWDGUI.kFieldMarge * 2);
                if (string.IsNullOrEmpty(tV) == false)
                {
                    int tIndentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;
                    tQ = EditorGUI.IntField(new Rect(tX + tWidth - tIntWidth - tEditWidth - NWDGUI.kFieldMarge * 2, tY + NWDGUI.kDatasSelectorYOffset, tIntWidth + NWDGUI.kFieldMarge, NWDGUI.kTextFieldStyle.fixedHeight), tQ);

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
                    if (!tValueListReferenceAllReady.Contains(tV))
                    {
                        tValueList[i] = tV + NWDConstants.kFieldSeparatorB + NWDToolbox.IntToString(tQ);
                        tValueListReferenceAllReady.Add(tV);
                    }
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
        public override void CreatePloters(NWDNodeCard sNodalCard, float tHeight)
        {
            int tCounter = 0;
            foreach (string tRef in GetSortedReferences())
            {
                sNodalCard.PloterList.Add(new NWDNodePloter(sNodalCard, tRef,
                    new Vector2(0,
                    tHeight
                   + (NWDGUI.kFieldMarge * tCounter) + NWDGUI.kDataSelectorFieldStyle.fixedHeight * (tCounter + 0.5f)
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