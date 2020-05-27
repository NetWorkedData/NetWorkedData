//=====================================================================================================================
//
//  ideMobi 2020©
//  All rights reserved by ideMobi
//
//=====================================================================================================================

using System;
using System.Collections.Generic;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassTrigrammeAttribute("AIF")]
    [NWDClassDescriptionAttribute("General Account Informations")]
    [NWDClassMenuNameAttribute("Account Infos")]
    [NWDClassClusterAttribute(1, 2)]
    public partial class NWDAccountInfos : NWDBasisAccountDependent
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Player Informations")]
        public NWDReferenceType<NWDAccountAvatar> Avatar { get; set; }
        public NWDReferenceType<NWDAccountNickname> Nickname { get; set; }
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