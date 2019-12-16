
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
    public class NWDAuthenticationDataTest
    {
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator TemporaryDataSync()
        {
            Debug.Log("TemporarySync() Reset account");
            NWDUnitTests.ResetDevice();
            NWDUnitTests.TemporaryAccount();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            Debug.Log("@@@@@@@@ account before" + tEnvironment.PlayerAccountReference);
            // Sync
            string tOperationName = "Temporary to cert_" + NWDToolbox.RandomStringUnix(16);
            NWDUnitTests.NewWebService(tOperationName, false);
            NWDOperationWebSynchronisation.AddOperation(tOperationName, NWDUnitTests.kSuccess, NWDUnitTests.kFailBlock, NWDUnitTests.kCancelBlock, NWDUnitTests.kProgressBlock, null, null, false, true, NWDOperationSpecial.None);
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
            // Ok create Data And Sync it
            NWDAccountAvatar tAvatar = new NWDAccountAvatar();
            // Save Data !
            //tAvatar.UpdateData();
            // Memorise Reference of Data
            string tReference = tAvatar.Reference + string.Empty;
            // Sync
            string tOperationNameB = "upload avatar data" + NWDToolbox.RandomStringUnix(16);
            NWDUnitTests.NewWebService(tOperationNameB, true);
            NWDOperationWebSynchronisation.AddOperation(tOperationNameB, NWDUnitTests.kSuccess, NWDUnitTests.kFailBlock, NWDUnitTests.kCancelBlock, NWDUnitTests.kProgressBlock, null, new List<Type>() { typeof(NWDAccountAvatar) }, false, true, NWDOperationSpecial.None);
            while (NWDUnitTests.WebServiceIsRunning(tOperationNameB))
            {
                yield return null;
            }
            Debug.Log("@@@@@@@@ Data is Sync");
            // delete Data
            tAvatar.DeleteData();
            // reload data from base
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            while (NWDDataManager.SharedInstance().DataIndexed == false)
            {
                yield return null;
            }
            // try get data with no success
            NWDAccountAvatar tAvatarTest = NWDBasisHelper.GetEditorDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreEqual(null,tAvatarTest);
            // Sync Force
            string tOperationNameC = "download avatar data" + NWDToolbox.RandomStringUnix(16);
            NWDUnitTests.NewWebService(tOperationNameC, true);
            NWDOperationWebSynchronisation.AddOperation(tOperationNameC, NWDUnitTests.kSuccess, NWDUnitTests.kFailBlock, NWDUnitTests.kCancelBlock, NWDUnitTests.kProgressBlock, null, new List<Type>() { typeof(NWDAccountAvatar) }, true, true, NWDOperationSpecial.None);
            while (NWDUnitTests.WebServiceIsRunning(tOperationNameC))
            {
                yield return null;
            }
            // try get data with success
            NWDAccountAvatar tAvatarTestB = NWDBasisHelper.GetEditorDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreNotEqual(tAvatarTestB, null);
        }
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator TemporaryNewDataSync()
        {
            Debug.Log("TemporarySync() Reset account");
            NWDUnitTests.ResetDevice();
            NWDUnitTests.TemporaryAccount();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            Debug.Log("@@@@@@@@ account before" + tEnvironment.PlayerAccountReference);
            // Sync
            string tOperationName = "Temporary to cert_" + NWDToolbox.RandomStringUnix(16);
            NWDUnitTests.NewWebService(tOperationName, false);
            NWDOperationWebSynchronisation.AddOperation(tOperationName, NWDUnitTests.kSuccess, NWDUnitTests.kFailBlock, NWDUnitTests.kCancelBlock, NWDUnitTests.kProgressBlock, null, null, false, true, NWDOperationSpecial.None);
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
            // Ok create Data And Sync it
            NWDAccountAvatar tAvatar = NWDBasisHelper.NewData<NWDAccountAvatar>();
            // Save Data !
            //tAvatar.UpdateData();
            // Memorise Reference of Data
            string tReference = tAvatar.Reference + string.Empty;
            // Sync
            string tOperationNameB = "upload avatar data" + NWDToolbox.RandomStringUnix(16);
            NWDUnitTests.NewWebService(tOperationNameB, true);
            NWDOperationWebSynchronisation.AddOperation(tOperationNameB, NWDUnitTests.kSuccess, NWDUnitTests.kFailBlock, NWDUnitTests.kCancelBlock, NWDUnitTests.kProgressBlock, null, new List<Type>() { typeof(NWDAccountAvatar) }, false, true, NWDOperationSpecial.None);
            while (NWDUnitTests.WebServiceIsRunning(tOperationNameB))
            {
                yield return null;
            }
            Debug.Log("@@@@@@@@ Data is Sync");
            // delete Data
            tAvatar.DeleteData();
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
            string tOperationNameC = "download avatar data" + NWDToolbox.RandomStringUnix(16);
            NWDUnitTests.NewWebService(tOperationNameC, true);
            NWDOperationWebSynchronisation.AddOperation(tOperationNameC, NWDUnitTests.kSuccess, NWDUnitTests.kFailBlock, NWDUnitTests.kCancelBlock, NWDUnitTests.kProgressBlock, null, new List<Type>() { typeof(NWDAccountAvatar) }, true, true, NWDOperationSpecial.None);
            while (NWDUnitTests.WebServiceIsRunning(tOperationNameC))
            {
                yield return null;
            }
            // try get data with success
            NWDAccountAvatar tAvatarTestB = NWDBasisHelper.GetEditorDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreNotEqual(tAvatarTestB, null);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================

