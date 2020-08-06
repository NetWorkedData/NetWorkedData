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
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountNickname : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountNickname CurrentData()
        {
            NWDAccountInfos tAccountInfos = NWDAccountInfos.CurrentData();
            NWDAccountNickname tReturn = tAccountInfos.Nickname.GetReachableData();
            return tReturn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetCurrentData(NWDAccountNickname sAccountNickname)
        {
            NWDAccountInfos tAccountInfos = NWDAccountInfos.CurrentData();
            tAccountInfos.Nickname.SetData(sAccountNickname);
            tAccountInfos.UpdateData();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================