

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

using UnityEngine;

//=====================================================================================================================
namespace NetWorkedData
{
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    public class NWENotification
	{
		//-------------------------------------------------------------------------------------------------------------
		public string NotificationKey;
		public object Sender;
		public object Datas;
		//-------------------------------------------------------------------------------------------------------------
		public NWENotification (string sNotificationKey,object sSender)
		{
			NotificationKey = sNotificationKey;
			Sender = sSender;
		}
		//-------------------------------------------------------------------------------------------------------------
		public NWENotification (string sNotificationKey,object sSender, object sDatas)
		{
			NotificationKey = sNotificationKey;
			Sender = sSender;
			Datas = sDatas;
		}
        //-------------------------------------------------------------------------------------------------------------
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
}
//=====================================================================================================================