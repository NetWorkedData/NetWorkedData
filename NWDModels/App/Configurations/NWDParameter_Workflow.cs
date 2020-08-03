//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#endif
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