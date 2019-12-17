using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

using NetWorkedData;

namespace NWDPlayModeSyncTests
{
    public class NWDSyncData
	{
        [UnityTest]
        public IEnumerator DataSync()
        {
            bool tFinish = false;
            NWDAppEnvironment tEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();

            //bool tLog = tEnvironment.LogInFileMode;
            //tEnvironment.LogInFileMode = false;
            //tEnvironment.LogMode = true;

            //NWDDataManager.SharedInstance().WebOperationQueue.Flush(tEnvironment.Environment);
           // tEnvironment.ResetPreferences();
            NWEOperationBlock tSuccess = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
            {
                tFinish = true;
            };
            NWEOperationBlock tFail = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
            {
                tFinish = true;
            };
            NWEOperationBlock tCancel = delegate (NWEOperation bOperation, float bProgress, NWEOperationResult bResult)
            {
                tFinish = true;
            };

            Debug.Log("Get certified account");
            tFinish = false;
            NWDOperationWebSynchronisation.AddOperation("Account Sync", tSuccess, tFail, tCancel, null, null, null, true, true, NWDOperationSpecial.None);
            while (tFinish == false)
            {
                yield return null;
            }
            /*
            string tAccountC = tEnvironment.PlayerAccountReference + string.Empty;
            NWDAccountAvatar tAvatar = new NWDAccountAvatar();
            int tSync = 0;
            int tSyncSerevr = 0;
            if (tEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                tSync = tAvatar.DevSync;
            }
            else if (tEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                tSync = tAvatar.PreprodSync;
            }
            else if (tEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                tSync = tAvatar.ProdSync;
            }

            Debug.Log("Sync data");
            tFinish = false;
            NWDOperationWebSynchronisation.AddOperation("SyncData", tSuccess, tFail, tCancel, null, null, null, true, true, NWDOperationSpecial.None);
            while (tFinish == false)
            {
                yield return null;
            }
            if (tEnvironment == NWDAppConfiguration.SharedInstance().DevEnvironment)
            {
                tSyncSerevr = tAvatar.DevSync;
            }
            else if (tEnvironment == NWDAppConfiguration.SharedInstance().PreprodEnvironment)
            {
                tSyncSerevr = tAvatar.PreprodSync;
            }
            else if (tEnvironment == NWDAppConfiguration.SharedInstance().ProdEnvironment)
            {
                tSyncSerevr = tAvatar.ProdSync;
            }
            Assert.AreNotEqual(tSyncSerevr, tSync);
            */

            //tEnvironment.LogInFileMode = tLog;
        }

    }
}
