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
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWBBenchmarkResultHelper : NWDHelper<NWBBenchmarkResult>
    {
    }
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("BMR")]
    [NWDClassDescriptionAttribute("Benchmark Result")]
    [NWDClassMenuNameAttribute("Benchmark Result")]
    //[NWDInternalKeyNotEditableAttribute]
    //[NWDInternalDescriptionNotEditable]
    public partial class NWBBenchmarkResult : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        public NWDDateTimeType HistoricDate { get; set; }
        public string Builder { get; set; }
        public string CompileOn { get; set; }
        public string CompileFor { get; set; }
        public string OSVersion { get; set; }
        public string CompileWith { get; set; }
        public string Device { get; set; }
        public bool PreloadDatas { get; set; }
        public int PreloadFaster { get; set; }
        public bool BenchmarkStep { get; set; }
        public double LaunchUnity { get; set; }
        public bool SQLSecure { get; set; }
        public string SQLVersion { get; set; }
        public double LauchNetworkedData { get; set; }
        public bool CopyDatabase { get; set; }
        public double LauchFinal { get; set; }
        public int RowsLoaded { get; set; }
        public int RowsMemoryIndexed { get; set; }
        public int MethodMemoryIndexation { get; set; }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        // never change the constructors! they are used by the NetWorkedData Writing System
        //-------------------------------------------------------------------------------------------------------------
        public NWBBenchmarkResult()
        {
            //Debug.Log("NWBBenchmarkResult Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWBBenchmarkResult(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWBBenchmarkResult Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
