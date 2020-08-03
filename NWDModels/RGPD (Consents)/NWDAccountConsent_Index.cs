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
using UnityEngine;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountConsent : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        static protected NWDIndex<NWDConsent, NWDAccountConsent> kAccountIndex = new NWDIndex<NWDConsent, NWDAccountConsent>();
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexInMemory]
        public void InsertInAccountIndex()
        {
            // Re-add to the actual indexation ?
            if (IsUsable())
            {
                // Re-add !
                if (Consent.GetRawData() != null)
                {
                    string tKey = Consent.GetReference() + NWDConstants.kFieldSeparatorA + Consent.GetRawData().Version.ToString() + NWDConstants.kFieldSeparatorA + Account.GetReference();
                    kAccountIndex.UpdateData(this, tKey);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDDeindexInMemory]
        public void RemoveFromAccountIndex()
        {
            // Remove from the actual indexation
            kAccountIndex.RemoveData(this);
        }
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountConsent FindDataByConsent(NWDConsent sConsent, bool sOrCreate = true)
        {
            string tKey = sConsent.Reference + NWDConstants.kFieldSeparatorA + sConsent.Version.ToString() + NWDConstants.kFieldSeparatorA + NWDAccount.CurrentReference();
            NWDAccountConsent rReturn = kAccountIndex.FirstRawDataByKey(tKey);
            if (rReturn == null && sOrCreate == true)
            {
                rReturn = NWDBasisHelper.NewData<NWDAccountConsent>();
                //rReturn.Account.SetReference(NWDAccount.CurrentReference());
                rReturn.Tag = NWDBasisTag.TagUserCreated;
                rReturn.Consent.SetReference(sConsent.Reference);
                rReturn.Version.SetValue(sConsent.Version.GetValue());
                rReturn.Authorization = NWESwitchState.Unknow;
                rReturn.UpdateData();
            }
            return rReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================