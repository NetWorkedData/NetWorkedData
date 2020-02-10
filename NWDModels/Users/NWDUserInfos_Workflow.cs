//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:42:22
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_IOS
using NotificationServices = UnityEngine.iOS.NotificationServices;
using NotificationType = UnityEngine.iOS.NotificationType;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDUserInfos : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserInfos() {}
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserInfos(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData) {}
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() {}
        //=============================================================================================================
        // PUBLIC STATIC METHOD
        //-------------------------------------------------------------------------------------------------------------
        public static void SetCurrentLastSignIn()
        {
            CurrentData().LastSignIn.SetCurrentDateTime();
            CurrentData().SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ResetSession()
        {
           NWDAppConfiguration.SharedInstance().SelectedEnvironment().ResetPreferences();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SynchronizeDatas()
        {
            NWDBasisHelper.SynchronizationFromWebService<NWDUserInfos>();
        }
        //=============================================================================================================
        // PUBLIC METHOD
        //-------------------------------------------------------------------------------------------------------------
        public void StartOnDevice()
        {
#if UNITY_ANDROID
            OSLastSignIn = NWDOperatingSystem.Android;
#elif UNITY_IOS
            OSLastSignIn = NWDOperatingSystem.IOS;
            NotificationServices.RegisterForNotifications( NotificationType.Alert | NotificationType.Badge | NotificationType.Sound);

            byte[] tToken = NotificationServices.deviceToken;
            if (tToken != null)
            {
                AppleNotificationToken = "%" + System.BitConverter.ToString(tToken).Replace('-', '%');
            }
#elif UNITY_STANDALONE_OSX  
            OSLastSignIn = NWDOperatingSystem.OSX;
#elif UNITY_STANDALONE_WIN
            OSLastSignIn = NWDOperatingSystem.WIN;
#elif UNITY_WP8
            OSLastSignIn = NWDOperatingSystem.WIN;
#elif UNITY_WINRT
            OSLastSignIn = NWDOperatingSystem.WINRT;
#endif

            if (UpdateDataIfModified())
            {
                NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>(){typeof(NWDUserInfos)}, true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================