
//
//  Copyright 2017  Kortex
//
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
//
//        http://www.apache.org/licenses/LICENSE-2.0
//
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

// Convention of layout for all our unity project!

//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityEngine.TestTools;
using NUnit.Framework;
using System.Collections;
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWDReferencesArrayTypeTest
    {
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void NWDReferencesArrayTypeTestSimplePasses()
        {
            // Use the Assert class to test conditions.
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void ContainsReferenceTest()
        {
            // Use the Assert class to test conditions.
            NWDReferencesArrayType<NWDItem> tTestArray = new NWDReferencesArrayType<NWDItem>();
            tTestArray.Value = "A";
            Assert.IsTrue(tTestArray.ContainsReference("A"));
            Assert.IsFalse(tTestArray.ContainsReference("B"));
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void SetReferencesTest()
        {
            // Use the Assert class to test conditions.
            NWDReferencesArrayType<NWDItem> tTestArray = new NWDReferencesArrayType<NWDItem>();
            tTestArray.SetReferences(new string[] { "A", "C" });
            Assert.IsTrue(tTestArray.ContainsReference("A"));
            Assert.IsTrue(tTestArray.ContainsReference("C"));
            Assert.IsFalse(tTestArray.ContainsReference("B"));
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void AddReferences()
        {
            // Use the Assert class to test conditions.
            NWDReferencesArrayType<NWDItem> tTestArray = new NWDReferencesArrayType<NWDItem>();
            tTestArray.SetReferences(new string[] { "A", "C" });
            tTestArray.AddReferences(new string[] { "D", "E" });
            tTestArray.AddReferences(new string[] { "A", "A" });
            NWDReferencesArrayType<NWDItem> tTestArrayExpected = new NWDReferencesArrayType<NWDItem>();
            tTestArrayExpected.SetReferences(new string[] { "A", "C", "D", "E", "A", "A" });

            Debug.Log("tTestArray.Value = " + tTestArray.Value);
            Debug.Log("tTestArrayExpected.Value = " + tTestArrayExpected.Value);
            Assert.AreEqual(tTestArray.Value, tTestArrayExpected.Value);

            Debug.Log("tTestArray.ToStringSorted () = " + tTestArray.ToStringSorted());
            Debug.Log("tTestArrayExpected.ToStringSorted () = " + tTestArrayExpected.ToStringSorted());
            Assert.AreEqual(tTestArray.ToStringSorted(), tTestArrayExpected.ToStringSorted());

        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void AddReferencesArrayTest()
        {
            // Use the Assert class to test conditions.
            NWDReferencesArrayType<NWDItem> tTestArray = new NWDReferencesArrayType<NWDItem>();
            tTestArray.SetReferences(new string[] { "A", "A", "C" });

            NWDReferencesArrayType<NWDItem> tTestArrayAddable = new NWDReferencesArrayType<NWDItem>();
            tTestArrayAddable.SetReferences(new string[] { "B", "A" });

            tTestArray.AddReferencesArray(tTestArrayAddable);

            NWDReferencesArrayType<NWDItem> tTestArrayExpected = new NWDReferencesArrayType<NWDItem>();
            tTestArrayExpected.SetReferences(new string[] { "A", "A", "A", "B", "C" });

            Debug.Log("tTestArray.Value = " + tTestArray.Value);
            Debug.Log("tTestArrayExpected.Value = " + tTestArrayExpected.Value);
            Assert.AreNotEqual(tTestArray.Value, tTestArrayExpected.Value);

            Debug.Log("tTestArray.ToStringSorted () = " + tTestArray.ToStringSorted());
            Debug.Log("tTestArrayExpected.ToStringSorted () = " + tTestArrayExpected.ToStringSorted());
            Assert.AreEqual(tTestArray.ToStringSorted(), tTestArrayExpected.ToStringSorted());
        }
        //-------------------------------------------------------------------------------------------------------------
        [Test]
        public void RemoveReferencesArrayTest()
        {
            // Use the Assert class to test conditions.
            NWDReferencesArrayType<NWDItem> tTestArray = new NWDReferencesArrayType<NWDItem>();
            tTestArray.SetReferences(new string[] { "A", "B", "C", "A", "A" });

            NWDReferencesArrayType<NWDItem> tTestArrayRemovable = new NWDReferencesArrayType<NWDItem>();
            tTestArrayRemovable.SetReferences(new string[] { "A", "C" });

            tTestArray.RemoveReferencesArray(tTestArrayRemovable);

            NWDReferencesArrayType<NWDItem> tTestArrayExpected = new NWDReferencesArrayType<NWDItem>();
            tTestArrayExpected.SetReferences(new string[] { "A", "A", "B" });

            Debug.Log("tTestArray.Value = " + tTestArray.Value);
            Debug.Log("tTestArrayExpected.Value = " + tTestArrayExpected.Value);
            Assert.AreNotEqual(tTestArray.Value, tTestArrayExpected.Value);

            Debug.Log("tTestArray.ToStringSorted () = " + tTestArray.ToStringSorted());
            Debug.Log("tTestArrayExpected.ToStringSorted () = " + tTestArrayExpected.ToStringSorted());
            Assert.AreEqual(tTestArray.ToStringSorted(), tTestArrayExpected.ToStringSorted());
        }
        //-------------------------------------------------------------------------------------------------------------
        // A UnityTest behaves like a coroutine in PlayMode
        // and allows you to yield null to skip a frame in EditMode
        [UnityTest]
        public IEnumerator NWDReferencesArrayTypeTestWithEnumeratorPasses()
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