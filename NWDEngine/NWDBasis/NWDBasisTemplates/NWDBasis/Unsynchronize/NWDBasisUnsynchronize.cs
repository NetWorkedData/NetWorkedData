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

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// This class is used to heritage for the class unsynchronize with the cluster.
    /// </summary>
    public partial class NWDBasisUnsynchronize : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisUnsynchronize()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDBasisUnsynchronize(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonInsertMe()
        {
            DevSync = -1;
            PreprodSync = -1;
            ProdSync = -1;
            DS = -1;
            base.AddonInsertMe();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonUpdateMe()
        {
            DevSync = -1;
            PreprodSync = -1;
            ProdSync = -1;
            DS = -1;
            base.AddonUpdateMe();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================