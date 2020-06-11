//=====================================================================================================================
//
//  ideMobi 2020©
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



//using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditorInternal;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDReference : NWEDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public virtual void ChangeReferenceForAnother(string sOldReference, string sNewReference) // TODO rename Change Reference
        {
            if (string.IsNullOrEmpty(Value) == false)
            {
                if (Value == sOldReference)
                {
                    Value = sNewReference;
                }
            }
            if (string.IsNullOrEmpty(Value) == true)
            {
                Value = string.Empty;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual bool ContainsReference(string sReference)
        {
            if (string.IsNullOrEmpty(sReference) == true)
            {
                return false;
            }
            return Value == sReference;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsEmpty()
        {
            bool rReturn = string.IsNullOrEmpty(Value);
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public bool IsNotEmpty()
        {
            return !IsEmpty();
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public virtual object[] GetEditorDatas()
        {
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual object GetEditorData()
        {
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            return NWDGUI.kDataSelectorFieldStyle.fixedHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            return this;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool ErrorAnalyze()
        {
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDReferenceSimple : NWDReference
    {
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        public override float ControlFieldHeight()
        {
            return NWDGUI.kDataSelectorFieldStyle.fixedHeight;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void NodalAnalyze(NWDNodeCard sCard)
        {
            //TODO : create analyze for nodal view and relative position of field/reference
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void CreatePloters(NWDNodeCard sNodalCard, float tHeight)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void CreatePlotersInvisible(NWDNodeCard sNodalCard, float tHeight)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDReferenceMultiple : NWDReference
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void ChangeReferenceForAnother(string sOldReference, string sNewReference) // TODO rename Change Reference
        {
            if (string.IsNullOrEmpty(Value) == false)
            {
                if (string.IsNullOrEmpty(sOldReference) == false)
                {
                    if (Value.Contains(sOldReference))
                    {
                        //Value = Value.Replace(sOldReference, sNewReference);
                        string tValue = NWDConstants.kFieldSeparatorA + Value + NWDConstants.kFieldSeparatorA;
                        tValue = tValue.Replace(NWDConstants.kFieldSeparatorA + sOldReference + NWDConstants.kFieldSeparatorA, NWDConstants.kFieldSeparatorA + sNewReference + NWDConstants.kFieldSeparatorA);
                        tValue = tValue.Replace(NWDConstants.kFieldSeparatorA + sOldReference + NWDConstants.kFieldSeparatorB, NWDConstants.kFieldSeparatorA + sNewReference + NWDConstants.kFieldSeparatorB);
                        tValue = tValue.TrimEnd(new char[] { NWDConstants.kFieldSeparatorA_char });
                        tValue = tValue.TrimStart(new char[] { NWDConstants.kFieldSeparatorA_char });
                        Value = tValue;
                    }
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool ContainsReference(string sReference)
        {
            if (string.IsNullOrEmpty(sReference) == true)
            {
                return false;
            }
            string tValueA = NWDConstants.kFieldSeparatorA + Value + NWDConstants.kFieldSeparatorA;
            if (tValueA.Contains(NWDConstants.kFieldSeparatorA + sReference + NWDConstants.kFieldSeparatorA))
            {
                return true;
            }
            string tValueB = NWDConstants.kFieldSeparatorA + Value + NWDConstants.kFieldSeparatorB;
            if (tValueB.Contains(NWDConstants.kFieldSeparatorA + sReference + NWDConstants.kFieldSeparatorB))
            {
                return true;
            }
            return false;
        }
        //-------------------------------------------------------------------------------------------------------------
        public int Count()
        {
            int tReturn = 0;
            if (Value != null && Value != string.Empty)
            {
                string[] tValueArray = Value.Split(new string[] { NWDConstants.kFieldSeparatorA }, StringSplitOptions.RemoveEmptyEntries);
                tReturn = tValueArray.Count();
            }
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
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
        public override object GetEditorData()
        {
            Debug.LogWarning("GetEditorData not available for Multiple Datas");
            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void CreatePloters(NWDNodeCard sNodalCard, float tHeight)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public virtual void CreatePlotersInvisible(NWDNodeCard sNodalCard, float tHeight)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================