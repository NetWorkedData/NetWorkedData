

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWENotificationManager
    {
        //-------------------------------------------------------------------------------------------------------------
        private static readonly NWENotificationManager kSharedInstance = new NWENotificationManager();
        //-------------------------------------------------------------------------------------------------------------
        public static NWENotificationManager SharedInstance()
        {
            return kSharedInstance;
        }
        //-------------------------------------------------------------------------------------------------------------
        Dictionary<string, NWENotificationObserversList> Notifications = new Dictionary<string, NWENotificationObserversList>();
        //-------------------------------------------------------------------------------------------------------------
        //[Obsolete("Used instead AddObserverForAll()")]
        //public void AddObserver(object sObserver, string sNotificationKey, NWENotificationBlock sBlockToUse)
        //{
        //    AddObserverForSender(sObserver, sNotificationKey, null, sBlockToUse);
        //}
        //-------------------------------------------------------------------------------------------------------------
        public void AddObserverForAll(object sObserver, string sNotificationKey, NWENotificationBlock sBlockToUse)
        {
            AddObserverForSender(sObserver, sNotificationKey, null, sBlockToUse);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void AddObserverForSender(object sObserver, string sNotificationKey, object sSender, NWENotificationBlock sBlockToUse)
        {
            if (string.IsNullOrEmpty(sNotificationKey))
            {
                Debug.Log("Null name specified for notificationKey in AddObserver.");
                return;
            }
            // If this specific notification doesn't exist yet, then create it.
            if (Notifications.ContainsKey(sNotificationKey) == false)
            {
                Notifications.Add(sNotificationKey, new NWENotificationObserversList());
            }
            NWENotificationObserversList tObserversList = Notifications[sNotificationKey] as NWENotificationObserversList;
            tObserversList.AddObserver(sObserver, sNotificationKey, sSender, sBlockToUse);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveObserverForSender(object sObserver, string sNotificationKey, object sSender)
        {
            RemoveObserver(sObserver, sNotificationKey, sSender, false);
        }
        //-------------------------------------------------------------------------------------------------------------
        [Obsolete("Use instead RemoveObserverForAll()")]
        public void RemoveObserver(object sObserver, string sNotificationKey)
        {
            RemoveObserver(sObserver, sNotificationKey, null, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveObserverForAll(object sObserver, string sNotificationKey)
        {
            RemoveObserver(sObserver, sNotificationKey, null, true);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveObserverEveryWhere(object sObserver)
        {
            foreach (KeyValuePair<string, NWENotificationObserversList> tKeyValue in Notifications)
            {
                tKeyValue.Value.RemoveObserver(sObserver, tKeyValue.Key, null, true);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveNotificationKey(string sNotificationKey)
        {
            if (string.IsNullOrEmpty(sNotificationKey))
            {
                Debug.Log("Null name specified for notificationKey in RemoveNotificationKey.");
                return;
            }
            if (Notifications.ContainsKey(sNotificationKey))
            {
                Notifications.Remove(sNotificationKey);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveObserver(object sObserver, string sNotificationKey, object sSender, bool sRemoveAll)
        {
            if (string.IsNullOrEmpty(sNotificationKey))
            {
                Debug.Log("Null name specified for notificationKey in RemoveObserver.");
                return;
            }
            if (Notifications.ContainsKey(sNotificationKey))
            {
                NWENotificationObserversList tObserversList = Notifications[sNotificationKey] as NWENotificationObserversList;
                tObserversList.RemoveObserver(sObserver, sNotificationKey, sSender, sRemoveAll);
            }
        }
        //-------------------------------------------------------------------------------------------------------------
        public void RemoveAll()
        {
            Notifications = new Dictionary<string, NWENotificationObserversList>();
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PostNotification(object sSender, string sNotificationKey)
        {
            PostNotification(sSender, sNotificationKey, null);
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PostNotification(object sSender, string sNotificationKey, object sData)
        {
            PostNotification(new NWENotification(sNotificationKey, sSender, sData));
        }
        //-------------------------------------------------------------------------------------------------------------
        public void PostNotification(NWENotification sNotification)
        {
            // First make sure that the name of the notification is valid.
            if (string.IsNullOrEmpty(sNotification.NotificationKey))
            {
                return;
            }
            if (Notifications.ContainsKey(sNotification.NotificationKey))
            {
                NWENotificationObserversList tObserversList = Notifications[sNotification.NotificationKey] as NWENotificationObserversList;
                if (tObserversList != null)
                {
                    //Debug.Log("have  " + tObserversList.ObserversProperties.Count + " observer for " + sNotification.NotificationKey);
                    //Debug.Log("will poste " + tObserversList.ObserversProperties.Count + " observer for " + sNotification.NotificationKey);
                    tObserversList.SendNotification(sNotification);
                }
                tObserversList = null;
            }
            //			else 
            //			{
            ////				Debug.Log ("no IDEObserversList for " + sNotification.NotificationKey + " in Notifications");
            //			}
        }
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================