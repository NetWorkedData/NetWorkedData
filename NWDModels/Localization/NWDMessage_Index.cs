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
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDMessageIndexer : NWDIndexer<NWDMessage>
    {
        //-------------------------------------------------------------------------------------------------------------
        public override void IndexData(NWDTypeClass sData)
        {
            NWDMessage tData = (NWDMessage)sData;
            tData.InsertInCodeIndex();
            tData.InsertInDomainCodeIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void DeindexData(NWDTypeClass sData)
        {
            NWDMessage tData = (NWDMessage)sData;
            tData.RemoveFromCodeIndex();
            tData.RemoveFromDomainCodeIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDMessage : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndexSimple<NWDMessage> kCodeIndex = new NWDIndexSimple<NWDMessage>();
        //-------------------------------------------------------------------------------------------------------------
        public void InsertInCodeIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                kCodeIndex.InsertData(this, this.Code);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveFromCodeIndex()
        {
            // Remove from the actual indexation
            kCodeIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDMessage FindDataByCode(string sCode)
        {
            return kCodeIndex.RawFirstDataByKey(sCode);
        }
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndexSimple<NWDMessage> kDomainCodeIndex = new NWDIndexSimple<NWDMessage>();
        //-------------------------------------------------------------------------------------------------------------
        public void InsertInDomainCodeIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                kDomainCodeIndex.InsertData(this, this.Domain + NWDConstants.kFieldSeparatorA + this.Code);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveFromDomainCodeIndex()
        {
            // Remove from the actual indexation
            kDomainCodeIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDMessage FindDataByDomainAndCode(string sDomain, string sCode)
        {
            return kDomainCodeIndex.RawFirstDataByKey(sDomain + NWDConstants.kFieldSeparatorA + sCode);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================