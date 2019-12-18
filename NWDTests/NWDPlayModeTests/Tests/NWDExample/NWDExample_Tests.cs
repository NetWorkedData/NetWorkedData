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
namespace NWDPlayModeTests
{
    public partial class NWDExample_Tests
    {
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void TestNewData()
        {
            NWDExample tItemA = NWDUnitTests.NewData<NWDExample>();
            NWDExample tItemB = NWDUnitTests.NewData<NWDExample>();
            Assert.AreNotEqual(tItemA.Reference, tItemB.Reference);
            NWDUnitTests.CleanUnitTests();
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void TestDataDupplicate()
        {
            NWDExample tItemA = NWDUnitTests.NewData<NWDExample>();
            tItemA.UpdateData();
            NWDExample tItemB = NWDBasisHelper.DuplicateData(tItemA, false);
            NWDUnitTests.SetUnitTestData(tItemB);

            Assert.AreNotEqual(tItemA.Reference, tItemB.Reference);
            NWDUnitTests.CleanUnitTests();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif