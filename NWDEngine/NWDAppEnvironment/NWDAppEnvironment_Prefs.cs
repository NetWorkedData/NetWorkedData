//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    public partial class NWDAppEnvironment
    {
        //-------------------------------------------------------------------------------------------------------------
        const string kPlayerAccountReferenceKey = "kPlayerAccountReference_Key";
        const string kRequesTokenKey = "kRequesToken_Key";
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
                BTBPrefsManager.ShareInstance().set(Environment + kPlayerStatusKey, PlayerStatut.ToString());
                BTBPrefsManager.ShareInstance().set(Environment + kPlayerAccountReferenceKey, PlayerAccountReference);
                BTBPrefsManager.ShareInstance().set(Environment + kRequesTokenKey, RequesToken);
                BTBPrefsManager.ShareInstance().set(Environment + kAnonymousPlayerAccountReferenceKey, AnonymousPlayerAccountReference);
                BTBPrefsManager.ShareInstance().set(Environment + kAnonymousResetPasswordKey, AnonymousResetPassword);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SavePreferences(NWDOperationResult sData)
        {
            SavePreferences();
            //TODO : save usefull information from webservice data result
        }
        //-------------------------------------------------------------------------------------------------------------
        public void LoadPreferences()
        {
            try
            {
                PlayerStatut = (NWDAppEnvironmentPlayerStatut)Enum.Parse(typeof(NWDAppEnvironmentPlayerStatut), BTBPrefsManager.ShareInstance().getString(Environment + kPlayerStatusKey), true);
            }
            catch (ArgumentException e)
            {
                Debug.Log(e.StackTrace);
            }

            PlayerAccountReference = BTBPrefsManager.ShareInstance().getString(Environment + kPlayerAccountReferenceKey);
            RequesToken = BTBPrefsManager.ShareInstance().getString(Environment + kRequesTokenKey);
            AnonymousPlayerAccountReference = BTBPrefsManager.ShareInstance().getString(Environment + kAnonymousPlayerAccountReferenceKey);
            AnonymousResetPassword = BTBPrefsManager.ShareInstance().getString(Environment + kAnonymousResetPasswordKey);

            if (string.IsNullOrEmpty(PlayerAccountReference))
            {
                //ResetSession();
                AnonymousPlayerAccountReference = NWDToolbox.GenerateUniqueID();
                AnonymousResetPassword = NWDToolbox.RandomStringUnix(36);
                PlayerAccountReference = AnonymousPlayerAccountReference;
                RequesToken = "";
                PlayerStatut = NWDAppEnvironmentPlayerStatut.Temporary;
                SavePreferences();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetPreferences()
        {
            ResetSession();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetSession()
        {
            //Debug.Log ("ResetSession in " + Environment);
            AnonymousPlayerAccountReference = NWDToolbox.GenerateUniqueID();
            AnonymousResetPassword = NWDToolbox.RandomStringUnix(36);
            PlayerAccountReference = AnonymousPlayerAccountReference;
            RequesToken = "";
            PlayerStatut = NWDAppEnvironmentPlayerStatut.Temporary;
            SavePreferences();
            // add notification
            NWDDataManager.SharedInstance().NotificationCenter.PostNotification(new BTBNotification(NWDGameDataManager.NOTIFICATION_USER_CHANGE, null));
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetPlayerSession()
        {
            //Debug.Log ("ResetPlayerSession in " + Environment);
            PlayerAccountReference = NWDToolbox.GenerateUniqueID();
            RequesToken = "";
            PlayerStatut = NWDAppEnvironmentPlayerStatut.Temporary;
            SavePreferences();
            // add notification
            NWDDataManager.SharedInstance().NotificationCenter.PostNotification(new BTBNotification(NWDGameDataManager.NOTIFICATION_USER_CHANGE, null));
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetAnonymousSession()
        {
            //Debug.Log ("ResetAnonymousSession in " + Environment);
            AnonymousPlayerAccountReference = NWDToolbox.GenerateUniqueID();
            AnonymousResetPassword = NWDToolbox.RandomStringUnix(36);
            SavePreferences();
            // add notification
            NWDDataManager.SharedInstance().NotificationCenter.PostNotification(new BTBNotification(NWDGameDataManager.NOTIFICATION_USER_CHANGE, null));
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RestaureAnonymousSession()
        {
            //Debug.Log ("RestaureAnonymousSession in " + Environment);
            PlayerAccountReference = AnonymousPlayerAccountReference;
            RequesToken = "";
            PlayerStatut = NWDAppEnvironmentPlayerStatut.Anonymous;
            // TODO :  must connect to server
            SavePreferences();
            // add notification
            NWDDataManager.SharedInstance().NotificationCenter.PostNotification(new BTBNotification(NWDGameDataManager.NOTIFICATION_USER_CHANGE, null));
        }
        //-------------------------------------------------------------------------------------------------------------
    }
}
//=====================================================================================================================