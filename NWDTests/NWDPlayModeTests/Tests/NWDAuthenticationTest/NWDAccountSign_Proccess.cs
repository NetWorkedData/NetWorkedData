//=====================================================================================================================
//
//  ideMobi copyright 2019
//
//  Date        2019-18-12 9:20:00
//  Author      Dolwen (Jérôme DEMYTTENAERE) 
//  Email       jerome.demyttenaere@gmail.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_INCLUDE_TESTS
using System.Collections;
using NUnit.Framework;
using UnityEngine.TestTools;
using NetWorkedData;

//=====================================================================================================================
namespace NWDPlayModeProccess
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountSign_Proccess
    {
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator CreateAndRegisterSocialNetwork()
        {
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            NWDUnitTests.EnableFakeDevice();
            
            // create account
            NWDUnitTests.LogNewTest();
            //NWDUnitTests.ResetFakeDevice();
            NWDUnitTests.UseTemporaryAccount();
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            string tAccountT = tEnvironment.PlayerAccountReference + string.Empty;
            
            // Sync
            NWDOperationWebSynchronisation tOperation = NWDOperationWebSynchronisation.AddOperation(null, null, null, null, null, null, null, true, false, NWDOperationSpecial.None);
            while (!tOperation.IsFinish)
            {
                yield return null;
            }

            string tAccountC = tEnvironment.PlayerAccountReference + string.Empty;

            NWDUnitTests.DisableFakeDevice();

            // Test result
            Assert.AreNotEqual(tAccountC, tAccountT);
            string tT = tAccountT.Substring(tAccountT.Length - 1);
            Assert.AreEqual(tT, NWDAccount.K_ACCOUNT_TEMPORARY_SUFFIXE);
            string tC = tAccountC.Substring(tAccountC.Length - 1);
            Assert.AreEqual(tC, NWDAccount.K_ACCOUNT_CERTIFIED_SUFFIXE);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////

            /////////////////////////////////////////////////////////////////////////////////////////////////////////
            NWDUnitTests.EnableFakeDevice();

            NWDUnitTests.LogStep("CreateAndRegisterSocialNetwork");
            bool IsFinish = false;
            NWEOperationBlock tFinishBlock = delegate(NWEOperation sOperation, float sProgress, NWEOperationResult sInfos)
            {
                IsFinish = true;
            };

            // Create and add social sign
            string tSign = NWDToolbox.RandomStringCypher(32);

            // Create and Sync
            NWDAccountSign.CreateAndRegisterSocialNetwork(tSign, NWDAccountSignType.Fake, tFinishBlock, tFinishBlock);
            while (!IsFinish)
            {
                yield return null;
            }

            // Get Account Sign by Social type
            NWDAccountSign tAccountSign = NWDAccountSign.GetFirstReacheableAccountSign(NWDAccountSignType.Fake);
            if (tAccountSign != null)
            {
                NWDUnitTests.Log("NWDAccountSign found!");
                NWDUnitTests.Log("AccountSign Reference =" + tAccountSign.Reference);
                NWDUnitTests.Log("AccountSign SignStatus =" + tAccountSign.SignStatus.ToString());
                NWDUnitTests.Log("AccountSign DevSync =" + tAccountSign.DevSync.ToString());

                // Test result
                Assert.AreEqual(NWDAccountSignAction.Associated, tAccountSign.SignStatus);
            }
            else
            {
                Assert.Fail("NWDAccountSign not found!");
            }

            NWDUnitTests.DisableFakeDevice();
            /////////////////////////////////////////////////////////////////////////////////////////////////////////
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif