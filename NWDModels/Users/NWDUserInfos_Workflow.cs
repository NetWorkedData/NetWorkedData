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
        public static string GetCurrentNickname()
        {
            NWDUserNickname tNickname = CurrentData().Nickname.GetReachableData();
            if (tNickname != null)
            {
                return tNickname.Nickname;
            }

            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Sprite GetCurrentAvatar(bool isRenderTexture = false)
        {
            NWDUserAvatar tAvatar = CurrentData().Avatar.GetReachableData();
            if (tAvatar != null)
            {
                NWDItem tItem = tAvatar.RenderItem.GetReachableData();
                if (tItem != null)
                {
                    if (isRenderTexture)
                    {
                        NWDImagePNGType tImage = tAvatar.RenderTexture;
                        if (tImage != null)
                        {
                            return tImage.ToSprite();
                        }
                    }
                    else
                    {
                        if (!tItem.SecondarySprite.ValueIsNullOrEmpty())
                        {
                            return tItem.SecondarySprite.ToSprite();
                        }
                        else
                        {
                            return tItem.PrimarySprite.ToSprite();
                        }
                    }
                }
            }

            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetNicknameByUserReference(string sReference)
        {
            NWDUserNickname tNickname = NWDBasisHelper.GetCorporateFirstData<NWDUserNickname>(sReference);
            if (tNickname != null)
            {
                return tNickname.Nickname;
            }

            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Sprite GetAvatarByUserReference(string sReference, bool isRenderTexture = false)
        {
            NWDUserAvatar tAvatar = NWDBasisHelper.GetCorporateFirstData<NWDUserAvatar>(sReference);
            if (tAvatar != null)
            {
                NWDItem tItem = tAvatar.RenderItem.GetRawData();
                if (tItem != null)
                {
                    if (isRenderTexture)
                    {
                        NWDImagePNGType tImage = tAvatar.RenderTexture;
                        if (tImage != null)
                        {
                            return tImage.ToSprite();
                        }
                    }
                    else
                    {
                        if (!tItem.SecondarySprite.ValueIsNullOrEmpty())
                        {
                            return tItem.SecondarySprite.ToSprite();
                        }
                        else
                        {
                            return tItem.PrimarySprite.ToSprite();
                        }
                    }
                }
            }

            return null;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetCurrentNickname(string sNickname)
        {
            NWDUserNickname tNickname = CurrentData().Nickname.GetRawData();
            if (tNickname == null)
            {
                tNickname = NWDBasisHelper.NewData<NWDUserNickname>();
                tNickname.InternalKey = NWDAccount.CurrentReference();
                tNickname.InternalDescription = sNickname;
                tNickname.Tag = NWDBasisTag.TagUserCreated;

                // Set a Nickname define by user
                CurrentData().Nickname.SetData(tNickname);
                CurrentData().SaveData();
            }

            // Set the new Nickname
            tNickname.Nickname = sNickname;
            tNickname.SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetCurrentAvatar(NWDItem sAvatar)
        {
            NWDUserAvatar tAvatar = CurrentData().Avatar.GetRawData();
            if (tAvatar == null)
            {
                tAvatar = NWDBasisHelper.NewData<NWDUserAvatar>();
                tAvatar.InternalKey = NWDAccount.CurrentReference();
                tAvatar.Tag = NWDBasisTag.TagUserCreated;

                // Set an Avatar define by user
                CurrentData().Avatar.SetData(tAvatar);
                CurrentData().SaveData();
            }

            // Set the new Avatar
            tAvatar.RenderItem.SetData(sAvatar);
            tAvatar.SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SetCurrentLastSignIn()
        {
            CurrentData().LastSignIn.SetCurrentDateTime();
            CurrentData().SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void ResetSession()
        {
            NWDAppEnvironment tAppEnvironment = NWDAppConfiguration.SharedInstance().SelectedEnvironment();
            tAppEnvironment.ResetPreferences();
        }
        //-------------------------------------------------------------------------------------------------------------
        public static void SynchronizeDatas()
        {
            NWDBasisHelper.SynchronizationFromWebService<NWDUserInfos>();
        }
        //=============================================================================================================
        // PUBLIC METHOD
        //-------------------------------------------------------------------------------------------------------------
        public string GetAbsoluteNickname()
        {
            string rNickname = NWEConstants.K_MINUS;
            NWDUserNickname tNickname = Nickname.GetRawData();
            if (tNickname != null)
            {
                rNickname = tNickname.Nickname;
            }
            return rNickname;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Sprite GetAbsoluteAvatar(bool isPrimarySprite = true)
        {
            Sprite rAvatar = null;
            NWDUserAvatar tAvatar = Avatar.GetRawData();
            if (tAvatar != null)
            {
                NWDItem tItem = tAvatar.RenderItem.GetRawData();
                if (tItem != null)
                {
                    if (isPrimarySprite)
                    {
                        rAvatar = tItem.PrimarySprite.ToSprite();
                    }
                    else
                    {
                        rAvatar = tItem.SecondarySprite.ToSprite();
                    }
                }
            }
            return rAvatar;
        }
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