﻿//=====================================================================================================================
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

//=====================================================================================================================
namespace NetWorkedData
{
//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDGameDomain : int
    {
        Account = 0,
        GameSave = 1,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("GSV")]
    [NWDClassDescriptionAttribute("Game Save")]
    [NWDClassMenuNameAttribute("Game Save")]
    [NWDClassClusterAttribute(1, 32)]
    public partial class NWDGameSave : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        public string Name
        {
            get; set;
        }
        public bool IsCurrent
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================