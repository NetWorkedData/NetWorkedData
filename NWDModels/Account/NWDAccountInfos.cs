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
    /// <summary>
    /// <see cref="NWDAccountInfos"/> is class to create instance with informations of account : current <see cref="NWDGameSave"/>, <see cref="NWDAccountAvatar"/>, <see cref="NWDAccountNickname"/> ... 
    /// </summary>
    [NWDClassTrigrammeAttribute("AIF")]
    [NWDClassDescriptionAttribute("General Account Informations")]
    [NWDClassMenuNameAttribute("Account Infos")]
    [NWDClassClusterAttribute(1, 2)]
    public partial class NWDAccountInfos : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Player Informations")]
#if NWD_ACCOUNT_IDENTITY
        public NWDReferenceType<NWDAccountAvatar> Avatar { get; set; }
        public NWDReferenceType<NWDAccountNickname> Nickname { get; set; }
#endif
        public NWDReferenceType<NWDGameSave> CurrentGameSave { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Host")]
        public NWDReferenceType<NWDServerDomain> Server { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Stat")]
        public NWDDateTimeType LastSignIn { get; set; }
        public NWDDateTimeType LastAppOpen { get; set; }
        [NWDInspectorGroupEnd]
        [NWDInspectorGroupStart("Push notification Options")]
        public NWDOperatingSystem OSLastSignIn { get; set; }
        public string AppleNotificationToken { get; set; }
        public string GoogleNotificationToken { get; set; }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================