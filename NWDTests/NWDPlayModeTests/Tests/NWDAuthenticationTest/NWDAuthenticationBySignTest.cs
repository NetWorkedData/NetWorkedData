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
    public class NWDAuthenticationBySignTest
	{
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator TemporarySyncAndSignUpSignInSignOut()
        {
            Debug.Log("TemporarySync() Reset account");
            NWDUnitTests.ResetDevice();
            NWDUnitTests.TemporaryAccount();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            Debug.Log("@@@@@@@@ account before" + tEnvironment.PlayerAccountReference);
            // Sync
            string tOperationName = NWDToolbox.RandomStringUnix(16);
            NWDUnitTests.NewWebService(tOperationName);
            NWDOperationWebSynchronisation.AddOperation(tOperationName, NWDUnitTests.kSuccess, NWDUnitTests.kFailBlock, NWDUnitTests.kCancelBlock, NWDUnitTests.kProgressBlock, null, null, true, true, NWDOperationSpecial.None);
            while (NWDUnitTests.WebServiceIsRunning(tOperationName))
            {
                yield return null;
            }
            Debug.Log("@@@@@@@@ account after" + tEnvironment.PlayerAccountReference);
            string tAccountC = tEnvironment.PlayerAccountReference + string.Empty;
            // test result
            Assert.AreNotEqual(tAccountC, tAccountT);
            string tT = tAccountT.Substring(tAccountT.Length - 1);
            Assert.AreEqual(tT, NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE);
            string tC = tAccountC.Substring(tAccountC.Length - 1);
            Assert.AreEqual(tC, NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE);

            // TODO FINISH TEST
        }

		// TODO Reset device signup, deco, signin

		// TODO Reset device signup, Reset device, reset account, signin

		// TODO sans reset, device signup, deco, signin

		// TODO sans reset, device signup, Reset device, reset account, signin

		// TODO Reset device signup, deco, signin, signin (error with sign17)

		//-------------------------------------------------------------------------------------------------------------
	}
	//+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
