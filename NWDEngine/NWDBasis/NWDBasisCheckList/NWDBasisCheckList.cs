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
    public partial class NWDBasisCheckList : NWEDataTypeMaskGeneric<NWDBasisCheckList>
    {
        //-------------------------------------------------------------------------------------------------------------
        public static NWDBasisCheckList Translate = Add(1, "Need Translate");
        public static NWDBasisCheckList Asset = Add(2, "Need Asset");
        public static NWDBasisCheckList Description = Add(3, "Need Description");
        public static NWDBasisCheckList Test = Add(4, "Need Test");


        public static NWDBasisCheckList WTF = Add(9, "WTF!");
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
