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