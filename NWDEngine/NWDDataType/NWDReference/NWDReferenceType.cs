//=====================================================================================================================
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
//=====================================================================================================================

//=====================================================================================================================
//
//  ideMobi copyright 2019
//  All rights reserved by ideMobi
//
//  Author Kortex (Jean-François CONTART) jfcontart@idemobi.com
//  Date 2019 4 12 18:2:36
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
    /// NWDReferenceType used to put a reference in value. Use properties with simple name, like 'Account', 'Spot', 'Bonus' , etc.
    /// </summary>
    [SerializeField]
    public class NWDReferenceType<K> : NWDReferenceSimple where K : NWDBasis, new()
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
        public bool ContainsData(K sData)
        {
            if (sData == null)
            {
                return false;
            }
            return Value.Contains(sData.Reference);
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
        public K GetData(string sAccountReference = null)
        {
            return NWDBasisHelper.GetCorporateDataByReference<K>(Value, sAccountReference) as K;
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] GetDatas(string sAccountReference = null)
        {
            K tObject = NWDBasisHelper.GetCorporateDataByReference<K>(Value, sAccountReference) as K;
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
        public K GetRawData()
        {
            return NWDBasisHelper.GetRawDataByReference<K>(Value) as K;
        }
        //-------------------------------------------------------------------------------------------------------------
        public K[] GetRawDatas()
        {
            K tObject = NWDBasisHelper.GetRawDataByReference<K>(Value) as K;
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
        public void SetData(K sObject)
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
                if (NWDBasisHelper.GetRawDataByReference<K>(tReference) == null)
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
                if (NWDBasisHelper.GetRawDataByReference<K>(Value) == null)
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
            if (string.IsNullOrEmpty(Value) == false)
            {
                K tObj = NWDBasisHelper.GetRawDataByReference<K>(Value) as K;
                //if (tObj != null)
                {
                    rReturn.Add(tObj);
                }
            }
            return rReturn.ToArray();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object GetEditorData()
        {
            K rReturn = null;
            if (string.IsNullOrEmpty(Value) == false)
            {
                K tObj = NWDBasisHelper.GetRawDataByReference<K>(Value) as K;
                //if (tObj != null)
                {
                    rReturn = tObj;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override object ControlField(Rect sPosition, string sEntitled, bool sDisabled, string sTooltips = NWEConstants.K_EMPTY_STRING, object sAdditionnal = null)
        {
            NWDReferenceType<K> tTemporary = new NWDReferenceType<K>();
            GUIContent tContent = new GUIContent(sEntitled, sTooltips);
            tTemporary.Value = Value;
            float tWidth = sPosition.width;
            float tHeight = sPosition.height;
            float tX = sPosition.position.x;
            float tY = sPosition.position.y;
            tTemporary.Value = NWDDatasSelector.Field(NWDBasisHelper.FindTypeInfos(typeof(K)), new Rect(tX, tY, tWidth, NWDGUI.kDataSelectorFieldStyle.fixedHeight), tContent, tTemporary.Value, sDisabled);
            tY = tY + NWDGUI.kFieldMarge + NWDGUI.kDataSelectorFieldStyle.fixedHeight;
            return tTemporary;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void CreatePloters(NWDNodeCard sNodalCard, float tHeight)
        {
            sNodalCard.PloterList.Add(new NWDNodePloter(sNodalCard, GetReference(), 
                new Vector2(0, 
                tHeight 
                 + NWDGUI.kDataSelectorFieldStyle.fixedHeight*0.5F
                )));
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void CreatePlotersInvisible(NWDNodeCard sNodalCard, float tHeight)
        {
            sNodalCard.PloterList.Add(new NWDNodePloter(sNodalCard, GetReference(), new Vector2(0, 
                tHeight
                + NWDGUI.kFieldMarge+  NWDGUI.kBoldFoldoutStyle.fixedHeight*0.5F
                )));
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================