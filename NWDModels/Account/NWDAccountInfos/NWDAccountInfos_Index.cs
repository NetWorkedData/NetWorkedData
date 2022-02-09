//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
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
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountInfos : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        static private NWDAccountInfos kAccountInfos;
        //-------------------------------------------------------------------------------------------------------------
        public static void ResetCurrentData()
        {
            kAccountInfos = null;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Get the current <see cref="NWDAccountInfos"/> instance for the current <see cref="NWDAccount"/>.
        /// </summary>
        /// <returns></returns>
        public static NWDAccountInfos CurrentData()
        {
            NWDBenchmark.Start();
            NWDAccountInfos tInfos = null;
            if (kAccountInfos == null)
            {
                NWDBenchmark.Step();
                if (NWDLauncher.GetState() == NWDStatut.NetWorkedDataReady)
                //if (NWDBasisHelper.FindTypeInfos(typeof(NWDAccountInfos)).IsLoaded())
                {
                    string tUniqueReference = NWDAccount.GetUniqueReferenceFromCurrentAccount<NWDAccountInfos>();
                    tInfos = NWDBasisHelper.GetRawDataByReference<NWDAccountInfos>(tUniqueReference);
                    //NWDBenchmark.Step();
                    if (tInfos == null && string.IsNullOrEmpty(tUniqueReference) == false)
                    {
                        tInfos = NWDBasisHelper.NewDataWithReference<NWDAccountInfos>(tUniqueReference);
                        tInfos.NotNullChecker();
                        tInfos.SaveData();
                    }
                    NWDBenchmark.Step();
                    kAccountInfos = tInfos;
                }
            }
            else
            {
                tInfos = kAccountInfos;
            }
            NWDBenchmark.Finish();
            return tInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        public static NWDAccountInfos CurrentDataForAccount(string sAccountReference)
        {
            NWDBenchmark.Start();
            NWDAccountInfos tInfos = null;
            if (NWDBasisHelper.FindTypeInfos(typeof(NWDAccountInfos)).IsLoaded())
            {
                string tUniqueReference = NWDAccount.GetUniqueReference(sAccountReference, typeof(NWDAccountInfos));
                tInfos = NWDBasisHelper.GetRawDataByReference<NWDAccountInfos>(tUniqueReference);
            }
            NWDBenchmark.Finish();
            return tInfos;
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
