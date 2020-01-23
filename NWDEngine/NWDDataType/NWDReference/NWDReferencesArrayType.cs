//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:28:22
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
    /// NWDReferencesArrayType used to put a reference with float in value. Use properties with name, like 'ItemArray', 'SpotArray', 'BonusArray' , etc.
    /// </summary>
    [SerializeField]
    public class NWDReferencesArrayType<K> : NWDReferenceMultiple where K : NWDBasis, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesArrayType()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesArrayType(string[] sReferences)
        {
            Value = string.Empty;
            this.SetReferences(sReferences);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesArrayType(K[] sObjects)
        {
            Value = string.Empty;
            this.SetDatas(sObjects);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesArrayType(NWDReferencesArrayType<K> sArray)
        {
            Value = string.Empty + sArray.Value;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetReferences(string[] sReferences)
        {
            List<string> tList = new List<string>();
            foreach (string tReference in sReferences)
            {
                tList.Add(tReference);
            }
            string[] tNextValueArray = tList.ToArray();
            Value = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
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
                tList.Add(tReference);
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
                tList.Remove(tReference);
            }
            string[] tNextValueArray = tList.ToArray();
            Value = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string[] GetReferences()
        {
            return Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
        }
        //-------------------------------------------------------------------------------------------------------------
        public string[] GetSortedReferences()
        {
            string[] tResult = GetReferences();
            Array.Sort(tResult, StringComparer.InvariantCulture);
            return tResult;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddReferencesArray(NWDReferencesArrayType<K> sArrayToRemove)
        {
            this.AddReferences(sArrayToRemove.GetReferences());
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveReferencesArray(NWDReferencesArrayType<K> sArrayToRemove)
        {
            this.RemoveReferences(sArrayToRemove.GetReferences());
        }
        //-------------------------------------------------------------------------------------------------------------
        //		public void Epurate ()
        //		{
        //			List<string> tList = new List<string>();
        //			foreach (string tObject in GetReferences ()) {
        //				tList.Add (tObject);
        //			}
        //			tList.Distinct();
        //			tList.Sort((a, b) => a.Length.CompareTo(b.Length));
        //			Value = string.Join (NWDConstants.kFieldSeparatorA, tList.ToArray());
        //		}
        //-------------------------------------------------------------------------------------------------------------
        public string ToStringSorted()
        {
            List<string> tList = new List<string>();
            foreach (string tObject in GetReferences())
            {
                tList.Add(tObject);
            }
            tList.Sort();
            return string.Join(NWDConstants.kFieldSeparatorA, tList.ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ContainsData(K sData)
        {
            if (sData == null)
            {
                return false;
            }
            return Value.Contains(sData.Reference);
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
        public List<K> GetReachableDatasList()
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
            return tList;
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
        public List<K> GetRawDatasList()
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
            return tList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetDatas(K[] sObjects)
        {
            List<string> tList = new List<string>();
            foreach (K tObject in sObjects)
            {
                if (tObject != null)
                {
                    tList.Add(tObject.Reference);
                }
                else
                {
                    tList.Add(string.Empty);
                }
            }
            SetReferences(tList.ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddData(K sObject)
        {
            if (sObject != null)
            {
                AddReferences(new string[] { sObject.Reference });
            }
            else
            {
                AddReferences(new string[] { string.Empty });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddDatas(K[] sObjects)
        {
            List<string> tList = new List<string>();
            foreach (K tObject in sObjects)
            {
                if (tObject != null)
                {
                    tList.Add(tObject.Reference);
                }
                else
                {
                    tList.Add(string.Empty);
                }
            }
            AddReferences(tList.ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveDatas(K[] sObjects)
        {
            List<string> tList = new List<string>();
            foreach (K tObject in sObjects)
            {
                tList.Add(tObject.Reference);
            }
            RemoveReferences(tList.ToArray());
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
        public override object[] GetEditorDatas()
        {
            List<K> rReturn = new List<K>();
            foreach (string tReference in GetReferences())
            {
                K tObj = NWDBasisHelper.GetRawDataByReference<K>(tReference);
                //if (tObj != null)
                {
                    rReturn.Add(tObj);
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
                if (NWDBasisHelper.GetRawDataByReference<K>(tReference) == null)
                {
                    rReturn.Add(tReference);
                }
            }
            return rReturn;
        }
        ////-------------------------------------------------------------------------------------------------------------
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
            NWDReferencesArrayType<K> tTemporary = new NWDReferencesArrayType<K>();
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
            string tNewReference = string.Empty;
            for (int i = 0; i < tValueList.Count; i++)
            {
                if (i > 0)
                {
                    tContent = new GUIContent("   ");
                }
                string tV = tValueList.ElementAt(i);
                tV = NWDDatasSelector.Field(NWDBasisHelper.FindTypeInfos(typeof(K)), new Rect(tX, tY, tWidth, NWDGUI.kDataSelectorFieldStyle.fixedHeight), tContent, tV, sDisabled);
                if (string.IsNullOrEmpty(tV) == false)
                {
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
                    tValueList[i] = tV;
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
            tValueList.Remove(NWDConstants.kFieldSeparatorA);
            string[] tNextValueArray = tValueList.ToArray();
            string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray) + tNewReference;
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================