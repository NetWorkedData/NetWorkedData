//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
        const string kPlayerAccountReferenceKey = "kPlayerAccountReference_Key";
        public const string kRequesTokenKey = "kRequesToken_Key";
        const string kAnonymousPlayerAccountReferenceKey = "kAnonymousPlayerAccountReference_Key";
        const string kAnonymousResetPasswordKey = "kAnonymousResetPassword_Key";
        const string kPlayerStatusKey = "kPlayerStatus_Key";
        //-------------------------------------------------------------------------------------------------------------
        public void SavePreferences()
        {
            //if (NWDLauncher.ActiveBenchmark)
            //{
            //NWEBenchmark.Start();
            //}
            if (string.IsNullOrEmpty(GetAccountReference()))
            {
                SetAccountReference(NWDToolbox.GenerateUniqueAccountID(this, true));
            }
            else
            {
                NWEPrefsManager.ShareInstance().set(Environment + kPlayerAccountReferenceKey, GetAccountReference());
                NWDBasisPreferences.SetString(kRequesTokenKey, this, RequesToken, false);
                NWDDataManager.SharedInstance().DataQueueExecute();
            }
            //if (NWDLauncher.ActiveBenchmark)
            //{
            //NWEBenchmark.Finish(true, "RequesToken = " + RequesToken);
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
        public void LoadPreferences()
        {
            //if (NWDLauncher.ActiveBenchmark)
            //{
            //    NWEBenchmark.Start();
            //}
            SetAccountReference(NWEPrefsManager.ShareInstance().getString(Environment + kPlayerAccountReferenceKey));
            RequesToken = NWDBasisPreferences.GetString(kRequesTokenKey, this, string.Empty, false);
            if (string.IsNullOrEmpty(GetAccountReference()))
            {
                SetAccountReference(NWDToolbox.GenerateUniqueAccountID(this, true));
                RequesToken = string.Empty;
                SavePreferences();
            }
            //if (NWDLauncher.ActiveBenchmark)
            //{
            //    NWEBenchmark.Finish(true, "RequesToken = " + RequesToken);
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetPreferences(bool withTemporaryAccount = true, string sWithSign = null)
        {
            //if (NWDLauncher.ActiveBenchmark)
            //{
            //    NWEBenchmark.Start();
            //}
            SavePreferences();
            NWDDataManager.SharedInstance().DataQueueExecute();
            SetAccountReference(NWDToolbox.GenerateUniqueAccountID(this, withTemporaryAccount));
            RequesToken = string.Empty;
            SavePreferences();
            if (withTemporaryAccount == false)
            {
                // create new account
                if (string.IsNullOrEmpty(sWithSign) == false)
                {
                    WithSpecialSDKI = sWithSign;
                }
                else
                {
                    WithSpecialSDKI = string.Empty;
                }
            }
            // add notification
            NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_ACCOUNT_CHANGE, null));
            NWDDataManager.SharedInstance().AccountLanguageLoad();
#if UNITY_EDITOR
            NWDAppEnvironmentChooser.Refresh();
#endif
            //if (NWDLauncher.ActiveBenchmark)
            //{
            //    NWEBenchmark.Finish(true, "RequesToken = " + RequesToken);
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================