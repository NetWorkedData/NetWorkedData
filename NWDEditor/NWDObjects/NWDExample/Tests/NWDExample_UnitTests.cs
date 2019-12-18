//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:22:47
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using NetWorkedData;

//=====================================================================================================================
namespace NWDEditorTests
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDExample_UnitTests
    {
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void Test_NewData()
        {
            NWDUnitTests.CleanUnitTests(); // clean environment before
            NWDExample tItemA = NWDUnitTests.NewData<NWDExample>();
            NWDExample tItemB = NWDUnitTests.NewData<NWDExample>();
            Assert.AreNotEqual(tItemA.Reference, tItemB.Reference);
            NWDUnitTests.CleanUnitTests(); // clean environment after
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
		public void Test_Duplicate()
        {
            NWDUnitTests.CleanUnitTests(); // clean environment before
            NWDExample tItemA = NWDUnitTests.NewData<NWDExample>();
			tItemA.UpdateData();
            NWDExample tItemB = NWDUnitTests.DuplicateData(tItemA);
			Assert.AreNotEqual(tItemA.Reference, tItemB.Reference);
			NWDUnitTests.CleanUnitTests(); // clean environment after
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
