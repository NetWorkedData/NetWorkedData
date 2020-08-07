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
#if UNITY_EDITOR
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassTrigrammeAttribute("RQT")]
    [NWDClassDescriptionAttribute("RequestToken descriptions Class")]
    [NWDClassMenuNameAttribute("RequestToken")]
#if UNITY_EDITOR
    [NWDClassClusterAttribute(1, 6)]
#endif
    public partial class NWDRequestToken : NWDBasisAccountRestricted
    {
        const string K_TOKEN_DELETE_INDEX = "K_TOKEN_DELETE_INDEX";
        //-------------------------------------------------------------------------------------------------------------
        [NWDIndexedAttribut(K_TOKEN_DELETE_INDEX)]
        [NWDAddIndexed(K_TOKEN_DELETE_INDEX, "DM")]
        [NWDAddIndexed(NWD.K_REQUEST_TOKEN_INDEX, "AC")]
        [NWDIndexedAttribut(NWD.K_REQUEST_TOKEN_INDEX)]
        [NWDAddIndexed(K_TOKEN_DELETE_INDEX, "Account")]
        [NWDAddIndexed(NWD.K_REQUEST_TOKEN_INDEX, "Account")]
        //[NWDCertified]
        //[NWDVarChar(256)]
        //public string UUIDHash
        //{
        //    get; set;
        //}
        [NWDIndexedAttribut(K_TOKEN_DELETE_INDEX)]
        [NWDCertified]
        [NWDVarChar(128)]
        public string Token
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
