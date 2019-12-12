//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:10
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using NUnit.Framework;
using NetWorkedData;

//=====================================================================================================================
namespace NetWorkedDataTests
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