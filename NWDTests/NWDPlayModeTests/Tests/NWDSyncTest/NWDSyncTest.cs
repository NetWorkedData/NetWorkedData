using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using NetWorkedData;

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
            string tOperationName = NWDToolbox.RandomStringUnix(16);
            NWDUnitTests.NewWebService(tOperationName);
            NWDOperationWebSynchronisation.AddOperation(tOperationName, NWDUnitTests.kSuccess, NWDUnitTests.kFailBlock, NWDUnitTests.kCancelBlock, NWDUnitTests.kProgressBlock, null, null, true, true, NWDOperationSpecial.None);
            while (NWDUnitTests.WebServiceIsRunning(tOperationName))
            {
                yield return null;
            }
            Debug.Log("TestSync() Finish");
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
