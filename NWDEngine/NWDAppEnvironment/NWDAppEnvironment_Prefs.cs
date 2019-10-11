//=====================================================================================================================
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
                //NWEPrefsManager.ShareInstance().set(Environment + kPlayerStatusKey, PlayerStatut.ToString());
                //NWEPrefsManager.ShareInstance().set(Environment + kPlayerAccountReferenceKey, PlayerAccountReference);
                //NWEPrefsManager.ShareInstance().set(Environment + kRequesTokenKey, RequesToken);
                //NWEPrefsManager.ShareInstance().set(Environment + kAnonymousPlayerAccountReferenceKey, AnonymousPlayerAccountReference);
                //NWEPrefsManager.ShareInstance().set(Environment + kAnonymousResetPasswordKey, AnonymousResetPassword);

                NWEPrefsManager.ShareInstance().set(Environment + kPlayerAccountReferenceKey, PlayerAccountReference);
                //NWDBasisPreferences.SetString(kPlayerAccountReferenceKey, this, PlayerAccountReference, false);
                //NWDBasisPreferences.SetString(kPlayerStatusKey, this, PlayerStatut.ToString(), true);
                NWDBasisPreferences.SetString(kRequesTokenKey, this, RequesToken, false);

                //if (NWDAppConfiguration.SharedInstance().AnonymousPlayerIsLocal == true)
                //{
                //    NWEPrefsManager.ShareInstance().set(Environment + kAnonymousPlayerAccountReferenceKey, AnonymousPlayerAccountReference);
                //    NWEPrefsManager.ShareInstance().set(Environment + kAnonymousResetPasswordKey, AnonymousResetPassword);
                //}
                //else
                //{
                //    NWDBasisPreferences.SetString(kAnonymousPlayerAccountReferenceKey, this, AnonymousPlayerAccountReference, true);
                //    NWDBasisPreferences.SetString(kAnonymousResetPasswordKey, this, AnonymousResetPassword, true);
                //}
                NWDDataManager.SharedInstance().DataQueueExecute();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void LoadPreferences()
        {
//            string tEnvCompiler = "player";
//#if UNITY_EDITOR
//            if (Application.isPlaying == false)
//            {
//                tEnvCompiler = "editor";
//            }
//#endif
            //Debug.Log("<color=blue>NWDAppEnvironment() LoadPreferences : " + Environment + " " + tEnvCompiler + "</color>");
            //try
            //{
            //    PlayerStatut = (NWDAppEnvironmentPlayerStatut)Enum.Parse(typeof(NWDAppEnvironmentPlayerStatut), NWEPrefsManager.ShareInstance().getString(Environment + kPlayerStatusKey), true);
            //}
            //catch (ArgumentException e)
            //{
            //    Debug.Log(e.StackTrace);
            //}

            //PlayerAccountReference = NWEPrefsManager.ShareInstance().getString(Environment + kPlayerAccountReferenceKey);
            //RequesToken = NWEPrefsManager.ShareInstance().getString(Environment + kRequesTokenKey);
            //AnonymousPlayerAccountReference = NWEPrefsManager.ShareInstance().getString(Environment + kAnonymousPlayerAccountReferenceKey);
            //AnonymousResetPassword = NWEPrefsManager.ShareInstance().getString(Environment + kAnonymousResetPasswordKey);

            //try
            //{
            //    PlayerStatut = (NWDAppEnvironmentPlayerStatut)Enum.Parse(typeof(NWDAppEnvironmentPlayerStatut), NWDBasisPreferences.GetString(kPlayerStatusKey, this, NWDAppEnvironmentPlayerStatut.Unknow.ToString(), true), true);
            //}
            //catch (ArgumentException e)
            //{
            //    Debug.Log(e.StackTrace);
            //}

            PlayerAccountReference = NWEPrefsManager.ShareInstance().getString(Environment + kPlayerAccountReferenceKey);
            //PlayerAccountReference = NWDBasisPreferences.GetString(kPlayerAccountReferenceKey, this, string.Empty, false);
            RequesToken = NWDBasisPreferences.GetString(kRequesTokenKey, this, string.Empty, false);

            //if (NWDAppConfiguration.SharedInstance().AnonymousPlayerIsLocal == true)
            //{
            //    AnonymousPlayerAccountReference = NWEPrefsManager.ShareInstance().getString(Environment + kAnonymousPlayerAccountReferenceKey);
            //    AnonymousResetPassword = NWEPrefsManager.ShareInstance().getString(Environment + kAnonymousResetPasswordKey);
            //}
            //else
            //{
            //    AnonymousPlayerAccountReference = NWDBasisPreferences.GetString(kAnonymousPlayerAccountReferenceKey, this, string.Empty, true);
            //    AnonymousResetPassword = NWDBasisPreferences.GetString(kAnonymousResetPasswordKey, this, string.Empty, true);
            //}

            if (string.IsNullOrEmpty(PlayerAccountReference))
            {
                //ResetSession();
                //AnonymousPlayerAccountReference = NWDToolbox.GenerateUniqueID();
                //AnonymousResetPassword = NWDToolbox.RandomStringUnix(36);
                PlayerAccountReference = NWDToolbox.GenerateUniqueID();
                RequesToken = string.Empty;
                //PlayerStatut = NWDAppEnvironmentPlayerStatut.Temporary;
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
            //AnonymousPlayerAccountReference = NWDToolbox.GenerateUniqueID();
            //AnonymousResetPassword = NWDToolbox.RandomStringUnix(36);
            PlayerAccountReference = NWDToolbox.GenerateUniqueID();
            RequesToken = string.Empty;
            //PlayerStatut = NWDAppEnvironmentPlayerStatut.Temporary;
            SavePreferences();
            // add notification
            NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_ACCOUNT_CHANGE, null));
            NWDDataManager.SharedInstance().PlayerLanguageLoad();
#if UNITY_EDITOR
            NWDAppEnvironmentChooser.Refresh();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        public void ResetPlayerSession()
        {
            SavePreferences(); // old datas for old Guy
            NWDDataManager.SharedInstance().DataQueueExecute();
            //Debug.Log ("ResetPlayerSession in " + Environment);
            PlayerAccountReference = NWDToolbox.GenerateUniqueID();
            RequesToken = string.Empty;
            //PlayerStatut = NWDAppEnvironmentPlayerStatut.Temporary;
            SavePreferences();
            // add notification
            NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_ACCOUNT_CHANGE, null));
            NWDDataManager.SharedInstance().PlayerLanguageLoad();
            NWDDataManager.SharedInstance().PlayerLanguageLoad();
#if UNITY_EDITOR
            NWDAppEnvironmentChooser.Refresh();
#endif
        }
        //-------------------------------------------------------------------------------------------------------------
        //        public void ResetAnonymousSession()
        //        {
        //            SavePreferences(); // old datas for old Guy
        //            NWDDataManager.SharedInstance().DataQueueExecute();
        //            //Debug.Log ("ResetAnonymousSession in " + Environment);
        //            //AnonymousPlayerAccountReference = NWDToolbox.GenerateUniqueID();
        //            //AnonymousResetPassword = NWDToolbox.RandomStringUnix(36);
        //            SavePreferences();
        //            // add notification
        //            NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_ACCOUNT_CHANGE, null));
        //            NWDDataManager.SharedInstance().PlayerLanguageLoad();
        //            NWDDataManager.SharedInstance().PlayerLanguageLoad();
        //#if UNITY_EDITOR
        //            NWDAppEnvironmentChooser.Refresh();
        //#endif
        //}
        //-------------------------------------------------------------------------------------------------------------
        //        public void RestaureAnonymousSession()
        //        {
        //            SavePreferences(); // old datas for old Guy
        //            NWDDataManager.SharedInstance().DataQueueExecute();
        //            //Debug.Log ("RestaureAnonymousSession in " + Environment);
        //            PlayerAccountReference = AnonymousPlayerAccountReference;
        //            RequesToken = string.Empty;
        //            PlayerStatut = NWDAppEnvironmentPlayerStatut.Anonymous;
        //            // TODO :  must connect to server
        //            SavePreferences();
        //            // add notification
        //            NWENotificationManager.SharedInstance().PostNotification(new NWENotification(NWDNotificationConstants.K_ACCOUNT_CHANGE, null));
        //            NWDDataManager.SharedInstance().PlayerLanguageLoad();
        //            NWDDataManager.SharedInstance().PlayerLanguageLoad();
        //#if UNITY_EDITOR
        //            NWDAppEnvironmentChooser.Refresh();
        //#endif
        //}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================