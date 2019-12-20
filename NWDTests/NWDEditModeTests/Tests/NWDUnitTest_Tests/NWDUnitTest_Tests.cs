//=====================================================================================================================
//
//  ideMobi copyright 2019. All rights reserved by ideMobi.
//
//=====================================================================================================================
#if UNITY_INCLUDE_TESTS
using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NetWorkedData;
//=====================================================================================================================
namespace NWDEditorTests
{
    public partial class NWDUnitTest_Tests
    {
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void TestNewData_NotClean()
        {
            NWDEditorModelTest tItemA = NWDBasisHelper.NewData<NWDEditorModelTest>();
            Assert.AreEqual(tItemA.Tag, NWDBasisTag.UnitTestToDelete);
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void TestNewLocalData_NotClean()
        {
            NWDEditorModelTest tItemA = NWDUnitTests.NewLocalData<NWDEditorModelTest>();
            Assert.AreEqual(tItemA.Tag, NWDBasisTag.UnitTestToDelete);
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void TestNewData()
        {
            NWDEditorModelTest tItemA = NWDBasisHelper.NewData<NWDEditorModelTest>();
            Assert.AreEqual(tItemA.Tag, NWDBasisTag.UnitTestToDelete);
            NWDUnitTests.CleanUnitTests();
            NWDEditorModelTest tItemB = NWDBasisHelper.GetRawDataByReference<NWDEditorModelTest>(tItemA.Reference);
            Assert.AreEqual(null, tItemB);
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void TestNewLocalData()
        {
            NWDEditorModelTest tItemA = NWDUnitTests.NewLocalData<NWDEditorModelTest>();
            Assert.AreEqual(tItemA.Tag, NWDBasisTag.UnitTestToDelete);
            NWDUnitTests.CleanUnitTests();
            NWDEditorModelTest tItemB = NWDBasisHelper.GetRawDataByReference<NWDEditorModelTest>(tItemA.Reference);
            Assert.AreEqual(null, tItemB);
        }
        //-------------------------------------------------------------------------------------------------------------
        string kPermanentDataReference = "123456789";
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void TestPermanentData()
        {
            NWDEditorModelTest tItemA = NWDUnitTests.PermanentData<NWDEditorModelTest>("unit test", kPermanentDataReference);
            Assert.AreEqual(tItemA.Tag, NWDBasisTag.UnitTestNotDelete);
            NWDUnitTests.CleanUnitTests();
            NWDEditorModelTest tItemB = NWDBasisHelper.GetRawDataByReference<NWDEditorModelTest>(kPermanentDataReference);
            Assert.AreNotEqual(null, tItemB);
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void TestLocalDataDupplicate()
        {
            NWDEditorModelTest tItemA = NWDUnitTests.NewLocalData<NWDEditorModelTest>();
            tItemA.UpdateData();
            NWDEditorModelTest tItemB = NWDBasisHelper.DuplicateData(tItemA, false);
            NWDUnitTests.SetUnitTestData(tItemB);
            Assert.AreNotEqual(tItemA.Reference, tItemB.Reference);
            NWDUnitTests.CleanUnitTests();
            NWDEditorModelTest tItemC = NWDBasisHelper.GetRawDataByReference<NWDEditorModelTest>(tItemA.Reference);
            Assert.AreEqual(null, tItemC);
            NWDEditorModelTest tItemD = NWDBasisHelper.GetRawDataByReference<NWDEditorModelTest>(tItemB.Reference);
            Assert.AreEqual(null, tItemD);
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void TestDataDupplicate()
        {
            NWDEditorModelTest tItemA = NWDBasisHelper.NewData<NWDEditorModelTest>();
            tItemA.UpdateData();
            NWDEditorModelTest tItemB = NWDBasisHelper.DuplicateData(tItemA, false);
            NWDUnitTests.SetUnitTestData(tItemB);
            Assert.AreNotEqual(tItemA.Reference, tItemB.Reference);
            NWDUnitTests.CleanUnitTests();
            NWDEditorModelTest tItemC = NWDBasisHelper.GetRawDataByReference<NWDEditorModelTest>(tItemA.Reference);
            Assert.AreEqual(null, tItemC);
            NWDEditorModelTest tItemD = NWDBasisHelper.GetRawDataByReference<NWDEditorModelTest>(tItemB.Reference);
            Assert.AreEqual(null, tItemD);
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void TestFakeDevice()
        {
            string SecretEditor = NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDeviceEditor() + string.Empty;
            string SecretPlayMode = NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevicePlayer() + string.Empty;

            NWDUnitTests.EnableFakeDevice();

            Assert.AreNotEqual(SecretEditor, NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDeviceEditor());
            Assert.AreNotEqual(SecretPlayMode, NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevicePlayer());

            NWDUnitTests.DisableFakeDevice();

            Assert.AreEqual(SecretEditor, NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDeviceEditor());
            Assert.AreEqual(SecretPlayMode, NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevicePlayer());
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void TestRestaureFakeDevice()
        {
            NWDUnitTests.ShowFakeDevice();

            string SecretEditor = NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDeviceEditor() + string.Empty;
            string SecretPlayMode = NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevicePlayer() + string.Empty;

            NWDUnitTests.EnableFakeDevice();
            NWDUnitTests.ShowFakeDevice();

            string SecretEditor_A = NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDeviceEditor() + string.Empty;
            string SecretPlayMode_A = NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevicePlayer() + string.Empty;

            Assert.AreNotEqual(SecretEditor, SecretEditor_A);
            Assert.AreNotEqual(SecretPlayMode, SecretPlayMode_A);

            NWDUnitTests.MemoryFakeDevice("A");

            NWDUnitTests.ResetFakeDevice();

            string SecretEditor_B = NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDeviceEditor() + string.Empty;
            string SecretPlayMode_B = NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevicePlayer() + string.Empty;

            Assert.AreNotEqual(SecretEditor_A, SecretEditor_B);
            Assert.AreNotEqual(SecretPlayMode_A, SecretPlayMode_B);
            Assert.AreNotEqual(SecretEditor, SecretEditor_B);
            Assert.AreNotEqual(SecretPlayMode, SecretPlayMode_B);

            NWDUnitTests.RestaureFakeDevice("A");
            NWDUnitTests.ShowFakeDevice();

            Assert.AreEqual(SecretEditor_A, NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDeviceEditor());
            Assert.AreEqual(SecretPlayMode_A, NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevicePlayer());

            NWDUnitTests.ResetFakeDevice();

            string SecretEditor_C = NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDeviceEditor() + string.Empty;
            string SecretPlayMode_C = NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevicePlayer() + string.Empty;

            Assert.AreNotEqual(SecretEditor_A, SecretEditor_C);
            Assert.AreNotEqual(SecretPlayMode_A, SecretPlayMode_C);
            Assert.AreNotEqual(SecretEditor_B, SecretEditor_C);
            Assert.AreNotEqual(SecretPlayMode_B, SecretPlayMode_C);
            Assert.AreNotEqual(SecretEditor, SecretEditor_C);
            Assert.AreNotEqual(SecretPlayMode, SecretPlayMode_C);

            NWDUnitTests.DisableFakeDevice();
            NWDUnitTests.ShowFakeDevice();

            Assert.AreEqual(SecretEditor, NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDeviceEditor());
            Assert.AreEqual(SecretPlayMode, NWDAppConfiguration.SharedInstance().SelectedEnvironment().SecretKeyDevicePlayer());

        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif