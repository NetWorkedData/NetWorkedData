//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
// EXECEPTION NEVER DEFINE HERE TO CHECK GLOBAL DEFINE SYMBOL OF PLATEFORM
//#if NWD_VERBOSE
//#if UNITY_EDITOR
//#define NWD_LOG
//#define NWD_BENCHMARK
//#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
//#endif
//#endif
//=====================================================================================================================

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public static partial class NWDLauncher
    {
        //-------------------------------------------------------------------------------------------------------------
        [RuntimeInitializeOnLoadMethod]
        static public void Launch()
        {
            // just alert if some defines are used in global Scripting Define Symbol
#if NWD_VERBOSE
            UnityEngine.Debug.Log("Macro <b>NWD_VERBOSE</b> is defined!");
#endif
#if NWD_LOG
            UnityEngine.Debug.Log("Macro  <b>NWD_LOG</b> is defined!");
#endif
#if NWD_BENCHMARK
           UnityEngine.Debug.Log("Macro  <b>NWD_BENCHMARK</b> is defined!");
#endif
            LaunchEngine();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
