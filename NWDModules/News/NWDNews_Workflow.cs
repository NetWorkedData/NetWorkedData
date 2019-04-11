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
    public partial class NWDNews : NWDBasis<NWDNews>
    {
        //-------------------------------------------------------------------------------------------------------------
        const string KReferenceKey = "kRef";
        static Dictionary<int, List<NWDNews>> kCheckLoopDictionary = new Dictionary<int, List<NWDNews>>();
        static List<NWDNews> kCheckReinstall = new List<NWDNews>();
        static List<NWDNews> kCheckScheduled = new List<NWDNews>();
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
        public static void Check() // call by invoke
        {
            kCheckReinstall.Clear();
            int tNow = Mathf.CeilToInt((float)BTBDateHelper.ConvertToTimestamp(DateTime.Now) / (float)60);
            //Debug.Log("NWDNews Check() timestamp seconds");
            foreach (KeyValuePair<int, List<NWDNews>> tKeyValue in kCheckLoopDictionary)
            {
                //Debug.Log("NWDNews Check() kCheckLoopDictionary[" + tKeyValue.Key + "] : "+ tKeyValue.Value.Count);
            }

            if (kCheckLoopDictionary.ContainsKey(tNow))
            {
                foreach (NWDNews tNew in kCheckLoopDictionary[tNow])
                {
                    //Debug.Log("NWDNews Check() FIND Timestamp List");
                    tNew.NotifyInGame();
                    if (tNew.NewsType == NWDNewsType.InGameNotificationRecurrent)
                    {
                        kCheckReinstall.Add(tNew);
                    }
                }
                kCheckLoopDictionary.Remove(tNow);
            }
            foreach (NWDNews tNew in kCheckScheduled)
            {
                //Debug.Log("NWDNews Check() check schedule ?");
                if (tNew.ScheduleDateTime.AvailableNow())
                {
                    //Debug.Log("NWDNews Check() FIND schedule");
                    tNew.NotifyInGame();
                }
            }
            foreach (NWDNews tNew in kCheckReinstall)
            {
                tNew.InstallNotification(false);
            }
            }
        //-------------------------------------------------------------------------------------------------------------
        public static void InstallAllNotifications(bool sPause)
        {
            if (NWDDataManager.SharedInstance().DataLoaded() == true)
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
                    if (tNew.NewsType != NWDNewsType.Programmatically)
                    {
                        NWDUserNewsRead tRead = NWDUserNewsRead.FindDataByNews(tNew);
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
        public void CancelNotification()
        {
            NWDUserNewsRead tRead = NWDUserNewsRead.FindDataByNews(this);
            if (tRead == null)
            {
                tRead = NWDUserNewsRead.NewData();
                tRead.News.SetObject(this);
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
            NWDUserNewsRead tRead = NWDUserNewsRead.FindDataByNews(this);
            if (tRead == null)
            {
                tRead = NWDUserNewsRead.NewData();
                tRead.News.SetObject(this);
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
                switch (NewsType)
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
            NWDUserNewsRead tRead = NWDUserNewsRead.FindDataByNews(this);
            if (tRead == null)
            {
                tRead = NWDUserNewsRead.NewData();
                tRead.News.SetObject(this);
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
                switch (NewsType)
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
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================