//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;

#if UNITY_IOS
using NotificationServices = UnityEngine.iOS.NotificationServices;
using NotificationType = UnityEngine.iOS.NotificationType;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountInfos : NWDBasis<NWDAccountInfos>
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountInfos()
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountInfos(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        private static NWDAccountInfos kCurrent = null;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountInfos GetAccountInfosOrCreate()
        {
            if (kCurrent != null)
            {
                if (kCurrent.Account.GetReference() != NWDAccount.GetCurrentAccountReference())
                {
                    kCurrent = null;
                }
            }
            
            if (kCurrent == null)
            {
                NWDAccountInfos tAccountInfos = GetFirstData(NWDAccount.GetCurrentAccountReference(), null);
                if (tAccountInfos == null)
                {
                    NWDAppEnvironment tAppEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
                    tAccountInfos = NewData();
                    tAccountInfos.Account.SetReference(NWDAccount.GetCurrentAccountReference());
                    tAccountInfos.AccountType = tAppEnvironment.PlayerStatut;
                    tAccountInfos.Tag = NWDBasisTag.TagUserCreated;
                    tAccountInfos.SaveData();
                }
                kCurrent = tAccountInfos;
            }
            
            return kCurrent;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetAccountType(NWDAppEnvironmentPlayerStatut tStatus)
        {
            NWDAccountInfos tActiveAccount = GetAccountInfosOrCreate();
            if (tActiveAccount != null)
            {
                tActiveAccount.AccountType = tStatus;
                tActiveAccount.SaveData();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void StartOnDevice()
        {
#if UNITY_ANDROID
            OSLastSignIn = NWDOperatingSystem.Android;
            // TODO register notification token

#elif UNITY_IOS
            OSLastSignIn = NWDOperatingSystem.IOS;
            // TODO register notification token

            NotificationServices.RegisterForNotifications( NotificationType.Alert | NotificationType.Badge | NotificationType.Sound);

            byte[] tToken = NotificationServices.deviceToken;
            if (tToken != null)
            {
                AppleNotificationToken = "%" + System.BitConverter.ToString(tToken).Replace('-', '%');
            }

#elif UNITY_STANDALONE_OSX
            OSLastSignIn = NWDOperatingSystem.OSX;
            // TODO register notification token

#elif UNITY_STANDALONE_WIN
            OSLastSignIn = NWDOperatingSystem.WIN;

#elif UNITY_WP8
            OSLastSignIn = NWDOperatingSystem.WIN;

#elif UNITY_WINRT
            OSLastSignIn = NWDOperatingSystem.WINRT;

#endif

            if (UpdateDataIfModified())
            {
                // TODO send to server immediatly
                NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>() { typeof(NWDAccountInfos) }, true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================