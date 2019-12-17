
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
    public class NWDAuthenticationDataTest
    {
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator TemporaryDataSync()
        {
            NWDUnitTests.EnableFakeDevice();
            NWDUnitTests.LogNewTest();
            NWDUnitTests.ResetFakeDevice();
            NWDUnitTests.UseTemporaryAccount();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            NWDUnitTests.Log("account before" + tEnvironment.PlayerAccountReference);
            // Sync
            NWDOperationWebSynchronisation tOperation = NWDOperationWebSynchronisation.AddOperation(null, null, null, null, null, null, null, true, false, NWDOperationSpecial.None);
            while (!tOperation.IsFinish)
            {
                yield return null;
            }
            NWDUnitTests.Log("account after" + tEnvironment.PlayerAccountReference);
            string tAccountC = tEnvironment.PlayerAccountReference + string.Empty;
            // test result
            Assert.AreNotEqual(tAccountC, tAccountT);
            string tT = tAccountT.Substring(tAccountT.Length - 1);
            Assert.AreEqual(tT, NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE);
            string tC = tAccountC.Substring(tAccountC.Length - 1);
            Assert.AreEqual(tC, NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE);


            NWDUnitTests.LogStep("add avatar and sync");
            // Ok create Data And Sync it
            //NWDAccountAvatar tAvatar = new NWDAccountAvatar();
            NWDAccountAvatar tAvatar = NWDBasisHelper.NewData<NWDAccountAvatar>();
            // Save Data !
            tAvatar.UpdateData();
            // Memorise Reference of Data
            string tReference = tAvatar.Reference + string.Empty;
            // Sync
            NWDOperationWebSynchronisation tOperationB = NWDOperationWebSynchronisation.AddOperation(null, null, null, null, null, null, new List<Type>(){typeof(NWDAccountAvatar)}, true, false, NWDOperationSpecial.None);
            while (!tOperationB.IsFinish)
            {
                yield return null;
            }

            NWDUnitTests.LogStep("delete avatar and reload");
            // delete Data
            tAvatar.DeleteData();
            // force apply modification in database
            NWDDataManager.SharedInstance().DataQueueExecute();
            // reload data from base
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            while (NWDDataManager.SharedInstance().DataIndexed == false)
            {
                yield return null;
            }
            // try get data with no success
            NWDAccountAvatar tAvatarTest = NWDBasisHelper.GetEditorDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreEqual(null, tAvatarTest);


            NWDUnitTests.LogStep("Force reload all data");
            // Sync Force
            NWDOperationWebSynchronisation tOperationC = NWDOperationWebSynchronisation.AddOperation(null, null, null, null, null, null, new List<Type>() { typeof(NWDAccountAvatar) }, true, false, NWDOperationSpecial.None);
            while (!tOperationC.IsFinish)
            {
                yield return null;
            }
            // try get data with success
            NWDAccountAvatar tAvatarTestB = NWDBasisHelper.GetEditorDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreNotEqual(null, tAvatarTestB);

            NWDUnitTests.DisableFakeDevice();
        }
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator TemporaryNewDataSync()
        {
            Debug.Log("TemporarySync() Reset account");
            NWDUnitTests.EnableFakeDevice();
            NWDUnitTests.ResetFakeDevice();
            NWDUnitTests.UseTemporaryAccount();
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
            // Ok create Data And Sync it
            NWDAccountAvatar tAvatar = NWDBasisHelper.NewData<NWDAccountAvatar>();
            // Save Data !
            //tAvatar.UpdateData();
            // Memorise Reference of Data
            string tReference = tAvatar.Reference + string.Empty;
            // Sync
            NWDOperationWebSynchronisation tOperationB = NWDOperationWebSynchronisation.AddOperation(null, null, null, null, null, null, new List<Type>() { typeof(NWDAccountAvatar) }, true, false, NWDOperationSpecial.None);
            while (!tOperationB.IsFinish)
            {
                yield return null;
            }
            Debug.Log("@@@@@@@@ Data is Sync");
            // delete Data
            tAvatar.DeleteData();
            // force apply modification in database
            NWDDataManager.SharedInstance().DataQueueExecute();
            // reload data from base
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            while (NWDDataManager.SharedInstance().DataIndexed == false)
            {
                yield return null;
            }
            // try get data with no success
            NWDAccountAvatar tAvatarTest = NWDBasisHelper.GetEditorDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreEqual(null, tAvatarTest);
            // Sync Force
            NWDOperationWebSynchronisation tOperationC = NWDOperationWebSynchronisation.AddOperation(null, null, null, null, null, null, new List<Type>() { typeof(NWDAccountAvatar) }, true, false, NWDOperationSpecial.None);
            while (!tOperationC.IsFinish)
            {
                yield return null;
            }
            // try get data with success
            NWDAccountAvatar tAvatarTestB = NWDBasisHelper.GetEditorDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreNotEqual(tAvatarTestB, null);
            NWDUnitTests.DisableFakeDevice();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif