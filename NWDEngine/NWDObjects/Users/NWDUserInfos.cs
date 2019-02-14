﻿//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDOperatingSystem : int
    {
        IOS = 0,
        OSX = 1,
        AppleTV = 2,
        Android = 3,
        WINRT = 8,
        WIN = 9,

        UNITY = 99,
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UIF")]
    [NWDClassDescriptionAttribute("General User Informations")]
    [NWDClassMenuNameAttribute("User Infos")]
    public partial class NWDUserInfos : NWDBasis<NWDUserInfos>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Player Informations")]
        [NWDTooltips("")]
        public NWDReferenceType<NWDAccount> Account { get; set; }
        public NWDReferenceType<NWDGameSave> GameSave { get; set; }
        public NWDReferenceType<NWDUserAvatar> Avatar { get; set; }
        public NWDReferenceType<NWDUserNickname> Nickname { get; set; }
        [NWDGroupEnd]

       

        [NWDGroupStart("Localization Options")]
        public NWDLanguageType Language { get; set; }
        [NWDGroupEnd]

       

        [NWDGroupStart("Push notification Options")]
        public NWDOperatingSystem OSLastSignIn { get; set; }
        public bool AcceptTradePush { get; set; }
        public bool AcceptBarterPush { get; set; }
        public bool AcceptShopPush { get; set; }
        public bool AcceptRelationshipPush { get; set; }
        public bool AcceptUserInterMessagePush { get; set; }
        public string AppleNotificationToken { get; set; }
        public string GoogleNotificationToken { get; set; }
        [NWDGroupEnd]

       

        [NWDGroupStart("Game Options")]
        public bool SFX { get; set; }
        public float SFXVolume { get; set; }
        public bool Music { get; set; }
        public float MusicVolume { get; set; }
        public NWDLocalizableStringType MusicVolumeLangu { get; set; }
        [NWDGroupEnd]

       

        [NWDGroupStart("Last Game Informations")]
        public NWDReferenceType<NWDItem> LastItemUsedReference { get; set; }
        public NWDReferenceType<NWDItem> LastItemWinReference { get; set; }
        public NWDReferenceType<NWDItem> LastSpiritUsedReference { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================