//=====================================================================================================================
//
// ideMobi copyright 2017 
// All rights reserved by ideMobi
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
using UnityEngine.iOS;
#endif

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public enum NWDNewsType : int
    {
        None = 0,

        //InGame = 1, // only in app

        Programmatically = 2, // only in app

        LocalNotificationNow = 10, // only in IOS/Android by local notification
        LocalNotificationDateFixe = 11, // only in IOS/Android by local notification
        LocalNotificationRecurrent = 12, // only in IOS/Android by local notification
        LocalNotificationSchedule = 13, // only in IOS/Android by local notification

        //PushNotificationDateNow = 20, // only in IOS/Android by push notification
        //PushNotificationDateFixe = 21, // only in IOS/Android by push notification
        //PushNotificationRecurrent = 22, // only in IOS/Android by push notification
        //PushNotificationSchedule = 23, // only in IOS/Android by push notification

        InGameNotificationNow = 30, // only in IOS/Android by start app notification
        InGameNotificationDateFixe = 31, // only in IOS/Android by start app notification
        InGameNotificationRecurrent = 32, // only in IOS/Android by start app notification
        InGameNotificationSchedule = 33, // only in IOS/Android by start app notification
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("NWS")]
    [NWDClassDescriptionAttribute("News Message")]
    [NWDClassMenuNameAttribute("News Message")]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDNews : NWDBasis<NWDNews>
    {
        //-------------------------------------------------------------------------------------------------------------
        [NWDInspectorGroupStart("Informations")]
        public NWDLocalizableStringType Title
        {
            get; set;
        }
        public NWDLocalizableStringType SubTitle
        {
            get; set;
        }
        public NWDLocalizableTextType Message
        {
            get; set;
        }
        //[NWDHidden]
        public NWDTextureType Image
        {
            get; set;
        }
        [NWDInspectorGroupEnd]
       
        [NWDInspectorGroupStart("Type of Event")]
        [NWDAlias("NewsType")]
        public NWDNewsType NewsType
        {
            get; set;
        }
        [NWDIf("NewsType", new int[] { (int)NWDNewsType.LocalNotificationDateFixe/*, (int)NWDNewsType.PushNotificationDateFixe*/, (int)NWDNewsType.InGameNotificationDateFixe }, false)]
        public NWDDateTimeType DistributionDate
        {
            get; set;
        }
        [NWDIf("NewsType", new int[] { (int)NWDNewsType.LocalNotificationRecurrent/*, (int)NWDNewsType.PushNotificationRecurrent*/, (int)NWDNewsType.InGameNotificationRecurrent }, false)]
        public int ReccurentLifeTime
        {
            get; set;
        }
        [NWDIf("NewsType", new int[] { (int)NWDNewsType.LocalNotificationSchedule/*, (int)NWDNewsType.PushNotificationSchedul*/, (int)NWDNewsType.InGameNotificationSchedule }, false)]
        [NWDNotWorking]
        [NWDInDevelopment]
        public NWDDateTimeScheduleType ScheduleDateTime
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================