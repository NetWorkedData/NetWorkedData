//=====================================================================================================================
//
//  ideMobi copyright 2019. All rights reserved by ideMobi.
//
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
namespace NWDEditorProcess
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