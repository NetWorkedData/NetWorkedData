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
//MACRO_DEFINE #if NWD_EXAMPLE_MACRO
#if UNITY_INCLUDE_TESTS
using System.Collections;
using UnityEngine.TestTools;
using NetWorkedData;
//=====================================================================================================================
namespace NWDEditModeProcess
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDExample_EditorProcessTests : NWDClassTest
    {
        // herited NWDClassTest
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDExample_EditorProcessTests : NWDClassTest
    {
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator Enumerator_Loop()
        {
            int tI = 0;
            while (tI < 10)
            {
                tI++;
                yield return null;
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif
//MACRO_DEFINE #endif //NWD_EXAMPLE_MACRO
