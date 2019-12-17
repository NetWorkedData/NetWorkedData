using System;
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
    public class NWDAuthenticationBySignTest
    {
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator TemporarySyncAddSign()
        {
            NWDUnitTests.ActiveDevice();
            // create account
            Debug.Log("TemporarySync() Reset account");
            NWDUnitTests.ResetDevice();
            NWDUnitTests.TemporaryAccount();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            Debug.Log("@@@@@@@@ account before" + tEnvironment.PlayerAccountReference);
            // Sync
            NWDOperationWebSynchronisation tOperation = NWDOperationWebSynchronisation.AddOperation(null, null, null, null, null, null, null, true, false, NWDOperationSpecial.None);
            while (!tOperation.IsFinish)
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

            // add sign
            NWDAccountSign tAccountSign = new NWDAccountSign();
            string tSign = NWDToolbox.RandomStringCypher(32);
            tAccountSign.RegisterSocialNetwork(tSign, NWDAccountSignType.Fake);
            // Sync
            NWDOperationWebSynchronisation tOperationB = NWDOperationWebSynchronisation.AddOperation(null, null, null, null, null, null, new List<Type>() { typeof(NWDAccountSign) }, true, false, NWDOperationSpecial.None);
            while (!tOperationB.IsFinish)
            {
                yield return null;
            }
            Debug.Log("@@@@@@@@ Data is Sync");
            // data associated with success
            Assert.AreEqual(NWDAccountSignAction.Associated, tAccountSign.SignStatus);
            NWDUnitTests.DisableDevice();
        }
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator TemporarySyncAndAddSignSignIn()
        {
            NWDUnitTests.ActiveDevice();
            // create account
            Debug.Log("TemporarySync() Reset account");
            NWDUnitTests.ResetDevice();
            NWDUnitTests.TemporaryAccount();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            Debug.Log("@@@@@@@@ account before" + tEnvironment.PlayerAccountReference);
            // Sync
            NWDOperationWebSynchronisation tOperation = NWDOperationWebSynchronisation.AddOperation(null, null, null, null, null, null, null, true, false, NWDOperationSpecial.None);
            while (!tOperation.IsFinish)
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

            // add sign
            NWDAccountSign tAccountSign = new NWDAccountSign();
            string tSign = NWDToolbox.RandomStringCypher(32);
            tAccountSign.RegisterSocialNetwork(tSign, NWDAccountSignType.Fake);
            // Sync
            NWDOperationWebSynchronisation tOperationB = NWDOperationWebSynchronisation.AddOperation(null, null, null, null, null, null, new List<Type>() { typeof(NWDAccountSign) }, true, false, NWDOperationSpecial.None);
            while (!tOperationB.IsFinish)
            {
                yield return null;
            }
            Debug.Log("@@@@@@@@ Data is Sync");
            // data associated with success
            Assert.AreEqual(NWDAccountSignAction.Associated, tAccountSign.SignStatus);


            NWDUnitTests.ResetDevice();
            NWDUnitTests.TemporaryAccount();
            string tAccountTB = tEnvironment.PlayerAccountReference + string.Empty;
            Debug.Log("@@@@@@@@ account on new device" + tEnvironment.PlayerAccountReference);
            Assert.AreNotEqual(tAccountC, tAccountT);
            Assert.AreNotEqual(tAccountTB, tAccountT);
            Assert.AreNotEqual(tAccountC, tAccountTB);

            string tOperationNameC = "test sign in" + NWDToolbox.RandomStringUnix(16);
            NWDOperationWebAccount tOperationWebAccount = NWDDataManager.SharedInstance().AddWebRequestSignInWithBlock(
                                                                    tSign,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    null,
                                                                    false,
                                                                    null);
            while (!tOperationWebAccount.IsFinish)
            {
                yield return null;
            }

            Debug.Log("@@@@@@@@ account after" + tEnvironment.PlayerAccountReference);
            string tAccountCB = tEnvironment.PlayerAccountReference + string.Empty;
            Assert.AreEqual(tAccountC, tAccountCB);

            NWDUnitTests.DisableDevice();
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
#endif