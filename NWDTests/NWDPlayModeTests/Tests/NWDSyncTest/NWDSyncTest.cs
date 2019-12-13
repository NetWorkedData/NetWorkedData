using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using NetWorkedData;

namespace Tests
{
    public class NWDSyncTest
    {
        [UnityTest]
        public IEnumerator Sync()
        {
            Debug.Log("TestSync() Start");
            bool tFinish = false;
            NWEOperationBlock tSuccess = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
            {
                Debug.Log("TestSync() tSuccess");
                tFinish = true;
            };
            NWEOperationBlock tFailBlock = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
            {
                Debug.Log("TestSync() tFailBlock");
                tFinish = true;
            };
            NWEOperationBlock tCancelBlock = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
            {
                Debug.Log("TestSync() tCancelBlock");
                tFinish = true;
            };
            NWEOperationBlock tProgressBlock = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
            {
                Debug.Log("TestSync() tProgressBlock");
            };
            NWDOperationWebSynchronisation.AddOperation("Test Sync", tSuccess, tFailBlock, tCancelBlock, tProgressBlock, null, null, true, true, NWDOperationSpecial.None);
            while (tFinish == false)
            {
                yield return null;
            }
            Debug.Log("TestSync() Finish");
        }

        [UnityTest]
        public IEnumerator TemporarySync()
        {
            Debug.Log("TestTemporarySync() Reset account");
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            tEnvironment.ResetPreferences();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            Debug.Log("TestTemporarySync() account " + tEnvironment.PlayerAccountReference);

            Debug.Log("TestTemporarySync() Start");
            bool tFinish = false;
            NWEOperationBlock tSuccess = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
            {
                Debug.Log("TestTemporarySync() tSuccess");
                tFinish = true;
            };
            NWDOperationWebSynchronisation.AddOperation("TestTemporarySync()", tSuccess, null, null, null, null, null, true, true, NWDOperationSpecial.None);
            while (tFinish == false)
            {
                yield return null;
            }
            Debug.Log("TestTemporarySync() Finish");
            Debug.Log("TestTemporarySync() account " + tEnvironment.PlayerAccountReference);
            string tAccountC = tEnvironment.PlayerAccountReference + string.Empty;
            Assert.AreNotEqual(tAccountC, tAccountT);

            string tT = tAccountT.Substring(tAccountT.Length - 1);
            Assert.AreEqual(tT, NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE);

            string tC = tAccountC.Substring(tAccountC.Length - 1);
            Assert.AreEqual(tC, NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE);
        }

        [UnityTest]
        public IEnumerator TemporaryUserTransfertSync()
        {
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            bool tLog = tEnvironment.LogInFileMode;
            tEnvironment.LogInFileMode = false;
            tEnvironment.LogMode = true;

            Debug.Log("TestTemporaryUserTransfertSync() Reset account");
            tEnvironment.ResetPreferences();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            NWDAccountAvatar tAvatar = new NWDAccountAvatar();
            Assert.AreEqual(tAccountT, tAvatar.Account.GetReference());
            Debug.Log("TestTemporaryUserTransfertSync() Start");
            bool tFinish = false;
            NWEOperation tOperation = null;
            NWEOperationBlock tSuccess = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
            {
                tFinish = true;
                tOperation = bOperation;
            };
            NWDOperationWebSynchronisation.AddOperation("TestTemporaryUserTransfertSync()", tSuccess, null, null, null, null, null, true, true, NWDOperationSpecial.None);
            while (tFinish == false)
            {
                yield return null;
            }
            Debug.Log("TestTemporaryUserTransfertSync() Finish");
            string tAccountC = tEnvironment.PlayerAccountReference + string.Empty;
            Assert.AreEqual(tAccountC, tAvatar.Account.GetReference());
            tEnvironment.LogInFileMode = tLog;
        }

        [UnityTest]
        public IEnumerator TemporaryUserTransfertSyncMoreComplexe()
        {
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            bool tLog = tEnvironment.LogInFileMode;
            tEnvironment.LogInFileMode = false;
            tEnvironment.LogMode = true;

            Debug.Log("TemporaryUserTransfertSyncMoreComplexe() Reset account");
            tEnvironment.ResetPreferences();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            NWDAccountAvatar tAvatar = new NWDAccountAvatar();
            NWDGameSave tGameSave = NWDGameSave.CurrentData();
            NWDAccountInfos tAccountInfos = NWDAccountInfos.CurrentData();

            Assert.AreEqual(tAccountT, tGameSave.Account.GetReference());
            Assert.AreEqual(tAccountT, tAccountInfos.Account.GetReference());
            Assert.AreEqual(tAccountT, tAvatar.Account.GetReference());
            Debug.Log("TemporaryUserTransfertSyncMoreComplexe() Start");
            bool tFinish = false;
            NWEOperation tOperation = null;
            NWEOperationBlock tSuccess = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
            {
                tFinish = true;
                tOperation = bOperation;
            };
            NWDOperationWebSynchronisation.AddOperation("TemporaryUserTransfertSyncMoreComplexe()", tSuccess, null, null, null, null, null, true, true, NWDOperationSpecial.None);
            while (tFinish == false)
            {
                yield return null;
            }
            Debug.Log("TemporaryUserTransfertSyncMoreComplexe() Finish");
            string tAccountC = tEnvironment.PlayerAccountReference + string.Empty;

            string tT = tAccountT.Substring(tAccountT.Length - 1);
            string tC = tAccountC.Substring(tAccountC.Length - 1);

            // test assert final
            Assert.AreNotEqual(tAccountC, tAccountT);
            Assert.AreEqual(tT, NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE);
            Assert.AreEqual(tC, NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE);
            Assert.AreEqual(tAccountC, tAvatar.Account.GetReference());
            Assert.AreEqual(tAccountC, tGameSave.Account.GetReference());
            Assert.AreEqual(tAccountC, tAccountInfos.Account.GetReference());

            tEnvironment.LogInFileMode = tLog;
        }



        
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

    }
}
