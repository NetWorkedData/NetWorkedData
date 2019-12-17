using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using NetWorkedData;

#if UNITY_INCLUDE_TESTS
//=====================================================================================================================
namespace NWDPlayModeTests
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDSyncTest
    {
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator Sync()
        {
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            NWDUnitTests.LogStep("web sync A");
            bool tTest = false;
            NWDUnitTests.EnableFakeDevice();
            NWDUnitTests.ResetFakeDevice();
            NWDOperationWebSynchronisation tOperation = NWDOperationWebSynchronisation.AddOperation(null,
                delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
                {
                    Debug.Log("NWDSyncTest Success");
                    tTest = true;
                }, null, null, null, null, null, true, false, NWDOperationSpecial.None);
            NWDUnitTests.Log("TestSync() Start");
            while (!tOperation.IsFinish)
            {
                //NWDUnitTests.Log("TestSync() in progress");
            }
            NWDUnitTests.Log("TestSync() Finished");
            NWDUnitTests.DisableFakeDevice();

            Assert.IsTrue(tTest);

            tTest = false;
            NWDUnitTests.EnableFakeDevice();
            NWDUnitTests.LogStep("web sync B");
            NWDOperationWebSynchronisation tOperationB = NWDOperationWebSynchronisation.AddOperation(null,
                delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
                {
                    Debug.Log("NWDSyncTest Success");
                    tTest = true;
                }, null, null, null, null, null, true, false, NWDOperationSpecial.None);
            NWDUnitTests.Log("TestSync() B Start");
            while (!tOperationB.IsFinish)
            {
                //NWDUnitTests.Log("TestSync() B in progress");
            }
            NWDUnitTests.Log("TestSync() B Finished");
            NWDUnitTests.DisableFakeDevice();

            Assert.IsTrue(tTest);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif