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
    public class NWDAverage
    {
        //-------------------------------------------------------------------------------------------------------------
        public float Total;
        public float Counter;
        public float Average;
        //-------------------------------------------------------------------------------------------------------------
        public NWDAverage()
        {
            Total = 0.0F;
            Counter = 0.0F;
            Average = 0.0F;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAverage(string sString)
        {
            string[] tValueArray = sString.Split(new string[] { NWDConstants.kFieldSeparatorC }, StringSplitOptions.RemoveEmptyEntries);
            if (tValueArray.Length > 0)
            {
                Total = NWDToolbox.FloatFromString(tValueArray[0]);
                //float.TryParse(tValueArray[0], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out Total);
            }
            if (tValueArray.Length > 1)
            {
                Counter = NWDToolbox.FloatFromString(tValueArray[1]);
                //float.TryParse(tValueArray[1], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out Counter);
            }
            if (tValueArray.Length > 2)
            {
                Average = NWDToolbox.FloatFromString(tValueArray[1]);
                //float.TryParse(tValueArray[2], System.Globalization.NumberStyles.Float, NWDConstants.FormatCountry, out Average);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAverage(float sValue)
        {
            AddValue(sValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        new public string ToString()
        {
            //return Total.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorC +
                        //Counter.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry) + NWDConstants.kFieldSeparatorC +
                        //Average.ToString(NWDConstants.FloatFormat, NWDConstants.FormatCountry);
            return NWDToolbox.FloatToString(Total) + NWDConstants.kFieldSeparatorC +
                       NWDToolbox.FloatToString(Counter) + NWDConstants.kFieldSeparatorC +
                       NWDToolbox.FloatToString(Average);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddValue(float sValue)
        {
            Total += sValue;
            Counter = Counter + 1.0F;
            ReEvaluate();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveValue(float sValue)
        {
            if (Counter > 0)
            {
                Total += sValue;
                Counter = Counter - 1.0F;
                ReEvaluate();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetValue()
        {
            Total = 0.0F;
            Counter = 0.0F;
            Average = 0.0F;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ReEvaluate()
        {
            if (Counter <= 0)
            {
                Total = 0.0F;
                Average = 0.0F;
            }
            else
            {
                Average = Total / Counter;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDReferencesAverageType used to put a reference with float in value. Use properties with name, like 'ItemAverage', 'SpotAverage', 'BonusAverage' , etc.
    /// </summary>
    [SerializeField]
    public class NWDReferencesAverageType<K> : BTBDataType where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesAverageType()
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
            SetReferenceAndAverage(GetReferenceAndAverage());
        }
        //-------------------------------------------------------------------------------------------------------------
        //public bool ContainedIn(NWDReferencesAverageType<K> sReferencesProportion, bool sExceptIfIsEmpty = true)
        //{
        //    bool rReturn = true;
        //    if (sExceptIfIsEmpty && Value == "")
        //    {
        //        return false;
        //    }
        //    // I compare all elemnt
        //    Dictionary<string, NWDAverage> tThis = GetReferenceAndProportion();
        //    Dictionary<string, NWDAverage> tOther = sReferencesProportion.GetReferenceAndProportion();

        //    foreach (KeyValuePair<string, NWDAverage> tKeyValue in tThis)
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
        //public bool ContainsReferencesProportion(NWDReferencesAverageType<K> sReferencesProportion)
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
        //public bool RemoveReferencesQuantity(NWDReferencesAverageType<K> sReferencesProportion, bool sCanBeNegative = true, bool sRemoveEmpty = true)
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
        //public void RemoveObjectQuantity(NWDBasis<K> sObject, float sQuantity, bool sCanBeNegative = true, bool sRemoveEmpty = true)
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
        //public void AddReferencesQuantity(NWDReferencesAverageType<K> sReferencesProportion)
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
        public void AddObjectValue(NWDBasis<K> sObject, float sValue)
        {
            // I compare all element
            Dictionary<string, NWDAverage> tThis = GetReferenceAndAverage();
            if (tThis.ContainsKey(sObject.Reference) == false)
            {
                tThis.Add(sObject.Reference, new NWDAverage(sValue));
            }
            else
            {
                tThis[sObject.Reference].AddValue(sValue);
            }
            SetReferenceAndAverage(tThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveObject(NWDBasis<K> sObject)
        {
            // I compare all element
            Dictionary<string, NWDAverage> tThis = GetReferenceAndAverage();
            if (tThis.ContainsKey(sObject.Reference) == true)
            {
                tThis.Remove(sObject.Reference);
            }
            SetReferenceAndAverage(tThis);
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
        public void SetReferenceAndAverage(Dictionary<string, NWDAverage> sDico)
        {
            List<string> tValueList = new List<string>();
            foreach (KeyValuePair<string, NWDAverage> tKeyValue in sDico)
            {
                tValueList.Add(tKeyValue.Key + NWDConstants.kFieldSeparatorB + tKeyValue.Value.ToString());
            }
            string[] tNextValueArray = tValueList.Distinct().ToArray();
            string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
            tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
            Value = tNextValue;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, NWDAverage> GetReferenceAndAverage()
        {
            Dictionary<string, NWDAverage> tValueDico = new Dictionary<string, NWDAverage>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        NWDAverage tQ = new NWDAverage(tLineValue[1]);
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
        public Dictionary<K, NWDAverage> GetObjectAndAverage(string sAccountReference = null)
        {
            Dictionary<K, NWDAverage> tValueDico = new Dictionary<K, NWDAverage>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        NWDAverage tQ = new NWDAverage(tLineValue[1]);
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
        public Dictionary<K, NWDAverage> GetObjectAndAverageAbsolute()
        {
            Dictionary<K, NWDAverage> tValueDico = new Dictionary<K, NWDAverage>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        NWDAverage tQ = new NWDAverage(tLineValue[1]);
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
        public string Description()
        {
            string rDescription = string.Empty;
            Dictionary<string, NWDAverage> tDescDico = GetReferenceAndAverage();
            foreach (KeyValuePair<string, NWDAverage> tKeyValue in tDescDico)
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
            this.AddObjectValue(tNewObject, 0.0F);
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
            int tConnection = 0;
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                tRow += tValueArray.Count();
                List<string> tValueListERROR = ReferenceInError(new List<string>(GetReferences()));
                if (tValueListERROR.Count > 0)
                {
                    tConnection = 1;
                }
            }
            if (tRow > 1)
            {
                tRow--;
            }
            float tHeight = (NWDConstants.kTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge) *2* tRow - NWDConstants.kFieldMarge +
                tConnection * (NWDConstants.kRedLabelStyle.fixedHeight + NWDConstants.kFieldMarge +
                               //tLabelAssetStyle.fixedHeight+NWDConstants.kFieldMarge+
                               NWDConstants.kMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge);

            // test if error in reference and add button height
            if (Value != null && Value != string.Empty)
            {
                if (ReferenceInError(new List<string>(Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries))).Count > 0)
                {
                    tHeight = tHeight + NWDConstants.kMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
                }
            }

            return tHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING)
        {
            //NWDConstants.LoadImages();
            //NWDConstants.LoadStyles();
            NWDReferencesAverageType<K> tTemporary = new NWDReferencesAverageType<K>();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            tTemporary.Value = Value;
            //Type sFromType = typeof(K);
            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;

            float tIntWidth = NWDConstants.kIntWidth;
            float tEditWidth = NWDConstants.kEditWidth;

            bool tConnection = true;

            List<string> tReferenceList = new List<string>();
            List<string> tInternalNameList = new List<string>();

            tReferenceList.Add(NWDConstants.kFieldSeparatorA);
            tInternalNameList.Add(NWDConstants.kFieldNone);

            foreach (KeyValuePair<string, string> tKeyValue in NWDBasisHelper.FindTypeInfos(typeof(K)).EditorDatasMenu.OrderBy(i => i.Value))
            {
                tReferenceList.Add(tKeyValue.Key);
                tInternalNameList.Add(tKeyValue.Value);
            }

            //tReferenceList.Add(NWDConstants.kFieldSeparatorA);
            //tInternalNameList.Add(" ");

            //var tReferenceListInfo = sFromType.GetField("ObjectsByReferenceList", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            //if (tReferenceListInfo != null)
            //{
            //    tReferenceList.AddRange(tReferenceListInfo.GetValue(null) as List<string>);
            //}

            //var tInternalNameListInfo = sFromType.GetField("ObjectsInEditorTableKeyList", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            //if (tInternalNameListInfo != null)
            //{
            //    tInternalNameList.AddRange(tInternalNameListInfo.GetValue(null) as List<string>);
            //}

            List<GUIContent> tContentFuturList = new List<GUIContent>();
            foreach (string tS in tInternalNameList.ToArray())
            {
                tContentFuturList.Add(new GUIContent(tS));
            }

            List<string> tValueList = new List<string>();
            List<string> tValueListReferenceAllReady = new List<string>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                tValueList = new List<string>(tValueArray);
            }

            List<string> tValueListERROR = ReferenceInError(new List<string>(GetReferences()));
            if (tValueListERROR.Count > 0)
            {
                tConnection = false;
            }

            EditorGUI.BeginDisabledGroup(!tConnection);

            bool tUp = false;
            bool tDown = false;
            int tIndexToMove = -1;

            tValueList.Add(string.Empty);
            string tNewReferenceQuantity = string.Empty;
            for (int i = 0; i < tValueList.Count; i++)
            {
                //string tFieldName = sEntitled;
                if (i > 0)
                {
                    //tFieldName = "   ";
                    tContent = new GUIContent("   ");
                }

                int tIndex = 0;
                NWDAverage tQ = new NWDAverage(string.Empty);
                string tV = string.Empty;
                string tLine = tValueList.ElementAt(i);
                string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                if (tLineValue.Length == 2)
                {
                    tV = tLineValue[0];
                    tQ = new NWDAverage(tLineValue[1]);
                    tIndex = tReferenceList.IndexOf(tV);
                }

                tIndex = EditorGUI.Popup(new Rect(tX, tY, tWidth - tEditWidth - NWDConstants.kFieldMarge , NWDConstants.kPopupdStyle.fixedHeight), tContent, tIndex, tContentFuturList.ToArray(), NWDConstants.kPopupdStyle);

                if (tValueListERROR.Contains(tV))
                {
                    GUI.Label(new Rect(tX + EditorGUIUtility.labelWidth + NWDConstants.kFieldMarge, tY + 1, tWidth - tIntWidth - EditorGUIUtility.labelWidth - NWDConstants.kFieldMarge * 4 - tEditWidth, NWDConstants.kGrayLabelStyle.fixedHeight), "? <" + tV + ">", NWDConstants.kGrayLabelStyle);
                }

                if (tIndex > 0)
                {
                    float tYb = tY + NWDConstants.kPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                    float tXb = sPosition.x + EditorGUIUtility.labelWidth;
                    float tTiersWidth = Mathf.Ceil((sPosition.width - EditorGUIUtility.labelWidth + NWDConstants.kFieldMarge) / 3.0F);
                    float tTiersWidthB = tTiersWidth - NWDConstants.kFieldMarge;

                    //remove EditorGUI.indentLevel to draw next controller without indent 
                    int tIndentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;

                    EditorGUI.LabelField(new Rect(tX , tYb, EditorGUIUtility.labelWidth-NWDConstants.kFieldMarge, NWDConstants.kLabelRightStyle.fixedHeight), "Average",NWDConstants.kLabelRightStyle);
                    tQ.Total = EditorGUI.FloatField(new Rect(tXb, tYb, tTiersWidthB, NWDConstants.kTextFieldStyle.fixedHeight), tQ.Total);
                    tQ.Counter = EditorGUI.FloatField(new Rect(tXb + tTiersWidth, tYb, tTiersWidthB, NWDConstants.kTextFieldStyle.fixedHeight), tQ.Counter);
                    tQ.Average = EditorGUI.FloatField(new Rect(tXb + tTiersWidth * 2, tYb, tTiersWidthB, NWDConstants.kTextFieldStyle.fixedHeight), tQ.Average);
                    tQ.ReEvaluate();
                    EditorGUI.indentLevel = tIndentLevel;
                    //if (GUI.Button(new Rect(tX + tWidth - tEditWidth, tY, tEditWidth, tPopupdStyle.fixedHeight), "!"))
                    GUIContent tDeleteContent = new GUIContent(NWDConstants.kImageTabReduce, "edit");
                    if (GUI.Button(new Rect(tX + tWidth - tEditWidth, tY, tEditWidth, NWDConstants.kPopupButtonStyle.fixedHeight), tDeleteContent, NWDConstants.kPopupButtonStyle))
                    {
                        NWDBasis<K>.SetObjectInEdition(NWDBasis<K>.GetDataByReference(tReferenceList.ElementAt(tIndex)), false);
                    }
                    if (i > 0)
                    {
                        GUIContent tUpContent = new GUIContent(NWDConstants.kImageUp, "up");
                        if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth - (tEditWidth + NWDConstants.kFieldMarge) * 2, tY, tEditWidth, NWDConstants.kPopupButtonStyle.fixedHeight), tUpContent, NWDConstants.kPopupButtonStyle))
                        {
                            tUp = true;
                            tIndexToMove = i;
                        }
                    }
                    if (i < tValueList.Count - 2)
                    {
                        GUIContent tDownContent = new GUIContent(NWDConstants.kImageDown, "down");
                        if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth - (tEditWidth + NWDConstants.kFieldMarge), tY, tEditWidth, NWDConstants.kPopupButtonStyle.fixedHeight), tDownContent, NWDConstants.kPopupButtonStyle))
                        {
                            tDown = true;
                            tIndexToMove = i;
                        }
                    }
                }
                else
                {
                    GUIContent tNewContent = new GUIContent(NWDConstants.kImageNew, "new");
                    if (GUI.Button(new Rect(tX + tWidth - tEditWidth, tY, tEditWidth, NWDConstants.kPopupButtonStyle.fixedHeight), tNewContent, NWDConstants.kPopupButtonStyle))
                    {
                        NWDBasis<K> tNewObject = NWDBasis<K>.NewData();
                        tNewReferenceQuantity = NWDConstants.kFieldSeparatorA + tNewObject.Reference + NWDConstants.kFieldSeparatorB + "1";
                        NWDBasis<K>.SetObjectInEdition(tNewObject, false, true);
                    }
                }

                tY += NWDConstants.kPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                tY += NWDConstants.kPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;

                if (tIndex > 0 && tIndex < tReferenceList.Count)
                {
                    //if (tQ != 0) {
                    if (!tValueListReferenceAllReady.Contains(tReferenceList.ElementAt(tIndex)))
                    {
                        tValueList[i] = tReferenceList.ElementAt(tIndex) + NWDConstants.kFieldSeparatorB + tQ.ToString();
                        tValueListReferenceAllReady.Add(tReferenceList.ElementAt(tIndex));
                    }
                }
                else
                {
                    tValueList[i] = string.Empty;
                }
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


            EditorGUI.EndDisabledGroup();

            if (tConnection == false)
            {
                tTemporary.Value = Value;

                GUI.Label(new Rect(tX + EditorGUIUtility.labelWidth, tY, tWidth, NWDConstants.kRedLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_LIST_ERROR, NWDConstants.kRedLabelStyle);
                tY = tY + NWDConstants.kFieldMarge + NWDConstants.kRedLabelStyle.fixedHeight;
                //				GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelAssetStyle.fixedHeight), Value.Replace (NWDAssetType.kAssetDelimiter, ""),tLabelAssetStyle);
                //				tY = tY + NWDConstants.kFieldMarge + tLabelAssetStyle.fixedHeight;
                NWDConstants.GUIRedButtonBegin();
                if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth, tY, 60.0F, NWDConstants.kDeleteButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_CLEAN, NWDConstants.kDeleteButtonStyle))
                {
                    Dictionary<string, NWDAverage> tDicoClean = GetReferenceAndAverage();
                    foreach (string tDeleteReference in tValueListERROR)
                    {
                        tDicoClean.Remove(tDeleteReference);
                    }
                    tTemporary.SetReferenceAndAverage(tDicoClean);
                }
                NWDConstants.GUIRedButtonEnd();
                tY = tY + NWDConstants.kFieldMarge + NWDConstants.kMiniButtonStyle.fixedHeight;
            }
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
                    //Debug.Log("I CHANGE " + sOldReference + " FOR " + sNewReference + "");
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