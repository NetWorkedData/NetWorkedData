// =====================================================================================================================
//
//  ideMobi 2019©
//
//  Date		2019-4-12 18:29:20
//  Author		Kortex (Jean-François CONTART) 
//  Email		jfcontart@idemobi.com
//  Project 	NetWorkedData for Unity3D
//
//  All rights reserved by ideMobi
//
// =====================================================================================================================

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
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("AIF")]
    [NWDClassDescriptionAttribute("General Account Informations")]
    [NWDClassMenuNameAttribute("Account Infos")]
    public partial class NWDAccountInfos : NWDBasis<NWDAccountInfos>
    {
        //-------------------------------------------------------------------------------------------------------------

        [NWDInspectorGroupStart("Player Informations")]
        public NWDReferenceType<NWDAccount> Account
        {
            get; set;
        }
        public NWDAppEnvironmentPlayerStatut AccountType
        {
            get; set;
        }
        public NWDReferenceType<NWDAccountAvatar> Avatar
        {
            get; set;
        }
        public NWDReferenceType<NWDAccountNickname> Nickname
        {
            get; set;
        }
        public NWDReferenceFreeType<NWDGameSave> CurrentGameSave
        {
            get; set;
        }
        public string FirstName
        {
            get; set;
        }
        public string LastName
        {
            get; set;
        }
        [NWDInspectorGroupEnd]

        [NWDInspectorGroupStart("Push notification Options")]
        public NWDOperatingSystem OSLastSignIn
        {
            get; set;
        }
        public string AppleNotificationToken
        {
            get; set;
        }
        public string GoogleNotificationToken
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================