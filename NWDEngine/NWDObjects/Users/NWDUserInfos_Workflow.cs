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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using UnityEngine;

using BasicToolBox;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
        public NWDUserInfos()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDUserInfos(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization()
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        private static NWDUserInfos kCurrent = null;
        //-------------------------------------------------------------------------------------------------------------
        public static NWDUserInfos GetUserInfosOrCreate()
        {
            if (kCurrent != null)
            {
                if (kCurrent.Account.GetReference() != NWDAccount.CurrentReference())
                {
                    kCurrent = null;
                }
            }
            
            if (kCurrent == null)
            {
                NWDUserInfos tUserInfos = NWDBasisHelper.GetCorporateFirstData<NWDUserInfos>(NWDAccount.CurrentReference());
                if (tUserInfos == null)
                {
                    tUserInfos = NWDBasisHelper.NewData<NWDUserInfos>();
                    #if UNITY_EDITOR
                    tUserInfos.InternalKey = NWDAccount.CurrentReference();
                    #endif
                    tUserInfos.Account.SetReference(NWDAccount.CurrentReference());
                    tUserInfos.Tag = NWDBasisTag.TagUserCreated;
                    tUserInfos.SaveData();
                }
                kCurrent = tUserInfos;
            }
            
            return kCurrent;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SynchronizeDatas()
        {
            NWDBasisHelper.SynchronizationFromWebService<NWDUserInfos>();
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
                NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>(){typeof(NWDUserInfos)}, true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================