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
//=====================================================================================================================
#if UNITY_EDITOR
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDUserNetWorkingState : int
    {
        Unknow = -1,
        OffLine = 0,
        OnLine = 1,

        NotDisturbe = 2,
        Masked = 9,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UNW")]
    [NWDClassDescriptionAttribute("User statut on Network")]
    [NWDClassMenuNameAttribute("User Net Working")]
#if UNITY_EDITOR
    [NWDClassClusterAttribute(1, 32)]
    [NWDWindowOwner(typeof(NWDUserWindow))]
#endif
    public partial class NWDUserNetWorking : NWDBasisGameSaveDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDDateTimeType NextUpdate
        {
            get; set;
        }
        public bool NotDisturbe
        {
            get; set;
        }
        public bool Masked
        {
            get; set;
        }
        // perhaps add some stats 
        public int TotalPlay
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================