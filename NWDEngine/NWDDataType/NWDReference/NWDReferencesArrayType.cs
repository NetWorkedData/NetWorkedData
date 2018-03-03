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
    [SerializeField]
    //-------------------------------------------------------------------------------------------------------------
    public class NWDReferencesArrayType<K> : BTBDataType where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesArrayType()
        {
            Value = "";
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesArrayType(string[] sReferences)
        {
            Value = "";
            this.SetReferences(sReferences);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesArrayType(K[] sObjects)
        {
            Value = "";
            this.SetObjects(sObjects);
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferencesArrayType(NWDReferencesArrayType<K> sArray)
        {
            Value = "" + sArray.Value;
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
            string[] tNextValueArray = tList.ToArray();
            Value = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddReferences(string[] sReferences)
        {
            List<string> tList = new List<string>();
            if (Value != null && Value != "")
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
            if (Value != null && Value != "")
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
        public bool ContainsObject(K sObject)
        {
            if (sObject == null)
            {
                return false;
            }
            return Value.Contains(sObject.Reference);
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] GetObjects()
        {
            List<K> tList = new List<K>();
            string[] tArray = GetReferences();
            foreach (string tRef in tArray)
            {
                K tObject = NWDBasis<K>.GetObjectByReference(tRef) as K;
                if (tObject != null)
                {
                    tList.Add(tObject);
                }
            }
            return tList.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public List<K> GetObjectsList()
        {
            List<K> tList = new List<K>();
            string[] tArray = GetReferences();
            foreach (string tRef in tArray)
            {
                K tObject = NWDBasis<K>.GetObjectByReference(tRef) as K;
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
        public K[] EditorGetObjects()
        {
            List<K> rReturn = new List<K>();
            foreach (string tReference in GetReferences())
            {
                K tObj = NWDBasis<K>.InstanceByReference(tReference) as K;
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
            K tNewObject = NWDBasis<K>.NewObject();
            this.AddObject(tNewObject);
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
                List<string> tValueListERROR = ReferenceInError(new List<string>(tValueArray));
                if (tValueListERROR.Count > 0)
                {
                    tConnection = 1;
                }
            }

            float tHeight = (NWDConstants.kPopupdStyle.fixedHeight + NWDConstants.kFieldMarge) * tRow - NWDConstants.kFieldMarge +
                             tConnection * (NWDConstants.kRedLabelStyle.fixedHeight + NWDConstants.kFieldMarge +
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
            NWDReferencesArrayType<K> tTemporary = new NWDReferencesArrayType<K>();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            tTemporary.Value = Value;
            Type sFromType = typeof(K);

            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;
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
            if (Value != null && Value != "")
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                tValueList = new List<string>(tValueArray);

            }

            List<string> tValueListERROR = ReferenceInError(tValueList);
            if (tValueListERROR.Count > 0)
            {
                tConnection = false;
            }

            EditorGUI.BeginDisabledGroup(!tConnection);

            tValueList.Add("");
            string tNewReference = "";
            for (int i = 0; i < tValueList.Count; i++)
            {
                string tFieldName = sEntitled;
                if (i > 0)
                {
                    tFieldName = "   ";
                    tContent = new GUIContent("   ");
                }
                string tV = tValueList.ElementAt(i);
                int tIndex = tReferenceList.IndexOf(tV);
                tIndex = EditorGUI.Popup(new Rect(tX, tY, tWidth - NWDConstants.kFieldMarge - tEditWidth, NWDConstants.kPopupdStyle.fixedHeight), tContent, tIndex, tContentFuturList.ToArray(), NWDConstants.kPopupdStyle);

                if (tValueListERROR.Contains(tV))
                {
                    GUI.Label(new Rect(tX + EditorGUIUtility.labelWidth + NWDConstants.kFieldMarge, tY + 1, tWidth - EditorGUIUtility.labelWidth - NWDConstants.kFieldMarge * 4 - tEditWidth, NWDConstants.kGrayLabelStyle.fixedHeight), "? <" + tV + ">", NWDConstants.kGrayLabelStyle);
                }

                if (tIndex >= 0)
                {//if (GUI.Button(new Rect(tX + tWidth - tEditWidth, tY, tEditWidth, tPopupdStyle.fixedHeight), "!"))
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
                        tNewReference = tNewObject.Reference + NWDConstants.kFieldSeparatorA;
                        NWDBasis<K>.SetObjectInEdition(tNewObject, false, true);
                    }
                }
                tY += NWDConstants.kPopupdStyle.fixedHeight + NWDConstants.kFieldMarge;
                if (tIndex >= 0 && tIndex < tReferenceList.Count)
                {
                    tValueList[i] = tReferenceList.ElementAt(tIndex);
                }
                else
                {
                    tValueList[i] = "";
                }
            }
            string[] tNextValueArray = tValueList.ToArray();
            string tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray)+tNewReference;
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
                    foreach (string tDeleteReference in tValueListERROR)
                    {
                        tValueList.Remove(tDeleteReference);
                    }
                    tNextValueArray = tValueList.ToArray();
                    tNextValue = string.Join(NWDConstants.kFieldSeparatorA, tNextValueArray);
                    tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
                    tTemporary.Value = tNextValue;
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
}
//=====================================================================================================================