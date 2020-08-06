//=====================================================================================================================
//
//  ideMobi 2020©
//
//=====================================================================================================================
// Define the use of Log and Benchmark only for this file!
// Add NWD_VERBOSE in scripting define symbols (Edit->Project Settings…->Player->[Choose Plateform]->Other Settings->Scripting Define Symbols)
#if NWD_VERBOSE
#if UNITY_EDITOR
#define NWD_LOG
#define NWD_BENCHMARK
#elif DEBUG
//#define NWD_LOG
//#define NWD_BENCHMARK
#endif
#else
#undef NWD_LOG
#undef NWD_BENCHMARK
#endif
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
    //[NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("UIF")]
    [NWDClassDescriptionAttribute("General User Informations")]
    [NWDClassMenuNameAttribute("User Infos")]
    [NWDClassClusterAttribute(1, 32)]
    public partial class NWDUserInfos : NWDBasisGameSaveDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Player Informations")]
        public NWDDateTimeType LastSignIn { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Localization Options")]
        public NWDLanguageType Language { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Push notification Options")]
        public NWDOperatingSystem OSLastSignIn { get; set; }
        public bool AcceptTradePush { get; set; }
        public bool AcceptBarterPush { get; set; }
        public bool AcceptShopPush { get; set; }
        public bool AcceptRelationshipPush { get; set; }
        public bool AcceptUserInterMessagePush { get; set; }
        public string AppleNotificationToken { get; set; }
        public string GoogleNotificationToken { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Game Options")]
        public bool SFX { get; set; }
        public float SFXVolume { get; set; }
        public bool Music { get; set; }
        public float MusicVolume { get; set; }
        public NWDLocalizableStringType MusicVolumeLangu { get; set; }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Last Game Informations")]
        public NWDReferenceType<NWDItem> LastItemUsedReference { get; set; }
        public NWDReferenceType<NWDItem> LastItemWinReference { get; set; }
        public NWDReferenceType<NWDItem> LastSpiritUsedReference { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================