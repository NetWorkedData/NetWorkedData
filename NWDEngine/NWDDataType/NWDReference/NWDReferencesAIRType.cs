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
    public class NWDAIRverage
    {
        //-------------------------------------------------------------------------------------------------------------
        public float Intolerance;
        public float Indifference;
        public float Friendship;
        public float Affinity;
        public float Admiration;
        public float Jealousy;
        //-------------------------------------------------------------------------------------------------------------
        public NWDAIRverage()
        {
            Intolerance = 0.0F;
            Indifference = 0.0F;
            Friendship = 0.0F;
            Affinity = 0.0F;
            Admiration = 0.0F;
            Jealousy = 0.0F;
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAIRverage(string sString)
        {
            string[] tValueArray = sString.Split(new string[] { NWDConstants.kFieldSeparatorC }, StringSplitOptions.RemoveEmptyEntries);
            if (tValueArray.Length > 0)
            {
                float.TryParse(tValueArray[0], out Intolerance);
            }
            if (tValueArray.Length > 1)
            {
                float.TryParse(tValueArray[1], out Indifference);
            }
            if (tValueArray.Length > 2)
            {
                float.TryParse(tValueArray[2], out Friendship);
            }
            if (tValueArray.Length > 3)
            {
                float.TryParse(tValueArray[3], out Affinity);
            }
            if (tValueArray.Length > 4)
            {
                float.TryParse(tValueArray[4], out Admiration);
            }
            if (tValueArray.Length > 5)
            {
                float.TryParse(tValueArray[5], out Jealousy);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        new public string ToString()
        {
            return Intolerance.ToString("F5") + NWDConstants.kFieldSeparatorC +
                   Indifference.ToString("F5") + NWDConstants.kFieldSeparatorC +
                   Friendship.ToString("F5") + NWDConstants.kFieldSeparatorC +
                   Affinity.ToString("F5") + NWDConstants.kFieldSeparatorC +
                   Admiration.ToString("F5") + NWDConstants.kFieldSeparatorC +
                   Jealousy.ToString("F5");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [SerializeField]
    public class NWDReferencesAIRType<K> : BTBDataType where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesAIRType()
        {
            Value = "";
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Default()
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
        //public bool ContainedIn(NWDReferencesAIRType<K> sReferencesProportion, bool sExceptIfIsEmpty = true)
        //{
        //    bool rReturn = true;
        //    if (sExceptIfIsEmpty && Value == "")
        //    {
        //        return false;
        //    }
        //    // I compare all elemnt
        //    Dictionary<string, NWDAIRverage> tThis = GetReferenceAndProportion();
        //    Dictionary<string, NWDAIRverage> tOther = sReferencesProportion.GetReferenceAndProportion();

        //    foreach (KeyValuePair<string, NWDAIRverage> tKeyValue in tThis)
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
        //public bool ContainsReferencesProportion(NWDReferencesAIRType<K> sReferencesProportion)
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
        //public bool RemoveReferencesQuantity(NWDReferencesAIRType<K> sReferencesProportion, bool sCanBeNegative = true, bool sRemoveEmpty = true)
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
        //public void AddReferencesQuantity(NWDReferencesAIRType<K> sReferencesProportion)
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
        public void AddObjectValue(NWDBasis<K> sObject)
        {
            // I compare all element
            Dictionary<string, NWDAIRverage> tThis = GetReferenceAndAverage();
            if (tThis.ContainsKey(sObject.Reference) == false)
            {
                tThis.Add(sObject.Reference, new NWDAIRverage());
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
                        float tCount = 0;
                        float.TryParse(tLineValue[1], out tCount);
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
        public void SetReferenceAndAverage(Dictionary<string, NWDAIRverage> sDico)
        {
            List<string> tValueList = new List<string>();
            foreach (KeyValuePair<string, NWDAIRverage> tKeyValue in sDico)
            {
                tValueList.Add(tKeyValue.Key + NWDConstants.kFieldSeparatorB + tKeyValue.Value.ToString());
            }
            string[] tNextValueArray = tValueList.Distinct().ToArray();
            string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
            tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
            Value = tNextValue;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<string, NWDAIRverage> GetReferenceAndAverage()
        {
            Dictionary<string, NWDAIRverage> tValueDico = new Dictionary<string, NWDAIRverage>();
            if (Value != null && Value != "")
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        NWDAIRverage tQ = new NWDAIRverage(tLineValue[1]);
                        tValueDico.Add(tLineValue[0], tQ);
                    }
                }
            }
            return tValueDico;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Dictionary<K, NWDAIRverage> GetObjectAndAverage(string sAccountReference = null)
        {
            Dictionary<K, NWDAIRverage> tValueDico = new Dictionary<K, NWDAIRverage>();
            if (Value != null && Value != "")
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        NWDAIRverage tQ = new NWDAIRverage(tLineValue[1]);
                        K tObject = NWDBasis<K>.GetObjectByReference(tLineValue[0], sAccountReference) as K;
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
        public Dictionary<K, NWDAIRverage> GetObjectAndAverageAbsolute()
        {
            Dictionary<K, NWDAIRverage> tValueDico = new Dictionary<K, NWDAIRverage>();
            if (Value != null && Value != "")
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string tLine in tValueArray)
                {
                    string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                    if (tLineValue.Length == 2)
                    {
                        NWDAIRverage tQ = new NWDAIRverage(tLineValue[1]);
                        K tObject = NWDBasis<K>.GetObjectAbsoluteByReference(tLineValue[0]) as K;
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
        public string Description()
        {
            string rDescription = "";
            Dictionary<string, NWDAIRverage> tDescDico = GetReferenceAndAverage();
            foreach (KeyValuePair<string, NWDAIRverage> tKeyValue in tDescDico)
            {
                K tObject = NWDBasis<K>.GetObjectByReference(tKeyValue.Key);
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
        public void EditorAddNewObject()
        {
            K tNewObject = NWDBasis<K>.NewObject();
            this.AddObjectValue(tNewObject);
            NWDBasis<K>.SetObjectInEdition(tNewObject, false, true);
        }
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
            float tHeight = (NWDConstants.kTextFieldStyle.fixedHeight + NWDConstants.kFieldMarge) *7* tRow - NWDConstants.kFieldMarge +
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
            NWDReferencesAIRType<K> tTemporary = new NWDReferencesAIRType<K>();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            tTemporary.Value = Value;
            Type sFromType = typeof(K);
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
                NWDAIRverage tQ = new NWDAIRverage("");
                string tV = "";
                string tLine = tValueList.ElementAt(i);
                string[] tLineValue = tLine.Split(new string[] { NWDConstants.kFieldSeparatorB }, StringSplitOptions.RemoveEmptyEntries);
                if (tLineValue.Length == 2)
                {
                    tV = tLineValue[0];
                    tQ = new NWDAIRverage(tLineValue[1]);
                    tIndex = tReferenceList.IndexOf(tV);
                }

                tIndex = EditorGUI.Popup(new Rect(tX, tY, tWidth - tEditWidth - NWDConstants.kFieldMarge , NWDConstants.kPopupdStyle.fixedHeight), tContent, tIndex, tContentFuturList.ToArray(), NWDConstants.kPopupdStyle);

                if (tValueListERROR.Contains(tV))
                {
                    GUI.Label(new Rect(tX + EditorGUIUtility.labelWidth + NWDConstants.kFieldMarge, tY + 1, tWidth - tIntWidth - EditorGUIUtility.labelWidth - NWDConstants.kFieldMarge * 4 - tEditWidth, NWDConstants.kGrayLabelStyle.fixedHeight), "? <" + tV + ">", NWDConstants.kGrayLabelStyle);
                }

                if (tIndex > 0)
                {
                    float tYb = tY +NWDConstants.kPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                    float tXb = sPosition.x + EditorGUIUtility.labelWidth;
                    float tTiersWidth = Mathf.Ceil((sPosition.width - EditorGUIUtility.labelWidth + NWDConstants.kFieldMarge) / 1.0F);
                    float tTiersWidthB = tTiersWidth - NWDConstants.kFieldMarge;

                    //remove EditorGUI.indentLevel to draw next controller without indent 
                    int tIndentLevel = EditorGUI.indentLevel;
                    EditorGUI.indentLevel = 0;

                    //EditorGUI.LabelField(new Rect(tX , tYb, EditorGUIUtility.labelWidth-NWDConstants.kFieldMarge, NWDConstants.kLabelRightStyle.fixedHeight), "AIR",NWDConstants.kLabelRightStyle);

        //            public float Intolerance;
        //public float Indifference;
        //public float Friendship;
        //public float Affinity;
        //public float Admiration;
        //public float Jealousy;
                    EditorGUI.LabelField(new Rect(tX , tYb, EditorGUIUtility.labelWidth-NWDConstants.kFieldMarge, NWDConstants.kLabelRightStyle.fixedHeight), "Intolerance",NWDConstants.kLabelRightStyle);
                    tQ.Intolerance = EditorGUI.FloatField(new Rect(tXb, tYb, tTiersWidthB, NWDConstants.kTextFieldStyle.fixedHeight), tQ.Intolerance);
                    tYb += NWDConstants.kPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;

                    EditorGUI.LabelField(new Rect(tX, tYb, EditorGUIUtility.labelWidth - NWDConstants.kFieldMarge, NWDConstants.kLabelRightStyle.fixedHeight), "Indifference", NWDConstants.kLabelRightStyle);
                    tQ.Indifference = EditorGUI.FloatField(new Rect(tXb, tYb, tTiersWidthB, NWDConstants.kTextFieldStyle.fixedHeight), tQ.Indifference);
                    tYb += NWDConstants.kPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;

                    EditorGUI.LabelField(new Rect(tX, tYb, EditorGUIUtility.labelWidth - NWDConstants.kFieldMarge, NWDConstants.kLabelRightStyle.fixedHeight), "Friendship", NWDConstants.kLabelRightStyle);
                    tQ.Friendship = EditorGUI.FloatField(new Rect(tXb, tYb, tTiersWidthB, NWDConstants.kTextFieldStyle.fixedHeight), tQ.Friendship);
                    tYb += NWDConstants.kPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;

                    EditorGUI.LabelField(new Rect(tX, tYb, EditorGUIUtility.labelWidth - NWDConstants.kFieldMarge, NWDConstants.kLabelRightStyle.fixedHeight), "Affinity", NWDConstants.kLabelRightStyle);
                    tQ.Affinity = EditorGUI.FloatField(new Rect(tXb, tYb, tTiersWidthB, NWDConstants.kTextFieldStyle.fixedHeight), tQ.Affinity);
                    tYb += NWDConstants.kPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;

                    EditorGUI.LabelField(new Rect(tX, tYb, EditorGUIUtility.labelWidth - NWDConstants.kFieldMarge, NWDConstants.kLabelRightStyle.fixedHeight), "Admiration", NWDConstants.kLabelRightStyle);
                    tQ.Admiration = EditorGUI.FloatField(new Rect(tXb, tYb, tTiersWidthB, NWDConstants.kTextFieldStyle.fixedHeight), tQ.Admiration);
                    tYb += NWDConstants.kPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;

                    EditorGUI.LabelField(new Rect(tX, tYb, EditorGUIUtility.labelWidth - NWDConstants.kFieldMarge, NWDConstants.kLabelRightStyle.fixedHeight), "Jealousy", NWDConstants.kLabelRightStyle);
                    tQ.Jealousy = EditorGUI.FloatField(new Rect(tXb, tYb, tTiersWidthB, NWDConstants.kTextFieldStyle.fixedHeight), tQ.Jealousy);
                    tYb += NWDConstants.kPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;




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
                        tNewReferenceQuantity = NWDConstants.kFieldSeparatorA + tNewObject.Reference + NWDConstants.kFieldSeparatorB + "1";
                        NWDBasis<K>.SetObjectInEdition(tNewObject, false, true);
                    }
                }

                tY += (NWDConstants.kPopupdStyle.fixedHeight + NWDConstants.kFieldMarge)*6;
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
                    Dictionary<string, NWDAIRverage> tDicoClean = GetReferenceAndAverage();
                    foreach (string tDeleteReference in tValueListERROR)
                    {
                        tDicoClean.Remove(tDeleteReference);
                    }
                    tTemporary.SetReferenceAndAverage(tDicoClean);
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