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
using System.Linq.Expressions;
using System.Reflection;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDBasisBundled : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisBundled() { }
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisBundled(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData) { }
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupOrder(NWD.InspectorBasisHeader, -1)]
        [NWDNotEditable]
        [NWDCertified]
        //[NWDHidden]
        public NWDBundle Bundle
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================