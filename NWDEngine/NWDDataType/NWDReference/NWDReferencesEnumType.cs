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

//using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDReferencesEnumType used to put a reference with float in value. Use properties with name, like 'ItemQuantity', 'SpotQuantity', 'BonusQuantity' , etc.
    /// </summary>
    [SerializeField]
    public class NWDReferencesEnumType<K, E> : NWDReferenceMultiple where E : Enum where K : NWDBasis, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesEnumType()
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
        public void SetDataState(K sData, E sState)
        {
            Dictionary<string, E> tThis = GetReferenceAndState();
            if (tThis.ContainsKey(sData.Reference) == false)
            {
                tThis.Add(sData.Reference, sState);
            }
            else
            {
                tThis[sData.Reference] = sState;
            }
            SetReferenceAndState(tThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetReferenceState(string sReference, E sState)
        {
            Dictionary<string, E> tThis = GetReferenceAndState();
            if (tThis.ContainsKey(sReference) == false)
            {
                tThis.Add(sReference, sState);
            }
            else
            {
                tThis[sReference] = sState;
            }
            SetReferenceAndState(tThis);
        }
        //-------------------------------------------------------------------------------------------------------------
        public E GetDataState(K sData, E sStateByDefault)
        {
            E rReturn = sStateByDefault;
            Dictionary<string, E> tThis = GetReferenceAndState();
            if (tThis.ContainsKey(sData.Reference) == true)
            {
                rReturn = tThis[sData.Reference];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public E GetReferenceState(string sReference, E sStateByDefault)
        {
            E rReturn = sStateByDefault;
            Dictionary<string, E> tThis = GetReferenceAndState();
            if (tThis.ContainsKey(sReference) == true)
            {
                rReturn = tThis[sReference];
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveData(K sData)
        {
            Dictionary<string, E> tThis = GetReferenceAndState();
            if (tThis.ContainsKey(sData.Reference) == true)
            {
                tThis.Remove(sData.Reference);
            }
            SetReferenceAndState(tThis);
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
        public void SetReferenceAndState(Dictionary<string, E> sDico)
        {
            List<string> tValueList = new List<string>();
            foreach (KeyValuePair<string, E> tKeyValue in sDico)
            {
                tValueList.Add(tKeyValue.Key + NWDConstants.kFieldSeparatorB + NWDToolbox.IntToString(Convert.ToInt32(tKeyValue.Value)));
            }
            string[] tNextValueArray = tValueList.Distinct().ToArray();
            string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
            tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
            Value = tNextValue;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, E> GetReferenceAndState()
        {
            Dictionary<string, E> tValueDico = new Dictionary<string, E>();
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        E tQ = (E)(object)NWDToolbox.IntFromString(tLineValue[1]);
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
            Dictionary<string, E> tDescDico = GetReferenceAndState();
            foreach (KeyValuePair<string, E> tKeyValue in tDescDico)
            {
                K tObject = NWDBasisHelper.GetRawDataByReference<K>(tKeyValue.Key);
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
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDReferencesEnumType<K, E> tTemporary = new NWDReferencesEnumType<K, E>();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            tTemporary.Value = Value;
            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;

            float tIntWidth = NWDGUI.kIntWidth;
            float tEditWidth = NWDGUI.kEditWidth;
            List<string> tValueList = new List<string>();
            List<string> tValueListReferenceAlReady = new List<string>();
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
                E tQ = (E)(object)0;

                string tV = string.Empty;
                string tLine = tValueList.ElementAt(i);
                string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                if (tLineValue.Length == 2)
                {
                    tV = tLineValue[0];
                    tQ =  (E)(object)NWDToolbox.IntFromString(tLineValue[1]);
                }

                tV = NWDDatasSelector.Field(NWDBasisHelper.FindTypeInfos(typeof(K)), new Rect(tX, tY, tWidth, NWDGUI.kDataSelectorFieldStyle.fixedHeight), tContent, tV,sDisabled, tIntWidth + NWDGUI.kFieldMarge * 2);
                if (string.IsNullOrEmpty(tV) == false)
                {
                    int tIndentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;
                    tQ = (E)EditorGUI.EnumPopup(new Rect(tX + tWidth - tIntWidth - tEditWidth - NWDGUI.kFieldMarge * 2, tY + NWDGUI.kDatasSelectorYOffset, tIntWidth + NWDGUI.kFieldMarge, NWDGUI.kTextFieldStyle.fixedHeight), tQ);
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
                    if (!tValueListReferenceAlReady.Contains(tV))
                    {
                        tValueList[i] = tV + NWDConstants.kFieldSeparatorB + NWDToolbox.IntToString(Convert.ToInt32(tQ));
                        tValueListReferenceAlReady.Add(tV);
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