﻿// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:28:19
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
    /// NWDReferenceHashType used to put a reference in value. Use properties with simple name, like 'Account', 'Spot', 'Bonus' , etc.
    /// </summary>
    [SerializeField]
    public class NWDReferenceHashType<K> : NWDReferenceSimple where K : NWDBasis<K>, new()
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferenceHashType()
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
            return NWDBasis<K>.GetCorporateDataByReference(Value, sAccountReference) as K;
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] GetObjects(string sAccountReference = null)
        {
            K tObject = NWDBasis<K>.GetCorporateDataByReference(Value, sAccountReference) as K;
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
        public K GetObjectAbsolute(string sAccountReference = null)
        {
            return NWDBasis<K>.RawDataByReference(Value) as K;
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] GetObjectsAbsolute(string sAccountReference = null)
        {
            K tObject = NWDBasis<K>.RawDataByReference(Value) as K;
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
                if (NWDBasis<K>.RawDataByReference(tReference) == null)
                {
                    rReturn.Add(tReference);
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override bool ErrorAnalyze()
        {
            bool rReturn = false;
            if (string.IsNullOrEmpty(Value) == false)
            {
                if (NWDBasis<K>.RawDataByReference(Value) == null)
                {
                    rReturn = true;
                }
            }
            InError = rReturn;
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_EditorGetObjects)]
        public override object[] EditorGetDatas()
        {
            List<K> rReturn = new List<K>();
            if (string.IsNullOrEmpty(Value) == false)
            {
                K tObj = NWDBasis<K>.RawDataByReference(Value) as K;
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
            NWDBasis<K>.BasisHelper().New_SetObjectInEdition(tNewObject, false, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            return NWDGUI.kDataSelectorFieldStyle.fixedHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDReferenceHashType<K> tTemporary = new NWDReferenceHashType<K>();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            tTemporary.Value = Value;
            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;
            tTemporary.Value =BTBSecurityTools.GenerateSha( NWDDatasSelector.Field(NWDBasisHelper.FindTypeInfos(typeof(K)), new Rect(tX, tY, tWidth, NWDGUI.kDataSelectorFieldStyle.fixedHeight), tContent, tTemporary.Value));
            tY = tY + NWDGUI.kFieldMarge + NWDGUI.kDataSelectorFieldStyle.fixedHeight;
            return tTemporary;
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