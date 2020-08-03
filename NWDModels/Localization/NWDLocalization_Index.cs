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
    public partial class NWDLocalization : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndexSimple<NWDLocalization> kKeyIndex = new NWDIndexSimple<NWDLocalization>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInMemory]
        public void InsertInKeyIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable() && string.IsNullOrEmpty(KeyValue) == false)
            {
                // Re-add !
                kKeyIndex.InsertData(this, this.KeyValue);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDDeindexInMemory]
        public void RemoveFromKeyIndex()
        {
            // Remove from the actual indexation
            kKeyIndex.RemoveData(this);
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
    //-------------------------------------------------------------------------------------------------------------
}
//=====================================================================================================================