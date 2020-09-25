//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
//=====================================================================================================================
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//=====================================================================================================================
#if UNITY_EDITOR
using NetWorkedData.NWDEditor;
#endif
//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
        const string kPlayerAccountReferenceKey = "kPlayerAccountReference_Key";
        const string kPlayerAccountSaltKey = "kPlayerAccountSalt_Key";
        public const string kRequesTokenKey = "kRequesToken_Key";
        //const string kAnonymousPlayerAccountReferenceKey = "kAnonymousPlayerAccountReference_Key";
        //const string kAnonymousResetPasswordKey = "kAnonymousResetPassword_Key";
        //const string kPlayerStatusKey = "kPlayerStatus_Key";
        //-------------------------------------------------------------------------------------------------------------
        public void SavePreferences()
        {
            NWDBenchmarkLauncher.Start();
            if (string.IsNullOrEmpty(GetAccountSalt()))
            {
                SetAccountSalt(NWDToolbox.RandomStringAlpha(NWDAccount.K_PERSONAL_SALT_LENGHT));
            }
            if (string.IsNullOrEmpty(GetAccountReference()))
            {
                SetAccountReference(NWDToolbox.GenerateUniqueAccountID(this, true));
            }
            else
            {
                NWDRuntimePrefs.ShareInstance().set(Environment + kPlayerAccountReferenceKey, GetAccountReference());
                NWDRuntimePrefs.ShareInstance().set(Environment + kPlayerAccountSaltKey, GetAccountSalt());
                NWDBasisPreferences.SetString(kRequesTokenKey, this, RequesToken, false);
                NWDDataManager.SharedInstance().DataQueueExecute();
            }
            NWDBenchmarkLauncher.Finish(true, "RequesToken = " + RequesToken);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void LoadPreferences()
        {
            NWDBenchmarkLauncher.Start();
            SetAccountReference(NWDRuntimePrefs.ShareInstance().getString(Environment + kPlayerAccountReferenceKey));
            SetAccountSalt(NWDRuntimePrefs.ShareInstance().getString(Environment + kPlayerAccountSaltKey));
            RequesToken = NWDBasisPreferences.GetString(kRequesTokenKey, this, string.Empty, false);
            if (string.IsNullOrEmpty(GetAccountSalt()))
            {
                SetAccountSalt(NWDToolbox.RandomStringAlpha(NWDAccount.K_PERSONAL_SALT_LENGHT));
            }
            if (string.IsNullOrEmpty(GetAccountReference()))
            {
                SetAccountReference(NWDToolbox.GenerateUniqueAccountID(this, true));
                RequesToken = string.Empty;
                SavePreferences();
            }
            NWDBenchmarkLauncher.Finish(true, "RequesToken = " + RequesToken);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetPreferences(bool withTemporaryAccount = true, string sWithSign = null)
        {
            NWDBenchmarkLauncher.Start();
            SavePreferences();
            NWDDataManager.SharedInstance().DataQueueExecute();
            SetAccountReference(NWDToolbox.GenerateUniqueAccountID(this, withTemporaryAccount));
            SetAccountSalt(NWDToolbox.RandomStringAlpha(NWDAccount.K_PERSONAL_SALT_LENGHT));
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
            NWDBenchmarkLauncher.Finish(true, "RequesToken = " + RequesToken);
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================
