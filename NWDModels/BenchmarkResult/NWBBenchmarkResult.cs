//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:10
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
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
    public partial class NWBBenchmarkResult : NWDBasisBundled
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Properties
        //-------------------------------------------------------------------------------------------------------------
        public NWDReferenceType<NWDAccount> Account { get; set; }
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