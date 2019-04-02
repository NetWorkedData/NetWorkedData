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
    public class NWDReferenceSimple : BTBDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_ChangeReferenceForAnother)]
        public string ChangeReferenceForAnother(string sOldReference, string sNewReference)
        {
            string rReturn = "NO";
            if (Value != null)
            {
                if (Value.Contains(sOldReference))
                {
                    Value = Value.Replace(sOldReference, sNewReference);
                    rReturn = "YES";
                }
            }
            return rReturn;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDReferenceMultiple : BTBDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_ChangeReferenceForAnother)]
        public string ChangeReferenceForAnother(string sOldReference, string sNewReference)
        {
            string rReturn = "NO";
            if (Value != null)
            {
                if (Value.Contains(sOldReference))
                {
                    Value = Value.Replace(sOldReference, sNewReference);
                    rReturn = "YES";
                }
            }
            return rReturn;
        }
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDReferenceType used to put a reference in value. Use properties with simple name, like 'Account', 'Spot', 'Bonus' , etc.
    /// </summary>
    [SerializeField]
    public class NWDReferenceType<K> : NWDReferenceSimple where K : NWDBasis<K>, new()
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
        public override bool ErrorAnalyze()
        {
            bool rReturn = false;
            if (string.IsNullOrEmpty(Value) == false)
            {
                if (NWDBasis<K>.GetDataByReference(Value) == null)
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
            return NWDGUI.kDatasSelectorRowStyle.fixedHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING)
        {
            NWDReferenceType<K> tTemporary = new NWDReferenceType<K>();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            tTemporary.Value = Value;
            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;
            tTemporary.Value = NWDDatasSelector<K>.Field(new Rect(tX, tY, tWidth, NWDGUI.kDatasSelectorRowStyle.fixedHeight), tContent, tTemporary.Value);
            tY = tY + NWDGUI.kFieldMarge + NWDGUI.kDatasSelectorRowStyle.fixedHeight;
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
        //[NWDAliasMethod(NWDConstants.M_ChangeReferenceForAnother)]
        //public string ChangeReferenceForAnother(string sOldReference, string sNewReference)
        //{
        //    string rReturn = "NO";
        //    if (Value != null)
        //    {
        //        if (Value.Contains(sOldReference))
        //        {
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