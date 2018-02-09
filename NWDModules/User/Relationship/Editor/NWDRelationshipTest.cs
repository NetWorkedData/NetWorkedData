#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
using System;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++


    public class NWDRelationshipTest
    {
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void CreateRelationShipTest()
        {
            // Use the Assert class to test conditions.
            NWDRelationship tCreated = NWDRelationship.CreateNewRelationship(new Type[] { typeof(NWDOwnership)});
            Assert.IsNotNull(tCreated);
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void CreateRelationShipDefaultTest()
        {
            // Use the Assert class to test conditions.
            NWDRelationship tCreated = NWDRelationship.CreateNewRelationshipDefault(new Type[] { typeof(NWDOwnership) });
            Assert.IsNotNull(tCreated);
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void CreateRelationAndAskPincodeShipDefaultTest()
        {
            // Use the Assert class to test conditions.
            NWDRelationship tCreated = NWDRelationship.CreateNewRelationshipDefault(new Type[] { typeof(NWDOwnership) });
            tCreated.AskPinCodeFromServer(new DateTime());
            Assert.IsNotNull(tCreated);
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void NWDRelationshipTestSimplePasses()
        {
            // Use the Assert class to test conditions.
        }
        //-------------------------------------------------------------------------------------------------------------
        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator NWDRelationshipTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // yield to skip a frame
            yield return null;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif