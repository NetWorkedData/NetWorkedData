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
    /// NWDReferenceType used to put a reference in value. Use properties with simple name, like 'Account', 'Spot', 'Bonus' , etc.
    /// </summary>
    [SerializeField]
    public class NWDReferenceType<K> : BTBDataType where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferenceType()
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
        public void SetReference(string sReference)
        {
            if (sReference == null)
            {
                sReference = string.Empty;
            }
            Value = sReference;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetReference()
        {
            if (Value == null)
            {
                return string.Empty;
            }
            return Value;
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
        public K GetObject(string sAccountReference = null)
        {
            return NWDBasis<K>.FindDataByReference(Value, sAccountReference) as K;
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] GetObjects(string sAccountReference = null)
        {
            K tObject = NWDBasis<K>.FindDataByReference(Value, sAccountReference) as K;
            if (tObject != null)
            {
                return new K[] {tObject};
            }
            else
            {
                return new K[] { };
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public K GetObjectAbsolute(string sAccountReference = null)
        {
            return NWDBasis<K>.GetDataByReference(Value) as K;
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] GetObjectsAbsolute(string sAccountReference = null)
        {
            K tObject = NWDBasis<K>.GetDataByReference(Value) as K;
            if (tObject != null)
            {
                return new K[] { tObject };
            }
            else
            {
                return new K[] { };
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetObject(K sObject)
        {
            if (sObject != null)
            {
                Value = sObject.Reference;
            }
            else
            {
                Value = string.Empty;
            }
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
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override bool IsInError()
        {
            bool rReturn = false;
            if (string.IsNullOrEmpty(Value) == false)
            {
                if (NWDBasis<K>.GetDataByReference(Value) == null)
                {
                    rReturn = true;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_EditorGetObjects)]
        public K[] EditorGetObjects()
        {
            List<K> rReturn = new List<K>();
            if (string.IsNullOrEmpty(Value) == false)
            {
                K tObj = NWDBasis<K>.GetDataByReference(Value) as K;
                //if (tObj != null)
                {
                    rReturn.Add(tObj);
                }
            }
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void EditorAddNewObject(NWDWritingMode sWritingMode = NWDWritingMode.ByDefaultLocal)
        {
            K tNewObject = NWDBasis<K>.NewData(sWritingMode);
            this.SetObject(tNewObject);
            NWDBasis<K>.SetObjectInEdition(tNewObject, false, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            //			Debug.Log ("Je suis dans l'invocation de hauteur");

            int tConnection = 0;
            if (Value != null && Value != string.Empty)
            {
                if (NWDBasis<K>.GetDataByReference(Value) == null)
                {
                    tConnection = 1;
                }
            }
            float tHeight = NWDConstants.kPopupdStyle.fixedHeight + tConnection * (NWDConstants.kRedLabelStyle.fixedHeight + NWDConstants.kFieldMarge +
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
            NWDReferenceType<K> tTemporary = new NWDReferenceType<K>();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            tTemporary.Value = Value;

            //Type sFromType = typeof(K);
            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;

            float tEditWidth = NWDConstants.kEditWidth;


            bool tConnection = true;
            if (Value != null && Value != string.Empty)
            {
                if (NWDBasis<K>.GetDataByReference(Value) == null)
                {
                    tConnection = false;
                }
            }

            EditorGUI.BeginDisabledGroup(!tConnection);
            List<string> tReferenceList = new List<string>();
            List<string> tInternalNameList = new List<string>();
            tReferenceList.Add(NWDConstants.kFieldSeparatorA);
            tInternalNameList.Add(NWDConstants.kFieldNone);

            foreach (KeyValuePair<string, string> tKeyValue in NWDBasisHelper.FindTypeInfos(typeof(K)).EditorDatasMenu.OrderBy(i=> i.Value))
            {
                tReferenceList.Add(tKeyValue.Key);
                tInternalNameList.Add(tKeyValue.Value);
            }


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

            int tIndex = tReferenceList.IndexOf(Value);
            int rIndex = EditorGUI.Popup(new Rect(tX, tY, tWidth - NWDConstants.kFieldMarge - tEditWidth, NWDConstants.kPopupdStyle.fixedHeight), tContent, tIndex, tContentFuturList.ToArray(), NWDConstants.kPopupdStyle);

            if (tConnection == false)
            {
                GUI.Label(new Rect(tX + EditorGUIUtility.labelWidth + NWDConstants.kFieldMarge, tY + 1, tWidth - EditorGUIUtility.labelWidth - NWDConstants.kFieldMarge * 4 - tEditWidth, NWDConstants.kGrayLabelStyle.fixedHeight), "? <" + Value + ">", NWDConstants.kGrayLabelStyle);

            }


            //GUIContent tChooseContent = new GUIContent(NWDConstants.kImageTabReduce, "choose");
            //if (GUI.Button(new Rect(tX + tEditWidth, tY, tEditWidth, NWDConstants.kPopupButtonStyle.fixedHeight), tChooseContent, NWDConstants.kPopupButtonStyle))
            //{
            //    string tTransitionReference =  NWDBasisChooserDialog.SelectObject(typeof(K), Value, null, NWDBasisChooserMode.Add, Value, 0);
            //    if (tTransitionReference!=null)
            //    {
            //        Debug.Log("TransitionReference is null");
            //    }
            //    else
            //    {
            //        Debug.Log("TransitionReference is " + tTransitionReference);
            //    }
            //}

            if (tIndex >= 0)
            {
                //if (GUI.Button(new Rect(tX + tWidth - tEditWidth, tY, tEditWidth, tPopupdStyle.fixedHeight), "!"))
                GUIContent tDeleteContent = new GUIContent(NWDConstants.kImageTabReduce, "edit");
                if (GUI.Button(new Rect(tX + tWidth - tEditWidth, tY, tEditWidth, NWDConstants.kPopupButtonStyle.fixedHeight), tDeleteContent, NWDConstants.kPopupButtonStyle))
                {
                    NWDBasis<K>.SetObjectInEdition(NWDBasis<K>.GetDataByReference(tReferenceList.ElementAt(rIndex)), false);
                }
            }
            else
            {
                GUIContent tNewContent = new GUIContent(NWDConstants.kImageNew, "New");
                if (GUI.Button(new Rect(tX + tWidth - tEditWidth, tY, tEditWidth, NWDConstants.kPopupButtonStyle.fixedHeight), tNewContent, NWDConstants.kPopupButtonStyle))
                {
                    NWDBasis<K> tNewObject = NWDBasis<K>.NewData();
                    tTemporary.Value = tNewObject.Reference;
                    NWDBasis<K>.SetObjectInEdition(tNewObject, false, true);
                }
            }

            if (rIndex != tIndex)
            {
                string tNextValue = tReferenceList.ElementAt(rIndex);
                tNextValue = tNextValue.Trim(NWDConstants.kFieldSeparatorA.ToCharArray()[0]);
                tTemporary.Value = tNextValue;
            }

            tY = tY + NWDConstants.kFieldMarge + NWDConstants.kPopupdStyle.fixedHeight;
            EditorGUI.EndDisabledGroup();

            if (tConnection == false)
            {
                tTemporary.Value = Value;

                GUI.Label(new Rect(tX + EditorGUIUtility.labelWidth, tY, tWidth, NWDConstants.kRedLabelStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_ERROR, NWDConstants.kRedLabelStyle);
                tY = tY + NWDConstants.kFieldMarge + NWDConstants.kRedLabelStyle.fixedHeight;
                //				GUI.Label (new Rect (tX + EditorGUIUtility.labelWidth, tY, tWidth, tLabelAssetStyle.fixedHeight), Value.Replace (NWDAssetType.kAssetDelimiter, ""),tLabelAssetStyle);
                //				tY = tY + NWDConstants.kFieldMarge + tLabelAssetStyle.fixedHeight;
                NWDConstants.GUIRedButtonBegin();
                if (GUI.Button(new Rect(tX + EditorGUIUtility.labelWidth, tY, 60.0F, NWDConstants.kDeleteButtonStyle.fixedHeight), NWDConstants.K_APP_BASIS_REFERENCE_CLEAN, NWDConstants.kDeleteButtonStyle))
                {
                    tTemporary.Value = string.Empty;
                }
                NWDConstants.GUIRedButtonEnd();
                tY = tY + NWDConstants.kFieldMarge + NWDConstants.kMiniButtonStyle.fixedHeight;
            }
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_ChangeReferenceForAnother)]
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