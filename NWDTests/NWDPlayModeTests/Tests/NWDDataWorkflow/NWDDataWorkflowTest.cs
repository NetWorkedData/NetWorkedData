
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
    public class NWDDataWorkflowTest
    {
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator BasisHelperNew()
        {
            Debug.Log("BasisHelperNew()");
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            NWDAccountAvatar tAvatar = new NWDAccountAvatar();
            // Save Data !
            tAvatar.UpdateData();
            // force to wrtie in database
            NWDDataManager.SharedInstance().DataQueueExecute();
            // Memorise Reference of Data
            string tReference = tAvatar.Reference + string.Empty;
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            NWDAccountAvatar tAvatarTest = NWDBasisHelper.GetRawDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreNotEqual(null, tAvatarTest);
        }
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator BasisHelperNewWithoutReccord()
        {
            Debug.Log("BasisHelperNew()");
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            NWDAccountAvatar tAvatar = new NWDAccountAvatar();
            // force to wrtie in database
            NWDDataManager.SharedInstance().DataQueueExecute();
            // Memorise Reference of Data
            string tReference = tAvatar.Reference + string.Empty;
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            NWDAccountAvatar tAvatarTest = NWDBasisHelper.GetRawDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreNotEqual(null, tAvatarTest);
        }
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator BasisHelperNewData()
        {
            Debug.Log("BasisHelperNewData()");
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            NWDAccountAvatar tAvatar = NWDBasisHelper.NewData<NWDAccountAvatar>();
            // Save Data !
            tAvatar.UpdateData();
            // force to wrtie in database
            NWDDataManager.SharedInstance().DataQueueExecute();
            // Memorise Reference of Data
            string tReference = tAvatar.Reference + string.Empty;
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            NWDAccountAvatar tAvatarTest = NWDBasisHelper.GetRawDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreNotEqual(null,tAvatarTest);
        }
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator BasisHelperNewAndDeleteData()
        {
            Debug.Log("BasisHelperNewData()");
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            NWDAccountAvatar tAvatar = new NWDAccountAvatar();
            string tReference = tAvatar.Reference + string.Empty;
            // Save Data !
            tAvatar.UpdateData();
            // force to write in database
            NWDDataManager.SharedInstance().DataQueueExecute();
            // Memorise Reference of Data
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            NWDAccountAvatar tAvatarTest = NWDBasisHelper.GetRawDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreNotEqual(null, tAvatarTest);
            tAvatarTest.DeleteData();
            // force to write in database
            NWDDataManager.SharedInstance().DataQueueExecute();
            // Memorise Reference of Data
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            NWDAccountAvatar tAvatarTestDeleted = NWDBasisHelper.GetRawDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreEqual(null, tAvatarTestDeleted);
        }
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator BasisHelperNewDataAndDeleteData()
        {
            Debug.Log("BasisHelperNewData()");
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            NWDAccountAvatar tAvatar = NWDBasisHelper.NewData<NWDAccountAvatar>();
            string tReference = tAvatar.Reference + string.Empty;
            // Save Data !
            tAvatar.UpdateData();
            // force to write in database
            NWDDataManager.SharedInstance().DataQueueExecute();
            // Memorise Reference of Data
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            NWDAccountAvatar tAvatarTest = NWDBasisHelper.GetRawDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreNotEqual(null, tAvatarTest);
            tAvatarTest.DeleteData();
            // force to write in database
            NWDDataManager.SharedInstance().DataQueueExecute();
            // Memorise Reference of Data
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            NWDAccountAvatar tAvatarTestDeleted = NWDBasisHelper.GetRawDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreEqual(null, tAvatarTestDeleted);
        }
        //-------------------------------------------------------------------------------------------------------------
        [UnityTest]
        public IEnumerator BasisHelperUpdateTestOne()
        {
            Debug.Log("BasisHelperNewData()");
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            // Create new Data
            NWDAccountAvatar tAvatar = NWDBasisHelper.NewData<NWDAccountAvatar>();
            // Memorise Reference of Data
            string tReference = tAvatar.Reference + string.Empty;
            // Create new description
            string tDescriptionTest = NWDToolbox.RandomStringUnix(32); 
            tAvatar.InternalDescription = tDescriptionTest;
            // Save Data !
            tAvatar.UpdateData();
            // force to write in database
            NWDDataManager.SharedInstance().DataQueueExecute();
            // reload datas
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            // test description was reccorded
            NWDAccountAvatar tAvatarTest = NWDBasisHelper.GetRawDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreEqual(tDescriptionTest, tAvatarTest.InternalDescription);
            // create new description
            string tDescriptionTestB = NWDToolbox.RandomStringUnix(32);
            // assign but not reccord
            tAvatarTest.InternalDescription = tDescriptionTestB;
            // force to write in database the other modification
            NWDDataManager.SharedInstance().DataQueueExecute();
            // reccord other of Data
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            NWDAccountAvatar tAvatarTestNotModified = NWDBasisHelper.GetRawDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreNotEqual(tDescriptionTestB, tAvatarTestNotModified.InternalDescription);
            // assign and save
            tAvatarTestNotModified.InternalDescription = tDescriptionTestB;
            // Save Data !
            tAvatar.UpdateData();
            // force to write in database
            NWDDataManager.SharedInstance().DataQueueExecute();
            // Memorise Reference of Data
            NWDDataManager.SharedInstance().ReloadAllObjectsAccount();
            while (NWDDataManager.SharedInstance().DatasAreNotReady())
            {
                yield return null;
            }
            NWDAccountAvatar tAvatarTestModified = NWDBasisHelper.GetRawDataByReference<NWDAccountAvatar>(tReference);
            Assert.AreNotEqual(tDescriptionTestB, tAvatarTestModified.InternalDescription);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
#endif