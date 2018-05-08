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
        UpperThan =1,
        UpperThanOrEqual = 2,
        LowerThan = 3,
        LowerThanOrEqual = 4,
        DifferentTo = 5,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
    public class NWDReferenceQuantityConditional<K> : BTBDataType where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Reference;
        public int Quantity;
        public NWDConditional Condition;
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferenceQuantityConditional(string sValue)
        {
            string[] tLineValue = sValue.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
            if (tLineValue.Length == 2)
            {
                Reference = tLineValue[0];
                int.TryParse(tLineValue[1], out Quantity);
                Condition = NWDConditional.EqualTo;
            }
            if (tLineValue.Length == 3)
            {
                Reference = tLineValue[0];
                int.TryParse(tLineValue[1], out Quantity);
                Condition = (NWDConditional)Enum.Parse(typeof(NWDConditional), tLineValue[2], true);
            }
            ReEvalute();
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferenceQuantityConditional(string sReference, int sQuantity, NWDConditional sCondition)
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
            K tObject = NWDBasis<K>.GetObjectByReference(Reference);
            string rDescription = Reference;
            if (tObject == null)
            {
                rDescription = Reference+" (in error) ";
            }
            else if (tObject.InternalKey == null)
            {
                rDescription = tObject.InternalKey;
            }
            switch (Condition)
            {
                case NWDConditional.DifferentTo:
                    {
                        rDescription += " != " + Quantity +"\r\n";
                    }
                    break;
                case NWDConditional.EqualTo:
                    {
                        rDescription += " !== " + Quantity+ "\r\n";
                    }
                    break;
                case NWDConditional.UpperThan:
                    {
                        rDescription += " > " + Quantity+ "\r\n";
                    }
                    break;
                case NWDConditional.UpperThanOrEqual:
                    {
                        rDescription += " >= " + Quantity+ "\r\n";
                    }
                    break;
                case NWDConditional.LowerThan:
                    {

                        rDescription += " < " + Quantity+ "\r\n";
                    }
                    break;
                case NWDConditional.LowerThanOrEqual:
                    {
                        rDescription += " <= " + Quantity+ "\r\n";
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
           return NWDBasis<K>.GetObjectByReference(Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool isValid(int sQuantity)
        {
            bool rReturn = true;
            switch (Condition)
            {
                case NWDConditional.DifferentTo :
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
                default :
                    break;
            }

            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
    public class NWDReferencesConditionalType<K> : BTBDataType where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesConditionalType()
        {
            Value = "";
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
            if (Value != "")
            {
                rReturn = false;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsNotEmpty()
        {
            bool rReturn = false;
            if (Value != "")
            {
                rReturn = true;
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //public bool ContainedIn(NWDReferencesConditionalType<K> sReferencesQuantity, bool sExceptIfIsEmpty = true)
        //{
        //    bool rReturn = true;
        //    if (sExceptIfIsEmpty && Value == "")
        //    {
        //        return false;
        //    }
        //    // I compare all elemnt
        //    Dictionary<string, int> tThis = GetReferenceAndQuantity();
        //    Dictionary<string, int> tOther = sReferencesQuantity.GetReferenceAndQuantity();
        //    foreach (KeyValuePair<string, int> tKeyValue in tThis)
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
        public K[] GetObjects(string sAccountReference = null)
        {
            List<K> tList = new List<K>();
            string[] tArray = GetReferences();
            foreach (string tRef in tArray)
            {
                K tObject = NWDBasis<K>.GetObjectByReference(tRef, sAccountReference) as K;
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
                K tObject = NWDBasis<K>.GetObjectAbsoluteByReference(tRef) as K;
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
            if (Value != null && Value != "")
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        int tCount = 0;
                        int.TryParse(tLineValue[1], out tCount);
                        for (int tJ = 0; tJ < tCount; tJ++)
                        {
                            tValueList.Add(tLineValue[0]);
                        }
                    }
                }
            }
            return tValueList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetReferenceQuantityConditional(List<NWDReferenceQuantityConditional<K>> sList)
        {
            List<string> tValueList = new List<string>();
            foreach (NWDReferenceQuantityConditional<K> tKeyValue in sList)
            {
                tValueList.Add(tKeyValue.Value);
            }
            string[] tNextValueArray = tValueList.Distinct().ToArray();
            string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
            tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
            Value = tNextValue;
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<NWDReferenceQuantityConditional<K>> GetReferenceQuantityConditional()
        {
            List<NWDReferenceQuantityConditional<K>> rList = new List<NWDReferenceQuantityConditional<K>>();
            if (Value != null && Value != "")
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    rList.Add(new NWDReferenceQuantityConditional<K>(tLine));
                }
            }
            return rList;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string Description()
        {
            List<NWDReferenceQuantityConditional<K>> tList = GetReferenceQuantityConditional();
            string rDescription = "";
            foreach (NWDReferenceQuantityConditional<K> tKeyValue in tList)
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
            if (Value != null && Value != "")
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        int tQ = 0;
                        int.TryParse(tLineValue[1], out tQ);
                        K tObject = NWDBasis<K>.GetObjectByReference(tLineValue[0]) as K;
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
        public override bool IsInError()
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
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] EditorGetObjects()
        {
            List<K> rReturn = new List<K>();
            foreach (string tReference in GetReferences())
            {
                K tObj = NWDBasis<K>.InstanceByReference(tReference) as K;
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
        //public void EditorAddNewObject()
        //{
        //    K tNewObject = NWDBasis<K>.NewObject();
        //    this.AddObjectQuantity(tNewObject,1);
        //    NWDBasis<K>.SetObjectInEdition(tNewObject, false, true);
        //}
        //-------------------------------------------------------------------------------------------------------------
        public List<string> ReferenceInError(List<string> sReferencesList)
        {
            List<string> rReturn = new List<string>();
            foreach (string tReference in sReferencesList)
            {
                if (NWDBasis<K>.InstanceByReference(tReference) == null)
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
            if (Value != null && Value != "")
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
            float tHeight = (NWDConstants.kTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge) * tRow - NWDConstants.kFieldMarge +
                tConnection * (NWDConstants.kRedLabelStyle.fixedHeight + NWDConstants.kFieldMarge +
                    //tLabelAssetStyle.fixedHeight+NWDConstants.kFieldMarge+
                               NWDConstants.kMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge);

            // test if error in reference and add button height
            if (Value != null && Value != "")
            {
                if (ReferenceInError(new List<string>(Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries))).Count > 0)
                {
                    tHeight = tHeight + NWDConstants.kMiniButtonStyle.fixedHeight + NWDConstants.kFieldMarge;
                }
            }

            return tHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, string sTooltips = "")
        {
            //NWDConstants.LoadImages();
            //NWDConstants.LoadStyles();
            NWDReferencesConditionalType<K> tTemporary = new NWDReferencesConditionalType<K>();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            tTemporary.Value = Value;
            Type sFromType = typeof(K);
            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;

            float tIntWidth = NWDConstants.kIntWidth;
            float tConWidth = NWDConstants.kConWidth;
            float tEditWidth = NWDConstants.kEditWidth;

            bool tConnection = true;

            List<string> tReferenceList = new List<string>();
            List<string> tInternalNameList = new List<string>();
            tReferenceList.Add(NWDConstants.kFieldSeparatorA);
            tInternalNameList.Add(" ");

            var tReferenceListInfo = sFromType.GetField("ObjectsByReferenceList", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (tReferenceListInfo != null)
            {
                tReferenceList.AddRange(tReferenceListInfo.GetValue(null) as List<string>);
            }

            var tInternalNameListInfo = sFromType.GetField("ObjectsInEditorTableKeyList", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (tInternalNameListInfo != null)
            {
                tInternalNameList.AddRange(tInternalNameListInfo.GetValue(null) as List<string>);
            }

            List<GUIContent> tContentFuturList = new List<GUIContent>();
            foreach (string tS in tInternalNameList.ToArray())
            {
                tContentFuturList.Add(new GUIContent(tS));
            }

            List<string> tValueList = new List<string>();
            List<string> tValueListReferenceAllReady = new List<string>();
            if (Value != null && Value != "")
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


            tValueList.Add("");
            string tNewReferenceQuantity = "";
            for (int i = 0; i < tValueList.Count; i++)
            {
                //string tFieldName = sEntitled;
                if (i > 0)
                {
                    //tFieldName = "   ";
                    tContent = new GUIContent("   ");
                }

                int tIndex = 0;
                int tQ = 1;
                int tC = 0;
                string tV = "";
                string tLine = tValueList.ElementAt(i);
                string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                if (tLineValue.Length == 2)
                {
                    tV = tLineValue[0];
                    tIndex = tReferenceList.IndexOf(tV);
                    int.TryParse(tLineValue[1], out tQ);
                }
                if (tLineValue.Length == 3)
                {
                    tV = tLineValue[0];
                    tIndex = tReferenceList.IndexOf(tV);
                    int.TryParse(tLineValue[1], out tQ);
                   // tC = (NWDConditional)Enum.Parse(typeof(NWDConditional), tLineValue[2], true);
                    int.TryParse(tLineValue[2], out tC);
                }

                tIndex = EditorGUI.Popup(new Rect(tX, tY, tWidth - tIntWidth - tConWidth - tEditWidth - NWDConstants.kFieldMarge*2, NWDConstants.kPopupdStyle.fixedHeight), tContent, tIndex, tContentFuturList.ToArray(), NWDConstants.kPopupdStyle);

                if (tValueListERROR.Contains(tV))
                {
                    GUI.Label(new Rect(tX + EditorGUIUtility.labelWidth + NWDConstants.kFieldMarge, tY + 1, tWidth - tIntWidth - EditorGUIUtility.labelWidth - NWDConstants.kFieldMarge * 4 - tEditWidth, NWDConstants.kGrayLabelStyle.fixedHeight), "? <" + tV + ">", NWDConstants.kGrayLabelStyle);
                }

                if (tIndex > 0)
                {
                    //tC = (NWDConditional)EditorGUI.EnumPopup(new Rect(tX + tWidth - tIntWidth * 2 - tEditWidth - NWDConstants.kFieldMarge * 4, tY, tIntWidth + NWDConstants.kFieldMarge,NWDConstants.kTextFieldStyle.fixedHeight),
                                                             //tC);
                    tC = EditorGUI.Popup(new Rect(tX + tWidth - tIntWidth - tConWidth - tEditWidth - NWDConstants.kFieldMarge * 4, tY, tConWidth + NWDConstants.kFieldMarge, NWDConstants.kTextFieldStyle.fixedHeight),
                                         tC, new string[] { "=", ">", ">=", "<", "=<", "!" });

                    //remove EditorGUI.indentLevel to draw next controller without indent 
                    int tIndentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;
                    tQ = EditorGUI.IntField(new Rect(tX + tWidth - tIntWidth - tEditWidth - NWDConstants.kFieldMarge * 2, tY, tIntWidth + NWDConstants.kFieldMarge, NWDConstants.kTextFieldStyle.fixedHeight), tQ);
                    
                    EditorGUI.indentLevel = tIndentLevel;
                    //if (GUI.Button(new Rect(tX + tWidth - tEditWidth, tY, tEditWidth, tPopupdStyle.fixedHeight), "!"))
                    GUIContent tDeleteContent = new GUIContent(NWDConstants.kImageTabReduce, "edit");
                    if (GUI.Button(new Rect(tX + tWidth - tEditWidth, tY, tEditWidth, NWDConstants.kPopupButtonStyle.fixedHeight), tDeleteContent, NWDConstants.kPopupButtonStyle))
                    {
                        NWDBasis<K>.SetObjectInEdition(NWDBasis<K>.InstanceByReference(tReferenceList.ElementAt(tIndex)), false);
                    }
                }
                else
                {
                    GUIContent tNewContent = new GUIContent(NWDConstants.kImageNew, "new");
                    if (GUI.Button(new Rect(tX + tWidth - tEditWidth, tY, tEditWidth, NWDConstants.kPopupButtonStyle.fixedHeight), tNewContent, NWDConstants.kPopupButtonStyle))
                    {
                        NWDBasis<K> tNewObject = NWDBasis<K>.NewObject();
                        tNewReferenceQuantity = NWDConstants.kFieldSeparatorA+ tNewObject.Reference + NWDConstants.kFieldSeparatorB + "1"+ tNewObject.Reference + NWDConstants.kFieldSeparatorB + "0";
                        NWDBasis<K>.SetObjectInEdition(tNewObject, false, true);
                    }
                }

                tY += NWDConstants.kPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;

                if (tIndex > 0 && tIndex < tReferenceList.Count)
                {
                    //if (tQ != 0) {
                    // no dupplicate reference
                   // if (!tValueListReferenceAllReady.Contains(tReferenceList.ElementAt(tIndex)))
                   // {
                        tValueList[i] = tReferenceList.ElementAt(tIndex) + NWDConstants.kFieldSeparatorB + tQ.ToString()+ NWDConstants.kFieldSeparatorB + tC.ToString();
                   //     tValueListReferenceAllReady.Add(tReferenceList.ElementAt(tIndex));
                   // }
                }
                else
                {
                    tValueList[i] = "";
                }
            }
            string[] tNextValueArray = tValueList.Distinct().ToArray();
            string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray) + tNewReferenceQuantity;
            tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
            tTemporary.Value = tNextValue;


            EditorGUI.EndDisabledGroup();

            if (tConnection == false)
            {
                tTemporary.Value = Value;

                GUI.Label(new Rect(tX + EditorGUIUtility.labelWidth, tY, tWidth, NWDConstants.kRedLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_LIST_ERROR, NWDConstants.kRedLabelStyle);
                tY = tY + NWDConstants.kFieldMarge + NWDConstants.kRedLabelStyle.fixedHeight;
                //				GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelAssetStyle.fixedHeight), Value.Replace (NWDAssetType.kAssetDelimiter, ""),tLabelAssetStyle);
                //				tY = tY + NWDConstants.kFieldMarge + tLabelAssetStyle.fixedHeight;
                Color tOldColor = GUI.backgroundColor;
                GUI.backgroundColor = NWDConstants.K_RED_BUTTON_COLOR;
                if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth, tY, 60.0F, NWDConstants.kDeleteButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_CLEAN, NWDConstants.kDeleteButtonStyle))
                {
                    List<NWDReferenceQuantityConditional<K>> tListToNanlyze = GetReferenceQuantityConditional();
                    List<NWDReferenceQuantityConditional<K>> tListCleaned = new List<NWDReferenceQuantityConditional<K>>();
                    foreach (NWDReferenceQuantityConditional < K > tObject in tListToNanlyze)
                    {
                        bool tInclude = true;
                        foreach (string tDeleteReference in tValueListERROR)
                        {
                            if (tObject.Reference == tDeleteReference)
                            {
                                tInclude = false;
                                break;
                            }
                        }
                        if (tInclude == true)
                        {
                            tListCleaned.Add(tObject);
                        }
                    }
                    tTemporary.SetReferenceQuantityConditional(tListCleaned);
                }
                GUI.backgroundColor = tOldColor;
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
                    Debug.Log("I CHANGE " + sOldReference + " FOR " + sNewReference + "");
                    Value = Value.Replace(sOldReference, sNewReference);
                    rReturn = "YES";
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================