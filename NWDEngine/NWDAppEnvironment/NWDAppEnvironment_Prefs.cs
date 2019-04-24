// =====================================================================================================================
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
// =====================================================================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BasicToolBox;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
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
                //BTBPrefsManager.ShareInstance().set(Environment + kPlayerStatusKey, PlayerStatut.ToString());
                //BTBPrefsManager.ShareInstance().set(Environment + kPlayerAccountReferenceKey, PlayerAccountReference);
                //BTBPrefsManager.ShareInstance().set(Environment + kRequesTokenKey, RequesToken);
                //BTBPrefsManager.ShareInstance().set(Environment + kAnonymousPlayerAccountReferenceKey, AnonymousPlayerAccountReference);
                //BTBPrefsManager.ShareInstance().set(Environment + kAnonymousResetPasswordKey, AnonymousResetPassword);


                NWDBasisPreferences.SetString(Environment + kPlayerAccountReferenceKey + kPlayerStatusKey, this, PlayerStatut.ToString(), true);
                NWDBasisPreferences.SetString(Environment + kPlayerAccountReferenceKey, this, PlayerAccountReference, false);
                NWDBasisPreferences.SetString(Environment + kPlayerAccountReferenceKey + kRequesTokenKey, this, RequesToken, true);
                if (NWDAppConfiguration.SharedInstance().AnonymousPlayerIsLocal == true)
                {
                    BTBPrefsManager.ShareInstance().set(Environment + kAnonymousPlayerAccountReferenceKey, AnonymousPlayerAccountReference);
                    BTBPrefsManager.ShareInstance().set(Environment + kAnonymousPlayerAccountReferenceKey + kAnonymousResetPasswordKey, AnonymousResetPassword);
                }
                else
                {
                    NWDBasisPreferences.SetString(Environment + kAnonymousPlayerAccountReferenceKey, this, AnonymousPlayerAccountReference, true);
                    NWDBasisPreferences.SetString(Environment + kAnonymousPlayerAccountReferenceKey + kAnonymousResetPasswordKey, this, AnonymousResetPassword, true);
                }
            }
        }
        ////-------------------------------------------------------------------------------------------------------------
        //public void SavePreferences(NWDOperationResult sData)
        //{
        //    SavePreferences();
        //    //TODO : save usefull information from webservice data result
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void LoadPreferences()
        {
            //try
            //{
            //    PlayerStatut = (NWDAppEnvironmentPlayerStatut)Enum.Parse(typeof(NWDAppEnvironmentPlayerStatut), BTBPrefsManager.ShareInstance().getString(Environment + kPlayerStatusKey), true);
            //}
            //catch (ArgumentException e)
            //{
            //    Debug.Log(e.StackTrace);
            //}

            //PlayerAccountReference = BTBPrefsManager.ShareInstance().getString(Environment + kPlayerAccountReferenceKey);
            //RequesToken = BTBPrefsManager.ShareInstance().getString(Environment + kRequesTokenKey);
            //AnonymousPlayerAccountReference = BTBPrefsManager.ShareInstance().getString(Environment + kAnonymousPlayerAccountReferenceKey);
            //AnonymousResetPassword = BTBPrefsManager.ShareInstance().getString(Environment + kAnonymousResetPasswordKey);


            try
            {
                PlayerStatut = (NWDAppEnvironmentPlayerStatut)Enum.Parse(typeof(NWDAppEnvironmentPlayerStatut), NWDBasisPreferences.GetString(Environment + kPlayerAccountReferenceKey + kPlayerStatusKey, this, NWDAppEnvironmentPlayerStatut.Unknow.ToString(), true), true);
            }
            catch (ArgumentException e)
            {
                Debug.Log(e.StackTrace);
            }

            PlayerAccountReference = NWDBasisPreferences.GetString(Environment + kPlayerAccountReferenceKey, this, string.Empty, false);
            RequesToken = NWDBasisPreferences.GetString(Environment + kPlayerAccountReferenceKey + kRequesTokenKey, this, string.Empty, true);
            if (NWDAppConfiguration.SharedInstance().AnonymousPlayerIsLocal == true)
            {
                AnonymousPlayerAccountReference = BTBPrefsManager.ShareInstance().getString(Environment + kAnonymousPlayerAccountReferenceKey);
                AnonymousResetPassword = BTBPrefsManager.ShareInstance().getString(Environment + kAnonymousPlayerAccountReferenceKey + kAnonymousResetPasswordKey);
            }
            else
            {
                AnonymousPlayerAccountReference = NWDBasisPreferences.GetString(Environment + kAnonymousPlayerAccountReferenceKey, this, string.Empty, true);
                AnonymousResetPassword = NWDBasisPreferences.GetString(Environment + kAnonymousPlayerAccountReferenceKey + kAnonymousResetPasswordKey, this, string.Empty, true);
            }


            if (string.IsNullOrEmpty(PlayerAccountReference))
            {
                //ResetSession();
                AnonymousPlayerAccountReference = NWDToolbox.GenerateUniqueID();
                AnonymousResetPassword = NWDToolbox.RandomStringUnix(36);
                PlayerAccountReference = AnonymousPlayerAccountReference;
                RequesToken = string.Empty;
                PlayerStatut = NWDAppEnvironmentPlayerStatut.Temporary;
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
            SavePreferences(); // old datas for old Guy
            NWDDataManager.SharedInstance().DataQueueExecute();
            //Debug.Log ("ResetSession in " + Environment);
            AnonymousPlayerAccountReference = NWDToolbox.GenerateUniqueID();
            AnonymousResetPassword = NWDToolbox.RandomStringUnix(36);
            PlayerAccountReference = AnonymousPlayerAccountReference;
            RequesToken = string.Empty;
            PlayerStatut = NWDAppEnvironmentPlayerStatut.Temporary;
            SavePreferences();
            // add notification
            BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_ACCOUNT_CHANGE, null));
            NWDDataManager.SharedInstance().PlayerLanguageLoad();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetPlayerSession()
        {
            SavePreferences(); // old datas for old Guy
            NWDDataManager.SharedInstance().DataQueueExecute();
            //Debug.Log ("ResetPlayerSession in " + Environment);
            PlayerAccountReference = NWDToolbox.GenerateUniqueID();
            RequesToken = string.Empty;
            PlayerStatut = NWDAppEnvironmentPlayerStatut.Temporary;
            SavePreferences();
            // add notification
            BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_ACCOUNT_CHANGE, null));
            NWDDataManager.SharedInstance().PlayerLanguageLoad();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetAnonymousSession()
        {
            SavePreferences(); // old datas for old Guy
            NWDDataManager.SharedInstance().DataQueueExecute();
            //Debug.Log ("ResetAnonymousSession in " + Environment);
            AnonymousPlayerAccountReference = NWDToolbox.GenerateUniqueID();
            AnonymousResetPassword = NWDToolbox.RandomStringUnix(36);
            SavePreferences();
            // add notification
            BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_ACCOUNT_CHANGE, null));
            NWDDataManager.SharedInstance().PlayerLanguageLoad();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RestaureAnonymousSession()
        {
            SavePreferences(); // old datas for old Guy
            NWDDataManager.SharedInstance().DataQueueExecute();
            //Debug.Log ("RestaureAnonymousSession in " + Environment);
            PlayerAccountReference = AnonymousPlayerAccountReference;
            RequesToken = string.Empty;
            PlayerStatut = NWDAppEnvironmentPlayerStatut.Anonymous;
            // TODO :  must connect to server
            SavePreferences();
            // add notification
            BTBNotificationManager.SharedInstance().PostNotification(new BTBNotification(NWDNotificationConstants.K_ACCOUNT_CHANGE, null));
            NWDDataManager.SharedInstance().PlayerLanguageLoad();
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================