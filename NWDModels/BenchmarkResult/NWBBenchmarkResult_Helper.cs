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
using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWBBenchmarkResultHelper : NWDHelper<NWBBenchmarkResult>
    {
        //-------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
        public override void  ErrorRegenerate()
        {
            //NWDError.CreateGenericError(ClassNamePHP, ClassTrigramme + "01", "your error 01", "your description", "OK", NWDErrorType.InGame, NWDBasisBundledTag.TagInternal);
        }
#endif
        //-------------------------------------------------------------------------------------------------------------
        public override void  ClassInitialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override List<Type>  OverrideClasseInThisSync()
        {
            return new List<Type>
            {
                typeof(NWBBenchmarkResult),
                //typeof(NWDOtehrType),
            };
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================