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
#if UNITY_INCLUDE_TESTS
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NetWorkedData;
//=====================================================================================================================
namespace NWDPlayModeProcess
{
    public class NWDExample_Process : NWDClassTest
    {
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator SyncEditorModelTest()
        {
            NWDBenchmark.Start();
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            NWDUnitTests.EnableFakeDevice();
            NWDUnitTests.LogNewTest();
            NWDUnitTests.ResetFakeDevice();
            NWDUnitTests.UseTemporaryAccount();
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            NWDUnitTests.LogStep("web sync");
            bool tTest = false;
            NWDOperationWebSynchronisation tOperation = NWDOperationWebSynchronisation.AddOperation(null,
                delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
                {
                    Debug.Log("NWDSyncTest Success");
                    tTest = true;
                }, null, null, null, null, new List<Type>() { typeof(NWDExample) }, null, true, false, NWDOperationSpecial.None);
            NWDUnitTests.Log("TestSync() Start");
            while (!tOperation.IsFinish)
            {
                NWDUnitTests.Log("TestSync() in progress");
                yield return null;
            }
            NWDUnitTests.Log("TestSync() Finished");
            NWDUnitTests.DisableFakeDevice();
            Assert.IsTrue(tTest);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            NWDBenchmark.Finish();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif