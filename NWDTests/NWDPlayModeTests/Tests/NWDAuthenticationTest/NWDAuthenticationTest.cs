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
    public class NWDAuthenticationTest
    {
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator TemporarySync()
        {
            Debug.Log("TemporarySync() Reset account");
            NWDUnitTests.EnableFakeDevice();
            NWDUnitTests.ResetFakeDevice();
            NWDUnitTests.UseTemporaryAccount();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            Debug.Log("@@@@@@@@ account before" + tEnvironment.PlayerAccountReference);
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
            NWDUnitTests.DisableFakeDevice();
        }
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator TemporarySync_Reset_ReSync()
        {
            Debug.Log("TemporarySync() Reset account");
            NWDUnitTests.EnableFakeDevice();
            NWDUnitTests.ResetFakeDevice();
            NWDUnitTests.UseTemporaryAccount();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            Debug.Log("@@@@@@@@ account before " + tEnvironment.PlayerAccountReference);
            // Sync
            NWDOperationWebSynchronisation tOperation = NWDOperationWebSynchronisation.AddOperation(null, null, null, null, null, null, null, true, false, NWDOperationSpecial.None);
            while (!tOperation.IsFinish)
            {
                yield return null;
            }
            Debug.Log("@@@@@@@@ account after " + tEnvironment.PlayerAccountReference);
            string tAccountC = tEnvironment.PlayerAccountReference + string.Empty;
            // test result
            Assert.AreNotEqual(tAccountC, tAccountT);
            string tT = tAccountT.Substring(tAccountT.Length - 1);
            Assert.AreEqual(tT, NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE);
            string tC = tAccountC.Substring(tAccountC.Length - 1);
            Assert.AreEqual(tC, NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE);

            NWDUnitTests.UseTemporaryAccount();
            NWDUnitTests.ShowFakeDevice();
            Debug.Log("@@@@@@@@ account before second " + tEnvironment.PlayerAccountReference);
            // Sync
            NWDOperationWebSynchronisation tOperationB = NWDOperationWebSynchronisation.AddOperation(null, null, null, null, null, null, null, true, false, NWDOperationSpecial.None);
            while (!tOperationB.IsFinish)
            {
                yield return null;
            }
            Debug.Log("@@@@@@@@ account after second " + tEnvironment.PlayerAccountReference);
            string tAccountB = tEnvironment.PlayerAccountReference + string.Empty;
            Assert.AreEqual(tAccountC, tAccountB);
            NWDUnitTests.DisableFakeDevice();
        }
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator TemporaryUserTransfertSync()
        {
            Debug.Log("TemporaryUserTransfertSync() Reset account");
            NWDUnitTests.EnableFakeDevice();
            NWDUnitTests.ResetFakeDevice();
            NWDUnitTests.UseTemporaryAccount();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            Debug.Log("@@@@@@@@ account before" + tEnvironment.PlayerAccountReference);
            NWDAccountAvatar tAvatar = new NWDAccountAvatar();
            // first test assignation
            Assert.AreEqual(tAccountT, tAvatar.Account.GetReference());
            // Sync
            NWDOperationWebSynchronisation tOperation = NWDOperationWebSynchronisation.AddOperation(null, null, null, null, null, null, null, true, false, NWDOperationSpecial.None);
            while (!tOperation.IsFinish)
            {
                yield return null;
            }
            Debug.Log("@@@@@@@@ account after" + tEnvironment.PlayerAccountReference);
            string tAccountC = tEnvironment.PlayerAccountReference + string.Empty;
            string tT = tAccountT.Substring(tAccountT.Length - 1);
            string tC = tAccountC.Substring(tAccountC.Length - 1);
            // test result
            Assert.AreNotEqual(tAccountC, tAccountT);
            Assert.AreEqual(tT, NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE);
            Assert.AreEqual(tC, NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE);
            Assert.AreEqual(tAccountC, tAvatar.Account.GetReference());
            NWDUnitTests.DisableFakeDevice();
        }
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator TemporaryUserTransfertSyncMoreComplexe()
        {
            Debug.Log("TemporaryUserTransfertSyncMoreComplexe() Reset account");
            NWDUnitTests.EnableFakeDevice();
            NWDUnitTests.ResetFakeDevice();
            NWDUnitTests.UseTemporaryAccount();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            Debug.Log("@@@@@@@@ account before" + tEnvironment.PlayerAccountReference);
            NWDAccountAvatar tAvatar = new NWDAccountAvatar();
            NWDGameSave tGameSave = NWDGameSave.CurrentData();
            NWDAccountInfos tAccountInfos = NWDAccountInfos.CurrentData();
            // test assignation
            Assert.AreEqual(tAccountT, tGameSave.Account.GetReference());
            Assert.AreEqual(tAccountT, tAccountInfos.Account.GetReference());
            Assert.AreEqual(tAccountT, tAvatar.Account.GetReference());
            // Sync
            NWDOperationWebSynchronisation tOperation = NWDOperationWebSynchronisation.AddOperation(null, null, null, null, null, null, null, true, false, NWDOperationSpecial.None);
            while (!tOperation.IsFinish)
            {
                yield return null;
            }
            Debug.Log("@@@@@@@@ account after" + tEnvironment.PlayerAccountReference);
            string tAccountC = tEnvironment.PlayerAccountReference + string.Empty;
            string tT = tAccountT.Substring(tAccountT.Length - 1);
            string tC = tAccountC.Substring(tAccountC.Length - 1);
            // test result
            Assert.AreNotEqual(tAccountC, tAccountT);
            Assert.AreEqual(tT, NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE);
            Assert.AreEqual(tC, NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE);
            Assert.AreEqual(tAccountC, tAvatar.Account.GetReference());
            Assert.AreEqual(tAccountC, tGameSave.Account.GetReference());
            Assert.AreEqual(tAccountC, tAccountInfos.Account.GetReference());
            NWDUnitTests.DisableFakeDevice();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif