﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:43
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDParameter : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDParameter()
        {
            //Debug.Log("NWDParameter Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDParameter(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDParameter Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString()+"");
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDParameter GetRawByReferenceOrCreate (string sReference, string sInternalKey, NWDMultiType sDefaultValue)
        {
            string tReferenceKey = NWDBasisHelper.BasisHelper<NWDParameter>().ClassTrigramme + "-" + sReference + "-999";
            NWDParameter rReturn = NWDBasisHelper.GetRawDataByReference<NWDParameter>(tReferenceKey);
            if (rReturn ==null)
            {
                rReturn = NWDBasisHelper.NewDataWithReference<NWDParameter>(tReferenceKey);
                rReturn.InternalKey = sInternalKey;
                rReturn.Value = sDefaultValue;
                rReturn.SaveData();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================