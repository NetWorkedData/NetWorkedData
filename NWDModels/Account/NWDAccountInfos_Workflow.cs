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
    public partial class NWDAccountInfos : NWDBasis
    {
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountInfos() { }
        //-------------------------------------------------------------------------------------------------------------
        public NWDAccountInfos(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData) { }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
#if UNITY_EDITOR
            Debug.Log("NWDAccountInfos Initialization()");
            InternalKey = "AccountInfos : " + DateTime.Today.ToShortDateString();
            Tag = NWDBasisTag.TagTestForDev;
#endif
        }
        //=============================================================================================================
        // PUBLIC STATIC METHOD
        //-------------------------------------------------------------------------------------------------------------
        public static void LoadBalacing(int sAvg)
        {
            //Debug.Log("LoadBalacing() " + sAvg);
            bool tChangeServer = true;
            NWDAccountInfos rAccountInfos = CurrentData();
            if (rAccountInfos != null)
            {
                NWDServerDomain tServer = null;
                if (rAccountInfos.Server != null)
                {
                    tServer = rAccountInfos.Server.GetReachableData();
                }
                if (tServer != null)
                {
                    if (tServer.BalanceLoad > sAvg)
                    {
                        tChangeServer = false;
                        Debug.Log("NOT CHANGE SERVER " + NWDAppEnvironment.SelectedEnvironment().SFTPBalanceLoad + " > " + sAvg);
                    }
                    else
                    {
                        Debug.Log("CHANGE SERVER " + tServer.BalanceLoad + " < " + sAvg);
                    }
                }
                else
                {
                    if (NWDAppEnvironment.SelectedEnvironment().SFTPBalanceLoad > sAvg)
                    {
                        tChangeServer = false;
                        Debug.Log("NOT CHANGE DEFAULT SERVER " + NWDAppEnvironment.SelectedEnvironment().SFTPBalanceLoad + " > " + sAvg);
                    }
                    else
                    {
                        Debug.Log("CHANGE DEFAULT SERVER " + NWDAppEnvironment.SelectedEnvironment().SFTPBalanceLoad + " < " + sAvg);
                    }
                }
                if (tChangeServer == true)
                {
                    List<NWDServerDomain> tServerList = new List<NWDServerDomain>();
                    foreach (NWDServerDomain tServerInList in NWDBasisHelper.GetReachableDatas<NWDServerDomain>())
                    {
                        if (tServerInList != tServer && tServerInList.ValidInSelectedEnvironment())
                        {
                            tServerList.Add(tServerInList);
                        }
                    }
                    if (tServerList.Count > 0)
                    {
                        if (tServerList.Count > 1)
                        {
                            tServerList.ShuffleList();
                        }
                        tServer = tServerList[0];
                    }
                    rAccountInfos.Server.SetData(tServer);
                    rAccountInfos.UpdateDataIfModified();
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        /*public static NWDAccountInfos GetCurrentAccount()
        {
            NWDAccountInfos rAccountInfos = CurrentData();
            if (rAccountInfos != null)
            {
                SetCurrentLastSignIn();
            }
            return rAccountInfos;
        }*/
        //-------------------------------------------------------------------------------------------------------------
        public static string GetCurrentNickname()
        {
            NWDAccountNickname tNickname = CurrentData().Nickname.GetReachableData();
            if (tNickname != null)
            {
                return tNickname.Nickname;
            }
            return string.Empty;
        }
        //-------------------------------------------------------------------------------------------------------------
        public static Sprite GetCurrentAvatar(bool isRenderTexture = false)
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
            NWDBasisHelper.SynchronizationFromWebService<NWDAccountInfos>();
        }
        //=============================================================================================================
        // PUBLIC METHOD
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
        public string GetAbsoluteLocalizedLastSignIn(NWDLocalizationConnection sConnectionDay, NWDLocalizationConnection sConnectionHour)
        {
            DateTime tCurrent = DateTime.Now;
            DateTime tLastSignIn = LastSignIn.ToDateTime();
            TimeSpan tDifference = tCurrent - tLastSignIn;

            string rLastSignIn = "Last login: xxx";
            if (tDifference.Days == 0)
            {
                string tHour = tDifference.Hours.ToString();
                if (tDifference.Hours < 0)
                {
                    tHour = "0";
                }

                NWDLocalization tLocalization = sConnectionHour.GetReachableData();
                if (tLocalization != null)
                {
                    rLastSignIn = tLocalization.TextValue.GetLocalString();
                }
                rLastSignIn = rLastSignIn.Replace("xxx", tHour);
            }
            else
            {
                NWDLocalization tLocalization = sConnectionDay.GetReachableData();
                if (tLocalization != null)
                {
                    rLastSignIn = tLocalization.TextValue.GetLocalString();
                }
                rLastSignIn = rLastSignIn.Replace("xxx", tDifference.Days.ToString());
            }

            return rLastSignIn;
        }
        //-------------------------------------------------------------------------------------------------------------
        public string GetAbsoluteLastSignIn()
        {
            DateTime tCurrent = DateTime.Today;
            DateTime tLastSignIn = LastSignIn.ToDateTime();
            TimeSpan rDifference = tCurrent - tLastSignIn;

            return rDifference.Days.ToString();
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
                NWDBasisHelper.SynchronizationFromWebService<NWDAccountInfos>();
            }
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================