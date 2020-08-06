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
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
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
    public partial class NWDError : NWDBasis
    {
        /*
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndexSimple<NWDError> kCodeIndex = new NWDIndexSimple<NWDError>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
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
        [NWDIndexRemove]
        public void RemoveFromCodeIndex()
        {
            // Remove from the actual indexation
            kCodeIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDError FindDataByCode(string sCode)
        {
            return kCodeIndex.RawFirstDataByKey(sCode);
        }
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndexSimple<NWDError> kDomainCodeIndex = new NWDIndexSimple<NWDError>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInsert]
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
        [NWDIndexRemove]
        public void RemoveFromDomainCodeIndex()
        {
            // Remove from the actual indexation
            kDomainCodeIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDError FindDataByDomainAndCode(string sDomain, string sCode)
        {
            return kDomainCodeIndex.RawFirstDataByKey(sDomain + NWDConstants.kFieldSeparatorA + sCode);
        }
        */
        //-------------------------------------------------------------------------------------------------------------
        public static NWDError FindDataByCode(string sCode)
        {
            NWDError rReturn = null;
            foreach (NWDError tObject in NWDBasisHelper.GetReachableDatas<NWDError>())
            {
                if (tObject.Code == sCode)
                {
                    rReturn = tObject;
                    break;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDError FindDataByDomainAndCode(string sDomain, string sCode)
        {
            NWDError rReturn = null;
            foreach (NWDError tObject in NWDBasisHelper.GetReachableDatas<NWDError>())
            {
                if (tObject.Code == sCode && tObject.Domain == sDomain)
                {
                    rReturn = tObject;
                    break;
                }
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================