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
        [UnityTest]
        public IEnumerator TemporarySync()
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
        }
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator TemporarySync_Reset_ReSync()
        {
            Debug.Log("TemporarySync() Reset account");
            NWDUnitTests.ResetDevice();
            NWDUnitTests.TemporaryAccount();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            Debug.Log("@@@@@@@@ account before " + tEnvironment.PlayerAccountReference);
            // Sync
            string tOperationName = NWDToolbox.RandomStringUnix(16);
            NWDUnitTests.NewWebService(tOperationName);
            NWDOperationWebSynchronisation.AddOperation(tOperationName, NWDUnitTests.kSuccess, NWDUnitTests.kFailBlock, NWDUnitTests.kCancelBlock, NWDUnitTests.kProgressBlock, null, null, true, true, NWDOperationSpecial.None);
            while (NWDUnitTests.WebServiceIsRunning(tOperationName))
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

            NWDUnitTests.TemporaryAccount();
            NWDUnitTests.ShowDevice();
            Debug.Log("@@@@@@@@ account before second " + tEnvironment.PlayerAccountReference);
            // Sync
            string tOperationNameB = NWDToolbox.RandomStringUnix(16);
            NWDUnitTests.NewWebService(tOperationNameB);
            NWDOperationWebSynchronisation.AddOperation(tOperationNameB, NWDUnitTests.kSuccess, NWDUnitTests.kFailBlock, NWDUnitTests.kCancelBlock, NWDUnitTests.kProgressBlock, null, null, true, true, NWDOperationSpecial.None);
            while (NWDUnitTests.WebServiceIsRunning(tOperationNameB))
            {
                yield return null;
            }
            Debug.Log("@@@@@@@@ account after second " + tEnvironment.PlayerAccountReference);
            string tAccountB = tEnvironment.PlayerAccountReference + string.Empty;
            Assert.AreEqual(tAccountC, tAccountB);
        }
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator TemporaryUserTransfertSync()
        {
            Debug.Log("TemporaryUserTransfertSync() Reset account");
            NWDUnitTests.ResetDevice();
            NWDUnitTests.TemporaryAccount();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            Debug.Log("@@@@@@@@ account before" + tEnvironment.PlayerAccountReference);
            NWDAccountAvatar tAvatar = new NWDAccountAvatar();
            // first test assignation
            Assert.AreEqual(tAccountT, tAvatar.Account.GetReference());
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
            string tT = tAccountT.Substring(tAccountT.Length - 1);
            string tC = tAccountC.Substring(tAccountC.Length - 1);
            // test result
            Assert.AreNotEqual(tAccountC, tAccountT);
            Assert.AreEqual(tT, NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE);
            Assert.AreEqual(tC, NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE);
            Assert.AreEqual(tAccountC, tAvatar.Account.GetReference());
        }
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator TemporaryUserTransfertSyncMoreComplexe()
        {
            Debug.Log("TemporaryUserTransfertSyncMoreComplexe() Reset account");
            NWDUnitTests.ResetDevice();
            NWDUnitTests.TemporaryAccount();
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

            string tOperationName = NWDToolbox.RandomStringUnix(16);
            NWDUnitTests.NewWebService(tOperationName);
            NWDOperationWebSynchronisation.AddOperation(tOperationName, NWDUnitTests.kSuccess, NWDUnitTests.kFailBlock, NWDUnitTests.kCancelBlock, NWDUnitTests.kProgressBlock, null, null, true, true, NWDOperationSpecial.None);
            while (NWDUnitTests.WebServiceIsRunning(tOperationName))
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
        }
        //-------------------------------------------------------------------------------------------------------------
        /*
        [UnityTest]
        public IEnumerator TemporaryUserToCertifiedAddSignAndUsedIt()
        {
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            bool tLogFile = tEnvironment.LogInFileMode;
            bool tLogMode = tEnvironment.LogMode;
            tEnvironment.LogInFileMode = false;
            tEnvironment.LogMode = false;
            tEnvironment.ResetPreferences();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;

            Debug.Log("######################  1 SYNC ######################");
            bool tFinish = false;
            NWEOperation tOperation = null;
            NWEOperationBlock tSuccess = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
            {
                tFinish = true;
                tOperation = bOperation;
            };

            // first sync
            tFinish = false;
            NWDDataManager.SharedInstance().AddWebRequestAllSynchronizationWithBlock(tSuccess);
            while (tFinish == false)
            {
                yield return null;
            }

            Debug.Log("######################  2 SYNC ######################");
            // second sync
            tFinish = false;
            tEnvironment.LogInFileMode = true;

            string tSign = NWDToolbox.RandomStringCypher(24);
            Debug.Log("sign = " + tSign);
            NWDAccountSign tAccountSign = new NWDAccountSign();
            tAccountSign.Tag = NWDBasisTag.TagTestForDev;
            tAccountSign.RegisterSocialNetwork(tSign, NWDAccountSignType.Google);
            Debug.Log("tAccountSign Reference = " + tAccountSign.Reference);

            NWDDataManager.SharedInstance().AddWebRequestAllSynchronizationWithBlock(tSuccess);
            while (tFinish == false)
            {
                yield return null;
            }
            string tAccountC = tEnvironment.PlayerAccountReference + string.Empty;

            string tT = tAccountT.Substring(tAccountT.Length - 1);
            string tC = tAccountC.Substring(tAccountC.Length - 1);

            Assert.AreNotEqual(tAccountC, tAccountT);
            Assert.AreEqual(tT, NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE);
            Assert.AreEqual(tC, NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE);

            // test assert sign is Associated
            Assert.AreEqual(tAccountC, tAccountSign.Account.GetReference());
            Assert.AreEqual(NWDAccountSignAction.Associated, tAccountSign.SignStatus);

            Debug.Log("######################  3 SYNC ######################");
            // tirdh sync
            tFinish = false;

            Debug.Log("######################  4 SYNC ######################");
            // fourth sync
            tFinish = false;

            tEnvironment.LogInFileMode = tLogFile;
            tEnvironment.LogMode = tLogMode;
        }
        */
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
