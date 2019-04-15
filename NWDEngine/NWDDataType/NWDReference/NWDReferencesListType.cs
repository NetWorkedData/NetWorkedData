// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:28:26
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
    /// <summary>
    /// NWDReferencesListType used to put a reference with float in value. Use properties with name, like 'ItemList', 'SpotList', 'BonusList' , etc.
    /// </summary>
	[SerializeField]
    public class NWDReferencesListType<K> : NWDReferenceMultiple where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesListType()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
        {
            Value = string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool ContainsReference(string sReference)
        {
            if (sReference == null)
            {
                return false;
            }
            return Value.Contains(sReference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetReferences(string[] sReferences)
        {
            List<string> tList = new List<string>();
            foreach (string tReference in sReferences)
            {
                tList.Add(tReference);
            }
            string[] tNextValueArray = tList.Distinct().ToArray();
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
            string[] tNextValueArray = tList.Distinct().ToArray();
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
            string[] tNextValueArray = tList.Distinct().ToArray();
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
        public bool ContainsObject(K sObject)
        {
            if (sObject == null)
            {
                return false;
            }
            return Value.Contains(sObject.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] GetObjects(string sAccountReference = null)
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
        public List<K> GetObjectsList(string sAccountReference = null)
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
            return tList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] GetObjectsAbsolute()
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
        public List<K> GetObjectsAbsoluteList()
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
            return tList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetObjects(K[] sObjects)
        {
            List<string> tList = new List<string>();
            foreach (K tObject in sObjects)
            {
                tList.Add(tObject.Reference);
            }
            SetReferences(tList.ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddObject(K sObject)
        {
            if (sObject != null)
            {
                AddReferences(new string[] { sObject.Reference });
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddObjects(K[] sObjects)
        {
            List<string> tList = new List<string>();
            foreach (K tObject in sObjects)
            {
                tList.Add(tObject.Reference);
            }
            AddReferences(tList.ToArray());
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveObjects(K[] sObjects)
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
        //[NWDAliasMethod(NWDConstants.M_EditorGetObjects)]
        public override object[] EditorGetObjects()
        {
            List<K> rReturn = new List<K>();
            foreach (string tReference in GetReferences())
            {
                K tObj = NWDBasis<K>.RawDataByReference(tReference);
                //if (tObj != null)
                {
                    rReturn.Add(tObj);
                }
            }
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void EditorAddNewObject()
        {
            K tNewObject = NWDBasis<K>.NewData();
            this.AddObject(tNewObject);
            NWDBasis<K>.BasisHelper().New_SetObjectInEdition(tNewObject, false, true);
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
            NWDReferencesListType<K> tTemporary = new NWDReferencesListType<K>();
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
                tV = NWDDatasSelector.Field(NWDBasisHelper.FindTypeInfos(typeof(K)), new Rect(tX, tY, tWidth, NWDGUI.kDataSelectorFieldStyle.fixedHeight), tContent, tV);
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
            List<string> tDistinct = new List<string>();
            foreach (string tVD in tValueList)
            {
                if (tDistinct.Contains(tVD) == false)
                {
                    tDistinct.Add(tVD);
                }
            }
            string[] tNextValueArray = tDistinct.ToArray();
            string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray) + tNewReference;
            tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
            tTemporary.Value = tNextValue;
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        //public string ChangeReferenceForAnother(string sOldReference, string sNewReference)
        //{
        //	string rReturn = "NO";
        //	if (Value != null) {
        //		if (Value.Contains (sOldReference)) {
        //			Value = Value.Replace (sOldReference, sNewReference);
        //			rReturn = "YES";
        //		}
        //	}
        //	return rReturn;
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================