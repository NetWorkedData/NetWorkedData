//=====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:22
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using UnityEngine;

#if UNITY_IOS
using NotificationServices = UnityEngine.iOS.NotificationServices;
using NotificationType = UnityEngine.iOS.NotificationType;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public partial class NWDAccountInfos : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountInfos() {}
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountInfos(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData) {}
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() {}
        //-------------------------------------------------------------------------------------------------------------
        private static NWDAccountInfos ActiveAccount => CheckAccount();
        //-------------------------------------------------------------------------------------------------------------
        public static NWDAccountInfos CheckAccount()
        {
            NWDAccountInfos rAccountInfos = CurrentData();
            if (rAccountInfos != null)
            {
                rAccountInfos.SetLastSignIn();
            }
            return rAccountInfos;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static string GetNickname()
        {
            NWDAccountNickname tNickname = CurrentData().Nickname.GetReachableData();
            if (tNickname != null)
            {
                return tNickname.Nickname;
            }
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Sprite GetAvatar(bool isRenderTexture = false)
        {            
            Sprite rSprite = null;
            NWDAccountAvatar tAvatar = CurrentData().Avatar.GetReachableData();
            if (tAvatar != null)
            {
                NWDItem tAvatarItem = tAvatar.RenderItem.GetRawData();
                if (tAvatarItem != null)
                {
                    if (isRenderTexture)
                    {
                        NWDImagePNGType tImage = tAvatar.RenderTexture;
                        if (tImage != null)
                        {
                            rSprite = tImage.ToSprite();
                        }
                    }
                    else
                    {
                        if (!tAvatarItem.SecondarySprite.ValueIsNullOrEmpty())
                        {
                            rSprite = tAvatarItem.SecondarySprite.ToSprite();
                        }
                        else
                        {
                            rSprite = tAvatarItem.PrimarySprite.ToSprite();
                        }
                    }
                }
            }
            return rSprite;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetAbsoluteNickname()
        {
            string rNickname = NWEConstants.K_MINUS;
            NWDAccountNickname tNickname = Nickname.GetRawData();
            if (tNickname != null)
            {
                rNickname = tNickname.Nickname;
            }
            return rNickname;
        }
        //-------------------------------------------------------------------------------------------------------------
        public Sprite GetAbsoluteAvatar(bool isPrimaryTexture = true)
        {
            Sprite rAvatar = null;
            NWDAccountAvatar tAvatar = Avatar.GetRawData();
            if (tAvatar != null)
            {
                NWDItem tItem = tAvatar.RenderItem.GetRawData();
                if (tItem != null)
                {
                    if (isPrimaryTexture)
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
        public void SetLastSignIn()
        {
            LastSignIn.SetCurrentDateTime();
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetAvatar(NWDItem sAvatar)
        {
            NWDAccountAvatar tAvatar = Avatar.GetRawData();
            if (tAvatar == null)
            {
                tAvatar = NWDBasisHelper.NewData<NWDAccountAvatar>();
                tAvatar.InternalKey = NWDAccount.CurrentReference();
                tAvatar.Tag = NWDBasisTag.TagUserCreated;
            }
            tAvatar.RenderItem.SetData(sAvatar);
            tAvatar.SaveData();
            
            Avatar.SetData(tAvatar);
            SaveData();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void SetNickname(string sNickname)
        {
            NWDAccountNickname tNickname = Nickname.GetRawData();
            if (tNickname == null)
            {
                tNickname = NWDBasisHelper.NewData<NWDAccountNickname>();
                tNickname.InternalKey = NWDAccount.CurrentReference();
                tNickname.InternalDescription = sNickname;
                tNickname.Tag = NWDBasisTag.TagUserCreated;
            }
            tNickname.Nickname = sNickname;
            tNickname.SaveData();

            Nickname.SetData(tNickname);
            SaveData();
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
                // TODO Check this
                //NWDDataManager.SharedInstance().AddWebRequestSynchronization(new List<Type>() { typeof(NWDAccountInfos) }, true);
                NWDBasisHelper.SynchronizationFromWebService<NWDAccountInfos>();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================