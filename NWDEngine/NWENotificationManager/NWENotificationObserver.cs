

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public delegate void NWENotificationBlock(NWENotification sNotification);
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWENotificationObserver
	{
		//-------------------------------------------------------------------------------------------------------------
		public object Observer;
		public string NotificationKey;
		public object Sender;
		public NWENotificationBlock BlockToUse;
		//-------------------------------------------------------------------------------------------------------------
		public NWENotificationObserver (object sObserver, string sNotificationKey, NWENotificationBlock sBlockToUse)
		{
			Observer = sObserver;
			NotificationKey = sNotificationKey;
			BlockToUse = sBlockToUse;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWENotificationObserver (object sObserver, string sNotificationKey, object sSender, NWENotificationBlock sBlockToUse)
		{
			Observer = sObserver;
			NotificationKey = sNotificationKey;
			Sender = sSender;
			BlockToUse = sBlockToUse;
		}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================