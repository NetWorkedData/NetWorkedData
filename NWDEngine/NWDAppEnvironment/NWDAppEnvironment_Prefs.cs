﻿//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:24:48
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
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
            if (string.IsNullOrEmpty(PlayerAccountReference))
            {
                PlayerAccountReference = NWDToolbox.GenerateUniqueID();
            }
            else
            {
                NWEPrefsManager.ShareInstance().set(Environment + kPlayerAccountReferenceKey, PlayerAccountReference);
                NWDBasisPreferences.SetString(kRequesTokenKey, this, RequesToken, false);
                NWDDataManager.SharedInstance().DataQueueExecute();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void LoadPreferences()
        {
            PlayerAccountReference = NWEPrefsManager.ShareInstance().getString(Environment + kPlayerAccountReferenceKey);
            RequesToken = NWDBasisPreferences.GetString(kRequesTokenKey, this, string.Empty, false);
            if (string.IsNullOrEmpty(PlayerAccountReference))
            {
                PlayerAccountReference = NWDToolbox.GenerateUniqueID();
                RequesToken = string.Empty;
                SavePreferences();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetPreferences()
        {
            ResetSession();
            // and ?....
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetSession()
        {
            SavePreferences();
            NWDDataManager.SharedInstance().DataQueueExecute();
            PlayerAccountReference = NWDToolbox.GenerateUniqueID();
            RequesToken = string.Empty;
            SavePreferences();
            // add notification
            NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_ACCOUNT_CHANGE, null));
            NWDDataManager.SharedInstance().PlayerLanguageLoad();
#if UNITY_EDITOR
            NWDAppEnvironmentChooser.Refresh();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================