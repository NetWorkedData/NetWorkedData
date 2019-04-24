// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:28:31
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

// =====================================================================================================================
//
//  ideMobi copyright 2019
//  All rights reserved by ideMobi
//
//  Author Kortex (Jean-François CONTART) jfcontart@idemobi.com
//  Date 2019 4 12 18:2:36
//
// =====================================================================================================================

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
    public class NWDReference : BTBDataType
    {
        //-------------------------------------------------------------------------------------------------------------
        public void ChangeReferenceForAnother(string sOldReference, string sNewReference) // TODO rename Change Reference
        {
            if (Value != null)
            {
                if (Value.Contains(sOldReference))
                {
                    Value = Value.Replace(sOldReference, sNewReference);
                }
            }
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
        public override object ControlField(Rect sPosition, string sEntitled, string sTooltips = BTBConstants.K_EMPTY_STRING, object sAdditionnal = null)
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