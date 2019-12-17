using System;
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
        public IEnumerator TemporarySyncAddSign()
        {
            // create account
            Debug.Log("TemporarySync() Reset account");
            NWDUnitTests.ResetDevice();
            NWDUnitTests.TemporaryAccount();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            Debug.Log("@@@@@@@@ account before" + tEnvironment.PlayerAccountReference);
            // Sync
            string tOperationName = "Account temporary to certified "+NWDToolbox.RandomStringUnix(16);
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

            // add sign
            NWDAccountSign tAccountSign = new NWDAccountSign();
            string tSign = NWDToolbox.RandomStringCypher(32);
            tAccountSign.RegisterSocialNetwork(tSign, NWDAccountSignType.Fake);
            // Sync
            string tOperationNameB = "upload sign data" + NWDToolbox.RandomStringUnix(16);
            NWDUnitTests.NewWebService(tOperationNameB, true);
            NWDOperationWebSynchronisation.AddOperation(tOperationNameB, NWDUnitTests.kSuccess, NWDUnitTests.kFailBlock, NWDUnitTests.kCancelBlock, NWDUnitTests.kProgressBlock, null, new List<Type>() { typeof(NWDAccountSign) }, false, true, NWDOperationSpecial.None);
            while (NWDUnitTests.WebServiceIsRunning(tOperationNameB))
            {
                yield return null;
            }
            Debug.Log("@@@@@@@@ Data is Sync");
            // data associated with success
            Assert.AreEqual(NWDAccountSignAction.Associated, tAccountSign.SignStatus);
        }
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator TemporarySyncAndAddSignSignIn()
        {
            // create account
            Debug.Log("TemporarySync() Reset account");
            NWDUnitTests.ResetDevice();
            NWDUnitTests.TemporaryAccount();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            Debug.Log("@@@@@@@@ account before" + tEnvironment.PlayerAccountReference);
            // Sync
            string tOperationName = NWDToolbox.RandomStringUnix(16);
            NWDUnitTests.NewWebService(tOperationName, false);
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

            // add sign
            NWDAccountSign tAccountSign = new NWDAccountSign();
            string tSign = NWDToolbox.RandomStringCypher(32);
            tAccountSign.RegisterSocialNetwork(tSign, NWDAccountSignType.Fake);
            // Sync
            string tOperationNameB = "upload sign data" + NWDToolbox.RandomStringUnix(16);
            NWDUnitTests.NewWebService(tOperationNameB, false);
            NWDOperationWebSynchronisation.AddOperation(tOperationNameB, NWDUnitTests.kSuccess, NWDUnitTests.kFailBlock, NWDUnitTests.kCancelBlock, NWDUnitTests.kProgressBlock, null, new List<Type>() { typeof(NWDAccountSign) }, false, true, NWDOperationSpecial.None);
            while (NWDUnitTests.WebServiceIsRunning(tOperationNameB))
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
            NWDUnitTests.NewWebService(tOperationNameC, true);
            NWDOperationWebAccount tOperationWebAccount = NWDDataManager.SharedInstance().AddWebRequestSignInWithBlock(
                                                                    tSign,
                                                                    NWDUnitTests.kSuccess,
                                                                    NWDUnitTests.kFailBlock,
                                                                    NWDUnitTests.kCancelBlock,
                                                                    NWDUnitTests.kProgressBlock,
                                                                    true,
                                                                    null);
            tOperationWebAccount.name = tOperationNameC;
            NWDOperationWebAccount sOperation = NWDOperationWebAccount.Create(tOperationNameC,
                                                                    NWDUnitTests.kSuccess,
                                                                    NWDUnitTests.kFailBlock,
                                                                    NWDUnitTests.kCancelBlock,
                                                                    NWDUnitTests.kProgressBlock,
                                                                    null);
            sOperation.Action = NWDOperationWebAccountAction.signin;
            sOperation.PasswordToken = tSign;
            NWDDataManager.SharedInstance().WebOperationQueue.AddOperation(sOperation, true);

            while (NWDUnitTests.WebServiceIsRunning(tOperationNameC))
            {
                yield return null;
            }

            Debug.Log("@@@@@@@@ account after" + tEnvironment.PlayerAccountReference);
            string tAccountCB = tEnvironment.PlayerAccountReference + string.Empty;
            Assert.AreEqual(tAccountC, tAccountCB);
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
