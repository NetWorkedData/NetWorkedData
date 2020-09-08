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
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDIPBanHelper : NWDHelper<NWDIPBan>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassTrigrammeAttribute("IPB")]
    [NWDClassDescriptionAttribute("IP banned")]
    [NWDClassMenuNameAttribute("IP Ban")]
    public partial class NWDIPBan : NWDBasisAccountRestricted
    {
        //-------------------------------------------------------------------------------------------------------------
        //PROPERTIES
        [NWDAddIndexed(NWD.K_IP_BAN_INDEX,"AXC")]
        [NWDIndexedAttribut(NWD.K_IP_BAN_INDEX)]
        public NWDIPType IP {get; set;}
        [NWDIndexedAttribut(NWD.K_IP_BAN_INDEX)]
		public NWDDateTimeUtcType Deadline {get; set;}
        [NWDIndexedAttribut(NWD.K_IP_BAN_INDEX)]
		public int Counter {get; set;}
        //[NWDNotEditable]
        [NWDIndexedAttribut(NWD.K_IP_BAN_INDEX)]
		public int CounterMaximum {get; set;}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
