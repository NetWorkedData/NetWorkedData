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

        //PushNotificationDateNow = 21, // only in IOS/Android by push notification
        //PushNotificationDateFixe = 21, // only in IOS/Android by push notification
        //PushNotificationRecurrent = 22, // only in IOS/Android by push notification
        //PushNotificationSchedule = 23, // only in IOS/Android by push notification

        InGameNotificationNow = 30, // only in IOS/Android by start app notification
        InGameNotificationDateFixe = 31, // only in IOS/Android by start app notification
        InGameNotificationRecurrent = 32, // only in IOS/Android by start app notification
        InGameNotificationSchedule = 33, // only in IOS/Android by start app notification
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// <para>Connection is used in MonBehaviour script to connect an object by its reference from popmenu list.</para>
    /// <para>The GameObject can use the object referenced by binding in game. </para>
    /// <example>
    /// Example :
    /// <code>
    /// public class MyScriptInGame : MonoBehaviour<br/>
    ///     {
    ///         NWDConnectionAttribut (true, true, true, true)] // optional
    ///         public NWDEventMessageConnection MyNetWorkedData;
    ///         public void UseData()
    ///             {
    ///                 NWDEventMessage tObject = MyNetWorkedData.GetObject();
    ///                 // Use tObject
    ///             }
    ///     }
    /// </code>
    /// </example>
    /// </summary>
    [Serializable]
    public class NWDNewsConnection : NWDConnection<NWDNews>
    {
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    /// <summary>
    /// NWDEventMessage class. This class is use for (complete description here).
    /// </summary>
    [NWDClassServerSynchronizeAttribute(true)]
    [NWDClassTrigrammeAttribute("NWS")]
    [NWDClassDescriptionAttribute("News Message")]
    [NWDClassMenuNameAttribute("News Message")]
    //[NWDInternalKeyNotEditableAttribute]
    public partial class NWDNews : NWDBasis<NWDNews>
    {
        //-------------------------------------------------------------------------------------------------------------
        #region Class Properties
        //-------------------------------------------------------------------------------------------------------------
        // Your static properties
        const string KReferenceKey = "kRef";
        static Dictionary<int, List<NWDNews>> kCheckLoopDictionary = new Dictionary<int, List<NWDNews>>();
        static List<NWDNews> kCheckReinstall = new List<NWDNews>();
        static List<NWDNews> kCheckScheduled = new List<NWDNews>();
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance Properties
        //-------------------------------------------------------------------------------------------------------------
        [NWDGroupStart("Informations")]
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
        [NWDGroupEnd]
       
        [NWDGroupStart("Type of Event")]
        public NWDNewsType EventType
        {
            get; set;
        }
        [NWDIf("EventType", new int[] { (int)NWDNewsType.LocalNotificationDateFixe/*, (int)NWDNewsType.PushNotificationDateFixe*/, (int)NWDNewsType.InGameNotificationDateFixe }, false)]
        public NWDDateTimeType DistributionDate
        {
            get; set;
        }
        [NWDIf("EventType", new int[] { (int)NWDNewsType.LocalNotificationRecurrent/*, (int)NWDNewsType.PushNotificationRecurrent*/, (int)NWDNewsType.InGameNotificationRecurrent }, false)]
        public int ReccurentLifeTime
        {
            get; set;
        }
        [NWDIf("EventType", new int[] { (int)NWDNewsType.LocalNotificationSchedule/*, (int)NWDNewsType.PushNotificationSchedul*/, (int)NWDNewsType.InGameNotificationSchedule }, false)]
        [NWDNotWorking]
        [NWDInDevelopment]
        public NWDDateTimeScheduleType ScheduleDateTime
        {
            get; set;
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Constructors
        //-------------------------------------------------------------------------------------------------------------
        public NWDNews()
        {
            //Debug.Log("NWDEventMessage Constructor");
        }
        //-------------------------------------------------------------------------------------------------------------
        public NWDNews(bool sInsertInNetWorkedData) : base(sInsertInNetWorkedData)
        {
            //Debug.Log("NWDEventMessage Constructor with sInsertInNetWorkedData : " + sInsertInNetWorkedData.ToString() + "");
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void Initialization() // INIT YOUR INSTANCE WITH THIS METHOD
        {
        }
        //-------------------------------------------------------------------------------------------------------------
        [NWDAliasMethod(NWDConstants.M_OverrideClasseInThisSync)]
        public static List<Type> OverrideClasseInThisSync()
        {
            return new List<Type> { typeof(NWDUserNewsRead), typeof(NWDNews) };
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Class methods
        //-------------------------------------------------------------------------------------------------------------
        public static void Check() // call by invoke
        {
            kCheckReinstall.Clear();
            int tNow = Mathf.CeilToInt((float)BTBDateHelper.ConvertToTimestamp(DateTime.Now) / (float)60);
            Debug.Log("NWDNews Check() timestamp seconds");
            foreach (KeyValuePair<int, List<NWDNews>> tKeyValue in kCheckLoopDictionary)
            {
                Debug.Log("NWDNews Check() kCheckLoopDictionary[" + tKeyValue.Key + "] : "+ tKeyValue.Value.Count);
            }

            if (kCheckLoopDictionary.ContainsKey(tNow))
            {
                foreach (NWDNews tNew in kCheckLoopDictionary[tNow])
                {
                    Debug.Log("NWDNews Check() FIND Timestamp List");
                    tNew.NotifyInGame();
                    if (tNew.EventType == NWDNewsType.InGameNotificationRecurrent)
                    {
                        kCheckReinstall.Add(tNew);
                    }
                }
                kCheckLoopDictionary.Remove(tNow);
            }
            foreach (NWDNews tNew in kCheckScheduled)
            {
                Debug.Log("NWDNews Check() check schedule ?");
                if (tNew.ScheduleDateTime.AvailableNow())
                {
                    Debug.Log("NWDNews Check() FIND schedule");
                    tNew.NotifyInGame();
                }
            }
            foreach (NWDNews tNew in kCheckReinstall)
            {
                tNew.InstallNotification(false);
            }
            }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for class method.
        /// </summary>
        public static void InstallAllNotifications(bool sPause)
        {
            if (NWDTypeLauncher.DataLoaded == true)
            {
#if UNITY_IOS
            // add notification to user authorization!
            UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert |
             UnityEngine.iOS.NotificationType.Badge |
                 UnityEngine.iOS.NotificationType.Sound);

            UnityEngine.iOS.LocalNotification[] tNotifs = UnityEngine.iOS.NotificationServices.scheduledLocalNotifications;
            if (tNotifs != null)
            {
                foreach (UnityEngine.iOS.LocalNotification tNotif in tNotifs)
                {
                    NWDNews tNew = NWDNews.GetDataByReference(tNotif.userInfo[KReferenceKey].ToString());
                    if (tNew != null)
                    {
                        if (tNew.EventType != NWDNewsType.Programmatically)
                        {
                            //remove the notification
                            UnityEngine.iOS.NotificationServices.CancelLocalNotification(tNotif);
                        }
                    }
                }
            }
#endif
                // find NWDUserNewsRead and put in uninstalled
                foreach (NWDNews tNew in FindDatas())
                {
                    if (tNew.EventType != NWDNewsType.Programmatically)
                    {
                        NWDUserNewsRead tRead = NWDUserNewsRead.FindFirstByIndex(tNew.Reference);
                        if (tRead != null)
                        {
                            tRead.IsInstalled = false;
                            tRead.SaveDataIfModified();
                        }
                    }
                }
                // find NWDNews and install
                foreach (NWDNews tNew in FindDatas())
                {
                    tNew.InstallNotification(sPause);
                }
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        #endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Instance methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Exampel of implement for instance method.
        /// </summary>
        public void CancelNotification()
        {
            NWDUserNewsRead tRead = NWDUserNewsRead.FindFirstByIndex(this.Reference);
            if (tRead == null)
            {
                tRead = NWDUserNewsRead.NewData();
                tRead.EventMessage.SetObject(this);
                tRead.SaveData();
            }
            if (kCheckScheduled.Contains(this) == true)
            {
                kCheckScheduled.Remove(this);
            }
            if (tRead.IsInstalled == true)
            {
#if UNITY_IOS
                UnityEngine.iOS.LocalNotification[] tNotifs = UnityEngine.iOS.NotificationServices.scheduledLocalNotifications;
                foreach (UnityEngine.iOS.LocalNotification tNotif in tNotifs)
                {
                    if (tNotif.userInfo[KReferenceKey].ToString() == this.Reference)
                    {
                        //remove the notification
                        UnityEngine.iOS.NotificationServices.CancelLocalNotification(tNotif);
                    }
                }
#endif
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InstallNotificationWithFireDate(DateTime sDateTime)
        {
            #if UNITY_IOS
            // add notification to user authorization!
            UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert |
                                                                          UnityEngine.iOS.NotificationType.Badge |
                                                                          UnityEngine.iOS.NotificationType.Sound);
            #endif

            // user satut for this message 
            NWDUserNewsRead tRead = NWDUserNewsRead.FindFirstByIndex(this.Reference);
            if (tRead == null)
            {
                tRead = NWDUserNewsRead.NewData();
                tRead.EventMessage.SetObject(this);
                tRead.SaveData();
            }
            if (kCheckScheduled.Contains(this) == true)
            {
                kCheckScheduled.Remove(this);
            }
                if (tRead.IsInstalled == true)
            {
                if (tRead.NotifyMe == true)
                {
#if UNITY_IOS
                    UnityEngine.iOS.LocalNotification[] tNotifs = UnityEngine.iOS.NotificationServices.scheduledLocalNotifications;
                    if (tNotifs != null)
                    {
                        foreach (UnityEngine.iOS.LocalNotification tNotif in tNotifs)
                        {
                            if (tNotif.userInfo[KReferenceKey].ToString() == this.Reference)
                            {
                                //remove the notification
                                UnityEngine.iOS.NotificationServices.CancelLocalNotification(tNotif);
                            }
                        }
                    }
#endif
                }
                switch (EventType)
                {
                    case NWDNewsType.Programmatically:
                        {
#if UNITY_IOS
                            if (sDateTime > DateTime.Now)
                            {
                                Debug.Log("NWDNews InstallNotification() method " + EventType.ToString());
                                UnityEngine.iOS.LocalNotification tNotif = new UnityEngine.iOS.LocalNotification();
                                Dictionary<string, string> tUserInfo = new Dictionary<string, string>();
                                tUserInfo.Add(KReferenceKey, this.Reference);
                                tNotif.userInfo = tUserInfo;
                                tNotif.fireDate = sDateTime;
                                tNotif.alertTitle = Title.GetLocalString();
                                tNotif.alertBody = Message.GetLocalString();
                                //tNotif.alertLaunchImage = Image.GetLocalString();
                                UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(tNotif);
                                tRead.IsInstalled = true;
                                tRead.IsRead = false;
                            }
#endif
                        }
                        break;
                }
            }
            tRead.SaveDataIfModified();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void NotifyInGame()
        {
            BTBNotificationManager.SharedInstance().PostNotification(this, NWDNotificationConstants.K_NEWS_NOTIFICATION, this);
            //if (EventType == NWDNewsType.InGameNotificationRecurrent)
            //{
            //    // reinstall notification
            //    InstallNotification(false);
            //}
        }
        //-------------------------------------------------------------------------------------------------------------
        public void InstallNotification(bool sPause)
        {
            #if UNITY_IOS
            // add notification to user!
            UnityEngine.iOS.NotificationServices.RegisterForNotifications(UnityEngine.iOS.NotificationType.Alert |
                                                                          UnityEngine.iOS.NotificationType.Badge |
                                                                          UnityEngine.iOS.NotificationType.Sound);
            #endif

            // user satut for this message 
            NWDUserNewsRead tRead = NWDUserNewsRead.FindFirstByIndex(this.Reference);
            if (tRead == null)
            {
                tRead = NWDUserNewsRead.NewData();
                tRead.EventMessage.SetObject(this);
                tRead.SaveData();
            }
            if (kCheckScheduled.Contains(this) == true)
            {
                kCheckScheduled.Remove(this);
            }
            if (tRead.IsInstalled == true)
            {
                if (tRead.NotifyMe == true)
                {
                    #if UNITY_IOS
                    UnityEngine.iOS.LocalNotification[] tNotifs = UnityEngine.iOS.NotificationServices.scheduledLocalNotifications;
                    if (tNotifs != null)
                    {
                        foreach (UnityEngine.iOS.LocalNotification tNotif in tNotifs)
                        {
                            if (tNotif.userInfo[KReferenceKey].ToString() == this.Reference)
                            {
                                //remove the notification
                                UnityEngine.iOS.NotificationServices.CancelLocalNotification(tNotif);
                            }
                        }
                    }
                    #endif
                }
                switch (EventType)
                {
                    //case NWDNewsType.InGame:
                    //{
                    //    tRead.IsInstalled = false;
                    //    tRead.IsRead = false;
                    //}
                    //break;
                    case NWDNewsType.LocalNotificationNow:
                        {
                            #if UNITY_IOS
                            Debug.Log("NWDNews InstallNotification() method " + EventType.ToString());
                            UnityEngine.iOS.LocalNotification tNotif = new UnityEngine.iOS.LocalNotification();
                            Dictionary<string, string> tUserInfo = new Dictionary<string, string>();
                            tUserInfo.Add(KReferenceKey, this.Reference);
                            tNotif.userInfo = tUserInfo;
                            tNotif.fireDate = DateTime.Now;
                            tNotif.alertTitle = Title.GetLocalString();
                            tNotif.alertBody = Message.GetLocalString();
                            //tNotif.alertLaunchImage = Image.GetLocalString();
                            UnityEngine.iOS.NotificationServices.PresentLocalNotificationNow(tNotif);
                            tRead.IsInstalled = true;
                            tRead.IsRead = false;
                            #endif
                        }
                        break;
                    case NWDNewsType.LocalNotificationDateFixe:
                        {
                            #if UNITY_IOS
                            DateTime tDate = DistributionDate.ToDateTime();
                            if (tDate > DateTime.Now)
                            {
                                Debug.Log("NWDNews InstallNotification() method " + EventType.ToString());
                                UnityEngine.iOS.LocalNotification tNotif = new UnityEngine.iOS.LocalNotification();
                                Dictionary<string, string> tUserInfo = new Dictionary<string, string>();
                                tUserInfo.Add(KReferenceKey, this.Reference);
                                tNotif.userInfo = tUserInfo;
                                tNotif.fireDate = tDate;
                                tNotif.alertTitle = Title.GetLocalString();
                                tNotif.alertBody = Message.GetLocalString();
                                //tNotif.alertLaunchImage = Image.GetLocalString();
                                UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(tNotif);
                                tRead.IsInstalled = true;
                                tRead.IsRead = false;
                            }
                            #endif
                        }
                        break;
                    case NWDNewsType.LocalNotificationRecurrent:
                        {
                            #if UNITY_IOS
                            if (ReccurentLifeTime > 0)
                            {
                                Debug.Log("NWDNews InstallNotification() method " + EventType.ToString());
                                UnityEngine.iOS.LocalNotification tNotif = new UnityEngine.iOS.LocalNotification();
                                Dictionary<string, string> tUserInfo = new Dictionary<string, string>();
                                tUserInfo.Add(KReferenceKey, this.Reference);
                                tNotif.userInfo = tUserInfo;
                                tNotif.fireDate = DateTime.Now.AddSeconds(ReccurentLifeTime);
                                tNotif.alertTitle = Title.GetLocalString();
                                tNotif.alertBody = Message.GetLocalString();
                                //tNotif.alertLaunchImage = Image.GetLocalString();
                                UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(tNotif);
                                tRead.IsInstalled = true;
                                tRead.IsRead = false;
                            }
                            #endif
                        }
                        break;
                    case NWDNewsType.LocalNotificationSchedule:
                        {
                            #if UNITY_IOS
                            DateTime tDate = ScheduleDateTime.NextDateTime();
                            if (tDate > DateTime.Now)
                            {
                                Debug.Log("NWDNews InstallNotification() method " + EventType.ToString());
                                UnityEngine.iOS.LocalNotification tNotif = new UnityEngine.iOS.LocalNotification();
                                Dictionary<string, string> tUserInfo = new Dictionary<string, string>();
                                tUserInfo.Add(KReferenceKey, this.Reference);
                                tNotif.userInfo = tUserInfo;
                                tNotif.fireDate = tDate;
                                tNotif.alertTitle = Title.GetLocalString();
                                tNotif.alertBody = Message.GetLocalString();
                                //tNotif.alertLaunchImage = Image.GetLocalString();
                                UnityEngine.iOS.NotificationServices.ScheduleLocalNotification(tNotif);
                                tRead.IsInstalled = true;
                                tRead.IsRead = false;
                            }
                            #endif
                        }
                        break;
                    //case NWDNewsType.PushNotificationNow:
                    //    {
                    //        // no install, use the server
                    //    }
                    //    break;
                    //case NWDNewsType.PushNotificationDateFixe:
                    //    {
                    //        // no install, use the server
                    //    }
                    //    break;
                    //case NWDNewsType.PushNotificationRecurrent:
                    //    {
                    //        // no install, use the server
                    //    }
                    //    break;
                    //case NWDNewsType.PushNotificationSchedule:
                    //{
                    //    // no install, use the server
                    //}
                    //break;
                    case NWDNewsType.InGameNotificationNow:
                        {
                            // no install, use the in gamestring tNow = DateTime.Now.ToString(KDateFormat);
                            NotifyInGame();
                        }
                        break;
                    case NWDNewsType.InGameNotificationDateFixe:
                        {
                            // no install, use the in game
                            DateTime tDate = DistributionDate.ToDateTime();
                            if (tDate > DateTime.Now)
                            {
                                int tNow = Mathf.CeilToInt((float)BTBDateHelper.ConvertToTimestamp(tDate) / (float)60);
                                if (kCheckLoopDictionary.ContainsKey(tNow))
                                {
                                    kCheckLoopDictionary[tNow].Add(this);
                                }
                                else
                                {
                                    kCheckLoopDictionary.Add(tNow, new List<NWDNews>());
                                    kCheckLoopDictionary[tNow].Add(this);
                                }
                            }
                        }
                        break;
                    case NWDNewsType.InGameNotificationRecurrent:
                        {
                            // no install, use the in game
                            if (ReccurentLifeTime > 0)
                            {
                                DateTime tDate = DateTime.Now.AddSeconds(ReccurentLifeTime);
                                int tNow = Mathf.CeilToInt((float)BTBDateHelper.ConvertToTimestamp(tDate) / (float)60);
                                if (kCheckLoopDictionary.ContainsKey(tNow))
                                {
                                    kCheckLoopDictionary[tNow].Add(this);
                                }
                                else
                                {
                                    kCheckLoopDictionary.Add(tNow, new List<NWDNews>());
                                    kCheckLoopDictionary[tNow].Add(this);
                                }
                            }
                        }
                        break;
                    case NWDNewsType.InGameNotificationSchedule:
                        {
                            // no install, use the in game
                            if (kCheckScheduled.Contains(this) == false)
                            {
                                kCheckScheduled.Add(this);
                            }
                        }
                        break;
                }
            }
            tRead.SaveDataIfModified();
        }
        //-------------------------------------------------------------------------------------------------------------
#endregion
        //-------------------------------------------------------------------------------------------------------------
        #region NetWorkedData addons methods
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just after loaded from database.
        /// </summary>
        public override void AddonLoadedMe()
        {
            // do something when object was loaded
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before unload from memory.
        /// </summary>
        public override void AddonUnloadMe()
        {
            // do something when object will be unload
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before insert.
        /// </summary>
        public override void AddonInsertMe()
        {
            // do something when object will be inserted
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before update.
        /// </summary>
        public override void AddonUpdateMe()
        {
            // do something when object will be updated
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated.
        /// </summary>
        public override void AddonUpdatedMe()
        {
            // do something when object finish to be updated
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method when updated me from Web.
        /// </summary>
        public override void AddonUpdatedMeFromWeb()
        {
            // do something when object finish to be updated from CSV from WebService response
            // TODO verif if method is call in good place in good timing
            InstallNotification(false);
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before dupplicate.
        /// </summary>
        public override void AddonDuplicateMe()
        {
            // do something when object will be dupplicate
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before enable.
        /// </summary>
        public override void AddonEnableMe()
        {
            // do something when object will be enabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before disable.
        /// </summary>
        public override void AddonDisableMe()
        {
            // do something when object will be disabled
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before put in trash.
        /// </summary>
        public override void AddonTrashMe()
        {
            // do something when object will be put in trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addon method just before remove from trash.
        /// </summary>
        public override void AddonUnTrashMe()
        {
            // do something when object will be remove from trash
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons the delete me.
        /// </summary>
        public override void AddonDeleteMe()
        {
            // do something when object will be delete from local base
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonWebversionUpgradeMe(int sOldWebversion, int sNewWebVersion)
        {
            // do something when object will be web service upgrade
            // TODO verif if method is call in good place in good timing
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonIndexMe()
        {
            // InsertInIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
        public override void AddonDesindexMe()
        {
            // RemoveFromIndex();
        }
        //-------------------------------------------------------------------------------------------------------------
#endregion
        //-------------------------------------------------------------------------------------------------------------
        #region Editor
#if UNITY_EDITOR
        //-------------------------------------------------------------------------------------------------------------
        //Addons for Edition
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons in edition state of object.
        /// </summary>
        /// <returns><c>true</c>, if object need to be update, <c>false</c> or not not to be update.</returns>
        /// <param name="sNeedBeUpdate">If set to <c>true</c> need be update in enter.</param>
        public override bool AddonEdited(bool sNeedBeUpdate)
        {
            if (sNeedBeUpdate == true)
            {
                // do something
            }
            return sNeedBeUpdate;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor interface.
        /// </summary>
        /// <returns>The editor height addon.</returns>
        /// <param name="sInRect">S in rect.</param>
        public override float AddonEditor(Rect sInRect)
        {
            // Draw the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Addons editor intreface expected height.
        /// </summary>
        /// <returns>The editor expected height.</returns>
        public override float AddonEditorHeight()
        {
            // Height calculate for the interface addon for editor
            float tYadd = 0.0f;
            return tYadd;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the width of node draw.
        /// </summary>
        /// <returns>The on node draw width.</returns>
        /// <param name="sDocumentWidth">S document width.</param>
        public override float AddOnNodeDrawWidth(float sDocumentWidth)
        {
            return 250.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds the height of node draw.
        /// </summary>
        /// <returns>The on node draw height.</returns>
        public override float AddOnNodeDrawHeight(float sCardWidth)
        {
            return 130.0f;
        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds node draw.
        /// </summary>
        /// <param name="sRect">S rect.</param>
        public override void AddOnNodeDraw(Rect sRect, bool sPropertysGroup)
        {

        }
        //-------------------------------------------------------------------------------------------------------------
        /// <summary>
        /// Adds color on node.
        /// </summary>
        /// <returns>The on node color.</returns>
        public override Color AddOnNodeColor()
        {
            return Color.gray;
        }
        //-------------------------------------------------------------------------------------------------------------
        public override bool AddonErrorFound()
        {
            bool rReturnErrorFound = false;
            // check if you found error in Data values.
            // normal way is return false!
            return rReturnErrorFound;
        }
        //-------------------------------------------------------------------------------------------------------------
#endif
#endregion
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================