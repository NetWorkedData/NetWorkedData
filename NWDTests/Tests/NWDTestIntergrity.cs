using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using NetWorkedData;

//=====================================================================================================================
namespace NetWorkedDataTests
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDTestIntegrity
    {
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void NWDTestIntegritySimpleCheck()
        {
            // create Data
            NWDExample tExample = NWDExample.NewData();
            // test
            string tIntegrityA = "" + tExample.Integrity;
            tExample.UpdateIntegrity();
            string tIntegrityB = "" + tExample.Integrity;
            // Use the Assert class to test conditions
            // Test 
            Assert.AreEqual(tIntegrityA, tIntegrityB);

            // clean base 
            tExample.DeleteData();
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void NWDTestIntegrityUpdateCheck()
        {
            // create Data
            NWDExample tExample = NWDExample.NewData();

            // test
            string tIntegrityA = "" + tExample.Integrity;
            tExample.InternalKey = "test";
            tExample.UpdateData();
            string tIntegrityB = "" + tExample.Integrity;
            // Use the Assert class to test conditions
            // Test 
            Assert.AreNotEqual(tIntegrityA, tIntegrityB);

            // clean base 
            tExample.DeleteData();
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void NWDTestIntegrityUpdateIfModifiedCheck()
        {
            // create Data
            NWDExample tExample = NWDExample.NewData();

            // test
            string tIntegrityA = "" + tExample.Integrity;
            tExample.SaveDataIfModified();
            string tIntegrityB = "" + tExample.Integrity;
            // Use the Assert class to test conditions
            // Test 
            Assert.AreEqual(tIntegrityA, tIntegrityB);

            tExample.InternalKey = "test";
            tExample.SaveDataIfModified();
            string tIntegrityC = "" + tExample.Integrity;
            // Use the Assert class to test conditions
            // Test 
            Assert.AreNotEqual(tIntegrityA, tIntegrityC);

            // clean base 
            tExample.DeleteData();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
