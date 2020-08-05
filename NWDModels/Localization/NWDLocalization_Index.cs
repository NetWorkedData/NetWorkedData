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
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDLocalizationIndexer : NWDIndexer<NWDLocalization>
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndexSimple<NWDLocalization> kKeyIndex = new NWDIndexSimple<NWDLocalization>();
        //-------------------------------------------------------------------------------------------------------------
        public override void IndexData(NWDTypeClass sData)
        {
            NWDLocalization tData = (NWDLocalization)sData;
            if (tData.IsUsable() && string.IsNullOrEmpty(tData.KeyValue) == false)
            {
                kKeyIndex.InsertData(tData, tData.KeyValue);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeindexData(NWDTypeClass sData)
        {
            NWDLocalization tData = (NWDLocalization)sData;
            kKeyIndex.RemoveData(tData);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDLocalization FindFirstDataByKey(string sKeyValue)
        {
            return kKeyIndex.RawFirstDataByKey(sKeyValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDLocalization> RawDatasWithKey()
        {
            return kKeyIndex.RawDatas();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDLocalization : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDLocalization FindFirstDataByKey(string sKeyValue)
        {
            return NWDLocalizationIndexer.FindFirstDataByKey(sKeyValue);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static List<NWDLocalization> RawDatasWithKey()
        {
            return NWDLocalizationIndexer.RawDatasWithKey();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================